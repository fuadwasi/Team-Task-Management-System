using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a GDPR log search model
/// </summary>
public partial record GdprLogSearchModel : BaseSearchModel
{
    #region Ctor

    public GdprLogSearchModel()
    {
        AvailableRequestTypes = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.Customers.GdprLog.List.SearchEmail")]
    [DataType(DataType.EmailAddress)]
    public string SearchEmail { get; set; }

    [ResourceDisplayName("Admin.Customers.GdprLog.List.SearchRequestType")]
    public int SearchRequestTypeId { get; set; }

    public IList<SelectListItem> AvailableRequestTypes { get; set; }

    #endregion
}