using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppointmentTracker.Models;

namespace AppointmentTracker.Data
{
    public interface IReadOnlySpaAppContext
    {
        IQueryable<AppointmentModel> Appointments { get; }

        IQueryable<CustomerModel> Customers { get; }

        IQueryable<ServiceProviderModel> Providers { get; }
    }
}
