using System.ComponentModel.DataAnnotations.Schema;

namespace AgentRest.Models
{
    public enum IsLiving
    {
        Living = 1,
        Ddead = 2,
    }

    public enum TargetStatus
    {
        Available,
        Assigned,
        Eliminated
    }

    public class TargetModel
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string position { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public StatusTarget StatusTarget { get; set; }
        [NotMapped]
        public MissionModel Mission { get; set; }
    }
    public enum StatusTarget
    {
        Live,
        Dead
    }
}
