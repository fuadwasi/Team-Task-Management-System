using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a customer attribute list model
/// </summary>
public partial record CustomerAttributeListModel : BasePagedListModel<CustomerAttributeModel>
{
}