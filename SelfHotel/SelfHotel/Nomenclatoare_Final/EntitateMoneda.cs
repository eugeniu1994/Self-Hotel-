using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitateMoneda
    {
        public int ID { get; set; }
        public string Cod { get; set; }
        public bool EsteNationala { get; set; }

        public decimal CursZi { get; set; }

        public override string ToString()
        {
            return Cod;
        }

        public decimal TotalInMoneda { get; set; }//pt a calcula cursul mediu
        public decimal TotalInRon { get; set; }//pt a calcula cursul mediu
        public decimal CursMediu
        {
            get
            {
                if (TotalInMoneda == 0 || TotalInRon == 0)
                {
                    return 0;
                }
                else
                {
                    return Math.Round(TotalInRon / TotalInMoneda, 4);
                }
            }
        }

        public static EntitateMoneda GetMonedaNationala()
        {
            EntitateMoneda rv = null;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT * FROM SOLON.[dbo].[NomMonede] WHERE [Sters] = 0 AND [EsteNationala] = 1";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            EntitateMoneda inst = new EntitateMoneda();
                            inst.ID = Convert.ToInt32(reader["IdMoneda"]);
                            //inst.MonedaCod = reader["MonedaCod"].ToString();
                            //inst.MonedaNume = reader["MonedaNume"] == DBNull.Value ? "" : reader["MonedaNume"].ToString();
                            //inst.Sters = Convert.ToBoolean(reader["Sters"]);
                            //inst.IdServer = reader["IdServer"] == DBNull.Value ? "" : reader["IdServer"].ToString();
                            //inst.IdRemote = reader["IdRemote"] == DBNull.Value ? 0 : Convert.ToInt64(reader["IdRemote"]);
                            inst.EsteNationala = Convert.ToBoolean(reader["EsteNationala"]);
                            rv = inst;
                        }
                    }
                }
                catch (Exception exc)
                {
                    rv = null;
                }
            }
            return rv;
        }
    }
}