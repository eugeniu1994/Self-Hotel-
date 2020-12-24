using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class Facturi
    {
        public int ID { get; set; }
        public int IdHotel { get; set; }
        public int IdFurnizor { get; set; }
        public int IdPunctLucru { get; set; }
        public int IdClient { get; set; }
        public string SediuClient { get; set; }
        public DateTime Data { get; set; }
        public string Serie { get; set; }
        public string Numar { get; set; }
        public int IdMoneda { get; set; }
        public decimal CursZi { get; set; }
        public string ExpDelegat { get; set; }
        public string ExpAct { get; set; }
        public string ExpTransport { get; set; }
        public DateTime ExpData { get; set; }
        public string Aviz { get; set; }
        public DateTime DataScadenta { get; set; }
        public decimal TotalValoareMoneda { get; set; }
        public decimal TotalTVAMoneda { get; set; }
        public decimal TotalGeneralMoneda { get; set; }
        public decimal TotalValoareRON { get; set; }
        public decimal TotalTVARON { get; set; }
        public decimal TotalGeneralRON { get; set; }
        public string ObsAntet { get; set; }
        public string ObsSubsol { get; set; }
        public string Intocmita { get; set; }
        public Boolean Emisa { get; set; }
        public int IdUserEmis { get; set; }
        public DateTime DataOraEmisa { get; set; }
        public Boolean Anulata { get; set; }
        public int IdUserAnulata { get; set; }
        public DateTime DataOraAnulata { get; set; }
        public DateTime DataOraCreare { get; set; }
        public DateTime DataOraModificare { get; set; }
        public Boolean CuTVAI { get; set; }
        public decimal TVAI_ExigibilBaza { get; set; }
        public decimal TVAI_ExigibilTVA { get; set; }
        public decimal TVAI_LaIncasareBaza { get; set; }
        public decimal TVAI_LaIncasareTVA { get; set; }
        public int IdRezervare { get; set; }
        public int IdCarnet { get; set; }
        public int StornoLaFactura { get; set; }
        public int IdChitanta { get; set; }
        public Boolean TaxareInversa { get; set; }
        public Boolean FolosesteCursuriEURUSD { get; set; }
        public decimal CursEUR { get; set; }
        public decimal CursUSD { get; set; }
        public Boolean Preluat { get; set; }

        public static List<Facturi> GetLista()
        {
            List<Facturi> rv = new List<Facturi>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT
                                         [ID]
                                        ,[IdHotel]
                                        ,[IdFurnizor]
                                        ,[IdPunctLucru]
                                        ,[IdClient]
                                        ,[SediuClient]
                                        ,[Data]
                                        ,[Serie]
                                        ,[Numar]
                                        ,[IdMoneda]
                                        ,[CursZi]
                                        ,[ExpDelegat]
                                        ,[ExpAct]
                                        ,[ExpTransport]
                                        ,[ExpData]
                                        ,[Aviz]
                                        ,[DataScadenta]
                                        ,[TotalValoareMoneda]
                                        ,[TotalTVAMoneda]
                                        ,[TotalGeneralMoneda]
                                        ,[TotalValoareRON]
                                        ,[TotalTVARON]
                                        ,[TotalGeneralRON]
                                        ,[ObsAntet]
                                        ,[ObsSubsol]
                                        ,[Intocmita]
                                        ,[Emisa]
                                        ,[IdUserEmis]
                                        ,[DataOraEmisa]
                                        ,[Anulata]
                                        ,[IdUserAnulata]
                                        ,[DataOraAnulata]
                                        ,[DataOraCreare]
                                        ,[DataOraModificare]
                                        ,[CuTVAI]
                                        ,[TVAI_ExigibilBaza]
                                        ,[TVAI_ExigibilTVA]
                                        ,[TVAI_LaIncasareBaza]
                                        ,[TVAI_LaIncasareTVA]
                                        ,[IdRezervare]
                                        ,[IdCarnet]
                                        ,[StornoLaFactura]
                                        ,[IdChitanta]
                                        ,[TaxareInversa]
                                        ,[FolosesteCursuriEURUSD]
                                        ,[CursEUR]
                                        ,[CursUSD]
                                        ,[Preluat]
                                    FROM [SOLON.H].[financiar].[Facturi]";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    //cmd.Parameters.Add(new SqlParameter("@CodRezervare", SqlDbType.VarChar)).Value = CodRezervare;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Facturi inst = new Facturi();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.IdHotel = Convert.ToInt32(reader["IdHotel"]);
                            inst.IdFurnizor = Convert.ToInt32(reader["IdFurnizor"]);
                            inst.IdPunctLucru = reader["IdPunctLucru"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdPunctLucru"]);
                            inst.IdClient = Convert.ToInt32(reader["IdClient"]);
                            inst.SediuClient = reader["SediuClient"] == DBNull.Value ? "" : reader["SediuClient"].ToString();
                            inst.Data = Convert.ToDateTime(reader["Data"]);
                            inst.Serie = reader["Serie"].ToString();
                            inst.Numar = reader["Numar"].ToString();
                            inst.IdMoneda = Convert.ToInt32(reader["IdMoneda"]);
                            inst.CursZi = Convert.ToDecimal(reader["CursZi"]);
                            inst.ExpDelegat = reader["ExpDelegat"] == DBNull.Value ? "" : reader["ExpDelegat"].ToString();
                            inst.ExpAct = reader["ExpAct"] == DBNull.Value ? "" : reader["ExpAct"].ToString();
                            inst.ExpTransport = reader["ExpTransport"] == DBNull.Value ? "" : reader["ExpTransport"].ToString();
                            inst.ExpData = reader["ExpData"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["ExpData"]);
                            inst.Aviz = reader["Aviz"] == DBNull.Value ? "" : reader["Aviz"].ToString();
                            inst.DataScadenta = reader["DataScadenta"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataScadenta"]);
                            inst.TotalValoareMoneda = Convert.ToDecimal(reader["TotalValoareMoneda"]);
                            inst.TotalTVAMoneda = Convert.ToDecimal(reader["TotalTVAMoneda"]);
                            inst.TotalGeneralMoneda = Convert.ToDecimal(reader["TotalGeneralMoneda"]);
                            inst.TotalValoareRON = Convert.ToDecimal(reader["TotalValoareRON"]);
                            inst.TotalTVARON = Convert.ToDecimal(reader["TotalTVARON"]);
                            inst.TotalGeneralRON = Convert.ToDecimal(reader["TotalGeneralRON"]);
                            inst.ObsAntet = reader["ObsAntet"] == DBNull.Value ? "" : reader["ObsAntet"].ToString();
                            inst.ObsSubsol = reader["ObsSubsol"] == DBNull.Value ? "" : reader["ObsSubsol"].ToString();
                            inst.Intocmita = reader["Intocmita"].ToString();
                            inst.Emisa = Convert.ToBoolean(reader["Emisa"]);
                            inst.IdUserEmis = reader["IdUserEmis"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdUserEmis"]);
                            inst.DataOraEmisa = reader["DataOraEmisa"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataOraEmisa"]);
                            inst.Anulata = Convert.ToBoolean(reader["Anulata"]);
                            inst.IdUserAnulata = reader["IdUserAnulata"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdUserAnulata"]);
                            inst.DataOraAnulata = reader["DataOraAnulata"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataOraAnulata"]);
                            inst.DataOraCreare = Convert.ToDateTime(reader["DataOraCreare"]);
                            inst.DataOraModificare = Convert.ToDateTime(reader["DataOraModificare"]);
                            inst.CuTVAI = Convert.ToBoolean(reader["CuTVAI"]);
                            inst.TVAI_ExigibilBaza = Convert.ToDecimal(reader["TVAI_ExigibilBaza"]);
                            inst.TVAI_ExigibilTVA = Convert.ToDecimal(reader["TVAI_ExigibilTVA"]);
                            inst.TVAI_LaIncasareBaza = Convert.ToDecimal(reader["TVAI_LaIncasareBaza"]);
                            inst.TVAI_LaIncasareTVA = Convert.ToDecimal(reader["TVAI_LaIncasareTVA"]);
                            inst.IdRezervare = Convert.ToInt32(reader["IdRezervare"]);
                            inst.IdCarnet = Convert.ToInt32(reader["IdCarnet"]);
                            inst.StornoLaFactura = Convert.ToInt32(reader["StornoLaFactura"]);
                            inst.IdChitanta = Convert.ToInt32(reader["IdChitanta"]);
                            inst.TaxareInversa = Convert.ToBoolean(reader["TaxareInversa"]);
                            inst.FolosesteCursuriEURUSD = Convert.ToBoolean(reader["FolosesteCursuriEURUSD"]);
                            inst.CursEUR = Convert.ToDecimal(reader["CursEUR"]);
                            inst.CursUSD = Convert.ToDecimal(reader["CursUSD"]);
                            inst.Preluat = Convert.ToBoolean(reader["Preluat"]);

                            rv.Add(inst);
                        }
                    }
                }
                catch (Exception exc)
                {
                    return null;
                }
            }
            return rv;
        }

        public static Facturi getLastNumar(int IdHotel, int IdCarnet)
        {
            Facturi rv = new Facturi();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT top 1 [ID]
                                          ,[IdHotel]
                                          ,[IdFurnizor]
                                          ,[IdPunctLucru]
                                          ,[IdClient]
                                          ,[SediuClient]
                                          ,[Data]
                                          ,[Serie]
                                          ,[Numar]
                                          ,[IdMoneda]
                                          ,[CursZi]
                                          ,[ExpDelegat]
                                          ,[ExpAct]
                                          ,[ExpTransport]
                                          ,[ExpData]
                                          ,[Aviz]
                                          ,[DataScadenta]
                                          ,[TotalValoareMoneda]
                                          ,[TotalTVAMoneda]
                                          ,[TotalGeneralMoneda]
                                          ,[TotalValoareRON]
                                          ,[TotalTVARON]
                                          ,[TotalGeneralRON]
                                          ,[ObsAntet]
                                          ,[ObsSubsol]
                                          ,[Intocmita]
                                          ,[Emisa]
                                          ,[IdUserEmis]
                                          ,[DataOraEmisa]
                                          ,[Anulata]
                                          ,[IdUserAnulata]
                                          ,[DataOraAnulata]
                                          ,[DataOraCreare]
                                          ,[DataOraModificare]
                                          ,[CuTVAI]
                                          ,[TVAI_ExigibilBaza]
                                          ,[TVAI_ExigibilTVA]
                                          ,[TVAI_LaIncasareBaza]
                                          ,[TVAI_LaIncasareTVA]
                                          ,[IdRezervare]
                                          ,[IdCarnet]
                                          ,[StornoLaFactura]
                                          ,[IdChitanta]
                                          ,[TaxareInversa]
                                          ,[FolosesteCursuriEURUSD]
                                          ,[CursEUR]
                                          ,[CursUSD]
                                          ,[Preluat]
                                      FROM [SOLON.H].[financiar].[Facturi]
                                      WHERE IdCarnet = @IdCarnet and IdHotel = @IdHotel
                                      ORDER BY ID DESC";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdCarnet", SqlDbType.BigInt)).Value = IdCarnet;
                    cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.BigInt)).Value = IdHotel;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Facturi inst = new Facturi();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.IdHotel = Convert.ToInt32(reader["IdHotel"]);
                            inst.IdFurnizor = Convert.ToInt32(reader["IdFurnizor"]);
                            inst.IdPunctLucru = reader["IdPunctLucru"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdPunctLucru"]);
                            inst.IdClient = Convert.ToInt32(reader["IdClient"]);
                            inst.SediuClient = reader["SediuClient"] == DBNull.Value ? "" : reader["SediuClient"].ToString();
                            inst.Data = Convert.ToDateTime(reader["Data"]);
                            inst.Serie = reader["Serie"].ToString();
                            inst.Numar = reader["Numar"].ToString();
                            inst.IdMoneda = Convert.ToInt32(reader["IdMoneda"]);
                            inst.CursZi = Convert.ToDecimal(reader["CursZi"]);
                            inst.ExpDelegat = reader["ExpDelegat"] == DBNull.Value ? "" : reader["ExpDelegat"].ToString();
                            inst.ExpAct = reader["ExpAct"] == DBNull.Value ? "" : reader["ExpAct"].ToString();
                            inst.ExpTransport = reader["ExpTransport"] == DBNull.Value ? "" : reader["ExpTransport"].ToString();
                            inst.ExpData = reader["ExpData"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["ExpData"]);
                            inst.Aviz = reader["Aviz"] == DBNull.Value ? "" : reader["Aviz"].ToString();
                            inst.DataScadenta = reader["DataScadenta"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataScadenta"]);
                            inst.TotalValoareMoneda = Convert.ToDecimal(reader["TotalValoareMoneda"]);
                            inst.TotalTVAMoneda = Convert.ToDecimal(reader["TotalTVAMoneda"]);
                            inst.TotalGeneralMoneda = Convert.ToDecimal(reader["TotalGeneralMoneda"]);
                            inst.TotalValoareRON = Convert.ToDecimal(reader["TotalValoareRON"]);
                            inst.TotalTVARON = Convert.ToDecimal(reader["TotalTVARON"]);
                            inst.TotalGeneralRON = Convert.ToDecimal(reader["TotalGeneralRON"]);
                            inst.ObsAntet = reader["ObsAntet"] == DBNull.Value ? "" : reader["ObsAntet"].ToString();
                            inst.ObsSubsol = reader["ObsSubsol"] == DBNull.Value ? "" : reader["ObsSubsol"].ToString();
                            inst.Intocmita = reader["Intocmita"].ToString();
                            inst.Emisa = Convert.ToBoolean(reader["Emisa"]);
                            inst.IdUserEmis = reader["IdUserEmis"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdUserEmis"]);
                            inst.DataOraEmisa = reader["DataOraEmisa"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataOraEmisa"]);
                            inst.Anulata = Convert.ToBoolean(reader["Anulata"]);
                            inst.IdUserAnulata = reader["IdUserAnulata"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdUserAnulata"]);
                            inst.DataOraAnulata = reader["DataOraAnulata"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataOraAnulata"]);
                            inst.DataOraCreare = Convert.ToDateTime(reader["DataOraCreare"]);
                            inst.DataOraModificare = Convert.ToDateTime(reader["DataOraModificare"]);
                            inst.CuTVAI = Convert.ToBoolean(reader["CuTVAI"]);
                            inst.TVAI_ExigibilBaza = Convert.ToDecimal(reader["TVAI_ExigibilBaza"]);
                            inst.TVAI_ExigibilTVA = Convert.ToDecimal(reader["TVAI_ExigibilTVA"]);
                            inst.TVAI_LaIncasareBaza = Convert.ToDecimal(reader["TVAI_LaIncasareBaza"]);
                            inst.TVAI_LaIncasareTVA = Convert.ToDecimal(reader["TVAI_LaIncasareTVA"]);
                            inst.IdRezervare = Convert.ToInt32(reader["IdRezervare"]);
                            inst.IdCarnet = Convert.ToInt32(reader["IdCarnet"]);
                            inst.StornoLaFactura = Convert.ToInt32(reader["StornoLaFactura"]);
                            inst.IdChitanta = Convert.ToInt32(reader["IdChitanta"]);
                            inst.TaxareInversa = Convert.ToBoolean(reader["TaxareInversa"]);
                            inst.FolosesteCursuriEURUSD = Convert.ToBoolean(reader["FolosesteCursuriEURUSD"]);
                            inst.CursEUR = Convert.ToDecimal(reader["CursEUR"]);
                            inst.CursUSD = Convert.ToDecimal(reader["CursUSD"]);
                            inst.Preluat = Convert.ToBoolean(reader["Preluat"]);

                            rv=inst;
                        }
                    }
                }
                catch (Exception exc)
                {
                    return null;
                }
            }
            return rv;
        }
    }
}