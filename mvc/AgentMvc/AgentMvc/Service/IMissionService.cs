using AgentMvc.Models;

namespace AgentMvc.Service
{
    public interface IMissionService
    {
        Task<List<MissionModel>> GetAllMissionsAsync();
        Task AgentsPursuitAsync();
        Task<MissionModel?> CommandmentToMissionAsync(int id);
        Task<MissionModel?> CreateAndAssignMissionAsync(int agentId, int targetId);
        Task<MissionModel?> GetMissionByIdAsync(int id);
    }
}
