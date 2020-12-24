using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class SetariBaza
    {
        public Int32 ID { get; set; }
        public string Denumire { get; set; }
        public string Valoare { get; set; }
        public int IdHotel { get; set; }
        public int IdComputer { get; set; }
        public string TipSetare { get; set; }

        public static List<SetariBaza> getLista()
        {
            List<SetariBaza> rv = new List<SetariBaza>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {

                    cnn.Open();
                    string sql = @"SELECT
                                         [ID]
                                        ,[Denumire]
                                        ,[Valoare]
                                        ,[IdHotel]
                                        ,[IdComputer]
                                        ,[TipSetare]
                                    FROM [SmartH].[dbo].[Setari]";

                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SetariBaza inst = new SetariBaza();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.ID = reader["ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID"]);
                            inst.Denumire = reader["Denumire"] == DBNull.Value ? "" : reader["Denumire"].ToString();
                            inst.Valoare = reader["Valoare"] == DBNull.Value ? "" : reader["Valoare"].ToString();
                            inst.IdHotel = reader["IdHotel"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdHotel"]);
                            inst.IdComputer = reader["IdComputer"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdComputer"]);
                            inst.TipSetare = reader["TipSetare"] == DBNull.Value ? "" : reader["TipSetare"].ToString();
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

        public static int Insert(SetariBaza s)
        {
            int rv = 0;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"INSERT INTO [SmartH].[dbo].[Setari]
                                                        ([Denumire]
                                                        ,[Valoare]
                                                        ,[IdHotel]
                                                        ,[IdComputer]
                                                        ,[TipSetare])
                                                    VALUES
                                                        (@Denumire
                                                        ,@Valoare
                                                        ,@IdHotel
                                                        ,@IdComputer
                                                        ,@TipSetare)";

                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@Denumire", SqlDbType.NVarChar)).Value = s.Denumire;
                    cmd.Parameters.Add(new SqlParameter("@Valoare", SqlDbType.NVarChar)).Value = s.Valoare;
                    cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.Int)).Value = s.IdHotel;
                    cmd.Parameters.Add(new SqlParameter("@IdComputer", SqlDbType.Int)).Value = s.IdComputer;
                    cmd.Parameters.Add(new SqlParameter("@TipSetare", SqlDbType.VarChar)).Value = s.TipSetare;

                    rv = cmd.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    return 0;
                }
            }
            return rv;
        }
    }
}