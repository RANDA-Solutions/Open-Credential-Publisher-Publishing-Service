using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenCredentialPublisher.PublishingService.Data;

namespace OpenCredentialPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    public class ScopeTestsController : ControllerBase
    {
        [Authorize(ScopeConstants.Publisher, AuthenticationSchemes = "Bearer")]
        [HttpGet("api/scopetests/publisher")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Publisher()
        {
            return Ok(new { Success = true });
        }

        [Authorize(ScopeConstants.Wallet, AuthenticationSchemes = "Bearer")]
        [HttpGet("api/scopetests/wallet")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Wallet()
        {
            return Ok(new { Success = true });
        }
    }

}
