using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class Utilizatori
    {
        public long ID { get; set; }
        public string Email { get; set; }
        public string Parola { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Telefon { get; set; }
        public string Tara { get; set; }
        public string Judet { get; set; }
        public string Localitate { get; set; }
        public string Strada { get; set; }
        public string NrStrada { get; set; }
        public Boolean Sters { get; set; }
        public Boolean Activat { get; set; }
        public string CodActivare { get; set; }
        public DateTime DataOraActivare { get; set; }
        public DateTime DataOraUltimLogin { get; set; }
        public DateTime DataOraInregistrare { get; set; }

        public static List<Utilizatori> GetLista()
        {
            List<Utilizatori> rv = new List<Utilizatori>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {

                    cnn.Open();
                    string sql = @"SELECT
                                     [ID]
                                    ,[Email]
                                    ,[Parola]
                                    ,[Nume]
                                    ,[Prenume]
                                    ,[Telefon]
                                    ,[Tara]
                                    ,[Judet]
                                    ,[Localitate]
                                    ,[Strada]
                                    ,[NrStrada]
                                    ,[Sters]
                                    ,[Activat]
                                    ,[CodActivare]
                                    ,[DataOraActivare]
                                    ,[DataOraUltimLogin]
                                    ,[DataOraInregistrare]
                                FROM [SOLON.H].[dbo].[Utilizatori];";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Utilizatori inst = new Utilizatori();
                            inst.ID = Convert.ToInt64(reader["ID"]);
                            inst.Email = reader["Email"].ToString();
                            inst.Parola = reader["Parola"].ToString();
                            inst.Nume = reader["Nume"].ToString();
                            inst.Prenume = reader["Prenume"].ToString();
                            inst.Telefon = reader["Telefon"].ToString();
                            inst.Tara = reader["Tara"].ToString();
                            inst.Judet = reader["Judet"].ToString();
                            inst.Localitate = reader["Localitate"].ToString();
                            inst.Strada = reader["Strada"].ToString();
                            inst.NrStrada = reader["NrStrada"].ToString();
                            inst.Sters = Convert.ToBoolean(reader["Sters"]);
                            inst.Activat = Convert.ToBoolean(reader["Activat"]);
                            inst.CodActivare = reader["CodActivare"].ToString();
                            inst.DataOraActivare = Convert.ToDateTime(reader["DataOraActivare"]);
                            inst.DataOraUltimLogin = Convert.ToDateTime(reader["DataOraUltimLogin"]);
                            inst.DataOraInregistrare = Convert.ToDateTime(reader["DataOraInregistrare"]);
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