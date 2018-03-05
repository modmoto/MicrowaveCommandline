using System.Threading.Tasks;
using Domain;

namespace GenericWebServiceBuilder.Domain
{
    public class AggregateStore
    {
        public async Task AddAggregate<T>(T aggregate)
        {
            using (var aggregateStore = new AggregateStoreContext())
            {
                var user = aggregate as User;
                if (user != null) aggregateStore.Users.Add(user);

                await aggregateStore.SaveChangesAsync();
            }
        }

        public async Task UpdateAggregate<T>(T aggregate)
        {
            using (var aggregateStore = new AggregateStoreContext())
            {
                var user = aggregate as User;
                if (user != null) aggregateStore.Users.Update(user);

                await aggregateStore.SaveChangesAsync();
            }
        }
    }
}