using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Add(Order order);
        void Delete(Order order);
        void Add(OrderDetail orderDetail);
        void Update(Order order);
        void Update(OrderDetail orderDetail);
        void Delete(OrderDetail orderDetail);
        IQueryable<Order> Orders { get; }
        IQueryable<OrderDetail> OrderDetails { get; }
        /// <summary>
        /// <param name = "id"></param>
        /// </summary>
        Task<Order> GetLastOrder(int id);
        /// <summary>
        /// <param name = "id"></param>
        /// </summary>
        Task<IEnumerable<Order>> GetAllOrderOfUser(int id);
        /// <summary>
        /// <param name = "id"></param>
        /// </summary>
        Task<List<OrderDetailShowbyShop>> GetOrderDetail(int id);

    }
}