using Microsoft.EntityFrameworkCore;
using VebTechTEstApi.Auth;
using VebTechTEstApi.Data;
using VebTechTEstApi.ViewModels.AuthViewModels;

namespace VebTechTEstApi.Services;

public interface IAuthService
{
    Task<AuthReqViewModel?> AuthAsync(LoginViewModel model);
}

class AuthService: IAuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AuthReqViewModel?> AuthAsync(LoginViewModel model)
    {
        var user = await _db.Users.
            Include(u => u.Roles )
            .FirstOrDefaultAsync(user =>
            !user.IsDeleted && user.Email == model.Email && user.Password == Password.HaspPassword(model.Password));

        if (user == null) return null;

        return new AuthReqViewModel
        {
            Id = user.Id.ToString(),
            Name = user.Name,
            Age = user.Age,
            Email = user.Email,
            Roles = user.Roles.Select(r => r.Name).ToList()
        };
    }
}