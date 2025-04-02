

using DAL;
using Infrastructure.Dto;
using Infrastructure.Option;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class UserControleService : IUserControleService
    {
        public ResultModel<string, Exception> GenerateAccessToken(UserDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),

            };
            if (user.Email != null)
            {
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
            }

            var jwt = new JwtSecurityToken(
                issuer: JwtOption.ISSUER,
                audience: JwtOption.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
                signingCredentials: new SigningCredentials(JwtOption.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return ResultModel<string, Exception>.CreateSuccessfulResult(new JwtSecurityTokenHandler().WriteToken(jwt));
        }

        public ResultModel<string, Exception> LoginUser(UserDto user)
        {
            throw new NotImplementedException();
        }

        public ResultModel<string, Exception> RegisterUser(UserDto user)
        {
            throw new NotImplementedException();
        }
    }
}
