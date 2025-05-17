using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a customer associated external authentication model
/// </summary>
public partial record CustomerAssociatedExternalAuthModel : BaseEntityModel
{
    #region Properties

    [ResourceDisplayName("Admin.Customers.Customers.AssociatedExternalAuth.Fields.Email")]
    public string Email { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.AssociatedExternalAuth.Fields.ExternalIdentifier")]
    public string ExternalIdentifier { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.AssociatedExternalAuth.Fields.AuthMethodName")]
    public string AuthMethodName { get; set; }

    #endregion
}