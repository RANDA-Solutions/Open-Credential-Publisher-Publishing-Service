using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenCredentialsPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    public class ScopeTestsController : ControllerBase
    {
        [Authorize("ocp-publisher", AuthenticationSchemes = "Bearer")]
        [HttpGet("api/scopetests/publisher")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Publisher()
        {
            return Ok(new { Success = true });
        }

        [Authorize("ocp-wallet", AuthenticationSchemes = "Bearer")]
        [HttpGet("api/scopetests/wallet")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Wallet()
        {
            return Ok(new { Success = true });
        }
    }

}
