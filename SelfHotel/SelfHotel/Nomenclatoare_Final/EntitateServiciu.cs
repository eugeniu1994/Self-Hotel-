using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitateServiciu
    {
        public bool Selectat { get; set; }

        //facturagrup - grupare pe tip idtipcamera
        public int IdTipCamera { get; set; }
        public int NrNopti { get; set; }
        //facturagrup

        //public EntitateServiciuExt ServiciuContainer { get; set; }
        public bool ForteazaPostareTrecut { get; set; }

        public int ID { get; set; }
        public int IdPartener { get; set; }
        public bool EsteVirament { get; set; }
        public int IdRezervareCamera { get; set; }
        public int IdVenit { get; set; }
        public int IdTarif { get; set; }
        public int IdPlanMasa { get; set; }
        public string DenumireServiciu { get; set; }
        public int IdMoneda { get; set; }
        public int IdCotaTVA { get; set; }
        public string CodCotaTVA { get; set; }
        public bool TaxaProcentuala { get; set; }
        public bool TaxaLaPersoane { get; set; }
        public decimal ValoareTaxa { get; set; }
        public bool Sters { get; set; }
        public int IdMotivStergere { get; set; }
        public string Observatii { get; set; }
        public bool PostareAmanata { get; set; }
        public int IdRezervareCameraMutat { get; set; }

        public bool esteAvans { get; set; }

        public string ParametriPostare { get; set; }
        public int ParametriPostareInt
        {
            get
            {
                int val = 0;
                if (!Int32.TryParse(ParametriPostare, out val))
                {
                    val = 0;
                }
                return val;
            }
        }

        public int partyID { get; set; }

        public int IdAvansRezervare { get; set; }//folosit la get date camera in form factura

        public EntitateServiciu ServiciuGrupat { get; set; }
        public bool ServiciuGrupatTrimisBF { get; set; }

        public string TipMasa { get; set; }

        public int VersiuneSplitare { get; set; }

        public decimal ValoareUnitara { get; set; }

        public bool LaSosire { get; set; }

        public DateTime DataCandAFostPlatit { get; set; }//pentru calcul sold
        public decimal CursulDinDataPlatii { get; set; }//pentru calcul sold
        public int IDTipCursPlata { get; set; }//pentru calcul sold

        public int IdCameraVirtualaGenerata { get; set; }
        public int IdServiciuAvansLa { get; set; }
        public int IdServiciuAvansStornat { get; set; }
        public bool PlatitIntegral { get; set; }
        public Dictionary<decimal, int> DicValoriZilnicePlatite { get; set; }
        public List<EntitateServiciu> ServiciiStornoAvans { get; private set; }
        public EntitateServiciu ServiciuAvans { get; set; }
        //public List<EntitateVZ> ListaVzPlatite { get; set; }
        public decimal ValoareStornouriMonedaSRV { get; set; }
        public decimal ValoareStornouriMonedaPLT { get; set; }
        public decimal ValoareStornouriMonedaRON { get; set; }

        public int Departament { get; set; }
        public int Articol { get; set; }

        public int StorneazaServiciul { get; set; }

        //public bool AreValoriZiuaPlecarii { get; set; }
        public bool AreValoriZiuaPlecarii(DateTime data)
        {
            return (Valori.FirstOrDefault(x => x.Data == data && ValoareMoneda != 0) != null);
        }

        public bool EsteStorno { get; set; }

        public decimal DeStornatRestituire { get; set; } // se foloseste la emiterea facturii dupa restituire plata

        public int IdPlataRestituire { get; set; }

        // atentie sa o si setezi
        public DateTime DataMax { get; set; }

        public bool AmZisDeValoriInAfaraIntervaluluiDeCazare { get; set; }

        public string perioadaServiciu
        {
            get
            {
                DateTime minValZi = Valori.Where(x => x.Valoare != 0).Min(x => x.Data);
                DateTime maxValZi = Valori.Where(x => x.Valoare != 0).Max(x => x.Data);

                if (minValZi == maxValZi)
                {
                    return minValZi.ToString("dd.MM.yyyy");
                }
                else
                {
                    return minValZi.ToString("dd.MM.yyyy") + " - " + maxValZi.ToString("dd.MM.yyyy");
                }
            }
        }

        public int OrdineCumulare
        {
            get
            {
                if (EsteCazare)
                {
                    return 0;
                }
                else if (EsteMasa)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }

        public override string ToString()
        {
            return DenumireServiciu + " " + Total.ToString("N2");
        }

        public string ObservatiiPrefix { get; set; }
        public string Observatii_Proxy
        {
            get
            {
                if (!string.IsNullOrEmpty(MotivRestanta))
                {
                    return "RESTANT: " + MotivRestanta;
                }
                if (string.IsNullOrEmpty(ObservatiiPrefix))
                {
                    return Observatii;
                }
                else
                {
                    return string.Format("{0}. {1}", ObservatiiPrefix, Observatii);
                }
            }
        }

        public string ObsFactura { get; set; }

        public bool EsteTaxa { get; set; }
        public bool EsteMasa { get; set; }
        public bool EsteCazare { get; set; }
        public bool EsteAltceva
        {
            get
            {
                return !EsteCazare && !EsteMasa && !EsteTaxa;
            }
        }

        public decimal PretMoneda { get; set; }
        public int IdPret { get; set; }
        public decimal PretRON
        {
            get
            {
                // este pretul care se va trimite la casa fiscala daca se merge pe cantitate produse
                // cantitate * pretron = valoare platita
                if (this.IdMoneda != ConexiuneDB.CursValutar)
                {
                    return Math.Round(this.PretMoneda * this.Curs, 2);
                }
                else
                {
                    return this.PretMoneda;
                }
            }
        }
        public decimal PretMonedaPlata { get; set; }
        public int CantitateAchitata { get; set; }      // cantitatea platita deja
        public int CantitateDePlata { get; set; }       // cantitatea ramasa de plata
        public int CantitateDePlata_Calc { get; set; }

        public string NumePartener { get; set; }
        public decimal ProcentTVA { get; set; }
        public string CodMoneda { get; set; }

        public decimal Total { get; set; }
        public string Total_Proxy { get; set; }
        public int Cantitate { get; set; }
        public string Cantitate_Proxy { get; set; }

        public int CantitateFactura
        {
            get
            {
                if (EsteCazare || EsteMasa || EsteTaxa)
                {
                    bool ok = false;// Setare.FolosesteCantitate1();
                    if (ok)
                    {
                        return 1;
                    }
                    else
                    {
                        if (Valori.Count(x => x.AchitatRON != 0) == Valori.Count(x => x.AchitatRON - x.ValoareRON == 0 && x.ValoareRON != 0))
                        {
                            return Valori.Count(x => x.AchitatRON != 0);
                        }
                        else if (Valori.Count(x => x.AchitatRON != 0) > 0)
                        {
                            return 1;
                        }
                        else if (Valori.Count() > 0)
                        {
                            return Valori.Count(x => x.Valoare != 0);
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
                return Cantitate;
            }
        }

        public string UM { get; set; }
        public string UM_Proxy
        {
            get
            {
                if (IdTarif != 0)
                {
                    return "";//return Setare.GetTarifDefaultUM();
                }
                else if (IdPlanMasa != 0)
                {
                    return "";//return Setare.GetPlanMasaDefaultUM();
                }
                else
                {
                    return UM;
                }
            }
        }

        //public DataGridViewRow Row { get; set; }

        public List<EntitateServiciu> CuplariMD { get; private set; }       // la cazare se ataseaza MD 9% si invers (pot sta doar impreuna)
        public EntitateServiciu CuplareCazare { get; set; }

        public decimal TotalPostat { get; set; }
        public decimal TotalZi { get; set; }

        public string TotalPostat_Proxy { get; set; }
        public string TotalZi_Proxy { get; set; }

        public decimal PlatitMoneda { get; set; }
        public decimal PlatitRon { get; set; }
        public decimal FacturatMoneda { get; set; }
        public decimal FacturatRon { get; set; }

        public decimal SoldMoneda { get; set; }           // sold in moneda serv
        public decimal SoldRON { get; set; }             // sold RON
        public decimal SoldMonedaPlata { get; set; }        // sold in moneda platii
        public decimal SoldCurs { get; set; }               // curs zi in moneda serviciului

        private decimal _DePlataMonedaServ { get; set; }
        public decimal DePlataMonedaServ
        {
            get
            {
                return this._DePlataMonedaServ;
            }
            set
            {
                this._DePlataMonedaServ = value;
                this.RamasDePlataMonedaServ = this._DePlataMonedaServ;
            }
        }      // de plata in moneda serviciului

        private decimal _DePlataMonedaPlata { get; set; }
        public decimal DePlataMonedaPlata
        {
            get
            {
                return this._DePlataMonedaPlata;
            }
            set
            {
                this._DePlataMonedaPlata = value;
                this.RamasDePlataMonedaPlata = this._DePlataMonedaPlata;
            }
        }     // de plata in moneda platii

        private decimal _DePlataRON { get; set; }
        public decimal DePlataRON
        {
            get
            {
                return this._DePlataRON;
            }
            set
            {
                this._DePlataRON = value;
                this.RamasDePlataRON = this._DePlataRON;
            }
        }             // de plata in RON
        public decimal DePlataCurs { get; set; }            // curs zi in moneda platii
        public decimal DePlataIdMoneda { get; set; }


        public decimal RamasDePlataMonedaServ { get; set; }      // de plata in moneda serviciului
        public decimal RamasDePlataMonedaPlata { get; set; }     // de plata in moneda platii
        public decimal RamasDePlataRON { get; set; }             // de plata in RON



        public decimal DeFactMonedaServ { get; set; }       // de facturat in moneda serviciului
        public decimal DeFactMonedaFact { get; set; }       // de facturat in moneda facturii
        public decimal DeFactRON { get; set; }              // de facturat in RON
        public decimal DeFactCurs { get; set; }             // curs zi in moneda facturii
        public decimal DeFactIdMoneda { get; set; }

        public decimal TotalRON { get; set; }           // folosit in formularul de discount
        public decimal TotalValuta { get; set; }        // folosit in formularul de discount

        public int IdGrupaFact1 { get; set; }
        public int IdGrupaFact2 { get; set; }
        public string GrupaFact1 { get; set; }
        public string GrupaFact2 { get; set; }

        public int IdServiciuCazareTaxa { get; set; }
        public int IdCategCopii { get; set; }

        public bool EsteTaxabil { get; set; }

        public List<EntitateServiciu> ServiciiDiscantuite { get; set; }

        public Boolean EsteDiscount { get; set; }
        public int IdServiciuDiscount { get; set; }
        public Boolean AreDiscount_Proxy
        {
            get
            {
                return IdServiciuDiscount > 0 ? true : false;
            }
        }

        public bool ArePlati { get; set; }
        public bool AreFacturi { get; set; }

        public bool EsteAchitat
        {
            get
            {
                return Math.Abs(Total - PlatitMoneda) <= 0.01m && PlatitMoneda != 0;
            }
        }

        public bool EsteFacturat
        {
            get
            {
                return Math.Abs(Total - FacturatMoneda) <= 0.01m && FacturatMoneda != 0;
            }
        }

        public decimal PonderePozitieFactura { get; set; }


        public void CalculTotalFacturare()
        {
            if (Valori.Count == 0)
            {
                Total = 0;
            }
            else
            {
                Total = Valori.Sum(x => x.Valoare);
                TotalRON = Valori.Sum(x => x.ValoareRON);
            }
            if (Math.Truncate(Total) == Total)
            {
                if (IdMoneda != ConexiuneDB.CursValutar)
                {
                    Total_Proxy = Total.ToString("N0");// +"/" + TotalRON.ToString("N2");
                }
                else
                {
                    Total_Proxy = Total.ToString("N0");
                }
            }
            else
            {
                if (IdMoneda != ConexiuneDB.CursValutar)
                {
                    Total_Proxy = Total.ToString("N2");// +"/" + TotalRON.ToString("N2");
                }
                else
                {
                    Total_Proxy = Total.ToString("N2");
                }
            }

            TotalPostat = 0;
            if (Valori.Count(x => x.Data < ConexiuneDB.DataLucr) > 0)
            {
                TotalPostat = Valori.Where(x => x.Data < ConexiuneDB.DataLucr).Sum(x => x.ValoareRON);
            }
            if (TotalPostat == 0)
            {
                TotalPostat_Proxy = "";
            }
            else
            {
                if (Math.Truncate(TotalPostat) == TotalPostat)
                {
                    TotalPostat_Proxy = TotalPostat.ToString("N0");
                }
                else
                {
                    TotalPostat_Proxy = TotalPostat.ToString("N2");
                }
            }

            TotalZi = 0;
            if (Valori.Count(x => x.Data == ConexiuneDB.DataLucr) > 0)
            {
                TotalZi = Valori.Where(x => x.Data == ConexiuneDB.DataLucr).Sum(x => x.ValoareRON);
            }
            if (TotalZi == 0)
            {
                TotalZi_Proxy = "";
            }
            else
            {
                if (Math.Truncate(TotalZi) == TotalZi)
                {
                    TotalZi_Proxy = TotalZi.ToString("N0");
                }
                else
                {
                    TotalZi_Proxy = TotalZi.ToString("N2");
                }
            }

            ///========CALCUL CANTITATE
            ///trebuie sa calculez nr de zile
            ///

            int nrZile = Valori.Count(x => x.Valoare != 0);

            if (EsteCazare)
            {
                Cantitate_Proxy = nrZile.ToString() + "z";
            }
            else if (EsteMasa)
            {
                Cantitate_Proxy = Cantitate.ToString() + "p x " + nrZile.ToString() + "z";
            }
            else if (EsteTaxa)
            {
                if (TaxaLaPersoane)
                {
                    Cantitate_Proxy = Cantitate.ToString() + "p x " + nrZile.ToString() + "z";
                }
                else
                {
                    Cantitate_Proxy = nrZile.ToString() + "z";
                }
            }
            else
            {
                if (nrZile > 1)
                {
                    if (Cantitate > 1)
                    {
                        Cantitate_Proxy = Cantitate.ToString() + " x " + nrZile.ToString() + "z";
                    }
                    else
                    {
                        Cantitate_Proxy = nrZile.ToString() + "z";
                    }
                }
                else
                {
                    Cantitate_Proxy = Cantitate.ToString();
                }
            }

            int cant = 0;
            if (EsteCazare)
            {
                cant = nrZile;

            }
            else if (EsteMasa)
            {
                cant = Cantitate * nrZile;
            }
            else if (EsteTaxa)
            {
                if (TaxaLaPersoane)
                {
                    cant = Cantitate * nrZile;
                }
                else
                {
                    cant = nrZile;
                }
            }
            else
            {
                if (nrZile > 1)
                {
                    if (Cantitate > 1)
                    {
                        cant = Cantitate * nrZile;
                    }
                    else
                    {
                        cant = nrZile;
                    }
                }
                else
                {
                    cant = Cantitate;
                }
            }

            ///END CALCUL CANTITATE

            if (cant > 0)
            {
                ValoareUnitara = Math.Round(Total / cant, 2);
            }
            else
            {
                ValoareUnitara = Total;
            }

            if (Valori.Count == 0)
            {
                this.ValoareMoneda = 0;
            }
            else
            {
                this.ValoareMoneda = Valori.Sum(x => x.Valoare);
            }

            if (Valori.Count == 0)
            {
                this.ValoareRon = 0;
            }
            else
            {
                this.ValoareRon = Valori.Sum(x => x.ValoareRON);
            }

            SoldMoneda = Total - PlatitMoneda;
            SoldRON = TotalRON - PlatitRon;
        }

        public void CalculTotal()
        {
            if (Valori.Count == 0)
            {
                Total = 0;
            }
            else
            {
                Total = Valori.Sum(x => x.Valoare);
                TotalRON = Valori.Sum(x => x.ValoareRON);
            }
            if (Math.Truncate(Total) == Total)
            {
                if (IdMoneda != ConexiuneDB.CursValutar)
                {
                    Total_Proxy = Total.ToString("N0");// +"/" + TotalRON.ToString("N2");
                }
                else
                {
                    Total_Proxy = Total.ToString("N0");
                }
            }
            else
            {
                if (IdMoneda != ConexiuneDB.CursValutar)
                {
                    Total_Proxy = Total.ToString("N2");// +"/" + TotalRON.ToString("N2");
                }
                else
                {
                    Total_Proxy = Total.ToString("N2");
                }
            }

            TotalPostat = 0;
            if (Valori.Count(x => x.Data < ConexiuneDB.DataLucr) > 0)
            {
                TotalPostat = Valori.Where(x => x.Data < ConexiuneDB.DataLucr).Sum(x => x.ValoareRON);
            }
            if (TotalPostat == 0)
            {
                TotalPostat_Proxy = "";
            }
            else
            {
                if (Math.Truncate(TotalPostat) == TotalPostat)
                {
                    TotalPostat_Proxy = TotalPostat.ToString("N0");
                }
                else
                {
                    TotalPostat_Proxy = TotalPostat.ToString("N2");
                }
            }

            TotalZi = 0;
            if (Valori.Count(x => x.Data == ConexiuneDB.DataLucr) > 0)
            {
                TotalZi = Valori.Where(x => x.Data == ConexiuneDB.DataLucr).Sum(x => x.ValoareRON);
            }
            if (TotalZi == 0)
            {
                TotalZi_Proxy = "";
            }
            else
            {
                if (Math.Truncate(TotalZi) == TotalZi)
                {
                    TotalZi_Proxy = TotalZi.ToString("N0");
                }
                else
                {
                    TotalZi_Proxy = TotalZi.ToString("N2");
                }
            }

            int nrZile = Valori.Count(x => x.Valoare != 0);

            if (EsteCazare)
            {
                Cantitate_Proxy = nrZile.ToString() + "z";
            }
            else if (EsteMasa)
            {
                Cantitate_Proxy = Cantitate.ToString() + "p x " + nrZile.ToString() + "z";
            }
            else if (EsteTaxa)
            {
                if (TaxaLaPersoane)
                {
                    Cantitate_Proxy = Cantitate.ToString() + "p x " + nrZile.ToString() + "z";
                }
                else
                {
                    Cantitate_Proxy = nrZile.ToString() + "z";
                }
            }
            else
            {
                if (nrZile > 1)
                {
                    if (Cantitate > 1)
                    {
                        Cantitate_Proxy = Cantitate.ToString() + " x " + nrZile.ToString() + "z";
                    }
                    else
                    {
                        Cantitate_Proxy = nrZile.ToString() + "z";
                    }
                }
                else
                {
                    Cantitate_Proxy = Cantitate.ToString();
                }
            }

            int cant = 0;
            if (EsteCazare)
            {
                cant = nrZile;

            }
            else if (EsteMasa)
            {
                cant = Cantitate * nrZile;
            }
            else if (EsteTaxa)
            {
                if (TaxaLaPersoane)
                {
                    cant = Cantitate * nrZile;
                }
                else
                {
                    cant = nrZile;
                }
            }
            else
            {
                if (nrZile > 1)
                {
                    if (Cantitate > 1)
                    {
                        cant = Cantitate * nrZile;
                    }
                    else
                    {
                        cant = nrZile;
                    }
                }
                else
                {
                    cant = Cantitate;
                }
            }

            if (cant > 0)
            {
                ValoareUnitara = Math.Round(Total / cant, 2);
            }
            else
            {
                ValoareUnitara = Total;
            }

            if (Valori.Count == 0)
            {
                this.ValoareMoneda = 0;
            }
            else
            {
                this.ValoareMoneda = Valori.Sum(x => x.Valoare);
            }

            if (Valori.Count == 0)
            {
                this.ValoareRon = 0;
            }
            else
            {
                this.ValoareRon = Valori.Sum(x => x.ValoareRON);
            }

            SoldMoneda = Total - PlatitMoneda;
            SoldRON = TotalRON - PlatitRon;
        }

        public string ProcentTVA_Proxy
        {
            get
            {
                if (Math.Truncate(ProcentTVA) == ProcentTVA)
                {
                    return ProcentTVA.ToString("N0") + "%";
                }
                else
                {
                    return ProcentTVA.ToString("N2") + "%";
                }
            }
        }

        private DateTime _DataStart;
        public DateTime DataStart
        {
            get
            {
                DateTime rv = DateTime.MaxValue;

                if (Valori.Count(x => x.Valoare != 0) > 0)
                {
                    rv = Valori.Where(x => x.Valoare != 0).Min(x => x.Data);
                }

                return rv;
            }
            set
            {
                _DataStart = value;
            }
        }

        public Dictionary<int, EntitateServiciuValoare> DicValori { get; set; }
        public List<EntitateServiciuValoare> Valori { get; set; }

        public EntitateServiciu()
        {
            Valori = new List<EntitateServiciuValoare>();
            CuplariMD = new List<EntitateServiciu>();
            Observatii = "";
            DenumireServiciu = "";
            ServiciiStornoAvans = new List<EntitateServiciu>();
        }

        //public FormFacturareServicii.EntitateTurist EntitateTuristFactServicii { get; set; }

        //public FormTelefoane.EntitateApel ApelTelefonic { get; set; }

        public string Camera { get; set; }

        private DateTime _DataEnd;
        public DateTime DataEnd
        {
            get
            {
                DateTime rv = DateTime.MinValue;

                if (Valori.Count(x => x.Valoare != 0) > 0)
                {
                    rv = Valori.Where(x => x.Valoare != 0).Max(x => x.Data).AddDays(1);
                }

                return rv;
            }
            set
            {
                _DataEnd = value;
            }
        }

        public string Data_Proxy//plata receptie
        {
            get
            {
                if (_DataStart == _DataEnd)
                {
                    return _DataStart.ToString("dd.MM.yyyy");
                }
                else
                {
                    return _DataStart.ToString("dd.MM.yyyy") + " - " + _DataEnd.ToString("dd.MM.yyyy");
                }
            }
        }

        public string CantitateExplicit { get; set; }//plataReceptie

        public Dictionary<int, EntitateDePlata> DePlataPerMetode = new Dictionary<int, EntitateDePlata>();

        public bool Restant { get; set; }
        public string MotivRestanta { get; set; }

        public decimal ValoareMoneda { get; set; }
        public decimal Curs { get; set; }
        public decimal ValoareRon { get; set; }

        public decimal ValoareRonCalculata
        {
            get
            {
                decimal rv = 0;
                decimal cursu = this.Curs;
                if (cursu == 0)
                {
                    cursu = 1;
                }
                foreach (EntitateServiciuValoare esv in Valori)
                {
                    rv += Math.Round(esv.Valoare * cursu, 2);
                }

                return rv;
            }
        }

        internal static EntitateServiciu GetServiciu(int p)
        {
            EntitateServiciu rv = null;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    SqlCommand cmd;
                    string sql = @"
                        SELECT
                             s.[EsteVirament]
                            ,s.[IdVenit]
                            ,s.[DenumireServiciu]
                            ,s.[IdMoneda]
                            ,s.[IdCotaTVA]
                            ,m.[MonedaCod]
                            ,c.[Procent]
                            ,v.[EsteTaxa]
                            ,v.[EsteCazare]
                            ,v.[EsteMasa]
                            ,s.Curs
                        FROM 
                            [SOLON.H].[hotel].[RezervariServicii] AS s INNER JOIN
                            [SOLON.H].[hotel].[Venituri] AS v ON v.[ID] = s.[IdVenit] INNER JOIN
                            [SOLON].[dbo].[NomMonede] AS m ON m.[IdMoneda] = s.[IdMoneda] INNER JOIN
                            [SOLON].[dbo].[CoteTva] AS c ON c.[IdCota] = s.[IdCotaTVA]
                        WHERE s.[ID] = @IdServiciu
                    ";
                    cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdServiciu", SqlDbType.Int)).Value = p;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        EntitateServiciu serv;
                        if (reader.Read())
                        {
                            serv = new EntitateServiciu()
                            {
                                EsteVirament = Convert.ToBoolean(reader["EsteVirament"]),
                                IdVenit = Convert.ToInt32(reader["IdVenit"]),
                                DenumireServiciu = reader["DenumireServiciu"].ToString(),
                                IdMoneda = Convert.ToInt32(reader["IdMoneda"]),
                                IdCotaTVA = Convert.ToInt32(reader["IdCotaTVA"]),
                                CodMoneda = reader["MonedaCod"].ToString(),
                                ProcentTVA = Convert.ToDecimal(reader["Procent"]),
                                EsteTaxa = Convert.ToBoolean(reader["EsteTaxa"]),
                                EsteCazare = Convert.ToBoolean(reader["EsteCazare"]),
                                EsteMasa = Convert.ToBoolean(reader["EsteMasa"]),
                                Curs = Convert.ToDecimal(reader["Curs"])
                            };
                            rv = serv;
                        }
                    }
                }
                catch (Exception exc)
                {}
            }
            return rv;
        }

        public int CantitateFacturaNew
        {
            get
            {
                int cant = 0;

                foreach (EntitateServiciuValoare val in Valori)
                {
                    if (val.FacturatRon != 0 && val.CantDeFact > 0.5m)
                    {
                        cant++;
                    }
                    else if (val.FacturatRon == 0 && val.CantDeFact > 0.49m)
                    {
                        cant++;
                    }
                }

                if (cant == 0)
                {
                    if (Cantitate < 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    if (EsteCazare)
                    {
                        if (Cantitate < 0 && cant < 0)
                        {
                            return -cant;
                        }
                        else
                        {
                            return cant;
                        }
                    }
                    else if (EsteMasa)
                    {
                        if (Cantitate < 0 && cant < 0)
                        {
                            return -(cant * Cantitate);
                        }
                        else
                        {
                            return cant * Cantitate;
                        }

                    }
                    else if (EsteTaxa)
                    {
                        //return cant;

                        if (Cantitate < 0 && cant < 0)
                        {
                            return -(cant * Cantitate);
                        }
                        else if (Cantitate == 0)
                        {
                            return 1 * cant;
                        }
                        else
                        {
                            return cant * Cantitate;
                        }
                    }
                    else
                    {
                        if (Cantitate < 0 && cant < 0)
                        {
                            return -(cant * Cantitate);
                        }
                        else
                        {
                            return cant * Cantitate;
                        }
                    }
                }
            }
        }

        public DateTime DataStartNew
        {
            get
            {
                DateTime rv = DateTime.MaxValue;
                foreach (EntitateServiciuValoare val in Valori)
                {
                    if (val.FacturatRon > 0 && val.CantDeFact > 0.5m)
                    {
                        if (val.Data < rv)
                        {
                            rv = val.Data;
                        }
                    }
                    else if (val.FacturatRon == 0 && val.CantDeFact > 0.49m)
                    {
                        if (val.Data < rv)
                        {
                            rv = val.Data;
                        }
                    }
                }
                if (rv == DateTime.MaxValue)
                {
                    EntitateServiciuValoare val = Valori.FirstOrDefault(x => x.ValDeFacturat != 0);
                    if (val != null)
                    {
                        rv = val.Data;
                    }
                }
                return rv;
            }
        }

        public DateTime DataEndNew
        {
            get
            {
                DateTime rv = DateTime.MinValue;
                foreach (EntitateServiciuValoare val in Valori)
                {
                    if (val.FacturatRon > 0 && val.CantDeFact > 0.5m)
                    {
                        if (val.Data > rv)
                        {
                            rv = val.Data;
                        }
                    }
                    else if (val.FacturatRon == 0 && val.CantDeFact > 0.49m)
                    {
                        if (val.Data > rv)
                        {
                            rv = val.Data;
                        }
                    }
                }
                if (rv == DateTime.MinValue)
                {
                    EntitateServiciuValoare val = Valori.LastOrDefault(x => x.ValDeFacturat != 0);
                    if (val != null)
                    {
                        rv = val.Data;
                    }
                }
                if (Valori.Count > 0 && !EsteAltceva && !EsteStorno)
                {
                    rv = rv.AddDays(1);
                }
                return rv;
            }
        }

        internal void RepartizeazaValDeFacturatOld()
        {
            foreach (EntitateServiciuValoare val in Valori)
            {
                val.ValDeFacturat = 0;
            }

            decimal valDeRepartizat = DeFactRON;

            foreach (EntitateServiciuValoare val in Valori.Where(x => (x.ValoareRON - FacturatRon) != 0))
            {
                if (valDeRepartizat == 0)
                {
                    break;
                }
                if (val.ValoareRON < 0)
                {
                    if (valDeRepartizat > (val.ValoareRON - val.FacturatRon))
                    {
                        val.ValDeFacturat = valDeRepartizat;
                        valDeRepartizat = 0;
                    }
                    else
                    {
                        val.ValDeFacturat = val.ValoareRON - val.FacturatRon;
                        valDeRepartizat -= val.ValDeFacturat;
                    }
                }
                else if (val.ValoareRON > 0)
                {
                    if (valDeRepartizat < (val.ValoareRON - val.FacturatRon))
                    {
                        val.ValDeFacturat = valDeRepartizat;
                        valDeRepartizat = 0;
                    }
                    else
                    {
                        val.ValDeFacturat = val.ValoareRON - val.FacturatRon;
                        valDeRepartizat -= val.ValDeFacturat;
                    }
                }
            }
        }

        internal void RepartizeazaValDeFacturat()
        {
            foreach (EntitateServiciuValoare val in Valori)
            {
                val.ValDeFacturat = 0;
            }

            decimal valDeRepartizat = DeFactRON;

            foreach (EntitateServiciuValoare val in Valori.OrderBy(x => x.Data))
            {
                if (valDeRepartizat == 0)
                {
                    break;
                }
                if (val.ValoareRON == val.FacturatRon)
                {
                    continue;
                }

                if (val.ValoareRON < 0)
                {
                    if (valDeRepartizat > (val.ValoareRON - val.FacturatRon))
                    {
                        val.ValDeFacturat = valDeRepartizat;
                        valDeRepartizat = 0;
                    }
                    else
                    {
                        val.ValDeFacturat = val.ValoareRON - val.FacturatRon;
                        valDeRepartizat -= val.ValDeFacturat;
                    }
                }
                else if (val.ValoareRON > 0)
                {
                    if (valDeRepartizat < (val.ValoareRON - val.FacturatRon))
                    {
                        val.ValDeFacturat = valDeRepartizat;
                        valDeRepartizat = 0;
                    }
                    else
                    {
                        val.ValDeFacturat = val.ValoareRON - val.FacturatRon;
                        valDeRepartizat -= val.ValDeFacturat;
                    }
                }
            }
        }

        public decimal CursServiciu { get; set; }

        internal EntitateServiciu Clone()
        {
            return new EntitateServiciu()
            {
                ID = 0,
                EsteVirament = EsteVirament,
                IdPartener = IdPartener,
                IdRezervareCamera = IdRezervareCamera,
                IdVenit = IdVenit,
                IdTarif = IdTarif,
                IdPlanMasa = IdPlanMasa,
                DenumireServiciu = DenumireServiciu,
                IdMoneda = IdMoneda,
                IdCotaTVA = IdCotaTVA,
                TaxaProcentuala = TaxaProcentuala,
                TaxaLaPersoane = TaxaLaPersoane,
                ValoareTaxa = ValoareTaxa,
                Observatii = Observatii,
                Sters = Sters,
                IdMotivStergere = IdMotivStergere,
                CodMoneda = CodMoneda,
                ProcentTVA = ProcentTVA,
                NumePartener = NumePartener,
                Cantitate = Cantitate,
                EsteTaxa = EsteTaxa,
                EsteCazare = EsteCazare,
                EsteMasa = EsteMasa,
                PostareAmanata = PostareAmanata,
                EsteDiscount = EsteDiscount,
                IdServiciuDiscount = IdServiciuDiscount,
                IdRezervareCameraMutat = IdRezervareCameraMutat,
                ParametriPostare = ParametriPostare,
                IdServiciuCazareTaxa = IdServiciuCazareTaxa,
                IdCategCopii = IdCategCopii,
                EsteTaxabil = EsteTaxabil,
                Restant = Restant,
                MotivRestanta = MotivRestanta,
                ValoareMoneda = ValoareMoneda,
                Curs = Curs,
                ValoareRon = ValoareRon,
                IdPlataRestituire = IdPlataRestituire,
                VersiuneSplitare = VersiuneSplitare,
                EsteStorno = EsteStorno,
                PretMoneda = PretMoneda,
                IdPret = IdPret,
                TipMasa = TipMasa,
                IdServiciuAvansLa = IdServiciuAvansLa,
                IdServiciuAvansStornat = IdServiciuAvansStornat,
                ObsFactura = ObsFactura,
                StorneazaServiciul = StorneazaServiciul
            };
        }

        internal EntitateServiciu CloneStorno()
        {
            EntitateServiciu srv = new EntitateServiciu()
            {
                ID = 0,
                EsteVirament = EsteVirament,
                IdPartener = IdPartener,
                IdRezervareCamera = IdRezervareCamera,
                IdVenit = IdVenit,
                IdTarif = IdTarif,
                IdPlanMasa = IdPlanMasa,
                DenumireServiciu = "STORNO " + DenumireServiciu,
                IdMoneda = IdMoneda,
                IdCotaTVA = IdCotaTVA,
                TaxaProcentuala = TaxaProcentuala,
                TaxaLaPersoane = TaxaLaPersoane,
                ValoareTaxa = ValoareTaxa,
                Observatii = "STORNO LA " + DenumireServiciu + " / " + perioadaServiciu,
                Sters = Sters,
                IdMotivStergere = IdMotivStergere,
                CodMoneda = CodMoneda,
                ProcentTVA = ProcentTVA,
                NumePartener = NumePartener,
                Cantitate = -Cantitate,
                EsteTaxa = EsteTaxa,
                EsteCazare = EsteCazare,
                EsteMasa = EsteMasa,
                PostareAmanata = PostareAmanata,
                EsteDiscount = EsteDiscount,
                IdServiciuDiscount = IdServiciuDiscount,
                IdRezervareCameraMutat = IdRezervareCameraMutat,
                ParametriPostare = ParametriPostare,
                IdServiciuCazareTaxa = IdServiciuCazareTaxa,
                IdCategCopii = IdCategCopii,
                EsteTaxabil = EsteTaxabil,
                Restant = Restant,
                MotivRestanta = MotivRestanta,
                ValoareMoneda = -ValoareMoneda,
                Curs = Curs,
                ValoareRon = -ValoareRon,
                IdPlataRestituire = IdPlataRestituire,
                VersiuneSplitare = VersiuneSplitare,
                EsteStorno = EsteStorno,
                PretMoneda = -PretMoneda,
                IdPret = IdPret,
                TipMasa = TipMasa,
                IdServiciuAvansLa = IdServiciuAvansLa,
                IdServiciuAvansStornat = IdServiciuAvansStornat,
                ObsFactura = ObsFactura,
                StorneazaServiciul = ID
            };

            srv.Valori = CloneValoriStorno();
            Observatii = "STORNAT LA DATA " + ConexiuneDB.DataLucr.ToString("dd.MM.yyyy") + " DE ";
            return srv;
        }

        public List<EntitateServiciuValoare> CloneValori()
        {
            List<EntitateServiciuValoare> vals = new List<EntitateServiciuValoare>();

            foreach (EntitateServiciuValoare val in Valori)
            {
                vals.Add(new EntitateServiciuValoare()
                {
                    ID = 0,
                    IdRezervareServiciu = val.IdRezervareServiciu,
                    Data = val.Data,
                    Valoare = val.Valoare,
                    Postat = val.Postat,
                    Curs = val.Curs,
                    ValoareRON = val.ValoareRON
                });
            }

            return vals;
        }

        public List<EntitateServiciuValoare> CloneValoriStorno()
        {
            List<EntitateServiciuValoare> vals = new List<EntitateServiciuValoare>();

            foreach (EntitateServiciuValoare val in Valori)
            {
                vals.Add(new EntitateServiciuValoare()
                {
                    ID = 0,
                    IdRezervareServiciu = val.IdRezervareServiciu,
                    Data = val.Data,
                    Valoare = -val.Valoare,
                    Postat = val.Postat,
                    Curs = val.Curs,
                    ValoareRON = -val.ValoareRON
                });
            }

            return vals;
        }

        internal void RepartizeazaValoareDePlata()
        {
            this.RamasDePlataMonedaPlata = this._DePlataMonedaPlata;
            this.RamasDePlataMonedaServ = this._DePlataMonedaServ;
            this.RamasDePlataRON = this._DePlataRON;

            foreach (EntitateServiciuValoare val in Valori.OrderBy(x => x.Data))
            {
                if (val.SoldPlataMonedaPlata != 0)
                {
                    if (RamasDePlataMonedaPlata > 0)
                    {
                        if (val.SoldPlataMonedaPlata > RamasDePlataMonedaPlata)
                        {
                            val.dePlataMonedaPlata = RamasDePlataMonedaPlata;
                            RamasDePlataMonedaPlata = 0;
                        }
                        else
                        {
                            val.dePlataMonedaPlata = val.SoldPlataMonedaPlata;
                            RamasDePlataMonedaPlata = RamasDePlataMonedaPlata - val.SoldPlataMonedaPlata;
                        }
                    }
                    else if (RamasDePlataMonedaPlata < 0)
                    {
                        if (val.SoldPlataMonedaPlata < RamasDePlataMonedaPlata)
                        {
                            val.dePlataMonedaPlata = RamasDePlataMonedaPlata;
                            RamasDePlataMonedaPlata = 0;
                        }
                        else
                        {
                            val.dePlataMonedaPlata = val.SoldPlataMonedaPlata;
                            RamasDePlataMonedaPlata = RamasDePlataMonedaPlata - val.SoldPlataMonedaPlata;
                        }
                    }
                    else
                    {
                        val.dePlataMonedaPlata = 0;
                        RamasDePlataMonedaPlata = 0;
                    }
                }

                if (val.SoldPlataMonedaServ != 0)
                {
                    if (RamasDePlataMonedaServ > 0)
                    {
                        if (val.SoldPlataMonedaServ > RamasDePlataMonedaServ)
                        {
                            val.dePlataMonedaServ = RamasDePlataMonedaServ;
                            RamasDePlataMonedaServ = 0;
                        }
                        else
                        {
                            val.dePlataMonedaServ = val.SoldPlataMonedaServ;
                            RamasDePlataMonedaServ = RamasDePlataMonedaServ - val.SoldPlataMonedaServ;
                        }
                    }
                    else if (RamasDePlataMonedaServ < 0)
                    {
                        if (val.SoldPlataMonedaServ < RamasDePlataMonedaServ)
                        {
                            val.dePlataMonedaServ = RamasDePlataMonedaServ;
                            RamasDePlataMonedaServ = 0;
                        }
                        else
                        {
                            val.dePlataMonedaServ = val.SoldPlataMonedaServ;
                            RamasDePlataMonedaServ = RamasDePlataMonedaServ - val.SoldPlataMonedaServ;
                        }
                    }
                    else
                    {
                        val.dePlataMonedaServ = 0;
                        RamasDePlataMonedaServ = 0;
                    }
                }

                if (val.SoldPlataRon != 0)
                {
                    if (RamasDePlataRON > 0)
                    {
                        if (val.SoldPlataRon > RamasDePlataRON)
                        {
                            val.dePlataRon = RamasDePlataRON;
                            RamasDePlataRON = 0;
                        }
                        else
                        {
                            val.dePlataRon = val.SoldPlataRon;
                            RamasDePlataRON = RamasDePlataRON - val.SoldPlataRon;
                        }
                    }
                    else if (RamasDePlataRON < 0)
                    {
                        if (val.SoldPlataRon < RamasDePlataRON)
                        {
                            val.dePlataRon = RamasDePlataRON;
                            RamasDePlataRON = 0;
                        }
                        else
                        {
                            val.dePlataRon = val.SoldPlataRon;
                            RamasDePlataRON = RamasDePlataRON - val.SoldPlataRon;
                        }
                    }
                    else
                    {
                        val.dePlataRon = 0;
                        RamasDePlataRON = 0;
                    }
                }
            }
        }

        public bool GrupeazaPeBon { get; set; }
        public string DenumirePlanMasa { get; set; }
        public string DenumireTarif { get; set; }
        public string DenumireVenit { get; set; }
        public string UnitateMasura { get; set; }

        public bool LaPersoana
        {
            get
            {
                if (TaxaLaPersoane)
                {
                    return true;
                }
                if (EsteMasa)
                {
                    return true;
                }

                return false;
            }
        }
        public bool Zilnica
        {
            get
            {
                return Valori.Count > 1;
            }
        }

        public bool EsteProdus { get; set; }
        public int NrAdulti { get; set; }
        public string CodCamera { get; set; }

        public int IdUtilizatorPostare { get; set; }

        public int IdCamera { get; set; }
    }


}