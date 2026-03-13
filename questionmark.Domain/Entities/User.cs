using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Domain.Entities
{
    public class User
    {
        public uint ID { get; set; }
        public byte[] auth { get; set; } = new byte[0];
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
