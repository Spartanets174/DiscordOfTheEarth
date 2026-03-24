namespace DOTE.Domain.DomainModel
{
    public interface IDomainEventBus
    {
        public void Publish<T>(T domainEvent) where T : IDomainEvent;
        public void Subscribe<T>() where T : IDomainEvent;
        public void Unsubscribe<T>() where T : IDomainEvent;
    }
}