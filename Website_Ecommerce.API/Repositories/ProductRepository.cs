using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _dataContext;

        public ProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<Product> Products => _dataContext.Products;

        public IUnitOfWork UnitOfWork => _dataContext;

        public IQueryable<ProductDetail> ProductDetails => _dataContext.ProductDetails;

        public IQueryable<ProductImage> ProductImages => _dataContext.ProductImages;

        public IQueryable<ProductCategory> ProductCategories => _dataContext.ProductCategories;

        public void Add(Product product)
        {
            _dataContext.Products.Add(product);
        }

        public void Add(ProductDetail productDetail)
        {
            _dataContext.ProductDetails.Add(productDetail);
        }

        public void Add(ProductImage productImage)
        {
            _dataContext.ProductImages.Add(productImage);
        }

        public void Add(ProductCategory productCategory)
        {
            _dataContext.ProductCategories.Add(productCategory);
        }

        public void Delete(Product product)
        {
            _dataContext.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Delete(ProductDetail productDetail)
        {
            _dataContext.Entry(productDetail).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Delete(ProductImage productImage)
        {
            _dataContext.Entry(productImage).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Delete(ProductCategory productCategory)
        {
            _dataContext.Entry(productCategory).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

        }


        public void Update(Product product)
        {
            _dataContext.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Update(ProductDetail productDetail)
        {
            _dataContext.Entry(productDetail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Update(ProductImage productImage)
        {
            _dataContext.Entry(productImage).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Update(ProductCategory productCategory)
        {
            _dataContext.Entry(productCategory).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        /// <summary>
        /// Avg Rating of product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<double> AvgRating(int id)
        {
            int[] listValueRate = await _dataContext.Comments.Where(x => x.Rate != 0).Select(x => x.Rate).ToArrayAsync();
            int totalRate = await _dataContext.Comments.CountAsync(x => x.ProductId == id);
            int sum = 0;
            foreach (int i in listValueRate)
            {
                sum += i;
            }
            double result = sum / totalRate;

            return Math.Round(result, 1);
        }

        /// <summary>
        /// Count rating of product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> CountRating(int id)
        {
            return await _dataContext.Comments.CountAsync(x => x.ProductId == id);
        }

        /// <summary>
        /// Get adll product
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductQueryModel>> GetAllProduct()
        {
            var products = _dataContext.Products;
            var productdetails = _dataContext.ProductDetails;
            var productimages = _dataContext.ProductImages;

            var pro_prodetail = products.Join(productdetails, p => p.Id, pd => pd.ProductId,
                                        (p, pd) => new
                                        {
                                            id = p.Id,
                                            name = p.Name,
                                            productdetailId = pd.Id,
                                            price = pd.Price,
                                            initialPrice = pd.InitialPrice,

                                        });


            var result = await pro_prodetail.Join(productimages, ppd => ppd.productdetailId, img => img.ProductDetailId,
                                        (ppd, img) => new ProductQueryModel
                                        {
                                            Id = ppd.id,
                                            Name = ppd.name,
                                            InitialPrice = ppd.initialPrice,
                                            Price = ppd.price,
                                            ImageURL = img.UrlImage,
                                            // Saled = ppd.saled
                                        }).ToListAsync();
            var result2 = (from p in result
                           group p by new { p.Id } //or group by new {p.ID, p.Name, p.Whatever}
                   into mygroup
                           select mygroup.FirstOrDefault()).OrderByDescending(x => x.Id).ToList();



            return result2;
        }

        /// <summary>
        /// Get list product of Shop by shopId
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public async Task<List<ProductQueryModel>> GetListProducByShop(int shopId)
        {
            var data = await _dataContext.Products
                .Where(x => x.ShopId == shopId)
                .Join(_dataContext.ProductDetails,
                product => product.Id,
                productDetail => productDetail.ProductId,
                (product, productDetail) => new { product, productDetail })
                .Join(_dataContext.ProductImages,
                productProductDetail => productProductDetail.productDetail.Id,
                productimage => productimage.ProductDetailId,
                (productProductDetail, productimage) => new ProductQueryModel
                {
                    Id = productProductDetail.product.Id,
                    Name = productProductDetail.product.Name,
                    Price = productProductDetail.productDetail.Price,
                    InitialPrice = productProductDetail.productDetail.InitialPrice,
                    ImageURL = productimage.UrlImage,
                    // Saled = productProductDetail.product.Saled
                })
                .ToListAsync();

            var result2 = (from p in data
                           group p by new { p.Id } //or group by new {p.ID, p.Name, p.Whatever}
                   into mygroup
                           select mygroup.FirstOrDefault()).OrderByDescending(x => x.Id).ToList();

            return result2;
        }

        /// <summary>
        /// Get list product of shop by shopId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ProductProductDetailQueryModel> GetProductById(int productId)
        {
            var data = await _dataContext.Products
                        .Where(x => x.Id == productId)
                        .Select(x => new ProductProductDetailQueryModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            TotalRate = x.TotalRate,
                            AverageRate = x.AverageRate,
                            Description = x.Description
                        })
                        .FirstOrDefaultAsync();

            return data;
        }

        /// <summary>
        /// Get list product detail by shop id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<List<ProductDetailQueryModel>> GetListProductDetailByProductId(int productId)
        {
            var data = await _dataContext.ProductDetails
                        .Where(x => x.ProductId == productId)
                        .Select(x => new ProductDetailQueryModel
                        {
                            Id = x.Id,
                            Size = x.Size,
                            Color = x.Color,
                            Amount = x.Amount,
                            Price = x.Price,
                            InitialPrice = x.InitialPrice
                        })
                        .ToListAsync();

            return data;
        }

        /// <summary>
        /// Get list product image by shop id
        /// </summary>
        /// <param name="productDetailId"></param>
        /// <returns></returns>
        public async Task<List<ProductImageQueryModel>> GetListProductImageByOfProductDetail(int productDetailId)
        {
            var data = await _dataContext.ProductImages
                        .Where(x => x.ProductDetailId == productDetailId)
                        .Select(x => new ProductImageQueryModel
                        {
                            Id = x.Id,
                            UrlImage = x.UrlImage
                        })
                        .ToListAsync();

            return data;
        }


        /// <summary>
        /// Search Product
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<ProductQueryModel>> SearchProduct(string key)
        {
            var data = await _dataContext.Products
                .Where(x => x.Name.ToLower().Contains(key))
                .Join(_dataContext.ProductDetails,
                product => product.Id,
                productDetail => productDetail.ProductId,
                (product, productDetail) => new { product, productDetail })
                .Join(_dataContext.ProductImages,
                productProductDetail => productProductDetail.productDetail.Id,
                productimage => productimage.ProductDetailId,
                (productProductDetail, productimage) => new ProductQueryModel
                {
                    Id = productProductDetail.product.Id,
                    Name = productProductDetail.product.Name,
                    Price = productProductDetail.productDetail.Price,
                    InitialPrice = productProductDetail.productDetail.InitialPrice,
                    ImageURL = productimage.UrlImage,
                    // Saled = productProductDetail.product.Saled
                })
                .ToListAsync();

            return data;
        }
        // public async Task<List<ShopViewProductDto>> ShopManagerProduct(int id){
        //     var productDetails = _dataContext.ProductDetails.Sum(s => s.Saled);
        //     var products =  _dataContext.Products.Where(p => p.ShopId == id)
        //                     .Join(productDetails,
        //                     product => product.Id,
        //                     productdetail => productdetail.ProductId,
        //                     (product, productdetail) => new{


        //                     });
        // }

    }
}