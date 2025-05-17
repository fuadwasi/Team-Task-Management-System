//using AssetForge.Core.Caching;
//using AssetForge.Core.Domain.Blogs;
//using AssetForge.Core.Domain.Catalog;
//using AssetForge.Core.Domain.Configuration;
//using AssetForge.Core.Domain.Localization;
//using AssetForge.Core.Domain.Media;
//
//using AssetForge.Core.Domain.Orders;
//using AssetForge.Core.Domain.Polls;
//
//using AssetForge.Core.Domain.Vendors;
//using AssetForge.Core.Events;
//using AssetForge.Services.Cms;
//using AssetForge.Services.Events;
//using AssetForge.Services.Plugins;
//using AssetForge.Web.Framework.Models.Cms;

//namespace AssetForge.Web.Infrastructure.Cache;

///// <summary>
///// Model cache event consumer (used for caching of presentation layer models)
///// </summary>
//public partial class ModelCacheEventConsumer :
//    //languages
//    IConsumer<EntityInsertedEvent<Language>>,
//    IConsumer<EntityUpdatedEvent<Language>>,
//    IConsumer<EntityDeletedEvent<Language>>,
//    //settings
//    IConsumer<EntityUpdatedEvent<Setting>>,
//    //manufacturers
//    IConsumer<EntityInsertedEvent<Manufacturer>>,
//    IConsumer<EntityUpdatedEvent<Manufacturer>>,
//    IConsumer<EntityDeletedEvent<Manufacturer>>,
//    //vendors
//    IConsumer<EntityInsertedEvent<Vendor>>,
//    IConsumer<EntityUpdatedEvent<Vendor>>,
//    IConsumer<EntityDeletedEvent<Vendor>>,
//    //categories
//    IConsumer<EntityInsertedEvent<Category>>,
//    IConsumer<EntityUpdatedEvent<Category>>,
//    IConsumer<EntityDeletedEvent<Category>>,
//    //product categories
//    IConsumer<EntityInsertedEvent<ProductCategory>>,
//    IConsumer<EntityDeletedEvent<ProductCategory>>,
//    //products
//    IConsumer<EntityInsertedEvent<Product>>,
//    IConsumer<EntityUpdatedEvent<Product>>,
//    IConsumer<EntityDeletedEvent<Product>>,
//    //product tags
//    IConsumer<EntityInsertedEvent<ProductTag>>,
//    IConsumer<EntityUpdatedEvent<ProductTag>>,
//    IConsumer<EntityDeletedEvent<ProductTag>>,
//    //Product attribute values
//    IConsumer<EntityUpdatedEvent<ProductAttributeValue>>,
//    //Pages
//    IConsumer<EntityInsertedEvent<Page>>,
//    IConsumer<EntityUpdatedEvent<Page>>,
//    IConsumer<EntityDeletedEvent<Page>>,
//    //Orders
//    IConsumer<EntityInsertedEvent<Order>>,
//    IConsumer<EntityUpdatedEvent<Order>>,
//    IConsumer<EntityDeletedEvent<Order>>,
//    //Picture
//    IConsumer<EntityInsertedEvent<Picture>>,
//    IConsumer<EntityUpdatedEvent<Picture>>,
//    IConsumer<EntityDeletedEvent<Picture>>,
//    //Product picture mapping
//    IConsumer<EntityInsertedEvent<ProductPicture>>,
//    IConsumer<EntityUpdatedEvent<ProductPicture>>,
//    IConsumer<EntityDeletedEvent<ProductPicture>>,
//    //Product review
//    IConsumer<EntityDeletedEvent<ProductReview>>,
//    //polls
//    IConsumer<EntityInsertedEvent<Poll>>,
//    IConsumer<EntityUpdatedEvent<Poll>>,
//    IConsumer<EntityDeletedEvent<Poll>>,
//    //blog posts
//    IConsumer<EntityInsertedEvent<BlogPost>>,
//    IConsumer<EntityUpdatedEvent<BlogPost>>,
//    IConsumer<EntityDeletedEvent<BlogPost>>,
//    //news items
//    IConsumer<EntityInsertedEvent<NewsItem>>,
//    IConsumer<EntityUpdatedEvent<NewsItem>>,
//    IConsumer<EntityDeletedEvent<NewsItem>>,
//    //shopping cart items
//    IConsumer<EntityUpdatedEvent<ShoppingCartItem>>,
//    //plugins
//    IConsumer<PluginUpdatedEvent>
//{
//    #region Fields

//    protected readonly CatalogSettings _catalogSettings;
//    protected readonly IStaticCacheManager _staticCacheManager;

//    #endregion

//    #region Ctor

//    public ModelCacheEventConsumer(CatalogSettings catalogSettings, IStaticCacheManager staticCacheManager)
//    {
//        _staticCacheManager = staticCacheManager;
//        _catalogSettings = catalogSettings;
//    }

