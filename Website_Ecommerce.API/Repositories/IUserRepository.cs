using Website_Ecommerce.API.Data.Entities;
using Website_Ecommerce.API.ModelQueries;

namespace Website_Ecommerce.API.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        IQueryable<User> Users { get; }
        void Add(User user);
        void Update(User user);
        void Delete(User user);

        IQueryable<UserRole> UserRoles { get; }
        void Add(UserRole userRole);
        void Update(UserRole userRole);
        void Delete(UserRole userRole);

        IQueryable<Role> Roles { get; }
        void Add(Role role);
        void Update(Role role);
        void Delete(Role role);

        /// <summary>
        /// Get info user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InfoUser> GetInfotUserById(int id);

    }
}