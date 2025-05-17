using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a plugin configuration model
/// </summary>
public partial record PluginConfigModel : BaseModel, IConfigModel
{
    #region Properties

    [ResourceDisplayName("Admin.Configuration.AppSettings.Plugin.UseUnsafeLoadAssembly")]
    public bool UseUnsafeLoadAssembly { get; set; }

    #endregion
}