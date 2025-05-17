using AssetForge.App.Areas.Admin.Factories;
using AssetForge.App.Areas.Admin.Infrastructure.Mapper.Extensions;
using AssetForge.App.Areas.Admin.Models.Customers;
using AssetForge.Core;
using AssetForge.Core.Domain.Attributes;
using AssetForge.Core.Domain.Common;
using AssetForge.Core.Domain.Customers;

using AssetForge.Core.Events;
using AssetForge.Services.Attributes;
using AssetForge.Services.Common;
using AssetForge.Services.Customers;
using AssetForge.Services.ExportImport;
using AssetForge.Services.Helpers;
using AssetForge.Services.Localization;
using AssetForge.Services.Logging;
using AssetForge.Services.Security;
using AssetForge.Services.Sites;
using AssetForge.Web.Framework.Controllers;
using AssetForge.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Globalization;
using System.Text;

namespace AssetForge.App.Areas.Admin.Controllers;

public partial class CustomerController : BaseAdminController
{
    #region Fields

    protected readonly CustomerSettings _customerSettings;
    protected readonly DateTimeSettings _dateTimeSettings;
    protected readonly IAddressService _addressService;
    protected readonly IAttributeParser<AddressAttribute, AddressAttributeValue> _addressAttributeParser;
    protected readonly IAttributeParser<CustomerAttribute, CustomerAttributeValue> _customerAttributeParser;
    protected readonly IAttributeService<CustomerAttribute, CustomerAttributeValue> _customerAttributeService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerModelFactory _customerModelFactory;
    protected readonly ICustomerRegistrationService _customerRegistrationService;
    protected readonly ICustomerService _customerService;
    protected readonly IDateTimeHelper _dateTimeHelper;
    protected readonly IEventPublisher _eventPublisher;
    protected readonly IExportManager _exportManager;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IImportManager _importManager;
    protected readonly ILocalizationService _localizationService;
    protected readonly IPermissionService _permissionService;
    protected readonly ISiteContext _siteContext;
    protected readonly ISiteService _siteService;
    protected readonly IWorkContext _workContext;
    private static readonly char[] _separator = [','];

    #endregion

    #region Ctor

    public CustomerController(CustomerSettings customerSettings,
        DateTimeSettings dateTimeSettings,
        IAddressService addressService,
        IAttributeParser<AddressAttribute, AddressAttributeValue> addressAttributeParser,
        IAttributeParser<CustomerAttribute, CustomerAttributeValue> customerAttributeParser,
        IAttributeService<CustomerAttribute, CustomerAttributeValue> customerAttributeService,
        ICustomerActivityService customerActivityService,
        ICustomerModelFactory customerModelFactory,
        ICustomerRegistrationService customerRegistrationService,
        ICustomerService customerService,
        IDateTimeHelper dateTimeHelper,
        IEventPublisher eventPublisher,
        IExportManager exportManager,
        IGenericAttributeService genericAttributeService,
        IImportManager importManager,
        ILocalizationService localizationService,
        IPermissionService permissionService,
        ISiteContext siteContext,
        ISiteService siteService,
        IWorkContext workContext)
    {
        _customerSettings = customerSettings;
        _dateTimeSettings = dateTimeSettings;
        _addressService = addressService;
        _addressAttributeParser = addressAttributeParser;
        _customerAttributeParser = customerAttributeParser;
        _customerAttributeService = customerAttributeService;
        _customerActivityService = customerActivityService;
        _customerModelFactory = customerModelFactory;
        _customerRegistrationService = customerRegistrationService;
        _customerService = customerService;
        _dateTimeHelper = dateTimeHelper;
        _eventPublisher = eventPublisher;
        _exportManager = exportManager;
        _genericAttributeService = genericAttributeService;
        _importManager = importManager;
        _localizationService = localizationService;
        _permissionService = permissionService;
        _siteContext = siteContext;
        _siteService = siteService;
        _workContext = workContext;
    }

