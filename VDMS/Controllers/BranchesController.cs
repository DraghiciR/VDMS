using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VDMS.Models;

namespace VDMS.Controllers
{
    [Authorize(Roles = "Admin, MBB Developer")]
    public class BranchesController : Controller
    {
        private VDMSModel db = new VDMSModel();

        // GET: Branches
        public ActionResult Index()
        {
            return View(db.Branches.ToList());
        }

        // GET: Branches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // GET: Branches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BranchID,Name,Address")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                if(db.Branches.Any(b => b.Name == branch.Name))
                {
                    ModelState.AddModelError("", "The name is already taken.");
                    return View();
                }

                db.Branches.Add(branch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(branch);
        }

        // GET: Branches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BranchID,Name,Address,Disabled")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                if (db.Branches.Any(b => b.Name == branch.Name && b.BranchID != branch.BranchID))
                {
                    ModelState.AddModelError("", "The name is already taken.");
                    return View();
                }

                db.Entry(branch).State = EntityState.Modified;

                if (branch.Disabled == true)
                    branch.DisabledDate = DateTime.Now;
                else
                    branch.DisabledDate = null;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(branch);
        }

        // GET: Branches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Branch branch = db.Branches.Find(id);
            
            if (branch == null)
            {
                return HttpNotFound();
            }

            branch.Disabled = !branch.Disabled;
            if (!branch.Disabled)
                branch.DisabledDate = null;
            else
                branch.DisabledDate = DateTime.Now;

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

        public VDMSModel GetModel()
        {
            return this.db;
        }
    }
}
