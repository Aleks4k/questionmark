using FluentValidation;
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
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        private readonly IUser _userRepository;
        private readonly IHashService _hashService;
        public UserLoginDtoValidator(IUser userRepository, IHashService hashService)
        {
            _userRepository = userRepository;
            _hashService = hashService;
            RuleFor(x => x.auth).NotEmpty().WithMessage("Auth is required.").Length(64).WithMessage("Auth is not in correct format.").Must(IsHexadecimal).WithMessage("Auth is not in correct format.");
            RuleFor(x => x).MustAsync((x, cancellation) => isLoginCorrect(x)).WithMessage("User is not registered.");
        }
        private async Task<bool> isLoginCorrect(UserLoginDto userLoginDto) //Ova metoda faktički proverava jel login tačan.
        {
            var authHash = await _hashService.HashAuthAsync(userLoginDto.auth);
            var task = await _userRepository.IsLoginCorrect(authHash);
            return task;
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
