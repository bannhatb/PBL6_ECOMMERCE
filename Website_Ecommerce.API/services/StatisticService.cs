using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.Enum;
using Website_Ecommerce.API.ModelDtos;

namespace Website_Ecommerce.API.services
{
    public class StatisticService : IStatisticService
    {
        private DataContext _dataContext;
        public StatisticService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IList<ShopStatisticProductDto>> StatisticProduct(int idShop)
        {
            var products = _dataContext.Products;
            var productdetails = _dataContext.ProductDetails;
            var orderConfirmed = _dataContext.OrderDetails.Where(x => x.State == (int)StateOrderEnum.SHOP_SENT && x.ShopId == idShop);
            var orderUnConfirm = _dataContext.OrderDetails.Where(x => x.State == (int)StateOrderEnum.SENT && x.ShopId == idShop);

            var result = await products.Join(productdetails, 
                                        p => p.Id,
                                        pd => pd.ProductId,
                                        (p, pd) => new ShopStatisticProductDto{
                                            Id = pd.Id,
                                            Name = p.Name,
                                            Size = pd.Size,
                                            Color = pd.Color,
                                            Sold = pd.Saled,
                                            Booked = pd.Booked,
                                            Inventory = pd.Amount
                                        }).ToListAsync();
            return result;
            // var listSold = orderConfirmed.GroupBy(o => o.ProductDetailId)
            //                                         .Select(od => new OrderDetail
            //                                         {
            //                                             ProductDetailId = od.First().ProductDetailId,
            //                                             Amount = od.Sum(d => d.Amount)
            //                                         });

            // var sumOrderUnconfirmed = orderUnConfirm.GroupBy(o => o.ProductDetailId)
            //                                         .Select(od => new OrderDetail
            //                                         {
            //                                             ProductDetailId = od.First().ProductDetailId,
            //                                             Amount = od.Sum(d => d.Amount)
            //                                         });
            // var order_product_detail = productdetails.GroupJoin(listSold, p => p.Id, s => s.ProductDetailId, (p, sol) =>
            //                                                     new
            //                                                     {
            //                                                         p = p,
            //                                                         sol = sol
            //                                                     }).SelectMany(temp => temp.sol.DefaultIfEmpty(),
            //                                                     (temp, Sol) =>
            //                                                     new
            //                                                     {
            //                                                         Sold = (Sol == null) ? "0" : Sol.Amount.ToString(),
            //                                                         Id = temp.p.Id,
            //                                                         ProductId = temp.p.ProductId,
            //                                                         Size = temp.p.Size,
            //                                                         Color = temp.p.Color,
            //                                                         Inventory = temp.p.Amount
                                                                    

            //                                                     }).Join(products, x => x.ProductId,
            //                                                             y => y.Id, (x,y) => new ShopStatisticProductDto{
            //                                                                 Id = x.Id,
            //                                                                 Name = y.Name,
            //                                                                 Size = x.Size,
            //                                                                 Color = x.Color,
            //                                                                 Sold = x.Sold,
            //                                                                 Booked = x.Booked,

            //                                                             })
            //                                                     ;


            // var sumInventory =  order_product_detail.Join(sumOrderUnconfirmed, op => op.Id, u => u.ProductDetailId, (op, u)=>
            //                                             new {
            //                                                 op = op,

            //                                         });

            // return await products.Join(sumInventory, p => p.Id, o => o.ProductId,(p, o)
            //                         => new ShopStatisticProductDto{
            //                                 Id = p.Id,
            //                                 Name = p.Name,
            //                                 Size = o.Size,
            //                                 Color = o.Color,
            //                                 Sold = o.Sold,
            //                                 Inventory = o.Inventory,
            //                                 Booked = o.Booked,
            // }).ToListAsync();;
            // throw new InvalidOperationException("Invalid");
        }
    }
}