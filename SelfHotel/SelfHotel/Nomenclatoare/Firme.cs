using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare
{
    public class Firme
    {
        public Int32 ID { get; set; }
        public String Denumire { get; set; }
        public Decimal CapitalSocial { get; set; }
        public String RegCom { get; set; }
        public String CUI { get; set; }
        public String Sediul { get; set; }
        public String Cont { get; set; }
        public Boolean Furnizor { get; set; }

        public static Firme getFurnizor()
        {
            Firme rv = null;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT [ID]
                                          ,[Denumire]
                                          ,[CapitalSocial]
                                          ,[RegCom]
                                          ,[CUI]
                                          ,[Sediul]
                                          ,[Cont]
                                          ,[Furnizor]
                                      FROM [SmartH].[dbo].[Firme]
                                      WHERE Furnizor=1";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            rv = new Firme();
                            rv.ID = Convert.ToInt32(reader["ID"]);
                            rv.Denumire = reader["Denumire"] == DBNull.Value ? "" : reader["Denumire"].ToString();
                            rv.CapitalSocial = reader["CapitalSocial"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["CapitalSocial"]);
                            rv.RegCom = reader["RegCom"] == DBNull.Value ? "" : reader["RegCom"].ToString();
                            rv.CUI = reader["CUI"] == DBNull.Value ? "" : reader["CUI"].ToString();
                            rv.Sediul = reader["Sediul"] == DBNull.Value ? "" : reader["Sediul"].ToString();
                            rv.Cont = reader["Cont"] == DBNull.Value ? "" : reader["Cont"].ToString();
                            rv.Furnizor = reader["Furnizor"] == DBNull.Value ? false : Convert.ToBoolean(reader["Furnizor"]);
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