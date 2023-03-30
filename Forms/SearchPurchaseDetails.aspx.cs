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
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;

public partial class Forms_SearchPurchaseDetails : System.Web.UI.Page
{
    ClsCommon objComm = new ClsCommon();
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
                Hid_Dated.Value = objComm.Trim(Request.QueryString["dated"]);
                Hid_DealSlipId.Value = objComm.Trim(Request.QueryString["id"]);
                Hid_SecurityId.Value = objComm.Trim(Request.QueryString["securityid"]);
                Hid_DealTransType.Value = objComm.Trim(Request.QueryString["dealtranstype"]);
                Hid_Marked.Value = objComm.Trim(Request.QueryString["marked"]);
                Hid_MarketType.Value = objComm.Trim(Request.QueryString["markettype"]);
                
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
        List<SqlParameter> lstParam = new List<SqlParameter>();

        try
        {
            lstParam.Add(new SqlParameter("@AsOn", objComm.Trim(Hid_Dated.Value)));
            lstParam.Add(new SqlParameter("@DealSlipId", objComm.Val(Hid_DealSlipId.Value)));
            lstParam.Add(new SqlParameter("@SecurityId", objComm.Val(Hid_SecurityId.Value)));
            lstParam.Add(new SqlParameter("@DealTransType", objComm.Trim(Hid_DealTransType.Value)));
            
            if (Hid_Marked.Value != "")
            {
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(Hid_Marked.Value, (typeof(DataTable)));
                lstParam.Add(new SqlParameter("@Marking", dt));
            }

            if (!string.IsNullOrEmpty(Session["CompId"] + ""))
            {
                lstParam.Add(new SqlParameter("@CompId", objComm.Val(Session["CompId"].ToString())));
            }
            ds = objComm.FillDetails(lstParam, "ID_Fill_Search_PurchaseTransferDetails");

            dgPurchaseDetails.DataSource = ds.Tables[0];
            dgPurchaseDetails.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
