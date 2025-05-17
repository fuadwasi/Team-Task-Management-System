using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Messages;

public partial record TestMessageTemplateModel : BaseEntityModel
{
    public TestMessageTemplateModel()
    {
        Tokens = new List<string>();
    }

    public int LanguageId { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Test.Tokens")]
    public List<string> Tokens { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Test.SendTo")]
    public string SendTo { get; set; }
}