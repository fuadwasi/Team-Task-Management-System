using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Common;

public partial record SystemWarningModel : BaseModel
{
    public SystemWarningLevel Level { get; set; }

    public string Text { get; set; }

    public bool DontEncode { get; set; }

    public override string ToString()
    {
        return Text;
    }
}