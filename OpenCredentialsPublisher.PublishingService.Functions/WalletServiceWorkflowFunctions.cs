using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace OpenCredentialsPublisher.PublishingService.Functions
{

    public static class WalletServiceWorkflowFunctions
    {

        [FunctionName("CreateRelationshipRequestsQueueTrigger")]
        public static void RunCreateRelationshipRequests([QueueTrigger("req-create-rel")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }

        [FunctionName("RelationshipInviteRequestsQueueTrigger")]
        public static void RunRelationshipInviteRequests([QueueTrigger("req-rel-invite")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }

        [FunctionName("AwaitInviteRequestsQueueTrigger")]
        public static void RunAwaitInviteRequests([QueueTrigger("req-await-invite")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }

        [FunctionName("WriteSchemaRequestsQueueTrigger")]
        public static void RunWriteSchemaRequests([QueueTrigger("req-write-schema")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }


        [FunctionName("WriteCredDefsRequestsQueueTrigger")]
        public static void RunWriteCredDefsRequests([QueueTrigger("req-write-creddefs")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }

        [FunctionName("SendOfferRequestsQueueTrigger")]
        public static void RunSendOfferRequests([QueueTrigger("req-send-offer")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }

}
