using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace OpenCredentialPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    [Route("api/connect-wallet")]
    public class ConnectWalletController : ControllerBase
    {
        public ConnectWalletController()
        {

        }

        /// <summary>
        /// Initiates the third-party wallet connection process 
        /// </summary>
        /// <param name="model">TBD</param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("{requestId}")]
        [ApiExplorerSettings(GroupName = "TODO")]
        public IActionResult InitiateConnection([FromBody] ConnectWalletRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
