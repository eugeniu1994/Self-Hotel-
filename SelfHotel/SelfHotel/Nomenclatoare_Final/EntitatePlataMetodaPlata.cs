using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitatePlataMetodaPlata
    {
        public int ID { get; set; }
        public int IdPlata { get; set; }
        public int IdMetodaPlata { get; set; }
        public Decimal ValoareMoneda { get; set; }
        public Decimal ValoareRON { get; set; }

        public string CodMetoda { get; set; }
    }
}