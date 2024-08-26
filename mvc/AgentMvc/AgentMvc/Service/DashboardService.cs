using AgentMvc.Models;
using AgentMvc.ViewModel;

namespace AgentMvc.Service
{
    public class DashboardService(IMissionService missionService, IAgentService agentService, ITargetService targetService) : IDashboardService
    {
        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var agents = await agentService.GetAllAgentsAsync();
            var targets = await targetService.GetAllTaretsAsync();
            var missions = await missionService.GetAllMissionsAsync();
            return new DashboardViewModel
            {
                TotalAgents = agents.Count,
                ActiveAgents = agents.Count(a => a.StatusAgent == StatusAgent.IsActive),
                TotalTargets = targets.Count,
                EliminatedTargets = targets.Count(t => t.StatusTarget == StatusTarget.Dead),
                TotalMissions = missions.Count,
                ActiveMissions = missions.Count(m => m.StatusMission == MissionStatus.Assigned),
                AgentToTargetRatio = (double)agents.Count / targets.Count,
                AvailableAgentToTargetRatio = (double)agents.Count(a => a.StatusAgent == StatusAgent.IsNnotActive) /
                                         targets.Count(t => t.StatusTarget == StatusTarget.Live)
            };
        }
    }
}
