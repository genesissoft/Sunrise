using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class Forms_ShowDocs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Id"].Trim() != "")
        {
            try
            {
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
                SqlCommand sqlComm = new SqlCommand();
                string Col = Request.QueryString["CollName"].Trim();
                string Tab = Request.QueryString["TableName"].Trim();
                string Col2 = Request.QueryString["Coll2Name"].Trim();
                string Id = Request.QueryString["Id"].Trim();

                sqlComm.Connection = sqlConn;
                sqlComm.CommandType = CommandType.Text;
                sqlComm.Parameters.Clear();
                if (Col2 != "")
                {
                    sqlComm.CommandText = "Select " + Col + " From " + Tab + " Where " + Col2 + " = " + Request.QueryString["Id"].Trim();
                }
                sqlConn.Open();
                SqlDataReader dr = sqlComm.ExecuteReader();
                dr.Read();

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "File";
                Response.AddHeader("content-disposition", "attachment;filename=" + dr[1].ToString());
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite((byte[])dr[0]);
                Response.End();

                if (sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
            }
        }
    }
}
