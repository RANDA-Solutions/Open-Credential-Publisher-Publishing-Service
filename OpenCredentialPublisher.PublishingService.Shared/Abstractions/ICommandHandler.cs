using System.Threading.Tasks;

namespace OpenCredentialPublisher.PublishingService.Shared
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }

}
