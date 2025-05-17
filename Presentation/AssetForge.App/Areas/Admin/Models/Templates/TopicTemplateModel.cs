using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Templates;

/// <summary>
/// Represents a topic template model
/// </summary>
public partial record TopicTemplateModel : BaseEntityModel
{
    #region Properties

    [ResourceDisplayName("Admin.System.Templates.Topic.Name")]
    public string Name { get; set; }

    [ResourceDisplayName("Admin.System.Templates.Topic.ViewPath")]
    public string ViewPath { get; set; }

    [ResourceDisplayName("Admin.System.Templates.Topic.DisplayOrder")]
    public int DisplayOrder { get; set; }

    #endregion
}