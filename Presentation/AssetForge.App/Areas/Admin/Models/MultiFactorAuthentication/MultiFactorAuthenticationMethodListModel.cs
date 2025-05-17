using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.MultiFactorAuthentication;

/// <summary>
/// Represents an multi-factor authentication method list model
/// </summary>
public partial record MultiFactorAuthenticationMethodListModel : BasePagedListModel<MultiFactorAuthenticationMethodModel>
{
}