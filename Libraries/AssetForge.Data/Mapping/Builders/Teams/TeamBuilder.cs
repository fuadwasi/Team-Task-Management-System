using AssetForge.Core.Domain.Teams;
using FluentMigrator.Builders.Create.Table;

namespace AssetForge.Data.Mapping.Builders.Teams;

public partial class TeamBuilder : EntityBuilder<Team>
{
    #region Methods

    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Team.Name)).AsString(500).NotNullable()
            .WithColumn(nameof(Team.Description)).AsString(int.MaxValue).Nullable();
    }

    #endregion
}