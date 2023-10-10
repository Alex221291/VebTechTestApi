using Microsoft.EntityFrameworkCore;
using VebTechTEstApi.Auth;
using VebTechTEstApi.ConstantsData;
using VebTechTEstApi.Models;

namespace VebTechTEstApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var admin = new Role { Id = Guid.NewGuid(), Name = DefaultRoles.Admin.ToString() };
        var superAdmin = new Role { Id = Guid.NewGuid(), Name = DefaultRoles.SuperAdmin.ToString() };
        var support = new Role { Id = Guid.NewGuid(), Name = DefaultRoles.Support.ToString() };
        var user = new Role { Id = Guid.NewGuid(), Name = DefaultRoles.User.ToString() };

        var superUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "SuperAdmin",
            Age = 31,
            Email = "superadmin@gmail.com",
            Password = Password.HaspPassword("12345678")
        };

        modelBuilder.Entity<Role>().HasData(admin, superAdmin, support, user);
        modelBuilder.Entity<User>().HasData(superUser);

        modelBuilder.Entity<Role>()
            .HasMany(r => r.Users)
            .WithMany(u => u.Roles)
            .UsingEntity(j => j.ToTable("RoleUser").HasData(
                new
                {
                    UsersId = superUser.Id,
                    RolesId = superAdmin.Id,
                }));
    }
}