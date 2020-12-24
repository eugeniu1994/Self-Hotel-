using AegisImplicitMail;
using SelfHotel.Nomenclatoare;
using SelfHotel.Nomenclatoare_Final;
using SelfHotel.NomenclatoareNew;
using SelfHotel.Setari;
using SolonCloud.Reporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using ZipEscort.Setari;

namespace SelfHotel.GUI.CHECKOUT
{
    public partial class PlataForm2 : System.Web.UI.Page
    {
        public static Boolean vineDePeEuPlatesc { get; set; }
        public static List<EntitateMetodaPlata> metodePlata = new List<EntitateMetodaPlata>();
        public static List<EntitateTipDoc> tipuriDoc = new List<EntitateTipDoc>();
        public static List<EntitateServiciu> entitateServiciiLista = new List<EntitateServiciu>();
        public static List<EntitateCota> listaCote = new List<EntitateCota>();
        public static Dictionary<int, EntitateDePlata> RamasDeRepartizat = new Dictionary<int, EntitateDePlata>();
        public static Decimal TotalDePlata { get; set; }
        public static List<EntitatePlata> listaPlati;// = new List<EntitatePlata>();
        public static List<RezervariCamere> RezervariCamereLista { get; set; }
        public static String DeUndeVine { get; set; }
        public static String IdFacturaGenerata { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Request.QueryString["paginaHome"] != null) //pentru GET 
            //{
            //    String paginaHome = Convert.ToString(Request["paginaHome"]);
            //}
            //if (Request["paginaHome"] != null) //pentru POST 
            //{
            //    String paginaHome = Convert.ToString(Request["paginaHome"]);
            //}

            //ConexiuneDB.SerieNefiscal = HelperCurs.GetSerieNefiscal();
            //ConexiuneDB.DocNr = HelperCurs.LoadNumar(ConexiuneDB.SerieNefiscal);
            //ConexiuneDB.moneda = EntitateMoneda.GetMonedaNationala();
            //List<DataLucruObj> listdDataLucru = DataLucruObj.GetLista().Where(x => x.IdHotel == ConexiuneDB.IdHotel).ToList();
            //DateTime DataL;
            //if (listdDataLucru.Count > 0)
            //{
            //    DataL = listdDataLucru[0].DataLucru;
            //}
            //else
            //{
            //    DataL = DateTime.Today;
            //}
            //ConexiuneDB.DataLucr = DataL;

            ConexiuneDB.initializare();//incarc setari aici

            //verifica daca vine de la euplatesc
            string actual = "";
            try
            {
                Uri myReferrer = Request.UrlReferrer;
                actual = myReferrer.ToString();
                DeUndeVine = actual;
            }
            catch (Exception exc) { LogErori.Salveaza(exc, "CheckIn2.Page_Load()"); }
            //actual = "asdasd.euplatesc.asasd";

            if (actual.Contains("euplatesc"))
            {
                TotalDePlata = 0;
                String IdRezervare = HttpContext.Current.Session["IdRezervare"] as String;
                String codRezervare = HttpContext.Current.Session["codRezervare"] as String;
                try
                {
                    if (!String.IsNullOrEmpty(IdRezervare) && IdRezervare != "undefined")
                    {
                        LoadData(0, "CRD");
                        RezervariCamereLista = RezervariCamere.GetLista(Convert.ToInt32(IdRezervare));
                        List<EntitateServiciu> listaServicii = LoadServicii(Convert.ToInt32(IdRezervare));
                        foreach (RezervariCamere rc in RezervariCamereLista)
                        {
                            rc.entitateServiciiLista = listaServicii.Where(x => x.IdRezervareCamera == rc.ID).ToList();
                            LoadPlati(Convert.ToInt32(IdRezervare), rc.entitateServiciiLista);
                            foreach (EntitateServiciu es in rc.entitateServiciiLista)
                            {
                                TotalDePlata += es.SoldRON;
                            }
                        }
                        IncarcaCarnet(RezervariCamereLista[0].IdTurist, listaServicii.Select(x => x.ID).ToList());
                    }
                    else
                    {
                        return;//a expirat sesiunea
                    }
                }
                catch (Exception exc)
                { }

                try
                {
                    List<EntitateTipDoc> EntitateTipDocListaNefiscal = EntitateTipDoc.GetLista().Where(x => x.Cod.ToLower() == "nf").ToList();
                    if (EntitateTipDocListaNefiscal.Count > 0)
                    {
                        ConexiuneDB.tipDoc = EntitateTipDocListaNefiscal[0];
                    }
                }
                catch (Exception exc) { LogErori.Salveaza(exc, "CheckIn2.Page_Load()"); }

                string amount = "";
                string curr = "";
                string invoice_id = "";
                string ep_id = "";
                string merch_id = "";
                string action = "";
                string message = "";
                string approval = "";
                string timestamp = "";
                string ExtraData = "";
                string nonce = "";

                if (Request["amount"] != null)
                {
                    amount = Convert.ToString(Request["amount"]);
                }
                if (Request["curr"] != null)
                {
                    curr = Convert.ToString(Request["curr"]);
                }
                if (Request["invoice_id"] != null)
                {
                    invoice_id = Convert.ToString(Request["invoice_id"]);
                }
                if (Request["ep_id"] != null)
                {
                    ep_id = Convert.ToString(Request["ep_id"]);
                }
                if (Request["merch_id"] != null)
                {
                    merch_id = Convert.ToString(Request["merch_id"]);
                }
                if (Request["action"] != null)
                {
                    action = Convert.ToString(Request["action"]);
                }
                if (Request["message"] != null)
                {
                    message = Convert.ToString(Request["message"]);
                }
                if (Request["approval"] != null)
                {
                    approval = Convert.ToString(Request["approval"]);
                }
                if (Request["timestamp"] != null)
                {
                    timestamp = Convert.ToString(Request["timestamp"]);
                }
                if (Request["nonce"] != null)
                {
                    nonce = Convert.ToString(Request["nonce"]);
                }
                if (Request["ExtraData "] != null)
                {
                    ExtraData = Convert.ToString(Request["ExtraData "]);
                }
                if (invoice_id == "")
                {
                    //afiseaza mesaj ca plata a fost eronata
                    //return;
                    invoice_id = "28";
                    action = "0";
                }
                if (String.IsNullOrEmpty(action))
                {
                    //afiseaza mesaj ca plata a fost eronata
                    return;
                }
                String IdPartener = HttpContext.Current.Session["IdPartener"] as String;
                if (string.IsNullOrEmpty(IdPartener))
                {
                    int tmp = 0;
                    Int32.TryParse(RezervariCamereLista[0].IdTurist.ToString(), out tmp);
                    IdPartener = tmp+"";
                }
                EntitateTipDoc tipDoc = ConexiuneDB.tipDoc;
                EntitateMoneda moneda = ConexiuneDB.moneda;
                string ccPosesor = "";
                string txtCUI = "";
                decimal totalRON = TotalDePlata;// Convert.ToDecimal(amount);
                bool ok=achita(Convert.ToInt32(IdRezervare), Convert.ToInt32(IdPartener), tipDoc, moneda, totalRON, totalRON, ccPosesor, txtCUI);
                if (ok)
                {
                    vineDePeEuPlatesc = true;
                }
                else
                {
                    vineDePeEuPlatesc = false;
                }
                if (ok)
                {
                    try
                    {
                        //LoadData(0, "NUM");
                        //List<EntitateServiciu> listaServicii = LoadServicii(Convert.ToInt32(IdRezervare));
                        //LoadPlati(Convert.ToInt32(IdRezervare), listaServicii);
                        List<int> idPlati = listaPlati.Select(x => x.ID).ToList();
                        if (idPlati.Count > 0)
                        {
                            LoadServiciiPlata(idPlati);
                        }
                        String serieFactura = ConexiuneDB.serieFactura;
                        try
                        {
                            
                            Facturi last = Facturi.getLastNumar(ConexiuneDB.IdHotel, ConexiuneDB.Carnet.ID);
                            int lastNumar = 0;
                            Int32.TryParse(last.Numar, out lastNumar);
                            ConexiuneDB.numarFactura = (lastNumar + 1).ToString();
                        }
                        catch (Exception exc) { }
                        String numarFactura = ConexiuneDB.numarFactura;
                        SalveazaFactura(false, Convert.ToInt32(IdPartener), serieFactura, numarFactura, Convert.ToInt32(IdRezervare));
                    }
                    catch (Exception exc) { }
                }
            }
            else
            {
                try
                {
                    vineDePeEuPlatesc = false;
                    List<EntitateTipDoc> EntitateTipDocListaNefiscal = EntitateTipDoc.GetLista().Where(x => x.Cod.ToLower() == "bf").ToList();
                    if (EntitateTipDocListaNefiscal.Count > 0)
                    {
                        ConexiuneDB.tipDoc = EntitateTipDocListaNefiscal[0];
                    }
                }
                catch (Exception exc) { LogErori.Salveaza(exc, "CheckIn2.Page_Load()"); }
            }
        }

        public class RezervariCamereTuple
        {
            public List<RezervariCamere> lista { get; set; }
            public int status { get; set; }
            public List<Pozitie> pozitiiCamera { get; set; }
            public string datalucru { get; set; }
            public string serieFactura { get; set; }
            public string numarFactura { get; set; }
            public string obsFactura { get; set; }
            public string Transport { get; set; }
            public string TermenPlata { get; set; }
        }

        public class PlataCashTuple
        {
            public List<RezervariCamere> lista { get; set; }
            public Decimal Suma { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static RezervariCamereTuple getRezervare(string CodRezervareTmp)
        {
            RezervariCamereTuple rezultat = new RezervariCamereTuple();
            TotalDePlata = 0;
            HttpContext.Current.Session.Clear();
            List<RezervariCamere> rv = new List<RezervariCamere>();
            try
            {
                rezultat.datalucru = ConexiuneDB.DataLucr.ToShortDateString();
                HttpContext.Current.Session["IdRezervare"] = null;
                HttpContext.Current.Session["codRezervare"] = null;
                Rezervari rezervare = Rezervari.GetRezervare(CodRezervareTmp.Trim());
                if (rezervare == null)
                {
                    rezultat.status = 0;//nu exista rezervarea
                    return rezultat;
                }
                HttpContext.Current.Session["IdRezervare"] = rezervare.ID.ToString();
                HttpContext.Current.Session["codRezervare"] = rezervare.CodRezervare.Trim();
                if (!rezervare.Cazat)
                {
                    rezultat.status = 1;//rezervare nu este cazata
                    return rezultat;
                }
                rv = RezervariCamere.GetLista(rezervare.ID);
                foreach (RezervariCamere rc in rv)
                {
                    if (rc.Iesit)
                    {
                        rezultat.status = 5; //rezervarea este iesita
                        return rezultat;
                    }
                }
                
                if (vineDePeEuPlatesc)
                {
                    LoadData(0, "CRD");
                    RezervariCamereLista = RezervariCamere.GetLista(rezervare.ID);
                    List<EntitateServiciu> listaServicii = LoadServicii(rezervare.ID);
                    try
                    {
                        calculSoldCazare(rezervare.ID);
                    }
                    catch { }
                    try
                    {
                        List<int> idPlati = listaPlati.Select(x => x.ID).ToList();
                        if (idPlati.Count > 0)
                        {
                            LoadServiciiPlata(idPlati);
                            rezultat.pozitiiCamera = pozitii;
                        }
                    }
                    catch { }
                    IncarcaCarnet(rv[0].IdTurist, listaServicii.Select(x => x.ID).ToList());
                    foreach (RezervariCamere rc in RezervariCamereLista)
                    {
                        rc.entitateServiciiLista = listaServicii.Where(x => x.IdRezervareCamera == rc.ID).ToList();
                        LoadPlati(rezervare.ID, rc.entitateServiciiLista);
                        foreach (EntitateServiciu es in rc.entitateServiciiLista)
                        {
                            TotalDePlata += es.SoldRON;
                        }
                        if (TotalDePlata > 0)
                        {
                            rezultat.status = 3;//sunt servicii neachitate
                        }
                        else
                        {
                            rezultat.status = 2;//serviciile sunt achitate
                        }
                        if (TotalDePlata > 0)
                        {
                            rezultat.status = 3;//sunt servicii neachitate
                            try
                            {
                                Facturi last = Facturi.getLastNumar(ConexiuneDB.IdHotel, ConexiuneDB.Carnet.ID);
                                int lastNumar = 0;
                                HttpContext.Current.Session["IdPartener"] = rv[0].IdTurist + "";
                                Int32.TryParse(last.Numar, out lastNumar);
                                ConexiuneDB.numarFactura = (lastNumar + 1).ToString();
                            }
                            catch { }
                        }
                        else
                        {
                            rezultat.status = 2;//serviciile sunt achitate
                            //preia factura existenta deja
                            try
                            {
                                List<Facturi> listaFacturi = Facturi.GetLista();
                                listaFacturi = listaFacturi.Where(x => x.IdHotel == ConexiuneDB.IdHotel && x.IdFurnizor == ConexiuneDB.Carnet.IdFurnizor && x.IdClient == rv[0].IdTurist && x.Serie == ConexiuneDB.serieFactura).ToList();
                                Facturi tmp = listaFacturi.FirstOrDefault();
                                HttpContext.Current.Session["IdPartener"] = rv[0].IdTurist + "";
                                if (tmp != null)
                                {
                                    ConexiuneDB.numarFactura = tmp.Numar;
                                }
                            }
                            catch (Exception exc) { }
                        }
                    }
                    
                    HttpContext.Current.Session["IdRezervare"] = rezervare.ID.ToString();
                    HttpContext.Current.Session["codRezervare"] = rezervare.CodRezervare.Trim();
                    rezultat.lista = RezervariCamereLista;
                    vineDePeEuPlatesc = false;
                }
                else
                {
                    RezervariCamereLista = null;
                   
                    LoadData(0, "NUM");
                    List<EntitateServiciu> listaServicii = LoadServicii(rezervare.ID);
                    LoadPlati(rezervare.ID, listaServicii);
                    try
                    {
                        calculSoldCazare(rezervare.ID);
                    }
                    catch { }
                    try
                    {
                        List<int> idPlati = listaPlati.Select(x => x.ID).ToList();
                        if (idPlati.Count > 0)
                        {
                            LoadServiciiPlata(idPlati);
                            rezultat.pozitiiCamera = pozitii;
                        }
                    }
                    catch { }
                    IncarcaCarnet(rv[0].IdTurist, listaServicii.Select(x => x.ID).ToList());
                    foreach (RezervariCamere rc in rv)
                    {
                        rc.entitateServiciiLista = listaServicii.Where(x => x.IdRezervareCamera == rc.ID).ToList();
                        
                        //LoadPlati(rezervare.ID, rc.entitateServiciiLista);
                        foreach (EntitateServiciu es in rc.entitateServiciiLista)
                        {
                            TotalDePlata += es.SoldRON;
                        }
                    }
                    if (TotalDePlata > 0)
                    {
                        rezultat.status = 3;//sunt servicii neachitate
                        try
                        {
                            Facturi last = Facturi.getLastNumar(ConexiuneDB.IdHotel, ConexiuneDB.Carnet.ID);
                            int lastNumar = 0;
                            Int32.TryParse(last.Numar, out lastNumar);
                            ConexiuneDB.numarFactura = (lastNumar + 1).ToString();
                        }
                        catch { }
                    }
                    else
                    {
                        rezultat.status = 2;//serviciile sunt achitate
                        //preia factura existenta deja
                        try
                        {
                            List<Facturi> listaFacturi = Facturi.GetLista();
                            listaFacturi = listaFacturi.Where(x => x.IdHotel == ConexiuneDB.IdHotel && x.IdFurnizor == ConexiuneDB.Carnet.IdFurnizor && x.IdClient == rv[0].IdTurist && x.Serie == ConexiuneDB.serieFactura).ToList();
                            Facturi tmp = listaFacturi.FirstOrDefault();
                            HttpContext.Current.Session["IdPartener"] = rv[0].IdTurist + "";
                            if (tmp != null)
                            {
                                ConexiuneDB.numarFactura = tmp.Numar;
                            }
                        }
                        catch (Exception exc) { }
                    }
                    
                    HttpContext.Current.Session["IdRezervare"] = rezervare.ID.ToString();
                    HttpContext.Current.Session["codRezervare"] = rezervare.CodRezervare.Trim();
                    rezultat.lista = rv;
                    RezervariCamereLista = rv;
                }
                Jurnal.Salveaza(
                            "SelfHotel.GUI.CHECKOUT.PlataForm2",
                            Jurnal.Actiune.VIZ,
                            Jurnal.ImportantaMare,
                            string.Format("Cauta rezervare: ID={0} Cod={1}, getRezervare()", rezervare.ID, rezervare.CodRezervare));
            }
            catch (Exception exc)
            {
                LogErori.Salveaza(exc, "CheckIn2.getRezervare()");
                rezultat.status = 4;//eroare necunoascuta
                return rezultat;
            }

            rezultat.serieFactura = ConexiuneDB.serieFactura;
            rezultat.numarFactura = ConexiuneDB.numarFactura;
            rezultat.obsFactura = ConexiuneDB.obsFactura;
            rezultat.Transport = ConexiuneDB.Transport;
            rezultat.TermenPlata = ConexiuneDB.TermenPlata.ToShortDateString();
            return rezultat;
        }

        [WebMethod(EnableSession = true)]
        public static NomParteneri getDetaliiClient(string id)
        {
            NomParteneri rv = new NomParteneri();
            try
            {
                Int32 idPartener = 0;
                Int32.TryParse(id, out idPartener);
                rv = NomParteneri.GetLista(idPartener);
                if (rv == null)
                {
                    rv = new NomParteneri();
                }
            }
            catch (Exception exc)
            {

            }
            return rv;
        }

        [WebMethod(EnableSession = true)]
        public static NomParteneri getDetaliiFurnizor()
        {
            NomParteneri rv = new NomParteneri();
            try
            {
                //EntitateCarnetFacturi ecf = EntitateCarnetFacturi.getCarnet();
                rv = NomParteneri.GetLista(ConexiuneDB.Carnet.IdFurnizor);
            }
            catch (Exception exc)
            {

            }
            return rv;
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
                    LogErori.Salveaza(exc, "PlataForm2.calculSoldCazare()");
                    return -1;
                }
            }
            return rv;
        }

