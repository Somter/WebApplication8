using System.Security.Cryptography;
using System.Text;

namespace WebApplication8.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            RandomNumberGenerator.Create().GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(salt + password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
