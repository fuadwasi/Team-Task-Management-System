using FluentMigrator.Builders.Create.Table;
using AssetForge.Core.Domain.Common;
using AssetForge.Core.Domain.Directory;
using AssetForge.Data.Extensions;
using System.Data;

namespace AssetForge.Data.Mapping.Builders.Common;

/// <summary>
/// Represents a address entity builder
/// </summary>
public partial class AddressBuilder : EntityBuilder<Address>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Address.CountryId)).AsInt32().Nullable().ForeignKey<Country>(onDelete: Rule.None)
            .WithColumn(nameof(Address.StateProvinceId)).AsInt32().Nullable().ForeignKey<StateProvince>(onDelete: Rule.None);
    }

    #endregion
}