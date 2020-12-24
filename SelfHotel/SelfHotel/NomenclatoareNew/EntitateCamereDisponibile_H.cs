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
    public class EntitateCamereDisponibile_H
    {
        public Int32 IdCam { get; set; }
        public Int32 IdTipCamera { get; set; }
        public String Cod { get; set; }
        public String DenTipCam { get; set; }
        public Int32 NrPaturi { get; set; }
        public Int32 MaxPaturiSuplim { get; set; }
        public Int32 MaxAdulti { get; set; }
        public Int32 MaxCopii { get; set; }
        public Decimal Valoare { get; set; }
        public Int32 IdTarif { get; set; }

        public static List<EntitateCamereDisponibile_H> GetLista(string data1, string data2) 
        {
            List<EntitateCamereDisponibile_H> rv = new List<EntitateCamereDisponibile_H>();
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
                                           ,tc.[Cod]
                                           ,tc.Denumire AS DenTipCam
                                           ,tc.NrPaturi
                                           ,tc.MaxPaturiSuplim
                                           ,NrPaturi+MaxPaturiSuplim as MaxAdulti
                                           ,MaxPaturiSuplim as MaxCopii
                                    FROM 
                                            [Solon.H].[hotel].[Camere] AS cam INNER JOIN
                                            [Solon.H].[hotel].[TipCamera] AS tc ON tc.ID = cam.IdTipCamera
                                    WHERE 
                                            cam.Suplimentara = 0 AND cam.virtuala = 0 AND cam.Ascunsa = 0  
                                            and cam.ID not in --not in camere ocupate
                                            (  
			                                    SELECT 
				                                    RezCam.IdCamera AS IdCamera
			                                    FROM [SOLON.H].hotel.RezervariCamere as RezCam
				                                    left outer join [SOLON.H].hotel.Rezervari as Rez on RezCam.IdRezervare=Rez.ID  
			                                    WHERE Rez.Sters=0 and RezCam.Sters=0 and
			                                    ((RezCam.Sosire >= @dela AND RezCam.Sosire <= @panala) 
									                                    OR
			                                    (RezCam.Plecare > @dela AND RezCam.Plecare <= @panala) 
									                                    OR
			                                    (RezCam.Sosire < @dela AND RezCam.Plecare > @panala))
		                                    )
                                    ORDER BY cam.ID ;";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@dela", SqlDbType.DateTime)).Value = dela.ToString();
                    cmd.Parameters.Add(new SqlParameter("@panala", SqlDbType.DateTime)).Value = panaLa.ToString();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EntitateCamereDisponibile_H inst = new EntitateCamereDisponibile_H();
                            inst.IdCam = Convert.ToInt32(reader["IdCam"]);
                            inst.IdTipCamera = reader["IdTipCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdTipCamera"]);
                            inst.Cod = reader["Cod"] == DBNull.Value ? "" : reader["Cod"].ToString();
                            inst.DenTipCam = reader["DenTipCam"] == DBNull.Value ? "" : reader["DenTipCam"].ToString();
                            inst.NrPaturi = reader["NrPaturi"] == DBNull.Value ? 0 : Convert.ToInt32(reader["NrPaturi"]);
                            inst.MaxPaturiSuplim = reader["MaxPaturiSuplim"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MaxPaturiSuplim"]);
                            inst.MaxAdulti = reader["MaxAdulti"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MaxAdulti"]);
                            inst.MaxCopii = reader["MaxCopii"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MaxCopii"]);
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