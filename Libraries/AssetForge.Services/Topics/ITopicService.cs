using AssetForge.Core.Domain.Topics;

namespace AssetForge.Services.Topics;

/// <summary>
/// Topic service interface
/// </summary>
public partial interface ITopicService
{
    Task DeleteTopicAsync(Topic topic);

    Task<Topic> GetTopicByIdAsync(int topicId);

    Task<Topic> GetTopicBySystemNameAsync(string systemName, int storeId = 0);

    Task<IList<Topic>> GetAllTopicsAsync(int storeId, bool ignoreAcl = false, bool showHidden = false, bool onlyIncludedInTopMenu = false);

    Task<IList<Topic>> GetAllTopicsAsync(int storeId, string keywords, bool ignoreAcl = false, bool showHidden = false, bool onlyIncludedInTopMenu = false);

    Task InsertTopicAsync(Topic topic);

    Task UpdateTopicAsync(Topic topic);
}
