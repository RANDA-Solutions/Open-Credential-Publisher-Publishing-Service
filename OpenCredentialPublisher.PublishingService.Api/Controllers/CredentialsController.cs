using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenCredentialPublisher.PublishingService.Data;
using OpenCredentialPublisher.PublishingService.Services;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    [Route("api/credentials")]
    public class CredentialsController : ControllerBase
    {

        private readonly IPublishService _publishService;

        public CredentialsController(IPublishService publishService)
        {
            _publishService = publishService;
        }

        /// <summary>
        /// Returns CLR/VC credential
        /// </summary>
        /// <param name="model">access key</param>
        /// <returns>Signed CLR or VC</returns>
        [Authorize(ScopeConstants.Wallet, AuthenticationSchemes = "Bearer")]
        [HttpPost("")]
        [RequestRateLimit(Name = nameof(GetCredentials), Milliseconds = 1000)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VerifiableCredentialResult))]
        public async Task<IActionResult> GetCredentials([FromBody] CredentialRequest model)
        {
            var vc = await _publishService.GetCredentialsAsync(model.AccessKey);

            return Ok(vc);
        }

    }


}
