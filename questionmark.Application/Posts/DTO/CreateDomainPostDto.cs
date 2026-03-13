using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Posts.DTO
{
    public class CreateDomainPostDto
    {
        public string text { get; set; } = string.Empty;
        public DateTime date { get; set; }
        public uint user_id { get; set; }
    }
}
