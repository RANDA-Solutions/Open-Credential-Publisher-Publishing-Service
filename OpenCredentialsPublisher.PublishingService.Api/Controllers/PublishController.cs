using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenCredentialsPublisher.PublishingService.Services;
using System;
using System.Threading.Tasks;

namespace OpenCredentialsPublisher.PublishingService.Api.Controllers
{

    [ApiController]
    [Route("api/publish")]
    public class PublishController : ControllerBase
    {
        private readonly IPublishService _publishService;

        public PublishController(IPublishService publishService)
        {
            _publishService = publishService;
        }

        /// <summary>
        /// Publish Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize("ocp-publisher", AuthenticationSchemes = "Bearer")]
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


    }

}
