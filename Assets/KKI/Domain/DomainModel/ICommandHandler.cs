namespace DOTE.Domain.DomainModel
{
    public interface ICommandHandler<T> where T: ICommand
    {
        public void Handle(T domainEvent);
    }
}