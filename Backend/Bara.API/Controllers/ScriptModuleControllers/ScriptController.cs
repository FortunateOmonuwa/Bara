using Microsoft.AspNetCore.Mvc;
using ScriptModule.DTOs;
using ScriptModule.Interfaces;
using SharedModule.Utils;

namespace Bara.API.Controllers.ScriptModuleControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScriptController : ControllerBase
    {
        private readonly IScriptService scriptService;
        private readonly ILogger<ScriptController> logger;
        public ScriptController(ILogger<ScriptController> logger, IScriptService scriptService)
        {
            this.logger = logger;
            this.scriptService = scriptService;
        }

        [HttpPost("{writerId}")]
        public async Task<IActionResult> AddScript([FromForm] PostScriptDetailDTO scriptDetail, Guid writerId)
        {
            try
            {
                if (scriptDetail == null || !ModelState.IsValid)
                {
                    return BadRequest("Script request body is null or invalid");
                }
                var response = await scriptService.AddScript(scriptDetail, writerId);
                var badRequestStatus = response.StatusCode > 400 || response.StatusCode < 500;
                if (response.IsSuccess is false && badRequestStatus)
                {
                    return BadRequest(response);
                }
                else if (response.IsSuccess is false && response.StatusCode == 500)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception was thrown at {ex.Source} while adding a script for writer {writerId}.", ex);
                return (IActionResult)ResponseDetail<IActionResult>.Failed(ex.Message, 500, "Internal Server Error");
            }
        }

        [HttpGet("scripts/writer/{writerId}/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetScriptsByWriterId(Guid writerId, int pageNumber, int pageSize)
        {
            try
            {
                var response = await scriptService.GetScriptsByWriterId(writerId, pageNumber, pageSize);
                if (response.IsSuccess is false)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception was thrown at {ex.Source} while fetching scripts for writer with Id:{writerId}.", ex);
                return (IActionResult)ResponseDetail<IActionResult>.Failed(ex.Message, 500, "Internal Server Error");
            }
        }

        [HttpGet("scripts/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetAllScripts(int pageNumber, int pageSize)
        {
            try
            {
                var response = await scriptService.GetScripts(pageNumber, pageSize);
                if (response.IsSuccess is false)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception was thrown at {ex.Source} while fetching scripts", ex);
                return (IActionResult)ResponseDetail<IActionResult>.Failed(ex.Message, 500, "Internal Server Error");
            }
        }
    }
}
