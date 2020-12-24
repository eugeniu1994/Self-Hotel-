using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitatePM
    {
        public int ID { get; set; }
        public string Cod { get; set; }
        public string Denumire { get; set; }
        public int IdMoneda { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataEnd { get; set; }
        public bool AreMD { get; set; }
        public bool AreDejun { get; set; }
        public bool AreCina { get; set; }
        public int IdVenitMD { get; set; }
        public int IdVenitDejun { get; set; }
        public int IdVenitCina { get; set; }
        public bool PostareAmanataMD { get; set; }
        public bool Sters { get; set; }

        public override string ToString()
        {
            return Denumire;
        }

        public static List<EntitatePM> GetLista()
        {
            List<EntitatePM> rv = new List<EntitatePM>();
            rv.Add(new EntitatePM()
            {
                ID = 0,
                Denumire = "---",
                DataStart = DateTime.MinValue,
                DataEnd = DateTime.MaxValue
            });
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"
                            SELECT
                                 [ID]
                                ,[Cod]
                                ,[Denumire]
                                ,[IdMoneda]
                                ,[DataStart]
                                ,[DataEnd]
                                ,[AreMD]
                                ,[AreDejun]
                                ,[AreCina]
                                ,[IdVenitMD]
                                ,[IdVenitDejun]
                                ,[IdVenitCina]
                                ,[PostareAmanataMD]
                                ,[Sters]
                            FROM [SOLON.H].[tarife].[PlanMasa] WHERE [Sters] = 0 AND [IdHotel] = @IdHotel
                            ORDER BY [Denumire]
                        ";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.Int)).Value = ConexiuneDB.IdHotel;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rv.Add(new EntitatePM()
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Cod = reader["Cod"].ToString(),
                                Denumire = reader["Denumire"].ToString(),
                                IdMoneda = Convert.ToInt32(reader["IdMoneda"]),
                                DataStart = Convert.ToDateTime(reader["DataStart"]),
                                DataEnd = Convert.ToDateTime(reader["DataEnd"]),
                                AreMD = Convert.ToBoolean(reader["AreMD"]),
                                AreDejun = Convert.ToBoolean(reader["AreDejun"]),
                                AreCina = Convert.ToBoolean(reader["AreCina"]),
                                IdVenitMD = reader["IdVenitMD"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdVenitMD"]),
                                IdVenitDejun = reader["IdVenitDejun"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdVenitDejun"]),
                                IdVenitCina = reader["IdVenitCina"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdVenitCina"]),
                                PostareAmanataMD = Convert.ToBoolean(reader["PostareAmanataMD"]),
                                Sters = Convert.ToBoolean(reader["Sters"])
                            });
                        }
                    }
                }
                catch (Exception exc)
                {}
            }

            return rv;
        }

        public bool Restant { get; set; }
        public string MotivRestanta { get; set; }
    }

}