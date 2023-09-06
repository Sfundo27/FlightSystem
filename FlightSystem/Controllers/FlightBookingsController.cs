using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FlightSystem.Models;
using Microsoft.AspNet.Identity;

namespace FlightSystem.Controllers
{
    public class FlightBookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FlightBookings
        public ActionResult Index()
        {
            var flightBookings = db.bookings.Include(f => f.flight).Include(f => f.user);
            return View(flightBookings.ToList());
        }
        public ActionResult UserFlight()
        {
            var id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var flights = db.bookings.Where(f => f.Id == id).ToList();
            return View(flights);
        }
        // GET: FlightBookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlightBooking flightBooking = db.bookings.Find(id);
            if (flightBooking == null)
            {
                return HttpNotFound();
            }
            return View(flightBooking);
        }
        public ActionResult Book(int? id)
        {
            ViewBag.FlightId = id;
            return View();
        }
        [HttpPost]

        public ActionResult Book(FlightBooking flightBooking)
        {
            var id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var pendingBooking = db.bookings.Where(b => b.Id == id && b.Status == "Pending Payment").Count();
            if (pendingBooking > 0)
            {
                ModelState.AddModelError("", "You cannot book another flight because there is pending payment of another booking");
                return View(flightBooking);
            }
            if (ModelState.IsValid)
            {
                flightBooking.DateOfBooking = DateTime.Now;
                flightBooking.TotalCost = flightBooking.CalcCost();
                flightBooking.Status = "Pending Payment";
                flightBooking.Id = id;
                db.bookings.Add(flightBooking);
                db.SaveChanges();
                return RedirectToAction("PassengeHome", "Home");
            }

            ViewBag.FlightId = new SelectList(db.flights, "FlightId", "FlightId", flightBooking.FlightId);
            return View(flightBooking);
        }
        // GET: FlightBookings/Create
        public ActionResult Create()
        {
            ViewBag.FlightId = new SelectList(db.flights, "FlightId", "FlightId");
            
            return View();
        }

        // POST: FlightBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingId,FlightClass,TripType,NumberOfPassenger,FlightId")] FlightBooking flightBooking)
        {
            var id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var pendingBooking = db.bookings.Where(b => b.Id == id && b.Status == "Pending Payment").Count();
            if (pendingBooking > 0)
            {
                ModelState.AddModelError("", "You cannot book another flight because there is pending payment of another booking");
                return View(flightBooking);
            }
            if (ModelState.IsValid)
            {
                flightBooking.DateOfBooking = DateTime.Now;
                flightBooking.TotalCost = flightBooking.CalcCost();
                flightBooking.Status = "Pending Payment";
                flightBooking.Id= id;
                db.bookings.Add(flightBooking);
                db.SaveChanges();
                return RedirectToAction("PassengeHome","Home");
            }

            ViewBag.FlightId = new SelectList(db.flights, "FlightId", "FlightId", flightBooking.FlightId);
            return View(flightBooking);
        }

        // GET: FlightBookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlightBooking flightBooking = db.bookings.Find(id);
            if (flightBooking == null)
            {
                return HttpNotFound();
            }
            ViewBag.FlightId = new SelectList(db.flights, "FlightId", "FlightId", flightBooking.FlightId);
            return View(flightBooking);
        }

        // POST: FlightBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,FlightClass,TripType,NumberOfPassenger,FlightId,DateOfBooking,Id")] FlightBooking flightBooking)
        {
            if (ModelState.IsValid)
            {
                flightBooking.TotalCost = flightBooking.CalcCost();
                flightBooking.Status = "Pending Payment";
                db.Entry(flightBooking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PassengeHome", "Home");
            }
            ViewBag.FlightId = new SelectList(db.flights, "FlightId", "FlightId", flightBooking.FlightId);
            return View(flightBooking);
        }

        // GET: FlightBookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlightBooking flightBooking = db.bookings.Find(id);
            if (flightBooking == null)
            {
                return HttpNotFound();
            }
            return View(flightBooking);
        }

        // POST: FlightBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FlightBooking flightBooking = db.bookings.Find(id);
            db.bookings.Remove(flightBooking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
