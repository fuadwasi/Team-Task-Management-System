using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a associated external auth records search model
/// </summary>
public partial record CustomerAssociatedExternalAuthRecordsSearchModel : BaseSearchModel
{
    #region Properties

    public int CustomerId { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.AssociatedExternalAuth")]
    public IList<CustomerAssociatedExternalAuthModel> AssociatedExternalAuthRecords { get; set; } = new List<CustomerAssociatedExternalAuthModel>();

    #endregion
}