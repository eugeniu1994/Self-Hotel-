using SelfHotel.Setari;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class Pozitie
    {
        public int ID { get; set; }

        //begin campuri pentru gruparile penntru factura de grup
        public int IdTipCamera { get; set; }
        public int NrNopti { get; set; }
        public int NrAdulti { get; set; }
        public bool EsteDinCodTarif { get; set; }
        //end 

        static int _IdNeg = 0;
        public static int IdNeg
        {
            get
            {
                _IdNeg--;
                return _IdNeg;
            }
        }

        public int Ordine { get; set; }
        public string Denumire { get; set; }
        public string UM { get; set; }
        public decimal Cantitate { get; set; }
        public decimal ProcentTVA { get; set; }
        public decimal IdCotaTVA { get; set; }
        public string CodCotaTVA { get; set; }
        public decimal PretRON { get; set; }
        public decimal ValoareFTVARON { get; set; }
        public decimal ValoareTVARON { get; set; }
        public decimal ValoareTotalaRON { get; set; }
        public decimal PretMoneda { get; set; }
        public decimal ValoareFTVAMoneda { get; set; }
        public decimal ValoareTVAMoneda { get; set; }
        public decimal ValoareTotalaMoneda { get; set; }
        public int IdGrupaFact { get; set; }
        public bool EsteDiferentaCurs { get; set; }

        public decimal ValoareTotalInitialaRON { get; set; }

        public decimal ProcentTVA_Initial { get; set; }
        public decimal IdCotaTVA_Initial { get; set; }

        public string CotaTVA_Proxy
        {
            get
            {
                string proc = "";
                if (Math.Round(ProcentTVA, 0) == ProcentTVA)
                {
                    proc = ProcentTVA.ToString("N0");
                }
                else if (Math.Round(ProcentTVA, 1) == ProcentTVA)
                {
                    proc = ProcentTVA.ToString("N1");
                }
                else
                {
                    proc = ProcentTVA.ToString("N2");
                }
                //if (CodCotaTVA == "S" && Setare.GetFolosesteSFDD())
                //{
                //    return string.Format("SFDD", proc);
                //}
                //if ( CodCotaTVA != null && Setare.GetCotaSFDD().Contains(CodCotaTVA))
                //{
                //    return string.Format("SFDD", proc);
                //}
                return string.Format("{0}%", proc);
            }
        }

        public List<EntitateServiciu> Servicii { get; private set; }

        public Pozitie()
        {
            UM = "BUC";
            Denumire = "";
            Servicii = new List<EntitateServiciu>();
        }

        public string NumeInitial { get; set; }

        internal void CalculServiciiDinTotal()
        {
            decimal RamasDeImpartit = ValoareTotalaRON;

            for (int i = 0; i < Servicii.Count; i++)
            {
                if (i == Servicii.Count - 1)
                {
                    Servicii[i].DeFactRON = RamasDeImpartit;
                }
                else
                {
                    Servicii[i].DeFactRON = Math.Round(Servicii[i].PonderePozitieFactura * ValoareTotalaRON, 2);
                }

                if (Servicii[i].IdMoneda == HelperCurs.IdRON)
                {
                    Servicii[i].DeFactMonedaServ = Servicii[i].DeFactRON;
                }
                else
                {
                    Servicii[i].DeFactMonedaServ = Math.Round(Servicii[i].DeFactRON / HelperCurs.GetCurs(ConexiuneDB.DataLucr, Servicii[i].IdMoneda, ConexiuneDB.IdTipCursValutar), 2);
                }

                Servicii[i].DeFactMonedaFact = Servicii[i].DeFactRON;

                RamasDeImpartit -= Servicii[i].DeFactRON;
            }

            ValoareFTVARON = Math.Round(ValoareTotalaRON / (1.0m + ProcentTVA / 100.0m), 2);
            ValoareTVARON = Math.Round(ValoareFTVARON * ProcentTVA / 100.0m, 2);
            PretRON = Math.Round(ValoareFTVARON / Cantitate, 3);
        }
    }
}