using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Infrastructure.AnswerObjects;
using Infrastructure.Dto;
using Infrastructure.Interfaces;
using Infrastructure.Option;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    public ResultModel<string, Exception> GenerateAccessToken(UserModel user)
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

    public ResultModel<string, Exception> GenerateSalt()
    {
        const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        try
        {
            var randomBytes = RandomNumberGenerator.GetBytes(32);
            var chars = randomBytes.Select(b => validChars[b % validChars.Length]);
            var result = new string(chars.ToArray());

            return string.IsNullOrEmpty(result) ? ResultModel<string, Exception>.CreateFailedResult(new InvalidOperationException("Failed to generate salt.")) : ResultModel<string, Exception>.CreateSuccessfulResult(result);
        }
        catch (Exception ex)
        {
            return ResultModel<string, Exception>.CreateFailedResult(ex);
        }
    }

    public ResultModel<string, Exception> LoginUser(LoginModel user)
    {
        throw new NotImplementedException();
    }

    public ResultModel<string, Exception> RegisterUser(RegisterModel user)
    {
        throw new NotImplementedException();
    }
}