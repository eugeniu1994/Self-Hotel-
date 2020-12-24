using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitatePlatiValoriZilnice
    {
        public int ID { get; set; }
        public int IdPlata { get; set; }
        public int IdModPlata { get; set; }
        public int IdValoareZilnica { get; set; }
        public decimal ValoareMoneda { get; set; }
        public decimal ValoareRON { get; set; }
        public decimal ValoareMonedaServ { get; set; }
        public bool Anulat { get; set; }
        public bool CuPricina { get; set; }
    }
}