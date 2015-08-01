using System.Linq;

namespace Zerobased.DataAccess
{
    /// <summary>
    /// Iterface for simple data interaction.
    /// </summary>
    /// <typeparam name="TEntity">Type of manipulated entities.</typeparam>
    /// <typeparam name="TKey">Type of kyes of manipulated entities.</typeparam>
    public interface IRepository<TEntity, in TKey> where TEntity : IIdentity<TKey>
    {
        /// <summary>
        /// Create list on entities by filter object with sorting and paging.
        /// </summary>
        /// <param name="filter">Object represents filter for entities.</param>
        /// <param name="options">Sorting and paging parameters.</param>
        /// <returns>Enumerable of entites.</returns>
        IQueryable<TEntity> Get(object filter, ListOptions options);

        /// <summary>
        /// Create count of entities by filter object.
        /// </summary>
        /// <param name="filter">Object represents filter for entities.</param>
        /// <returns>Count of entities.</returns>
        int Count(object filter);

        /// <summary>
        /// Create single entity by <paramref name="id"/> or <value>NULL</value> if entity is missing.
        /// </summary>
        /// <param name="id">Id to find.</param>
        /// <returns>Entity.</returns>
        TEntity Get(TKey id);

        /// <summary>
        /// Add new entity to storage.
        /// </summary>
        /// <param name="item">Entity to add.</param>
        /// <returns>Added entity.</returns>
        TEntity Add(TEntity item);

        /// <summary>
        /// Updates entity in storage.
        /// </summary>
        /// <param name="item">Entity to update.</param>
        /// <returns>Updated entity.</returns>
        TEntity Update(TEntity item);

        /// <summary>
        /// Delete batch of entities by filter. Returns <value>true</value> if any enitity was deleted. 
        /// Returns <value>false</value> if filter have retured empty collection.
        /// </summary>
        /// <param name="filter">Filter of deleting entities.</param>
        /// <returns></returns>
        bool Delete(object filter);

        /// <summary>
        /// Delete entity by id. Returns <value>true</value> if entity was deleted. Returns <value>false</value> if entity was missing.
        /// </summary>
        /// <param name="id">Id of deleting entity.</param>
        /// <returns></returns>
        bool Delete(TKey id);

        /// <summary>
        /// Delete entity. Returns <value>true</value> if entity was deleted. Returns <value>false</value> if entity was missing.
        /// </summary>
        /// <param name="item">Item to delete.</param>
        /// <returns></returns>
        bool Delete(TEntity item);
    }
}
