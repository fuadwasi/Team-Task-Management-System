using AssetForge.Core.Domain.Customers;
using AssetForge.Core;
using Microsoft.AspNetCore.Mvc;
using AssetForge.Services.Security;
using DocumentFormat.OpenXml.EMMA;
using AssetForge.App.Models.Api;
using AssetForge.App.Models.Customer;
using AssetForge.App.Models.Teams;
using AssetForge.Core.Domain.Teams;
using AssetForge.Services.Teams;
using DocumentFormat.OpenXml.Wordprocessing;
using AssetForge.App.Areas.Admin.Models.Customers;
using AssetForge.App.Areas.Admin.Factories;

namespace AssetForge.App.Controllers.API
{
    [ApiController]
    [Route("api/Team")]
    public class TeamApiController : BaseApiController
    {
        private readonly IPermissionService _permissionService;
        private readonly ICustomerModelFactory _customerModelFactory;
        private readonly ITeamService _teamService;

        public TeamApiController(IPermissionService permissionService,
            ICustomerModelFactory customerModelFactory,
            ITeamService teamService)
        {
            _permissionService = permissionService;
            _customerModelFactory = customerModelFactory;
            _teamService = teamService;
        }

        [HttpGet("CreateTeam")]
        public virtual async Task<IActionResult> CreateTeam()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeam))
                return BadRequest();

            var model = new TeamModel();
            return OkWrap(model);
        }

        [HttpPost("CreateTeam")]
        public virtual async Task<IActionResult> CreateTeam([FromBody] BaseQueryModel<TeamModel> queryModel)
        {
            var model = queryModel.Data;
            var response = new GenericResponseModel<TeamModel>();
            var responseData = new TeamModel();

            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeam))
                return BadRequest();

            var team = new Team
            {
                Name = model.Name,
                Description = model.Description,
            };
            await _teamService.InsertTeamAsync(team);
            model.Id = team.Id;

            return OkWrap(model);
        }

        [HttpPost("EditTeam")]
        public virtual async Task<IActionResult> EditTeam([FromBody] BaseQueryModel<TeamModel> queryModel)
        {
            var model = queryModel.Data;

            var team = await _teamService.GetTeamByIdAsync(model.Id);
            if (team == null)
                return BadRequest();
            var response = new GenericResponseModel<TeamModel>();
            var responseData = new TeamModel();

            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeam))
                return BadRequest();

            team.Name = model.Name;
            team.Description = model.Description;
            await _teamService.UpdateTeamAsync(team);
            model.Id = team.Id;

            return OkWrap(model);
        }

        [HttpGet("ViewTeam")]
        public virtual async Task<IActionResult> ViewTeam([FromBody] BaseQueryModel<TeamModel> queryModel)
        {
            var model = queryModel.Data;

            var team = await _teamService.GetTeamByIdAsync(model.Id);
            if (team == null)
                return BadRequest();

            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeam))
                return BadRequest();

            model.Id = team.Id;
            model.Name = team.Name;
            model.Description = team.Description;
            return OkWrap(model);
        }

        [HttpGet("GetTeamList")]
        public virtual async Task<IActionResult> GetTeamList()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeam))
                return BadRequest();

            var searchModel = new TeamSearchModel();


            //prepare page parameters
            searchModel.SetGridPageSize();

            return OkWrap(searchModel);
        }

        [HttpPost("GetTeamList")]
        public virtual async Task<IActionResult> GetTeamList([FromBody] BaseQueryModel<TeamSearchModel> queryModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeam))
                return BadRequest();

            var model = queryModel.Data;

            var teams = await _teamService.GetAllTeamsAsync(
                keyword: model.Keyword,
                pageIndex: model.Page - 1,
                pageSize: model.PageSize);

            var teamModels = teams.Select(t => new TeamModel()
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
            }).ToList();

            return OkWrap(new TeamListModel() { Teams = teamModels });
        }

        [HttpGet("GetUsers")]
        public virtual async Task<IActionResult> GetUsers()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeam))
                return BadRequest();

            var model = await _customerModelFactory.PrepareCustomerSearchModelAsync(new CustomerSearchModel());

            return OkWrap(model);
        }

        [HttpPost("GetUsers")]
        public virtual async Task<IActionResult> GetUsers([FromBody] BaseQueryModel<CustomerSearchModel> queryModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeam))
                return BadRequest();

            var model = queryModel.Data;
            var customerList = await _customerModelFactory.PrepareCustomerListModelAsync(model);

            var data = new CustomerListTeamModel
            {
                Customers = customerList.Data.ToList()
            };

            return OkWrap(data);
        }
    }
}
