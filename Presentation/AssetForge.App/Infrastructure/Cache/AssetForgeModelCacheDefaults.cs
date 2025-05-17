using AssetForge.Core.Caching;

namespace AssetForge.App.Infrastructure.Cache;

public static partial class AssetForgeModelCacheDefaults
{
    /// <summary>
    /// Key for ManufacturerNavigationModel caching
    /// </summary>
    /// <remarks>
    /// {0} : current manufacturer id
    /// {1} : language id
    /// {2} : roles of the current user
    /// {3} : current site ID
    /// </remarks>
    public static CacheKey ManufacturerNavigationModelKey => new("AssetForge.pres.manufacturer.navigation-{0}-{1}-{2}-{3}", ManufacturerNavigationPrefixCacheKey);
    public static string ManufacturerNavigationPrefixCacheKey => "AssetForge.pres.manufacturer.navigation";

    /// <summary>
    /// Key for VendorNavigationModel caching
    /// </summary>
    public static CacheKey VendorNavigationModelKey => new("AssetForge.pres.vendor.navigation");

    /// <summary>
    /// Key for list of CategorySimpleModel caching
    /// </summary>
    /// <remarks>
    /// {0} : language id
    /// {1} : roles of the current user
    /// {2} : current site ID
    /// </remarks>
    public static CacheKey CategoryAllModelKey => new("AssetForge.pres.category.all-{0}-{1}-{2}", CategoryAllPrefixCacheKey);
    public static string CategoryAllPrefixCacheKey => "AssetForge.pres.category.all";

    /// <summary>
    /// Key for list of CategorySimpleModel caching
    /// </summary>
    /// <remarks>
    /// {0} : language id
    /// {1} : roles of the current user
    /// {2} : current site ID
    /// </remarks>
    public static CacheKey PageMenuModelKey => new("AssetForge.pres.PageMenu.all-{0}-{1}-{2}", PageMenuPrefixCacheKey);
    public static string PageMenuPrefixCacheKey => "AssetForge.pres.PageMenu.all";

    /// <summary>
    /// Key for caching of categories displayed on home page
    /// </summary>
    /// <remarks>
    /// {0} : current site ID
    /// {1} : roles of the current user
    /// {2} : picture size
    /// {3} : language ID
    /// {4} : is connection SSL secured (included in a category picture URL)
    /// </remarks>
    public static CacheKey CategoryHomepageKey => new("AssetForge.pres.category.homepage-{0}-{1}-{2}-{3}-{4}", CategoryHomepagePrefixCacheKey);
    public static string CategoryHomepagePrefixCacheKey => "AssetForge.pres.category.homepage";

    /// <summary>
    /// Key for Xml document of CategorySimpleModels caching
    /// </summary>
    /// <remarks>
    /// {0} : language id
    /// {1} : roles of the current user
    /// {2} : current site ID
    /// </remarks>
    public static CacheKey CategoryXmlAllModelKey => new("AssetForge.pres.categoryXml.all-{0}-{1}-{2}", CategoryXmlAllPrefixCacheKey);
    public static string CategoryXmlAllPrefixCacheKey => "AssetForge.pres.categoryXml.all";

    /// <summary>
    /// Key for bestsellers identifiers displayed on the home page
    /// </summary>
    /// <remarks>
    /// {0} : current site ID
    /// </remarks>
    public static CacheKey HomepageBestsellersIdsKey => new("AssetForge.pres.bestsellers.homepage-{0}", HomepageBestsellersIdsPrefixCacheKey);
    public static string HomepageBestsellersIdsPrefixCacheKey => "AssetForge.pres.bestsellers.homepage";

    /// <summary>
    /// Key for "also purchased" product identifiers displayed on the product details page
    /// </summary>
    /// <remarks>
    /// {0} : current product id
    /// {1} : current site ID
    /// </remarks>
    public static CacheKey ProductsAlsoPurchasedIdsKey => new("AssetForge.pres.alsopuchased-{0}-{1}", ProductsAlsoPurchasedIdsPrefixCacheKey);
    public static string ProductsAlsoPurchasedIdsPrefixCacheKey => "AssetForge.pres.alsopuchased";

    /// <summary>
    /// Key for product picture caching on the product catalog pages (all pictures)
    /// </summary>
    /// <remarks>
    /// {0} : product id
    /// {1} : picture size
    /// {2} : value indicating whether a default picture is displayed in case if no real picture exists
    /// {3} : value indicating whether to display all product pictures
    /// {4} : language ID ("alt" and "title" can depend on localized product name)
    /// {5} : is connection SSL secured?
    /// {6} : current site ID
    /// </remarks>
    public static CacheKey ProductOverviewPicturesModelKey => new("AssetForge.pres.product.overviewpictures-{0}-{1}-{2}-{3}-{4}-{5}-{6}", ProductOverviewPicturesPrefixCacheKey, ProductOverviewPicturesPrefixCacheKeyById);
    public static string ProductOverviewPicturesPrefixCacheKey => "AssetForge.pres.product.overviewpictures";
    public static string ProductOverviewPicturesPrefixCacheKeyById => "AssetForge.pres.product.overviewpictures-{0}-";

