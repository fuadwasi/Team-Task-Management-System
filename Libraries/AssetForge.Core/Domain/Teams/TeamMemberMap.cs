namespace AssetForge.Core.Domain.Teams
{
    public class TeamMemberMap : BaseEntity
    {
        public int CustomerId { get; set; }
        public int TeamId { get; set; }

        public int RoleTypeId { get; set; }

        public RoleType RoleType { 
            get => (RoleType) RoleTypeId; 
            set => RoleTypeId = (int) value; 
        }
    }
}
