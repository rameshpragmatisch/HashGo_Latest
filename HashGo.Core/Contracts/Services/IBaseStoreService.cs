using HashGo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Contracts.Services
{
    public interface IBaseStoreService<TEntity> : IDomainService
        where TEntity : class
    {
        Task<TEntity?> AddOrUpdateSync(TEntity entity);

        Task<TEntity?> AddAsync(TEntity entity);
        bool Update(TEntity entity);

        bool Remove(TEntity entity);

        Task<TEntity?> FindAsync(int identifier);

        Task<IReadOnlyCollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IReadOnlyCollection<TEntity>> ReadAllAsync();
        IReadOnlyCollection<TEntity> ReadAll();
    }
}
