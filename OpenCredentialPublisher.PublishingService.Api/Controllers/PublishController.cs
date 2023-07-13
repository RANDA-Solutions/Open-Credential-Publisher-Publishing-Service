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
    [Route("api/publish")]
    public class PublishController : ControllerBase
    {
        private readonly IPublishService _publishService;
        private readonly ConfigurationDbContext _configurationDbContext;

        public PublishController(IPublishService publishService, ConfigurationDbContext configurationDbContext)
        {
            _publishService = publishService;
            _configurationDbContext = configurationDbContext;
        }

        /// <summary>
        /// Publish Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(ScopeConstants.Publisher, AuthenticationSchemes = "Bearer")]
        [ValidationFilter]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClrPublishResult))]
        [HttpPost("")]
        public async Task<IActionResult> Publish(ClrPublishRequest request)
        {
           
            try
            {
                string clientId = User.ClientId();

                var requestId = await _publishService.ProcessRequestAsync(request.Identity.Id, request.Clr, clientId);

                return Ok(new ClrPublishResult() { RequestId = requestId });
            }
            catch (Exception ex)
            {
                return BadRequest(new ClrPublishResult() { Error = true, ErrorMessage = new string[] { ex.Message }  }); 
            }
           
        }

        /// <summary>
        /// Publish Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(ScopeConstants.Publisher, AuthenticationSchemes = "Bearer")]
        [ValidationFilter]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClrPublishResult))]
        [HttpPost("ThenPush")]
        public async Task<IActionResult> PublishThenPush(ClrPublishRequest request)
        {
            try
            {
                string clientId = User.ClientId();
                Func<string, Task<string>> getPushUriClaim = async clientId => {
                    var client = await _configurationDbContext.Clients.Include(cl => cl.Claims).AsNoTracking().FirstOrDefaultAsync(c => c.ClientId == clientId);
                    var claim = client.Claims.Find(cl => cl.Type == ClaimConstants.PushUri);
                    return claim?.Value;
                };

                var pushUri = await getPushUriClaim(clientId);

                var requestId = await _publishService.ProcessRequestAsync(request.Identity.Id, request.Clr, clientId, true, pushUri);

                return Ok(new ClrPublishResult() { RequestId = requestId });
            }
            catch (Exception ex)
            {
                return BadRequest(new ClrPublishResult() { Error = true, ErrorMessage = new string[] { ex.Message } });
            }

        }


    }

}
