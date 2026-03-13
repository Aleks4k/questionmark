using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Infrastructure.Settings
{
    public class HashSettings
    {
        public string authHashKey { get; set; } = string.Empty;
        public string cipherHashKey { get; set; } = string.Empty;
    }
}
