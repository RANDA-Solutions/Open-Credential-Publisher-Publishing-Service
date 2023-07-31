using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Clr;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Interfaces;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Utilities;
using OpenCredentialPublisher.Credentials.Clrs.v2_0;
using OpenCredentialPublisher.Credentials.Cryptography;
using OpenCredentialPublisher.Credentials.VerifiableCredentials;
using OpenCredentialPublisher.PublishingService.Data;
using OpenCredentialPublisher.PublishingService.Services;
using OpenCredentialPublisher.PublishingService.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Functions
{
    public class PublishSignClrHandler : PublishMessageHandlerBase, ICommandHandler<PublishSignClrCommand>
    {
        private readonly IMediator _mediator;
        private readonly IFileStoreService _fileService;
        private readonly IKeyStore _keyStore;

        private readonly string _appBaseUri;

        private readonly string readyState = PublishProcessingStates.PublishSignClrReady;
        private readonly string processingState = PublishProcessingStates.PublishSignClrProcessing;
        private readonly string failureState = PublishProcessingStates.PublishSignClrFailure;

        public PublishSignClrHandler(IConfiguration configuration, IOptions<AzureBlobOptions> blobOptions, OcpDbContext context, ILogger<PublishMessageHandlerBase> log, 
                            IMediator mediator, IFileStoreService fileService, IKeyStore keyStore) : base(blobOptions, context, log)
        {
            _mediator = mediator;
            _fileService = fileService;
            _appBaseUri = configuration["AppBaseUri"];
            _keyStore = keyStore;
        }

        public async Task HandleAsync(PublishSignClrCommand command)
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


        private async Task<PublishRequest> PreProcessAsync(PublishSignClrCommand command)
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
            //var latestFile = publishRequest.ContainsPdf.Value ? publishRequest.GetQrCodeImprintedClr() : publishRequest.GetOriginalClr();
            var latestFile = publishRequest.GetOriginalClr();
            // Download PdfQrCodeClrFilePath or OriginalClrFilePath
            var contents = await _fileService.DownloadAsStringAsync(latestFile.FileName);

            var baseUri = new System.Uri(_appBaseUri);

            // Inspect Package, Does it have PDF?
            var clr = JsonConvert.DeserializeObject<ClrDType>(contents);
            if (publishRequest.Pathway == Pathways.Publish2_0)
            {
                var transformService = new Clr1_0ToClr2_0Service();
                var clrCredential = await transformService.Transform(_appBaseUri, publishRequest, clr);
                await SaveChangesAsync();

                var vcFilename = VcWrappedClrFilename(publishRequest.RequestId);

                // Upload CLR to Blob
                await _fileService.StoreAsync(vcFilename, JsonConvert.SerializeObject(clrCredential));

                publishRequest.Files.Add(File.CreateVCWrapped(vcFilename));
                if (publishRequest.PushAfterPublish)
                {
                    publishRequest.ProcessingState = PublishProcessingStates.PublishPushReady;
                    publishRequest.PublishState = PublishStates.Pushing;
                }
                else
                {
                    publishRequest.ProcessingState = PublishProcessingStates.PublishNotifyReady;
                    publishRequest.PublishState = PublishStates.Complete;
                }
                publishRequest.PackageSignedTimestamp = DateTimeOffset.UtcNow;
                Log.LogInformation($"VC-Wrapped File Added: {vcFilename}");
                Log.LogInformation($"Next PublishState: '{publishRequest.PublishState}, Next ProcessingState: '{publishRequest.ProcessingState}'");

                await SaveChangesAsync();
            } 
            else
            {
                var key = SigningKey.Create(clr.Publisher?.Id);
                key.KeyType = CryptoSuites.RsaSignature2018;
                key.StoredInKeyVault = true;
                // Create Key in KeyVault
                var keyIdentifier = await _keyStore.CreateKeyAsync(key.KeyName, key.IssuerId);

                // Persist VaultKeyId to ClrPublishRequest record
                key.VaultKeyIdentifier = keyIdentifier;

                publishRequest.SigningKeys.Add(key);

                await SaveChangesAsync();

                var credentials = new OcpSigningCredentials()
                {
                    KeyId = key.KeyName,
                    Algorithm = SecurityAlgorithms.RsaSha512,
                    KeyIdentifier = keyIdentifier
                };


                var signingUtility = new SigningUtility(_keyStore);

                // Sign CLR
                var signedClr = signingUtility.Sign(clr, new Uri(UriUtility.Combine(baseUri, "api")), credentials: credentials);

                var signedFilename = ClrWithSignatureFilename(publishRequest.RequestId);

                var clrJson = JsonConvert.SerializeObject(clr);

                // Upload Signed CLR to Blob
                await _fileService.StoreAsync(signedFilename, clrJson);

                // Add Signed File Database
                publishRequest.Files.Add(File.CreateSigned(signedFilename));
                Log.LogInformation($"Signed File Added: {signedFilename}");

                await SaveChangesAsync();

                var clrSet = new ClrSetSubject();
                clrSet.SignedClrs ??= new List<string>();
                clrSet.SignedClrs.Add(signedClr);
                clrSet.Clrs ??= new List<ClrDType>();
                //clrSet.Clrs.Add(clr);

                var verifiableCredential = new Credentials.VerifiableCredentials.VerifiableCredential
                {
                    Contexts = new List<string>(new[] { "https://www.w3.org/2018/credentials/v1", "https://purl.imsglobal.org/spec/clr/v1p0/context/clr_v1p0.jsonld" }),
                    Types = new List<string>(new[] { "VerifiableCredential" }),
                    Id = UriUtility.Combine(baseUri, "api", "credentials", publishRequest.RequestId),
                    Issuer = UriUtility.Combine(baseUri, "api", "issuers", publishRequest.ClientId),
                    IssuanceDate = DateTime.UtcNow,
                    CredentialSubjects = new List<Credentials.VerifiableCredentials.ICredentialSubject>(new[] { clrSet }),
                    CredentialStatus = new CredentialStatus { Id = UriUtility.Combine(baseUri, "api", "status", publishRequest.RevocationList.PublicId), Type = nameof(RevocationList) }
                };

                var challenge = Guid.NewGuid().ToString();
                verifiableCredential.Proof = null;
                await verifiableCredential.CreateProof(credentials, _keyStore, ProofPurposeEnum.assertionMethod, new Uri(UriUtility.Combine(baseUri, "api", "keys", key.IssuerId, key.KeyName)), challenge);

                var vcFilename = VcWrappedClrFilename(publishRequest.RequestId);

                // Upload CLR to Blob
                await _fileService.StoreAsync(vcFilename, JsonConvert.SerializeObject(verifiableCredential));

            publishRequest.Files.Add(File.CreateVCWrapped(vcFilename));
            if (publishRequest.PushAfterPublish)
            {
                publishRequest.ProcessingState = PublishProcessingStates.PublishPushReady;
                publishRequest.PublishState = PublishStates.Pushing;
            } 
            else
            {
                publishRequest.ProcessingState = PublishProcessingStates.PublishNotifyReady;
                publishRequest.PublishState = PublishStates.Complete;
            }
            
            publishRequest.PackageSignedTimestamp = DateTimeOffset.UtcNow;
            Log.LogInformation($"VC-Wrapped File Added: {vcFilename}");
            Log.LogInformation($"Next PublishState: '{publishRequest.PublishState}, Next ProcessingState: '{publishRequest.ProcessingState}'");

                await SaveChangesAsync();
            }
           
        }

        private async Task PostProcessAsync(PublishRequest publishRequest)
        {
            switch (publishRequest.ProcessingState)
            {

                case PublishProcessingStates.PublishNotifyReady:
                    await _mediator.Publish(new PublishNotifyCommand(publishRequest.RequestId));
                    break;
                case PublishProcessingStates.PublishPushReady:
                    await _mediator.Publish(new PublishPushCommand(publishRequest.RequestId));
                    break;

                default:
                    throw new Exception("Invalid State");
            }
        }

        private string ClrWithSignatureFilename(string requestId)
        {
            return string.Format("{0:yyyy}/{0:MM}/{0:dd}/{0:HH}/clrsigned_{1}_{0:mmssffff}.json", DateTime.UtcNow, requestId);
        }

        private string VcWrappedClrFilename(string requestId)
        {
            return string.Format("{0:yyyy}/{0:MM}/{0:dd}/{0:HH}/clrvcwrap_{1}_{0:mmssffff}.json", DateTime.UtcNow, requestId);
        }

    }

}
