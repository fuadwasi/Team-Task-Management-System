using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Plugins;

/// <summary>
/// Represents a plugin model
/// </summary>
public partial record PluginModel : BaseModel, IAclSupportedModel, ILocalizedModel<PluginLocalizedModel>, IPluginModel, ISiteMappingSupportedModel
{
    #region Ctor

    public PluginModel()
    {
        Locales = new List<PluginLocalizedModel>();

        SelectedSiteIds = new List<int>();
        AvailableSites = new List<SelectListItem>();
        SelectedCustomerRoleIds = new List<int>();
        AvailableCustomerRoles = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.Configuration.Plugins.Fields.Group")]
    public string Group { get; set; }

    [ResourceDisplayName("Admin.Configuration.Plugins.Fields.FriendlyName")]
    public string FriendlyName { get; set; }

    [ResourceDisplayName("Admin.Configuration.Plugins.Fields.SystemName")]
    public string SystemName { get; set; }

    [ResourceDisplayName("Admin.Configuration.Plugins.Fields.Version")]
    public string Version { get; set; }

    [ResourceDisplayName("Admin.Configuration.Plugins.Fields.Author")]
    public string Author { get; set; }

    [ResourceDisplayName("Admin.Configuration.Plugins.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    [ResourceDisplayName("Admin.Configuration.Plugins.Fields.Configure")]
    public string ConfigurationUrl { get; set; }

    [ResourceDisplayName("Admin.Configuration.Plugins.Fields.Installed")]
    public bool Installed { get; set; }

    public string Description { get; set; }

    public bool CanChangeEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Plugins.Fields.IsEnabled")]
    public bool IsEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Plugins.Fields.Logo")]
    public string LogoUrl { get; set; }

    public IList<PluginLocalizedModel> Locales { get; set; }

    //ACL (customer roles)
    [ResourceDisplayName("Admin.Configuration.Plugins.Fields.AclCustomerRoles")]
    public IList<int> SelectedCustomerRoleIds { get; set; }

    public IList<SelectListItem> AvailableCustomerRoles { get; set; }

    //site mapping
    [ResourceDisplayName("Admin.Configuration.Plugins.Fields.LimitedToSites")]
    public IList<int> SelectedSiteIds { get; set; }

    public IList<SelectListItem> AvailableSites { get; set; }

    public bool IsActive { get; set; }

    #endregion
}

public partial record PluginLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [ResourceDisplayName("Admin.Configuration.Plugins.Fields.FriendlyName")]
    public string FriendlyName { get; set; }
}