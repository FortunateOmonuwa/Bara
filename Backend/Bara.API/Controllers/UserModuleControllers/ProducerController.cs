using Microsoft.AspNetCore.Mvc;
using SharedModule.Utils;
using UserModule.DTOs.ProducerDTOs;
using UserModule.Interfaces.UserInterfaces;

namespace Bara.API.Controllers.UserModuleControllers
{
    [Route("api/producer")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly IProducerService producerService;
        private readonly ILogger<ProducerController> logger;
        public ProducerController(IProducerService producerService, ILogger<ProducerController> logger)
        {
            this.producerService = producerService;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a new producer profile on the platform.
        /// </summary>
        /// <param name="producerDetail">
        /// The detailed information required to register a producer, including personal info, contact details, and optionally a profile image or document.
        /// </param>
        /// <returns>
        /// Returns a 200 OK with the created producer’s details if successful,
        /// 400 Bad Request if the model state is invalid or the creation fails due to user-side issues,
        /// or 500 Internal Server Error if the server encounters an unexpected problem.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> AddProducer([FromForm] PostProducerDetailDTO producerDetail)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request body");
                }
                var response = await producerService.AddProducer(producerDetail);
                if (response.IsSuccess is false && response.StatusCode == 500)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
                else if (response.IsSuccess is false)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception {ex.GetType().Name} was thrown at {ex.Source} while creating new producer profile: {producerDetail.FirstName} {producerDetail.LastName}..." +
                    $"\nBase Exception: {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}", ex.Message);
                return StatusCode(500, ResponseDetail<string>.Failed("Your request failed...", 500, "Error"));
            }
        }

        /// <summary>
        /// Retrieves the complete profile of a specific producer using their unique identifier.
        /// </summary>
        /// <param name="producerId">
        /// The unique ID of the producer whose profile is to be fetched.
        /// </param>
        /// <returns>
        /// Returns a 200 OK with the producer's profile details if found,
        /// or 400 Bad Request if the producer does not exist or if an error occurs.
        /// </returns>
        [HttpGet("profile/{producerId}")]
        public async Task<IActionResult> GetProducerDetail(Guid producerId)
        {
            try
            {
                var res = await producerService.GetProducer(producerId);
                if (res.IsSuccess is false)
                {
                    return BadRequest(res);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception {ex.GetType().Name} was thrown at {ex.Source} while fetching producer profile..." +
                    $"\nBase Exception: {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}", ex.Message);
                return StatusCode(500, ResponseDetail<string>.Failed("Your request failed...", 500, "Error"));
            }
        }
    }
}
