using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Bara.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var response = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Service = "Bara.API",
                Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            };

            return Ok(response);
        }

        [HttpGet("detailed")]
        public IActionResult GetDetailed()
        {
            var response = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Service = "Bara.API",
                Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                MachineName = Environment.MachineName,
                ProcessId = Environment.ProcessId,
                WorkingSet = Environment.WorkingSet,
                Uptime = Environment.TickCount64
            };

            return Ok(response);
        }
    }
}
