using System;
using System.Threading.Tasks;
using Domain.Users;
using GenericWebServiceBuilder.Domain;

namespace GeneratedWebService.Controllers
{
    internal class UserRepository : IUserRepository
    {
        public async Task<User> GetUser(Guid id)
        {
            using (var aggregateStore = new AggregateStoreContext())
            {
                return await aggregateStore.Users.FindAsync(id);
            }
        }

        public async Task UpdateUser(User user)
        {
            using (var aggregateStore = new AggregateStoreContext())
            {
                aggregateStore.Users.Update(user);
                await aggregateStore.SaveChangesAsync();
            }
        }

        public async Task CreateUser(User user)
        {
            using (var aggregateStore = new AggregateStoreContext())
            {
                aggregateStore.Users.Add(user);
                await aggregateStore.SaveChangesAsync();
            }
        }
    }

    public interface IUserRepository
    {
        Task<User> GetUser(Guid id);
        Task UpdateUser(User parsedUser);
        Task CreateUser(User userEventUser);
    }
}