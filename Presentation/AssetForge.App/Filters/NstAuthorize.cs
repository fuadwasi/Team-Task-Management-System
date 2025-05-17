using AssetForge.App.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace AssetForge.App.Filters
{
    public class NstAuthorizeAttribute : TypeFilterAttribute
    {
        #region Ctor

        public NstAuthorizeAttribute() : base(typeof(NstAuthorize))
        {

        }

        #endregion

        #region Nested filter

        public class NstAuthorize : IActionFilter
        {
            public void OnActionExecuting(ActionExecutingContext filterContext)
            {
                return;
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                //do nothing
            }

            private long ConvertToTimestamp(DateTime value)
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var elapsedTime = value - epoch;
                return (long)elapsedTime.TotalSeconds;
            }
        }

        #endregion
    }
}
