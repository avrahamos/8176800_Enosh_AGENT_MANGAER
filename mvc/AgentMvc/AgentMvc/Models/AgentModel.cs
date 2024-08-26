using System.ComponentModel.DataAnnotations.Schema;

namespace AgentMvc.Models
{
    public class AgentModel
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string NickName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public StatusAgent StatusAgent { get; set; }
        [NotMapped]
        public List<MissionModel> Missions { get; set; } = [];


    }
    public enum StatusAgent
    {
        IsActive,
        IsNnotActive
    }
}

