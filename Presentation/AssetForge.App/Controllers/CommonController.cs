using AssetForge.App.Controllers;
using AssetForge.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.Web.Controllers;

[AutoValidateAntiforgeryToken]
public partial class CommonController : BasePublicController
{
    #region Methods

    //available even when a site is closed
    [CheckAccessClosedSite(ignore: true)]
    //available even when navigation is not allowed
    [CheckAccessPublicSite(ignore: true)]
    public virtual IActionResult FallbackRedirect()
    {
        //nothing was found
        return InvokeHttp404();
    }

    #endregion Methods
}