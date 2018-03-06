using System;
using System.Collections.Generic;
using Domain;

namespace GenericWebservice.Domain
{
    public interface IEventStore
    {
        T Load<T>(Guid id);
        void AppendAll(List<DomainEventBase> domainEvents);
    }

    public class EventStore : IEventStore
    {
        public T Load<T>(Guid id)
        {
            using (var storeContext = new EventStoreContext())
            {
                throw new NotImplementedException();
                //storeContext.EventHistory.
            }
        }

        public void AppendAll(List<DomainEventBase> domainEvents)
        {
            throw new NotImplementedException();
        }
    }
}