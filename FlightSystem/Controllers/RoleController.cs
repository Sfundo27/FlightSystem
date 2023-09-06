using FlightSystem.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlightSystem.Controllers
{
    public class RoleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Role
        public ActionResult Index()
        {
            var roles = db.Roles.ToList();
            return View(roles);
        }
        public ActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddRole(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }
        public ActionResult Remove(string Id)
        {
            var role = (from r in db.Roles
                        where r.Id == Id
                        select r).Single();
            return View(role);
        }
        [HttpPost, ActionName("Remove")]
        public ActionResult RemoveConfirmed(string Id)
        {
            var role = db.Roles.Find(Id);
            db.Roles.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}