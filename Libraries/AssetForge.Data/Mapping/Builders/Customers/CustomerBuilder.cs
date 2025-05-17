using FluentMigrator.Builders.Create.Table;
using AssetForge.Core.Domain.Common;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Localization;
using AssetForge.Data.Extensions;
using System.Data;

namespace AssetForge.Data.Mapping.Builders.Customers;

/// <summary>
/// Represents a customer entity builder
/// </summary>
public partial class CustomerBuilder : EntityBuilder<Customer>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Customer.Username)).AsString(1000).Nullable()
            .WithColumn(nameof(Customer.Email)).AsString(1000).Nullable()
            .WithColumn(nameof(Customer.EmailToRevalidate)).AsString(1000).Nullable()
            .WithColumn(nameof(Customer.FirstName)).AsString(1000).Nullable()
            .WithColumn(nameof(Customer.LastName)).AsString(1000).Nullable()
            .WithColumn(nameof(Customer.Gender)).AsString(1000).Nullable()
            .WithColumn(nameof(Customer.Company)).AsString(1000).Nullable()
            .WithColumn(nameof(Customer.StreetAddress)).AsString(1000).Nullable()
            .WithColumn(nameof(Customer.StreetAddress2)).AsString(1000).Nullable()
            .WithColumn(nameof(Customer.ZipPostalCode)).AsString(1000).Nullable()
            .WithColumn(nameof(Customer.City)).AsString(1000).Nullable()
            .WithColumn(nameof(Customer.County)).AsString(1000).Nullable()
            .WithColumn(nameof(Customer.Phone)).AsString(1000).Nullable()
            .WithColumn(nameof(Customer.Fax)).AsString(1000).Nullable()
            .WithColumn(nameof(Customer.TimeZoneId)).AsString(1000).Nullable()
            .WithColumn(nameof(Customer.CustomCustomerAttributesXML)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(Customer.DateOfBirth)).AsDateTime2().Nullable()
            .WithColumn(nameof(Customer.SystemName)).AsString(400).Nullable()
            .WithColumn(nameof(Customer.LastIpAddress)).AsString(100).Nullable()
            .WithColumn(nameof(Customer.LanguageId)).AsInt32().ForeignKey<Language>(onDelete: Rule.SetNull).Nullable();
    }

    #endregion
}