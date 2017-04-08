using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Farfetch.Domain.Entities.Base;

namespace Farfetch.Application.Interfaces.Base
{
    public interface IRepository<TEntity, in TId> : IDisposable
        where TEntity : Entity<TEntity, TId>
    {
        Task<TEntity> SaveAsync(TEntity entity);

        Task<IEnumerable<TEntity>> SaveAsync(IEnumerable<TEntity> entities);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(IEnumerable<TEntity> entities);

        Task<TEntity> GetAsync(TId id);

        Task<IEnumerable<TEntity>> GetAllAsync(string ordering = null, bool ascending = true);

        Task<PagedList<TEntity>> GetAllAsync(int index, int limit, string ordering = null, bool ascending = true);
    }
}