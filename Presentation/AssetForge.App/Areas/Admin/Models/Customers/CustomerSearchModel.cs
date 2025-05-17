using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a customer search model
/// </summary>
public partial record CustomerSearchModel : BaseSearchModel, IAclSupportedModel
{
    #region Ctor

    public CustomerSearchModel()
    {
        SelectedCustomerRoleIds = new List<int>();
        AvailableCustomerRoles = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.Customers.Customers.List.CustomerRoles")]
    public IList<int> SelectedCustomerRoleIds { get; set; }

    public IList<SelectListItem> AvailableCustomerRoles { get; set; }

    [DataType(DataType.EmailAddress)]
    [ResourceDisplayName("Admin.Customers.Customers.List.SearchEmail")]
    public string SearchEmail { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.List.SearchUsername")]
    public string SearchUsername { get; set; }

    public bool UsernamesEnabled { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.List.SearchFirstName")]
    public string SearchFirstName { get; set; }
    public bool FirstNameEnabled { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.List.SearchLastName")]
    public string SearchLastName { get; set; }
    public bool LastNameEnabled { get; set; }

    [UIHint("DateNullable")]
    [ResourceDisplayName("Admin.Customers.Customers.List.SearchLastActivityFrom")]
    public DateTime? SearchLastActivityFrom { get; set; }

    [UIHint("DateNullable")]
    [ResourceDisplayName("Admin.Customers.Customers.List.SearchLastActivityTo")]
    public DateTime? SearchLastActivityTo { get; set; }

    [UIHint("DateNullable")]
    [ResourceDisplayName("Admin.Customers.Customers.List.SearchRegistrationDateFrom")]
    public DateTime? SearchRegistrationDateFrom { get; set; }

    [UIHint("DateNullable")]
    [ResourceDisplayName("Admin.Customers.Customers.List.SearchRegistrationDateTo")]
    public DateTime? SearchRegistrationDateTo { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.List.SearchDateOfBirth")]
    public string SearchDayOfBirth { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.List.SearchDateOfBirth")]
    public string SearchMonthOfBirth { get; set; }

    public bool DateOfBirthEnabled { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.List.SearchCompany")]
    public string SearchCompany { get; set; }

    public bool CompanyEnabled { get; set; }

    [DataType(DataType.PhoneNumber)]
    [ResourceDisplayName("Admin.Customers.Customers.List.SearchPhone")]
    public string SearchPhone { get; set; }

    public bool PhoneEnabled { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.List.SearchZipCode")]
    public string SearchZipPostalCode { get; set; }

    public bool ZipPostalCodeEnabled { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.List.SearchIpAddress")]
    public string SearchIpAddress { get; set; }

    public bool AvatarEnabled { get; internal set; }

    #endregion
}