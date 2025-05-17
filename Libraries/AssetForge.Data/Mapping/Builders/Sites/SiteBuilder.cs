using FluentMigrator.Builders.Create.Table;
using AssetForge.Core.Domain.Sites;

namespace AssetForge.Data.Mapping.Builders.Sites;

/// <summary>
/// Represents a site entity builder
/// </summary>
public partial class SiteBuilder : EntityBuilder<Site>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Site.Name)).AsString(400).NotNullable()
            .WithColumn(nameof(Site.Url)).AsString(400).NotNullable()
            .WithColumn(nameof(Site.Hosts)).AsString(1000).Nullable()
            .WithColumn(nameof(Site.CompanyName)).AsString(1000).Nullable()
            .WithColumn(nameof(Site.CompanyAddress)).AsString(1000).Nullable()
            .WithColumn(nameof(Site.CompanyPhoneNumber)).AsString(1000).Nullable()
            .WithColumn(nameof(Site.CompanyVat)).AsString(1000).Nullable();
    }

    #endregion
}