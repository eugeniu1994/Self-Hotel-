using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitateServiciuValoare
    {
        public int ID { get; set; }
        public int IdRezervareServiciu { get; set; }
        public DateTime Data { get; set; }
        public decimal Valoare { get; set; }
        public decimal ValoareRON { get; set; }
        public bool Postat { get; set; }
        public decimal Curs { get; set; }
        public decimal ValoareRonCalculata
        {
            get
            {
                return Math.Round(Valoare * Curs, 2);
            }
        }

        public decimal AchitatMoneda { get; set; }
        public decimal AchitatRON { get; set; }
        public decimal SoldMoneda
        {
            get
            {
                return Valoare - AchitatMoneda;
            }
        }

        public decimal GetValoareRON(decimal curs)
        {
            return Math.Round(AchitatRON + SoldMoneda * curs, 2);
        }

        public decimal FacturatMoneda { get; set; }
        public decimal FacturatRon { get; set; }

        //public DataGridViewCell Cell { get; set; }

        public decimal ValDeFacturat { get; set; }

        public decimal DeFactMonedaServ { get; set; }       // de facturat in moneda serviciului
        public decimal DeFactMonedaFact { get; set; }       // de facturat in moneda facturii
        public decimal DeFactRON { get; set; }              // de facturat in RON
        public decimal DeFactCurs { get; set; }             // curs zi in moneda facturii
        public decimal DeFactIdMoneda { get; set; }         // id moneda de facturat

        public List<EntitateFacturaValoareZilnica> ValoriFacturate { get; set; }
        public List<EntitatePlatiValoriZilnice> ValoriPlatite { get; set; }

        public decimal CantDeFact
        {
            get
            {
                if (ValDeFacturat != 0)
                {
                    return Math.Round(1 / (ValoareRON / ValDeFacturat), 2);
                }
                else if (DeFactRON != 0)
                {
                    return Math.Round(1 / (ValoareRON / DeFactRON), 2);
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool AreFacturi { get; set; }

        public decimal SoldPlataMonedaPlata { get; set; }
        public decimal dePlataMonedaPlata { get; set; }

        public decimal SoldPlataMonedaServ
        {
            get
            {
                return Valoare - AchitatMoneda;
            }
        }
        public decimal dePlataMonedaServ { get; set; }

        public decimal SoldPlataRon
        {
            get
            {
                return ValoareRON - AchitatRON;
            }
        }
        public decimal dePlataRon { get; set; }

        public bool EsteProdus { get; set; }

        public bool DePlataPartial
        {
            get
            {
                if (EsteProdus)
                {
                    return false;
                }
                else
                {
                    if (AchitatRON == 0)
                    {
                        return SoldPlataRon != dePlataRon;
                    }
                    else
                    {
                        return AchitatRON != 0 && dePlataRon != 0;
                    }
                }
                //return SoldPlataRon != dePlataRon;
            }
        }

        public Dictionary<int, EntitateDePlata> DePlataPerMetode = new Dictionary<int, EntitateDePlata>();
    }

}