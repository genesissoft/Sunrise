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

public partial class Forms_DealPurchaseDetailsSell : System.Web.UI.Page
{
    ClsCommon objComm = new ClsCommon();
    Util objUtil = new Util();
    string PgName = "DealPurchaseDetailsSell.aspx";
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
                Hid_MarketType.Value = objComm.Trim(Request.QueryString["markettype"]);
                Hid_Marked.Value = objComm.Trim(Request.QueryString["marked"]);
                FillFields();
            }
        }
        catch (Exception ex)
        {
            objUtil.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex);
        }
    }

    private void FillFields()
    {
        DataSet ds = new DataSet();
        List<SqlParameter> lstParam = new List<SqlParameter>();

        try
        {
            txtFaceValue.Text = objComm.Trim(Request.QueryString["facevalue"]);
            txtNoofBond.Text = objComm.Trim(Request.QueryString["noofbond"]);
            lstParam.Add(new SqlParameter("@AsOn", objComm.Trim(Hid_Dated.Value)));
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
            ds = objComm.FillDetails(lstParam, "ID_Fill_PurchaseDetails");

            dgPurchaseDetails.DataSource = ds.Tables[0];
            dgPurchaseDetails.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                Hid_CurrSecFaceValue.Value = objComm.Trim(ds.Tables[0].Rows[0]["CurrSecFaceValue"].ToString());
            }
        }
        catch (Exception ex)
        {
            objUtil.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex);
        }
    }
}