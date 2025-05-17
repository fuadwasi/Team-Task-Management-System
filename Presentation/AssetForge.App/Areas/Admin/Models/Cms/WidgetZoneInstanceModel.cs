using AssetForge.App.Areas.Admin.Models.Attributes;
using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Cms
{
    public record WidgetZoneInstanceModel : BaseEntityModel
    {
        public WidgetZoneInstanceModel()
        {
            AvailablePositions = new List<SelectListItem>();
            AvailableTemplates = new List<SelectListItem>();
            WidgetAttributeMappingSearchModel = new WidgetAttributeMappingSearchModel();
        }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Fields.Name")]
        public string Name { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Fields.Position")]
        public string Position { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Fields.WidgetZone")]
        public int WidgetZoneId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Fields.IsActive")]
        public bool IsActive { get; set; }

        public IList<SelectListItem> AvailablePositions { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Fields.Template")]
        public int TemplateId { get; set; }

        public WidgetAttributeMappingSearchModel WidgetAttributeMappingSearchModel { get; set; }

        public IList<SelectListItem> AvailableTemplates { get; set; }
    }
}
