using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfHotel.Nomenclatoare
{
    public class Pictures
    {
        public int ID { get; set; }
        public Guid Guid { get; set; }
        public string Descriere { get; set; }
        public byte[] FileData { get; set; }
    }
}