using questionmark.Application.Common.Contracts;
using questionmark.Application.Posts.DTO;
using questionmark.Application.Users.DTO;
using questionmark.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Infrastructure.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly EncryptionSettings _encryptionSettings;
        public EncryptionService(EncryptionSettings encryptionSettings)
        {
            _encryptionSettings = encryptionSettings;
        }
        public UserEncryptedIdDto EncryptUserID(uint ID)
        {
            var key = Convert.FromHexString(_encryptionSettings.AESKey_user);
            byte[] tag = new byte[16];
            var textBytes = System.Text.Encoding.UTF8.GetBytes(ID.ToString());
            byte[] cipherText = new byte[textBytes.Length];
            var iv = GenerateIV();
            using (var aesGcm = new AesGcm(key, 16))
            {
                aesGcm.Encrypt(iv, textBytes, cipherText, tag);
                var user = new UserEncryptedIdDto
                {
                    cipher = cipherText,
                    nonce = iv,
                    tag = tag
                };
                return user;
            }
        }
        public uint DecryptUserID(byte[] cipher, byte[] nonce, byte[] tag)
        {
            var key = Convert.FromHexString(_encryptionSettings.AESKey_user);
            byte[] plainText = new byte[cipher.Length];
            using (var aesGcm = new AesGcm(key, 16))
            {
                aesGcm.Decrypt(nonce, cipher, tag, plainText);
            }
            var plainTextString = System.Text.Encoding.UTF8.GetString(plainText);
            return uint.Parse(plainTextString);
        }
        private static byte[] GenerateIV()
        {
            byte[] iv = new byte[12];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(iv);
            }
            return iv;
        }
    }
}
