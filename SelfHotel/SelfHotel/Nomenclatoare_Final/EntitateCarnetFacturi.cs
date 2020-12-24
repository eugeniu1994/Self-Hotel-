using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class EntitateCarnetFacturi
    {
        public int ID { get; set; }
        public string Serie { get; set; }
        public string NumarDeLa { get; set; }
        public string NumarPanaLa { get; set; }
        public bool EsteReceptie { get; set; }
        public bool EsteVirament { get; set; }
        public bool EsteAvans { get; set; }

        public List<EntitateMetodaPlata> MetodePlata { get; set; }

        public string LastNumber { get; set; }
        public DateTime LastData { get; set; }
        public long LastNumberParsed { get; set; }

        public string Numar { get; set; }
        public string MetodePlata_Proxy { get; set; }
        public string EsteReceptie_Proxy { get; set; }
        public string EsteVirament_Proxy { get; set; }
        public string EsteAvans_Proxy { get; set; }

        public int IdFurnizor { get; set; }
        public string NumeFurnizor { get; set; }

        public EntitateCarnetFacturi()
        {
            MetodePlata = new List<EntitateMetodaPlata>();
        }

        public static EntitateCarnetFacturi getCarnetOld()
        {
            EntitateCarnetFacturi rv = null;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT [ID]
                                          ,[TipDocument]
                                          ,[Serie]
                                          ,[NumarDeLa]
                                          ,[NumarPanaLa]
                                          ,[IdPartener]
                                          ,[IdHotel]
                                          ,[InFolosinta]
                                          ,[Sters]
                                          ,[Consumat]
                                          ,[ValabilDeLa]
                                          ,[ValabilPanaLa]
                                          ,[EsteReceptie]
                                          ,[EsteVirament]
                                          ,[EsteAvans]
                                      FROM [SOLON.H].[financiar].[CarneteDocumenteEmise]
                                      WHERE InFolosinta = 1 AND Consumat = 0 AND Sters = 0 AND EsteAvans = 0 AND ValabilDeLa <= @paramDataLucru AND ValabilPanaLa >=@paramDataLucru and IdHotel = @IdHotel";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@paramDataLucru", SqlDbType.DateTime)).Value = ConexiuneDB.DataLucr;
                    cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.BigInt)).Value = ConexiuneDB.IdHotel;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            EntitateCarnetFacturi inst = new EntitateCarnetFacturi();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.Serie = reader["Serie"].ToString();
                            inst.NumarDeLa = reader["NumarDeLa"].ToString();
                            inst.NumarPanaLa = reader["NumarPanaLa"].ToString();
                            inst.IdFurnizor = Convert.ToInt32(reader["IdPartener"]);
                            //inst.IdHotel = Convert.ToInt32(reader["IdHotel"]);
                            //inst.InFolosinta = Convert.ToBoolean(reader["InFolosinta"]);
                            //inst.Sters = Convert.ToBoolean(reader["Sters"]);
                            //inst.Consumat = Convert.ToBoolean(reader["Consumat"]);
                            //inst.ValabilDeLa = Convert.ToDateTime(reader["ValabilDeLa"]);
                            //inst.ValabilPanaLa = Convert.ToDateTime(reader["ValabilPanaLa"]);
                            inst.EsteReceptie = Convert.ToBoolean(reader["EsteReceptie"]);
                            inst.EsteVirament = Convert.ToBoolean(reader["EsteVirament"]);
                            inst.EsteAvans = Convert.ToBoolean(reader["EsteAvans"]);
                            rv = inst;
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

        public static void setCarnetConsumat(int ID)
        {
            EntitateCarnetFacturi rv = null;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"UPDATE [SOLON.H].[financiar].[CarneteDocumenteEmise]
                                                       SET 
                                                          [Consumat] = 1
                                                     WHERE ID=@ID";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)).Value = ID;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                }
            }
        }
    }
}