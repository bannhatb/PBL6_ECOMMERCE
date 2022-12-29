using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.Enum;
using Website_Ecommerce.API.ModelDtos;
using Website_Ecommerce.API.Repositories;
using Website_Ecommerce.API.Response;
using Website_Ecommerce.API.services;

namespace Website_Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityServices _identityServices;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public AuthController(
            IUserRepository userRepository,
            IIdentityServices identityServices,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _identityServices = identityServices;
            _httpContext = httpContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Register 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request, CancellationToken cancellationToken)
        {
            // Check username & email khong trung
            User isExist = await _userRepository.Users.FirstOrDefaultAsync(x => x.Username == request.Username || x.Email == request.Email);

            if (isExist != null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExistUserOrEmail
                });
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Gender = request.Gender,
                IsBlock = false,
                Password = _identityServices.GetMD5(request.Password),
                DateCreate = DateTime.Now
            };

            _userRepository.Add(user);

            await _userRepository.UnitOfWork.SaveAsync(cancellationToken);

            var userRole = new UserRole()
            {
                UserId = user.Id,
                RoleId = 4
            };

            _userRepository.Add(userRole);

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
            else
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.BadRequest
                });
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns> Token </returns>
        [HttpPost("login")]
        public async Task<Response<ResponseToken>> Login(LoginDto request, CancellationToken cancellationToken)
        {
            // Check username va account active
            User user = await _userRepository.Users.FirstOrDefaultAsync(x => x.Username == request.Username && x.IsBlock == false);

            if (user == null)
            {
                return new Response<ResponseToken>()
                {
                    State = false,
                    Message = ErrorCode.NotFound
                };
            }

            // Check password
            if (_identityServices.VerifyMD5Hash(user.Password, _identityServices.GetMD5(request.Password)))
            {
                // Thoi gian token co hieu luc
                int timeOut = 60 * 60 * 24;
                // Lay roleId cua user
                List<int> roleIds = _userRepository.UserRoles
                    .Where(x => x.UserId == user.Id).Select(x => x.RoleId).ToList();

                string token = _identityServices.GenerateToken(
                    user.Id, user.Username,
                    roleIds,
                    timeOut);

                return new Response<ResponseToken>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseToken()
                    {
                        Token = token,
                    }
                };
            }
            else
            {
                return new Response<ResponseToken>()
                {
                    State = false,
                    Message = ErrorCode.BadRequest,
                };
            }
        }

        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = "MyAuthKey")]
        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto request, CancellationToken cancellationToken)
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            User user = await _userRepository.Users.FirstOrDefaultAsync(x => x.Username == request.Username);

            if (user == null)
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

            // Check pass
            if (_identityServices.VerifyMD5Hash(user.Password, _identityServices.GetMD5(request.PasswordOld)))
            {
                if (request.PasswordNew == request.RePassword)
                {
                    user.Password = _identityServices.GetMD5(request.PasswordNew);
                    _userRepository.Update(user);
                }
                else
                {
                    return BadRequest(new Response<ResponseDefault>()
                    {
                        State = false,
                        Message = ErrorCode.PasswordNotMatch,
                        Result = new ResponseDefault()
                        {
                            Data = "Password not match"
                        }
                    });
                }
            }
            else
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.InvalidPassword,
                    Result = new ResponseDefault()
                    {
                        Data = "Password invalid"
                    }
                });
            }

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
            else
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.BadRequest
                });
            }
        }

        /// <summary>
        /// Forget password
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto request, CancellationToken cancellationToken)
        {
            User user = await _userRepository.Users.FirstOrDefaultAsync(x => x.Username == request.Username && x.Email == request.Email);

            if (user == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "Not Found"
                    }
                });
            }

            var code = _identityServices.SendingPasswordByEmail("vovanban184@gmail.com", user.Email, "bbvimewwunotzlin");

            user.Password = _identityServices.GetMD5(code);
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
            else
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.BadRequest
                });
            }
        }



    }
}