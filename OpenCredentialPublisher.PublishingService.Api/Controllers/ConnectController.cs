using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace OpenCredentialPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    public class ConnectController : ControllerBase
    {

        private readonly IDynamicClientRegistrationService _registrationService;

        public ConnectController(IDynamicClientRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }


        /// <summary>
        /// Dynamic Client Registration
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("connect/register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OcpDynamicClientRegistrationResult))]
        [RequestRateLimit(Name = nameof(ClientRegistration), Milliseconds = 3000)]
        public async Task<IActionResult> ClientRegistration([FromBody] OcpDynamicClientRegistrationRequest model)
        {

            if (model.TokenEndpointAuthenticationMethod != EndpointAuthenticationMethods.BasicAuthentication)
            {
                throw new Exception("Value for token_endpoint_auth_method is not supported");
            }

            var response = await _registrationService.RegisterClientAsync(model);

            return Ok(response);
        }


        // Use built-in, GET /.well-known/openid-configuration http://docs.identityserver.io/en/dev/endpoints/discovery.html

        //[AllowAnonymous]
        //[HttpGet("discovery")]
        //public IActionResult GetDiscovery()
        //{
        //    // TODO: do we need this?  can we use /.well-known/openid-configuration
        //    return Ok(new { });
        //}



        // Use built-in, GET /connect/authorize http://docs.identityserver.io/en/dev/endpoints/authorize.html

        //[AllowAnonymous]
        //[HttpGet("authorize")]
        //public IActionResult Authorize([FromUrl] )
        //{
        //}



        // Use built-in, POST /connect/token http://docs.identityserver.io/en/dev/endpoints/token.html

        //[AllowAnonymous]
        //[HttpPost("token")]
        //public IActionResult RequestToken([FromBody] )
        //{
        //}

    }



}
