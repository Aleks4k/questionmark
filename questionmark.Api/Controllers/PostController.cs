using Microsoft.AspNetCore.Mvc;
using questionmark.Application.Posts.Commands;
using questionmark.Application.Posts.Queries;

namespace questionmark.Api.Controllers
{
    public class PostController : BaseController
    {
        public PostController(){}
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Create(CreatePostCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpPost]
        [Route("load")]
        public async Task<ActionResult> Load(LoadPostsQurey query)
        {
            return Ok(await Mediator.Send(query));
        }
        [HttpPost]
        [Route("loadUserPosts")]
        public async Task<ActionResult> Load(LoadUserPostsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
        [HttpPost]
        [Route("react")]
        public async Task<ActionResult> React(CreateReactionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpPost]
        [Route("loadPostComments")]
        public async Task<ActionResult> LoadPostComments(LoadPostCommentsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
        [HttpPost]
        [Route("comment")]
        public async Task<ActionResult> Comment(CreateCommentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
