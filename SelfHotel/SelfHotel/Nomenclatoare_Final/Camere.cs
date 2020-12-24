using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class Camere
    {
        public int ID { get; set; }
        public int IdHotel { get; set; }
        public int IdCorp { get; set; }
        public int IdNivel { get; set; }
        public int IdTipCamera { get; set; }
        public string Cod { get; set; }
        public string Denumire { get; set; }
        public Boolean Virtuala { get; set; }
        public Boolean Ascunsa { get; set; }
        public Boolean Curata { get; set; }
        public Boolean Suplimentara { get; set; }
        public int Ordine { get; set; }
        public int SchimbLenjerie { get; set; }
        public int VzLatime { get; set; }
        public int VzInaltime { get; set; }
        public int VzX { get; set; }
        public int VzY { get; set; }
        public int ReselFlags { get; set; }
        public int ReselEconomizor { get; set; }
        public int TipUtilizator { get; set; }
        public decimal ReselTemperatura { get; set; }
        public int ReselCodCameraV2 { get; set; }
        public string AdelCodCamera { get; set; }
        public int ReselAccesTip { get; set; }
        public DateTime ReselAccesData { get; set; }
        public string CodTelefon { get; set; }
        public decimal ReselLastConfortTemp { get; set; }
        public string ReselCodTempPreset { get; set; }
        public Boolean ReselForteazaECNF { get; set; }
        public Boolean ReselAutoECNF { get; set; }
        public DateTime ReselDataInchidYalaMD { get; set; }
        public int ReselAccesariEcCamerista { get; set; }
        public DateTime ReselDataExpirareEcCamerista { get; set; }
        public DateTime ReselDataDeblocareEcCamerista { get; set; }
        public DateTime ReselDataExpirareEcApp { get; set; }
        public Boolean Banquet { get; set; }
        public string CaliromRoomId { get; set; }
        public string CaliromRoomNumber { get; set; }
        public Boolean ReselMentineValvaApaBaieInchisa { get; set; }
        public DateTime ReselDataOraIncepereEcoFD { get; set; }
        public Boolean ReselEcoFD { get; set; }
        public decimal ReselTemperaturaFREC { get; set; }
        public Boolean ReselForteazaFREC { get; set; }
        public string CodGoodumLocks { get; set; }
        public int ReselModTermostat { get; set; }
        public Boolean ReselInchideTermostatLaEco { get; set; }
        public int ReselInterfonIndex { get; set; }
        public int ReselInterfonCodApartament { get; set; }
        public Boolean TrimiteTemperaturaCONF { get; set; }
        public String DenumireCamera { get; set; }

        public static List<Camere> getLista()
        {
            List<Camere> rv = new List<Camere>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {

                    cnn.Open();
                    string sql = @"SELECT c.[ID]
                                          ,[IdHotel]
                                          ,[IdCorp]
                                          ,[IdNivel]
                                          ,[IdTipCamera]
                                          ,c.[Cod]
                                          ,c.[Denumire]
                                          ,c.[Virtuala]
                                          ,[Ascunsa]
                                          ,[Curata]
                                          ,c.[Suplimentara]
                                          ,c.[Ordine]
                                          ,[SchimbLenjerie]
                                          ,[VzLatime]
                                          ,[VzInaltime]
                                          ,[VzX]
                                          ,[VzY]
                                          ,[ReselFlags]
                                          ,[ReselEconomizor]
                                          ,[TipUtilizator]
                                          ,[ReselTemperatura]
                                          ,[ReselCodCameraV2]
                                          ,[AdelCodCamera]
                                          ,[ReselAccesTip]
                                          ,[ReselAccesData]
                                          ,[CodTelefon]
                                          ,[ReselLastConfortTemp]
                                          ,[ReselCodTempPreset]
                                          ,[ReselForteazaECNF]
                                          ,[ReselAutoECNF]
                                          ,[ReselDataInchidYalaMD]
                                          ,[ReselAccesariEcCamerista]
                                          ,[ReselDataExpirareEcCamerista]
                                          ,[ReselDataDeblocareEcCamerista]
                                          ,[ReselDataExpirareEcApp]
                                          ,[Banquet]
                                          ,[CaliromRoomId]
                                          ,[CaliromRoomNumber]
                                          ,[ReselMentineValvaApaBaieInchisa]
                                          ,[ReselDataOraIncepereEcoFD]
                                          ,[ReselEcoFD]
                                          ,[ReselTemperaturaFREC]
                                          ,[ReselForteazaFREC]
                                          ,[CodGoodumLocks]
                                          ,[ReselModTermostat]
                                          ,[ReselInchideTermostatLaEco]
                                          ,[ReselInterfonIndex]
                                          ,[ReselInterfonCodApartament]
                                          ,[TrimiteTemperaturaCONF]
                                          ,tc.Denumire as DenumireCamera
                                      FROM [SOLON.H].[hotel].[Camere] as c left outer join [SOLON.H].[hotel].[TipCamera] as tc on tc.ID = c.IdTipCamera
                                      WHERE tc.Sters=0 and tc.Virtuala=0 and tc.Suplimentara=0";

                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Camere inst = new Camere();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.IdHotel = Convert.ToInt32(reader["IdHotel"]);
                            inst.IdCorp = Convert.ToInt32(reader["IdCorp"]);
                            inst.IdNivel = Convert.ToInt32(reader["IdNivel"]);
                            inst.IdTipCamera = Convert.ToInt32(reader["IdTipCamera"]);
                            inst.Cod = reader["Cod"].ToString();
                            inst.Denumire = reader["Denumire"].ToString();
                            inst.Virtuala = Convert.ToBoolean(reader["Virtuala"]);
                            inst.Ascunsa = Convert.ToBoolean(reader["Ascunsa"]);
                            inst.Curata = Convert.ToBoolean(reader["Curata"]);
                            inst.Suplimentara = Convert.ToBoolean(reader["Suplimentara"]);
                            inst.Ordine = Convert.ToInt32(reader["Ordine"]);
                            inst.SchimbLenjerie = Convert.ToInt32(reader["SchimbLenjerie"]);
                            inst.VzLatime = Convert.ToInt32(reader["VzLatime"]);
                            inst.VzInaltime = Convert.ToInt32(reader["VzInaltime"]);
                            inst.VzX = Convert.ToInt32(reader["VzX"]);
                            inst.VzY = Convert.ToInt32(reader["VzY"]);
                            inst.ReselFlags = Convert.ToInt32(reader["ReselFlags"]);
                            inst.ReselEconomizor = Convert.ToInt32(reader["ReselEconomizor"]);
                            inst.TipUtilizator = Convert.ToInt32(reader["TipUtilizator"]);
                            inst.ReselTemperatura = Convert.ToDecimal(reader["ReselTemperatura"]);
                            inst.ReselCodCameraV2 = Convert.ToInt32(reader["ReselCodCameraV2"]);
                            inst.AdelCodCamera = reader["AdelCodCamera"].ToString();
                            inst.ReselAccesTip = Convert.ToInt32(reader["ReselAccesTip"]);
                            inst.ReselAccesData = reader["ReselAccesData"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["ReselAccesData"]);
                            inst.CodTelefon = reader["CodTelefon"].ToString();
                            inst.ReselLastConfortTemp = Convert.ToDecimal(reader["ReselLastConfortTemp"]);
                            inst.ReselCodTempPreset = reader["ReselCodTempPreset"].ToString();
                            inst.ReselForteazaECNF = Convert.ToBoolean(reader["ReselForteazaECNF"]);
                            inst.ReselAutoECNF = Convert.ToBoolean(reader["ReselAutoECNF"]);
                            inst.ReselDataInchidYalaMD = reader["ReselDataInchidYalaMD"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["ReselDataInchidYalaMD"]);
                            inst.ReselAccesariEcCamerista = Convert.ToInt32(reader["ReselAccesariEcCamerista"]);
                            inst.ReselDataExpirareEcCamerista = reader["ReselDataExpirareEcCamerista"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["ReselDataExpirareEcCamerista"]);
                            inst.ReselDataDeblocareEcCamerista = reader["ReselDataDeblocareEcCamerista"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["ReselDataDeblocareEcCamerista"]);
                            inst.ReselDataExpirareEcApp = reader["ReselDataExpirareEcApp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["ReselDataExpirareEcApp"]);
                            inst.Banquet = Convert.ToBoolean(reader["Banquet"]);
                            inst.CaliromRoomId = reader["CaliromRoomId"].ToString();
                            inst.CaliromRoomNumber = reader["CaliromRoomNumber"].ToString();
                            inst.ReselMentineValvaApaBaieInchisa = Convert.ToBoolean(reader["ReselMentineValvaApaBaieInchisa"]);
                            inst.ReselDataOraIncepereEcoFD = reader["ReselDataOraIncepereEcoFD"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["ReselDataOraIncepereEcoFD"]);
                            inst.ReselEcoFD = Convert.ToBoolean(reader["ReselEcoFD"]);
                            inst.ReselTemperaturaFREC = Convert.ToDecimal(reader["ReselTemperaturaFREC"]);
                            inst.ReselForteazaFREC = Convert.ToBoolean(reader["ReselForteazaFREC"]);
                            inst.CodGoodumLocks = reader["CodGoodumLocks"].ToString();
                            inst.ReselModTermostat = Convert.ToInt32(reader["ReselModTermostat"]);
                            inst.ReselInchideTermostatLaEco = Convert.ToBoolean(reader["ReselInchideTermostatLaEco"]);
                            inst.ReselInterfonIndex = Convert.ToInt32(reader["ReselInterfonIndex"]);
                            inst.ReselInterfonCodApartament = Convert.ToInt32(reader["ReselInterfonCodApartament"]);
                            inst.TrimiteTemperaturaCONF = Convert.ToBoolean(reader["TrimiteTemperaturaCONF"]);
                            inst.DenumireCamera = reader["DenumireCamera"].ToString();
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