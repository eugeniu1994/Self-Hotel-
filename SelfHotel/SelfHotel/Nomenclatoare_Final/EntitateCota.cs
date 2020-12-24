using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitateCota
    {
        public DateTime DataIni { get; set; }
        public DateTime DataFin { get; set; }
        public int IdCota { get; set; }
        public string Denumire { get; set; }
        public decimal Procent { get; set; }

        public bool Expirata
        {
            get
            {
                return ConexiuneDB.DataLucr < DataIni || ConexiuneDB.DataLucr > DataFin;
            }
        }
        public bool AreServicii { get; set; }
    }
}