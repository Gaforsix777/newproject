using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Services
{
    public static class PasswordHelper
    {
        // Formato almacenado: saltBase64$hashBase64
        public static string Hash(string password, int iterations = 10000)
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[16];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);
            return $"{Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        public static bool Verify(string stored, string password, int iterations = 10000)
        {
            if (string.IsNullOrWhiteSpace(stored) || !stored.Contains('$')) return false;
            var parts = stored.Split('$');
            var salt = Convert.FromBase64String(parts[0]);
            var expected = Convert.FromBase64String(parts[1]);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var computed = pbkdf2.GetBytes(32);

            // comparación constante
            var diff = 0;
            for (int i = 0; i < expected.Length; i++)
                diff |= expected[i] ^ computed[i];
            return diff == 0;
        }
    }
}
