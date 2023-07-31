using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Interfaces;
using System;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    [Route("api/keys")]
    public class KeysController : ControllerBase
    {
        private readonly IKeyStore _keyStore;

        public KeysController(IKeyStore keyStore)
        {
            _keyStore = keyStore;
        }

        /// <summary>
        /// Returns Issuer’s public key
        /// </summary>
        /// <param name="issuerId"></param>
        /// <param name="keyId"></param>
        /// <returns>PublicKey</returns>
        [AllowAnonymous]
        [HttpGet("{issuerId}/{keyId}")]
        [ResponseCache(Duration =60)]
        [ApiExplorerSettings(GroupName = "TODO")]
        public async Task<IActionResult> GetIssuerPublicKey(string issuerId, string keyId)
        {
            try
            {
                var value = await _keyStore.GetPublicKeyAsync(keyId, issuerId);

                if (value == null)
                { 
                    return NotFound();
                }

                return Ok(value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Verify ownership of private key
        /// Notes: Not applicable for all encryption types and may be unnecessary
        /// </summary>
        /// <param name="issuerId"></param>
        /// <param name="keyId"></param>
        /// <param name="model">encrypted with the referenced public key</param>
        /// <returns>decrypted with the private key pair</returns>
        [AllowAnonymous]
        [HttpPost("{issuerId}/{keyId}/verify")]
        [RequestRateLimit(Name = nameof(VerifyOwnership), Milliseconds = 1000)]
        [ApiExplorerSettings(GroupName = "TODO")]
        public IActionResult VerifyOwnership([FromRoute] string issuerId, [FromRoute] string keyId, [FromBody] KeyVerificationRequest model)
        {
            return Ok(new KeyVerificationResult());
        }
    }
}
