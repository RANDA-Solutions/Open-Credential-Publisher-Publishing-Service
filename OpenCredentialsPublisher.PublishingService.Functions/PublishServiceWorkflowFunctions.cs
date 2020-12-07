using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenCredentialsPublisher.PublishingService.Data;
using OpenCredentialsPublisher.PublishingService.Shared;
using System;
using System.Threading.Tasks;

namespace OpenCredentialsPublisher.PublishingService.Functions
{
    public class PublishServiceWorkflowFunctions
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILogger<PublishServiceWorkflowFunctions> log;

        public PublishServiceWorkflowFunctions(ICommandDispatcher commandDispatcher, ILogger<PublishServiceWorkflowFunctions> log)
        {
            _commandDispatcher = commandDispatcher;
            this.log = log;
        }

        [FunctionName("PublishProcessRequestQueueTrigger")]
        public async Task RunPublishProcessRequest([QueueTrigger(PublishQueues.PublishRequest)] PublishProcessRequestCommand command)
        {
            log.LogInformation($"PublishProcessRequestQueueTrigger function processed: {JsonConvert.SerializeObject(command)}");

            try
            {
                await _commandDispatcher.HandleAsync(command);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                throw;
            }
        }


        [FunctionName("PublishPackageClrQueueTrigger")]
        public async Task RunPublishPackageClr([QueueTrigger(PublishQueues.PublishPackageClr)] PublishPackageClrCommand command)
        {
            log.LogInformation($"PublishPackageClrQueueTrigger function processed: {JsonConvert.SerializeObject(command)}");

            try
            {
                await _commandDispatcher.HandleAsync(command);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                throw;
            }
        }

        [FunctionName("PublishSignClrQueueTrigger")]
        public async Task PublishSignPackage([QueueTrigger(PublishQueues.PublishSignClr)] PublishSignClrCommand command)
        {
            log.LogInformation($"PublishSignClrQueueTrigger function processed: {JsonConvert.SerializeObject(command)}");

            try
            {
                await _commandDispatcher.HandleAsync(command);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                throw;
            }
        }


        [FunctionName("PublishNotifyQueueTrigger")]
        public async Task PublishNotifyPackage([QueueTrigger(PublishQueues.PublishNotify)] PublishNotifyCommand command, 
            int dequeueCount)
        {
            log.LogInformation($"PublishNotifyQueueTrigger function processed: {JsonConvert.SerializeObject(command)}");

            try
            {
                await _commandDispatcher.HandleAsync(command);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                throw;
            }
        }

    }

}
