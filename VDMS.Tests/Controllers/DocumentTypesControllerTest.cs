using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VDMS.Controllers;
using VDMS.Tests.Mocks;
using System.Web.Mvc;
using System.Linq;
using VDMS.Models;

namespace VDMS.Tests.Controllers
{

    [TestClass]
    public class DocumentTypesControllerTest
    {
        private DocumentTypesController _controller;
        object result = null;
        string errorMessage = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            _controller = new DocumentTypesController { ControllerContext = MockWebContext.BasicContext() };
        }

        #region index

        [TestMethod]
        public void DocTypes_Index_ReturnsView()
        {
            try
            {
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

        #endregion

        #region Details

        [TestMethod]
        public void DocTypes_Details_ReturnExistingElement()
        {
            try
            {
                result = _controller.Details(_controller.GetModel().Branches.First().BranchID);
            }
            catch (Exception ex)
            {
                result = null; ;
                errorMessage = ex.Message;
            }

            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void DocTypes_Details_NullElementAsParameter()
        {
            try
            {
                result = _controller.Details(null);
            }
            catch (Exception ex)
            {
                result = null; ;
                errorMessage = ex.Message;
            }

            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
        }

        [TestMethod]
        public void DocTypes_Details_NonExistingElementAsParameter()
        {
            try
            {
                result = _controller.Details(-1);
            }
            catch (Exception ex)
            {
                result = null; ;
                errorMessage = ex.Message;
            }

            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        #endregion

        #region Create

        [TestMethod]
        public void Branch_Create_ReturnsView()
        {
            try
            {
                result = _controller.Create();
            }
            catch (Exception ex)
            {
                result = null; ;
                errorMessage = ex.Message;
            }

            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Branch_Create_NewBranch()
        {
            try
            {
                result = _controller.Create(CreateCustomBranch("Unite Test", "UT1"));
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
        public void Branch_Create_Existing()
        {
            try
            {
                result = _controller.Create(CreateCustomBranch("Avrig 3-5, Bucharest", "Vodafone Avrig"));
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            ICollection<ModelState> errors = _controller.ModelState.Values;
            string creationError = errors.ToArray()[0].Errors[0].ErrorMessage;
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsNotNull(errors);
            Assert.AreEqual(creationError, "The name is already taken.");
        }

        [TestMethod]
        public void Branch_Create_NotAllParametersProvided()
        {
            try
            {
                result = _controller.Create(CreateCustomBranch(null, null));
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }

            Assert.IsNull(result);
            Assert.IsNotNull(errorMessage);
        }

        #endregion

        #region Helpers
        private DocumentType CreateCustomBranch(string name, string serial)
        {
            DocumentType docType = new DocumentType();

            docType.Serial = serial;
            docType.Name = name;

            return docType;
        }

        private DocumentType CreateCustomBranchForEditing(int docTypeID, string name, string serial, bool disabled)
        {
            DocumentType docType = new DocumentType();

            docType.DocTypeID = docTypeID;
            docType.Name = name;
            docType.Serial = serial;
            docType.Disabled = disabled;

            return docType;
        }

        #endregion

    }
}
