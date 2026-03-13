using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using questionmark.Application.Users.Commands;
using questionmark.Application.Users.Queries;


namespace questionmark.Api.Controllers
{
    public class UserController : BaseController
    {
        public UserController(){}
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<ActionResult> LogIn(UserLoginCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<ActionResult> RegisterUser(UserCreateCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("refresh")]
        public async Task<ActionResult> RefreshAccessToken(UserRefreshTokenQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
        [HttpPost]
        [Route("logout")]
        public async Task<ActionResult> Logout(UserLogoutQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
