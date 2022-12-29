
using Microsoft.AspNetCore.Mvc;
using Website_Ecommerce.API.Response;
using Website_Ecommerce.API.services;
using Newtonsoft.Json;
using Website_Ecommerce.API.ModelDtos;
using Website_Ecommerce.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class HomeController : ControllerBase
    {
        private readonly IHostEnvironment _environment;
        private readonly IProductRepository _productRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        public HomeController(
            IHostEnvironment environment,
            IProductRepository productRepository,
            IShopRepository shopRepository,
            IUserRepository userRepository,
            ICommentRepository commentRepository,
            IMapper mapper
            )
        {
            _environment = environment;
            _productRepository = productRepository;
            _shopRepository = shopRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// UpFile
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UpFile([FromForm] List<IFormFile> files)
        {
            const bool AllowLimitFileSize = true;

            var baseUrl = "https://localhost:7220";
            var listFileError = new List<FileUploadInfo>();
            var limitFileSize = 8388608;
            string result = "";

            if (files.Count <= 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = "Please select file to upload",
                    Result = new ResponseDefault()
                    {
                        Data = result
                    }
                });
            }
            // var listFileTypeAllow = "jpg|png|gif|xls|xlsx";   

            if (listFileError.Count() > 0)
            {

                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = JsonConvert.SerializeObject(listFileError)
                    }
                });
            }

            if (AllowLimitFileSize)
            {
                foreach (var i in files)
                {
                    if (i.Length > limitFileSize)
                    {
                        listFileError.Add(new FileUploadInfo()
                        {
                            filename = i.FileName,
                            filesize = i.Length
                        });
                    }
                }
            }

            var listLinkUploaded = new List<string>();
            if (listFileError.Count() > 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = JsonConvert.SerializeObject(listFileError)
                    }
                });
            }

            foreach (var i in files)
            {
                if (i.Length > 0)
                {
                    var templateUrl = i.FileName;
                    string filePath = Path.Combine($"{_environment.ContentRootPath}/wwwroot/", templateUrl);
                    string fileName = Path.GetFileName(filePath);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await i.CopyToAsync(stream);
                    }
                    listLinkUploaded.Add($"{baseUrl}/wwwroot/{i.FileName}");
                }

            }

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = JsonConvert.SerializeObject(listLinkUploaded)
                }
            });
        }

        /// <summary>
        /// Get all product
        /// </summary>
        /// <returns></returns>get list product
        [HttpGet("get-list-product")]
        public async Task<IActionResult> GetListProduct()
        {
            var products = await _productRepository.GetAllProduct();

            if (products == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "NotFound Product"
                    }
                });
            }

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = products
                }
            });
        }

        /// <summary>
        /// Get ProductDetail by ProductId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("get-product-detail-by/{productId}")]
        public async Task<IActionResult> GetProductDetailByProductId(int productId)
        {
            ProductProductDetailQueryModel productProductDetail = await _productRepository.GetProductById(productId);
            productProductDetail.ProductDetails = await _productRepository.GetListProductDetailByProductId(productId);
            // var productDetails = productProductDetail.ProductDetails;
            foreach (var item in productProductDetail.ProductDetails)
            {
                item.ProductImages = await _productRepository.GetListProductImageByOfProductDetail(item.Id);
            }

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = productProductDetail
                }
            });
        }

        /// <summary>
        /// Get ProductDetail by productDetailId
        /// </summary>
        /// <param name="productDetailId"></param>
        /// <returns></returns>
        [HttpGet("get-productdetail-by/{productDetailId}")]
        public async Task<IActionResult> GetProductDetailById(int productDetailId)
        {
            ProductDetail productDetail = await _productRepository.ProductDetails.Where(p => p.Id == productDetailId).FirstOrDefaultAsync();
            ProductDetailDto productDetailDto = _mapper.Map<ProductDetailDto>(productDetail);
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = productDetailDto
                }
            });
        }

        /// <summary>
        /// Get list product by shopId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-list-product-of-shop-by/{id}")]
        public async Task<IActionResult> GetListProducByShop(int id)
        {
            if (id.ToString() is null)
            {
                return BadRequest(null);
            }

            var listProduct = await _productRepository.GetListProducByShop(id);

            if (listProduct == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "NotFound Product"
                    }
                });
            }

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = listProduct
                }
            });
        }

        /// <summary>
        /// Search product by productName, categoryName
        /// </summary>
        /// <returns></returns>
        [HttpGet("search-product-by/{key}")]
        public async Task<IActionResult> SearchProduct(string key)
        {
            if (key is null)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = await _productRepository.GetAllProduct()
                    }
                });
            }

            var listProduct = await _productRepository.SearchProduct(key);

            if (listProduct == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "NotFound Product"
                    }
                });
            }

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = listProduct
                }
            });
        }

        /// <summary>
        /// List comment of product 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("list-comment-by/{id}")]
        public async Task<IActionResult> GetListComment(int productId)
        {
            var listCommentDetails = await _commentRepository.GetCommentDetails();

            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = listCommentDetails
                }
            });
        }
    }


}