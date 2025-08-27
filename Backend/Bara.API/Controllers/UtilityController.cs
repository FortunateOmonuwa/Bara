﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Paystack;
using SharedModule.Utils;

namespace Bara.API.Controllers
{
    [Route("api/utilities")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        private readonly ILogger<UtilityController> logger;
        private readonly LogHelper<UtilityController> logHelper;
        private readonly IPaystackService PaystackService;
        public UtilityController(ILogger<UtilityController> logger, LogHelper<UtilityController> logHelper, IPaystackService paystackService)
        {
            this.logger = logger;
            this.logHelper = logHelper;
            PaystackService = paystackService;
        }

        /// <summary>
        /// Fetches a list of banks from Paystack.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Producer, Writer")]
        [HttpGet("banks")]
        public async Task<IActionResult> GetBanks()
        {
            try
            {
                var response = await PaystackService.GetBanksAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                logHelper.LogExceptionError(ex.GetType().Name, ex.GetBaseException().GetType().Name, "Fetching banks");
                return StatusCode(500, ResponseDetail<string>.Failed("An error occured", 500, "Internal server error"));
            }
        }
    }
}
