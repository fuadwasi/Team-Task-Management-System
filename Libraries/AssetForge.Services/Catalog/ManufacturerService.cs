using AssetForge.Core;
using AssetForge.Core.Caching;
using AssetForge.Core.Domain.Catalog;
using AssetForge.Core.Domain.Customers;
using AssetForge.Data;
using AssetForge.Services.Customers;
using AssetForge.Services.Security;
using AssetForge.Services.Sites;

namespace AssetForge.Services.Catalog;

/// <summary>
/// Manufacturer service
/// </summary>
public partial class ManufacturerService : IManufacturerService
{
    #region Fields

    protected readonly IAclService _aclService;
    protected readonly ICustomerService _customerService;
    protected readonly IRepository<Manufacturer> _manufacturerRepository;
    protected readonly IStaticCacheManager _staticCacheManager;
    protected readonly ISiteContext _siteContext;
    protected readonly ISiteMappingService _siteMappingService;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public ManufacturerService(
        //CatalogSettings catalogSettings,
        IAclService aclService,
        //ICategoryService categoryService,
        ICustomerService customerService,
        IRepository<Manufacturer> manufacturerRepository,
        //IRepository<Product> productRepository,
        //IRepository<ProductManufacturer> productManufacturerRepository,
        //IRepository<ProductCategory> productCategoryRepository,
        IStaticCacheManager staticCacheManager,
        ISiteContext siteContext,
        ISiteMappingService siteMappingService,
        IWorkContext workContext)
    {
        _aclService = aclService;
        _customerService = customerService;
        _manufacturerRepository = manufacturerRepository;
        _staticCacheManager = staticCacheManager;
        _siteContext = siteContext;
        _siteMappingService = siteMappingService;
        _workContext = workContext;
    }

    #endregion

    #region Methods

    ///// <summary>
    ///// Clean up manufacturer references for a specified discount
    ///// </summary>
    ///// <param name="discount">Discount</param>
    ///// <returns>A task that represents the asynchronous operation</returns>
    //public virtual async Task ClearDiscountManufacturerMappingAsync(Discount discount)
    //{
    //    ArgumentNullException.ThrowIfNull(discount);

    //    var mappings = _discountManufacturerMappingRepository.Table.Where(dcm => dcm.DiscountId == discount.Id);

    //    await _discountManufacturerMappingRepository.DeleteAsync(mappings.ToList());
    //}

    /// <summary>
    /// Deletes a manufacturer
    /// </summary>
    /// <param name="manufacturer">Manufacturer</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteManufacturerAsync(Manufacturer manufacturer)
    {
        await _manufacturerRepository.DeleteAsync(manufacturer);
    }

    /// <summary>
    /// Delete manufacturers
    /// </summary>
    /// <param name="manufacturers">Manufacturers</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteManufacturersAsync(IList<Manufacturer> manufacturers)
    {
        await _manufacturerRepository.DeleteAsync(manufacturers);
    }

    /// <summary>
    /// Gets all manufacturers
    /// </summary>
    /// <param name="manufacturerName">Manufacturer name</param>
    /// <param name="siteId">Site identifier; 0 if you want to get all records</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <param name="overridePublished">
    /// null - process "Published" property according to "showHidden" parameter
    /// true - load only "Published" products
    /// false - load only "Unpublished" products
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the manufacturers
    /// </returns>
    public virtual async Task<IPagedList<Manufacturer>> GetAllManufacturersAsync(string manufacturerName = "",
        int siteId = 0,
        int pageIndex = 0,
        int pageSize = int.MaxValue,
        bool showHidden = false,
        bool? overridePublished = null)
    {
        return await _manufacturerRepository.GetAllPagedAsync(async query =>
        {
            if (!showHidden)
                query = query.Where(m => m.Published);
            else if (overridePublished.HasValue)
                query = query.Where(m => m.Published == overridePublished.Value);

            if (!showHidden || siteId > 0)
            {
                //apply site mapping constraints
                query = await _siteMappingService.ApplySiteMapping(query, siteId);
            }

            if (!showHidden)
            {
                //apply ACL constraints
                var customer = await _workContext.GetCurrentCustomerAsync();
                query = await _aclService.ApplyAcl(query, customer);
            }

            query = query.Where(m => !m.Deleted);

            if (!string.IsNullOrWhiteSpace(manufacturerName))
                query = query.Where(m => m.Name.Contains(manufacturerName));

            return query.OrderBy(m => m.DisplayOrder).ThenBy(m => m.Id);
        }, pageIndex, pageSize);
    }

