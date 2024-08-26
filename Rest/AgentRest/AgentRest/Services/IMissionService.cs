﻿using AgentRest.Dto;
using AgentRest.Models;

namespace AgentRest.Services
{
    public interface IMissionService
    {
        void CreateMissionByAgentAsync(AgentModel agentModel);
        void CreateMissionByTargetAsync(TargetModel targetModel);
        void IfMissionIsRrelevantAsync();

        Task AgentsPursuitAsync();

        Task<List<MissionModel>> GetAllMissionAsync();
        Task<MissionModel> CommandmentToMissionAsync(int id);

        Task<List<MissionDto>> GetAllAsync();
    }
}
