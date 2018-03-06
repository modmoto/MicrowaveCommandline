using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace GenericWebservice.Domain
{
    public interface IEventStore
    {
        T Load<T>(Guid id) where T : class;
        void AppendAll(List<DomainEventBase> domainEvents);
    }

    public class EventStore : IEventStore
    {
        public T Load<T>(Guid id) where T : class
        {
            using (var storeContext = new EventStoreContext())
            {
                var domainEventBases = storeContext.EventHistory.Where(ev => ev.EntityId == id);
                return Replay<T>(domainEventBases);
            }
        }

        private T Replay<T>(IQueryable<DomainEventBase> domainEventBases) where T : class
        {
            return null;
        }

        public void AppendAll(List<DomainEventBase> domainEvents)
        {
            throw new NotImplementedException();
        }
    }
}