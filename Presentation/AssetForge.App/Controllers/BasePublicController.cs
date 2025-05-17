using AssetForge.Web.Framework.Controllers;
using AssetForge.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.App.Controllers;

[WwwRequirement]
[CheckLanguageSeoCode]
[CheckAccessPublicSite]
[CheckAccessClosedSite]
public abstract partial class BasePublicController : BaseController
{
    protected virtual IActionResult InvokeHttp404()
    {
        Response.StatusCode = 404;
        return new EmptyResult();
    }
}