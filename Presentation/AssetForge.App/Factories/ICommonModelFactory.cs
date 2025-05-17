using AssetForge.App.Models.Common;

namespace AssetForge.App.Factories;

/// <summary>
/// Represents the interface of the common models factory
/// </summary>
public partial interface ICommonModelFactory
{
    Task<LogoModel> PrepareLogoModelAsync();
    //Task<LanguageSelectorModel> PrepareLanguageSelectorModelAsync();
    //Task<CurrencySelectorModel> PrepareCurrencySelectorModelAsync();
    //Task<TaxTypeSelectorModel> PrepareTaxTypeSelectorModelAsync();
    Task<HeaderLinksModel> PrepareHeaderLinksModelAsync();
    Task<AdminHeaderLinksModel> PrepareAdminHeaderLinksModelAsync();
    //Task<SocialModel> PrepareSocialModelAsync();
    //Task<FooterModel> PrepareFooterModelAsync();
    //Task<ContactUsModel> PrepareContactUsModelAsync(ContactUsModel model, bool excludeProperties);
    //Task<ContactVendorModel> PrepareContactVendorModelAsync(ContactVendorModel model, Vendor vendor, bool excludeProperties);
    //Task<SiteThemeSelectorModel> PrepareSiteThemeSelectorModelAsync();
    //Task<FaviconAndAppIconsModel> PrepareFaviconAndAppIconsModelAsync();
    Task<string> PrepareRobotsTextFileAsync();

    //Task<List<PageMenuModel>> PreparePageMenuModelsAsync(int parentPageId, bool loadChildPages = true);

    //Task<List<PageMenuModel>> PreparePageMenuModelsAsync();

    //Task<TopMenuModel> PrepareTopMenuModelAsync();

}