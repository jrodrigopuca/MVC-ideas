using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;

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

        // Controlador del GET del UsarStorage
        [HttpGet]
        public ActionResult UsarStorage()
        {
            ViewBag.Message = "Sube tu imagen a Azure Storage";
            ViewBag.Nombre = "Khabib Nurmagomedov";
            ViewBag.Profesion = "Artista Marcial";
            ViewBag.Result = "";
            ViewBag.Url = "";
            return View();
        }

        // Subir la imagen y devolver el link
        public async Task<string> subirImagen(HttpPostedFileBase imagen)
        {
            
            string linkImagen ="";
            try
            {
                // Conectar con Azure
                CloudStorageAccount cuenta = Models.Conexion.TraerCadena();
                CloudBlobClient blobClient = cuenta.CreateCloudBlobClient();
                CloudBlobContainer contenedor = blobClient.GetContainerReference("images");

                // Si no hay contenedor lo va a crear
                if (await contenedor.CreateIfNotExistsAsync())
                {
                    await contenedor.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                // Subir imagen
                string nombreImagen = Guid.NewGuid().ToString() + "." + Path.GetExtension(imagen.FileName);
                CloudBlockBlob bloque = contenedor.GetBlockBlobReference(nombreImagen);
                bloque.Properties.ContentType = imagen.ContentType;
                await bloque.UploadFromStreamAsync(imagen.InputStream);
              
                // Obtener Link de Imagen
                linkImagen = bloque.Uri.ToString();
            }
            catch (Exception ex)
            {
                //TODO: Mostrar error
            }
         
            return linkImagen;
        }

        // Controlador del POST del UsarStorage
        [HttpPost]
        public async Task<ActionResult> UsarStorage(HttpPostedFileBase archivo)
        {
            // subir la imagen del POST
            var linkImagen = await subirImagen(archivo);

            // Si no hay link de la imagen entonces hubo error
            ViewBag.Message = (linkImagen != "")? "Listo en Azure" : "Error";
            ViewBag.Result = linkImagen;
            ViewBag.Url = linkImagen;
            ViewBag.Nombre = "Khabib Nurmagomedov";
            ViewBag.Profesion = "Artista Marcial";
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
            // Este control sirve para un Hosting, "podría servir para Azure" pero es mejor ir con AzureStorage
            // Guardar imagen: Si no está el directorio Imagenes lo va a crear
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