using AgentRest.Dto;
using AgentRest.Models;
using AgentRest.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgentRest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MissionsController(IMissionService missionService) : ControllerBase
    {
        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AgentsPursuit()
        {
            await missionService.AgentsPursuitAsync();
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetAllMission() =>
            Ok(await missionService.GetAllMissionAsync());
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateStatusMission(int id)
        {

            try
            {
                MissionModel missionModel = await missionService.CommandmentToMissionAsync(id);
                return Created("new user", missionModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetAll() =>
            Ok(await missionService.GetAllAsync());

        [HttpPost("assign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AssignMission([FromBody] MissionDto dto)
        {
            try
            {
                var mission = await missionService.CreateAndAssignMissionAsync(dto.AgentID , dto.TargetId);
                return Ok(mission);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}