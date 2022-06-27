using LSE.TradeHub.Core.Models;

namespace LSE.TradeHub.Core.Interfaces;

public interface IServiceBase<TEntity, TId> where TEntity : EntityBase<TId> {
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetById(TId id);
    Task<TEntity> Create(TEntity entity);
}

public interface IDataSeeder<TEntity> {
    Task CreateRange(TEntity[] entities, bool forceSeed);
}
