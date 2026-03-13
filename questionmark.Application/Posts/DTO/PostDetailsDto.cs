using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Posts.DTO
{
    public class PostDetailsDto
    {
        public uint post_id { get; set; }
        public string text { get; set; } = string.Empty;
        public int likeCount { get; set; }
        public int dislikeCount { get; set; }
        public int commentsCount { get; set; }
        public int currentReaction { get; set; }
        public string timeAgo { get; set; } = string.Empty;
    }
}
