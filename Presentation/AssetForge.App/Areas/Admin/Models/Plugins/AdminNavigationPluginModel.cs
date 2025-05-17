using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Plugins;

/// <summary>
/// Represents a plugin model that is used for admin navigation
/// </summary>
public partial record AdminNavigationPluginModel : BaseModel
{
    #region Properties

    public string FriendlyName { get; set; }

    public string ConfigurationUrl { get; set; }

    #endregion
}