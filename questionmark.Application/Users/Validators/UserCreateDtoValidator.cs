using FluentValidation;
using MediatR;
using questionmark.Application.Common.Contracts;
using questionmark.Application.Users.Contracts;
using questionmark.Application.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Users.Validators
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        private readonly IUser _userRepository;
        private readonly IHashService _hashService;
        public UserCreateDtoValidator(IUser userRepository, IHashService hashService)
        {
            _userRepository = userRepository;
            _hashService = hashService;
            RuleFor(x => x.auth).NotEmpty().WithMessage("Auth is required.").Length(64).WithMessage("Auth is not in correct format.").Must(IsHexadecimal).WithMessage("Auth is not in correct format.");
            RuleFor(x => x).MustAsync((x, cancellation) => isUserRegistered(x)).WithMessage("User is already registered.");
        }
        private async Task<bool> isUserRegistered(UserCreateDto userCreateDto)
        {
            var authHash = await _hashService.HashAuthAsync(userCreateDto.auth);
            var task = await _userRepository.IsUserRegistered(authHash);
            return !task;
        }
        private bool IsHexadecimal(string auth)
        {
            foreach (char c in auth)
            {
                if (!((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
