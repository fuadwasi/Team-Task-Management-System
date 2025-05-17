using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a GDPR log (request) model
/// </summary>
public partial record GdprLogModel : BaseEntityModel
{
    #region Properties

    [ResourceDisplayName("Admin.Customers.GdprLog.Fields.CustomerInfo")]
    public string CustomerInfo { get; set; }

    [ResourceDisplayName("Admin.Customers.GdprLog.Fields.RequestType")]
    public string RequestType { get; set; }

    [ResourceDisplayName("Admin.Customers.GdprLog.Fields.RequestDetails")]
    public string RequestDetails { get; set; }

    [ResourceDisplayName("Admin.Customers.GdprLog.Fields.CreatedOn")]
    public DateTime CreatedOn { get; set; }

    #endregion
}