using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;

namespace Prototipos.Models
{
    public class Conexion
    {
        // Trae los datos del webconfig global y los devuelve como una cadena
        static string cuenta = CloudConfigurationManager.GetSetting("StorageAccountName");
        static string clave= CloudConfigurationManager.GetSetting("StorageAccountKey");
        public static CloudStorageAccount TraerCadena()
        {
            string cadena= string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", cuenta, clave);
            return CloudStorageAccount.Parse(cadena);
        }
    }
}