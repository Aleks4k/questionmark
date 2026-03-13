using MediatR;
using Microsoft.AspNetCore.Http;
using questionmark.Application.Common.Contracts;
using questionmark.Application.Sessions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Users.Queries
{
    public class UserLogoutQuery : IRequest<Unit>
    {
        public UserLogoutQuery(){}
        public class UserLogoutQueryHandler : IRequestHandler<UserLogoutQuery, Unit>
        {
            private readonly IJwtService _jwtService;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IHashService _hashService;
            private readonly Sessions.Contracts.ISession _sessionRepo;
            public UserLogoutQueryHandler(IJwtService jwtService, IHttpContextAccessor httpContextAccessor, IHashService hashService, Sessions.Contracts.ISession sessionRepository)
            {
                _jwtService = jwtService;
                _httpContextAccessor = httpContextAccessor;
                _hashService = hashService;
                _sessionRepo = sessionRepository;
            }
            public async Task<Unit> Handle(UserLogoutQuery request, CancellationToken cancellationToken)
            {
                var authorizationHeader = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"].FirstOrDefault();
                if(authorizationHeader != null)
                {
                    var token = authorizationHeader?.Substring("Bearer ".Length).Trim();
                    if(token != null)
                    {
                        var encryptedUserId = await _jwtService.GetEncryptedUserIdFromAccessToken(token);
                        if (!string.IsNullOrEmpty(encryptedUserId))
                        {
                            //Do ovde bi trebalo uvek metoda da stigne ali nije na odmet da uradimo par provera :D
                            var encryptedUserCipherHash = await _hashService.HashCipherAsync(Convert.FromHexString(encryptedUserId));
                            await _sessionRepo.EndSession(encryptedUserCipherHash);
                        }
                    }
                }
                return Unit.Value;
            }
        }
    }
}