//    #endregion

//    #region Methods

//    #region Languages

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityInsertedEvent<Language> eventMessage)
//    {
//        //clear all localizable models
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ManufacturerNavigationPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryAllPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryXmlAllPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<Language> eventMessage)
//    {
//        //clear all localizable models
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ManufacturerNavigationPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryAllPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryXmlAllPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityDeletedEvent<Language> eventMessage)
//    {
//        //clear all localizable models
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ManufacturerNavigationPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryAllPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryXmlAllPrefixCacheKey);
//    }

//    #endregion

//    #region Setting

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<Setting> eventMessage)
//    {
//        //clear models which depend on settings
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ManufacturerNavigationPrefixCacheKey); //depends on CatalogSettings.ManufacturersBlockItemsToDisplay
//        await _staticCacheManager.RemoveAsync(AssetForgeModelCacheDefaults.VendorNavigationModelKey); //depends on VendorSettings.VendorBlockItemsToDisplay
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryAllPrefixCacheKey); //depends on CatalogSettings.ShowCategoryProductNumber and CatalogSettings.ShowCategoryProductNumberIncludingSubcategories
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryXmlAllPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.HomepageBestsellersIdsPrefixCacheKey); //depends on CatalogSettings.NumberOfBestsellersOnHomepage
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductsAlsoPurchasedIdsPrefixCacheKey); //depends on CatalogSettings.ProductsAlsoPurchasedNumber
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.BlogPrefixCacheKey); //depends on BlogSettings.NumberOfTags
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.NewsPrefixCacheKey); //depends on NewsSettings.MainPageNewsCount
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey); //depends on distinct sitemap settings
//        await _staticCacheManager.RemoveByPrefixAsync(WidgetModelDefaults.WidgetPrefixCacheKey); //depends on WidgetSettings and certain settings of widgets
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SiteLogoPathPrefixCacheKey); //depends on SiteInformationSettings.LogoPictureId
//    }

//    #endregion

//    #region Vendors

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityInsertedEvent<Vendor> eventMessage)
//    {
//        await _staticCacheManager.RemoveAsync(AssetForgeModelCacheDefaults.VendorNavigationModelKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<Vendor> eventMessage)
//    {
//        await _staticCacheManager.RemoveAsync(AssetForgeModelCacheDefaults.VendorNavigationModelKey);
//        await _staticCacheManager.RemoveByPrefixAsync(string.Format(AssetForgeModelCacheDefaults.VendorPicturePrefixCacheKeyById, eventMessage.Entity.Id));
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityDeletedEvent<Vendor> eventMessage)
//    {
//        await _staticCacheManager.RemoveAsync(AssetForgeModelCacheDefaults.VendorNavigationModelKey);
//    }

//    #endregion

//    #region  Manufacturers

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityInsertedEvent<Manufacturer> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ManufacturerNavigationPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<Manufacturer> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ManufacturerNavigationPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(string.Format(AssetForgeModelCacheDefaults.ManufacturerPicturePrefixCacheKeyById, eventMessage.Entity.Id));
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityDeletedEvent<Manufacturer> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ManufacturerNavigationPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    #endregion

//    #region Categories

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityInsertedEvent<Category> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryAllPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryXmlAllPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryHomepagePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<Category> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryAllPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryXmlAllPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryHomepagePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(string.Format(AssetForgeModelCacheDefaults.CategoryPicturePrefixCacheKeyById, eventMessage.Entity.Id));
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityDeletedEvent<Category> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryAllPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryXmlAllPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryHomepagePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    #endregion

//    #region Product categories

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityInsertedEvent<ProductCategory> eventMessage)
//    {
//        if (_catalogSettings.ShowCategoryProductNumber)
//        {
//            //depends on CatalogSettings.ShowCategoryProductNumber (when enabled)
//            //so there's no need to clear this cache in other cases
//            await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryAllPrefixCacheKey);
//            await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryXmlAllPrefixCacheKey);
//        }
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityDeletedEvent<ProductCategory> eventMessage)
//    {
//        if (_catalogSettings.ShowCategoryProductNumber)
//        {
//            //depends on CatalogSettings.ShowCategoryProductNumber (when enabled)
//            //so there's no need to clear this cache in other cases
//            await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryAllPrefixCacheKey);
//            await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryXmlAllPrefixCacheKey);
//        }
//    }

//    #endregion

//    #region Products

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityInsertedEvent<Product> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<Product> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.HomepageBestsellersIdsPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductsAlsoPurchasedIdsPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(string.Format(AssetForgeModelCacheDefaults.ProductReviewsPrefixCacheKeyById, eventMessage.Entity.Id));
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityDeletedEvent<Product> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.HomepageBestsellersIdsPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductsAlsoPurchasedIdsPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    #endregion

