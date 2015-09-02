using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VDMS.Models;
using VDMS.Models.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
namespace VDMS.Controllers
{
    public class DocumentsController : Controller
    {
        private VDMSModel db = new VDMSModel();
        private static int counter = 0;
        private static string date = DateTime.Today.ToString("ddMMyyy");

        // GET: Documents
        [Authorize(Roles = "Viewer,User,Admin,Helpdesk,MBB Developer")]
        public ActionResult Index()
        {
            var documents = db.Documents.Include(d => d.Branch).Include(d => d.DocumentType);
            return View(documents.ToList());
        }

        // GET: Documents/Details/5
        [Authorize(Roles = "Viewer,User,Admin,Helpdesk,MBB Developer")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            OperationLogger.LogDocumentEvent(User.Identity.GetUserId(), document.DocID, OperationLogger.GetEnumDescription(OperationType.View));
            return View(document);
        }

        // GET: Documents/Create
        [Authorize(Roles = "User,Admin,MBB Developer")]
        public ActionResult Create()
        {
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "Name");
            ViewBag.DocTypeID = new SelectList(db.DocumentTypes, "DocTypeID", "Name");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocTypeID,BranchID,UserID,Inbound,Recipient,Description")] Document document)
        {
            ComputeSerialNumber(document);
            if (ModelState.IsValid)
            {
                db.Documents.Add(document);
                db.SaveChanges();
                OperationLogger.LogDocumentEvent(User.Identity.GetUserId(), document.DocID, OperationLogger.GetEnumDescription(OperationType.Create));
                return RedirectToAction("Index");
            }

            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "Name", document.BranchID);
            ViewBag.DocTypeID = new SelectList(db.DocumentTypes, "DocTypeID", "Name", document.DocTypeID);

            return View(document);
        }

        // GET: Documents/Edit/5
        [Authorize(Roles = "Admin,MBB Developer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "Name", document.BranchID);
            ViewBag.DocTypeID = new SelectList(db.DocumentTypes, "DocTypeID", "Name", document.DocTypeID);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DocID,DocSerial,DocTypeID,BranchID,UserID,Inbound,Recipient,Description,CreationDate")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                OperationLogger.LogDocumentEvent(User.Identity.GetUserId(), document.DocID, OperationLogger.GetEnumDescription(OperationType.Edit));
                return RedirectToAction("Index");
            }
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "Name", document.BranchID);
            ViewBag.DocTypeID = new SelectList(db.DocumentTypes, "DocTypeID", "Name", document.DocTypeID);
            return View(document);
        }

        // GET: Documents/Delete/5
        [Authorize(Roles = "MBB Developer")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            db.Documents.Remove(document);
            db.SaveChanges();
            OperationLogger.LogDocumentEvent(User.Identity.GetUserId(), document.DocID, OperationLogger.GetEnumDescription(OperationType.Delete));
            return RedirectToAction("Index");
        }

        // GET: Documents
        [Authorize(Roles = "Viewer,User,Admin,Helpdesk,MBB Developer")]
        public ActionResult Reports()
        {
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "Name");
            ViewBag.DocTypeID = new SelectList(db.DocumentTypes, "DocTypeID", "Name");
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ComputeSerialNumber(Document document)
        {
            string typeSerial = (from docTypes in db.DocumentTypes
                                 where docTypes.DocTypeID == document.DocTypeID
                                 select docTypes.Serial).FirstOrDefault();
            document.DocSerial = ViewBag.DocSerial = string.Concat(typeSerial, String.Format("{0:D5}", ++counter), date) ?? string.Empty;
        }
    }
}
