using System.ComponentModel.DataAnnotations.Schema;

namespace AgentMvc.Models
{
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
