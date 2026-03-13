using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Domain.Entities
{
    public class Reaction
    {
        public ulong ID { get; set; }
        public uint post_id { get; set; }
        public DateTime date { get; set; }
        public uint user_id { get; set; }
        public bool reaction { get; set; }
        public User? User { get; set; }
        public Post? Post { get; set; }
    }
}
