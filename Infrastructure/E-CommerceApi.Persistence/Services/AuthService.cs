using E_CommerceApi.Application.Abstractions.Services;
using E_CommerceApi.Application.Abstractions.Token;
using E_CommerceApi.Application.DTOs;
using E_CommerceApi.Application.Exceptions;
using E_CommerceApi.Application.Features.Commands.AppUser.LoginUser;
using E_CommerceApi.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly IConfiguration _configuration;
        readonly SignInManager<AppUser> _signInManager;
        readonly IUserService _userService;

        public AuthService(UserManager<AppUser> userManager, ITokenHandler tokenHandler, IConfiguration configuration, SignInManager<AppUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _signInManager = signInManager;
            _userService = userService;
        }

        async Task<Token> CreateUserExternalAsync(AppUser user,string email,string name, UserLoginInfo info,int accesTokenLifeTime)
        {
            //user null ise result false olur değilse true olur...
            bool result = user != null;

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        NameSurnname = name
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }
            if (result)
            {
                await _userManager.AddLoginAsync(user, info);
                Token token = _tokenHandler.CreateAccessToken(accesTokenLifeTime,user);
                await _userService.UpdateRefreshToken(token.refreshToken, user, token.Expiration, 5);
                return token;
            }
            else
                throw new Exception("Invalid External Authentication.");

          
        }

        public async Task<Token> GoogleLoginAsync(string idToken, int accesTokenLifeTime)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["ExternalLoginSettings:Google:ClientId"] }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            //dış kaynaktan gelen kullanıcı bilgilerini sorguluyor kayıtlı değilse aspNetUserLogins e kaydeder bilgilerini,örneğin
            //google dan ilk defa girş yapan kullanıcının  bilgilerini bu tabloya kaydeder.
            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");

            AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            return await CreateUserExternalAsync(user, payload.Email, payload.Name, info, accesTokenLifeTime);
           
        }

        public async Task<Token> LoginAsync(string UserNameOrEmail, string Password,int accesTokenLifeTime)
        {
            AppUser user = await _userManager.FindByNameAsync(UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(UserNameOrEmail);
            }
            if (user == null)
            {
                throw new NotFoundUserException("Kullanıcı veya şifre hatalı...");
            }
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, Password, false);

            if (result.Succeeded)
            {
                Token token = _tokenHandler.CreateAccessToken(accesTokenLifeTime, user);
                await _userService.UpdateRefreshToken(token.refreshToken, user, token.Expiration, 5);

                return token;
            }
            else
            {
                throw new AuthenticationErrorException();
            }
            //return new LoginUserErrorCommandResponse()
            //{
            //    Message="Kullanıcı adı veya şifre hatalı..."
            //};
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
          AppUser? user = await  _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user != null && user?.RefreshTokenEndDate>DateTime.UtcNow)
            {
               Token token= _tokenHandler.CreateAccessToken(15,user);
               await _userService.UpdateRefreshToken(token.refreshToken,user,token.Expiration, 300);
                return token;
            }
            else
                throw new NotFoundUserException();


        }
    }
}
