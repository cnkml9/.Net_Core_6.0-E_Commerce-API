using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_CommerceApi.Application.Exceptions;
using E_CommerceApi.Application.Abstractions.Services;
using E_CommerceApi.Application.DTOs.User;

namespace E_CommerceApi.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
       readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
          CreateUserResponse response = await  _userService.createAsync(new()
            {
                Email = request.Email,
                NameSurname=request.NameSurname,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm,
                UserName = request.UserName
            });

            return new()
            {
                Message = response.Message,
                Succeded = response.Succeded
            };

            //throw new UserCreateFailedException();

        }
    }
}
