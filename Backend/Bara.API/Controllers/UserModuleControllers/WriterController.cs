using Microsoft.AspNetCore.Mvc;
using SharedModule.Utils;
using UserModule.DTOs.WriterDTOs;
using UserModule.Interfaces.UserInterfaces;

namespace Bara.API.Controllers.UserModuleControllers
{
    [Route("api/writer")]
    [ApiController]
    public class WriterController : ControllerBase
    {
        private readonly IWriterService writerService;
        private readonly ILogger<WriterController> logger;
        public WriterController(IWriterService writerService, ILogger<WriterController> logger)
        {
            this.logger = logger;
            this.writerService = writerService;
        }

        [HttpPost]
        public async Task<IActionResult> AddWriter([FromForm] PostWriterDetailDTO writerDetail)
        {
            try
            {
                if (writerDetail == null || !ModelState.IsValid)
                {
                    return BadRequest("Writer request body is null or invalid");
                }
                var response = await writerService.AddWriter(writerDetail);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception {ex.GetType()} was thrown at {ex.Source} while creating a new writer profile: {writerDetail.FirstName} {writerDetail.LastName}...\nBase Exception {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}");
                return (IActionResult)ResponseDetail<IActionResult>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected Error");
            }
        }
        [HttpGet("profile/{writerId}")]
        public async Task<IActionResult> GetProducerDetail(Guid writerId)
        {
            try
            {
                var res = await writerService.GetWriterDetail(writerId);
                if (res.IsSuccess is false)
                {
                    return BadRequest(res);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception {ex.GetType().Name} was thrown at {ex.Source} while fetching writer profile..." +
                    $"\nBase Exception: {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}", ex.Message);
                return (IActionResult)ResponseDetail<IActionResult>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected Error");
            }
        }
    }
}