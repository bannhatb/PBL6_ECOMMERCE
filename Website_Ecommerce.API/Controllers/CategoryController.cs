using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelDtos;
using Website_Ecommerce.API.ModelQueries;
using Website_Ecommerce.API.Repositories;
using Website_Ecommerce.API.Response;

namespace Website_Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "MyAuthKey")]
    // [CustomAuthorize(Allows = "2")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategroyRepository _categroyRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public CategoryController(
            ICategroyRepository categroyRepository,
            IMapper mapper,
            IHttpContextAccessor httpContext)
        {
            _categroyRepository = categroyRepository;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        /// <summary>
        /// Add category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto request, CancellationToken cancellationToken)
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());

            Category category = new Category
            {
                Name = request.Name,
                CreateBy = userId,
                DateCreate = DateTime.Now
            };
            _categroyRepository.Add(category);
            var result = await _categroyRepository.UnitOfWork.SaveAsync(cancellationToken);

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = category.Id.ToString()
                    }
                });
            }
            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Excute Db error"
                }
            });
        }

        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("update-category")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto request, CancellationToken cancellationToken)
        {
            Category category = _categroyRepository.Categories.FirstOrDefault(c => c.Id == request.Id);
            if (category == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "Update category fail"
                    }
                });
            }

            category.Name = request.Name;
            _categroyRepository.Update(category);
            var result = await _categroyRepository.UnitOfWork.SaveAsync(cancellationToken);

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = category.Id.ToString()
                    }
                });
            }
            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Update category fail"
                }
            });
        }

        /// <summary>
        /// Delete category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Category category = _categroyRepository.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "Delete category fail"
                    }
                });
            }

            _categroyRepository.Delete(category);
            var result = await _categroyRepository.UnitOfWork.SaveAsync();

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = category.Id.ToString()
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
        /// Get list category
        /// </summary>
        /// <returns></returns>
        [HttpGet("list-category")]
        public async Task<IActionResult> GetListCategory()
        {
            var categories = await _categroyRepository.Categories.Select(x => new CategoryQueryModel { Id = x.Id, Name = x.Name }).ToListAsync();

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = categories
                }
            });
        }

        /// <summary>
        /// Get category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-category-by/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            return Ok(await _categroyRepository.Categories
                .Select(x => new CategoryQueryModel { Id = x.Id, Name = x.Name })
                .FirstOrDefaultAsync(x => x.Id == id)
            );
        }
    }
}