using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfHotel.Nomenclatoare_Final
{
    public class BorderouAgentie
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

        public decimal Cazare { get; set; }
        public decimal MicDejun { get; set; }
        public decimal Masa { get; set; }
        public decimal AlteServicii { get; set; }
        public decimal Total
        {
            get
            {
                return Cazare + MicDejun + Masa + AlteServicii + PatSup;
            }
        }

        public int IdRezCamera { get; set; }
        public string Turist { get; set; }

        public DateTime DataIntrare { get; set; }
        public decimal PatSup { get; set; }
    }

}