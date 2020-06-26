using es.mityc.firmaJava.libreria.xades;
using es.mityc.javasign.xml.refs;
using java.io;
using java.util.logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaElectronica.Models
{
    class XAdESBESSignature : GenericXMLSignature
    {
        private static string nameFile;
        private static string pathFile;
        private string fileToSign;

        public XAdESBESSignature(string fileToSign)
        {
            this.fileToSign = fileToSign;
        }

        public static void firmar(string xmlPath, string pathSignature, string passSignature, string pathOut, string nameFileOut)
        {
            XAdESBESSignature signature = new XAdESBESSignature(xmlPath);
            signature.setPassSignature(passSignature);
            signature.setPathSignature(pathSignature);
            pathFile = pathOut;
            nameFile = nameFileOut;

            signature.Execute();
        }

        protected override DataToSign createDataToSign()
        {
            DataToSign datosAFirmar = new DataToSign();

            datosAFirmar.setXadesFormat(EnumFormatoFirma.XAdES_BES);

            datosAFirmar.setEsquema(XAdESSchemas.XAdES_132);
            datosAFirmar.setXMLEncoding("UTF-8");
            datosAFirmar.setEnveloped(true);
            datosAFirmar.addObject(new ObjectToSign(new InternObjectToSign("comprobante"), "contenido comprobante", null, "text/xml", null));
            datosAFirmar.setParentSignNode("comprobante");

            org.w3c.dom.Document docToSign = null;
            try
            {
                docToSign = getDocument(this.fileToSign);
            }
            catch (IOException ex)
            {
                throw new IOException(ex.getMessage());
            }

            datosAFirmar.setDocument(docToSign);
     
            return datosAFirmar;
        }

        protected override string getPathOut()
        {
            return nameFile;
        }

        protected override string getSignatureFileName()
        {
            return pathFile;
        }
    }
}
