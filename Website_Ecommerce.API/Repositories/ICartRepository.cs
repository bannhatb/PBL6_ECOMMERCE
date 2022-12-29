using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        void Add(Cart item);
        void Update(Cart item);
        void Delete(Cart item);
        IQueryable<Cart> Carts { get; }
        /// <summary>
        /// Get all item by userId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ItemCartQueryModel>> GetAllItemByIdUser(int id);
    }
}