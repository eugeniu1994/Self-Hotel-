using SelfHotel.Nomenclatoare_Final;
using SelfHotel.NomenclatoareNew;
using SelfHotel.Setari;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ZipEscort.Setari
{
    public class ConexiuneDB
    {
        public static DateTime DataLucr { get; set; } //preluat
        public static EntitateMoneda moneda{get;set;}
        public static Int32 _IdHotel = 1; //o sa-l preiau din configurari  preluat
        public static Int32 IdHotel //preluat
        {
            get
            {
                return _IdHotel;
                //return Convert.ToInt32(ConfigurationManager.AppSettings["idHotel"]); //return _IdHotel;
            }
        }

        public static EntitatePL cmbPunctLucru { get; set; }

        public static Int32 _IdFirma = 1; //sa verifici aici daca idFirma este 0, sal preiei din fisier
        public static Int32 IdFirma
        {
            get
            {
                return _IdFirma;
                //return Convert.ToInt32(ConfigurationManager.AppSettings["idFirma"]);
            }
        }
        public static Int32 IdUtilizator { get; set; }
        public static Int32 Versiune { get; set; }
        public static string CatalogFirma { get; set; }
        public static EntitateTipDoc tipDoc { get; set; } //preluat separat

        public static string emailAdministrator{get;set;}
        public static decimal CursValutar { get; set; }// = 4;
        public static string SerieNefiscal { get; set; } //preluat
        public static string DocNr { get; set; } //preluat
        public static Int32 IdTipCursValutar { get; set; } //preluat

        public static String serieFactura { get; set; }
        public static String numarFactura { get; set; }
        public static string obsFactura { get; set; }
        public static string AdminMail { get; set; }

        public static EntitateCarnetFacturi Carnet { get; set; }
        public static String Transport { get; set; }
        public static DateTime TermenPlata { get; set; }

        public static void initializare()
        {
            TermenPlata = DateTime.Today;
            try
            {
                 List<SetariBaza> listaSetari = SetariBaza.getLista();
                 serieFactura = "serie";
                 numarFactura = "N1";
                 obsFactura = "Observatii din baza";
                 AdminMail = "vezeteu.eugeniu@yahoo.com";
                 try
                 {
                     SetariBaza iserieFactura = listaSetari.Where(x => x.Denumire.ToLower() == "seriefactura").ToList()[0];
                     serieFactura = iserieFactura.Valoare;
                     SetariBaza iobsFactura = listaSetari.Where(x => x.Denumire.ToLower() == "obsfactura").ToList()[0];
                     obsFactura = iobsFactura.Valoare;
                     SetariBaza iAdminMail = listaSetari.Where(x => x.Denumire.ToLower() == "adminmail").ToList()[0];
                     AdminMail = iAdminMail.Valoare;
                     SetariBaza iTransport = listaSetari.Where(x => x.Denumire.ToLower() == "transport").ToList()[0];
                     Transport = iTransport.Valoare;
                    
                 }
                 catch (Exception exc)
                 {
                     Transport = serieFactura = numarFactura = obsFactura = AdminMail = "---";
                 }

                try
                {
                    SetariBaza iHotelSetare = listaSetari.Where(x => x.Denumire.ToLower() == "idhotel").ToList()[0]; Int32.TryParse(iHotelSetare.Valoare, out _IdHotel);
                    SetariBaza iFirmaSetare = listaSetari.Where(x => x.Denumire.ToLower() == "idfirma").ToList()[0]; Int32.TryParse(iFirmaSetare.Valoare, out _IdFirma);
                }
                catch { }

                List<DataLucruObj> listdDataLucru = DataLucruObj.GetLista().Where(x => x.IdHotel == ConexiuneDB.IdHotel).ToList();
                DateTime DataL;
                if (listdDataLucru.Count > 0)
                {
                    DataL = listdDataLucru[0].DataLucru;
                }
                else
                {
                    DataL = DateTime.Today;
                }
                ConexiuneDB.DataLucr = DataL;
                SetariBaza iTermenPlata = listaSetari.Where(x => x.Denumire.ToLower() == "termenplata").ToList()[0];
                int nrZile = 0;
                Int32.TryParse(iTermenPlata.Valoare, out nrZile);
                TermenPlata = ConexiuneDB.DataLucr.AddDays(nrZile);
            }
            catch { }
            try
            {
                ConexiuneDB.SerieNefiscal = HelperCurs.GetSerieNefiscal();
            }
            catch { }
            try
            {
                ConexiuneDB.DocNr = HelperCurs.LoadNumar(ConexiuneDB.SerieNefiscal);
            }
            catch { }
            try
            {
                ConexiuneDB.moneda = EntitateMoneda.GetMonedaNationala();
            }
            catch { }
            try
            {
                ConexiuneDB.IdTipCursValutar=HelperCurs.GetTipCursValutar();
                ConexiuneDB.CursValutar = HelperCurs.GetCurs(ConexiuneDB.DataLucr, ConexiuneDB.moneda.ID, ConexiuneDB.IdTipCursValutar);
                //Carnet = EntitateCarnetFacturi.getCarnet();
            }
            catch { }
        }

        public static string CnnString
        {
            get
            {
                //return "Data Source=MSF-PROG-EV/SMARTHOTELDEMO;User Id=sa;Password=ics101pass;";
                //return "Data Source=82.79.90.13,33344;User ID=ICSAdmin;Password=ics101pass";
                return ConfigurationManager.AppSettings["CnnString"];
            }
        }
        public static bool ListeazaImprimanta = true;
        public static int port
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
            }
        }
    }
}