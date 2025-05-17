using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Plugins;

/// <summary>
/// Represents a plugins configuration model
/// </summary>
public partial record PluginsConfigurationModel : BaseModel
{
    #region Ctor

    public PluginsConfigurationModel()
    {
        PluginsLocal = new PluginSearchModel();
    }

    #endregion

    #region Properties

    public PluginSearchModel PluginsLocal { get; set; }

    //public OfficialFeedPluginSearchModel AllPluginsAndThemes { get; set; }

    #endregion
}