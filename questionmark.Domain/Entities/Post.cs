using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Domain.Entities
{
    public class Post
    {
        public uint ID { get; set; }
        public string text { get; set; } = string.Empty;
        public DateTime date { get; set; }
        public uint user_id { get; set; }
        public User? User { get; set; }
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
