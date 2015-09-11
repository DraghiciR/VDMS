using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VDMS.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult Index()
        {
            return View("Error");
        }
        public ViewResult Denied()
        {
            ViewBag.errorMessage = "Insufficient Permissions";
            Response.StatusCode = 401;  //Status: denied
            return View("Denied");
        }
    }
}