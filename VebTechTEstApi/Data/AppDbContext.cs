using Microsoft.EntityFrameworkCore;
using VebTechTEstApi.Models;

namespace VebTechTEstApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Role>()
    //        .HasMany(c => c.Users)
    //        .WithMany(s => s.Roles)
    //        .UsingEntity(j => j.ToTable("UsersRoles"));
    //}
}