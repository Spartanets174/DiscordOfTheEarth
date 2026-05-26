namespace DOTE.SharedKernel.Domain
{
    public interface ICommandHandler
    {
        public void Handle(ICommand command);
    }

    public interface ICommandHandler<TCommand> : ICommandHandler where TCommand : ICommand
    {
        public void Handle(TCommand command);
    }

    public abstract class ACommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        public void Handle(TCommand command)
        {
            HandleHook(command);
        }

        public void Handle(ICommand command)
        {
            HandleHook((TCommand)command);
        }

        public abstract void HandleHook(TCommand command);
    }
}