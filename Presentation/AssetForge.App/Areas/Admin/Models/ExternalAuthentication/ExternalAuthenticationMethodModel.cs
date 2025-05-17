using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.ExternalAuthentication;

/// <summary>
/// Represents an external authentication method model
/// </summary>
public partial record ExternalAuthenticationMethodModel : BaseModel, IPluginModel
{
    #region Properties

    [ResourceDisplayName("Admin.Configuration.Authentication.ExternalMethods.Fields.FriendlyName")]
    public string FriendlyName { get; set; }

    [ResourceDisplayName("Admin.Configuration.Authentication.ExternalMethods.Fields.SystemName")]
    public string SystemName { get; set; }

    [ResourceDisplayName("Admin.Configuration.Authentication.ExternalMethods.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    [ResourceDisplayName("Admin.Configuration.Authentication.ExternalMethods.Fields.IsActive")]
    public bool IsActive { get; set; }

    [ResourceDisplayName("Admin.Configuration.Authentication.ExternalMethods.Configure")]
    public string ConfigurationUrl { get; set; }

    public string LogoUrl { get; set; }

    #endregion
}