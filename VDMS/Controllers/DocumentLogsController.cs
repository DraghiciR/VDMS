using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VDMS.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace VDMS.Controllers
{
    public class DocumentLogsController : Controller
    {
        private VDMSModel db = new VDMSModel();

        // GET: DocumentLogs
        public async Task<ActionResult> Index()
        {
            var documentlogs = db.DocumentLogs.OrderByDescending(d => d.LogDate);
            foreach (var doclog in documentlogs)
            {
                doclog.UserName = GetUserName(doclog.UserID);
            }
            return View(await documentlogs.ToListAsync());
        }
        private string GetUserName(string userID)
        {
            //easier to use the statements below than to mock ApplicationUserManager in Unit Tests
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = UserManager.FindById(userID);
            if (user == null)
            {
                return string.Empty;
            }
            else
            {
                return user.UserName;
            }

        }
    }
}
