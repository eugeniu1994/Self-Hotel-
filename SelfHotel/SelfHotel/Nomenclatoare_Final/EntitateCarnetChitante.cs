using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitateCarnetChitante
    {
        public int ID { get; set; }
        public string Serie { get; set; }
        public string NumarDeLa { get; set; }
        public string NumarPanaLa { get; set; }
        public string Numar { get; set; }
        public bool EsteReceptie { get; set; }
        public bool EsteVirament { get; set; }
        public bool EsteAvans { get; set; }
        public string LastNumber { get; set; }
        public DateTime LastData { get; set; }
        public long LastNumberParsed { get; set; }
        public int IdFurnizor { get; set; }
        public string NumeFurnizor { get; set; }
    }
}