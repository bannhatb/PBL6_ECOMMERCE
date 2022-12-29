using Website_Ecommerce.API.Data;
using Website_Ecommerce.API.Data.Entities;

namespace Website_Ecommerce.API.Repositories
{
    public class CategroyRepository : ICategroyRepository
    {
        private readonly DataContext _dataContext;

        public CategroyRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<Category> Categories => _dataContext.Categories;

        public IUnitOfWork UnitOfWork => _dataContext;

        public void Add(Category category)
        {
            _dataContext.Categories.Add(category);
        }

        public void Delete(Category category)
        {
            _dataContext.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Update(Category category)
        {
            _dataContext.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}