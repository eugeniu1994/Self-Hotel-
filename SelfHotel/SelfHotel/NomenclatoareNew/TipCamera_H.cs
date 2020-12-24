using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.NomenclatoareNew
{
    public class TipCamera_H
    {
        public Int32 ID { get; set; }
        public string Cod { get; set; }
        public string Denumire { get; set; }
        public int NrPaturi { get; set; }
        public int MaxPaturiSuplim { get; set; }
        public int Ordine { get; set; }
        public Boolean Sters { get; set; }
        public Boolean Suplimentara { get; set; }
        public Boolean Virtuala { get; set; }
        public int ReselNrAutoECNF { get; set; }

        public static List<TipCamera_H> GetLista()
        {
            List<TipCamera_H> rv = new List<TipCamera_H>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {

                    cnn.Open();
                    string sql = @"SELECT 
                                         [ID]
                                        ,[Cod]
                                        ,[Denumire]
                                        ,[NrPaturi]
                                        ,[MaxPaturiSuplim]
                                        ,[Ordine]
                                        ,[Sters]
                                        ,[Suplimentara]
                                        ,[Virtuala]
                                        ,[ReselNrAutoECNF]
                                    FROM [SOLON.H].[hotel].[TipCamera];";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TipCamera_H inst = new TipCamera_H();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.Cod = reader["Cod"].ToString();
                            inst.Denumire = reader["Denumire"].ToString();
                            inst.NrPaturi = Convert.ToInt32(reader["NrPaturi"]);
                            inst.MaxPaturiSuplim = Convert.ToInt32(reader["MaxPaturiSuplim"]);
                            inst.Ordine = Convert.ToInt32(reader["Ordine"]);
                            inst.Sters = Convert.ToBoolean(reader["Sters"]);
                            inst.Suplimentara = Convert.ToBoolean(reader["Suplimentara"]);
                            inst.Virtuala = Convert.ToBoolean(reader["Virtuala"]);
                            inst.ReselNrAutoECNF = Convert.ToInt32(reader["ReselNrAutoECNF"]);
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