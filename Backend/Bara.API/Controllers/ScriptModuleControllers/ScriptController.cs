﻿using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Uploads a new script to the platform for a specified writer.
        /// </summary>
        /// <param name="scriptDetail">
        /// Contains the details of the script to be uploaded, including title, genre, logline, synopsis, price,
        /// file, registration details, IP ownership rights, and optional metadata like cover image or proof of copyright.
        /// </param>
        /// <param name="writerId">
        /// The unique identifier of the writer uploading the script. This links the script to the correct user.
        /// </param>
        /// <returns>
        /// Returns a 200 OK response with upload confirmation if successful, a 400 Bad Request if validation fails,
        /// or a 500 Internal Server Error if an unexpected exception occurs.
        /// </returns>

        [HttpPost("{writerId}")]
        [Authorize(Roles = "Writer", Policy = "Verified")]
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
                return StatusCode(500, ResponseDetail<string>.Failed("Your request failed...", 500, "Error"));
            }
        }

        /// <summary>
        /// Retrieves a paginated list of scripts uploaded by a specific writer.
        /// </summary>
        /// <param name="writerId">
        /// The unique identifier of the writer whose scripts are to be fetched.
        /// </param>
        /// <param name="pageNumber">
        /// The page number to retrieve. Must be greater than 0.
        /// </param>
        /// <param name="pageSize">
        /// The number of scripts to retrieve per page.
        /// </param>
        /// <returns>
        /// Returns a 200 OK response with the list of scripts if found, 
        /// 400 Bad Request if retrieval fails due to invalid input or other client errors, 
        /// or 500 Internal Server Error in case of unexpected server issues.
        /// </returns>

        [HttpGet("scripts/writer/{writerId}/{pageNumber}/{pageSize}")]
        [Authorize(Roles = "Writer, Admin", Policy = "Verified")]
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
                return StatusCode(500, ResponseDetail<string>.Failed("Your request failed...", 500, "Error"));
            }
        }

        /// <summary>
        /// Retrieves a paginated list of all available scripts on the platform.
        /// </summary>
        /// <param name="pageNumber">
        /// The page number to retrieve. Starts from 1.
        /// </param>
        /// <param name="pageSize">
        /// The number of scripts to include per page.
        /// </param>
        /// <returns>
        /// Returns a 200 OK response with a paginated list of scripts if successful,
        /// or 400 Bad Request if the operation fails due to client-side errors,
        /// or 500 Internal Server Error for unexpected failures.
        /// </returns>

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
                return StatusCode(500, ResponseDetail<string>.Failed("Your request failed...", 500, "Error"));
            }
        }

        /// <summary>
        /// Deletes a specific script uploaded by a writer.
        /// </summary>
        /// <param name="scriptId">
        /// The unique identifier of the script to delete.
        /// </param>
        /// <param name="writerId">
        /// The unique identifier of the writer requesting the deletion (used for validation/authorization).
        /// </param>
        /// <returns>
        /// Returns a 200 OK if the script is deleted successfully,
        /// 400 Bad Request for invalid IDs or unauthorized access,
        /// or 500 Internal Server Error if something goes wrong on the server.
        /// </returns>

        [HttpDelete("delete/{scriptId}/{writerId}")]
        [Authorize(Roles = "Writer, Admin", Policy = "Verified")]
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
                return StatusCode(500, ResponseDetail<string>.Failed("Your request failed...", 500, "Error"));
            }
        }

        /// <summary>
        /// Downloads the full content of a script file as a binary stream.
        /// </summary>
        /// <param name="scriptId">
        /// The unique identifier of the script to download.
        /// </param>
        /// <returns>
        /// Returns a file stream containing the script PDF if successful (200 OK),
        /// or a 400/500 response if the script does not exist or an error occurs.
        /// </returns>

        [HttpGet("{scriptId}")]
        [Authorize(Roles = "Writer, Admin, Producer", Policy = "Verified")]
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
                return StatusCode(500, ResponseDetail<string>.Failed("Your request failed...", 500, "Error"));
            }
        }
    }
}
