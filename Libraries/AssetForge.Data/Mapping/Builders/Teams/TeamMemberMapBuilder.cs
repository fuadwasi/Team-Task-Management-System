using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Teams;
using AssetForge.Data.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace AssetForge.Data.Mapping.Builders.Teams;

public partial class TeamMemberMapBuilder : EntityBuilder<TeamMemberMap>
{
    #region Methods

    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(TeamMemberMap.RoleTypeId)).AsInt32().NotNullable()
            .WithColumn(nameof(TeamMemberMap.TeamId)).AsInt32().ForeignKey<Team>()
            .WithColumn(nameof(TeamMemberMap.CustomerId)).AsInt32().ForeignKey<Customer>();
    }

    #endregion
}
