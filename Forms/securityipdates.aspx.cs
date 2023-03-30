using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

public partial class Forms_securityipdates : System.Web.UI.Page
{
    ClsCommon objComm = new ClsCommon();
    clsCommonFuns objCommon = new clsCommonFuns();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D);
            Response.Expires = -1500;
            Response.CacheControl = "no-cache";
            Response.AppendHeader("Cache-Control", "no-store");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.AppendHeader("pragma", "no-cache");

            if (!Page.IsPostBack)
            {
                Hid_Id.Value = HttpUtility.UrlDecode(objCommon.DecryptText(objComm.Trim(Request.QueryString["id"])));
                FillFields();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FillFields()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        List<SqlParameter> lstParam = new List<SqlParameter>();

        try
        {
            if (Hid_Id.Value.Trim() != "")
            {
                lstParam.Add(new SqlParameter("@SecurityId", Convert.ToInt32(Hid_Id.Value)));
            }
            ds = objComm.FillDetails(lstParam, "WDM_Fill_SecurityIPDates");
            if (ds.Tables.Count > 0)
            {
                dgInterestDetails.DataSource = ds.Tables[0];
                dgInterestDetails.DataBind();

                dgMaturityDetails.DataSource = ds.Tables[1];
                dgMaturityDetails.DataBind();

                dgCallDetails.DataSource = ds.Tables[2];
                dgCallDetails.DataBind();

                dgPutDetails.DataSource = ds.Tables[3];
                dgPutDetails.DataBind();

                if (ds.Tables[4].Rows.Count > 0)
                {
                    Hid_FirstInterestDate.Value = ds.Tables[4].Rows[0]["FirstInterestDate"].ToString();
                    Hid_MaturityDate.Value = ds.Tables[4].Rows[0]["MaturityDate"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        List<SqlParameter> lstParam = new List<SqlParameter>();
        string strResult = "";
        try
        {
            if (Hid_Id.Value.Trim() != "")
            {
                lstParam.Add(new SqlParameter("@SecurityId", Convert.ToInt32(Hid_Id.Value)));
            }
            lstParam.Add(new SqlParameter("@UpdateType", cboUpdateType.SelectedValue));
            if (Hid_Data.Value != "")
            {
                XmlDocument xmlDoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + Hid_Data.Value + "}", "root");
                lstParam.Add(new SqlParameter("@IPDates_Xml", xmlDoc.OuterXml));
            }
            lstParam.Add(new SqlParameter("@UserId", Session["UserId"].ToString()));
            strResult = objComm.InsertUpdateDeleteDetailsMsg(lstParam, "WDM_InsertUpdate_SecurityIPDates");
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Close window", "CloseWindow()", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void dgInterestDetails_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
            {
                ((DropDownList)e.Item.FindControl("cboDivisor")).SelectedValue = ((Literal)e.Item.FindControl("litDivisor")).Text;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}