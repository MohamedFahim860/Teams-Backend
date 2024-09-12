using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.Domain.Models;
using Teams.Persistence.EntityConfiguration;

namespace Teams.Persistence.Context
{
    public class TeamsDbContext : DbContext
    {
        public TeamsDbContext(DbContextOptions<TeamsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserChannelEntityTypeConfiguration());

            //modelBuilder.ApplyConfiguration(new EmployeeDetailsEntityTypeConfiguration());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<UserChannel> UserChannels { get; set; }
        public DbSet<Message> Message { get; set; }

    }
}

//The DbContext class in Entity Framework Core is the primary class responsible for interacting with the database.
//It acts as a bridge between your .NET application and the database, allowing you to query, insert, update, and delete data using .NET objects.

//1. Database Connection Management
//2. Tracking Entity Changes
//3. Model Configuration
//4. LINQ Queries
//5. Unit of Work Pattern
//6. Entity Sets(DbSet)
