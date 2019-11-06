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
    public class CustomerController : Controller
    {
        private readonly IRepository _repository;

        public CustomerController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: ServiceProvider
        public ActionResult Index()
        {
            return View(_repository.Customers);
        }

        // GET: ServiceProvider/Details/5
        public ActionResult Details(int id)
        {
            return View(_repository.GetCustomer(id));
        }

        // GET: ServiceProvider/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceProvider/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerModel customer)
        {
            try
            {
                _repository.AddCustomer(customer);
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
            return View(_repository.GetCustomer(id));
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CustomerModel customer)
        {
            try
            {
                _repository.UpdateCustomer(id, customer);
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
            return View(_repository.GetCustomer(id));
        }

        // POST: ServiceProvider/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CustomerModel customerProvider)
        {
            try
            {
                _repository.DeleteCustomer(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}