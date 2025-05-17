using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Cms;

/// <summary>
/// Represents a widget model
/// </summary>
public partial record WidgetModel : BaseModel, IPluginModel
{
    #region Properties

    [ResourceDisplayName("Admin.ContentManagement.Widgets.Fields.FriendlyName")]
    public string FriendlyName { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Widgets.Fields.SystemName")]
    public string SystemName { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Widgets.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Widgets.Fields.IsActive")]
    public bool IsActive { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Widgets.Configure")]
    public string ConfigurationUrl { get; set; }

    public string LogoUrl { get; set; }

    public string WidgetViewComponentName { get; set; }

    public RouteValueDictionary WidgetViewComponentArguments { get; set; }

    #endregion
}