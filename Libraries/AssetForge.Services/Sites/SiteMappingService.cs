using AssetForge.Core;
using AssetForge.Core.Caching;
using AssetForge.Core.Domain.Sites;
using AssetForge.Data;

namespace AssetForge.Services.Sites
{
    /// <summary>
    /// Site mapping service
    /// </summary>
    public partial class SiteMappingService : ISiteMappingService
    {
        #region Fields

        private readonly IRepository<SiteMapping> _siteMappingRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ISiteContext _siteContext;

        #endregion

        #region Ctor

        public SiteMappingService(
            IRepository<SiteMapping> siteMappingRepository,
            IStaticCacheManager staticCacheManager,
            ISiteContext siteContext)
        {
            _siteMappingRepository = siteMappingRepository;
            _staticCacheManager = staticCacheManager;
            _siteContext = siteContext;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Inserts a site mapping record
        /// </summary>
        /// <param name="siteMapping">Site mapping</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task InsertSiteMappingAsync(SiteMapping siteMapping)
        {
            await _siteMappingRepository.InsertAsync(siteMapping);
        }

        /// <summary>
        /// Get a value indicating whether a site mapping exists for an entity type
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports site mapping</typeparam>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue if exists; otherwise false
        /// </returns>
        protected virtual async Task<bool> IsEntityMappingExistsAsync<TEntity>() where TEntity : BaseEntity, ISiteMappingSupported
        {
            var entityName = typeof(TEntity).Name;
            var key = _staticCacheManager.PrepareKeyForDefaultCache(SiteDefaults.SiteMappingExistsCacheKey, entityName);

            var query = from sm in _siteMappingRepository.Table
                        where sm.EntityName == entityName
                        select sm.SiteId;

            return await _staticCacheManager.GetAsync(key, query.Any);
        }

        #endregion

        #region Methods

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
        public virtual async Task<IQueryable<TEntity>> ApplySiteMapping<TEntity>(IQueryable<TEntity> query, int siteId)
            where TEntity : BaseEntity, ISiteMappingSupported
        {
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            return from entity in query
                   where !entity.LimitedToSites || _siteMappingRepository.Table.Any(sm =>
                         sm.EntityName == typeof(TEntity).Name && sm.EntityId == entity.Id && sm.SiteId == siteId)
                   select entity;
        }

        /// <summary>
        /// Deletes a site mapping record
        /// </summary>
        /// <param name="siteMapping">Site mapping record</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteSiteMappingAsync(SiteMapping siteMapping)
        {
            await _siteMappingRepository.DeleteAsync(siteMapping);
        }

        /// <summary>
        /// Gets site mapping records
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports site mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the site mapping records
        /// </returns>
        public virtual async Task<IList<SiteMapping>> GetSiteMappingsAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, ISiteMappingSupported
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityId = entity.Id;
            var entityName = entity.GetType().Name;

            var key = _staticCacheManager.PrepareKeyForDefaultCache(SiteDefaults.SiteMappingsCacheKey, entityId, entityName);

            var query = from sm in _siteMappingRepository.Table
                        where sm.EntityId == entityId &&
                        sm.EntityName == entityName
                        select sm;

            var siteMappings = await _staticCacheManager.GetAsync(key, async () => await query.ToListAsync());

            return siteMappings;
        }

        /// <summary>
        /// Inserts a site mapping record
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports site mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="siteId">Site id</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertSiteMappingAsync<TEntity>(TEntity entity, int siteId) where TEntity : BaseEntity, ISiteMappingSupported
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (siteId == 0)
                throw new ArgumentOutOfRangeException(nameof(siteId));

            var entityId = entity.Id;
            var entityName = entity.GetType().Name;

            var siteMapping = new SiteMapping
            {
                EntityId = entityId,
                EntityName = entityName,
                SiteId = siteId
            };

            await InsertSiteMappingAsync(siteMapping);
        }

        /// <summary>
        /// Find site identifiers with granted access (mapped to the entity)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports site mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the site identifiers
        /// </returns>
        public virtual async Task<int[]> GetSitesIdsWithAccessAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, ISiteMappingSupported
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityId = entity.Id;
            var entityName = entity.GetType().Name;

            var key = _staticCacheManager.PrepareKeyForDefaultCache(SiteDefaults.SiteMappingIdsCacheKey, entityId, entityName);

            var query = from sm in _siteMappingRepository.Table
                        where sm.EntityId == entityId &&
                              sm.EntityName == entityName
                        select sm.SiteId;

            return await _staticCacheManager.GetAsync(key, () => query.ToArray());
        }

        /// <summary>
        /// Find site identifiers with granted access (mapped to the entity)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports site mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// The site identifiers
        /// </returns>
        public virtual int[] GetSitesIdsWithAccess<TEntity>(TEntity entity) where TEntity : BaseEntity, ISiteMappingSupported
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityId = entity.Id;
            var entityName = entity.GetType().Name;

            var key = _staticCacheManager.PrepareKeyForDefaultCache(SiteDefaults.SiteMappingIdsCacheKey, entityId, entityName);

            var query = from sm in _siteMappingRepository.Table
                        where sm.EntityId == entityId &&
                              sm.EntityName == entityName
                        select sm.SiteId;

            return _staticCacheManager.Get(key, () => query.ToArray());
        }

        /// <summary>
        /// Authorize whether entity could be accessed in the current site (mapped to this site)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports site mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue - authorized; otherwise, false
        /// </returns>
        public virtual async Task<bool> AuthorizeAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, ISiteMappingSupported
        {
            var site = await _siteContext.GetCurrentSiteAsync();

            return await AuthorizeAsync(entity, site.Id);
        }

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
        public virtual async Task<bool> AuthorizeAsync<TEntity>(TEntity entity, int siteId) where TEntity : BaseEntity, ISiteMappingSupported
        {
            if (entity == null)
                return false;

            if (siteId == 0)
                //return true if no site specified/found
                return true;

            if (!entity.LimitedToSites)
                return true;

            foreach (var siteIdWithAccess in await GetSitesIdsWithAccessAsync(entity))
                if (siteId == siteIdWithAccess)
                    //yes, we have such permission
                    return true;

            //no permission found
            return false;
        }

        /// <summary>
        /// Authorize whether entity could be accessed in a site (mapped to this site)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that supports site mapping</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="siteId">Site identifier</param>
        /// <returns>
        /// The rue - authorized; otherwise, false
        /// </returns>
        public virtual bool Authorize<TEntity>(TEntity entity, int siteId) where TEntity : BaseEntity, ISiteMappingSupported
        {
            if (entity == null)
                return false;

            if (siteId == 0)
                //return true if no site specified/found
                return true;


            if (!entity.LimitedToSites)
                return true;

            foreach (var siteIdWithAccess in GetSitesIdsWithAccess(entity))
                if (siteId == siteIdWithAccess)
                    //yes, we have such permission
                    return true;

            //no permission found
            return false;
        }

        #endregion
    }
}