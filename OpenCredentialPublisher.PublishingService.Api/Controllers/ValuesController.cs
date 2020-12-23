using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenCredentialPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [HttpGet("api/values")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetValues()
        {
            var user = User;

            return Ok(new { ClientId = user.FindFirst(c => c.Type == "client_id").Value });
        }

    }
}
