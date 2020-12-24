using SelfHotel.Nomenclatoare_Final;
using SelfHotel.NomenclatoareNew;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ZipEscort.Setari;

namespace SelfHotel.GUI.Admin
{
    public partial class Administrare : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                createContent();
            }
        }

        protected void createContent(){
            //List<TipCamera_H> rv = null;
            //try
            //{
            //    rv = TipCamera_H.GetLista().Where(x => x.Sters == false && x.Suplimentara == false && x.Virtuala == false).ToList();
            //}
            List<Camere> rv = null;
            try
            {
                rv = Camere.getLista().Where(x => x.Ascunsa == false && x.Virtuala == false).ToList();
            }
            catch (Exception ex)
            {}

            HtmlGenericControl createDiv = new HtmlGenericControl("DIV");
            createDiv.ID = "createDiv";
            createDiv.Attributes.Add("class", "our-webcoderskull padding-lg");
            //this.Controls.Add(createDiv);
            this.form1.Controls.Add(createDiv);
            HtmlGenericControl div = new HtmlGenericControl("div");
            createDiv.Controls.Add(div);
            div.Attributes.Add("class", "container");
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            div.Controls.Add(ul);
            ul.Attributes.Add("class", "row");
            foreach (Camere tc in rv)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                li.Attributes.Add("class", "col-12 col-md-6 col-lg-3");
                ul.Controls.Add(li);
                HtmlGenericControl div1 = new HtmlGenericControl("div");
                li.Controls.Add(div1);
                div1.Attributes.Add("class", "cnt-block equal-hight");
                div1.Attributes.Add("style", "height: 349px;");
                HtmlGenericControl img = new HtmlGenericControl("img");
                div1.Controls.Add(img);
                img.Attributes.Add("class", "img-responsive");
                img.Attributes.Add("id", "Img_"+tc.ID);
                img.Attributes.Add("alt", "Imagine");
                img.Attributes.Add("src", "../../Nomenclatoare/Handler1.ashx?id=" + tc.ID);
                HtmlGenericControl h3 = new HtmlGenericControl("h3");
                div1.Controls.Add(h3);
                HtmlGenericControl a = new HtmlGenericControl("a");
                h3.Controls.Add(a);
                a.Attributes.Add("href", "#");
                a.InnerHtml = tc.DenumireCamera;
                HtmlGenericControl p = new HtmlGenericControl("p");
                p.InnerHtml = "Camera "+tc.Denumire;
                div1.Controls.Add(p);
                HtmlGenericControl ulCams = new HtmlGenericControl("ul");
                div1.Controls.Add(ulCams);
                ulCams.Attributes.Add("class", "follow-us clearfix");
                ulCams.Attributes.Add("style", "white-space: nowrap;overflow: hidden;");
                HtmlGenericControl li1 = new HtmlGenericControl("li");
                ulCams.Controls.Add(li1);

                FileUpload upload = new FileUpload();
                upload.Attributes.Add("runat", "server");
                upload.ID = "upload_"+tc.ID;
                

                li1.Controls.Add(upload);
                HtmlGenericControl ulCams2 = new HtmlGenericControl("ul");
                div1.Controls.Add(ulCams2);
                ulCams2.Attributes.Add("class", "follow-us clearfix");
                Button b2 = new Button();
                b2.Attributes.Add("type","button");
                b2.Attributes.Add("runat", "server");
                b2.Attributes.Add("data-idcamera", tc.ID+"");
                b2.Attributes.Add("data-idtipcamera", tc.IdTipCamera + "");
                b2.Attributes.Add("id", "btn_"+tc.ID);
                HtmlGenericControl li2 = new HtmlGenericControl("li");
                ulCams2.Controls.Add(li2);
                li2.Controls.Add(b2);
                b2.Text = "Salveaza";
                b2.Click += btnSubmit_Click;
            }

            List<NomFirme> nomFirme = new List<NomFirme>();
            try
            {
                 nomFirme = NomFirme.getLista().Where(x => x.Sters == false).ToList();
            }
            catch (Exception ex)
            { }

            foreach (NomFirme nf in nomFirme)
            {
                this.selectFirmaID.Items.Add(new ListItem(nf.Firma, nf.IdFirma+""));
            }

            try
            {
                List<SetariBaza> lista = SetariBaza.getLista().ToList();
                List<SetariBaza> AccountKeySetare = lista.Where(x => x.ID == 17).ToList();
                List<SetariBaza> MerchantIDSetare = lista.Where(x => x.ID == 18).ToList();
                this.txtAccountKey.Text = AccountKeySetare[0].Valoare;
                this.txtMerchantID.Text = MerchantIDSetare[0].Valoare;
                List<SetariBaza> mailSetare = lista.Where(x => x.ID == 22).ToList();
                this.txtMailAdmin.Text = mailSetare[0].Valoare;
                List<SetariBaza> ObservatiiSetare = lista.Where(x => x.ID == 21).ToList();
                this.txtObsFactura.Text = ObservatiiSetare[0].Valoare;
            }
            catch (Exception exc) { }
        }
        private Control FindControlRecursive(Control rootControl, string controlID)
        {
            if (rootControl.ID == controlID) return rootControl;

            foreach (Control controlToSearch in rootControl.Controls)
            {
                Control controlToReturn =
                    FindControlRecursive(controlToSearch, controlID);
                if (controlToReturn != null) return controlToReturn;
            }
            return null;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            String txtCameraAtribut = (sender as Button).Attributes["data-idcamera"] as string;
            String txtTipCameraAtribut = (sender as Button).Attributes["data-idtipcamera"] as string;
            Int32 IdCam = 0,IdTipCam=0;
            Int32.TryParse(txtCameraAtribut, out IdCam);Int32.TryParse(txtTipCameraAtribut, out IdTipCam);
            if (IdCam > 0)
            {
                FileUpload img = (FileUpload)FindControlRecursive(form1, "upload_" + IdCam);
                Byte[] imgByte = null;
                SqlConnection connection = null;
                try
                {
                    if (img.HasFile && img.PostedFile != null)
                    {
                        HttpPostedFile File = img.PostedFile;
                        imgByte = new Byte[File.ContentLength];
                        File.InputStream.Read(imgByte, 0, File.ContentLength);

                        string conn = ConexiuneDB.CnnString;
                        connection = new SqlConnection(conn);

                        connection.Open();
                        string sql = @"INSERT INTO [SmartH].[dbo].[_Pictures]
                                                               ([Guid]
                                                               ,[Descriere]
                                                               ,[FileData]
                                                               ,[Default]
                                                               ,[IdCam_H]
                                                               ,[IdTipCamera])
                                                         VALUES
                                                               (NEWID()
                                                               ,@Descriere
                                                               ,@FileData
                                                               ,@Default
                                                               ,@IdCam_H
                                                               ,@IdTipCamera)";
                        SqlCommand cmd = new SqlCommand(sql, connection);
                        cmd.Parameters.Add(new SqlParameter("@Descriere", SqlDbType.VarChar)).Value = "";
                        cmd.Parameters.Add(new SqlParameter("@FileData", SqlDbType.Binary)).Value = imgByte;
                        cmd.Parameters.Add(new SqlParameter("@Default", SqlDbType.Bit)).Value = true;
                        cmd.Parameters.Add(new SqlParameter("@IdCam_H", SqlDbType.BigInt)).Value = IdCam;
                        cmd.Parameters.Add(new SqlParameter("@IdTipCamera", SqlDbType.BigInt)).Value = IdTipCam;

                        bool ok = cmd.ExecuteNonQuery() > 0;
                    }
                    else
                    {
                        //lblResult.Text = String.Format("Selectati un fisier");
                    }
                }
                catch (Exception exc)
                {
                    //lblResult.Text = "A intervenit o eroare";
                }
                finally
                {
                    try
                    {
                        connection.Close();
                    }
                    catch { }
                }
            }            
        }

        protected void btnVizualizeaza_Click(object sender, EventArgs e)
        {
            try
            {
                //int id = Convert.ToInt32(this.txtCameraID.Text);
                //Image1.ImageUrl = "~/Nomenclatoare/Handler1.ashx?id=" + id;                
            }
            catch (Exception exc)
            {

            }
        }

        protected void btnSalveazaFirmasiHotel_Click(object sender, EventArgs e)
        {
            String IdFirmaStr = this.selectFirmaID.SelectedItem.Value;
            Int32 IdFirma = 0;
            Int32.TryParse(IdFirmaStr, out IdFirma);
            if (IdFirma > 0)
            {

                using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                {
                    try
                    {
                        cnn.Open();
                        string sql = @"UPDATE [SmartH].[dbo].[Setari]
                               SET 
                                  [Valoare] = @Valoare
                             WHERE Denumire = 'IdFirma'";

                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.Parameters.Add(new SqlParameter("@Valoare", SqlDbType.VarChar)).Value = IdFirma + "";
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    { }
                }
            }
        }

        protected void btnEuPlatesc_Click(object sender, EventArgs e)
        {
            String AccountKey = this.txtAccountKey.Text;
            String MerchantID = this.txtMerchantID.Text;

            if (!String.IsNullOrEmpty(AccountKey) && !String.IsNullOrEmpty(MerchantID))
            {
                using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                {
                    try
                    {
                        cnn.Open();
                        string sql = @"UPDATE [SmartH].[dbo].[Setari]
                               SET 
                                  [Valoare] = @Valoare
                             WHERE ID = @ID";

                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.Parameters.Add(new SqlParameter("@Valoare", SqlDbType.VarChar)).Value = AccountKey;
                        cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)).Value = 17;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    { }
                }

                using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                {
                    try
                    {
                        cnn.Open();
                        string sql = @"UPDATE [SmartH].[dbo].[Setari]
                               SET 
                                  [Valoare] = @Valoare
                             WHERE ID = @ID";

                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.Parameters.Add(new SqlParameter("@Valoare", SqlDbType.VarChar)).Value = MerchantID;
                        cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)).Value = 18;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    { }
                }
            }
            String AdminMail = this.txtMailAdmin.Text;
            try
            {
                var eMailValidator = new System.Net.Mail.MailAddress(AdminMail);
                using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                {
                    try
                    {
                        cnn.Open();
                        string sql = @"UPDATE [SmartH].[dbo].[Setari]
                               SET 
                                  [Valoare] = @Valoare
                             WHERE ID = @ID";

                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.Parameters.Add(new SqlParameter("@Valoare", SqlDbType.VarChar)).Value = eMailValidator.Address;
                        cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)).Value = 22;
                        bool ok=cmd.ExecuteNonQuery()>0;
                    }
                    catch (Exception exc)
                    { }
                }
            }
            catch (FormatException ex)
            {
                // wrong e-mail address
            }

            String observatiiFactura = this.txtObsFactura.Text;
            if (!String.IsNullOrEmpty(observatiiFactura))
            {
                using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
                {
                    try
                    {
                        cnn.Open();
                        string sql = @"UPDATE [SmartH].[dbo].[Setari]
                               SET 
                                  [Valoare] = @Valoare
                             WHERE ID = @ID";

                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.Parameters.Add(new SqlParameter("@Valoare", SqlDbType.VarChar)).Value = observatiiFactura;
                        cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)).Value = 21;
                        bool ok = cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception exc)
                    { }
                }
            }
        }
    }
}