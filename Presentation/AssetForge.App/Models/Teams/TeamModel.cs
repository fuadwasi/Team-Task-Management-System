using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Models.Teams
{
    public record TeamModel : BaseEntityModel
    {
        public TeamModel()
        {
            TeamMembers = new List<TeamMemberModel>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<TeamMemberModel> TeamMembers { get; set; }
    }
    public record TeamListModel : BaseModel
    {
        public TeamListModel()
        {
            Teams = new List<TeamModel>();
        }
        public IList<TeamModel> Teams { get; set; }
    }
}
