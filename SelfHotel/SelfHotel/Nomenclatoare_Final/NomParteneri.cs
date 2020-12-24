using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class NomParteneri
    {
        public long IdPartener { get; set; }
        public string NumePartener { get; set; }
        public string CodFiscalNumar { get; set; }
        public string CodFiscalAtribut { get; set; }
        public string RegCom { get; set; }
        public string Banca { get; set; }
        public string ContBanca { get; set; }
        public string Oras { get; set; }
        public string Judet { get; set; }
        public string Telefon { get; set; }
        public string Patron { get; set; }
        public string Director { get; set; }
        public string Strada { get; set; }
        public string Nr { get; set; }
        public string Bloc { get; set; }
        public string Scara { get; set; }
        public string Etaj { get; set; }
        public string Apartament { get; set; }
        public string MailAddress { get; set; }
        public string WebAddress { get; set; }
        public Boolean Sters { get; set; }
        public string IdServer { get; set; }
        public long IdRemote { get; set; }
        public int IdgestICS { get; set; }
        public int IdStrada { get; set; }
        public string TipJuridic { get; set; }
        public string Oras2 { get; set; }
        public string Judet2 { get; set; }
        public string Strada2 { get; set; }
        public string Nr2 { get; set; }
        public string Bloc2 { get; set; }
        public string Scara2 { get; set; }
        public string Etaj2 { get; set; }
        public string Apartament2 { get; set; }
        public int IdStrada2 { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Tip { get; set; }
        public string DetaliiSupFactura { get; set; }
        public DateTime DataStare { get; set; }
        public int Stare { get; set; }
        public string Administrator { get; set; }
        public string Adresa { get; set; }
        public string TelAdm { get; set; }
        public string Aplicatie { get; set; }
        public string Prescurtare { get; set; }
        public Boolean EModificat { get; set; }
        public string CapitalSocial { get; set; }
        public long IdParinte { get; set; }
        public int Status { get; set; }
        public string CodSIRUES { get; set; }
        public string Subunitate { get; set; }
        public string Cod { get; set; }
        public string Alias { get; set; }
        public int FilieraID { get; set; }
        public string Filiera { get; set; }
        public string Tara { get; set; }
        public string ContDeEmail { get; set; }
        public long IdJudet { get; set; }

        public static NomParteneri GetLista(int ID)
        {
            NomParteneri rv = null;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT 
                                     [IdPartener]
                                    ,[NumePartener]
                                    ,[CodFiscalNumar]
                                    ,[CodFiscalAtribut]
                                    ,[RegCom]
                                    ,[Banca]
                                    ,[ContBanca]
                                    ,[Oras]
                                    ,[Judet]
                                    ,[Telefon]
                                    ,[Patron]
                                    ,[Director]
                                    ,[Strada]
                                    ,[Nr]
                                    ,[Bloc]
                                    ,[Scara]
                                    ,[Etaj]
                                    ,[Apartament]
                                    ,[MailAddress]
                                    ,[WebAddress]
                                    ,[Sters]
                                    ,[IdServer]
                                    ,[IdRemote]
                                    ,[IdgestICS]
                                    ,[IdStrada]
                                    ,[TipJuridic]
                                    ,[Oras2]
                                    ,[Judet2]
                                    ,[Strada2]
                                    ,[Nr2]
                                    ,[Bloc2]
                                    ,[Scara2]
                                    ,[Etaj2]
                                    ,[Apartament2]
                                    ,[IdStrada2]
                                    ,[Nume]
                                    ,[Prenume]
                                    ,[Tip]
                                    ,[DetaliiSupFactura]
                                    ,[DataStare]
                                    ,[Stare]
                                    ,[Administrator]
                                    ,[Adresa]
                                    ,[TelAdm]
                                    ,[Aplicatie]
                                    ,[Prescurtare]
                                    ,[EModificat]
                                    ,[CapitalSocial]
                                    ,[IdParinte]
                                    ,[Status]
                                    ,[CodSIRUES]
                                    ,[Subunitate]
                                    ,[Cod]
                                    ,[Alias]
                                    ,[FilieraID]
                                    ,[Filiera]
                                    ,[Tara]
                                    ,[ContDeEmail]
                                    ,[IdJudet]
                                FROM [SOLON].[dbo].[NomParteneri]
                                WHERE IdPartener=@IdPartener";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdPartener", SqlDbType.BigInt)).Value = ID;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NomParteneri inst = new NomParteneri();
                            inst.IdPartener = Convert.ToInt64(reader["IdPartener"]);
                            inst.NumePartener = reader["NumePartener"] == DBNull.Value ? "" : reader["NumePartener"].ToString().Trim();
                            inst.CodFiscalNumar = reader["CodFiscalNumar"] == DBNull.Value ? "" : reader["CodFiscalNumar"].ToString().Trim();
                            inst.CodFiscalAtribut = reader["CodFiscalAtribut"] == DBNull.Value ? "" : reader["CodFiscalAtribut"].ToString().Trim();
                            inst.RegCom = reader["RegCom"] == DBNull.Value ? "" : reader["RegCom"].ToString().Trim();
                            inst.Banca = reader["Banca"] == DBNull.Value ? "" : reader["Banca"].ToString().Trim();
                            inst.ContBanca = reader["ContBanca"] == DBNull.Value ? "" : reader["ContBanca"].ToString().Trim();
                            inst.Oras = reader["Oras"] == DBNull.Value ? "" : reader["Oras"].ToString().Trim();
                            inst.Judet = reader["Judet"] == DBNull.Value ? "" : reader["Judet"].ToString().Trim();
                            inst.Telefon = reader["Telefon"] == DBNull.Value ? "" : reader["Telefon"].ToString().Trim();
                            inst.Patron = reader["Patron"] == DBNull.Value ? "" : reader["Patron"].ToString().Trim();
                            inst.Director = reader["Director"] == DBNull.Value ? "" : reader["Director"].ToString().Trim();
                            inst.Strada = reader["Strada"] == DBNull.Value ? "" : reader["Strada"].ToString().Trim();
                            inst.Nr = reader["Nr"] == DBNull.Value ? "" : reader["Nr"].ToString().Trim();
                            inst.Bloc = reader["Bloc"] == DBNull.Value ? "" : reader["Bloc"].ToString().Trim();
                            inst.Scara = reader["Scara"] == DBNull.Value ? "" : reader["Scara"].ToString().Trim();
                            inst.Etaj = reader["Etaj"] == DBNull.Value ? "" : reader["Etaj"].ToString().Trim();
                            inst.Apartament = reader["Apartament"] == DBNull.Value ? "" : reader["Apartament"].ToString().Trim();
                            inst.MailAddress = reader["MailAddress"] == DBNull.Value ? "" : reader["MailAddress"].ToString().Trim();
                            inst.WebAddress = reader["WebAddress"] == DBNull.Value ? "" : reader["WebAddress"].ToString().Trim();
                            inst.Sters = Convert.ToBoolean(reader["Sters"]);
                            inst.IdServer = reader["IdServer"] == DBNull.Value ? "" : reader["IdServer"].ToString().Trim();
                            inst.IdRemote = reader["IdRemote"] == DBNull.Value ? 0 : Convert.ToInt64(reader["IdRemote"]);
                            inst.IdgestICS = Convert.ToInt32(reader["IdgestICS"]);
                            inst.IdStrada = Convert.ToInt32(reader["IdStrada"]);
                            inst.TipJuridic = reader["TipJuridic"].ToString().Trim();
                            inst.Oras2 = reader["Oras2"] == DBNull.Value ? "" : reader["Oras2"].ToString().Trim();
                            inst.Judet2 = reader["Judet2"] == DBNull.Value ? "" : reader["Judet2"].ToString().Trim();
                            inst.Strada2 = reader["Strada2"] == DBNull.Value ? "" : reader["Strada2"].ToString().Trim();
                            inst.Nr2 = reader["Nr2"] == DBNull.Value ? "" : reader["Nr2"].ToString().Trim();
                            inst.Bloc2 = reader["Bloc2"] == DBNull.Value ? "" : reader["Bloc2"].ToString().Trim();
                            inst.Scara2 = reader["Scara2"] == DBNull.Value ? "" : reader["Scara2"].ToString().Trim();
                            inst.Etaj2 = reader["Etaj2"] == DBNull.Value ? "" : reader["Etaj2"].ToString().Trim();
                            inst.Apartament2 = reader["Apartament2"] == DBNull.Value ? "" : reader["Apartament2"].ToString().Trim();
                            inst.IdStrada2 = Convert.ToInt32(reader["IdStrada2"]);
                            inst.Nume = reader["Nume"] == DBNull.Value ? "" : reader["Nume"].ToString().Trim();
                            inst.Prenume = reader["Prenume"] == DBNull.Value ? "" : reader["Prenume"].ToString().Trim();
                            inst.Tip = reader["Tip"] == DBNull.Value ? "" : reader["Tip"].ToString().Trim();
                            inst.DetaliiSupFactura = reader["DetaliiSupFactura"].ToString().Trim();
                            inst.DataStare = reader["DataStare"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataStare"]);
                            inst.Stare = reader["Stare"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Stare"]);
                            inst.Administrator = reader["Administrator"] == DBNull.Value ? "" : reader["Administrator"].ToString().Trim();
                            inst.Adresa = reader["Adresa"] == DBNull.Value ? "" : reader["Adresa"].ToString().Trim();
                            inst.TelAdm = reader["TelAdm"] == DBNull.Value ? "" : reader["TelAdm"].ToString().Trim();
                            inst.Aplicatie = reader["Aplicatie"] == DBNull.Value ? "" : reader["Aplicatie"].ToString().Trim();
                            inst.Prescurtare = reader["Prescurtare"] == DBNull.Value ? "" : reader["Prescurtare"].ToString().Trim();
                            inst.EModificat = Convert.ToBoolean(reader["EModificat"]);
                            inst.CapitalSocial = reader["CapitalSocial"] == DBNull.Value ? "" : reader["CapitalSocial"].ToString().Trim();
                            inst.IdParinte = reader["IdParinte"] == DBNull.Value ? 0 : Convert.ToInt64(reader["IdParinte"]);
                            inst.Status = Convert.ToInt32(reader["Status"]);
                            inst.CodSIRUES = reader["CodSIRUES"] == DBNull.Value ? "" : reader["CodSIRUES"].ToString().Trim();
                            inst.Subunitate = reader["Subunitate"] == DBNull.Value ? "" : reader["Subunitate"].ToString().Trim();
                            inst.Cod = reader["Cod"] == DBNull.Value ? "" : reader["Cod"].ToString().Trim();
                            inst.Alias = reader["Alias"] == DBNull.Value ? "" : reader["Alias"].ToString().Trim();
                            inst.FilieraID = reader["FilieraID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["FilieraID"]);
                            inst.Filiera = reader["Filiera"] == DBNull.Value ? "" : reader["Filiera"].ToString().Trim();
                            inst.Tara = reader["Tara"] == DBNull.Value ? "" : reader["Tara"].ToString().Trim();
                            inst.ContDeEmail = reader["ContDeEmail"].ToString().Trim();
                            inst.IdJudet = Convert.ToInt64(reader["IdJudet"]);
                            
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

        public static List<NomParteneri> GetLista()
        {
            List<NomParteneri> rv = new List<NomParteneri>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT 
                                     [IdPartener]
                                    ,[NumePartener]
                                    ,[CodFiscalNumar]
                                    ,[CodFiscalAtribut]
                                    ,[RegCom]
                                    ,[Banca]
                                    ,[ContBanca]
                                    ,[Oras]
                                    ,[Judet]
                                    ,[Telefon]
                                    ,[Patron]
                                    ,[Director]
                                    ,[Strada]
                                    ,[Nr]
                                    ,[Bloc]
                                    ,[Scara]
                                    ,[Etaj]
                                    ,[Apartament]
                                    ,[MailAddress]
                                    ,[WebAddress]
                                    ,[Sters]
                                    ,[IdServer]
                                    ,[IdRemote]
                                    ,[IdgestICS]
                                    ,[IdStrada]
                                    ,[TipJuridic]
                                    ,[Oras2]
                                    ,[Judet2]
                                    ,[Strada2]
                                    ,[Nr2]
                                    ,[Bloc2]
                                    ,[Scara2]
                                    ,[Etaj2]
                                    ,[Apartament2]
                                    ,[IdStrada2]
                                    ,[Nume]
                                    ,[Prenume]
                                    ,[Tip]
                                    ,[DetaliiSupFactura]
                                    ,[DataStare]
                                    ,[Stare]
                                    ,[Administrator]
                                    ,[Adresa]
                                    ,[TelAdm]
                                    ,[Aplicatie]
                                    ,[Prescurtare]
                                    ,[EModificat]
                                    ,[CapitalSocial]
                                    ,[IdParinte]
                                    ,[Status]
                                    ,[CodSIRUES]
                                    ,[Subunitate]
                                    ,[Cod]
                                    ,[Alias]
                                    ,[FilieraID]
                                    ,[Filiera]
                                    ,[Tara]
                                    ,[ContDeEmail]
                                    ,[IdJudet]
                                FROM [SOLON].[dbo].[NomParteneri]";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NomParteneri inst = new NomParteneri();
                            inst.IdPartener = Convert.ToInt64(reader["IdPartener"]);
                            inst.NumePartener = reader["NumePartener"] == DBNull.Value ? "" : reader["NumePartener"].ToString();
                            inst.CodFiscalNumar = reader["CodFiscalNumar"] == DBNull.Value ? "" : reader["CodFiscalNumar"].ToString();
                            inst.CodFiscalAtribut = reader["CodFiscalAtribut"] == DBNull.Value ? "" : reader["CodFiscalAtribut"].ToString();
                            inst.RegCom = reader["RegCom"] == DBNull.Value ? "" : reader["RegCom"].ToString();
                            inst.Banca = reader["Banca"] == DBNull.Value ? "" : reader["Banca"].ToString();
                            inst.ContBanca = reader["ContBanca"] == DBNull.Value ? "" : reader["ContBanca"].ToString();
                            inst.Oras = reader["Oras"] == DBNull.Value ? "" : reader["Oras"].ToString();
                            inst.Judet = reader["Judet"] == DBNull.Value ? "" : reader["Judet"].ToString();
                            inst.Telefon = reader["Telefon"] == DBNull.Value ? "" : reader["Telefon"].ToString();
                            inst.Patron = reader["Patron"] == DBNull.Value ? "" : reader["Patron"].ToString();
                            inst.Director = reader["Director"] == DBNull.Value ? "" : reader["Director"].ToString();
                            inst.Strada = reader["Strada"] == DBNull.Value ? "" : reader["Strada"].ToString();
                            inst.Nr = reader["Nr"] == DBNull.Value ? "" : reader["Nr"].ToString();
                            inst.Bloc = reader["Bloc"] == DBNull.Value ? "" : reader["Bloc"].ToString();
                            inst.Scara = reader["Scara"] == DBNull.Value ? "" : reader["Scara"].ToString();
                            inst.Etaj = reader["Etaj"] == DBNull.Value ? "" : reader["Etaj"].ToString();
                            inst.Apartament = reader["Apartament"] == DBNull.Value ? "" : reader["Apartament"].ToString();
                            inst.MailAddress = reader["MailAddress"] == DBNull.Value ? "" : reader["MailAddress"].ToString();
                            inst.WebAddress = reader["WebAddress"] == DBNull.Value ? "" : reader["WebAddress"].ToString();
                            inst.Sters = Convert.ToBoolean(reader["Sters"]);
                            inst.IdServer = reader["IdServer"] == DBNull.Value ? "" : reader["IdServer"].ToString();
                            inst.IdRemote = reader["IdRemote"] == DBNull.Value ? 0 : Convert.ToInt64(reader["IdRemote"]);
                            inst.IdgestICS = Convert.ToInt32(reader["IdgestICS"]);
                            inst.IdStrada = Convert.ToInt32(reader["IdStrada"]);
                            inst.TipJuridic = reader["TipJuridic"].ToString();
                            inst.Oras2 = reader["Oras2"] == DBNull.Value ? "" : reader["Oras2"].ToString();
                            inst.Judet2 = reader["Judet2"] == DBNull.Value ? "" : reader["Judet2"].ToString();
                            inst.Strada2 = reader["Strada2"] == DBNull.Value ? "" : reader["Strada2"].ToString();
                            inst.Nr2 = reader["Nr2"] == DBNull.Value ? "" : reader["Nr2"].ToString();
                            inst.Bloc2 = reader["Bloc2"] == DBNull.Value ? "" : reader["Bloc2"].ToString();
                            inst.Scara2 = reader["Scara2"] == DBNull.Value ? "" : reader["Scara2"].ToString();
                            inst.Etaj2 = reader["Etaj2"] == DBNull.Value ? "" : reader["Etaj2"].ToString();
                            inst.Apartament2 = reader["Apartament2"] == DBNull.Value ? "" : reader["Apartament2"].ToString();
                            inst.IdStrada2 = Convert.ToInt32(reader["IdStrada2"]);
                            inst.Nume = reader["Nume"] == DBNull.Value ? "" : reader["Nume"].ToString();
                            inst.Prenume = reader["Prenume"] == DBNull.Value ? "" : reader["Prenume"].ToString();
                            inst.Tip = reader["Tip"] == DBNull.Value ? "" : reader["Tip"].ToString();
                            inst.DetaliiSupFactura = reader["DetaliiSupFactura"].ToString();
                            inst.DataStare = reader["DataStare"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataStare"]);
                            inst.Stare = reader["Stare"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Stare"]);
                            inst.Administrator = reader["Administrator"] == DBNull.Value ? "" : reader["Administrator"].ToString();
                            inst.Adresa = reader["Adresa"] == DBNull.Value ? "" : reader["Adresa"].ToString();
                            inst.TelAdm = reader["TelAdm"] == DBNull.Value ? "" : reader["TelAdm"].ToString();
                            inst.Aplicatie = reader["Aplicatie"] == DBNull.Value ? "" : reader["Aplicatie"].ToString();
                            inst.Prescurtare = reader["Prescurtare"] == DBNull.Value ? "" : reader["Prescurtare"].ToString();
                            inst.EModificat = Convert.ToBoolean(reader["EModificat"]);
                            inst.CapitalSocial = reader["CapitalSocial"] == DBNull.Value ? "" : reader["CapitalSocial"].ToString();
                            inst.IdParinte = reader["IdParinte"] == DBNull.Value ? 0 : Convert.ToInt64(reader["IdParinte"]);
                            inst.Status = Convert.ToInt32(reader["Status"]);
                            inst.CodSIRUES = reader["CodSIRUES"] == DBNull.Value ? "" : reader["CodSIRUES"].ToString();
                            inst.Subunitate = reader["Subunitate"] == DBNull.Value ? "" : reader["Subunitate"].ToString();
                            inst.Cod = reader["Cod"] == DBNull.Value ? "" : reader["Cod"].ToString();
                            inst.Alias = reader["Alias"] == DBNull.Value ? "" : reader["Alias"].ToString();
                            inst.FilieraID = reader["FilieraID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["FilieraID"]);
                            inst.Filiera = reader["Filiera"] == DBNull.Value ? "" : reader["Filiera"].ToString();
                            inst.Tara = reader["Tara"] == DBNull.Value ? "" : reader["Tara"].ToString();
                            inst.ContDeEmail = reader["ContDeEmail"].ToString();
                            inst.IdJudet = Convert.ToInt64(reader["IdJudet"]);

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

        public static int Insert(NomParteneri p, int idRezCamera)
        {
            int rv = 0;
            bool exista = false;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT  
                                     [IdPartener]
                                    ,[NumePartener]
                                    ,[CodFiscalNumar]
                                    ,[CodFiscalAtribut]
                                    ,[RegCom]
                                    ,[Banca]
                                    ,[ContBanca]
                                    ,[Oras]
                                    ,[Judet]
                                    ,[Telefon]
                                    ,[Patron]
                                    ,[Director]
                                    ,[Strada]
                                    ,[Nr]
                                    ,[Bloc]
                                    ,[Scara]
                                    ,[Etaj]
                                    ,[Apartament]
                                    ,[MailAddress]
                                    ,[WebAddress]
                                    ,[Sters]
                                    ,[IdServer]
                                    ,[IdRemote]
                                    ,[IdgestICS]
                                    ,[IdStrada]
                                    ,[TipJuridic]
                                    ,[Oras2]
                                    ,[Judet2]
                                    ,[Strada2]
                                    ,[Nr2]
                                    ,[Bloc2]
                                    ,[Scara2]
                                    ,[Etaj2]
                                    ,[Apartament2]
                                    ,[IdStrada2]
                                    ,[Nume]
                                    ,[Prenume]
                                    ,[Tip]
                                    ,[DetaliiSupFactura]
                                    ,[DataStare]
                                    ,[Stare]
                                    ,[Administrator]
                                    ,[Adresa]
                                    ,[TelAdm]
                                    ,[Aplicatie]
                                    ,[Prescurtare]
                                    ,[EModificat]
                                    ,[CapitalSocial]
                                    ,[IdParinte]
                                    ,[Status]
                                    ,[CodSIRUES]
                                    ,[Subunitate]
                                    ,[Cod]
                                    ,[Alias]
                                    ,[FilieraID]
                                    ,[Filiera]
                                    ,[Tara]
                                    ,[ContDeEmail]
                                    ,[IdJudet]
                                FROM [SOLON].[dbo].[NomParteneri]
                                WHERE [IdPartener]=@IdPartener;";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdPartener", SqlDbType.BigInt)).Value = p.IdPartener;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            exista = true;
                            rv = Convert.ToInt32(reader["IdPartener"]);
                        }
                    }

                    if (exista == false)
                    {
                        sql = @"INSERT INTO [SOLON].[dbo].[NomParteneri]
                                                    ([NumePartener]
                                                    ,[CodFiscalNumar]
                                                    ,[CodFiscalAtribut]
                                                    ,[RegCom]
                                                    ,[Banca]
                                                    ,[ContBanca]
                                                    ,[Oras]
                                                    ,[Judet]
                                                    ,[Telefon]
                                                    ,[Patron]
                                                    ,[Director]
                                                    ,[Strada]
                                                    ,[Nr]
                                                    ,[Bloc]
                                                    ,[Scara]
                                                    ,[Etaj]
                                                    ,[Apartament]
                                                    ,[MailAddress]
                                                    ,[WebAddress]
                                                    ,[Sters]
                                                    ,[IdServer]
                                                    ,[IdRemote]
                                                    ,[IdgestICS]
                                                    ,[IdStrada]
                                                    ,[TipJuridic]
                                                    ,[Oras2]
                                                    ,[Judet2]
                                                    ,[Strada2]
                                                    ,[Nr2]
                                                    ,[Bloc2]
                                                    ,[Scara2]
                                                    ,[Etaj2]
                                                    ,[Apartament2]
                                                    ,[IdStrada2]
                                                    ,[Nume]
                                                    ,[Prenume]
                                                    ,[Tip]
                                                    ,[DetaliiSupFactura]
                                                    --,[DataStare]
                                                    ,[Stare]
                                                    ,[Administrator]
                                                    ,[Adresa]
                                                    ,[TelAdm]
                                                    ,[Aplicatie]
                                                    ,[Prescurtare]
                                                    ,[EModificat]
                                                    ,[CapitalSocial]
                                                    ,[IdParinte]
                                                    ,[Status]
                                                    ,[CodSIRUES]
                                                    ,[Subunitate]
                                                    ,[Cod]
                                                    ,[Alias]
                                                    ,[FilieraID]
                                                    ,[Filiera]
                                                    ,[Tara]
                                                    ,[ContDeEmail]
                                                    ,[IdJudet])
                                                VALUES
                                                    (@NumePartener
                                                    ,@CodFiscalNumar
                                                    ,@CodFiscalAtribut
                                                    ,@RegCom
                                                    ,@Banca
                                                    ,@ContBanca
                                                    ,@Oras
                                                    ,@Judet
                                                    ,@Telefon
                                                    ,@Patron
                                                    ,@Director
                                                    ,@Strada
                                                    ,@Nr
                                                    ,@Bloc
                                                    ,@Scara
                                                    ,@Etaj
                                                    ,@Apartament
                                                    ,@MailAddress
                                                    ,@WebAddress
                                                    ,@Sters
                                                    ,@IdServer
                                                    ,@IdRemote
                                                    ,@IdgestICS
                                                    ,@IdStrada
                                                    ,@TipJuridic
                                                    ,@Oras2
                                                    ,@Judet2
                                                    ,@Strada2
                                                    ,@Nr2
                                                    ,@Bloc2
                                                    ,@Scara2
                                                    ,@Etaj2
                                                    ,@Apartament2
                                                    ,@IdStrada2
                                                    ,@Nume
                                                    ,@Prenume
                                                    ,@Tip
                                                    ,@DetaliiSupFactura
                                                    --,@DataStare
                                                    ,@Stare
                                                    ,@Administrator
                                                    ,@Adresa
                                                    ,@TelAdm
                                                    ,@Aplicatie
                                                    ,@Prescurtare
                                                    ,@EModificat
                                                    ,@CapitalSocial
                                                    ,@IdParinte
                                                    ,@Status
                                                    ,@CodSIRUES
                                                    ,@Subunitate
                                                    ,@Cod
                                                    ,@Alias
                                                    ,@FilieraID
                                                    ,@Filiera
                                                    ,@Tara
                                                    ,@ContDeEmail
                                                    ,@IdJudet);
                                                 SELECT SCOPE_IDENTITY();";
                        cmd = new SqlCommand(sql, cnn);

                        cmd.Parameters.Add(new SqlParameter("@IdPartener", SqlDbType.BigInt)).Value = p.IdPartener;
                        cmd.Parameters.Add(new SqlParameter("@NumePartener", SqlDbType.VarChar)).Value = p.NumePartener;
                        cmd.Parameters.Add(new SqlParameter("@CodFiscalNumar", SqlDbType.VarChar)).Value = p.CodFiscalNumar;
                        cmd.Parameters.Add(new SqlParameter("@CodFiscalAtribut", SqlDbType.VarChar)).Value = p.CodFiscalAtribut;
                        cmd.Parameters.Add(new SqlParameter("@RegCom", SqlDbType.VarChar)).Value = p.RegCom;
                        cmd.Parameters.Add(new SqlParameter("@Banca", SqlDbType.VarChar)).Value = p.Banca;
                        cmd.Parameters.Add(new SqlParameter("@ContBanca", SqlDbType.VarChar)).Value = p.ContBanca;
                        cmd.Parameters.Add(new SqlParameter("@Oras", SqlDbType.VarChar)).Value = p.Oras;
                        cmd.Parameters.Add(new SqlParameter("@Judet", SqlDbType.VarChar)).Value = p.Judet;
                        cmd.Parameters.Add(new SqlParameter("@Telefon", SqlDbType.VarChar)).Value = p.Telefon;
                        cmd.Parameters.Add(new SqlParameter("@Patron", SqlDbType.VarChar)).Value = p.Patron;
                        cmd.Parameters.Add(new SqlParameter("@Director", SqlDbType.VarChar)).Value = p.Director;
                        cmd.Parameters.Add(new SqlParameter("@Strada", SqlDbType.VarChar)).Value = p.Strada;
                        cmd.Parameters.Add(new SqlParameter("@Nr", SqlDbType.VarChar)).Value = p.Nr;
                        cmd.Parameters.Add(new SqlParameter("@Bloc", SqlDbType.VarChar)).Value = p.Bloc;
                        cmd.Parameters.Add(new SqlParameter("@Scara", SqlDbType.VarChar)).Value = p.Scara;
                        cmd.Parameters.Add(new SqlParameter("@Etaj", SqlDbType.VarChar)).Value = p.Etaj;
                        cmd.Parameters.Add(new SqlParameter("@Apartament", SqlDbType.VarChar)).Value = p.Apartament;
                        cmd.Parameters.Add(new SqlParameter("@MailAddress", SqlDbType.VarChar)).Value = p.MailAddress;
                        cmd.Parameters.Add(new SqlParameter("@WebAddress", SqlDbType.VarChar)).Value = p.WebAddress;
                        cmd.Parameters.Add(new SqlParameter("@Sters", SqlDbType.Bit)).Value = p.Sters;
                        cmd.Parameters.Add(new SqlParameter("@IdServer", SqlDbType.VarChar)).Value = p.IdServer;
                        cmd.Parameters.Add(new SqlParameter("@IdRemote", SqlDbType.BigInt)).Value = p.IdRemote;
                        cmd.Parameters.Add(new SqlParameter("@IdgestICS", SqlDbType.Int)).Value = p.IdgestICS;
                        cmd.Parameters.Add(new SqlParameter("@IdStrada", SqlDbType.Int)).Value = p.IdStrada;
                        cmd.Parameters.Add(new SqlParameter("@TipJuridic", SqlDbType.VarChar)).Value = p.TipJuridic;
                        cmd.Parameters.Add(new SqlParameter("@Oras2", SqlDbType.VarChar)).Value = p.Oras2;
                        cmd.Parameters.Add(new SqlParameter("@Judet2", SqlDbType.VarChar)).Value = p.Judet2;
                        cmd.Parameters.Add(new SqlParameter("@Strada2", SqlDbType.VarChar)).Value = p.Strada2;
                        cmd.Parameters.Add(new SqlParameter("@Nr2", SqlDbType.VarChar)).Value = p.Nr2;
                        cmd.Parameters.Add(new SqlParameter("@Bloc2", SqlDbType.VarChar)).Value = p.Bloc2;
                        cmd.Parameters.Add(new SqlParameter("@Scara2", SqlDbType.VarChar)).Value = p.Scara2;
                        cmd.Parameters.Add(new SqlParameter("@Etaj2", SqlDbType.VarChar)).Value = p.Etaj2;
                        cmd.Parameters.Add(new SqlParameter("@Apartament2", SqlDbType.VarChar)).Value = p.Apartament2;
                        cmd.Parameters.Add(new SqlParameter("@IdStrada2", SqlDbType.Int)).Value = p.IdStrada2;
                        cmd.Parameters.Add(new SqlParameter("@Nume", SqlDbType.NVarChar)).Value = p.Nume;
                        cmd.Parameters.Add(new SqlParameter("@Prenume", SqlDbType.NVarChar)).Value = p.Prenume;
                        cmd.Parameters.Add(new SqlParameter("@Tip", SqlDbType.VarChar)).Value = p.Tip;
                        cmd.Parameters.Add(new SqlParameter("@DetaliiSupFactura", SqlDbType.VarChar)).Value = p.DetaliiSupFactura;
                        //cmd.Parameters.Add(new SqlParameter("@DataStare", SqlDbType.DateTime)).Value = p.DataStare;
                        cmd.Parameters.Add(new SqlParameter("@Stare", SqlDbType.Int)).Value = p.Stare;
                        cmd.Parameters.Add(new SqlParameter("@Administrator", SqlDbType.NVarChar)).Value = p.Administrator;
                        cmd.Parameters.Add(new SqlParameter("@Adresa", SqlDbType.NVarChar)).Value = p.Adresa;
                        cmd.Parameters.Add(new SqlParameter("@TelAdm", SqlDbType.NVarChar)).Value = p.TelAdm;
                        cmd.Parameters.Add(new SqlParameter("@Aplicatie", SqlDbType.NVarChar)).Value = p.Aplicatie;
                        cmd.Parameters.Add(new SqlParameter("@Prescurtare", SqlDbType.VarChar)).Value = p.Prescurtare;
                        cmd.Parameters.Add(new SqlParameter("@EModificat", SqlDbType.Bit)).Value = p.EModificat;
                        cmd.Parameters.Add(new SqlParameter("@CapitalSocial", SqlDbType.NVarChar)).Value = p.CapitalSocial;
                        cmd.Parameters.Add(new SqlParameter("@IdParinte", SqlDbType.BigInt)).Value = p.IdParinte;
                        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int)).Value = p.Status;
                        cmd.Parameters.Add(new SqlParameter("@CodSIRUES", SqlDbType.NVarChar)).Value = p.CodSIRUES;
                        cmd.Parameters.Add(new SqlParameter("@Subunitate", SqlDbType.NVarChar)).Value = p.Subunitate;
                        cmd.Parameters.Add(new SqlParameter("@Cod", SqlDbType.NVarChar)).Value = p.Cod;
                        cmd.Parameters.Add(new SqlParameter("@Alias", SqlDbType.NVarChar)).Value = p.Alias;
                        cmd.Parameters.Add(new SqlParameter("@FilieraID", SqlDbType.Int)).Value = p.FilieraID;
                        cmd.Parameters.Add(new SqlParameter("@Filiera", SqlDbType.NVarChar)).Value = p.Filiera;
                        cmd.Parameters.Add(new SqlParameter("@Tara", SqlDbType.NVarChar)).Value = p.Tara;
                        cmd.Parameters.Add(new SqlParameter("@ContDeEmail", SqlDbType.VarChar)).Value = p.ContDeEmail;
                        cmd.Parameters.Add(new SqlParameter("@IdJudet", SqlDbType.BigInt)).Value = p.IdJudet;

                        rv = Convert.ToInt32(cmd.ExecuteScalar());

                        try
                        {
                            if (rv > 0 && idRezCamera > 0)
                            {
                                //modifica rezervari camere set id turist = @rv where ID = @idRezCamera
                                sql = @"UPDATE [SOLON.H].[hotel].[RezervariCamere]
                                                   SET 
                                                      [IdTurist] = @IdTurist
                                                 WHERE ID=@ID";
                                cmd = new SqlCommand(sql, cnn);
                                cmd.Parameters.Add(new SqlParameter("@IdTurist", SqlDbType.BigInt)).Value = rv;
                                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)).Value = idRezCamera;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception exc) { }
                    }
                    else
                    {
                        //update where id=rv
//                        sql = @"UPDATE [SOLON].[dbo].[NomParteneri]
//                                    SET
//	                                    [ContDeEmail] = @ContDeEmail
//                                       ,[MailAddress] = @ContDeEmail
//                                    WHERE
//                                        [IdPartener] = @IdPartener;";
                        sql = @"
                                --declare @Nume as nvarchar(64);
                                --declare @Prenume as nvarchar(64);
                                --declare @MailAddress as varchar(30);
                                --declare @ContDeEmail as varchar(100);
                                --declare @Adresa as nvarchar(256);
                                --declare @Oras as varchar(30);
                                --declare @Tara as nvarchar(32);
                                --declare @Telefon as varchar(20);
                                --declare @CodFiscalNumar as varchar(15);
                                --declare @NumePartener as varchar(100);
                                --declare @RegCom as varchar(30);

                                UPDATE [SOLON].[dbo].[NomParteneri]
                                   SET  Nume = CASE 
			                                WHEN (Nume = '' OR Nume IS NULL) THEN @Nume
			                                ELSE Nume
                                        END,
         
                                        Prenume = CASE 
			                                WHEN (Prenume = '' OR Prenume IS NULL) THEN @Prenume
			                                ELSE Prenume
                                        END,
        
                                        MailAddress = @MailAddress,
                                        ContDeEmail=@ContDeEmail,
        
                                        Adresa = CASE 
			                                WHEN (Adresa = '' OR Adresa IS NULL) THEN @Adresa
			                                ELSE Adresa
                                        END,
        
                                        Oras = CASE 
			                                WHEN (Oras = '' OR Oras IS NULL) THEN @Oras
			                                ELSE Oras
                                        END,
        
                                        Tara = CASE 
			                                WHEN (Tara = '' OR Tara IS NULL) THEN @Tara
			                                ELSE Tara
                                        END,
        
                                        Telefon = CASE 
			                                WHEN (Telefon = '' OR Telefon IS NULL) THEN @Telefon
			                                ELSE Telefon
                                        END,
        
                                        CodFiscalNumar = CASE 
			                                WHEN (CodFiscalNumar = '' OR CodFiscalNumar IS NULL) THEN @CodFiscalNumar
			                                ELSE CodFiscalNumar
                                        END,
        
                                        NumePartener = CASE 
			                                WHEN (NumePartener = '' OR NumePartener IS NULL) THEN @NumePartener
			                                ELSE NumePartener
                                        END,
        
                                        RegCom = CASE 
			                                WHEN (RegCom = '' OR RegCom IS NULL) THEN @RegCom
			                                ELSE RegCom
                                        END
       
                                 WHERE IdPartener=@IdPartener
                                ";
                        cmd = new SqlCommand(sql, cnn);
                        cmd.Parameters.Add(new SqlParameter("@Nume", SqlDbType.NVarChar)).Value = !String.IsNullOrEmpty(p.Nume) && p.Nume != "null" ? p.Nume : "";
                        cmd.Parameters.Add(new SqlParameter("@Prenume", SqlDbType.NVarChar)).Value = !String.IsNullOrEmpty(p.Prenume) && p.Nume != "null" ? p.Prenume : ""; 
                        cmd.Parameters.Add(new SqlParameter("@MailAddress", SqlDbType.VarChar)).Value = p.MailAddress;
                        cmd.Parameters.Add(new SqlParameter("@ContDeEmail", SqlDbType.VarChar)).Value = p.ContDeEmail;
                        cmd.Parameters.Add(new SqlParameter("@Adresa", SqlDbType.NVarChar)).Value = !String.IsNullOrEmpty(p.Adresa) && p.Adresa != "null" ? p.Adresa : ""; 
                        cmd.Parameters.Add(new SqlParameter("@Oras", SqlDbType.VarChar)).Value = !String.IsNullOrEmpty(p.Oras) && p.Oras != "null" ? p.Oras : ""; 
                        cmd.Parameters.Add(new SqlParameter("@Tara", SqlDbType.NVarChar)).Value = !String.IsNullOrEmpty(p.Tara) && p.Tara != "null" ? p.Tara : ""; 
                        cmd.Parameters.Add(new SqlParameter("@Telefon", SqlDbType.VarChar)).Value = !String.IsNullOrEmpty(p.Telefon) && p.Telefon != "null" ? p.Telefon : ""; 
                        cmd.Parameters.Add(new SqlParameter("@CodFiscalNumar", SqlDbType.VarChar)).Value = !String.IsNullOrEmpty(p.CodFiscalNumar) && p.CodFiscalNumar != "null" ? p.CodFiscalNumar : ""; 
                        cmd.Parameters.Add(new SqlParameter("@NumePartener", SqlDbType.VarChar)).Value = !String.IsNullOrEmpty(p.NumePartener) && p.NumePartener != "null" ? p.NumePartener : ""; 
                        cmd.Parameters.Add(new SqlParameter("@RegCom", SqlDbType.VarChar)).Value = !String.IsNullOrEmpty(p.RegCom) && p.RegCom != "null" ? p.RegCom : "";
                        cmd.Parameters.Add(new SqlParameter("@IdPartener", SqlDbType.BigInt)).Value = rv;
                        int ok=cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception exc)
                {
                    return 0;
                }
            }
            return rv;
        }

        public static int Edit(string ContDeEmail, int IdPartener)
        {
            int rv = 0;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"UPDATE [SOLON].[dbo].[NomParteneri]
                                    SET
	                                    [ContDeEmail] = @ContDeEmail
                                    WHERE
                                        [IdPartener] = @IdPartener;";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@ContDeEmail", SqlDbType.VarChar)).Value = ContDeEmail;
                    cmd.Parameters.Add(new SqlParameter("@IdPartener", SqlDbType.BigInt)).Value = IdPartener;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    return 0;
                }
            }
            return rv;
        }
    }
}