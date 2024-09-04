using System.Threading.Tasks;
using TableTennis.Model;
using DTO.UserModel;

namespace TableTennis.Service.Common
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(User user);
        Task<User> LoginAsync(string email, string password);
    }
}
