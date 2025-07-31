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
                logger.LogError($"An exception was thrown at {ex.Source} while creating new producer profile: {producerDetail.FirstName} {producerDetail.LastName}.", ex);
                return (IActionResult)ResponseDetail<IActionResult>.Failed(ex.Message, 500);
            }
        }
    }
}