    /// <summary>
    /// Gets a manufacturer
    /// </summary>
    /// <param name="manufacturerId">Manufacturer identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the manufacturer
    /// </returns>
    public virtual async Task<Manufacturer> GetManufacturerByIdAsync(int manufacturerId)
    {
        return await _manufacturerRepository.GetByIdAsync(manufacturerId, cache => default);
    }

    /// <summary>
    /// Gets the manufacturers by category identifier
    /// </summary>
    /// <param name="categoryId">Category identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the manufacturers
    /// </returns>
    //public virtual async Task<IList<Manufacturer>> GetManufacturersByCategoryIdAsync(int categoryId)
    //{
    //    if (categoryId <= 0)
    //        return new List<Manufacturer>();

    //    // get available products in category
    //    var productsQuery =
    //        from p in _productRepository.Table
    //        where !p.Deleted && p.Published &&
    //              (p.ParentGroupedProductId == 0 || p.VisibleIndividually) &&
    //              (!p.AvailableStartDateTimeUtc.HasValue || p.AvailableStartDateTimeUtc <= DateTime.UtcNow) &&
    //              (!p.AvailableEndDateTimeUtc.HasValue || p.AvailableEndDateTimeUtc >= DateTime.UtcNow)
    //        select p;

    //    var site = await _siteContext.GetCurrentSiteAsync();
    //    var customer = await _workContext.GetCurrentCustomerAsync();
    //    var customerRoleIds = await _customerService.GetCustomerRoleIdsAsync(customer);

    //    //apply site mapping constraints
    //    productsQuery = await _siteMappingService.ApplySiteMapping(productsQuery, site.Id);

    //    //apply ACL constraints
    //    productsQuery = await _aclService.ApplyAcl(productsQuery, customerRoleIds);

    //    var subCategoryIds = _catalogSettings.ShowProductsFromSubcategories
    //        ? await _categoryService.GetChildCategoryIdsAsync(categoryId, site.Id)
    //        : null;

    //    var productCategoryQuery =
    //        from pc in _productCategoryRepository.Table
    //        where (pc.CategoryId == categoryId || (_catalogSettings.ShowProductsFromSubcategories && subCategoryIds.Contains(pc.CategoryId))) &&
    //              (_catalogSettings.IncludeFeaturedProductsInNormalLists || !pc.IsFeaturedProduct)
    //        select pc;

    //    var manufacturerQuery = from m in _manufacturerRepository.Table
    //        where !m.Deleted && m.Published
    //        select m;

    //    //apply site mapping constraints
    //    manufacturerQuery = await _siteMappingService.ApplySiteMapping(manufacturerQuery, site.Id);

    //    //apply ACL constraints
    //    manufacturerQuery = await _aclService.ApplyAcl(manufacturerQuery, customerRoleIds);

    //    // get manufacturers of the products
    //    var manufacturersQuery =
    //        from m in manufacturerQuery
    //        join pm in _productManufacturerRepository.Table on m.Id equals pm.ManufacturerId
    //        join p in productsQuery on pm.ProductId equals p.Id
    //        join pc in productCategoryQuery on p.Id equals pc.ProductId
    //        orderby
    //            m.DisplayOrder, m.Name
    //        select m;

    //    var key = _staticCacheManager
    //        .PrepareKeyForDefaultCache(AssetForgeCatalogDefaults.ManufacturersByCategoryCacheKey, categoryId, site, customerRoleIds);

    //    return await _staticCacheManager.GetAsync(key, async () => await manufacturersQuery.Distinct().ToListAsync());
    //}

    /// <summary>
    /// Gets manufacturers by identifier
    /// </summary>
    /// <param name="manufacturerIds">manufacturer identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the manufacturers
    /// </returns>
    public virtual async Task<IList<Manufacturer>> GetManufacturersByIdsAsync(int[] manufacturerIds)
    {
        return await _manufacturerRepository.GetByIdsAsync(manufacturerIds, includeDeleted: false);
    }

