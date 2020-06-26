using FirmaElectronica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaElectronica
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathArchivo = "C:\\2506202001160056085600110010010000000091234567814_gg.xml";//RUTA DEL ARCHIVO XML
            string pathFirma = "C:\\gregory_stalin_moran_lituma.p12"; //RUTA DE LA FIRMA ELECTRONICA  
            string claveFirma = "MoranGut2027"; //CLAVE DE LA FIRMA ELECTRONICA
            string pathArchivoFirmado = @"C:\\"; //RUTA DONDE SE ALMACENARA EL ARCHIVO FIRMADO
            string nombreArchivoSalida = "2506202001160056085600110010010000000091234567814_gg_f.xml";

            //string mensaje = "";

            DevelopedSignature firma = new DevelopedSignature(pathArchivo, pathFirma, claveFirma, pathArchivoFirmado, nombreArchivoSalida);
            firma.Firmar();

            //try
            //{
            //    //INSTANCIAMOS LA CLASE PARA SER USADA
            //    FirmarXML firma = new FirmarXML();
            //    if (firma.Firmar(pathFirma, claveFirma, pathArchivo, pathArchivoFirmado+@"\"+nombreArchivoSalida,ref mensaje)){
            //        Console.WriteLine("Archivo firmado con exito");
            //    }
            //    else
            //    {
            //        Console.WriteLine(mensaje);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }
    }
}
