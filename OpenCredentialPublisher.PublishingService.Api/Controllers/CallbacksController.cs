using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace OpenCredentialPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    [Route("callbacks")]
    public class CallbacksController : ControllerBase
    {

        public CallbacksController()
        {

        }

        /// <summary>
        /// To receive the results of a request made to the Verity API
        /// </summary>
        [AllowAnonymous]
        [HttpPost("verity")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleVerityEvent()
        {
            throw new NotImplementedException();
        }

    }

}