    /// <summary>
    /// Inserts a manufacturer
    /// </summary>
    /// <param name="manufacturer">Manufacturer</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task InsertManufacturerAsync(Manufacturer manufacturer)
    {
        await _manufacturerRepository.InsertAsync(manufacturer);
    }

    /// <summary>
    /// Updates the manufacturer
    /// </summary>
    /// <param name="manufacturer">Manufacturer</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateManufacturerAsync(Manufacturer manufacturer)
    {
        await _manufacturerRepository.UpdateAsync(manufacturer);
    }

    /// <summary>
    /// Deletes a product manufacturer mapping
    /// </summary>
    /// <param name="productManufacturer">Product manufacturer mapping</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    //public virtual async Task DeleteProductManufacturerAsync(ProductManufacturer productManufacturer)
    //{
    //    await _productManufacturerRepository.DeleteAsync(productManufacturer);
    //}

    ///// <summary>
    ///// Gets product manufacturer collection
    ///// </summary>
    ///// <param name="manufacturerId">Manufacturer identifier</param>
    ///// <param name="pageIndex">Page index</param>
    ///// <param name="pageSize">Page size</param>
    ///// <param name="showHidden">A value indicating whether to show hidden records</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the product manufacturer collection
    ///// </returns>
    //public virtual async Task<IPagedList<ProductManufacturer>> GetProductManufacturersByManufacturerIdAsync(int manufacturerId,
    //    int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
    //{
    //    if (manufacturerId == 0)
    //        return new PagedList<ProductManufacturer>(new List<ProductManufacturer>(), pageIndex, pageSize);

    //    var query = from pm in _productManufacturerRepository.Table
    //        join p in _productRepository.Table on pm.ProductId equals p.Id
    //        where pm.ManufacturerId == manufacturerId && !p.Deleted
    //        orderby pm.DisplayOrder, pm.Id
    //        select pm;

    //    if (!showHidden)
    //    {
    //        var manufacturersQuery = _manufacturerRepository.Table.Where(m => m.Published);

    //        //apply site mapping constraints
    //        var site = await _siteContext.GetCurrentSiteAsync();
    //        manufacturersQuery = await _siteMappingService.ApplySiteMapping(manufacturersQuery, site.Id);

    //        //apply ACL constraints
    //        var customer = await _workContext.GetCurrentCustomerAsync();
    //        manufacturersQuery = await _aclService.ApplyAcl(manufacturersQuery, customer);

    //        query = query.Where(pm => manufacturersQuery.Any(m => m.Id == pm.ManufacturerId));
    //    }

    //    return await query.ToPagedListAsync(pageIndex, pageSize);
    //}

    ///// <summary>
    ///// Gets a product manufacturer mapping collection
    ///// </summary>
    ///// <param name="productId">Product identifier</param>
    ///// <param name="showHidden">A value indicating whether to show hidden records</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the product manufacturer mapping collection
    ///// </returns>
    //public virtual async Task<IList<ProductManufacturer>> GetProductManufacturersByProductIdAsync(int productId,
    //    bool showHidden = false)
    //{
    //    if (productId == 0)
    //        return new List<ProductManufacturer>();

    //    var site = await _siteContext.GetCurrentSiteAsync();
    //    var customer = await _workContext.GetCurrentCustomerAsync();
    //    var customerRoleIds = await _customerService.GetCustomerRoleIdsAsync(customer);

    //    var key = _staticCacheManager
    //        .PrepareKeyForDefaultCache(AssetForgeCatalogDefaults.ProductManufacturersByProductCacheKey, productId, showHidden, customerRoleIds, site);

    //    var query = from pm in _productManufacturerRepository.Table
    //        join m in _manufacturerRepository.Table on pm.ManufacturerId equals m.Id
    //        where pm.ProductId == productId && !m.Deleted
    //        orderby pm.DisplayOrder, pm.Id
    //        select pm;

    //    if (!showHidden)
    //    {
    //        var manufacturersQuery = _manufacturerRepository.Table.Where(m => m.Published);

    //        //apply site mapping constraints
    //        manufacturersQuery = await _siteMappingService.ApplySiteMapping(manufacturersQuery, site.Id);

