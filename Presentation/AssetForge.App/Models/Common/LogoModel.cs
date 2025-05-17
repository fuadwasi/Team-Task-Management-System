using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Models.Common;

public partial record LogoModel : BaseModel
{
    public string SiteName { get; set; }

    public string LogoPath { get; set; }
}