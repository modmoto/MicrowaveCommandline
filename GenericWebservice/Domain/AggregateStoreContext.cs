using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace GenericWebServiceBuilder.Domain
{
    public class AggregateStoreContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;");
        }
    }
}