    #endregion

    #region Utilities

    protected virtual async Task<string> ValidateCustomerRolesAsync(IList<CustomerRole> customerRoles, IList<CustomerRole> existingCustomerRoles)
    {
        ArgumentNullException.ThrowIfNull(customerRoles);

        ArgumentNullException.ThrowIfNull(existingCustomerRoles);

        //check ACL permission to manage customer roles
        var rolesToAdd = customerRoles.Except(existingCustomerRoles, new CustomerRoleComparerByName());
        var rolesToDelete = existingCustomerRoles.Except(customerRoles, new CustomerRoleComparerByName());
        if (rolesToAdd.Any(role => role.SystemName != CustomerDefaults.RegisteredRoleName) || rolesToDelete.Any())
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAcl))
                return await _localizationService.GetResourceAsync("Admin.Customers.Customers.CustomerRolesManagingError");
        }

        //ensure a customer is not added to both 'Guests' and 'Registered' customer roles
        //ensure that a customer is in at least one required role ('Guests' and 'Registered')
        var isInGuestsRole = customerRoles.FirstOrDefault(cr => cr.SystemName == CustomerDefaults.GuestsRoleName) != null;
        var isInRegisteredRole = customerRoles.FirstOrDefault(cr => cr.SystemName == CustomerDefaults.RegisteredRoleName) != null;
        if (isInGuestsRole && isInRegisteredRole)
            return await _localizationService.GetResourceAsync("Admin.Customers.Customers.GuestsAndRegisteredRolesError");
        if (!isInGuestsRole && !isInRegisteredRole)
            return await _localizationService.GetResourceAsync("Admin.Customers.Customers.AddCustomerToGuestsOrRegisteredRoleError");

        //no errors
        return string.Empty;
    }

    protected virtual async Task<string> ParseCustomCustomerAttributesAsync(IFormCollection form)
    {
        ArgumentNullException.ThrowIfNull(form);

        var attributesXml = string.Empty;
        var customerAttributes = await _customerAttributeService.GetAllAttributesAsync();
        foreach (var attribute in customerAttributes)
        {
            var controlId = $"{AssetForgeCustomerServicesDefaults.CustomerAttributePrefix}{attribute.Id}";
            StringValues ctrlAttributes;

            switch (attribute.AttributeControlType)
            {
                case AttributeControlType.DropdownList:
                case AttributeControlType.RadioList:
                    ctrlAttributes = form[controlId];
                    if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                    {
                        var selectedAttributeId = int.Parse(ctrlAttributes);
                        if (selectedAttributeId > 0)
                            attributesXml = _customerAttributeParser.AddAttribute(attributesXml,
                                attribute, selectedAttributeId.ToString());
                    }

                    break;
                case AttributeControlType.Checkboxes:
                    var cblAttributes = form[controlId];
                    if (!StringValues.IsNullOrEmpty(cblAttributes))
                    {
                        foreach (var item in cblAttributes.ToString()
                                     .Split(_separator, StringSplitOptions.RemoveEmptyEntries))
                        {
                            var selectedAttributeId = int.Parse(item);
                            if (selectedAttributeId > 0)
                                attributesXml = _customerAttributeParser.AddAttribute(attributesXml,
                                    attribute, selectedAttributeId.ToString());
                        }
                    }

                    break;
                case AttributeControlType.ReadonlyCheckboxes:
                    //load read-only (already server-side selected) values
                    var attributeValues = await _customerAttributeService.GetAttributeValuesAsync(attribute.Id);
                    foreach (var selectedAttributeId in attributeValues
                                 .Where(v => v.IsPreSelected)
                                 .Select(v => v.Id)
                                 .ToList())
                    {
                        attributesXml = _customerAttributeParser.AddAttribute(attributesXml,
                            attribute, selectedAttributeId.ToString());
                    }

                    break;
                case AttributeControlType.TextBox:
                case AttributeControlType.MultilineTextbox:
                    ctrlAttributes = form[controlId];
                    if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                    {
                        var enteredText = ctrlAttributes.ToString().Trim();
                        attributesXml = _customerAttributeParser.AddAttribute(attributesXml,
                            attribute, enteredText);
                    }

                    break;
                case AttributeControlType.Datepicker:
                case AttributeControlType.ColorSquares:
                case AttributeControlType.ImageSquares:
                case AttributeControlType.FileUpload:
                //not supported customer attributes
                default:
                    break;
            }
        }

        return attributesXml;
    }

    protected virtual async Task<bool> SecondAdminAccountExistsAsync(Customer customer)
    {
        var customers = await _customerService.GetAllCustomersAsync(customerRoleIds: [(await _customerService.GetCustomerRoleBySystemNameAsync(CustomerDefaults.AdministratorsRoleName)).Id]);

        return customers.Any(c => c.Active && c.Id != customer.Id);
    }

    #endregion

    #region Customers

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    public virtual async Task<IActionResult> List()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return AccessDeniedView();

        //prepare model
        var model = await _customerModelFactory.PrepareCustomerSearchModelAsync(new CustomerSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> CustomerList(CustomerSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _customerModelFactory.PrepareCustomerListModelAsync(searchModel);

        return Json(model);
    }

    public virtual async Task<IActionResult> Create()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return AccessDeniedView();

        //prepare model
        var model = await _customerModelFactory.PrepareCustomerModelAsync(new CustomerModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [FormValueRequired("save", "save-continue")]
    public virtual async Task<IActionResult> Create(CustomerModel model, bool continueEditing, IFormCollection form)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return AccessDeniedView();

        if (!string.IsNullOrWhiteSpace(model.Email) && await _customerService.GetCustomerByEmailAsync(model.Email) != null)
            ModelState.AddModelError(string.Empty, "Email is already registered");

        if (!string.IsNullOrWhiteSpace(model.Username) && _customerSettings.UsernamesEnabled &&
            await _customerService.GetCustomerByUsernameAsync(model.Username) != null)
        {
            ModelState.AddModelError(string.Empty, "Username is already registered");
        }

        //validate customer roles
        var allCustomerRoles = await _customerService.GetAllCustomerRolesAsync(true);
        var newCustomerRoles = new List<CustomerRole>();
        foreach (var customerRole in allCustomerRoles)
            if (model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                newCustomerRoles.Add(customerRole);
        var customerRolesError = await ValidateCustomerRolesAsync(newCustomerRoles, new List<CustomerRole>());
        if (!string.IsNullOrEmpty(customerRolesError))
        {
            ModelState.AddModelError(string.Empty, customerRolesError);
        }

        // Ensure that valid email address is entered if Registered role is checked to avoid registered customers with empty email address
        if (newCustomerRoles.Any() && newCustomerRoles.FirstOrDefault(c => c.SystemName == CustomerDefaults.RegisteredRoleName) != null &&
            !CommonHelper.IsValidEmail(model.Email))
        {
            ModelState.AddModelError(string.Empty, await _localizationService.GetResourceAsync("Admin.Customers.Customers.ValidEmailRequiredRegisteredRole"));
        }

        //custom customer attributes
        var customerAttributesXml = await ParseCustomCustomerAttributesAsync(form);
        if (newCustomerRoles.Any() && newCustomerRoles.FirstOrDefault(c => c.SystemName == CustomerDefaults.RegisteredRoleName) != null)
        {
            var customerAttributeWarnings = await _customerAttributeParser.GetAttributeWarningsAsync(customerAttributesXml);
            foreach (var error in customerAttributeWarnings)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        if (ModelState.IsValid)
        {
            //fill entity from model
            var customer = model.ToEntity<Customer>();
            var currentSite = await _siteContext.GetCurrentSiteAsync();

            customer.CustomerGuid = Guid.NewGuid();
            customer.CreatedOnUtc = DateTime.UtcNow;
            customer.LastActivityDateUtc = DateTime.UtcNow;
            customer.RegisteredInSiteId = currentSite.Id;

            //form fields
            if (_dateTimeSettings.AllowCustomersToSetTimeZone)
                customer.TimeZoneId = model.TimeZoneId;
            if (_customerSettings.GenderEnabled)
                customer.Gender = model.Gender;
            if (_customerSettings.FirstNameEnabled)
                customer.FirstName = model.FirstName;
            if (_customerSettings.LastNameEnabled)
                customer.LastName = model.LastName;
            if (_customerSettings.DateOfBirthEnabled)
                customer.DateOfBirth = model.DateOfBirth;
            if (_customerSettings.CompanyEnabled)
                customer.Company = model.Company;
            if (_customerSettings.StreetAddressEnabled)
                customer.StreetAddress = model.StreetAddress;
            if (_customerSettings.StreetAddress2Enabled)
                customer.StreetAddress2 = model.StreetAddress2;
            if (_customerSettings.ZipPostalCodeEnabled)
                customer.ZipPostalCode = model.ZipPostalCode;
            if (_customerSettings.CityEnabled)
                customer.City = model.City;
            if (_customerSettings.CountyEnabled)
                customer.County = model.County;
            if (_customerSettings.CountryEnabled)
                customer.CountryId = model.CountryId;
            if (_customerSettings.CountryEnabled && _customerSettings.StateProvinceEnabled)
                customer.StateProvinceId = model.StateProvinceId;
            if (_customerSettings.PhoneEnabled)
                customer.Phone = model.Phone;
            if (_customerSettings.FaxEnabled)
                customer.Fax = model.Fax;
            customer.CustomCustomerAttributesXML = customerAttributesXml;

            await _customerService.InsertCustomerAsync(customer);


            //password
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var changePassRequest = new ChangePasswordRequest(model.Email, false, _customerSettings.DefaultPasswordFormat, model.Password);
                var changePassResult = await _customerRegistrationService.ChangePasswordAsync(changePassRequest);
                if (!changePassResult.Success)
                {
                }
            }

            //customer roles
            foreach (var customerRole in newCustomerRoles)
            {
                //ensure that the current customer cannot add to "Administrators" system role if he's not an admin himself
                if (customerRole.SystemName == CustomerDefaults.AdministratorsRoleName && !await _customerService.IsAdminAsync(await _workContext.GetCurrentCustomerAsync()))
                    continue;

                await _customerService.AddCustomerRoleMappingAsync(new CustomerCustomerRoleMapping { CustomerId = customer.Id, CustomerRoleId = customerRole.Id });
            }

            await _customerService.UpdateCustomerAsync(customer);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewCustomer",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewCustomer"), customer.Id), customer);
            // _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Customers.Customers.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = customer.Id });
        }

        //prepare model
        model = await _customerModelFactory.PrepareCustomerModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    public virtual async Task<IActionResult> Edit(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return AccessDeniedView();

        //try to get a customer with the specified id
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null || customer.Deleted)
            return RedirectToAction("List");

        //prepare model
        var model = await _customerModelFactory.PrepareCustomerModelAsync(null, customer);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [FormValueRequired("save", "save-continue")]
    public virtual async Task<IActionResult> Edit(CustomerModel model, bool continueEditing, IFormCollection form)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return AccessDeniedView();

        //try to get a customer with the specified id
        var customer = await _customerService.GetCustomerByIdAsync(model.Id);
        if (customer == null || customer.Deleted)
            return RedirectToAction("List");

        //validate customer roles
        var allCustomerRoles = await _customerService.GetAllCustomerRolesAsync(true);
        var newCustomerRoles = new List<CustomerRole>();
        foreach (var customerRole in allCustomerRoles)
            if (model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                newCustomerRoles.Add(customerRole);

        var customerRolesError = await ValidateCustomerRolesAsync(newCustomerRoles, await _customerService.GetCustomerRolesAsync(customer));

        if (!string.IsNullOrEmpty(customerRolesError))
        {
            ModelState.AddModelError(string.Empty, customerRolesError);
            // _notificationService.ErrorNotification(customerRolesError);
        }

        // Ensure that valid email address is entered if Registered role is checked to avoid registered customers with empty email address
        if (newCustomerRoles.Any() && newCustomerRoles.FirstOrDefault(c => c.SystemName == CustomerDefaults.RegisteredRoleName) != null &&
            !CommonHelper.IsValidEmail(model.Email))
        {
            ModelState.AddModelError(string.Empty, await _localizationService.GetResourceAsync("Admin.Customers.Customers.ValidEmailRequiredRegisteredRole"));
            // _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Customers.Customers.ValidEmailRequiredRegisteredRole"));
        }

        //custom customer attributes
        var customerAttributesXml = await ParseCustomCustomerAttributesAsync(form);
        if (newCustomerRoles.Any() && newCustomerRoles.FirstOrDefault(c => c.SystemName == CustomerDefaults.RegisteredRoleName) != null)
        {
            var customerAttributeWarnings = await _customerAttributeParser.GetAttributeWarningsAsync(customerAttributesXml);
            foreach (var error in customerAttributeWarnings)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        if (ModelState.IsValid)
        {
            try
            {
                customer.AdminComment = model.AdminComment;

                //prevent deactivation of the last active administrator
                if (!await _customerService.IsAdminAsync(customer) || model.Active || await SecondAdminAccountExistsAsync(customer))
                    customer.Active = model.Active;
                else
                    // _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Customers.Customers.AdminAccountShouldExists.Deactivate"));

                //email
                if (!string.IsNullOrWhiteSpace(model.Email))
                    await _customerRegistrationService.SetEmailAsync(customer, model.Email, false);
                else
                    customer.Email = model.Email;

                //username
                if (_customerSettings.UsernamesEnabled)
                {
                    if (!string.IsNullOrWhiteSpace(model.Username))
                        await _customerRegistrationService.SetUsernameAsync(customer, model.Username);
                    else
                        customer.Username = model.Username;
                }

                //form fields
                if (_dateTimeSettings.AllowCustomersToSetTimeZone)
                    customer.TimeZoneId = model.TimeZoneId;
                if (_customerSettings.GenderEnabled)
                    customer.Gender = model.Gender;
                if (_customerSettings.FirstNameEnabled)
                    customer.FirstName = model.FirstName;
                if (_customerSettings.LastNameEnabled)
                    customer.LastName = model.LastName;
                if (_customerSettings.DateOfBirthEnabled)
                    customer.DateOfBirth = model.DateOfBirth;
                if (_customerSettings.CompanyEnabled)
                    customer.Company = model.Company;
                if (_customerSettings.StreetAddressEnabled)
                    customer.StreetAddress = model.StreetAddress;
                if (_customerSettings.StreetAddress2Enabled)
                    customer.StreetAddress2 = model.StreetAddress2;
                if (_customerSettings.ZipPostalCodeEnabled)
                    customer.ZipPostalCode = model.ZipPostalCode;
                if (_customerSettings.CityEnabled)
                    customer.City = model.City;
                if (_customerSettings.CountyEnabled)
                    customer.County = model.County;
                if (_customerSettings.CountryEnabled)
                    customer.CountryId = model.CountryId;
                if (_customerSettings.CountryEnabled && _customerSettings.StateProvinceEnabled)
                    customer.StateProvinceId = model.StateProvinceId;
                if (_customerSettings.PhoneEnabled)
                    customer.Phone = model.Phone;
                if (_customerSettings.FaxEnabled)
                    customer.Fax = model.Fax;

                //custom customer attributes
                customer.CustomCustomerAttributesXML = customerAttributesXml;


                var currentCustomerRoleIds = await _customerService.GetCustomerRoleIdsAsync(customer, true);

                //customer roles
                foreach (var customerRole in allCustomerRoles)
                {
                    //ensure that the current customer cannot add/remove to/from "Administrators" system role
                    //if he's not an admin himself
                    if (customerRole.SystemName == CustomerDefaults.AdministratorsRoleName &&
                        !await _customerService.IsAdminAsync(await _workContext.GetCurrentCustomerAsync()))
                        continue;

                    if (model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                    {
                        //new role
                        if (currentCustomerRoleIds.All(roleId => roleId != customerRole.Id))
                            await _customerService.AddCustomerRoleMappingAsync(new CustomerCustomerRoleMapping { CustomerId = customer.Id, CustomerRoleId = customerRole.Id });
                    }
                    else
                    {
                        //prevent attempts to delete the administrator role from the user, if the user is the last active administrator
                        if (customerRole.SystemName == CustomerDefaults.AdministratorsRoleName && !await SecondAdminAccountExistsAsync(customer))
                        {
                            // _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Customers.Customers.AdminAccountShouldExists.DeleteRole"));
                            continue;
                        }

                        //remove role
                        if (currentCustomerRoleIds.Any(roleId => roleId == customerRole.Id))
                            await _customerService.RemoveCustomerRoleMappingAsync(customer, customerRole);
                    }
                }

                await _customerService.UpdateCustomerAsync(customer);

                //activity log
                await _customerActivityService.InsertActivityAsync("EditCustomer",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditCustomer"), customer.Id), customer);

                // _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Customers.Customers.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = customer.Id });
            }
            catch (Exception exc)
            {
                // _notificationService.ErrorNotification(exc.Message);
            }
        }

        //prepare model
        model = await _customerModelFactory.PrepareCustomerModelAsync(model, customer, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost, ActionName("Edit")]
    [FormValueRequired("changepassword")]
    public virtual async Task<IActionResult> ChangePassword(CustomerModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return AccessDeniedView();

        //try to get a customer with the specified id
        var customer = await _customerService.GetCustomerByIdAsync(model.Id);
        if (customer == null)
            return RedirectToAction("List");

        //ensure that the current customer cannot change passwords of "Administrators" if he's not an admin himself
        if (await _customerService.IsAdminAsync(customer) && !await _customerService.IsAdminAsync(await _workContext.GetCurrentCustomerAsync()))
        {
            // _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Customers.Customers.OnlyAdminCanChangePassword"));
            return RedirectToAction("Edit", new { id = customer.Id });
        }

        var changePassRequest = new ChangePasswordRequest(customer.Email,
            false, _customerSettings.DefaultPasswordFormat, model.Password);
        var changePassResult = await _customerRegistrationService.ChangePasswordAsync(changePassRequest);


        return RedirectToAction("Edit", new { id = customer.Id });
    }

    [HttpPost]
    public virtual async Task<IActionResult> RemoveBindMFA(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return AccessDeniedView();

        //try to get a customer with the specified id
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
            return RedirectToAction("List");

        await _genericAttributeService.SaveAttributeAsync(customer, CustomerDefaults.SelectedMultiFactorAuthenticationProviderAttribute, string.Empty);

        //raise event       
        await _eventPublisher.PublishAsync(new CustomerChangeMultiFactorAuthenticationProviderEvent(customer));

        // _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Customers.Customers.UnbindMFAProvider"));

        return RedirectToAction("Edit", new { id = customer.Id });
    }

    [HttpPost]
    public virtual async Task<IActionResult> Delete(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return AccessDeniedView();

        //try to get a customer with the specified id
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
            return RedirectToAction("List");

        try
        {
            //prevent attempts to delete the user, if it is the last active administrator
            if (await _customerService.IsAdminAsync(customer) && !await SecondAdminAccountExistsAsync(customer))
            {
                // _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Customers.Customers.AdminAccountShouldExists.DeleteAdministrator"));
                return RedirectToAction("Edit", new { id = customer.Id });
            }

            //ensure that the current customer cannot delete "Administrators" if he's not an admin himself
            if (await _customerService.IsAdminAsync(customer) && !await _customerService.IsAdminAsync(await _workContext.GetCurrentCustomerAsync()))
            {
                // _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Customers.Customers.OnlyAdminCanDeleteAdmin"));
                return RedirectToAction("Edit", new { id = customer.Id });
            }

            //delete
            await _customerService.DeleteCustomerAsync(customer);


            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteCustomer",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteCustomer"), customer.Id), customer);

            // _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Customers.Customers.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            // _notificationService.ErrorNotification(exc.Message);
            return RedirectToAction("Edit", new { id = customer.Id });
        }
    }

    [HttpPost, ActionName("Edit")]
    [FormValueRequired("impersonate")]
    public virtual async Task<IActionResult> Impersonate(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.AllowCustomerImpersonation))
            return AccessDeniedView();

        //try to get a customer with the specified id
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
            return RedirectToAction("List");

        if (!customer.Active)
        {
            return RedirectToAction("Edit", customer.Id);
        }

        //ensure that a non-admin user cannot impersonate as an administrator
        //otherwise, that user can simply impersonate as an administrator and gain additional administrative privileges
        var currentCustomer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsAdminAsync(currentCustomer) && await _customerService.IsAdminAsync(customer))
        {
            // _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Customers.Customers.NonAdminNotImpersonateAsAdminError"));
            return RedirectToAction("Edit", customer.Id);
        }

        //activity log
        await _customerActivityService.InsertActivityAsync("Impersonation.Started",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.Impersonation.Started.SiteOwner"), customer.Email, customer.Id), customer);
        await _customerActivityService.InsertActivityAsync(customer, "Impersonation.Started",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.Impersonation.Started.Customer"), currentCustomer.Email, currentCustomer.Id), currentCustomer);

        //ensure login is not required
        customer.RequireReLogin = false;
        await _customerService.UpdateCustomerAsync(customer);
        await _genericAttributeService.SaveAttributeAsync<int?>(currentCustomer, CustomerDefaults.ImpersonatedCustomerIdAttribute, customer.Id);

        return RedirectToAction("Index", "Home", new { area = string.Empty });
    }

    [HttpPost, ActionName("Edit")]
    [FormValueRequired("send-welcome-message")]
    public virtual async Task<IActionResult> SendWelcomeMessage(CustomerModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return AccessDeniedView();

        //try to get a customer with the specified id
        var customer = await _customerService.GetCustomerByIdAsync(model.Id);
        if (customer == null)
            return RedirectToAction("List");

        // _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Customers.Customers.SendWelcomeMessage.Success"));

        return RedirectToAction("Edit", new { id = customer.Id });
    }

    [HttpPost, ActionName("Edit")]
    [FormValueRequired("resend-activation-message")]
    public virtual async Task<IActionResult> ReSendActivationMessage(CustomerModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return AccessDeniedView();

        //try to get a customer with the specified id
        var customer = await _customerService.GetCustomerByIdAsync(model.Id);
        if (customer == null)
            return RedirectToAction("List");

        //email validation message
        await _genericAttributeService.SaveAttributeAsync(customer, CustomerDefaults.AccountActivationTokenAttribute, Guid.NewGuid().ToString());
        // await _workflowMessageService.SendCustomerEmailValidationMessageAsync(customer, (await _workContext.GetWorkingLanguageAsync()).Id);

        // _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Customers.Customers.ReSendActivationMessage.Success"));

        return RedirectToAction("Edit", new { id = customer.Id });
    }

    #endregion

    #region Customer

    public virtual async Task<IActionResult> LoadCustomerStatistics(string period)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return Content(string.Empty);

        var result = new List<object>();

        var nowDt = await _dateTimeHelper.ConvertToUserTimeAsync(DateTime.Now);
        var timeZone = await _dateTimeHelper.GetCurrentTimeZoneAsync();
        var searchCustomerRoleIds = new[] { (await _customerService.GetCustomerRoleBySystemNameAsync(CustomerDefaults.RegisteredRoleName)).Id };

        var culture = new CultureInfo((await _workContext.GetWorkingLanguageAsync()).LanguageCulture);

        switch (period)
        {
            case "year":
                //year statistics
                var yearAgoDt = nowDt.AddYears(-1).AddMonths(1);
                var searchYearDateUser = new DateTime(yearAgoDt.Year, yearAgoDt.Month, 1);
                for (var i = 0; i <= 12; i++)
                {
                    result.Add(new
                    {
                        date = searchYearDateUser.Date.ToString("Y", culture),
                        value = (await _customerService.GetAllCustomersAsync(
                            createdFromUtc: _dateTimeHelper.ConvertToUtcTime(searchYearDateUser, timeZone),
                            createdToUtc: _dateTimeHelper.ConvertToUtcTime(searchYearDateUser.AddMonths(1), timeZone),
                            customerRoleIds: searchCustomerRoleIds,
                            pageIndex: 0,
                            pageSize: 1, getOnlyTotalCount: true)).TotalCount.ToString()
                    });

                    searchYearDateUser = searchYearDateUser.AddMonths(1);
                }

                break;
            case "month":
                //month statistics
                var monthAgoDt = nowDt.AddDays(-30);
                var searchMonthDateUser = new DateTime(monthAgoDt.Year, monthAgoDt.Month, monthAgoDt.Day);
                for (var i = 0; i <= 30; i++)
                {
                    result.Add(new
                    {
                        date = searchMonthDateUser.Date.ToString("M", culture),
                        value = (await _customerService.GetAllCustomersAsync(
                            createdFromUtc: _dateTimeHelper.ConvertToUtcTime(searchMonthDateUser, timeZone),
                            createdToUtc: _dateTimeHelper.ConvertToUtcTime(searchMonthDateUser.AddDays(1), timeZone),
                            customerRoleIds: searchCustomerRoleIds,
                            pageIndex: 0,
                            pageSize: 1, getOnlyTotalCount: true)).TotalCount.ToString()
                    });

                    searchMonthDateUser = searchMonthDateUser.AddDays(1);
                }

                break;
            case "week":
            default:
                //week statistics
                var weekAgoDt = nowDt.AddDays(-7);
                var searchWeekDateUser = new DateTime(weekAgoDt.Year, weekAgoDt.Month, weekAgoDt.Day);
                for (var i = 0; i <= 7; i++)
                {
                    result.Add(new
                    {
                        date = searchWeekDateUser.Date.ToString("d dddd", culture),
                        value = (await _customerService.GetAllCustomersAsync(
                            createdFromUtc: _dateTimeHelper.ConvertToUtcTime(searchWeekDateUser, timeZone),
                            createdToUtc: _dateTimeHelper.ConvertToUtcTime(searchWeekDateUser.AddDays(1), timeZone),
                            customerRoleIds: searchCustomerRoleIds,
                            pageIndex: 0,
                            pageSize: 1, getOnlyTotalCount: true)).TotalCount.ToString()
                    });

                    searchWeekDateUser = searchWeekDateUser.AddDays(1);
                }

                break;
        }

        return Json(result);
    }

    #endregion

    #region Activity log

    [HttpPost]
    public virtual async Task<IActionResult> ListActivityLog(CustomerActivityLogSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return await AccessDeniedDataTablesJson();

        //try to get a customer with the specified id
        var customer = await _customerService.GetCustomerByIdAsync(searchModel.CustomerId)
            ?? throw new ArgumentException("No customer found with the specified id");

        //prepare model
        var model = await _customerModelFactory.PrepareCustomerActivityLogListModelAsync(searchModel, customer);

        return Json(model);
    }

    #endregion
}