using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TesseractWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title="My Bootstrap Site";
            return View();
        }

        public void langList()
        {
            DirectoryInfo dinfo = new DirectoryInfo(Server.MapPath(@"~/tessdata"));
            FileInfo[] Files = dinfo.GetFiles("*.traineddata");
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (FileInfo file in Files)
            {
                string[] val = file.Name.ToString().Split('.');
                items.Add(new SelectListItem { Text = file.Name, Value = val[0] });
            }
            ViewBag.LangList = items.ToArray();
        }
    }
}