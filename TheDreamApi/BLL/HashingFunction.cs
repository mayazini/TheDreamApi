using System.Security.Cryptography;
using System.Text;


namespace TheDreamApi.BLL
{
    public class HashingFunction
    {
        public static string GenerateSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        public static string HashPasswordWithSalt(string password, string salt)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    
    }
}
