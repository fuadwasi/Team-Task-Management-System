using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    /// <summary>
    /// Represents a model of widgets that use the widget attribute
    /// </summary>
    public partial record WidgetAttributeWidgetModel : BaseEntityModel
    {
        #region Properties

        [ResourceDisplayName("Admin.Attributes.WidgetAttributes.UsedByWidgets.Widget")]
        public string WidgetName { get; set; }

        [ResourceDisplayName("Admin.Attributes.WidgetAttributes.UsedByWidgets.Published")]
        public bool Published { get; set; }

        #endregion
    }
}