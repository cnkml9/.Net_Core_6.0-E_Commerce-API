using E_CommerceApi.Application.Abstractions.Services;
using E_CommerceApi.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Application.Features.Commands.AppUser.LoginRefreshToken
{
    public class LoginRefreshTokenCommandHandler : IRequestHandler<LoginRefreshTokenCommandRequest, LoginRefreshTokenCommandResponse>
    {
        readonly IAuthService _authService;

        public LoginRefreshTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginRefreshTokenCommandResponse> Handle(LoginRefreshTokenCommandRequest request, CancellationToken cancellationToken)
        {
          Token token= await _authService.RefreshTokenLoginAsync(request.refreshToken);
            return new()
            {
                token = token,
            };
        }
    }
}
