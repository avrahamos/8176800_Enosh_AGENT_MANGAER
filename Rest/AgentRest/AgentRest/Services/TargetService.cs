
using AgentRest.Dto;
using AgentRest.Models;
using AgentRest.Utils;
using AgentsRest.Date;
using Microsoft.EntityFrameworkCore;

namespace AgentRest.Services
{
    public class TargetService(IMissionService missionService, ApplicationDbContext context) : ITargetService
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

        public async Task<TargetModel> CreateNewTargetAsync(TargetDto targetDto)
        {
            if (targetDto == null)
            {
                throw new ArgumentNullException(nameof(targetDto));
            }
            var target = new TargetModel
            {
                Image = targetDto.photoUrl,

                Name = targetDto.name,

                x = -1,
                y = -1,
                position = targetDto.position,
                StatusTarget = StatusTarget.Live

            };
            await context.AddAsync(target);
            await context.SaveChangesAsync();
            return target;
        }

        public async Task<List<TargetModel>> GetAllTargetAsync()=> await context.Targets.ToListAsync();
        

        public async Task<TargetModel> MoveTargetAsync(DirectionDto directionDto, int id)
        {
            var target = await context.Targets.FirstOrDefaultAsync(x => x.Id == id);
            if (target == null)
            {
                throw new Exception();
            }
            var IfDirectionExists = xy.TryGetValue(directionDto.Diretion, out var risult);
            if (!IfDirectionExists) { throw new Exception($"The direction '{directionDto.Diretion}' is not correct"); }
            var (x, y) = risult;
            var IfDirectionInRange =Calculate.IsInRange1000(target.x += x, target.y += y);
            if (!IfDirectionInRange) { throw new Exception($"Locations out of range of the clipboard"); }
            target.x += x;
            target.y += y;
            await context.SaveChangesAsync();
            missionService.CreateMissionByTargetAsync(target);
            missionService.IfMissionIsRrelevantAsync();
            return target;
        }

        public async Task<TargetModel> UpdateLocationTargetAsync(LocationDto locationDto, int id)
        {
            var IfDirectionInRange =Calculate.IsInRange1000(locationDto.X, locationDto.Y);
            if (!IfDirectionInRange) { throw new Exception($"Locations out of range of the clipboard"); }
            
            var targetModel = await context.Targets.FirstOrDefaultAsync(x => x.Id == id);
            if (targetModel == null) { throw new Exception($"not find targett by id {id}"); }

            targetModel.x = locationDto.X;
            targetModel.y = locationDto.Y;

            await context.SaveChangesAsync();
            missionService.CreateMissionByTargetAsync(targetModel);
            return targetModel;
        }
    }
}