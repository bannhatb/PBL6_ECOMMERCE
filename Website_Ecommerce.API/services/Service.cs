using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.Response;
using Website_Ecommerce.API.Repositories;
using Microsoft.EntityFrameworkCore;
using PBL6_ECOMMERCE.Website_Ecommerce.API.Response;
using PBL6_ECOMMERCE.Website_Ecommerce.API.ModelDtos;
using System.Text;
using System.Reflection;

namespace PBL6_ECOMMERCE.Website_Ecommerce.API.services
{
    public class Service : IServices
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;
        public Service(IConfiguration configurationManager,
            IOrderRepository orderRepository
        )
        {
            _configuration = configurationManager;
            _orderRepository = orderRepository;
        }
        public async Task<Response<ResponseDefault>> getPaymentLink(int orderId, string vnp_Returnurl)
        {
            Response<ResponseDefault> response = new Response<ResponseDefault>();
            try
            {
                string vnp_Url = _configuration["vnp_Url"]; //URL thanh toan cua VNPAY 
                string vnp_TmnCode = _configuration["vnp_TmnCode"]; //Ma website
                string vnp_HashSecret = _configuration["vnp_HashSecret"]; //Chuoi bi mat
                Order order = await _orderRepository.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
                if (order == null)
                {
                    response.State = false;
                    response.Message = "NotFound";
                    return response;
                }
                VnPayLibrary vnpay = new VnPayLibrary();
                //Save order to db
                order.Reference = VnPayLibrary.GenerateReferences(10); // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
                order.State = 0; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" 1: thanh cong 2   that bai
                _orderRepository.Update(order);
                await _orderRepository.UnitOfWork.SaveAsync(new CancellationToken());

                //Build URL for VNPAY

                vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                vnpay.AddRequestData("vnp_Amount", (order.TotalPrice * 10000).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
                vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_Locale", "vn");
                vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.Reference);
                vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
                vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                vnpay.AddRequestData("vnp_TxnRef", order.Reference); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

                //Add Params of 2.1.0 Version
                vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"));

                string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                response.State = true;
                response.Message = "Success";
                response.Result = new ResponseDefault()
                {
                    Data = paymentUrl
                };
                return response;
            }
            catch (Exception e)
            {
                response.State = false;
                response.Message = e.Message;
                return response;
            }
        }

        public async Task<ResponseVnPay> returnUrl(ReturnRequest request)
        {
            ResponseVnPay res = new ResponseVnPay();
            if (request != null)
            {
                string vnp_HashSecret = _configuration["vnp_HashSecret"]; //Secret key
                VnPayLibrary vnpay = new VnPayLibrary();
                string secureHash = "";
                IList<PropertyInfo> properties = request.GetType().GetProperties().ToList();
                foreach (var property in properties)
                {
                    var val = property.GetValue(request)?.ToString();
                    if (String.IsNullOrEmpty(val) || property.Name == "vnp_SecureHash")
                    {
                        if (property.Name == "vnp_SecureHash")
                        {
                            secureHash = val;
                        }
                        continue;
                    }
                    else
                    {
                        vnpay.AddResponseData(property.Name, val);
                    }
                }
                string orderRef = vnpay.GetResponseData("vnp_TxnRef");
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 10000;
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                bool checkSignature = vnpay.ValidateSignature(secureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    Order order = await _orderRepository.Orders.FirstOrDefaultAsync(x => x.Reference == orderRef);
                    if (order != null)
                    {
                        if (order.TotalPrice == vnp_Amount)
                        {
                            if (order.State == 0) // pending
                            {
                                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                                {
                                    order.State = 1; // thanh cong
                                }
                                else
                                {
                                    order.State = 2; // loi
                                }
                                _orderRepository.Update(order);
                                res.Message = "Confirm Success";
                                res.RspCode = "00";
                                return res;
                            }
                            else
                            {
                                res.Message = "Order already confirmed";
                                res.RspCode = "02";
                                return res;
                            }
                        }
                        else
                        {
                            res.Message = "invalid amount";
                            res.RspCode = "04";
                            return res;
                        }
                    }
                    else
                    {
                        res.Message = "Order not found";
                        res.RspCode = "01";
                        return res;
                    }
                }
                else
                {
                    res.Message = "Invalid signature";
                    res.RspCode = "97";
                    return res;
                }
            }
            else
            {
                res.Message = "Input data required";
                res.RspCode = "99";
                return res;
            }
        }
    }
}