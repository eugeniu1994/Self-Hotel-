using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitateFacturaAgentie
    {
        public string TipDoc { get; set; }
        public string NrDoc { get; set; }
        public DateTime DataDoc { get; set; }

        public string Document_Proxy
        {
            get
            {
                string rv = "";
                if (!string.IsNullOrEmpty(TipDoc))
                {
                    rv = string.Format("{0} / {1:dd.MM.yyyy}", NrDoc, DataDoc);
                }
                return rv;
            }
        }

        public int OrdineCam { get; set; }
        public int OrdineNiv { get; set; }
        public int OrdineCrp { get; set; }

        public int IdRezCamera { get; set; }
        public string Turist { get; set; }
        public string Camera { get; set; }

        public int NrAdulti { get; set; }

        public DateTime Sosire { get; set; }
        public int NrNopti { get; set; }
        public DateTime Plecare { get; set; }

        public decimal Cazare { get; set; }
        public decimal MicDejun { get; set; }
        public decimal MasaCard { get; set; }
        public decimal AlteServicii { get; set; }
        public decimal Total
        {
            get
            {
                return Cazare + PatSup + MicDejun + Dejun + Cina + MasaCard + AlteServicii;
            }
        }

        public decimal PatSup { get; set; }

        public decimal Dejun { get; set; }
        public decimal Cina { get; set; }

        public decimal TarifUnitar
        {
            get
            {
                if (NrNopti == 0)
                {
                    return Cazare;
                }
                else
                {
                    return Math.Round(Cazare / NrNopti, 2);
                }
            }
        }
    }
    
}