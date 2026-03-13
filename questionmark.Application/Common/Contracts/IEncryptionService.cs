using questionmark.Application.Posts.DTO;
using questionmark.Application.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Common.Contracts
{
    public interface IEncryptionService
    {
        UserEncryptedIdDto EncryptUserID(uint ID);
        uint DecryptUserID(byte[] cipher, byte[] nonce, byte[] tag);
    }
}
