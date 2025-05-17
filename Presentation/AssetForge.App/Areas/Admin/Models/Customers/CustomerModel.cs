using AssetForge.Core.Domain.Attributes;
using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a customer model
/// </summary>
public partial record CustomerModel : BaseEntityModel, IAclSupportedModel
{
    #region Ctor

    public CustomerModel()
    {
        AvailableTimeZones = new List<SelectListItem>();
        SendEmail = new SendEmailModel() { SendImmediately = true };
        SendPm = new SendPmModel();

        SelectedCustomerRoleIds = new List<int>();
        AvailableCustomerRoles = new List<SelectListItem>();

        AvailableCountries = new List<SelectListItem>();
        AvailableStates = new List<SelectListItem>();
        CustomerAttributes = new List<CustomerAttributeModel>();
        AvailableNewsletterSubscriptionSites = new List<SelectListItem>();
        SelectedNewsletterSubscriptionSiteIds = new List<int>();
        CustomerActivityLogSearchModel = new CustomerActivityLogSearchModel();
        CustomerAssociatedExternalAuthRecordsSearchModel = new CustomerAssociatedExternalAuthRecordsSearchModel();
    }

    #endregion

    #region Properties

    public bool UsernamesEnabled { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.Username")]
    public string Username { get; set; }

    [DataType(DataType.EmailAddress)]
    [ResourceDisplayName("Admin.Customers.Customers.Fields.Email")]
    public string Email { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    //form fields & properties
    public bool GenderEnabled { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.Gender")]
    public string Gender { get; set; }

    public bool NeutralGenderEnabled { get; set; }

    public bool FirstNameEnabled { get; set; }
    [ResourceDisplayName("Admin.Customers.Customers.Fields.FirstName")]
    public string FirstName { get; set; }

    public bool LastNameEnabled { get; set; }
    [ResourceDisplayName("Admin.Customers.Customers.Fields.LastName")]
    public string LastName { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.FullName")]
    public string FullName { get; set; }

    public bool DateOfBirthEnabled { get; set; }

    [UIHint("DateNullable")]
    [ResourceDisplayName("Admin.Customers.Customers.Fields.DateOfBirth")]
    public DateTime? DateOfBirth { get; set; }

    public bool CompanyEnabled { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.Company")]
    public string Company { get; set; }

    public bool StreetAddressEnabled { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.StreetAddress")]
    public string StreetAddress { get; set; }

    public bool StreetAddress2Enabled { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.StreetAddress2")]
    public string StreetAddress2 { get; set; }

    public bool ZipPostalCodeEnabled { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.ZipPostalCode")]
    public string ZipPostalCode { get; set; }

    public bool CityEnabled { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.City")]
    public string City { get; set; }

    public bool CountyEnabled { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.County")]
    public string County { get; set; }

    public bool CountryEnabled { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.Country")]
    public int CountryId { get; set; }

    public IList<SelectListItem> AvailableCountries { get; set; }

    public bool StateProvinceEnabled { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.StateProvince")]
    public int StateProvinceId { get; set; }

    public IList<SelectListItem> AvailableStates { get; set; }

    public bool PhoneEnabled { get; set; }

    [DataType(DataType.PhoneNumber)]
    [ResourceDisplayName("Admin.Customers.Customers.Fields.Phone")]
    public string Phone { get; set; }

    public bool FaxEnabled { get; set; }

    [DataType(DataType.PhoneNumber)]
    [ResourceDisplayName("Admin.Customers.Customers.Fields.Fax")]
    public string Fax { get; set; }

    public List<CustomerAttributeModel> CustomerAttributes { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.RegisteredInSite")]
    public string RegisteredInSite { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.AdminComment")]
    public string AdminComment { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.Active")]
    public bool Active { get; set; }

    //time zone
    [ResourceDisplayName("Admin.Customers.Customers.Fields.TimeZoneId")]
    public string TimeZoneId { get; set; }

    public bool AllowCustomersToSetTimeZone { get; set; }

    public IList<SelectListItem> AvailableTimeZones { get; set; }


    public bool DisplayRegisteredInSite { get; set; }

    //registration date
    [ResourceDisplayName("Admin.Customers.Customers.Fields.CreatedOn")]
    public DateTime CreatedOn { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.LastActivityDate")]
    public DateTime LastActivityDate { get; set; }

    //IP address
    [ResourceDisplayName("Admin.Customers.Customers.Fields.IPAddress")]
    public string LastIpAddress { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.LastVisitedPage")]
    public string LastVisitedPage { get; set; }

    //customer roles
    [ResourceDisplayName("Admin.Customers.Customers.Fields.CustomerRoles")]
    public string CustomerRoleNames { get; set; }

    //binding with multi-factor authentication provider
    [ResourceDisplayName("Admin.Customers.Customers.Fields.MultiFactorAuthenticationProvider")]
    public string MultiFactorAuthenticationProvider { get; set; }

    public IList<SelectListItem> AvailableCustomerRoles { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.CustomerRoles")]
    public IList<int> SelectedCustomerRoleIds { get; set; }

    //newsletter subscriptions (per site)
    [ResourceDisplayName("Admin.Customers.Customers.Fields.Newsletter")]
    public IList<SelectListItem> AvailableNewsletterSubscriptionSites { get; set; }

    [ResourceDisplayName("Admin.Customers.Customers.Fields.Newsletter")]
    public IList<int> SelectedNewsletterSubscriptionSiteIds { get; set; }

    //send email model
    public SendEmailModel SendEmail { get; set; }

    //send PM model
    public SendPmModel SendPm { get; set; }

    //send a private message
    public bool AllowSendingOfPrivateMessage { get; set; }

    //send the welcome message
    public bool AllowSendingOfWelcomeMessage { get; set; }

    //re-send the activation message
    public bool AllowReSendingOfActivationMessage { get; set; }

    //GDPR enabled
    public bool GdprEnabled { get; set; }

    public string AvatarUrl { get; internal set; }

    public CustomerActivityLogSearchModel CustomerActivityLogSearchModel { get; set; }

    public CustomerAssociatedExternalAuthRecordsSearchModel CustomerAssociatedExternalAuthRecordsSearchModel { get; set; }

    #endregion

    #region Nested classes

    public partial record SendEmailModel : BaseModel
    {
        [ResourceDisplayName("Admin.Customers.Customers.SendEmail.Subject")]
        public string Subject { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.SendEmail.Body")]
        public string Body { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.SendEmail.SendImmediately")]
        public bool SendImmediately { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.SendEmail.DontSendBeforeDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? DontSendBeforeDate { get; set; }
    }

    public partial record SendPmModel : BaseModel
    {
        [ResourceDisplayName("Admin.Customers.Customers.SendPM.Subject")]
        public string Subject { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.SendPM.Message")]
        public string Message { get; set; }
    }

    public partial record CustomerAttributeModel : BaseEntityModel
    {
        public CustomerAttributeModel()
        {
            Values = new List<CustomerAttributeValueModel>();
        }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// Default value for textboxes
        /// </summary>
        public string DefaultValue { get; set; }

        public AttributeControlType AttributeControlType { get; set; }

        public IList<CustomerAttributeValueModel> Values { get; set; }
    }

    public partial record CustomerAttributeValueModel : BaseEntityModel
    {
        public string Name { get; set; }

        public bool IsPreSelected { get; set; }
    }

    #endregion
}