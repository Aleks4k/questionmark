using questionmark.Application.Sessions.DTO;
using questionmark.Domain.Entities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Sessions.Mappers
{
    [Mapper]
    public static partial class SessionMapper
    {
        [MapperIgnoreTarget(nameof(Session.ID))]
        public static partial Session ToEntity(this SessionCreateDto dto);
        [MapperIgnoreSource(nameof(Session.ID))]
        [MapperIgnoreSource(nameof(Session.end))]
        public static partial SessionDetailsDto ToDetailsDto(this Session entity);
    }
}
