using Microsoft.EntityFrameworkCore;
using Website_Ecommerce.API.Data;
using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<User> Users => _dataContext.Users;

        public IQueryable<UserRole> UserRoles => _dataContext.UserRoles;

        public IUnitOfWork UnitOfWork => _dataContext;

        public IQueryable<Role> Roles => _dataContext.Roles;

        public void Add(User user)
        {
            _dataContext.Users.Add(user);
        }

        public void Add(UserRole userRole)
        {
            _dataContext.UserRoles.Add(userRole);
        }

        public void Add(Role role)
        {
            _dataContext.Roles.Add(role);
        }

        public void Delete(User user)
        {
            _dataContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Delete(UserRole userRole)
        {
            _dataContext.Entry(userRole).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Delete(Role role)
        {
            _dataContext.Entry(role).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Update(User user)
        {
            _dataContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Update(UserRole userRole)
        {
            _dataContext.Entry(userRole).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Update(Role role)
        {
            _dataContext.Entry(role).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        /// <summary>
        /// Get info user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InfoUser> GetInfotUserById(int id)
        {
            var data = await _dataContext.Users
                        .Where(x => x.Id == id)
                        .Select(x => new InfoUser
                        {
                            Email = x.Email,
                            DateOfBirth = x.DateOfBirth,
                            Gender = x.Gender,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            Phone = x.Phone,
                            UrlAvatar = x.UrlAvatar
                        })
                        .FirstOrDefaultAsync();
            return data;
        }
    }
}