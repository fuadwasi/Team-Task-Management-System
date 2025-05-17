using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Logging;

/// <summary>
/// Represents an activity log type search model
/// </summary>
public partial record ActivityLogTypeSearchModel : BaseSearchModel
{
    #region Properties       

    public IList<ActivityLogTypeModel> ActivityLogTypeListModel { get; set; }

    #endregion
}