﻿
using AgentRest.Dto;
using AgentRest.Models;
using AgentRest.Utils;
using AgentsRest.Date;
using Microsoft.EntityFrameworkCore;

namespace AgentRest.Services
{
    public class MissionService(IDbContextFactory<ApplicationDbContext> dbContextFactory) : IMissionService
    {
        public async Task AgentsPursuitAsync()
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var missions = dbContext.Missions.Where(x => x.StatusMission == MissionStatus.Assigned).ToList();
            foreach (var mission in missions)
            {
                TargetModel? target = await dbContext.Targets.FindAsync(mission.TargetId);
                AgentModel? agent = await dbContext.Agents.FindAsync(mission.AgentID);
                if (target != null && agent != null)
                {
                    agent.X = target.x > agent.X ? agent.X + 1 : agent.X;
                    agent.Y = target.y > agent.Y ? agent.Y + 1 : agent.Y;
                    agent.X = target.x < agent.X ? agent.X - 1 : agent.X;
                    agent.Y = target.y < agent.Y ? agent.Y - 1 : agent.Y;
                    if (agent.X == target.x && agent.Y == target.y)
                    {
                        mission.StatusMission = MissionStatus.Completed;
                        target.StatusTarget = StatusTarget.Dead;
                        agent.StatusAgent = StatusAgent.IsNnotActive;
                        mission.TimeRemaining = 0;
                    }
                    var IfDirectionInRange =Calculate.IsInRange1000(agent.X, agent.Y);
                    if (!IfDirectionInRange) { throw new Exception($"Locations out of range of the clipboard"); }
                    mission.TimeRemaining =Calculate.DistanceCalculation(agent.X, agent.Y, target.x, target.y) / 5;
                    await dbContext.SaveChangesAsync();
                }

            }

        }

        public async Task<MissionModel> CommandmentToMissionAsync(int id)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var mission = await dbContext.Missions
         .Include(m => m.Agent)
         .Include(m => m.Target)
         .FirstOrDefaultAsync(m => m.Id == id);


            AgentModel? agent = await dbContext.Agents.FindAsync(mission?.AgentID);
            var agentInMission = dbContext.Missions.Where(x => x.AgentID == agent.Id && x.StatusMission == MissionStatus.Assigned).ToList();
            if (agentInMission.Any())
            { 
                throw new Exception($"The agent on the id {id} is already in action");
            }
            var tergetInmission = dbContext.Missions.Where(x => x.TargetId == mission.TargetId && x.StatusMission == MissionStatus.Assigned).ToList();
            if (tergetInmission.Any()) 
            { 
                throw new Exception($"The target on the id {id} is already in action"); 
            }

