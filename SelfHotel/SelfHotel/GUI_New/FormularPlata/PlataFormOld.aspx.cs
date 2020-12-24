using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZipEscort.Setari;

namespace SelfHotel.GUI_New.FormularPlata
{
    public partial class PlataForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Decimal calculSoldCazare(int idRezervare, List<int> idRezCams)
        {
            Decimal rv = 0;
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
                    return -1;
                }
            }
            return rv;
        }

        public static bool seteazaCamerePlatite(int idRezervare, List<int> idRezCams)
        {
            bool rv = false;


            return rv;
        }
    }
}