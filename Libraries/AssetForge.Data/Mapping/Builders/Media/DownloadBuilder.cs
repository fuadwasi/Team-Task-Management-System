using FluentMigrator.Builders.Create.Table;
using AssetForge.Core.Domain.Media;

namespace AssetForge.Data.Mapping.Builders.Media;

/// <summary>
/// Represents a download entity builder
/// </summary>
public partial class DownloadBuilder : EntityBuilder<Download>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
    }

    #endregion
}