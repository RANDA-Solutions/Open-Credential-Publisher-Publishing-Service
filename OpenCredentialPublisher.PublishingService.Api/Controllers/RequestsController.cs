using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.PublishingService.Data;
using OpenCredentialPublisher.PublishingService.Services;
using System;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    [Route("api/requests")]
    public class RequestsController : ControllerBase
    {
        private readonly IPublishService _publishService;
        private readonly ConfigurationDbContext _configurationDbContext;

        public RequestsController(IPublishService publishService, ConfigurationDbContext configurationDbContext)
        {
            _publishService = publishService;
            _configurationDbContext = configurationDbContext;
        }

        /// <summary>
        /// A publisher can use this endpoint to get the current state of a publish request
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [Authorize(ScopeConstants.Publisher, AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PublishStatusResult))]
        [HttpGet("{requestId}")]
        public async Task<IActionResult> InquirePublishStatus([FromRoute] string requestId)
        {
            string clientId = User.ClientId();
            Func<string, Task<string>> getAccesskeyClaim = async clientId => {
                var client = await _configurationDbContext.Clients.Include(cl => cl.Claims).AsNoTracking().FirstOrDefaultAsync(c => c.ClientId == clientId);
                var claim = client.Claims.Find(cl => cl.Type == ClaimConstants.AccessKeyBaseUri);
                return claim?.Value;
            };

            var accessKeyBaseUri = await getAccesskeyClaim(clientId); 
            PublishStatusResult response = await _publishService.GetAsync(requestId, clientId, accessKeyBaseUri, ScopeConstants.Wallet, DiscoveryDocumentCustomEndpointsConstants.CredentialsEndpoint, HttpMethods.Post);

            if (response == null)
                return NotFound();

            return Ok(response);
        }



        /// <summary>
        /// Revoke a Request
        /// </summary>
        [Authorize(ScopeConstants.Publisher, AuthenticationSchemes = "Bearer")]
        [ValidationFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("{requestId}")]
        public async Task<IActionResult> RevokeRequest([FromRoute] string requestId)
        {

            try
            {
                string clientId = User.ClientId();

                await _publishService.RevokeAsync(requestId, clientId);

                return Ok(new { Message = "Revocation Successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ClrPublishResult() { Error = true, ErrorMessage = new string[] { ex.Message } });
            }

        }

    }

}
