using SelfHotel.Nomenclatoare_Final;
using SelfHotel.NomenclatoareNew;
using SelfHotel.Setari;
using SolonCloud.Reporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZipEscort.Setari;

namespace SelfHotel.GUI.CHECKIN
{
    public partial class CheckIn2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    List<DataLucruObj> listdDataLucru = DataLucruObj.GetLista().Where(x => x.IdHotel == ConexiuneDB.IdHotel).ToList();
            //    DateTime DataL;
            //    if (listdDataLucru.Count > 0)
            //    {
            //        DataL = listdDataLucru[0].DataLucru;
            //    }
            //    else
            //    {
            //        DataL = DateTime.Today;
            //    }
            //    ConexiuneDB.DataLucr = DataL;
            //}
            //catch { }
            ConexiuneDB.initializare();//incarc setari aici
            try
            {
                String codRezervare = HttpContext.Current.Session["codRezervare"] as String;
                Uri myReferrer = Request.UrlReferrer;
                String actual = myReferrer.ToString();
                if (actual.Contains("PlataForm") && !string.IsNullOrEmpty(codRezervare))
                {
                    this.txtCod.Value = codRezervare;
                    this.parinteInput.Attributes["class"] += " hasValue";
                }
            }
            catch { }
        }

        public class RezervariCamereTuple
        {
            public List<RezervariCamere> lista { get; set; }
            public int status { get; set; }
            public string DataIn { get; set; }
            public string DataOut { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static RezervariCamereTuple getRezervare(string CodRezervareTmp)
        {
            RezervariCamereTuple rezultat = new RezervariCamereTuple();
            List<RezervariCamere> rv = null;
            try
            {
                Rezervari rezervare = Rezervari.GetRezervare(CodRezervareTmp.Trim());
                if (rezervare == null)
                {
                    rezultat.status = 0;//nu exista rezervarea
                    return rezultat;
                }
                rezultat.DataIn = rezervare.Sosire.ToShortDateString();
                rezultat.DataOut = rezervare.Plecare.ToShortDateString();
                HttpContext.Current.Session["IdRezervare"] = rezervare.ID.ToString();
                HttpContext.Current.Session["codRezervare"] = rezervare.CodRezervare.Trim();
                if (rezervare.Cazat)
                {
                    rezultat.status = 1;//rezervare deja cazata
                    return rezultat;
                }
                if (rezervare.Sosire != ConexiuneDB.DataLucr)
                {
                    rezultat.status = 2;//nu este ziua checkIn-ului
                    return rezultat;
                }
                try
                {
                    calculSoldCazare(rezervare.ID);
                }
                catch { }
                rv = RezervariCamere.GetLista(rezervare.ID);
                    foreach (RezervariCamere rc in rv)
                    {
                        rc.listaServicii = RezervariServicii.GetLista(rc.ID);
                    }
                    try
                    {
                        foreach (RezervariCamere rc in rv)
                        {
                            rc.turist = NomParteneri.GetLista(rc.IdTurist);
                        }
                    }
                    catch (Exception exc) { }
                HttpContext.Current.Session["IdRezervare"] = rezervare.ID.ToString();
                HttpContext.Current.Session["codRezervare"] = rezervare.CodRezervare.Trim();
                
                rezultat.status = 4;//ok
                rezultat.lista = rv;
                Jurnal.Salveaza(
                            "SelfHotel.GUI.CHECKIN.CheckIn2",
                            Jurnal.Actiune.VIZ,
                            Jurnal.ImportantaMare,
                            string.Format("Cauta rezervare: ID={0} Cod={1}, getRezervare()", rezervare.ID, rezervare.CodRezervare));
            }
            catch (Exception ex)
            {
                LogErori.Salveaza(ex, "CheckIn2.getRezervare()");
                rezultat.status = 3;//eroare necunoascuta
                return rezultat;
            }
            return rezultat;
        }

        public static Decimal calculSoldCazare(int idRezervare)
        {
            Decimal rv = 0;
            List<int> idRezCams = new List<int>();
            try
            {
                 idRezCams = RezervariCamere.GetLista(idRezervare).Select(x => x.ID).ToList();
            }
            catch { }
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = "";
                    SqlCommand cmd = null;

                    sql = @"[SOLON.H].[dbo].[usp_calculSoldCazare]";

                    foreach (int IdRezCam in idRezCams)
                    {
                        cmd = new SqlCommand(sql, cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdRezCamera", SqlDbType.Int)).Value = IdRezCam;
                        cmd.ExecuteNonQuery();
                    }

                    foreach (int ID in idRezCams)
                    {
                        sql = @"SELECT  rc.[ID]
                                   ,[IdRezervare]
                                   ,[SoldRec]
                                   ,[SoldVir]
                         FROM [SOLON.H].[hotel].[RezervariCamere] as rc left outer join [SOLON.H].[hotel].[TipCamera] as tc on tc.ID=rc.IdTipCamera
                         WHERE rc.Sters=0 and tc.Sters=0 and tc.Virtuala=0 and tc.Suplimentara=0 and IdRezervare=@IdRezervare and rc.[ID]=@ID;";
                        cmd = new SqlCommand(sql, cnn);
                        cmd.Parameters.Add(new SqlParameter("@IdRezervare", SqlDbType.BigInt)).Value = idRezervare;
                        cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)).Value = ID;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rv += Convert.ToDecimal(reader["SoldRec"]);
                                rv += Convert.ToDecimal(reader["SoldVir"]);
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    LogErori.Salveaza(exc, "CheckIn2.calculSoldCazare()");
                    return -1;
                }
            }
            return rv;
        }

        public class temp
        {
            public string checkIn { get; set; }
            public string chekcOut { get; set; }
            public string Descriere { get; set; }
        }

        public class EntitateFisa
        {
            public string NumePrenume { get; set; }
            public string DataNastere { get; set; }
            public string LocNastere { get; set; }
            public string Cetatenie { get; set; }
            public string Localitatea { get; set; }
            public string Strada { get; set; }
            public string NrStrada { get; set; }
            public string Tara { get; set; }
            public string DataSosire { get; set; }
            public string DataPlecare { get; set; }
            public string Scop { get; set; }
            public string ActIdentitate { get; set; }
            public string Seria { get; set; }
            public string NrAct { get; set; }
            public string CodRezervare { get; set; }
            public string IdCam { get; set; }
            public string TipCam { get; set; }
            public string IdTurist { get; set; }
            public string IdRezervare { get; set; }
            public string IdRezervareCamera { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static bool incarcaFiseInBaza(List<EntitateFisa> str)
        {
            bool rv = false;
            try
            {
                String codRezervare = HttpContext.Current.Session["codRezervare"] as String;
                String IdRezervare = HttpContext.Current.Session["IdRezervare"] as String;
                if (!string.IsNullOrEmpty(codRezervare) || !string.IsNullOrEmpty(IdRezervare))
                {
                    foreach (EntitateFisa fc in str)
                    {
                        RezervariFiseTurist fisa = new RezervariFiseTurist();
                        fisa.NumePrenume = fc.NumePrenume;
                        try
                        {
                            fisa.DataNasterii = DateTime.ParseExact(fc.DataNastere.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                        catch (Exception exc)
                        {
                            fisa.DataNasterii = DateTime.MinValue;
                        }
                        try
                        {
                            fisa.Sosire = DateTime.ParseExact(fc.DataSosire.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                        catch (Exception exc)
                        {
                            fisa.Sosire = DateTime.MinValue;
                        }
                        try
                        {
                            fisa.Plecare = DateTime.ParseExact(fc.DataPlecare.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                        catch (Exception exc)
                        {
                            fisa.Plecare = DateTime.MinValue;
                        }
                        fisa.Cetatenia = fc.Cetatenie;
                        fisa.Localitate = fc.Localitatea;
                        fisa.Strada = fc.Strada;
                        fisa.Numar = fc.NrStrada;
                        fisa.Tara = fc.Tara;
                        fisa.Scopul = fc.Scop;
                        fisa.ActTip = fc.ActIdentitate;
                        fisa.ActSerie = fc.Seria;
                        fisa.ActNumar = fc.NrAct;
                        fisa.Camera = fc.IdCam;
                        int idRezervareCamera = 0;
                        Int32.TryParse(fc.IdRezervareCamera, out idRezervareCamera);
                        fisa.IdRezervareCamera = idRezervareCamera;
                        int IdTurist = 0; Int32.TryParse(fc.IdTurist, out IdTurist);
                        fisa.IdTurist = IdTurist;
                        fisa.IdHotel = ConexiuneDB.IdHotel;
                        fisa.IdCarnet = 0;
                        fisa.Serie = fc.Seria;
                        fisa.Nr = fc.NrAct;
                        fisa.DataOraCreare = DateTime.Now;
                        fisa.IdModRezervare = 0;
                        fisa.IdSegmentPiata = 0;
                        fisa.IdSursaRezervare = 0;
                        fisa.Anulata = false;
                        fisa.IdUserAnulare = 0;
                        fisa.DataLucru = ConexiuneDB.DataLucr;

                        bool x = RezervariFiseTurist.Insert(fisa);
                    }
                    rv=setRezervareCazata(IdRezervare);
                    Jurnal.Salveaza(
                            "SelfHotel.GUI.CHECKIN.CheckIn2",
                            Jurnal.Actiune.VIZ,
                            Jurnal.ImportantaMare,
                            string.Format("incarca Fise In Baza , pentru rezervarea ID={0} Cod={1}, incarcaFiseInBaza()", IdRezervare, codRezervare));
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exc)
            {
                LogErori.Salveaza(exc, "CheckIn2.incarcaFiseInBaza()");
                rv = false;
            }
            return rv;
        }

        public class FisierDescarcare
        {
            public string Tip { get; set; }
            public string Denumire { get; set; }
            public byte[] Continut { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static FisierDescarcare printeazaDocument(List<EntitateFisa> str)
        {
            FisierDescarcare rez = new FisierDescarcare();
            string bf = "JurnalOperatii.html";
            try
            {
                DocGenHtml dgh = new DocGenHtml(bf, str, "", true);
                dgh.Salveaza();
                try
                {
                    if (File.Exists(dgh.LocatieFisierNou))
                    {
                        byte[] array = File.ReadAllBytes(dgh.LocatieFisierNou);
                        File.Delete(dgh.LocatieFisierNou);
                        rez.Continut = array;
                        rez.Tip = "application/pdf";
                        rez.Denumire = "FisaTurist_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + ".pdf";
                    }
                    Jurnal.Salveaza(
                            "SelfHotel.GUI.CHECKIN.CheckIn2",
                            Jurnal.Actiune.VIZ,
                            Jurnal.ImportantaMare,
                            string.Format("printeaza fise pentru rezervarea: {0} {1}, printeazaDocument()","",""));
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception exceptie)
            {
                LogErori.Salveaza(exceptie, "CheckIn2.printeazaDocument()");
                return null;
            }
            return rez;
        }

        [WebMethod(EnableSession = true)]
        public static bool setRezervareCazata(string id)
        {
            bool rv = false;
            try
            {
                Int32 IdRezervare = Convert.ToInt32(id);
                String codRezervare = HttpContext.Current.Session["codRezervare"] as String;
                if (!string.IsNullOrEmpty(codRezervare))
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
                                    string sql = "";
                                    SqlCommand cmd = null;

                                    sql = @"UPDATE [SOLON.H].[hotel].[Rezervari]
                                                   SET 
                                                      [Cazat] = 1
                                                 WHERE ID=@ID";
                                    cmd = new SqlCommand(sql, cnn, tran);
                                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)).Value = IdRezervare;
                                    int randuriModificate = Convert.ToInt32(cmd.ExecuteNonQuery());

                                    sql = @"UPDATE [SOLON.H].[hotel].[RezervariCamere]
                                               SET [Cazat] = 1
                                                  ,[DataPrezentarii]=@DataPrezentarii
                                            WHERE [IdRezervare] = @IdRezervare";
                                    cmd = new SqlCommand(sql, cnn, tran);
                                    cmd.Parameters.Add(new SqlParameter("@IdRezervare", SqlDbType.BigInt)).Value = IdRezervare;
                                    cmd.Parameters.Add(new SqlParameter("@DataPrezentarii", SqlDbType.Date)).Value = DateTime.Today;
                                    int randuriModificate2 = Convert.ToInt32(cmd.ExecuteNonQuery());

                                    List<Int32> idCamereRezervari = new List<Int32>();
                                    sql = @"SELECT [ID]
                                                  ,[IdRezervare]
                                            FROM [SOLON.H].[hotel].[RezervariCamere]
                                            WHERE Sters=0 AND Cazat=1 and IdRezervare=@IdRezervare;";
                                    cmd = new SqlCommand(sql, cnn, tran);
                                    cmd.Parameters.Add(new SqlParameter("@IdRezervare", SqlDbType.BigInt)).Value = IdRezervare;
                                    using (SqlDataReader reader = cmd.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            idCamereRezervari.Add(Convert.ToInt32(reader["ID"]));
                                        }
                                    }
                                    foreach (Int32 idRezCam in idCamereRezervari)
                                    {
                                        rv = PosteazaServiciiAvans(IdRezervare, idRezCam, cnn, tran, cmd);
                                    }
                                    tran.Commit();
                                    rv = true;
                                    Jurnal.Salveaza(
                                        "SelfHotel.GUI.CHECKIN.CheckIn2",
                                        Jurnal.Actiune.VIZ,
                                        Jurnal.ImportantaMare,
                                        string.Format("Seteaza rezervarea cazata: ID={0}, setRezervareCazata()", IdRezervare));
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
                            LogErori.Salveaza(exc, "CheckIn2.setRezervareCazata()");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogErori.Salveaza(ex, "CheckIn2.setRezervareCazata()");
                return false;
            }
            return rv;
        }

        public static bool PosteazaServiciiAvans(int idRezervare, int idRezCamera, SqlConnection cnn, SqlTransaction tran, SqlCommand cmd) //dupa ce se cazeaza
        {
            if (idRezervare == 0 || idRezCamera == 0)
            {
                return false;
            }

            bool rv = false;

            List<int> idsAvansuri = new List<int>();
            List<int> idsPlati = new List<int>();
            DateTime sosire = ConexiuneDB.DataLucr;
            int idRon = ConexiuneDB.moneda == null ? 0 : HelperCurs.IdRON;// ConexiuneDB.moneda.IdMoneda;// CursValutar.HelperCurs.IdRON;

            string sql = @"
                                SELECT 
                                    ID, 
                                    IdPlata,
                                    IdRezervareCamera
                                FROM 
                                    [SOLON.H].hotel.RezervariAvans 
                                WHERE 
                                    IdRezervare = @IdRezervare AND
                                    IdPlata IS NOT NULL AND Utilizat = 0
                            ";
            cmd = new SqlCommand(sql, cnn, tran);
            cmd.Parameters.Add(new SqlParameter("@IdRezervare", SqlDbType.Int)).Value = idRezervare;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int IdRezCamAvans = reader[2] == DBNull.Value ? 0 : Convert.ToInt32(reader[2]);
                    if (IdRezCamAvans == 0)// avans toata rezervarea
                    {
                        idsAvansuri.Add(Convert.ToInt32(reader[0]));
                        idsPlati.Add(Convert.ToInt32(reader[1]));
                    }
                    else if (IdRezCamAvans == idRezCamera)//avans per camera si verific daca e pt camera asta
                    {
                        idsAvansuri.Add(Convert.ToInt32(reader[0]));
                        idsPlati.Add(Convert.ToInt32(reader[1]));
                    }
                }
            }

            if (idsAvansuri.Count > 0)
            {
                sql = "SELECT Sosire FROM [SOLON.H].hotel.RezervariCamere WHERE ID = @ID";
                cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = idRezCamera;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        sosire = Convert.ToDateTime(reader[0]);
                    }
                }
                List<int> idsServicii = new List<int>();
                List<int> idsCoteOld = new List<int>();     // id-uri cote de TVA serviciile de avans postate si platite la momentul rezervarii (posibil avand o cota expirata intre timp)
                List<int> idsCoteNew = new List<int>();     // id-uri cote de TVA de pus pe serviciile storno avans postate in fisa (nefiind platite trebuie sa aibe un id cota curent)
                List<int> idsServiciiNew = new List<int>();

                string sqlServ = @"
                                    SELECT 
                                        rs.ID 
                                       ,rs.[IdCotaTVA]
                                       ,c.[IdCotaNew]
                                    FROM 
                                        [SOLON.H].[hotel].[RezervariServicii] AS rs LEFT OUTER JOIN
                                        [SOLON.H].[financiar].[CoteTvaLeg] AS c ON c.[IdCota] = rs.[IdCotaTVA]
                                    WHERE 
                                        rs.[IdAvansRezervare] = @IdAvansRezervare AND 
                                        rs.[Sters] = 0
                                ";
                SqlCommand cmdServ = new SqlCommand(sqlServ, cnn, tran);
                cmdServ.Parameters.Add(new SqlParameter("@IdAvansRezervare", SqlDbType.Int));

                string sqlPlata = @"
                                    SELECT 
                                         ps.IdRezervareServiciu 
                                        ,rs.[IdCotaTVA]
                                        ,c.[IdCotaNew]
                                    FROM 
                                        [SOLON.H].financiar.PlatiServicii AS ps LEFT OUTER JOIN 
                                        [SOLON.H].financiar.Plati as plata ON plata.Id = ps.IdPlata LEFT OUTER JOIN
                                        [SOLON.H].[hotel].[RezervariServicii] AS rs ON rs.[ID] = ps.IdRezervareServiciu LEFT OUTER JOIN
                                        [SOLON.H].[financiar].[CoteTvaLeg] AS c ON c.[IdCota] = rs.[IdCotaTVA]
                                    WHERE 
                                        IdPlata = @IdPlata AND 
                                        plata.[Sters] = 0
                                ";
                SqlCommand cmdPlata = new SqlCommand(sqlPlata, cnn, tran);
                cmdPlata.Parameters.Add(new SqlParameter("@IdPlata", SqlDbType.Int));

                //foreach (int id in idsAvansuri)
                for (int i = 0; i < idsAvansuri.Count; i++)
                {
                    bool dinAvans = false;
                    cmdServ.Parameters["@IdAvansRezervare"].Value = idsAvansuri[i];
                    using (SqlDataReader reader = cmdServ.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dinAvans = true;

                            idsServicii.Add(Convert.ToInt32(reader[0]));

                            idsCoteOld.Add(reader[1] == DBNull.Value ? 0 : Convert.ToInt32(reader[1]));
                            idsCoteNew.Add(reader[2] == DBNull.Value ? 0 : Convert.ToInt32(reader[2]));
                        }
                    }
                    if (!dinAvans)
                    {
                        cmdPlata.Parameters["@IdPlata"].Value = idsPlati[i];
                        using (SqlDataReader reader = cmdPlata.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idsServicii.Add(Convert.ToInt32(reader[0]));

                                idsCoteOld.Add(reader[1] == DBNull.Value ? 0 : Convert.ToInt32(reader[1]));
                                idsCoteNew.Add(reader[2] == DBNull.Value ? 0 : Convert.ToInt32(reader[2]));
                            }
                        }
                    }
                }

                //                                sql = @"
                //                                    SELECT 
                //                                        ps.IdRezervareServiciu
                //                                    FROM 
                //                                        financiar.PlatiServicii AS ps
                //                                    WHERE IdPlata IN (<ids_plati>)
                //                                ";
                //                                sql = sql.Replace("<ids_plati>", idsPlati.Select(x => x.ToString()).Aggregate((x1, x2) => x1 + "," + x2));
                //                                cmd = new SqlCommand(sql, cnn, tran);
                //                                using (SqlDataReader reader = cmd.ExecuteReader())
                //                                {
                //                                    int idRS;
                //                                    while (reader.Read())
                //                                    {
                //                                        idRS = Convert.ToInt32(reader[0]);
                //                                        idsServicii.Add(idRS);
                //                                    }
                //                                }

                List<string> facturi = new List<string>();
                if (idsServicii.Count > 0)
                {
                    sql = @"SELECT 
                                            ISNULL(f.serie,' ')+ ' ' +isnull(f.Numar,' ') AS Factura
                                            FROM 
                                            [SOLON.H].hotel.RezervariServicii AS rs LEFT OUTER JOIN
                                            [SOLON.H].financiar.FacturiServicii AS fs ON fs.IdRezervareServiciu = rs.ID LEFT OUTER JOIN 
                                            [SOLON.H].financiar.Facturi AS f ON f.ID  = fs.IdFactura
                                            WHERE f.Anulata = 0 AND rs.ID = @ID";
                    cmd = new SqlCommand(sql, cnn, tran);
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int));
                    foreach (int id in idsServicii)
                    {
                        cmd.Parameters["@ID"].Value = id;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            string f;
                            if (reader.Read())
                            {
                                f = reader[0] == DBNull.Value ? "" : reader[0].ToString();
                                f = f.Trim();
                                facturi.Add(f);
                            }
                        }
                    }

                    sql = @"
                                        INSERT INTO [SOLON.H].[hotel].[RezervariServicii]
                                            ([IdPartener]
                                            ,[EsteVirament]
                                            ,[IdRezervareCamera]
                                            ,[IdVenit]
                                            ,[IdTarif]
                                            ,[IdPlanMasa]
                                            ,[DenumireServiciu]
                                            ,[IdMoneda]
                                            ,[IdCotaTVA]
                                            ,[TaxaProcentuala]
                                            ,[TaxaLaPersoane]
                                            ,[ValoareTaxa]
                                            ,[Observatii]
                                            ,[Sters]
                                            ,[IdMotivStergere]
                                            ,[Cantitate]
                                            ,[PostareAmanata]
                                            ,[EsteDiscount]
                                            ,[IdServiciuDiscount]
                                            ,[IdRezervareCameraMutat]
                                            ,[ParametriPostare]
                                            ,[EsteStorno]
                                            ,[ValoareMoneda]
                                            ,[Curs]
                                            ,[ValoareRon])
                                        SELECT
                                             [IdPartener]
                                            ,[EsteVirament]
                                            ,@IdRezervareCamera
                                            ,[IdVenit]
                                            ,[IdTarif]
                                            ,[IdPlanMasa]
                                            ,'STORNO ' + [DenumireServiciu] + @Factura
                                            ,@IdMoneda
                                            ,@IdCotaTVA
                                            ,[TaxaProcentuala]
                                            ,[TaxaLaPersoane]
                                            ,[ValoareTaxa]
                                            ,[Observatii]
                                            ,[Sters]
                                            ,[IdMotivStergere]
                                            ,[Cantitate]
                                            ,[PostareAmanata]
                                            ,[EsteDiscount]
                                            ,[IdServiciuDiscount]
                                            ,[IdRezervareCameraMutat]
                                            ,[ParametriPostare]
                                            ,1
                                            ,-[ValoareRon] --[ValoareMoneda]
                                            ,1 --[Curs]
                                            ,-[ValoareRon]
                                        FROM [SOLON.H].[hotel].[RezervariServicii]
                                        WHERE ID = @ID;
                                        SELECT SCOPE_IDENTITY()
                                    ";
                    cmd = new SqlCommand(sql, cnn, tran);
                    cmd.Parameters.Add(new SqlParameter("@IdRezervareCamera", SqlDbType.Int)).Value = idRezCamera;
                    cmd.Parameters.Add(new SqlParameter("@IdMoneda", SqlDbType.Int)).Value = idRon;
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@Factura", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@IdCotaTVA", SqlDbType.Int));
                    int index = 0;
                    foreach (int id in idsServicii)
                    {
                        cmd.Parameters["@ID"].Value = id;
                        if (facturi.Count > 0 && index < facturi.Count)
                        {
                            cmd.Parameters["@Factura"].Value = string.IsNullOrEmpty(facturi[index].Trim()) ? "" : " - " + facturi[index];
                        }
                        else
                        {
                            cmd.Parameters["@Factura"].Value = "";
                        }

                        if (ConexiuneDB.DataLucr < new DateTime(2017, 1, 1))
                        {
                            cmd.Parameters["@IdCotaTVA"].Value = idsCoteOld[index];
                        }
                        else
                        {
                            if (idsCoteNew[index] != 0)
                            {
                                cmd.Parameters["@IdCotaTVA"].Value = idsCoteNew[index];
                            }
                            else
                            {
                                cmd.Parameters["@IdCotaTVA"].Value = idsCoteOld[index];
                            }
                        }

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            int idNew;
                            if (reader.Read())
                            {
                                idNew = reader[0] == DBNull.Value ? 0 : Convert.ToInt32(reader[0]);
                                idsServiciiNew.Add(idNew);
                            }
                        }
                        index++;
                    }

                    sql = @"
                                        INSERT INTO [SOLON.H].[hotel].[RezervariServiciiValori]
                                            ([IdRezervareServiciu]
                                            ,[Data]
                                            ,[Valoare]
                                            ,[Curs]
                                            ,[ValoareRON]
                                            ,[Postat])
                                        SELECT
                                             @IdRezervareServiciuNou
                                            ,@Data
                                            ,-[ValoareRon] --*[Curs]
                                            ,1 --[Curs]
                                            ,-[ValoareRon] --*[Curs]
                                            ,0
                                        FROM [SOLON.H].[hotel].[RezervariServiciiValori]
                                        WHERE [IdRezervareServiciu] = @IdRezervareServiciuOld
                                    ";
                    cmd = new SqlCommand(sql, cnn, tran);
                    cmd.Parameters.Add(new SqlParameter("@IdRezervareServiciuNou", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@Data", SqlDbType.DateTime)).Value = sosire;
                    //cmd.Parameters.Add(new SqlParameter("@Curs", SqlDbType.Decimal)).Value = 1.0m;
                    cmd.Parameters.Add(new SqlParameter("@IdRezervareServiciuOld", SqlDbType.Int));

                    // deocamdata merge si asa dar ar trebui sa inseram si in tabela Inc (TODO); pt. moment se regleaza automat la prima salvare de fisa.

                    for (int i = 0; i < idsServicii.Count; i++)
                    {
                        if (idsServiciiNew[i] == 0)
                        {
                            throw new Exception("Nu au putut fi obtinute ID-uri pentru servicii noi de avans!");
                        }
                        cmd.Parameters["@IdRezervareServiciuNou"].Value = idsServiciiNew[i];
                        cmd.Parameters["@IdRezervareServiciuOld"].Value = idsServicii[i];
                        cmd.ExecuteNonQuery();
                    }
                }

                sql = "UPDATE [SOLON.H].hotel.RezervariAvans SET IdRezervareCamera = @IdRezervareCamera, Utilizat = 1 WHERE ID = @ID";
                cmd = new SqlCommand(sql, cnn, tran);
                cmd.Parameters.Add(new SqlParameter("@IdRezervareCamera", SqlDbType.Int)).Value = idRezCamera;
                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int));
                foreach (int id in idsAvansuri)
                {
                    cmd.Parameters["@ID"].Value = id;
                    cmd.ExecuteNonQuery();
                }
            }
            Jurnal.Salveaza(
                            "SelfHotel.GUI.CHECKIN.CheckIn2",
                            Jurnal.Actiune.VIZ,
                            Jurnal.ImportantaMare,
                            string.Format("Poteaza servicii avans pentru rezervarea: ID={0} si IdRezCamera={1}, PosteazaServiciiAvans()", idRezervare, idRezCamera));
            rv = true;
            return rv;
        }

        [WebMethod(EnableSession = true)]
        public static List<String> getSetariHeader()
        {
            List<String> rv = new List<string>();
            try
            {
                List<SetariBaza> lista=SetariBaza.getLista().ToList();
                if (lista.Count > 0)
                {
                    List<SetariBaza> setare = lista.Where(x => x.ID == 13).ToList();
                    String nrStele = setare[0].Valoare;
                    rv.Add(nrStele);
                    setare = lista.Where(x => x.ID == 14).ToList();
                    String denumireHotel = setare[0].Valoare;
                    rv.Add(denumireHotel);
                    setare = lista.Where(x => x.ID == 16).ToList();
                    String adresaHotel = setare[0].Valoare;
                    rv.Add(adresaHotel);
                }
            }
            catch (Exception exc)
            {
                return null;
            }
            return rv;
        }

        [WebMethod(EnableSession = true)]
        public static bool mergiLaPlata(){
            bool rv = false;
            try
            {
                string vineDeLaCheckOut = "true";
                HttpContext.Current.Session["vineDeLaCheckOut"] = vineDeLaCheckOut.ToString();
                rv = true;
            }catch(Exception exc){
                rv = false;
            }
            return rv;
        }

        [WebMethod(EnableSession = true)]
        public static String getTermeniConditiiBaza()
        {
            String rv = "";
            try
            {
                using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                {
                    try
                    {

                        cnn.Open();
                        string sql = @"SELECT [ID]
                                              ,[Denumire]
                                              ,[Valoare]
                                              ,[IdHotel]
                                              ,[IdComputer]
                                              ,[TipSetare]
                                          FROM [SmartH].[dbo].[Setari]
                                          WHERE ID=24;";
                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                rv = reader["Valoare"] == DBNull.Value ? "" : reader["Valoare"].ToString();
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        return null;
                    }
                }
            }catch(Exception exc){
                rv = "";
            }
            return rv;
        }
    }
}