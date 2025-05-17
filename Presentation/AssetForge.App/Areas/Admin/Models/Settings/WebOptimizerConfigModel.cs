using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents WebOptimizer config model
/// </summary>
public partial record WebOptimizerConfigModel : BaseModel, IConfigModel
{
    #region Properties

    [ResourceDisplayName("Admin.Configuration.AppSettings.WebOptimizer.EnableJavaScriptBundling")]
    public bool EnableJavaScriptBundling { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.WebOptimizer.EnableCssBundling")]
    public bool EnableCssBundling { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.WebOptimizer.EnableDiskCache")]
    public bool EnableDiskCache { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.WebOptimizer.CacheDirectory")]
    public string CacheDirectory { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.WebOptimizer.JavaScriptBundleSuffix")]
    public string JavaScriptBundleSuffix { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.WebOptimizer.CssBundleSuffix")]
    public string CssBundleSuffix { get; set; }

    #endregion
}