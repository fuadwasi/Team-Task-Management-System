using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    /// <summary>
    /// Represents a search model of widgets that use the widget attribute
    /// </summary>
    public partial record WidgetAttributeWidgetSearchModel : BaseSearchModel
    {
        #region Properties

        public int WidgetAttributeId { get; set; }

        #endregion
    }
}