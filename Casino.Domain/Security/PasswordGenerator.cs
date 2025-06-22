using System.Security.Cryptography;
using System.Text;

namespace Casino.Domain.Security
{
    public static class PasswordGenerator
    {
        private const string SECRET_KEY = "MyUltraSecureSuperSecretKey2025$#@!";
        public static string GenerateHash(string input)
        {
            using (SHA512 sha = SHA512.Create())
            {
                byte[] bytes = Encoding.ASCII.GetBytes(input + SECRET_KEY);
                byte[] hashBytes = sha.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
