using AssetForge.Core.Domain.Topics;
using AssetForge.Data;

namespace AssetForge.Services.Topics;

/// <summary>
/// Topic template service
/// </summary>
public partial class TopicTemplateService : ITopicTemplateService
{
    #region Fields

    protected readonly IRepository<TopicTemplate> _topicTemplateRepository;

    #endregion

    #region Ctor

    public TopicTemplateService(IRepository<TopicTemplate> topicTemplateRepository)
    {
        _topicTemplateRepository = topicTemplateRepository;
    }

    #endregion

    #region Methods

    public virtual async Task DeleteTopicTemplateAsync(TopicTemplate topicTemplate)
    {
        await _topicTemplateRepository.DeleteAsync(topicTemplate);
    }

    public virtual async Task<IList<TopicTemplate>> GetAllTopicTemplatesAsync()
    {
        var templates = await _topicTemplateRepository.GetAllAsync(query =>
        {
            return from pt in query
                   orderby pt.DisplayOrder, pt.Id
                   select pt;
        }, cache => default);

        return templates;
    }

    public virtual async Task<TopicTemplate> GetTopicTemplateByIdAsync(int topicTemplateId)
    {
        return await _topicTemplateRepository.GetByIdAsync(topicTemplateId, cache => default);
    }

    public virtual async Task InsertTopicTemplateAsync(TopicTemplate topicTemplate)
    {
        await _topicTemplateRepository.InsertAsync(topicTemplate);
    }

    public virtual async Task UpdateTopicTemplateAsync(TopicTemplate topicTemplate)
    {
        await _topicTemplateRepository.UpdateAsync(topicTemplate);
    }

    #endregion
}
