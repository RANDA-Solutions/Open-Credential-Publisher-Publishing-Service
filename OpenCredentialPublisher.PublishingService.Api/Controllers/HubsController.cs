using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    [Route("hubs")]
    public class HubsController : ControllerBase
    {

        private readonly IHubContext<RequestNotificationHub> _hubContext;
        //private readonly string _securityKey;

        public HubsController(IHubContext<RequestNotificationHub> hubContext)
        {
            _hubContext = hubContext;
            //_securityKey = "ocp";
        }


        /// <summary>
        /// To receive the results of a request made to the API
        /// </summary>
        [AllowAnonymous]
        [HttpPost("requests/{requestId}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ProcessHubRequest([FromRoute] string requestId, [FromBody] ProcessHubRequest model)
        {

            if (model.Status == "Complete")
                await _hubContext.Clients.Group(requestId.ToLower()).SendAsync("PublishUpdate", requestId, model.Status);
            else
                await _hubContext.Clients.Group(requestId.ToLower()).SendAsync("PublishUpdate", requestId, model.Status);

            return Ok();
        }

    }
}
