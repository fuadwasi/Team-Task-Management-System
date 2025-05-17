using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a setting model
/// </summary>
public partial record SettingModel : BaseEntityModel
{
    #region Ctor

    public SettingModel()
    {
        AvailableSites = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.Name")]
    public string Name { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.Value")]
    public string Value { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.SiteName")]
    public string Site { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.Site")]
    public int SiteId { get; set; }
    public IList<SelectListItem> AvailableSites { get; set; }

    #endregion
}