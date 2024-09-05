using System;
using System.Threading.Tasks;
using TableTennis.Model;

namespace TableTennis.Repository.Common
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> RegisterUserAsync(User user);
        Task<Guid?> GetRoleIdByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
    }
}
