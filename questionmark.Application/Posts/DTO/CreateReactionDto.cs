using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Posts.DTO
{
    public class CreateReactionDto
    {
        public uint postId { get; set; }
        public int currentReaction { get; set; }
    }
}
