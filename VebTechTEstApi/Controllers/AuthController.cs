using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VebTechTEstApi.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using VebTechTEstApi.Auth;
using VebTechTEstApi.ViewModels.AuthViewModels;

namespace VebTechTEstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Авторизация в системе.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Auth
        ///     {
        ///         "login": "login@mail.ru", - email
        ///         "password": "11111111" - пароль
        ///     }
        ///
        /// </remarks>
        /// <returns>Данные пользователя и token авторизации</returns>
        /// <response code="200">Вернёт данные авторизированного пользователя
        /// 
        ///     GET /Auth
        ///     {
        ///         "id": "44b2d7af-bf9f-4a51-9eae-3dfe153ef315",
        ///         "name": "SuperAdmin",
        ///         "age": 31,
        ///         "email": "user@gmail.com",
        ///         "roles": [
        ///             "SuperAdmin"
        ///             ],
        ///         "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjQ0YjJkN2FmLWJmOWYtNGE1MS05ZWFlLTNkZmUxNTNlZjMxNSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlN1cGVyQWRtaW4iLCJuYmYiOjE2OTY5NDI4MTUsImV4cCI6MTY5Njk3ODgxNSwiaXNzIjoiTXlBdXRoU2VydmVyIiwiYXVkIjoiTXlBdXRoQ2xpZW50In0.T5zWZI-VLqKdQuduYATFglNL9I1JbIWuGBibA_azN6M"
        ///     }
        /// </response>
        /// <response code="400">Неверные данные авторизации</response>
        [HttpGet("Auth")]
        public async Task<ObjectResult> Auth([FromQuery] LoginViewModel model)
        {
            var user = await _authService.AuthAsync(model);

            if (user == null) return BadRequest("Неверный email или пароль!");

            var identity = GetIdentity(user.Id, user.Roles);
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromHours(10)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            user.Token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(user);
        }

        private static ClaimsIdentity GetIdentity(string id, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.UniqueName, id),
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
