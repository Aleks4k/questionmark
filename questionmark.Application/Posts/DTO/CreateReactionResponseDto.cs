using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Posts.DTO
{
    public class CreateReactionResponseDto
    {
        public int likeCount { get; set; }
        public int dislikeCount { get; set; }
    }
}
