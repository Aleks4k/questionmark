using Microsoft.EntityFrameworkCore;
using questionmark.Application.Sessions.Contracts;
using questionmark.Application.Sessions.DTO;
using questionmark.Application.Sessions.Mappers;
using questionmark.Application.Users.DTO;
using questionmark.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Infrastructure.Repository
{
    public class SessionRepository : ISession
    {
        private readonly AppDbContext _context;
        public SessionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task EndSession(byte[] hash)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(x => x.hash == hash);
            if(session != null)
            {
                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<SessionDetailsDto?> GetSessionDetails(byte[] hash)
        {
            var session = await _context.Sessions.AsNoTracking().FirstOrDefaultAsync(x => x.hash == hash);
            return session?.ToDetailsDto();
        }
        public async Task StartSession(SessionCreateDto session, CancellationToken cancellationToken)
        {
            var entity = session.ToEntity();
            await _context.Sessions.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
