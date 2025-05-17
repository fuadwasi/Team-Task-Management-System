using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Cms
{
    public record LayoutModel : BaseEntityModel, ISiteMappingSupportedModel
    {
        public LayoutModel()
        {
            AvailableTemplates = new List<SelectListItem>();
            AvailableWidgets = new List<SelectListItem>();

            SelectedSiteIds = new List<int>();
            AvailableSites = new List<SelectListItem>();
        }

        [ResourceDisplayName("Admin.ContentManagement.Layouts.Fields.Name")]
        public string Name { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Layouts.Fields.Template")]
        public int TemplateId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Layouts.Fields.Template")]
        public string TemplateName { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Layouts.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Layouts.Fields.IsMobile")]
        public bool IsMobile { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Layouts.Fields.IsActive")]
        public bool IsActive { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Layouts.Fields.IsDefault")]
        public bool IsDefault { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Layouts.Fields.IsControlPanel")]
        public bool IsControlPanel { get; set; }


        public IList<SelectListItem> AvailableTemplates { get; set; }

        public IList<SelectListItem> AvailableWidgets { get; set; }

        //Site mapping
        [ResourceDisplayName("Admin.ContentManagement.Layouts.Fields.LimitedToSites")]
        public IList<int> SelectedSiteIds { get; set; }

        public IList<SelectListItem> AvailableSites { get; set; }
    }
}
