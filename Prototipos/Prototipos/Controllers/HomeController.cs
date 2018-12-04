using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Prototipos.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult GuardarImagen()
        {
            ViewBag.Message = "";
            return View();
            
        }

        [HttpPost]
        public ActionResult GuardarImagen(HttpPostedFileBase archivo)
        {
            if (archivo != null)
            {
                string path = Server.MapPath("~/Imagenes/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string url = "../Imagenes/"+ Path.GetFileName(archivo.FileName);
                archivo.SaveAs(path + Path.GetFileName(archivo.FileName));
                ViewBag.Url = url;
                ViewBag.Message = "Archivo subido";
            }
            else
            {
                ViewBag.Message = "Un error ha ocurrido";
            }
            return View();
        }
    }
}