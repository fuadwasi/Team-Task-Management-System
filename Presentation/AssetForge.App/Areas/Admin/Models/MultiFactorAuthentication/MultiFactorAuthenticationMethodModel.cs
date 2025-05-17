using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.MultiFactorAuthentication;

/// <summary>
/// Represents an multi-factor authentication method model
/// </summary>
public partial record MultiFactorAuthenticationMethodModel : BaseModel, IPluginModel
{
    #region Properties

    [ResourceDisplayName("Admin.Configuration.Authentication.MultiFactorMethods.Fields.FriendlyName")]
    public string FriendlyName { get; set; }

    [ResourceDisplayName("Admin.Configuration.Authentication.MultiFactorMethods.Fields.SystemName")]
    public string SystemName { get; set; }

    [ResourceDisplayName("Admin.Configuration.Authentication.MultiFactorMethods.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    [ResourceDisplayName("Admin.Configuration.Authentication.MultiFactorMethods.Fields.IsActive")]
    public bool IsActive { get; set; }

    [ResourceDisplayName("Admin.Configuration.Authentication.MultiFactorMethods.Configure")]
    public string ConfigurationUrl { get; set; }

    public string LogoUrl { get; set; }

    #endregion
}