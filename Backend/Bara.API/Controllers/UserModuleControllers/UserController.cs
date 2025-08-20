using Microsoft.AspNetCore.Mvc;
using SharedModule.Utils;
using UserModule.DTOs.UserDTO;
using UserModule.Enums;
using UserModule.Interfaces.UserInterfaces;

namespace Bara.API.Controllers.UserModuleControllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly LogHelper<UserController> logHelper;
        private readonly IUserService userService;

        public UserController(ILogger<UserController> logger, LogHelper<UserController> logHelper, IUserService userService)
        {
            this.logHelper = logHelper;
            this.logger = logger;
            this.userService = userService;
        }

        /// <summary>
        /// Registers a new user on the platform.
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDTO payload)
        {
            try
            {
                var registrationResponse = await userService.BeginRegistration(payload);
                if (registrationResponse.IsSuccess)
                {
                    return Ok(registrationResponse);
                }
                else if (registrationResponse.StatusCode == 409)
                {
                    return Conflict(registrationResponse);
                }
                else
                {
                    return BadRequest(registrationResponse);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception {ex.GetType().Name} was thrown at {ex.Source} while registering user: {payload.Email}..." +
                    $"\nBase Exception: {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseDetail<string>.Failed("Your request failed...", 500, "Error"));
            }
        }

        /// <summary>
        /// Registers a new admin user on the platform.
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        //[Authorize(Roles = "Admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDTO payload)
        {
            try
            {
                payload.Type = Role.Admin;
                var registrationResponse = await userService.BeginRegistration(payload);
                if (registrationResponse.IsSuccess)
                {
                    return Ok(registrationResponse);
                }
                else if (registrationResponse.StatusCode == 409)
                {
                    return Conflict(registrationResponse);
                }
                else
                {
                    return BadRequest(registrationResponse);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception {ex.GetType().Name} was thrown at {ex.Source} while registering admin: {payload.Email}..." +
                    $"\nBase Exception: {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseDetail<string>.Failed("Your request failed...", 500, "Error"));
            }
        }
    }
}
