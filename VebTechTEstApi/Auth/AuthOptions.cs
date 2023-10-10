using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace VebTechTEstApi.Auth;

public static class AuthOptions
{
    public const string ISSUER = "MyAuthServer";
    public const string AUDIENCE = "MyAuthClient";
    const string KEY = "hr_gpt_secretkey";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(KEY));
}