using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL6_ECOMMERCE.Website_Ecommerce.API.ModelDtos;
using PBL6_ECOMMERCE.Website_Ecommerce.API.services;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelDtos;
using Website_Ecommerce.API.Repositories;
using Website_Ecommerce.API.Response;

namespace Website_Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "MyAuthKey")]
    public class CartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IServices _services;
        public CartController(
            ICartRepository cartRepository,
            IMapper mapper,
            IHttpContextAccessor httpContext,
            IServices services)
        {
            _httpContext = httpContext;
            _mapper = mapper;
            _cartRepository = cartRepository;
            _services = services;
        }

        /// <summary>
        /// Add item to cart
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("add-item-to-cart")]
        public async Task<IActionResult> AddItemToCart([FromBody] CartDto request, CancellationToken cancellationToken)
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());

            Cart item = new Cart()
            {
                State = true,
                UserId = userId
            };
            item = _mapper.Map(request, item);
            _cartRepository.Add(item);
            var result = await _cartRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Add ProductImage fail"
                    }
                });
            }
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Add ProductImage success"
                }
            });
        }

        /// <summary>
        /// Update item in cart
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("update-item-in-cart")]
        public async Task<IActionResult> UpdateItemCart([FromBody] CartDto request, CancellationToken cancellationToken)
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());

            Cart item = new Cart()
            {
                State = true,
                UserId = userId
            };
            item = _mapper.Map(request, item);
            _cartRepository.Update(item);
            var result = await _cartRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Update Item to Cart fail"
                    }
                });
            }

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Update Item to Cart success"
                }
            });
        }

        /// <summary>
        /// Delete item in cart by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete-item-in-cart/{id}")]
        public async Task<IActionResult> DeleteItemInCart(int id)
        {
            if (id.ToString() is null)
            {
                return BadRequest(null);
            }
            var item = await _cartRepository.Carts.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "Delete item fail"
                    }
                });
            }

            _cartRepository.Delete(item);
            var result = await _cartRepository.UnitOfWork.SaveAsync();

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = item.Id.ToString()
                    }
                });
            }
            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExistedDB,
                Result = new ResponseDefault()
                {
                    Data = "Delete item fail"
                }
            });
        }

        /// <summary>
        /// Get all items of user
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-items-of-user")]
        public async Task<IActionResult> GetAllItemByIdUser()
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            var listItems = await _cartRepository.GetAllItemByIdUser(userId);
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = listItems
                }
            });
        }

        /// <summary>
        /// Get payment link
        /// </summary>
        /// <returns></returns>
        [HttpGet("vnay-payment-link")]
        public async Task<IActionResult> PaymentLink(int orderId, string vnp_Returnurl)
        {
            var res = await _services.getPaymentLink(orderId, vnp_Returnurl);
            if(!res.State)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        /// <summary>
        /// update db
        /// </summary>
        /// <returns></returns>
        [HttpGet("return-url")]
        [AllowAnonymous]
        public async Task<IActionResult> returnURL([FromQuery]ReturnRequest request)
        {
            var res = await _services.returnUrl(request);
            return Ok(res);
        }
    }
}