using AssetForge.Core;
using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Models.Customer;

public partial record CustomerInfoModel : BaseModel
{
    public CustomerInfoModel()
    {
        AvailableTimeZones = new List<SelectListItem>();
        AvailableCountries = new List<SelectListItem>();
        AvailableStates = new List<SelectListItem>();
        AssociatedExternalAuthRecords = new List<AssociatedExternalAuthModel>();
        CustomerAttributes = new List<CustomerAttributeModel>();
    }

    [DataType(DataType.EmailAddress)]
    [ResourceDisplayName("Account.Fields.Email")]
    public string Email { get; set; }
    [DataType(DataType.EmailAddress)]
    [ResourceDisplayName("Account.Fields.EmailToRevalidate")]
    public string EmailToRevalidate { get; set; }

    public bool CheckUsernameAvailabilityEnabled { get; set; }
    public bool AllowUsersToChangeUsernames { get; set; }
    public bool UsernamesEnabled { get; set; }
    [ResourceDisplayName("Account.Fields.Username")]
    public string Username { get; set; }

    //form fields & properties
    public bool GenderEnabled { get; set; }
    [ResourceDisplayName("Account.Fields.Gender")]
    public string Gender { get; set; }

    public bool NeutralGenderEnabled { get; set; }

    public bool FirstNameEnabled { get; set; }
    [ResourceDisplayName("Account.Fields.FirstName")]
    public string FirstName { get; set; }
    public bool FirstNameRequired { get; set; }
    public bool LastNameEnabled { get; set; }
    [ResourceDisplayName("Account.Fields.LastName")]
    public string LastName { get; set; }
    public bool LastNameRequired { get; set; }

    public bool DateOfBirthEnabled { get; set; }
    [ResourceDisplayName("Account.Fields.DateOfBirth")]
    public int? DateOfBirthDay { get; set; }
    [ResourceDisplayName("Account.Fields.DateOfBirth")]
    public int? DateOfBirthMonth { get; set; }
    [ResourceDisplayName("Account.Fields.DateOfBirth")]
    public int? DateOfBirthYear { get; set; }
    public bool DateOfBirthRequired { get; set; }
    public DateTime? ParseDateOfBirth()
    {
        return CommonHelper.ParseDate(DateOfBirthYear, DateOfBirthMonth, DateOfBirthDay);
    }

    public bool CompanyEnabled { get; set; }
    public bool CompanyRequired { get; set; }
    [ResourceDisplayName("Account.Fields.Company")]
    public string Company { get; set; }
    public bool StreetAddressEnabled { get; set; }
    public bool StreetAddressRequired { get; set; }
    [ResourceDisplayName("Account.Fields.StreetAddress")]
    public string StreetAddress { get; set; }

    public bool StreetAddress2Enabled { get; set; }
    public bool StreetAddress2Required { get; set; }
    [ResourceDisplayName("Account.Fields.StreetAddress2")]
    public string StreetAddress2 { get; set; }

    public bool ZipPostalCodeEnabled { get; set; }
    public bool ZipPostalCodeRequired { get; set; }
    [ResourceDisplayName("Account.Fields.ZipPostalCode")]
    public string ZipPostalCode { get; set; }

    public bool CityEnabled { get; set; }
    public bool CityRequired { get; set; }
    [ResourceDisplayName("Account.Fields.City")]
    public string City { get; set; }

    public bool CountyEnabled { get; set; }
    public bool CountyRequired { get; set; }
    [ResourceDisplayName("Account.Fields.County")]
    public string County { get; set; }

    public bool CountryEnabled { get; set; }
    public bool CountryRequired { get; set; }
    [ResourceDisplayName("Account.Fields.Country")]
    public int CountryId { get; set; }
    public IList<SelectListItem> AvailableCountries { get; set; }

    public bool StateProvinceEnabled { get; set; }
    public bool StateProvinceRequired { get; set; }
    [ResourceDisplayName("Account.Fields.StateProvince")]
    public int StateProvinceId { get; set; }
    public IList<SelectListItem> AvailableStates { get; set; }

    public bool PhoneEnabled { get; set; }
    public bool PhoneRequired { get; set; }
    [DataType(DataType.PhoneNumber)]
    [ResourceDisplayName("Account.Fields.Phone")]
    public string Phone { get; set; }

    public bool FaxEnabled { get; set; }
    public bool FaxRequired { get; set; }
    [DataType(DataType.PhoneNumber)]
    [ResourceDisplayName("Account.Fields.Fax")]
    public string Fax { get; set; }

    public bool NewsletterEnabled { get; set; }
    [ResourceDisplayName("Account.Fields.Newsletter")]
    public bool Newsletter { get; set; }

    //preferences
    public bool SignatureEnabled { get; set; }
    [ResourceDisplayName("Account.Fields.Signature")]
    public string Signature { get; set; }

    //time zone
    [ResourceDisplayName("Account.Fields.TimeZone")]
    public string TimeZoneId { get; set; }
    public bool AllowCustomersToSetTimeZone { get; set; }
    public IList<SelectListItem> AvailableTimeZones { get; set; }

    //external authentication
    [ResourceDisplayName("Account.AssociatedExternalAuth")]
    public IList<AssociatedExternalAuthModel> AssociatedExternalAuthRecords { get; set; }
    public int NumberOfExternalAuthenticationProviders { get; set; }
    public bool AllowCustomersToRemoveAssociations { get; set; }

    public IList<CustomerAttributeModel> CustomerAttributes { get; set; }

    #region Nested classes

    public partial record AssociatedExternalAuthModel : BaseEntityModel
    {
        public string Email { get; set; }

        public string ExternalIdentifier { get; set; }

        public string AuthMethodName { get; set; }
    }

    #endregion
}