using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Posts.DTO
{
    public class CommentDetailsDto
    {
        public string text { get; set; } = string.Empty;
        public string timeAgo { get; set; } = string.Empty;
    }
}
