using FluentMigrator.Builders.Create.Table;
using AssetForge.Core.Domain.Sites;
using AssetForge.Data.Extensions;

namespace AssetForge.Data.Mapping.Builders.Sites;

/// <summary>
/// Represents a site mapping entity builder
/// </summary>
public partial class SiteMappingBuilder : EntityBuilder<SiteMapping>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(SiteMapping.EntityName)).AsString(400).NotNullable()
            .WithColumn(nameof(SiteMapping.SiteId)).AsInt32().ForeignKey<Site>();
    }

    #endregion
}