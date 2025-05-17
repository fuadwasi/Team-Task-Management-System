using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a customer role list model
/// </summary>
public partial record CustomerRoleListModel : BasePagedListModel<CustomerRoleModel>
{
}