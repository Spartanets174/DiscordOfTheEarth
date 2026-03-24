namespace DOTE.Domain.DomainModel
{
    public interface ICommandBus
    {
        public void RegisterHandler<T>(ICommandHandler<T> handler) where T : ICommand;
        public void Send<T>(T command) where T : ICommand;
    }
}