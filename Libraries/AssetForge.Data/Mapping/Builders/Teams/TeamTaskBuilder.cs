using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Teams;
using AssetForge.Data.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace AssetForge.Data.Mapping.Builders.Teams;

public partial class TeamTaskBuilder : EntityBuilder<TeamTask>
{
    #region Methods

    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(TeamTask.Title)).AsString(500).NotNullable()
            .WithColumn(nameof(TeamTask.Description)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(TeamTask.DoneOnUtc)).AsDateTime2().Nullable()
            .WithColumn(nameof(TeamTask.AssignedToTeamMemberMapId)).AsInt32().ForeignKey<TeamMemberMap>();
    }

    #endregion
}