using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class StdClassesController : Controller
    {
        private Context db = new Context();
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: StdClasses
        public ActionResult Index()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());


            var classes = db.Classes.Where(s => s.UserId == currentUser.Id);
          /*  var classes = from m in db.Classes
                        where m.UserId.Equals(currentUser.Id)
                        select m;*/


            return View(classes.ToList());
        }

        // GET: StdClasses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StdClass stdClass = db.Classes.Find(id);
            if (stdClass == null)
            {
                return HttpNotFound();
            }
            return View(stdClass);
        }

        // GET: StdClasses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StdClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClassId,ClassName,UserId")] StdClass stdClass)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            stdClass.UserId = currentUser.Id;
            if (ModelState.IsValid)
            {
                db.Classes.Add(stdClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(stdClass);
        }

        // GET: StdClasses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StdClass stdClass = db.Classes.Find(id);
            if (stdClass == null)
            {
                return HttpNotFound();
            }
            return View(stdClass);
        }

        // POST: StdClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClassId,ClassName,UserId")] StdClass stdClass)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            stdClass.UserId = currentUser.Id;
            if (ModelState.IsValid)
            {
                db.Entry(stdClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stdClass);
        }

        // GET: StdClasses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StdClass stdClass = db.Classes.Find(id);
            if (stdClass == null)
            {
                return HttpNotFound();
            }
            return View(stdClass);
        }

        // POST: StdClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StdClass stdClass = db.Classes.Find(id);
            db.Classes.Remove(stdClass);
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
