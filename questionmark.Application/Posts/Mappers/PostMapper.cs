using questionmark.Application.Common.Contracts;
using questionmark.Application.Posts.DTO;
using questionmark.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace questionmark.Application.Posts.Mappers
{
    [Mapper]
    public static partial class PostMapper
    {
        [MapperIgnoreTarget(nameof(Post.ID))]
        [MapperIgnoreTarget(nameof(Post.Reactions))]
        [MapperIgnoreTarget(nameof(Post.User))]
        [MapperIgnoreTarget(nameof(Post.Comments))]
        public static partial Post ToEntity(this CreateDomainPostDto dto);
        public static PostDetailsDto ToDetailsDto(this Post entity, uint user_id)
        {
            return new PostDetailsDto
            {
                post_id = entity.ID,
                likeCount = entity.Reactions.Where(x => x.reaction).Count(),
                dislikeCount = entity.Reactions.Where(x => !x.reaction).Count(),
                commentsCount = entity.Comments.Count(),
                currentReaction = GetCurrentReaction(entity.Reactions, user_id),
                timeAgo = GetTimeAgo(entity.date),
                text = entity.text
            };
        }
        public static CommentDetailsDto ToDetailsDto(this Comment entity)
        {
            return new CommentDetailsDto
            {
                text = entity.text,
                timeAgo = GetTimeAgo(entity.date)
            };
        }
        [MapperIgnoreTarget(nameof(Comment.ID))]
        [MapperIgnoreTarget(nameof(Comment.User))]
        [MapperIgnoreTarget(nameof(Comment.Post))]
        public static partial Comment ToEntity(this CreateDomainCommentDto dto);
        private static string GetTimeAgo(DateTime date)
        {
            var timeSpan = DateTime.UtcNow - date;
            if (timeSpan.Days > 365)
                return $"{Math.Floor(timeSpan.Days / 365.0)} years ago";
            if (timeSpan.Days > 30)
                return $"{Math.Floor(timeSpan.Days / 30.0)} months ago";
            if (timeSpan.Days > 0)
                return $"{timeSpan.Days} days ago";
            if (timeSpan.Hours > 0)
                return $"{timeSpan.Hours} hours ago";
            if (timeSpan.Minutes > 0)
                return $"{timeSpan.Minutes} minutes ago";
            return "Just now";
        }
        private static int GetCurrentReaction(ICollection<Reaction> reactions, uint user_id)
        {
            var reaction = reactions.FirstOrDefault(r => r.user_id == user_id);
            if (reaction == null)
                return 2; //Nema reakcije
            return reaction.reaction ? 1 : 0; // 1 like, 0 dislike
        }
    }
}
