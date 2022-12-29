using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelDtos;
using Website_Ecommerce.API.Repositories;
using Website_Ecommerce.API.Response;

namespace Website_Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "MyAuthKey")]

    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IShopRepository _shopRepository;
        private readonly IMapper _mapper;

        public UserController(
            IUserRepository userRepository,
            IHttpContextAccessor httpContext,
            IShopRepository shopRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _httpContext = httpContext;
            _shopRepository = shopRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Update profile
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDto request, CancellationToken cancellationToken)
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());

            User user = _userRepository.Users.FirstOrDefault(x => x.Id == userId);

            user = _mapper.Map(request, user);

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
                        Data = user.Id.ToString()
                    }
                });
            }
            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Update profile fail"
                }
            });
        }

        /// <summary>
        /// Request role shop
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("request-role-shop")]
        public async Task<IActionResult> RequestRoleShop([FromBody] ShopDto request, CancellationToken cancellationToken)
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());

            var user = _userRepository.Users.FirstOrDefault(x => x.Id == userId);

            Shop shop = new Shop()
            {
                Email = user.Email,
                //totalrate = -1 //confirm role shop
                TotalRate = -1,
                AverageRate = 0,
                UserId = userId,
                Status = false
            };

            shop = _mapper.Map(request, shop);
            _shopRepository.Add(shop);

            var result = await _shopRepository.UnitOfWork.SaveAsync(cancellationToken);

            if (result == 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Add shop fail"
                    }
                });
            }

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Add shop success"
                }
            });
        }

        /// <summary>
        /// Get info current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("info-current-user")]
        public async Task<IActionResult> GetInfoCurrentUser()
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            var user = await _userRepository.GetInfotUserById(userId);
            if (user is null)
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
                    Data = user
                }
            });
        }

    }
}