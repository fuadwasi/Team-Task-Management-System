using AssetForge.Core;
using AssetForge.Core.Caching;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Teams;
using AssetForge.Data;
using AssetForge.Services.Localization;
using AssetForge.Services.Logging;

namespace AssetForge.Services.Teams
{
    public class TeamService : ITeamService
    {

        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Team> _teamRepository;
        private readonly IRepository<TeamMemberMap> _teamMemberMapRepository;
        private readonly IRepository<TeamTask> _teamTaskRepository;
        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        public TeamService(
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            IRepository<Customer> customerRepository,
            IRepository<Team> teamRepository,
            IRepository<TeamTask> teamTaskRepository,
            IRepository<TeamMemberMap> teamMemberMapRepository,
            IStaticCacheManager staticCacheManager)
        {
            _customerActivityService = customerActivityService;
            _localizationService = localizationService;
            _customerRepository = customerRepository;
            _teamRepository = teamRepository;
            _teamTaskRepository = teamTaskRepository;
            _teamMemberMapRepository = teamMemberMapRepository;
            _staticCacheManager = staticCacheManager;
        }

        #endregion

        #region Methods

        #region team

        public async Task DeleteTeamAsync(Team team)
        {
            await _teamRepository.DeleteAsync(team);
        }

        public async Task DeleteTeamAsync(IList<Team> teams)
        {
            ArgumentNullException.ThrowIfNull(teams);

            await _teamRepository.DeleteAsync(teams);
        }

        public async Task InsertTeamAsync(Team team)
        {
            await _teamRepository.InsertAsync(team);
        }

        public async Task UpdateTeamAsync(Team team)
        {
            await _teamRepository.UpdateAsync(team);
        }

        public async Task<Team> GetTeamByIdAsync(int teamId)
        {
            if (teamId == 0)
                return null;

            return await _teamRepository.GetByIdAsync(teamId, cache => default);
        }

        public virtual async Task<IList<Team>> GetTeamsByIdsAsync(int[] teamIds)
        {
            return await _teamRepository.GetByIdsAsync(teamIds, includeDeleted: false);
        }


        public async Task<IPagedList<Team>> GetAllTeamsAsync(string keyword = null, string userEmail = null,  string userName = null, IList<int> teamIds = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _teamRepository.Table;

            if (teamIds != null && teamIds.Any())
                query = query.Where(q => teamIds.Contains(q.Id));

            if(!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));

            if (!string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(userEmail))
            {
                var customerQuery = _customerRepository.Table;

                if (!string.IsNullOrEmpty(userName))
                    customerQuery = customerQuery.Where(x => x.Username.Contains(userName));

                if (!string.IsNullOrEmpty(userEmail))
                    customerQuery = customerQuery.Where(x => x.Email.Contains(userEmail));

                query = from q in query
                        join tm in _teamMemberMapRepository.Table on q.Id equals tm.TeamId
                        join c in customerQuery on tm.CustomerId equals c.Id
                        select q;
            }

            query = query.OrderByDescending(q => q.Id);

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        #endregion

        #region Team Member Map

        public async Task DeleteTeamMemberMapAsync(TeamMemberMap teamMemberMap)
        {
            await _teamMemberMapRepository.DeleteAsync(teamMemberMap);
        }

        public async Task DeleteTeamMemberMapAsync(IList<TeamMemberMap> teamMemberMaps)
        {
            ArgumentNullException.ThrowIfNull(teamMemberMaps);

            await _teamMemberMapRepository.DeleteAsync(teamMemberMaps);
        }

        public async Task InsertTeamMemberMapAsync(TeamMemberMap teamMemberMap)
        {
            await _teamMemberMapRepository.InsertAsync(teamMemberMap);
        }

        public async Task UpdateTeamMemberMapAsync(TeamMemberMap teamMemberMap)
        {
            await _teamMemberMapRepository.UpdateAsync(teamMemberMap);
        }

        public async Task<TeamMemberMap> GetTeamMemberMapByIdAsync(int teamMemberMapId)
        {
            if (teamMemberMapId == 0)
                return null;

            return await _teamMemberMapRepository.GetByIdAsync(teamMemberMapId, cache => default);
        }

        public virtual async Task<IList<TeamMemberMap>> GetTeamMemberMapsByIdsAsync(int[] teamMemberMapIds)
        {
            return await _teamMemberMapRepository.GetByIdsAsync(teamMemberMapIds, includeDeleted: false);
        }

