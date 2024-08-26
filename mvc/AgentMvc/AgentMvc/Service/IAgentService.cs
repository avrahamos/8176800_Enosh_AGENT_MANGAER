using AgentMvc.Models;

namespace AgentMvc.Service
{
    public interface IAgentService
    {
        Task<List<AgentModel?>> GetAllAgentsAsync();
        Task<AgentModel?>Details(int agentId);
    }
}
