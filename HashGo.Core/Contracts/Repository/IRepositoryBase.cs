using System.Linq.Expressions;

namespace HashGo.Core.Contracts.Repository
{
    public interface IRepositoryBase<TEntity, TIdentifier>
        where TEntity : class
        where TIdentifier : struct
    {
        IReadOnlyCollection<TEntity> GetAll();

        Task<IReadOnlyCollection<TEntity>> GetAllAsync();

        TEntity? GetId(TIdentifier id);

        Task<TEntity?> GetIdAsync(TIdentifier id);

        TEntity? Get(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);

        IReadOnlyCollection<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);

        Task<IReadOnlyCollection<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity? Add(in TEntity model);

        Task<TEntity?> AddAsync(TEntity model);

        bool AddRange(IEnumerable<TEntity> objModel);

        bool Update(in TEntity model);

        bool Remove(in TEntity objModel);

        bool Remove(in TIdentifier id);

        int Count();

        Task<int> CountAsync();        
    }
}
