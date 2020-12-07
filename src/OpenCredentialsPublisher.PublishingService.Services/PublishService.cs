using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenCredentialsPublisher.Credentials.Clrs.Clr;
using OpenCredentialsPublisher.Credentials.Drawing;
using OpenCredentialsPublisher.PublishingService.Data;
using OpenCredentialsPublisher.PublishingService.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialsPublisher.PublishingService.Services
{

    public class PublishService : IPublishService
    {
        private readonly OcpDbContext _context;
        private readonly IFileStoreService _store;
        private readonly IMediator _mediator;

        public PublishService(OcpDbContext context, IFileStoreService store, IMediator mediator)
        {
            _context = context;
            _store = store;
            _mediator = mediator;
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


        public async Task<PublishStatusResult> GetAsync(string requestId, string clientId)
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

            var key = request.LatestAccessKey()?.Key;

            ClrPublishQrCode qrCode = null;

            if (key != null && request.PublishState == PublishStates.Complete)
            {
                var url = PublishRequestExtensions.AccessKeyUrl(key);

                qrCode = new ClrPublishQrCode 
                {
                    MimeType = "image/png",
                    Data = Convert.ToBase64String(QRCodeUtility.Create(url))
                };
            }

            return new PublishStatusResult
            {
                Status = request.PublishState,
                AccessKey = key,
                QrCode = qrCode
            };
        }

        private string ClrFilename(string requestId)
        {
            return string.Format("{0:yyyy}/{0:MM}/{0:dd}/{0:HH}/clr_{1}_{0:mmssffff}.json", DateTime.UtcNow, requestId);
        }


    }



}
