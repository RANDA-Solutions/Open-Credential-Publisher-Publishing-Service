using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenCredentialPublisher.PublishingService.Data;
using OpenCredentialPublisher.PublishingService.Services;
using OpenCredentialPublisher.PublishingService.Shared;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Web;
using static QRCoder.PayloadGenerator;

namespace OpenCredentialPublisher.PublishingService.Functions
{

    public class PublishPushHandler : PublishMessageHandlerBase, ICommandHandler<PublishPushCommand>
    {
        private readonly IMediator _mediator;
        private readonly IFileStoreService _fileService;
        private readonly string readyState = PublishProcessingStates.PublishPushReady;
        private readonly string processingState = PublishProcessingStates.PublishPushProcessing;
        private readonly string failureState = PublishProcessingStates.PublishPushFailure;

        public PublishPushHandler(IOptions<AzureBlobOptions> blobOptions, OcpDbContext context, ILogger<PublishMessageHandlerBase> log, 
                            IMediator mediator, IFileStoreService fileService) : base(blobOptions, context, log)
        {
            _mediator = mediator;
            _fileService = fileService;
        }

        public async Task HandleAsync(PublishPushCommand command)
        {
            var publishRequest = await PreProcessAsync(command);

            if (publishRequest == null) return;

            try
            {
                var leaseId = await AcquireLockAsync("pub", publishRequest.RequestId, TimeSpan.FromSeconds(30));

                try
                {
                    await ProcessAsync(publishRequest);

                    await ReleaseLockAsync();

                    await PostProcessAsync(publishRequest);
                }
                catch (Exception)
                {
                    publishRequest.ProcessingState = failureState;
                    await SaveChangesAsync();

                    throw;
                }
            }
            catch (Exception)
            {
                await ReleaseLockAsync();

                throw;
            }
        }


        private async Task<PublishRequest> PreProcessAsync(PublishPushCommand command)
        {
            var publishRequest = await GetPublishRequestAsync(command.RequestId);

            if (publishRequest == null)
            {
                throw new Exception($"RequestId '{command.RequestId}' not found");
            }

            var validProcessingStates = new string[] { readyState, processingState, failureState };

            if (!validProcessingStates.Contains(publishRequest.ProcessingState))
            {
                Log.LogWarning($"'{publishRequest.ProcessingState}' is not a valid state for Processing");
                return null;
            }

            return publishRequest;
        }

        private async Task ProcessAsync(PublishRequest publishRequest)
        {
            publishRequest.ProcessingState = processingState;
            await SaveChangesAsync();

            // send push notification to endpoint specified in request
            string accessKey = publishRequest.LatestAccessKey()?.Key;
            if (accessKey != null)
            {
                var bodyJson = new JsonObject
                {
                    { "endpoint", DiscoveryDocumentCustomEndpointsConstants.CredentialsEndpoint },
                    { "scope", ScopeConstants.Wallet },
                    { "payload", JsonConvert.SerializeObject(new AccessKeyPayload { AccessKey = accessKey }) },
                    { "issuer", publishRequest.AppUri },
                    { "method", HttpMethod.Post.ToString() }
                };
                var body = bodyJson.ToString();
                var request = new HttpRequestMessage(HttpMethod.Post, publishRequest.PushUri);

                var client = new HttpClient();
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync(request.RequestUri, request.Content);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    publishRequest.ProcessingState = PublishProcessingStates.PublishNotifyReady;
                    publishRequest.PublishState = PublishStates.Complete;
                    Log.LogInformation($"Next PublishState: '{publishRequest.PublishState}, Next ProcessingState: '{publishRequest.ProcessingState}'");
                    await SaveChangesAsync();
                }
            }
        }

        private async Task PostProcessAsync(PublishRequest publishRequest)
        {
            switch (publishRequest.ProcessingState)
            {
                case PublishProcessingStates.PublishNotifyReady:
                    await _mediator.Publish(new PublishNotifyCommand(publishRequest.RequestId));
                    break;

                default:
                    throw new Exception("Invalid State");
            }
        }

    }

}
