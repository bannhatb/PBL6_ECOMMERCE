using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Repositories
{
    public class ShopRepository : IShopRepository
    {
        private readonly DataContext _dataContext;

        public ShopRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<Shop> Shops => _dataContext.Shops;

        public IUnitOfWork UnitOfWork => _dataContext;

        public IQueryable<VoucherProduct> voucherProducts => _dataContext.VoucherProducts;
        public void Add(Shop shop)
        {
            _dataContext.Shops.Add(shop);
        }

        public void Add(VoucherProduct voucherProduct)
        {
            _dataContext.VoucherProducts.Add(voucherProduct);
        }

        public void Delete(Shop shop)
        {
            _dataContext.Entry(shop).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Delete(VoucherProduct voucherProduct)
        {
            _dataContext.Entry(voucherProduct).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public async Task<List<VoucherProduct>> GetVoucherMatch(ProductDetail productdetail)
        {
            return await _dataContext.VoucherProducts.Where(p => p.MinPrice < productdetail.Price).ToListAsync();
        }

        public void Update(Shop shop)
        {
            _dataContext.Entry(shop).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Update(VoucherProduct voucherProduct)
        {
            _dataContext.Entry(voucherProduct).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public async Task<int> CountTotalProduct(int id)
        {
            return await _dataContext.Products.CountAsync(x => x.ShopId == id);
        }

        public async Task<int> CountTotalRating(int id)
        {
            var listQuantityRate = await _dataContext.Shops
            .Join(_dataContext.Products,
            shop => shop.Id,
            product => product.ShopId,
            (shop, product) => new { shop, product })
            .Where(x => x.shop.Id == id)
            .Select(x => x.product.TotalRate)
            .ToListAsync();

            int result = 0;
            foreach (var i in listQuantityRate)
            {
                result += i;
            }

            return result;
        }

        public Task<double> AvgRating(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get info shop by id 
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public async Task<InfoShop> GetInfoShopBy(int shopId)
        {
            var data = await _dataContext.Shops
                        .Where(x => x.Id == shopId)
                        .Select(x => new InfoShop
                        {
                            Address = x.Address,
                            Email = x.Email,
                            AverageRate = x.AverageRate,
                            Name = x.Name,
                            Phone = x.Phone,
                            TotalProduct = x.TotalProduct,
                            TotalRate = x.TotalRate,
                            UrlAvatar = x.UrlAvatar
                        })
                        .FirstOrDefaultAsync();
            return data;
        }

    }
}