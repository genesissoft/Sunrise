using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

public partial class Forms_ConfirmPassword : System.Web.UI.Page
{
    clsCommonFuns objComm = new clsCommonFuns();
    string strConnection = ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D);
        Response.Expires = -1500;
        Response.CacheControl = "no-cache";
        try
        {
            if (!Page.IsPostBack)
            {
                Hid_IssueId.Value = Convert.ToString(Request.QueryString["IssueId"]);
                Hid_UserId.Value = Convert.ToString(Request.QueryString["UserId"]);
                if (Hid_IssueId.Value == "")
                {
                    Hid_IssueId.Value = "0";
                }

                if (Hid_UserId.Value == "")
                {
                  
                }
                Hid_UserId.Value = "266";
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }

    }
    protected void btn_Save_Click(object sender, EventArgs e)
    {

        try
        {
            if (IsUserExist(txt_Password.Text.Trim()))
            {

            }
            else
            {
                string msg = "No data found for this user";
                string strHtml;
                strHtml = "alert('" + msg + "');";


            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private bool IsUserExist(string strPassword)
    {
        SqlConnection sqlConn = new SqlConnection(strConnection);
        SqlCommand sqlComm = new SqlCommand();
        DataTable dt = new DataTable();
        PasswordDatabase.clsDatabase objPwdDB = new PasswordDatabase.clsDatabase();
        PasswordDatabase.clsEncryption objPwdEncrypt = new PasswordDatabase.clsEncryption();
        byte[] hashedBytes = null;
        bool blnExist = false;

        dt = objPwdDB.GetConfigTable(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        hashedBytes = objPwdEncrypt.GetEncryptedPassword(strPassword, dt.Rows[0]["EncryptionType"].ToString());
        try
        {
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = "GET_ISUSER_EXIST";
            sqlComm.Parameters.Clear();
            if (Hid_UserId.Value != "")
            {
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt16(Hid_UserId.Value);
            }
            sqlComm.Parameters.Add("@Password", SqlDbType.Binary, 16).Value = hashedBytes;
            SqlDataAdapter sqlDa = new SqlDataAdapter(sqlComm);
            dt = null;
            sqlDa.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                blnExist = true;
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        finally
        {

            sqlComm = null;

        }
        return blnExist;
    }
}
