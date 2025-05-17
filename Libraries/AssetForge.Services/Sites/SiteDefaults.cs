using AssetForge.Core.Caching;

namespace AssetForge.Services.Sites
{
    /// <summary>
    /// Represents default values related to sites services
    /// </summary>
    public static partial class SiteDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static CacheKey SiteMappingIdsCacheKey => new("AssetForge.sitemapping.ids.{0}-{1}");

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static CacheKey SiteMappingsCacheKey => new("AssetForge.sitemapping.{0}-{1}");

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity name
        /// </remarks>
        public static CacheKey SiteMappingExistsCacheKey => new("AssetForge.sitemapping.exists.{0}");

        #endregion
    }
}