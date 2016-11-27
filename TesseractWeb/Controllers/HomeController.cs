using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesseract;

namespace TesseractWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Үнэгүй-OCR";
            langList();
            return View(false);
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            string lang = Request["language"];
            if (postedFile != null && postedFile.ContentLength > 0)
            {
                // for now just fail hard if there's any error however in a propper app I would expect a full demo.

                using (var engine = new TesseractEngine(Server.MapPath(@"~/tessdata"), lang, EngineMode.Default))
                {
                    // have to load Pix via a bitmap since Pix doesn't support loading a stream.
                    using (var image = new System.Drawing.Bitmap(postedFile.InputStream))
                    {
                        using (var pix = PixConverter.ToPix(image))
                        {
                            using (var page = engine.Process(pix))
                            {
                                ViewBag.Text = page.GetText();
                            }
                        }
                    }
                }
                string path = Server.MapPath("~/UserImages/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                ViewBag.Message = "Файл илрүүлэлт амжилттай боллоо!";
            }
            langList();
            return View(true);
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