    /// <summary>
    /// Key for product picture caching on the product details page (all pictures)
    /// </summary>
    /// <remarks>
    /// {0} : product id
    /// {1} : picture size
    /// {2} : isAssociatedProduct?
    /// {3} : language ID ("alt" and "title" can depend on localized product name)
    /// {4} : is connection SSL secured?
    /// {5} : current site ID
    /// </remarks>
    public static CacheKey ProductDetailsPicturesModelKey => new("AssetForge.pres.product.detailspictures-{0}-{1}-{2}-{3}-{4}-{5}", ProductDetailsPicturesPrefixCacheKey, ProductDetailsPicturesPrefixCacheKeyById);
    public static string ProductDetailsPicturesPrefixCacheKey => "AssetForge.pres.product.detailspictures";
    public static string ProductDetailsPicturesPrefixCacheKeyById => "AssetForge.pres.product.detailspictures-{0}-";

    /// <summary>
    /// Key for product reviews caching
    /// </summary>
    /// <remarks>
    /// {0} : product id
    /// {1} : current site ID
    /// </remarks>
    public static CacheKey ProductReviewsModelKey => new("AssetForge.pres.product.reviews-{0}-{1}", ProductReviewsPrefixCacheKey, ProductReviewsPrefixCacheKeyById);

    public static string ProductReviewsPrefixCacheKey => "AssetForge.pres.product.reviews";
    public static string ProductReviewsPrefixCacheKeyById => "AssetForge.pres.product.reviews-{0}-";

    /// <summary>
    /// Key for product attribute picture caching on the product details page
    /// </summary>
    /// <remarks>
    /// {0} : picture id
    /// {1} : is connection SSL secured?
    /// {2} : current site ID
    /// </remarks>
    public static CacheKey ProductAttributePictureModelKey => new("AssetForge.pres.productattribute.picture-{0}-{1}-{2}", ProductAttributePicturePrefixCacheKey);
    public static string ProductAttributePicturePrefixCacheKey => "AssetForge.pres.productattribute.picture";

    /// <summary>
    /// Key for product attribute picture caching on the product details page
    /// </summary>
    /// <remarks>
    /// {0} : picture id
    /// {1} : is connection SSL secured?
    /// {2} : current site ID
    /// </remarks>
    public static CacheKey ProductAttributeImageSquarePictureModelKey => new("AssetForge.pres.productattribute.imagesquare.picture-{0}-{1}-{2}", ProductAttributeImageSquarePicturePrefixCacheKey);
    public static string ProductAttributeImageSquarePicturePrefixCacheKey => "AssetForge.pres.productattribute.imagesquare.picture";

    /// <summary>
    /// Key for category picture caching
    /// </summary>
    /// <remarks>
    /// {0} : category id
    /// {1} : picture size
    /// {2} : value indicating whether a default picture is displayed in case if no real picture exists
    /// {3} : language ID ("alt" and "title" can depend on localized category name)
    /// {4} : is connection SSL secured?
    /// {5} : current site ID
    /// </remarks>
    public static CacheKey CategoryPictureModelKey => new("AssetForge.pres.category.picture-{0}-{1}-{2}-{3}-{4}-{5}", CategoryPicturePrefixCacheKey, CategoryPicturePrefixCacheKeyById);
    public static string CategoryPicturePrefixCacheKey => "AssetForge.pres.category.picture";
    public static string CategoryPicturePrefixCacheKeyById => "AssetForge.pres.category.picture-{0}-";

    /// <summary>
    /// Key for manufacturer picture caching
    /// </summary>
    /// <remarks>
    /// {0} : manufacturer id
    /// {1} : picture size
    /// {2} : value indicating whether a default picture is displayed in case if no real picture exists
    /// {3} : language ID ("alt" and "title" can depend on localized manufacturer name)
    /// {4} : is connection SSL secured?
    /// {5} : current site ID
    /// </remarks>
    public static CacheKey ManufacturerPictureModelKey => new("AssetForge.pres.manufacturer.picture-{0}-{1}-{2}-{3}-{4}-{5}", ManufacturerPicturePrefixCacheKey, ManufacturerPicturePrefixCacheKeyById);
    public static string ManufacturerPicturePrefixCacheKey => "AssetForge.pres.manufacturer.picture";
    public static string ManufacturerPicturePrefixCacheKeyById => "AssetForge.pres.manufacturer.picture-{0}-";

