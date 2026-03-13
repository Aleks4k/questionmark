using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Domain.Entities
{
    public class Comment
    {
        public ulong ID { get; set; }
        public uint user_id { get; set; }
        public User? User { get; set; }
        public uint post_id { get; set; }
        public Post? Post { get; set; }
        public DateTime date { get; set; }
        public string text { get; set; } = string.Empty;
    }
}
