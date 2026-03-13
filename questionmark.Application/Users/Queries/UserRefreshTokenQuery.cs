using FluentValidation;
using MediatR;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using questionmark.Application.Common.Contracts;
using questionmark.Application.Users.DTO;
using questionmark.Application.Users.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Users.Queries
{
    public class UserRefreshTokenQueryValidator : AbstractValidator<UserRefreshTokenQuery>
    {
        public UserRefreshTokenQueryValidator(IJwtService _jwtService)
        {
            RuleFor(x => x.User).SetValidator(new UserRefreshTokenDtoValidator(_jwtService));
        }
    }
    public class UserRefreshTokenQuery : IRequest<string?>
    {
        public required UserRefreshTokenDto User { get; set; }
        public UserRefreshTokenQuery() { }
        public class UserRefreshTokenQueryHandler : IRequestHandler<UserRefreshTokenQuery, string?>
        {
            private readonly IJwtService _jwtService;
            public UserRefreshTokenQueryHandler(IJwtService jwtService)
            {
                _jwtService = jwtService;
            }
            public async Task<string?> Handle(UserRefreshTokenQuery request, CancellationToken cancellationToken)
            {
                var encryptedUserId = await _jwtService.GetEncryptedUserIdFromRefreshToken(request.User.token);
                if (string.IsNullOrEmpty(encryptedUserId))
                {
                    throw new UnauthorizedAccessException("Refresh token is not valid.");
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, encryptedUserId)
                };
                var token = _jwtService.GenerateAccessToken(claims);
                return JsonConvert.SerializeObject(new { access = token });
            }
        }
    }
}
