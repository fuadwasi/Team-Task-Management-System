using AssetForge.App.Areas.Admin.Factories;
using AssetForge.App.Areas.Admin.Infrastructure.Mapper.Extensions;
using AssetForge.App.Areas.Admin.Models.Customers;
using AssetForge.Core;
using AssetForge.Core.Domain.Customers;
using AssetForge.Services.Customers;
using AssetForge.Services.Localization;
using AssetForge.Services.Logging;
using AssetForge.Services.Messages;
using AssetForge.Services.Security;
using AssetForge.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.App.Areas.Admin.Controllers;

public partial class CustomerRoleController : BaseAdminController
{
    #region Fields

    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerRoleModelFactory _customerRoleModelFactory;
    protected readonly ICustomerService _customerService;
    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public CustomerRoleController(ICustomerActivityService customerActivityService,
        ICustomerRoleModelFactory customerRoleModelFactory,
        ICustomerService customerService,
        ILocalizationService localizationService,
        INotificationService notificationService,
        IPermissionService permissionService,
        IWorkContext workContext)
    {
        _customerActivityService = customerActivityService;
        _customerRoleModelFactory = customerRoleModelFactory;
        _customerService = customerService;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _workContext = workContext;
    }

    #endregion

    #region Methods

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    public virtual async Task<IActionResult> List()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return AccessDeniedView();

        //prepare model
        var model = await _customerRoleModelFactory.PrepareCustomerRoleSearchModelAsync(new CustomerRoleSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> List(CustomerRoleSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _customerRoleModelFactory.PrepareCustomerRoleListModelAsync(searchModel);

        return Json(model);
    }

    public virtual async Task<IActionResult> Create()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers) || !await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAcl))
            return AccessDeniedView();

        //prepare model
        var model = await _customerRoleModelFactory.PrepareCustomerRoleModelAsync(new CustomerRoleModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    public virtual async Task<IActionResult> Create(CustomerRoleModel model, bool continueEditing)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers) || !await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAcl))
            return AccessDeniedView();

        if (ModelState.IsValid)
        {
            var customerRole = model.ToEntity<CustomerRole>();
            await _customerService.InsertCustomerRoleAsync(customerRole);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewCustomerRole",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewCustomerRole"), customerRole.Name), customerRole);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Customers.CustomerRoles.Added"));

            return continueEditing ? RedirectToAction("Edit", new { id = customerRole.Id }) : RedirectToAction("List");
        }

        //prepare model
        model = await _customerRoleModelFactory.PrepareCustomerRoleModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    public virtual async Task<IActionResult> Edit(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers) || !await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAcl))
            return AccessDeniedView();

        //try to get a customer role with the specified id
        var customerRole = await _customerService.GetCustomerRoleByIdAsync(id);
        if (customerRole == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _customerRoleModelFactory.PrepareCustomerRoleModelAsync(null, customerRole);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    public virtual async Task<IActionResult> Edit(CustomerRoleModel model, bool continueEditing)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers) || !await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAcl))
            return AccessDeniedView();

        //try to get a customer role with the specified id
        var customerRole = await _customerService.GetCustomerRoleByIdAsync(model.Id);
        if (customerRole == null)
            return RedirectToAction("List");

        try
        {
            if (ModelState.IsValid)
            {
                if (customerRole.IsSystemRole && !model.Active)
                    throw new AssetForgeException(await _localizationService.GetResourceAsync("Admin.Customers.CustomerRoles.Fields.Active.CantEditSystem"));

                if (customerRole.IsSystemRole && !customerRole.SystemName.Equals(model.SystemName, StringComparison.InvariantCultureIgnoreCase))
                    throw new AssetForgeException(await _localizationService.GetResourceAsync("Admin.Customers.CustomerRoles.Fields.SystemName.CantEditSystem"));

                //if (CustomerDefaults.RegisteredRoleName.Equals(customerRole.SystemName, StringComparison.InvariantCultureIgnoreCase) &&
                //    model.PurchasedWithProductId > 0)
                //    throw new AssetForgeException(await _localizationService.GetResourceAsync("Admin.Customers.CustomerRoles.Fields.PurchasedWithProduct.Registered"));

                customerRole = model.ToEntity(customerRole);
                await _customerService.UpdateCustomerRoleAsync(customerRole);

                //activity log
                await _customerActivityService.InsertActivityAsync("EditCustomerRole",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditCustomerRole"), customerRole.Name), customerRole);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Customers.CustomerRoles.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = customerRole.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = await _customerRoleModelFactory.PrepareCustomerRoleModelAsync(model, customerRole, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = customerRole.Id });
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> Delete(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers) || !await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAcl))
            return AccessDeniedView();

        //try to get a customer role with the specified id
        var customerRole = await _customerService.GetCustomerRoleByIdAsync(id);
        if (customerRole == null)
            return RedirectToAction("List");

        try
        {
            await _customerService.DeleteCustomerRoleAsync(customerRole);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteCustomerRole",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteCustomerRole"), customerRole.Name), customerRole);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Customers.CustomerRoles.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            _notificationService.ErrorNotification(exc.Message);
            return RedirectToAction("Edit", new { id = customerRole.Id });
        }
    }

    //public virtual async Task<IActionResult> AssociateProductToCustomerRolePopup()
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers) || !await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAcl))
    //        return AccessDeniedView();

    //    //prepare model
    //    var model = await _customerRoleModelFactory.PrepareCustomerRoleProductSearchModelAsync(new CustomerRoleProductSearchModel());

    //    return View(model);
    //}

    //[HttpPost]
    //public virtual async Task<IActionResult> AssociateProductToCustomerRolePopupList(CustomerRoleProductSearchModel searchModel)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers) || !await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAcl))
    //        return await AccessDeniedDataTablesJson();

    //    //prepare model
    //    var model = await _customerRoleModelFactory.PrepareCustomerRoleProductListModelAsync(searchModel);

    //    return Json(model);
    //}

    //[HttpPost]
    //[FormValueRequired("save")]
    //public virtual async Task<IActionResult> AssociateProductToCustomerRolePopup([Bind(Prefix = nameof(AddProductToCustomerRoleModel))] AddProductToCustomerRoleModel model)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers) || !await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAcl))
    //        return AccessDeniedView();

    //    //try to get a product with the specified id
    //    var associatedProduct = await _productService.GetProductByIdAsync(model.AssociatedToProductId);
    //    if (associatedProduct == null)
    //        return Content("Cannot load a product");

    //    //a vendor should have access only to his products
    //    var currentVendor = await _workContext.GetCurrentVendorAsync();
    //    if (currentVendor != null && associatedProduct.VendorId != currentVendor.Id)
    //        return Content("This is not your product");

    //    ViewBag.RefreshPage = true;
    //    ViewBag.productId = associatedProduct.Id;
    //    ViewBag.productName = associatedProduct.Name;

    //    return View(new CustomerRoleProductSearchModel());
    //}

    #endregion
}