    //        //apply ACL constraints
    //        manufacturersQuery = await _aclService.ApplyAcl(manufacturersQuery, customerRoleIds);

    //        query = query.Where(pm => manufacturersQuery.Any(m => m.Id == pm.ManufacturerId));
    //    }

    //    return await _staticCacheManager.GetAsync(key, query.ToList);
    //}

    ///// <summary>
    ///// Gets a product manufacturer mapping 
    ///// </summary>
    ///// <param name="productManufacturerId">Product manufacturer mapping identifier</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the product manufacturer mapping
    ///// </returns>
    //public virtual async Task<ProductManufacturer> GetProductManufacturerByIdAsync(int productManufacturerId)
    //{
    //    return await _productManufacturerRepository.GetByIdAsync(productManufacturerId, cache => default);
    //}

    ///// <summary>
    ///// Inserts a product manufacturer mapping
    ///// </summary>
    ///// <param name="productManufacturer">Product manufacturer mapping</param>
    ///// <returns>A task that represents the asynchronous operation</returns>
    //public virtual async Task InsertProductManufacturerAsync(ProductManufacturer productManufacturer)
    //{
    //    await _productManufacturerRepository.InsertAsync(productManufacturer);
    //}

    ///// <summary>
    ///// Updates the product manufacturer mapping
    ///// </summary>
    ///// <param name="productManufacturer">Product manufacturer mapping</param>
    ///// <returns>A task that represents the asynchronous operation</returns>
    //public virtual async Task UpdateProductManufacturerAsync(ProductManufacturer productManufacturer)
    //{
    //    await _productManufacturerRepository.UpdateAsync(productManufacturer);
    //}

    ///// <summary>
    ///// Get manufacturer IDs for products
    ///// </summary>
    ///// <param name="productIds">Products IDs</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the manufacturer IDs for products
    ///// </returns>
    //public virtual async Task<IDictionary<int, int[]>> GetProductManufacturerIdsAsync(int[] productIds)
    //{
    //    var query = _productManufacturerRepository.Table;

    //    return (await query.Where(p => productIds.Contains(p.ProductId))
    //            .Select(p => new { p.ProductId, p.ManufacturerId })
    //            .ToListAsync())
    //        .GroupBy(a => a.ProductId)
    //        .ToDictionary(items => items.Key, items => items.Select(a => a.ManufacturerId).ToArray());
    //}

    /// <summary>
    /// Returns a list of names of not existing manufacturers
    /// </summary>
    /// <param name="manufacturerIdsNames">The names and/or IDs of the manufacturers to check</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of names and/or IDs not existing manufacturers
    /// </returns>
    public virtual async Task<string[]> GetNotExistingManufacturersAsync(string[] manufacturerIdsNames)
    {
        ArgumentNullException.ThrowIfNull(manufacturerIdsNames);

        var query = _manufacturerRepository.Table;//.Where(m => !m.Deleted);
        var queryFilter = manufacturerIdsNames.Distinct().ToArray();
        //filtering by name
        var filter = query.Select(m => m.Name).Where(m => queryFilter.Contains(m)).ToList();
        queryFilter = queryFilter.Except(filter).ToArray();

        //if some names not found
        if (!queryFilter.Any())
            return queryFilter.ToArray();

        //filtering by IDs
        filter = await query.Select(c => c.Id.ToString())
            .Where(c => queryFilter.Contains(c))
            .ToListAsync();

        return queryFilter.Except(filter).ToArray();
    }

    ///// <summary>
    ///// Returns a ProductManufacturer that has the specified values
    ///// </summary>
    ///// <param name="source">Source</param>
    ///// <param name="productId">Product identifier</param>
    ///// <param name="manufacturerId">Manufacturer identifier</param>
    ///// <returns>A ProductManufacturer that has the specified values; otherwise null</returns>
    //public virtual ProductManufacturer FindProductManufacturer(IList<ProductManufacturer> source, int productId, int manufacturerId)
    //{
    //    foreach (var productManufacturer in source)
    //        if (productManufacturer.ProductId == productId && productManufacturer.ManufacturerId == manufacturerId)
    //            return productManufacturer;

    //    return null;
    //}

    #endregion
}