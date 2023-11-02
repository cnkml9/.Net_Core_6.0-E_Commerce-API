using E_CommerceApi.Application.Features.Commands.AppUser.CreateUser;
using E_CommerceApi.Application.Features.Commands.AppUser.GoogleLogin;
using E_CommerceApi.Application.Features.Commands.AppUser.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse response = await _mediator.Send(createUserCommandRequest);
            return Ok(response);
        }
        [HttpPost("action")]

        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
           LoginUserCommandResponse response =  await _mediator.Send(loginUserCommandRequest);
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
