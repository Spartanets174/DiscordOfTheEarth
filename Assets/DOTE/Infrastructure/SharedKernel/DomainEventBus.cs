using DOTE.SharedKernel.Domain;
using System;
using System.Collections.Generic;

namespace DOTE.SharedKernel.Infrastructure
{
    public class DomainEventBus : IDomainEventBus
    {
        private Dictionary<Type, List<Delegate>> listenedEventsMap;

        public DomainEventBus()
        {
            listenedEventsMap = new();
        }

        public void Publish<T>(T domainEvent) where T : IDomainEvent
        {
            Type type = domainEvent.GetType();
            if (listenedEventsMap.TryGetValue(type, out List<Delegate> listenedEventActions))
            {
                foreach (Action<IDomainEvent> listenedEventAction in listenedEventActions)
                {
                    listenedEventAction?.Invoke(domainEvent);
                }
            }
        }

        public void Subscribe<T>(Action<T> action) where T : IDomainEvent
        {
            Type domainEventType = typeof(T);

            if (!listenedEventsMap.ContainsKey(domainEventType))
            {
                listenedEventsMap[domainEventType] = new List<Delegate>();
            }
                
            listenedEventsMap[domainEventType].Add(action);
        }

        public void Unsubscribe<T>(Action<T> action) where T : IDomainEvent
        {
            Type domainEventType = typeof(T);

            if (listenedEventsMap.TryGetValue(domainEventType, out List<Delegate> listenedEventActions))
            {
                listenedEventActions.Remove(action);
            }              
        }
    }
}