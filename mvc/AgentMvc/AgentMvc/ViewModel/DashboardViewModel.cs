namespace AgentMvc.ViewModel
{
    public class DashboardViewModel
    {
        public int TotalAgents { get; set; }
        public int ActiveAgents { get; set; }
        public int TotalTargets { get; set; }
        public int EliminatedTargets { get; set; }
        public int TotalMissions { get; set; }
        public int ActiveMissions { get; set; }
        public double AgentToTargetRatio { get; set; }
        public double AvailableAgentToTargetRatio { get; set; }
    }
}
