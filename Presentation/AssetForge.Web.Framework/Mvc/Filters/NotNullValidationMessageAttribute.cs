using AssetForge.Core.Http.Extensions;
using AssetForge.Data;
using AssetForge.Services.Localization;
using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using AssetForge.Web.Framework.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AssetForge.Web.Framework.Mvc.Filters;

/// <summary>
/// Represents filter attribute that adds a detailed validation message that a value cannot be empty
/// </summary>
public sealed class NotNullValidationMessageAttribute : TypeFilterAttribute
{
    #region Ctor

    /// <summary>
    /// Create instance of the filter attribute
    /// </summary>
    public NotNullValidationMessageAttribute() : base(typeof(NotNullValidationMessageFilter))
    {
    }

    #endregion

    #region Nested filter

    /// <summary>
    /// Represents a filter that adds a detailed validation message that a value cannot be empty
    /// </summary>
    private class NotNullValidationMessageFilter : IAsyncActionFilter
    {
        #region Fields

        protected readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public NotNullValidationMessageFilter(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Called asynchronously before the action, after model binding is complete.
        /// </summary>
        /// <param name="context">A context for action filters</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        private async Task CheckNotNullValidationAsync(ActionExecutingContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            //only in POST requests
            if (!context.HttpContext.Request.IsPostRequest())
                return;

            if (!DataSettingsManager.IsDatabaseInstalled())
                return;

            //whether the model state is invalid
            if (context.ModelState.ErrorCount == 0)
                return;

            var nullModelValues = context.ModelState
                .Where(modelState => modelState.Value.ValidationState == ModelValidationState.Invalid
                                     && modelState.Value.Errors.Any(error => error.ErrorMessage.Equals(ValidationDefaults.NotNullValidationLocaleName)))
                .ToList();
            if (!nullModelValues.Any())
                return;

            //get model passed to the action
            var model = context.ActionArguments.Values.OfType<BaseModel>().FirstOrDefault();
            if (model is null)
                return;

            //get model properties that failed validation
            var modelType = model.GetType();
            var properties = modelType.GetProperties();
            var locale = await _localizationService.GetResourceAsync(ValidationDefaults.NotNullValidationLocaleName);
            foreach (var modelState in nullModelValues)
            {
                if (modelState.Value == null)
                    continue;

                var property = properties
                    .FirstOrDefault(propertyInfo => propertyInfo.Name.Equals(modelState.Key, StringComparison.InvariantCultureIgnoreCase));

                if (property is null)
                    continue;

                var resourceName = modelType.GetProperty(property.Name)?.GetCustomAttributes(typeof(ResourceDisplayNameAttribute), true)
                    .OfType<ResourceDisplayNameAttribute>()
                    .FirstOrDefault()?.ResourceKey;

                string resourceValue = null;

                if (!string.IsNullOrEmpty(resourceName))
                    //get locale resource value
                    resourceValue = await _localizationService.GetResourceAsync(resourceName);

                if (resourceValue?.Equals(resourceName, StringComparison.InvariantCultureIgnoreCase) ?? false)
                    resourceValue = property.Name;

                //set localized error message
                modelState.Value.Errors.Clear();
                modelState.Value.Errors.Add(string.Format(locale, resourceValue));
            }
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
            await CheckNotNullValidationAsync(context);
            if (context.Result == null)
                await next();
        }

        #endregion
    }

    #endregion
}