using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Areas.Admin.Models.Logging;

/// <summary>
/// Represents an activity log model
/// </summary>
public partial record ActivityLogModel : BaseEntityModel
{
    #region Properties

    [ResourceDisplayName("Admin.Customers.ActivityLog.Fields.ActivityLogType")]
    public string ActivityLogTypeName { get; set; }

    [ResourceDisplayName("Admin.Customers.ActivityLog.Fields.Customer")]
    public int CustomerId { get; set; }

    [ResourceDisplayName("Admin.Customers.ActivityLog.Fields.CustomerEmail")]
    [DataType(DataType.EmailAddress)]
    public string CustomerEmail { get; set; }

    [ResourceDisplayName("Admin.Customers.ActivityLog.Fields.Comment")]
    public string Comment { get; set; }

    [ResourceDisplayName("Admin.Customers.ActivityLog.Fields.CreatedOn")]
    public DateTime CreatedOn { get; set; }

    [ResourceDisplayName("Admin.Customers.ActivityLog.Fields.IpAddress")]
    public string IpAddress { get; set; }

    #endregion
}