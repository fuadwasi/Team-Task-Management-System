using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a cache configuration model
/// </summary>
public partial record CacheConfigModel : BaseModel, IConfigModel
{
    #region Properties

    [ResourceDisplayName("Admin.Configuration.AppSettings.Cache.DefaultCacheTime")]
    public int DefaultCacheTime { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.Cache.LinqDisableQueryCache")]
    public bool LinqDisableQueryCache { get; set; }

    #endregion
}