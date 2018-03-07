using System;
using System.Threading.Tasks;
using Domain.Users;

namespace GenericWebServiceBuilder.Domain
{
    public interface IAggregateStore
    {
        Task AddAggregate<T>(T aggregate);
        Task UpdateAggregate<T>(T aggregate);
        Task<dynamic> GetAggregate<T>(Guid id);
    }

    public class AggregateStore : IAggregateStore
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

        public async Task<dynamic> GetAggregate<T>(Guid id)
        {
            using (var aggregateStore = new AggregateStoreContext())
            {
                if (typeof(T) == typeof(User))
                {
                    return await aggregateStore.Users.FindAsync(id);
                }

                return null;
            }
        }
    }
}