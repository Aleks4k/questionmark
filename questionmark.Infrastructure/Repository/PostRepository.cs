using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using questionmark.Application.Common.Contracts;
using questionmark.Application.Posts.Contracts;
using questionmark.Application.Posts.DTO;
using questionmark.Application.Posts.Mappers;
using questionmark.Domain.Data;
using questionmark.Domain.Entities;

namespace questionmark.Infrastructure.Repository
{
    public class PostRepository : IPost
    {
        private readonly AppDbContext _context;
        public PostRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PostDetailsDto> CreatePost(CreateDomainPostDto post, CancellationToken cancellationToken)
        {
            var entity = post.ToEntity();
            await _context.Posts.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.ToDetailsDto(entity.user_id);
        }
        public async Task<bool> DoesPostExists(uint post_id)
        {
            var response = await _context.Posts.AsNoTracking().Where(x => x.ID == post_id).FirstOrDefaultAsync();
            return response != null;
        }
        public async Task<List<PostDetailsDto>> LoadPosts(uint user_id)
        {
            var posts = await _context.Posts
                .AsNoTracking()
                .OrderByDescending(x => x.date)
                .Include(x => x.Reactions)
                .Include(x => x.Comments)
                .ToListAsync();
            return posts.Select(x => x.ToDetailsDto(user_id)).ToList();
        }
        public async Task<List<PostDetailsDto>> LoadUserPosts(uint user_id)
        {
            var posts = await _context.Posts
                .AsNoTracking()
                .OrderByDescending(x => x.date)
                .Include(x => x.Reactions)
                .Include(x => x.Comments)
                .Where(x => x.user_id == user_id)
                .ToListAsync();
            return posts.Select(x => x.ToDetailsDto(user_id)).ToList();
        }
        public async Task<CreateReactionResponseDto> CreateReaction(CreateDomainReactionDto reaction)
        {
            var result = await _context.Reactions.Where(x => x.user_id == reaction.user_id && x.post_id == reaction.postId).FirstOrDefaultAsync();
            if(result != null)
            {
                if (reaction.currentReaction == 2) //Brišemo
                {
                    _context.Reactions.Remove(result);
                }
                else
                {
                    result.reaction = reaction.currentReaction == 1 ? true : false;
                }
            } 
            else
            {
                if(reaction.currentReaction != 2)
                {
                    Reaction r = new Reaction
                    {
                        date = reaction.date,
                        post_id = reaction.postId,
                        user_id = reaction.user_id,
                        reaction = reaction.currentReaction == 1 ? true : false
                    };
                    await _context.Reactions.AddAsync(r);
                }
            }
            await _context.SaveChangesAsync();
            var post = await _context.Posts
                .AsNoTracking()
                .Include(x => x.Reactions)
                .Where(x => x.ID == reaction.postId)
                .FirstOrDefaultAsync();
            return new CreateReactionResponseDto { 
                dislikeCount = post!.Reactions.Where(x => !x.reaction).Count(),
                likeCount = post!.Reactions.Where(x => x.reaction).Count(),
            };
        }
        public async Task<List<CommentDetailsDto>> LoadPostComments(uint post_id)
        {
            var comments = await _context.Comments.AsNoTracking().OrderByDescending(x => x.date).Where(x => x.post_id == post_id).ToListAsync();
            return comments.Select(x => x.ToDetailsDto()).ToList();
        }
        public async Task<CommentDetailsDto> CreateComment(CreateDomainCommentDto comment)
        {
            var entity = comment.ToEntity();
            await _context.Comments.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.ToDetailsDto();
        }
    }
}
