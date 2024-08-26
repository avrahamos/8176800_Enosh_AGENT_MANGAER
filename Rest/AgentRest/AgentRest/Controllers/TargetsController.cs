using AgentRest.Dto;
using AgentRest.Models;
using AgentRest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TargetsController(ITargetService targetService) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateTarget([FromBody] TargetDto targetDto)
        {
            try
            {
                var targetModel = await targetService.CreateNewTargetAsync(targetDto);
                return Created("new user", new IdDto { Id = targetModel.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetAllTarget() =>
            Ok(await targetService.GetAllTargetAsync());

        [HttpPut("{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateLocationTarget(LocationDto locationDto, int id)
        {
            try
            {
                await targetService.UpdateLocationTargetAsync(locationDto, id);
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
        public async Task<ActionResult> moveTarget(DirectionDto directionDto, int id)
        {
            try
            {
                await targetService.MoveTargetAsync(directionDto, id);
                return Created("new user", directionDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
