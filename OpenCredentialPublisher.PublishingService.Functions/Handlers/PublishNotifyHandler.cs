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
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Functions
{
    public class PublishNotifyHandler : PublishMessageHandlerBase, ICommandHandler<PublishNotifyCommand>
    {
        private readonly string _appBaseUri;

        private readonly string readyState = PublishProcessingStates.PublishNotifyReady;
        private readonly string processingState = PublishProcessingStates.PublishNotifyProcessing;
        private readonly string failureState = PublishProcessingStates.PublishNotifyFailure;

        public PublishNotifyHandler(IConfiguration configuration, IOptions<AzureBlobOptions> blobOptions, OcpDbContext context, 
                        ILogger<PublishMessageHandlerBase> log) : base(blobOptions, context, log)
        {
            _appBaseUri = configuration["AppBaseUri"];
        }

        public async Task HandleAsync(PublishNotifyCommand command)
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

                    //await PostProcessAsync(publishRequest);
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


        private async Task<PublishRequest> PreProcessAsync(PublishNotifyCommand command)
        {
            var publishRequest = await GetPublishRequestAsync(command.RequestId);

            if (publishRequest == null)
            {
                throw new Exception($"RequestId '{command.RequestId}' not found");
            }

            var validProcessingStates = new string[] { null, readyState, processingState, failureState };

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

            var baseUri = new System.Uri(_appBaseUri);

            string url = $"/hubs/requests/{publishRequest.RequestId}";

            var model = new { Status = "Complete" };

            using (var client = new HttpClient())
            {
                client.BaseAddress = baseUri;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string stringData = JsonConvert.SerializeObject(model);
                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                
                HttpResponseMessage response = client.PostAsync(url, contentData).Result;

                response.EnsureSuccessStatusCode();
            }

            publishRequest.ProcessingState = PublishProcessingStates.Complete;
            publishRequest.PublishState = PublishStates.Complete;

            Log.LogInformation($"Next PublishState: '{publishRequest.PublishState}, Next ProcessingState: '{publishRequest.ProcessingState}'");

            await SaveChangesAsync();
        }

        //private async Task PostProcessAsync(PublishRequest publishRequest)
        //{
        //    await Task.Delay(0);

        //}

    }

}
