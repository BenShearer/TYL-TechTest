using LSE.TradeHub.Core.Interfaces;
using LSE.TradeHub.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LSE.TradeHub.Core.Services;

public class ServiceBase<TEntity, TId> : IServiceBase<TEntity, TId>, IDataSeeder<TEntity> where TEntity : EntityBase<TId> {
    protected TradeDataContext DataContext;
    protected DbSet<TEntity> DataSet;

    public ServiceBase(TradeDataContext dataContext) {
        DataContext = dataContext;
        DataSet = dataContext.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync() {
        return await DataSet.ToListAsync();
    }

    public async Task<TEntity?> GetById(TId id) {
        return await DataSet.Where(s => s.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public virtual async Task<TEntity> Create(TEntity entity) {
        var added = await DataSet.AddAsync(entity);

        await DataContext.SaveChangesAsync();

        return added.Entity;
    }

    public async Task CreateRange(TEntity[] entities, bool forceSeed) {
        if (forceSeed) {
            DataSet.RemoveRange(DataSet);
        } else if (DataSet.Any()) {
            return;
        }

        await DataSet.AddRangeAsync(entities);
        await DataContext.SaveChangesAsync();
    }
}
