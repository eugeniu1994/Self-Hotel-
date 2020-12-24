using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZipEscort.Setari;

namespace SelfHotel.GUI.CHECKOUT
{
    public partial class RezervaIFrame : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //goToAlin();
            try
            {
                string idHotel = ConexiuneDB.IdHotel.ToString();
                string paginaHome = "./Home.aspx";
                string idFirma = ConexiuneDB.IdFirma.ToString();
                string StepHeaderBackground= "2f709e"; 
                string ButtonBorderColor= "19294a"; 
                string ButtonBackground= "23577d";
                string Lang = "RO";
                if (Request.Cookies["lang"] != null)
                {
                    Lang = Request.Cookies["lang"].Value;
                }

                string urlAddress = "http://rezervari.multisoft.ro/Booking/?Hotel=13TYD3&CheckIn=20-12-2018&CheckOut=21-12-2018?idHotel=" + idHotel + "&paginaHome=" + paginaHome + "&idFirma=" + idFirma + "&StepHeaderBackground=" + StepHeaderBackground + "&ButtonBorderColor=" + ButtonBorderColor + "&ButtonBackground=" + ButtonBackground + "&Lang=" + Lang;
                //urlAddress = "http://localhost:31363/GUI/CHECKOUT/PlataForm2.aspx?idHotel=" + idHotel + "&paginaHome=" + paginaHome + "&idFirma=" + idFirma;
                //urlAddress = "http://192.168.127.81/Solon.OB/Booking/?Hotel=1SE7T4&idFirma=1";
                using (WebClient client = new WebClient())
                {
                    string pagesource = client.DownloadString(urlAddress);
                }
               
            }
            catch (Exception exc)
            { }
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
                  //(HttpWebRequest)WebRequest.Create("http://rezervari.multisoft.ro/Booking/?Hotel=13TYD3&CheckIn=20-12-2018&CheckOut=21-12-2018");
                  (HttpWebRequest)WebRequest.Create("http://localhost:31363/GUI/CHECKOUT/PlataForm2.aspx");
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
    }
}