using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Common.Contracts
{
    public interface IHashService
    {
        Task<string> HashAuthAsync(string auth);
        Task<byte[]> HashCipherAsync(byte[] cipher);
    }
}
