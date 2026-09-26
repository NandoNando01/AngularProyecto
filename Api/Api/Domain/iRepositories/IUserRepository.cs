using Api.Domain.Models;

namespace Api.Domain.iRepositories
{
  public interface IUserRepository : IRepository<User>
  {
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
  }
}