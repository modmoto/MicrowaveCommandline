using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace GenericWebservice.Domain
{
    public interface IEventStore
    {
        Task AppendAll(List<DomainEventBase> domainEvents);
    }

    public class EventStore : IEventStore
    {
        public async Task AppendAll(List<DomainEventBase> domainEvents)
        {
            using (var context = new EventStoreContext())
            {
                context.EventHistory.AddRange(domainEvents);
                await context.SaveChangesAsync();
            }
        }
    }
}