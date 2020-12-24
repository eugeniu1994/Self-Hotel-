using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitateMetodaPlata
    {
        private bool _Selected = false;
        public bool Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                if (value != _Selected)
                {
                    _Selected = value;
                    if (!_Selected)
                    {
                        Valoare = 0;
                    }
                }
            }
        }
        public decimal Valoare { get; set; }

        public int ID { get; set; }
        public string Cod { get; set; }
        public string Denumire { get; set; }
        public int GrupareID { get; set; }
        public bool EsteNumerar { get; set; }
        public bool EsteCardBancar { get; set; }

        public bool EsteOP { get; set; }

        //public DataGridViewRow Row { get; set; }
    }

}