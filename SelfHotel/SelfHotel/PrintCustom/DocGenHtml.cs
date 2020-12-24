using HtmlAgilityPack;
using SelfHotel.Nomenclatoare;
using SelfHotel.Nomenclatoare_Final;
using SelfHotel.NomenclatoareNew;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using ZipEscort.Setari;

namespace SolonCloud.Reporting
{
    public class DocGenHtml
    {
        public string LocatieFisierNou { get; private set; }
        public List<SelfHotel.GUI.CHECKIN.CheckIn2.EntitateFisa> fise { get; set; }
        public SelfHotel.GUI.CHECKOUT.PlataForm2.ClasaFacturare fise2 { get; set; }
        public string masini { get; set; }

        public int Top { get; set; }
        public int Bottom { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }

        public bool checkIn { get; set; }

        public HtmlDocument HtmlHeader;
        public HtmlDocument HtmlBody;
        public HtmlDocument HtmlFooter;

        public DateTime Timestamp;

        public string BodyFilename = "";

        public bool FolosesteHeader { get; set; }
        public bool FolosesteFooter { get; set; }

        public string CaleHeader { get; private set; }
        public string CaleBody { get; private set; }
        public string CaleFooter { get; private set; }

        private static string DefaultHeaderFilename = "header.html";
        private static string DefaultFooterFilename = "footer.html";

