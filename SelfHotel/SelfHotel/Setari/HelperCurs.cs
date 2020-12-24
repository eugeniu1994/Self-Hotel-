using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Setari
{
    public class HelperCurs
    {
        public static string GetCodMoneda(int IdMoneda)
        {
            string rv = "";
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT [MonedaCod] FROM [SOLON.H].[dbo].[NomMonede] WHERE [Sters] = 0 AND [IdMoneda] = @IdMoneda";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdMoneda", SqlDbType.Int)).Value = IdMoneda;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            rv = reader[0].ToString();
                        }
                    }
                }
                catch (Exception exc)
                {
                    //Log.ExceptieSql(exc, "HelperCurs.GetCodMoneda()");
                }
            }
            return rv;
        }
        private static int _IdRON = 0;
        public static int IdRON
        {
            get
            {
                if (_IdRON == 0)
                {
                    _IdRON = GetIdRon();
                }
                return _IdRON;
            }
        }

        public static int GetIdRon()
        {
            int rv = 0;

            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT [IdMoneda] FROM [SOLON].[dbo].[NomMonede] WHERE [Sters] = 0 AND [EsteNationala] = 1";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            rv = Convert.ToInt32(reader[0]);
                        }
                    }
                }
                catch (Exception exc)
                {
                    //Log.ExceptieSql(exc, "HelperCurs.GetIdRon()");
                }
            }

            return rv;
        }

        public static Int32 GetTipCursValutar()
        {
            Int32 rv = 0;
            try
            {
                using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                {
                    try
                    {
                        cnn.Open();
                        string sql = @"SELECT  [ID]
                                              ,[Denumire]
                                              ,[Valoare]
                                              ,[IdHotel]
                                              ,[IdUtilizator]
                                              ,[IdComputer]
                                              ,[TipSetare]
                                          FROM [SOLON.H].[setari].[Setari]
                                          WHERE Denumire  = 'TipCursValutar'";
                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                rv = Convert.ToInt32(reader[2]);
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        //Log.ExceptieSql(exc, "HelperCurs.GetCurs()");
                    }
                }
            }
            catch (Exception exc)
            {
                rv = 0;
            }
            return rv;
        }

        public static decimal GetCurs(DateTime data, int idMoneda, int idTipCurs)
        {
            decimal rv = 0;

            if (idMoneda == IdRON)
            {
                rv = 1;
            }
            else
            {
                if (data > ConexiuneDB.DataLucr)
                {
                    data = ConexiuneDB.DataLucr;
                }
                using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                {
                    try
                    {
                        cnn.Open();
                        string sql = @"
                            SELECT [Valoare] FROM [SOLON].[dbo].[CursValutar]
                            WHERE [Data] = @Data AND [IdMoneda] = @IdMoneda AND [TipCursID] = @IdTipCurs";
                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.Parameters.Add(new SqlParameter("@Data", SqlDbType.DateTime)).Value = data;
                        cmd.Parameters.Add(new SqlParameter("@IdMoneda", SqlDbType.Int)).Value = idMoneda;
                        cmd.Parameters.Add(new SqlParameter("@IdTipCurs", SqlDbType.Int)).Value = idTipCurs;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                rv = Convert.ToDecimal(reader[0]);
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        //Log.ExceptieSql(exc, "HelperCurs.GetCurs()");
                    }
                }
            }

            return rv;
        }

        public static decimal GetCurs(DateTime data, int idMoneda, int idTipCurs, SqlConnection cnn, SqlTransaction tran)
        {
            decimal rv = 0;

            if (idMoneda == IdRON)
            {
                rv = 1;
            }
            else
            {
                if (data > ConexiuneDB.DataLucr)
                {
                    data = ConexiuneDB.DataLucr;
                }

                string sql = @"
                    SELECT [Valoare] FROM [SOLON].[dbo].[CursValutar]
                    WHERE [Data] = @Data AND [IdMoneda] = @IdMoneda AND [TipCursID] = @IdTipCurs
                ";
                SqlCommand cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@Data", SqlDbType.DateTime)).Value = data;
                cmd.Parameters.Add(new SqlParameter("@IdMoneda", SqlDbType.Int)).Value = idMoneda;
                cmd.Parameters.Add(new SqlParameter("@IdTipCurs", SqlDbType.Int)).Value = idTipCurs;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        rv = Convert.ToDecimal(reader[0]);
                    }
                }
            }

            return rv;
        }

        public static decimal MonedaToRon(decimal val, DateTime data, int idMoneda, int idTipCurs)
        {
            decimal rv = 0;

            decimal curs = GetCurs(data, idMoneda, idTipCurs);
            rv = val * curs;

            rv = Math.Round(rv, 2);
            return rv;
        }

        public static decimal RonToMoneda(decimal val, DateTime data, int idMoneda, int idTipCurs)
        {
            decimal rv = 0;

            decimal curs = GetCurs(data, idMoneda, idTipCurs);
            if (curs > 0)
            {
                rv = val / curs;
            }

            rv = Math.Round(rv, 2);
            return rv;
        }

        public static decimal MonedaToMoneda(decimal val, DateTime data, int idMonedaSrc, int idMonedaDst, int idTipCurs)
        {
            decimal rv = 0;

            if (idMonedaSrc == idMonedaDst)
            {
                rv = val;
            }
            else if (idMonedaSrc == IdRON)
            {
                rv = RonToMoneda(val, data, idMonedaDst, idTipCurs);
            }
            else if (idMonedaDst == IdRON)
            {
                rv = MonedaToRon(val, data, idMonedaSrc, idTipCurs);
            }
            else
            {
                rv = MonedaToRon(val, data, idMonedaSrc, idTipCurs);
                rv = RonToMoneda(rv, data, idMonedaDst, idTipCurs);
            }

            rv = Math.Round(rv, 2);
            return rv;
        }

        public static bool VerificareExistentaCurs(DateTime data, int idTipCursBNR)
        {
            bool rv = false;

            // 1. verifica in baza daca am cursuri definite in ziua pe tip curs BNR
            //      - daca DA rv = true
            //      - daca NU se duce pe site si incearca sa descarce cursul din ziua resp
            //              - daca cu SUCCES rv = true
            //              - negasit sau eroare rv = false

            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    String sql = @"
                        SELECT 
                            cv.[Valoare] 
                        FROM 
                            [SOLON.H].[dbo].[CursValutar] AS cv --INNER JOIN
                            --[dbo].[CursValutarTipuri] AS cvt ON cvt.[ID] = cv.[TipCursID]
                        WHERE 
                            cv.[Data] = @Data AND 
                            cv.[IdTipCurs] = @IdTipCurs
                            --cvt.[Descriere] = @Descriere";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@Data", SqlDbType.DateTime)).Value = data;
                    cmd.Parameters.Add(new SqlParameter("@IdTipCurs", SqlDbType.Int)).Value = idTipCursBNR;
                    //cmd.Parameters.Add(new SqlParameter("@Descriere", SqlDbType.NVarChar)).Value = "Curs BNR";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            rv = true;
                        }
                    }
                }
                catch (Exception exc)
                {
                    //Log.ExceptieSql(exc, "HelperCurs.VerificareExistentaCurs()");
                }
            }
            if (rv)
            {
                return rv;
            }

            String downloadedString = "";
            try
            {
                WebClient client = new WebClient();
                downloadedString = client.DownloadString(String.Format("http://update.multisoft.ro/CursValutar/PreluareCursValutar.aspx?Data={0}", data.ToString("yyyy-MM-dd")));
                downloadedString = downloadedString.Substring(downloadedString.IndexOf("<span id=\"lblCursuri\">") + "<span id=\"lblCursuri\">".Length);
                downloadedString = downloadedString.Substring(
                    0,
                    downloadedString.IndexOf("</span>"));
                if (downloadedString == "-")
                {
                    return rv;
                }
            }
            catch (Exception exc)
            {
                //LogErori.Salveaza(exc, "HelperCurs.VerificareExistentaCurs()", null);
            }
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    using (SqlTransaction tran = cnn.BeginTransaction())
                    {
                        try
                        {
                            String sqlInsert = @"
                                INSERT INTO [SOLON.H].[dbo].[CursValutar]
                                    ([Data]
                                    ,[IdMoneda]
                                    ,[Valoare]
                                    ,[TipCursID])
                                VALUES
                                    (@Data
                                    ,@IdMoneda
                                    ,@Valoare
                                    ,@TipCursID)";
                            //                            int tipCursId = 0;
                            //                            String sqlTipCursId = @"
                            //                                SELECT 
                            //                                    [ID] 
                            //                                FROM 
                            //                                    [dbo].[CursValutarTipuri] 
                            //                                WHERE 
                            //                                    [Descriere] = @Descriere";
                            //                            SqlCommand cmdTipCursId = new SqlCommand(sqlTipCursId, cnn, tran);
                            //                            cmdTipCursId.Parameters.Add(new SqlParameter("@Descriere", SqlDbType.NVarChar)).Value = "Curs BNR";
                            //                            using (SqlDataReader reader = cmdTipCursId.ExecuteReader())
                            //                            {
                            //                                if (reader.Read())
                            //                                {
                            //                                    tipCursId = Convert.ToInt32(reader["ID"]);
                            //                                }
                            //                            }

                            foreach (String monedaValoare in downloadedString.Split('_'))
                            {
                                String monedaCod = monedaValoare.Split('=')[0];
                                Decimal valoare = Convert.ToDecimal(monedaValoare.Split('=')[1], CultureInfo.InvariantCulture);
                                int idMoneda = 0;
                                String sqlIdMoneda = @"
                                    SELECT
                                        [IdMoneda]
                                    FROM 
                                        [SOLON.H].[dbo].[NomMonede]
                                    WHERE
                                        [MonedaCod] = @MonedaCod";
                                SqlCommand cmdIdMoneda = new SqlCommand(sqlIdMoneda, cnn, tran);
                                cmdIdMoneda.Parameters.Add(new SqlParameter("@MonedaCod", SqlDbType.NVarChar)).Value = monedaCod;
                                using (SqlDataReader reader = cmdIdMoneda.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        idMoneda = Convert.ToInt32(reader["IdMoneda"]);
                                    }
                                }
                                SqlCommand cmdInsert = new SqlCommand(sqlInsert, cnn, tran);
                                cmdInsert.Parameters.Add(new SqlParameter("@Data", SqlDbType.DateTime)).Value = data;
                                cmdInsert.Parameters.Add(new SqlParameter("@IdMoneda", SqlDbType.Int)).Value = idMoneda;
                                cmdInsert.Parameters.Add(new SqlParameter("@Valoare", SqlDbType.Decimal)).Value = valoare;
                                cmdInsert.Parameters.Add(new SqlParameter("@TipCursID", SqlDbType.Int)).Value = idTipCursBNR;// tipCursId;
                                cmdInsert.ExecuteNonQuery();
                            }
                            tran.Commit();
                            rv = true;
                        }
                        catch
                        {
                            try
                            {
                                tran.Rollback();
                            }
                            catch { }
                            throw;
                        }
                    }
                }
                catch (Exception exc)
                {
                    //Log.ExceptieSql(exc, "HelperCurs.VerificareExistentaCurs()");
                }
            }

            return rv;
        }

        public static void GenereazaCursuriRetroactiv()
        {
            Dictionary<int, ServiciuCursValutar> servicii = ServiciuCursValutar.GetServicii();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    String sqlserv = @" UPDATE [SOLON.H].[hotel].[RezervariServicii]
                                           SET [ValoareMoneda] = @ValoareMoneda
                                              ,[Curs] = @Curs
                                              ,[ValoareRon] = @ValoareRon
                                         WHERE ID = @IdServ";

                    String sqlValzi = @"UPDATE [SOLON.H].[hotel].[RezervariServiciiValori]
                                           SET [Curs] = @Curs
                                              ,[ValoareRON] = @ValoareRon
                                         WHERE ID = @IdVal";
                    SqlCommand cmd = null;
                    using (SqlTransaction tran = cnn.BeginTransaction())
                    {
                        try
                        {
                            foreach (ServiciuCursValutar serv in servicii.Values)
                            {
                                foreach (ServiciuCursValutar.ValoriZilniceCursValutar valz in serv.valori.Values)
                                {
                                    valz.ValoareRon = Math.Round(valz.Valoare * serv.Curs, 2);

                                    cmd = new SqlCommand(sqlValzi, cnn, tran);
                                    cmd.Parameters.Add(new SqlParameter("@Curs", SqlDbType.Decimal)).Value = serv.Curs;
                                    cmd.Parameters.Add(new SqlParameter("@ValoareRon", SqlDbType.Decimal)).Value = valz.ValoareRon;
                                    cmd.Parameters.Add(new SqlParameter("@IdVal", SqlDbType.Int)).Value = valz.ID;
                                    cmd.ExecuteNonQuery();
                                }

                                cmd = new SqlCommand(sqlserv, cnn, tran);
                                cmd.Parameters.Add(new SqlParameter("@ValoareMoneda", SqlDbType.Decimal)).Value = serv.ValoareMoneda;
                                cmd.Parameters.Add(new SqlParameter("@Curs", SqlDbType.Decimal)).Value = serv.Curs;
                                cmd.Parameters.Add(new SqlParameter("@ValoareRon", SqlDbType.Decimal)).Value = serv.ValoareRon;
                                cmd.Parameters.Add(new SqlParameter("@IdServ", SqlDbType.Int)).Value = serv.IDServ;

                                cmd.ExecuteNonQuery();


                            }

                            tran.Commit();
                        }
                        catch
                        {
                            try
                            {
                                tran.Rollback();
                            }
                            catch { }
                            throw;
                        }
                    }
                }
                catch (Exception exc)
                {
                    //Log.ExceptieSql(exc, "HelperCurs.ServiciuCursValutar.GenereazaCursuriRetroactiv()");
                }
            }
        }

        public class ServiciuCursValutar
        {
            public int IDServ { get; set; }
            public DateTime DataServ { get; set; }
            public decimal CursInZiua { get; set; }
            public decimal ValoareMoneda
            {
                get
                {
                    return valori.Values.Sum(x => x.Valoare);
                }
            }
            public decimal Curs
            {
                get
                {
                    if (IdMoneda == IdRON)
                    {
                        return 1;
                    }
                    else
                    {
                        if (DataDocument != DateTime.MinValue && CursDocument != 0)
                        {
                            return CursDocument;
                        }
                        else
                        {
                            return CursInZiua;
                        }
                    }
                }
            }
            public decimal ValoareRon
            {
                get
                {
                    if (IdMoneda == IdRON)
                    {
                        return ValoareMoneda;
                    }
                    else
                    {
                        //return Math.Round(ValoareMoneda * Curs,2);
                        return valori.Values.Sum(x => x.ValoareRon);
                    }
                }
            }
            public int IdMoneda { get; set; }
            public DateTime DataDocument = DateTime.MinValue;
            public decimal CursDocument { get; set; }
            public Dictionary<int, ValoriZilniceCursValutar> valori = new Dictionary<int, ValoriZilniceCursValutar>();

            public class ValoriZilniceCursValutar
            {
                public int ID { get; set; }
                public decimal Valoare { get; set; }
                public decimal ValoareRon { get; set; }
                public int IDServiciu { get; set; }
            }

            public class CursLaDataCursValutar
            {
                public DateTime Data { get; set; }
                public int IdMoneda { get; set; }
                public int IdTipCurs { get; set; }
                public decimal Valoare { get; set; }
            }

            public class MonedaCursValutar
            {
                public int IdMoneda { get; set; }
                public string Cod { get; set; }
                public bool EsteNationala { get; set; }
                public Dictionary<DateTime, CursLaDataCursValutar> cursuri = new Dictionary<DateTime, CursLaDataCursValutar>();
            }

            public static Dictionary<int, MonedaCursValutar> GetMonede()
            {
                Dictionary<int, MonedaCursValutar> monede = new Dictionary<int, MonedaCursValutar>();
                using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                {
                    try
                    {
                        cnn.Open();
                        string sql = @"SELECT mon.IdMoneda,mon.MonedaCod,mon.EsteNationala FROM SOLON.dbo.NomMonede as mon";
                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["IdMoneda"]);
                                if (!monede.ContainsKey(id))
                                {
                                    monede.Add(id, new MonedaCursValutar
                                    {
                                        IdMoneda = id,
                                        Cod = reader["MonedaCod"].ToString(),
                                        EsteNationala = Convert.ToBoolean(reader["EsteNationala"])
                                    }
                                    );
                                }
                            }
                        }

                        sql = @"SELECT cv.Data,cv.IdMoneda,cv.TipCursID,cv.Valoare FROM SOLON.dbo.CursValutar as cv WHERE cv.TipCursID = @IdTipCurs";
                        cmd = new SqlCommand(sql, cnn);
                        cmd.Parameters.Add(new SqlParameter("@IdTipCurs", SqlDbType.Int)).Value = ConexiuneDB.CursValutar;// LoginInfo.IdTipCurs;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime Data = Convert.ToDateTime(reader["Data"]);
                                int IdMoneda = Convert.ToInt32(reader["IdMoneda"]);

                                if (!monede.ContainsKey(IdMoneda))
                                {
                                    monede.Add(IdMoneda, new MonedaCursValutar
                                    {
                                        IdMoneda = IdMoneda,
                                        Cod = ":O MONEDA NECUNOSCUTA :O",
                                        EsteNationala = false
                                    }
                                    );
                                }

                                if (!monede[IdMoneda].cursuri.ContainsKey(Data))
                                {
                                    monede[IdMoneda].cursuri.Add(Data, new CursLaDataCursValutar
                                    {
                                        Data = Data,
                                        IdMoneda = IdMoneda,
                                        IdTipCurs = ConexiuneDB.IdTipCursValutar,  // LoginInfo.IdTipCurs,
                                        Valoare = Convert.ToDecimal(reader["Valoare"])
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        //Log.ExceptieSql(exc, "HelperCurs.ServiciuCursValutar.GetMonede()");
                    }
                };

                return monede;
            }

            public static Dictionary<int, ServiciuCursValutar> GetServicii()
            {
                Dictionary<int, ServiciuCursValutar.MonedaCursValutar> monede = ServiciuCursValutar.GetMonede();
                Dictionary<int, ServiciuCursValutar> servicii = new Dictionary<int, ServiciuCursValutar>();
                using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                {
                    try
                    {
                        cnn.Open();
                        string sql = @"SELECT
	                                     rs.ID AS IDServ
                                        ,rsv.ID AS IDValServ
                                        ,rsv.Valoare AS ValZi
                                        ,rs.DataOra AS DataServ
                                        ,pl.DataLucru AS DataPlata
                                        ,fac.Data AS DataFac
                                        ,pl.ID AS IdPlata
                                        ,pl.Sters AS PlataStearsa
                                        ,fac.ID AS IdFac
                                        ,fac.Anulata AS FacAnulata
                                        ,rs.IdMoneda AS IdMoneda
                                    FROM [SOLON.H].hotel.RezervariServiciiValori AS rsv LEFT OUTER JOIN
                                    [SOLON.H].hotel.RezervariServicii AS rs ON rs.ID = rsv.IdRezervareServiciu LEFT OUTER JOIN
                                    [SOLON.H].financiar.PlatiServicii AS ps ON ps.IdRezervareServiciu = rs.ID LEFT OUTER JOIN
                                    [SOLON.H].financiar.Plati AS pl ON pl.ID = ps.IdPlata LEFT OUTER JOIN
                                    [SOLON.H].financiar.FacturiServicii AS fs ON fs.IdRezervareServiciu = rs.ID LEFT OUTER JOIN
                                    [SOLON.H].financiar.Facturi AS fac ON fac.ID = fs.IdFactura
                                    WHERE rs.Sters = 0 
                                          AND rs.ValoareMoneda = 0
                                    ORDER BY rs.DataOra ASC";
                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int IdServ = Convert.ToInt32(reader["IDServ"]);
                                int IdValZ = Convert.ToInt32(reader["IDValServ"]);
                                DateTime DataServ = Convert.ToDateTime(reader["DataServ"]);
                                DateTime DataPlata = reader["DataPlata"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataPlata"]);
                                DateTime DataFac = reader["DataFac"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataFac"]);
                                int IdMoneda = Convert.ToInt32(reader["IdMoneda"]);
                                decimal ValZi = Convert.ToDecimal(reader["ValZI"]);

                                bool PlataStearsa = reader["PlataStearsa"] == DBNull.Value ? false : Convert.ToBoolean(reader["PlataStearsa"]);
                                bool FacAnulata = reader["FacAnulata"] == DBNull.Value ? false : Convert.ToBoolean(reader["FacAnulata"]);


                                int IdPlata = reader["IdPlata"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdPlata"]);
                                int IdFac = reader["IdFac"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdFac"]);

                                if (!servicii.ContainsKey(IdServ))
                                {
                                    servicii.Add(IdServ, new ServiciuCursValutar
                                    {
                                        IDServ = IdServ,
                                        DataServ = Convert.ToDateTime(reader["DataServ"]).Date,
                                        IdMoneda = Convert.ToInt32(reader["IdMoneda"]),
                                    });


                                    if (IdMoneda == HelperCurs.IdRON)
                                    {
                                        servicii[IdServ].CursInZiua = 1;
                                        servicii[IdServ].CursDocument = 1;
                                    }
                                    else
                                    {
                                        //Get curs serviciu
                                        if (monede.ContainsKey(IdMoneda))
                                        {
                                            DateTime lastDate = DateTime.MinValue;
                                            if (monede[IdMoneda].cursuri.Values.Count(x => x.Data > DataServ) > 0)
                                            {
                                                lastDate = monede[IdMoneda].cursuri.Values.FirstOrDefault(x => x.Data > DataServ).Data;
                                            }
                                            else if (monede[IdMoneda].cursuri.Values.Count > 0)
                                            {
                                                lastDate = monede[IdMoneda].cursuri.Values.LastOrDefault(x => x.Valoare != 0).Data;
                                            }

                                            if (monede[IdMoneda].cursuri.ContainsKey(DataServ))
                                            {
                                                servicii[IdServ].CursInZiua = monede[IdMoneda].cursuri[DataServ].Valoare;
                                            }
                                            else if (lastDate != DateTime.MinValue)
                                            {
                                                servicii[IdServ].CursInZiua = monede[IdMoneda].cursuri[lastDate].Valoare;
                                            }
                                            else
                                            {
                                                servicii[IdServ].CursInZiua = 0;
                                            }
                                        }
                                        else
                                        {
                                            servicii[IdServ].CursInZiua = 0;
                                        }

                                        //Get curs Plata
                                        if (IdPlata != 0 && !PlataStearsa)
                                        {
                                            if (DataPlata < servicii[IdServ].DataDocument)
                                            {
                                                if (monede.ContainsKey(IdMoneda))
                                                {
                                                    DateTime lastDate = DateTime.MinValue;
                                                    if (monede[IdMoneda].cursuri.Values.Count(x => x.Data > DataPlata) > 0)
                                                    {
                                                        lastDate = monede[IdMoneda].cursuri.Values.FirstOrDefault(x => x.Data > DataPlata).Data;
                                                    }

                                                    if (monede[IdMoneda].cursuri.ContainsKey(DataPlata))
                                                    {
                                                        servicii[IdServ].CursDocument = monede[IdMoneda].cursuri[DataPlata].Valoare;
                                                        servicii[IdServ].DataDocument = DataPlata;
                                                    }
                                                    else if (lastDate != DateTime.MinValue && lastDate < servicii[IdServ].DataDocument)
                                                    {
                                                        servicii[IdServ].CursDocument = monede[IdMoneda].cursuri[lastDate].Valoare;
                                                        servicii[IdServ].DataDocument = lastDate;
                                                    }
                                                }
                                            }
                                        }


                                        //Get curs Factura
                                        if (IdFac != 0 && !FacAnulata)
                                        {
                                            if (DataFac < servicii[IdServ].DataDocument)
                                            {
                                                if (monede.ContainsKey(IdMoneda))
                                                {
                                                    DateTime lastDate = DateTime.MinValue;
                                                    if (monede[IdMoneda].cursuri.Values.Count(x => x.Data > DataFac) > 0)
                                                    {
                                                        lastDate = monede[IdMoneda].cursuri.Values.FirstOrDefault(x => x.Data > DataFac).Data;
                                                    }

                                                    if (monede[IdMoneda].cursuri.ContainsKey(DataFac))
                                                    {
                                                        servicii[IdServ].CursDocument = monede[IdMoneda].cursuri[DataFac].Valoare;
                                                        servicii[IdServ].DataDocument = DataFac;
                                                    }
                                                    else if (lastDate != DateTime.MinValue && lastDate < servicii[IdServ].DataDocument)
                                                    {
                                                        servicii[IdServ].CursDocument = monede[IdMoneda].cursuri[lastDate].Valoare;
                                                        servicii[IdServ].DataDocument = lastDate;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (!servicii[IdServ].valori.ContainsKey(IdValZ))
                                {
                                    servicii[IdServ].valori.Add(IdValZ, new ValoriZilniceCursValutar
                                    {
                                        ID = IdValZ,
                                        IDServiciu = IdServ,
                                        Valoare = ValZi
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        //Log.ExceptieSql(exc, "HelperCurs.ServiciuCursValutar.GetServicii()");
                    }
                };

                return servicii;
            }
        }

        internal static decimal GetCurs(DateTime data, int idMoneda, int idTipCurs, SqlConnection cnn)
        {
            decimal rv = 0;

            if (idMoneda == IdRON)
            {
                rv = 1;
            }
            else
            {
                if (data > ConexiuneDB.DataLucr)
                {
                    data = ConexiuneDB.DataLucr;
                }

                string sql = @"
                    SELECT [Valoare] FROM [SOLON].[dbo].[CursValutar]
                    WHERE [Data] = @Data AND [IdMoneda] = @IdMoneda AND [TipCursID] = @IdTipCurs
                ";
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.Add(new SqlParameter("@Data", SqlDbType.DateTime)).Value = data;
                cmd.Parameters.Add(new SqlParameter("@IdMoneda", SqlDbType.Int)).Value = idMoneda;
                cmd.Parameters.Add(new SqlParameter("@IdTipCurs", SqlDbType.Int)).Value = idTipCurs;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        rv = Convert.ToDecimal(reader[0]);
                    }
                }
            }

            return rv;
        }

        public static string GetSerieNefiscal()
        {
            string serie = "";
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"
                        SELECT
                            [Valoare]
                        FROM
                            [SOLON.H].[setari].[Setari]
                        WHERE
                            [IdHotel] = @IdHotel AND
                            [Denumire] = @Denumire";
                    bool exista = false;
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.Int)).Value = ConexiuneDB.IdHotel;
                    cmd.Parameters.Add(new SqlParameter("@Denumire", SqlDbType.NVarChar)).Value = "SerieNefiscal";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            serie = reader["Valoare"].ToString();
                            exista = true;
                        }
                    }
                    if (!exista)
                    {
                        sql = @"
                            INSERT INTO [SOLON.H].[setari].[Setari]
                                ([Denumire]
                                ,[Valoare]
                                ,[IdHotel]
                                ,[IdUtilizator]
                                ,[IdComputer]
                                ,[TipSetare])
                            VALUES
                                (@Denumire
                                ,@Valoare
                                ,@IdHotel
                                ,@IdUtilizator
                                ,@IdComputer
                                ,@TipSetare)
                        ";
                        cmd = new SqlCommand(sql, cnn);
                        cmd.Parameters.Add(new SqlParameter("@Denumire", SqlDbType.NVarChar)).Value = "SerieNefiscal";
                        cmd.Parameters.Add(new SqlParameter("@Valoare", SqlDbType.NVarChar)).Value = "";
                        cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.Int)).Value = ConexiuneDB.IdHotel;
                        cmd.Parameters.Add(new SqlParameter("@IdUtilizator", SqlDbType.Int)).Value = DBNull.Value;
                        cmd.Parameters.Add(new SqlParameter("@IdComputer", SqlDbType.Int)).Value = DBNull.Value;
                        cmd.Parameters.Add(new SqlParameter("@TipSetare", SqlDbType.NChar)).Value = "H";
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception exc)
                {
                    //LogErori.Salveaza(exc, "Setare.GetSerieNefiscal()", null);
                }
            }
            return serie;
        }
        
        public static string LoadNumar(string seria)
        {
            string rv = "";
            seria = seria.Trim().ToUpper();
            if (string.IsNullOrEmpty(seria))
            {
                return "0";
            }
            string lastNumber = "";
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"
                        SELECT TOP 1 DocNr
                        FROM [SOLON.H].[financiar].[Plati] 
                        WHERE IdHotel = @IdHotel AND DocSeria = @Seria ORDER BY DataOraCreare DESC
                    ";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.Int)).Value = ConexiuneDB.IdHotel;
                    cmd.Parameters.Add(new SqlParameter("@Seria", SqlDbType.NVarChar)).Value = seria;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lastNumber = reader[0] == DBNull.Value ? "" : reader[0].ToString();
                        }
                        else
                        {
                            lastNumber = "0";
                        }
                    }
                }
                catch (Exception exc)
                {
                   // Log.ExceptieSql(exc, "FormPlata.LoadNumar()");
                }
            }
            if (string.IsNullOrEmpty(lastNumber))
            {
                return "";
            }
            long ln;
            if (!long.TryParse(lastNumber, out ln))
            {
                return "";
            }
            rv=(ln + 1).ToString();
            return rv;
        }
    }
}