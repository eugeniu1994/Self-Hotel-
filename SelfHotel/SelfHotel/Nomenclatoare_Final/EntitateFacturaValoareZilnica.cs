using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitateFacturaValoareZilnica
    {
        public int ID { get; set; }
        public int IdFactura { get; set; }
        public int IdPozitie { get; set; }
        public int IdValoareZilnica { get; set; }
        public decimal ValoareMoneda { get; set; }
        public decimal ValoareRON { get; set; }
        public decimal ValoareMonedaServ { get; set; }
        public bool Anulat { get; set; }
    }
}