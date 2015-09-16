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
    [Authorize(Roles = "MBB Developer")]
    public class UserLogsController : Controller
    {
        private VDMSModel db = new VDMSModel();

        // GET: UserLogs
        public async Task<ActionResult> Index()
        {
            var userLogs = db.UserLogs.OrderByDescending(d => d.LogDate);
            foreach (var ulog in userLogs)
            {
                ulog.UserName = GetUserName(ulog.UserID);
                ulog.AffectedUserName = GetUserName(ulog.AffectedUserID);
            }
            return View(await userLogs.ToListAsync());
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
