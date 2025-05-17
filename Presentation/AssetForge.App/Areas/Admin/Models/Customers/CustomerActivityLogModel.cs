using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a customer activity log model
/// </summary>
public partial record CustomerActivityLogModel : BaseEntityModel
{
    #region Properties

    [ResourceDisplayName("Admin.Customers.Customers.ActivityLog.ActivityLogType")]
    public string ActivityLogTypeName { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.ActivityLog.Comment")]
    public string Comment { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.ActivityLog.CreatedOn")]
    public DateTime CreatedOn { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.ActivityLog.IpAddress")]
    public string IpAddress { get; set; }

    #endregion
}