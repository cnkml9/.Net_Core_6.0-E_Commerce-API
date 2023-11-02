using E_CommerceApi.Application.Abstractions.Services;
using E_CommerceApi.Application.DTOs.User;
using E_CommerceApi.Application.Exceptions;
using E_CommerceApi.Application.Features.Commands.AppUser.CreateUser;
using E_CommerceApi.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Persistence.Services
{
    public class UserServices : IUserService
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public UserServices(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> createAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                NameSurnname = model.NameSurname
            }, model.Password);

            CreateUserResponse response = new()
            {
                Succeded=result.Succeeded
            };

            if (result.Succeeded)
                response.Message = "kullanıcı başarıyla oluşuruldu";
            else
            {
                foreach (var error in result.Errors)
                {
                    response.Message = $"{error.Code}-{error.Description}\n";
                }
            }

            return response;

        }

        public async Task UpdateRefreshToken(string refreshToken, AppUser user,DateTime AccessTokenDate,int addOnAccessTokenDate)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate=AccessTokenDate.AddSeconds(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
            }
            else
                throw new NotFoundUserException();


        }
    }
}
