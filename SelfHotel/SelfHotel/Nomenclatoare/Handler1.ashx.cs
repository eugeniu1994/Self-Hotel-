using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using ZipEscort.Setari;

namespace SelfHotel.Nomenclatoare
{
    public class Handler1 : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                Int32 empno;
                context.Response.Clear();
                try
                {
                    if (context.Request.QueryString["id"] != null)
                        empno = Convert.ToInt32(context.Request.QueryString["id"]);
                    else
                        empno = 0;
                    //throw new ArgumentException("Nu exista Id");
                }
                catch (Exception exc)
                {
                    empno = 0;
                }
                context.Response.ContentType = "image/jpeg";
                Stream strm = ShowEmpImage(empno);
                byte[] buffer = new byte[4096];
                int byteSeq = strm.Read(buffer, 0, 4096);

                while (byteSeq > 0)
                {
                    context.Response.OutputStream.Write(buffer, 0, byteSeq);
                    byteSeq = strm.Read(buffer, 0, 4096);
                }
            }
            catch (Exception exc)
            {

            }
            //context.Response.BinaryWrite(buffer);
        }

        public Stream ShowEmpImage(int empno)
        {
            using (SqlConnection cnn = new SqlConnection(ConexiuneDB.CnnString))
            {
                try
                {
                    cnn.Open();
                    string sql = @"SELECT [FileData]
                                        FROM [SmartH].[dbo].[_Pictures] 
                                   WHERE IdCam_H = @ID and [Default]=1;";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@ID", empno);
                    object img = cmd.ExecuteScalar();
                       try
                        {
                            return new MemoryStream((byte[])img);
                        }
                        catch
                        {
                            return null;
                        }
                }
                catch (Exception exc)
                {
                    return null;
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }   
    }
}