using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Users.DTO
{
    public class UserEncryptedIdDto
    {
        public byte[] cipher { get; set; } = new byte[0];
        public byte[] nonce { get; set; } = new byte[0];
        public byte[] tag { get; set; } = new byte[0];
    }
}
