using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.NomenclatoareNew
{
    public class DataLucruObj
    {
        public Int32 IdHotel { get; set; }
        public DateTime DataLucru { get; set; }
        public Int32 IdUser { get; set; }
        public DateTime DataOraModificare { get; set; }
        public Boolean EsteDataInapoiNew { get; set; }

        public static List<DataLucruObj> GetLista()
        {
            List<DataLucruObj> rv = new List<DataLucruObj>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT  [IdHotel]
                                          ,[DataLucru]
                                          ,[IdUser]
                                          ,[DataOraModificare]
                                          ,[EsteDataInapoiNew]
                                      FROM [SOLON.H].[hotel].[DataLucru];";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataLucruObj inst = new DataLucruObj();
                            inst.IdHotel = Convert.ToInt32(reader["IdHotel"]);
                            inst.DataLucru = reader["DataLucru"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataLucru"]);
                            inst.IdUser = reader["IdUser"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdUser"]);
                            inst.DataOraModificare = reader["DataOraModificare"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataOraModificare"]);
                            inst.EsteDataInapoiNew = reader["EsteDataInapoiNew"] == DBNull.Value ? false : Convert.ToBoolean(reader["EsteDataInapoiNew"]);
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