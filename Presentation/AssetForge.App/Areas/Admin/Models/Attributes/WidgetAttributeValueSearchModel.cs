using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    /// <summary>
    /// Represents a widget attribute value search model
    /// </summary>
    public partial record WidgetAttributeValueSearchModel : BaseSearchModel
    {
        #region Properties

        public int WidgetAttributeMappingId { get; set; }

        #endregion
    }
}