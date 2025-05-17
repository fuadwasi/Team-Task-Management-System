using AssetForge.Core.Domain.Topics;

namespace AssetForge.Services.Topics;

/// <summary>
/// Topic template service interface
/// </summary>
public partial interface ITopicTemplateService
{
    Task DeleteTopicTemplateAsync(TopicTemplate topicTemplate);

    Task<IList<TopicTemplate>> GetAllTopicTemplatesAsync();

    Task<TopicTemplate> GetTopicTemplateByIdAsync(int topicTemplateId);

    Task InsertTopicTemplateAsync(TopicTemplate topicTemplate);

    Task UpdateTopicTemplateAsync(TopicTemplate topicTemplate);
}
