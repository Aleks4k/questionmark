using questionmark.Application.Sessions.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Sessions.Contracts
{
    public interface ISession
    {
        Task<SessionDetailsDto?> GetSessionDetails(byte[] hash);
        Task StartSession(SessionCreateDto session, CancellationToken cancellationToken);
        Task EndSession(byte[] hash);
    }
}
