using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a date time settings model
/// </summary>
public partial record DateTimeSettingsModel : BaseModel, ISettingsModel
{
    #region Ctor

    public DateTimeSettingsModel()
    {
        AvailableTimeZones = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AllowCustomersToSetTimeZone")]
    public bool AllowCustomersToSetTimeZone { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.DefaultSiteTimeZone")]
    public string DefaultSiteTimeZoneId { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.DefaultSiteTimeZone")]
    public IList<SelectListItem> AvailableTimeZones { get; set; }

    #endregion
}