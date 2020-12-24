using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare_Final
{
        public class HelperValoriZilniceFactura
        {
            public static void AnuleazaFactura(int idFactura, SqlConnection cnn, SqlTransaction tran)
            {
                string sql = @"
                UPDATE [SOLON.H].[financiar].[FacturiValoriZilnice]
                SET
	                [Anulat] = 1
                WHERE
                    [IdFactura] = @IdFactura
            ";
                SqlCommand cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                cmd.ExecuteNonQuery();
            }

            public static void ReRepartizeazaFactura(int idFactura)
            {
                using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                {
                    try
                    {
                        cnn.Open();
                        using (SqlTransaction tran = cnn.BeginTransaction())
                        {
                            try
                            {
                                ReRepartizeazaFactura(idFactura, cnn, tran);
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
                        //LogErori.Salveaza(exc, "HelperFactura.ReRepartizeazaFactura() - " + idFactura.ToString(), null);
                    }
                }
            }

            public static void ReRepartizeazaFactura(int idFactura, SqlConnection cnn, SqlTransaction tran)
            {
                List<EntitateServiciu> lista = new List<EntitateServiciu>();
                Dictionary<int, EntitateServiciu> dic = new Dictionary<int, EntitateServiciu>();  // cheia este id serviciu
                Dictionary<int, FacturaServicii> dicFS = new Dictionary<int, FacturaServicii>();                    // cheia este id serviciu
                bool EsteAvans = false;

                string sql = "";
                SqlCommand cmd = null;

                bool anulata = false;
                // 1. Verificare daca factura este anulata
                sql = @"
                SELECT
                    [Anulata]
                FROM 
                    [SOLON.H].[financiar].[Facturi]
                WHERE
                    [ID] = @ID
            ";
                cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = idFactura;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        anulata = Convert.ToBoolean(reader[0]);
                    }
                }
                if (anulata)
                {
                    return;
                }

                // 2. Stergere repartizare valori pe factura
                sql = @"
                DELETE FROM [SOLON.H].[financiar].[FacturiValoriZilnice]
                WHERE
                    [IdFactura] = @IdFactura
            ";
                cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                cmd.ExecuteNonQuery();

                // 3. Incarcare servicii cu valorile lor si cat a fost deja facturat din ele
                sql = @"
                SELECT
                     s.[ID]
                    ,s.[IdPartener]
                    ,s.[EsteVirament]
                    ,s.[IdRezervareCamera]
                    ,s.[IdVenit]
                    ,s.[IdTarif]
                    ,s.[IdPlanMasa]
                    ,s.[DenumireServiciu]
                    ,s.[IdMoneda]
                    ,s.[IdCotaTVA]
                    ,s.[TaxaProcentuala]
                    ,s.[TaxaLaPersoane]
                    ,s.[ValoareTaxa]
                    ,s.[Observatii]
                    ,s.[Sters]
                    ,s.[IdMotivStergere]
                    ,s.[Cantitate]
                    ,s.[PostareAmanata]
                    ,s.[IdAvansRezervare]
                    ,s.[ValoareMoneda]
                    ,s.[Curs]
                    ,s.[ValoareRon]
                    ,m.[MonedaCod]
                    ,c.[Procent]
                    ,c.[Denumire] AS CodCotaTVA
                    ,p.[NumePartener]
                    ,v.[EsteTaxa]
                    ,v.[EsteCazare]
                    ,v.[EsteMasa]
                    ,v.[IdGrupaFact]
                    ,v.[IdGrupaFact2]
                    ,n1.[Denumire] AS Grupa1
                    ,n2.[Denumire] AS Grupa2
                    ,cam.[Denumire] AS Camera
                    ,ra.IdRezervareCamera RezCamAvans
                    ,s.[ObsFactura] AS ObsFactura
                    ,s.[EsteStorno] AS EsteStorno
                FROM 
                    [SOLON.H].[hotel].[RezervariServicii] AS s INNER JOIN
                    [SOLON.H].[hotel].[Venituri] AS v ON v.[ID] = s.[IdVenit] INNER JOIN
                    [SOLON].[dbo].[NomMonede] AS m ON m.[IdMoneda] = s.[IdMoneda] INNER JOIN
                    [SOLON].[dbo].[CoteTva] AS c ON c.[IdCota] = s.[IdCotaTVA] LEFT OUTER JOIN
                    [SOLON].[dbo].[NomParteneri] AS p ON p.[IdPartener] = s.[IdPartener] LEFT OUTER JOIN
                    [SOLON.H].[setari].[Nomenclatoare] AS n1 ON n1.ID = v.[IdGrupaFact] LEFT OUTER JOIN
                    [SOLON.H].[setari].[Nomenclatoare] AS n2 ON n2.ID = v.[IdGrupaFact2] LEFT OUTER JOIN
                    [SOLON.H].[hotel].[RezervariCamere] AS rc ON rc.id = s.IdRezervareCamera LEFT OUTER JOIN
                    [SOLON.H].[hotel].[Camere] AS cam ON cam.ID = rc.IdCamera LEFT OUTER JOIN
                    [SOLON.H].[hotel].[RezervariAvans] AS ra ON s.IdAvansRezervare = ra.ID
                WHERE 
                    s.[ID] IN (SELECT [IdRezervareServiciu] FROM [SOLON.H].[financiar].[FacturiServicii] WHERE [IdFactura] = @IdFactura)
            ";
                cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    EntitateServiciu serv;
                    while (reader.Read())
                    {
                        int IdRezCam = reader["IdRezervareCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdRezervareCamera"]);
                        if (EsteAvans)
                        {
                            IdRezCam = reader["RezCamAvans"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RezCamAvans"]);
                        }

                        if (IdRezCam == 0)
                        {
                            IdRezCam = reader["IdRezervareCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdRezervareCamera"]);
                        }

                        serv = new EntitateServiciu()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            EsteVirament = Convert.ToBoolean(reader["EsteVirament"]),
                            IdPartener = reader["IdPartener"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdPartener"]),
                            IdRezervareCamera = IdRezCam,
                            IdVenit = Convert.ToInt32(reader["IdVenit"]),
                            IdTarif = reader["IdTarif"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdTarif"]),
                            IdPlanMasa = reader["IdPlanMasa"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdPlanMasa"]),
                            DenumireServiciu = reader["DenumireServiciu"].ToString(),
                            IdMoneda = Convert.ToInt32(reader["IdMoneda"]),
                            IdCotaTVA = Convert.ToInt32(reader["IdCotaTVA"]),
                            TaxaProcentuala = Convert.ToBoolean(reader["TaxaProcentuala"]),
                            TaxaLaPersoane = Convert.ToBoolean(reader["TaxaLaPersoane"]),
                            ValoareTaxa = Convert.ToDecimal(reader["ValoareTaxa"]),
                            Observatii = reader["Observatii"] == DBNull.Value ? "" : reader["Observatii"].ToString(),
                            Sters = Convert.ToBoolean(reader["Sters"]),
                            IdMotivStergere = reader["IdMotivStergere"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdMotivStergere"]),
                            CodMoneda = reader["MonedaCod"].ToString(),
                            ProcentTVA = Convert.ToDecimal(reader["Procent"]),
                            CodCotaTVA = reader["CodCotaTVA"].ToString(),
                            NumePartener = reader["NumePartener"] == DBNull.Value ? "" : reader["NumePartener"].ToString(),
                            Cantitate = Convert.ToInt32(reader["Cantitate"]),
                            EsteTaxa = Convert.ToBoolean(reader["EsteTaxa"]),
                            EsteCazare = Convert.ToBoolean(reader["EsteCazare"]),
                            EsteMasa = Convert.ToBoolean(reader["EsteMasa"]),
                            PostareAmanata = Convert.ToBoolean(reader["PostareAmanata"]),
                            IdGrupaFact1 = reader["IdGrupaFact"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdGrupaFact"]),
                            IdGrupaFact2 = reader["IdGrupaFact2"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdGrupaFact2"]),
                            GrupaFact1 = reader["Grupa1"] == DBNull.Value ? "" : reader["Grupa1"].ToString(),
                            GrupaFact2 = reader["Grupa2"] == DBNull.Value ? "" : reader["Grupa2"].ToString(),
                            Camera = reader["Camera"] == DBNull.Value ? "" : reader["Camera"].ToString(),
                            ValoareMoneda = reader["ValoareMoneda"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareMoneda"]),
                            Curs = reader["Curs"] == DBNull.Value ? 1 : Convert.ToDecimal(reader["Curs"]),
                            ValoareRon = reader["ValoareRon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareRon"]),
                            ObsFactura = reader["ObsFactura"] == DBNull.Value ? "" : reader["ObsFactura"].ToString(),
                            IdAvansRezervare = reader["IdAvansRezervare"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdAvansRezervare"]),
                            EsteStorno = reader["EsteStorno"] == DBNull.Value ? false : Convert.ToBoolean(reader["EsteStorno"])
                        };
                        lista.Add(serv);
                        dic.Add(serv.ID, serv);
                    }
                }
                Dictionary<int, int> valZilnice = new Dictionary<int, int>();
                sql = @"
                SELECT
                     v.[ID]
                    ,v.[IdRezervareServiciu]
                    ,v.[Data]
                    ,v.[Valoare]
                    ,v.[Postat]
                    ,v.[ValoareRON]
                FROM
                    [SOLON.H].[hotel].[RezervariServicii] AS s INNER JOIN 
                    [SOLON.H].[hotel].[RezervariServiciiValori] AS v ON v.[IdRezervareServiciu] = s.[ID]
                WHERE s.[ID] IN (SELECT [IdRezervareServiciu] FROM [SOLON.H].[financiar].[FacturiServicii] WHERE [IdFactura] = @IdFactura)
            ";
                cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    EntitateServiciu serv;
                    int idServ;
                    while (reader.Read())
                    {
                        idServ = Convert.ToInt32(reader["IdRezervareServiciu"]);
                        serv = dic[idServ];
                        int IdValZilnica = reader["ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID"]);

                        if (!valZilnice.ContainsKey(IdValZilnica))
                        {
                            valZilnice.Add(IdValZilnica, idServ);
                        }
                        serv.Valori.Add(new EntitateServiciuValoare()
                        {
                            ID = IdValZilnica,
                            IdRezervareServiciu = idServ,
                            Data = Convert.ToDateTime(reader["Data"]),
                            Valoare = Convert.ToDecimal(reader["Valoare"]),
                            Postat = Convert.ToBoolean(reader["Postat"]),
                            ValoareRON = Convert.ToDecimal(reader["ValoareRON"])
                        });
                    }
                }
                // factura curenta care se repartizeaza nu da rezultate la query-ul urmator
                // deoarece FacturiValoriZilnice-le daca existau au fost deja sterse
                sql = @"
                SELECT 
                     fsvz.[ID]
                    ,fsvz.[IdValoareZilnica]
                    ,[ValoareRON]
                    ,[ValoareMonedaServ]
                FROM 
                    [SOLON.H].[financiar].[FacturiValoriZilnice] AS fsvz LEFT OUTER JOIN
                    [SOLON.H].[financiar].[Facturi] AS f ON f.ID = fsvz.IdFactura LEFT OUTER JOIN
                    [SOLON.H].[financiar].[Facturi] AS fstorno ON fstorno.StornoLaFactura = f.ID
                WHERE 
                    fsvz.[IdValoareZilnica] IN (<valori_zilnice>) AND 
                    f.[Anulata] = 0 AND 
                    f.[StornoLaFactura] = 0 AND 
                    (fstorno.ID IS NULL OR (fstorno.ID IS NOT NULL AND fstorno.Anulata = 1))
            ";
                sql = sql.Replace("<valori_zilnice>", valZilnice.Keys.Aggregate("-1", (x, y) => x + "," + y));
                HashSet<int> idurivals = new HashSet<int>();
                cmd = new SqlCommand(sql, cnn, tran);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    EntitateServiciu serv;
                    int idServ;
                    while (reader.Read())
                    {
                        int ID = reader["ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID"]);
                        if (!idurivals.Contains(ID))
                        {
                            idurivals.Add(ID);

                            int IdValZilnica = reader["IdValoareZilnica"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdValoareZilnica"]);

                            if (valZilnice.ContainsKey(IdValZilnica))
                            {
                                idServ = valZilnice[IdValZilnica];

                                serv = dic[idServ];

                                EntitateServiciuValoare val = serv.Valori.FirstOrDefault(x => x.ID == IdValZilnica);
                                if (val != null)
                                {
                                    // valorile facturate deja pe fiecare valoare zilnica
                                    val.FacturatMoneda += reader["ValoareMonedaServ"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareMonedaServ"]);
                                    val.FacturatRon += reader["ValoareRON"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareRON"]);
                                }
                            }
                        }
                    }
                }

                // 4. Incarcare factura (valori facturate per serviciu)
                sql = @"
                SELECT
                     [ID]
                    ,[IdFactura]
                    ,[IdPozitie]
                    ,[IdRezervareServiciu]
                    ,[ValoareMonedaFact]
                    ,[ValoareRON]
                    ,[ValoareMonedaServ]
                    ,[CursServiciu]
                    ,[CursFactura]
                    ,[DiferentaCurs]
                FROM [SOLON.H].[financiar].[FacturiServicii]
                WHERE
                    [IdFactura] = @IdFactura
            ";
                cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    int idRezServiciu;
                    while (reader.Read())
                    {
                        idRezServiciu = Convert.ToInt32(reader["IdRezervareServiciu"]);
                        if (!dicFS.ContainsKey(idRezServiciu))
                        {
                            // la o factura pentru un rezervareserviciu corespunde un singur facturaservicii
                            // a.i. putem folosi id rezervare serviciu ca cheie la dictionar
                            dicFS.Add(idRezServiciu, new FacturaServicii()
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                IdFactura = idFactura,
                                IdPozitie = Convert.ToInt32(reader["IdPozitie"]),
                                IdRezervareServiciu = idRezServiciu,
                                ValoareMonedaFact = Convert.ToDecimal(reader["ValoareMonedaFact"]),
                                ValoareRON = Convert.ToDecimal(reader["ValoareRON"]),
                                ValoareMonedaServ = Convert.ToDecimal(reader["ValoareMonedaServ"]),
                                CursServiciu = Convert.ToDecimal(reader["CursServiciu"]),
                                CursFactura = Convert.ToDecimal(reader["CursFactura"]),
                                DiferentaCurs = Convert.ToDecimal(reader["DiferentaCurs"])
                            });
                        }
                    }
                }

                // 5. Repartizare valori (cu metoda din EntitateServiciu.RepartizeazaValDeFacturat())
                lista = lista.OrderBy(x => x.DataStart).ThenBy(x => x.ID).ToList();
                decimal deRepartizat, deRepartizatRON, deRepartizatFact, curs, cursFact, dif, difRON, difFact;
                //bool esteRepartizat;
                FacturaServicii fs = null;
                List<EntitateServiciuValoare> vzs = null;

                foreach (EntitateServiciu es in lista)
                {
                    es.CalculTotal();
                    es.Valori = es.Valori.OrderBy(x => x.Data).ToList();

                    deRepartizat = 0;
                    deRepartizatRON = 0;
                    deRepartizatFact = 0;
                    dif = 0;
                    difRON = 0;
                    difFact = 0;

                    if (dicFS.ContainsKey(es.ID))
                    {
                        fs = dicFS[es.ID];
                        vzs = es.Valori;

                        deRepartizat = fs.ValoareMonedaServ;
                        deRepartizatRON = fs.ValoareRON;
                        deRepartizatFact = fs.ValoareMonedaFact;

                        curs = fs.CursServiciu;
                        cursFact = fs.CursFactura;

                        for (int i = 0; i < vzs.Count; i++)
                        {
                            if (vzs[i].FacturatMoneda == es.Valori[i].Valoare)
                            {
                                continue;
                            }
                            if (vzs[i].Valoare >= 0)
                            {
                                if (deRepartizat >= 0)
                                {
                                    if (vzs[i].FacturatMoneda + deRepartizat > vzs[i].Valoare)
                                    {
                                        dif = vzs[i].Valoare - vzs[i].FacturatMoneda;
                                        difRON = Math.Round(dif * curs, 2);
                                        difFact = Math.Round(dif * curs / cursFact, 2);

                                        vzs[i].FacturatMoneda += dif;
                                        vzs[i].FacturatRon += difRON;

                                        deRepartizat -= dif;
                                        deRepartizatRON -= difRON;
                                        deRepartizatFact -= difFact;

                                        if (dif != 0)
                                        {
                                            fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                            {
                                                ID = 0,
                                                IdFactura = fs.IdFactura,
                                                IdPozitie = fs.IdPozitie,
                                                IdValoareZilnica = vzs[i].ID,
                                                ValoareMonedaFact = difFact,
                                                ValoareRON = difRON,
                                                ValoareMonedaServ = dif,
                                                Anulat = false,
                                                FF_ValoareZilnica = vzs[i]
                                            });
                                        }
                                    }
                                    else
                                    {
                                        dif = deRepartizat;
                                        difRON = deRepartizatRON;
                                        difFact = deRepartizatFact;

                                        vzs[i].FacturatMoneda += dif;
                                        vzs[i].FacturatRon += difRON;

                                        deRepartizat = 0;
                                        deRepartizatRON = 0;

                                        if (dif != 0)
                                        {
                                            fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                            {
                                                ID = 0,
                                                IdFactura = fs.IdFactura,
                                                IdPozitie = fs.IdPozitie,
                                                IdValoareZilnica = vzs[i].ID,
                                                ValoareMonedaFact = difFact,
                                                ValoareRON = difRON,
                                                ValoareMonedaServ = dif,
                                                Anulat = false,
                                                FF_ValoareZilnica = vzs[i]
                                            });
                                        }
                                    }
                                } // if (deRepartizat >= 0)
                                else
                                {
                                    // suma de repartizat este negativa
                                    dif = vzs[i].Valoare - vzs[i].FacturatMoneda;
                                    difRON = Math.Round(dif * curs, 2);
                                    difFact = Math.Round(dif * curs / cursFact, 2);

                                    vzs[i].FacturatMoneda += dif;
                                    vzs[i].FacturatRon += difRON;

                                    deRepartizat -= dif;
                                    deRepartizatRON -= difRON;
                                    deRepartizatFact -= difFact;

                                    if (dif != 0)
                                    {
                                        fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                        {
                                            ID = 0,
                                            IdFactura = fs.IdFactura,
                                            IdPozitie = fs.IdPozitie,
                                            IdValoareZilnica = vzs[i].ID,
                                            ValoareMonedaFact = difFact,
                                            ValoareRON = difRON,
                                            ValoareMonedaServ = dif,
                                            Anulat = false,
                                            FF_ValoareZilnica = vzs[i]
                                        });
                                    }
                                }
                            } // if (vzs[i].Valoare >= 0)
                            else
                            {
                                // valoarea zilnica este negativa
                                if (deRepartizat < 0)
                                {
                                    // suma de repartizat este tot negativa
                                    if (vzs[i].FacturatMoneda + deRepartizat < vzs[i].Valoare)
                                    {
                                        dif = vzs[i].Valoare - vzs[i].FacturatMoneda;
                                        difRON = Math.Round(dif * curs, 2);
                                        difFact = Math.Round(dif * curs / cursFact, 2);

                                        vzs[i].FacturatMoneda += dif;
                                        vzs[i].FacturatRon += difRON;

                                        deRepartizat -= dif;
                                        deRepartizatRON -= difRON;
                                        deRepartizatFact -= difFact;

                                        if (dif != 0)
                                        {
                                            fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                            {
                                                ID = 0,
                                                IdFactura = fs.IdFactura,
                                                IdPozitie = fs.IdPozitie,
                                                IdValoareZilnica = vzs[i].ID,
                                                ValoareMonedaFact = difFact,
                                                ValoareRON = difRON,
                                                ValoareMonedaServ = dif,
                                                Anulat = false,
                                                FF_ValoareZilnica = vzs[i]
                                            });
                                        }
                                    }
                                    else
                                    {
                                        dif = deRepartizat;
                                        difRON = deRepartizatRON;
                                        difFact = deRepartizatFact;

                                        vzs[i].FacturatMoneda += dif;
                                        vzs[i].FacturatRon += difRON;

                                        deRepartizat = 0;
                                        deRepartizatRON = 0;

                                        if (dif != 0)
                                        {
                                            fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                            {
                                                ID = 0,
                                                IdFactura = fs.IdFactura,
                                                IdPozitie = fs.IdPozitie,
                                                IdValoareZilnica = vzs[i].ID,
                                                ValoareMonedaFact = difFact,
                                                ValoareRON = difRON,
                                                ValoareMonedaServ = dif,
                                                Anulat = false,
                                                FF_ValoareZilnica = vzs[i]
                                            });
                                        }
                                    }


                                } // if (deRepartizat < 0)
                                else if (deRepartizat > 0)
                                {
                                    // suma de repartizat este pozitiva
                                    dif = vzs[i].Valoare - vzs[i].FacturatMoneda;
                                    difRON = Math.Round(dif * curs, 2);
                                    difFact = Math.Round(dif * curs / cursFact, 2);

                                    vzs[i].FacturatMoneda += dif;
                                    vzs[i].FacturatRon += difRON;

                                    deRepartizat -= dif;
                                    deRepartizatRON -= difRON;
                                    deRepartizatFact -= difFact;

                                    if (dif != 0)
                                    {
                                        fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                        {
                                            ID = 0,
                                            IdFactura = fs.IdFactura,
                                            IdPozitie = fs.IdPozitie,
                                            IdValoareZilnica = vzs[i].ID,
                                            ValoareMonedaFact = difFact,
                                            ValoareRON = difRON,
                                            ValoareMonedaServ = dif,
                                            Anulat = false,
                                            FF_ValoareZilnica = vzs[i]
                                        });
                                    }
                                } // else if (deRepartizat > 0)
                            }
                            if (deRepartizat == 0)
                            {
                                break;
                            }
                        } // for (int i = 0; i < vzs.Count; i++)
                        if (deRepartizat != 0 && vzs.Count > 0)
                        {
                            FacturaValoriZilnice fvzUltima = null;
                            for (int i = vzs.Count - 1; i >= 0; i--)
                            {
                                if (fs.DicFacturiValoriZilnice.ContainsKey(vzs[i].ID))
                                {
                                    if (fs.DicFacturiValoriZilnice[vzs[i].ID].ID == 0)
                                    {
                                        fvzUltima = fs.DicFacturiValoriZilnice[vzs[i].ID];
                                        break;
                                    }
                                }
                            }
                            if (fvzUltima != null)
                            {
                                fvzUltima.ValoareRON += deRepartizatRON;
                                fvzUltima.ValoareMonedaServ += deRepartizat;
                                fvzUltima.ValoareMonedaFact += deRepartizatFact;
                            }
                        }


                    } // if (dicFS.ContainsKey(es.ID))
                    else
                    {
                        continue;
                    }
                } // foreach (FormFisa.EntitateServiciu es in lista)

                // 5. Salvare valori facturate
                sql = @"
                INSERT INTO [SOLON.H].[financiar].[FacturiValoriZilnice]
                    ([IdFactura]
                    ,[IdPozitie]
                    ,[IdValoareZilnica]
                    ,[ValoareMoneda]
                    ,[ValoareRON]
                    ,[ValoareMonedaServ]
                    ,[Anulat])
                VALUES
                    (@IdFactura
                    ,@IdPozitie
                    ,@IdValoareZilnica
                    ,@ValoareMoneda
                    ,@ValoareRON
                    ,@ValoareMonedaServ
                    ,@Anulat);
                SELECT SCOPE_IDENTITY();
            ";
                cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                cmd.Parameters.Add(new SqlParameter("@IdPozitie", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@IdValoareZilnica", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@ValoareMoneda", SqlDbType.Decimal));
                cmd.Parameters.Add(new SqlParameter("@ValoareRON", SqlDbType.Decimal));
                cmd.Parameters.Add(new SqlParameter("@ValoareMonedaServ", SqlDbType.Decimal));
                cmd.Parameters.Add(new SqlParameter("@Anulat", SqlDbType.Bit)).Value = false;

                foreach (EntitateServiciu es in lista)
                {
                    if (dicFS.ContainsKey(es.ID))
                    {
                        fs = dicFS[es.ID];
                        fs.ListaFacturiValoriZilnice = fs.DicFacturiValoriZilnice.Values.OrderBy(x => x.FF_ValoareZilnica.Data).ToList();
                        foreach (FacturaValoriZilnice fvz in fs.ListaFacturiValoriZilnice)
                        {
                            cmd.Parameters["@IdPozitie"].Value = fvz.IdPozitie;
                            cmd.Parameters["@IdValoareZilnica"].Value = fvz.IdValoareZilnica;
                            cmd.Parameters["@ValoareMoneda"].Value = fvz.ValoareMonedaFact;
                            cmd.Parameters["@ValoareRON"].Value = fvz.ValoareRON;
                            cmd.Parameters["@ValoareMonedaServ"].Value = fvz.ValoareMonedaServ;

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    fvz.ID = Convert.ToInt32(reader[0]);
                                }
                            }
                        }
                    }
                }

                //            // 6. Marcare factura ca repusa
                //            sql = @"
                //                UPDATE [financiar].[Facturi]
                //                SET
                //                    [Anulata] = @Anulata
                //                WHERE
                //                    [ID] = @ID
                //            ";
                //            cmd = new SqlCommand(sql, cnn, tran);
                //            cmd.Parameters.Add(new SqlParameter("@Anulata", SqlDbType.Bit)).Value = false;
                //            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = idFactura;
                //            cmd.ExecuteNonQuery();

                // 7. Marcare avansuri inapoi daca factura este una de avans
                List<int> idsAvansuri = new List<int>();
                sql = @"
                SELECT 
                    DISTINCT(rs.IdAvansRezervare) AS IdAvansRezervare
                FROM 
                    [SOLON.H].financiar.Facturi AS f INNER JOIN
                    [SOLON.H].financiar.FacturiServicii AS fs ON fs.IdFactura = f.ID INNER JOIN
                    [SOLON.H].hotel.RezervariServicii AS rs ON rs.ID = fs.IdRezervareServiciu 
                WHERE
                    f.ID = @IdFactura
            ";

                cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@IdFactura", idFactura)).SqlDbType = SqlDbType.Int;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        idsAvansuri.Add(Convert.ToInt32(dr["IdAvansRezervare"]));
                    }
                }

                if (idsAvansuri.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < idsAvansuri.Count; i++)
                    {
                        if (i > 0)
                        {
                            sb.Append(',');
                        }
                        sb.Append(idsAvansuri[i].ToString());
                    }

                    sql = @"UPDATE [SOLON.H].hotel.RezervariAvans SET IdFactura = @IdFactura WHERE ID IN (<lista>)";
                    sql = sql.Replace("<lista>", sb.ToString());
                    cmd = new SqlCommand(sql, cnn, tran);
                    cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                    cmd.ExecuteNonQuery();
                }
            }

            public static void RepuneFactura(int idFactura)
            {
                List<EntitateServiciu> lista = new List<EntitateServiciu>();
                Dictionary<int, EntitateServiciu> dic = new Dictionary<int, EntitateServiciu>();  // cheia este id serviciu
                Dictionary<int, FacturaServicii> dicFS = new Dictionary<int, FacturaServicii>();                    // cheia este id serviciu
                bool EsteAvans = false;

                using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                {
                    try
                    {
                        cnn.Open();
                        using (SqlTransaction tran = cnn.BeginTransaction())
                        {
                            try
                            {
                                string sql = "";
                                SqlCommand cmd = null;

                                // 2. Stergere repartizare valori pe factura
                                sql = @"
                                DELETE FROM [SOLON.H].[financiar].[FacturiValoriZilnice]
                                WHERE
                                    [IdFactura] = @IdFactura
                            ";
                                cmd = new SqlCommand(sql, cnn, tran);
                                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                                cmd.ExecuteNonQuery();

                                // 3. Incarcare servicii cu valorile lor si cat a fost deja facturat din ele
                                sql = @"
                                SELECT
                                     s.[ID]
                                    ,s.[IdPartener]
                                    ,s.[EsteVirament]
                                    ,s.[IdRezervareCamera]
                                    ,s.[IdVenit]
                                    ,s.[IdTarif]
                                    ,s.[IdPlanMasa]
                                    ,s.[DenumireServiciu]
                                    ,s.[IdMoneda]
                                    ,s.[IdCotaTVA]
                                    ,s.[TaxaProcentuala]
                                    ,s.[TaxaLaPersoane]
                                    ,s.[ValoareTaxa]
                                    ,s.[Observatii]
                                    ,s.[Sters]
                                    ,s.[IdMotivStergere]
                                    ,s.[Cantitate]
                                    ,s.[PostareAmanata]
                                    ,s.[IdAvansRezervare]
                                    ,s.[ValoareMoneda]
                                    ,s.[Curs]
                                    ,s.[ValoareRon]
                                    ,m.[MonedaCod]
                                    ,c.[Procent]
                                    ,c.[Denumire] AS CodCotaTVA
                                    ,p.[NumePartener]
                                    ,v.[EsteTaxa]
                                    ,v.[EsteCazare]
                                    ,v.[EsteMasa]
                                    ,v.[IdGrupaFact]
                                    ,v.[IdGrupaFact2]
                                    ,n1.[Denumire] AS Grupa1
                                    ,n2.[Denumire] AS Grupa2
                                    ,cam.[Denumire] AS Camera
                                    ,ra.IdRezervareCamera RezCamAvans
                                    ,s.[ObsFactura] AS ObsFactura
                                    ,s.[EsteStorno] AS EsteStorno
                                FROM 
                                    [SOLON.H].[hotel].[RezervariServicii] AS s INNER JOIN
                                    [SOLON.H].[hotel].[Venituri] AS v ON v.[ID] = s.[IdVenit] INNER JOIN
                                    [SOLON].[dbo].[NomMonede] AS m ON m.[IdMoneda] = s.[IdMoneda] INNER JOIN
                                    [SOLON].[dbo].[CoteTva] AS c ON c.[IdCota] = s.[IdCotaTVA] LEFT OUTER JOIN
                                    [SOLON].[dbo].[NomParteneri] AS p ON p.[IdPartener] = s.[IdPartener] LEFT OUTER JOIN
                                    [SOLON.H].[setari].[Nomenclatoare] AS n1 ON n1.ID = v.[IdGrupaFact] LEFT OUTER JOIN
                                    [SOLON.H].[setari].[Nomenclatoare] AS n2 ON n2.ID = v.[IdGrupaFact2] LEFT OUTER JOIN
                                    [SOLON.H]. [hotel].[RezervariCamere] AS rc ON rc.id = s.IdRezervareCamera LEFT OUTER JOIN
                                    [SOLON.H].[hotel].[Camere] AS cam ON cam.ID = rc.IdCamera LEFT OUTER JOIN
                                    [SOLON.H].[hotel].[RezervariAvans] AS ra ON s.IdAvansRezervare = ra.ID
                                WHERE 
                                    s.[ID] IN (SELECT [IdRezervareServiciu] FROM [SOLON.H].[financiar].[FacturiServicii] WHERE [IdFactura] = @IdFactura)
                            ";
                                cmd = new SqlCommand(sql, cnn, tran);
                                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    EntitateServiciu serv;
                                    while (reader.Read())
                                    {
                                        int IdRezCam = reader["IdRezervareCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdRezervareCamera"]);
                                        if (EsteAvans)
                                        {
                                            IdRezCam = reader["RezCamAvans"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RezCamAvans"]);
                                        }

                                        if (IdRezCam == 0)
                                        {
                                            IdRezCam = reader["IdRezervareCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdRezervareCamera"]);
                                        }

                                        serv = new EntitateServiciu()
                                        {
                                            ID = Convert.ToInt32(reader["ID"]),
                                            EsteVirament = Convert.ToBoolean(reader["EsteVirament"]),
                                            IdPartener = reader["IdPartener"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdPartener"]),
                                            IdRezervareCamera = IdRezCam,
                                            IdVenit = Convert.ToInt32(reader["IdVenit"]),
                                            IdTarif = reader["IdTarif"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdTarif"]),
                                            IdPlanMasa = reader["IdPlanMasa"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdPlanMasa"]),
                                            DenumireServiciu = reader["DenumireServiciu"].ToString(),
                                            IdMoneda = Convert.ToInt32(reader["IdMoneda"]),
                                            IdCotaTVA = Convert.ToInt32(reader["IdCotaTVA"]),
                                            TaxaProcentuala = Convert.ToBoolean(reader["TaxaProcentuala"]),
                                            TaxaLaPersoane = Convert.ToBoolean(reader["TaxaLaPersoane"]),
                                            ValoareTaxa = Convert.ToDecimal(reader["ValoareTaxa"]),
                                            Observatii = reader["Observatii"] == DBNull.Value ? "" : reader["Observatii"].ToString(),
                                            Sters = Convert.ToBoolean(reader["Sters"]),
                                            IdMotivStergere = reader["IdMotivStergere"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdMotivStergere"]),
                                            CodMoneda = reader["MonedaCod"].ToString(),
                                            ProcentTVA = Convert.ToDecimal(reader["Procent"]),
                                            CodCotaTVA = reader["CodCotaTVA"].ToString(),
                                            NumePartener = reader["NumePartener"] == DBNull.Value ? "" : reader["NumePartener"].ToString(),
                                            Cantitate = Convert.ToInt32(reader["Cantitate"]),
                                            EsteTaxa = Convert.ToBoolean(reader["EsteTaxa"]),
                                            EsteCazare = Convert.ToBoolean(reader["EsteCazare"]),
                                            EsteMasa = Convert.ToBoolean(reader["EsteMasa"]),
                                            PostareAmanata = Convert.ToBoolean(reader["PostareAmanata"]),
                                            IdGrupaFact1 = reader["IdGrupaFact"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdGrupaFact"]),
                                            IdGrupaFact2 = reader["IdGrupaFact2"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdGrupaFact2"]),
                                            GrupaFact1 = reader["Grupa1"] == DBNull.Value ? "" : reader["Grupa1"].ToString(),
                                            GrupaFact2 = reader["Grupa2"] == DBNull.Value ? "" : reader["Grupa2"].ToString(),
                                            Camera = reader["Camera"] == DBNull.Value ? "" : reader["Camera"].ToString(),
                                            ValoareMoneda = reader["ValoareMoneda"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareMoneda"]),
                                            Curs = reader["Curs"] == DBNull.Value ? 1 : Convert.ToDecimal(reader["Curs"]),
                                            ValoareRon = reader["ValoareRon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareRon"]),
                                            ObsFactura = reader["ObsFactura"] == DBNull.Value ? "" : reader["ObsFactura"].ToString(),
                                            IdAvansRezervare = reader["IdAvansRezervare"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdAvansRezervare"]),
                                            EsteStorno = reader["EsteStorno"] == DBNull.Value ? false : Convert.ToBoolean(reader["EsteStorno"])
                                        };
                                        lista.Add(serv);
                                        dic.Add(serv.ID, serv);
                                    }
                                }
                                Dictionary<int, int> valZilnice = new Dictionary<int, int>();
                                sql = @"
                                SELECT
                                     v.[ID]
                                    ,v.[IdRezervareServiciu]
                                    ,v.[Data]
                                    ,v.[Valoare]
                                    ,v.[Postat]
                                    ,v.[ValoareRON]
                                FROM
                                    [SOLON.H].[hotel].[RezervariServicii] AS s INNER JOIN 
                                    [SOLON.H].[hotel].[RezervariServiciiValori] AS v ON v.[IdRezervareServiciu] = s.[ID]
                                WHERE s.[ID] IN (SELECT [IdRezervareServiciu] FROM [financiar].[FacturiServicii] WHERE [IdFactura] = @IdFactura)
                            ";
                                cmd = new SqlCommand(sql, cnn, tran);
                                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    EntitateServiciu serv;
                                    int idServ;
                                    while (reader.Read())
                                    {
                                        idServ = Convert.ToInt32(reader["IdRezervareServiciu"]);
                                        serv = dic[idServ];
                                        int IdValZilnica = reader["ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID"]);

                                        if (!valZilnice.ContainsKey(IdValZilnica))
                                        {
                                            valZilnice.Add(IdValZilnica, idServ);
                                        }
                                        serv.Valori.Add(new EntitateServiciuValoare()
                                        {
                                            ID = IdValZilnica,
                                            IdRezervareServiciu = idServ,
                                            Data = Convert.ToDateTime(reader["Data"]),
                                            Valoare = Convert.ToDecimal(reader["Valoare"]),
                                            Postat = Convert.ToBoolean(reader["Postat"]),
                                            ValoareRON = Convert.ToDecimal(reader["ValoareRON"])
                                        });
                                    }
                                }
                                // factura curenta care se repartizeaza nu da rezultate la query-ul urmator
                                // deoarece FacturiValoriZilnice-le daca existau au fost deja sterse
                                sql = @"
                                SELECT 
                                     fsvz.[ID]
                                    ,fsvz.[IdValoareZilnica]
                                    ,[ValoareRON]
                                    ,[ValoareMonedaServ]
                                FROM 
                                    [SOLON.H].[financiar].[FacturiValoriZilnice] AS fsvz LEFT OUTER JOIN
                                    [SOLON.H].[financiar].[Facturi] AS f ON f.ID = fsvz.IdFactura LEFT OUTER JOIN
                                    [SOLON.H].[financiar].[Facturi] AS fstorno ON fstorno.StornoLaFactura = f.ID
                                WHERE 
                                    fsvz.[IdValoareZilnica] IN (<valori_zilnice>) AND 
                                    f.[Anulata] = 0 AND 
                                    f.[StornoLaFactura] = 0 AND 
                                    (fstorno.ID IS NULL OR (fstorno.ID IS NOT NULL AND fstorno.Anulata = 1))
                            ";
                                sql = sql.Replace("<valori_zilnice>", valZilnice.Keys.Aggregate("-1", (x, y) => x + "," + y));
                                HashSet<int> idurivals = new HashSet<int>();
                                cmd = new SqlCommand(sql, cnn, tran);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    EntitateServiciu serv;
                                    int idServ;
                                    while (reader.Read())
                                    {
                                        int ID = reader["ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID"]);
                                        if (!idurivals.Contains(ID))
                                        {
                                            idurivals.Add(ID);

                                            int IdValZilnica = reader["IdValoareZilnica"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdValoareZilnica"]);

                                            if (valZilnice.ContainsKey(IdValZilnica))
                                            {
                                                idServ = valZilnice[IdValZilnica];

                                                serv = dic[idServ];

                                                EntitateServiciuValoare val = serv.Valori.FirstOrDefault(x => x.ID == IdValZilnica);
                                                if (val != null)
                                                {
                                                    // valorile facturate deja pe fiecare valoare zilnica
                                                    val.FacturatMoneda += reader["ValoareMonedaServ"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareMonedaServ"]);
                                                    val.FacturatRon += reader["ValoareRON"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareRON"]);
                                                }
                                            }
                                        }
                                    }
                                }

                                // 4. Incarcare factura (valori facturate per serviciu)
                                sql = @"
                                SELECT
                                     [ID]
                                    ,[IdFactura]
                                    ,[IdPozitie]
                                    ,[IdRezervareServiciu]
                                    ,[ValoareMonedaFact]
                                    ,[ValoareRON]
                                    ,[ValoareMonedaServ]
                                    ,[CursServiciu]
                                    ,[CursFactura]
                                    ,[DiferentaCurs]
                                FROM [SOLON.H].[financiar].[FacturiServicii]
                                WHERE
                                    [IdFactura] = @IdFactura
                            ";
                                cmd = new SqlCommand(sql, cnn, tran);
                                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    int idRezServiciu;
                                    while (reader.Read())
                                    {
                                        idRezServiciu = Convert.ToInt32(reader["IdRezervareServiciu"]);
                                        if (!dicFS.ContainsKey(idRezServiciu))
                                        {
                                            // la o factura pentru un rezervareserviciu corespunde un singur facturaservicii
                                            // a.i. putem folosi id rezervare serviciu ca cheie la dictionar
                                            dicFS.Add(idRezServiciu, new FacturaServicii()
                                            {
                                                ID = Convert.ToInt32(reader["ID"]),
                                                IdFactura = idFactura,
                                                IdPozitie = Convert.ToInt32(reader["IdPozitie"]),
                                                IdRezervareServiciu = idRezServiciu,
                                                ValoareMonedaFact = Convert.ToDecimal(reader["ValoareMonedaFact"]),
                                                ValoareRON = Convert.ToDecimal(reader["ValoareRON"]),
                                                ValoareMonedaServ = Convert.ToDecimal(reader["ValoareMonedaServ"]),
                                                CursServiciu = Convert.ToDecimal(reader["CursServiciu"]),
                                                CursFactura = Convert.ToDecimal(reader["CursFactura"]),
                                                DiferentaCurs = Convert.ToDecimal(reader["DiferentaCurs"])
                                            });
                                        }
                                    }
                                }

                                // 5. Repartizare valori (cu metoda din EntitateServiciu.RepartizeazaValDeFacturat())
                                lista = lista.OrderBy(x => x.DataStart).ThenBy(x => x.ID).ToList();
                                decimal deRepartizat, deRepartizatRON, deRepartizatFact, curs, cursFact, dif, difRON, difFact;
                                //bool esteRepartizat;
                                FacturaServicii fs = null;
                                List<EntitateServiciuValoare> vzs = null;

                                foreach (EntitateServiciu es in lista)
                                {
                                    es.CalculTotal();
                                    es.Valori = es.Valori.OrderBy(x => x.Data).ToList();

                                    deRepartizat = 0;
                                    deRepartizatRON = 0;
                                    deRepartizatFact = 0;
                                    dif = 0;
                                    difRON = 0;
                                    difFact = 0;

                                    if (dicFS.ContainsKey(es.ID))
                                    {
                                        fs = dicFS[es.ID];
                                        vzs = es.Valori;

                                        deRepartizat = fs.ValoareMonedaServ;
                                        deRepartizatRON = fs.ValoareRON;
                                        deRepartizatFact = fs.ValoareMonedaFact;

                                        curs = fs.CursServiciu;
                                        cursFact = fs.CursFactura;

                                        for (int i = 0; i < vzs.Count; i++)
                                        {
                                            if (vzs[i].FacturatMoneda == es.Valori[i].Valoare)
                                            {
                                                continue;
                                            }
                                            if (vzs[i].Valoare >= 0)
                                            {
                                                if (deRepartizat >= 0)
                                                {
                                                    if (vzs[i].FacturatMoneda + deRepartizat > vzs[i].Valoare)
                                                    {
                                                        dif = vzs[i].Valoare - vzs[i].FacturatMoneda;
                                                        difRON = Math.Round(dif * curs, 2);
                                                        difFact = Math.Round(dif * curs / cursFact, 2);

                                                        vzs[i].FacturatMoneda += dif;
                                                        vzs[i].FacturatRon += difRON;

                                                        deRepartizat -= dif;
                                                        deRepartizatRON -= difRON;
                                                        deRepartizatFact -= difFact;

                                                        if (dif != 0)
                                                        {
                                                            fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                                            {
                                                                ID = 0,
                                                                IdFactura = fs.IdFactura,
                                                                IdPozitie = fs.IdPozitie,
                                                                IdValoareZilnica = vzs[i].ID,
                                                                ValoareMonedaFact = difFact,
                                                                ValoareRON = difRON,
                                                                ValoareMonedaServ = dif,
                                                                Anulat = false,
                                                                FF_ValoareZilnica = vzs[i]
                                                            });
                                                        }
                                                    }
                                                    else
                                                    {
                                                        dif = deRepartizat;
                                                        difRON = deRepartizatRON;
                                                        difFact = deRepartizatFact;

                                                        vzs[i].FacturatMoneda += dif;
                                                        vzs[i].FacturatRon += difRON;

                                                        deRepartizat = 0;
                                                        deRepartizatRON = 0;

                                                        if (dif != 0)
                                                        {
                                                            fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                                            {
                                                                ID = 0,
                                                                IdFactura = fs.IdFactura,
                                                                IdPozitie = fs.IdPozitie,
                                                                IdValoareZilnica = vzs[i].ID,
                                                                ValoareMonedaFact = difFact,
                                                                ValoareRON = difRON,
                                                                ValoareMonedaServ = dif,
                                                                Anulat = false,
                                                                FF_ValoareZilnica = vzs[i]
                                                            });
                                                        }
                                                    }
                                                } // if (deRepartizat >= 0)
                                                else
                                                {
                                                    // suma de repartizat este negativa
                                                    dif = vzs[i].Valoare - vzs[i].FacturatMoneda;
                                                    difRON = Math.Round(dif * curs, 2);
                                                    difFact = Math.Round(dif * curs / cursFact, 2);

                                                    vzs[i].FacturatMoneda += dif;
                                                    vzs[i].FacturatRon += difRON;

                                                    deRepartizat -= dif;
                                                    deRepartizatRON -= difRON;
                                                    deRepartizatFact -= difFact;

                                                    if (dif != 0)
                                                    {
                                                        fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                                        {
                                                            ID = 0,
                                                            IdFactura = fs.IdFactura,
                                                            IdPozitie = fs.IdPozitie,
                                                            IdValoareZilnica = vzs[i].ID,
                                                            ValoareMonedaFact = difFact,
                                                            ValoareRON = difRON,
                                                            ValoareMonedaServ = dif,
                                                            Anulat = false,
                                                            FF_ValoareZilnica = vzs[i]
                                                        });
                                                    }
                                                }
                                            } // if (vzs[i].Valoare >= 0)
                                            else
                                            {
                                                // valoarea zilnica este negativa
                                                if (deRepartizat < 0)
                                                {
                                                    // suma de repartizat este tot negativa
                                                    if (vzs[i].FacturatMoneda + deRepartizat < vzs[i].Valoare)
                                                    {
                                                        dif = vzs[i].Valoare - vzs[i].FacturatMoneda;
                                                        difRON = Math.Round(dif * curs, 2);
                                                        difFact = Math.Round(dif * curs / cursFact, 2);

                                                        vzs[i].FacturatMoneda += dif;
                                                        vzs[i].FacturatRon += difRON;

                                                        deRepartizat -= dif;
                                                        deRepartizatRON -= difRON;
                                                        deRepartizatFact -= difFact;

                                                        if (dif != 0)
                                                        {
                                                            fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                                            {
                                                                ID = 0,
                                                                IdFactura = fs.IdFactura,
                                                                IdPozitie = fs.IdPozitie,
                                                                IdValoareZilnica = vzs[i].ID,
                                                                ValoareMonedaFact = difFact,
                                                                ValoareRON = difRON,
                                                                ValoareMonedaServ = dif,
                                                                Anulat = false,
                                                                FF_ValoareZilnica = vzs[i]
                                                            });
                                                        }
                                                    }
                                                    else
                                                    {
                                                        dif = deRepartizat;
                                                        difRON = deRepartizatRON;
                                                        difFact = deRepartizatFact;

                                                        vzs[i].FacturatMoneda += dif;
                                                        vzs[i].FacturatRon += difRON;

                                                        deRepartizat = 0;
                                                        deRepartizatRON = 0;

                                                        if (dif != 0)
                                                        {
                                                            fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                                            {
                                                                ID = 0,
                                                                IdFactura = fs.IdFactura,
                                                                IdPozitie = fs.IdPozitie,
                                                                IdValoareZilnica = vzs[i].ID,
                                                                ValoareMonedaFact = difFact,
                                                                ValoareRON = difRON,
                                                                ValoareMonedaServ = dif,
                                                                Anulat = false,
                                                                FF_ValoareZilnica = vzs[i]
                                                            });
                                                        }
                                                    }


                                                } // if (deRepartizat < 0)
                                                else if (deRepartizat > 0)
                                                {
                                                    // suma de repartizat este pozitiva
                                                    dif = vzs[i].Valoare - vzs[i].FacturatMoneda;
                                                    difRON = Math.Round(dif * curs, 2);
                                                    difFact = Math.Round(dif * curs / cursFact, 2);

                                                    vzs[i].FacturatMoneda += dif;
                                                    vzs[i].FacturatRon += difRON;

                                                    deRepartizat -= dif;
                                                    deRepartizatRON -= difRON;
                                                    deRepartizatFact -= difFact;

                                                    if (dif != 0)
                                                    {
                                                        fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                                        {
                                                            ID = 0,
                                                            IdFactura = fs.IdFactura,
                                                            IdPozitie = fs.IdPozitie,
                                                            IdValoareZilnica = vzs[i].ID,
                                                            ValoareMonedaFact = difFact,
                                                            ValoareRON = difRON,
                                                            ValoareMonedaServ = dif,
                                                            Anulat = false,
                                                            FF_ValoareZilnica = vzs[i]
                                                        });
                                                    }
                                                } // else if (deRepartizat > 0)
                                            }
                                            if (deRepartizat == 0)
                                            {
                                                break;
                                            }
                                        } // for (int i = 0; i < vzs.Count; i++)
                                        if (deRepartizat != 0 && vzs.Count > 0)
                                        {
                                            FacturaValoriZilnice fvzUltima = null;
                                            for (int i = vzs.Count - 1; i >= 0; i--)
                                            {
                                                if (fs.DicFacturiValoriZilnice.ContainsKey(vzs[i].ID))
                                                {
                                                    if (fs.DicFacturiValoriZilnice[vzs[i].ID].ID == 0)
                                                    {
                                                        fvzUltima = fs.DicFacturiValoriZilnice[vzs[i].ID];
                                                        break;
                                                    }
                                                }
                                            }
                                            if (fvzUltima != null)
                                            {
                                                fvzUltima.ValoareRON += deRepartizatRON;
                                                fvzUltima.ValoareMonedaServ += deRepartizat;
                                                fvzUltima.ValoareMonedaFact += deRepartizatFact;
                                            }
                                        }


                                    } // if (dicFS.ContainsKey(es.ID))
                                    else
                                    {
                                        continue;
                                    }
                                } // foreach (FormFisa.EntitateServiciu es in lista)

                                // 5. Salvare valori facturate
                                sql = @"
                                INSERT INTO [SOLON.H].[financiar].[FacturiValoriZilnice]
                                    ([IdFactura]
                                    ,[IdPozitie]
                                    ,[IdValoareZilnica]
                                    ,[ValoareMoneda]
                                    ,[ValoareRON]
                                    ,[ValoareMonedaServ]
                                    ,[Anulat])
                                VALUES
                                    (@IdFactura
                                    ,@IdPozitie
                                    ,@IdValoareZilnica
                                    ,@ValoareMoneda
                                    ,@ValoareRON
                                    ,@ValoareMonedaServ
                                    ,@Anulat);
                                SELECT SCOPE_IDENTITY();
                            ";
                                cmd = new SqlCommand(sql, cnn, tran);
                                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                                cmd.Parameters.Add(new SqlParameter("@IdPozitie", SqlDbType.Int));
                                cmd.Parameters.Add(new SqlParameter("@IdValoareZilnica", SqlDbType.Int));
                                cmd.Parameters.Add(new SqlParameter("@ValoareMoneda", SqlDbType.Decimal));
                                cmd.Parameters.Add(new SqlParameter("@ValoareRON", SqlDbType.Decimal));
                                cmd.Parameters.Add(new SqlParameter("@ValoareMonedaServ", SqlDbType.Decimal));
                                cmd.Parameters.Add(new SqlParameter("@Anulat", SqlDbType.Bit)).Value = false;

                                foreach (EntitateServiciu es in lista)
                                {
                                    if (dicFS.ContainsKey(es.ID))
                                    {
                                        fs = dicFS[es.ID];
                                        fs.ListaFacturiValoriZilnice = fs.DicFacturiValoriZilnice.Values.OrderBy(x => x.FF_ValoareZilnica.Data).ToList();
                                        foreach (FacturaValoriZilnice fvz in fs.ListaFacturiValoriZilnice)
                                        {
                                            cmd.Parameters["@IdPozitie"].Value = fvz.IdPozitie;
                                            cmd.Parameters["@IdValoareZilnica"].Value = fvz.IdValoareZilnica;
                                            cmd.Parameters["@ValoareMoneda"].Value = fvz.ValoareMonedaFact;
                                            cmd.Parameters["@ValoareRON"].Value = fvz.ValoareRON;
                                            cmd.Parameters["@ValoareMonedaServ"].Value = fvz.ValoareMonedaServ;

                                            using (SqlDataReader reader = cmd.ExecuteReader())
                                            {
                                                if (reader.Read())
                                                {
                                                    fvz.ID = Convert.ToInt32(reader[0]);
                                                }
                                            }
                                        }
                                    }
                                }

                                // 6. Marcare factura ca repusa
                                sql = @"
                                UPDATE [SOLON.H].[financiar].[Facturi]
                                SET
                                    [Anulata] = @Anulata
                                WHERE
                                    [ID] = @ID
                            ";
                                cmd = new SqlCommand(sql, cnn, tran);
                                cmd.Parameters.Add(new SqlParameter("@Anulata", SqlDbType.Bit)).Value = false;
                                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = idFactura;
                                cmd.ExecuteNonQuery();

                                // 7. Marcare avansuri inapoi daca factura este una de avans
                                List<int> idsAvansuri = new List<int>();
                                sql = @"
                                SELECT 
                                    DISTINCT(rs.IdAvansRezervare) AS IdAvansRezervare
                                FROM 
                                    [SOLON.H].financiar.Facturi AS f INNER JOIN
                                    [SOLON.H].financiar.FacturiServicii AS fs ON fs.IdFactura = f.ID INNER JOIN
                                    [SOLON.H].hotel.RezervariServicii AS rs ON rs.ID = fs.IdRezervareServiciu 
                                WHERE
                                    f.ID = @IdFactura
                            ";

                                cmd = new SqlCommand(sql, cnn, tran);
                                cmd.Parameters.Add(new SqlParameter("@IdFactura", idFactura)).SqlDbType = SqlDbType.Int;
                                using (SqlDataReader dr = cmd.ExecuteReader())
                                {
                                    while (dr.Read())
                                    {
                                        idsAvansuri.Add(Convert.ToInt32(dr["IdAvansRezervare"]));
                                    }
                                }

                                if (idsAvansuri.Count > 0)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    for (int i = 0; i < idsAvansuri.Count; i++)
                                    {
                                        if (i > 0)
                                        {
                                            sb.Append(',');
                                        }
                                        sb.Append(idsAvansuri[i].ToString());
                                    }

                                    sql = @"UPDATE [SOLON.H].hotel.RezervariAvans SET IdFactura = @IdFactura WHERE ID IN (<lista>)";
                                    sql = sql.Replace("<lista>", sb.ToString());
                                    cmd = new SqlCommand(sql, cnn, tran);
                                    cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                                    cmd.ExecuteNonQuery();
                                }

                                tran.Commit();
                            } // try
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
                        //LogErori.Salveaza(exc, "HelperFactura.RepuneFactura() - " + idFactura.ToString(), null);
                    }
                }
            }

            public static void StorneazaFactura(int idFactura, int idFacturaStorno, SqlConnection cnn, SqlTransaction tran)
            {
                string sql = "";
                SqlCommand cmd = null;

                List<int> idsPozitii = new List<int>();
                List<int> idsPozitiiStorno = new List<int>();

                sql = @"
                SELECT
                    ID
                FROM
                    [SOLON.H].[financiar].[FacturiPozitii]
                WHERE
                    [IdFactura] = @IdFactura
                ORDER BY
                    [Ordine] ASC, [ID]
            ";
                cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int));

                cmd.Parameters["@IdFactura"].Value = idFactura;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        idsPozitii.Add(Convert.ToInt32(reader[0]));
                    }
                }

                cmd.Parameters["@IdFactura"].Value = idFacturaStorno;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        idsPozitiiStorno.Add(Convert.ToInt32(reader[0]));
                    }
                }

                if (idsPozitii.Count == idsPozitiiStorno.Count)
                {
                    sql = @"
                    INSERT INTO [SOLON.H].[financiar].[FacturiValoriZilnice]
                        ([IdFactura]
                        ,[IdPozitie]
                        ,[IdValoareZilnica]
                        ,[ValoareMoneda]
                        ,[ValoareRON]
                        ,[ValoareMonedaServ]
                        ,[Anulat])
                    SELECT
                         @IdFacturaStorno
                        ,@IdPozitieStorno
                        ,[IdValoareZilnica]
                        ,-[ValoareMoneda]
                        ,-[ValoareRON]
                        ,-[ValoareMonedaServ]
                        ,[Anulat]
                    FROM 
                        [SOLON.H].[financiar].[FacturiValoriZilnice]
                    WHERE
                        [IdFactura] = @IdFactura AND
                        [IdPozitie] = @IdPozitie
                ";
                    cmd = new SqlCommand(sql, cnn, tran);
                    cmd.Parameters.Add(new SqlParameter("@IdFacturaStorno", SqlDbType.Int)).Value = idFacturaStorno;
                    cmd.Parameters.Add(new SqlParameter("@IdPozitieStorno", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                    cmd.Parameters.Add(new SqlParameter("@IdPozitie", SqlDbType.Int));

                    for (int i = 0; i < idsPozitii.Count; i++)
                    {
                        cmd.Parameters["@IdPozitieStorno"].Value = idsPozitiiStorno[i];
                        cmd.Parameters["@IdPozitie"].Value = idsPozitii[i];
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            public static void GetFacturi(int idRezervareServiciu, SqlConnection cnn, SqlTransaction tran, out Dictionary<int, FacturaServicii> dicFacturi, out Dictionary<int, ValoareZilnica> dicValoriZilnice)
            {
                string sql = "";
                SqlCommand cmd = null;

                dicFacturi = new Dictionary<int, FacturaServicii>();
                dicValoriZilnice = new Dictionary<int, ValoareZilnica>();
                FacturaServicii fs;

                sql = @"
                SELECT
                     fs.[ID]
                    ,fs.[IdFactura]
                    ,fs.[IdPozitie]
                    ,fs.[IdRezervareServiciu]
                    ,fs.[ValoareMonedaFact]
                    ,fs.[ValoareRON]
                    ,fs.[ValoareMonedaServ]
                FROM 
	                [SOLON.H].[financiar].[FacturiServicii] AS fs INNER JOIN
	                [SOLON.H].[financiar].[Facturi] AS f ON f.[ID] = fs.[IdFactura]
                WHERE
	                fs.[IdRezervareServiciu] = @IdRezervareServiciu AND
	                f.[Anulata] = 0 AND
	                f.[Emisa] = 1
            ";
                cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@IdRezervareServiciu", SqlDbType.Int)).Value = idRezervareServiciu;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fs = new FacturaServicii();
                        fs.ID = Convert.ToInt32(reader["ID"]);
                        fs.IdFactura = Convert.ToInt32(reader["IdFactura"]);
                        fs.IdPozitie = Convert.ToInt32(reader["IdPozitie"]);
                        fs.IdRezervareServiciu = Convert.ToInt32(reader["IdRezervareServiciu"]);
                        fs.ValoareMonedaFact = Convert.ToDecimal(reader["ValoareMonedaFact"]);
                        fs.ValoareRON = Convert.ToDecimal(reader["ValoareRON"]);
                        fs.ValoareMonedaServ = Convert.ToDecimal(reader["ValoareMonedaServ"]);
                        if (!dicFacturi.ContainsKey(fs.ID))
                        {
                            dicFacturi.Add(fs.IdFactura, fs);
                        }
                    }
                }

                sql = @"
                SELECT
                     fvz.[ID]
                    ,fvz.[IdFactura]
                    ,fvz.[IdPozitie]
                    ,fvz.[IdValoareZilnica]
                    ,fvz.[ValoareMoneda]
                    ,fvz.[ValoareRON]
                    ,fvz.[ValoareMonedaServ]
                    ,fvz.[Anulat]
                FROM 
	                [SOLON.H].[financiar].[FacturiValoriZilnice] AS fvz INNER JOIN
	                [SOLON.H].[financiar].[Facturi] AS f ON f.[ID] = fvz.[IdFactura]
                WHERE
	                f.[Anulata] = 0 AND
	                f.[Emisa] = 1 AND
	                fvz.[IdValoareZilnica] IN (SELECT [ID] FROM [SOLON.H].[hotel].[RezervariServiciiValori] WHERE [IdRezervareServiciu] = @IdRezervareServiciu)
            ";
                cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@IdRezervareServiciu", SqlDbType.Int)).Value = idRezervareServiciu;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FacturaValoriZilnice fvz;
                    while (reader.Read())
                    {
                        fvz = new FacturaValoriZilnice();
                        fvz.ID = Convert.ToInt32(reader["ID"]);
                        fvz.IdFactura = Convert.ToInt32(reader["IdFactura"]);
                        fvz.IdPozitie = Convert.ToInt32(reader["IdPozitie"]);
                        fvz.IdValoareZilnica = Convert.ToInt32(reader["IdValoareZilnica"]);
                        fvz.ValoareMonedaFact = Convert.ToDecimal(reader["ValoareMoneda"]);
                        fvz.ValoareRON = Convert.ToDecimal(reader["ValoareRON"]);
                        fvz.ValoareMonedaServ = Convert.ToDecimal(reader["ValoareMonedaServ"]);
                        fvz.Anulat = Convert.ToBoolean(reader["Anulat"]);

                        if (dicFacturi.ContainsKey(fvz.IdFactura))
                        {
                            fs = dicFacturi[fvz.IdFactura];
                            if (!fs.DicFacturiValoriZilnice.ContainsKey(fvz.IdValoareZilnica))
                            {
                                fs.DicFacturiValoriZilnice.Add(fvz.IdValoareZilnica, fvz);
                            }
                        }
                    }
                }

                sql = @"
                SELECT
                     [ID]
                    ,[IdRezervareServiciu]
                    ,[Data]
                    ,[Valoare]
                    ,[Curs]
                    ,[ValoareRON]
                    ,[Postat]
                    ,[Platit]
                    ,[PlatitRON]
                    ,[Facturat]
                    ,[FacturatRON]
                    ,[Sters]
                FROM [SOLON.H].[hotel].[RezervariServiciiValori]
                WHERE
	                [IdRezervareServiciu] = @IdRezervareServiciu
            ";
                cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@IdRezervareServiciu", SqlDbType.Int)).Value = idRezervareServiciu;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    ValoareZilnica vz;
                    while (reader.Read())
                    {
                        vz = new ValoareZilnica();
                        vz.ID = Convert.ToInt32(reader["ID"]);
                        vz.IdRezervareServiciu = Convert.ToInt32(reader["IdRezervareServiciu"]);
                        vz.Data = Convert.ToDateTime(reader["Data"]);
                        vz.Valoare = Convert.ToDecimal(reader["Valoare"]);
                        vz.Curs = Convert.ToDecimal(reader["Curs"]);
                        vz.ValoareRON = Convert.ToDecimal(reader["ValoareRON"]);
                        vz.Postat = Convert.ToBoolean(reader["Postat"]);
                        vz.Platit = Convert.ToDecimal(reader["Platit"]);
                        vz.PlatitRON = Convert.ToDecimal(reader["PlatitRON"]);
                        vz.Facturat = Convert.ToDecimal(reader["Facturat"]);
                        vz.FacturatRON = Convert.ToDecimal(reader["FacturatRON"]);
                        vz.Sters = Convert.ToBoolean(reader["Sters"]);

                        if (!dicValoriZilnice.ContainsKey(vz.ID))
                        {
                            dicValoriZilnice.Add(vz.ID, vz);
                        }
                    }
                }
            }

            public static void RepartizeazaValoriServicii(List<int> idsServicii)
            {
                using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                {
                    try
                    {
                        cnn.Open();
                        using (SqlTransaction tran = cnn.BeginTransaction())
                        {
                            try
                            {
                                foreach (int id in idsServicii)
                                {
                                    RepartizeazaValori(id, cnn, tran);
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
                        //Log.ExceptieSql(exc, "HelperValoriZilniceFactura.RepartizeazaValoriServicii()");
                    }
                }
            }

            public static void RepartizeazaValori(int idRezervareServiciu, SqlConnection cnn, SqlTransaction tran)
            {
                Dictionary<int, FacturaServicii> dicFacturi = null;
                Dictionary<int, ValoareZilnica> dicValori = null;

                GetFacturi(idRezervareServiciu, cnn, tran, out dicFacturi, out dicValori);

                decimal deRepartizat, deRepartizatRON, deRepartizatFact, curs, cursFact, dif, difRON, difFact;
                bool esteRepartizat;

                foreach (FacturaServicii fs in dicFacturi.Values.OrderBy(x => x.IdFactura))
                {
                    foreach (FacturaValoriZilnice fvz in fs.DicFacturiValoriZilnice.Values)
                    {
                        dicValori[fvz.IdValoareZilnica].Suma += fvz.ValoareMonedaServ;
                        dicValori[fvz.IdValoareZilnica].SumaRON += fvz.ValoareRON;
                    }
                }

                List<ValoareZilnica> vzs = dicValori.Values.OrderBy(x => x.Data).ToList();
                List<FacturaValoriZilnice> lista = null;
                foreach (FacturaServicii fs in dicFacturi.Values.OrderBy(x => x.IdFactura))
                {
                    lista = fs.DicFacturiValoriZilnice.Values.ToList();
                    esteRepartizat = true;
                    deRepartizat = 0;
                    deRepartizatRON = 0;
                    deRepartizatFact = 0;
                    if (fs.ValoareMonedaServ != 0)
                    {
                        curs = fs.ValoareRON / fs.ValoareMonedaServ;
                    }
                    else
                    {
                        curs = 1;
                    }
                    if (fs.ValoareMonedaFact != 0)
                    {
                        cursFact = fs.ValoareRON / fs.ValoareMonedaFact;
                    }
                    else
                    {
                        cursFact = 1;
                    }
                    if (lista.Count == 0 || lista.Sum(x => x.ValoareMonedaFact) != fs.ValoareMonedaFact)
                    {
                        esteRepartizat = false;
                        if (lista.Count > 0)
                        {
                            deRepartizat = fs.ValoareMonedaServ - lista.Sum(x => x.ValoareMonedaServ);
                            deRepartizatRON = fs.ValoareRON - lista.Sum(x => x.ValoareRON);
                            deRepartizatFact = fs.ValoareMonedaFact - lista.Sum(x => x.ValoareMonedaFact);
                        }
                        else
                        {
                            deRepartizat = fs.ValoareMonedaServ;
                            deRepartizatRON = fs.ValoareRON;
                            deRepartizatFact = fs.ValoareMonedaFact;
                        }
                    }
                    if (!esteRepartizat)
                    {
                        for (int i = 0; i < vzs.Count; i++)
                        {
                            if (vzs[i].Suma == vzs[i].Valoare)
                            {
                                continue;
                            }
                            if (fs.DicFacturiValoriZilnice.ContainsKey(vzs[i].ID))
                            {
                                continue;
                            }
                            if (vzs[i].Valoare >= 0)
                            {
                                // valoarea zilnica este pozitiva
                                if (deRepartizat >= 0)
                                {
                                    // suma de repartizat este pozitiva
                                    if (vzs[i].Suma + deRepartizat > vzs[i].Valoare)
                                    {
                                        dif = vzs[i].Valoare - vzs[i].Suma;
                                        difRON = Math.Round(dif * curs, 2);
                                        difFact = Math.Round(dif * curs / cursFact, 2);

                                        vzs[i].Suma += dif;
                                        vzs[i].SumaRON += difRON;

                                        deRepartizat -= dif;
                                        deRepartizatRON -= difRON;
                                        deRepartizatFact -= difFact;

                                        if (dif != 0)
                                        {
                                            fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                            {
                                                ID = 0,
                                                IdFactura = fs.IdFactura,
                                                IdPozitie = fs.IdPozitie,
                                                IdValoareZilnica = vzs[i].ID,
                                                ValoareMonedaFact = difFact,
                                                ValoareRON = difRON,
                                                ValoareMonedaServ = dif,
                                                Anulat = false
                                            });
                                        }
                                    }
                                    else
                                    {
                                        dif = deRepartizat;
                                        difRON = deRepartizatRON;
                                        difFact = deRepartizatFact;

                                        vzs[i].Suma += dif;
                                        vzs[i].SumaRON += difRON;

                                        deRepartizat = 0;
                                        deRepartizatRON = 0;

                                        if (dif != 0)
                                        {
                                            fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                            {
                                                ID = 0,
                                                IdFactura = fs.IdFactura,
                                                IdPozitie = fs.IdPozitie,
                                                IdValoareZilnica = vzs[i].ID,
                                                ValoareMonedaFact = difFact,
                                                ValoareRON = difRON,
                                                ValoareMonedaServ = dif,
                                                Anulat = false
                                            });
                                        }
                                    }
                                }
                                else
                                {
                                    // suma de repartizat este negativa
                                    dif = vzs[i].Valoare - vzs[i].Suma;
                                    difRON = Math.Round(dif * curs, 2);
                                    difFact = Math.Round(dif * curs / cursFact, 2);

                                    vzs[i].Suma += dif;
                                    vzs[i].SumaRON += difRON;

                                    deRepartizat -= dif;
                                    deRepartizatRON -= difRON;
                                    deRepartizatFact -= difFact;

                                    if (dif != 0)
                                    {
                                        fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                        {
                                            ID = 0,
                                            IdFactura = fs.IdFactura,
                                            IdPozitie = fs.IdPozitie,
                                            IdValoareZilnica = vzs[i].ID,
                                            ValoareMonedaFact = difFact,
                                            ValoareRON = difRON,
                                            ValoareMonedaServ = dif,
                                            Anulat = false
                                        });
                                    }
                                }
                            }
                            else
                            {
                                // valoarea zilnica este negativa
                                if (deRepartizat < 0)
                                {
                                    // suma de repartizat este tot negativa
                                    if (vzs[i].Suma + deRepartizat < vzs[i].Valoare)
                                    {
                                        dif = vzs[i].Valoare - vzs[i].Suma;
                                        difRON = Math.Round(dif * curs, 2);
                                        difFact = Math.Round(dif * curs / cursFact, 2);

                                        vzs[i].Suma += dif;
                                        vzs[i].SumaRON += difRON;

                                        deRepartizat -= dif;
                                        deRepartizatRON -= difRON;
                                        deRepartizatFact -= difFact;

                                        if (dif != 0)
                                        {
                                            fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                            {
                                                ID = 0,
                                                IdFactura = fs.IdFactura,
                                                IdPozitie = fs.IdPozitie,
                                                IdValoareZilnica = vzs[i].ID,
                                                ValoareMonedaFact = difFact,
                                                ValoareRON = difRON,
                                                ValoareMonedaServ = dif,
                                                Anulat = false
                                            });
                                        }
                                    }
                                    else
                                    {
                                        dif = deRepartizat;
                                        difRON = deRepartizatRON;
                                        difFact = deRepartizatFact;

                                        vzs[i].Suma += dif;
                                        vzs[i].SumaRON += difRON;

                                        deRepartizat = 0;
                                        deRepartizatRON = 0;

                                        if (dif != 0)
                                        {
                                            fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                            {
                                                ID = 0,
                                                IdFactura = fs.IdFactura,
                                                IdPozitie = fs.IdPozitie,
                                                IdValoareZilnica = vzs[i].ID,
                                                ValoareMonedaFact = difFact,
                                                ValoareRON = difRON,
                                                ValoareMonedaServ = dif,
                                                Anulat = false
                                            });
                                        }
                                    }
                                }
                                else if (deRepartizat > 0)
                                {
                                    // suma de repartizat este pozitiva
                                    dif = vzs[i].Valoare - vzs[i].Suma;
                                    difRON = Math.Round(dif * curs, 2);
                                    difFact = Math.Round(dif * curs / cursFact, 2);

                                    vzs[i].Suma += dif;
                                    vzs[i].SumaRON += difRON;

                                    deRepartizat -= dif;
                                    deRepartizatRON -= difRON;
                                    deRepartizatFact -= difFact;

                                    if (dif != 0)
                                    {
                                        fs.DicFacturiValoriZilnice.Add(vzs[i].ID, new FacturaValoriZilnice()
                                        {
                                            ID = 0,
                                            IdFactura = fs.IdFactura,
                                            IdPozitie = fs.IdPozitie,
                                            IdValoareZilnica = vzs[i].ID,
                                            ValoareMonedaFact = difFact,
                                            ValoareRON = difRON,
                                            ValoareMonedaServ = dif,
                                            Anulat = false
                                        });
                                    }
                                }
                            }
                            if (deRepartizat == 0)
                            {
                                break;
                            }
                        }
                        if (deRepartizat != 0 && vzs.Count > 0)
                        {
                            FacturaValoriZilnice fvzUltima = null;
                            for (int i = vzs.Count - 1; i >= 0; i--)
                            {
                                if (fs.DicFacturiValoriZilnice.ContainsKey(vzs[i].ID))
                                {
                                    if (fs.DicFacturiValoriZilnice[vzs[i].ID].ID == 0)
                                    {
                                        fvzUltima = fs.DicFacturiValoriZilnice[vzs[i].ID];
                                        break;
                                    }
                                }
                            }
                            if (fvzUltima != null)
                            {
                                fvzUltima.ValoareRON += deRepartizatRON;
                                fvzUltima.ValoareMonedaServ += deRepartizat;
                                fvzUltima.ValoareMonedaFact += deRepartizatFact;
                            }
                        }
                    }
                }

                foreach (FacturaServicii fs in dicFacturi.Values)
                {
                    fs.Save(cnn, tran);
                }
            }

            public class FacturaValoriZilnice
            {
                public int ID { get; set; }
                public int IdFactura { get; set; }
                public int IdPozitie { get; set; }
                public int IdValoareZilnica { get; set; }
                public decimal ValoareMonedaFact { get; set; }
                public decimal ValoareRON { get; set; }
                public decimal ValoareMonedaServ { get; set; }
                public bool Anulat { get; set; }

                public EntitateServiciuValoare FF_ValoareZilnica { get; set; }
                public ValoareZilnica ValoareZilnica { get; set; }

                public void Save(SqlConnection cnn, SqlTransaction tran)
                {
                    string sql = "";
                    SqlCommand cmd = null;

                    if (this.ID == 0)
                    {
                        sql = @"
                        INSERT INTO [SOLON.H].[financiar].[FacturiValoriZilnice]
                            ([IdFactura]
                            ,[IdPozitie]
                            ,[IdValoareZilnica]
                            ,[ValoareMoneda]
                            ,[ValoareRON]
                            ,[ValoareMonedaServ]
                            ,[Anulat])
                        VALUES
                            (@IdFactura
                            ,@IdPozitie
                            ,@IdValoareZilnica
                            ,@ValoareMoneda
                            ,@ValoareRON
                            ,@ValoareMonedaServ
                            ,@Anulat);
                        SELECT SCOPE_IDENTITY();
                    ";
                        cmd = new SqlCommand(sql, cnn, tran);
                        cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = IdFactura;
                        cmd.Parameters.Add(new SqlParameter("@IdPozitie", SqlDbType.Int)).Value = IdPozitie;
                        cmd.Parameters.Add(new SqlParameter("@IdValoareZilnica", SqlDbType.Int)).Value = IdValoareZilnica;
                        cmd.Parameters.Add(new SqlParameter("@ValoareMoneda", SqlDbType.Decimal)).Value = ValoareMonedaFact;
                        cmd.Parameters.Add(new SqlParameter("@ValoareRON", SqlDbType.Decimal)).Value = ValoareRON;
                        cmd.Parameters.Add(new SqlParameter("@ValoareMonedaServ", SqlDbType.Decimal)).Value = ValoareMonedaServ;
                        cmd.Parameters.Add(new SqlParameter("@Anulat", SqlDbType.Bit)).Value = false;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                this.ID = reader[0] == DBNull.Value ? 0 : Convert.ToInt32(reader[0]);
                            }
                        }
                    }
                    if (this.ID == 0)
                    {
                        throw new Exception(string.Format("Nu a putut fi obtinut ID-ul inreg. in FacturiValoriZilnice coresp. (IdFactura = {0}, IdPozitie = {1}, IdValoareZilnica = {2})", IdFactura, IdPozitie, IdValoareZilnica));
                    }
                }
            }

            public class FacturaServicii
            {
                public int ID { get; set; }
                public int IdFactura { get; set; }
                public int IdPozitie { get; set; }
                public int IdRezervareServiciu { get; set; }
                public decimal ValoareMonedaFact { get; set; }
                public decimal ValoareRON { get; set; }
                public decimal ValoareMonedaServ { get; set; }
                public decimal CursServiciu { get; set; }
                public decimal CursFactura { get; set; }
                public decimal DiferentaCurs { get; set; }

                public Dictionary<int, FacturaValoriZilnice> DicFacturiValoriZilnice { get; private set; }
                public List<FacturaValoriZilnice> ListaFacturiValoriZilnice { get; set; }

                public FacturaServicii()
                {
                    DicFacturiValoriZilnice = new Dictionary<int, FacturaValoriZilnice>();
                    ListaFacturiValoriZilnice = new List<FacturaValoriZilnice>();   // ar fi preferabil ca si aceste valori facturate la valori zilnice sa fie salvate in ordinea datei valorilor zilnice
                }

                public void Save(SqlConnection cnn, SqlTransaction tran)
                {
                    foreach (FacturaValoriZilnice fvz in DicFacturiValoriZilnice.Values)
                    {
                        fvz.Save(cnn, tran);
                    }
                }
            }

            public class ValoareZilnica
            {
                public int ID { get; set; }
                public int IdRezervareServiciu { get; set; }
                public DateTime Data { get; set; }
                public decimal Valoare { get; set; }
                public decimal Curs { get; set; }
                public decimal ValoareRON { get; set; }
                public bool Postat { get; set; }
                public decimal Platit { get; set; }
                public decimal PlatitRON { get; set; }
                public decimal Facturat { get; set; }
                public decimal FacturatRON { get; set; }
                public bool Sters { get; set; }

                public decimal Suma { get; set; }
                public decimal SumaRON { get; set; }
            }
        }
}