using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitatePL
    {
        public int ID { get; set; }
        public string Denumire { get; set; }
        public string Oras { get; set; }
        public string Judet { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Denumire);
            if (!string.IsNullOrEmpty(Oras))
            {
                sb.AppendFormat(", {0}", Oras.ToUpper());
            }
            if (!string.IsNullOrEmpty(Judet))
            {
                sb.AppendFormat(", Jud. {0}", Judet.ToUpper());
            }
            return sb.ToString();
        }
    }
}