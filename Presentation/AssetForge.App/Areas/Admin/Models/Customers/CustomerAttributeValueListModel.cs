using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a customer attribute value list model
/// </summary>
public partial record CustomerAttributeValueListModel : BasePagedListModel<CustomerAttributeValueModel>
{
}