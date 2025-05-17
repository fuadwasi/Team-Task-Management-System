using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Cms
{
    public record WidgetZoneModel : BaseEntityModel
    {
        public WidgetZoneModel()
        {
            WidgetInstanceSearchModel = new WidgetZoneInstanceSearchModel();
        }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZones.Fields.Name")]
        public string Name { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZones.Fields.SystemName")]
        public string SystemName { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZones.Fields.IsActive")]
        public bool IsActive { get; set; }

        public WidgetZoneInstanceSearchModel WidgetInstanceSearchModel { get; set; }
    }
}
