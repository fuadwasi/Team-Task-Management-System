using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Models.Teams
{
    public record TeamSearchModel : BaseSearchModel
    {
        public string Keyword { get; set; }
    }
}
