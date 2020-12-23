using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OpenCredentialPublisher.PublishingService.Data;

namespace OpenCredentialPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HubTestsController : ControllerBase
    {
        private readonly IHubContext<RequestNotificationHub> _hubContext;

        public HubTestsController(IHubContext<RequestNotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }


        [AllowAnonymous]
        [HttpGet("api/hubtests/publishupdate")]
        public IActionResult SendPublishUpdate(string requestId, string status)
        {
            _hubContext.Clients.Groups(requestId.ToLower()).SendAsync("PublishUpdate", requestId, status);

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
