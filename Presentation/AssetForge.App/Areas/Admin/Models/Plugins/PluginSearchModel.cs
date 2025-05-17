using Microsoft.AspNetCore.Mvc.Rendering;
using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Plugins;

/// <summary>
/// Represents a plugin search model
/// </summary>
public partial record PluginSearchModel : BaseSearchModel
{
    #region Ctor

    public PluginSearchModel()
    {
        AvailableLoadModes = new List<SelectListItem>();
        AvailableGroups = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.Configuration.Plugins.LoadMode")]
    public int SearchLoadModeId { get; set; }

    [ResourceDisplayName("Admin.Configuration.Plugins.Group")]
    public string SearchGroup { get; set; }

    [ResourceDisplayName("Admin.Configuration.Plugins.FriendlyName")]
    public string SearchFriendlyName { get; set; }

    [ResourceDisplayName("Admin.Configuration.Plugins.Author")]
    public string SearchAuthor { get; set; }

    [ResourceDisplayName("Admin.Configuration.Plugins.LoadMode")]
    public IList<SelectListItem> AvailableLoadModes { get; set; }

    [ResourceDisplayName("Admin.Configuration.Plugins.Group")]
    public IList<SelectListItem> AvailableGroups { get; set; }

    public bool NeedToRestart { get; set; }

    #endregion
}