    /// <summary>
    /// Key for vendor picture caching
    /// </summary>
    /// <remarks>
    /// {0} : vendor id
    /// {1} : picture size
    /// {2} : value indicating whether a default picture is displayed in case if no real picture exists
    /// {3} : language ID ("alt" and "title" can depend on localized category name)
    /// {4} : is connection SSL secured?
    /// {5} : current site ID
    /// </remarks>
    public static CacheKey VendorPictureModelKey => new("AssetForge.pres.vendor.picture-{0}-{1}-{2}-{3}-{4}-{5}", VendorPicturePrefixCacheKey, VendorPicturePrefixCacheKeyById);
    public static string VendorPicturePrefixCacheKey => "AssetForge.pres.vendor.picture";
    public static string VendorPicturePrefixCacheKeyById => "AssetForge.pres.vendor.picture-{0}-";

    /// <summary>
    /// Key for cart picture caching
    /// </summary>
    /// <remarks>
    /// {0} : shopping cart item id
    /// P.S. we could cache by product ID. it could increase performance.
    /// but it won't work for product attributes with custom images
    /// {1} : picture size
    /// {2} : value indicating whether a default picture is displayed in case if no real picture exists
    /// {3} : language ID ("alt" and "title" can depend on localized product name)
    /// {4} : is connection SSL secured?
    /// {5} : current site ID
    /// </remarks>
    public static CacheKey CartPictureModelKey => new("AssetForge.pres.cart.picture-{0}-{1}-{2}-{3}-{4}-{5}", CartPicturePrefixCacheKey);
    public static string CartPicturePrefixCacheKey => "AssetForge.pres.cart.picture";

    /// <summary>
    /// Key for cart picture caching
    /// </summary>
    /// <remarks>
    /// {0} : order item id
    /// P.S. we could cache by product ID. it could increase performance.
    /// but it won't work for product attributes with custom images
    /// {1} : picture size
    /// {2} : value indicating whether a default picture is displayed in case if no real picture exists
    /// {3} : language ID ("alt" and "title" can depend on localized product name)
    /// {4} : is connection SSL secured?
    /// {5} : current site ID
    /// </remarks>
    public static CacheKey OrderPictureModelKey => new("AssetForge.pres.order.picture-{0}-{1}-{2}-{3}-{4}-{5}", OrderPicturePrefixCacheKey);
    public static string OrderPicturePrefixCacheKey => "AssetForge.pres.order.picture";

    /// <summary>
    /// Key for home page polls
    /// </summary>
    /// <remarks>
    /// {0} : language ID
    /// {1} : current site ID
    /// </remarks>
    public static CacheKey HomepagePollsModelKey => new("AssetForge.pres.poll.homepage-{0}-{1}", PollsPrefixCacheKey);
    /// <summary>
    /// Key for polls by system name
    /// </summary>
    /// <remarks>
    /// {0} : poll system name
    /// {1} : language ID
    /// {2} : current site ID
    /// </remarks>
    public static CacheKey PollBySystemNameModelKey => new("AssetForge.pres.poll.systemname-{0}-{1}-{2}", PollsPrefixCacheKey);
    public static string PollsPrefixCacheKey => "AssetForge.pres.poll";

    /// <summary>
    /// Key for blog archive (years, months) block model
    /// </summary>
    /// <remarks>
    /// {0} : language ID
    /// {1} : current site ID
    /// </remarks>
    public static CacheKey BlogMonthsModelKey => new("AssetForge.pres.blog.months-{0}-{1}", BlogPrefixCacheKey);
    public static string BlogPrefixCacheKey => "AssetForge.pres.blog";

    /// <summary>
    /// Key for home page news
    /// </summary>
    /// <remarks>
    /// {0} : language ID
    /// {1} : current site ID
    /// </remarks>
    public static CacheKey HomepageNewsModelKey => new("AssetForge.pres.news.homepage-{0}-{1}", NewsPrefixCacheKey);
    public static string NewsPrefixCacheKey => "AssetForge.pres.news";

    /// <summary>
    /// Key for logo
    /// </summary>
    /// <remarks>
    /// {0} : current site ID
    /// {1} : current theme
    /// {2} : is connection SSL secured (included in a picture URL)
    /// </remarks>
    public static CacheKey SiteLogoPath => new("AssetForge.pres.logo-{0}-{1}-{2}", SiteLogoPathPrefixCacheKey);
    public static string SiteLogoPathPrefixCacheKey => "AssetForge.pres.logo";

    /// <summary>
    /// Key for sitemap on the sitemap page
    /// </summary>
    /// <remarks>
    /// {0} : language id
    /// {1} : roles of the current user
    /// {2} : current site ID
    /// </remarks>
    public static CacheKey SitemapPageModelKey => new("AssetForge.pres.sitemap.page-{0}-{1}-{2}", SitemapPrefixCacheKey);
    /// <summary>
    /// Key for sitemap on the sitemap SEO page
    /// </summary>
    /// <remarks>
    /// {0} : sitemap identifier
    /// {1} : language id
    /// {2} : roles of the current user
    /// {3} : current site ID
    /// </remarks>
    public static CacheKey SitemapSeoModelKey => new("AssetForge.pres.sitemap.seo-{0}-{1}-{2}-{3}", SitemapPrefixCacheKey);
    public static string SitemapPrefixCacheKey => "AssetForge.pres.sitemap";
}