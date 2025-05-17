using FluentMigrator.Builders.Create.Table;
using AssetForge.Core.Domain.Customers;

namespace AssetForge.Data.Mapping.Builders.Customers;

/// <summary>
/// Represents a customer attribute entity builder
/// </summary>
public partial class CustomerAttributeBuilder : EntityBuilder<CustomerAttribute>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(CustomerAttribute.Name)).AsString(400).NotNullable();
    }

    #endregion
}