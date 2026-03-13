using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Posts.DTO
{
    public class CreateDomainReactionDto
    {
        public uint user_id { get; set; }
        public uint postId { get; set; }
        public int currentReaction { get; set; }
        public DateTime date { get; set; }
    }
}
