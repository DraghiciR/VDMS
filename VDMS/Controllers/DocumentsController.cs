﻿using System;
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
using System.Globalization;
using System.Data.SqlClient;
using System.Web.Helpers;

namespace VDMS.Controllers
{
    public class DocumentsController : Controller
    {
        private VDMSModel db = new VDMSModel();
        private IList<ApplicationUser> users = new ApplicationDbContext().Users.ToList();
        List<Document> filteredDocuments = null;

        // GET: Documents
        [Authorize(Roles = "Viewer,User,Admin,Helpdesk,MBB Developer")]
        public ActionResult Index()
        {
            var documents = db.Documents.Include(d => d.Branch).Include(d => d.DocumentType);
            foreach (var doc in documents)
            {
                doc.UserName = GetUserName(doc.UserID);
            }
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
            document.UserID = GetUserName(document.UserID);

            if (document == null)
            {
                return HttpNotFound();
            }
            OperationLogger.LogDocumentEvent(User.Identity.GetUserId(), document.DocID, OperationLogger.GetEnumDescription(OperationType.View));
            return View(document);
        }

        // GET: Documents/Create
        public ActionResult Create()
        {
            if (Request.IsAuthenticated && (User.IsInRole("Viewer") || User.IsInRole("HelpDesk")))
            {
                return Redirect("~/Error/Denied");
            }

            ViewBag.BranchID = new SelectList(db.Branches.Where(b => !b.Disabled), "BranchID", "Name");
            ViewBag.DocTypeID = new SelectList(db.DocumentTypes.Where(d => !d.Disabled), "DocTypeID", "Name");

            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin,MBB Developer")]
        public ActionResult Create([Bind(Include = "DocTypeID,BranchID,UserID,Inbound,Recipient,Description,CreationDate")] Document document)
        {
            ComputeSerialNumber(document);
            if (ModelState.IsValid)
            {
                document.UserID = User.Identity.GetUserId();
                db.Documents.Add(document);
                db.SaveChanges();
                OperationLogger.LogDocumentEvent(document.UserID, document.DocID, OperationLogger.GetEnumDescription(OperationType.Create));
                return RedirectToAction("Index");
            }

            ViewBag.BranchID = new SelectList(db.Branches.Where(b => !b.Disabled), "BranchID", "Name", document.BranchID);
            ViewBag.DocTypeID = new SelectList(db.DocumentTypes.Where(d => !d.Disabled), "DocTypeID", "Name", document.DocTypeID);

            return View(document);
        }

        // GET: Documents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Request.IsAuthenticated && (User.IsInRole("Viewer") || User.IsInRole("HelpDesk") || (User.IsInRole("User"))))
            {
                return Redirect("~/Error/Denied");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            document.UserID = GetUserName(document.UserID);
            

            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchID = new SelectList(db.Branches.Where(b => !b.Disabled), "BranchID", "Name", document.BranchID);
            ViewBag.DocTypeID = new SelectList(db.DocumentTypes.Where(d => !d.Disabled), "DocTypeID", "Name", document.DocTypeID);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,MBB Developer")]
        public ActionResult Edit([Bind(Include = "DocID,DocSerial,DocTypeID,BranchID,UserID,Inbound,Recipient,Description,CreationDate")] Document document)
        {
            if (ModelState.IsValid)
            {
                document.UserID = User.Identity.GetUserId();
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                OperationLogger.LogDocumentEvent(User.Identity.GetUserId(), document.DocID, OperationLogger.GetEnumDescription(OperationType.Edit));
                return RedirectToAction("Index", "Documents");
            }
            ViewBag.BranchID = new SelectList(db.Branches.Where(b => !b.Disabled), "BranchID", "Name", document.BranchID);
            ViewBag.DocTypeID = new SelectList(db.DocumentTypes.Where(d => !d.Disabled), "DocTypeID", "Name", document.DocTypeID);
            return View(document);
        }

        // GET: Documents/Delete/5
        [Authorize(Roles = "Admin,MBB Developer")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            document.UserID = GetUserName(document.UserID);

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

        //GET
        [Authorize(Roles = "Viewer,User,Admin,Helpdesk,MBB Developer")]
        public ActionResult Reports()
        {
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "Name");
            ViewBag.DocTypeID = new SelectList(db.DocumentTypes, "DocTypeID", "Name");
            ViewBag.UserID = new SelectList(users, "Id", "Email");

            var documents = db.Documents.Include(d => d.Branch).Include(d => d.DocumentType);
            foreach (var doc in documents)
            {
                doc.UserName = GetUserName(doc.UserID);
            }
            return View(documents.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reports(DateTime? startDate, DateTime? endDate, int? docTypeID, int? branchID, string userID, string inbound, string recipient, string submit)
        {
            ViewBag.BranchID = new SelectList(db.Branches.Where(b => !b.Disabled), "BranchID", "Name");
            ViewBag.DocTypeID = new SelectList(db.DocumentTypes.Where(d => !d.Disabled), "DocTypeID", "Name");
            ViewBag.UserID = new SelectList(users, "Id", "Email");

            filteredDocuments = FilterDocuments(startDate, endDate, docTypeID, branchID, userID, inbound, recipient);

            if (submit == "Export")
            {
                string exportContent = GetExcel();
                return File(new System.Text.UTF8Encoding().GetBytes(exportContent), "application/xls", "DocumentsReport.xls");
            }
            else
            {
                return View(filteredDocuments);
            }
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
            var typeSerial = (from docTypes in db.DocumentTypes
                              where docTypes.DocTypeID == document.DocTypeID
                              select docTypes.Serial).FirstOrDefault();

            document.DocSerial = ViewBag.DocSerial = string.Concat(typeSerial, String.Format("{0:D5}",
                                                                                                db.Documents.Count(d => d.DocTypeID == document.DocTypeID && d.CreationDate >= DateTime.Today) + 1),
                                                                                                DateTime.Today.ToString("ddMMyyy")) ?? string.Empty;
        }

        private string GetUserName(string userID)
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = UserManager.FindById(userID);
            return user.UserName;
        }

        private string GetExcel()
        {
            var grid = new WebGrid(source: filteredDocuments, canPage: false, canSort: false);
            string gridData = grid.GetHtml(columns: grid.Columns(
                                                    grid.Column("DocSerial", "DocSerial"),
                                                    grid.Column("Branch.Name", "Branch Name"),
                                                    grid.Column("DocumentType.Name", "Type"),
                                                    grid.Column("UserName", "Created by"),
                                                    grid.Column("Inbound", "Inbound"),
                                                    grid.Column("Recipient", "Recipient"),
                                                    grid.Column("Description", "Description"),
                                                    grid.Column("CreationDate", "Creation Date"))).ToString();

            return gridData;
        }

        private List<Document> FilterDocuments(DateTime? startDate, DateTime? endDate, int? docTypeID, int? branchID, string userID, string inbound, string recipient)
        {
            filteredDocuments = new List<Document>();

            string whereClause = string.Empty;
            whereClause += string.Format(" creationdate >= '{0}'", startDate ?? DateTime.MinValue);
            whereClause += string.Format(" and creationdate <= '{0}'", endDate ?? DateTime.Now);
            whereClause += string.Format((docTypeID.HasValue ? " and doctypeid = {0}" : string.Empty), docTypeID);
            whereClause += string.Format((branchID.HasValue ? " and branchid = {0}" : string.Empty), branchID);
            whereClause += string.Format((!string.IsNullOrEmpty(userID) ? " and userid = '{0}'" : " and userid is not null"), userID);
            whereClause += string.Format((!string.IsNullOrEmpty(recipient) ? " and lower(recipient) LIKE lower('%{0}%')" : " and recipient is not null"), recipient);
            if (inbound != "All" && inbound != null)
            {
                whereClause += string.Format(" and [inbound] = '{0}'", (inbound == "Inbound" ? true : false));
            }

            using (SqlConnection sqlConn = new SqlConnection(ServerSettings.SqlConnectionString))
            {
                sqlConn.Open();

                using (SqlCommand sqlComm = sqlConn.CreateCommand())
                {
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.CommandText = string.Format("SELECT DocID,DocSerial,DocTypeID,BranchID,UserID,Inbound,Recipient,[Description],CreationDate FROM dbo.Documents WHERE {0}", whereClause);

                    using (SqlDataReader reader = sqlComm.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Document document = new Document()
                                {
                                    DocID = (int)reader["DocID"],
                                    DocSerial = reader["DocSerial"].ToString(),
                                    DocTypeID = (int)reader["DocTypeID"],
                                    BranchID = (int)reader["BranchID"],
                                    UserID = reader["UserID"].ToString(),
                                    Inbound = (bool)reader["Inbound"],
                                    Recipient = reader["Recipient"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    CreationDate = (DateTime)reader["CreationDate"]
                                };

                                document.Branch = db.Branches.First(b => b.BranchID == document.BranchID);
                                document.DocumentType = db.DocumentTypes.First(b => b.DocTypeID == document.DocTypeID);
                                document.UserName = GetUserName(document.UserID);
                                filteredDocuments.Add(document);
                            }
                        }
                    }
                }
            }
            return filteredDocuments;
        }
    }
}
