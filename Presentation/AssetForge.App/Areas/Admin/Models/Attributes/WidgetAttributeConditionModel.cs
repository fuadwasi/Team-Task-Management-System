using System.Collections.Generic;
using AssetForge.Core.Domain.Attributes;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    public partial record WidgetAttributeConditionModel : BaseModel
    {
        public WidgetAttributeConditionModel()
        {
            WidgetAttributes = new List<WidgetAttributeModel>();
        }
        
        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Condition.EnableCondition")]
        public bool EnableCondition { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Condition.Attributes")]
        public int SelectedWidgetAttributeId { get; set; }
        public IList<WidgetAttributeModel> WidgetAttributes { get; set; }

        public int WidgetAttributeMappingId { get; set; }

        #region Nested classes

        public partial record WidgetAttributeModel : BaseEntityModel
        {
            public WidgetAttributeModel()
            {
                Values = new List<WidgetAttributeValueModel>();
            }

            public int WidgetAttributeId { get; set; }

            public string Name { get; set; }

            public string TextPrompt { get; set; }

            public bool IsRequired { get; set; }

            public AttributeControlType AttributeControlType { get; set; }

            public IList<WidgetAttributeValueModel> Values { get; set; }
        }

        public partial record WidgetAttributeValueModel : BaseEntityModel
        {
            public string Name { get; set; }

            public bool IsPreSelected { get; set; }
        }

        #endregion
    }
}