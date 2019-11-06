using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentTracker.Models;
using AppointmentTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentTracker.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IRepository _repository;

        public AppointmentController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: Appointment
        public ActionResult Index()
        {
            return View(_repository.Appointments.Include(x => x.Provider).Include(x => x.Client));
        }

        // GET: Appointment/Details/5
        public ActionResult Details(int id)
        {
            return View(_repository.GetAppointment(id));
        }

        // GET: Appointment/Create
        public ActionResult Create()
        {                
                return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppointmentModel Appt)
        {
            try
            {
                _repository.AddAppointment(Appt);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }
        }

        // GET: Appointment/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_repository.GetAppointment(id));
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AppointmentModel Appt)
        {
            try
            {
                _repository.UpdateAppointment(id, Appt);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(_repository.GetAppointment(id));
            }
        }

        // GET: Appointment/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_repository.GetAppointment(id));
        }

        // POST: Appointment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AppointmentModel Appt)
        {
            try
            {
                _repository.DeleteAppointment(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }
        }
    }
}