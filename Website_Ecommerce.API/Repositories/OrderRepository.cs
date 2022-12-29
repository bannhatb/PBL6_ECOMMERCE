using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.Enum;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _dataContext;

        public IUnitOfWork UnitOfWork => _dataContext;

        public IQueryable<Order> Orders => _dataContext.Orders;

        public IQueryable<OrderDetail> OrderDetails => _dataContext.OrderDetails;

        public OrderRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Order order)
        {
            _dataContext.Orders.Add(order);
        }

        /// <summary>
        /// Get last order
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Order> GetLastOrder(int userId)
        {
            var Orders = _dataContext.Orders;
            var orderDetails = _dataContext.OrderDetails;

            return await _dataContext.Orders.LastOrDefaultAsync(x => x.UserId == userId);
        }

        /// <summary>
        /// Get all order of user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Order>> GetAllOrderOfUser(int id)
        {
            return await _dataContext.Orders.Where(x => x.UserId == id).ToListAsync();
        }

        /// <summary>
        /// Get order detail
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>

        public void Delete(Order order)
        {
            _dataContext.Entry(order).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Add(OrderDetail orderDetail)
        {
            _dataContext.OrderDetails.Add(orderDetail);
        }

        public void Delete(OrderDetail orderDetail)
        {
            _dataContext.Entry(orderDetail).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

        }

        public void Update(OrderDetail orderDetail)
        {
            _dataContext.Entry(orderDetail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Update(Order order)
        {
            _dataContext.Entry(order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public async Task<List<OrderDetailShowbyShop>> GetOrderDetail(int id)
        {
            var orderdetails = _dataContext.OrderDetails.Where(x => x.ShopId == id);
            var products = _dataContext.Products.Where(x => x.ShopId == id);
            var productdetails = _dataContext.ProductDetails;
            var voucherproducts = _dataContext.VoucherProducts;
            var orders = _dataContext.Orders;

            var result = orderdetails.Join(productdetails,
                                        od => od.ProductDetailId,
                                        pd => pd.Id,
                                        (od, pd) => new {
                                            id = od.OrderId,
                                            size = pd.Size,
                                            color = pd.Color,
                                            amount = od.Amount,
                                            confirm = od.ShopConfirmDate,
                                            price = od.Price,
                                            note = od.Note,
                                            state = od.State,
                                            idproduct = pd.ProductId,
                                            voucher = od.VoucherProductId
                                        }).Join(voucherproducts, x => x.voucher, v => v.Id
                                                ,(x, v) => new { 
                                                    id = x.id,
                                                    size = x.size,
                                                    color = x.color,
                                                    amount = x.amount,
                                                    confirm = x.confirm,
                                                    price = x.price,
                                                    note = x.note,
                                                    state = x.state,
                                                    idproduct = x.idproduct,
                                                    voucher = v.Value
                                                });
            var result2 = result.Join(products, opd => opd.idproduct
                                                , p => p.Id,
                                                (opd, p)=> new {
                                                    id = opd.id,
                                                    name = p.Name,
                                                    size = opd.size,
                                                    color = opd.color,
                                                    amount = opd.amount,
                                                    confirm = opd.confirm,
                                                    voucher = opd.voucher,
                                                    price = opd.price,
                                                    note = opd.note,
                                                    state = opd.state,
                                                });
            var result3 = await result2.Join(orders, x => x.id, o => o.Id,
                                                (x, o) => new OrderDetailShowbyShop{
                                                    OrderId = x.id,
                                                    Name = x.name,
                                                    Size = x.size,
                                                    Color = x.color,
                                                    Amount = x.amount,
                                                    ConfirmAt = x.confirm,
                                                    CreateAt = o.CreateDate,
                                                    Price = x.price,
                                                    DiscountShop = x.voucher,
                                                    Note = x.note,
                                                    Sate = x.state

                                    }).OrderByDescending(x => x.OrderId).ToListAsync();
            return result3;
        }
    }
}