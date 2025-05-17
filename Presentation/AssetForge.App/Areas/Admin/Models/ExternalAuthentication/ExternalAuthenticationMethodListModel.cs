using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.ExternalAuthentication;

/// <summary>
/// Represents an external authentication method list model
/// </summary>
public partial record ExternalAuthenticationMethodListModel : BasePagedListModel<ExternalAuthenticationMethodModel>
{
}