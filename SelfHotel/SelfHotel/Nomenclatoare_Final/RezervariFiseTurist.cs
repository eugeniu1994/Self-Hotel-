using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class RezervariFiseTurist
    {
        public int ID { get; set; }
        public int IdRezervareCamera { get; set; }
        public int IdTurist { get; set; }
        public int IdHotel { get; set; }
        public string Camera { get; set; }
        public string NumePrenume { get; set; }
        public DateTime DataNasterii { get; set; }
        public string LoculNasterii { get; set; }
        public string Cetatenia { get; set; }
        public string Localitate { get; set; }
        public string Strada { get; set; }
        public string Numar { get; set; }
        public string Bloc { get; set; }
        public string Scara { get; set; }
        public string Etaj { get; set; }
        public string Ap { get; set; }
        public string CodPostal { get; set; }
        public string NrAuto { get; set; }
        public string Judet { get; set; }
        public string Tara { get; set; }
        public DateTime Sosire { get; set; }
        public DateTime Plecare { get; set; }
        public string Scopul { get; set; }
        public string ActTip { get; set; }
        public string ActSerie { get; set; }
        public string ActNumar { get; set; }
        public int IdCarnet { get; set; }
        public string Serie { get; set; }
        public string Nr { get; set; }
        public DateTime DataOraCreare { get; set; }
        public int IdModRezervare { get; set; }
        public int IdSegmentPiata { get; set; }
        public int IdSursaRezervare { get; set; }
        public Boolean Anulata { get; set; }
        public string Telefon { get; set; }
        public string Mail { get; set; }
        public DateTime DataOraAnulare { get; set; }
        public int IdUserAnulare { get; set; }
        public DateTime DataLucru { get; set; }

        public static bool Insert(RezervariFiseTurist f)
        {
            bool rv = false;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"INSERT INTO [SOLON.H].[hotel].[RezervariFiseTurist]
                                                    ([IdRezervareCamera]
                                                    ,[IdTurist]
                                                    ,[IdHotel]
                                                    ,[Camera]
                                                    ,[NumePrenume]
                                                    ,[DataNasterii]
                                                    ,[LoculNasterii]
                                                    ,[Cetatenia]
                                                    ,[Localitate]
                                                    ,[Strada]
                                                    ,[Numar]
                                                    ,[Bloc]
                                                    ,[Scara]
                                                    ,[Etaj]
                                                    ,[Ap]
                                                    ,[CodPostal]
                                                    ,[NrAuto]
                                                    ,[Judet]
                                                    ,[Tara]
                                                    ,[Sosire]
                                                    ,[Plecare]
                                                    ,[Scopul]
                                                    ,[ActTip]
                                                    ,[ActSerie]
                                                    ,[ActNumar]
                                                    ,[IdCarnet]
                                                    ,[Serie]
                                                    ,[Nr]
                                                    ,[DataOraCreare]
                                                    ,[IdModRezervare]
                                                    ,[IdSegmentPiata]
                                                    ,[IdSursaRezervare]
                                                    ,[Anulata]
                                                    ,[Telefon]
                                                    ,[Mail]
                                                    ,[DataOraAnulare]
                                                    ,[IdUserAnulare]
                                                    ,[DataLucru])
                                                VALUES
                                                    (@IdRezervareCamera
                                                    ,@IdTurist
                                                    ,@IdHotel
                                                    ,@Camera
                                                    ,@NumePrenume
                                                    ,@DataNasterii
                                                    ,@LoculNasterii
                                                    ,@Cetatenia
                                                    ,@Localitate
                                                    ,@Strada
                                                    ,@Numar
                                                    ,@Bloc
                                                    ,@Scara
                                                    ,@Etaj
                                                    ,@Ap
                                                    ,@CodPostal
                                                    ,@NrAuto
                                                    ,@Judet
                                                    ,@Tara
                                                    ,@Sosire
                                                    ,@Plecare
                                                    ,@Scopul
                                                    ,@ActTip
                                                    ,@ActSerie
                                                    ,@ActNumar
                                                    ,@IdCarnet
                                                    ,@Serie
                                                    ,@Nr
                                                    ,@DataOraCreare
                                                    ,@IdModRezervare
                                                    ,@IdSegmentPiata
                                                    ,@IdSursaRezervare
                                                    ,@Anulata
                                                    ,@Telefon
                                                    ,@Mail
                                                    ,@DataOraAnulare
                                                    ,@IdUserAnulare
                                                    ,@DataLucru)";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdRezervareCamera", SqlDbType.Int)).Value = f.IdRezervareCamera;
                    cmd.Parameters.Add(new SqlParameter("@IdTurist", SqlDbType.Int)).Value = f.IdTurist;
                    cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.Int)).Value = f.IdHotel;
                    cmd.Parameters.Add(new SqlParameter("@Camera", SqlDbType.NVarChar)).Value = f.Camera;
                    cmd.Parameters.Add(new SqlParameter("@NumePrenume", SqlDbType.NVarChar)).Value = f.NumePrenume;
                    cmd.Parameters.Add(new SqlParameter("@DataNasterii", SqlDbType.DateTime)).Value = f.DataNasterii == DateTime.MinValue ? SqlDateTime.MinValue : f.DataNasterii;
                    cmd.Parameters.Add(new SqlParameter("@LoculNasterii", SqlDbType.NVarChar)).Value = f.LoculNasterii==null ? "":f.LoculNasterii;
                    cmd.Parameters.Add(new SqlParameter("@Cetatenia", SqlDbType.NVarChar)).Value = f.Cetatenia == null ? "" : f.Cetatenia;
                    cmd.Parameters.Add(new SqlParameter("@Localitate", SqlDbType.NVarChar)).Value = f.Localitate == null ? "" : f.Localitate;
                    cmd.Parameters.Add(new SqlParameter("@Strada", SqlDbType.NVarChar)).Value = f.Strada == null ? "" : f.Strada;
                    cmd.Parameters.Add(new SqlParameter("@Numar", SqlDbType.NVarChar)).Value = f.Numar == null ? "" : f.Numar;
                    cmd.Parameters.Add(new SqlParameter("@Bloc", SqlDbType.NVarChar)).Value = f.Bloc == null ? "" : f.Bloc;
                    cmd.Parameters.Add(new SqlParameter("@Scara", SqlDbType.NVarChar)).Value = f.Scara == null ? "" : f.Scara;
                    cmd.Parameters.Add(new SqlParameter("@Etaj", SqlDbType.NVarChar)).Value = f.Etaj == null ? "" : f.Etaj;
                    cmd.Parameters.Add(new SqlParameter("@Ap", SqlDbType.NVarChar)).Value = f.Ap == null ? "" : f.Ap;
                    cmd.Parameters.Add(new SqlParameter("@CodPostal", SqlDbType.NVarChar)).Value = f.CodPostal == null ? "" : f.CodPostal;
                    cmd.Parameters.Add(new SqlParameter("@NrAuto", SqlDbType.NVarChar)).Value = f.NrAuto == null ? "" : f.NrAuto;
                    cmd.Parameters.Add(new SqlParameter("@Judet", SqlDbType.NVarChar)).Value = f.Judet == null ? "" : f.Judet; 
                    cmd.Parameters.Add(new SqlParameter("@Tara", SqlDbType.NVarChar)).Value = f.Tara == null ? "" : f.Tara;
                    cmd.Parameters.Add(new SqlParameter("@Sosire", SqlDbType.DateTime)).Value = f.Sosire == DateTime.MinValue ? SqlDateTime.MinValue : f.Sosire;
                    cmd.Parameters.Add(new SqlParameter("@Plecare", SqlDbType.DateTime)).Value = f.Plecare == DateTime.MinValue ? SqlDateTime.MinValue : f.Plecare;
                    cmd.Parameters.Add(new SqlParameter("@Scopul", SqlDbType.NVarChar)).Value = f.Scopul == null ? "" : f.Scopul;
                    cmd.Parameters.Add(new SqlParameter("@ActTip", SqlDbType.NVarChar)).Value = f.ActTip == null ? "" : f.ActTip; 
                    cmd.Parameters.Add(new SqlParameter("@ActSerie", SqlDbType.NVarChar)).Value = f.ActSerie == null ? "" : f.ActSerie;
                    cmd.Parameters.Add(new SqlParameter("@ActNumar", SqlDbType.NVarChar)).Value = f.ActNumar == null ? "" : f.ActNumar;
                    cmd.Parameters.Add(new SqlParameter("@IdCarnet", SqlDbType.Int)).Value = f.IdCarnet;
                    cmd.Parameters.Add(new SqlParameter("@Serie", SqlDbType.NVarChar)).Value = f.Serie == null ? "" : f.Serie;
                    cmd.Parameters.Add(new SqlParameter("@Nr", SqlDbType.NVarChar)).Value = f.Nr == null ? "" : f.Nr;
                    cmd.Parameters.Add(new SqlParameter("@DataOraCreare", SqlDbType.DateTime)).Value = f.DataOraCreare == DateTime.MinValue ? SqlDateTime.MinValue : f.DataOraCreare;
                    cmd.Parameters.Add(new SqlParameter("@IdModRezervare", SqlDbType.Int)).Value = f.IdModRezervare;
                    cmd.Parameters.Add(new SqlParameter("@IdSegmentPiata", SqlDbType.Int)).Value = f.IdSegmentPiata;
                    cmd.Parameters.Add(new SqlParameter("@IdSursaRezervare", SqlDbType.Int)).Value = f.IdSursaRezervare;
                    cmd.Parameters.Add(new SqlParameter("@Anulata", SqlDbType.Bit)).Value = f.Anulata;
                    cmd.Parameters.Add(new SqlParameter("@Telefon", SqlDbType.NVarChar)).Value = f.Telefon == null ? "" : f.Telefon;
                    cmd.Parameters.Add(new SqlParameter("@Mail", SqlDbType.NVarChar)).Value = f.Mail == null ? "" : f.Mail;
                    cmd.Parameters.Add(new SqlParameter("@DataOraAnulare", SqlDbType.DateTime)).Value = f.DataOraAnulare == DateTime.MinValue ? SqlDateTime.MinValue : f.DataOraAnulare;
                    cmd.Parameters.Add(new SqlParameter("@IdUserAnulare", SqlDbType.Int)).Value = f.IdUserAnulare;
                    cmd.Parameters.Add(new SqlParameter("@DataLucru", SqlDbType.DateTime)).Value = f.DataLucru== DateTime.MinValue ? SqlDateTime.MinValue : f.DataLucru;
                  
                    rv = cmd.ExecuteNonQuery()>0;
                }
                catch (Exception exc)
                {
                    return false;
                }
            }
            return rv;
        }

        public static List<RezervariFiseTurist> GetLista(int IdRezervareCamera)
        {
            List<RezervariFiseTurist> rv = new List<RezervariFiseTurist>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"
                                    SELECT [ID]
                                          ,[IdRezervareCamera]
                                          ,[IdTurist]
                                          ,[IdHotel]
                                          ,[Camera]
                                          ,[NumePrenume]
                                          ,[DataNasterii]
                                          ,[LoculNasterii]
                                          ,[Cetatenia]
                                          ,[Localitate]
                                          ,[Strada]
                                          ,[Numar]
                                          ,[Bloc]
                                          ,[Scara]
                                          ,[Etaj]
                                          ,[Ap]
                                          ,[CodPostal]
                                          ,[NrAuto]
                                          ,[Judet]
                                          ,[Tara]
                                          ,[Sosire]
                                          ,[Plecare]
                                          ,[Scopul]
                                          ,[ActTip]
                                          ,[ActSerie]
                                          ,[ActNumar]
                                          ,[IdCarnet]
                                          ,[Serie]
                                          ,[Nr]
                                          ,[DataOraCreare]
                                          ,[IdModRezervare]
                                          ,[IdSegmentPiata]
                                          ,[IdSursaRezervare]
                                          ,[Anulata]
                                          ,[Telefon]
                                          ,[Mail]
                                          ,[DataOraAnulare]
                                          ,[IdUserAnulare]
                                          ,[DataLucru]
                                      FROM [SOLON.H].[hotel].[RezervariFiseTurist]
                                        WHERE IdRezervareCamera=@IdRezervareCamera";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdRezervareCamera", SqlDbType.Int)).Value = IdRezervareCamera;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RezervariFiseTurist inst = new RezervariFiseTurist();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.IdRezervareCamera = Convert.ToInt32(reader["IdRezervareCamera"]);
                            inst.IdTurist = Convert.ToInt32(reader["IdTurist"]);
                            inst.IdHotel = Convert.ToInt32(reader["IdHotel"]);
                            inst.Camera = reader["Camera"] == DBNull.Value ? "" : reader["Camera"].ToString();
                            inst.NumePrenume = reader["NumePrenume"] == DBNull.Value ? "" : reader["NumePrenume"].ToString();
                            inst.DataNasterii = reader["DataNasterii"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataNasterii"]);
                            inst.LoculNasterii = reader["LoculNasterii"] == DBNull.Value ? "" : reader["LoculNasterii"].ToString();
                            inst.Cetatenia = reader["Cetatenia"] == DBNull.Value ? "" : reader["Cetatenia"].ToString();
                            inst.Localitate = reader["Localitate"] == DBNull.Value ? "" : reader["Localitate"].ToString();
                            inst.Strada = reader["Strada"] == DBNull.Value ? "" : reader["Strada"].ToString();
                            inst.Numar = reader["Numar"] == DBNull.Value ? "" : reader["Numar"].ToString();
                            inst.Bloc = reader["Bloc"] == DBNull.Value ? "" : reader["Bloc"].ToString();
                            inst.Scara = reader["Scara"] == DBNull.Value ? "" : reader["Scara"].ToString();
                            inst.Etaj = reader["Etaj"] == DBNull.Value ? "" : reader["Etaj"].ToString();
                            inst.Ap = reader["Ap"] == DBNull.Value ? "" : reader["Ap"].ToString();
                            inst.CodPostal = reader["CodPostal"] == DBNull.Value ? "" : reader["CodPostal"].ToString();
                            inst.NrAuto = reader["NrAuto"] == DBNull.Value ? "" : reader["NrAuto"].ToString();
                            inst.Judet = reader["Judet"] == DBNull.Value ? "" : reader["Judet"].ToString();
                            inst.Tara = reader["Tara"] == DBNull.Value ? "" : reader["Tara"].ToString();
                            inst.Sosire = Convert.ToDateTime(reader["Sosire"]);
                            inst.Plecare = Convert.ToDateTime(reader["Plecare"]);
                            inst.Scopul = reader["Scopul"] == DBNull.Value ? "" : reader["Scopul"].ToString();
                            inst.ActTip = reader["ActTip"] == DBNull.Value ? "" : reader["ActTip"].ToString();
                            inst.ActSerie = reader["ActSerie"] == DBNull.Value ? "" : reader["ActSerie"].ToString();
                            inst.ActNumar = reader["ActNumar"] == DBNull.Value ? "" : reader["ActNumar"].ToString();
                            inst.IdCarnet = Convert.ToInt32(reader["IdCarnet"]);
                            inst.Serie = reader["Serie"].ToString();
                            inst.Nr = reader["Nr"].ToString();
                            inst.DataOraCreare = Convert.ToDateTime(reader["DataOraCreare"]);
                            inst.IdModRezervare = Convert.ToInt32(reader["IdModRezervare"]);
                            inst.IdSegmentPiata = Convert.ToInt32(reader["IdSegmentPiata"]);
                            inst.IdSursaRezervare = Convert.ToInt32(reader["IdSursaRezervare"]);
                            inst.Anulata = Convert.ToBoolean(reader["Anulata"]);
                            inst.Telefon = reader["Telefon"] == DBNull.Value ? "" : reader["Telefon"].ToString();
                            inst.Mail = reader["Mail"] == DBNull.Value ? "" : reader["Mail"].ToString();
                            inst.DataOraAnulare = reader["DataOraAnulare"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataOraAnulare"]);
                            inst.IdUserAnulare = Convert.ToInt32(reader["IdUserAnulare"]);
                            inst.DataLucru = Convert.ToDateTime(reader["DataLucru"]);
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

        public static RezervariFiseTurist GetFisa(int IdRezervareCamera)
        {
            RezervariFiseTurist rv = null;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"
                                    SELECT TOP 1 [ID]
                                          ,[IdRezervareCamera]
                                          ,[IdTurist]
                                          ,[IdHotel]
                                          ,[Camera]
                                          ,[NumePrenume]
                                          ,[DataNasterii]
                                          ,[LoculNasterii]
                                          ,[Cetatenia]
                                          ,[Localitate]
                                          ,[Strada]
                                          ,[Numar]
                                          ,[Bloc]
                                          ,[Scara]
                                          ,[Etaj]
                                          ,[Ap]
                                          ,[CodPostal]
                                          ,[NrAuto]
                                          ,[Judet]
                                          ,[Tara]
                                          ,[Sosire]
                                          ,[Plecare]
                                          ,[Scopul]
                                          ,[ActTip]
                                          ,[ActSerie]
                                          ,[ActNumar]
                                          ,[IdCarnet]
                                          ,[Serie]
                                          ,[Nr]
                                          ,[DataOraCreare]
                                          ,[IdModRezervare]
                                          ,[IdSegmentPiata]
                                          ,[IdSursaRezervare]
                                          ,[Anulata]
                                          ,[Telefon]
                                          ,[Mail]
                                          ,[DataOraAnulare]
                                          ,[IdUserAnulare]
                                          ,[DataLucru]
                                      FROM [SOLON.H].[hotel].[RezervariFiseTurist]
                                      WHERE IdRezervareCamera=@IdRezervareCamera";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdRezervareCamera", SqlDbType.Int)).Value = IdRezervareCamera;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            RezervariFiseTurist inst = new RezervariFiseTurist();
                            inst.ID = Convert.ToInt32(reader["ID"]);
                            inst.IdRezervareCamera = Convert.ToInt32(reader["IdRezervareCamera"]);
                            inst.IdTurist = Convert.ToInt32(reader["IdTurist"]);
                            inst.IdHotel = Convert.ToInt32(reader["IdHotel"]);
                            inst.Camera = reader["Camera"] == DBNull.Value ? "" : reader["Camera"].ToString();
                            inst.NumePrenume = reader["NumePrenume"] == DBNull.Value ? "" : reader["NumePrenume"].ToString();
                            inst.DataNasterii = reader["DataNasterii"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataNasterii"]);
                            inst.LoculNasterii = reader["LoculNasterii"] == DBNull.Value ? "" : reader["LoculNasterii"].ToString();
                            inst.Cetatenia = reader["Cetatenia"] == DBNull.Value ? "" : reader["Cetatenia"].ToString();
                            inst.Localitate = reader["Localitate"] == DBNull.Value ? "" : reader["Localitate"].ToString();
                            inst.Strada = reader["Strada"] == DBNull.Value ? "" : reader["Strada"].ToString();
                            inst.Numar = reader["Numar"] == DBNull.Value ? "" : reader["Numar"].ToString();
                            inst.Bloc = reader["Bloc"] == DBNull.Value ? "" : reader["Bloc"].ToString();
                            inst.Scara = reader["Scara"] == DBNull.Value ? "" : reader["Scara"].ToString();
                            inst.Etaj = reader["Etaj"] == DBNull.Value ? "" : reader["Etaj"].ToString();
                            inst.Ap = reader["Ap"] == DBNull.Value ? "" : reader["Ap"].ToString();
                            inst.CodPostal = reader["CodPostal"] == DBNull.Value ? "" : reader["CodPostal"].ToString();
                            inst.NrAuto = reader["NrAuto"] == DBNull.Value ? "" : reader["NrAuto"].ToString();
                            inst.Judet = reader["Judet"] == DBNull.Value ? "" : reader["Judet"].ToString();
                            inst.Tara = reader["Tara"] == DBNull.Value ? "" : reader["Tara"].ToString();
                            inst.Sosire = Convert.ToDateTime(reader["Sosire"]);
                            inst.Plecare = Convert.ToDateTime(reader["Plecare"]);
                            inst.Scopul = reader["Scopul"] == DBNull.Value ? "" : reader["Scopul"].ToString();
                            inst.ActTip = reader["ActTip"] == DBNull.Value ? "" : reader["ActTip"].ToString();
                            inst.ActSerie = reader["ActSerie"] == DBNull.Value ? "" : reader["ActSerie"].ToString();
                            inst.ActNumar = reader["ActNumar"] == DBNull.Value ? "" : reader["ActNumar"].ToString();
                            inst.IdCarnet = Convert.ToInt32(reader["IdCarnet"]);
                            inst.Serie = reader["Serie"].ToString();
                            inst.Nr = reader["Nr"].ToString();
                            inst.DataOraCreare = Convert.ToDateTime(reader["DataOraCreare"]);
                            inst.IdModRezervare = Convert.ToInt32(reader["IdModRezervare"]);
                            inst.IdSegmentPiata = Convert.ToInt32(reader["IdSegmentPiata"]);
                            inst.IdSursaRezervare = Convert.ToInt32(reader["IdSursaRezervare"]);
                            inst.Anulata = Convert.ToBoolean(reader["Anulata"]);
                            inst.Telefon = reader["Telefon"] == DBNull.Value ? "" : reader["Telefon"].ToString();
                            inst.Mail = reader["Mail"] == DBNull.Value ? "" : reader["Mail"].ToString();
                            inst.DataOraAnulare = reader["DataOraAnulare"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataOraAnulare"]);
                            inst.IdUserAnulare = Convert.ToInt32(reader["IdUserAnulare"]);
                            inst.DataLucru = Convert.ToDateTime(reader["DataLucru"]);
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