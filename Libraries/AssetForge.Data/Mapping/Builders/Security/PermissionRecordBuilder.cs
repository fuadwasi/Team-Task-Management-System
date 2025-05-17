using FluentMigrator.Builders.Create.Table;
using AssetForge.Core.Domain.Security;

namespace AssetForge.Data.Mapping.Builders.Security;

/// <summary>
/// Represents a permission record entity builder
/// </summary>
public partial class PermissionRecordBuilder : EntityBuilder<PermissionRecord>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(PermissionRecord.Name)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(PermissionRecord.SystemName)).AsString(255).NotNullable()
            .WithColumn(nameof(PermissionRecord.Category)).AsString(255).NotNullable();
    }

    #endregion
}