using System.Threading.Tasks;
using Domain;

namespace GenericWebservice.Domain
{
    public class EventStore
    {
        public async Task AddAggregate<T>(T aggregate)
        {
            using (var aggregateStore = new EventStoreContext())
            {
                var user = aggregate as CreateUserEvent;
                if (user != null) aggregateStore.CreateUserEvents.Add(user);

                await aggregateStore.SaveChangesAsync();
            }
        }

        public async Task UpdateAggregate<T>(T aggregate)
        {
            using (var aggregateStore = new EventStoreContext())
            {
                var user = aggregate as UserUpdateAgeEvent;
                if (user != null) aggregateStore.UserUpdateAgeEvents.Update(user);

                await aggregateStore.SaveChangesAsync();
            }
        }
    }
}