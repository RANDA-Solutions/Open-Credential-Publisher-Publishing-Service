using System.Threading.Tasks;

namespace OpenCredentialsPublisher.PublishingService.Shared
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }

}
