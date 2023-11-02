using E_CommerceApi.Application.Abstractions.Services;
using MediatR;

namespace E_CommerceApi.Application.Features.Commands.AppUser.GoogleLogin
{

    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        readonly IAuthService _authService;

        public GoogleLoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            //burada 900 saniye cinsinden
           var token =  await _authService.GoogleLoginAsync(request.IdToken, 900);
            return new()
            {
                Token = token
            };
   
        }
    }
}
