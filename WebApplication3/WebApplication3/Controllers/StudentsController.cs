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
    public class StudentsController : Controller
    {
        private Context db = new Context();
        ApplicationDbContext context = new ApplicationDbContext();
       

        // GET: Students
        public ActionResult Index(string Student_Name, int? searchid, string Class_Name)
        {
           
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

            var ClassList = new List<string>();
            var ClassQry = from d in db.Classes
                           where d.UserId.Equals(currentUser.Id)
                           orderby d.ClassName
                           select d.ClassName;

            ClassList.AddRange(ClassQry.Distinct());
            ViewBag.Class_Name = new SelectList(ClassList);

            var students = db.Students.Include(s => s.Class)
                                      .Where(s => s.UserId== currentUser.Id);

            if (!String.IsNullOrEmpty(Student_Name))
            {
                students = students.Where(s => s.Name.Contains(Student_Name));
            }

            if (searchid != null)
            {
                students = students.Where(s => s.ID == searchid);
            }
            if (!string.IsNullOrEmpty(Class_Name))
            {
                students = students.Where(x => x.Class.ClassName == Class_Name);
            }

            return View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Students students = db.Students.Find(id);
            Students students = (from u in db.Students where u.ID == id select u).Include(u => u.Class).First();
            
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            ViewBag.ClassName = new SelectList(db.Classes.Where(s => s.UserId== currentUser.Id), "ClassId", "ClassName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Age,Major,Average,ClassName,UserId")] Students students)
        {
            
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            students.UserId = currentUser.Id;
            if (ModelState.IsValid)
            {
                db.Students.Add(students);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassName = new SelectList(db.Classes, "ClassId", "ClassName", students.ClassName);
            return View(students);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
             var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
             ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassName = new SelectList(db.Classes.Where(o => o.UserId == currentUser.Id), "ClassId", "ClassName", students.ClassName);
            return View(students);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Age,Major,Average,ClassName,UserId")] Students students)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            students.UserId = currentUser.Id;
            if (ModelState.IsValid)
            {
                db.Entry(students).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassName = new SelectList(db.Classes, "ClassId", "ClassName", students.ClassName);
            return View(students);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Students students = db.Students.Find(id);
            Students students = (from u in db.Students where u.ID == id select u).Include(u => u.Class).First();

            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Students students = db.Students.Find(id);
            db.Students.Remove(students);
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
