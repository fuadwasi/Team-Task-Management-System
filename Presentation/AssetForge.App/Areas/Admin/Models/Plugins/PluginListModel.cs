using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Plugins;

/// <summary>
/// Represents a plugin list model
/// </summary>
public partial record PluginListModel : BasePagedListModel<PluginModel>
{
}