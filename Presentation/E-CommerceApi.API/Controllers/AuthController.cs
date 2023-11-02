using E_CommerceApi.Application.Features.Commands.AppUser.GoogleLogin;
using E_CommerceApi.Application.Features.Commands.AppUser.LoginRefreshToken;
using E_CommerceApi.Application.Features.Commands.AppUser.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("action")]

        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
            LoginUserCommandResponse response = await _mediator.Send(loginUserCommandRequest);
            return Ok(response);
        }
        [HttpGet("action")]
        public async Task<IActionResult> RefreshToken([FromForm]LoginRefreshTokenCommandRequest loginRefreshTokenCommandRequest)
        {
            LoginRefreshTokenCommandResponse response = await _mediator.Send(loginRefreshTokenCommandRequest);
            return Ok(response);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> googleLogin(GoogleLoginCommandRequest googleLoginCommandRequest)
        {
            await _mediator.Send(googleLoginCommandRequest);
            return Ok();
        }
    }
}
