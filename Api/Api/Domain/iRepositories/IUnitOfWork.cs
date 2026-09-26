namespace Api.Domain.iRepositories
{
  public interface IUnitOfWork : IAsyncDisposable
  {
    IProductRepository Products { get; }
    IUserRepository Users { get; }
    Task<int> CompleteAsync();
  }
}