        private static void LoadData(int IdTurist, string Cod)
        {
            metodePlata = new List<EntitateMetodaPlata>();
            tipuriDoc = new List<EntitateTipDoc>();

            listaCote = new List<EntitateCota>();

            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT
                                         [ID]
                                        ,[Cod]
                                        ,[Denumire]
                                        ,[GrupareID]
                                        ,[EsteNumerar]
                                        ,[EsteCardBancar]
                                        ,[EsteOP]
                                    FROM [SOLON.H].[financiar].[MetodeDePlata] WHERE [Sters] = 0 and  [Cod]=@Cod ORDER BY [Ordine]
                                  ";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@Cod", SqlDbType.VarChar)).Value = Cod;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            metodePlata.Add(new EntitateMetodaPlata()
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Cod = reader["Cod"].ToString(),
                                Denumire = reader["Denumire"].ToString(),
                                GrupareID = Convert.ToInt32(reader["GrupareID"]),
                                EsteNumerar = Convert.ToBoolean(reader["EsteNumerar"]),
                                EsteCardBancar = Convert.ToBoolean(reader["EsteCardBancar"]),
                                EsteOP = Convert.ToBoolean(reader["EsteOP"]),
                                Selected = true
                            });
                        }
                    }

                    sql = @"
                        SELECT
                             [DataIni]
                            ,[DataFin]
                            ,[IdCota]
                            ,[Denumire]
                            ,[Procent]
                        FROM [SOLON].[dbo].[CoteTva]
                    ";
                    cmd = new SqlCommand(sql, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        EntitateCota inst;
                        while (reader.Read())
                        {
                            inst = new EntitateCota();
                            inst.DataIni = Convert.ToDateTime(reader["DataIni"]);
                            inst.DataFin = Convert.ToDateTime(reader["DataFin"]);
                            inst.IdCota = Convert.ToInt32(reader["IdCota"]);
                            inst.Denumire = reader["Denumire"].ToString();
                            inst.Procent = Convert.ToDecimal(reader["Procent"]);
                            listaCote.Add(inst);
                        }
                    }
                    string NumeTurist = "";
                    if (IdTurist != 0)
                    {
                        sql = @"SELECT [NumePartener] FROM [SOLON].[dbo].[NomParteneri] WHERE [IdPartener] = @IdPartener";
                        cmd = new SqlCommand(sql, cnn);
                        cmd.Parameters.Add(new SqlParameter("@IdPartener", SqlDbType.BigInt)).Value = IdTurist;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                NumeTurist = reader[0].ToString();
                            }
                        }
                    }

                    sql = @"
                        SELECT
                             [ID]
                            ,[Cod]
                            ,[Denumire]
                            ,[Descriere]
                            ,[Fiscal]
                            ,[EsteChitanta]
                            ,[Sters]
                            ,[EsteDispozitie]
                            ,[EsteOP]
                        FROM [SOLON.H].[financiar].[TipDocumentPlata]
                        WHERE Sters = 0 
                    ";

                    cmd = new SqlCommand(sql, cnn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tipuriDoc.Add(new EntitateTipDoc()
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Cod = reader["Cod"].ToString(),
                                Denumire = reader["Denumire"].ToString(),
                                Descriere = reader["Descriere"].ToString(),
                                Fiscal = Convert.ToBoolean(reader["Fiscal"]),
                                EsteChitanta = Convert.ToBoolean(reader["EsteChitanta"]),
                                EsteDispozitie = Convert.ToBoolean(reader["EsteDispozitie"]),
                                EsteOP = Convert.ToBoolean(reader["EsteOP"])
                            });
                        }
                    }
                    Jurnal.Salveaza(
                            "SelfHotel.GUI.CHECKOUT.PlataForm2",
                            Jurnal.Actiune.VIZ,
                            Jurnal.ImportantaMare,
                            string.Format("Incarca date :cu  IdTurist={0} Cod={1}, LoadData()", IdTurist, Cod));
                }
                catch (Exception exc)
                {
                    LogErori.Salveaza(exc, "CheckIn2.LoadData()");
                }
            }
        }

        private static void LoadPlati(int IdRezervare, List<EntitateServiciu> lista)
        {
            listaPlati = new List<EntitatePlata>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string IdsServicii = "";
                    List<int> idServos = new List<int>();

                    string sql = @"SELECT DISTINCT rs.ID
                    FROM [SOLON.H].[hotel].[Rezervari] as r left outer join [SOLON.H].[hotel].[RezervariCamere] as rc on rc.IdRezervare=r.ID
                    left outer join [SOLON.H].[hotel].[RezervariServicii] as rs on rs.IdRezervareCamera = rc.ID
                    WHERE r.IdHotel=@IdHotel AND r.Sters=0 and rc.Sters=0 and r.ID=@IdRezervare";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.Int)).Value = ConexiuneDB.IdHotel;
                    cmd.Parameters.Add(new SqlParameter("@IdRezervare", SqlDbType.Int)).Value = IdRezervare;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        EntitateServiciu serv;
                        EntitateCota ec;
                        while (reader.Read())
                        {
                            idServos.Add(Convert.ToInt32(reader["ID"]));
                        }
                    }
                    IdsServicii = idServos.Aggregate("-1", (i, j) => i + "," + j);


                    sql = @"
                        SELECT
                             s.[ID] AS sID
                            ,ps.[ID] AS psID
                            ,ps.[ValoareMoneda] AS psValoareMoneda
                            ,ps.[ValoareRON] AS psValoareRON
                            ,ps.[ValoareMonedaServ] AS psValoareMonedaServ
                            ,p.[ID] AS pID
                            ,p.[IdHotel] AS pIdHotel
                            ,p.[IdTipDocPlata] AS pIdTipDocPlata
                            ,p.[NumarIntern] AS pNumarIntern
                            ,p.[DocNr] AS pDocNr
                            ,p.[DocSeria] AS pDocSeria
                            ,p.[DocData] AS pDocData
                            ,p.[IdPartener] AS pIdPartener
                            ,p.[IdMoneda] AS pIdMoneda
                            ,p.[CursZi] AS pCursZi
                            ,p.[ValoareMoneda] AS pValoareMoneda
                            ,p.[ValoareRON] AS pValoareRON
                            ,p.[DataLucru] AS pDataLucru
                            ,p.[DataOraCreare] AS pDataOraCreare
                            ,p.[IdUser] AS pIdUser
                            ,p.[Sters] AS pSters
                            ,pm.[ID] AS pmID
                            ,pm.[IdMetodaPlata] AS pmIdMetodaPlata
                            ,pm.[ValoareMoneda] AS pmValoareMoneda
                            ,pm.[ValoareRON] AS pmValoareRON
                            ,m.[Cod] AS mCod
                            ,t.[Cod] AS tCod
                            ,t.[Fiscal] AS tFiscal
                            ,mon.[MonedaCod] AS monCod
                        FROM 
                            [SOLON.H].[hotel].[RezervariServicii] AS s INNER JOIN
                            [SOLON.H].[financiar].[PlatiServicii] AS ps ON ps.IdRezervareServiciu = s.ID INNER JOIN
                            [SOLON.H].[financiar].[Plati] AS p ON p.ID = ps.IdPlata INNER JOIN
                            [SOLON.H].[financiar].[PlatiMetodePlata] AS pm ON pm.IdPlata = p.ID INNER JOIN
                            [SOLON.H].[financiar].[MetodeDePlata] AS m ON m.ID = pm.IdMetodaPlata INNER JOIN
                            [SOLON.H].[financiar].[TipDocumentPlata] AS t ON t.ID = p.IdTipDocPlata INNER JOIN
                            [SOLON].[dbo].[NomMonede] AS mon ON mon.IdMoneda = p.IdMoneda
                        WHERE s.[ID] IN (<lista_ids_servicii>) AND p.[Sters] = 0
                    ";
                    sql = sql.Replace("<lista_ids_servicii>", IdsServicii);
                    cmd = new SqlCommand(sql, cnn);
                    //cmd.Parameters.Add(new SqlParameter("@IdRezervareCamera", SqlDbType.Int)).Value = IdRezervareCamera;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        EntitatePlata plata = null;
                        EntitatePlataServiciu ps;
                        EntitatePlataMetodaPlata pm;
                        HashSet<int> hsPlati = new HashSet<int>();
                        HashSet<int> hsServPlati = new HashSet<int>();
                        HashSet<int> hsMetode = new HashSet<int>();
                        int id;
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["pID"]);
                            if (!hsPlati.Contains(id))
                            {
                                plata = new EntitatePlata()
                                {
                                    ID = id,
                                    IdHotel = Convert.ToInt32(reader["pIdHotel"]),
                                    IdTipDocPlata = Convert.ToInt32(reader["pIdTipDocPlata"]),
                                    CodTipDoc = reader["tCod"].ToString(),
                                    NumarIntern = Convert.ToInt32(reader["pNumarIntern"]),
                                    DocNr = reader["pDocNr"] == DBNull.Value ? "" : reader["pDocNr"].ToString(),
                                    DocSeria = reader["pDocSeria"] == DBNull.Value ? "" : reader["pDocSeria"].ToString(),
                                    DocData = reader["pDocData"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["pDocData"]),
                                    IdPartener = Convert.ToInt32(reader["pIdPartener"]),
                                    IdMoneda = Convert.ToInt32(reader["pIdMoneda"]),
                                    CodMoneda = reader["monCod"].ToString(),
                                    CursZi = Convert.ToDecimal(reader["pCursZi"]),
                                    ValoareMoneda = Convert.ToDecimal(reader["pValoareMoneda"]),
                                    ValoareRON = Convert.ToDecimal(reader["pValoareRON"]),
                                    DataLucru = Convert.ToDateTime(reader["pDataLucru"]),
                                    DataOraCreare = Convert.ToDateTime(reader["pDataOraCreare"]),
                                    IdUser = Convert.ToInt32(reader["pIdUser"]),
                                    Sters = Convert.ToBoolean(reader["pSters"]),
                                    EsteFiscal = Convert.ToBoolean(reader["tFiscal"])
                                };
                                listaPlati.Add(plata);
                                hsPlati.Add(id);
                            }
                            else
                            {
                                plata = listaPlati.FirstOrDefault(x => x.ID == id);
                            }
                            id = Convert.ToInt32(reader["psID"]);
                            if (!hsServPlati.Contains(id))
                            {
                                ps = new EntitatePlataServiciu()
                                {
                                    ID = id,
                                    IdPlata = plata.ID,
                                    IdRezervareServiciu = Convert.ToInt32(reader["sID"]),
                                    ValoareMoneda = Convert.ToDecimal(reader["psValoareMoneda"]),
                                    ValoareMonedaServ = Convert.ToDecimal(reader["psValoareMonedaServ"]),
                                    ValoareRON = Convert.ToDecimal(reader["psValoareRON"])
                                };
                                plata.PlatiServicii.Add(ps);
                                hsServPlati.Add(id);
                            }
                            id = Convert.ToInt32(reader["pmID"]);
                            if (!hsMetode.Contains(id))
                            {
                                pm = new EntitatePlataMetodaPlata()
                                {
                                    ID = id,
                                    IdMetodaPlata = Convert.ToInt32(reader["pmIdMetodaPlata"]),
                                    IdPlata = plata.ID,
                                    ValoareMoneda = Convert.ToDecimal(reader["pmValoareMoneda"]),
                                    ValoareRON = Convert.ToDecimal(reader["pmValoareRON"]),
                                    CodMetoda = reader["mCod"].ToString()
                                };
                                plata.PlatiMetode.Add(pm);
                                hsMetode.Add(id);
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    LogErori.Salveaza(exc, "CheckIn2.LoadPlati()");
                }
            }

            EntitatePlataServiciu eps;
            EntitateMoneda em, emRON = ConexiuneDB.moneda;  // monede.FirstOrDefault(x => x.EsteNationala);
            foreach (EntitateServiciu es in lista.Where(x => !x.Sters))
            {
                //em = monede.FirstOrDefault(x => x.ID == es.IdMoneda);
                foreach (EntitatePlata ep in listaPlati.Where(x => !x.Sters))
                {
                    eps = ep.PlatiServicii.FirstOrDefault(x => x.IdRezervareServiciu == es.ID);
                    if (eps != null)
                    {
                        es.PlatitMoneda += eps.ValoareMonedaServ;
                        es.PlatitRon += eps.ValoareRON;
                    }
                }

                es.SoldMoneda = es.ValoareMoneda - es.PlatitMoneda;
                es.SoldCurs = es.Curs;
                //es.SoldRON = es.ValoareRon - Math.Round(es.PlatitMoneda * es.Curs,2);
                es.SoldRON = es.ValoareRon - es.PlatitRon;

                //if (PlataFactura)
                //{
                //    es.DePlataMonedaPlata = es.DePlataRON;
                //    es.DePlataCurs = 1;
                //    es.DePlataIdMoneda = emRON.ID;
                //}
                ////else if (EsteRestituire)
                ////{
                ////    es.DePlataMonedaPlata = es.PlatitRon;
                ////    es.DePlataCurs = 1;
                ////    es.DePlataIdMoneda = emRON.ID;
                ////}
                //else
                //{
                es.DePlataMonedaServ = es.SoldMoneda;
                es.DePlataRON = es.SoldRON;
                es.DePlataMonedaPlata = es.SoldRON;
                es.DePlataCurs = 1;
                es.DePlataIdMoneda = emRON.ID;
                // }


                //if (STR_PlatiPartialeCuAvansuri)
                //{
                //    es.ValoareStornouriMonedaPLT = 0.0m;
                //    es.ValoareStornouriMonedaSRV = 0.0m;
                //    es.ValoareStornouriMonedaRON = 0.0m;
                //    foreach (FormFisa.EntitateServiciu ss in es.ServiciiStornoAvans)
                //    {
                //        ss.CalculTotal();

                //        es.SoldMoneda += ss.ValoareMoneda;
                //        es.SoldRON += ss.ValoareRon;

                //        es.DePlataMonedaServ += ss.ValoareMoneda;
                //        es.DePlataRON += ss.ValoareRon;
                //        es.DePlataMonedaPlata += ss.ValoareRon;

                //        es.ValoareStornouriMonedaSRV += ss.ValoareMoneda;
                //        es.ValoareStornouriMonedaRON += ss.ValoareRon;
                //        es.ValoareStornouriMonedaPLT += ss.ValoareRon;
                //    }
                //}
            }
            Jurnal.Salveaza(
                            "SelfHotel.GUI.CHECKOUT.PlataForm2",
                            Jurnal.Actiune.VIZ,
                            Jurnal.ImportantaMare,
                            string.Format("Incarca plati pentru rezervarea cu  ID={0}, LoadPlati()", IdRezervare));
        }

        [WebMethod(EnableSession = true)]
        public static int checkOutClick(bool iesireAnticipata)
        {
            int rv = -1;
            if (TotalDePlata <= 0)
            {
                String IdRezervare = HttpContext.Current.Session["IdRezervare"] as String;
                String codRezervare = HttpContext.Current.Session["codRezervare"] as String;
            
                if (!String.IsNullOrEmpty(IdRezervare) && IdRezervare != "undefined")
                {
                    DateTime InitialPlecare, InitialSosire;
                    Rezervari rezervare = Rezervari.GetRezervare(Convert.ToInt32(IdRezervare));
                    if (rezervare != null)
                    {
                        InitialPlecare = rezervare.Plecare;
                        InitialSosire = rezervare.Sosire;
                    }
                    else
                    {
                        return -1;
                    }

                    if (!iesireAnticipata) //sigur doriti sa faceti iesire anticipata ? ? ?
                    {
                         if (InitialPlecare > ConexiuneDB.DataLucr)
                        {
                            return 2;
                        }
                    }

                        #region "iesire anticipata"

                        if (InitialPlecare > ConexiuneDB.DataLucr)
                        {
                            //if (txtSoldRec.ValoareNumerica > 0 || txtSoldVir.ValoareNumerica > 0)
                            //{
                            //    StringBuilder sb = new StringBuilder();
                            //    sb.AppendFormat("Cazarea FOLIO {0} are sume restante de plata:{1}", IdRezervareCamera, Environment.NewLine);
                            //    sb.AppendFormat("    REC: {0} RON{1}", txtSoldRec.Text, Environment.NewLine);
                            //    sb.AppendFormat("    VIR: {0} RON{1}", txtSoldVir.Text, Environment.NewLine);

                            //    if (Setare.GetNuPermiteChekoutCuServiciiNeplatiteRec() && txtSoldRec.ValoareNumerica > 0)
                            //    {
                            //        sb.Append("Nu se poate efectua iesirea cazarii cu sold (REC)!");
                            //        Mesaj.Warning(sb.ToString());
                            //        return;
                            //    }
                            //    else if (Setare.GetNuPermiteChekoutCuServiciiNeplatiteVir() && txtSoldVir.ValoareNumerica > 0)
                            //    {
                            //        sb.Append("Nu se poate efectua iesirea cazarii cu sold (VIR)!");
                            //        Mesaj.Warning(sb.ToString());
                            //        return;
                            //    }
                            //    sb.Append("Sunteti sigur ca doriti sa faceti iesirea?");
                            //    if (!Mesaj.Confirmare(sb.ToString()))
                            //    {
                            //        return;
                            //    }
                            //    iesireCuSold = true;
                            //}

                            //if (!Mesaj.Confirmare("Doriti trunchierea serviciilor cu valori a caror data este viitoare datei de lucru?"))
                            //{
                            //    foreach (EntitateServiciu serv in serviciiReceptie.Union(serviciiVirament).Where(x => !x.Sters))
                            //    {
                            //        if (serv.Valori.Count(x => x.Data > LoginInfo.DataLucru) > 0)
                            //        {
                            //            EntitateServiciuValoare esvDataLucru = serv.Valori.FirstOrDefault(x => x.Data == LoginInfo.DataLucru);
                            //            if (esvDataLucru == null)
                            //            {
                            //                esvDataLucru = new EntitateServiciuValoare() { Data = LoginInfo.DataLucru, Valoare = 0 };
                            //                serv.Valori.Add(esvDataLucru);
                            //            }
                            //            esvDataLucru.Valoare += serv.Valori.Where(x => x.Data > LoginInfo.DataLucru).Sum(x => x.Valoare);
                            //            serv.Valori.RemoveAll(x => x.Data > LoginInfo.DataLucru);
                            //        }
                            //        serv.CalculTotal();
                            //    }
                            //}
                            //else
                            //{
                            //    /*foreach (EntitateServiciu serv in serviciiReceptie.Union(serviciiVirament).Where(x => !x.Sters))
                            //    {
                            //        serv.Valori.RemoveAll(x => x.Data > LoginInfo.DataLucru);
                            //        serv.CalculTotal();
                            //    }*/
                            //    foreach (EntitateServiciu serv in serviciiReceptie.Union(serviciiVirament).Where(x => !x.Sters && x.IdRezervareCameraMutat == 0))
                            //    {
                            //        if (serv.Valori.Count() == 1)
                            //        {
                            //            continue;
                            //        }

                            //        //serv.Valori.RemoveAll(x => x.Data >= LoginInfo.DataLucru);

                            //        List<EntitateServiciuValoare> ListaValori = new List<EntitateServiciuValoare>();
                            //        foreach (EntitateServiciuValoare esv in serv.Valori)
                            //        {
                            //            ListaValori.Add(esv);
                            //        }
                            //        foreach (EntitateServiciuValoare esv in ListaValori)
                            //        {
                            //            DateTime ziuaCurenta = esv.Data;
                            //            decimal PlataValoareZilnica = HelperCazare.GetPlataValoareZilnica(serv.ID, ziuaCurenta);
                            //            decimal FacturaValoareZilnica = HelperCazare.GetFacturaValoareZilnica(serv.ID, ziuaCurenta);

                            //            if (esv.Data >= LoginInfo.DataLucru && PlataValoareZilnica == 0 && FacturaValoareZilnica == 0)
                            //            {
                            //                serv.Valori.Remove(esv);
                            //            }
                            //        }

                            //        serv.CalculTotal();
                            //    }
                            //}

                            //IncarcaValoriZilnice();
                            //SalveazaServicii();

                            //SalveazaFisa();

                            //IESIRE + CURATARE CAMERA
                            List<RezervariCamere> rezCams = RezervariCamere.GetLista(Convert.ToInt32(IdRezervare));
                            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                            {
                                try
                                {
                                    cnn.Open();
                                    using (SqlTransaction tran = cnn.BeginTransaction())
                                    {
                                        try
                                        {
                                            foreach (RezervariCamere rc in rezCams)
                                            {
                                                string sql = @"UPDATE [SOLON.H].[hotel].[RezervariCamere] SET [Iesit] = @Iesit, [Plecare] = @DataDeLucru, [NrNopti] = @NrNopti, [OraE] = GETDATE() WHERE [ID] = @ID";
                                                SqlCommand cmd = new SqlCommand(sql, cnn, tran);
                                                cmd.Parameters.Add(new SqlParameter("@Iesit", SqlDbType.Bit)).Value = true;
                                                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = rc.ID;
                                                cmd.Parameters.Add(new SqlParameter("@DataDeLucru", SqlDbType.DateTime)).Value = ConexiuneDB.DataLucr;
                                                cmd.Parameters.Add(new SqlParameter("@NrNopti", SqlDbType.Int)).Value = Convert.ToInt32((ConexiuneDB.DataLucr - InitialSosire).TotalDays);
                                                cmd.ExecuteNonQuery();

                                                int IdRezCamMut = 0;
                                                sql = @"SELECT TOP 1 ID FROM [SOLON.H].[hotel].[RezervariCamereMutari] WHERE [IdRezervareCamera] = @ID ORDER BY Iesire DESC";
                                                cmd = new SqlCommand(sql, cnn, tran);
                                                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = rc.ID;
                                                cmd.Parameters.Add(new SqlParameter("@DataDeLucru", SqlDbType.DateTime)).Value = ConexiuneDB.DataLucr;
                                                using (SqlDataReader reader = cmd.ExecuteReader())
                                                {
                                                    if (reader.Read())
                                                    {
                                                        IdRezCamMut = reader["ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID"]);
                                                    }
                                                }

                                                if (IdRezCamMut == 0)
                                                {
                                                    sql = @"UPDATE [SOLON.H].[hotel].[RezervariCamereMutari] SET [Iesire] = @DataDeLucru WHERE [IdRezervareCamera] = @ID";
                                                    cmd = new SqlCommand(sql, cnn, tran);
                                                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = rc.ID;
                                                    cmd.Parameters.Add(new SqlParameter("@DataDeLucru", SqlDbType.DateTime)).Value = ConexiuneDB.DataLucr;
                                                    cmd.ExecuteNonQuery();
                                                }
                                                else
                                                {
                                                    sql = @"UPDATE [SOLON.H].[hotel].[RezervariCamereMutari] SET [Iesire] = @DataDeLucru WHERE [IdRezervareCamera] = @ID AND ID =@IdRezCamMut";
                                                    cmd = new SqlCommand(sql, cnn, tran);
                                                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = rc.ID;
                                                    cmd.Parameters.Add(new SqlParameter("@IdRezCamMut", SqlDbType.Int)).Value = IdRezCamMut;
                                                    cmd.Parameters.Add(new SqlParameter("@DataDeLucru", SqlDbType.DateTime)).Value = ConexiuneDB.DataLucr;
                                                    cmd.ExecuteNonQuery();
                                                }

                                                sql = "UPDATE [SOLON.H].[hotel].[Camere] SET [Curata] = @Curata WHERE [ID] = @ID AND Suplimentara = 0";
                                                cmd = new SqlCommand(sql, cnn, tran);
                                                cmd.Parameters.Add(new SqlParameter("@Curata", SqlDbType.Bit)).Value = false;
                                                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = rc.IdCamera;
                                                cmd.ExecuteNonQuery();
                                            }

                                            tran.Commit();
                                            rv = 0;
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
                                    rv = -1;
                                    LogErori.Salveaza(exc, "CheckIn2.checkOutClick()");
                                }
                            }
                        }

                        #endregion

                        #region "iesire normala"
                        else
                            //if (1 == 1)
                            if (InitialPlecare == ConexiuneDB.DataLucr)
                            {
                                //if (txtSoldRec.ValoareNumerica > 0 || txtSoldVir.ValoareNumerica > 0)
                                //{
                                //    StringBuilder sb = new StringBuilder();
                                //    sb.AppendFormat("Cazarea FOLIO {0} are sume restante de plata:{1}", IdRezervareCamera, Environment.NewLine);
                                //    sb.AppendFormat("    REC: {0} RON{1}", txtSoldRec.Text, Environment.NewLine);
                                //    sb.AppendFormat("    VIR: {0} RON{1}", txtSoldVir.Text, Environment.NewLine);
                                //    if (Setare.GetNuPermiteChekoutCuServiciiNeplatite())
                                //    {
                                //        sb.Append("Nu se poate efectua iesirea cazarii cu sold!");
                                //        Mesaj.Warning(sb.ToString());
                                //        return;
                                //    }
                                //    sb.Append("Sunteti sigur ca doriti sa faceti iesirea?");
                                //    if (!Mesaj.Confirmare(sb.ToString()))
                                //    {
                                //        return;
                                //    }
                                //    iesireCuSold = true;
                                //}
                                List<RezervariCamere> rezCams = RezervariCamere.GetLista(Convert.ToInt32(IdRezervare));
                                //IESIRE + CURATARE CAMERA
                                using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                                {
                                    try
                                    {
                                        cnn.Open();
                                        foreach (RezervariCamere rc in rezCams)
                                        {
                                            string sql = @"UPDATE [SOLON.H].[hotel].[RezervariCamere] SET [Iesit] = @Iesit,[OraE] = GETDATE() WHERE [ID] = @ID";
                                            SqlCommand cmd = new SqlCommand(sql, cnn);
                                            cmd.Parameters.Add(new SqlParameter("@Iesit", SqlDbType.Bit)).Value = true;
                                            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = rc.ID;
                                            cmd.ExecuteNonQuery();

                                            sql = "UPDATE [SOLON.H].[hotel].[Camere] SET [Curata] = @Curata WHERE [ID] = @ID";
                                            cmd = new SqlCommand(sql, cnn);
                                            cmd.Parameters.Add(new SqlParameter("@Curata", SqlDbType.Bit)).Value = false;
                                            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = rc.IdCamera;
                                            cmd.ExecuteNonQuery();
                                        }
                                        rv = 0;
                                    }
                                    catch (Exception exc)
                                    {
                                        rv = -1;
                                        LogErori.Salveaza(exc, "CheckIn2.checkOutClick()");
                                    }
                                }
                            }
                        #endregion

                        HttpContext.Current.Session["IdRezervare"] = null;
                        HttpContext.Current.Session["codRezervare"] = null;
                        Jurnal.Salveaza(
                                "SelfHotel.GUI.CHECKOUT.PlataForm2",
                                Jurnal.Actiune.VIZ,
                                Jurnal.ImportantaMare,
                                string.Format("Check out rezervare: ID={0} Cod={1}, checkOutClick()", rezervare.ID, rezervare.CodRezervare));
                }
                else
                {
                    rv = -1; //sa piedut conexiunea
                }
            }
            else
            {
                rv = 1; // total de plata nu este 0
            }
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            return rv;
        }

        [WebMethod(EnableSession = true)]
        public static PlataCashTuple plataCashFunction(string IdNomPartener, bool io, string FirstName, string LastName, string Tara, string Localitate, string Strada, string Email, string CUI, string RegCom, string Telefon, string Denumire, string CNP, string Delegat)
        {                                                                                                   
            Decimal rv = 0;
            String IdRezervare = HttpContext.Current.Session["IdRezervare"] as String;
            String codRezervare = HttpContext.Current.Session["codRezervare"] as String;

            if (!String.IsNullOrEmpty(IdRezervare) && IdRezervare != "undefined")
            {
                long CuiFaraLitere = 0;
                if (!String.IsNullOrEmpty(CUI))
                {
                    if (!IsCIF(CUI))
                    {
                        //return "";
                    }
                    string b = "";
                    for (int i = 0; i < CUI.Length; i++)
                    {
                        if (Char.IsDigit(CUI[i]))
                            b += CUI[i];
                    }

                    if (b.Length > 0)
                    {
                        bool success = Int64.TryParse(b, out CuiFaraLitere);
                        if (!success)
                        {
                            //return "";
                        }
                    }
                }
                    NomParteneri part = new NomParteneri();
                    Int32 IdPartener = 0;
                    Int32.TryParse(IdNomPartener, out IdPartener);
                    part.IdPartener = IdPartener;
                    part.NumePartener = !io ? Denumire : FirstName + " " + LastName;
                    part.CodFiscalNumar = !io ? CuiFaraLitere + "" : CNP;
                    part.CodFiscalAtribut = "";
                    part.RegCom = RegCom;
                    part.Banca = "";
                    part.ContBanca = "";
                    part.Oras = Localitate;
                    part.Judet = "";
                    part.Telefon = Telefon;
                    part.Patron = "";
                    part.Director = "";
                    part.Strada = Strada;
                    part.Nr = "";
                    part.Bloc = "";
                    part.Scara = "";
                    part.Etaj = "";
                    part.Apartament = "";
                    part.MailAddress = Email;
                    part.WebAddress = "";
                    part.Sters = false;
                    part.IdServer = "";
                    part.IdRemote = 0;
                    part.IdgestICS = 0;
                    part.IdStrada = 0;
                    part.TipJuridic = "";
                    part.Oras2 = "";
                    part.Judet2 = "";
                    part.Strada2 = "";
                    part.Nr2 = "";
                    part.Bloc2 = "";
                    part.Scara2 = "";
                    part.Etaj2 = "";
                    part.Apartament2 = "";
                    part.IdStrada2 = 0;
                    String[] dateDelegat = Delegat.Split(new char[] { ' ' });
                    String numeDelegat="", prenumeDelegat="";
                    if (dateDelegat.Length>=2)
                    {
                        numeDelegat = dateDelegat[0];
                        prenumeDelegat = dateDelegat[1];
                    }
                    else if (dateDelegat.Length >= 1)
                    {
                        numeDelegat = dateDelegat[0];
                    }

                    part.Nume = String.IsNullOrEmpty(FirstName) || FirstName=="null" ? numeDelegat : FirstName;
                    part.Prenume = String.IsNullOrEmpty(LastName) || LastName == "null" ? prenumeDelegat : LastName;
                    part.Tip = "";
                    part.DetaliiSupFactura = "";
                    //part.DataStare = DateTime.MinValue;
                    part.Stare = 0;
                    part.Administrator = "";
                    part.Adresa = Strada;
                    part.TelAdm = "";
                    part.Aplicatie = "";
                    part.Prescurtare = "";
                    part.EModificat = false;
                    part.CapitalSocial = "";
                    part.IdParinte = 0;
                    part.Status = 0;
                    part.CodSIRUES = "";
                    part.Subunitate = "";
                    part.Cod = "";
                    part.Alias = "";
                    part.FilieraID = 0;
                    part.Filiera = "";
                    part.Tara = Tara;
                    part.ContDeEmail = Email;
                    part.IdJudet = 0;

                    int idRezCamera = 0;
                    Int32.TryParse(RezervariCamereLista[0].ID + "", out idRezCamera);

                    int adaugat = NomParteneri.Insert(part, idRezCamera);

                    HttpContext.Current.Session["IdPartener"] = adaugat+"";
                    Jurnal.Salveaza(
                                "SelfHotel.GUI.CHECKOUT.PlataForm2",
                                Jurnal.Actiune.VIZ,
                                Jurnal.ImportantaMare,
                                string.Format("Apel pentru plata cash de la kiosk, pentru rezervarea: ID={0} Cod={1}, plataCashFunction()", IdRezervare, codRezervare));
            }

            rv =  TotalDePlata;
            PlataCashTuple rezultat = new PlataCashTuple();
            rezultat.lista = RezervariCamereLista;
            rezultat.Suma = rv;

            return rezultat;
        }

        [WebMethod(EnableSession = true)]
        public static bool IsCIF(string cif)
        {
            Regex re = new Regex(@"[^0-9]");
            string p_strCIF = re.Replace(cif, "");

            int[] _aMask = { 2, 3, 5, 7, 1, 2, 3, 5, 7 };
            int _intSum = 0;

            string _strInversat = "";
            try
            {
                for (int i = p_strCIF.Length - 1; i > -1; i--)
                {
                    _strInversat += p_strCIF.Substring(i, 1);
                }
                for (int i = 1; i < _strInversat.Length; i++)
                {
                    _intSum += _aMask[i - 1] * Convert.ToInt32(_strInversat.Substring(i, 1));
                }
                _intSum *= 10;
                _intSum = _intSum % 11;
                if (_intSum == 10)
                {
                    _intSum = 0;
                }
                if (_intSum != Convert.ToInt32(_strInversat.Substring(0, 1)))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        [WebMethod(EnableSession = true)]
        public static bool achitaCash()
        {
            bool rv = false;
                
            String IdRezervare = HttpContext.Current.Session["IdRezervare"] as String;
            String codRezervare = HttpContext.Current.Session["codRezervare"] as String;
            String IdPartener = HttpContext.Current.Session["IdPartener"] as String;
            if (string.IsNullOrEmpty(IdPartener))
            {
                IdPartener = RezervariCamereLista[0].IdTurist.ToString();
            }

            EntitateTipDoc tipDoc = ConexiuneDB.tipDoc;
            EntitateMoneda moneda = ConexiuneDB.moneda;
            string ccPosesor = "";
            string txtCUI = "";
            decimal totalRON = TotalDePlata;
            rv = achita(Convert.ToInt32(IdRezervare), Convert.ToInt32(IdPartener), tipDoc, moneda, totalRON, totalRON, ccPosesor, txtCUI);

            if (rv)
            {
                try
                {
                    LoadData(0, "NUM");
                    List<EntitateServiciu> listaServicii = LoadServicii(Convert.ToInt32(IdRezervare));
                    LoadPlati(Convert.ToInt32(IdRezervare), listaServicii);
                    List<int> idPlati = listaPlati.Select(x => x.ID).ToList();
                    if (idPlati.Count > 0)
                    {
                        LoadServiciiPlata(idPlati);
                    }
                    String serieFactura = ConexiuneDB.serieFactura;
                    try
                    {
                        Facturi last = Facturi.getLastNumar(ConexiuneDB.IdHotel, ConexiuneDB.Carnet.ID);
                        int lastNumar = 0;
                        Int32.TryParse(last.Numar, out lastNumar);
                        ConexiuneDB.numarFactura = (lastNumar + 1).ToString();
                    }
                    catch (Exception exc) { }
                    String numarFactura = ConexiuneDB.numarFactura;
                    SalveazaFactura(false, Convert.ToInt32(IdPartener), serieFactura, numarFactura, Convert.ToInt32(IdRezervare));
                }catch(Exception exc){}
            }

            Jurnal.Salveaza(
                                "SelfHotel.GUI.CHECKOUT.PlataForm2",
                                Jurnal.Actiune.VIZ,
                                Jurnal.ImportantaMare,
                                string.Format("Apel functie plata cash, pentru rezervarea: ID={0} Cod={1}, achitaCash()", IdRezervare, codRezervare));
            return rv;
        }

        public static bool achita(int IdRezervare, int IdPartener, EntitateTipDoc tipDoc, EntitateMoneda moneda, decimal totalMoneda, decimal totalRON, string ccPosesor, string txtCUI)
        {
            bool rv = false;
            try
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
                                string sql = @"SELECT MAX(NumarIntern) FROM [SOLON.H].financiar.Plati WHERE IdTipDocPlata = @IdTipDocPlata AND IdHotel = @IdHotel";
                                SqlCommand cmd = new SqlCommand(sql, cnn, tran);
                                cmd.Parameters.Add(new SqlParameter("@IdTipDocPlata", SqlDbType.Int)).Value = tipDoc.ID;
                                cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.Int)).Value = ConexiuneDB.IdHotel;
                                int nrIntern = 0;
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        nrIntern = reader[0] == DBNull.Value ? 1 : Convert.ToInt32(reader[0]) + 1;
                                    }
                                }

                                sql = @"
						        INSERT INTO [SOLON.H].[financiar].[Plati]
							        ([IdHotel]
							        ,[IdTipDocPlata]
							        ,[NumarIntern]
							        ,[DocNr]
							        ,[DocSeria]
							        ,[DocData]
							        ,[IdPartener]
							        ,[IdMoneda]
							        ,[CursZi]
							        ,[ValoareMoneda]
							        ,[ValoareRON]
							        ,[DataLucru]
							        ,[DataOraCreare]
							        ,[IdUser]
							        ,[Explic]
							        ,[Sters]
							        ,[CCTip]
							        ,[CCPosesor]
							        ,[CCUltimeleCifre]
							        ,[IdRezervare]
							        ,[CCInvoiceNumber]
							        ,[CCCardIssuer]
							        ,[RestituirePentru]
							        ,[IdBacsis]
                                    ,[IdCarnet]
                                    ,[CUI]
                                    ,[PlataFaraFactura]
                                    ,[EsteOP]
                                    ,[IdCasaBanca]
                                    ,[BazaSolonXXX]
                                    ,[IdFirma])
						        VALUES
							        (@IdHotel
							        ,@IdTipDocPlata
							        ,@NumarIntern
							        ,@DocNr
							        ,@DocSeria
							        ,@DocData
							        ,@IdPartener
							        ,@IdMoneda
							        ,@CursZi
							        ,@ValoareMoneda
							        ,@ValoareRON
							        ,@DataLucru
							        ,getdate()
							        ,@IdUser
							        ,@Explic
							        ,0
							        ,@CCTip
							        ,@CCPosesor
							        ,@CCUltimeleCifre
							        ,@IdRezervare
							        ,@CCInvoiceNumber
							        ,@CCCardIssuer
							        ,@RestituirePentru
							        ,@IdBacsis
                                    ,@IdCarnet
                                    ,@CUI
                                    ,@PlataFaraFactura
                                    ,@EsteOP
                                    ,@IdCasaBanca
                                    ,@BazaSolonXXX
                                    ,@IdFirma);
						        SELECT SCOPE_IDENTITY();
					        ";
                                cmd = new SqlCommand(sql, cnn, tran);
                                cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.Int)).Value = ConexiuneDB.IdHotel;
                                cmd.Parameters.Add(new SqlParameter("@IdTipDocPlata", SqlDbType.Int)).Value = tipDoc.ID;
                                cmd.Parameters.Add(new SqlParameter("@NumarIntern", SqlDbType.Int)).Value = nrIntern;

                                //bool areSerieNumar = !string.IsNullOrEmpty(txtDocNr.Text) || !string.IsNullOrEmpty(txtDocSeria.Text) || !tipDoc.Fiscal;     // doar la BF este optional
                                bool areSerieNumar = !string.IsNullOrEmpty(ConexiuneDB.DocNr) || !string.IsNullOrEmpty(ConexiuneDB.SerieNefiscal) || !tipDoc.Fiscal;     // doar la BF este optional
                                cmd.Parameters.Add(new SqlParameter("@DocNr", SqlDbType.NVarChar)).Value = areSerieNumar ? (object)(ConexiuneDB.DocNr ?? "") : DBNull.Value;
                                cmd.Parameters.Add(new SqlParameter("@DocSeria", SqlDbType.NVarChar)).Value = areSerieNumar ? (object)(ConexiuneDB.SerieNefiscal ?? "") : DBNull.Value;
                                cmd.Parameters.Add(new SqlParameter("@DocData", SqlDbType.DateTime)).Value = DateTime.Today;

                                cmd.Parameters.Add(new SqlParameter("@IdPartener", SqlDbType.Int)).Value = IdPartener;
                                cmd.Parameters.Add(new SqlParameter("@IdMoneda", SqlDbType.Int)).Value = moneda.ID;
                                cmd.Parameters.Add(new SqlParameter("@CursZi", SqlDbType.Decimal)).Value = moneda.CursZi;
                                //if (EsteRestituire)
                                //{
                                //    totalMoneda = -totalMoneda;
                                //    totalRON = -totalRON;
                                //}
                                cmd.Parameters.Add(new SqlParameter("@ValoareMoneda", SqlDbType.Decimal)).Value = totalMoneda;
                                cmd.Parameters.Add(new SqlParameter("@ValoareRON", SqlDbType.Decimal)).Value = totalRON;

                                cmd.Parameters.Add(new SqlParameter("@DataLucru", SqlDbType.DateTime)).Value = ConexiuneDB.DataLucr;
                                cmd.Parameters.Add(new SqlParameter("@IdUser", SqlDbType.Int)).Value = 0;
                                cmd.Parameters.Add(new SqlParameter("@Explic", SqlDbType.NVarChar)).Value = "Plata efectuata prin Self hotel";

                                cmd.Parameters.Add(new SqlParameter("@RestituirePentru", SqlDbType.Int)).Value = (object)DBNull.Value; // EsteRestituire ? (object)IdPlata : (object)DBNull.Value;

                                int ccTip = 0;
                                //if (cmbCCtip.SelectedItem != null)
                                //{
                                //    ccTip = (cmbCCtip.SelectedItem as ComboBoxItem).ID;
                                //}
                                cmd.Parameters.Add(new SqlParameter("@CCTip", SqlDbType.Int)).Value = ccTip;
                                cmd.Parameters.Add(new SqlParameter("@CCPosesor", SqlDbType.NVarChar)).Value = ConexiuneDB.SerieNefiscal;
                                cmd.Parameters.Add(new SqlParameter("@CCUltimeleCifre", SqlDbType.NVarChar)).Value = ConexiuneDB.SerieNefiscal;// ccUltimeleCifre;

                                cmd.Parameters.Add(new SqlParameter("@IdRezervare", SqlDbType.Int)).Value = IdRezervare;

                                cmd.Parameters.Add(new SqlParameter("@CCInvoiceNumber", SqlDbType.NVarChar)).Value = ConexiuneDB.SerieNefiscal;// ccInvoiceNumber;
                                cmd.Parameters.Add(new SqlParameter("@CCCardIssuer", SqlDbType.NVarChar)).Value = ""; // ccIssuer;
                                cmd.Parameters.Add(new SqlParameter("@IdBacsis", SqlDbType.Int)).Value = 0;// IdBacsis;

                                //pentru carnet chitante
                                //EntitateTipDoc etd = cmbTipDoc.SelectedItem as EntitateTipDoc;
                                cmd.Parameters.Add(new SqlParameter("@IdCarnet", SqlDbType.Int)).Value = tipDoc.Carnet != null ? tipDoc.Carnet.ID : 0;
                                //end carnet chitanta

                                cmd.Parameters.Add(new SqlParameter("@CUI", SqlDbType.NVarChar)).Value = txtCUI;// chkTiparesteCUI.Checked ? (object)txtCUI.Text.Trim() : (object)DBNull.Value;

                                cmd.Parameters.Add(new SqlParameter("@PlataFaraFactura", SqlDbType.Bit)).Value = false;//EstePlataRapida;

                                cmd.Parameters.Add(new SqlParameter("@EsteOP", SqlDbType.Bit)).Value = tipDoc.EsteOP;

                                cmd.Parameters.Add(new SqlParameter("@IdCasaBanca", SqlDbType.Int)).Value = 0;

                                cmd.Parameters.Add(new SqlParameter("@BazaSolonXXX", SqlDbType.NVarChar)).Value = ConexiuneDB.CatalogFirma == null ? "" : ConexiuneDB.CatalogFirma;  // LoginInfo.CatalogFirma;
                                cmd.Parameters.Add(new SqlParameter("@IdFirma", SqlDbType.Int)).Value = ConexiuneDB.IdFirma;// LoginInfo.IdFirma;

                                Int32 idPlata = 0;
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        idPlata = reader[0] == DBNull.Value ? 0 : Convert.ToInt32(reader[0]);
                                    }
                                }

                                List<int> idServos = new List<int>();
                                sql = @"SELECT DISTINCT rs.ID
                                    FROM [SOLON.H].[hotel].[Rezervari] as r left outer join [SOLON.H].[hotel].[RezervariCamere] as rc on rc.IdRezervare=r.ID
                                    left outer join [SOLON.H].[hotel].[RezervariServicii] as rs on rs.IdRezervareCamera = rc.ID
                                    WHERE r.IdHotel=@IdHotel AND r.Sters=0 and rc.Sters=0 and r.ID=@IdRezervare";
                                cmd = new SqlCommand(sql, cnn, tran);
                                cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.Int)).Value = ConexiuneDB.IdHotel;
                                cmd.Parameters.Add(new SqlParameter("@IdRezervare", SqlDbType.Int)).Value = IdRezervare;
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    EntitateServiciu serv;
                                    EntitateCota ec;
                                    while (reader.Read())
                                    {
                                        idServos.Add(Convert.ToInt32(reader["ID"]));
                                    }
                                }
                                string IdsServicii = idServos.Aggregate("-1", (i, j) => i + "," + j);


                                sql = @"
						        INSERT INTO [SOLON.H].[financiar].[PlatiMetodePlata]
							        ([IdPlata]
							        ,[IdMetodaPlata]
							        ,[ValoareMoneda]
							        ,[ValoareRON])
						        VALUES
							        (@IdPlata
							        ,@IdMetodaPlata
							        ,@ValoareMoneda
							        ,@ValoareRON)
					            ";
                                cmd = new SqlCommand(sql, cnn, tran);
                                cmd.Parameters.Add(new SqlParameter("@IdPlata", SqlDbType.Int)).Value = idPlata;
                                cmd.Parameters.Add(new SqlParameter("@IdMetodaPlata", SqlDbType.Int));
                                cmd.Parameters.Add(new SqlParameter("@ValoareMoneda", SqlDbType.Decimal));
                                cmd.Parameters.Add(new SqlParameter("@ValoareRON", SqlDbType.Decimal));

                                if (metodePlata.Count(x => x.Valoare > 0) > 0)
                                {
                                    foreach (EntitateMetodaPlata emp in metodePlata.Where(x => x.Valoare != 0))
                                    {
                                        cmd.Parameters["@IdMetodaPlata"].Value = emp.ID;
                                        cmd.Parameters["@ValoareMoneda"].Value = emp.Valoare;
                                        cmd.Parameters["@ValoareRON"].Value = Math.Round(emp.Valoare * moneda.CursZi, 2);

                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    foreach (EntitateMetodaPlata emp in metodePlata.Where(x => x.Selected))
                                    {
                                        cmd.Parameters["@IdMetodaPlata"].Value = emp.ID;
                                        cmd.Parameters["@ValoareMoneda"].Value = emp.Valoare;
                                        cmd.Parameters["@ValoareRON"].Value = Math.Round(emp.Valoare * moneda.CursZi, 2);

                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                sql = @"INSERT INTO [SOLON.H].[financiar].[PlatiServicii]
							                ([IdPlata]
							                ,[IdRezervareServiciu]
							                ,[ValoareMoneda]
							                ,[ValoareRON]
							                ,[ValoareMonedaServ])
						                VALUES
							                (@IdPlata
							                ,@IdRezervareServiciu
							                ,@ValoareMoneda
							                ,@ValoareRON
							                ,@ValoareMonedaServ)";
                                cmd = new SqlCommand(sql, cnn, tran);
                                cmd.Parameters.Add(new SqlParameter("@IdPlata", SqlDbType.Int)).Value = idPlata;
                                cmd.Parameters.Add(new SqlParameter("@IdRezervareServiciu", SqlDbType.Int));
                                cmd.Parameters.Add(new SqlParameter("@ValoareMoneda", SqlDbType.Decimal));
                                cmd.Parameters.Add(new SqlParameter("@ValoareRON", SqlDbType.Decimal));
                                cmd.Parameters.Add(new SqlParameter("@ValoareMonedaServ", SqlDbType.Decimal));

                                foreach (EntitateServiciu es in entitateServiciiLista.Where(x => x.DePlataMonedaPlata != 0))
                                {
                                    cmd.Parameters["@IdRezervareServiciu"].Value = es.ID;
                                    cmd.Parameters["@ValoareMoneda"].Value = es.DePlataMonedaPlata;
                                    cmd.Parameters["@ValoareRON"].Value = es.DePlataRON;
                                    cmd.Parameters["@ValoareMonedaServ"].Value = es.DePlataMonedaServ;
                                    cmd.ExecuteNonQuery();
                                }

                                //Rapartizam sumele din metode de plata pe valori zilnice si servicii
                                //CalculValoriPeMetodaDePlata(IdMonedaPlata,txtTotal);
                                CalculValoriPeMetodaDePlata(moneda.ID, totalRON);

                                //apoi le salvam in baza de date


                                string sqlServiciiMP = @"INSERT INTO [SOLON.H].[financiar].[PlatiServiciiValoriModPlata]
							                                ([IDPlata]
							                                ,[IDRezervareServiciu]
							                                ,[IdModalitatePlata]
							                                ,[ValoareMoneda]
							                                ,[ValoareRON]
							                                ,[ValoareMonedaServ])
						                                VALUES
							                                (@IDPlata
							                                ,@IDRezervareServiciu
							                                ,@IdModalitatePlata
							                                ,@ValoareMoneda
							                                ,@ValoareRON
							                                ,@ValoareMonedaServ)";

                                string sqlValoriMP = @"INSERT INTO [SOLON.H].[financiar].[PlatiValoriZilnice]
                                                               ([IdPlata]
                                                               ,[IdModPlata]
                                                               ,[IdValoareZilnica]
                                                               ,[ValoareMoneda]
                                                               ,[ValoareRON]
                                                               ,[ValoareMonedaServ]
                                                               ,[Anulat]
                                                               ,[IdRezervareServiciu])
                                                         VALUES
                                                               (@IdPlata
                                                               ,@IdModPlata
                                                               ,@IdValoareZilnica
                                                               ,@ValoareMoneda
                                                               ,@ValoareRON
                                                               ,@ValoareMonedaServ
                                                               ,0
                                                               ,@IdRezervareServiciu)";
                                foreach (EntitateServiciu es in entitateServiciiLista.Where(x => x.DePlataRON != 0))
                                {
                                    foreach (int idMP in es.DePlataPerMetode.Keys)
                                    {
                                        if (es.DePlataPerMetode[idMP].DePlataRon != 0)
                                        {
                                            cmd = new SqlCommand(sqlServiciiMP, cnn, tran);
                                            cmd.Parameters.Add(new SqlParameter("@IDPlata", SqlDbType.Int)).Value = idPlata;
                                            cmd.Parameters.Add(new SqlParameter("@IDRezervareServiciu", SqlDbType.Int)).Value = es.ID;
                                            cmd.Parameters.Add(new SqlParameter("@IdModalitatePlata", SqlDbType.Int)).Value = idMP;
                                            cmd.Parameters.Add(new SqlParameter("@ValoareMoneda", SqlDbType.Decimal)).Value = es.DePlataPerMetode[idMP].DePlataMonedaPlata;
                                            cmd.Parameters.Add(new SqlParameter("@ValoareRON", SqlDbType.Decimal)).Value = es.DePlataPerMetode[idMP].DePlataRon;
                                            cmd.Parameters.Add(new SqlParameter("@ValoareMonedaServ", SqlDbType.Decimal)).Value = es.DePlataPerMetode[idMP].DePlataMonedaServ;
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    foreach (EntitateServiciuValoare val in es.Valori.Where(x => x.dePlataRon != 0))
                                    {
                                        foreach (int idMP in val.DePlataPerMetode.Keys)
                                        {
                                            if (val.DePlataPerMetode[idMP].DePlataRon != 0)
                                            {
                                                cmd = new SqlCommand(sqlValoriMP, cnn, tran);
                                                cmd.Parameters.Add(new SqlParameter("@IDPlata", SqlDbType.Int)).Value = idPlata;
                                                cmd.Parameters.Add(new SqlParameter("@IdModPlata", SqlDbType.Int)).Value = idMP;
                                                cmd.Parameters.Add(new SqlParameter("@IdValoareZilnica", SqlDbType.Int)).Value = val.ID;
                                                cmd.Parameters.Add(new SqlParameter("@ValoareMoneda", SqlDbType.Decimal)).Value = val.DePlataPerMetode[idMP].DePlataMonedaPlata;
                                                cmd.Parameters.Add(new SqlParameter("@ValoareRON", SqlDbType.Decimal)).Value = val.DePlataPerMetode[idMP].DePlataRon;
                                                cmd.Parameters.Add(new SqlParameter("@ValoareMonedaServ", SqlDbType.Decimal)).Value = val.DePlataPerMetode[idMP].DePlataMonedaServ;
                                                cmd.Parameters.Add(new SqlParameter("@IdRezervareServiciu", SqlDbType.Int)).Value = es.ID;
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                    }
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
                        Jurnal.Salveaza(
                                "SelfHotel.GUI.CHECKOUT.PlataForm2",
                                Jurnal.Actiune.VIZ,
                                Jurnal.ImportantaMare,
                                string.Format("Apel functie de inregistrare plata, pentru rezervarea: ID={0} partener={1} cu totalRON={2}, achita()", IdRezervare, IdPartener, totalRON));
                    }
                    catch (Exception exc)
                    {
                        LogErori.Salveaza(exc, "CheckIn2.achita()");
                    }
                }
            }
            catch (Exception exc)
            {
                LogErori.Salveaza(exc, "CheckIn2.achita()");
            }
            if (rv)
            {
                TotalDePlata = 0;
            }
           
            return rv;
        }

        [WebMethod(EnableSession = true)]
        public static String plataCardFunction(string IdNomPartener, bool io, string FirstName, string LastName, string Tara, string Localitate, string Strada, string Email, string CUI, string RegCom, string Telefon, string Denumire, string CNP, string Delegat)
        {
            string htmlForm = "";
            String IdRezervare = HttpContext.Current.Session["IdRezervare"] as String;
            String codRezervare = HttpContext.Current.Session["codRezervare"] as String;
            
            if (!String.IsNullOrEmpty(IdRezervare) && IdRezervare != "undefined")
            {
                if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Email))
                {
                    return "";
                }
                long CuiFaraLitere = 0;
                if (!String.IsNullOrEmpty(CUI))
                {
                    if (!IsCIF(CUI))
                    {}
                    string b = "";
                    for (int i = 0; i < CUI.Length; i++)
                    {
                        if (Char.IsDigit(CUI[i]))
                            b += CUI[i];
                    }

                    if (b.Length > 0)
                    {
                        bool success = Int64.TryParse(b, out CuiFaraLitere);
                        if (!success)
                        {}
                    }
                }

                NomParteneri part = new NomParteneri();
                Int32 IdPartener = 0;
                Int32.TryParse(IdNomPartener, out IdPartener);
                part.IdPartener = IdPartener;
                part.NumePartener = !io ? Denumire : FirstName + " " + LastName;
                part.CodFiscalNumar = !io ? CuiFaraLitere + "" : CNP;
                part.CodFiscalAtribut = "";
                part.RegCom = RegCom;
                part.Banca = "";
                part.ContBanca = "";
                part.Oras = Localitate;
                part.Judet = "";
                part.Telefon = Telefon;
                part.Patron = "";
                part.Director = "";
                part.Strada = Strada;
                part.Nr = "";
                part.Bloc = "";
                part.Scara = "";
                part.Etaj = "";
                part.Apartament = "";
                part.MailAddress = Email;
                part.WebAddress = "";
                part.Sters = false;
                part.IdServer = "";
                part.IdRemote = 0;
                part.IdgestICS = 0;
                part.IdStrada = 0;
                part.TipJuridic = "";
                part.Oras2 = "";
                part.Judet2 = "";
                part.Strada2 = "";
                part.Nr2 = "";
                part.Bloc2 = "";
                part.Scara2 = "";
                part.Etaj2 = "";
                part.Apartament2 = "";
                part.IdStrada2 = 0;
                String[] dateDelegat = Delegat.Split(new char[] { ' ' });
                String numeDelegat = "", prenumeDelegat = "";
                if (dateDelegat.Length >= 2)
                {
                    numeDelegat = dateDelegat[0];
                    prenumeDelegat = dateDelegat[1];
                }
                else if (dateDelegat.Length >= 1)
                {
                    numeDelegat = dateDelegat[0];
                }

                part.Nume = String.IsNullOrEmpty(FirstName) || FirstName == "null" ? numeDelegat : FirstName;
                part.Prenume = String.IsNullOrEmpty(LastName) || LastName == "null" ? prenumeDelegat : LastName;
                part.Tip = "";
                part.DetaliiSupFactura = "";
                //part.DataStare = DateTime.MinValue;
                part.Stare = 0;
                part.Administrator = "";
                part.Adresa = Strada;
                part.TelAdm = "";
                part.Aplicatie = "";
                part.Prescurtare = "";
                part.EModificat = false;
                part.CapitalSocial = "";
                part.IdParinte = 0;
                part.Status = 0;
                part.CodSIRUES = "";
                part.Subunitate = "";
                part.Cod = "";
                part.Alias = "";
                part.FilieraID = 0;
                part.Filiera = "";
                part.Tara = Tara;
                part.ContDeEmail = Email;
                part.IdJudet = 0;

                int idRezCamera = 0;
                Int32.TryParse(RezervariCamereLista[0].ID + "", out idRezCamera);

                int adaugat = NomParteneri.Insert(part, idRezCamera);
                    HttpContext.Current.Session["IdPartener"] = adaugat + "";

                long InvoiceID = Convert.ToInt64(IdRezervare);
                string Currency = "RON";
                CCPerson PayingPerson = new CCPerson();
                PayingPerson.FirstName = FirstName;
                PayingPerson.LastName = LastName;
                PayingPerson.Country = Tara;
                PayingPerson.Company = Denumire;
                PayingPerson.City = Localitate;
                PayingPerson.Address = Strada;
                PayingPerson.Email = Email;
                PayingPerson.Phone = Telefon;
                PayingPerson.Fax = "";

                CCPerson TargetPerson = new CCPerson();
                TargetPerson.FirstName = FirstName;
                TargetPerson.LastName = LastName;
                TargetPerson.Country = Tara;
                TargetPerson.Company = Denumire;
                PayingPerson.City = Localitate;
                PayingPerson.Address = Strada;
                TargetPerson.Email = Email;
                TargetPerson.Phone = Telefon;
                TargetPerson.Fax = "";

                List<SetariBaza> lista = SetariBaza.getLista().ToList();
                List<SetariBaza> AccountKeySetare = lista.Where(x => x.ID == 17).ToList();

                String AccountKey = "";//din baza
                AccountKey = AccountKeySetare[0].Valoare == "" ? "63113B4E13ECE418E2881BDFC5ABBC6D30C7E1C8" : AccountKeySetare[0].Valoare;
                byte[] key = GetCheie(AccountKey); // "63113B4E13ECE418E2881BDFC5ABBC6D30C7E1C8"
                if (key == null)
                {
                    return "";
                }
                CCAuthorize cca = new CCAuthorize();
                string ExtraData = CuiFaraLitere.ToString();
                string MerchantID = "44840983439"; // din baza cand vor fi configurate
                List<SetariBaza> MerchantIDSetare = lista.Where(x => x.ID == 18).ToList();
                MerchantID = MerchantIDSetare[0].Valoare == "" ? "44840983439" : MerchantIDSetare[0].Valoare;
                htmlForm = cca.StartTransaction(MerchantID, key, Convert.ToDouble(TotalDePlata), "RON", InvoiceID, "", PayingPerson, TargetPerson, ExtraData);
                Jurnal.Salveaza(
                                "SelfHotel.GUI.CHECKOUT.PlataForm2",
                                Jurnal.Actiune.VIZ,
                                Jurnal.ImportantaMare,
                                string.Format("Apel functie plata cu cardul, pentru rezervarea: ID={0} partener={1}, plataCardFunction()", IdRezervare, codRezervare));
            }
            return htmlForm;
        } 

        private static byte[] GetCheie(string cheie)
        {
            byte[] rv = null;

            try
            {
                if (cheie.Length == 0 || cheie.Length % 2 != 0)
                {
                    rv = null;
                }
                else
                {
                    rv = new byte[cheie.Length / 2];
                    for (int i = 0; i < cheie.Length; i += 2)
                    {
                        byte x = 0;
                        if (Byte.TryParse(cheie.Substring(i, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out x))
                        {
                            rv[i / 2] = x;
                        }
                        else
                        {
                            rv[i / 2] = 0;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                LogErori.Salveaza(exc, "CheckIn2.GetCheie()");
                rv = null;
            }

            return rv;
        }

        private static void CalculValoriPeMetodaDePlata(int IdMonedaPlata, Decimal txtTotal)
        {
            RamasDeRepartizat = new Dictionary<int, EntitateDePlata>();
            foreach (EntitateMetodaPlata emp in metodePlata.Where(x => x.Selected).OrderBy(x => x.Valoare))
            {
                if (!RamasDeRepartizat.ContainsKey(emp.ID))
                {
                    RamasDeRepartizat.Add(emp.ID, new EntitateDePlata());
                }

                RamasDeRepartizat[emp.ID].DePlataMonedaPlata = emp.Valoare;
                RamasDeRepartizat[emp.ID].DePlataRon = HelperCurs.MonedaToRon(emp.Valoare, DateTime.Today, IdMonedaPlata, ConexiuneDB.IdTipCursValutar);
            }

            foreach (EntitateServiciu es in entitateServiciiLista.Where(x => x.DePlataRON != 0).OrderBy(x => x.DePlataRON))
            {
                foreach (EntitateServiciuValoare val in es.Valori.Where(x => x.dePlataRon != 0).OrderBy(x => x.Data))
                {
                    decimal valDeRepartizatMonedaPlata = val.dePlataRon; //val.dePlataMonedaPlata;
                    decimal valDeRepartizatMonedaServ = val.dePlataMonedaServ;
                    decimal valDeRepartizatRon = val.dePlataRon;

                    foreach (EntitateMetodaPlata emp in metodePlata.Where(x => x.Selected).OrderBy(x => x.Valoare))
                    {
                        if (txtTotal != 0 && RamasDeRepartizat[emp.ID].DePlataMonedaPlata == 0)
                        {
                            continue;
                        }
                        if (valDeRepartizatMonedaPlata == 0)
                        {
                            break;
                        }

                        if (!es.DePlataPerMetode.ContainsKey(emp.ID))
                        {
                            es.DePlataPerMetode.Add(emp.ID, new EntitateDePlata());
                        }
                        if (!val.DePlataPerMetode.ContainsKey(emp.ID))
                        {
                            val.DePlataPerMetode.Add(emp.ID, new EntitateDePlata());
                        }

                        //Aici se face repartizarea pe valoarea zilnica
                        if ((RamasDeRepartizat[emp.ID].DePlataMonedaPlata < 0 && valDeRepartizatMonedaPlata > 0)
                            || (RamasDeRepartizat[emp.ID].DePlataMonedaPlata > 0 && valDeRepartizatMonedaPlata < 0)
                            || (Math.Abs(RamasDeRepartizat[emp.ID].DePlataMonedaPlata) >= Math.Abs(valDeRepartizatMonedaPlata))
                            || (RamasDeRepartizat[emp.ID].DePlataMonedaPlata >= valDeRepartizatMonedaPlata)
                            || txtTotal == 0)
                        {
                            val.DePlataPerMetode[emp.ID].DePlataMonedaPlata += valDeRepartizatMonedaPlata;
                            val.DePlataPerMetode[emp.ID].DePlataRon += valDeRepartizatRon;
                            if (es.Curs > 0)
                            {
                                val.DePlataPerMetode[emp.ID].DePlataMonedaServ += valDeRepartizatRon / es.Curs;
                            }
                            else
                            {
                                val.DePlataPerMetode[emp.ID].DePlataMonedaServ += valDeRepartizatRon;
                            }


                            RamasDeRepartizat[emp.ID].DePlataMonedaPlata -= valDeRepartizatMonedaPlata;
                            RamasDeRepartizat[emp.ID].DePlataRon -= valDeRepartizatRon;

                            valDeRepartizatMonedaPlata = 0;
                            valDeRepartizatRon = 0;
                            valDeRepartizatMonedaServ = 0;
                        }
                        else
                        {
                            val.DePlataPerMetode[emp.ID].DePlataMonedaPlata += RamasDeRepartizat[emp.ID].DePlataMonedaPlata;
                            val.DePlataPerMetode[emp.ID].DePlataRon += RamasDeRepartizat[emp.ID].DePlataRon;
                            if (es.Curs > 0)
                            {
                                val.DePlataPerMetode[emp.ID].DePlataMonedaServ += val.DePlataPerMetode[emp.ID].DePlataRon / es.Curs;
                            }
                            else
                            {
                                val.DePlataPerMetode[emp.ID].DePlataMonedaServ += val.DePlataPerMetode[emp.ID].DePlataRon;
                            }


                            valDeRepartizatMonedaPlata -= RamasDeRepartizat[emp.ID].DePlataMonedaPlata;
                            valDeRepartizatRon -= RamasDeRepartizat[emp.ID].DePlataRon;

                            if (es.Curs > 0)
                            {
                                valDeRepartizatMonedaServ -= val.DePlataPerMetode[emp.ID].DePlataRon / es.Curs;
                            }
                            else
                            {
                                valDeRepartizatMonedaServ -= val.DePlataPerMetode[emp.ID].DePlataRon;
                            }

                            RamasDeRepartizat[emp.ID].DePlataMonedaPlata = 0;
                            RamasDeRepartizat[emp.ID].DePlataRon = 0;
                        }

                        es.DePlataPerMetode[emp.ID].DePlataMonedaPlata += val.DePlataPerMetode[emp.ID].DePlataMonedaPlata;
                        es.DePlataPerMetode[emp.ID].DePlataMonedaServ += val.DePlataPerMetode[emp.ID].DePlataMonedaServ;
                        es.DePlataPerMetode[emp.ID].DePlataRon += val.DePlataPerMetode[emp.ID].DePlataRon;
                    }
                }
            }
            Jurnal.Salveaza(
                                "SelfHotel.GUI.CHECKOUT.PlataForm2",
                                Jurnal.Actiune.VIZ,
                                Jurnal.ImportantaMare,
                                string.Format("Apel functie calcul valori pe metoda de plata, cu: IdMonedaPlata={0} txtTotal={1}, CalculValoriPeMetodaDePlata()", IdMonedaPlata, txtTotal));
        }

        private static List<EntitateServiciu> LoadServicii(int IdRezervare)
        {
            Dictionary<int, EntitateServiciu> dic = new Dictionary<int, EntitateServiciu>();
            Dictionary<int, EntitateServiciu> dicServiciiStorno = new Dictionary<int, EntitateServiciu>();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string IdsServicii = "";
                    List<int> idServos = new List<int>();

                    string sql = @"SELECT DISTINCT rs.ID
                    FROM [SOLON.H].[hotel].[Rezervari] as r left outer join [SOLON.H].[hotel].[RezervariCamere] as rc on rc.IdRezervare=r.ID
                    left outer join [SOLON.H].[hotel].[RezervariServicii] as rs on rs.IdRezervareCamera = rc.ID
                    WHERE r.IdHotel=@IdHotel AND r.Sters=0 and rc.Sters=0 and r.ID=@IdRezervare";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.Int)).Value = ConexiuneDB.IdHotel;
                    cmd.Parameters.Add(new SqlParameter("@IdRezervare", SqlDbType.Int)).Value = IdRezervare;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        EntitateServiciu serv;
                        EntitateCota ec;
                        while (reader.Read())
                        {
                            idServos.Add(Convert.ToInt32(reader["ID"]));
                        }
                    }
                    IdsServicii = idServos.Aggregate("-1", (i, j) => i + "," + j);

                    sql = @"
                        SELECT
	                         s.[ID]
	                        ,s.[IdPartener]
	                        ,s.[EsteVirament]
	                        ,s.[IdRezervareCamera]
	                        ,s.[IdVenit]
	                        ,v.Denumire AS DenumireVenit
	                        ,s.[IdTarif]
	                        ,tr.DenBonFiscal AS DenumireTarif
	                        ,s.[IdPlanMasa]
	                        ,pm.DenBonFiscal AS DenumirePlanMasa
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
	                        ,s.[ValoareMoneda]
	                        ,s.[Curs]
	                        ,s.[ValoareRon]
	                        ,s.[IdPret]
	                        ,s.[PretMoneda]
                            ,s.[IdAvansRezervare]
                            ,s.[ObsFactura]
                            ,s.[EsteDiscount]
                            ,s.[IdServiciuDiscount]
	                        ,m.[MonedaCod]
	                        ,c.[Procent]
	                        ,c.[Denumire] AS CodCotaTVA
	                        ,p.[NumePartener]
	                        ,v.[EsteTaxa]
	                        ,v.[EsteCazare]
	                        ,v.[EsteMasa]
	                        ,v.[IdGrupaFact]
	                        ,v.[IdGrupaFact2]
	                        ,V.[CodDepartament]
	                        ,V.[CodGrupaMarfa] 
	                        ,s.[EsteStorno]
	                        ,g.[ID] AS gID
	                        ,v.[GrupeazaPeBon]
	                        ,v.[SuportaGestiune]
                            ,rc.NrAdulti AS NrAdulti
                            ,tc.Cod AS CodCamera
                            ,v.[EsteAvans]
                            ,v.[UM]
	                        ,(SELECT SUM(fs.ValoareMonedaServ) FROM [SOLON.H].financiar.FacturiServicii as fs LEFT OUTER JOIN [SOLON.H].financiar.Facturi as fac on fac.ID = fs.IdFactura WHERE fac.Anulata = 0 and fs.IdRezervareServiciu = s.ID) AS FacturatMoneda
	                        ,(SELECT SUM(fs.ValoareRON) FROM [SOLON.H].financiar.FacturiServicii as fs LEFT OUTER JOIN [SOLON.H].financiar.Facturi as fac on fac.ID = fs.IdFactura WHERE fac.Anulata = 0 and fs.IdRezervareServiciu = s.ID) AS FacturatRon
	                        --,(SELECT SUM(ps.ValoareMonedaServ) FROM [SOLON.H].financiar.PlatiServicii as ps LEFT OUTER JOIN [SOLON.H].financiar.Plati as pla on pla.ID = ps.IdPlata WHERE pla.Sters = 0 and ps.IdRezervareServiciu = s.ID) AS PlatitMoneda
	                        --,(SELECT SUM(ps.ValoareRON) FROM [SOLON.H].financiar.PlatiServicii as ps LEFT OUTER JOIN [SOLON.H].financiar.Plati as pla on pla.ID = ps.IdPlata WHERE pla.Sters = 0 and ps.IdRezervareServiciu = s.ID) AS PlatitRon
                        FROM 
	                        [SOLON.H].[hotel].[RezervariServicii] AS s INNER JOIN
	                        [SOLON.H].[hotel].[Venituri] AS v ON v.[ID] = s.[IdVenit] INNER JOIN
	                        [SOLON].[dbo].[NomMonede] AS m ON m.[IdMoneda] = s.[IdMoneda] INNER JOIN
	                        [SOLON].[dbo].[CoteTva] AS c ON c.[IdCota] = s.[IdCotaTVA] LEFT OUTER JOIN
	                        [SOLON].[dbo].[NomParteneri] AS p ON p.[IdPartener] = s.[IdPartener] LEFT OUTER JOIN
	                        [SOLON.H].[hotel].[CamereVirtualeGenerate] AS g ON g.[IDRezCam] = s.[IdRezervareCamera] LEFT OUTER JOIN
	                        [SOLON.H].[tarife].[PlanMasa] AS pm ON s.IdPlanMasa = pm.ID LEFT OUTER JOIN
	                        [SOLON.H].[tarife].[Tarife] AS tr ON s.IdTarif = tr.ID LEFT OUTER JOIN
                            [SOLON.H].[hotel].[RezervariCamere] AS rc ON s.IdRezervareCamera = rc.ID LEFT OUTER JOIN
                            [SOLON.H].[hotel].[TipCamera] AS tc ON rc.IdTipCamera = tc.ID
                        WHERE 
	                        s.[ID] IN (<lista_ids_servicii>) AND
	                        s.[IdServiciuAvansLa] = 0 AND
	                        s.[IdServiciuAvansStornat] = 0 and s.Sters=0
                    ";
                    //StringBuilder sb = new StringBuilder();
                    //for (int i = 0; i < IdsServicii.Count; i++)
                    //{
                    //    if (i > 0)
                    //    {
                    //        sb.Append(",");
                    //    }
                    //    sb.Append(IdsServicii[i]);
                    //}
                    sql = sql.Replace("<lista_ids_servicii>", IdsServicii);
                    cmd = new SqlCommand(sql, cnn);
                    //cmd.Parameters.Add(new SqlParameter("@IdRezervareCamera", SqlDbType.Int)).Value = IdRezervareCamera;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        EntitateServiciu serv;
                        EntitateCota ec;
                        while (reader.Read())
                        {
                            serv = new EntitateServiciu()
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                IdPartener = reader["IdPartener"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdPartener"]),
                                EsteVirament = reader["EsteVirament"] == DBNull.Value ? false : Convert.ToBoolean(reader["EsteVirament"]),
                                //IdRezervareCamera = IdRezervareCamera == 0 ? reader["IdRezervareCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdRezervareCamera"]) : IdRezervareCamera,
                                IdRezervareCamera = reader["IdRezervareCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdRezervareCamera"]),
                                IdVenit = Convert.ToInt32(reader["IdVenit"]),
                                IdTarif = reader["IdTarif"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdTarif"]),
                                IdPlanMasa = reader["IdPlanMasa"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdPlanMasa"]),
                                DenumireServiciu = reader["DenumireServiciu"].ToString(),
                                IdMoneda = Convert.ToInt32(reader["IdMoneda"]),
                                IdCotaTVA = Convert.ToInt32(reader["IdCotaTVA"]),
                                CodCotaTVA = reader["CodCotaTVA"].ToString(),
                                TaxaProcentuala = Convert.ToBoolean(reader["TaxaProcentuala"]),
                                TaxaLaPersoane = Convert.ToBoolean(reader["TaxaLaPersoane"]),
                                ValoareTaxa = Convert.ToDecimal(reader["ValoareTaxa"]),
                                Observatii = reader["Observatii"] == DBNull.Value ? "" : reader["Observatii"].ToString(),
                                Sters = Convert.ToBoolean(reader["Sters"]),
                                IdMotivStergere = reader["IdMotivStergere"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdMotivStergere"]),
                                CodMoneda = reader["MonedaCod"].ToString(),
                                ProcentTVA = Convert.ToDecimal(reader["Procent"]),
                                NumePartener = reader["NumePartener"] == DBNull.Value ? "" : reader["NumePartener"].ToString(),
                                Cantitate = Convert.ToInt32(reader["Cantitate"]),
                                EsteTaxa = Convert.ToBoolean(reader["EsteTaxa"]),
                                EsteCazare = Convert.ToBoolean(reader["EsteCazare"]),
                                EsteMasa = Convert.ToBoolean(reader["EsteMasa"]),
                                PostareAmanata = Convert.ToBoolean(reader["PostareAmanata"]),
                                IdGrupaFact1 = reader["IdGrupaFact"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdGrupaFact"]),
                                IdGrupaFact2 = reader["IdGrupaFact2"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdGrupaFact2"]),
                                ValoareMoneda = reader["ValoareMoneda"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareMoneda"]),
                                Curs = reader["Curs"] == DBNull.Value ? 1 : Convert.ToDecimal(reader["Curs"]),
                                ValoareRon = reader["ValoareRon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareRon"]),
                                EsteStorno = reader["EsteStorno"] == DBNull.Value ? false : Convert.ToBoolean(reader["EsteStorno"]),
                                FacturatMoneda = reader["FacturatMoneda"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["FacturatMoneda"]),
                                FacturatRon = reader["FacturatRon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["FacturatRon"]),
                                IdPret = reader["IdPret"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdPret"]),
                                PretMoneda = reader["PretMoneda"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["PretMoneda"]),
                                IdCameraVirtualaGenerata = reader["gID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["gID"]),
                                Departament = reader["CodDepartament"] == DBNull.Value ? 1 : Convert.ToInt32(reader["CodDepartament"]),
                                Articol = reader["CodGrupaMarfa"] == DBNull.Value ? 1 : Convert.ToInt32(reader["CodGrupaMarfa"]),
                                //PlatitMoneda = reader["PlatitMoneda"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["PlatitMoneda"]),
                                //PlatitRon = reader["PlatitRon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["PlatitRon"])
                                GrupeazaPeBon = reader["GrupeazaPeBon"] == DBNull.Value ? false : Convert.ToBoolean(reader["GrupeazaPeBon"]),
                                DenumirePlanMasa = reader["DenumirePlanMasa"] == DBNull.Value ? "" : reader["DenumirePlanMasa"].ToString(),
                                DenumireTarif = reader["DenumireTarif"] == DBNull.Value ? "" : reader["DenumireTarif"].ToString(),
                                DenumireVenit = reader["DenumireVenit"] == DBNull.Value ? "" : reader["DenumireVenit"].ToString(),
                                EsteProdus = reader["SuportaGestiune"] == DBNull.Value ? false : Convert.ToBoolean(reader["SuportaGestiune"]),

                                NrAdulti = reader["NrAdulti"] == DBNull.Value ? 1 : Convert.ToInt32(reader["NrAdulti"]),
                                CodCamera = reader["CodCamera"] == DBNull.Value ? "" : reader["CodCamera"].ToString(),
                                IdAvansRezervare = reader["IdAvansRezervare"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdAvansRezervare"]),
                                ObsFactura = reader["ObsFactura"] == DBNull.Value ? "" : reader["ObsFactura"].ToString(),
                                esteAvans = reader["EsteAvans"] == DBNull.Value ? false : Convert.ToBoolean(reader["EsteAvans"]),
                                UM = reader["UM"] == DBNull.Value ? "" : reader["UM"].ToString(),

                                EsteDiscount = reader["EsteDiscount"] == DBNull.Value ? false : Convert.ToBoolean(reader["EsteDiscount"]),
                                IdServiciuDiscount = reader["IdServiciuDiscount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdServiciuDiscount"]),
                                //public string UnitateMasura { get; set; }
                            };
                            ec = listaCote.FirstOrDefault(x => x.IdCota == serv.IdCotaTVA);
                            if (ec != null)
                            {
                                ec.AreServicii = true;
                                if (!ec.Expirata)
                                {
                                    dic.Add(serv.ID, serv);
                                }
                            }
                        }
                    }

                    // acum incarcare valori servicii
                    sql = @"
                        SELECT
                             v.[ID]
                            ,v.[IdRezervareServiciu]
                            ,v.[Data]
                            ,v.[Valoare]
                            ,v.[Postat]
                            ,v.[ValoareRon]
                            ,(SELECT SUM(fvz.ValoareMonedaServ) FROM  [SOLON.H].financiar.FacturiValoriZilnice as fvz LEFT OUTER JOIN  [SOLON.H].financiar.Facturi as fac on fac.ID = fvz.IdFactura WHERE fvz.IdValoareZilnica = v.ID AND fac.Anulata = 0) AS FacturatMoneda
                            ,(SELECT SUM(fvz.ValoareRON) FROM  [SOLON.H].financiar.FacturiValoriZilnice as fvz LEFT OUTER JOIN  [SOLON.H].financiar.Facturi as fac on fac.ID = fvz.IdFactura WHERE fvz.IdValoareZilnica = v.ID AND fac.Anulata = 0) AS FacturatRon
                            ,(SELECT SUM(pvz.ValoareMonedaServ) FROM  [SOLON.H].financiar.PlatiValoriZilnice as pvz LEFT OUTER JOIN  [SOLON.H].financiar.Plati as pla on pla.ID = pvz.IdPlata WHERE pvz.IdValoareZilnica = v.ID AND pla.Sters = 0) AS PlatitMoneda
                            ,(SELECT SUM(pvz.ValoareRON) FROM  [SOLON.H].financiar.PlatiValoriZilnice as pvz LEFT OUTER JOIN  [SOLON.H].financiar.Plati as pla on pla.ID = pvz.IdPlata WHERE pvz.IdValoareZilnica = v.ID AND pla.Sters = 0) AS PlatitRon
                        FROM
                             [SOLON.H].[hotel].[RezervariServicii] AS s INNER JOIN 
                             [SOLON.H].[hotel].[RezervariServiciiValori] AS v ON v.[IdRezervareServiciu] = s.[ID]
                        WHERE 
                            s.[ID] IN (<lista_ids_servicii>) AND
                            s.[IdServiciuAvansLa] = 0 AND
                            s.[IdServiciuAvansStornat] = 0
                    ";
                    sql = sql.Replace("<lista_ids_servicii>", IdsServicii);
                    cmd = new SqlCommand(sql, cnn);
                    //cmd.Parameters.Add(new SqlParameter("@IdRezervareCamera", SqlDbType.Int)).Value = IdRezervareCamera;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        EntitateServiciu serv;
                        int idServ;
                        while (reader.Read())
                        {
                            idServ = Convert.ToInt32(reader["IdRezervareServiciu"]);
                            if (dic.ContainsKey(idServ))
                            {
                                serv = dic[idServ];
                                serv.Valori.Add(new EntitateServiciuValoare()
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    IdRezervareServiciu = idServ,
                                    Data = Convert.ToDateTime(reader["Data"]),
                                    Valoare = Convert.ToDecimal(reader["Valoare"]),
                                    Postat = Convert.ToBoolean(reader["Postat"]),
                                    Curs = serv.Curs,
                                    ValoareRON = Convert.ToDecimal(reader["ValoareRon"]),
                                    FacturatMoneda = reader["FacturatMoneda"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["FacturatMoneda"]),
                                    FacturatRon = reader["FacturatRon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["FacturatRon"]),
                                    AchitatMoneda = reader["PlatitMoneda"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["PlatitMoneda"]),
                                    AchitatRON = reader["PlatitRon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["PlatitRon"]),
                                    EsteProdus = serv.EsteProdus
                                });
                            }
                        }
                    }

                    //aici verific daca exista facturi
                    foreach (EntitateServiciu serv in dic.Values)
                    {
                        List<int> iduriFacturi = new List<int>();
                        sql = @"
                        SELECT 
                            fact.ID,
                            fact.StornoLaFactura
                        FROM
                             [SOLON.H].financiar.Facturi AS fact LEFT OUTER JOIN
                             [SOLON.H].financiar.FacturiServicii AS factserv ON factserv.IdFactura = fact.ID
                        WHERE 
                            IdRezervareServiciu = @IdServiciu AND fact.Anulata = 0
                        ";
                        sql = sql.Replace("<lista_ids_servicii>", IdsServicii);
                        cmd = new SqlCommand(sql, cnn);
                        cmd.Parameters.Add(new SqlParameter("@IdServiciu", SqlDbType.Int)).Value = serv.ID;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int IdFactura = reader["ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ID"]);
                                int IdStorno = reader["StornoLaFactura"] == DBNull.Value ? 0 : Convert.ToInt32(reader["StornoLaFactura"]);

                                if (!iduriFacturi.Contains(IdFactura))
                                {
                                    iduriFacturi.Add(IdFactura);
                                }
                                if (iduriFacturi.Contains(IdStorno))
                                {
                                    iduriFacturi.Remove(IdStorno);
                                }
                            }
                        }

                        if (iduriFacturi.Count > 0)
                        {
                            serv.AreFacturi = true;
                        }
                    }


                    if (dicServiciiStorno.Count > 0)
                    {
                        // incarcare valori
                        sql = @"
                                SELECT
                                     v.[ID]
                                    ,v.[IdRezervareServiciu]
                                    ,v.[Data]
                                    ,v.[Valoare]
                                    ,v.[Postat]
                                    ,v.[ValoareRon]
                                    ,(SELECT SUM(fvz.ValoareMonedaServ) FROM  [SOLON.H].financiar.FacturiValoriZilnice as fvz LEFT OUTER JOIN  [SOLON.H].financiar.Facturi as fac on fac.ID = fvz.IdFactura WHERE fvz.IdValoareZilnica = v.ID AND fac.Anulata = 0) AS FacturatMoneda
                                    ,(SELECT SUM(fvz.ValoareRON) FROM  [SOLON.H].financiar.FacturiValoriZilnice as fvz LEFT OUTER JOIN  [SOLON.H].financiar.Facturi as fac on fac.ID = fvz.IdFactura WHERE fvz.IdValoareZilnica = v.ID AND fac.Anulata = 0) AS FacturatRon
                                    ,(SELECT SUM(pvz.ValoareMonedaServ) FROM  [SOLON.H].financiar.PlatiValoriZilnice as pvz LEFT OUTER JOIN  [SOLON.H].financiar.Plati as pla on pla.ID = pvz.IdPlata WHERE pvz.IdValoareZilnica = v.ID AND pla.Sters = 0) AS PlatitMoneda
                                    ,(SELECT SUM(pvz.ValoareRON) FROM  [SOLON.H].financiar.PlatiValoriZilnice as pvz LEFT OUTER JOIN  [SOLON.H].financiar.Plati as pla on pla.ID = pvz.IdPlata WHERE pvz.IdValoareZilnica = v.ID AND pla.Sters = 0) AS PlatitRon
                                FROM
                                     [SOLON.H].[hotel].[RezervariServicii] AS s INNER JOIN 
                                     [SOLON.H].[hotel].[RezervariServiciiValori] AS v ON v.[IdRezervareServiciu] = s.[ID]
                                WHERE s.[ID] IN (<lista_ids_servicii>)
                            ";
                        //sb = new StringBuilder();
                        //for (int i = 0; i < listaServiciiStorno.Count; i++)
                        //{
                        //    if (i > 0)
                        //    {
                        //        sb.Append(",");
                        //    }
                        //    sb.Append(listaServiciiStorno[i].ID);
                        //}
                        string lista_idServiciiStorno = dicServiciiStorno.Keys.Aggregate("-1", (i, j) => i + "," + j);
                        sql = sql.Replace("<lista_ids_servicii>", lista_idServiciiStorno);
                        cmd = new SqlCommand(sql, cnn);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            EntitateServiciu serv;
                            int idServ;
                            while (reader.Read())
                            {
                                idServ = Convert.ToInt32(reader["IdRezervareServiciu"]);
                                if (dicServiciiStorno.ContainsKey(idServ))
                                {
                                    serv = dicServiciiStorno[idServ];
                                    dicServiciiStorno[idServ].Valori.Add(new EntitateServiciuValoare()
                                    {
                                        ID = Convert.ToInt32(reader["ID"]),
                                        IdRezervareServiciu = idServ,
                                        Data = Convert.ToDateTime(reader["Data"]),
                                        Valoare = Convert.ToDecimal(reader["Valoare"]),
                                        Postat = Convert.ToBoolean(reader["Postat"]),
                                        Curs = serv.Curs,
                                        ValoareRON = Convert.ToDecimal(reader["ValoareRon"]),
                                        FacturatMoneda = reader["FacturatMoneda"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["FacturatMoneda"]),
                                        FacturatRon = reader["FacturatRon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["FacturatRon"]),
                                        AchitatMoneda = reader["PlatitMoneda"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["PlatitMoneda"]),
                                        AchitatRON = reader["PlatitRon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["PlatitRon"])
                                    });
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    LogErori.Salveaza(exc, "CheckIn2.LoadServicii()");
                }
            }

            foreach (EntitateServiciu es in dic.Values)
            {
                es.CalculTotal();
                es.RepartizeazaValoareDePlata();
            }
            entitateServiciiLista = dic.Values.OrderBy(x => x.DataStart).ThenBy(x => x.ID).ToList();
            Jurnal.Salveaza(
                      "SelfHotel.GUI.CHECKOUT.PlataForm2",
                       Jurnal.Actiune.VIZ,
                       Jurnal.ImportantaMare,
                       string.Format("Apel functie incarca servicii, pentru rezervarea: ID={0}, LoadServicii()", IdRezervare));
            return dic.Values.ToList().OrderBy(x => x.DataStart).ThenBy(x => x.ID).ToList();
        }

        public class FisierDescarcare
        {
            public string Tip { get; set; }
            public string Denumire { get; set; }
            public byte[] Continut { get; set; }
        }

        public class ClasaFacturare
        {
            public List<RezervariCamere> listaCamere { get; set; }
            public Int32 client { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static FisierDescarcare printeazaDocument(string IdNomPartener)
        {
            FisierDescarcare rez = new FisierDescarcare();
            try
            {
                string bf = "FacturaLei.html";
                ClasaFacturare pCA = new ClasaFacturare();
                pCA.listaCamere = RezervariCamereLista;
                Int32 client = 0;
                try
                {
                    String IdPartener = HttpContext.Current.Session["IdPartener"] as String;
                    Int32.TryParse(IdPartener, out client);
                    if (client == 0)
                    {
                        Int32.TryParse(IdNomPartener, out client);
                    }
                }
                catch (Exception exc)
                {
                    Int32.TryParse(IdNomPartener, out client);
                    LogErori.Salveaza(exc, "CheckIn2.printezaDocument()");
                }
                if (client != null)
                {
                    pCA.client = client;
                }
                else
                {
                    pCA.client = 0;
                }
                DocGenHtml dgh = new DocGenHtml(bf, pCA, "", false);
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
                     "SelfHotel.GUI.CHECKOUT.PlataForm2",
                      Jurnal.Actiune.VIZ,
                      Jurnal.ImportantaMare,
                      string.Format("Printeaza factura, {0}, printeazaDocument()", ""));
                }
                catch (Exception ex)
                {
                    LogErori.Salveaza(ex, "CheckIn2.printeazaDocument()");
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
        public static bool trimiteFacturaPeMail(string IdNomPartener)
        {
            bool rv = false;
            try
            {
                ClasaFacturare pCA = new ClasaFacturare();
                pCA.listaCamere = RezervariCamereLista;
                Int32 client = 0;
                try
                {
                    String IdPartener = HttpContext.Current.Session["IdPartener"] as String;
                    Int32.TryParse(IdPartener, out client);
                    if (client == 0)
                    {
                        Int32.TryParse(IdNomPartener, out client);
                    }
                }
                catch (Exception exc)
                {
                    Int32.TryParse(IdNomPartener, out client);
                    LogErori.Salveaza(exc, "CheckIn2.trimiteFacturaPeMail()");
                }
                pCA.client = client;
                

                String mesaj = "<style type='text/css'>" +
                   ".tg  {border-collapse:collapse;border-spacing:0;margin:0px auto;}" +
                   ".tg td{font-family:Arial, sans-serif;font-size:14px;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:black;}" +
                   ".tg th{font-family:Arial, sans-serif;font-size:14px;font-weight:normal;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:black;}" +
                   ".tg .tg-88nc{font-weight:bold;border-color:inherit;text-align:center}" +
                   ".tg .tg-kiyi{font-weight:bold;border-color:inherit;text-align:left}" +
                   ".tg .tg-c3ow{border-color:inherit;text-align:center;vertical-align:top}" +
                   ".tg .tg-0pky{border-color:inherit;text-align:left;vertical-align:top}" +
                   ".tg .tg-fymr{font-weight:bold;border-color:inherit;text-align:left;vertical-align:top}" +
                   ".tg .tg-7btt{font-weight:bold;border-color:inherit;text-align:center;vertical-align:top}" +
                   ".tg .tg-xldj{border-color:inherit;text-align:left}" +
                   ".tg-sort-header::-moz-selection{background:0 0}.tg-sort-header::selection{background:0 0}.tg-sort-header{cursor:pointer}.tg-sort-header:after{content:'';float:right;margin-top:7px;border-width:0 5px 5px;border-style:solid;border-color:#404040 transparent;visibility:hidden}.tg-sort-header:hover:after{visibility:visible}.tg-sort-asc:after,.tg-sort-asc:hover:after,.tg-sort-desc:after{visibility:visible;opacity:.4}.tg-sort-desc:after{border-bottom:none;border-width:5px 5px 0}@media screen and (max-width: 767px) {.tg {width: auto !important;}.tg col {width: auto !important;}.tg-wrap {overflow-x: auto;-webkit-overflow-scrolling: touch;margin: auto 0px;}}</style>" +
                   "<div class='tg-wrap'><table id='tg-uekmP' class='tg'>" +
                    " <tr>" +
                    "   <th class='tg-0pky' colspan='2'>Furnizor: $[Furnizor]<br>Capital social: $[Capitalsocial]<br>Reg com: $[Regcom]<br>Codul Fiscal: $[Cif]<br>Sediul: $[Sediul]<br>$[Cont]</th>" +
                    "   <th class='tg-c3ow' colspan='3'><span style='font-weight:700'>Seria: "+ConexiuneDB.serieFactura+" Nr. "+ConexiuneDB.numarFactura+"</span><br><span style='font-weight:700'>Factura</span><br><span style='font-weight:700'>Fiscala</span><br><span style='font-weight:700'>Nr. facturii: "+ConexiuneDB.numarFactura+"</span><br><span style='font-weight:700'>Data: "+ConexiuneDB.DataLucr.ToShortDateString()+"</span><br></th>" +
                     "  <th class='tg-0pky' colspan='3'>Cumparator: $[Cumparator]<br>Nr. Reg. Com.: $[RegComcumparator]<br>CUI/CNP: $[Cifcumparator]<br>Sediul: $[Sediulcumparator]<br>$[ContCumparator]<br>$[BancaCumparator]</th>" +
                     "</tr>" +
                     "<tr>" +
                    "   <td class='tg-fymr'>Nr.<br>Crt.</td>" +
                    "   <td class='tg-kiyi'>Denumirea produselor sau a serviciilor</td>" +
                    "   <td class='tg-7btt'>Cota <br>TVA<br><br></td>" +
                    "   <td class='tg-7btt'>U.M.</td>" +
                    "   <td class='tg-88nc'>Cantitate</td>" +
                    "   <td class='tg-7btt'>Pret unitar <br>(fara TVA)<br>lei</td>" +
                     "  <td class='tg-7btt'>Valoare<br>(fara TVA)<br>lei</td>" +
                     "  <td class='tg-88nc'>Valoare<br>TVA<br>LEI</td>" +
                    " </tr>" +
                    "$[Camere]" +
                    //<tr>
                    //  <td class='tg-0pky'>$[t0]</td>
                    //  <td class='tg-xldj'>$[t1]</td>
                    //  <td class='tg-0pky'>$[t2]</td>
                    //  <td class='tg-0pky'>$[t3]</td>
                    //  <td class='tg-xldj'>$[t4]</td>
                    //  <td class='tg-0pky'>$[t5]</td>
                    //  <td class='tg-0pky'>$[t6]</td>
                    //  <td class='tg-xldj'>$[t7]</td>
                    //</tr>
                     "<tr>" +
                     "  <td class='tg-0pky' colspan='8'>$[ObsSubsol]<br>Termen de plata $[TermenDePlata] zile<br></td>" +
                     "</tr>" +
                     "<tr>" +
                     "  <td class='tg-0pky' rowspan='2'>Semnatura si stampila<br>furnizorului<br><br><br><br></td>" +
                     "  <td class='tg-0pky' rowspan='2'>Data privind expeditia<br>Numele delegatului: $[Delegat]<br>Buletin/cartea de identitate: $[CIDelegat]<br>Mijloc de transport: $[Transport]</td>" +
                     "  <td class='tg-0pky' colspan='2'>Total LEI</td>" +
                     "  <td class='tg-0pky' colspan='2'>$[TotalFaraTVA]</td>" +
                     "  <td class='tg-0pky' colspan='2'>$[TVA]</td>" +
                    " </tr>" +
                    " <tr>" +
                    "   <td class='tg-0pky' colspan='2'>Semnatura de primire<br><br></td>" +
                    "   <td class='tg-0pky' colspan='2'>Total de plata LEI</td>" +
                    "   <td class='tg-0pky' colspan='2'>$[Total]</td>" +
                    " </tr>" +
                   "</table></div>";
                int contor = 1;
                String camere = "";
                decimal totalFaraTVA = 0;
                decimal totalTVAProcent = 0;
                foreach (RezervariCamere cB in pCA.listaCamere)
                {
                    camere += "<tr>" +
                                   "<td class='tg-0pky'>" + contor + "</td>" +
                                   "<td class='tg-xldj'>Camera " + cB.Denumire + "</td>" +
                                   "<td class='tg-0lax'></td>" +
                                   "<td class='tg-0lax'></td>" +
                                   "<td class='tg-xldj'></td>" +
                                   "<td class='tg-0lax'></td>" +
                                   "<td class='tg-0lax'></td>" +
                                   "<td class='tg-xldj'></td>" +
                             "</tr>";
                    List<EntitateServiciu> entitateServiciiLista = cB.entitateServiciiLista;
                    foreach (EntitateServiciu serviciu in entitateServiciiLista)
                    {
                        decimal totalServiciuFaraTVA = serviciu.TotalRON - serviciu.ProcentTVA * serviciu.TotalRON / 100;
                        decimal PretUnitar = totalServiciuFaraTVA / serviciu.Cantitate;
                        Decimal tvaCameraValoare = serviciu.TotalRON * serviciu.ProcentTVA / 100;
                        camere += "<tr>" +
                                       "<td class='tg-0pky celulaAscunsa'></td>" +
                                       "<td class='tg-xldj celulaAscunsa'>" + serviciu.DenumireServiciu + "</td>" +
                                       "<td class='tg-0lax celulaAscunsa'>" + serviciu.ProcentTVA + "</td>" +
                                       "<td class='tg-0lax celulaAscunsa'>" + serviciu.UM + "</td>" +
                                       "<td class='tg-xldj celulaAscunsa'>" + serviciu.Cantitate + "</td>" +
                                       "<td class='tg-0lax celulaAscunsa'>" + PretUnitar.ToString("F") + "</td>" +
                                       "<td class='tg-0lax celulaAscunsa'>" + (serviciu.Cantitate * PretUnitar).ToString("F") + "</td>" +
                                       "<td class='tg-xldj celulaAscunsa'>" + tvaCameraValoare.ToString("F") + "</td>" +
                                 "</tr>";
                        totalTVAProcent += (serviciu.TotalRON * serviciu.ProcentTVA / 100);
                        totalFaraTVA += serviciu.TotalRON - (serviciu.TotalRON * serviciu.ProcentTVA / 100);
                    }
                    Int32 NrNopti = cB.NrNopti;
                    contor++;
                }
                NomParteneri partener = NomParteneri.GetLista(pCA.client);
                if (partener == null)
                {
                    partener = new NomParteneri();
                }
                //Firme firme = Firme.getFurnizor();
                //EntitateCarnetFacturi ecf = EntitateCarnetFacturi.getCarnet();
                NomParteneri partenerFurnizor = NomParteneri.GetLista(ConexiuneDB.Carnet.IdFurnizor);
                //NomParteneri partenerFurnizor = NomParteneri.GetLista(ConexiuneDB.tipDoc.Carnet.IdFurnizor);
                mesaj = mesaj.Replace("$[Furnizor]", partenerFurnizor.NumePartener);
                mesaj = mesaj.Replace("$[Capitalsocial]", partenerFurnizor.CapitalSocial);
                mesaj = mesaj.Replace("$[Regcom]", partenerFurnizor.RegCom);
                mesaj = mesaj.Replace("$[Cif]", partenerFurnizor.CodFiscalAtribut +" "+partenerFurnizor.CodFiscalNumar);
                mesaj = mesaj.Replace("$[Sediul]", partenerFurnizor.Judet +" "+partenerFurnizor.Oras+" "+partenerFurnizor.Strada+" "+partenerFurnizor.Nr+" "+partenerFurnizor.Bloc);
                mesaj = mesaj.Replace("$[Cont]", partenerFurnizor.ContBanca);
                mesaj = mesaj.Replace("$[Numar]", pCA.listaCamere[0].IdRezervare.ToString());
                mesaj = mesaj.Replace("$[TVA]", Math.Round(Convert.ToDecimal(totalTVAProcent), 2).ToString());
                mesaj = mesaj.Replace("$[Camere]", camere);
                mesaj = mesaj.Replace("$[Cumparator]", !String.IsNullOrEmpty(partener.NumePartener) && partener.NumePartener != "null" ? partener.NumePartener : "---");
                mesaj = mesaj.Replace("$[PL]", partenerFurnizor.Judet+ " "+partenerFurnizor.Strada+" "+partenerFurnizor.Nr);
                mesaj = mesaj.Replace("$[Transport]", ConexiuneDB.Transport);
                mesaj = mesaj.Replace("$[RegComcumparator]", !String.IsNullOrEmpty(partener.RegCom) && partener.RegCom != "null" ? partener.RegCom : "---");
                mesaj = mesaj.Replace("$[Cifcumparator]", !String.IsNullOrEmpty(partener.CodFiscalNumar) && partener.CodFiscalNumar != "null" ? partener.CodFiscalNumar : "---");
                mesaj = mesaj.Replace("$[TotalFaraTVA]", totalFaraTVA + "");
                mesaj = mesaj.Replace("$[Total]", Math.Round((Convert.ToDecimal(totalFaraTVA + totalTVAProcent)), 2).ToString());

                mesaj = mesaj.Replace("$[Data]", DateTime.Today.ToShortDateString());

                mesaj = mesaj.Replace("$[Sediulcumparator]", !String.IsNullOrEmpty(partener.Strada) && partener.Strada != "null" ? partener.Strada : "---");
                mesaj = mesaj.Replace("$[ContCumparator]", !String.IsNullOrEmpty(partener.ContBanca) && partener.ContBanca != "null" ? partener.ContBanca : "---");
                mesaj = mesaj.Replace("$[BancaCumparator]",!String.IsNullOrEmpty(partener.Banca) && partener.Banca != "null" ? partener.Banca : "---");
                mesaj = mesaj.Replace("$[Delegat]", partener.Nume+" "+partener.Prenume);
                mesaj = mesaj.Replace("$[CIDelegat]", !String.IsNullOrEmpty(partener.CodFiscalNumar) && partener.CodFiscalNumar != "null" ? partener.CodFiscalNumar : "---");
                mesaj = mesaj.Replace("$[Termendeplata]", ConexiuneDB.TermenPlata.ToShortDateString());
                String to = string.IsNullOrEmpty(partener.ContDeEmail) ? partener.MailAddress : partener.ContDeEmail;

                rv = SendEmail("Factura ", mesaj, to);
                Jurnal.Salveaza(
                     "SelfHotel.GUI.CHECKOUT.PlataForm2",
                      Jurnal.Actiune.VIZ,
                      Jurnal.ImportantaMare,
                      string.Format("Trimite factura pe email, {0}, trimiteFacturaPeMail()", ""));
            }
            catch (Exception exc)
            {
                LogErori.Salveaza(exc, "CheckIn2.trimiteFacturaPeMail()");
                return false;
            }
            return rv;
        }

        public static bool SendEmail(string subiect, string mesaj, string To)
        {
            string SmtpHost = "";
            string SmtpUser = "";
            string SmtpPass = "";
            string SmtpPort = "";
            bool EsteSSL = false;
            string SmtpSslMode = "";
            string SmtpAuthType = "";
            try
            {
                List<SetariBaza> listaSetari = SetariBaza.getLista();
                if (listaSetari.Count > 0)
                {
                    SmtpHost = listaSetari.Where(x => x.ID == 1).ToList()[0].Valoare.ToString();
                    SmtpUser = listaSetari.Where(x => x.ID == 2).ToList()[0].Valoare.ToString();
                    SmtpPass = listaSetari.Where(x => x.ID == 3).ToList()[0].Valoare.ToString();
                    SmtpPort = listaSetari.Where(x => x.ID == 4).ToList()[0].Valoare.ToString();
                    EsteSSL = Convert.ToBoolean(listaSetari.Where(x => x.ID == 5).ToList()[0].Valoare.ToString());
                    SmtpSslMode = listaSetari.Where(x => x.ID == 6).ToList()[0].Valoare.ToString();
                    SmtpAuthType = listaSetari.Where(x => x.ID == 7).ToList()[0].Valoare.ToString();
                }
            }
            catch (Exception ex)
            {
                LogErori.Salveaza(ex, "CheckIn2.SendEmail()");
                return false;
            }
            

            string From = SmtpUser;

            if (SmtpHost == "" || SmtpUser == "" || SmtpPass == "" || SmtpPort == "")
            {
                return false;
            }

            SslMode SetareSslMode = AegisImplicitMail.SslMode.None;
            if (SmtpSslMode == "SSL") SetareSslMode = AegisImplicitMail.SslMode.Ssl;
            else if (SmtpSslMode == "TLS") SetareSslMode = AegisImplicitMail.SslMode.Tls;
            else if (SmtpSslMode == "AUTO") SetareSslMode = AegisImplicitMail.SslMode.Auto;

            AuthenticationType SetareAuthType = AegisImplicitMail.AuthenticationType.UseDefualtCridentials;
            if (SmtpAuthType == "Base64") SetareAuthType = AegisImplicitMail.AuthenticationType.Base64;
            else if (SmtpAuthType == "PlainText") SetareAuthType = AegisImplicitMail.AuthenticationType.PlainText;

            try
            {
                MimeMailAddress senderMail = new MimeMailAddress(From);
                MimeMailMessage mailMessage = new MimeMailMessage()
                {
                    Subject = subiect,
                    Body = mesaj,
                    IsBodyHtml = true,
                    From = senderMail,
                    Sender = senderMail
                };
                //foreach (EntitateFilePath fp in ListaFiles)
                //{
                //    if (!string.IsNullOrEmpty(fp.FilePath))
                //    {
                //        if (File.Exists(fp.FilePath))
                //        {
                //            if (fp.FileType == "csv")
                //            {
                //                System.Net.Mime.ContentType content = new System.Net.Mime.ContentType("text/csv");
                //                mailMessage.Attachments.Add(new MimeAttachment(fp.FilePath, content, new AttachmentLocation()));
                //            }
                //            else if (fp.FileType == "pdf")
                //            {
                //                System.Net.Mime.ContentType content = new System.Net.Mime.ContentType("application/pdf");
                //                mailMessage.Attachments.Add(new MimeAttachment(fp.FilePath, content, new AttachmentLocation()));
                //            }
                //            else if (fp.FileType == "html")
                //            {
                //                System.Net.Mime.ContentType content = new System.Net.Mime.ContentType("text/html");
                //                mailMessage.Attachments.Add(new MimeAttachment(fp.FilePath, content, new AttachmentLocation()));
                //            }
                //            else if (fp.FileType == "xls")
                //            {
                //                System.Net.Mime.ContentType content = new System.Net.Mime.ContentType("application/vnd.ms-excel");
                //                mailMessage.Attachments.Add(new MimeAttachment(fp.FilePath, content, new AttachmentLocation()));
                //            }
                //            else if (fp.FileType == "doc")
                //            {
                //                System.Net.Mime.ContentType content = new System.Net.Mime.ContentType("application/msword");
                //                mailMessage.Attachments.Add(new MimeAttachment(fp.FilePath, content, new AttachmentLocation()));
                //            }
                //            else
                //            {
                //                mailMessage.Attachments.Add(new MimeAttachment(fp.FilePath));
                //            }
                //        }
                //    }
                //}

                mailMessage.To.Add(new MimeMailAddress(To.Trim()));
                //if (HotelEmail != "")
                //{
                //    mailMessage.CC.Add(new MimeMailAddress(HotelEmail));
                //}
                mailMessage.Bcc.Add(new MimeMailAddress(From));

                MimeMailer emailer = new MimeMailer(SmtpHost, Convert.ToInt32(SmtpPort))
                {
                    MailMessage = mailMessage,
                    SslType = SetareSslMode,
                    EnableImplicitSsl = EsteSSL,
                    User = SmtpUser,
                    Password = SmtpPass,
                    AuthenticationMode = SetareAuthType,
                };
                emailer.SendMail(emailer.MailMessage);

                mailMessage.Dispose();
                emailer.Dispose();
            }
            catch (Exception exc)
            {
                LogErori.Salveaza(exc, "CheckIn2.SendEmail()");
                return false;
            }

            return true;
        }

        [WebMethod(EnableSession = true)]
        public static List<NomParteneri> getDateFirma(string cifIntrodus)
        {
            List<NomParteneri> listaClienti = new List<NomParteneri>();
            string ciff = cifIntrodus.Trim();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ciff.Length; i++)
            {
                if (char.IsDigit(ciff[i]))
                {
                    sb.Append(ciff[i]);
                }
            }
            ciff = sb.ToString();
            try
            {
                TcpClient tcpClientTest = new TcpClient();
                var result = tcpClientTest.BeginConnect("datefirma.multisoft.ro", 81, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(1000, false);

                if (success)
                {
                    XDocument xDoc = XDocument.Load(String.Format("http://datefirma.multisoft.ro:81/DateFirma.ashx?CUI={0}&Key={1}&AppName={2}", Uri.EscapeDataString(ciff), "8h6oQgL6rOvngn7Q", Uri.EscapeDataString("SolON.ZipEscort")));
                    if (xDoc != null)
                    {
                        XElement xeFirme = xDoc.Element("Firme");
                        if (xeFirme != null)
                        {
                            XAttribute xaExista = xeFirme.Attribute("exista");
                            if (xaExista != null && xaExista.Value == "true")
                            {
                                List<XElement> Firme = xeFirme.Elements("Firma").ToList();
                                if (Firme != null && Firme.Count > 0)
                                {
                                    //----------------------------------------------------------------
                                    if (Firme.Count > 1)
                                    {
                                        #region mai multe firme definition
                                        foreach (XElement xeFirma in Firme)
                                        {
                                            NomParteneri firma = new NomParteneri();
                                            XElement xeDateRegCom = xeFirma.Element("DateRegCom");
                                            if (xeDateRegCom != null)
                                            {
                                                XAttribute xaDateRegComExista = xeDateRegCom.Attribute("exista");
                                                if (xaDateRegComExista != null && xaDateRegComExista.Value == "true")
                                                {
                                                    XElement xeDenumire = xeDateRegCom.Element("Denumire");
                                                    XElement xeCUI = xeDateRegCom.Element("CUI");
                                                    XElement xeRegCom = xeDateRegCom.Element("RegCom");
                                                    XElement xeStari = xeDateRegCom.Element("Stari");
                                                    XElement xeJudet = xeDateRegCom.Element("Judet");
                                                    XElement xeLocalitate = xeDateRegCom.Element("Localitate");

                                                    if (xeDenumire != null && xeDenumire.Value != null)
                                                    {
                                                        firma.NumePartener = xeDenumire.Value;
                                                    }

                                                    if (xeRegCom != null && xeRegCom.Value != null)
                                                    {
                                                        firma.RegCom = xeRegCom.Value;
                                                    }

                                                    if (xeJudet != null && xeJudet.Value != null)
                                                    {
                                                        firma.Judet = xeJudet.Value;
                                                    }

                                                    if (xeLocalitate != null && xeLocalitate.Value != null)
                                                    {
                                                        firma.Oras = xeLocalitate.Value;
                                                    }
                                                }
                                            }

                                            XElement xeInfoANAF = xeFirma.Element("InfoANAF");
                                            if (xeInfoANAF != null)
                                            {
                                                XAttribute xaInfoANAFExista = xeInfoANAF.Attribute("exista");
                                                if (xaInfoANAFExista != null && xaInfoANAFExista.Value == "true")
                                                {
                                                    XElement xeCUI = xeInfoANAF.Element("CUI");
                                                    XElement xeData = xeInfoANAF.Element("Data");
                                                    XElement xeDenumire = xeInfoANAF.Element("Denumire");
                                                    XElement xeAdresa = xeInfoANAF.Element("Adresa");
                                                    XElement xeTVA = xeInfoANAF.Element("TVA");
                                                    XElement xeDataSfarsit = xeInfoANAF.Element("DataSfarsit");
                                                    XElement xeDataAnulImp = xeInfoANAF.Element("DataAnulImp");
                                                    XElement xeMesaj = xeInfoANAF.Element("Mesaj");

                                                    if (String.IsNullOrEmpty(firma.NumePartener))
                                                    {
                                                        firma.Nume = xeDenumire.Value;
                                                    }
                                                }
                                            }
                                            listaClienti.Add(firma);
                                        }

                                    }
                                        #endregion

                                    #region 1 singura firma definition2
                                    else
                                    {
                                        XElement xeFirma = Firme[0];

                                        NomParteneri firma = new NomParteneri();
                                        bool existaDate = false;
                                        XElement xeDateRegCom = xeFirma.Element("DateRegCom");
                                        if (xeDateRegCom != null)
                                        {
                                            XAttribute xaDateRegComExista = xeDateRegCom.Attribute("exista");
                                            if (xaDateRegComExista != null && xaDateRegComExista.Value == "true")
                                            {
                                                XElement xeDenumire = xeDateRegCom.Element("Denumire");
                                                XElement xeCUI = xeDateRegCom.Element("CUI");
                                                XElement xeRegCom = xeDateRegCom.Element("RegCom");
                                                XElement xeStari = xeDateRegCom.Element("Stari");
                                                XElement xeJudet = xeDateRegCom.Element("Judet");
                                                XElement xeLocalitate = xeDateRegCom.Element("Localitate");

                                                if (xeDenumire != null && xeDenumire.Value != null)
                                                {
                                                    firma.NumePartener = xeDenumire.Value;
                                                }

                                                if (xeRegCom != null && xeRegCom.Value != null)
                                                {
                                                    firma.RegCom = xeRegCom.Value;
                                                }

                                                if (xeJudet != null && xeJudet.Value != null)
                                                {
                                                    firma.Judet = xeJudet.Value;
                                                }

                                                if (xeLocalitate != null && xeLocalitate.Value != null)
                                                {
                                                    firma.Oras = xeLocalitate.Value;
                                                }

                                                existaDate = true;
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        else
                                        {
                                            return null;
                                        }

                                        XElement xeInfoANAF = xeFirma.Element("InfoANAF");
                                        if (xeInfoANAF != null)
                                        {
                                            XAttribute xaInfoANAFExista = xeInfoANAF.Attribute("exista");
                                            if (xaInfoANAFExista != null && xaInfoANAFExista.Value == "true")
                                            {
                                                XElement xeCUI = xeInfoANAF.Element("CUI");
                                                XElement xeData = xeInfoANAF.Element("Data");
                                                XElement xeDenumire = xeInfoANAF.Element("Denumire");
                                                XElement xeAdresa = xeInfoANAF.Element("Adresa");
                                                XElement xeTVA = xeInfoANAF.Element("TVA");
                                                XElement xeDataSfarsit = xeInfoANAF.Element("DataSfarsit");
                                                XElement xeDataAnulImp = xeInfoANAF.Element("DataAnulImp");
                                                XElement xeMesaj = xeInfoANAF.Element("Mesaj");
                                                if (String.IsNullOrEmpty(firma.NumePartener))
                                                {
                                                    firma.Nume = xeDenumire.Value;
                                                }
                                                existaDate = true;
                                                listaClienti.Add(firma);
                                            }
                                        }
                                        if (!existaDate)
                                        {
                                            return null;
                                        }
                                    }
                                    #endregion

                                }
                                else
                                {
                                    return null;
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    return null;
                }
                Jurnal.Salveaza(
                     "SelfHotel.GUI.CHECKOUT.PlataForm2",
                      Jurnal.Actiune.VIZ,
                      Jurnal.ImportantaMare,
                      string.Format("Get date firma pentru CIF={0}, getDateFirma()", cifIntrodus));
            }
            catch (Exception exc)
            {
                return null;
                if (exc.Message.Contains("404"))
                {
                }
                else
                {
                }
            }
            return listaClienti;
        }

        [WebMethod(EnableSession = true)]
        public static List<NomParteneri> getClientRezervare()
        {
            List<NomParteneri> rv = new List<NomParteneri>();
            try
            {
                String IdRezervare = HttpContext.Current.Session["IdRezervare"] as String;
                String codRezervare = HttpContext.Current.Session["codRezervare"] as String;
                String IdPartener = HttpContext.Current.Session["IdPartener"] as String;

                Rezervari rezervare = Rezervari.GetRezervare(codRezervare);
                if (rezervare != null)
                {
                    List<RezervariCamere> lista = RezervariCamere.GetLista(rezervare.ID);
                    foreach (RezervariCamere rc in lista)
                    {
                        if (rc.IdTurist > 0)
                        {
                            NomParteneri partener = NomParteneri.GetLista(rc.IdTurist);
                            if (partener != null)
                            {
                                rv.Add(partener);
                            }
                        }
                    }
                }
                else
                {
                    return null;
                }
                Jurnal.Salveaza(
                     "SelfHotel.GUI.CHECKOUT.PlataForm2",
                      Jurnal.Actiune.VIZ,
                      Jurnal.ImportantaMare,
                      string.Format("Get client rezervare IdRezervare={0},codRezervare={1}, IdPartener={2} getClientRezervare()", IdRezervare, codRezervare, IdPartener));
            }
            catch (Exception ex)
            {
                LogErori.Salveaza(ex, "CheckIn2.getClientRezervare()");
                return null;
            }
            return rv;
        }
        
        [WebMethod(EnableSession = true)]
        public static String autocompleteRezervare()
        {
            String rv = null;
            
            try
            {
                String codRezervare = HttpContext.Current.Session["codRezervare"] as String;
                String vineDeLaCheckOut = HttpContext.Current.Session["vineDeLaCheckOut"] as String;
                if (Convert.ToBoolean(vineDeLaCheckOut))
                {
                    if (DeUndeVine.Contains("CheckIn") && !string.IsNullOrEmpty(codRezervare))
                    {
                        rv = codRezervare;
                    }
                    HttpContext.Current.Session["vineDeLaCheckOut"] = null;
                }
            }
            catch {
                rv = null;
            }

            return rv;
        }

        [WebMethod(EnableSession = true)]
        public static List<String> getSetariHeader()
        {
            List<String> rv = new List<string>();
            try
            {
                List<SetariBaza> lista = SetariBaza.getLista().ToList();
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
        public static bool goToAlin()
        {
            bool rv = false;
            try
            {
                string idHotel = ConexiuneDB.IdHotel.ToString();
                string paginaHome = "./Home.aspx";
                string idFirma = ConexiuneDB.IdFirma.ToString();

                ASCIIEncoding encoding = new ASCIIEncoding();
                string postData = "idHotel=" + idHotel;
                postData += ("&paginaHome=" + paginaHome);
                postData += ("&idFirma=" + idFirma);

                byte[] data = encoding.GetBytes(postData);

                HttpWebRequest myRequest =
                  (HttpWebRequest)WebRequest.Create("http://rezervari.multisoft.ro/Booking/?Hotel=13TYD3&CheckIn=20-12-2018&CheckOut=21-12-2018");
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = data.Length;
                Stream newStream = myRequest.GetRequestStream();

                newStream.Write(data, 0, data.Length);
                newStream.Close();
                rv = true;
            }
            catch (Exception exc)
            {
                rv = false;
            }
            return rv;
        }

        public static List<EntitatePlataServiciu> listaPlatiServicii = null;
        public static List<Pozitie> pozitii = new List<Pozitie>();

        private static void LoadServiciiPlata(List<int> ListaIdPlati) //in, load metode de plata
        {
            //List<int> ListaIdPlati=new List<int>();//// EntitatePlata.select id
            bool AdaugaInformatiiCamereTuristi = false;
            listaPlatiServicii = new List<EntitatePlataServiciu>();
            bool esteDispozitie = false;
            StringBuilder MetodePlataStr = new StringBuilder();
            StringBuilder DocumentStr = new StringBuilder();
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"";
                    SqlCommand cmd;
                    if (ListaIdPlati.Count > 0) 
                    {
                            sql = @"SELECT DISTINCT metpl.Denumire
                                     FROM [SOLON.H].[financiar].[PlatiMetodePlata] as pmp left outer join 
                                           [SOLON.H].[financiar].[MetodeDePlata] as metpl on metpl.id=pmp.idmetodaplata 
                                     WHERE pmp.[IdPlata] in (<IDURI_PLATI>)";
                        sql = sql.Replace("<IDURI_PLATI>", ListaIdPlati.Aggregate("-1", (x, y) => x + "," + y));
                        cmd = new SqlCommand(sql, cnn);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (MetodePlataStr.Length == 0)
                                {
                                    MetodePlataStr.Append(reader["Denumire"].ToString());
                                }
                                else
                                {
                                    MetodePlataStr.Append(", " + reader["Denumire"].ToString());
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    LogErori.Salveaza(exc, "LoadServiciiPlata(Load metode de plata)");
                }
            }

            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"";
                    SqlCommand cmd;
                    if (ListaIdPlati.Count > 0)
                    {
                        sql = @"
                        SELECT
                             [ID]
                            ,[IdPlata]
                            ,[IdRezervareServiciu]
                            ,[ValoareMoneda]
                            ,[ValoareRON]
                            ,[ValoareMonedaServ]
                        FROM [SOLON.H].[financiar].[PlatiServicii]
                        WHERE [IdPlata] in (<IDURI_PLATI>)
                    ";
                        sql = sql.Replace("<IDURI_PLATI>", ListaIdPlati.Aggregate("-1", (x, y) => x + "," + y));
                        cmd = new SqlCommand(sql, cnn);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listaPlatiServicii.Add(new EntitatePlataServiciu()
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    IdPlata = reader["IdPlata"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdPlata"]),
                                    IdRezervareServiciu = Convert.ToInt32(reader["IdRezervareServiciu"]),
                                    ValoareMoneda = Convert.ToDecimal(reader["ValoareMoneda"]),
                                    ValoareRON = Convert.ToDecimal(reader["ValoareRON"]),
                                    ValoareMonedaServ = Convert.ToDecimal(reader["ValoareMonedaServ"])
                                });
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    LogErori.Salveaza(exc, "LoadServiciiPlata()");
                }
            }
            List<int> IdsServicii = listaPlatiServicii.Select(x => x.IdRezervareServiciu).ToList();

            entitateServiciiLista = new List<EntitateServiciu>();
            Dictionary<int, EntitateServiciu> dic = new Dictionary<int, EntitateServiciu>();

            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();

                    string sql = "";
                    SqlCommand cmd = null;

                    if (IdsServicii != null && IdsServicii.Count > 0)
                    {
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
                                ,s.[ObsFactura] AS ObsFactura
                                ,s.IdAvansRezervare AS IdAvansRezervare
                                ,ra.IdRezervareCamera AS AvansIdRezervareCamera
                                ,s.[EsteStorno] AS EsteStorno
                                ,v.[UM]
                                ,rc.[NrNopti] AS NrNopti            --FacturaDeGrup
                                ,rc.[NrAdulti] AS NrAdulti
                                ,rc.[IdTipCamera] As IdTipCamera    --FacturaDeGrup
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
                                [SOLON.H].[hotel].[RezervariAvans] AS ra ON ra.ID = s.IdAvansRezervare
                            WHERE s.[ID] IN (<lista_ids_servicii>)";
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < IdsServicii.Count; i++)
                        {
                            if (i > 0)
                            {
                                sb.Append(",");
                            }
                            sb.Append(IdsServicii[i]);
                        }
                        sql = sql.Replace("<lista_ids_servicii>", sb.ToString());
                        cmd = new SqlCommand(sql, cnn);
                        //cmd.Parameters.Add(new SqlParameter("@IdRezervareCamera", SqlDbType.Int)).Value = IdRezervareCamera;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            EntitateServiciu serv;
                            while (reader.Read())
                            {
                                serv = new EntitateServiciu()
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    EsteVirament = Convert.ToBoolean(reader["EsteVirament"]),
                                    IdPartener = reader["IdPartener"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdPartener"]),
                                    IdRezervareCamera = Convert.ToInt32(reader["IdRezervareCamera"]),
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
                                    ValoareMoneda = reader["ValoareMoneda"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareMoneda"]),
                                    Curs = reader["Curs"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Curs"]),
                                    ValoareRon = reader["ValoareRon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareRon"]),
                                    Camera = reader["Camera"] == DBNull.Value ? "" : reader["Camera"].ToString(),
                                    ObsFactura = reader["ObsFactura"] == DBNull.Value ? "" : reader["ObsFactura"].ToString(),
                                    IdAvansRezervare = reader["IdAvansRezervare"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdAvansRezervare"]),
                                    EsteStorno = reader["EsteStorno"] == DBNull.Value ? false : Convert.ToBoolean(reader["EsteStorno"]),
                                    UM = reader["UM"] == DBNull.Value ? "" : reader["UM"].ToString(),
                                    //FacturaDeGrup
                                    NrNopti = reader["NrNopti"] == DBNull.Value ? 0 : Convert.ToInt32(reader["NrNopti"]),
                                    NrAdulti = reader["NrAdulti"] == DBNull.Value ? 0 : Convert.ToInt32(reader["NrAdulti"]),
                                    IdTipCamera = reader["IdTipCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdTipCamera"])
                                    //FacturaDeGrup
                                };
                                if (serv.IdRezervareCamera == 0)
                                {
                                    serv.IdRezervareCamera = reader["AvansIdRezervareCamera"] == DBNull.Value ? 0 : Convert.ToInt32(reader["AvansIdRezervareCamera"]);
                                }
                                entitateServiciiLista.Add(serv);
                                dic.Add(serv.ID, serv);
                            }
                        }
                    }

                    Dictionary<int, int> valZilnice = new Dictionary<int, int>();
                    sql = @"
                        SELECT
                             [ID]
                            ,[Data]
                            ,[Valoare]
                            ,[Curs]
                            ,[ValoareRON]
                            ,[Postat]
                        FROM [SOLON.H].[hotel].[RezervariServiciiValori]
                        WHERE [IdRezervareServiciu] = @IdRezervareServiciu
                    ";
                    cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdRezervareServiciu", SqlDbType.Int));
                    foreach (EntitateServiciu es in entitateServiciiLista)
                    {
                        cmd.Parameters["@IdRezervareServiciu"].Value = es.ID;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int Idval = Convert.ToInt32(reader["ID"]);
                                if (!valZilnice.ContainsKey(Idval))
                                {
                                    valZilnice.Add(Idval, es.ID);
                                }
                                es.Valori.Add(new EntitateServiciuValoare()
                                {
                                    ID = Idval,
                                    IdRezervareServiciu = es.ID,
                                    Data = Convert.ToDateTime(reader["Data"]),
                                    Valoare = Convert.ToDecimal(reader["Valoare"]),
                                    Curs = Convert.ToDecimal(reader["Curs"]),
                                    ValoareRON = Convert.ToDecimal(reader["ValoareRON"]),
                                    Postat = Convert.ToBoolean(reader["Postat"])
                                });
                            }
                        }
                    }

                    sql = @"
                        SELECT fsvz.[ID]
                              ,fsvz.[IdValoareZilnica]
                              ,[ValoareRON]
                              ,[ValoareMonedaServ]
                          FROM [SOLON.H].[financiar].[FacturiValoriZilnice] AS fsvz LEFT OUTER JOIN
		                        [SOLON.H].financiar.Facturi AS f ON f.ID = fsvz.IdFactura LEFT OUTER JOIN
		                        [SOLON.H].financiar.Facturi AS fstorno ON fstorno.StornoLaFactura = f.ID
                          WHERE fsvz.IdValoareZilnica IN (<valori_zilnice>) 
                                AND f.Anulata = 0 AND f.StornoLaFactura = 0
                                AND (fstorno.ID IS NULL OR (fstorno.ID IS NOT NULL AND fstorno.Anulata = 1))
                    ";
                    sql = sql.Replace("<valori_zilnice>", valZilnice.Keys.Aggregate("-1", (x, y) => x + "," + y));
                    HashSet<int> idurivals = new HashSet<int>();
                    cmd = new SqlCommand(sql, cnn);
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

                                    EntitateServiciuValoare valesv = serv.Valori.FirstOrDefault(x => x.ID == IdValZilnica);
                                    if (valesv != null)
                                    {
                                        valesv.FacturatMoneda += reader["ValoareMonedaServ"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareMonedaServ"]);
                                        valesv.FacturatRon += reader["ValoareRON"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ValoareRON"]);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    LogErori.Salveaza(exc, "LoadServiciiPlata()");
                }
            }
            entitateServiciiLista = entitateServiciiLista.OrderBy(x => x.DataStart).ThenBy(x => x.ID).ToList();
            foreach (EntitateServiciu es in entitateServiciiLista)
            {
                es.CalculTotal();
            }

                bool FolosescInFactura = false;
                if (!FolosescInFactura)
                {
                    foreach (EntitateServiciu serv in entitateServiciiLista)
                    {
                        serv.ObsFactura = "";
                    }
                }
                Pozitie poz;
                foreach (EntitateServiciu serv in entitateServiciiLista.Where(x => x.EsteCazare && x.IdGrupaFact2 != 0))
                {
                    if (entitateServiciiLista.Count(x => x.EsteMasa && x.IdGrupaFact1 == serv.IdGrupaFact2) > 0 || serv.IdGrupaFact1 == 0)
                    {
                        // il adauga cu grupa 2
                        poz = pozitii.FirstOrDefault(x => x.IdGrupaFact == serv.IdGrupaFact2 && x.IdCotaTVA == serv.IdCotaTVA);
                        if (poz == null)
                        {
                            poz = new Pozitie();
                            poz.IdGrupaFact = serv.IdGrupaFact2;
                            poz.NumeInitial = esteDispozitie ? "Restituire " + serv.GrupaFact2 : serv.GrupaFact2;
                            if (!AdaugaInformatiiCamereTuristi)
                            {
                                if (esteDispozitie)
                                {
                                    poz.Denumire = "Restituire " + serv.GrupaFact2;
                                }
                                else
                                {
                                    poz.Denumire = serv.GrupaFact2;
                                }
                            }

                            if (!string.IsNullOrEmpty(serv.UM_Proxy))
                            {
                                poz.UM = serv.UM_Proxy;
                            }
                            else
                            {
                                poz.UM = "BUC";
                            }

                            poz.IdCotaTVA = serv.IdCotaTVA;
                            poz.ProcentTVA = serv.ProcentTVA;
                            poz.CodCotaTVA = serv.CodCotaTVA;
                            if (!String.IsNullOrEmpty(serv.ObsFactura) && !poz.Denumire.Contains(serv.ObsFactura))
                            {
                                poz.Denumire += " " + serv.ObsFactura;
                                poz.NumeInitial = poz.NumeInitial ?? "";
                                poz.NumeInitial += " " + serv.ObsFactura;
                            }
                            pozitii.Add(poz);
                        }
                        poz.Servicii.Add(serv);
                    }
                    else
                    {
                        // il adaug cu grupa 1
                        poz = pozitii.FirstOrDefault(x => x.IdGrupaFact == serv.IdGrupaFact1 && x.IdCotaTVA == serv.IdCotaTVA);
                        if (poz == null)
                        {
                            poz = new Pozitie();
                            poz.IdGrupaFact = serv.IdGrupaFact1;
                            poz.NumeInitial = esteDispozitie ? "Restituire " + serv.GrupaFact1 : serv.GrupaFact1;
                            if (!AdaugaInformatiiCamereTuristi)
                            {
                                if (esteDispozitie)
                                {
                                    poz.Denumire = "Restituire " + serv.GrupaFact1;
                                }
                                else
                                {
                                    poz.Denumire = serv.GrupaFact1;
                                }

                                if (serv.EsteCazare)
                                {
                                    string p = " (" + serv.DataStart.ToString("dd") + "-" + serv.DataEnd.ToString("dd.MM") + ")";
                                    string c = serv.Camera;
                                    if (!poz.Denumire.Contains(p))
                                    {
                                        poz.Denumire += " " + p;
                                    }
                                    if (!poz.Denumire.Contains(c))
                                    {
                                        poz.Denumire += " " + c;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(serv.UM_Proxy))
                            {
                                poz.UM = serv.UM_Proxy;
                            }
                            else
                            {
                                poz.UM = "BUC";
                            }

                            poz.IdCotaTVA = serv.IdCotaTVA;
                            poz.ProcentTVA = serv.ProcentTVA;
                            poz.CodCotaTVA = serv.CodCotaTVA;
                            if (!String.IsNullOrEmpty(serv.ObsFactura) && !poz.Denumire.Contains(serv.ObsFactura))
                            {
                                poz.Denumire += " " + serv.ObsFactura;
                                poz.NumeInitial = poz.NumeInitial ?? "";
                                poz.NumeInitial += " " + serv.ObsFactura;
                            }
                            pozitii.Add(poz);
                        }
                        else
                        {
                            poz.NumeInitial = esteDispozitie ? "Restituire " + serv.GrupaFact1 : serv.GrupaFact1;
                            if (!AdaugaInformatiiCamereTuristi)
                            {
                                if (serv.EsteCazare)
                                {
                                    string p = " (" + serv.DataStart.ToString("dd") + "-" + serv.DataEnd.ToString("dd.MM") + ")";
                                    string c = serv.Camera;
                                    if (!poz.Denumire.Contains(p))
                                    {
                                        poz.Denumire += " " + p;
                                    }
                                    if (!poz.Denumire.Contains(c))
                                    {
                                        poz.Denumire += " " + c;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(serv.UM_Proxy))
                            {
                                poz.UM = serv.UM_Proxy;
                            }
                            else
                            {
                                poz.UM = "BUC";
                            }
                        }
                        poz.Servicii.Add(serv);
                        if (!String.IsNullOrEmpty(serv.ObsFactura) && !poz.Denumire.Contains(serv.ObsFactura))
                        {
                            poz.Denumire += " " + serv.ObsFactura;
                            poz.NumeInitial = poz.NumeInitial ?? "";
                            poz.NumeInitial += " " + serv.ObsFactura;
                        }
                    }
                    serv.Selectat = true;
                }
                foreach (EntitateServiciu serv in entitateServiciiLista.Where(x => !x.Selectat))
                {
                    poz = null;
                    if (serv.IdGrupaFact1 != 0)
                    {
                        // il adauga in grupa 1
                        poz = pozitii.FirstOrDefault(x => x.IdGrupaFact == serv.IdGrupaFact1 && x.IdCotaTVA == serv.IdCotaTVA);
                        if (poz == null)
                        {
                            poz = new Pozitie();
                            poz.IdGrupaFact = serv.IdGrupaFact1;
                            poz.NumeInitial = esteDispozitie ? "Restituire " + serv.GrupaFact1 : serv.GrupaFact1;
                            if (!AdaugaInformatiiCamereTuristi)
                            {
                                if (esteDispozitie)
                                {
                                    poz.Denumire = "Restituire " + serv.GrupaFact1;
                                }
                                else
                                {
                                    poz.Denumire = serv.GrupaFact1;
                                }
                            }

                            if (!string.IsNullOrEmpty(serv.UM_Proxy))
                            {
                                poz.UM = serv.UM_Proxy;
                            }
                            else
                            {
                                poz.UM = "BUC";
                            }

                            poz.IdCotaTVA = serv.IdCotaTVA;
                            poz.ProcentTVA = serv.ProcentTVA;
                            poz.CodCotaTVA = serv.CodCotaTVA;
                            if (!String.IsNullOrEmpty(serv.ObsFactura) && !poz.Denumire.Contains(serv.ObsFactura))
                            {
                                poz.Denumire += " " + serv.ObsFactura;
                                poz.NumeInitial = poz.NumeInitial ?? "";
                                poz.NumeInitial += " " + serv.ObsFactura;
                            }
                            pozitii.Add(poz);
                        }
                    }
                    else
                    {
                        // il adaug singur intr-o pozitie
                        poz = new Pozitie();
                        poz.IdGrupaFact = Pozitie.IdNeg;
                        poz.NumeInitial = esteDispozitie ? "Restituire " + serv.DenumireServiciu : serv.DenumireServiciu;
                        if (!AdaugaInformatiiCamereTuristi)
                        {
                            if (esteDispozitie)
                            {
                                poz.Denumire = "Restituire " + serv.DenumireServiciu;
                            }
                            else
                            {
                                poz.Denumire = serv.DenumireServiciu;
                            }
                        }

                        if (!string.IsNullOrEmpty(serv.UM_Proxy))
                        {
                            poz.UM = serv.UM_Proxy;
                        }
                        else
                        {
                            poz.UM = "BUC";
                        }

                        poz.IdCotaTVA = serv.IdCotaTVA;
                        poz.ProcentTVA = serv.ProcentTVA;
                        poz.CodCotaTVA = serv.CodCotaTVA;
                        pozitii.Add(poz);
                    }
                    poz.Servicii.Add(serv);
                    if (!String.IsNullOrEmpty(serv.ObsFactura) && !poz.Denumire.Contains(serv.ObsFactura))
                    {
                        poz.Denumire += " " + serv.ObsFactura;
                        poz.NumeInitial = poz.NumeInitial ?? "";
                        poz.NumeInitial += " " + serv.ObsFactura;
                    }
                    serv.Selectat = true;
                }
                CalculPozitiiNew();
            }
        
        private static void CalculPozitiiNew()
        {
            //EntitateMoneda mon;
            decimal val, valtva, valftva, totval = 0, totvaltva = 0, totvalftva = 0;
            for (int i = 0; i < pozitii.Count; i++)
            {
                pozitii[i].Ordine = i + 1;
                if (!pozitii[i].EsteDiferentaCurs)
                {
                    pozitii[i].ValoareTotalaRON = 0;
                    foreach (EntitateServiciu es in pozitii[i].Servicii.Where(x => !x.Sters))
                    {
                        es.PlatitRon = listaPlatiServicii.Where(x => x.IdRezervareServiciu == es.ID).Sum(x => x.ValoareRON);
                        es.PlatitMoneda = listaPlatiServicii.Where(x => x.IdRezervareServiciu == es.ID).Sum(x => x.ValoareMonedaServ);

                            val = es.PlatitRon - es.FacturatRon;

                            //mon = monede.FirstOrDefault(x => x.ID == es.IdMoneda);
                            //if (!mon.EsteNationala)
                            //{
                                //val = Math.Round(val * es.Curs, 2);
                                val = es.PlatitRon - es.FacturatRon;
                            //}
                            pozitii[i].ValoareTotalaRON += val;

                            es.DeFactMonedaServ = es.PlatitMoneda - es.FacturatMoneda;
                            es.DeFactRON = es.PlatitRon - es.FacturatRon;
                    }
                }

                valftva = pozitii[i].ValoareTotalaRON / (1.0m + pozitii[i].ProcentTVA / 100.0m);
                valtva = valftva * pozitii[i].ProcentTVA / 100.0m;

                pozitii[i].ValoareFTVARON = Math.Round(valftva, 2);
                pozitii[i].ValoareTVARON = Math.Round(valtva, 2);

                if (pozitii[i].Cantitate == 0)
                {
                    pozitii[i].Cantitate = 1;
                }
                pozitii[i].PretRON = Math.Round(pozitii[i].ValoareFTVARON / pozitii[i].Cantitate, 3);

                //2014-05-29 Traian
                if (pozitii[i].Cantitate > 0 && pozitii[i].PretRON < 0)
                {
                    pozitii[i].Cantitate = -pozitii[i].Cantitate;
                    pozitii[i].PretRON = -pozitii[i].PretRON;
                }
                //2014-05-29

                totvaltva += pozitii[i].ValoareTVARON;
                totvalftva += pozitii[i].ValoareFTVARON;

                pozitii[i].ValoareTotalInitialaRON = pozitii[i].ValoareTotalaRON;
            }

            for (int i = 0; i < pozitii.Count; i++)
            {
                decimal totalVirtualPozitie = pozitii[i].Servicii.Where(x => !x.Sters).Sum(x => Math.Abs(x.DeFactRON));
                foreach (EntitateServiciu es in pozitii[i].Servicii.Where(x => !x.Sters))
                {
                    if (totalVirtualPozitie == 0)
                    {
                        es.PonderePozitieFactura = 100 / pozitii[i].Servicii.Count();
                    }
                    else
                    {
                        es.PonderePozitieFactura = Math.Abs(es.DeFactRON) / totalVirtualPozitie;
                    }
                    //es.PonderePozitieFactura = es.DeFactRON / pozitii[i].ValoareTotalaRON;
                }
            }

            totval = totvalftva + totvaltva;

            //bs.DataSource = pozitii;
            //if (dg.DataSource == null)
            //{
            //    dg.DataSource = bs;
            //}
            //else
            //{
            //    bs.ResetBindings(false);
            //}
            Console.WriteLine(totvalftva + " " + totvaltva + " " + totval);
        }

        private static void SalveazaFactura(bool emite, int IdClient, string serieFactura, string numarFactura, int IdRezervare)
        {
            bool ok = true;
            EntitateMoneda mon = ConexiuneDB.moneda; //cmbMoneda.SelectedItem as EntitateMoneda;
            //EntitateCarnetFacturi Carnet = EntitateCarnetFacturi.getCarnet();
            NomParteneri partenerFurnizor = NomParteneri.GetLista(ConexiuneDB.Carnet.IdFurnizor);
            NomParteneri partenerClient = NomParteneri.GetLista(IdClient);

            int IdFurnizor = ConexiuneDB.Carnet.IdFurnizor;
            foreach (Pozitie poz in pozitii)
            {
                if (mon.EsteNationala)
                {
                    poz.PretMoneda = poz.PretRON;
                    poz.ValoareFTVAMoneda = poz.ValoareFTVARON;
                    poz.ValoareTVAMoneda = poz.ValoareTVARON;
                    poz.ValoareTotalaMoneda = poz.ValoareTotalaRON;
                }
                else
                {
                    poz.PretMoneda = Math.Round(poz.PretRON / mon.CursZi, 2);
                    poz.ValoareFTVAMoneda = Math.Round(poz.ValoareFTVARON / mon.CursZi, 2);
                    poz.ValoareTVAMoneda = Math.Round(poz.ValoareTVARON / mon.CursZi, 2);
                    poz.ValoareTotalaMoneda = Math.Round(poz.ValoareTotalaRON / mon.CursZi, 2);
                }
            }

            int idPL = 0;
            EntitatePL epl = ConexiuneDB.cmbPunctLucru;
            
            if (epl != null)
            {
                idPL = epl.ID;
            }
            else
            {
                epl = new EntitatePL() { ID = 0 };
            }

            List<string> cfs = new List<string>();

            string conturiFurnizor = "";
            if (cfs.Count > 0)
            {
                conturiFurnizor = cfs.Aggregate((x1, x2) => x1 + ";" + x2);
            }
            string conturiClient = "";
           

            decimal cursEUR = 0;
            decimal cursUSD = 0;

            string emailPartener = "";
            string emailTurist = "";

            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    using (SqlTransaction tran = cnn.BeginTransaction())
                    {
                        try
                        {
                            int idFactura = 0;
                            string sql = @"
                                INSERT INTO [SOLON.H].[financiar].[Facturi]
                                    ([IdHotel]
                                    ,[IdFurnizor]
                                    ,[IdPunctLucru]
                                    ,[IdClient]
                                    ,[SediuClient]
                                    ,[Data]
                                    ,[Serie]
                                    ,[Numar]
                                    ,[IdMoneda]
                                    ,[CursZi]
                                    ,[ExpDelegat]
                                    ,[ExpAct]
                                    ,[ExpTransport]
                                    ,[ExpData]
                                    ,[Aviz]
                                    ,[DataScadenta]
                                    ,[TotalValoareMoneda]
                                    ,[TotalTVAMoneda]
                                    ,[TotalGeneralMoneda]
                                    ,[TotalValoareRON]
                                    ,[TotalTVARON]
                                    ,[TotalGeneralRON]
                                    ,[ObsAntet]
                                    ,[ObsSubsol]
                                    ,[Intocmita]
                                    ,[Emisa]
                                    ,[IdUserEmis]
                                    ,[DataOraEmisa]
                                    ,[Anulata]
                                    ,[DataOraCreare]
                                    ,[DataOraModificare]
                                    ,[IdRezervare]
                                    ,[IdCarnet]
                                    ,[TVAI_ExigibilBaza]
                                    ,[TVAI_ExigibilTVA]
                                    ,[TVAI_LaIncasareBaza]
                                    ,[TVAI_LaIncasareTVA]
                                    ,[IdChitanta]
                                    ,[TaxareInversa]
                                    ,[FolosesteCursuriEURUSD]
                                    ,[CursEUR]
                                    ,[CursUSD])
                                VALUES
                                    (@IdHotel
                                    ,@IdFurnizor
                                    ,@IdPunctLucru
                                    ,@IdClient
                                    ,@SediuClient
                                    ,@Data
                                    ,@Serie
                                    ,@Numar
                                    ,@IdMoneda
                                    ,@CursZi
                                    ,@ExpDelegat
                                    ,@ExpAct
                                    ,@ExpTransport
                                    ,@ExpData
                                    ,@Aviz
                                    ,@DataScadenta
                                    ,@TotalValoareMoneda
                                    ,@TotalTVAMoneda
                                    ,@TotalGeneralMoneda
                                    ,@TotalValoareRON
                                    ,@TotalTVARON
                                    ,@TotalGeneralRON
                                    ,@ObsAntet
                                    ,@ObsSubsol
                                    ,@Intocmita
                                    ,1
                                    ,@IdUserEmis
                                    ,getdate()
                                    ,0
                                    ,getdate()
                                    ,getdate()
                                    ,@IdRezervare
                                    ,@IdCarnet
                                    ,@TVAIExigibilBaza
                                    ,@TVAIExigibilTVA
                                    ,@TVAILaIncasareBaza
                                    ,@TVAILaIncasareTVA
                                    ,@IdChitanta
                                    ,@TaxareInversa
                                    ,@FolosesteCursuriEURUSD
                                    ,@CursEUR
                                    ,@CursUSD);
                                SELECT SCOPE_IDENTITY();
                            ";
                            SqlCommand cmd = new SqlCommand(sql, cnn, tran);
                            cmd.Parameters.Add(new SqlParameter("@IdHotel", SqlDbType.Int)).Value = ConexiuneDB.IdHotel;
                            cmd.Parameters.Add(new SqlParameter("@IdFurnizor", SqlDbType.Int)).Value = IdFurnizor;
                            cmd.Parameters.Add(new SqlParameter("@IdPunctLucru", SqlDbType.Int)).Value = idPL == 0 ? DBNull.Value : (object)idPL;
                            cmd.Parameters.Add(new SqlParameter("@IdClient", SqlDbType.Int)).Value =  IdClient;
                            cmd.Parameters.Add(new SqlParameter("@SediuClient", SqlDbType.NVarChar)).Value = partenerClient.Judet + " " + partenerClient.Adresa;// txtClientSediul.Text;
                            cmd.Parameters.Add(new SqlParameter("@Data", SqlDbType.DateTime)).Value = ConexiuneDB.DataLucr; //dtpData.ValoareData.Date;
                            cmd.Parameters.Add(new SqlParameter("@Serie", SqlDbType.NVarChar)).Value = serieFactura;     //  txtSeria.Text.Trim();
                            cmd.Parameters.Add(new SqlParameter("@Numar", SqlDbType.NVarChar)).Value = numarFactura;     //  txtNumar.Text.Trim();
                            cmd.Parameters.Add(new SqlParameter("@IdMoneda", SqlDbType.Int)).Value = mon.ID;
                            cmd.Parameters.Add(new SqlParameter("@CursZi", SqlDbType.Decimal)).Value = ConexiuneDB.CursValutar;// txtCurs.ValoareNumerica;
                            cmd.Parameters.Add(new SqlParameter("@ExpDelegat", SqlDbType.NVarChar)).Value = partenerClient.Nume + " " + partenerClient.Prenume;// txtExpDelegat.Text;
                            cmd.Parameters.Add(new SqlParameter("@ExpAct", SqlDbType.NVarChar)).Value = partenerClient.Cod; // txtExpCI.Text;
                            cmd.Parameters.Add(new SqlParameter("@ExpTransport", SqlDbType.NVarChar)).Value = ConexiuneDB.Transport;   //txtExpTransport.Text;
                            cmd.Parameters.Add(new SqlParameter("@ExpData", SqlDbType.DateTime)).Value = ConexiuneDB.DataLucr; //din baza SalveazaFactura incar perioada //dtpDataExp.ValoareData.Date;
                            cmd.Parameters.Add(new SqlParameter("@Aviz", SqlDbType.NVarChar)).Value = "";// txtAviz.Text;
                            cmd.Parameters.Add(new SqlParameter("@DataScadenta", SqlDbType.DateTime)).Value = ConexiuneDB.TermenPlata; // modifica si aici din baza dtpDataScadenta.ValoareData.Date;
                            cmd.Parameters.Add(new SqlParameter("@TotalValoareMoneda", SqlDbType.Decimal)).Value = pozitii.Sum(x => x.ValoareFTVAMoneda);
                            cmd.Parameters.Add(new SqlParameter("@TotalTVAMoneda", SqlDbType.Decimal)).Value = pozitii.Sum(x => x.ValoareTVAMoneda);
                            cmd.Parameters.Add(new SqlParameter("@TotalGeneralMoneda", SqlDbType.Decimal)).Value = pozitii.Sum(x => x.ValoareTotalaMoneda);
                            cmd.Parameters.Add(new SqlParameter("@TotalValoareRON", SqlDbType.Decimal)).Value = pozitii.Sum(x => x.ValoareFTVARON);
                            cmd.Parameters.Add(new SqlParameter("@TotalTVARON", SqlDbType.Decimal)).Value = pozitii.Sum(x => x.ValoareTVARON);
                            cmd.Parameters.Add(new SqlParameter("@TotalGeneralRON", SqlDbType.Decimal)).Value = pozitii.Sum(x => x.ValoareTotalaRON);   // DACA NU BINE TRAGETI-L DE URECHI PE ADRIAN!
                            cmd.Parameters.Add(new SqlParameter("@ObsAntet", SqlDbType.NVarChar)).Value = "";// txtObsAntet.Text;
                            cmd.Parameters.Add(new SqlParameter("@ObsSubsol", SqlDbType.NVarChar)).Value = "";// txtObsSubsol.Text;
                            cmd.Parameters.Add(new SqlParameter("@Intocmita", SqlDbType.NVarChar)).Value = "";// txtIntocmita.Text;
                            cmd.Parameters.Add(new SqlParameter("@IdUserEmis", SqlDbType.Int)).Value = 0;// LoginInfo.IdUtilizator;
                            cmd.Parameters.Add(new SqlParameter("@IdRezervare", SqlDbType.Int)).Value = IdRezervare;
                            cmd.Parameters.Add(new SqlParameter("@IdCarnet", SqlDbType.Int)).Value = ConexiuneDB.Carnet.ID;
                            cmd.Parameters.Add(new SqlParameter("@TVAIExigibilBaza", SqlDbType.Decimal)).Value = 0; // this.TVAI_ExigibilBaza;
                            cmd.Parameters.Add(new SqlParameter("@TVAIExigibilTVA", SqlDbType.Decimal)).Value = 0; // this.TVAI_ExigibilTVA;
                            cmd.Parameters.Add(new SqlParameter("@TVAILaIncasareBaza", SqlDbType.Decimal)).Value = 0; // this.TVAI_LaIncasareBaza;
                            cmd.Parameters.Add(new SqlParameter("@TVAILaIncasareTVA", SqlDbType.Decimal)).Value = 0; // this.TVAI_LaIncasareTVA;
                            cmd.Parameters.Add(new SqlParameter("@IdChitanta", SqlDbType.Int)).Value = 0; //ChitFactImp ? IdPlata : 0;
                            cmd.Parameters.Add(new SqlParameter("@TaxareInversa", SqlDbType.Bit)).Value = false; // chkTaxare.Checked;
                            cmd.Parameters.Add(new SqlParameter("@FolosesteCursuriEURUSD", SqlDbType.Bit)).Value = false; // LoginInfo.FolosesteCursuri_EUR_USD_Factura;
                            cmd.Parameters.Add(new SqlParameter("@CursEUR", SqlDbType.Decimal)).Value = cursEUR;
                            cmd.Parameters.Add(new SqlParameter("@CursUSD", SqlDbType.Decimal)).Value = cursUSD;

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    idFactura = reader[0] == DBNull.Value ? 0 : Convert.ToInt32(reader[0]);
                                }
                            }

//                            foreach (EntitateCont ec in clbFurnizorConturi.CheckedItems)
//                            {
//                                sql = @"
//                                    INSERT INTO [financiar].[FacturiConturiBanci]
//                                        ([IdFactura]
//                                        ,[Cont]
//                                        ,[Banca]
//                                        ,[EsteClient])
//                                    VALUES
//                                        (@IdFactura
//                                        ,@Cont
//                                        ,@Banca
//                                        ,@EsteClient)";
//                                cmd = new SqlCommand(sql, cnn, tran);
//                                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
//                                cmd.Parameters.Add(new SqlParameter("@Cont", SqlDbType.NVarChar)).Value = ec.IBAN;
//                                cmd.Parameters.Add(new SqlParameter("@Banca", SqlDbType.NVarChar)).Value = ec.Denumire;
//                                cmd.Parameters.Add(new SqlParameter("@EsteClient", SqlDbType.Bit)).Value = false;
//                                cmd.ExecuteNonQuery();
//                            }


//                            foreach (EntitateCont ec in clbClientConturi.CheckedItems)
//                            {
//                                sql = @"
//                                    INSERT INTO [financiar].[FacturiConturiBanci]
//                                        ([IdFactura]
//                                        ,[Cont]
//                                        ,[Banca]
//                                        ,[EsteClient])
//                                    VALUES
//                                        (@IdFactura
//                                        ,@Cont
//                                        ,@Banca
//                                        ,@EsteClient)";
//                                cmd = new SqlCommand(sql, cnn, tran);
//                                cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
//                                cmd.Parameters.Add(new SqlParameter("@Cont", SqlDbType.NVarChar)).Value = ec.IBAN;
//                                cmd.Parameters.Add(new SqlParameter("@Banca", SqlDbType.NVarChar)).Value = ec.Denumire;
//                                cmd.Parameters.Add(new SqlParameter("@EsteClient", SqlDbType.Bit)).Value = true;
//                                cmd.ExecuteNonQuery();
//                            }


                            sql = @"
                                INSERT INTO [SOLON.H].[financiar].[FacturiExtra]
                                    ([IdFactura]
                                    ,[FurnizorDenumire]
                                    ,[FurnizorPunctLucru]
                                    ,[FurnizorCapitalSocial]
                                    ,[FurnizorRegCom]
                                    ,[FurnizorCIF]
                                    ,[FurnizorSediu]
                                    ,[FurnizorJudet]
                                    ,[ClientDenumire]
                                    ,[ClientRegCom]
                                    ,[ClientCIF]
                                    ,[ClientSediu]
                                    ,[ClientJudet])
                                VALUES
                                    (@IdFactura
                                    ,@FurnizorDenumire
                                    ,@FurnizorPunctLucru
                                    ,@FurnizorCapitalSocial
                                    ,@FurnizorRegCom
                                    ,@FurnizorCIF
                                    ,@FurnizorSediu
                                    ,@FurnizorJudet
                                    ,@ClientDenumire
                                    ,@ClientRegCom
                                    ,@ClientCIF
                                    ,@ClientSediu
                                    ,@ClientJudet)";
                            cmd = new SqlCommand(sql, cnn, tran);
                            cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                            cmd.Parameters.Add(new SqlParameter("@FurnizorDenumire", SqlDbType.NVarChar)).Value = partenerFurnizor.NumePartener;// txtFurnizorDenumire.Text;
                            //cmd.Parameters.Add(new SqlParameter("@FurnizorPunctLucru", SqlDbType.NVarChar)).Value = (cmbPunctLucru.SelectedItem as EntitatePL) != null ? (cmbPunctLucru.SelectedItem as EntitatePL).Denumire : "";
                            cmd.Parameters.Add(new SqlParameter("@FurnizorPunctLucru", SqlDbType.NVarChar)).Value = epl.ToString();
                            cmd.Parameters.Add(new SqlParameter("@FurnizorCapitalSocial", SqlDbType.NVarChar)).Value = partenerFurnizor.CapitalSocial;// txtFurnizorCapitalSocial.Text;
                            cmd.Parameters.Add(new SqlParameter("@FurnizorRegCom", SqlDbType.NVarChar)).Value = partenerFurnizor.RegCom;// txtFurnizorRegCom.Text;
                            cmd.Parameters.Add(new SqlParameter("@FurnizorCIF", SqlDbType.NVarChar)).Value = partenerFurnizor.CodFiscalAtribut + " " + partenerFurnizor.CodFiscalNumar;   //txtFurnizorCIF.Text;
                            cmd.Parameters.Add(new SqlParameter("@FurnizorSediu", SqlDbType.NVarChar)).Value = partenerFurnizor.Adresa;// txtFurnizorSediul.Text;
                            cmd.Parameters.Add(new SqlParameter("@FurnizorJudet", SqlDbType.NVarChar)).Value = partenerFurnizor.Judet; // txtFurnizorJudet.Text;
                            cmd.Parameters.Add(new SqlParameter("@ClientDenumire", SqlDbType.NVarChar)).Value = partenerClient.NumePartener; // txtClientDenumire.Text;
                            cmd.Parameters.Add(new SqlParameter("@ClientRegCom", SqlDbType.NVarChar)).Value = partenerClient.RegCom;// txtClientRegCom.Text;
                            cmd.Parameters.Add(new SqlParameter("@ClientCIF", SqlDbType.NVarChar)).Value = partenerClient.CodFiscalAtribut + " " + partenerClient.CodFiscalNumar;// txtClientCIF.Text;
                            cmd.Parameters.Add(new SqlParameter("@ClientSediu", SqlDbType.NVarChar)).Value = partenerClient.Adresa;// txtClientSediul.Text;
                            cmd.Parameters.Add(new SqlParameter("@ClientJudet", SqlDbType.NVarChar)).Value = partenerClient.Judet;// txtClientJudet.Text;
                            cmd.ExecuteNonQuery();

                            sql = @"
                                INSERT INTO [SOLON.H].[financiar].[FacturiPozitii]
                                    ([IdFactura]
                                    ,[Denumire]
                                    ,[UM]
                                    ,[Cantitate]
                                    ,[PretMoneda]
                                    ,[ValoareMoneda]
                                    ,[TVAMoneda]
                                    ,[PretRON]
                                    ,[ValoareRON]
                                    ,[TVARON]
                                    ,[IdCotaTVA]
                                    ,[CotaTVA]
                                    ,[Ordine]
                                    ,[EsteDiferentaCurs])
                                VALUES
                                    (@IdFactura
                                    ,@Denumire
                                    ,@UM
                                    ,@Cantitate
                                    ,@PretMoneda
                                    ,@ValoareMoneda
                                    ,@TVAMoneda
                                    ,@PretRON
                                    ,@ValoareRON
                                    ,@TVARON
                                    ,@IdCotaTVA
                                    ,@CotaTVA
                                    ,@Ordine
                                    ,@EsteDiferentaCurs);
                                SELECT SCOPE_IDENTITY();
                            ";
                            cmd = new SqlCommand(sql, cnn, tran);
                            cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                            cmd.Parameters.Add(new SqlParameter("@Denumire", SqlDbType.NVarChar));
                            cmd.Parameters.Add(new SqlParameter("@UM", SqlDbType.NVarChar));
                            cmd.Parameters.Add(new SqlParameter("@Cantitate", SqlDbType.Decimal));
                            cmd.Parameters.Add(new SqlParameter("@PretMoneda", SqlDbType.Decimal));
                            cmd.Parameters.Add(new SqlParameter("@ValoareMoneda", SqlDbType.Decimal));
                            cmd.Parameters.Add(new SqlParameter("@TVAMoneda", SqlDbType.Decimal));
                            cmd.Parameters.Add(new SqlParameter("@PretRON", SqlDbType.Decimal));
                            cmd.Parameters.Add(new SqlParameter("@ValoareRON", SqlDbType.Decimal));
                            cmd.Parameters.Add(new SqlParameter("@TVARON", SqlDbType.Decimal));
                            cmd.Parameters.Add(new SqlParameter("@IdCotaTVA", SqlDbType.Int));
                            cmd.Parameters.Add(new SqlParameter("@CotaTVA", SqlDbType.Decimal));
                            cmd.Parameters.Add(new SqlParameter("@Ordine", SqlDbType.Int));
                            cmd.Parameters.Add(new SqlParameter("@EsteDiferentaCurs", SqlDbType.Bit));
                            foreach (Pozitie poz in pozitii)
                            {
                                cmd.Parameters["@Denumire"].Value = poz.Denumire;
                                cmd.Parameters["@UM"].Value = poz.UM;
                                cmd.Parameters["@Cantitate"].Value = poz.Cantitate;
                                cmd.Parameters["@PretMoneda"].Value = poz.PretMoneda;
                                cmd.Parameters["@ValoareMoneda"].Value = poz.ValoareFTVAMoneda;
                                cmd.Parameters["@TVAMoneda"].Value = poz.ValoareTVAMoneda;
                                cmd.Parameters["@PretRON"].Value = poz.PretRON;
                                cmd.Parameters["@ValoareRON"].Value = poz.ValoareFTVARON;
                                cmd.Parameters["@TVARON"].Value = poz.ValoareTVARON;
                                cmd.Parameters["@IdCotaTVA"].Value = poz.IdCotaTVA;
                                cmd.Parameters["@CotaTVA"].Value = poz.ProcentTVA;
                                cmd.Parameters["@Ordine"].Value = poz.Ordine;
                                cmd.Parameters["@EsteDiferentaCurs"].Value = poz.EsteDiferentaCurs;
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        poz.ID = reader[0] == DBNull.Value ? 0 : Convert.ToInt32(reader[0]);
                                    }
                                }
                            }

                            sql = @"
                                INSERT INTO [SOLON.H].[financiar].[FacturiServicii]
                                    ([IdFactura]
                                    ,[IdPozitie]
                                    ,[IdRezervareServiciu]
                                    ,[ValoareMonedaFact]
                                    ,[ValoareRON]
                                    ,[ValoareMonedaServ]
                                    ,[CursServiciu]
                                    ,[CursFactura]
                                    ,[DiferentaCurs])
                                VALUES
                                    (@IdFactura
                                    ,@IdPozitie
                                    ,@IdRezervareServiciu
                                    ,@ValoareMonedaFact
                                    ,@ValoareRON
                                    ,@ValoareMonedaServ
                                    ,@CursServiciu
                                    ,@CursFactura
                                    ,@DiferentaCurs)
                            ";
                            cmd = new SqlCommand(sql, cnn, tran);
                            cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int)).Value = idFactura;
                            cmd.Parameters.Add(new SqlParameter("@IdPozitie", SqlDbType.Int));
                            cmd.Parameters.Add(new SqlParameter("@IdRezervareServiciu", SqlDbType.Int));
                            cmd.Parameters.Add(new SqlParameter("@ValoareMonedaFact", SqlDbType.Decimal));
                            cmd.Parameters.Add(new SqlParameter("@ValoareRON", SqlDbType.Decimal));
                            cmd.Parameters.Add(new SqlParameter("@ValoareMonedaServ", SqlDbType.Decimal));
                            cmd.Parameters.Add(new SqlParameter("@CursServiciu", SqlDbType.Decimal));
                            cmd.Parameters.Add(new SqlParameter("@CursFactura", SqlDbType.Decimal));
                            cmd.Parameters.Add(new SqlParameter("@DiferentaCurs", SqlDbType.Decimal));
                            //foreach (Pozitie poz in pozitii)
                            //{
                            //    cmd.Parameters["@IdPozitie"].Value = poz.ID;
                            //    foreach (FormFisa.EntitateServiciu es in poz.Servicii)
                            //    {
                            //        decimal cursBNRServ = HelperCurs.GetCurs(LoginInfo.DataLucru, es.IdMoneda, IdTipCursBNR, cnn, tran);
                            //        decimal valMonedaFact = 0, valRON = 0, valMonedaServ = 0;
                            //        EntitateMoneda monServ = monede.FirstOrDefault(x => x.ID == es.IdMoneda);
                            //        //deja comentat
                            //        //if (FacturarePlata)
                            //        //{
                            //        //    EntitatePlataServiciu eps = listaPlati.FirstOrDefault(x => x.IdRezervareServiciu == es.ID);
                            //        //    if (eps != null)
                            //        //    {
                            //        //        valMonedaServ = eps.ValoareMonedaServ;
                            //        //    }
                            //        //}
                            //        //else
                            //        //{
                            //        //    //valMonedaServ = es.Total - es.FacturatMoneda;
                            //        //    valMonedaServ = es.DeFactMonedaServ;
                            //        //}
                            //        //if (monServ.EsteNationala)
                            //        //{
                            //        //    valRON = valMonedaServ;
                            //        //}
                            //        //else
                            //        //{
                            //        //    valRON = Math.Round(valMonedaServ * es.Curs, 2);
                            //        //}
                            //        //if (mon.EsteNationala)
                            //        //{
                            //        //    valMonedaFact = valRON;
                            //        //}
                            //        //else
                            //        //{
                            //        //    valMonedaFact = Math.Round(valRON / mon.CursZi, 2);
                            //        //}

                            //        //if (FacturarePlata)
                            //        //{
                            //        //    valMonedaFact = es.DeFactMonedaFact;
                            //        //        valRON = es.DeFactRON;
                            //        //        valMonedaServ = es.DeFactMonedaServ;
                            //        //}
                            //        //else
                            //        //{
                            //        //    valMonedaFact = es.DeFactMonedaFact;
                            //        //    valRON = es.DeFactRON;
                            //        //    valMonedaServ = es.DeFactMonedaServ;
                            //        //}
                            //        //end deja comentat

                            //        valMonedaFact = es.DeFactRON;
                            //        valRON = es.DeFactRON;
                            //        valMonedaServ = es.DeFactMonedaServ;

                            //        cmd.Parameters["@IdRezervareServiciu"].Value = es.ID;
                            //        cmd.Parameters["@ValoareMonedaFact"].Value = valMonedaFact;
                            //        cmd.Parameters["@ValoareRON"].Value = valRON;
                            //        cmd.Parameters["@ValoareMonedaServ"].Value = valMonedaServ;
                            //        cmd.Parameters["@CursServiciu"].Value = es.Curs;
                            //        cmd.Parameters["@CursFactura"].Value = cursBNRServ;
                            //        cmd.Parameters["@DiferentaCurs"].Value = valRON - Math.Round(valMonedaServ * cursBNRServ, 2);

                            //        cmd.ExecuteNonQuery();
                            //    }
                            //}

                            // adaugare in 20190228
                            //intai calculez valoarea in Euro a serviciilor din pozitii la cursul facturii
                            decimal cursBNRServ = HelperCurs.GetCurs(ConexiuneDB.DataLucr, mon.ID, ConexiuneDB.IdTipCursValutar, cnn, tran);
                            //pentru a verifica daca totalul general in euro corespunde cu suma servciilor convertite in euro
                            decimal TotalDeFactMonedaFact = 0;

                            foreach (Pozitie poz in pozitii)
                            {
                                foreach (EntitateServiciu es in poz.Servicii)
                                {
                                    //verific sa nu se editeze cursul direct in factura
                                    if (!mon.EsteNationala)
                                    {
                                        cursBNRServ = mon.CursZi;
                                    }

                                    EntitateMoneda monServ = ConexiuneDB.moneda;  //monede.FirstOrDefault(x => x.ID == es.IdMoneda);

                                    if (!mon.EsteNationala)
                                    {
                                        es.DeFactMonedaFact = Math.Round(es.DeFactRON / cursBNRServ, 2);
                                    }
                                    else
                                    {
                                        es.DeFactMonedaFact = es.DeFactRON;
                                    }

                                    TotalDeFactMonedaFact += es.DeFactMonedaFact;
                                }
                            }

                            decimal difEuro = TotalDeFactMonedaFact - pozitii.Sum(x => x.ValoareTotalaMoneda);
                            //daca exista diferente ajustez serviciile pe campul DeFactMonedaFact
                            if (difEuro != 0 && difEuro > 0)
                            {
                                foreach (Pozitie poz in pozitii)
                                {
                                    foreach (EntitateServiciu es in poz.Servicii)
                                    {
                                        if (difEuro == 0)
                                        {
                                            break;
                                        }
                                        es.DeFactMonedaFact += -0.01m;
                                        difEuro += -0.01m;

                                    }
                                }
                            }
                            else
                            {
                                foreach (Pozitie poz in pozitii)
                                {
                                    foreach (EntitateServiciu es in poz.Servicii)
                                    {
                                        if (difEuro == 0)
                                        {
                                            break;
                                        }
                                        es.DeFactMonedaFact += 0.01m;
                                        difEuro += 0.01m;
                                    }
                                }
                            }
                            //terminat partea de calcul a valorii ValoareMonedaFact pentru fiecare pozitie ce se va salva in FacturiServicii.

                            foreach (Pozitie poz in pozitii)
                            {
                                cmd.Parameters["@IdPozitie"].Value = poz.ID;
                                foreach (EntitateServiciu es in poz.Servicii)
                                {

                                    decimal valMonedaFact = 0, valRON = 0, valMonedaServ = 0;

                                    valMonedaFact = es.DeFactMonedaFact;
                                    valRON = es.DeFactRON;
                                    valMonedaServ = es.DeFactMonedaServ;

                                    cmd.Parameters["@IdRezervareServiciu"].Value = es.ID;
                                    cmd.Parameters["@ValoareMonedaFact"].Value = valMonedaFact;
                                    cmd.Parameters["@ValoareRON"].Value = valRON;
                                    cmd.Parameters["@ValoareMonedaServ"].Value = valMonedaServ;
                                    cmd.Parameters["@CursServiciu"].Value = es.Curs;
                                    cmd.Parameters["@CursFactura"].Value = cursBNRServ;
                                    cmd.Parameters["@DiferentaCurs"].Value = valRON - Math.Round(valMonedaServ * cursBNRServ, 2);

                                    cmd.ExecuteNonQuery();
                                }
                            }
                            //end 20190228

                            tran.Commit();
                            int IdFactura = idFactura;
                            IdFacturaGenerata = idFactura+"";

                            Jurnal.Salveaza(
                                "SolonH.Financiar.Factura",
                                Jurnal.Actiune.ADD,
                                Jurnal.ImportantaMedie,
                                String.Format("S-a emis o factura {0} {1} / {2}, IdFactura={3}", ConexiuneDB.Carnet.Serie, ConexiuneDB.Carnet.Numar, ConexiuneDB.DataLucr.ToString("dd.MM.yyyy"), idFactura.ToString()));
                           
                            //if (CursPropus != CursAles)
                            //{
                            //    Jurnal.Salveaza(
                            //                    "SolonH.Financiar.CursDocumente",
                            //                    Jurnal.Actiune.ADD,
                            //                    Jurnal.ImportantaMedie,
                            //                    String.Format("Factura {0} {1} / {2}, IdFactura={3} a fost emisa cu cursul ales de {4}", txtSeria.Text.Trim(), txtNumar.Text.Trim(), LoginInfo.DataLucru.ToString("dd.MM.yyyy"), idFactura.ToString(), CursAles.ToString("N4")));
                            //}
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
                    ok = false;
                    LogErori.Salveaza(exc, "CheckIn2.SalveazaFactura()");
                }
            }

            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"
                                SELECT
                                     IdPartener
                                    ,MailAddress
                                FROM
                                    SOLON.dbo.NomParteneri
                                WHERE
                                    IdPartener = @IdPartener OR IdPartener = @IdTurist";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.Parameters.Add(new SqlParameter("@IdPartener", SqlDbType.Int)).Value = partenerFurnizor.IdPartener;
                    cmd.Parameters.Add(new SqlParameter("@IdTurist", SqlDbType.Int)).Value = partenerClient.IdPartener;// IdTurist;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int IdPart = Convert.ToInt32(reader["IdPartener"]);
                            if (IdPart == partenerFurnizor.IdPartener)
                            {
                                emailPartener = reader["MailAddress"].ToString();
                            }
                            else if (IdPart == partenerClient.IdPartener)
                            {
                                emailTurist = reader["MailAddress"].ToString();
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    LogErori.Salveaza(exc, "CheckIn2.SalveazaFactura()");
                }
            }

            if (ok)
            {
                HashSet<int> hsServicii = new HashSet<int>();
                foreach (Pozitie poz in pozitii)
                {
                    foreach (EntitateServiciu es in poz.Servicii)
                    {
                        if (!hsServicii.Contains(es.ID))
                        {
                            hsServicii.Add(es.ID);
                        }
                    }
                }
                HelperValoriZilniceFactura.RepartizeazaValoriServicii(hsServicii.ToList());
            }
        }

        private static void IncarcaCarnet(int IdFurnizor, List<int> IdsServicii)
        {
            try
            {
                bool esteRec = true;//din baza carnet facturi speciale pentru mine
                bool esteVir = true;
                bool esteAvans = false;
                List<int> idsMetode = new List<int>();
                HelperFactura.GetIdsMetodePlata(IdsServicii, ref idsMetode, ref esteRec, ref esteVir);
                List<EntitateCarnetFacturi> carnete = new List<EntitateCarnetFacturi>();
                carnete = HelperFactura.GetCarnete(idsMetode, esteRec, esteVir, esteAvans);
                ConexiuneDB.Carnet = carnete.FirstOrDefault();
            }
            catch (Exception exc) { }
            if (ConexiuneDB.Carnet == null)
            {
                ConexiuneDB.Carnet = EntitateCarnetFacturi.getCarnetOld();
            }
            ConexiuneDB.serieFactura = ConexiuneDB.Carnet.Serie;
            try
            {
                if (ConexiuneDB.Carnet != null)
                {
                    long NrCurent;
                    long LastNumar;
                    if (long.TryParse(ConexiuneDB.Carnet.Numar, out NrCurent))
                    {
                        if (long.TryParse(ConexiuneDB.Carnet.NumarPanaLa, out LastNumar))
                        {
                            if (LastNumar - NrCurent <= 20)
                            {
                                SendEmail("Mesaj generat automat aplicatie Self Hotel", string.Format("ATENTIE!!! {0} Mai aveti un numar de {1} facturi pe carnetul {2} {3}{4}-{3}{5}!", Environment.NewLine, LastNumar - NrCurent, ConexiuneDB.Carnet.NumeFurnizor, ConexiuneDB.Carnet.Serie, ConexiuneDB.Carnet.NumarDeLa, ConexiuneDB.Carnet.NumarPanaLa), ConexiuneDB.AdminMail);
                                //Mesaj.Info(string.Format("ATENTIE!!! {0} Mai aveti un numar de {1} facturi pe carnetul {2} {3}{4}-{3}{5}!", Environment.NewLine, LastNumar - NrCurent, Carnet.NumeFurnizor, Carnet.Serie, Carnet.NumarDeLa, Carnet.NumarPanaLa));
                            }
                            if (NrCurent == LastNumar)
                            {
                                InchideCarnet();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { LogErori.Salveaza(ex, "CheckIn2.IncarcaCarnet()"); }
        }

        private static void InchideCarnet(){
            EntitateCarnetFacturi.setCarnetConsumat(ConexiuneDB.Carnet.ID);
        }
    }
}