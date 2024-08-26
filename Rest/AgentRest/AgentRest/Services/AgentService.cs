
using AgentRest.Dto;
using AgentRest.Models;
using AgentsRest.Date;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static AgentRest.Utils.Calculate;

namespace AgentRest.Services
{
    public class AgentService(ApplicationDbContext context, IServiceProvider serviceProvider, IMissionService missionService) : IAgentService
    {

        private readonly Dictionary<string, (int x, int y)> xy = new()
        {
              {"nw", (-1, 1)},
              {"n", (0, 1)},
              {"ne", (1, 1)},
              {"w", (-1, 0)},
              {"e", (1, 0)},
              {"sw", (-1, -1)},
              {"s", (0, -1)},
              {"se", (1, -1)}
        };

        public async Task<AgentModel> CreateNewAgentAsync(AgentDto agentDto)
        {
            if (agentDto == null)
            {
                throw new Exception("The object is empty");
            }
            AgentModel agentModel = new()
            {
                Image = agentDto.photoUrl,
                NickName = agentDto.nickname,
                X = -1,
                Y = -1,
                StatusAgent = StatusAgent.IsNnotActive,

            };
            await context.AddAsync(agentModel);
            await context.SaveChangesAsync();
            return agentModel;
        }

        public async Task<AgentModel> GetAgentByIdAsync(int id)
        {
            var agent = await context.Agents.FindAsync(id);
            if (agent == null)
            {
                throw new Exception();
            }
            return agent;
        }

        public async Task<List<AgentModel>> GetAllAgentAsync()=> await context.Agents.ToListAsync();
       

        public async Task<AgentModel> MoveAgentAsync(DirectionDto directionDto, int id)
        {
            var agentModel = await context.Agents.FirstOrDefaultAsync(x => x.Id == id);
            if (agentModel == null) { throw new Exception($"not find Agent by id {id}"); }
            if (agentModel.StatusAgent == StatusAgent.IsNnotActive)
            {
                var a = xy.TryGetValue(directionDto.Diretion, out var risult);
                if (!a) { throw new Exception($"The direction '{directionDto.Diretion}' is not correct"); }
                var (x, y) = risult;
                var IfDirectionInRange = IsInRange1000(agentModel.X += x, agentModel.Y += y);
                if (!IfDirectionInRange) { throw new Exception($"Locations out of range of the clipboard"); }
                agentModel.X += x;
                agentModel.Y += y;
                await context.SaveChangesAsync();
                missionService.CreateMissionByAgentAsync(agentModel);
                missionService.IfMissionIsRrelevantAsync();

            }
            return agentModel;

        }

        public async Task<AgentModel> UpdateLocationAgentAsync(LocationDto locationDto, int id)
        {
            var isWithRange = IsInRange1000(locationDto.X, locationDto.Y);
            if (!isWithRange)
            {
                throw new Exception();
            }
            var agentModel = await context.Agents.FirstOrDefaultAsync(a => a.Id == id);
            if (agentModel == null) 
            {
                throw new Exception();
            }
            agentModel.X = locationDto.X;
            agentModel.Y = locationDto.Y;
            await context.SaveChangesAsync();
            missionService.CreateMissionByAgentAsync(agentModel);
            return agentModel;
        }
    }

      
    }
