using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitatePlataServiciuValoareModPlata
    {
        public int ID { get; set; }
        public int IDPlata { get; set; }
        public int IDRezervareServiciu { get; set; }
        public int IdModalitatePlata { get; set; }
        public decimal ValoareMoneda { get; set; }
        public decimal ValoareRON { get; set; }
        public decimal ValoareMonedaServ { get; set; }
    }
}