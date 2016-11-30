using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TesseractWeb.DB;
using TesseractWeb.Models;

namespace TesseractWeb.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Download()
        {
            string type = Request["type"];
            string fileName = Request["name"];
            byte[] fileBytes=null;
            try
            {
                if (type == "image") { fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/UserImages/" + fileName)); }
                else { fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/UserOutputs/" + fileName)); }
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
                Connect cn = new Connect();
                return View("FileHistory", cn.CompanyDatatable(cn.getUserIdByEmail(HttpContext.User.Identity.Name)).Rows);
            }
        }
        
        public ActionResult Delete()
        {
            int historyId = Convert.ToInt32(Request["HistoryId"]);
            Connect cn = new Connect();
            string ret= cn.FileHistoryDelete(historyId);
            if (ret == "1")
            {
                ViewBag.Message = "Амжилттай устгагдлаа";
            }
            else {
                ViewBag.ErrorMessage = "Устгахад алдаа гарлаа";
            }
            return View("FileHistory", cn.CompanyDatatable(cn.getUserIdByEmail(HttpContext.User.Identity.Name)).Rows);
        }
        public ActionResult FileHistory()
        {
            Connect cn = new Connect();
            DataTable dt = cn.CompanyDatatable(cn.getUserIdByEmail(HttpContext.User.Identity.Name));

            return View(dt.Rows);
        }
        [Authorize]
        public ActionResult Profile()
        {
            Connect cn = new Connect();
            DataTable dt= cn.UserDatatable(cn.getUserIdByEmail(HttpContext.User.Identity.Name));
            UserModel um = new UserModel();
            um.UserId = Convert.ToInt32(dt.Rows[0]["UserId"]);
            um.UserName = dt.Rows[0]["UserName"].ToString();
            um.Email = dt.Rows[0]["Email"].ToString();
            return View(um);
        }
        [HttpPost]
        public ActionResult Profile(UserModel um)
        {
            Connect cn = new Connect();

            string ret = cn.UserUpdate(um);
            if (ret == "1") { ViewBag.Message = "Амжилттай засагдлаа"; }
            else { ViewBag.ErrorMessage = ret; }
            return View();
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