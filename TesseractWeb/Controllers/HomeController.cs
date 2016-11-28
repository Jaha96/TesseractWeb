using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesseract;
using TesseractWeb.Models;
using TesseractWeb.DB;
using System.Web.Security;
using Novacode;

namespace TesseractWeb.Controllers
{
    public class HomeController : Controller
    {
        Connect cn = new Connect();
        FileModel FM = new FileModel();
        UserModel UM = new UserModel();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserModel UM)
        {
            string ret = cn.UserConnect(UM.Email, UM.Password);
            if (ret == "Error") {
                ViewBag.ErrorMessage = "Нууц үг эсвэл нэвтрэх нэр буруу байна!";
                return View();
            }
            FormsAuthentication.SetAuthCookie(UM.Email, UM.RememberMe);
            HttpContext.Items["UserId"] = ret;
            Session["UserId"] = ret;
            return RedirectToAction("FileHistory", "User");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserModel UM) // Calling on http post (on Submit)
        {
            string ret= cn.UserAdd(UM);
            if (ret == "1") {
                ViewBag.Message = "Хэрэглэгч амжилттай бүртгэгдлээ";
            }
            else {
                ViewBag.ErrorMessage = ret;
            }
            
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
            string format = Request["format"];
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
                FM.inputFile = postedFile.FileName;
                if (Request.IsAuthenticated) { postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName)); }
                ViewBag.Message = "Файл илрүүлэлт амжилттай боллоо!";
            }
            if (Request.IsAuthenticated) { 
                switch (format) {
                    case "1": Write2Word(postedFile, ViewBag.Text); break;
                    case "2": Write2Word(postedFile, ViewBag.Text); break;
                    case "3": createText(postedFile, ViewBag.Text); break;
                }
            }
            
            langList();
            if (Request.IsAuthenticated)
            {
                FM.date = DateTime.Now;
                FM.userId = cn.getUserIdByEmail(HttpContext.User.Identity.Name);
                cn.FileHistoryAdd(FM);
            }
            return View(true);
        }

        public void Write2Word(HttpPostedFileBase postedFile,string text)
        {
            string fileName = postedFile.FileName;
            fileName = fileName.Split('.')[0];

            string Docpath = Server.MapPath("~/UserOutputs/");
            if (!Directory.Exists(Docpath))
            {
                Directory.CreateDirectory(Docpath);
            }
            var doc = DocX.Create(Docpath+fileName+".docx");
            FM.outputFile = fileName + ".docx";
            doc.InsertParagraph(text);
            doc.Save();
        }
        /*
        public void text2Pdf()
        {
            MemoryStream workStream = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();
            document.Add(new Paragraph("Hello World"));
            document.Add(new Paragraph(DateTime.Now.ToString()));
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment; filename= " + Server.HtmlEncode("abc.pdf"));
            Response.ContentType = "APPLICATION/pdf";
            Response.BinaryWrite(byteInfo);
            //return new FileStreamResult(workStream, "application/pdf");
        }
        */
        public void createText(HttpPostedFileBase postedFile,string text)
        {
            string fileName = postedFile.FileName;
            fileName = fileName.Split('.')[0];
            string Docpath = Server.MapPath("~/UserOutputs/");
            if (!Directory.Exists(Docpath))
            {
                Directory.CreateDirectory(Docpath);
            }
            System.IO.File.WriteAllText(Docpath + fileName+".txt", text);
            FM.outputFile = fileName + ".txt";
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