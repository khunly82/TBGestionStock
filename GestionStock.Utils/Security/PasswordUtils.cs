using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Utils.Security
{
    public static class PasswordUtils
    {
        public static string HashPassword(string password, Guid salt)
        {
            byte[] hashBytes = SHA512.HashData(Encoding.UTF8.GetBytes(password + salt));
            string hash = Convert.ToBase64String(hashBytes);
            return salt + hash;
        }

        public static bool CheckPassword(string password, string encodedPassword)
        {
            string salt = encodedPassword[..36];
            return encodedPassword == HashPassword(password, Guid.Parse(salt));
        }
    }
}
