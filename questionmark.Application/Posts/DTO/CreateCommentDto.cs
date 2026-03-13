using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Posts.DTO
{
    public class CreateCommentDto
    {
        public uint postId { get; set; }
        public string text { get; set; } = string.Empty;
    }
}
