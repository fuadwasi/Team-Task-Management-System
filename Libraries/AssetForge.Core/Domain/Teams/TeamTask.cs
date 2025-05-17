namespace AssetForge.Core.Domain.Teams
{
    public class TeamTask : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssignedToTeamMemberMapId { get; set; }
        public int TaskStatusId { get; set; }
        public int CreatedByUserId { get; set; }
        //public int TeamId { get; set; }
        public DateTime DueDateUtc { get; set; }

        public DateTime CreatedDateUtc { get; set; }
        public DateTime? DoneOnUtc { get; set; }

        public TeamTaskStatus TaskStatus { 
            get => (TeamTaskStatus) TaskStatusId; 
            set => TaskStatusId = (int)TaskStatus; 
        }
    }
}
