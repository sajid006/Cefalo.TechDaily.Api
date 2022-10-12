using Cefalo.TechDaily.Service.Utils.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.Utils.Services
{
    public class PasswordHandler : IPasswordHandler
    {
        public Tuple<byte[], byte[]> CreatePasswordHash(string password)
        {
            byte[] passwordSalt, passwordHash;
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            return new Tuple<byte[], byte[]>(passwordSalt, passwordHash);
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
