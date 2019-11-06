using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppointmentTracker.Models;

namespace AppointmentTracker.Data
{
    public class SpaAppContext : DbContext, IReadOnlySpaAppContext
    {
        public SpaAppContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppointmentModel>().HasKey(x => x.Id).ForSqlServerIsClustered();
            modelBuilder.Entity<AppointmentModel>().Property(x => x.Id).UseSqlServerIdentityColumn();

            modelBuilder.Entity<CustomerModel>().HasKey(x => x.Id).ForSqlServerIsClustered();
            modelBuilder.Entity<CustomerModel>().Property(x => x.Id).UseSqlServerIdentityColumn();

            modelBuilder.Entity<ServiceProviderModel>().HasKey(x => x.Id).ForSqlServerIsClustered();
            modelBuilder.Entity<ServiceProviderModel>().Property(x => x.Id).UseSqlServerIdentityColumn();

            modelBuilder.Entity<AppointmentModel>().HasOne(x => x.Provider).WithMany(x => x.Appointments).HasForeignKey(x => x.ProviderId);
            modelBuilder.Entity<AppointmentModel>().HasIndex(x => x.ProviderId).HasName($"IX_{nameof(AppointmentModel)}_{nameof(AppointmentModel.Provider)}");
            modelBuilder.Entity<AppointmentModel>().HasOne(x => x.Client).WithMany(x => x.Appointments).HasForeignKey(x => x.ClientId);
            modelBuilder.Entity<AppointmentModel>().HasIndex(x => x.ClientId).HasName($"IX_{nameof(AppointmentModel)}_{nameof(AppointmentModel.Client)}");

            modelBuilder.Entity<ServiceProviderModel>().HasMany(x => x.Appointments).WithOne(x => x.Provider);

            modelBuilder.Entity<CustomerModel>().HasMany(x => x.Appointments).WithOne(x => x.Client);
        }

        public DbSet<AppointmentModel> Appointments { get; set; }

        public DbSet<CustomerModel> Customers { get; set; }

        public DbSet<ServiceProviderModel> Providers { get; set; }

        IQueryable<AppointmentModel> IReadOnlySpaAppContext.Appointments { get => Appointments.AsNoTracking(); }

        IQueryable<CustomerModel> IReadOnlySpaAppContext.Customers { get => Customers.AsNoTracking(); }

        IQueryable<ServiceProviderModel> IReadOnlySpaAppContext.Providers { get => Providers.AsNoTracking(); }

    }
}
