using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.PublishingService.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    [Route("api/configuration")]
    public class ConfigurationController : ControllerBase
    {
        private readonly ConfigurationDbContext _dbContext;
        public ConfigurationController(ConfigurationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Provide DID vendor credentials
        /// </summary>
        /// <param name="model">TBD</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(ScopeConstants.Publisher, AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConfigurationResult))]
        public async Task<IActionResult> Configuration(ConfigurationRequest model)
        {
            var clientId = User.ClientId();
            var client = await _dbContext.Clients.Include(cl => cl.Claims).FirstOrDefaultAsync(cl => cl.ClientId == clientId);
            client.Claims ??= new List<ClientClaim>();
            if (client.Claims.Any(cl => cl.Type == nameof(model.AccessKeyBaseUri)))
            {
                var claim = client.Claims.FirstOrDefault(cl => cl.Type == nameof(model.AccessKeyBaseUri));
                if (string.IsNullOrEmpty(model.AccessKeyBaseUri)) {
                    _dbContext.Remove(claim);
                }
                else
                {
                    claim.Value = model.AccessKeyBaseUri;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(model.AccessKeyBaseUri))
                {
                    return Ok();
                }
                client.Claims.Add(new ClientClaim
                {
                    Type = nameof(model.AccessKeyBaseUri),
                    Value = model.AccessKeyBaseUri
                });
            }

            if (client.Claims.Any(cl => cl.Type == nameof(model.PushUri)))
            {
                var claim = client.Claims.FirstOrDefault(cl => cl.Type == nameof(model.PushUri));
                if (string.IsNullOrEmpty(model.PushUri))
                {
                    _dbContext.Remove(claim);
                }
                else
                {
                    claim.Value = model.PushUri;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(model.PushUri))
                {
                    return Ok();
                }
                client.Claims.Add(new ClientClaim
                {
                    Type = nameof(model.PushUri),
                    Value = model.PushUri
                });
            }

            await _dbContext.SaveChangesAsync();

            return Ok(new ConfigurationResult { AccessKeyBaseUri = model.AccessKeyBaseUri, PushUri = model.PushUri });
        }

    }
}
