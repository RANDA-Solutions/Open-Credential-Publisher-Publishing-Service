using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenCredentialPublisher.Credentials.Clrs.Clr;
using OpenCredentialPublisher.Credentials.Clrs.Interfaces;
using OpenCredentialPublisher.Credentials.Drawing;
using OpenCredentialPublisher.Credentials.VerifiableCredentials;
using OpenCredentialPublisher.PublishingService.Data;
using OpenCredentialPublisher.PublishingService.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Services
{

    public class PublishService : IPublishService
    {
        private readonly OcpDbContext _context;
        private readonly IFileStoreService _store;
        private readonly IMediator _mediator;
        private readonly IKeyStore _keyStore;
        private readonly string _appBaseUri;
        private readonly string _accessKeyUrl;

        public PublishService(IConfiguration configuration, OcpDbContext context, IFileStoreService store, IMediator mediator, IKeyStore keyStore)
        {
            _appBaseUri = configuration["AppBaseUri"];
            _accessKeyUrl = configuration["accessKeyUrl"];
            _context = context;
            _store = store;
            _mediator = mediator;
            _keyStore = keyStore;
        }

        public async Task<string> ProcessRequestAsync(string id, ClrDType clr, string clientId)
        {
            var requestId = Guid.NewGuid().ToString("d");

            string filename = ClrFilename(requestId);

            var contents = JsonConvert.SerializeObject(clr);

            var filepath = await _store.StoreAsync(filename, contents);

            var request = new PublishRequest(requestId, clientId, id, filepath);

            _context.PublishRequests.Add(request);

            _context.ClrPublishLogs.Add(new ClrPublishLog(request.ClientId, request.RequestId, request.PublishState, "Request Received"));

            await _context.SaveChangesAsync();

            await _mediator.Publish(new PublishProcessRequestCommand(requestId));

            _context.ClrPublishLogs.Add(new ClrPublishLog(request.ClientId, requestId, request.PublishState, "Request issued to workflow"));

            await _context.SaveChangesAsync();

            return requestId;
        }


        public async Task<PublishStatusResult> GetAsync(string requestId, string clientId, string scope, string endpoint, string method)
        {
            var request = await _context.PublishRequests
                .Include(r => r.AccessKeys)
                .Where(r => r.RequestId == requestId).FirstOrDefaultAsync();

            if (request == null)
            {
                return null;
            }

            _context.ClrPublishLogs.Add(new ClrPublishLog(clientId, requestId, "Inquiry", $"Request status inquiry (State={request.PublishState})"));

            await _context.SaveChangesAsync();

            string accessKey = request.LatestAccessKey()?.Key;

            if (accessKey != null && request.PublishState == PublishStates.Complete)
            {
                var url = PublishRequestExtensions.AccessKeyUrl(_accessKeyUrl, accessKey, _appBaseUri, scope, endpoint, method);

                var qrCode = new ClrPublishQrCode 
                {
                    MimeType = "image/png",
                    Data = Convert.ToBase64String(QRCodeUtility.Create(url))
                };

                return new PublishStatusResult
                {
                    Status = request.PublishState,
                    Url = url,
                    AccessKey = accessKey,
                    QrCode = qrCode
                };

            }
            else
            {
                return new PublishStatusResult
                {
                    Status = request.PublishState
                };

            }
        }

        private string ClrFilename(string requestId)
        {
            return string.Format("{0:yyyy}/{0:MM}/{0:dd}/{0:HH}/clr_{1}_{0:mmssffff}.json", DateTime.UtcNow, requestId);
        }

        public async Task RevokeAsync(string requestId, string clientId)
        {
            var request = await _context.PublishRequests
                .Include(r => r.AccessKeys)
                .Include(r => r.SigningKeys)
                .Where(r => r.RequestId == requestId).FirstOrDefaultAsync();

            if (request == null)
            {
                throw new Exception("request does not exist");
            }

            if (request.ClientId != clientId)
            {                 
                throw new Exception("client_id requesting revocation is not the client who published");
            }

            if (request.PublishState == PublishStates.Revoked)
            {
                return;
            }

            request.PublishState = PublishStates.Revoked;
            request.ProcessingState = PublishProcessingStates.RevokedByClient;

            foreach (var accessKey in request.AccessKeys)
            {
                accessKey.Expired = true;
            }

            foreach (var signKey in request.SigningKeys)
            {
                signKey.Expired = true;

                await _keyStore.DeleteKeyAsync(signKey.KeyName);
            }
            
            await _context.SaveChangesAsync();

        }

        public async Task<VerifiableCredential> GetCredentialsAsync(string accessKey)
        {
            var request = await _context.PublishRequests
               .Include(r => r.Files)
               .Where(r => r.AccessKeys.Any(k => k.Key == accessKey && !k.Expired))
               .FirstOrDefaultAsync();

            var vcFileName = request?.GetVcWrappedClr()?.FileName;

            if (vcFileName == null)
                return null;

            var contents = await _store.DownloadAsStringAsync(vcFileName);

            var vc = JsonConvert.DeserializeObject<VerifiableCredential>(contents);

            return vc;
        }
    }



}
