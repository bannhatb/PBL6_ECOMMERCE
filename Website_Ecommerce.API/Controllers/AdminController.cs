using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.Repositories;
using Website_Ecommerce.API.Response;
using Website_Ecommerce.API.services;

namespace Website_Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(AuthenticationSchemes = "MyAuthKey")]
    // [CustomAuthorize(Allows = "1")]
    public class AdminController : ControllerBase
    {
        private readonly IShopRepository _shopRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public AdminController(
            IShopRepository shopRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IHttpContextAccessor httpContext)
        {
            _productRepository = productRepository;
            _shopRepository = shopRepository;
            _httpContext = httpContext;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list user
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-list-user")]
        public async Task<IActionResult> GetListUser()
        {
            List<User> users = await _userRepository.Users.ToListAsync();
            if (users == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "Not Found User"
                    }
                });
            }

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = users
                }
            });
        }


        /// <summary>
        /// Update state of user by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("update-state-user-by/{id}")]
        public async Task<IActionResult> BLockUser(int userId, CancellationToken cancellationToken)
        {
            User user = await _userRepository.Users.FirstOrDefaultAsync(x => x.Id == userId);
            user.IsBlock = !user.IsBlock;
            _userRepository.Update(user);
            var result = await _userRepository.UnitOfWork.SaveAsync(cancellationToken);

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = $"State of user {user.Id.ToString()}: {user.IsBlock.ToString()}"
                    }
                });
            }
            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Block user fail"
                }
            });
        }

        #region confirm role shop, update state of shop, get list shop waiting confirm role shop

        /// <summary>
        /// Get list shop active
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-list-shop-active")]
        public async Task<IActionResult> GetListShopActive()
        {
            // State = true la active
            List<Shop> shops = await _shopRepository.Shops.Where(x => x.Status == true).ToListAsync();
            if (shops == null)
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
                    Data = shops
                }
            });
        }

        /// <summary>
        /// Get list shop no active
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-list-shop-no-active")]
        public async Task<IActionResult> GetListShopNoActive()
        {
            List<Shop> shops = await _shopRepository.Shops.Where(x => x.Status == false).ToListAsync();
            if (shops == null)
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
                    Data = shops
                }
            });
        }

        /// <summary>
        /// Get list shop wating confirm register by admin
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-list-shop-waiting-confirm")]
        public async Task<IActionResult> GetListShopWaitingConfirm()
        {
            // State xac nhan la Status = false & TotalRate = -1
            List<Shop> shops = await _shopRepository.Shops.Where(x => x.Status == false && x.TotalRate == -1).ToListAsync();
            if (shops == null)
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
                    Data = shops
                }
            });
        }

        /// <summary>
        /// Confirm role shop of user by id
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("confirm-role-shop-of-user-by/{id}")]
        public async Task<IActionResult> ConfirmRoleShopOfUser(int shopId, CancellationToken cancellationToken)
        {

            Shop shop = await _shopRepository.Shops.FirstOrDefaultAsync(x => x.Id == shopId);
            shop.Status = true;
            shop.TotalRate = 0;
            _shopRepository.Update(shop);

            var userRole = new UserRole()
            {
                UserId = shop.UserId,
                RoleId = 2
            };

            _userRepository.Add(userRole);

            var result = await _shopRepository.UnitOfWork.SaveAsync(cancellationToken);
            var result1 = await _userRepository.UnitOfWork.SaveAsync(cancellationToken);

            if (result > 0 && result1 > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = $"State of shop  {shop.Id.ToString()}: {shop.Status.ToString()}"
                    }
                });
            }
            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Block user fail"
                }
            });
        }

        /// <summary>
        /// Update state active of shop by id
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("update-state-shop-by/{id}")]
        public async Task<IActionResult> BlockShop(int shopId, CancellationToken cancellationToken)
        {
            Shop shop = await _shopRepository.Shops.FirstOrDefaultAsync(x => x.Id == shopId);
            shop.Status = !shop.Status;
            _shopRepository.Update(shop);
            var result = await _shopRepository.UnitOfWork.SaveAsync(cancellationToken);

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = $"State of shop {shop.Id.ToString()}: {shop.Status.ToString()}"
                    }
                });
            }
            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Block user fail"
                }
            });
        }

        #endregion
    }
}