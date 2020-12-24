using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitateTipDoc
    {
        public int ID { get; set; }
        public string Cod { get; set; }
        public string Denumire { get; set; }
        public string Descriere { get; set; }
        public bool Fiscal { get; set; }
        public bool EsteChitanta { get; set; }
        public bool EsteDispozitie { get; set; }
        public bool Sters { get; set; }
        public bool EsteOP { get; set; }

        public EntitateCarnetChitante Carnet { get; set; }

        public override string ToString()
        {
            return Denumire;
        }

        public static List<EntitateTipDoc> GetLista()
        {
            List<EntitateTipDoc> rv = new List<EntitateTipDoc>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT  [ID]
                                          ,[Cod]
                                          ,[Denumire]
                                          ,[Descriere]
                                          ,[Fiscal]
                                          ,[EsteChitanta]
                                          ,[Sters]
                                          ,[EsteDispozitie]
                                          ,[EsteOP]
                                      FROM [SOLON.H].[financiar].[TipDocumentPlata];";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EntitateTipDoc inst = new EntitateTipDoc();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.Cod = reader["Cod"] == DBNull.Value ? "" : reader["Cod"].ToString();
                            inst.Denumire = reader["Denumire"] == DBNull.Value ? "" : reader["Denumire"].ToString();
                            inst.Descriere = reader["Descriere"] == DBNull.Value ? "" : reader["Descriere"].ToString();
                            inst.Fiscal = reader["Fiscal"] == DBNull.Value ? false : Convert.ToBoolean(reader["Fiscal"]);
                            inst.EsteChitanta = reader["EsteChitanta"] == DBNull.Value ? false : Convert.ToBoolean(reader["EsteChitanta"]);
                            inst.Sters = reader["Sters"] == DBNull.Value ? false : Convert.ToBoolean(reader["Sters"]);
                            inst.EsteDispozitie = reader["EsteDispozitie"] == DBNull.Value ? false : Convert.ToBoolean(reader["EsteDispozitie"]);
                            inst.EsteOP = reader["EsteOP"] == DBNull.Value ? false : Convert.ToBoolean(reader["EsteOP"]);
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