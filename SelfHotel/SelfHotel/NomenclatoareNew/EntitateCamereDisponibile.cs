using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.NomenclatoareNew
{
    public class EntitateCamereDisponibile
    {
        public Int32 IdCam { get; set; }
        public Int32 IdTipCamera { get; set; }
        public String DenTipCamera { get; set; }
        public Int32 MaxAdulti { get; set; }
        public Int32 MaxCopii { get; set; }
        public Int32 NrAdulti { get; set; }

        public static List<EntitateCamereDisponibile> GetLista(string data1, string data2)
        {
            List<EntitateCamereDisponibile> rv = new List<EntitateCamereDisponibile>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                DateTime dela; DateTime panaLa;
                try
                {
                    dela = DateTime.ParseExact(data1.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    panaLa = DateTime.ParseExact(data2.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (dela > panaLa)
                    {
                        return null;
                    }
                }
                catch (Exception conversie)
                {
                    return null;
                }
                try
                {

                    cnn.Open();
                    string sql = @"SELECT 
                                        cam.ID as IdCam
                                       ,tc.[ID] as IdTipCamera
                                       ,tc.DenTipCamera
                                       ,tc.MaxAdulti
                                       ,tc.MaxCopii
                                       ,tc.NrAdulti
                                FROM 
                                        [SmartH].[dbo].[_Camere] AS cam INNER JOIN
                                        [SmartH].[dbo].[_TipuriCamere] AS tc ON tc.ID = cam.IdTipCamera
                                WHERE 
                                        tc.Sters=0
                                        and cam.ID not in --not in camere ocupate
                                        (
			                                SELECT 
				                                RezCam.IdCam AS IdCamera
			                                FROM [SmartH].[dbo].[_RezervariCamere] as RezCam
				                                left outer join [SmartH].[dbo].[_Rezervari] as Rez on RezCam.IdRez=Rez.ID  
			                                WHERE 
			                                ((Rez.Sosire >= @dela AND Rez.Sosire <= @panala) 
									                                OR
			                                (Rez.Plecare > @dela AND Rez.Plecare <= @panala) 
									                                OR
			                                (Rez.Sosire < @dela AND Rez.Plecare > @panala))
		                                )
                                ORDER BY cam.ID;";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@dela", SqlDbType.DateTime)).Value = dela.ToString();
                    cmd.Parameters.Add(new SqlParameter("@panala", SqlDbType.DateTime)).Value = panaLa.ToString();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EntitateCamereDisponibile inst = new EntitateCamereDisponibile();
                            inst.IdCam = Convert.ToInt32(reader["IdCam"]);
                            inst.IdTipCamera = reader["IdTipCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdTipCamera"]);
                            inst.DenTipCamera = reader["DenTipCamera"] == DBNull.Value ? "" : reader["DenTipCamera"].ToString();
                            inst.MaxAdulti = reader["MaxAdulti"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MaxAdulti"]);
                            inst.MaxCopii = reader["MaxCopii"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MaxCopii"]);
                            inst.NrAdulti = reader["NrAdulti"] == DBNull.Value ? 0 : Convert.ToInt32(reader["NrAdulti"]);
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