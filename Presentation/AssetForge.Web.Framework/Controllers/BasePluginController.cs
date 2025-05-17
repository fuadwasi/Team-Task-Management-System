using AssetForge.Web.Framework.Mvc.Filters;

namespace AssetForge.Web.Framework.Controllers;

/// <summary>
/// Base controller for plugins
/// </summary>
[NotNullValidationMessage]
public abstract partial class BasePluginController : BaseController
{
}