            if (agent == null || mission == null) { throw new Exception($"not find by id {id}"); }
            agent.StatusAgent = StatusAgent.IsActive;
            mission.StatusMission = MissionStatus.Assigned;
            await dbContext.SaveChangesAsync();
            return mission;

        }

        public async void CreateMissionByAgentAsync(AgentModel agentModel)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var targetlive = dbContext.Targets.Where(x => x.StatusTarget == StatusTarget.Live).ToList();


            foreach (var target in targetlive)
            {
                int xA = agentModel.X;
                int xT = target.x;
                int yA = agentModel.Y;
                int yT = target.y;

                var ifExistsAMission = dbContext.Missions.Any(x => x.TargetId == target.Id && x.AgentID == agentModel.Id);
                var distance =Calculate.DistanceCalculation(xA, yA, xT, yT);

                var targetInMission = dbContext.Missions.Where(x => x.TargetId == target.Id).ToList();

                targetInMission = targetInMission.Where(x => x.StatusMission == MissionStatus.Assigned).ToList();

                if (distance <= 200 && !targetInMission.Any() && !ifExistsAMission)
                {
                    MissionModel newModel = new()
                    {
                        AgentID = agentModel.Id,
                        TargetId = target.Id,
                        TimeRemaining = distance / 5,
                        StatusMission = MissionStatus.Proposed,
                    };
                    await dbContext.AddAsync(newModel);
                    await dbContext.SaveChangesAsync();
                }
            }

        }

        public async void CreateMissionByTargetAsync(TargetModel targetModel)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var agentsNotActive = dbContext.Agents.Where(x => x.StatusAgent == StatusAgent.IsNnotActive).ToList();


            foreach (var agent in agentsNotActive)
            {
                int xT = targetModel.x;
                int xA = agent.X;
                int yT = targetModel.y;
                int yA = agent.Y;

                var ifExistsAMission = dbContext.Missions.Any(x => x.TargetId == targetModel.Id && x.AgentID == agent.Id);

                var distance =Calculate.DistanceCalculation(xA, yA, xT, yT);

                var agentsInMission = dbContext.Missions.Where(x => x.AgentID == agent.Id).ToList();

                agentsInMission = agentsInMission.Where(x => x.StatusMission == MissionStatus.Assigned).ToList();

                if (distance <= 200 && !agentsInMission.Any() && !ifExistsAMission)
                {
                    MissionModel newModel = new()
                    {
                        AgentID = agent.Id,
                        TargetId = targetModel.Id,
                        TimeRemaining = distance / 5,
                        StatusMission = MissionStatus.Proposed,
                    };
                    await dbContext.AddAsync(newModel);
                    await dbContext.SaveChangesAsync();
                }
            }

        }

        public async Task<List<MissionModel>> GetAllMissionAsync()
        {
            var db = await dbContextFactory.CreateDbContextAsync();
            return await db.Missions.ToListAsync();
        }

        public async Task<List<MissionDto>> GetAllAsync()
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var a = await dbContext.Missions.Include(x => x.Agent).Include(x => x.Target).ToListAsync();
            List<MissionDto> missions = new List<MissionDto>();
            foreach (MissionModel mission in a)
            {
                missions.Add(new MissionDto()
                {
                    ImageA = mission.Agent.Image,
                    NickName = mission.Agent.NickName,
                    xA = mission.Agent.X,
                    yA = mission.Agent.Y,
                    StatusAgent = mission.Agent.StatusAgent,
                    AgentID = mission.AgentID,
                    TargetId = mission.TargetId,
                    TimeRemaining = mission.TimeRemaining,
                    ActualExecutionTime = mission.ActualExecutionTime,
                    StatusMission = mission.StatusMission,
                    ImageT = mission.Target.Image,
                    Name = mission.Target.Name,
                    position = mission.Target.position,
                    xT = mission.Target.x,
                    yT = mission.Target.y,
                    StatusTarget = mission.Target.StatusTarget,
                });
            }
            return missions;
        }

        public async void IfMissionIsRrelevantAsync()
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var MissionOffer = dbContext.Missions.Where(x => x.StatusMission == MissionStatus.Proposed).ToList();

            foreach (var mission in MissionOffer)
            {
                TargetModel? target = await dbContext.Targets.FindAsync(mission.TargetId);
                AgentModel? agent = await dbContext.Agents.FindAsync(mission.AgentID);

                var distance =Calculate.DistanceCalculation(agent.X, agent.Y, target.x, target.y);
                if (distance > 200)
                {
                    dbContext.Remove(mission);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
        public async Task<MissionModel> CreateAndAssignMissionAsync(int agentId, int targetId)
        {
            var context = await dbContextFactory.CreateDbContextAsync();
            var agent = await context.Agents.FindAsync(agentId);
            var target = await context.Targets.FindAsync(targetId);

            if (agent == null)
                throw new Exception($"Agent with id {agentId} not found");
            if (target == null)
                throw new Exception($"Target with id {targetId} not found");

            if (agent.StatusAgent != StatusAgent.IsNnotActive)
                throw new Exception($"Agent {agent.NickName} is not available for assignment");

            if (target.StatusTarget != StatusTarget.Live)
                throw new Exception($"Target {target.Name} is not available for assignment");

            double distance = Calculate.DistanceCalculation(agent.X, agent.Y, target.x, target.y);
            if (distance > 200) 
                throw new Exception("Agent and target are not within range for assignment");

            var mission = new MissionModel
            {
                AgentID = agentId,
                TargetId = targetId,
                StatusMission = MissionStatus.Assigned,
                TimeRemaining = distance / 5, 
                ActualExecutionTime = 0,
                Agent = agent,
                Target = target
            };

            agent.StatusAgent = StatusAgent.IsActive;
            target.StatusTarget = StatusTarget.Live;

            context.Missions.Add(mission);

            await context.SaveChangesAsync();

            return mission;
        }

        public async Task<MissionModel> GetMissionByIdAsinc(int id)
        {
            var context = await dbContextFactory.CreateDbContextAsync();
            var mission = await context.Missions.FindAsync(id);
            if (mission == null)
            {
                throw new KeyNotFoundException($"Mission with id {id} does not found");
            }
            return mission;
        }
    }
}