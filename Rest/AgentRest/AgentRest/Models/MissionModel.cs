namespace AgentRest.Models
{
    public class MissionModel
    {
        public int Id { get; set; }
        public int AgentID { get; set; }

        public AgentModel Agent { get; set; }
        public int TargetId { get; set; }
        public TargetModel Target { get; set; }
        public double TimeRemaining { get; set; }
        public int ActualExecutionTime { get; set; }

        public MissionStatus StatusMission { get; set; }
    }

    public enum MissionStatus
    {
        Proposed,
        Assigned,
        Completed
    }
}
