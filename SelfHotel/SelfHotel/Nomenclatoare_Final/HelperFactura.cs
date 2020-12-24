using SelfHotel.Setari;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
    public class HelperFactura
    {
        public static string GetSerieNumar(int IdFactura)
        {
            string rv = "";
            string serie = "";
            DateTime data = new DateTime();

            string sql = @"SELECT 
			                    Serie + ' ' +Numar AS Serie,
                                Data AS Data
                           FROM
	                            [SOLON.H].financiar.Facturi
                            WHERE 
	                            ID = @IdFactura";

            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdFactura", IdFactura)).SqlDbType = SqlDbType.Int;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        serie = dr["Serie"].ToString();
                        data = Convert.ToDateTime(dr["Data"]);
                    }

                }
                catch
                {
                }
            }

            rv = serie + "/" + data.ToString("dd.MM.yyyy");
            return rv;
        }

        public static string GetSerieNumarPlata(int IdPlata)
        {
            string rv = "";
            string serie = "";
            DateTime data = new DateTime();

            //            string sql = @"SELECT
            //		                             mdp.Cod + '(' +tdp.Cod + ' ' + cast(p.NumarIntern as nvarchar) + ')' AS Serie,
            //		                             p.DataLucru AS Data
            //                            FROM 
            //	                            financiar.Plati AS p 
            //	                            INNER JOIN financiar.PlatiMetodePlata AS pmp ON pmp.IdPlata = p.ID
            //	                            INNER JOIN financiar.MetodeDePlata AS mdp ON mdp.ID = pmp.IdMetodaPlata
            //	                            INNER JOIN financiar.TipDocumentPlata AS tdp ON tdp.ID = p.IdTipDocPlata
            //                            WHERE
            //                                p.id = @IdPlata";

            string sql = @"SELECT
		                             mdp.Cod + ': ' + tdp.Cod +' '+ ISNULL(p.DocNr,'') +' ' + ' (#' + cast(p.NumarIntern as nvarchar) + ')' AS Serie,
		                             p.DataLucru AS Data
                            FROM 
	                            [SOLON.H].financiar.Plati AS p 
	                            INNER JOIN [SOLON.H].financiar.PlatiMetodePlata AS pmp ON pmp.IdPlata = p.ID
	                            INNER JOIN [SOLON.H].financiar.MetodeDePlata AS mdp ON mdp.ID = pmp.IdMetodaPlata
	                            INNER JOIN [SOLON.H].financiar.TipDocumentPlata AS tdp ON tdp.ID = p.IdTipDocPlata
                            WHERE
                                p.id = @IdPlata";



            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdPlata", IdPlata)).SqlDbType = SqlDbType.Int;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        serie = dr["Serie"].ToString();
                        data = Convert.ToDateTime(dr["Data"]);
                    }

                }
                catch
                {
                }
            }

            rv = serie + "/" + data.ToString("dd.MM.yyyy");
            return rv;
        }

        public static bool PotStorna(int idFactura, bool Totala, ref string mesaj)
        {
            bool rv = true;
            mesaj = "";

            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"
                                SELECT 
                                    Anulata
                                FROM
                                    [SOLON.H].[financiar].[Facturi]
                                WHERE
                                    ID = @IdFactura
                            ";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (Convert.ToBoolean(reader[0]))
                            {
                                rv = false;
                                mesaj = "Factura selectata este anulata!";
                            }
                        }
                    }

                    if (rv)
                    {
                        sql = @"
                            SELECT 
                                StornoLaFactura
                            FROM
                                [SOLON.H].[financiar].[Facturi]
                            WHERE
                                ID = @IdFactura
                        ";
                        cmd = new SqlCommand(sql, cnn);
                        cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (Convert.ToInt32(reader[0]) != 0)
                                {
                                    rv = false;
                                    mesaj = "Factura selectata este o factura de storno!";
                                }
                            }
                        }

                        if (rv)
                        {
                            if (Totala)
                            {
                                sql = @"
                                    SELECT 
                                        ID
                                    FROM
                                        [SOLON.H].[financiar].[Facturi]
                                    WHERE
                                        [StornoLaFactura] = @IdFactura AND
                                        [Anulata] = 0
                                ";
                                cmd = new SqlCommand(sql, cnn);
                                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        rv = false;
                                        mesaj = "Factura selectata este deja stornata!";
                                    }
                                }
                            }
                            else
                            {

                                sql = @"
                                    SELECT 
                                         fac.ID
                                        ,(SELECT COUNT(id) FROM [SOLON.H].financiar.FacturiPozitii WHERE IdFactura = fac.ID) AS POZITII_FACTURA 
                                        ,(
                                            SELECT 
                                                COUNT(fp.id) 
                                            FROM 
                                                [SOLON.H].financiar.FacturiPozitii as fp LEFT OUTER JOIN 
                                                [SOLON.H].financiar.Facturi as storn ON storn.ID = fp.IdFactura 
                                            WHERE 
                                                storn.StornoLaFactura = fac.id AND 
                                                storn.Anulata = 0
                                          ) AS POZITII_STORNO
                                    FROM
                                        [SOLON.H].[financiar].[Facturi] AS fac
                                    WHERE
                                        fac.ID = @IdFactura AND
                                        fac.[Anulata] = 0
                                ";
                                cmd = new SqlCommand(sql, cnn);
                                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        int pozStorno = Convert.ToInt32(reader["POZITII_STORNO"]);
                                        int pozFac = Convert.ToInt32(reader["POZITII_FACTURA"]);

                                        if (pozFac != 0)
                                        {
                                            if (pozStorno < pozFac)
                                            {
                                                rv = true;
                                            }
                                            else
                                            {
                                                rv = false;
                                                mesaj = "Factura selectata este deja stornata!";
                                            }
                                        }
                                        else
                                        {
                                            rv = false;
                                            mesaj = "Factura selectata este deja stornata!";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    rv = false;
                    //Log.ExceptieSql(exc, "HelperFactura.PotStorna()");
                }
            }

            return rv;
        }

        public static List<EntitateCarnetFacturi> GetCarnete(List<int> IdsMetodePlata, bool EsteReceptie, bool EsteVirament, bool EsteAvands)
        {
            List<EntitateCarnetFacturi> carnete = new List<EntitateCarnetFacturi>();
            bool EsteAvans = false;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"";
                    SqlCommand cmd;

                    if (IdsMetodePlata == null)
                    {
                        IdsMetodePlata = new List<int>();
                    }
                    if (IdsMetodePlata.Count == 0)
                    {
                        sql = @"SELECT [ID] FROM [SOLON.H].[financiar].[MetodeDePlata] WHERE Sters = 0";
                        cmd = new SqlCommand(sql, cnn);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IdsMetodePlata.Add(Convert.ToInt32(reader[0]));
                            }
                        }
                    }
                    if (IdsMetodePlata.Count == 0)
                    {
                        throw new Exception("Nu exista metode de plata nesterse!");
                    }

                    string ids = IdsMetodePlata.Select(x => x.ToString()).Aggregate((x1, x2) => x1 + "," + x2);

                        if (EsteReceptie && EsteVirament)
                        {
                            sql = @"
                        SELECT
                             c.[ID]
                            ,c.[Serie]
                            ,c.[NumarDeLa]
                            ,c.[NumarPanaLa]
                            ,c.[EsteReceptie]
                            ,c.[EsteVirament]
                            ,c.[EsteAvans]
                            ,m.[ID] AS mID
                            ,m.[Cod] AS mCod
                            ,m.[Denumire] AS mDenumire
                            ,m.[Ordine] AS mOrdine
                            ,part.[IdPartener] AS IdFurnizor
                            ,part.[NumePartener] AS NumePartener
                        FROM 
                            [SOLON.H].[financiar].[CarneteDocumenteEmise] AS c INNER JOIN
                            [SOLON.H].[financiar].[CarneteDocEmiseMetPlata] as cm ON cm.[IDCarnet] = c.[ID] INNER JOIN
                            [SOLON.H].[financiar].[MetodeDePlata] as m on m.[ID] = cm.[IDMetodaPlata] LEFT OUTER JOIN
                            [Solon].[dbo].[NomParteneri] as part on part.IdPartener = c.IdPartener
                        WHERE
                            c.[InFolosinta] = 1 AND
                            c.[Sters] = 0 AND
                            c.[Consumat] = 0 AND
                            c.[ValabilDeLa] <= @DeLa AND
                            c.[ValabilPanaLa] >= @PanaLa AND
                            c.[IdHotel] = @IdHotel AND
                            (c.[EsteReceptie] = @Rec1 OR 
                            c.[EsteVirament] = @Rec2) AND
                            m.[Sters] = 0 AND
                            c.[TipDocument] like 'TDEFAC' AND
                            m.ID IN (<ids>)
                    ";
                        }
                        else if (EsteReceptie)
                        {
                            sql = @"
                        SELECT
                             c.[ID]
                            ,c.[Serie]
                            ,c.[NumarDeLa]
                            ,c.[NumarPanaLa]
                            ,c.[EsteReceptie]
                            ,c.[EsteVirament]
                            ,c.[EsteAvans]
                            ,m.[ID] AS mID
                            ,m.[Cod] AS mCod
                            ,m.[Denumire] AS mDenumire
                            ,m.[Ordine] AS mOrdine
                            ,part.[IdPartener] AS IdFurnizor
                            ,part.[NumePartener] AS NumePartener
                        FROM 
                            [SOLON.H].[financiar].[CarneteDocumenteEmise] AS c INNER JOIN
                            [SOLON.H].[financiar].[CarneteDocEmiseMetPlata] as cm ON cm.[IDCarnet] = c.[ID] INNER JOIN
                            [SOLON.H].[financiar].[MetodeDePlata] as m on m.[ID] = cm.[IDMetodaPlata] LEFT OUTER JOIN
                            [Solon].[dbo].[NomParteneri] as part on part.IdPartener = c.IdPartener
                        WHERE
                            c.[InFolosinta] = 1 AND
                            c.[Sters] = 0 AND
                            c.[Consumat] = 0 AND
                            c.[ValabilDeLa] <= @DeLa AND
                            c.[ValabilPanaLa] >= @PanaLa AND
                            c.[IdHotel] = @IdHotel AND
                            c.[EsteReceptie] = @Rec1 AND
                            m.[Sters] = 0 AND
                            c.[TipDocument] like 'TDEFAC' AND
                            m.ID IN (<ids>)
                    ";
                        }
                        else if (EsteVirament)
                        {
                            sql = @"
                        SELECT
                             c.[ID]
                            ,c.[Serie]
                            ,c.[NumarDeLa]
                            ,c.[NumarPanaLa]
                            ,c.[EsteReceptie]
                            ,c.[EsteVirament]
                            ,c.[EsteAvans]
                            ,m.[ID] AS mID
                            ,m.[Cod] AS mCod
                            ,m.[Denumire] AS mDenumire
                            ,m.[Ordine] AS mOrdine
                            ,part.[IdPartener] AS IdFurnizor
                            ,part.[NumePartener] AS NumePartener
                        FROM 
                            [SOLON.H].[financiar].[CarneteDocumenteEmise] AS c INNER JOIN
                            [SOLON.H].[financiar].[CarneteDocEmiseMetPlata] as cm ON cm.[IDCarnet] = c.[ID] INNER JOIN
                            [SOLON.H].[financiar].[MetodeDePlata] as m on m.[ID] = cm.[IDMetodaPlata] LEFT OUTER JOIN
                            [Solon].[dbo].[NomParteneri] as part on part.IdPartener = c.IdPartener
                        WHERE
                            c.[InFolosinta] = 1 AND
                            c.[Sters] = 0 AND
                            c.[Consumat] = 0 AND
                            c.[ValabilDeLa] <= @DeLa AND
                            c.[ValabilPanaLa] >= @PanaLa AND
                            c.[IdHotel] = @IdHotel AND
                            c.[EsteVirament] = @Rec2 AND
                            m.[Sters] = 0 AND
                            c.[TipDocument] like 'TDEFAC' AND
                            m.ID IN (<ids>)
                    ";
                        }
                   
                    
                    sql = sql.Replace("<ids>", ids);
                    cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@DeLa", SqlDbType.DateTime)).Value = ConexiuneDB.DataLucr;
                    cmd.Parameters.Add(new SqlParameter("@PanaLa", SqlDbType.DateTime)).Value = ConexiuneDB.DataLucr;
                    cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.Int)).Value = ConexiuneDB.IdHotel;
                   
                        if (!EsteAvans)
                        {
                            if (EsteReceptie && EsteVirament)
                            {
                                cmd.Parameters.Add(new SqlParameter("@Rec1", SqlDbType.Bit)).Value = EsteReceptie;
                                cmd.Parameters.Add(new SqlParameter("@Rec2", SqlDbType.Bit)).Value = EsteVirament;
                            }
                            else if (EsteReceptie)
                            {
                                cmd.Parameters.Add(new SqlParameter("@Rec1", SqlDbType.Bit)).Value = EsteReceptie;
                            }
                            else if (EsteVirament)
                            {
                                cmd.Parameters.Add(new SqlParameter("@Rec2", SqlDbType.Bit)).Value = EsteVirament;
                            }
                        }
                        else
                        {
                            if (EsteReceptie && EsteVirament)
                            {
                                cmd.Parameters.Add(new SqlParameter("@Rec1", SqlDbType.Bit)).Value = EsteReceptie;
                                cmd.Parameters.Add(new SqlParameter("@Rec2", SqlDbType.Bit)).Value = EsteVirament;
                                cmd.Parameters.Add(new SqlParameter("@Rec3", SqlDbType.Bit)).Value = EsteAvans;
                            }
                            else if (EsteReceptie)
                            {
                                cmd.Parameters.Add(new SqlParameter("@Rec1", SqlDbType.Bit)).Value = EsteReceptie;
                                cmd.Parameters.Add(new SqlParameter("@Rec3", SqlDbType.Bit)).Value = EsteAvans;
                            }
                            else if (EsteVirament)
                            {
                                cmd.Parameters.Add(new SqlParameter("@Rec2", SqlDbType.Bit)).Value = EsteVirament;
                                cmd.Parameters.Add(new SqlParameter("@Rec3", SqlDbType.Bit)).Value = EsteAvans;
                            }
                        }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        EntitateCarnetFacturi ec;
                        int id;
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["ID"]);
                            ec = carnete.FirstOrDefault(x => x.ID == id);
                            if (ec == null)
                            {
                                ec = new EntitateCarnetFacturi();
                                ec.ID = id;
                                ec.Serie = reader["Serie"].ToString();
                                ec.NumarDeLa = reader["NumarDeLa"].ToString();
                                ec.NumarPanaLa = reader["NumarPanaLa"].ToString();
                                ec.EsteReceptie = Convert.ToBoolean(reader["EsteReceptie"]);
                                ec.EsteVirament = Convert.ToBoolean(reader["EsteVirament"]);
                                ec.EsteAvans = Convert.ToBoolean(reader["EsteAvans"]);
                                ec.IdFurnizor = Convert.ToInt32(reader["IdFurnizor"]);
                                ec.NumeFurnizor = reader["NumePartener"].ToString();
                                carnete.Add(ec);
                            }
                            ec.MetodePlata.Add(new EntitateMetodaPlata()
                            {
                                ID = Convert.ToInt32(reader["mID"]),
                                Cod = reader["mCod"].ToString(),
                                Denumire = reader["mDenumire"].ToString(),
                                //Ordine = Convert.ToInt32(reader["mOrdine"])
                            });
                        }
                    }

                    sql = @"
                        SELECT TOP 1 
                            Numar,
                            Data
                        FROM [SOLON.H].[financiar].[Facturi] 
                        WHERE 
                            IdCarnet = @IdCarnet AND 
                            Emisa = 1
                        ORDER BY ID DESC
                        --ORDER BY Data DESC, ID DESC
                    ";
                    cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdCarnet", SqlDbType.Int));
                    foreach (EntitateCarnetFacturi car in carnete)
                    {
                        cmd.Parameters["@IdCarnet"].Value = car.ID;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                car.LastNumber = reader[0] == DBNull.Value ? "" : reader[0].ToString();
                                car.LastData = reader[1] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader[1]);
                            }
                            else
                            {
                                car.LastNumber = "0";
                                car.LastData = DateTime.MinValue;
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    //Log.ExceptieSql(exc, "FormSelCarnetFacturi.GetLista()");
                }
            }

            foreach (EntitateCarnetFacturi ecf in carnete)
            {
                ecf.MetodePlata = ecf.MetodePlata.ToList();
                ecf.MetodePlata_Proxy = ecf.MetodePlata.Select(x => string.IsNullOrEmpty(x.Cod) ? x.Denumire : x.Cod).Aggregate((x1, x2) => x1 + ", " + x2);
                ecf.EsteReceptie_Proxy = ecf.EsteReceptie ? "DA" : "";
                ecf.EsteVirament_Proxy = ecf.EsteVirament ? "DA" : "";
                ecf.EsteAvans_Proxy = ecf.EsteAvans ? "DA" : "";

                long n1 = 0, n2 = 0, ln;

                string initial = ecf.NumarDeLa;
                string spec = "";
                for (int i = 1; i <= initial.Length; i++)
                {
                    spec += "0";
                }

                if (long.TryParse(ecf.NumarDeLa, out n1) && long.TryParse(ecf.NumarPanaLa, out n2))
                {
                    if (ecf.LastNumber == "0")
                    {
                        ecf.Numar = n1.ToString(spec);
                    }
                    else
                    {
                        if (long.TryParse(ecf.LastNumber, out ln))
                        {
                            ecf.LastNumberParsed = ln;
                            ecf.Numar = (ln + 1).ToString(spec);
                        }
                    }
                }
            }
            carnete = carnete.OrderBy(x => x.Serie).ToList();

            return carnete;
        }

        public static List<EntitateCarnetFacturi> GetCarnete(List<int> IdsMetodePlata, bool EsteReceptie, bool EsteVirament, bool EsteAvanss, int IdFurnizor)
        {
            List<EntitateCarnetFacturi> carnete = new List<EntitateCarnetFacturi>();
            bool EsteAvans = false;
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"";
                    SqlCommand cmd;

                    if (IdsMetodePlata == null)
                    {
                        IdsMetodePlata = new List<int>();
                    }
                    if (IdsMetodePlata.Count == 0)
                    {
                        sql = @"SELECT [ID] FROM [SOLON.H].[financiar].[MetodeDePlata] WHERE Sters = 0";
                        cmd = new SqlCommand(sql, cnn);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IdsMetodePlata.Add(Convert.ToInt32(reader[0]));
                            }
                        }
                    }
                    if (IdsMetodePlata.Count == 0)
                    {
                        throw new Exception("Nu exista metode de plata nesterse!");
                    }

                    string ids = IdsMetodePlata.Select(x => x.ToString()).Aggregate((x1, x2) => x1 + "," + x2);
                        if (EsteReceptie && EsteVirament)
                        {
                            sql = @"
                        SELECT
                             c.[ID]
                            ,c.[Serie]
                            ,c.[NumarDeLa]
                            ,c.[NumarPanaLa]
                            ,c.[EsteReceptie]
                            ,c.[EsteVirament]
                            ,c.[EsteAvans]
                            ,m.[ID] AS mID
                            ,m.[Cod] AS mCod
                            ,m.[Denumire] AS mDenumire
                            ,m.[Ordine] AS mOrdine
                            ,part.[IdPartener] AS IdFurnizor
                            ,part.[NumePartener] AS NumePartener
                        FROM 
                            [SOLON.H].[financiar].[CarneteDocumenteEmise] AS c INNER JOIN
                            [SOLON.H].[financiar].[CarneteDocEmiseMetPlata] as cm ON cm.[IDCarnet] = c.[ID] INNER JOIN
                            [SOLON.H].[financiar].[MetodeDePlata] as m on m.[ID] = cm.[IDMetodaPlata] LEFT OUTER JOIN
                            [Solon].[dbo].[NomParteneri] as part on part.IdPartener = c.IdPartener
                        WHERE
                            c.[InFolosinta] = 1 AND
                            c.[Sters] = 0 AND
                            c.[Consumat] = 0 AND
                            c.[ValabilDeLa] <= @DeLa AND
                            c.[ValabilPanaLa] >= @PanaLa AND
                            c.[IdHotel] = @IdHotel AND
                            (c.[EsteReceptie] = @Rec1 OR 
                            c.[EsteVirament] = @Rec2) AND
                            m.[Sters] = 0 AND
                            c.[TipDocument] like 'TDEFAC' AND
                            m.ID IN (<ids>) AND
                            part.[IdPartener] = @IdPartener
                    ";
                        }
                        else if (EsteReceptie)
                        {
                            sql = @"
                        SELECT
                             c.[ID]
                            ,c.[Serie]
                            ,c.[NumarDeLa]
                            ,c.[NumarPanaLa]
                            ,c.[EsteReceptie]
                            ,c.[EsteVirament]
                            ,c.[EsteAvans]
                            ,m.[ID] AS mID
                            ,m.[Cod] AS mCod
                            ,m.[Denumire] AS mDenumire
                            ,m.[Ordine] AS mOrdine
                            ,part.[IdPartener] AS IdFurnizor
                            ,part.[NumePartener] AS NumePartener
                        FROM 
                            [SOLON.H].[financiar].[CarneteDocumenteEmise] AS c INNER JOIN
                            [SOLON.H].[financiar].[CarneteDocEmiseMetPlata] as cm ON cm.[IDCarnet] = c.[ID] INNER JOIN
                            [SOLON.H].[financiar].[MetodeDePlata] as m on m.[ID] = cm.[IDMetodaPlata] LEFT OUTER JOIN
                            [Solon].[dbo].[NomParteneri] as part on part.IdPartener = c.IdPartener
                        WHERE
                            c.[InFolosinta] = 1 AND
                            c.[Sters] = 0 AND
                            c.[Consumat] = 0 AND
                            c.[ValabilDeLa] <= @DeLa AND
                            c.[ValabilPanaLa] >= @PanaLa AND
                            c.[IdHotel] = @IdHotel AND
                            c.[EsteReceptie] = @Rec1 AND
                            m.[Sters] = 0 AND
                            c.[TipDocument] like 'TDEFAC' AND
                            m.ID IN (<ids>) AND
                            part.[IdPartener] = @IdPartener
                    ";
                        }
                        else if (EsteVirament)
                        {
                            sql = @"
                        SELECT
                             c.[ID]
                            ,c.[Serie]
                            ,c.[NumarDeLa]
                            ,c.[NumarPanaLa]
                            ,c.[EsteReceptie]
                            ,c.[EsteVirament]
                            ,c.[EsteAvans]
                            ,m.[ID] AS mID
                            ,m.[Cod] AS mCod
                            ,m.[Denumire] AS mDenumire
                            ,m.[Ordine] AS mOrdine
                            ,part.[IdPartener] AS IdFurnizor
                            ,part.[NumePartener] AS NumePartener
                        FROM 
                            [SOLON.H].[financiar].[CarneteDocumenteEmise] AS c INNER JOIN
                            [SOLON.H].[financiar].[CarneteDocEmiseMetPlata] as cm ON cm.[IDCarnet] = c.[ID] INNER JOIN
                            [SOLON.H].[financiar].[MetodeDePlata] as m on m.[ID] = cm.[IDMetodaPlata] LEFT OUTER JOIN
                            [Solon].[dbo].[NomParteneri] as part on part.IdPartener = c.IdPartener
                        WHERE
                            c.[InFolosinta] = 1 AND
                            c.[Sters] = 0 AND
                            c.[Consumat] = 0 AND
                            c.[ValabilDeLa] <= @DeLa AND
                            c.[ValabilPanaLa] >= @PanaLa AND
                            c.[IdHotel] = @IdHotel AND
                            c.[EsteVirament] = @Rec2 AND
                            m.[Sters] = 0 AND
                            c.[TipDocument] like 'TDEFAC' AND
                            m.ID IN (<ids>) AND
                            part.[IdPartener] = @IdPartener
                    ";
                        }


                    sql = sql.Replace("<ids>", ids);
                    cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@DeLa", SqlDbType.DateTime)).Value = ConexiuneDB.DataLucr;
                    cmd.Parameters.Add(new SqlParameter("@PanaLa", SqlDbType.DateTime)).Value = ConexiuneDB.DataLucr;
                    cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.Int)).Value = ConexiuneDB.IdHotel;
                    cmd.Parameters.Add(new SqlParameter("@IdPartener", SqlDbType.Int)).Value = IdFurnizor;
                   
                        if (!EsteAvans)
                        {
                            if (EsteReceptie && EsteVirament)
                            {
                                cmd.Parameters.Add(new SqlParameter("@Rec1", SqlDbType.Bit)).Value = EsteReceptie;
                                cmd.Parameters.Add(new SqlParameter("@Rec2", SqlDbType.Bit)).Value = EsteVirament;
                            }
                            else if (EsteReceptie)
                            {
                                cmd.Parameters.Add(new SqlParameter("@Rec1", SqlDbType.Bit)).Value = EsteReceptie;
                            }
                            else if (EsteVirament)
                            {
                                cmd.Parameters.Add(new SqlParameter("@Rec2", SqlDbType.Bit)).Value = EsteVirament;
                            }
                        }
                        else
                        {
                            if (EsteReceptie && EsteVirament)
                            {
                                cmd.Parameters.Add(new SqlParameter("@Rec1", SqlDbType.Bit)).Value = EsteReceptie;
                                cmd.Parameters.Add(new SqlParameter("@Rec2", SqlDbType.Bit)).Value = EsteVirament;
                                cmd.Parameters.Add(new SqlParameter("@Rec3", SqlDbType.Bit)).Value = EsteAvans;
                            }
                            else if (EsteReceptie)
                            {
                                cmd.Parameters.Add(new SqlParameter("@Rec1", SqlDbType.Bit)).Value = EsteReceptie;
                                cmd.Parameters.Add(new SqlParameter("@Rec3", SqlDbType.Bit)).Value = EsteAvans;
                            }
                            else if (EsteVirament)
                            {
                                cmd.Parameters.Add(new SqlParameter("@Rec2", SqlDbType.Bit)).Value = EsteVirament;
                                cmd.Parameters.Add(new SqlParameter("@Rec3", SqlDbType.Bit)).Value = EsteAvans;
                            }
                        }
                    

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        EntitateCarnetFacturi ec;
                        int id;
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["ID"]);
                            ec = carnete.FirstOrDefault(x => x.ID == id);
                            if (ec == null)
                            {
                                ec = new EntitateCarnetFacturi();
                                ec.ID = id;
                                ec.Serie = reader["Serie"].ToString();
                                ec.NumarDeLa = reader["NumarDeLa"].ToString();
                                ec.NumarPanaLa = reader["NumarPanaLa"].ToString();
                                ec.EsteReceptie = Convert.ToBoolean(reader["EsteReceptie"]);
                                ec.EsteVirament = Convert.ToBoolean(reader["EsteVirament"]);
                                ec.EsteAvans = Convert.ToBoolean(reader["EsteAvans"]);
                                ec.IdFurnizor = Convert.ToInt32(reader["IdFurnizor"]);
                                ec.NumeFurnizor = reader["NumePartener"].ToString();
                                carnete.Add(ec);
                            }
                            ec.MetodePlata.Add(new EntitateMetodaPlata()
                            {
                                ID = Convert.ToInt32(reader["mID"]),
                                Cod = reader["mCod"].ToString(),
                                Denumire = reader["mDenumire"].ToString(),
                                //Ordine = Convert.ToInt32(reader["mOrdine"])
                            });
                        }
                    }

                    sql = @"
                        SELECT TOP 1 
                            Numar,
                            Data
                        FROM [SOLON.H].[financiar].[Facturi] 
                        WHERE 
                            IdCarnet = @IdCarnet AND 
                            Emisa = 1
                        ORDER BY ID DESC
                        --ORDER BY Data DESC, Numar DESC
                    ";
                    cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdCarnet", SqlDbType.Int));
                    foreach (EntitateCarnetFacturi car in carnete)
                    {
                        cmd.Parameters["@IdCarnet"].Value = car.ID;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                car.LastNumber = reader[0] == DBNull.Value ? "" : reader[0].ToString();
                                car.LastData = reader[1] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader[1]);
                            }
                            else
                            {
                                car.LastNumber = "0";
                                car.LastData = DateTime.MinValue;
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    //Log.ExceptieSql(exc, "FormSelCarnetFacturi.GetLista()");
                }
            }

            foreach (EntitateCarnetFacturi ecf in carnete)
            {
                ecf.MetodePlata = ecf.MetodePlata.ToList();
                ecf.MetodePlata_Proxy = ecf.MetodePlata.Select(x => string.IsNullOrEmpty(x.Cod) ? x.Denumire : x.Cod).Aggregate((x1, x2) => x1 + ", " + x2);
                ecf.EsteReceptie_Proxy = ecf.EsteReceptie ? "DA" : "";
                ecf.EsteVirament_Proxy = ecf.EsteVirament ? "DA" : "";

                long n1 = 0, n2 = 0, ln;

                string initial = ecf.NumarDeLa;
                string spec = "";
                for (int i = 1; i <= initial.Length; i++)
                {
                    spec += "0";
                }

                if (long.TryParse(ecf.NumarDeLa, out n1) && long.TryParse(ecf.NumarPanaLa, out n2))
                {
                    if (ecf.LastNumber == "0")
                    {
                        ecf.Numar = n1.ToString(spec);
                    }
                    else
                    {
                        if (long.TryParse(ecf.LastNumber, out ln))
                        {
                            ecf.LastNumberParsed = ln;
                            ecf.Numar = (ln + 1).ToString(spec);
                        }
                    }
                }
            }
            carnete = carnete.OrderBy(x => x.Serie).ToList();

            return carnete;
        }

        public static void GetIdsMetodePlata(List<int> idsServicii, ref List<int> idsMetode, ref bool esteRec, ref bool esteVir)
        {
            if (idsServicii == null || idsServicii.Count == 0)
            {
                return;
            }
            if (idsMetode == null)
            {
                return;
            }
            string ids = idsServicii.Select(x => x.ToString()).Aggregate((x1, x2) => x1 + "," + x2);
            bool setatRec = false;
            bool setatVir = false;
            HashSet<int> hs = new HashSet<int>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"
                        SELECT
                             rs.[EsteVirament]
                            ,pm.[IdMetodaPlata]
                        FROM
                            [SOLON.H].hotel.RezervariServicii AS rs LEFT OUTER JOIN
                            [SOLON.H].financiar.PlatiServicii AS ps ON ps.IdRezervareServiciu = rs.ID LEFT OUTER JOIN
                            [SOLON.H].financiar.Plati AS p ON p.ID = ps.IdPlata LEFT OUTER JOIN
                            [SOLON.H].financiar.PlatiMetodePlata AS pm ON pm.IdPlata = p.ID
                        WHERE
                            (p.Sters = 0 OR p.ID IS NULL) AND 
                            rs.ID in (<ids>)
                    ";
                    sql = sql.Replace("<ids>", ids);
                    SqlCommand cmd = new SqlCommand(sql, cnn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        bool ev;
                        int id;
                        while (reader.Read())
                        {
                            ev = Convert.ToBoolean(reader[0]);
                            if (ev)
                            {
                                setatVir = true;
                                esteVir = true;
                            }
                            else
                            {
                                setatRec = true;
                                esteRec = true;
                            }
                            if (reader[1] != DBNull.Value)
                            {
                                id = Convert.ToInt32(reader[1]);
                                if (!hs.Contains(id))
                                {
                                    hs.Add(id);
                                }
                            }
                        }
                    }
                    if (!setatVir && !setatRec)
                    {
                        esteRec = esteVir = true;
                    }
                    else if (!setatVir)
                    {
                        esteVir = false;
                    }
                    else if (!setatRec)
                    {
                        esteRec = false;
                    }

                    if (hs.Count == 0)
                    {
                        sql = "SELECT ID FROM [SOLON.H].financiar.MetodeDePlata WHERE Sters = 0";
                        cmd = new SqlCommand(sql, cnn);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                hs.Add(Convert.ToInt32(reader[0]));
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    //Log.ExceptieSql(exc, "HelperFactura.GetIdsMetodePlata");
                }
            }
            idsMetode.Clear();
            idsMetode.AddRange(hs);
        }

        public static void Asociaza(int IdFactura, int IdPlata, int IdTipAsociere)
        {
            List<EntitateAsociereFacturaPlata> asocieriFacturaPlata = new List<EntitateAsociereFacturaPlata>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = "";
                    SqlCommand cmd;

                    sql = @"SELECT  psvmp.[ID] AS IDPLATASERV
	                               ,fs.[ID] AS ID_FAC_SERV
	                               ,psvmp.[IDRezervareServiciu] AS ID_REZ_SERV
	                               ,pl.ID AS ID_PLATA
	                               ,pl.DataLucru AS DATA_PLATA
	                               ,fac.ID AS ID_FACTURA
	                               ,fac.Data AS DATA_FAC
                             FROM  [SOLON.H].[financiar].[PlatiServiciiValoriModPlata] AS psvmp LEFT OUTER JOIN
                                   [SOLON.H].[financiar].[Plati] AS pl ON pl.[ID] = psvmp.[IDPlata] LEFT OUTER JOIN
                                   [SOLON.H].[financiar].[FacturiServicii] AS fs ON fs.[IdRezervareServiciu] = psvmp.[IDRezervareServiciu] LEFT OUTER JOIN
                                   [SOLON.H].[financiar].[Facturi] AS fac ON fac.[ID] = fs.[IdFactura]
                            WHERE  psvmp.[IDPlata] = @IdPlata AND fac.[ID] = @Factura";
                    cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdPlata", SqlDbType.Int)).Value = IdPlata;
                    cmd.Parameters.Add(new SqlParameter("@Factura", SqlDbType.Int)).Value = IdFactura;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EntitateAsociereFacturaPlata eafp = new EntitateAsociereFacturaPlata
                            {
                                IdPSVMP = reader["IDPLATASERV"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IDPLATASERV"]),
                                IdFacturaServ = reader["ID_FAC_SERV"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_FAC_SERV"]),
                                IdRezervareServiciu = reader["ID_REZ_SERV"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_REZ_SERV"]),
                                IdPlata = reader["ID_PLATA"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_PLATA"]),
                                DataPlata = reader["DATA_PLATA"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DATA_PLATA"]),
                                IdFactura = reader["ID_FACTURA"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID_FACTURA"]),
                                DataFactura = reader["DATA_FAC"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DATA_FAC"])
                            };

                            if (eafp.AsociereValida)
                            {
                                asocieriFacturaPlata.Add(eafp);
                            }
                        }
                    }

                    sql = @"
                        INSERT INTO [SOLON.H].[financiar].[Facturi_Plati]
                                   ([IDPlatiServValModPlata]
                                   ,[IDFacturaServiciu]
                                   ,[IDRezervareServiciu]
                                   ,[IDPlata]
                                   ,[DataPlata]
                                   ,[IDFactura]
                                   ,[DataFactura]
                                   ,[TipLegatura])
                             VALUES
                                   (@IDPlatiServValModPlata
                                   ,@IDFacturaServiciu
                                   ,@IDRezervareServiciu
                                   ,@IDPlata
                                   ,@DataPlata
                                   ,@IDFactura
                                   ,@DataFactura
                                   ,@TipLegatura)";
                    foreach (EntitateAsociereFacturaPlata easfp in asocieriFacturaPlata)
                    {
                        cmd = new SqlCommand(sql, cnn);
                        cmd.Parameters.Add(new SqlParameter("@IDPlatiServValModPlata", SqlDbType.Int)).Value = easfp.IdPSVMP;
                        cmd.Parameters.Add(new SqlParameter("@IDFacturaServiciu", SqlDbType.Int)).Value = easfp.IdFacturaServ;
                        cmd.Parameters.Add(new SqlParameter("@IDRezervareServiciu", SqlDbType.Int)).Value = easfp.IdRezervareServiciu;
                        cmd.Parameters.Add(new SqlParameter("@IDPlata", SqlDbType.Int)).Value = easfp.IdPlata;
                        cmd.Parameters.Add(new SqlParameter("@DataPlata", SqlDbType.DateTime)).Value = easfp.DataPlata;
                        cmd.Parameters.Add(new SqlParameter("@IDFactura", SqlDbType.Int)).Value = easfp.IdFactura;
                        cmd.Parameters.Add(new SqlParameter("@DataFactura", SqlDbType.DateTime)).Value = easfp.DataFactura;
                        cmd.Parameters.Add(new SqlParameter("@TipLegatura", SqlDbType.Int)).Value = IdTipAsociere;

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception exc)
                {
                    //LogErori.Salveaza(exc, "HelperFactura.Asociaza()", null);
                }
            }
        }

        internal static void EliminaAsociere(int IDAsociere, int IdPlata, int IdFactura)
        {
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = "";
                    SqlCommand cmd;
                    sql = @"UPDATE [SOLON.H].[financiar].[Facturi_Plati]
                               SET [Sters] = 1
                             WHERE 
                                   <fitru_asoc>
                                   <fitru_plata>
                                   <fitru_factura>";
                    if (IDAsociere != 0 && IdPlata == 0 && IdFactura == 0)
                    {
                        sql = sql.Replace("<fitru_asoc>", "ID = @ID");
                        sql = sql.Replace("<fitru_plata>", "");
                        sql = sql.Replace("<fitru_factura>", "");
                    }
                    else if (IdPlata != 0 && IdFactura != 0)
                    {
                        sql = sql.Replace("<fitru_asoc>", "");
                        sql = sql.Replace("<fitru_plata>", "IDPlata = @IDPlata");
                        sql = sql.Replace("<fitru_factura>", "AND IDFactura = @IDFactura");
                    }
                    else if (IdPlata != 0)
                    {
                        sql = sql.Replace("<fitru_asoc>", "");
                        sql = sql.Replace("<fitru_plata>", "IDPlata = @IDPlata");
                        sql = sql.Replace("<fitru_factura>", "");
                    }
                    else if (IdFactura != 0)
                    {
                        sql = sql.Replace("<fitru_asoc>", "");
                        sql = sql.Replace("<fitru_plata>", "");
                        sql = sql.Replace("<fitru_factura>", "IDFactura = @IDFactura");
                    }

                    cmd = new SqlCommand(sql, cnn);

                    if (IDAsociere != 0 && IdPlata == 0 && IdFactura == 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = IDAsociere;
                    }
                    else if (IdPlata != 0 && IdFactura != 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@IDPlata", SqlDbType.Int)).Value = IdPlata;
                        cmd.Parameters.Add(new SqlParameter("@IDFactura", SqlDbType.Int)).Value = IdFactura;
                    }
                    else if (IdPlata != 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@IDPlata", SqlDbType.Int)).Value = IdPlata;
                    }
                    else if (IdFactura != 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@IDFactura", SqlDbType.Int)).Value = IdFactura;
                    }

                    cmd.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    //LogErori.Salveaza(exc, "HelperFactura.EliminaAsociere()", null);
                }
            }
        }

        internal static void ListaFacturaAgentie(List<int> list)
        {
            throw new NotImplementedException();
        }

        public static bool EsteFiscalizatAvans(List<int> list, out string infoAvansFacturat)
        {
            bool rv = true;
            infoAvansFacturat = "";
            HashSet<int> iduriCuFiscal = new HashSet<int>();

            List<EntitateServiciu> lstServicii = new List<EntitateServiciu>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    SqlCommand cmd;
                    string sql = @"
                               SELECT DISTINCT f.IdClient,rs.ID,rs.DenumireServiciu,rsv.Data,rsv.ValoareRON,rs.IdRezervareCamera
                                        FROM [SOLON.H].[hotel].[RezervariServicii] AS rs LEFT OUTER JOIN
                                        [SOLON.H].[Hotel].[RezervariAvans] AS ra ON ra.IdRezervareCamera = rs.IdRezervareCamera LEFT OUTER JOIN
                                        [SOLON.H].[hotel].[RezervariServiciiValori] AS rsv ON rs.ID = rsv.IdRezervareServiciu 
                                        LEFT OUTER JOIN
                                        (SELECT fc.IdClient,fc.ID FROM [SOLON.H].financiar.Facturi AS fc LEFT OUTER JOIN
                                            [SOLON.H].financiar.Facturi AS st ON fc.ID = st.StornoLaFactura AND st.Anulata = 0
                                            WHERE fc.Anulata = 0 AND ISNULL(st.ID,0) = 0) AS f ON ra.IdFactura = f.ID
                                        LEFT OUTER JOIN
                                        (SELECT pl.IdPartener AS IdClient,pl.ID  FROM [SOLON.H].financiar.Plati AS pl LEFT OUTER JOIN
                                        [SOLON.H].financiar.TipDocumentPlata AS tdp ON pl.IdTipDocPlata = tdp.ID
	                                            WHERE pl.Sters = 0 AND tdp.Fiscal = 1) AS p ON ra.IdPlata = p.ID
                                        WHERE
	                                            rs.Sters = 0 
                                            AND rs.EsteStorno = 1 
                                            AND ra.ID IS NOT NULL 
                                            AND ra.Utilizat = 1
                                            AND ISNULL(f.ID,0) = 0
                                            AND ISNULL(p.ID,0) = 0 
                                        AND rs.ID in (<ids_servicii>)
                            ";
                    sql = sql.Replace("<ids_servicii>", list.Aggregate("-1", (x, y) => x + "," + y));
                    cmd = new SqlCommand(sql, cnn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int IDServiciu = reader["ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID"]);
                            string DenumireServiciu = reader["DenumireServiciu"] == DBNull.Value ? "" : reader["DenumireServiciu"].ToString();
                            DateTime DataServiciu = reader["Data"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["Data"]);
                            Decimal ValoareServiciu = reader["ValoareRON"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareRON"]);
                            int Folio = reader["IdRezervareCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdRezervareCamera"]);
                            if (!iduriCuFiscal.Contains(IDServiciu))
                            {
                                iduriCuFiscal.Add(IDServiciu);
                                //PASCU ADRIAN 03.03.2017
                                //Daca ii apare cuiva un mesaj mai mare de 5 stornouri.. decomentati randurile
                                //if (iduriCuFiscal.Count <= 5)
                                //{
                                infoAvansFacturat += string.Format("AVANSUL aferent pozitiei: {0} din {1} in valoare de {2} FOLIO {3} nu este fiscalizat {4}",
                                    DenumireServiciu, DataServiciu.ToString("dd.MM.yyyy"), ValoareServiciu.ToString("N2"), Folio.ToString(), Environment.NewLine);
                                //}
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    //LogErori.Salveaza(exc, "", null);
                }
            }
            //PASCU ADRIAN 03.03.2017
            //Daca ii apare cuiva un mesaj mai mare de 5 stornouri.. decomentati randurile
            //if (iduriCuFiscal.Count > 5)
            //{
            //    infoAvansFacturat += "...(inca " + (iduriCuFiscal.Count - 5) + ")";
            //}
            rv = iduriCuFiscal.Count == 0;


            return rv;
        }

        internal static bool EstePlataFaraFactura(int IdPlata)
        {
            bool rv = false;

            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    SqlCommand cmd;
                    string sql = @"SELECT [PlataFaraFactura] FROM [SOLON.H].[financiar].[Plati] WHERE ID = @IdPlata";
                    cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdPlata", SqlDbType.Int)).Value = IdPlata;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            rv = reader["PlataFaraFactura"] == DBNull.Value ? false : Convert.ToBoolean(reader["PlataFaraFactura"]);
                        }
                    }
                }
                catch (Exception exc)
                {
                    //LogErori.Salveaza(exc, "", null);
                }
            }
            return rv;
        }

        internal static void AreFacturaPlataRapida(int idPlata, bool Are)
        {
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    SqlCommand cmd;
                    string sql = @"UPDATE [SOLON.H].[financiar].[Plati] SET [PlataFaraFactura] = @Are WHERE ID = @IdPlata";
                    cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdPlata", SqlDbType.Int)).Value = idPlata;
                    cmd.Parameters.Add(new SqlParameter("@Are", SqlDbType.Bit)).Value = Are;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    //LogErori.Salveaza(exc, "", null);
                }
            }
        }

        internal static bool EsteFacturaPreluataInConta(int IdFactura)
        {
            bool rv = false;

            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    SqlCommand cmd;
                    string sql = @"SELECT [Preluat] FROM [SOLON.H].[financiar].[Facturi] WHERE ID = @IdFactura";
                    cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = IdFactura;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            rv = reader["Preluat"] == DBNull.Value ? false : Convert.ToBoolean(reader["Preluat"]);
                        }
                    }
                }
                catch (Exception exc)
                {
                    //LogErori.Salveaza(exc, "", null);
                }
            }
            return rv;
        }

        internal static bool EstePlataPreluataInConta(int IdPlata)
        {
            bool rv = false;

            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    SqlCommand cmd;
                    string sql = @"SELECT [Preluat] FROM [SOLON.H].[financiar].[Plati] WHERE ID = @IdPlata";
                    cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdPlata", SqlDbType.Int)).Value = IdPlata;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            rv = reader["Preluat"] == DBNull.Value ? false : Convert.ToBoolean(reader["Preluat"]);
                        }
                    }
                }
                catch (Exception exc)
                {
                    //LogErori.Salveaza(exc, "", null);
                }
            }
            return rv;
        }
    }
}