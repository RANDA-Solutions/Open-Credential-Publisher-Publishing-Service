using System.Threading.Tasks;

namespace OpenCredentialsPublisher.PublishingService.Shared
{
    public interface ICommandDispatcher
    {
        Task HandleAsync<T>(T command) where T : ICommand;
    }

}
