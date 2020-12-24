using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitateCursValutar
    {
        public int IdMoneda { get; set; }
        public decimal Valoare { get; set; }
        public DateTime Zi { get; set; }
    }
}