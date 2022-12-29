using PBL6_ECOMMERCE.Website_Ecommerce.API.ModelDtos;
using PBL6_ECOMMERCE.Website_Ecommerce.API.Response;
using Website_Ecommerce.API.Response;

namespace PBL6_ECOMMERCE.Website_Ecommerce.API.services
{
    public interface IServices
    {
        Task<Response<ResponseDefault>> getPaymentLink(int orderId, string vnp_Returnurl);
        Task<ResponseVnPay> returnUrl(ReturnRequest request);
    }
}