using AssetForge.Core;
using AssetForge.Core.Caching;
using AssetForge.Core.Domain.Topics;
using AssetForge.Data;
using AssetForge.Services.Customers;
using AssetForge.Services.Security;
using AssetForge.Services.Sites;

namespace AssetForge.Services.Topics;

/// <summary>
/// Topic service
/// </summary>
public partial class TopicService : ITopicService
{
    protected readonly IAclService _aclService;
    protected readonly ICustomerService _customerService;
    protected readonly IRepository<Topic> _topicRepository;
    protected readonly IStaticCacheManager _staticCacheManager;
    protected readonly ISiteMappingService _siteMappingService;
    protected readonly IWorkContext _workContext;

    public TopicService(
        IAclService aclService,
        ICustomerService customerService,
        IRepository<Topic> topicRepository,
        IStaticCacheManager staticCacheManager,
        ISiteMappingService siteMappingService,
        IWorkContext workContext)
    {
        _aclService = aclService;
        _customerService = customerService;
        _topicRepository = topicRepository;
        _staticCacheManager = staticCacheManager;
        _siteMappingService = siteMappingService;
        _workContext = workContext;
    }

    public virtual async Task DeleteTopicAsync(Topic topic)
    {
        await _topicRepository.DeleteAsync(topic);
    }

    public virtual async Task<Topic> GetTopicByIdAsync(int topicId)
    {
        return await _topicRepository.GetByIdAsync(topicId, cache => default);
    }

    public virtual async Task<Topic> GetTopicBySystemNameAsync(string systemName, int siteId = 0)
    {
        if (string.IsNullOrEmpty(systemName))
            return null;

        var customer = await _workContext.GetCurrentCustomerAsync();
        var customerRoleIds = await _customerService.GetCustomerRoleIdsAsync(customer);

        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(AssetForgeTopicDefaults.TopicBySystemNameCacheKey, systemName, siteId, customerRoleIds);

        return await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            var query = _topicRepository.Table
                .Where(t => t.Published);

            query = await _siteMappingService.ApplySiteMapping(query, siteId);

            query = await _aclService.ApplyAcl(query, customerRoleIds);

            return query.Where(t => t.SystemName == systemName)
                .OrderBy(t => t.Id)
                .FirstOrDefault();
        });
    }

    public virtual async Task<IList<Topic>> GetAllTopicsAsync(int siteId,
        bool ignoreAcl = false, bool showHidden = false, bool onlyIncludedInTopMenu = false)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        var customerRoleIds = await _customerService.GetCustomerRoleIdsAsync(customer);

        return await _topicRepository.GetAllAsync(async query =>
        {
            if (!showHidden || siteId > 0)
            {
                query = await _siteMappingService.ApplySiteMapping(query, siteId);
            }

            if (!showHidden)
            {
                query = query.Where(t => t.Published);

                if (!ignoreAcl)
                    query = await _aclService.ApplyAcl(query, customerRoleIds);
            }

            if (onlyIncludedInTopMenu)
                query = query.Where(t => t.IncludeInTopMenu);

            return query.OrderBy(t => t.DisplayOrder).ThenBy(t => t.SystemName);
        }, cache =>
        {
            return ignoreAcl
                ? cache.PrepareKeyForDefaultCache(AssetForgeTopicDefaults.TopicsAllCacheKey, siteId, showHidden, onlyIncludedInTopMenu)
                : cache.PrepareKeyForDefaultCache(AssetForgeTopicDefaults.TopicsAllWithACLCacheKey, siteId, showHidden, onlyIncludedInTopMenu, customerRoleIds);
        });
    }

    public virtual async Task<IList<Topic>> GetAllTopicsAsync(int siteId, string keywords,
        bool ignoreAcl = false, bool showHidden = false, bool onlyIncludedInTopMenu = false)
    {
        var topics = await GetAllTopicsAsync(siteId,
            ignoreAcl: ignoreAcl,
            showHidden: showHidden,
            onlyIncludedInTopMenu: onlyIncludedInTopMenu);

        if (!string.IsNullOrWhiteSpace(keywords))
        {
            return topics
                .Where(topic => (topic.Title?.Contains(keywords, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                                (topic.Body?.Contains(keywords, StringComparison.InvariantCultureIgnoreCase) ?? false))
                .ToList();
        }

        return topics;
    }

    public virtual async Task InsertTopicAsync(Topic topic)
    {
        await _topicRepository.InsertAsync(topic);
    }

    public virtual async Task UpdateTopicAsync(Topic topic)
    {
        await _topicRepository.UpdateAsync(topic);
    }
}
