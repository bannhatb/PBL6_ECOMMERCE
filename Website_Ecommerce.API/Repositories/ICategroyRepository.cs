using Website_Ecommerce.API.Data.Entities;

namespace Website_Ecommerce.API.Repositories
{
    public interface ICategroyRepository : IRepository<Category>
    {
        IQueryable<Category> Categories { get; }
        void Add(Category category);
        void Update(Category category);
        void Delete(Category category);
    }
}