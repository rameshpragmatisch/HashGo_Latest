using HashGo.Core.Contracts.Repository;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Db;
using HashGo.Domain.DataContext;
using HashGo.Infrastructure.DataContext;
using System.Linq.Expressions;

namespace HashGo.Domain.Services.Base;

public class BaseStoreService<TEntity> : IBaseStoreService<TEntity>
    where TEntity : BaseEntity
{
    private readonly ILoggingService _logger;
    private readonly IRepositoryBase<TEntity, int> _Repository;

    public BaseStoreService(ILoggingService loggingService, IRepositoryBase<TEntity, int> repository)
    {
        _logger = loggingService;
        _Repository = repository;
    }

    public async Task<TEntity?> AddOrUpdateSync(TEntity entity)
    {
        if (entity != null)
        {
            try
            {
                if (entity.Id == 0)
                {
                    var addedItem = await _Repository.AddAsync(entity);
                    return addedItem;
                }
                else
                {
                    var returnRepo = _Repository.Update(entity);
                    return entity;
                }
            }
            catch (Exception ex)
            {
                _logger.TraceException(ex);
            }
        }

        return null;
    }

    public async Task<TEntity?> AddAsync(TEntity entity)
    {
        if (entity != null)
            try
            {
                var addedItem = await _Repository.AddAsync(entity);
                return addedItem;
            }
            catch (Exception ex)
            {
                _logger.TraceException(ex);
            }
        return null;
    }

    public bool Remove(TEntity entity)
    {
        if (entity != null)
        {
            try
            {
                return _Repository.Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.TraceException(ex);
            }
        }

        return false;
    }

    public async Task<IReadOnlyCollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        IReadOnlyCollection<TEntity> connectItems = null;

        try
        {
            connectItems = await this._Repository.GetListAsync(predicate);

        }
        catch (Exception ex)
        {
            _logger.TraceException(ex);

            if (connectItems == null)
            {
                connectItems = Array.Empty<TEntity>();
            }
        }

        return connectItems;
    }

    public async Task<TEntity?> FindAsync(int identifier)
    {
        try
        {
            var connectItem = await this._Repository.GetIdAsync(identifier);

            return connectItem;
        }
        catch (Exception ex)
        {
            _logger.TraceException(ex);
        }

        return null;
    }

    public async Task<IReadOnlyCollection<TEntity>> ReadAllAsync()
    {
        IReadOnlyCollection<TEntity> connectItems = null;

        try
        {
            connectItems = await this._Repository.GetAllAsync();

            if(connectItems != null && connectItems.Any())
            {
                foreach (var item in connectItems)
                    if (item is TenantConnect)
                        ApplicationStateContext.TenantConnectItems.Add(item as TenantConnect);
            }
        }
        catch (Exception ex)
        {
            _logger.TraceException(ex);

            if (connectItems == null)
            {
                connectItems = Array.Empty<TEntity>();
            }
        }

        return connectItems;
    }

    public  IReadOnlyCollection<TEntity> ReadAll()
    {
        IReadOnlyCollection<TEntity> connectItems = null;

        try
        {
            connectItems = this._Repository.GetAll();

            if (connectItems != null && connectItems.Any())
            {
                foreach (var item in connectItems)
                    if (item is TenantConnect)
                        ApplicationStateContext.TenantConnectItems.Add(item as TenantConnect);
            }
        }
        catch (Exception ex)
        {
            _logger.TraceException(ex);

            if (connectItems == null)
            {
                connectItems = Array.Empty<TEntity>();
            }
        }

        return connectItems;
    }

    public bool Update(TEntity entity)
    {
        if (entity != null)
        {
            try
            {
                return _Repository.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.TraceException(ex);
            }
        }

        return false;
    }
}