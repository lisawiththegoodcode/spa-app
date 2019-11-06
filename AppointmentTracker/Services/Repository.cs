using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppointmentTracker.Data;
using AppointmentTracker.Models;

namespace AppointmentTracker.Services
{
    public class Repository : IRepository
    {
        private readonly SpaAppContext _spaAppContext;
        private readonly IReadOnlySpaAppContext _readOnlySpaAppContext;

        public Repository(SpaAppContext SpaAppContext, IReadOnlySpaAppContext readOnlySpaAppContext)
        {
            _spaAppContext = SpaAppContext;
            _readOnlySpaAppContext = readOnlySpaAppContext;
        }

        public IQueryable<AppointmentModel> Appointments => _readOnlySpaAppContext.Appointments;

        public IQueryable<CustomerModel> Customers => _readOnlySpaAppContext.Customers;

        public IQueryable<ServiceProviderModel> Providers => _readOnlySpaAppContext.Providers;

        #region Appointment Methods
        public void AddAppointment(AppointmentModel appointment)
        {
            if (!IsProviderAvailable(appointment))
            {
                throw new Exception("The selected service provider is not available for an appointment at this time.");
            }

            if (!IsClientAvailable(appointment))
            {
                throw new Exception("The selected customer already has an appointment booked at this time.");
            }

            _spaAppContext.Appointments.Add(appointment);
            _spaAppContext.SaveChanges();
        }

        public void UpdateAppointment(int id, AppointmentModel appointment)
        {
            if (!IsProviderAvailable(appointment))
            {
                throw new Exception("The selected service provider is not available for an appointment at this time.");
            }

            if (!IsClientAvailable(appointment))
            {
                throw new Exception("The selected customer already has an appointment booked at this time.");
            }

            appointment.Id = id;
            _spaAppContext.Appointments.Update(appointment);
            _spaAppContext.SaveChanges();
        }

        public void DeleteAppointment(int id)
        {
            var toBeDeleted = _spaAppContext.Appointments.First(SelectAppointmentById(id));
            _spaAppContext.Appointments.Remove(toBeDeleted);
            _spaAppContext.SaveChanges();
        }

        public AppointmentModel GetAppointment(int id)
        {
            return _readOnlySpaAppContext.Appointments
                .Include(x => x.Provider)
                .Include(x => x.Client)
                .First(SelectAppointmentById(id));
        }
        #endregion

        #region Appointment Checker Methods
        public bool IsProviderAvailable(AppointmentModel proposedAppt)
        {
            //NOTE: I chose to assume a provider would be free so long that two appts did not start at the same time. 
            //If I were to develop this further, services could have a defined time allotments that I would take into consideration in the provider's availability.

            return !_readOnlySpaAppContext.Appointments
                .Any(appt => appt.AppointmentTime == proposedAppt.AppointmentTime
                    && appt.ProviderId == proposedAppt.ProviderId
                    && appt.Id != proposedAppt.Id);
        }

        public bool IsClientAvailable(AppointmentModel proposedAppt)
        {
            //NOTE: I chose to assume a client would be free so long that two appts did not start at the same time. 
            //If I were to develop this further, services could have a defined time allotments that I would take into consideration in the provider's availability.

            return !_readOnlySpaAppContext.Appointments
                .Any(appt => appt.AppointmentTime == proposedAppt.AppointmentTime
                    && appt.ClientId == proposedAppt.ClientId
                    && appt.Id != proposedAppt.Id);
        }
        #endregion

        #region Provider Methods
        public void AddProvider(ServiceProviderModel provider)
        {
            _spaAppContext.Providers.Add(provider);
            _spaAppContext.SaveChanges();
        }

        public void UpdateProvider(int id, ServiceProviderModel provider)
        {
            provider.Id = id;
            _spaAppContext.Providers.Update(provider);
            _spaAppContext.SaveChanges();
        }

        public void DeleteProvider(int id)
        {
            var toBeDeleted = _spaAppContext.Providers.First(SelectProvidersById(id));
            _spaAppContext.Providers.Remove(toBeDeleted);
            _spaAppContext.SaveChanges();
        }

        public ServiceProviderModel GetProvider(int id)
        {
            return _readOnlySpaAppContext.Providers.First(SelectProvidersById(id));
        }

        public List<AppointmentModel> GetAppointmentsForProvider(int providerID)
        {
            return _readOnlySpaAppContext.Appointments
                .Where(appt => appt.ProviderId == providerID)
                .OrderBy(appt => appt.AppointmentTime)
                .Include(appt => appt.Client)
                .ToList();
        }
        #endregion

        #region Customer Methods
        public void AddCustomer(CustomerModel customer)
        {
            _spaAppContext.Customers.Add(customer);
            _spaAppContext.SaveChanges();
        }

        public void UpdateCustomer(int id, CustomerModel customer)
        {
            customer.Id = id;
            _spaAppContext.Customers.Update(customer);
            _spaAppContext.SaveChanges();
        }

        public void DeleteCustomer(int id)
        {
            var toBeDeleted = _spaAppContext.Customers.First(SelectCustomersById(id));
            _spaAppContext.Customers.Remove(toBeDeleted);
            _spaAppContext.SaveChanges();
        }

        public CustomerModel GetCustomer(int id)
        {
            return _readOnlySpaAppContext.Customers.First(SelectCustomersById(id));
        }
        #endregion

        #region Selector Functions
        private static Func<AppointmentModel, bool> SelectAppointmentById(int id)
        {
            return appointment => appointment.Id == id;
        }

        private static Func<ServiceProviderModel, bool> SelectProvidersById(int id)
        {
            return provider => provider.Id == id;
        }

        private static Func<CustomerModel, bool> SelectCustomersById(int id)
        {
            return customer => customer.Id == id;
        }

        public void Dispose()
        {
            _spaAppContext?.Dispose();
            (_readOnlySpaAppContext as IDisposable)?.Dispose();
        }
        #endregion
    }
}
