using AgentRest.Dto;
using AgentRest.Models;

namespace AgentRest.Services
{
    public interface ITargetService
    {
        Task<List<TargetModel>> GetAllTargetAsync();
        Task<TargetModel> CreateNewTargetAsync(TargetDto targetDto);
        Task<TargetModel> UpdateLocationTargetAsync(LocationDto locationDto, int id);
        Task<TargetModel> MoveTargetAsync(DirectionDto directionDto, int id);
    }
}
