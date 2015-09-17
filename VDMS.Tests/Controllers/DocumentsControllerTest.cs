﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;
using VDMS.Controllers;
using VDMS.Tests.Mocks;
using VDMS.Models;

using Moq;
using System.Security.Principal;

namespace VDMS.Tests.Controllers
{

    [TestClass]
    public class DocumentsControllerTest
    {
        private DocumentsController _controller;
        Document document = null;
        object result = null;
        string errorMessage = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            _controller = new DocumentsController { ControllerContext = MockWebContext.BasicContext() };
            _controller.GetUserId = () => "21f99dff-792a-4824-bcf7-d8f741a30aca";
        }

        #region Authenticated User

        [TestMethod]
        public void Documents_Index_AuthenticatedUser_ReturnsView() //documents grid
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("ralucavianina.draghici@vodafone.com", new[] { "Viewer", "User" }, true);
                result = _controller.Index();
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [Ignore]  //not working because a webgrid is declared inside the method from the controller
        [TestMethod]
        public void Documents_Report_AuthenticatedUser_ExportsToExcel() //report generation
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("ralucavianina.draghici@vodafone.com", new[] { "Viewer", "User" }, true);
                result = _controller.Index(DateTime.MinValue, DateTime.MaxValue, null, null, _controller.GetUserId(), "All", string.Empty, "Export");
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Documents_Report_AuthenticatedUser_GeneratesPreview() //report preview
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("ralucavianina.draghici@vodafone.com", new[] { "Viewer", "User" }, true);
                result = _controller.Index(DateTime.MinValue, DateTime.MaxValue, null, null, _controller.GetUserId(), "All", string.Empty, "Preview");
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Documents_Create_AuthenticatedUser_UserInUserRole_ReturnsViewCreateDocument()
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("ralucavianina.draghici@vodafone.com", new[] { "User" }, true);
                result = _controller.Create();
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Documents_Create_AuthenticatedUser_UserInUserRole_DocumentCreated() //document inserted in DB
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("ralucavianina.draghici@vodafone.com", new[] { "User" }, true);
                result = _controller.Create(CreateCustomDocument());
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Documents_Edit_AuthenticatedUser_UserInAdminRole_ReturnsViewEditDocument()
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("ralucavianina.draghici@vodafone.com", new[] { "Admin" }, true);

                document = _controller.db.Documents.Where(doc => doc.Description.Contains("Unit test")).FirstOrDefault();
                if (document == null)
                {
                    Documents_Create_AuthenticatedUser_UserInUserRole_DocumentCreated();
                    document = _controller.db.Documents.Where(doc => doc.Description.Contains("Unit test")).First();
                }

