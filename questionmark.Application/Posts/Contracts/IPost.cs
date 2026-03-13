using questionmark.Application.Posts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Posts.Contracts
{
    public interface IPost
    {
        Task<PostDetailsDto> CreatePost(CreateDomainPostDto post, CancellationToken cancellationToken);
        Task<List<PostDetailsDto>> LoadPosts(uint user_id);
        Task<List<PostDetailsDto>> LoadUserPosts(uint user_id);
        Task<bool> DoesPostExists(uint post_id);
        Task<CreateReactionResponseDto> CreateReaction(CreateDomainReactionDto reaction);
        Task<List<CommentDetailsDto>> LoadPostComments(uint post_id);
        Task<CommentDetailsDto> CreateComment(CreateDomainCommentDto comment);
    }
}
