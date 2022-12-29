using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Repositories
{
    public interface IShopRepository : IRepository<Shop>
    {
        IQueryable<Shop> Shops { get; }
        void Add(Shop shop);
        void Update(Shop shop);
        void Delete(Shop shop);
        IQueryable<VoucherProduct> voucherProducts { get; }
        void Add(VoucherProduct voucherProduct);
        void Update(VoucherProduct voucherProduct);
        void Delete(VoucherProduct voucherProduct);

        /// <summary>
        /// Get voucher match
        /// </summary>
        /// <param name="productdetail"></param>
        /// <returns></returns>
        Task<List<VoucherProduct>> GetVoucherMatch(ProductDetail productdetail);

        /// <summary>
        /// Count totalRating
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> CountTotalRating(int id);

        /// <summary>
        /// Count totalProduct
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> CountTotalProduct(int id);

        /// <summary>
        /// Avg Rating
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<double> AvgRating(int id);

        /// <summary>
        /// Get info shop by id 
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        Task<InfoShop> GetInfoShopBy(int shopId);

    }
}