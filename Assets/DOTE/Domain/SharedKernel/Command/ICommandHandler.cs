namespace DOTE.SharedKernel.Domain
{
    public interface ICommandHandler
    {
        public void Handle(ICommand command);
    }

    public interface ICommandHandler<TCommand>: ICommandHandler where TCommand : ICommand
    {
        public void Handle(TCommand command);
    }
}