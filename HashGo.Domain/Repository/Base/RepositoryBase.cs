using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using HashGo.Core.Contracts.Repository;

namespace HashGo.Domain.Repository.Base
{
    public class RepositoryBase<TDBContext, TEntity, TIdentifier> : IRepositoryBase<TEntity, TIdentifier>, IDisposable
        where TDBContext : DbContext, new()
        where TEntity : class
        where TIdentifier : struct
    {

        private readonly TDBContext _context;
        private DbSet<TEntity> EntityDbSet { get { return this._context.Set<TEntity>(); } }

        public RepositoryBase()
        {
            _context = new();
        }

        public virtual TEntity? Add(in TEntity model)
        {
            var entity = EntityDbSet.Add(model);
            
            var itemSaved = _context.SaveChanges();
            if (itemSaved > 0)
            {
                return entity.Entity;
            }

            return null;
        }

        public async virtual  Task<TEntity?> AddAsync(TEntity model)
        {
            var entity = await EntityDbSet.AddAsync(model);

            var itemSaved = await _context.SaveChangesAsync();
            if (itemSaved > 0)
            {
                return entity.Entity;
            }

            return null;
        }

        public virtual bool AddRange(IEnumerable<TEntity> model)
        {
            EntityDbSet.AddRange(model);
            
            var itemSaved = _context.SaveChanges();
            return itemSaved > 0;
        }

        public virtual TEntity? GetId(TIdentifier id)
        {
            return EntityDbSet.Find(id);
        }

        public async virtual Task<TEntity?> GetIdAsync(TIdentifier id)
        {
            return await EntityDbSet.FindAsync(id);
        }

        public virtual TEntity? Get(Expression<Func<TEntity, bool>> predicate)
        {
            return EntityDbSet.FirstOrDefault(predicate);
        }

        public async virtual Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await EntityDbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual IReadOnlyCollection<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return EntityDbSet.Where(predicate).ToList();
        }

        public async virtual Task<IReadOnlyCollection<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(() => EntityDbSet.Where(predicate).ToArray());
        }

        public virtual IReadOnlyCollection<TEntity> GetAll()
        {
            return EntityDbSet.ToList();
        }

        public async virtual Task<IReadOnlyCollection<TEntity>> GetAllAsync()
        {
            return await Task.Run(() => EntityDbSet.ToList());
        }

        public virtual int Count()
        {
            return EntityDbSet.Count();
        }

        public async virtual Task<int> CountAsync()
        {
            return await EntityDbSet.CountAsync();
        }

        public virtual bool Update(in TEntity objModel)
        {
            _context.Entry(objModel).State = EntityState.Modified;
            var savedChanges = _context.SaveChanges();

            return savedChanges > 0;
        }

        public virtual bool Remove(in TEntity objModel)
        {
            var entity = EntityDbSet.Remove(objModel);
            var savedChanges = _context.SaveChanges();

            return savedChanges > 0;
        }

        public virtual bool Remove(in TIdentifier id)
        {
            var item = GetId(id);
            if (item != null)
            {
                return Remove(item);
            }

            return false;
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
