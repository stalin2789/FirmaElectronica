using com.sun.org.apache.bcel.@internal.generic;
using com.sun.xml.@internal.txw2;
using es.mityc.firmaJava.libreria.xades;
using iTextSharp.text;
using java.io;
using java.lang;
using java.security;
using java.security.cert;
using javax.swing.text;
using javax.xml.parsers;
using javax.xml.transform;
using javax.xml.transform.dom;
using javax.xml.transform.stream;
using org.w3c.dom;
using org.xml.sax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaElectronica.Models
{
    public abstract class GenericXMLSignature
    {
        private string pathSignature; //recibe el cert
        private string passSignature; //pass del cert

        public string getPathSignature()
        {
            return this.pathSignature;
        }

        public void setPathSignature(string pathSignature)
        {
            this.pathSignature = pathSignature;
        }

        public string getPassSignature()
        {
            return this.passSignature;
        }

        public void setPassSignature(string passSignature)
        {
            this.passSignature = passSignature;
        }

        protected void Execute() 
        {
            KeyStore keyStore = getKeyStore();
            if (keyStore == null)
            {
                throw new IOException("No se pudo obtener almacen de firma.");
            }
            string alias = getAlias(keyStore);

            X509Certificate certificate = null;
            try
            {
                certificate = (X509Certificate)keyStore.getCertificate(alias);
                if (certificate == null)
                {
                    throw new IOException("No existe ningún certificado para firmar.");
                }
            }
            catch (KeyStoreException e1)
            {
                throw new IOException("Error: " + e1.getMessage());
            }

            PrivateKey privateKey = null;
            KeyStore tmpKs = keyStore;

            try
            {
                privateKey = (PrivateKey)tmpKs.getKey(alias, this.passSignature.ToCharArray());
            }
            catch (UnrecoverableKeyException e)
            {
                throw new IOException("No existe clave privada para firmar.");
            }
            catch (KeyStoreException e)
            {
                throw new IOException("No existe clave privada para firmar.");
            }
            catch (NoSuchAlgorithmException e)
            {
                throw new IOException("No existe clave privada para firmar.");
            }

            Provider provider = keyStore.getProvider();
            DataToSign dataToSign = createDataToSign();
            FirmaXML firma = new FirmaXML();
            org.w3c.dom.Document docSigned = null; //aqui es con la libreria org.w3c.Document   originalmente

            try
            {
                java.lang.Object[] res = (java.lang.Object[])firma.signFile(certificate, dataToSign, privateKey, provider);
                docSigned = (org.w3c.dom.Document)res[0];
            }
            catch (java.lang.Exception ex)
            {
                throw new IOException("Error realizando la firma: " + ex.Message);
            }

            string filePath = getPathOut() + File.separatorChar + getSignatureFileName();

            saveDocumenteDisk(docSigned, filePath);
        }

        private void saveDocumenteDisk(org.w3c.dom.Document document, string pathXml)
        {
            try
            {
                DOMSource source = new DOMSource(document);
                StreamResult result = new StreamResult(new File(pathXml));

                TransformerFactory transformerFactory = TransformerFactory.newInstance();

                Transformer transformer = transformerFactory.newTransformer();
                transformer.transform(source, result);
            }
            catch (TransformerConfigurationException e)
            {
                throw new IOException("Error: " + e.getMessage());
            }
            catch (TransformerException e)
            {
                throw new IOException("Error: " + e.getMessage());
            }
        }

        protected abstract DataToSign createDataToSign();
        protected abstract string getSignatureFileName();

        protected abstract string getPathOut();

        protected org.w3c.dom.Document getDocument(string resource)
        {
            org.w3c.dom.Document doc = null;
            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            dbf.setNamespaceAware(true);
            File file = new File(resource);

            try
            {
                DocumentBuilder db = dbf.newDocumentBuilder();
                doc = db.parse(file);
            }
            catch (ParserConfigurationException ex)
            { 
                throw new IOException("Error al parsear el documento: " + ex.getMessage());
            }
            catch (SAXException ex)
            {
                throw new IOException("Error al parsear el documento: " + ex.getMessage());
            }
            catch (java.io.IOException ex)
            {
                throw new IOException("Error al parsear el documento: " + ex.getMessage());
            }
            catch (IllegalArgumentException ex)
            {
                throw new IOException("Error al parsear el documento: " + ex.getMessage());
            }

            return doc;
        }

        private string getAlias(KeyStore keyStore)
        {
            string alias = null;

            try
            {
                java.util.Enumeration nombres = keyStore.aliases();
                while (nombres.hasMoreElements())
                {
                    string tmpAlias = (string)nombres.nextElement();
                    if (keyStore.isKeyEntry(tmpAlias))
                    {
                        alias = tmpAlias;
                    }
                }
            }
            catch (KeyStoreException e)
            {
                throw new IOException("Error: " + e.getMessage());
            }
            return alias;
        }

        private KeyStore getKeyStore()
        {
            KeyStore ks = null;

            try
            {
                ks = KeyStore.getInstance("PKCS12");
                ks.load(new FileInputStream(this.pathSignature), this.passSignature.ToCharArray());
            }
            catch (KeyStoreException e)
            {

                throw new IOException("Error: " + e.getMessage());
            }
            catch (NoSuchAlgorithmException e)
            {
                throw new IOException("Error: " + e.getMessage());
            }
            catch (CertificateException e)
            {
                throw new IOException("Error: " + e.getMessage());
            }
            catch (IOException e)
            {
                throw new IOException("Error: " + e.getMessage());
            }
            return ks;
        }
    }
}
