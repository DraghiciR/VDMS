using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VDMS.Controllers;
using VDMS.Models;
using VDMS.Tests.Mocks;
using System.Web.Mvc;

namespace VDMS.Tests.Controllers
{

    [TestClass]
    public class BranchesControllerTest
    {
        private BranchesController _controller;
        Branch branch = null;
        object result = null;
        string userID = null;
        string errorMessage = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            _controller = new BranchesController { ControllerContext = MockWebContext.BasicContext() };
            userID = "fc386649-3d68-4cfa-a602-356b938173c1";
        }

        #region Authenticated Users

        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                _controller.ControllerContext = MockWebContext.AuthenticatedContext("andrei.dana@vodafone.com", new[] { "Viewer", "User" }, true);
                result = _controller.Index();
            }
            catch (Exception ex)
            {
                result = null;
                errorMessage = ex.Message;
            }
            Assert.IsNotNull(result, errorMessage);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(((ViewResult)result).ViewName, "Index");
        }

        #endregion
    }
}
