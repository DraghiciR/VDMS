using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VDMS.Controllers;
using VDMS.Models;
using VDMS.Tests.Mocks;
using System.Web.Mvc;
using System.Linq;

namespace VDMS.Tests.Controllers
{

    [TestClass]
    public class BranchesControllerTest
    {
        private BranchesController _controller;
        object result = null;
        string errorMessage = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            _controller = new BranchesController { ControllerContext = MockWebContext.BasicContext() };
        }

        #region Index

        [TestMethod]
        public void Branch_Index_ReturnsView()
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
        public void Branch_Details_ReturnExistingElement()
        {
            try
            {
                result = _controller.Details(_controller.GetModel().Branches.First().BranchID);
            }
            catch(Exception ex)
            {
                result = null; ;
                errorMessage = ex.Message;
            }

            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Branch_Details_NullElementAsParameter()
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
        public void Branch_Details_NonExistingElementAsParameter()
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
                result = _controller.Create(CreateCustomBranch("Piata Presei Libere nr. 123","Central HQ"));
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

        #region Edit

        [TestMethod]
        public void Branch_Edit_NonExistingElementAsParameter()
        {
            try
            {
                result = _controller.Edit(-1);
            }
            catch (Exception ex)
            {
                result = null; ;
                errorMessage = ex.Message;
            }

            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Branch_Edit_ReturnsExistingElement()
        {
            try
            {
                result = _controller.Edit(_controller.GetModel().Branches.First().BranchID);
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
        public void Branch_Edit_ExistingItem()
        {
            try
            {
                result = _controller.Edit(CreateCustomBranchForEditing(5, "Pipera2", "d", false));
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
        public void Branch_Edit_ToExistingName()
        {
            try
            {
                result = _controller.Edit(CreateCustomBranchForEditing(5, "Pipera", "d", false));
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
        public void Branch_Edit_NonExistingItem()
        {
            try
            {
                result = _controller.Edit(CreateCustomBranchForEditing(7, "Pipera", "d", false));
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
        #endregion

        #region Disable/Enable

        [TestMethod]
        public void Branch_Delete_ReturnView()
        {
            try
            {
                result = _controller.Delete(4);
            }
            catch(Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }

            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Branch_Delete_DeleteExistingItem()
        {
            try
            {
                result = _controller.DeleteConfirmed(4);
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
        public void Branch_Delete_DeleteNonExistingItem()
        {
            try
            {
                result = _controller.DeleteConfirmed(-1);
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }

            Assert.IsNotNull(errorMessage);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        #endregion

        #region Helpers
        private Branch CreateCustomBranch(string adress, string name)
        {
            Branch branch = new Branch();

            branch.Address = adress;
            branch.Name = name;

            return branch;
        }

        private Branch CreateCustomBranchForEditing(int branchID, string name, string adress, bool disabled)
        {
            Branch branch = new Branch();

            branch.BranchID = branchID;
            branch.Name = name;
            branch.Address = adress;
            branch.Disabled = disabled;

            return branch;
        }

        #endregion
    }
}
