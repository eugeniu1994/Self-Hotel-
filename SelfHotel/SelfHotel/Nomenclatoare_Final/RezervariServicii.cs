using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class RezervariServicii
    {
        public Int32 ID { get; set; }
        public Int32 IdRezervareCamera { get; set; }
        public Int32 IdVenit { get; set; }
        public Int32 IdTarif { get; set; }
        public String DenumireServiciu { get; set; }
        public Int32 IdMoneda { get; set; }
        public Int32 IdCotaTVA { get; set; }
        public Int32 Cantitate { get; set; }
        public Int32 IdRezervareCameraMutat { get; set; }
        public DateTime DataOra { get; set; }
        public Decimal ValoareRon { get; set; }
        public Decimal Curs { get; set; }
        public Int32 IdCategCopii { get; set; }
        public Int32 IdPret { get; set; }
        public Int32 IdServiciuAvansLa { get; set; }
        public Int32 IdServiciuAvansStornat { get; set; }

        public static List<RezervariServicii> GetLista(int IdRezervareCamera)
        {
            List<RezervariServicii> rv = new List<RezervariServicii>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT rs.ID
                                          ,rs.[IdRezervareCamera]
                                          ,rs.[IdVenit]
                                          ,rs.[IdTarif]
                                          ,rs.[DenumireServiciu]
                                          ,rs.[IdMoneda]
                                          ,rs.[IdCotaTVA]
                                          ,[Cantitate]
                                          ,[IdRezervareCameraMutat]
                                          ,[DataOra]
                                          ,[ValoareRon]
                                          ,Curs
                                          ,[IdCategCopii]
                                          ,[IdPret]
                                          ,IdServiciuAvansLa
                                          ,IdServiciuAvansStornat
                                      FROM [SOLON.H].[hotel].[RezervariServicii] as rs
                                      where 
                                          ((IdRezervareCamera=@IdRezervareCamera and ISNULL(IdRezervareCameraMutat,0)=0) or IdRezervareCameraMutat=@IdRezervareCamera) and Sters=0 ";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdRezervareCamera", SqlDbType.BigInt)).Value = IdRezervareCamera;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RezervariServicii inst = new RezervariServicii();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.IdRezervareCamera = reader["IdRezervareCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdRezervareCamera"]);
                            inst.IdVenit = reader["IdVenit"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdVenit"]);
                            inst.IdTarif = reader["IdTarif"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdTarif"]);
                            inst.DenumireServiciu = reader["DenumireServiciu"] == DBNull.Value ? "" : reader["DenumireServiciu"].ToString();
                            inst.IdMoneda = reader["IdMoneda"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdMoneda"]);
                            inst.IdCotaTVA = reader["IdCotaTVA"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdCotaTVA"]);
                            inst.Cantitate = reader["Cantitate"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Cantitate"]);
                            inst.IdRezervareCameraMutat = reader["IdRezervareCameraMutat"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdRezervareCameraMutat"]);
                            inst.DataOra = reader["DataOra"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataOra"]);
                            inst.ValoareRon = reader["ValoareRon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareRon"]);
                            inst.Curs = reader["Curs"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Curs"]);
                            inst.IdCategCopii = reader["IdCategCopii"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdCategCopii"]);
                            inst.IdPret = reader["IdPret"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdPret"]);
                            inst.IdServiciuAvansLa = reader["IdServiciuAvansLa"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdServiciuAvansLa"]);
                            inst.IdServiciuAvansStornat = reader["IdServiciuAvansStornat"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdServiciuAvansStornat"]);
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
      
        public static List<RezervariServicii> GetLista(int IdRezervareCamera, SqlConnection cnn, SqlCommand cmd, SqlTransaction tran)
        {
            List<RezervariServicii> rv = new List<RezervariServicii>();
                    string sql = @"SELECT rs.ID
                                          ,rs.[IdRezervareCamera]
                                          ,rs.[IdVenit]
                                          ,rs.[IdTarif]
                                          ,rs.[DenumireServiciu]
                                          ,rs.[IdMoneda]
                                          ,rs.[IdCotaTVA]
                                          ,[Cantitate]
                                          ,[IdRezervareCameraMutat]
                                          ,[DataOra]
                                          ,[ValoareRon]
                                          ,Curs
                                          ,[IdCategCopii]
                                          ,[IdPret]
                                          ,IdServiciuAvansLa
                                          ,IdServiciuAvansStornat
                                      FROM [SOLON.H].[hotel].[RezervariServicii] as rs
                                      where 
                                          ((IdRezervareCamera=@IdRezervareCamera and ISNULL(IdRezervareCameraMutat,0)=0) or IdRezervareCameraMutat=@IdRezervareCamera) and Sters=0 ";
                    cmd = new SqlCommand(sql, cnn, tran);
                    cmd.Parameters.Add(new SqlParameter("@IdRezervareCamera", SqlDbType.BigInt)).Value = IdRezervareCamera;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RezervariServicii inst = new RezervariServicii();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.IdRezervareCamera = reader["IdRezervareCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdRezervareCamera"]);
                            inst.IdVenit = reader["IdVenit"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdVenit"]);
                            inst.IdTarif = reader["IdTarif"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdTarif"]);
                            inst.DenumireServiciu = reader["DenumireServiciu"] == DBNull.Value ? "" : reader["DenumireServiciu"].ToString();
                            inst.IdMoneda = reader["IdMoneda"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdMoneda"]);
                            inst.IdCotaTVA = reader["IdCotaTVA"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdCotaTVA"]);
                            inst.Cantitate = reader["Cantitate"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Cantitate"]);
                            inst.IdRezervareCameraMutat = reader["IdRezervareCameraMutat"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdRezervareCameraMutat"]);
                            inst.DataOra = reader["DataOra"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataOra"]);
                            inst.ValoareRon = reader["ValoareRon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareRon"]);
                            inst.Curs = reader["Curs"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Curs"]);
                            inst.IdCategCopii = reader["IdCategCopii"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdCategCopii"]);
                            inst.IdPret = reader["IdPret"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdPret"]);
                            inst.IdServiciuAvansLa = reader["IdServiciuAvansLa"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdServiciuAvansLa"]);
                            inst.IdServiciuAvansStornat = reader["IdServiciuAvansStornat"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdServiciuAvansStornat"]);
                            rv.Add(inst);
                        }
                    }
            return rv;
        }
    }
}