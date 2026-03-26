using System;

namespace DOTE.SharedKernel.Domain
{
    public interface IDomainEventBus
    {
        public void Publish<T>(T domainEvent) where T : IDomainEvent;
        public void Subscribe<T>(Action<T> action) where T : IDomainEvent;
        public void Unsubscribe<T>(Action<T> action) where T : IDomainEvent;
    }
}