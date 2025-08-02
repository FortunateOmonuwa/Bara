using Microsoft.AspNetCore.Mvc;
using ScriptModule.DTOs;
using ScriptModule.Interfaces;
using SharedModule.Utils;

namespace Bara.API.Controllers.ScriptModuleControllers
{
    [Route("api/script")]
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
                logger.LogError($"An exception:  {ex.GetType().Name}  was thrown at  {ex.Source}  while adding a script...\nBase Exception: {ex.GetBaseException().GetType()}", $"Exception Code: {ex.HResult}");
                return (IActionResult)ResponseDetail<IActionResult>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
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
                logger.LogError($"An exception:  {ex.GetType().Name}  was thrown at  {ex.Source}  while fetching scripts for writer...\nBase Exception: {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}");
                return (IActionResult)ResponseDetail<IActionResult>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
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
                logger.LogError($"An exception: {ex.GetType().Name} was thrown at {ex.Source} while fetching scripts...\nBase Exception: {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}");
                return (IActionResult)ResponseDetail<IActionResult>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }

        [HttpDelete("delete/{scriptId}/{writerId}")]
        public async Task<IActionResult> DeleteScript(Guid scriptId, Guid writerId)
        {
            try
            {
                var response = await scriptService.DeleteScript(scriptId, writerId);
                if (response.IsSuccess is false && response.StatusCode == 500)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
                else if (response.IsSuccess == false)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception: {ex.GetType().Name} was thrown at {ex.Source} while deleting script...\nBase Exception: {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}");
                return (IActionResult)ResponseDetail<IActionResult>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }

        [HttpGet("{scriptId}")]
        public async Task<IActionResult> Download(Guid scriptId)
        {
            try
            {
                var result = await scriptService.DownloadScript(scriptId);

                if (!result.IsSuccess || result.Data == null)
                {
                    return StatusCode(result.StatusCode, result);
                }

                return File(result.Data.File, result.Data.ContentType, "script.pdf");
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception: {ex.GetType().Name} was thrown at {ex.Source} while downloading a script...\nBase Exception: {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}");
                return (IActionResult)ResponseDetail<IActionResult>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }

    }
}
