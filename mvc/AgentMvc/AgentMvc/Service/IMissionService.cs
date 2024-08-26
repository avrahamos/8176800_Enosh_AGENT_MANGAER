using AgentMvc.Models;

namespace AgentMvc.Service
{
    public interface IMissionService
    {
        Task<List<MissionModel>> GetAllMissionsAsync();
        Task AssignMissionAsync(int agentId, int targetId);

    }
}
