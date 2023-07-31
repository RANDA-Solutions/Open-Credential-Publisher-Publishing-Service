using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr;
using OpenCredentialPublisher.PublishingService.Data;
using OpenCredentialPublisher.PublishingService.Services;
using OpenCredentialPublisher.PublishingService.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Functions
{

    public class PublishProcessRequestHandler : PublishMessageHandlerBase, ICommandHandler<PublishProcessRequestCommand>
    {
        private readonly IMediator _mediator;
        private readonly IFileStoreService _fileService;

        private readonly string readyState = PublishProcessingStates.PublishRequestReady;
        private readonly string processingState = PublishProcessingStates.PublishRequestProcessing;
        private readonly string failureState = PublishProcessingStates.PublishRequestFailure;

        public PublishProcessRequestHandler(IOptions<AzureBlobOptions> blobOptions, OcpDbContext context, ILogger<PublishMessageHandlerBase> log, 
                            IMediator mediator, IFileStoreService fileService) : base(blobOptions, context, log)
        {
            _mediator = mediator;
            _fileService = fileService;
        }

        public async Task HandleAsync(PublishProcessRequestCommand command)
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


        private async Task<PublishRequest> PreProcessAsync(PublishProcessRequestCommand command)
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

            // Read File
           // var contents = await _fileService.DownloadAsStringAsync(publishRequest.GetOriginalClr()?.FileName);

            // Inspect Package, Does it have PDF?
            //var clr = JsonConvert.DeserializeObject<ClrDType>(contents);

            //var artifacts = clr.Assertions?
            //    .Where(a => a.Evidence != null)
            //    .SelectMany(a => a.Evidence)?
            //        .Where(e => e.Artifacts != null)  
            //        .SelectMany(e => e.Artifacts)
            //            .ToList();

            //var pdfs = artifacts?
            //    .Where(a => a.Url != null && a.Url.StartsWith("data:application/pdf"))
            //    .ToList();

            //var pdfCount = pdfs?.Count ?? 0;
            //var hasPdf = pdfCount > 0;
            // Create Access Key, Update Record (AccessKey, ClrHasPdf)
            //publishRequest.ContainsPdf = hasPdf;
            publishRequest.AccessKeys.Add(AccessKey.Create());

            // QRCode on PDF is not currently necessary
            //publishRequest.ProcessingState = (pdfCount > 0) ? PublishProcessingStates.PublishPackageClrReady : PublishProcessingStates.PublishSignClrReady;
            //publishRequest.PublishState = (pdfCount > 0) ? PublishStates.Packaging : PublishStates.SignClr;

            publishRequest.ProcessingState = PublishProcessingStates.PublishSignClrReady;
            publishRequest.PublishState = PublishStates.SignClr;

            Log.LogInformation($"Next PublishState: '{publishRequest.PublishState}, Next ProcessingState: '{publishRequest.ProcessingState}'");

            await SaveChangesAsync();
        }

        private async Task PostProcessAsync(PublishRequest publishRequest)
        {
            switch (publishRequest.ProcessingState)
            {
                case PublishProcessingStates.PublishPackageClrReady:
                    await _mediator.Publish(new PublishPackageClrCommand(publishRequest.RequestId));
                    break;

                case PublishProcessingStates.PublishSignClrReady:
                    await _mediator.Publish(new PublishSignClrCommand(publishRequest.RequestId));
                    break;

                default:
                    throw new Exception("Invalid State");
            }
        }

    }

}
