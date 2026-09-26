using System.Linq.Expressions;
using Api.Domain.iRepositories;
using Api.Persistence.Contex;
using Microsoft.EntityFrameworkCore;

namespace Api.Persistence.Repositories
{
  public class Repository<T> : IRepository<T> where T : class
  {
    protected readonly ApiDbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(ApiDbContext context)
    {
      Context = context;
      DbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id) => await DbSet.FindAsync(id);

    public virtual async Task<IEnumerable<T>> GetAllAsync() => await DbSet.AsNoTracking().ToListAsync();

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
      => await DbSet.AsNoTracking().Where(predicate).ToListAsync();

    public virtual async Task AddAsync(T entity) => await DbSet.AddAsync(entity);

    public virtual void Update(T entity) => DbSet.Update(entity);

    public virtual void Remove(T entity) => DbSet.Remove(entity);

    public virtual async Task<bool> ExistsAsync(int id)
    {
      var parameter = Expression.Parameter(typeof(T), "e");
      var property = Expression.Property(parameter, "Id");
      var constant = Expression.Constant(id);
      var body = Expression.Equal(property, constant);
      var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);

      return await DbSet.AsNoTracking().AnyAsync(predicate);
    }
  }
}
