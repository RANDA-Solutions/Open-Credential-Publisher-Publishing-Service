using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Shared
{
    public interface ICommandDispatcher
    {
        Task HandleAsync<T>(T command) where T : ICommand;
    }

}
