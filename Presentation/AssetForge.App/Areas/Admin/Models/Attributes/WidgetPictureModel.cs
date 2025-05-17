using System.ComponentModel.DataAnnotations;
using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    /// <summary>
    /// Represents a widget picture model
    /// </summary>
    public partial record WidgetPictureModel : BaseEntityModel
    {
        #region Properties

        public int WidgetInstanceId { get; set; }

        [UIHint("MultiPicture")]
        [ResourceDisplayName("Admin.Catalog.Widgets.Multimedia.Pictures.Fields.Picture")]
        public int PictureId { get; set; }

        [ResourceDisplayName("Admin.Catalog.Widgets.Multimedia.Pictures.Fields.Picture")]
        public string PictureUrl { get; set; }

        [ResourceDisplayName("Admin.Catalog.Widgets.Multimedia.Pictures.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ResourceDisplayName("Admin.Catalog.Widgets.Multimedia.Pictures.Fields.OverrideAltAttribute")]
        public string OverrideAltAttribute { get; set; }

        [ResourceDisplayName("Admin.Catalog.Widgets.Multimedia.Pictures.Fields.OverrideTitleAttribute")]
        public string OverrideTitleAttribute { get; set; }

        #endregion
    }
}