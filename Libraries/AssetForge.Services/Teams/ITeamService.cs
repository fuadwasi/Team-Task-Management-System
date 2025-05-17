using AssetForge.Core;
using AssetForge.Core.Domain.Teams;

namespace AssetForge.Services.Teams
{
    public interface ITeamService
    {
        #region Team  

        Task DeleteTeamAsync(Team team);
        Task DeleteTeamAsync(IList<Team> teams);
        Task InsertTeamAsync(Team team);
        Task UpdateTeamAsync(Team team);
        Task<Team> GetTeamByIdAsync(int teamId);
        Task<IList<Team>> GetTeamsByIdsAsync(int[] teamIds);
        Task<IPagedList<Team>> GetAllTeamsAsync(string keyword = null, string userEmail = null, string userName = null, IList<int> teamIds = null, int pageIndex = 0, int pageSize = int.MaxValue);

        #endregion

        #region Team Member Map  

        Task DeleteTeamMemberMapAsync(TeamMemberMap teamMemberMap);
        Task DeleteTeamMemberMapAsync(IList<TeamMemberMap> teamMemberMaps);
        Task InsertTeamMemberMapAsync(TeamMemberMap teamMemberMap);
        Task UpdateTeamMemberMapAsync(TeamMemberMap teamMemberMap);
        Task<TeamMemberMap> GetTeamMemberMapByIdAsync(int teamMemberMapId);
        Task<IList<TeamMemberMap>> GetTeamMemberMapsByIdsAsync(int[] teamMemberMapIds);
        Task<IPagedList<TeamMemberMap>> GetAllTeamMemberMapsAsync(string keyword = null, string userEmail = null, string userName = null, IList<int> teamIds = null, int pageIndex = 0, int pageSize = int.MaxValue);

        #endregion

        #region Team Task  

        Task DeleteTeamTaskAsync(TeamTask teamTask);
        Task DeleteTeamTaskAsync(IList<TeamTask> teamTasks);
        Task InsertTeamTaskAsync(TeamTask teamTask);
        Task UpdateTeamTaskAsync(TeamTask teamTask);
        Task<TeamTask> GetTeamTaskByIdAsync(int teamTaskId);
        Task<IList<TeamTask>> GetTeamTasksByIdsAsync(int[] teamTaskIds);
        Task<IPagedList<TeamTask>> GetAllTeamTasksAsync(string keyword = null, string userEmail = null, string userName = null, IList<int> teamIds = null, int? assignToUserId = null, int? createdByUserId = null, int pageIndex = 0, int pageSize = int.MaxValue);

        #endregion
    }
}
