using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetForge.Core.Domain.Teams
{
    public class TeamTask : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public int AssignedToUserId { get; set; }
        public int CreatedByUserId { get; set; }
        public int TeamId { get; set; }
        public DateTime DueDateUtc { get; set; }

        public DateTime CreatedDateUtc { get; set; }
        public DateTime? DoneOnUtc { get; set; }
    }
}
