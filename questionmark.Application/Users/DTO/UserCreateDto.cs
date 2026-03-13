using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Users.DTO
{
    public class UserCreateDto
    {
        public required string auth { get; set; } = string.Empty;
    }
}
