using Konscious.Security.Cryptography;
using questionmark.Application.Common.Contracts;
using questionmark.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Infrastructure.Services
{
    public class HashService : IHashService
    {
        private readonly HashSettings _hashSettings;
        public HashService(HashSettings hashSettings)
        {
            _hashSettings = hashSettings;
        }
        public async Task<string> HashAuthAsync(string auth)
        {
            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(auth)))
            {
                argon2.Salt = Encoding.UTF8.GetBytes(_hashSettings.authHashKey);
                argon2.DegreeOfParallelism = 4;
                argon2.MemorySize = 65536;
                argon2.Iterations = 4;
                var hash = await argon2.GetBytesAsync(64);
                return Convert.ToHexString(hash);
            }
        }
        public async Task<byte[]> HashCipherAsync(byte[] cipher)
        {
            using (var argon2 = new Argon2id(cipher))
            {
                argon2.Salt = Encoding.UTF8.GetBytes(_hashSettings.cipherHashKey);
                argon2.DegreeOfParallelism = 4;
                argon2.MemorySize = 65536;
                argon2.Iterations = 4;
                var hash = await argon2.GetBytesAsync(32);
                return hash;
            }
        }
    }
}
