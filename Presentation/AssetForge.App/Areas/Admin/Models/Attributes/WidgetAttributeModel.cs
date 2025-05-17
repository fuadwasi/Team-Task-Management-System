using AssetForge.App.Areas.Admin.Models.Attributes;
using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    /// <summary>
    /// Represents a widget attribute model
    /// </summary>
    public partial record WidgetAttributeModel : BaseEntityModel, ILocalizedModel<WidgetAttributeLocalizedModel>
    {
        #region Ctor

        public WidgetAttributeModel()
        {
            Locales = new List<WidgetAttributeLocalizedModel>();
            PredefinedWidgetAttributeValueSearchModel = new PredefinedWidgetAttributeValueSearchModel();
            WidgetAttributeWidgetSearchModel = new WidgetAttributeWidgetSearchModel();
        }

        #endregion

        #region Properties

        [ResourceDisplayName("Admin.Attributes.WidgetAttributes.Fields.Name")]
        public string Name { get; set; }

        [ResourceDisplayName("Admin.Attributes.WidgetAttributes.Fields.SystemName")]
        public string SystemName { get; set; }

        [ResourceDisplayName("Admin.Attributes.WidgetAttributes.Fields.Description")]
        public string Description {get;set;}

        public IList<WidgetAttributeLocalizedModel> Locales { get; set; }

        public PredefinedWidgetAttributeValueSearchModel PredefinedWidgetAttributeValueSearchModel { get; set; }

        public WidgetAttributeWidgetSearchModel WidgetAttributeWidgetSearchModel { get; set; }

        #endregion
    }

    public partial record WidgetAttributeLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [ResourceDisplayName("Admin.Attributes.WidgetAttributes.Fields.Name")]
        public string Name { get; set; }

        [ResourceDisplayName("Admin.Attributes.WidgetAttributes.Fields.Description")]
        public string Description {get;set;}
    }
}