using FluentValidation;
using MediatR;
using questionmark.Application.Common.Contracts;
using questionmark.Application.Sessions.Contracts;
using questionmark.Application.Sessions.DTO;
using questionmark.Application.Users.Contracts;
using questionmark.Application.Users.DTO;
using questionmark.Application.Users.Validators;
using System.Security.Claims;

namespace questionmark.Application.Users.Commands
{
    public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
    {
        public UserCreateCommandValidator(IUser userRepository, IHashService hashService)
        {
            RuleFor(x => x.User).SetValidator(new UserCreateDtoValidator(userRepository, hashService));
        }
    }
    public class UserCreateCommand : IRequest<UserDetailsDto>
    {
        public required UserCreateDto User { get; set; }
        public UserCreateCommand() { }
        public class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, UserDetailsDto>
        {
            private readonly IJwtService _jwtService;
            private readonly IHashService _hashService;
            private readonly IEncryptionService _encryptionService;
            private readonly IUser _userRepo;
            private readonly ISession _sessionRepo;
            public UserCreateCommandHandler(IJwtService jwtService, IHashService hashService, IEncryptionService encryptionService, IUser userRepository, ISession sessionRepository)
            {
                _jwtService = jwtService;
                _hashService = hashService;
                _encryptionService = encryptionService;
                _userRepo = userRepository;
                _sessionRepo = sessionRepository;
            }
            public async Task<UserDetailsDto> Handle(UserCreateCommand request, CancellationToken cancellationToken)
            {
                request.User.auth = await _hashService.HashAuthAsync(request.User.auth);
                var user = await _userRepo.RegisterUser(request.User, cancellationToken);
                var userEncrypted = _encryptionService.EncryptUserID(user.ID);
                var encryptedUserCipherHash = await _hashService.HashCipherAsync(userEncrypted.cipher);
                var session = new SessionCreateDto
                {
                    hash = encryptedUserCipherHash,
                    nonce = userEncrypted.nonce,
                    tag = userEncrypted.tag,
                    end = DateTime.UtcNow.AddDays(_jwtService.getRefreshTokenTTL())
                };
                await _sessionRepo.StartSession(session, cancellationToken);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToHexString(userEncrypted.cipher))
                };
                var user_return = new UserDetailsDto();
                user_return.access = _jwtService.GenerateAccessToken(claims);
                user_return.refresh = _jwtService.GenerateRefreshToken(claims);
                return user_return;
            }
        }
    }
}
