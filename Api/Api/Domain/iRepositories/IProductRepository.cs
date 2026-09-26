using Api.Domain.Models;

namespace Api.Domain.iRepositories
{
  public interface IProductRepository : IRepository<Product>
  {
    Task<IEnumerable<Product>> GetActiveProductsAsync();
  }
}
