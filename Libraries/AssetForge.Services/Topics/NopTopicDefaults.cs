using AssetForge.Core.Caching;
using AssetForge.Core.Domain.Topics;

namespace AssetForge.Services.Topics;

/// <summary>
/// Represents default values related to topic services
/// </summary>
public static partial class AssetForgeTopicDefaults
{
    public static CacheKey TopicsAllCacheKey => new("AssetForge.topic.all.{0}-{1}-{2}", EntityCacheDefaults<Topic>.AllPrefix);

    public static CacheKey TopicsAllWithACLCacheKey => new("AssetForge.topic.all.withacl.{0}-{1}-{2}-{3}", EntityCacheDefaults<Topic>.AllPrefix);

    public static CacheKey TopicBySystemNameCacheKey => new("AssetForge.topic.bysystemname.{0}-{1}-{2}", TopicBySystemNamePrefix);

    public static string TopicBySystemNamePrefix => "AssetForge.topic.bysystemname.{0}";
}
