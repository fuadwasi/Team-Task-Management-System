using AssetForge.App.Areas.Admin.Models.Sites;
using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a site scope configuration model
/// </summary>
public partial record SiteScopeConfigurationModel : BaseModel
{
    #region Ctor

    public SiteScopeConfigurationModel()
    {
        Sites = new List<SiteModel>();
    }

    #endregion

    #region Properties

    public int SiteId { get; set; }

    public IList<SiteModel> Sites { get; set; }

    #endregion
}