using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Api
{

    public class RequestNotificationHub : Hub {

        public async Task JoinGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName)) return;

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName.ToLower());
        }

        public async Task LeaveGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName)) return;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName.ToLower());
        }

        public void SendPublishStatusUpdate(string requestId, string status)
        {
            //Clients.Group(clientId).SendAsync("PublishUpdate", requestId, status);
            Clients.Group(requestId.ToLower()).SendAsync("PublishUpdate", requestId, status);
        }
    }

}
