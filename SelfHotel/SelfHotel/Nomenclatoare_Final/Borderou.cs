using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfHotel.Nomenclatoare_Final
{
    public class Borderou
    {
        public int IdFactura { get; set; }

        public string Camera { get; set; }
        public string Turist { get; set; }
        public string Serviciu { get; set; }
        public decimal Valoare { get; set; }
        public string Document { get; set; }
        public string NrDoc { get; set; }
        public DateTime DataDoc { get; set; }
        public DateTime Sosire { get; set; }
        public DateTime Plecare { get; set; }

        public DateTime ServDeLa { get; set; }
        public DateTime ServPanaLa { get; set; }

        public String DenumireServiciu
        {
            get
            {
                string DataServiciu = "";
                if (ServDeLa == DateTime.MinValue && ServPanaLa == DateTime.MinValue)
                {
                    DataServiciu = "";
                }
                else if (ServDeLa != DateTime.MinValue && ServPanaLa == DateTime.MinValue)
                {
                    DataServiciu = ServDeLa.ToString("dd.MM");
                }
                else if (ServDeLa == DateTime.MinValue && ServPanaLa != DateTime.MinValue)
                {
                    DataServiciu = ServPanaLa.ToString("dd.MM");
                }
                else
                {
                    if (ServDeLa == ServPanaLa)
                    {
                        DataServiciu = ServDeLa.ToString("dd.MM");
                    }
                    else
                    {
                        DataServiciu = ServDeLa.ToString("dd.MM") + " - " + ServPanaLa.ToString("dd.MM");
                    }
                }

                return string.Format("{0} {1}", Serviciu, string.IsNullOrEmpty(DataServiciu) ? "" : "(" + DataServiciu + ")");
            }
        }

        public String InfoCamera
        {
            get
            {
                string DataServiciu = "";
                if (Sosire == DateTime.MinValue && Plecare == DateTime.MinValue)
                {
                    DataServiciu = "";
                }
                else if (Sosire != DateTime.MinValue && Plecare == DateTime.MinValue)
                {
                    DataServiciu = Sosire.ToString("dd.MM");
                }
                else if (Sosire == DateTime.MinValue && Plecare != DateTime.MinValue)
                {
                    DataServiciu = Plecare.ToString("dd.MM");
                }
                else
                {
                    if (Sosire == Plecare)
                    {
                        DataServiciu = Sosire.ToString("dd.MM");
                    }
                    else
                    {
                        DataServiciu = Sosire.ToString("dd.MM") + " - " + Plecare.ToString("dd.MM");
                    }
                }

                return string.Format("{0} {1}", Camera, string.IsNullOrEmpty(DataServiciu) ? "" : "/ " + DataServiciu);
            }
        }

        public string SerieFac { get; set; }
        public string NumarFac { get; set; }
        public string CumparatorFac { get; set; }
        public DateTime DataFac { get; set; }
    }

}