using E_CommerceApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceApi.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
       DTOs.Token  CreateAccessToken(int minute,AppUser appUser);
        string CreateRefreshToken();
    }
}
