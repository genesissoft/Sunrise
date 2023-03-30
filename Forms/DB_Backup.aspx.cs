using System;
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using System.Data.SqlClient; 

public partial class Forms_DB_Backup : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection();
    SqlCommand sqlcmd = new SqlCommand();
    SqlDataAdapter da = new SqlDataAdapter();
    DataTable dt = new DataTable();  
    string constr, Query, sqlconn;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
      private void connection()
    {
        sqlconn = ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ConnectionString;
        con = new SqlConnection(sqlconn);
    }
    protected void btnBackup_Click(object sender, EventArgs e)  
    {
        //IF SQL Server Authentication then Connection String  
        //con.ConnectionString = @"Server=SERVER\SQLEXPRESS;database=" + "eInstadeal_Synergee" + ";uid=sa;password=genesis;";  

        //IF Window Authentication then Connection String  
        //con.ConnectionString = @"Server=MyPC\SqlServer2k8;database=Test;Integrated Security=true;";

        connection();
        
        string backupDIR = "E:\\eInstadeal_DB_Backup";
        if (!System.IO.Directory.Exists(backupDIR))
        {
            System.IO.Directory.CreateDirectory(backupDIR);
        }
        try
        {
            con.Open();
            sqlcmd.CommandTimeout = 0;
            //sqlcmd = new SqlCommand("id_backup_database", con);
            sqlcmd = new SqlCommand("backup database eInstadeal to disk='" + backupDIR + "\\" + "eInstadeal_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".Bak'", con);
            sqlcmd.ExecuteNonQuery();
            con.Close();
            lblError.Text = "Database Backup successfull";
        }
        catch (Exception ex)
        {
            lblError.Text = "Error Occured During DB backup process !<br>" + ex.ToString();
        }  
    }  
  
}
