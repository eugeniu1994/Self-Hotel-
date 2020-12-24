using SelfHotel.Nomenclatoare;
using SelfHotel.NomenclatoareNew;
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

namespace SelfHotel
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConexiuneDB.initializare();//incarc setari aici
        }

        [WebMethod(EnableSession = true)]
        public static List<TipCamera_H> getListaTipCamere()
        {
            List<TipCamera_H> rv;
            try
            {
                rv = TipCamera_H.GetLista().Where(x => x.Sters == false && x.Suplimentara==false && x.Virtuala==false).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
            return rv;
        }

        [WebMethod(EnableSession = true)]
        public static String getDataLucru()
        {
            try
            {
                return ConexiuneDB.DataLucr.ToShortDateString();
            }
            catch { return "2019:12:04";}
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
    }
}