using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.NomenclatoareNew
{
    public class _Pictures
    {
        public Int32 ID { get; set; }
        public Guid Guid { get; set; }
        public string Descriere { get; set; }
        public byte[] FileData { get; set; }
        public Int32 IdCamera { get; set; }
        public Boolean Default { get; set; }
        public Int32 IdCam_H { get; set; }

        public static List<_Pictures> GetLista()
        {
            List<_Pictures> rv = new List<_Pictures>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {

                    cnn.Open();
                    string sql = @"SELECT  [ID]
                                          ,[Guid]
                                          ,[Descriere]
                                          ,[FileData]
                                          ,[IdCamera]
                                          ,[Default]
                                          ,[IdCam_H]
                                      FROM [SmartH].[dbo].[_Pictures];";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _Pictures inst = new _Pictures();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.Descriere = reader["Descriere"] == DBNull.Value ? "" : reader["Descriere"].ToString();
                            inst.IdCamera = reader["IdCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdCamera"]);
                            inst.Default = reader["Default"] == DBNull.Value ? false : Convert.ToBoolean(reader["Default"]);
                            inst.IdCam_H = reader["IdCam_H"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdCam_H"]);
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