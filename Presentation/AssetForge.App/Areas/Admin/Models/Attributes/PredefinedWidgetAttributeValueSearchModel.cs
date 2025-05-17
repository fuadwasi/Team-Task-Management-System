using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    /// <summary>
    /// Represents a predefined widget attribute value search model
    /// </summary>
    public partial record PredefinedWidgetAttributeValueSearchModel : BaseSearchModel
    {
        #region Properties

        public int WidgetAttributeId { get; set; }

        #endregion
    }
}