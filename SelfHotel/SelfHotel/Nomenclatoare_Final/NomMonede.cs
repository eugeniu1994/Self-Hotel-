using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class NomMonede
    {
        public int IdMoneda { get; set; }
        public string MonedaCod { get; set; }
        public string MonedaNume { get; set; }
        public Boolean Sters { get; set; }
        public string IdServer { get; set; }
        public long IdRemote { get; set; }
        public Boolean EsteNationala { get; set; }

        public static NomMonede GetMonedaNationala()
        {
            NomMonede rv = null;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT [IdMoneda] FROM [dbo].[NomMonede] WHERE [Sters] = 0 AND [EsteNationala] = 1";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NomMonede inst = new NomMonede();
                            inst.IdMoneda = Convert.ToInt32(reader["IdMoneda"]);
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