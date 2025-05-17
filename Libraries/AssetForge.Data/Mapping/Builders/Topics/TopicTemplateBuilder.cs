using FluentMigrator.Builders.Create.Table;
using AssetForge.Core.Domain.Topics;

namespace AssetForge.Data.Mapping.Builders.Topics;

public partial class TopicTemplateBuilder : EntityBuilder<TopicTemplate>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(TopicTemplate.Name)).AsString(400).NotNullable()
            .WithColumn(nameof(TopicTemplate.ViewPath)).AsString(400).NotNullable();
    }
}