        private static string _AppDirectory = null;
        public static string AppDirectory
        {
            get
            {
                if (_AppDirectory == null)
                {
                    _AppDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                    if (_AppDirectory.StartsWith(@"file:\") || _AppDirectory.StartsWith(@"file:/"))
                    {
                        _AppDirectory = _AppDirectory.Substring(@"file:\".Length);
                    }
                }
                return _AppDirectory;
            }
        }

        private static string _TemplatesDirectory = null;
        public static string TemplatesDirectory
        {
            get
            {
                if (_TemplatesDirectory == null)
                {
                    //_TemplatesDirectory = AppDirectory;
                    _TemplatesDirectory = Path.Combine(Path.GetDirectoryName( AppDirectory), "PrintCustom");
                    //_TemplatesDirectory = Path.Combine(AppDirectory, "Rapoarte");
                    //_TemplatesDirectory = Path.Combine(_TemplatesDirectory, "Templates");
                }
                return _TemplatesDirectory;
            }
        }

        private static string _DocumentsDirectory = null;
        public static string DocumentsDirectory
        {
            get
            {
                if (_DocumentsDirectory == null)
                {
                    //_DocumentsDirectory = ConfigurationManager.AppSettings["DocumentsDirectory"];
                    //_DocumentsDirectory = AppDirectory;
                    _DocumentsDirectory = Path.Combine(Path.GetDirectoryName(AppDirectory), "PrintCustom");
                }
                return _DocumentsDirectory;
            }
        }

        private static string _WkHtmlToPdfPath = null;
        public static string WkHtmlToPdfPath
        {
            get
            {
                if (_WkHtmlToPdfPath == null)
                {
                    _WkHtmlToPdfPath = Path.Combine(DocumentsDirectory, "wkhtmltopdf.exe");
                }
                return _WkHtmlToPdfPath;
            }
        }

        public DocGenHtml(string bodyFilename, List<SelfHotel.GUI.CHECKIN.CheckIn2.EntitateFisa> i, string masini, bool cki)
        {
            this.Timestamp = DateTime.Now;
            this.fise = i;
            this.masini = masini;

            this.checkIn = cki;

            this.BodyFilename = bodyFilename;

            this.FolosesteHeader = true;
            this.FolosesteFooter = true;

            this.CaleHeader = Path.Combine(TemplatesDirectory, DefaultHeaderFilename);
            this.CaleBody = Path.Combine(TemplatesDirectory, BodyFilename);
            this.CaleFooter = Path.Combine(TemplatesDirectory, DefaultFooterFilename);

            HtmlHeader = new HtmlDocument();
            HtmlBody = new HtmlDocument();
            HtmlFooter = new HtmlDocument();

            HtmlHeader.Load(CaleHeader, Encoding.UTF8);
            HtmlBody.Load(CaleBody, Encoding.UTF8);
            HtmlFooter.Load(CaleFooter, Encoding.UTF8);

            ReplaceScalarHeader("dataEmitere", Timestamp.ToString("dd.MM.yyyy HH:mm"));

            ReplaceScalarFooter("verSC", "1.0");

            Top = Bottom = Left = Right = -1;
        }

        public DocGenHtml(string bodyFilename, SelfHotel.GUI.CHECKOUT.PlataForm2.ClasaFacturare i, string masini, bool cki)
        {
            this.Timestamp = DateTime.Now;
            this.fise2 = i;
            this.masini = masini;
            this.checkIn = cki;

            this.BodyFilename = bodyFilename;

            this.FolosesteHeader = true;
            this.FolosesteFooter = true;

            this.CaleHeader = Path.Combine(TemplatesDirectory, DefaultHeaderFilename);
            this.CaleBody = Path.Combine(TemplatesDirectory, BodyFilename);
            this.CaleFooter = Path.Combine(TemplatesDirectory, DefaultFooterFilename);

            HtmlHeader = new HtmlDocument();
            HtmlBody = new HtmlDocument();
            HtmlFooter = new HtmlDocument();

            HtmlHeader.Load(CaleHeader, Encoding.UTF8);
            HtmlBody.Load(CaleBody, Encoding.UTF8);
            HtmlFooter.Load(CaleFooter, Encoding.UTF8);

            ReplaceScalarHeader("dataEmitere", Timestamp.ToString("dd.MM.yyyy HH:mm"));

            ReplaceScalarFooter("verSC", "1.0");

            Top = Bottom = Left = Right = -1;
        }

        public void Salveaza()
        {
            Salveaza("", "", "");
        }

        private void Modifica(string template)
        {
            if (File.Exists(template))
            {
                StreamReader sr = new StreamReader(template);
                string content = sr.ReadToEnd();
                sr.Close();
                StreamWriter sw = new StreamWriter(template);
                String camere = String.Empty;
                Decimal totalFaraTVA = 0;
                Decimal totalTVAProcent = 0;
                if (checkIn)
                {
                    foreach (SelfHotel.GUI.CHECKIN.CheckIn2.EntitateFisa fc in this.fise)
                    {
                        camere += "<table id='tg-OSBii' class='tg'>" +
                          "<tr>" +
                            "<th class='tg-0lax' colspan='4'>" + fc.IdCam + "&nbsp;&nbsp;" + fc.TipCam + "</th>" +
                          "</tr>" +
                          "<tr>" +
                            "<td class='tg-xldj' colspan='4'>Numele si Prenumele<br>Surname and Christian Name<br>Name und Vorname<br>Nom et Prenom   " + fc.NumePrenume + "</td>" +
                          "</tr>" +
                         " <tr>" +
                            "<td class='tg-xldj' colspan='2'>Data Nasterii<br>Date of birth<br>Geburtsdatum<br>Date de naissance           " + fc.DataNastere + "</td>" +
                            "<td class='tg-xldj'>Locul Nasterii<br>Place of birth<br>Gebertsort<br>Lieu de naissance  " + fc.LocNastere + "</td>" +
                           " <td class='tg-xldj'>Cetatenie<br>Nationality<br>Staatsangehorigkeit<br>Nationalite                     " + fc.Cetatenie + "</td>" +
                         " </tr>" +
                         " <tr>" +
                           " <td class='tg-xldj'>Domiciliu:Localitatea<br>Residence:Locality<br>Wohnsitz:Ort<br>Domicile:Localite         " + fc.Localitatea + "</td>" +
                           " <td class='tg-xldj'>Strada<br>Street<br>Strasse<br>Rue            " + fc.Strada + "</td>" +
                           " <td class='tg-xldj'>Nr.<br>No.<br>Nr.<br>No.    " + fc.NrStrada + "</td>" +
                           " <td class='tg-xldj'>Tara<br>Country<br>Land<br>Pays                " + fc.Tara + "</td>" +
                         " </tr>" +
                        "  <tr>" +
                        "    <td class='tg-xldj'>Data sosirii<br>Date of arrival<br>Ankunftsdatum<br>Date d'arrivee       " + fc.DataSosire + "</td>" +
                         "   <td class='tg-xldj'>Data plecarii<br>Date of departure<br>Abreisedatum<br>Date de depart      " + fc.DataPlecare + "</td>" +
                         "   <td class='tg-xldj' colspan='2'>Scopul calatoriei in Romania<br>Porpose of travelling to Romania<br>Zweck der reise nach Rumanien<br>Motive de votre voyage en Roumanie   " + fc.Scop + "</td>" +
                         " </tr>" +
                         " <tr>" +
                         "   <td class='tg-xldj'>Act de identitate<br>Identity card<br>Personalausweis<br>Papiers d'identite    " + fc.ActIdentitate + "</td>" +
                         "   <td class='tg-xldj'>Seria<br>Series<br>Serie<br>Serie     " + fc.Seria + "</td>" +
                         "   <td class='tg-xldj' colspan='2'>Nr.<br>No.<br>Nr.<br>No.   " + fc.NrAct + "</td>" +
                         " </tr>" +
                         " <tr>" +
                         "   <td class='tg-xldj' colspan='2'>Semnatura turistului<br>Tourist's signature<br>Interschrift des touristen<br>Signature du touriste</td>" +
                         "   <td class='tg-xldj' colspan='2'>Semnatura receptionerlui<br>Receptionit's signature<br>Unterschrift des empfangschefs<br>Signature du receptionannaire</td>" +
                         " </tr>" +
                        "</table>" +
                        "<hr>";
                    }
                }
                else
                {
                    int contor = 1;
                    totalFaraTVA = 0;
                    totalTVAProcent = 0;
                    NomParteneri partener = NomParteneri.GetLista(this.fise2.client);
                    if (partener == null)
                    {
                        partener = new NomParteneri();
                    }
                    foreach (RezervariCamere cB in this.fise2.listaCamere)
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
                            decimal totalServiciuFaraTVA=serviciu.TotalRON - serviciu.ProcentTVA*serviciu.TotalRON/100;
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
                    //EntitateCarnetFacturi ecf = EntitateCarnetFacturi.getCarnet();
                    NomParteneri partenerFurnizor = NomParteneri.GetLista(ConexiuneDB.Carnet.IdFurnizor);

                    content = content.Replace("$[Furnizor]", !String.IsNullOrEmpty(partenerFurnizor.NumePartener) && partenerFurnizor.NumePartener!="null" ? partenerFurnizor.NumePartener : "---");
                    content = content.Replace("$[Capitalsocial]", !String.IsNullOrEmpty(partenerFurnizor.CapitalSocial) && partenerFurnizor.CapitalSocial != "null" ? partenerFurnizor.CapitalSocial : "---");
                    content = content.Replace("$[Regcom]", !String.IsNullOrEmpty(partenerFurnizor.RegCom) && partenerFurnizor.RegCom != "null" ? partenerFurnizor.RegCom : "---");
                    content = content.Replace("$[Cif]", partenerFurnizor.CodFiscalAtribut + " " + partenerFurnizor.CodFiscalNumar);
                    content = content.Replace("$[Sediul]", partenerFurnizor.Judet + " " + partenerFurnizor.Oras + " " + partenerFurnizor.Strada + " " + partenerFurnizor.Nr + " " + partenerFurnizor.Bloc);
                    content = content.Replace("$[Cont]", !String.IsNullOrEmpty(partenerFurnizor.ContBanca) && partenerFurnizor.ContBanca != "null" ? partenerFurnizor.ContBanca : "---");
                    //content = content.Replace("$[Numar]", this.fise2.listaCamere[0].IdRezervare.ToString());
                    content = content.Replace("$[TVA]", Math.Round(Convert.ToDecimal(totalTVAProcent), 2).ToString());
                    content = content.Replace("$[Cumparator]", !String.IsNullOrEmpty(partener.NumePartener) && partener.NumePartener != "null" ? partener.NumePartener : "---");
                    content = content.Replace("$[RegComcumparator]", String.IsNullOrEmpty(partener.RegCom) || partener.RegCom=="null" ? "---":partener.RegCom);
                    content = content.Replace("$[Cifcumparator]", String.IsNullOrEmpty(partener.CodFiscalNumar) || partener.CodFiscalNumar == "null" ? "---" : partener.CodFiscalNumar);
                    content = content.Replace("$[TotalFaraTVA]", totalFaraTVA + "");
                    content = content.Replace("$[Total]", Math.Round((Convert.ToDecimal(totalFaraTVA + totalTVAProcent)), 2).ToString());
                    content = content.Replace("$[Sediulcumparator]", String.IsNullOrEmpty(partener.Strada) || partener.Strada == "null" ? "---" : partener.Strada);
                    content = content.Replace("$[PL]",partenerFurnizor.Judet+" "+partenerFurnizor.Strada+ " "+partenerFurnizor.Nr);
                    content = content.Replace("$[ContCumparator]", String.IsNullOrEmpty(partener.ContBanca) || partener.ContBanca == "null" ? "---" : partener.ContBanca);
                    content = content.Replace("$[Seria]", ConexiuneDB.serieFactura);
                    content = content.Replace("$[Numar]", ConexiuneDB.numarFactura);
                    content = content.Replace("$[ObsSubsol]", ConexiuneDB.obsFactura);
                    content = content.Replace("$[Transport]", ConexiuneDB.Transport);
                    content = content.Replace("$[BancaCumparator]", String.IsNullOrEmpty(partener.Banca) || partener.Banca == "null" ? "---" : partener.Banca);
                    content = content.Replace("$[Delegat]", partener.Nume + " " + partener.Prenume);
                    content = content.Replace("$[CIDelegat]", String.IsNullOrEmpty(partener.CodFiscalNumar) || partener.CodFiscalNumar == "null" ? "---" : partener.CodFiscalNumar);
                    content = content.Replace("$[Termendeplata]", ConexiuneDB.TermenPlata.ToShortDateString());
                }

                content = content.Replace("$[Camere]", camere);

                content = content.Replace("$[Data]", DateTime.Today.ToShortDateString());
                
                sw.Write(content);
                sw.Close();
            }
        } 

        public void Salveaza(string caleFisierPdf, string tipPagina, string orientare)
        {
            string headerPath = Path.Combine(DocumentsDirectory, String.Format("Header_{0}.html", Timestamp.ToString("yyyy-MM-dd_HH-mm-ss-fff")));
            if (FolosesteHeader)
            {
                HtmlHeader.Save(headerPath, Encoding.UTF8);
            }
            string bodyPath = Path.Combine(DocumentsDirectory, String.Format("Body_{0}.html", Timestamp.ToString("yyyy-MM-dd_HH-mm-ss-fff")));
            
            HtmlBody.Save(bodyPath, Encoding.UTF8);
            Modifica(bodyPath);

            string footerPath = Path.Combine(DocumentsDirectory, String.Format("Footer_{0}.html", Timestamp.ToString("yyyy-MM-dd_HH-mm-ss-fff")));
            if (FolosesteFooter)
            {
                HtmlFooter.Save(footerPath, Encoding.UTF8);
            }

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = WkHtmlToPdfPath;
            List<string> arguments = new List<string>();
            arguments.Add("-T " + (Top != -1 ? Top.ToString() : "25"));
            arguments.Add("-B " + (Bottom != -1 ? Bottom.ToString() : "15"));
            arguments.Add("-L " + (Left != -1 ? Left.ToString() : "10"));
            arguments.Add("-R " + (Right != -1 ? Right.ToString() : "10"));
            if (!String.IsNullOrEmpty(tipPagina))
            {
                arguments.Add("--page-size " + tipPagina);
            }
            else
            {
                arguments.Add("--page-size A4");
            }
            if (!String.IsNullOrEmpty(orientare))
            {
                arguments.Add("--orientation " + orientare);
            }
            if (FolosesteHeader)
            {
                arguments.Add(String.Format("--header-html \"{0}\"", headerPath));
            }
            if (FolosesteFooter)
            {
                arguments.Add(String.Format("--footer-html \"{0}\"", footerPath));
            }

            arguments.Add('"' + bodyPath + '"');

            if (String.IsNullOrEmpty(caleFisierPdf))
            {
                caleFisierPdf = Path.Combine(DocumentsDirectory, String.Format("Factura{0}.pdf", Timestamp.ToString("yyyy-MM-dd_HH-mm-ss-fff")));
            }

            arguments.Add('"' + caleFisierPdf + '"');

            psi.Arguments = arguments.Aggregate((x, y) => x + " " + y);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process proc = new Process();
            proc.StartInfo = psi;
            proc.Start();
            proc.WaitForExit(60000);

            if (File.Exists(headerPath))
            {
                File.Delete(headerPath);
            }
            if (File.Exists(bodyPath))
            {
                File.Delete(bodyPath);
            }
            if (File.Exists(footerPath))
            {
                File.Delete(footerPath);
            }
            LocatieFisierNou = caleFisierPdf;
            //if (File.Exists(LocatieFisierNou))
            //{
            //    Process.Start("explorer.exe", LocatieFisierNou);
            //}
        }      

        public void ReplaceScalarHeader(string scalarName, string replaceValue)
        {
            ReplaceScalar(HtmlHeader, scalarName, replaceValue);
        }

        public void ReplaceScalarBody(string scalarName, string replaceValue)
        {
            ReplaceScalar(HtmlBody, scalarName, replaceValue);
        }

        public void ReplaceScalarFooter(string scalarName, string replaceValue)
        {
            ReplaceScalar(HtmlFooter, scalarName, replaceValue);
        }

        public void ReplaceScalar(HtmlDocument htmlDocument, string scalarName, string replaceValue)
        {
            HtmlNode node = htmlDocument.DocumentNode.Descendants("span").FirstOrDefault(x => x.InnerHtml == "$[" + scalarName + "]");
            if (node != null)
            {
                node.InnerHtml = replaceValue ?? "";
            }
        }

        public static void ReplaceScalar(HtmlNode htmlNode, string scalarName, string replaceValue)
        {
            HtmlNode node = htmlNode.Descendants("span").FirstOrDefault(x => x.InnerHtml == "$[" + scalarName + "]");
            if (node != null)
            {
                node.InnerHtml = replaceValue ?? "";
            }
        }

        public static void ReplaceTextBoxValue(HtmlNode htmlNode, string idTxt, string replaceValue)
        {
            HtmlNode inp = htmlNode.Descendants("input").FirstOrDefault(x => x.NodeType == HtmlNodeType.Text && x.Id == idTxt);
            if (inp != null)
            {
                inp.SetAttributeValue("value",replaceValue ?? "");
            }
        }

        public static void CheckFolders()
        {
            DirectoryInfo di = new DirectoryInfo(DocumentsDirectory);
            if (!di.Exists)
            {
                di.Create();
            }
            di = new DirectoryInfo(TemplatesDirectory);
            if (!di.Exists)
            {
                di.Create();
            }
        }
    }
}