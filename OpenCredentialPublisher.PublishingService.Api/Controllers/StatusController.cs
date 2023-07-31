using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Interfaces;
using OpenCredentialPublisher.Credentials.VerifiableCredentials;
using OpenCredentialPublisher.PublishingService.Services.Abstractions;
using System;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    [Route("api/status")]
    public class StatusController : ControllerBase
    {
        private readonly IRevocationListService _revocationListService;

        public StatusController(IRevocationListService revocationListService)
        {
            _revocationListService = revocationListService;
        }

        /// <summary>
        /// Returns Revocation Document for provided Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>RevocationDocument</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ResponseCache(Duration =60)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RevocationDocument))]
        public async Task<IActionResult> GetRevocationDocument([FromRoute]string id)
        {
            try
            {
                var document = await _revocationListService.GetRevocationDocument(id);

                if (document == null)
                { 
                    return NotFound();
                }

                return Ok(document);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
