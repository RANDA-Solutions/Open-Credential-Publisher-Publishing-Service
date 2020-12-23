using OpenCredentialPublisher.PublishingService.Shared;
using System;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Functions
{

    // https://dejanstojanovic.net/aspnet/2019/may/using-dispatcher-class-to-resolve-commands-and-queries-in-aspnet-core/
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public async Task HandleAsync<T>(T command) where T : ICommand
        {
            var service = this._serviceProvider.GetService(typeof(ICommandHandler<T>)) as ICommandHandler<T>;
            await service.HandleAsync(command);
        }
    }

}
