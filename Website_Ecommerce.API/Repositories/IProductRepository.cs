using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IQueryable<Product> Products { get; }

        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
        IQueryable<ProductDetail> ProductDetails { get; }
        void Add(ProductDetail productDetail);
        void Update(ProductDetail productDetail);
        void Delete(ProductDetail productDetail);
        IQueryable<ProductImage> ProductImages { get; }
        void Add(ProductImage productImage);
        void Update(ProductImage productImage);
        void Delete(ProductImage productImage);

        IQueryable<ProductCategory> ProductCategories { get; }
        void Add(ProductCategory productCategory);
        void Update(ProductCategory productCategory);
        void Delete(ProductCategory productCategory);
        /// <summary>
        /// Get all product
        /// </summary>
        /// <returns></returns>
        Task<List<ProductQueryModel>> GetAllProduct();

        /// <summary>
        /// Count Rating of Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> CountRating(int id);

        /// <summary>
        /// AvgRating of Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<double> AvgRating(int id);

        /// <summary>
        /// Get list product of shop by shopId
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        Task<List<ProductQueryModel>> GetListProducByShop(int shopId);

        /// <summary>
        /// Search Product
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<List<ProductQueryModel>> SearchProduct(string key);

        /// <summary>
        /// Get list product of shop by shopId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ProductProductDetailQueryModel> GetProductById(int productId);

        /// <summary>
        /// Get list product detail by shop id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<List<ProductDetailQueryModel>> GetListProductDetailByProductId(int productId);

        /// <summary>
        /// Get list product image by shop id
        /// </summary>
        /// <param name="productDetailId"></param>
        /// <returns></returns>
        Task<List<ProductImageQueryModel>> GetListProductImageByOfProductDetail(int productDetailId);
    }
}