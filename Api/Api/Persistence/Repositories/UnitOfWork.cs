using Api.Domain.iRepositories;
using Api.Persistence.Contex;

namespace Api.Persistence.Repositories
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly ApiDbContext _context;
    private IProductRepository? _products;
    private IUserRepository? _users;

    public UnitOfWork(ApiDbContext context)
    {
      _context = context;
    }

    public IProductRepository Products => _products ??= new ProductRepository(_context);

    public IUserRepository Users => _users ??= new UserRepository(_context);

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();

    public ValueTask DisposeAsync()
    {
      _context.Dispose();
      return ValueTask.CompletedTask;
    }
  }
}
