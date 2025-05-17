using AssetForge.Core;
using AssetForge.Core.Domain.Sites;

namespace AssetForge.Services.Sites
{
    /// <summary>
    /// Site mapping service interface
    /// </summary>
    public partial interface ISiteMappingService
    {
        /// <summary>
        /// Apply site mapping to the passed query
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports site mapping</typeparam>
        /// <param name="query">Query to filter</param>
        /// <param name="siteId">Site identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the filtered query
        /// </returns>
        Task<IQueryable<TEntity>> ApplySiteMapping<TEntity>(IQueryable<TEntity> query, int siteId) where TEntity : BaseEntity, ISiteMappingSupported;

        /// <summary>
        /// Deletes a site mapping record
        /// </summary>
        /// <param name="siteMapping">Site mapping record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteSiteMappingAsync(SiteMapping siteMapping);

        /// <summary>
        /// Gets site mapping records
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports site mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the site mapping records
        /// </returns>
        Task<IList<SiteMapping>> GetSiteMappingsAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, ISiteMappingSupported;

        /// <summary>
        /// Inserts a site mapping record
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports site mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="siteId">Site id</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertSiteMappingAsync<TEntity>(TEntity entity, int siteId) where TEntity : BaseEntity, ISiteMappingSupported;

        /// <summary>
        /// Find site identifiers with granted access (mapped to the entity)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports site mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the site identifiers
        /// </returns>
        Task<int[]> GetSitesIdsWithAccessAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, ISiteMappingSupported;

        /// <summary>
        /// Find site identifiers with granted access (mapped to the entity)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports site mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// The site identifiers
        /// </returns>
        int[] GetSitesIdsWithAccess<TEntity>(TEntity entity) where TEntity : BaseEntity, ISiteMappingSupported;

        /// <summary>
        /// Authorize whether entity could be accessed in the current site (mapped to this site)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports site mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue - authorized; otherwise, false
        /// </returns>
        Task<bool> AuthorizeAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, ISiteMappingSupported;

        /// <summary>
        /// Authorize whether entity could be accessed in a site (mapped to this site)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports site mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="siteId">Site identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue - authorized; otherwise, false
        /// </returns>
        Task<bool> AuthorizeAsync<TEntity>(TEntity entity, int siteId) where TEntity : BaseEntity, ISiteMappingSupported;

        /// <summary>
        /// Authorize whether entity could be accessed in a site (mapped to this site)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports site mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="siteId">Site identifier</param>
        /// <returns>
        /// The rue - authorized; otherwise, false
        /// </returns>
        bool Authorize<TEntity>(TEntity entity, int siteId) where TEntity : BaseEntity, ISiteMappingSupported;
    }
}