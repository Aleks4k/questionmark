using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using questionmark.Application.Common.Contracts;
using questionmark.Application.Posts.Contracts;
using questionmark.Application.Posts.DTO;
using questionmark.Application.Posts.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Posts.Commands
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator(IPost postRepository)
        {
            RuleFor(x => x.Comment).SetValidator(new CreateCommentDtoValidator(postRepository));
        }
    }
    public class CreateCommentCommand : IRequest<CommentDetailsDto>
    {
        public required CreateCommentDto Comment { get; set; }
        public CreateCommentCommand(){}
        public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentDetailsDto>
        {
            private readonly IJwtService _jwtService;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly Sessions.Contracts.ISession _sessionRepo;
            private readonly IHashService _hashService;
            private readonly IEncryptionService _encryptionService;
            private readonly IPost _postRepo;
            public CreateCommentCommandHandler(IJwtService jwtService, IHttpContextAccessor httpContextAccessor, Sessions.Contracts.ISession sessionRepo, IHashService hashService, IEncryptionService encryptionService, IPost postRepo)
            {
                _httpContextAccessor = httpContextAccessor;
                _jwtService = jwtService;
                _sessionRepo = sessionRepo;
                _hashService = hashService;
                _encryptionService = encryptionService;
                _postRepo = postRepo;
            }
            public async Task<CommentDetailsDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
            {
                var authorizationHeader = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"].FirstOrDefault();
                var token = authorizationHeader!.Substring("Bearer ".Length).Trim();
                var encryptedUserId = await _jwtService.GetEncryptedUserIdFromAccessToken(token!);
                if (string.IsNullOrEmpty(encryptedUserId))
                    throw new UnauthorizedAccessException("Access token invalid.");
                var encryptedUserCipherHash = await _hashService.HashCipherAsync(Convert.FromHexString(encryptedUserId));
                var sessionDetails = await _sessionRepo.GetSessionDetails(encryptedUserCipherHash);
                if (sessionDetails == null)
                    throw new UnauthorizedAccessException("Invalid session data.");
                var user_id = _encryptionService.DecryptUserID(Convert.FromHexString(encryptedUserId), sessionDetails.nonce, sessionDetails.tag);
                var comment = new CreateDomainCommentDto
                {
                    user_id = user_id,
                    date = DateTime.UtcNow,
                    post_id = request.Comment.postId,
                    text = request.Comment.text
                };
                return await _postRepo.CreateComment(comment);
            }
        }
    }
}