                result = _controller.Edit(document.DocID);
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        //in case of automatic runs, the creation of the document should be run first
        [TestMethod]
        public void Documents_Edit_AuthenticatedUser_UserInUserRole_DocumentUpdated()
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("ralucavianina.draghici@vodafone.com", new[] { "Admin" }, true);
                document = _controller.db.Documents.Where(doc => doc.Description.Contains("Unit test")).FirstOrDefault();
                if (document == null)
                {
                    Documents_Create_AuthenticatedUser_UserInUserRole_DocumentCreated();
                    document = _controller.db.Documents.Where(doc => doc.Description.Contains("Unit test")).First();
                }
                result = _controller.Edit(EditDocument(document));
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }


        [TestMethod]
        public void Documents_Delete_AuthenticatedUser_UserInAdminRole_ReturnsViewDeleteDocument()
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("ralucavianina.draghici@vodafone.com", new[] { "Admin" }, true);

                document = _controller.db.Documents.Where(doc => doc.Description.Contains("Unit test")).FirstOrDefault();
                if (document == null)
                {
                    Documents_Create_AuthenticatedUser_UserInUserRole_DocumentCreated();
                    document = _controller.db.Documents.Where(doc => doc.Description.Contains("Unit test")).First();
                }

                result = _controller.Delete(document.DocID);
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        //in case of automatic runs, the creation and editing of the document should be run first
        [TestMethod]
        public void Documents_Delete_AuthenticatedUser_UserInAdminRole_DocumentDeleted()
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("ralucavianina.draghici@vodafone.com", new[] { "Admin" }, true);

                document = _controller.db.Documents.Where(doc => doc.Description.Contains("Unit test")).FirstOrDefault();
                if (document == null)
                {
                    Documents_Create_AuthenticatedUser_UserInUserRole_DocumentCreated();
                    document = _controller.db.Documents.Where(doc => doc.Description.Contains("Unit test")).First();
                }

                result = _controller.DeleteConfirmed(document.DocID);
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }


        //details of any document
        [TestMethod]
        public void Documents_Details_AuthenticatedUser_ReturnsViewDetailsDocument()
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("ralucavianina.draghici@vodafone.com", new[] { "Viewer" }, true);
                result = _controller.Details(_controller.db.Documents.First().DocID);
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        #endregion Authenticated User

        #region Insufficient Permissions for Viewer

        [TestMethod]
        public void Documents_Create_UserInViewerRole_ReturnsErrorDueToInsufficientPermissions()
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("ralucavianina.draghici@vodafone.com", new[] { "Viewer" }, true);
                result = _controller.Create();
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual(((RedirectResult)result).Url, "~/Error/Denied");
        }

        [TestMethod]
        public void Documents_Edit_UserInViewerRole_ReturnsErrorDueToInsufficientPermissions()
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("ralucavianina.draghici@vodafone.com", new[] { "Viewer" }, true);
                result = _controller.Edit(-1);
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual(((RedirectResult)result).Url, "~/Error/Denied");
        }

        [TestMethod]
        public void Documents_Delete_UserInViewerRole_ReturnsErrorDueToInsufficientPermissions()
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("ralucavianina.draghici@vodafone.com", new[] { "Viewer" }, true);
                result = _controller.Delete(-1);
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual(((RedirectResult)result).Url, "~/Error/Denied");
        }

        #endregion Insufficient Permissions

        #region Insufficient Permissions for User

        [TestMethod]
        public void Documents_Edit_UserInUserRole_ReturnsErrorDueToInsufficientPermissions()
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("ralucavianina.draghici@vodafone.com", new[] { "User" }, true);
                result = _controller.Edit(-1);
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual(((RedirectResult)result).Url, "~/Error/Denied");
        }

        [TestMethod]
        public void Documents_Delete_UserInUserRole_ReturnsErrorDueToInsufficientPermissions()
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("ralucavianina.draghici@vodafone.com", new[] { "User" }, true);
                result = _controller.Delete(-1);
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual(((RedirectResult)result).Url, "~/Error/Denied");
        }

        #endregion Insufficient Permissions

        #region Helpers

        private Document CreateCustomDocument()
        {
            document = new Document();
            document.BranchID = _controller.db.Branches.Where(b => !b.Disabled).FirstOrDefault().BranchID;
            document.CreationDate = DateTime.Now;
            document.Description = "Document created from Unit test ";
            document.DocTypeID = _controller.db.DocumentTypes.Where(b => !b.Disabled).FirstOrDefault().DocTypeID;

            string typeSerial = _controller.db.DocumentTypes.Where(b => b.DocTypeID == document.DocTypeID).FirstOrDefault().Serial;

            document.DocSerial = string.Concat(typeSerial, String.Format("{0:D5}",
                       _controller.db.Documents.Count(d => d.DocTypeID == document.DocTypeID && d.CreationDate >= DateTime.Today) + 1),
                       DateTime.Today.ToString("ddMMyyy")) ?? string.Empty;

            document.Inbound = true;
            document.Recipient = "Unit test";
            document.UserName = "ralucavianina.draghici@vodafone.com"; //new ApplicationDbContext().EditUserViewModels.FirstOrDefault().Email;
            return document;
        }

        private Document EditDocument(Document document)
        {
            document.CreationDate = DateTime.Now;
            document.Description = "Document updated from Unit test ";
            document.Inbound = false;
            document.Recipient = "Unit test updater";
            document.UserName = "maxim.tabacari@vodafone.com";
            return document;
        }

        #endregion Helpers
    }
}
