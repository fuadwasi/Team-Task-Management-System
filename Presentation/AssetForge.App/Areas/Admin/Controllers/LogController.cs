using AssetForge.App.Areas.Admin.Factories;
using AssetForge.App.Areas.Admin.Models.Logging;
using AssetForge.Services.Localization;
using AssetForge.Services.Logging;
using AssetForge.Services.Messages;
using AssetForge.Services.Security;
using AssetForge.Web.Framework.Controllers;
using Microsoft.AspNetCore.Mvc;
using ILogger = AssetForge.Services.Logging.ILogger;

namespace AssetForge.App.Areas.Admin.Controllers;

public partial class LogController : BaseAdminController
{
    #region Fields

    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILogger _logger;
    protected readonly ILogModelFactory _logModelFactory;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;

    #endregion

    #region Ctor

    public LogController(ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        ILogger logger,
        ILogModelFactory logModelFactory,
        INotificationService notificationService,
        IPermissionService permissionService)
    {
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _logger = logger;
        _logModelFactory = logModelFactory;
        _notificationService = notificationService;
        _permissionService = permissionService;
    }

    #endregion

    #region Methods

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    public virtual async Task<IActionResult> List()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSystemLog))
            return AccessDeniedView();

        //prepare model
        var model = await _logModelFactory.PrepareLogSearchModelAsync(new LogSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> LogList(LogSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSystemLog))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _logModelFactory.PrepareLogListModelAsync(searchModel);

        return Json(model);
    }

    [HttpPost, ActionName("List")]
    [FormValueRequired("clearall")]
    public virtual async Task<IActionResult> ClearAll()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSystemLog))
            return AccessDeniedView();

        await _logger.ClearLogAsync();

        //activity log
        await _customerActivityService.InsertActivityAsync("DeleteSystemLog", await _localizationService.GetResourceAsync("ActivityLog.DeleteSystemLog"));

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.System.Log.Cleared"));

        return RedirectToAction("List");
    }

    public virtual async Task<IActionResult> View(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSystemLog))
            return AccessDeniedView();

        //try to get a log with the specified id
        var log = await _logger.GetLogByIdAsync(id);
        if (log == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _logModelFactory.PrepareLogModelAsync(null, log);

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> Delete(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSystemLog))
            return AccessDeniedView();

        //try to get a log with the specified id
        var log = await _logger.GetLogByIdAsync(id);
        if (log == null)
            return RedirectToAction("List");

        await _logger.DeleteLogAsync(log);

        //activity log
        await _customerActivityService.InsertActivityAsync("DeleteSystemLog", await _localizationService.GetResourceAsync("ActivityLog.DeleteSystemLog"), log);

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.System.Log.Deleted"));

        return RedirectToAction("List");
    }

    [HttpPost]
    public virtual async Task<IActionResult> DeleteSelected(ICollection<int> selectedIds)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSystemLog))
            return await AccessDeniedDataTablesJson();

        if (selectedIds == null || !selectedIds.Any())
            return NoContent();

        await _logger.DeleteLogsAsync((await _logger.GetLogByIdsAsync([.. selectedIds])).ToList());

        //activity log
        await _customerActivityService.InsertActivityAsync("DeleteSystemLog", await _localizationService.GetResourceAsync("ActivityLog.DeleteSystemLog"));

        return Json(new { Result = true });
    }

    #endregion
}