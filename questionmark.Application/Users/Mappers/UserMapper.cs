using questionmark.Application.Users.DTO;
using questionmark.Domain.Entities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Users.Mappers
{
    [Mapper]
    public static partial class UserMapper
    {
        [MapProperty(nameof(UserCreateDto.auth), nameof(User.auth), Use = nameof(MapAuth))]
        private static byte[] MapAuth(string hexString)
        {
            return Convert.FromHexString(hexString);
        }
        [MapperIgnoreTarget(nameof(User.ID))]
        [MapperIgnoreTarget(nameof(User.Reactions))]
        [MapperIgnoreTarget(nameof(User.Posts))]
        [MapperIgnoreTarget(nameof(User.Comments))]
        public static partial User ToEntity(this UserCreateDto dto);
        [MapperIgnoreSource(nameof(User.auth))]
        [MapperIgnoreSource(nameof(User.Reactions))]
        [MapperIgnoreSource(nameof(User.Posts))]
        [MapperIgnoreSource(nameof(User.Comments))]
        public static partial UserCreateDtoResponse ToCreateResponseDto(this User entity);
        [MapperIgnoreSource(nameof(User.auth))]
        [MapperIgnoreSource(nameof(User.Reactions))]
        [MapperIgnoreSource(nameof(User.Posts))]
        [MapperIgnoreSource(nameof(User.Comments))]
        public static partial UserLoginDtoResponse ToUserLoginDtoResponse(this User entity);
    }
}
