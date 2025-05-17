namespace AssetForge.Web.Framework.Models.Cms;

public partial record RenderWidgetModel : BaseModel
{
    public Type WidgetViewComponent { get; set; }
    public object WidgetViewComponentArguments { get; set; }
}