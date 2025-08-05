using Microsoft.AspNetCore.Mvc;
using UserModule.Interfaces.UserInterfaces;

namespace Bara.API.Controllers.UserModuleControllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ILogger<AuthController> logger;
        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            this.logger = logger;
            this.authService = authService;
        }
    }
}
