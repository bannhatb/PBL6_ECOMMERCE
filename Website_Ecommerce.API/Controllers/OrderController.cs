using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.Enum;
using Website_Ecommerce.API.ModelDtos;
using Website_Ecommerce.API.Repositories;
using Website_Ecommerce.API.Response;

namespace Website_Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "MyAuthKey")]

    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IVoucherOrderRepository _voucherOrderRepository;
        private readonly IShopRepository _shopRepository;
        public OrderController(IOrderRepository orderRepository,
                IHttpContextAccessor httpContext,
                IProductRepository productRepository,
                IShopRepository shopRepository,
                IVoucherOrderRepository voucherOrderRepository)
        {
            _shopRepository = shopRepository;
            _voucherOrderRepository = voucherOrderRepository;
            _orderRepository = orderRepository;
            _httpContext = httpContext;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Add order
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("add-order")]
        public async Task<IActionResult> AddOrder([FromBody] OrderDto request, CancellationToken cancellationToken)
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            VoucherOrder voucherOrder = _voucherOrderRepository.VoucherOrders.FirstOrDefault(x => x.Id == request.VoucherId);

            var order = new Order
            {
                Id = request.Id,
                UserId = userId,
                Address = request.Address,
                RecipientName = request.RecipientName,
                RecipientPhone = request.RecipientPhone,
                CreateDate = DateTime.Now,
                VoucherId = request.VoucherId,
                State = (int)StateOrderEnum.SENT,
                TotalPrice = request.totalPrice
            };


            if (voucherOrder != null)
            {
                voucherOrder.Booked = voucherOrder.Booked + 1;
                _voucherOrderRepository.Update(voucherOrder);
                _orderRepository.Add(order);
            }
            else
            {
                order.VoucherId = null;
                _orderRepository.Add(order);
            }

            var result = await _orderRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "ExcutedDB"
                    }
                });
            }

            Order thisOrder = _orderRepository.Orders.OrderByDescending(x => x.CreateDate).FirstOrDefault(x => x.UserId == userId);

            foreach (var i in request.ItemOrderDtos)
            {
                ProductDetail productDetail = _productRepository.ProductDetails.FirstOrDefault(x => x.Id == i.ProductDetailId);
                Product product = _productRepository.Products.FirstOrDefault(x => x.Id == productDetail.ProductId);
                Shop shop = _shopRepository.Shops.FirstOrDefault(x => x.Id == product.ShopId);
                VoucherProduct voucherProduct = new VoucherProduct();
                if (i.VoucherProductId == 0)
                {
                    i.VoucherProductId = null;
                    voucherProduct.Value = 0;
                }
                else
                {
                    voucherProduct = _shopRepository.voucherProducts.FirstOrDefault(x => x.Id == i.VoucherProductId);
                }

                var orderDetail = new OrderDetail
                {
                    OrderId = thisOrder.Id,
                    ProductDetailId = i.ProductDetailId,
                    Amount = i.Amount,
                    ShopId = shop.Id,
                    State = (int)StateOrderDetailEnum.UNCONFIRMED,
                    Price = i.Price,
                    VoucherProductId = i.VoucherProductId
                };
                _orderRepository.Add(orderDetail);

                productDetail.Booked = productDetail.Booked + orderDetail.Amount;
                if (orderDetail.VoucherProductId != null)
                {
                    voucherProduct.Amount = voucherProduct.Booked + 1;
                    _shopRepository.Update(voucherProduct);
                }
                _productRepository.Update(productDetail);

                var resultI = await _orderRepository.UnitOfWork.SaveAsync(cancellationToken);
                if (resultI <= 0)
                {
                    _orderRepository.Delete(thisOrder);
                    var delete = await _orderRepository.UnitOfWork.SaveAsync(cancellationToken);
                    return BadRequest(new Response<ResponseDefault>()
                    {
                        State = true,
                        Message = ErrorCode.ExcuteDB,
                        Result = new ResponseDefault()
                        {
                            Data = "ExcuteDB"
                        }
                    });
                }
            }
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Add order success"
                }
            });
        }

        /// <summary>
        /// Cancel order detail
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productDetailId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch("cancel-order-detail")]
        public async Task<IActionResult> CancelOrder(int orderId, int productDetailId, CancellationToken cancellationToken)
        {
            var orderDetail = await _orderRepository.OrderDetails.FirstOrDefaultAsync(x => x.OrderId == orderId && x.ProductDetailId == productDetailId);
            if (orderDetail == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "Not Found OrderDetail"
                    }
                });
            }

            if (orderDetail.State > (int)StateOrderEnum.RECEIVED)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Can't cancel order"
                    }
                });
            }
            orderDetail.State = (int)StateOrderEnum.CANCALLED;
            _orderRepository.Update(orderDetail);

            var result = await _orderRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Can't cancel order"
                    }
                });
            }

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Cancel Order Success"
                }
            });

        }

        /// <summary>
        /// View Order by orderId
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("view-order")]
        public async Task<IActionResult> ViewOrder(int orderId)
        {
            Order order = await _orderRepository.Orders.FirstOrDefaultAsync(p => p.Id == orderId);

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = order
                }
            });
        }

        /// <summary>
        /// View Order of User
        /// </summary>
        /// <returns></returns>
        [HttpGet("view-order-of-user")]
        public async Task<IActionResult> ViewOrderByStatus()
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            var orders = _orderRepository.Orders.Where(i => i.UserId == userId);
            var orderdetails = _orderRepository.OrderDetails;
            var data = await orders.Join(orderdetails, o => o.Id, od => od.OrderId,
                                    (o, od) => new
                                    {
                                        id = od.OrderId,
                                        idShop = od.ShopId,
                                        state = od.State,
                                        idProductDetail = od.ProductDetailId,
                                        amount = od.Amount,
                                        price = od.Price
                                    }).ToListAsync();

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = data
                }
            });
        }

        // }
        // [HttpPatch("update-state-order-by/{idOrder}")]
        // public async Task<IActionResult> UpdateStateOrder(int idOrder){
        //     var orderdetails = await _orderRepository.GetOrderDetail(idOrder);
        //     var check = 0;
        //     foreach (var item in orderdetails)
        //     {
        //      if(item.State == (int)StateOrderEnum.SENT){
        //         check = 1;
        //      }

        //     }
        // }


    }

}