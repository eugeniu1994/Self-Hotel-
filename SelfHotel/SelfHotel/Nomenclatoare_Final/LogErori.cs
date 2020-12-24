using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace SelfHotel.Nomenclatoare_Final
{
    public static class LogErori
    {
        #region "   Proprietati                               "

        private static string Despartitor = "\r\n ------------------------------------------------------------------------------------------ \r\n";
        private static object SyncFisier = new object();

        #endregion

        #region "   Metoda de salvare                         "
        //StringBuilder eroare = new StringBuilder();

        public static void ScrieLog(string mesaj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Despartitor);
            sb.AppendFormat("{1}Data: {2}{1}Mesaj APP: {0}{1}", mesaj, Environment.NewLine, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            ScrieInFisier(sb.ToString());
        }

        public static void SalveazaFaraMesaj(Exception exc, string locatie)
        {
            StringBuilder eroare = new StringBuilder();
            //Salveaza(exc, locatie);
            eroare = new StringBuilder();
            eroare.Append(Despartitor);
            eroare.Append("\r\n Data: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\r\n In: " + locatie + "\r\n Exceptie:" + exc.Message + "\r\n" + exc.StackTrace);
            ScrieInFisier(eroare.ToString());
            return;
        }
        #endregion

        #region "   Metode private                            "

        public  static void Salveaza(Exception exc, string locatie)
        {
            StringBuilder eroare = new StringBuilder();
            eroare.Append(Despartitor);
            eroare.Append("\r\n Data: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\r\n In: " + locatie + "\r\n Exceptie:" + exc.Message + "\r\n" + exc.StackTrace);
            ScrieInFisier(eroare.ToString());
        }

        private static void ScrieInFisier(String eroare)
        {
            lock (SyncFisier)
            {
                if (!Directory.Exists("c:\\MultiSoftLogs"))
                {
                    try
                    {
                        Directory.CreateDirectory("c:\\MultiSoftLogs");
                    }
                    catch (Exception exc)
                    {}
                }

                if (File.Exists("c:\\MultiSoftLogs\\SolONH.Log"))
                {
                    try
                    {
                        FileInfo info = new FileInfo("c:\\MultiSoftLogs\\SolONH.Log");
                        if ((int)((info.Length / 1024) / 1024) >= 4)
                        {
                            if (File.Exists("c:\\MultiSoftLogs\\SolONHold.Log"))
                            {
                                File.Delete("c:\\MultiSoftLogs\\SolONHold.Log");
                            }
                            File.Move("c:\\MultiSoftLogs\\SolONH.Log", "c:\\MultiSoftLogs\\SolONHold.Log");

                        }
                    }
                    catch (Exception exc)
                    {}
                }

                try
                {
                    System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\MultiSoftLogs\\SolONH.Log", true);

                    file.WriteLine(eroare);
                    file.Close();
                }
                catch (Exception exc)
                {}
            }
        }

        #endregion
    }
}