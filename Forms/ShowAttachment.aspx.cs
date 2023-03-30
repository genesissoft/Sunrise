using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

public partial class Forms_ShowAttachment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string attachment = Request.QueryString["attachment"];
        string UserName = Request.QueryString["UserName"];
        string strPath = string.Empty;
        if (Convert.ToString(Session["UserType"]).ToLower() == "administrator")
        {
            strPath = ConfigurationManager.AppSettings["CRMPath"].ToString() + UserName + "\\";
        }
        else
        {
            strPath = ConfigurationManager.AppSettings["CRMPath"].ToString() + Convert.ToString(Session["UserName"]) + "\\";
        }
        
        if (Convert.ToString(attachment) != "" && Convert.ToString(attachment) != null)
        {
            if (IsVaildFile(attachment))
            {
                if (File.Exists(strPath + attachment))
                {
                    Response.Clear();
                    Response.Charset = "";
                    Response.ClearHeaders();
                    Response.AddHeader("content-disposition", "attachment;filename=" + attachment);
                    Response.ContentType = ("content-disposition");
                    Response.WriteFile(strPath + attachment);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    div_Main.InnerHtml = "<h4 align='center'>File does not Exist</h4>";
                }
            }
        }
        else
        {
            div_Main.InnerHtml = "<h4 align='center'>You cannot access these file types.</h4>";            
        }



    }
    private bool IsVaildFile(string FileName)
    {


        string Ext = System.IO.Path.GetExtension(FileName);
        switch (Ext.ToLower())
        {            
           
            case ".pdf":
                return true;
            case ".doc":
                return true;
            case ".docx":
                return true;
            case ".xls":
                return true;
            case ".xlsx":
                return true;
            case ".txt":
                return true;
            default:
                {
                    return false;  
                }
        }


    }
}
