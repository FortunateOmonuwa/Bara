using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services;
using Services.YouVerifyIntegration;
using SharedModule.Settings;
using SharedModule.Utils;
using System.Text;
using System.Text.Json;
using UserModule.Interfaces.UserInterfaces;

namespace Bara.API.Controllers
{
    [Route("api/webkooks")]
    [ApiController]

    /// <summary>
    /// This controller is used to handle requests to external clients.
    /// </summary>
    public class WebHookController : ControllerBase
    {
        private readonly IYouVerifyService youVerify;
        private readonly Secrets secrets;
        private readonly LogHelper<WebHookController> logHelper;
        private readonly ILogger<WebHookController> logger;
        private readonly IUserService userService;
        public WebHookController(IYouVerifyService youVerifyService, IOptions<Secrets> secretOptions, LogHelper<WebHookController> logHelper, IUserService userService, ILogger<WebHookController> logger)
        {
            youVerify = youVerifyService;
            secrets = secretOptions.Value;
            this.logHelper = logHelper;
            this.userService = userService;
            this.logger = logger;
        }

        //[HttpPost("youverify")]
        //public async Task<IActionResult> ReceiveKycVerificationResponse([FromBody] YouVerifyWebhookEvent payload)
        //{
        //    try
        //    {
        //        Request.EnableBuffering();

        //        var isValid = YouVerifyWebhookVerifier.IsValidYouVerifySignature(Request, secrets.YouVerifyWebhookSigningSecret);
        //        if (!isValid)
        //        {
        //            return Unauthorized("Invalid webhook signature");
        //        }
        //        logger.LogInformation("Received YouVerify webhook event {Payload}", payload);
        //        var isSuccessful = false;
        //        if (payload.Data.DataValidation == true)
        //        {
        //            isSuccessful = true;
        //        }
        //        var updateUserReq = await userService.UpdateUserVerificationStatus(payload.Data.IdNumber, payload.Data.DateOfBirth, isSuccessful, payload.Data.FirstName, payload.Data.LastName, payload.Data.Type);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        logHelper.LogExceptionError(ex.GetType().Name, ex.GetBaseException().GetType().Name, "Receiving KYC verification response from YouVerify");
        //        return BadRequest(new { message = "An error occurred while processing the request." });
        //    }
        //}

        [Authorize(Roles = "Admin, Producer, Writer")]
        [HttpPost("youverify")]
        public async Task<IActionResult> ReceiveKycVerificationResponse()
        {
            try
            {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);

                string rawBody = await reader.ReadToEndAsync();
                Request.Body.Position = 0;

                logger.LogInformation("Received YouVerify webhook event {Payload}", rawBody);
                var isValid = YouVerifyWebhookVerifier.IsValidYouVerifySignature(rawBody, Request.Headers["x-youverify-signature"], secrets.YouVerifyWebhookSigningSecret);
                if (!isValid)
                {
                    return Unauthorized("Invalid webhook signature");
                }

                var payload = JsonSerializer.Deserialize<YouVerifyWebhookEvent>(rawBody);

                if (payload == null)
                {
                    return BadRequest("Invalid payload");
                }

                bool isSuccessful = payload.Data.DataValidation == true;

                var updateUserReq = await userService.UpdateUserVerificationStatus(
                    payload.Data.IdNumber,
                    payload.Data.DateOfBirth,
                    isSuccessful,
                    payload.Data.FirstName,
                    payload.Data.LastName,
                    payload.Data.Type);

                return Ok();
            }
            catch (Exception ex)
            {
                logHelper.LogExceptionError(ex.GetType().Name, ex.GetBaseException().GetType().Name, "Receiving KYC verification response from YouVerify");
                return BadRequest(new { message = "An error occurred while processing the request." });
            }
        }

        /// <summary>
        /// This endpoint is used to perfrom a health check.
        /// </summary>
        /// <returns></returns>
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("App is working!");
        }
    }
}
