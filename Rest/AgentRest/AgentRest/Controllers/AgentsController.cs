using AgentRest.Dto;
using AgentRest.Models;
using AgentRest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AgentsController(IAgentService agentService) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromBody] AgentDto agentDto)
        {
            try
            {
                var agentModel = await agentService.CreateNewAgentAsync(agentDto);
                return Created("new user", new IdDto { Id = agentModel.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetAllAgent() =>
            Ok(await agentService.GetAllAgentAsync());

        [HttpPut("{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateLocationAgent(LocationDto locationDto, int id)
        {
            try
            {
                await agentService.UpdateLocationAgentAsync(locationDto, id);
                return Created("new user", locationDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> MoveAgent(DirectionDto directionDto, int id)
        {
            try
            {
                await agentService.MoveAgentAsync(directionDto, id);
                return Created("new user", directionDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAgentById(int id)
        {
            try
            {
                var agent = await agentService.GetAgentByIdAsync(id);
                return Ok(agent);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
