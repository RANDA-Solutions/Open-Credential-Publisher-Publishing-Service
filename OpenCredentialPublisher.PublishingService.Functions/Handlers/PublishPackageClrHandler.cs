using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Utilities;
using OpenCredentialPublisher.Credentials.Drawing;
using OpenCredentialPublisher.PublishingService.Data;
using OpenCredentialPublisher.PublishingService.Services;
using OpenCredentialPublisher.PublishingService.Shared;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Functions
{
    public class PublishPackageClrHandler : PublishMessageHandlerBase, ICommandHandler<PublishPackageClrCommand>
    {
        private readonly IMediator _mediator;
        private readonly IFileStoreService _fileService;

        private readonly string readyState = PublishProcessingStates.PublishPackageClrReady;
        private readonly string processingState = PublishProcessingStates.PublishPackageClrProcessing;
        private readonly string failureState = PublishProcessingStates.PublishPackageClrFailure;
        private readonly string _appBaseUri;
        private readonly string _accessKeyUrl;

        public PublishPackageClrHandler(IConfiguration configuration, IOptions<AzureBlobOptions> blobOptions, OcpDbContext context, ILogger<PublishMessageHandlerBase> log, 
                        IMediator mediator, IFileStoreService fileService) : base(blobOptions, context, log)
        {
            _appBaseUri = configuration["AppBaseUri"];
            _accessKeyUrl = configuration["AccessKeyUrl"];

            _mediator = mediator;
            _fileService = fileService;
        }

        public async Task HandleAsync(PublishPackageClrCommand command)
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


        private async Task<PublishRequest> PreProcessAsync(PublishPackageClrCommand command)
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

            // Read File
            var contents = await _fileService.DownloadAsStringAsync(publishRequest.GetOriginalClr()?.FileName);

            // Inspect Package, Does it have PDF?
            var clr = JsonConvert.DeserializeObject<ClrDType>(contents);

            var artifacts = clr.Assertions?
                .Where(a => a.Evidence != null)
                .SelectMany(a => a.Evidence)?
                    .Where(e => e.Artifacts != null)
                    .SelectMany(e => e.Artifacts)
                        .ToList();

            var pdfs = artifacts?
                .Where(a => a.Url != null && a.Url.StartsWith("data:application/pdf"))
                .ToList();

            // Get most recent AccessKey
            string key = publishRequest.LatestAccessKey()?.Key;
            string url = PublishRequestExtensions.AccessKeyUrl(_accessKeyUrl, key, _appBaseUri, ScopeConstants.Wallet, DiscoveryDocumentCustomEndpointsConstants.CredentialsEndpoint, HttpMethods.Post);

            for (int pdfIndex = 0; pdfIndex < pdfs.Count; pdfIndex++)
            {
                var pdf = pdfs[pdfIndex];

                var (mimeType, bytes) = DataUrlUtility.ParseDataUrl(pdf.Url);

                // Create QR Code from AccessKey + WebViewer URL (Where is this retrieved?)
                // Append Page to PDF with QR Code
                var pdfBytes = PdfUtility.AppendQRCodePage(bytes, url, "PublishingService");

                var pdfFilename = PdfQrFilename(pdfIndex, publishRequest.RequestId);

                await _fileService.StoreAsync(pdfFilename, pdfBytes);

                publishRequest.Files.Add(File.CreateQrCodeImprintedPdf(pdfFilename));

                Log.LogInformation($"QR-Code Imprinted File Added: {pdfFilename}");

                // Update the Url;
                // Re-Encode PDF, Update CLR
                pdf.Url = DataUrlUtility.PdfToDataUrl(pdfBytes);

            }

            var filename = ClrWithPdfQrFilename(publishRequest.RequestId);

            // Upload CLR to Blob
            await _fileService.StoreAsync(filename, JsonConvert.SerializeObject(clr));

            // Update Database
            publishRequest.Files.Add(File.CreateQrCodeImprintedClr(filename));

            publishRequest.ProcessingState = PublishProcessingStates.PublishSignClrReady;
            publishRequest.PublishState = PublishStates.SignClr;

            Log.LogInformation($"Modified CLR (QR-Code Imprinted) File Added: {filename}");
            Log.LogInformation($"Next PublishState: '{publishRequest.PublishState}, Next ProcessingState: '{publishRequest.ProcessingState}'");

            await SaveChangesAsync();
        }

        private async Task PostProcessAsync(PublishRequest publishRequest)
        {
            switch (publishRequest.ProcessingState)
            {
                case PublishProcessingStates.PublishSignClrReady:
                    await _mediator.Publish(new PublishSignClrCommand(publishRequest.RequestId));
                    break;

                default:
                    throw new Exception("Invalid State");
            }
        }

        private string PdfQrFilename(int pdfIndex, string requestId)
        {
            return string.Format("{0:yyyy}/{0:MM}/{0:dd}/{0:HH}/clrpdfqr_{1}_{2}_{0:mmssffff}.pdf", DateTime.UtcNow, pdfIndex, requestId);
        }

        private string ClrWithPdfQrFilename(string requestId)
        {
            return string.Format("{0:yyyy}/{0:MM}/{0:dd}/{0:HH}/clrpdfqr_{1}_{0:mmssffff}.json", DateTime.UtcNow, requestId);
        }

    }

}
