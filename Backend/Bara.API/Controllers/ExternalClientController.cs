using Microsoft.AspNetCore.Mvc;
using Services.YouVerifyIntegration;

namespace Bara.API.Controllers
{
    [Route("api/external_client")]
    [ApiController]

    /// <summary>
    /// This controller is used to handle requests to external clients.
    /// </summary>
    public class ExternalClientController : ControllerBase
    {
        private readonly IYouVerifyService youVerify;
        public ExternalClientController(IYouVerifyService youVerifyService)
        {

        }
    }
}
