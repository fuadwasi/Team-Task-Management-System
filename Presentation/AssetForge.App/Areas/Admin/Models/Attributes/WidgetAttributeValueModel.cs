using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    /// <summary>
    /// Represents a widget attribute value model
    /// </summary>
    public partial record WidgetAttributeValueModel : BaseEntityModel, ILocalizedModel<WidgetAttributeValueLocalizedModel>
    {
        #region Ctor

        public WidgetAttributeValueModel()
        {
            WidgetPictureModels = new List<WidgetPictureModel>();
            Locales = new List<WidgetAttributeValueLocalizedModel>();
            PictureIds = new List<int>();
        }

        #endregion

        #region Properties

        public int WidgetAttributeMappingId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.AttributeValueType")]
        public int AttributeValueTypeId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.AttributeValueType")]
        public string AttributeValueTypeName { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.AssociatedWidget")]
        public int AssociatedWidgetZoneInstanceId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.AssociatedWidget")]
        public string AssociatedWidgetName { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.Name")]
        public string Name { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.ColorSquaresRgb")]
        public string ColorSquaresRgb { get; set; }

        public bool DisplayColorSquaresRgb { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.ImageSquaresPicture")]
        [UIHint("Picture")]
        public int ImageSquaresPictureId { get; set; }

        public bool DisplayImageSquaresPicture { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.PriceAdjustment")]
        public decimal PriceAdjustment { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.PriceAdjustment")]
        //used only on the values list page
        public string PriceAdjustmentStr { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.PriceAdjustmentUsePercentage")]
        public bool PriceAdjustmentUsePercentage { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.WeightAdjustment")]
        public decimal WeightAdjustment { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.WeightAdjustment")]
        //used only on the values list page
        public string WeightAdjustmentStr { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.Cost")]
        public decimal Cost { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.CustomerEntersQty")]
        public bool CustomerEntersQty { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.Quantity")]
        public int Quantity { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.IsPreSelected")]
        public bool IsPreSelected { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.Pictures")]
        public IList<int> PictureIds { get; set; }


        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.Picture")]
        public string PictureThumbnailUrl { get; set; }

        public IList<WidgetPictureModel> WidgetPictureModels { get; set; }

        public IList<WidgetAttributeValueLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial record WidgetAttributeValueLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.WidgetZoneInstances.Attributes.Values.Fields.Name")]
        public string Name { get; set; }
    }
}