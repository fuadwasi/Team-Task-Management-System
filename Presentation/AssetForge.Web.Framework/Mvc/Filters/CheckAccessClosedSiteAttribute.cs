using AssetForge.Core;
using AssetForge.Core.Domain;
using AssetForge.Data;
using AssetForge.Services.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;


namespace AssetForge.Web.Framework.Mvc.Filters;

/// <summary>
/// Represents a filter attribute that confirms access to a closed site
/// </summary>
public sealed class CheckAccessClosedSiteAttribute : TypeFilterAttribute
{
    #region Ctor

    /// <summary>
    /// Create instance of the filter attribute
    /// </summary>
    /// <param name="ignore">Whether to ignore the execution of filter actions</param>
    public CheckAccessClosedSiteAttribute(bool ignore = false) : base(typeof(CheckAccessClosedSiteFilter))
    {
        IgnoreFilter = ignore;
        Arguments = [ignore];
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether to ignore the execution of filter actions
    /// </summary>
    public bool IgnoreFilter { get; }

    #endregion

    #region Nested filter

    /// <summary>
    /// Represents a filter that confirms access to closed site
    /// </summary>
    private class CheckAccessClosedSiteFilter : IAsyncActionFilter
    {
        #region Fields

        protected readonly bool _ignoreFilter;
        protected readonly IPermissionService _permissionService;
        protected readonly ISiteContext _siteContext;
        protected readonly SiteInformationSettings _siteInformationSettings;

        #endregion

        #region Ctor

        public CheckAccessClosedSiteFilter(bool ignoreFilter,
            IPermissionService permissionService,
            ISiteContext siteContext,
            SiteInformationSettings siteInformationSettings)
        {
            _ignoreFilter = ignoreFilter;
            _permissionService = permissionService;
            _siteContext = siteContext;
            _siteInformationSettings = siteInformationSettings;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Called asynchronously before the action, after model binding is complete.
        /// </summary>
        /// <param name="context">A context for action filters</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        private async Task CheckAccessClosedSiteAsync(ActionExecutingContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (!DataSettingsManager.IsDatabaseInstalled())
                return;

            //check whether this filter has been overridden for the Action
            var actionFilter = context.ActionDescriptor.FilterDescriptors
                .Where(filterDescriptor => filterDescriptor.Scope == FilterScope.Action)
                .Select(filterDescriptor => filterDescriptor.Filter)
                .OfType<CheckAccessClosedSiteAttribute>()
                .FirstOrDefault();

            //ignore filter (the action is available even if a site is closed)
            if (actionFilter?.IgnoreFilter ?? _ignoreFilter)
                return;

            //site isn't closed
            if (!_siteInformationSettings.SiteClosed)
                return;

            //get action and controller names
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var actionName = actionDescriptor?.ActionName;
            var controllerName = actionDescriptor?.ControllerName;

            if (string.IsNullOrEmpty(actionName) || string.IsNullOrEmpty(controllerName))
                return;

            //two factor verification accessible when a site is closed
            if (controllerName.Equals("Customer", StringComparison.InvariantCultureIgnoreCase) &&
                actionName.Equals("MultiFactorVerification", StringComparison.InvariantCultureIgnoreCase))
                return;

            //pages accessible when a site is closed
            if (controllerName.Equals("Page", StringComparison.InvariantCultureIgnoreCase) &&
                actionName.Equals("PageDetails", StringComparison.InvariantCultureIgnoreCase))
            {
                //get identifiers of pages are accessible when a site is closed

                //var site = await _siteContext.GetCurrentSiteAsync();
                //var allowedPageIds = (await _pageService.GetAllPagesAsync(site.Id))
                //    .Where(page => page.AccessibleWhenSiteClosed)
                //    .Select(page => page.Id);

                //check whether requested page is allowed
                //var requestedPageId = context.RouteData.Values["pageId"] as int?;
                //if (requestedPageId.HasValue && allowedPageIds.Contains(requestedPageId.Value))
                //    return;
            }

            //check whether current customer has access to a closed site
            if (await _permissionService.AuthorizeAsync(StandardPermissionProvider.AccessClosedSite))
                return;

            //site is closed and no access, so redirect to 'SiteClosed' page
            context.Result = new RedirectToRouteResult("SiteClosed", null);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called asynchronously before the action, after model binding is complete.
        /// </summary>
        /// <param name="context">A context for action filters</param>
        /// <param name="next">A delegate invoked to execute the next action filter or the action itself</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await CheckAccessClosedSiteAsync(context);
            if (context.Result == null)
                await next();
        }

        #endregion
    }

    #endregion
}