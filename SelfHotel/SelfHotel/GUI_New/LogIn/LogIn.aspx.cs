using SelfHotel.Nomenclatoare_Final;
using SelfHotel.NomenclatoareNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZipEscort.Setari;

namespace SelfHotel.GUI_New.LogIn
{
    public partial class LogIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static Utilizatori verificaDisponibilitateLista(string Utilizator, string Parola)
        {
            Utilizatori rv;
            try
            {
                List<Utilizatori> lista = Utilizatori.GetLista().Where(x => x.Email == Utilizator.Trim() && x.Parola == _SetariClass.Encrypt(Parola,true)).ToList();
                if (lista.Count > 0)
                {
                    rv = lista[0];
                }
                else
                {
                    rv = null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return rv;
        }
    }
}