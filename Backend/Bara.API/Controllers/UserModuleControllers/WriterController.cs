using Microsoft.AspNetCore.Mvc;
using SharedModule.Utils;
using UserModule.DTOs.WriterDTOs;
using UserModule.Interfaces.UserInterfaces;

namespace Bara.API.Controllers.UserControllers
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
                logger.LogError($"An exception was thrown at {ex.Source} while creating a new writer profile: {writerDetail.FirstName} {writerDetail.LastName}.", ex);
                return (IActionResult)ResponseDetail<IActionResult>.Failed(ex.Message, 500);
            }
        }
    }
}