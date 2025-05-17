using AssetForge.App.Areas.Admin.Models.Customers;
using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a customer user settings model
/// </summary>
public partial record CustomerUserSettingsModel : BaseModel, ISettingsModel
{
    #region Ctor

    public CustomerUserSettingsModel()
    {
        CustomerSettings = new CustomerSettingsModel();
        DateTimeSettings = new DateTimeSettingsModel();
        ExternalAuthenticationSettings = new ExternalAuthenticationSettingsModel();
        MultiFactorAuthenticationSettings = new MultiFactorAuthenticationSettingsModel();
        CustomerAttributeSearchModel = new CustomerAttributeSearchModel();
    }

    #endregion

    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    public CustomerSettingsModel CustomerSettings { get; set; }

    public DateTimeSettingsModel DateTimeSettings { get; set; }

    public ExternalAuthenticationSettingsModel ExternalAuthenticationSettings { get; set; }

    public MultiFactorAuthenticationSettingsModel MultiFactorAuthenticationSettings { get; set; }

    public CustomerAttributeSearchModel CustomerAttributeSearchModel { get; set; }

    //public AddressAttributeSearchModel AddressAttributeSearchModel { get; set; }

    #endregion
}