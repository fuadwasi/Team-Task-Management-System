using FluentMigrator.Builders.Create.Table;
using AssetForge.Core.Domain.Media;
using AssetForge.Data.Extensions;

namespace AssetForge.Data.Mapping.Builders.Media;

/// <summary>
/// Represents a picture binary entity builder
/// </summary>
public partial class PictureBinaryBuilder : EntityBuilder<PictureBinary>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(PictureBinary.PictureId)).AsInt32().ForeignKey<Picture>();
    }

    #endregion
}