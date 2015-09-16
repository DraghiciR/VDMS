﻿using System;
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
    [Authorize(Roles = "HelpDesk, MBB Developer")]
    public class DocumentTypesController : Controller
    {
        private VDMSModel db = new VDMSModel();

        // GET: DocumentTypes
        public ActionResult Index()
        {
            return View(db.DocumentTypes.ToList());
        }

        // GET: DocumentTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentType documentType = db.DocumentTypes.Find(id);
            if (documentType == null)
            {
                return HttpNotFound();
            }
            return View(documentType);
        }

        // GET: DocumentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DocumentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocTypeID,Name,Serial")] DocumentType documentType)
        {
            if (ModelState.IsValid)
            {
                if (db.DocumentTypes.Any(d => d.Name == documentType.Name))
                {
                    ModelState.AddModelError("", "The name is already taken.");
                    return View();
                }

                if (db.DocumentTypes.Any(d => d.Serial == documentType.Serial))
                {
                    ModelState.AddModelError("", "The serial is already assigned.");
                    return View();
                }

                db.DocumentTypes.Add(documentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(documentType);
        }

        // GET: DocumentTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentType documentType = db.DocumentTypes.Find(id);
            if (documentType == null)
            {
                return HttpNotFound();
            }
            return View(documentType);
        }

        // POST: DocumentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DocTypeID,Name,Serial,Disabled")] DocumentType documentType)
        {
            if (ModelState.IsValid)
            {
                if (db.DocumentTypes.Any(d => d.Name == documentType.Name && d.DocTypeID != documentType.DocTypeID))
                {
                    ModelState.AddModelError("", "The name is already taken.");
                    return View();
                }

                if (db.DocumentTypes.Any(d => d.Serial == documentType.Serial && d.DocTypeID != documentType.DocTypeID))
                {
                    ModelState.AddModelError("", "The serial is already assigned.");
                    return View();
                }

                db.Entry(documentType).State = EntityState.Modified;

                if (documentType.Disabled == true)
                    documentType.DisabledDate = DateTime.Now;
                else
                    documentType.DisabledDate = null;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(documentType);
        }

        // GET: DocumentTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentType documentType = db.DocumentTypes.Find(id);
            if (documentType == null)
            {
                return HttpNotFound();
            }
            return View(documentType);
        }

        // POST: DocumentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentType documentType = db.DocumentTypes.Find(id);
            documentType.Disabled = !documentType.Disabled;
            if (!documentType.Disabled)
                documentType.DisabledDate = null;
            else
                documentType.DisabledDate = DateTime.Now;

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
