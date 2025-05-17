using AssetForge.Core.Caching;
using AssetForge.Core.Domain.Configuration;

namespace AssetForge.Services.Configuration
{
    /// <summary>
    /// Represents default values related to settings
    /// </summary>
    public static partial class SettingsDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey SettingsAllAsDictionaryCacheKey => new("AssetForge.setting.all.dictionary.", EntityCacheDefaults<Setting>.Prefix);

        #endregion
    }
}