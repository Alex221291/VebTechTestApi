using Microsoft.EntityFrameworkCore;
using VebTechTEstApi.Data;
using VebTechTEstApi.Extensions;
using VebTechTEstApi.Models;
using VebTechTEstApi.ViewModels.UserViewModels;

namespace VebTechTEstApi.Services;

public interface IUserService
{
    Task<UserListViewModel?> GetUsersAsync(IndexViewModel model);
    Task<UserFullInfoViewModel?> GetByIdAsync(string userId);
    Task<UserFullInfoViewModel?> CreateUserAsync(UserCreateViewModel model);
    Task<UserFullInfoViewModel?> AddRolesForUserAsync(UserAddRoleViewModel model);
    Task<UserFullInfoViewModel?> UpdateUserAsync(UserUpdateViewModel model);
    Task<int?> DeleteUserAsync(string userId);
    Task<bool> CheckEmailAsync(string email);
}

public class UserService: IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UserListViewModel?> GetUsersAsync(IndexViewModel model)
    {
        try
        {
            var users = _db.Users
                .Include(u => u.Roles.OrderBy(r => r.Name))
                .Select(user => new UserViewModel
                {
                    Id = user.Id.ToString(),
                    Name = user.Name,
                    Age = user.Age,
                    Email = user.Email,
                    Roles = user.Roles.Select(r => r.Name).OrderBy(name => name).ToList()
                });

            if (model.AgeFilter != null)
            {
                users = model.AgeFilter.Min != null ? users.Where(u => u.Age >= model.AgeFilter.Min) : users;
                users = model.AgeFilter.Max != null ? users.Where(u => u.Age <= model.AgeFilter.Max) : users;
            }

            if (model.RoleFilter is { Count: > 0 })
            {
                foreach (var role in model.RoleFilter)
                {
                    users = users.Where(u => u.Roles != null && u.Roles.Contains(role));
                }
            }

            if (model.NameFilter != null)
            {
                users = users.Where(u => u.Name.ToLower().Contains(model.NameFilter.ToLower()));
            }

            if (model.EmailFilter != null)
            {
                users = users.Where(u => u.Email.ToLower().Contains(model.EmailFilter.ToLower()));
            }

            if (model.OrderBy != null)
            {
                users = users.OrderByPropertyName(model.OrderBy.SortField, model.OrderBy.Ascending);
            }

            if (model.Page != null)
            {
                users = users.Skip(model.Page.Size * (model.Page.Number - 1))
                    .Take(model.Page.Size);
            }

            return new UserListViewModel
            {
                TotalItems = users.Count(),
                Users = await users.ToListAsync()
            };
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<UserFullInfoViewModel?> GetByIdAsync(string userId)
    {
        return await _db.Users
            .Include(u => u.Roles)
            .Select(user => new UserFullInfoViewModel
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Age = user.Age,
                Email = user.Email,
                Password = user.Password,
                Roles = user.Roles.Select(r => r.Name).ToList()
            })
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<UserFullInfoViewModel?> CreateUserAsync(UserCreateViewModel model)
    {
        var checkUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (checkUser != null) return null;

        var roles = await _db.Roles.Where(r => model.Roles.Contains(r.Id.ToString())).ToListAsync();

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Age = (int) model.Age,
            Email = model.Email,
            Password = model.Password,
            Roles = roles
        };

        await _db.Users.AddAsync(newUser);
        await _db.SaveChangesAsync();

        return await GetByIdAsync(newUser.Id.ToString());
    }

    public async Task<UserFullInfoViewModel?> AddRolesForUserAsync(UserAddRoleViewModel model)
    {
        var user = await _db.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == Guid.Parse(model.Id));
        if (user == null) return null;

        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == Guid.Parse(model.RoleId));
        if(role == null) return null;

        if (user.Roles.Any(r => r.Id == Guid.Parse(model.RoleId))) return null;
        
        user.Roles.Add(role);
        await _db.SaveChangesAsync();

        return new UserFullInfoViewModel
        {
            Id = user.Id.ToString(),
            Name = user.Name,
            Age = user.Age,
            Email = user.Email,
            Roles = user.Roles.Select(r => r.Name).ToList()
        };
    }

    public async Task<UserFullInfoViewModel?> UpdateUserAsync(UserUpdateViewModel model)
    {
        var updateUser = await _db.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(model.Id));

        if (updateUser == null ) return null;

        updateUser.Name = model.Name ?? updateUser.Name;
        updateUser.Age = model.Age ?? updateUser.Age;
        updateUser.Email = model.Email ?? updateUser.Email;

        await _db.SaveChangesAsync();

        return new UserFullInfoViewModel
        {
            Id = updateUser.Id.ToString(),
            Name = updateUser.Name,
            Age = updateUser.Age,
            Email = updateUser.Email,
            Password = updateUser.Password,
        };
    }

    public async Task<int?> DeleteUserAsync(string userId)
    {
        var deleteUser = await _db.Users
            .FirstOrDefaultAsync(user => !user.IsDeleted && user.Id == Guid.Parse(userId));

        if (deleteUser == null) return 0;

        deleteUser.IsDeleted = true;
        await _db.SaveChangesAsync();

        return 1;
    }

    public async Task<bool> CheckEmailAsync(string email)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null) return false;
        return true;
    }
}