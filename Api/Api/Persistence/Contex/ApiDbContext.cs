using Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Persistence.Contex
{
  public class ApiDbContext : DbContext
  {
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Product>(entity =>
      {
        entity.ToTable("Products");
        entity.HasKey(p => p.Id);
        entity.Property(p => p.Id).ValueGeneratedOnAdd();
        entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
        entity.Property(p => p.Description).HasMaxLength(500);
        entity.Property(p => p.Price).HasPrecision(18, 2);
        entity.Property(p => p.Stock).IsRequired();
        entity.Property(p => p.IsActive).IsRequired();
        entity.Property(p => p.CreatedAt).IsRequired();
      });

      modelBuilder.Entity<User>(entity =>
      {
        entity.ToTable("Users");
        entity.HasKey(u => u.Id);
        entity.Property(u => u.Id).ValueGeneratedOnAdd();
        entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
        entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
        entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(500);
        entity.Property(u => u.CreatedAt).IsRequired();
        entity.HasIndex(u => u.Email).IsUnique();
      });
    }
  }
}