using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a customer list model
/// </summary>
public partial record CustomerListModel : BasePagedListModel<CustomerModel>
{
}