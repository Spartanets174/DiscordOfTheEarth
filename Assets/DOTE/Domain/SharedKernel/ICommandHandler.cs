namespace DOTE.SharedKernel.Domain
{
    public interface ICommandHandler<T> where T: ICommand
    {
        public void Handle(T domainEvent);
    }
}