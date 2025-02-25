using channel_alert_front.ApiService.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OpenTelemetry.Resources;
using System.Linq.Expressions;

namespace channel_alert_front.ApiService.Repository;

public interface IRepositoryBase<T>
{
    public IQueryable<T> FindAll(bool trackChanges);
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate, bool trackChanges = true);
    public void Create(T entity);
    public Task<int> UpdateAsync(Expression<Func<T, bool>> predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls);
    public Task<int> DeleteAsync(Expression<Func<T, bool>> predicate);
    public bool Save();
    public Task<bool> SaveAsync();
}

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected RepositoryContext RepositoryContext;

    public RepositoryBase(RepositoryContext repositoryContext)
    {
        RepositoryContext = repositoryContext;
    }

    public IQueryable<T> FindAll(bool trackChanges = true)
    {
        if (trackChanges)
            return RepositoryContext.Set<T>();

        return RepositoryContext.Set<T>().AsNoTracking();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate, bool trackChanges = true)
    {
        IQueryable<T> found = RepositoryContext.Set<T>().Where(predicate);
        if (trackChanges)
            return found;

        return found.AsNoTracking();
    }

    public void Create(T entity)
    {
        RepositoryContext.Set<T>().Add(entity);
    }

    public async Task<int> UpdateAsync(Expression<Func<T, bool>> predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
    {
        int updated = await RepositoryContext.Set<T>()
            .Where<T>(predicate)
            .ExecuteUpdateAsync<T>(setPropertyCalls);
        return updated;
    }

    public async Task<int> DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        int deleted = await RepositoryContext
            .Set<T>()
            .Where<T>(predicate)
            .ExecuteDeleteAsync<T>();

        return deleted;
    }

    public bool Save()
    {
        int success = RepositoryContext.SaveChanges();

        return success > 0;
    }

    public async Task<bool> SaveAsync()
    {
        int success = await RepositoryContext.SaveChangesAsync();
        return success > 0;
    }
}
