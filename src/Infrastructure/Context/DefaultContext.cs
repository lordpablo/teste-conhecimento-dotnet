using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SampleTest.Domain.Models;
using SampleTest.Infrastructure.Context.Seeds;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SampleTest.Infrastructure.Context
{
    [ExcludeFromCodeCoverage]
    public partial class DefaultContext : DbContext
    {
        [ExcludeFromCodeCoverage]
        public DefaultContext(DbContextOptions<DefaultContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }
        public virtual DbSet<UserModel> Users { get; set; }
        public virtual DbSet<ClientModel> Clients { get; set; }

        [ExcludeFromCodeCoverage]

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                return;
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    foreach (var property in entityType.GetProperties())
            //    {
            //        if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
            //        {
            //            property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
            //                   v => DateTime.SpecifyKind(v.ToUniversalTime(), DateTimeKind.Utc), // Save UTC
            //                   v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Read UTC
            //               ));
            //        }
            //    }
            //}

            //This defines UniqueFields on database
            modelBuilder.Entity<UserModel>().HasIndex(e => e.Username).IsUnique();
            modelBuilder.Entity<ClientModel>().HasIndex(e => e.CPF).IsUnique();
            modelBuilder.Entity<ClientModel>().HasIndex(e => e.Email).IsUnique();

            //This is for add to migrations new data from the start only for tests.
            modelBuilder.Entity<ClientModel>().HasData(ClientSeeds.NewSeeds());
            modelBuilder.Entity<UserModel>().HasData(UserSeeds.NewSeeds());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
