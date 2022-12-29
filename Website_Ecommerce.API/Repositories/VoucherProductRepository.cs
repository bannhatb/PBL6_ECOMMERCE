using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Repositories
{
    public class VoucherProductRepository : IVoucherProductRepository
    {
        private readonly DataContext _dataContext;
        public IUnitOfWork UnitOfWork => _dataContext;

        public IQueryable<VoucherProduct> VoucherProducts => _dataContext.VoucherProducts;

        public VoucherProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(VoucherProduct voucherProduct)
        {
            _dataContext.VoucherProducts.Add(voucherProduct);
        }

        public void Delete(VoucherProduct voucherProduct)
        {
            _dataContext.Entry(voucherProduct).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Update(VoucherProduct voucherProduct)
        {
            _dataContext.Entry(voucherProduct).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        }
        /// <summary>
        /// Get list VoucherProduct by shopId
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public async Task<List<VoucherProductQueryModel>> GetListVoucherProductById(int shopId)
        {
            var data = await _dataContext.VoucherProducts
                                    .Where(x => x.ShopId == shopId)
                                    .Select(x => new VoucherProductQueryModel
                                    {
                                        Id = x.Id,
                                        Value = x.Value,
                                        MinPrice = x.MinPrice,
                                        Amount = x.Amount,
                                        CreateAt = x.CreateAt,
                                        Expired = x.Expired,
                                        Description = x.Description
                                    })
                                    .ToListAsync();

            return data;
        }
    }
}