using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppointmentTracker.Data;
using AppointmentTracker.Models;

namespace AppointmentTracker.Services
{
    public interface IRepository : IDisposable
    {
        IQueryable<AppointmentModel> Appointments { get; }
        IQueryable<ServiceProviderModel> Providers { get; }
        IQueryable<CustomerModel> Customers { get; }

        void AddAppointment(AppointmentModel appointment);
        void AddProvider(ServiceProviderModel provider);
        void AddCustomer(CustomerModel customer);

        void DeleteAppointment(int id);
        void DeleteProvider(int id);
        void DeleteCustomer(int id);

        void UpdateAppointment(int id, AppointmentModel appointment);
        void UpdateProvider(int id, ServiceProviderModel provider);
        void UpdateCustomer(int id, CustomerModel customer);

        AppointmentModel GetAppointment(int id);
        ServiceProviderModel GetProvider(int id);
        CustomerModel GetCustomer(int id);

        List<AppointmentModel> GetAppointmentsForProvider(int providerId);


    }
}
