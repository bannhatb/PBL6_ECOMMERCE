using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.Enum;
using Website_Ecommerce.API.ModelDtos;
using Website_Ecommerce.API.ModelQueries;
using Website_Ecommerce.API.Repositories;
using Website_Ecommerce.API.Response;

namespace Website_Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "MyAuthKey")]
    public class ShopController : ControllerBase
    {
        private readonly IShopRepository _shopRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IVoucherOrderRepository _voucherOrderRepository;

        public ShopController(
            IShopRepository shopRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IHttpContextAccessor httpContext,
            IVoucherOrderRepository voucherOrderRepository)
        {
            _productRepository = productRepository;
            _shopRepository = shopRepository;
            _httpContext = httpContext;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _voucherOrderRepository = voucherOrderRepository;
        }
        #region "API  update/ delete / getListShop"

        /// <summary>
        /// Update Shop
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("update-shop")]
        public async Task<IActionResult> UpdateShop(ShopDto request, CancellationToken cancellationToken)
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            // Check userId
            Shop shop = _shopRepository.Shops.FirstOrDefault(x => x.UserId == userId);
            if (shop == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "Not Found Shop"
                    }
                });
            }

            shop = _mapper.Map(request, shop);

            _shopRepository.Update(shop);
            var result = await _shopRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Update shop fail"
                    }
                });
            }

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Update shop success"
                }
            });
        }

        /// <summary>
        /// Delete Shop by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete-shop/{id}")]
        public async Task<IActionResult> DeleteShop(int id)
        {
            if (id.ToString() is null)
            {
                return BadRequest(null);
            }
            var shop = _shopRepository.Shops.FirstOrDefault(x => x.Id == id);
            if (shop == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "NotFound Shop"
                    }
                });
            }

            _shopRepository.Delete(shop);

            var result = await _shopRepository.UnitOfWork.SaveAsync();

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = shop.Id.ToString()
                    }
                });
            }
            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Delete category fail"
                }
            });
        }

        /// <summary>
        /// Get shop of current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-shop-by-current-user")]
        public async Task<IActionResult> GetShopById()
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            // Check userId
            Shop shop = await _shopRepository.Shops.FirstOrDefaultAsync(x => x.UserId == userId);
            if (shop == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "Not Found Shop"
                    }
                });
            }

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = shop
                }
            });
        }


        #endregion
        #region "API Voucher Shop"

        /// <summary>
        /// Add Voucher of Shop
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("add-voucher-of-shop")]
        public async Task<IActionResult> AddVoucherShop(VoucherShopDto request, CancellationToken cancellationToken)
        {
            request.CreateAt = DateTime.Now;
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());

            var user = _userRepository.Users.FirstOrDefault(x => x.Id == userId);
            var shop = _shopRepository.Shops.FirstOrDefault(x => x.UserId == userId);

            VoucherProduct voucherProduct = _mapper.Map<VoucherProduct>(request);
            voucherProduct.ShopId = shop.Id;
            _shopRepository.Add(voucherProduct);
            var result = await _shopRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Add voucher product fail"
                    }
                });
            }
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Add voucher Product success"
                }
            });
        }

        /// <summary>
        /// Update Voucher of Shop
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("update-voucher-by-shop")]
        public async Task<IActionResult> UpdateVoucherShop(VoucherShopDto request, CancellationToken cancellationToken)
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());


            var shop = _shopRepository.Shops.FirstOrDefault(x => x.UserId == userId);


            VoucherProduct voucher = _mapper.Map<VoucherProduct>(request);
            voucher.ShopId = shop.Id;

            _shopRepository.Update(voucher);
            var result = await _shopRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Update Voucher fail!"
                    }
                });
            }
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Update voucher Product success!"
                }
            });
        }

        /// <summary>
        /// Get Voucher of Shop
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-voucher-of-shop")]
        public async Task<IActionResult> GetVoucherOfShop()
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            var shop = _shopRepository.Shops.FirstOrDefault(x => x.UserId == userId);

            List<VoucherProduct> list = await _shopRepository.voucherProducts.Where(x => x.ShopId == shop.Id).ToListAsync();
            List<VoucherShopDto> listDto = _mapper.Map<List<VoucherShopDto>>(list);
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = listDto
                }
            });
        }

        /// <summary>
        /// Get Voucher of Shop
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-voucher-of-shop-by/{id}")]
        public async Task<IActionResult> GetVoucherbyId(int id)
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            var shop = _shopRepository.Shops.FirstOrDefault(x => x.UserId == userId);

            VoucherProduct voucherProduct = await _shopRepository.voucherProducts.Where(x => x.ShopId == shop.Id && x.Id == id).FirstOrDefaultAsync();
            VoucherShopDto voucherProductDto = _mapper.Map<VoucherShopDto>(voucherProduct);
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = voucherProductDto
                }
            });
        }

        /// <summary>
        /// Get voucher of Shop match
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        [HttpGet("get-voucher-of-shop-match")]
        public async Task<IActionResult> GetVoucherShopMatch(int price)
        {
            List<VoucherProduct> vouchers = await _shopRepository.voucherProducts.Where(p => p.MinPrice > price && p.Amount > 0).ToListAsync();
            if (vouchers.Count() <= 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = "Khong co voucher nao phu hop"
                    }
                });
            }
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = vouchers
                }
            });
        }
        #endregion
        #region "API Order of Shop"
        [HttpGet("get-all-order-detail-of-shop")]
        public async Task<IActionResult> GetListOrderDetailOfShop()
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            var shop = _shopRepository.Shops.FirstOrDefault(x => x.UserId == userId);
            var shopId = shop.Id;
            var listOrderDetailOfShop = await _orderRepository.GetOrderDetail(shopId);

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.BadRequest,
                Result = new ResponseDefault()
                {
                    Data = listOrderDetailOfShop
                }
            });
        }

        /// <summary>
        /// Get list OrderDetail unconfirm by shop
        /// </summary>
        /// <returns></returns>

        [HttpGet("get-list-order-detail-unconfirm-by-shop")]
        public async Task<IActionResult> GetListUnConfirmOrder()
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            var shop = _shopRepository.Shops.FirstOrDefault(x => x.UserId == userId);
            var shopId = shop.Id;
            var listOrderDetailOfShop = await _orderRepository.OrderDetails
                                        .Where(r => r.ShopId == shopId && r.State == (int)StateOrderDetailEnum.UNCONFIRMED)
                                        .ToListAsync();
            if (listOrderDetailOfShop.Count == 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.BadRequest,
                    Result = new ResponseDefault()
                    {
                        Data = "Không có đơn hàng nào"
                    }
                });
            }
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.BadRequest,
                Result = new ResponseDefault()
                {
                    Data = listOrderDetailOfShop
                }
            });

        }
        // [HttpGet("get-list-order-detail-of-shop")]
        // public async Task<IActionResult> getListOrderDetailOfShop(){

        //     return Ok( new Response<ResponseDefault>()
        //         {
        //             State = true,
        //             Message = ErrorCode.BadRequest,
        //             Result = new ResponseDefault()
        //             {
        //                 Data = orderDetails
        //             }
        //         });

        // }

        /// <summary>
        /// Confirm Order
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productDetailId"></param>
        /// <param name="state"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch("confirm-order")]
        public async Task<IActionResult> ConfirmOrder(int orderID, int productDetailId, int state, CancellationToken cancellationToken)
        {
            var orderDetail = await _orderRepository.OrderDetails
                                .FirstOrDefaultAsync(r => r.OrderId == orderID && r.ProductDetailId == productDetailId);

            if (orderDetail == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.BadRequest,
                    Result = new ResponseDefault()
                    {
                        Data = "Not found Order"
                    }
                });
            }

            orderDetail.State = state /*(int)StateOrderDetailEnum.CONFIRMED*/;
            var thisProductDetail = _productRepository.ProductDetails.Where(x => x.Id == orderDetail.ProductDetailId).FirstOrDefault();
            thisProductDetail.Amount = thisProductDetail.Amount - orderDetail.Amount;
            thisProductDetail.Booked = thisProductDetail.Booked - orderDetail.Amount;
            thisProductDetail.Saled = thisProductDetail.Saled + orderDetail.Amount;
            var saveProduct = await _productRepository.UnitOfWork.SaveAsync(cancellationToken);
            var thisVoucherProduct = await _shopRepository.voucherProducts.Where(x => x.Id == orderDetail.VoucherProductId).FirstOrDefaultAsync();
            if (thisVoucherProduct != null)
            {
                thisVoucherProduct.Amount = thisVoucherProduct.Amount - 1;
                thisVoucherProduct.Sale = thisVoucherProduct.Sale + 1;
                thisVoucherProduct.Booked = thisVoucherProduct.Booked - 1;
                var saveVoucherProduct = _shopRepository.UnitOfWork.SaveAsync();
            }
            var order = _orderRepository.Orders.Where(x => x.Id == orderID).FirstOrDefault();
            var thisVoucherOrder = _voucherOrderRepository.VoucherOrders.Where(x => x.Id == order.VoucherId).FirstOrDefault();
            if (thisVoucherOrder != null)
            {
                thisVoucherOrder.Amount = thisVoucherOrder.Amount - 1;
                thisVoucherOrder.Sale = thisVoucherOrder.Sale + 1;
                thisVoucherOrder.Booked = thisVoucherOrder.Booked - 1;
                var saveVoucherOrder = _voucherOrderRepository.UnitOfWork.SaveAsync();
            }
            // var thisvoucherOrder = _voucherOrderRepository.VoucherOrders.Where(x => x.Id == orderDetail.)
            if (state == (int)StateOrderEnum.CONFIRMED)
            {
                orderDetail.ShopConfirmDate = DateTime.Now;
            }
            else
            {
                orderDetail.ShopSendDate = DateTime.Now;
            }
            _orderRepository.Update(orderDetail);

            var result = await _orderRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.BadRequest,
                    Result = new ResponseDefault()
                    {
                        Data = "Can't confirm item"
                    }
                });
            }
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.BadRequest,
                Result = new ResponseDefault()
                {
                    Data = "successful"
                }
            });

        }
        #endregion
        #region "API Product Manager"
        // [HttpGet("Get-all-product-of-shop-manager")]
        // public async Task<IActionResult> GetAllProductOfShopManager(){
        //     int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());

        //     var user = _userRepository.Users.FirstOrDefault(x => x.Id == userId);
        //     var shop = _shopRepository.Shops.FirstOrDefault(x => x.UserId == userId);

        //     var products =  _productRepository.Products.Where(p => p.ShopId == shop.Id).ToList();

        //    }
        /// <summary>
        /// Get info shop current
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-info-current-shop")]
        public async Task<IActionResult> GetInfoCurrentShop()
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            var shop = await _shopRepository.Shops.FirstOrDefaultAsync(x => x.UserId == userId);
            var infoShop = await _shopRepository.GetInfoShopBy(shop.Id);
            if (infoShop is null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "NotFound"
                    }
                });
            }

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = infoShop
                }
            });
        }

        [HttpGet("get-product-id-last")]
        public async Task<IActionResult> GetIdProductlasest(int id)
        {
            int i = _productRepository.Products.Where(p => p.ShopId == id).OrderByDescending(p => p.Id).FirstOrDefault().Id;
            return Ok(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = i
                }
            });
        }
        #endregion


    }
}