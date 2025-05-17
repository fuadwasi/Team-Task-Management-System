using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents an online customer model
/// </summary>
public partial record OnlineCustomerModel : BaseEntityModel
{
    #region Properties

    [ResourceDisplayName("Admin.Customers.OnlineCustomers.Fields.CustomerInfo")]
    public string CustomerInfo { get; set; }

    [ResourceDisplayName("Admin.Customers.OnlineCustomers.Fields.IPAddress")]
    public string LastIpAddress { get; set; }

    [ResourceDisplayName("Admin.Customers.OnlineCustomers.Fields.Location")]
    public string Location { get; set; }

    [ResourceDisplayName("Admin.Customers.OnlineCustomers.Fields.LastActivityDate")]
    public DateTime LastActivityDate { get; set; }

    [ResourceDisplayName("Admin.Customers.OnlineCustomers.Fields.LastVisitedPage")]
    public string LastVisitedPage { get; set; }

    #endregion
}