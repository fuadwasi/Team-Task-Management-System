using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a customer settings model
/// </summary>
public partial record CustomerSettingsModel : BaseModel, ISettingsModel
{
    #region Ctor

    public CustomerSettingsModel()
    {
        AvailableCountries = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.UsernamesEnabled")]
    public bool UsernamesEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AllowUsersToChangeUsernames")]
    public bool AllowUsersToChangeUsernames { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CheckUsernameAvailabilityEnabled")]
    public bool CheckUsernameAvailabilityEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.UsernameValidationEnabled")]
    public bool UsernameValidationEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.UsernameValidationUseRegex")]
    public bool UsernameValidationUseRegex { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.UsernameValidationRule")]
    public string UsernameValidationRule { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.UserRegistrationType")]
    public int UserRegistrationType { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AllowCustomersToUploadAvatars")]
    public bool AllowCustomersToUploadAvatars { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.DefaultAvatarEnabled")]
    public bool DefaultAvatarEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.ShowCustomersLocation")]
    public bool ShowCustomersLocation { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.ShowCustomersJoinDate")]
    public bool ShowCustomersJoinDate { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AllowViewingProfiles")]
    public bool AllowViewingProfiles { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.NotifyNewCustomerRegistration")]
    public bool NotifyNewCustomerRegistration { get; set; }

    //[ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.RequireRegistrationForDownloadableProducts")]
    //public bool RequireRegistrationForDownloadableProducts { get; set; }

    //[ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AllowCustomersToCheckGiftCardBalance")]
    //public bool AllowCustomersToCheckGiftCardBalance { get; set; }

    //[ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.HideDownloadableProductsTab")]
    //public bool HideDownloadableProductsTab { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.HideBackInStockSubscriptionsTab")]
    public bool HideBackInStockSubscriptionsTab { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CustomerNameFormat")]
    public int CustomerNameFormat { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PasswordMinLength")]
    public int PasswordMinLength { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PasswordMaxLength")]
    public int PasswordMaxLength { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PasswordRequireLowercase")]
    public bool PasswordRequireLowercase { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PasswordRequireUppercase")]
    public bool PasswordRequireUppercase { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PasswordRequireNonAlphanumeric")]
    public bool PasswordRequireNonAlphanumeric { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PasswordRequireDigit")]
    public bool PasswordRequireDigit { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.UnduplicatedPasswordsNumber")]
    public int UnduplicatedPasswordsNumber { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PasswordRecoveryLinkDaysValid")]
    public int PasswordRecoveryLinkDaysValid { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.DefaultPasswordFormat")]
    public int DefaultPasswordFormat { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PasswordLifetime")]
    public int PasswordLifetime { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.FailedPasswordAllowedAttempts")]
    public int FailedPasswordAllowedAttempts { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.FailedPasswordLockoutMinutes")]
    public int FailedPasswordLockoutMinutes { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.NewsletterEnabled")]
    public bool NewsletterEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.NewsletterTickedByDefault")]
    public bool NewsletterTickedByDefault { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.HideNewsletterBlock")]
    public bool HideNewsletterBlock { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.NewsletterBlockAllowToUnsubscribe")]
    public bool NewsletterBlockAllowToUnsubscribe { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.SiteLastVisitedPage")]
    public bool SiteLastVisitedPage { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.SiteIpAddresses")]
    public bool SiteIpAddresses { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.EnteringEmailTwice")]
    public bool EnteringEmailTwice { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.GenderEnabled")]
    public bool GenderEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.NeutralGenderEnabled")]
    public bool NeutralGenderEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.FirstNameEnabled")]
    public bool FirstNameEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.FirstNameRequired")]
    public bool FirstNameRequired { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.LastNameEnabled")]
    public bool LastNameEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.LastNameRequired")]
    public bool LastNameRequired { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.DateOfBirthEnabled")]
    public bool DateOfBirthEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.DateOfBirthRequired")]
    public bool DateOfBirthRequired { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.DateOfBirthMinimumAge")]
    [UIHint("Int32Nullable")]
    public int? DateOfBirthMinimumAge { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CompanyEnabled")]
    public bool CompanyEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CompanyRequired")]
    public bool CompanyRequired { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.StreetAddressEnabled")]
    public bool StreetAddressEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.StreetAddressRequired")]
    public bool StreetAddressRequired { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.StreetAddress2Enabled")]
    public bool StreetAddress2Enabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.StreetAddress2Required")]
    public bool StreetAddress2Required { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.ZipPostalCodeEnabled")]
    public bool ZipPostalCodeEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.ZipPostalCodeRequired")]
    public bool ZipPostalCodeRequired { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CityEnabled")]
    public bool CityEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CityRequired")]
    public bool CityRequired { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CountyEnabled")]
    public bool CountyEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CountyRequired")]
    public bool CountyRequired { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CountryEnabled")]
    public bool CountryEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CountryRequired")]
    public bool CountryRequired { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.DefaultCountry")]
    public int? DefaultCountryId { get; set; }
    public IList<SelectListItem> AvailableCountries { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.StateProvinceEnabled")]
    public bool StateProvinceEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.StateProvinceRequired")]
    public bool StateProvinceRequired { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PhoneEnabled")]
    public bool PhoneEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PhoneRequired")]
    public bool PhoneRequired { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PhoneNumberValidationEnabled")]
    public bool PhoneNumberValidationEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PhoneNumberValidationUseRegex")]
    public bool PhoneNumberValidationUseRegex { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PhoneNumberValidationRule")]
    public string PhoneNumberValidationRule { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.FaxEnabled")]
    public bool FaxEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.FaxRequired")]
    public bool FaxRequired { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AcceptPrivacyPolicyEnabled")]
    public bool AcceptPrivacyPolicyEnabled { get; set; }

    #endregion
}