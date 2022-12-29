using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Repositories
{
    public interface IVoucherProductRepository : IRepository<VoucherProduct>
    {
        void Add(VoucherProduct voucherProduct);
        void Update(VoucherProduct voucherProduct);
        void Delete(VoucherProduct voucherProduct);
        IQueryable<VoucherProduct> VoucherProducts { get; }

        /// <summary>
        /// Get list VoucherProduct by shopId
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        Task<List<VoucherProductQueryModel>> GetListVoucherProductById(int shopId);
    }
}