using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitateCasaBanca
    {
        public int IdCasaBanca { get; set; }
        public string Denumire { get; set; }
        public string ContBancar { get; set; }
        public string Proxy
        {
            get
            {
                return Denumire + (string.IsNullOrEmpty(ContBancar) ? "" : " (" + ContBancar + ")");
            }
        }
    }
}