using FluentValidation;
using MediatR;
using questionmark.Application.Common.Contracts;
using questionmark.Application.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Users.Validators
{
    public class UserRefreshTokenDtoValidator : AbstractValidator<UserRefreshTokenDto>
    {
        private readonly IJwtService _jwtService;
        public UserRefreshTokenDtoValidator(IJwtService jwtService)
        {
            _jwtService = jwtService;
            RuleFor(x => x.token).NotEmpty().WithMessage("Refresh token is empty.").MustAsync((x, cancellation) => isRefreshTokenValid(x)).WithMessage("Token is not valid.");
        }
        private async Task<bool> isRefreshTokenValid(string token)
        {
            return await _jwtService.ValidateRefreshToken(token);
        }
    }
}
