using MediatR;
using questionmark.Application.Posts.Contracts;
using questionmark.Application.Posts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Application.Posts.Queries
{
    public class LoadPostCommentsQuery : IRequest<List<CommentDetailsDto>>
    {
        public required GetCommentsDto Post { get; set; }
        public LoadPostCommentsQuery(){}
        public class LoadPostCommentsQueryHandler : IRequestHandler<LoadPostCommentsQuery, List<CommentDetailsDto>>
        {
            private readonly IPost _postRepo;
            public LoadPostCommentsQueryHandler(IPost postRepo)
            {
                _postRepo = postRepo;
            }
            public async Task<List<CommentDetailsDto>> Handle(LoadPostCommentsQuery request, CancellationToken cancellationToken)
            {
                return await _postRepo.LoadPostComments(request.Post.postId);
            }
        }
    }
}
