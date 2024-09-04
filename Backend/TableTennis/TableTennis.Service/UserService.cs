using System;
using System.Threading.Tasks;
using TableTennis.Model;
using TableTennis.Repository.Common;
using TableTennis.Service.Common;
using System.Security.Cryptography;
using System.Text;

namespace TableTennis.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Registracija novog korisnika
        public async Task<User> RegisterUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Korisnik ne može biti null.");
            }

            // Provjera postoji li korisnik s istim emailom
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new Exception("Korisnik s ovim emailom već postoji.");
            }

            // Provjerite da je lozinka ispravna prije hashiranja
            if (string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentNullException(nameof(user.Password), "Lozinka ne smije biti prazna.");
            }

            // Hashiraj lozinku prije spremanja
            user.Password = HashPassword(user.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.UserId = Guid.NewGuid(); // Generiranje jedinstvenog ID-a za korisnika

            // Kreiranje novog korisnika
            var registeredUser = await _userRepository.RegisterUserAsync(user);

            return registeredUser;
        }

        // Prijava korisnika
        public async Task<User> LoginAsync(string email, string password)
        {
            // Dohvati korisnika prema emailu
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || !VerifyPassword(password, user.Password))
            {
                throw new Exception("Pogrešan email ili lozinka.");
            }

            return user;
        }

        // Hashiranje lozinke
        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Lozinka ne smije biti prazna.");
            }

            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        // Provjera lozinke
        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            var hashOfInput = HashPassword(inputPassword);
            return hashOfInput == hashedPassword;
        }
    }
}
