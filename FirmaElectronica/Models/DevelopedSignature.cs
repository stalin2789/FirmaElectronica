using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaElectronica.Models
{
    public class DevelopedSignature
    {
        private string XmlPath;
        private string PathSignature;
        private string PassSignature;
        private string PathOut;
        private string NameFileOut;

        public DevelopedSignature(string xmlPath, string pathSignature, string passSignature, string pathOut, string nameFileOut)
        {
            XmlPath = xmlPath;
            PathSignature = pathSignature;
            PassSignature = passSignature;
            PathOut = pathOut;
            NameFileOut = nameFileOut;
        }

        public void Firmar()
        {
            Console.WriteLine("Ruta del XML de entrada: " + XmlPath);
            Console.WriteLine("Ruta Certificado: " + PathSignature);
            Console.WriteLine("Clave del Certificado: " + PassSignature);
            Console.WriteLine("Ruta de salida del XML: " + PathOut);
            Console.WriteLine("Nombre del archivo salido: " + NameFileOut);
            try
            {
                XAdESBESSignature.firmar(XmlPath, PathSignature, PassSignature, PathOut, NameFileOut);
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                Console.ReadKey();
            }
        }
    }
}
