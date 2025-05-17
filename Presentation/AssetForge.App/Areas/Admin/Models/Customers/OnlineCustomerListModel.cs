using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents an online customer list model
/// </summary>
public partial record OnlineCustomerListModel : BasePagedListModel<OnlineCustomerModel>
{
}