using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    /// <summary>
    /// Represents a widget attribute mapping model
    /// </summary>
    public partial record WidgetAttributeMappingModel : BaseEntityModel, ILocalizedModel<WidgetAttributeMappingLocalizedModel>
    {
        #region Ctor

        public WidgetAttributeMappingModel()
        {
            AvailableWidgetAttributes = new List<SelectListItem>();
            Locales = new List<WidgetAttributeMappingLocalizedModel>();
            ConditionModel = new WidgetAttributeConditionModel();
            WidgetAttributeValueSearchModel = new WidgetAttributeValueSearchModel();
        }

        #endregion

        #region Properties

        public int WidgetZoneInstanceId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Fields.Attribute")]
        public int WidgetAttributeId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Fields.Attribute")]
        public string WidgetAttribute { get; set; }

        public IList<SelectListItem> AvailableWidgetAttributes { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Fields.TextPrompt")]
        public string TextPrompt { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Fields.IsRequired")]
        public bool IsRequired { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Fields.AttributeControlType")]
        public int AttributeControlTypeId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Fields.AttributeControlType")]
        public string AttributeControlTypeStr { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        //validation fields
        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.ValidationRules.MinLength")]
        [UIHint("Int32Nullable")]
        public int? ValidationMinLength { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.ValidationRules.MaxLength")]
        [UIHint("Int32Nullable")]
        public int? ValidationMaxLength { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.ValidationRules.FileAllowedExtensions")]
        public string ValidationFileAllowedExtensions { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.ValidationRules.FileMaximumSize")]
        [UIHint("Int32Nullable")]
        public int? ValidationFileMaximumSize { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.ValidationRules.DefaultValue")]
        public string DefaultValue { get; set; }

        public string ValidationRulesString { get; set; }

        //condition
        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Condition")]
        public bool ConditionAllowed { get; set; }

        public string ConditionString { get; set; }

        public WidgetAttributeConditionModel ConditionModel { get; set; }

        public IList<WidgetAttributeMappingLocalizedModel> Locales { get; set; }

        public WidgetAttributeValueSearchModel WidgetAttributeValueSearchModel { get; set; }

        #endregion
    }

    public partial record WidgetAttributeMappingLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Fields.TextPrompt")]
        public string TextPrompt { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.ValidationRules.DefaultValue")]
        public string DefaultValue { get; set; }
    }
}