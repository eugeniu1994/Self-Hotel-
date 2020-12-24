using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class RezervariCamere
    {
        public int ID { get; set; }
        public int IdRezervare { get; set; }
        public DateTime Sosire { get; set; }
        public String SosireStr { get; set; }
        public int NrNopti { get; set; }
        public DateTime Plecare { get; set; }
        public String PlecareStr { get; set; }
        public DateTime OraI { get; set; }
        public DateTime OraE { get; set; }
        public int IdCamera { get; set; }
        public int IdTipCamera { get; set; }
        public int NrAdulti { get; set; }
        public int NrPaturiSuplim { get; set; }
        public int IdTurist { get; set; }
        public int IdTarif { get; set; }
        public Boolean EsteTarifFix { get; set; }
        public decimal TarifFixNormal { get; set; }
        public decimal TarifFixWeekend { get; set; }
        public int IdPlanMasa { get; set; }
        public Boolean TotiLaMasa { get; set; }
        public int NrAdultiMasa { get; set; }
        public Boolean IntraLaMasaDejun { get; set; }
        public Boolean PlataLaReceptie { get; set; }
        public Boolean AcceptaPartaj { get; set; }
        public int IdTipDoc { get; set; }
        public string DocNr { get; set; }
        public DateTime DocData { get; set; }
        public Boolean Cazat { get; set; }
        public Boolean Sters { get; set; }
        public int IdMotivStergere { get; set; }
        public long IdSursaRezervare { get; set; }
        public int IdSegPiata { get; set; }
        public string Observatii { get; set; }
        public string CardTurist { get; set; }
        public int Culoare { get; set; }
        public int IdUtilizator { get; set; }
        public decimal SoldRec { get; set; }
        public decimal SoldVir { get; set; }
        public Boolean Iesit { get; set; }
        public int IdHotel { get; set; }
        public Boolean EsteYP { get; set; }
        public int NrCopii { get; set; }
        public string CodBratara { get; set; }
        public int IdRezCamYP { get; set; }
        public int IdRezervareOld { get; set; }
        public Boolean AtentionareYPDiferentaTotal { get; set; }
        public int IdMotivScutireTaxa { get; set; }
        public string ObsScutireTaxa { get; set; }
        public int IdTarifSursa { get; set; }
        public decimal TarifFixNormalSursa { get; set; }
        public decimal TarifFixWeekendSursa { get; set; }
        public Boolean EstePensionar { get; set; }
        public Boolean EsteNoshow { get; set; }
        public int idTipBanquet { get; set; }
        public int idModAranjare { get; set; }
        public string CaliromReservationId { get; set; }
        public Boolean EsteOB { get; set; }
        public Boolean CameraBlocata { get; set; }
        public int IdSerie { get; set; }
        public int IdCalitate { get; set; }
        public int IdJudet { get; set; }
        public Boolean BiletContributie { get; set; }
        public Boolean ScutitTVA { get; set; }
        public decimal Contributie { get; set; }
        public DateTime DataPrezentarii { get; set; }
        public DateTime DataPlecarii { get; set; }
        public string CaliromServerIP { get; set; }
        public Boolean DinAlocari { get; set; }
        public string Eveniment { get; set; }
        public string IntraLaMasaCu { get; set; }
        public Boolean NecesitaUpdateCalirom { get; set; }
        public DateTime UltimulUpdateCalirom { get; set; }
        public string Cod { get; set; }
        public string Denumire { get; set; }
        public NomParteneri turist { get; set; }

        public List<RezervariServicii> listaServicii { get; set; }
        public List<EntitateServiciu> entitateServiciiLista { get; set; }

        public static List<RezervariCamere> GetLista(int idRezervare)
        {
            List<RezervariCamere> rv = new List<RezervariCamere>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT  rc.[ID]
                                          ,[IdRezervare]
                                          ,[Sosire]
                                          ,[NrNopti]
                                          ,[Plecare]
                                          ,[OraI]
                                          ,[OraE]
                                          ,[IdCamera]
                                          ,[IdTipCamera]
                                          ,[NrAdulti]
                                          ,[NrPaturiSuplim]
                                          ,[IdTurist]
                                          ,[IdTarif]
                                          ,[EsteTarifFix]
                                          ,[TarifFixNormal]
                                          ,[TarifFixWeekend]
                                          ,[Cazat]
                                          ,[SoldRec]
                                          ,[SoldVir]
                                          ,[IdHotel]
                                          ,[NrCopii]
                                          ,[DataPrezentarii]
                                          ,[DataPlecarii]
                                          ,tc.Cod
                                          ,tc.Denumire
                                          ,rc.Iesit
                                      FROM [SOLON.H].[hotel].[RezervariCamere] as rc left outer join [SOLON.H].[hotel].[TipCamera] as tc on tc.ID=rc.IdTipCamera
                                      WHERE rc.Sters=0 and tc.Sters=0 and tc.Virtuala=0 and tc.Suplimentara=0 and IdRezervare=@IdRezervare;";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdRezervare", SqlDbType.BigInt)).Value = idRezervare;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RezervariCamere inst = new RezervariCamere();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.IdRezervare = Convert.ToInt32(reader["IdRezervare"]);
                            inst.Sosire = Convert.ToDateTime(reader["Sosire"]);
                            inst.SosireStr = inst.Sosire.ToShortDateString();
                            inst.NrNopti = Convert.ToInt32(reader["NrNopti"]);
                            inst.Plecare = Convert.ToDateTime(reader["Plecare"]);
                            inst.PlecareStr = inst.Plecare.ToShortDateString();
                            inst.OraI = Convert.ToDateTime(reader["OraI"]);
                            inst.OraE = Convert.ToDateTime(reader["OraE"]);
                            inst.IdCamera = reader["IdCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdCamera"]);
                            inst.IdTipCamera = reader["IdTipCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdTipCamera"]);
                            inst.NrAdulti = Convert.ToInt32(reader["NrAdulti"]);
                            inst.NrPaturiSuplim = Convert.ToInt32(reader["NrPaturiSuplim"]);
                            inst.IdTurist = reader["IdTurist"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdTurist"]);
                            inst.IdTarif = reader["IdTarif"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdTarif"]);
                            inst.EsteTarifFix = Convert.ToBoolean(reader["EsteTarifFix"]);
                            inst.TarifFixNormal = Convert.ToDecimal(reader["TarifFixNormal"]);
                            inst.TarifFixWeekend = Convert.ToDecimal(reader["TarifFixWeekend"]);
                            inst.Cazat = Convert.ToBoolean(reader["Cazat"]);
                            inst.SoldRec = Convert.ToDecimal(reader["SoldRec"]);
                            inst.SoldVir = Convert.ToDecimal(reader["SoldVir"]);
                            inst.IdHotel = Convert.ToInt32(reader["IdHotel"]);
                            inst.NrCopii = Convert.ToInt32(reader["NrCopii"]);
                            inst.DataPrezentarii = reader["DataPrezentarii"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataPrezentarii"]);
                            inst.DataPlecarii = reader["DataPlecarii"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataPlecarii"]);
                            inst.Cod = reader["Cod"] == DBNull.Value ? "" : reader["Cod"].ToString();
                            inst.Denumire = reader["Denumire"] == DBNull.Value ? "" : reader["Denumire"].ToString();
                            inst.Iesit = reader["Iesit"] == DBNull.Value ? false : Convert.ToBoolean(reader["Iesit"]);
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
    }
}