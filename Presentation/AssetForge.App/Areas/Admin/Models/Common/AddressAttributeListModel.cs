using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Common;

/// <summary>
/// Represents an address attribute list model
/// </summary>
public partial record AddressAttributeListModel : BasePagedListModel<AddressAttributeModel>
{
}