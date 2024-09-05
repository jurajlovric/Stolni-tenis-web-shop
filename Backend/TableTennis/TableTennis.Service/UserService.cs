using System;
using System.Threading.Tasks;
using TableTennis.Model;
using TableTennis.Repository.Common;
using TableTennis.Service.Common;

namespace TableTennis.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new Exception("Korisnik s ovim emailom već postoji.");
            }

            user.UserId = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            await _userRepository.RegisterUserAsync(user);

            return user;
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || user.Password != password)
            {
                throw new Exception("Pogrešan email ili lozinka.");
            }
            return user;
        }
    }
}
