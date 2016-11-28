using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TesseractWeb.DB;

namespace TesseractWeb.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FileHistory()
        {
            Connect cn = new Connect();
            return View(cn.CompanyDatatable(Convert.ToInt32(Session["UserId"])).Rows);
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Items.Remove("UserId");
            return RedirectToAction("Index", "Home");
        }
    }
}