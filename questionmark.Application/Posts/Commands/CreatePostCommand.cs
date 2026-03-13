using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using questionmark.Application.Common.Contracts;
using questionmark.Application.Posts.Contracts;
using questionmark.Application.Posts.DTO;
using questionmark.Application.Posts.Validators;

namespace questionmark.Application.Posts.Commands
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.Post).SetValidator(new CreatePostDtoValidator());
        }
    }
    public class CreatePostCommand : IRequest<PostDetailsDto>
    {
        public required CreatePostDto Post { get; set; }
        public CreatePostCommand(){}
        public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDetailsDto>
        {
            private readonly IJwtService _jwtService;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly Sessions.Contracts.ISession _sessionRepo;
            private readonly IHashService _hashService;
            private readonly IEncryptionService _encryptionService;
            private readonly IPost _postRepo;
            public CreatePostCommandHandler(IJwtService jwtService, IHttpContextAccessor httpContextAccessor, Sessions.Contracts.ISession sessionRepo, IHashService hashService, IEncryptionService encryptionService, IPost postRepo)
            {
                _httpContextAccessor = httpContextAccessor;
                _jwtService = jwtService;
                _sessionRepo = sessionRepo;
                _hashService = hashService;
                _encryptionService = encryptionService;
                _postRepo = postRepo;
            }
            public async Task<PostDetailsDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
            {
                var authorizationHeader = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"].FirstOrDefault();
                var token = authorizationHeader!.Substring("Bearer ".Length).Trim();
                var encryptedUserId = await _jwtService.GetEncryptedUserIdFromAccessToken(token!);
                if(string.IsNullOrEmpty(encryptedUserId))
                    throw new UnauthorizedAccessException("Access token invalid.");
                var encryptedUserCipherHash = await _hashService.HashCipherAsync(Convert.FromHexString(encryptedUserId));
                var sessionDetails = await _sessionRepo.GetSessionDetails(encryptedUserCipherHash);
                if(sessionDetails == null)
                    throw new UnauthorizedAccessException("Invalid session data.");
                var user_id = _encryptionService.DecryptUserID(Convert.FromHexString(encryptedUserId), sessionDetails.nonce, sessionDetails.tag);
                var postForSave = new CreateDomainPostDto
                {
                    date = DateTime.UtcNow,
                    user_id = user_id,
                    text = request.Post.text,
                };
                return await _postRepo.CreatePost(postForSave, cancellationToken);
            }
        }
    }
}