//    #region Product tags

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityInsertedEvent<ProductTag> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<ProductTag> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityDeletedEvent<ProductTag> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    #endregion

//    #region Product attributes

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<ProductAttributeValue> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductAttributePicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductAttributeImageSquarePicturePrefixCacheKey);
//    }

//    #endregion

//    #region Pages

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityInsertedEvent<Page> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<Page> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityDeletedEvent<Page> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    #endregion

//    #region Orders

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityInsertedEvent<Order> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.HomepageBestsellersIdsPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductsAlsoPurchasedIdsPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<Order> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.HomepageBestsellersIdsPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductsAlsoPurchasedIdsPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityDeletedEvent<Order> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.HomepageBestsellersIdsPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductsAlsoPurchasedIdsPrefixCacheKey);
//    }

//    #endregion

//    #region Pictures

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityInsertedEvent<Picture> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductAttributePicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CartPicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.OrderPicturePrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<Picture> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductAttributePicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CartPicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.OrderPicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductDetailsPicturesPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductOverviewPicturesPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryPicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ManufacturerPicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.VendorPicturePrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityDeletedEvent<Picture> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductAttributePicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CartPicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.OrderPicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductDetailsPicturesPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductOverviewPicturesPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CategoryPicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ManufacturerPicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.VendorPicturePrefixCacheKey);
//    }

//    #endregion

//    #region Product picture mappings

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityInsertedEvent<ProductPicture> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(string.Format(AssetForgeModelCacheDefaults.ProductOverviewPicturesPrefixCacheKeyById, eventMessage.Entity.ProductId));
//        await _staticCacheManager.RemoveByPrefixAsync(string.Format(AssetForgeModelCacheDefaults.ProductDetailsPicturesPrefixCacheKeyById, eventMessage.Entity.ProductId));
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductAttributePicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CartPicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.OrderPicturePrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<ProductPicture> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(string.Format(AssetForgeModelCacheDefaults.ProductOverviewPicturesPrefixCacheKeyById, eventMessage.Entity.ProductId));
//        await _staticCacheManager.RemoveByPrefixAsync(string.Format(AssetForgeModelCacheDefaults.ProductDetailsPicturesPrefixCacheKeyById, eventMessage.Entity.ProductId));
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductAttributePicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CartPicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.OrderPicturePrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityDeletedEvent<ProductPicture> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(string.Format(AssetForgeModelCacheDefaults.ProductOverviewPicturesPrefixCacheKeyById, eventMessage.Entity.ProductId));
//        await _staticCacheManager.RemoveByPrefixAsync(string.Format(AssetForgeModelCacheDefaults.ProductDetailsPicturesPrefixCacheKeyById, eventMessage.Entity.ProductId));
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.ProductAttributePicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CartPicturePrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.OrderPicturePrefixCacheKey);
//    }

//    #endregion

//    #region Polls

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityInsertedEvent<Poll> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.PollsPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<Poll> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.PollsPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityDeletedEvent<Poll> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.PollsPrefixCacheKey);
//    }

//    #endregion

//    #region Blog posts

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityInsertedEvent<BlogPost> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.BlogPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<BlogPost> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.BlogPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityDeletedEvent<BlogPost> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.BlogPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    #endregion

//    #region News items

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityInsertedEvent<NewsItem> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.NewsPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<NewsItem> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.NewsPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityDeletedEvent<NewsItem> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.NewsPrefixCacheKey);
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.SitemapPrefixCacheKey);
//    }

//    #endregion

//    #region Shopping cart items

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityUpdatedEvent<ShoppingCartItem> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(AssetForgeModelCacheDefaults.CartPicturePrefixCacheKey);
//    }

//    #endregion

//    #region Product reviews

//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(EntityDeletedEvent<ProductReview> eventMessage)
//    {
//        await _staticCacheManager.RemoveByPrefixAsync(string.Format(AssetForgeModelCacheDefaults.ProductReviewsPrefixCacheKeyById, eventMessage.Entity.ProductId));
//    }

//    #endregion

//    #region Plugin

//    /// <summary>
//    /// Handle plugin updated event
//    /// </summary>
//    /// <param name="eventMessage">Event message</param>
//    /// <returns>A task that represents the asynchronous operation</returns>
//    public async Task HandleEventAsync(PluginUpdatedEvent eventMessage)
//    {
//        if (eventMessage?.Plugin?.Instance<IWidgetPlugin>() != null)
//            await _staticCacheManager.RemoveByPrefixAsync(WidgetModelDefaults.WidgetPrefixCacheKey);
//    }

//    #endregion

//    #endregion
//}