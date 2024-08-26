using AgentRest.Dto;
using AgentRest.Models;

namespace AgentRest.Services
{
    public interface IAgentService
    {
        Task<List<AgentModel>> GetAllAgentAsync();
        Task<AgentModel> CreateNewAgentAsync(AgentDto agentDto);
        Task<AgentModel> UpdateLocationAgentAsync(LocationDto locationDto, int id);
        Task<AgentModel> MoveAgentAsync(DirectionDto directionDto, int id);
        Task<AgentModel> GetAgentByIdAsync(int id); 
    }
}
