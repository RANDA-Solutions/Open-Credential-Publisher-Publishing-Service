using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenCredentialsPublisher.PublishingService.Services;
using System;
using System.Threading.Tasks;

namespace OpenCredentialsPublisher.PublishingService.Api.Controllers
{
    [ApiController]
    [Route("api/requests")]
    public class RequestsController : ControllerBase
    {
        private readonly IPublishService _publishService;

        public RequestsController(IPublishService publishService)
        {
            _publishService = publishService;
        }

        /// <summary>
        /// A publisher can use this endpoint to get the current state of a publish request
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [Authorize("ocp-publisher", AuthenticationSchemes = "Bearer")]
        [RequestRateLimit(Name = nameof(InquirePublishStatus), Milliseconds = 1000)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PublishStatusResult))]
        [HttpGet("{requestId}")]
        public async Task<IActionResult> InquirePublishStatus([FromRoute] string requestId)
        {
            string clientId = User.ClientId();

            PublishStatusResult response = await _publishService.GetAsync(requestId, clientId);

            if (response == null)
                return NotFound();

            return Ok(response);
        }



        /// <summary>
        /// Revoke a Request
        /// </summary>
        [Authorize("ocp-publisher", AuthenticationSchemes = "Bearer")]
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
