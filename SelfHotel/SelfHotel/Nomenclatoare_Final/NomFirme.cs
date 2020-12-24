using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class NomFirme
    {
        public Int32 IdFirma { get; set; }
        public Int32 IdTata { get; set; }
        public string Firma { get; set; }
        public string AttrFiscal { get; set; }
        public string CodFiscal { get; set; }
        public string RegCom { get; set; }
        public string Tara { get; set; }
        public string Judet { get; set; }
        public string Oras { get; set; }
        public string Strada { get; set; }
        public string Numar { get; set; }
        public string Bloc { get; set; }
        public string Scara { get; set; }
        public string Etaj { get; set; }
        public string Apartament { get; set; }
        public string CodPostal { get; set; }
        public string Telefon { get; set; }
        public string Fax { get; set; }
        public string Cont { get; set; }
        public string Banca { get; set; }
        public string WebPage { get; set; }
        public string Email { get; set; }
        public string FormaProp { get; set; }
        public string ActivitPrinc { get; set; }
        public string CodCAEN { get; set; }
        public decimal CapitalSocial { get; set; }
        public string DBName { get; set; }
        public Boolean Bugetar { get; set; }
        public Boolean Sters { get; set; }
        public string IdServer { get; set; }
        public Int32 IdRemote { get; set; }
        public string ProcentDeTvaDefault { get; set; }
        public string IPServer { get; set; }
        public string MODServer { get; set; }
        public string ProceduraCumulare { get; set; }
        public string DBReceptie { get; set; }
        public string DBSalarii { get; set; }
        public string TipFacturaPrint { get; set; }
        public string PunctDeLucru { get; set; }
        public string emailSMTPHost { get; set; }
        public string emailSMTPPort { get; set; }
        public Int32 emailSMTPTLS { get; set; }
        public string emailSMTPUserName { get; set; }
        public string emailSMTPPassword { get; set; }
        public string ftpHost { get; set; }
        public string ftpUserName { get; set; }
        public string ftpPassword { get; set; }
        public Int32 EsteCumul { get; set; }

        public static List<NomFirme> getLista()
        {
            List<NomFirme> rv = new List<NomFirme>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT [IdFirma]
                                      ,[IdTata]
                                      ,[Firma]
                                      ,[AttrFiscal]
                                      ,[CodFiscal]
                                      ,[RegCom]
                                      ,[Tara]
                                      ,[Judet]
                                      ,[Oras]
                                      ,[Strada]
                                      ,[Numar]
                                      ,[Bloc]
                                      ,[Scara]
                                      ,[Etaj]
                                      ,[Apartament]
                                      ,[CodPostal]
                                      ,[Telefon]
                                      ,[Fax]
                                      ,[Cont]
                                      ,[Banca]
                                      ,[WebPage]
                                      ,[Email]
                                      ,[FormaProp]
                                      ,[ActivitPrinc]
                                      ,[CodCAEN]
                                      ,[CapitalSocial]
                                      ,[DBName]
                                      ,[Bugetar]
                                      ,[Sters]
                                      ,[IdServer]
                                      ,[IdRemote]
                                      ,[ProcentDeTvaDefault]
                                      ,[IPServer]
                                      ,[MODServer]
                                      ,[ProceduraCumulare]
                                      ,[DBReceptie]
                                      ,[DBSalarii]
                                      ,[TipFacturaPrint]
                                      ,[PunctDeLucru]
                                      ,[emailSMTPHost]
                                      ,[emailSMTPPort]
                                      ,[emailSMTPTLS]
                                      ,[emailSMTPUserName]
                                      ,[emailSMTPPassword]
                                      ,[ftpHost]
                                      ,[ftpUserName]
                                      ,[ftpPassword]
                                      ,[EsteCumul]
                                  FROM [SOLON].[dbo].[NomFirme]";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NomFirme inst = new NomFirme();
                            inst.IdFirma = Convert.ToInt32(reader["IdFirma"]);
                            inst.IdTata = Convert.ToInt32(reader["IdTata"]);
                            inst.Firma = reader["Firma"].ToString();
                            inst.AttrFiscal = reader["AttrFiscal"] == DBNull.Value ? "" : reader["AttrFiscal"].ToString();
                            inst.CodFiscal = reader["CodFiscal"] == DBNull.Value ? "" : reader["CodFiscal"].ToString();
                            inst.RegCom = reader["RegCom"] == DBNull.Value ? "" : reader["RegCom"].ToString();
                            inst.Tara = reader["Tara"] == DBNull.Value ? "" : reader["Tara"].ToString();
                            inst.Judet = reader["Judet"] == DBNull.Value ? "" : reader["Judet"].ToString();
                            inst.Oras = reader["Oras"] == DBNull.Value ? "" : reader["Oras"].ToString();
                            inst.Strada = reader["Strada"] == DBNull.Value ? "" : reader["Strada"].ToString();
                            inst.Numar = reader["Numar"] == DBNull.Value ? "" : reader["Numar"].ToString();
                            inst.Bloc = reader["Bloc"] == DBNull.Value ? "" : reader["Bloc"].ToString();
                            inst.Scara = reader["Scara"] == DBNull.Value ? "" : reader["Scara"].ToString();
                            inst.Etaj = reader["Etaj"] == DBNull.Value ? "" : reader["Etaj"].ToString();
                            inst.Apartament = reader["Apartament"] == DBNull.Value ? "" : reader["Apartament"].ToString();
                            inst.CodPostal = reader["CodPostal"] == DBNull.Value ? "" : reader["CodPostal"].ToString();
                            inst.Telefon = reader["Telefon"] == DBNull.Value ? "" : reader["Telefon"].ToString();
                            inst.Fax = reader["Fax"] == DBNull.Value ? "" : reader["Fax"].ToString();
                            inst.Cont = reader["Cont"] == DBNull.Value ? "" : reader["Cont"].ToString();
                            inst.Banca = reader["Banca"] == DBNull.Value ? "" : reader["Banca"].ToString();
                            inst.WebPage = reader["WebPage"] == DBNull.Value ? "" : reader["WebPage"].ToString();
                            inst.Email = reader["Email"] == DBNull.Value ? "" : reader["Email"].ToString();
                            inst.FormaProp = reader["FormaProp"] == DBNull.Value ? "" : reader["FormaProp"].ToString();
                            inst.ActivitPrinc = reader["ActivitPrinc"] == DBNull.Value ? "" : reader["ActivitPrinc"].ToString();
                            inst.CodCAEN = reader["CodCAEN"] == DBNull.Value ? "" : reader["CodCAEN"].ToString();
                            inst.CapitalSocial = reader["CapitalSocial"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["CapitalSocial"]);
                            inst.DBName = reader["DBName"] == DBNull.Value ? "" : reader["DBName"].ToString();
                            inst.Bugetar = Convert.ToBoolean(reader["Bugetar"]);
                            inst.Sters = Convert.ToBoolean(reader["Sters"]);
                            inst.IdServer = reader["IdServer"] == DBNull.Value ? "" : reader["IdServer"].ToString();
                            inst.IdRemote = Convert.ToInt32(reader["IdRemote"]);
                            inst.ProcentDeTvaDefault = reader["ProcentDeTvaDefault"].ToString();
                            inst.IPServer = reader["IPServer"].ToString();
                            inst.MODServer = reader["MODServer"].ToString();
                            inst.ProceduraCumulare = reader["ProceduraCumulare"].ToString();
                            inst.DBReceptie = reader["DBReceptie"] == DBNull.Value ? "" : reader["DBReceptie"].ToString();
                            inst.DBSalarii = reader["DBSalarii"] == DBNull.Value ? "" : reader["DBSalarii"].ToString();
                            inst.TipFacturaPrint = reader["TipFacturaPrint"].ToString();
                            inst.PunctDeLucru = reader["PunctDeLucru"].ToString();
                            inst.emailSMTPHost = reader["emailSMTPHost"].ToString();
                            inst.emailSMTPPort = reader["emailSMTPPort"].ToString();
                            inst.emailSMTPTLS = Convert.ToInt32(reader["emailSMTPTLS"]);
                            inst.emailSMTPUserName = reader["emailSMTPUserName"].ToString();
                            inst.emailSMTPPassword = reader["emailSMTPPassword"].ToString();
                            inst.ftpHost = reader["ftpHost"].ToString();
                            inst.ftpUserName = reader["ftpUserName"].ToString();
                            inst.ftpPassword = reader["ftpPassword"].ToString();
                            inst.EsteCumul = Convert.ToInt32(reader["EsteCumul"]);

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