using FluentMigrator.Builders.Create.Table;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Logging;
using AssetForge.Data.Extensions;

namespace AssetForge.Data.Mapping.Builders.Logging;

/// <summary>
/// Represents a log entity builder
/// </summary>
public partial class LogBuilder : EntityBuilder<Log>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Log.ShortMessage)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(Log.IpAddress)).AsString(100).Nullable()
            .WithColumn(nameof(Log.CustomerId)).AsInt32().Nullable().ForeignKey<Customer>();
    }

    #endregion
}