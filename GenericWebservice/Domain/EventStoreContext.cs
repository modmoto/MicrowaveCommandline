using Domain;
using Microsoft.EntityFrameworkCore;

namespace GenericWebservice.Domain
{
    public class EventStoreContext : DbContext
    {
        public DbSet<UserUpdateAgeEvent> UserUpdateAgeEvents { get; set; }
        public DbSet<UserUpdateNameEvent> UserUpdateNameEvents { get; set; }
        public DbSet<CreateUserEvent> CreateUserEvents { get; set; }
        public DbSet<CreatePostEvent> CreatePostEvents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;");
        }
    }
}