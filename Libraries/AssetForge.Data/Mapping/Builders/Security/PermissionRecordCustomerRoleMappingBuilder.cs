using FluentMigrator.Builders.Create.Table;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Security;
using AssetForge.Data.Extensions;

namespace AssetForge.Data.Mapping.Builders.Security;

/// <summary>
/// Represents a permission record customer role mapping entity builder
/// </summary>
public partial class PermissionRecordCustomerRoleMappingBuilder : EntityBuilder<PermissionRecordCustomerRoleMapping>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(NameCompatibilityManager.GetColumnName(typeof(PermissionRecordCustomerRoleMapping), nameof(PermissionRecordCustomerRoleMapping.PermissionRecordId)))
            .AsInt32().PrimaryKey().ForeignKey<PermissionRecord>()
            .WithColumn(NameCompatibilityManager.GetColumnName(typeof(PermissionRecordCustomerRoleMapping), nameof(PermissionRecordCustomerRoleMapping.CustomerRoleId)))
            .AsInt32().PrimaryKey().ForeignKey<CustomerRole>();
    }

    #endregion
}