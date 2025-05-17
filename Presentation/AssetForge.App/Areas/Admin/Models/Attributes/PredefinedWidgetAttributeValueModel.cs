using System.Collections.Generic;
using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    /// <summary>
    /// Represents a predefined widget attribute value model
    /// </summary>
    public partial record PredefinedWidgetAttributeValueModel : BaseEntityModel, ILocalizedModel<PredefinedWidgetAttributeValueLocalizedModel>
    {
        #region Ctor

        public PredefinedWidgetAttributeValueModel()
        {
            Locales = new List<PredefinedWidgetAttributeValueLocalizedModel>();
        }

        #endregion

        #region Properties

        public int WidgetAttributeId { get; set; }

        [ResourceDisplayName("Admin.Attributes.WidgetAttributes.PredefinedValues.Fields.Name")]
        public string Name { get; set; }

        [ResourceDisplayName("Admin.Attributes.WidgetAttributes.PredefinedValues.Fields.IsPreSelected")]
        public bool IsPreSelected { get; set; }

        [ResourceDisplayName("Admin.Attributes.WidgetAttributes.PredefinedValues.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<PredefinedWidgetAttributeValueLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial record PredefinedWidgetAttributeValueLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [ResourceDisplayName("Admin.Attributes.WidgetAttributes.PredefinedValues.Fields.Name")]
        public string Name { get; set; }
    }
}