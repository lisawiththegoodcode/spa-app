using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentTracker.Models;
using AppointmentTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AppointmentTracker.Controllers
{
    public class ServiceProviderController : Controller
    {
        private readonly IRepository _repository;

        public ServiceProviderController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: ServiceProvider
        public ActionResult Index()
        {
            return View(_repository.Providers);
        }

        // GET: ServiceProvider/Details/5
        public ActionResult Details(int id, ServiceProviderModel serviceProvider)
        {
            serviceProvider.Appointments = _repository.GetAppointmentsForProvider(id);
            return View(_repository.GetProvider(id));
        }

        // GET: ServiceProvider/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceProvider/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceProviderModel serviceProvider)
        {
            try
            {
                _repository.AddProvider(serviceProvider);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_repository.GetProvider(id));
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ServiceProviderModel serviceProvider)
        {
            try
            {
                _repository.UpdateProvider(id, serviceProvider);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ServiceProvider/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_repository.GetProvider(id));
        }

        // POST: ServiceProvider/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ServiceProviderModel serviceProvider)
        {
            try
            {
                _repository.DeleteProvider(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}