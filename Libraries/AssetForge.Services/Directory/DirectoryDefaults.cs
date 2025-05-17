using AssetForge.Core.Caching;
using AssetForge.Core.Domain.Directory;

namespace AssetForge.Services.Directory;

/// <summary>
/// Represents default values related to directory services
/// </summary>
public static partial class DirectoryDefaults
{
    #region Caching defaults

    #region Countries

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : Two letter ISO code
    /// </remarks>
    public static CacheKey CountriesByTwoLetterCodeCacheKey => new("AssetForge.country.bytwoletter.{0}", EntityCacheDefaults<Country>.Prefix);

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : Two letter ISO code
    /// </remarks>
    public static CacheKey CountriesByThreeLetterCodeCacheKey => new("AssetForge.country.bythreeletter.{0}", EntityCacheDefaults<Country>.Prefix);

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : language ID
    /// {1} : show hidden records?
    /// {2} : current site ID
    /// </remarks>
    public static CacheKey CountriesAllCacheKey => new("AssetForge.country.all.{0}-{1}-{2}", EntityCacheDefaults<Country>.Prefix);

    #endregion

    #region States and provinces

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : country ID
    /// {1} : language ID
    /// {2} : show hidden records?
    /// </remarks>
    public static CacheKey StateProvincesByCountryCacheKey => new("AssetForge.stateprovince.bycountry.{0}-{1}-{2}", EntityCacheDefaults<StateProvince>.Prefix);

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : show hidden records?
    /// </remarks>
    public static CacheKey StateProvincesAllCacheKey => new("AssetForge.stateprovince.all.{0}", EntityCacheDefaults<StateProvince>.Prefix);

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : abbreviation
    /// {1} : country ID
    /// </remarks>
    public static CacheKey StateProvincesByAbbreviationCacheKey => new("AssetForge.stateprovince.byabbreviation.{0}-{1}", EntityCacheDefaults<StateProvince>.Prefix);

    #endregion

    #endregion
}