        public async Task<IPagedList<TeamMemberMap>> GetAllTeamMemberMapsAsync(string keyword = null, string userEmail = null, string userName = null, IList<int> teamIds = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var teamQuery = _teamRepository.Table;

            if (teamIds != null && teamIds.Any())
                teamQuery = teamQuery.Where(q => teamIds.Contains(q.Id));

            if (!string.IsNullOrEmpty(keyword))
                teamQuery = teamQuery.Where(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));

            var customerQuery = _customerRepository.Table;

            if (!string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(userEmail))
            {

                if (!string.IsNullOrEmpty(userName))
                    customerQuery = customerQuery.Where(x => x.Username.Contains(userName));

                if (!string.IsNullOrEmpty(userEmail))
                    customerQuery = customerQuery.Where(x => x.Email.Contains(userEmail));
            }


            var teamMemberQuery = from q in teamQuery
                        join tm in _teamMemberMapRepository.Table on q.Id equals tm.TeamId
                        join c in customerQuery on tm.CustomerId equals c.Id
                        select tm;

            teamMemberQuery = teamMemberQuery.OrderByDescending(q => q.Id);

            return await teamMemberQuery.ToPagedListAsync(pageIndex, pageSize);
        }

        #endregion

        #region Team Member Map

        public async Task DeleteTeamTaskAsync(TeamTask teamTask)
        {
            await _teamTaskRepository.DeleteAsync(teamTask);
        }

        public async Task DeleteTeamTaskAsync(IList<TeamTask> teamTasks)
        {
            ArgumentNullException.ThrowIfNull(teamTasks);

            await _teamTaskRepository.DeleteAsync(teamTasks);
        }

        public async Task InsertTeamTaskAsync(TeamTask teamTask)
        {
            await _teamTaskRepository.InsertAsync(teamTask);
        }

        public async Task UpdateTeamTaskAsync(TeamTask teamTask)
        {
            await _teamTaskRepository.UpdateAsync(teamTask);
        }

        public async Task<TeamTask> GetTeamTaskByIdAsync(int teamTaskId)
        {
            if (teamTaskId == 0)
                return null;

            return await _teamTaskRepository.GetByIdAsync(teamTaskId, cache => default);
        }

        public virtual async Task<IList<TeamTask>> GetTeamTasksByIdsAsync(int[] teamTaskIds)
        {
            return await _teamTaskRepository.GetByIdsAsync(teamTaskIds, includeDeleted: false);
        }

        public async Task<IPagedList<TeamTask>> GetAllTeamTasksAsync(string keyword = null, string userEmail = null, string userName = null, IList<int> teamIds = null,
            int? assignToUserId = null, int? createdByUserId = null, 
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var teamQuery = _teamRepository.Table;
            var teamTaskQuery = _teamTaskRepository.Table;

            if (teamIds != null && teamIds.Any())
                teamQuery = teamQuery.Where(q => teamIds.Contains(q.Id));

            if (!string.IsNullOrEmpty(keyword))
                teamTaskQuery = teamTaskQuery.Where(x => x.Title.Contains(keyword) || x.Description.Contains(keyword));

            if(createdByUserId.HasValue)
                teamTaskQuery = teamTaskQuery.Where(x => x.CreatedByUserId == createdByUserId.Value);

            var customerQuery = _customerRepository.Table;

            if (!string.IsNullOrEmpty(userName))
                customerQuery = customerQuery.Where(x => x.Username.Contains(userName));

            if (!string.IsNullOrEmpty(userEmail))
                customerQuery = customerQuery.Where(x => x.Email.Contains(userEmail));

            if(assignToUserId.HasValue)
                customerQuery = customerQuery.Where(x => x.Id == assignToUserId.Value);

            teamTaskQuery = from q in teamTaskQuery
                            join tmm in _teamMemberMapRepository.Table on q.AssignedToTeamMemberMapId equals tmm.Id
                            join tq in teamQuery on tmm.TeamId equals tq.Id
                            join c in customerQuery on tmm.CustomerId equals c.Id
                            select q;


            teamTaskQuery = teamTaskQuery.OrderByDescending(q => q.Id);

            return await teamTaskQuery.ToPagedListAsync(pageIndex, pageSize);
        }

        #endregion

        #endregion
    }
}
