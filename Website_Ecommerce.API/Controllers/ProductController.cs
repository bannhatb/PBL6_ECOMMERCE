
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
    //1:admin  2:shop  3:shipper  4:customer
    // [CustomAuthorize(Allows = "2")]

    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public ProductController(
            IProductRepository productRepository,
            IShopRepository shopRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IHttpContextAccessor httpContext)
        {
            _productRepository = productRepository;
            _shopRepository = shopRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContext = httpContext;

        }
        #region Product

        /// <summary>
        /// Add Product
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto request, CancellationToken cancellationToken)
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            var shop = _shopRepository.Shops.FirstOrDefault(x => x.UserId == userId);
            Product product = new Product()
            {
                Name = request.Name,
                Material = request.Material,
                ShopId = shop.Id, //get shopid from token
                Origin = request.Origin,
                Description = request.Description,
                Status = request.Status,
                TotalRate = 0,

                AverageRate = 0,
            };
            _productRepository.Add(product);

            var result = await _productRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Add product fail"
                    }
                });
            }

            // Add ProductCategories
            ProductCategory productCategory = new ProductCategory();
            foreach (var items in request.Categories)
            {
                productCategory.ProductId = product.Id;
                productCategory.CategoryId = items;
                _productRepository.Add(productCategory);
                var result1 = await _productRepository.UnitOfWork.SaveAsync(cancellationToken);
                if (result1 == 0)
                {
                    return BadRequest(new Response<ResponseDefault>()
                    {
                        State = false,
                        Message = ErrorCode.ExcuteDB,
                        Result = new ResponseDefault()
                        {
                            Data = "Add product category fail"
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
                    Data = "Add Product success"
                }
            });
        }

        [HttpPost("Add-product-full")]
        public async Task<IActionResult> AddProductFull([FromBody] ProductFullQueryModel request, CancellationToken cancellationToken)
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            var shop = _shopRepository.Shops.FirstOrDefault(x => x.UserId == userId);
            Product product = new Product()
            {
                Id = 0,
                Status = false,
                TotalRate = 0,
                AverageRate = 0,
                Saled = 0
            };
            product.Name = request.Name;
            product.Material = request.Material;
            product.Description = request.Description;
            product.Origin = request.Origin;
            product.ShopId = shop.Id;

            _productRepository.Add(product);
            var result = await _productRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Add product category fail"
                    }
                });
            }
            Product thisProduct = _productRepository.Products.Where(x => x.ShopId == shop.Id).OrderByDescending(p => p.Id).FirstOrDefault();
            foreach (var i in request.productdetails)
            {
                ProductDetail productDetail = new ProductDetail()
                {
                    Id = 0,
                    Size = i.Size,
                    Color = i.Color,
                    InitialPrice = i.InitialPrice,
                    Price = i.Price,
                    Amount = i.Amount,
                    Saled = 0,
                    Booked = 0,
                    ProductId = thisProduct.Id

                };
                _productRepository.Add(productDetail);
                try
                {
                    await _productRepository.UnitOfWork.SaveAsync(cancellationToken);
                }
                catch
                {
                    _productRepository.Delete(thisProduct);
                    await _productRepository.UnitOfWork.SaveAsync(cancellationToken);
                    return BadRequest(new Response<ResponseDefault>()
                    {
                        State = false,
                        Message = ErrorCode.ExcuteDB,
                        Result = new ResponseDefault()
                        {
                            Data = "Add product detail fail"
                        }
                    }
                    );
                }
                var thisProductDetail = _productRepository.ProductDetails.Where(p => p.Id == productDetail.Id).OrderByDescending(p => p.Id).FirstOrDefault();
                foreach (var j in i.ProductImages)
                {
                    ProductImage productimage = new ProductImage()
                    {
                        Id = 0,
                        ProductDetailId = thisProductDetail.Id,
                        UrlImage = j.UrlImage
                    };
                    _productRepository.Add(productimage);
                    try
                    {
                        await _productRepository.UnitOfWork.SaveAsync(cancellationToken);
                    }
                    catch
                    {
                        _productRepository.Delete(productimage);
                        _productRepository.Delete(thisProductDetail);
                        await _productRepository.UnitOfWork.SaveAsync(cancellationToken);
                        return BadRequest(new Response<ResponseDefault>()
                        {
                            State = false,
                            Message = ErrorCode.ExcuteDB,
                            Result = new ResponseDefault()
                            {
                                Data = "Add product detail fail"
                            }
                        }
                        );
                    }
                }
            }


            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Add Product success"
                }
            });
        }
        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto request, CancellationToken cancellationToken)
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            var shop = _shopRepository.Shops.FirstOrDefault(x => x.UserId == userId);
            // Lay userId, nguoi tao la nguoi xoa
            Product product = _productRepository.Products.FirstOrDefault(p => p.Id == request.Id && p.ShopId == shop.Id);
            if (product == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "Not Found Product"
                    }
                });
            }

            product = _mapper.Map(request, product);
            _productRepository.Update(product);

            List<ProductCategory> cateOld = _productRepository.ProductCategories.Where(p => p.ProductId == product.Id).ToList();
            if (cateOld.Count != 0)
            {
                // Get nhung category khong thay doi
                HashSet<ProductCategory> cateSame = new HashSet<ProductCategory>();
                foreach (int cateId in request.Categories)
                {
                    ProductCategory same = cateOld.FirstOrDefault(x => x.CategoryId == cateId);
                    if (same != null)
                    {
                        cateSame.Add(same);
                    }
                }
                // Get nhung category khong con (delete) 
                List<ProductCategory> cateDel = cateOld.Except(cateSame).ToList();
                foreach (ProductCategory examDel in cateDel)
                {
                    _productRepository.Delete(examDel);
                }
                HashSet<int> cateNew = request.Categories
                    .Except(cateSame.Select(x => x.CategoryId)
                    .ToHashSet()).ToHashSet();
                foreach (int cateIdNew in cateNew)
                {
                    _productRepository.Add(new ProductCategory() { CategoryId = cateIdNew, ProductId = product.Id });
                }
            }
            else
            {
                foreach (int cateId in request.Categories)
                {
                    _productRepository.Add(new ProductCategory() { CategoryId = cateId, ProductId = product.Id });
                }
            }
            int result = await _productRepository.UnitOfWork.SaveAsync(cancellationToken);

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = product.Id.ToString() + " update success"
                    }
                });
            }
            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Update product fail"
                }
            });
        }

        /// <summary>
        /// Delete product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
            var shop = _shopRepository.Shops.FirstOrDefault(x => x.UserId == userId);

            if (id.ToString() is null)
            {
                return BadRequest(null);
            }
            // Get shopId, check shop nao tao => shop do xoa
            Product product = await _productRepository.Products.FirstOrDefaultAsync(p => p.Id == id && p.ShopId == shop.Id);
            if (product == null)
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
            product.Status = false;
            _productRepository.Update(product);
            var result = await _productRepository.UnitOfWork.SaveAsync();

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = product.Id.ToString()
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

        #endregion

        #region ProductDetail

        /// <summary>
        /// Add ProductDetail
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("add-product-detail")]
        public async Task<IActionResult> AddProductDetail([FromBody] ProductDetailDto request, CancellationToken cancellationToken)
        {
            var productDetail = _mapper.Map<ProductDetailDto, ProductDetail>(request);

            _productRepository.Add(productDetail);
            var result = await _productRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Add ProductDetail fail"
                    }
                });
            }
            return Ok(new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Add ProductDetail success"
                }
            });
        }

        /// <summary>
        /// Update ProductDetail
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("update-product-detail")]
        public async Task<IActionResult> UpdateProductDetail([FromBody] ProductDetailDto request, CancellationToken cancellationToken)
        {
            ProductDetail productDetail = _productRepository.ProductDetails.FirstOrDefault(p => p.Id == request.Id);
            if (productDetail == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "Not Found ProductDetail"
                    }
                });
            }
            productDetail = _mapper.Map<ProductDetail>(request);

            _productRepository.Update(productDetail);
            var result = await _productRepository.UnitOfWork.SaveAsync(cancellationToken);

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = productDetail.Id.ToString()
                    }
                });
            }
            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Update ProductDetail fail"
                }
            });
        }

        /// <summary>
        /// Delete ProductDetail by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete-product-detail-by/{id}")]
        public async Task<IActionResult> DeleteProductDetail(int id)
        {
            if (id.ToString() is null)
            {
                return BadRequest(null);
            }

            ProductDetail product = await _productRepository.ProductDetails.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "NotFound ProductImage"
                    }
                });
            }

            _productRepository.Delete(product);
            var result = await _productRepository.UnitOfWork.SaveAsync();

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = product.Id.ToString()
                    }
                });
            }
            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Delete ProductDetail fail"
                }
            });
        }


        #endregion

        #region ProductImage

        /// <summary>
        /// Add ProductImage
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("add-product-image")]
        public async Task<IActionResult> AddProductImage([FromBody] ProductImageDto request, CancellationToken cancellationToken)
        {
            ProductImage productImage = new ProductImage
            {
                UrlImage = request.UrlImage,
                ProductDetailId = request.ProductDetailId
            };
            _productRepository.Add(productImage);

            var result = await _productRepository.UnitOfWork.SaveAsync(cancellationToken);
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
        /// Delete ProductImage by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete-product-image-by/{id}")]
        public async Task<IActionResult> DeleteProductImage([FromQuery] int id)
        {
            if (id.ToString() is null)
            {
                return BadRequest(null);
            }
            // Get shopId, check shop tao => moi xoa
            ProductImage product = await _productRepository.ProductImages.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return BadRequest(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "NotFound ProductImage"
                    }
                });
            }
            _productRepository.Delete(product);

            var result = await _productRepository.UnitOfWork.SaveAsync();

            if (result > 0)
            {
                return Ok(new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = product.Id.ToString()
                    }
                });
            }

            return BadRequest(new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.ExcuteDB,
                Result = new ResponseDefault()
                {
                    Data = "Delete ProductImage fail"
                }
            });

        }

        #endregion

    }
}