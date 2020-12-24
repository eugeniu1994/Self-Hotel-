using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class Rezervari
    {
        public int ID { get; set; }
        public int IdHotel { get; set; }
        public DateTime DataRezervare { get; set; }
        public int Culoare { get; set; }
        public long IdSursaRezervare { get; set; }
        public DateTime Sosire { get; set; }
        public int NrNopti { get; set; }
        public DateTime Plecare { get; set; }
        public int NrCamere { get; set; }
        public int NrAdulti { get; set; }
        public string DenumireGrup { get; set; }
        public int IdModRezervare { get; set; }
        public string RezervatDe { get; set; }
        public int IdSegPiata { get; set; }
        public int IdTipRezervare { get; set; }
        public string Observatii { get; set; }
        public Boolean Cazat { get; set; }
        public Boolean Sters { get; set; }
        public int IdMotivStergere { get; set; }
        public decimal SumaBlocataCard { get; set; }
        public int IdPartenerReceptie { get; set; }
        public int IdPartenerVirament { get; set; }
        public Boolean Confirmata { get; set; }
        public string CodRezervare { get; set; }

        public static List<Rezervari> GetLista(string CodRezervare)
        {
            List<Rezervari> rv = new List<Rezervari>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT
                                         [ID]
                                        ,[IdHotel]
                                        ,[DataRezervare]
                                        ,[Culoare]
                                        ,[IdSursaRezervare]
                                        ,[Sosire]
                                        ,[NrNopti]
                                        ,[Plecare]
                                        ,[NrCamere]
                                        ,[NrAdulti]
                                        ,[DenumireGrup]
                                        ,[IdModRezervare]
                                        ,[RezervatDe]
                                        ,[IdSegPiata]
                                        ,[IdTipRezervare]
                                        ,[Observatii]
                                        ,[Cazat]
                                        ,[Sters]
                                        ,[IdMotivStergere]
                                        ,[SumaBlocataCard]
                                        ,[IdPartenerReceptie]
                                        ,[IdPartenerVirament]
                                        ,[Confirmata]
                                        ,[CodRezervare]
                                    FROM [SOLON.H].[hotel].[Rezervari]
                                    WHERE CodRezervare=@CodRezervare";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@CodRezervare", SqlDbType.VarChar)).Value = CodRezervare;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Rezervari inst = new Rezervari();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.IdHotel = Convert.ToInt32(reader["IdHotel"]);
                            inst.DataRezervare = Convert.ToDateTime(reader["DataRezervare"]);
                            inst.Culoare = reader["Culoare"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Culoare"]);
                            inst.IdSursaRezervare = reader["IdSursaRezervare"] == DBNull.Value ? 0 : Convert.ToInt64(reader["IdSursaRezervare"]);
                            inst.Sosire = Convert.ToDateTime(reader["Sosire"]);
                            inst.NrNopti = Convert.ToInt32(reader["NrNopti"]);
                            inst.Plecare = Convert.ToDateTime(reader["Plecare"]);
                            inst.NrCamere = Convert.ToInt32(reader["NrCamere"]);
                            inst.NrAdulti = Convert.ToInt32(reader["NrAdulti"]);
                            inst.DenumireGrup = reader["DenumireGrup"].ToString();
                            inst.IdModRezervare = reader["IdModRezervare"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdModRezervare"]);
                            inst.RezervatDe = reader["RezervatDe"].ToString();
                            inst.IdSegPiata = reader["IdSegPiata"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdSegPiata"]);
                            inst.IdTipRezervare = reader["IdTipRezervare"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdTipRezervare"]);
                            inst.Observatii = reader["Observatii"] == DBNull.Value ? "" : reader["Observatii"].ToString();
                            inst.Cazat = Convert.ToBoolean(reader["Cazat"]);
                            inst.Sters = Convert.ToBoolean(reader["Sters"]);
                            inst.IdMotivStergere = reader["IdMotivStergere"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdMotivStergere"]);
                            inst.SumaBlocataCard = Convert.ToDecimal(reader["SumaBlocataCard"]);
                            inst.IdPartenerReceptie = Convert.ToInt32(reader["IdPartenerReceptie"]);
                            inst.IdPartenerVirament = Convert.ToInt32(reader["IdPartenerVirament"]);
                            inst.Confirmata = Convert.ToBoolean(reader["Confirmata"]);
                            inst.CodRezervare = reader["CodRezervare"] == DBNull.Value ? "" : reader["CodRezervare"].ToString();
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

        public static Rezervari GetRezervare(int ID)
        {
            Rezervari rv = null;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT
                                         [ID]
                                        ,[IdHotel]
                                        ,[DataRezervare]
                                        ,[Culoare]
                                        ,[IdSursaRezervare]
                                        ,[Sosire]
                                        ,[NrNopti]
                                        ,[Plecare]
                                        ,[NrCamere]
                                        ,[NrAdulti]
                                        ,[DenumireGrup]
                                        ,[IdModRezervare]
                                        ,[RezervatDe]
                                        ,[IdSegPiata]
                                        ,[IdTipRezervare]
                                        ,[Observatii]
                                        ,[Cazat]
                                        ,[Sters]
                                        ,[IdMotivStergere]
                                        ,[SumaBlocataCard]
                                        ,[IdPartenerReceptie]
                                        ,[IdPartenerVirament]
                                        ,[Confirmata]
                                        ,[CodRezervare]
                                    FROM [SOLON.H].[hotel].[Rezervari]
                                    WHERE ID=@ID";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)).Value = ID;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Rezervari inst = new Rezervari();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.IdHotel = Convert.ToInt32(reader["IdHotel"]);
                            inst.DataRezervare = Convert.ToDateTime(reader["DataRezervare"]);
                            inst.Culoare = reader["Culoare"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Culoare"]);
                            inst.IdSursaRezervare = reader["IdSursaRezervare"] == DBNull.Value ? 0 : Convert.ToInt64(reader["IdSursaRezervare"]);
                            inst.Sosire = Convert.ToDateTime(reader["Sosire"]);
                            inst.NrNopti = Convert.ToInt32(reader["NrNopti"]);
                            inst.Plecare = Convert.ToDateTime(reader["Plecare"]);
                            inst.NrCamere = Convert.ToInt32(reader["NrCamere"]);
                            inst.NrAdulti = Convert.ToInt32(reader["NrAdulti"]);
                            inst.DenumireGrup = reader["DenumireGrup"].ToString();
                            inst.IdModRezervare = reader["IdModRezervare"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdModRezervare"]);
                            inst.RezervatDe = reader["RezervatDe"].ToString();
                            inst.IdSegPiata = reader["IdSegPiata"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdSegPiata"]);
                            inst.IdTipRezervare = reader["IdTipRezervare"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdTipRezervare"]);
                            inst.Observatii = reader["Observatii"] == DBNull.Value ? "" : reader["Observatii"].ToString();
                            inst.Cazat = Convert.ToBoolean(reader["Cazat"]);
                            inst.Sters = Convert.ToBoolean(reader["Sters"]);
                            inst.IdMotivStergere = reader["IdMotivStergere"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdMotivStergere"]);
                            inst.SumaBlocataCard = Convert.ToDecimal(reader["SumaBlocataCard"]);
                            inst.IdPartenerReceptie = Convert.ToInt32(reader["IdPartenerReceptie"]);
                            inst.IdPartenerVirament = Convert.ToInt32(reader["IdPartenerVirament"]);
                            inst.Confirmata = Convert.ToBoolean(reader["Confirmata"]);
                            inst.CodRezervare = reader["CodRezervare"] == DBNull.Value ? "" : reader["CodRezervare"].ToString();
                            rv=inst;
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

        public static Rezervari GetRezervare(string CodRezervare)
        {
            Rezervari rv = null;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT
                                         [ID]
                                        ,[IdHotel]
                                        ,[DataRezervare]
                                        ,[Culoare]
                                        ,[IdSursaRezervare]
                                        ,[Sosire]
                                        ,[NrNopti]
                                        ,[Plecare]
                                        ,[NrCamere]
                                        ,[NrAdulti]
                                        ,[DenumireGrup]
                                        ,[IdModRezervare]
                                        ,[RezervatDe]
                                        ,[IdSegPiata]
                                        ,[IdTipRezervare]
                                        ,[Observatii]
                                        ,[Cazat]
                                        ,[Sters]
                                        ,[IdMotivStergere]
                                        ,[SumaBlocataCard]
                                        ,[IdPartenerReceptie]
                                        ,[IdPartenerVirament]
                                        ,[Confirmata]
                                        ,[CodRezervare]
                                    FROM [SOLON.H].[hotel].[Rezervari]
                                    WHERE CodRezervare=@CodRezervare";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@CodRezervare", SqlDbType.VarChar)).Value = CodRezervare;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Rezervari inst = new Rezervari();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.IdHotel = Convert.ToInt32(reader["IdHotel"]);
                            inst.DataRezervare = Convert.ToDateTime(reader["DataRezervare"]);
                            inst.Culoare = reader["Culoare"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Culoare"]);
                            inst.IdSursaRezervare = reader["IdSursaRezervare"] == DBNull.Value ? 0 : Convert.ToInt64(reader["IdSursaRezervare"]);
                            inst.Sosire = Convert.ToDateTime(reader["Sosire"]);
                            inst.NrNopti = Convert.ToInt32(reader["NrNopti"]);
                            inst.Plecare = Convert.ToDateTime(reader["Plecare"]);
                            inst.NrCamere = Convert.ToInt32(reader["NrCamere"]);
                            inst.NrAdulti = Convert.ToInt32(reader["NrAdulti"]);
                            inst.DenumireGrup = reader["DenumireGrup"].ToString();
                            inst.IdModRezervare = reader["IdModRezervare"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdModRezervare"]);
                            inst.RezervatDe = reader["RezervatDe"].ToString();
                            inst.IdSegPiata = reader["IdSegPiata"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdSegPiata"]);
                            inst.IdTipRezervare = reader["IdTipRezervare"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdTipRezervare"]);
                            inst.Observatii = reader["Observatii"] == DBNull.Value ? "" : reader["Observatii"].ToString();
                            inst.Cazat = Convert.ToBoolean(reader["Cazat"]);
                            inst.Sters = Convert.ToBoolean(reader["Sters"]);
                            inst.IdMotivStergere = reader["IdMotivStergere"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdMotivStergere"]);
                            inst.SumaBlocataCard = Convert.ToDecimal(reader["SumaBlocataCard"]);
                            inst.IdPartenerReceptie = Convert.ToInt32(reader["IdPartenerReceptie"]);
                            inst.IdPartenerVirament = Convert.ToInt32(reader["IdPartenerVirament"]);
                            inst.Confirmata = Convert.ToBoolean(reader["Confirmata"]);
                            inst.CodRezervare = reader["CodRezervare"] == DBNull.Value ? "" : reader["CodRezervare"].ToString();
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
    }
}