using Api.Domain.iRepositories;
using Api.Domain.Models;
using Api.Persistence.Contex;
using Microsoft.EntityFrameworkCore;

namespace Api.Persistence.Repositories
{
  public class ProductRepository : Repository<Product>, IProductRepository
  {
    public ProductRepository(ApiDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
      => await DbSet.AsNoTracking().Where(p => p.IsActive).ToListAsync();
  }
}
