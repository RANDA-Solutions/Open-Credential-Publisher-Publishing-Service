using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenCredentialsPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    [Route("api/configuration")]
    public class ConfigurationController : ControllerBase
    {

        public ConfigurationController()
        {

        }

        /// <summary>
        /// Provide DID vendor credentials
        /// </summary>
        /// <param name="model">TBD</param>
        /// <returns></returns>
        [Authorize("ocp-publisher", AuthenticationSchemes = "Bearer")]
        [HttpPost("{requestId}")]
        [ApiExplorerSettings(GroupName = "TODO")]
        public IActionResult Configuration([FromBody] ConfigurationRequest model)
        {
            return Ok(new ConfigurationResult());
        }

    }
}
