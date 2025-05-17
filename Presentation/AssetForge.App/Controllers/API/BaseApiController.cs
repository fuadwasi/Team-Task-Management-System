using AssetForge.App.Filters;
using AssetForge.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.App.Controllers.API
{
    [TokenAuthorize]
    [PublishModelEvents]
    //[DeviceIdAuthorize]
    [SaveIpAddress]
    [SaveLastActivity]
    [NstAuthorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BaseApiController : AssetForgeApiController
    {
    }
}
