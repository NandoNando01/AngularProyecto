using Api.Domain.iRepositories;
using Api.Domain.Models;
using Api.Persistence.Contex;
using Microsoft.EntityFrameworkCore;

namespace Api.Persistence.Repositories
{
  public class UserRepository : Repository<User>, IUserRepository
  {
    public UserRepository(ApiDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
      => await DbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

    public async Task<bool> ExistsByEmailAsync(string email)
      => await DbSet.AsNoTracking().AnyAsync(u => u.Email == email);
  }
}