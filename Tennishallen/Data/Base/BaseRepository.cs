using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tennishallen.Data.Models;

namespace Tennishallen.Data.Base;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// A Repository that Queries the db context.
/// </summary>
/// <param name="context">The DbContext to retrieve TEntity from.</param>
/// <typeparam name="TEntity">The entity that the repository will retrieve.</typeparam>
/// <typeparam name="TIdentifier">The type of the id of TEntity.</typeparam>
public class BaseRepository<TEntity, TIdentifier>(ApplicationDbContext context)
    where TEntity : class, IBaseEntity<TIdentifier>  where TIdentifier : IEquatable<TIdentifier>
{

    /// <summary>
    /// Adds a value to the repository.
    /// </summary>
    /// <param name="entity">The value to add.</param>
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        var entry = await context.Set<TEntity>().AddAsync(entity);
        await context.SaveChangesAsync();
        return entry.Entity;
    }
    
    /// <summary>
    /// Delete value from the repository the value where the id matches.
    /// </summary>
    /// <param name="id">The id to match.</param>
    public async Task DeleteAsync(TIdentifier id)
    {
        var entity = await context.Set<TEntity>().FirstOrDefaultAsync(n => n.Id.Equals(id));
        if (entity != null)
        {
            EntityEntry entityEntry = context.Entry(entity);
            entityEntry.State = EntityState.Deleted;
        }

        await context.SaveChangesAsync();
    }
    
    
    /// <summary>
    /// Get all values from the repository and include the given properties.
    /// </summary>
    /// <param name="includeProperties">The properties to include</param>
    /// <returns>All values found in the repository</returns>
    public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        query = includeProperties.Aggregate(
            query, 
            (current, includeProperty) => current.Include(includeProperty)
            );
        return await query.ToListAsync();
    
    }
    

    /// <summary>
    /// Delete value from the repository the value where the id matches.
    /// </summary>
    /// <param name="id">The id to match.</param>
    /// <param name="includeProperties">The properties to include.</param>>
    /// <returns>The value that matches the id.</returns>>
    public async Task<TEntity?> GetByIdAsync(TIdentifier id, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity?> query = context.Set<TEntity>();
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return await query.FirstOrDefaultAsync(n => n.Id.Equals(id));
    }
    
    /// <summary>
    /// Update the value in the repository.
    /// </summary>
    /// <param name="entity">The entity with the new values.</param>
    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        EntityEntry<TEntity> entityEntry =  context.Entry<TEntity>(entity);
        entityEntry.State = EntityState.Modified;
        await context.SaveChangesAsync();
        return entityEntry.Entity;
    }
}