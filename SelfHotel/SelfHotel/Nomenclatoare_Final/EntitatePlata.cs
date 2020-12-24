using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitatePlata
    {
        public int ID { get; set; }
        public int IdHotel { get; set; }
        public int IdTipDocPlata { get; set; }
        public int NumarIntern { get; set; }
        public string DocNr { get; set; }
        public string DocSeria { get; set; }
        public DateTime DocData { get; set; }
        public Int64 IdPartener { get; set; }
        public int IdMoneda { get; set; }
        public Decimal CursZi { get; set; }
        public Decimal ValoareMoneda { get; set; }
        public Decimal ValoareRON { get; set; }

        public string ValoareMoneda_Proxy
        {
            get
            {
                if (EsteLinieZ)
                {
                    return "";
                }
                else
                {
                    return ValoareMoneda.ToString("0.00");
                }
            }
        }

        public DateTime DataLucru { get; set; }
        public string DataLucru_Proxy
        {
            get
            {
                if (DataLucru == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return DataLucru.ToString("dd.MM.yyyy");
                }
            }
        }
        public DateTime DataOraCreare { get; set; }
        public int IdUser { get; set; }
        public String Explic { get; set; }
        public bool Sters { get; set; }
        public int IdRezervare { get; set; }        // folosit ca identificare si legatura revalidare doar pentru platile de avans

        public bool EsteFiscal { get; set; }

        public int IdBacsis { get; set; }
        public String Bacsis_Proxy
        {
            get
            {
                if (IdBacsis == 0)
                {
                    return "";
                }
                else
                {
                    return "DA";
                }
            }
        }

        public bool EsteDispozitie { get; set; }
        public bool EsteChitanta { get; set; } //folosti in plati pentru emite copie
        public string CodFiscalNumarPartener { get; set; } //folosti in plati pentru emite copie
        public string CodFiscalAtributPartener { get; set; } //folosti in plati pentru emite copie
        public string RegComPartener { get; set; } //folosti in plati pentru emite copie
        public string OrasPartener { get; set; } //folosti in plati pentru emite copie
        public string StradaPartener { get; set; } //folosti in plati pentru emite copie
        public string NrPartener { get; set; } //folosti in plati pentru emite copie
        public string CUIPartener//folosti in plati pentru emite copie
        {
            get
            {
                return CodFiscalAtributPartener + CodFiscalNumarPartener;
            }
        }

        public string AdresaPartener//folosti in plati pentru emite copie
        {
            get
            {
                string oras = string.IsNullOrEmpty(OrasPartener) ? "" : OrasPartener + ", ";
                string strada = string.IsNullOrEmpty(StradaPartener) ? "" : StradaPartener + ", ";
                string nr = string.IsNullOrEmpty(NrPartener) ? "" : NrPartener + ", ";
                return oras + strada + nr;
            }
        }

        public int CCTip { get; set; }
        public string CCPosesor { get; set; }
        public string CCUltimeleCifre { get; set; }

        public string CodTipDoc { get; set; }
        public string CodMoneda { get; set; }
        public String NumePartener { get; set; }
        public String User { get; set; }

        public List<EntitatePlataServiciu> PlatiServicii { get; private set; }
        public List<EntitatePlataMetodaPlata> PlatiMetode { get; private set; }

        public bool Select { get; set; } //folosit la selectare plati multiple pt facturare (Form Agentie)

        public string Document_Proxy
        {
            get
            {
                StringBuilder sb = new StringBuilder(CodTipDoc);

                if (EsteLinieZ)
                {
                    sb.Append("Emitere raport Z");
                }
                else
                {
                    if (!string.IsNullOrEmpty(DocSeria))
                    {
                        sb.AppendFormat(" {0}", DocSeria);
                        if (!string.IsNullOrEmpty(DocNr))
                        {
                            sb.AppendFormat(" {0}", DocNr);
                        }

                        if (NumarIntern != 0)
                        {
                            sb.AppendFormat(" (#{0})", NumarIntern.ToString());
                        }
                        if (DocData > DateTime.MinValue)
                        {
                            sb.AppendFormat(" / {0}", DocData.ToString("dd.MM.yyyy"));
                        }
                    }
                    else
                    {
                        sb.AppendFormat(" {0} / {1}", NumarIntern, DataLucru.ToString("dd.MM.yyyy"));
                    }
                }

                return sb.ToString();
            }
        }

        public String Anulat_Proxy
        {
            get
            {
                if (Sters)
                {
                    return "DA";
                }
                else
                {
                    return "";
                }
            }
        }

        public string Avans_Proxy
        {
            get
            {
                string rv = "";
                if (IdRezervare != 0)
                {
                    rv = "DA";
                    if (IdFacturaRezAvans != 0)
                    {
                        rv += "(F)";
                    }
                }
                else
                {
                    rv = "";
                }
                return rv;
            }
        }

        public int IdFacturaRezAvans { get; set; }

        public bool EsteDeAvans
        {
            get
            {
                return IdRezervare != 0;
            }
        }

        public EntitatePlata()
        {
            PlatiServicii = new List<EntitatePlataServiciu>();
            PlatiMetode = new List<EntitatePlataMetodaPlata>();
        }

        internal static int GetIdNumerar()
        {
            int rv = 0;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    SqlCommand cmd;
                    string sql = @"
                                SELECT TOP 1 [ID]
                                  FROM [SOLON.H].[financiar].[MetodeDePlata]
                                WHERE EsteNumerar = 1 and Sters = 0
                    ";
                    cmd = new SqlCommand(sql, cnn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            rv = reader["ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID"]);
                        }
                    }
                }
                catch (Exception exc)
                {
                }
            }
            return rv;
        }

        internal static int GetIdCardCredit()
        {
            int rv = 0;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    SqlCommand cmd;
                    string sql = @"
                                SELECT TOP 1 [ID]
                                  FROM [SOLON.H].[financiar].[MetodeDePlata]
                                WHERE EsteCardBancar = 1 and Sters = 0";
                    cmd = new SqlCommand(sql, cnn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            rv = reader["ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID"]);
                        }
                    }
                }
                catch (Exception exc)
                {
                }
            }
            return rv;
        }

        public bool EsteLinieZ { get; set; }
    }

}