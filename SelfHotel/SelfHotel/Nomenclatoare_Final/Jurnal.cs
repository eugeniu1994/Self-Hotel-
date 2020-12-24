using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class Jurnal
    {
        public static int IDUtilizator { get; set; }//login
        public static int IDFirma { get; set; }//login
        public static int IDObiect { get; set; }//by nume
        public static int IDActiune { get; set; }//by nume
        public static int Importanta { get; set; }//manual
        public static string Detalii { get; set; }//manual
        public static string IPComputer { get; set; }//din system
        public static string NumeComputer { get; set; }//din system
        public static DateTime DataComputer { get; set; }//DateTime.Now
        public static string UserComputer { get; set; }//din system

        public static string GetIp()
        {
            IPHostEntry host;
            string IPLocal = "127.0.0.1";
            try
            {
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        IPLocal = ip.ToString();
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                LogErori.SalveazaFaraMesaj(exc, "Jurnal.GetIp()");
            }
            
            return IPLocal;
        }
        private static string GetNumeComputer()
        {
            return "Kiosk";// System.Windows.Forms.SystemInformation.ComputerName;
        }
        private static string GetUserComputer()
        {
            return "Kiosk";// System.Windows.Forms.SystemInformation.UserName;
        }
        private static int GetObiectId(String Obiect)
        {
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand(@"
                            SELECT [IdObiect]
                              FROM [SOLON].[dbo].[NomObiecte]
                            WHERE [Obiect]=@Obiect", cnn);
                    cmd.Parameters.Add(new SqlParameter("@Obiect", SqlDbType.VarChar)).Value = Obiect;
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch { return 0; }
            }
        }

        private static int GetObiectId(String Obiect, SqlConnection cnn, SqlTransaction tran)
        {
            try
            {
                string sql = @"
                    SELECT
                        [IdObiect]
                    FROM
                        [SOLON].[dbo].[NomObiecte]
                    WHERE
                        [Obiect] = @Obiect";
                SqlCommand cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@Obiect", SqlDbType.VarChar)).Value = Obiect;
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch { return 0; }
        }

        public static int ImportantaMare { get { return 2; } }
        public static int ImportantaMedie { get { return 1; } }
        public static int ImportantaMica { get { return 0; } }

        public struct Actiune
        {
            private static int _VIZ = 0;
            public static int VIZ
            {
                get
                {
                    if (_VIZ == 0)
                    {
                        using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                        {
                            try
                            {
                                cnn.Open();
                                SqlCommand cmd = new SqlCommand(@"SELECT [ID] FROM [SOLON].[dbo].[NomActiuni] WHERE [Denumire]='VIZ'", cnn);
                                _VIZ = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            catch { }
                        }
                    }
                    return _VIZ;
                }
            }

            private static int _ADD = 0;
            public static int ADD
            {
                get
                {
                    if (_ADD == 0)
                    {
                        using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                        {
                            try
                            {
                                cnn.Open();
                                SqlCommand cmd = new SqlCommand(@"SELECT [ID] FROM [SOLON].[dbo].[NomActiuni] WHERE [Denumire]='ADD'", cnn);
                                return Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            catch { }
                        }
                    }
                    return _ADD;
                }
            }

            private static int _MOD = 0;
            public static int MOD
            {
                get
                {
                    if (_MOD == 0)
                    {
                        using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                        {
                            try
                            {
                                cnn.Open();
                                SqlCommand cmd = new SqlCommand(@"SELECT [ID] FROM [SOLON].[dbo].[NomActiuni] WHERE [Denumire]='MOD'", cnn);
                                return Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            catch { }
                        }
                    }
                    return _MOD;
                }
            }

            private static int _DEL = 0;
            public static int DEL
            {
                get
                {
                    if (_DEL == 0)
                    {
                        using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                        {
                            try
                            {
                                cnn.Open();
                                SqlCommand cmd = new SqlCommand(@"SELECT [ID] FROM [SOLON].[dbo].[NomActiuni] WHERE [Denumire]='DEL'", cnn);
                                return Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            catch { }
                        }
                    }
                    return _DEL;
                }
            }

            private static int _ADM = 0;
            public static int ADM
            {
                get
                {
                    if (_ADM == 0)
                    {
                        using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                        {
                            try
                            {
                                cnn.Open();
                                SqlCommand cmd = new SqlCommand(@"SELECT [ID] FROM [SOLON].[dbo].[NomActiuni] WHERE [Denumire]='ADM'", cnn);
                                return Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            catch { }
                        }
                    }
                    return _ADM;
                }
            }

            private static int _LIS = 0;
            public static int LIS
            {
                get
                {
                    if (_LIS == 0)
                    {
                        using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                        {
                            try
                            {
                                cnn.Open();
                                SqlCommand cmd = new SqlCommand(@"SELECT [ID] FROM [SOLON].[dbo].[NomActiuni] WHERE [Denumire]='LIS'", cnn);
                                return Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            catch { }
                        }
                    }
                    return _LIS;
                }
            }

            private static int _EXP = 0;
            public static int EXP
            {
                get
                {
                    if (_EXP == 0)
                    {
                        using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                        {
                            try
                            {
                                cnn.Open();
                                SqlCommand cmd = new SqlCommand(@"SELECT [ID] FROM [SOLON].[dbo].[NomActiuni] WHERE [Denumire]='EXP'", cnn);
                                return Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            catch { }
                        }
                    }
                    return _EXP;
                }
            }

            private static int _COR = 0;
            public static int COR
            {
                get
                {
                    if (_COR == 0)
                    {
                        using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                        {
                            try
                            {
                                cnn.Open();
                                SqlCommand cmd = new SqlCommand(@"SELECT [ID] FROM [SOLON].[dbo].[NomActiuni] WHERE [Denumire]='COR'", cnn);
                                return Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            catch { }
                        }
                    }
                    return _COR;
                }
            }

            private static int _INC = 0;
            public static int INC
            {
                get
                {
                    if (_INC == 0)
                    {
                        using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                        {
                            try
                            {
                                cnn.Open();
                                SqlCommand cmd = new SqlCommand(@"SELECT [ID] FROM [SOLON].[dbo].[NomActiuni] WHERE [Denumire]='INC'", cnn);
                                return Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            catch { }
                        }
                    }
                    return _INC;
                }
            }

            private static int _ANU = 0;
            public static int ANU
            {
                get
                {
                    if (_ANU == 0)
                    {
                        using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                        {
                            try
                            {
                                cnn.Open();
                                SqlCommand cmd = new SqlCommand(@"SELECT [ID] FROM [SOLON].[dbo].[NomActiuni] WHERE [Denumire]='ANU'", cnn);
                                return Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            catch { }
                        }
                    }
                    return _ANU;
                }
            }
        }

        public static void Salveaza(String Obiect, int Actiune, int Importanta, String Detalii)
        {
            try
            {
                IDObiect = GetObiectId(Obiect);
                if (IDObiect == 0)
                {
                    using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                    {
                        IDFirma = ConexiuneDB.IdFirma;
                        IDUtilizator = ConexiuneDB.IdUtilizator;

                        try
                        {
                            cnn.Open();
                            string sql = @"IF NOT EXISTS(SELECT IdObiect FROM [SOLON].[dbo].[NomObiecte] WHERE Obiect = @Obiect) 
                                    INSERT INTO [SOLON].[dbo].[NomObiecte] ([Obiect],[sters]) VALUES (@Obiect, 0)";
                            SqlCommand cmd = new SqlCommand(sql, cnn);
                            cmd.Parameters.Add(new SqlParameter("@Obiect", SqlDbType.VarChar)).Value = Obiect;
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception exc)
                        {
                            LogErori.SalveazaFaraMesaj(exc, "Jurnal.Salveaza()");
                        }
                    }
                    return;
                }

                using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                {
                    IDFirma = ConexiuneDB.IdFirma;
                    IDUtilizator = ConexiuneDB.IdUtilizator;

                    try
                    {
                        cnn.Open();
                        string sql = @"
                        INSERT INTO [SOLON].[dbo].[Jurnal]
                            ([MachineDate]
                            ,[MachineName]
                            ,[MachineIP]
                            ,[MachineUser]
                            ,[SolOnUserID]
                            ,[FirmaID]
                            ,[ObiectID]
                            ,[ActionID]
                            ,[Details]
                            ,[Importanta]
                            ,[VersiuneProgram]
                            ,[IdLocatie])
                        VALUES
                            (@MachineDate
                            ,@MachineName
                            ,@MachineIP
                            ,@MachineUser
                            ,@SolOnUserID
                            ,@FirmaID
                            ,@ObiectID
                            ,@ActionID
                            ,@Details
                            ,@Importanta
                            ,@VersiuneProgram
                            ,@IdLocatie)
                    ";
                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.Parameters.Add(new SqlParameter("@MachineDate", SqlDbType.DateTime)).Value = DateTime.Now;
                        cmd.Parameters.Add(new SqlParameter("@MachineName", SqlDbType.NVarChar)).Value = GetNumeComputer();
                        cmd.Parameters.Add(new SqlParameter("@MachineIP", SqlDbType.VarChar)).Value = GetIp();
                        cmd.Parameters.Add(new SqlParameter("@MachineUser", SqlDbType.VarChar)).Value = GetUserComputer();
                        cmd.Parameters.Add(new SqlParameter("@SolOnUserID", SqlDbType.Int)).Value = IDUtilizator;
                        cmd.Parameters.Add(new SqlParameter("@FirmaID", SqlDbType.BigInt)).Value = IDFirma;
                        cmd.Parameters.Add(new SqlParameter("@ObiectID", SqlDbType.Int)).Value = IDObiect;
                        cmd.Parameters.Add(new SqlParameter("@ActionID", SqlDbType.SmallInt)).Value = Actiune;
                        cmd.Parameters.Add(new SqlParameter("@Details", SqlDbType.NText)).Value = Detalii;
                        cmd.Parameters.Add(new SqlParameter("@Importanta", SqlDbType.Int)).Value = Importanta;
                        cmd.Parameters.Add(new SqlParameter("@VersiuneProgram", SqlDbType.NVarChar)).Value = ConexiuneDB.Versiune;
                        cmd.Parameters.Add(new SqlParameter("@IdLocatie", SqlDbType.Int)).Value = ConexiuneDB.IdHotel;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    {
                        LogErori.Salveaza(exc, "Jurnal.Salveaza()");
                    }
                }
            }
            catch
            {}
        }

        public static void Salveaza(String Obiect, int Actiune, int Importanta, String Detalii, SqlConnection cnn, SqlTransaction tran)
        {
            IDObiect = GetObiectId(Obiect, cnn, tran);
            if (IDObiect == 0)
            {
                //inserez obiect nou
                return;
            }


            IDFirma = ConexiuneDB.IdFirma;
            IDUtilizator = ConexiuneDB.IdUtilizator;

            string sql = @"
                INSERT INTO [SOLON].[dbo].[Jurnal]
                    ([MachineDate]
                    ,[MachineName]
                    ,[MachineIP]
                    ,[MachineUser]
                    ,[SolOnUserID]
                    ,[FirmaID]
                    ,[ObiectID]
                    ,[ActionID]
                    ,[Details]
                    ,[Importanta]
                    ,[VersiuneProgram]
                    ,[IdLocatie])
                VALUES
                    (@MachineDate
                    ,@MachineName
                    ,@MachineIP
                    ,@MachineUser
                    ,@SolOnUserID
                    ,@FirmaID
                    ,@ObiectID
                    ,@ActionID
                    ,@Details
                    ,@Importanta
                    ,@VersiuneProgram
                    ,@IdLocatie)";
            SqlCommand cmd = new SqlCommand(sql, cnn, tran);
            cmd.Parameters.Add(new SqlParameter("@MachineDate", SqlDbType.DateTime)).Value = DateTime.Now;
            cmd.Parameters.Add(new SqlParameter("@MachineName", SqlDbType.NVarChar)).Value = GetNumeComputer();
            cmd.Parameters.Add(new SqlParameter("@MachineIP", SqlDbType.VarChar)).Value = GetIp();
            cmd.Parameters.Add(new SqlParameter("@MachineUser", SqlDbType.VarChar)).Value = GetUserComputer();
            cmd.Parameters.Add(new SqlParameter("@SolOnUserID", SqlDbType.Int)).Value = IDUtilizator;
            cmd.Parameters.Add(new SqlParameter("@FirmaID", SqlDbType.BigInt)).Value = IDFirma;
            cmd.Parameters.Add(new SqlParameter("@ObiectID", SqlDbType.Int)).Value = IDObiect;
            cmd.Parameters.Add(new SqlParameter("@ActionID", SqlDbType.SmallInt)).Value = Actiune;
            cmd.Parameters.Add(new SqlParameter("@Details", SqlDbType.NText)).Value = Detalii;
            cmd.Parameters.Add(new SqlParameter("@Importanta", SqlDbType.Int)).Value = Importanta;
            cmd.Parameters.Add(new SqlParameter("@VersiuneProgram", SqlDbType.NVarChar)).Value = ConexiuneDB.Versiune;
            cmd.Parameters.Add(new SqlParameter("@IdLocatie", SqlDbType.Int)).Value = ConexiuneDB.IdHotel;
            cmd.ExecuteNonQuery();
        }
    }
}