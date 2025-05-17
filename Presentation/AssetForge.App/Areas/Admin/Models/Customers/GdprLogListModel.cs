using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a GDPR request list model
/// </summary>
public partial record GdprLogListModel : BasePagedListModel<GdprLogModel>
{
}