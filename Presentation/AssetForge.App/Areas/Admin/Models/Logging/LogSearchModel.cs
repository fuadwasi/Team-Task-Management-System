using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Areas.Admin.Models.Logging;

/// <summary>
/// Represents a log search model
/// </summary>
public partial record LogSearchModel : BaseSearchModel
{
    #region Ctor

    public LogSearchModel()
    {
        AvailableLogLevels = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.System.Log.List.CreatedOnFrom")]
    [UIHint("DateNullable")]
    public DateTime? CreatedOnFrom { get; set; }

    [ResourceDisplayName("Admin.System.Log.List.CreatedOnTo")]
    [UIHint("DateNullable")]
    public DateTime? CreatedOnTo { get; set; }

    [ResourceDisplayName("Admin.System.Log.List.Message")]
    public string Message { get; set; }

    [ResourceDisplayName("Admin.System.Log.List.LogLevel")]
    public int LogLevelId { get; set; }

    public IList<SelectListItem> AvailableLogLevels { get; set; }

    #endregion
}