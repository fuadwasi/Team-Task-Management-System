using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    /// <summary>
    /// Represents a widget attribute mapping search model
    /// </summary>
    public partial record WidgetAttributeMappingSearchModel : BaseSearchModel
    {
        #region Properties

        public int WidgetZoneInstanceId { get; set; }

        #endregion
    }
}