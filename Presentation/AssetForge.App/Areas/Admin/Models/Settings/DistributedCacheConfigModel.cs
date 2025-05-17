using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents distributed cache configuration model
/// </summary>
public partial record DistributedCacheConfigModel : BaseModel, IConfigModel
{
    #region Properties

    [ResourceDisplayName("Admin.Configuration.AppSettings.DistributedCache.DistributedCacheType")]
    public SelectList DistributedCacheTypeValues { get; set; }
    public int DistributedCacheType { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.DistributedCache.Enabled")]
    public bool Enabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.DistributedCache.ConnectionString")]
    public string ConnectionString { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.DistributedCache.SchemaName")]
    public string SchemaName { get; set; } = "dbo";

    [ResourceDisplayName("Admin.Configuration.AppSettings.DistributedCache.TableName")]
    public string TableName { get; set; } = "DistributedCache";

    [ResourceDisplayName("Admin.Configuration.AppSettings.DistributedCache.InstanceName")]
    public string InstanceName { get; protected set; } = string.Empty;

    [ResourceDisplayName("Admin.Configuration.AppSettings.DistributedCache.PublishIntervalMs")]
    public int PublishIntervalMs { get; protected set; }

    #endregion
}