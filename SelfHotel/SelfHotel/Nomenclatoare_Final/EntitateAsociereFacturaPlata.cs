using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitateAsociereFacturaPlata
    {
        public int IdPSVMP { get; set; }
        public int IdFacturaServ { get; set; }
        public int IdRezervareServiciu { get; set; }
        public int IdPlata { get; set; }
        public DateTime DataPlata { get; set; }
        public int IdFactura { get; set; }
        public DateTime DataFactura { get; set; }
        public bool AsociereValida
        {
            get
            {
                return IdPSVMP != 0 && IdFacturaServ != 0 && IdRezervareServiciu != 0 && IdPlata != 0 &&
                    DataPlata != DateTime.MinValue && IdFactura != 0 && DataFactura != DateTime.MinValue;
            }
        }
    }

}