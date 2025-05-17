using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a customer attribute value search model
/// </summary>
public partial record CustomerAttributeValueSearchModel : BaseSearchModel
{
    #region Properties

    public int CustomerAttributeId { get; set; }

    #endregion
}