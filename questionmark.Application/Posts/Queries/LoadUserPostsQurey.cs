using MediatR;
using Microsoft.AspNetCore.Http;
using questionmark.Application.Common.Contracts;
using questionmark.Application.Posts.Contracts;
using questionmark.Application.Posts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Posts.Queries
{
    public class LoadUserPostsQuery : IRequest<List<PostDetailsDto>>
    {
        public LoadUserPostsQuery() { }
        public class LoadUserPostsQueryHandler : IRequestHandler<LoadUserPostsQuery, List<PostDetailsDto>>
        {
            private readonly IJwtService _jwtService;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly Sessions.Contracts.ISession _sessionRepo;
            private readonly IHashService _hashService;
            private readonly IEncryptionService _encryptionService;
            private readonly IPost _postRepo;
            public LoadUserPostsQueryHandler(IJwtService jwtService, IHttpContextAccessor httpContextAccessor, Sessions.Contracts.ISession sessionRepo, IHashService hashService, IEncryptionService encryptionService, IPost postRepo)
            {
                _httpContextAccessor = httpContextAccessor;
                _jwtService = jwtService;
                _sessionRepo = sessionRepo;
                _hashService = hashService;
                _encryptionService = encryptionService;
                _postRepo = postRepo;
            }
            public async Task<List<PostDetailsDto>> Handle(LoadUserPostsQuery request, CancellationToken cancellationToken)
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
                return await _postRepo.LoadUserPosts(user_id);
            }
        }
    }
}
