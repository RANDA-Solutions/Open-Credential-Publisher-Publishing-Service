using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenCredentialsPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    [Route("api/credentials")]
    public class CredentialsController : ControllerBase
    {

        public CredentialsController()
        {

        }

        /// <summary>
        /// Returns CLR/VC credential
        /// </summary>
        /// <param name="model">access key</param>
        /// <returns>Signed CLR or VC</returns>
        [Authorize("ocp-wallet", AuthenticationSchemes = "Bearer")]
        [HttpPost("")]
        [ApiExplorerSettings(GroupName = "TODO")]
        public IActionResult Credentials([FromBody] CredentialRequest model)
        {
            return Ok(new CredentialResult());
        }

    }


}
