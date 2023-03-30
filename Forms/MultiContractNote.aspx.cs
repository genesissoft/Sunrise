using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;

public partial class Forms_MultiContractNote : System.Web.UI.Page
{
    SqlConnection sqlconn = new SqlConnection();
    ReportDocument rptDoc = new ReportDocument();
    string strPath = string.Empty;
 
    //string strRptName = "ContractNoteBSE_GST.rpt";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Hid_ReportType.Value = "MultiContractNote";
            Col_Headers.InnerHtml = "WDM Contract Note";
            //srh_TransCode.Columns.Add("WDMDealNumber");
            //srh_TransCode.Columns.Add("Convert(varchar,DealDate,103) As DealDate");
            //srh_TransCode.Columns.Add("SecurityName");
            //srh_TransCode.Columns.Add("BuySrNo");
            //srh_TransCode.Columns.Add("SellSrNo");
            //srh_TransCode.Columns.Add("ExchangeName");
            //srh_TransCode.Columns.Add("DealEntryId");
            Session.Remove("dt");
        }
        if (Session["dt"] != null)
        {
            string strRptName = "";
            if (ViewState["BackSide"] == null)
            {
                DataTable dt = Session["dt"] as DataTable;
                if (Convert.ToString(dt.Rows[0]["ExchangeId1"]).Trim() == "1")
                {
                    strRptName = "ContractNoteBSE_GST.rpt";
                }
                else
                {
                    strRptName = "ContractNoteNSEBrokGST.rpt";
                }
                strPath = ConfigurationSettings.AppSettings["ReportsPath"] + "\\" + strRptName;
                
                sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
                rptDoc.Load(strPath);
                rptDoc.SetDataSource(dt);
                CrystalReportViewer1.ReportSource = rptDoc;
            }
            else
            {
                strPath = ConfigurationSettings.AppSettings["ReportsPath"] + "BacksideContractNote.rpt";
                rptDoc.Load(strPath);
                CrystalReportViewer1.ReportSource = rptDoc;
            }
        }


    }

    protected void srh_TransCodeClick(object sender, EventArgs e)
    {
        SqlDataAdapter sqlDa = new SqlDataAdapter();
        DataTable dt = new DataTable();
        
        sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());

        SqlCommand sqlComm = new SqlCommand("dbo.ID_FILL_ContractNoteBSE_RPT", sqlconn);
        sqlconn.Open();
        sqlComm.CommandType = CommandType.StoredProcedure;
        sqlComm.Parameters.Clear();
        sqlComm.Parameters.Add("@DealEntryId", SqlDbType.Int).Value = srh_TransCode.SelectedId;
        sqlComm.Parameters.Add("@BuySell", SqlDbType.Char).Value = rdo_ContNotefor.SelectedValue;
        sqlDa.SelectCommand = sqlComm;
        sqlDa.Fill(dt);
        sqlconn.Close();
        if (dt.Rows.Count > 0)
        {
            if (Convert.ToString(dt.Rows[0]["BuySrNo"]).Trim() != "" && Convert.ToString(dt.Rows[0]["SellSrNo"]).Trim() != "")
            {
                rdo_ContNotefor.Enabled = true;
            }
            else if (Convert.ToString(dt.Rows[0]["BuySrNo"]).Trim() != "" && Convert.ToString(dt.Rows[0]["SellSrNo"]).Trim() == "")
            {
                rdo_ContNotefor.SelectedValue = "B";
                rdo_ContNotefor.Enabled = false;
            }
            else if (Convert.ToString(dt.Rows[0]["SellSrNo"]).Trim() != "" && Convert.ToString(dt.Rows[0]["BuySrNo"]).Trim() == "")
            {
                rdo_ContNotefor.SelectedValue = "S";
                rdo_ContNotefor.Enabled = false;
            }
            if (Convert.ToString(dt.Rows[0]["SellSrNo"]).Trim() == "" && Convert.ToString(dt.Rows[0]["BuySrNo"]).Trim() == "")
            {
                btn_Print.Enabled = false;
            }
            else
            {
                btn_Print.Enabled = true;
            }

            
        }
        Session.Remove("dt");
        rptDoc.Close();
        rptDoc.Dispose();
        CrystalReportViewer1.Visible = false;
    }

    public void BuildReportCond()
    {
        try
        {
            string strCond = "";
            string strRpt = "";
            strRpt = Hid_ReportType.Value;
            BindReport();
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }
    }


    public void BindReport()
    {
        try
        {
            string strRptName = "";
            SqlDataAdapter sqlDa = new SqlDataAdapter();
            DataTable dt = new DataTable();
           
            sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());

            SqlCommand sqlComm = new SqlCommand("dbo.ID_FILL_ContractNoteBSE_RPT", sqlconn);
            sqlconn.Open();
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Parameters.Clear();
            sqlComm.Parameters.Add("@DealEntryId", SqlDbType.Int).Value = srh_TransCode.SelectedId;
            sqlComm.Parameters.Add("@BuySell", SqlDbType.Char).Value = rdo_ContNotefor.SelectedValue;
            sqlDa.SelectCommand = sqlComm;
            sqlDa.Fill(dt);

            CrystalReportViewer1.Visible = true;
            sqlconn.Close();
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["ExchangeId1"]).Trim() == "1")
                {
                    strRptName = "ContractNoteBSE_GST.rpt";
                }
                else
                {
                    strRptName = "ContractNoteNSEBrokGST.rpt";
                }
                strPath = ConfigurationSettings.AppSettings["ReportsPath"] + "\\" + strRptName;
                Session["dt"] = dt;
                rptDoc.Load(strPath);
                rptDoc.SetDataSource(dt);
                CrystalReportViewer1.ReportSource = rptDoc;
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }

    }

    protected void btn_Print_Click(object sender, EventArgs e)
    {
        ViewState.Remove("BackSide");
        BindReport();
    }

    protected void btn_Printbackside_Click(object sender, EventArgs e)
    {
        ViewState["BackSide"] = 1;
        strPath = ConfigurationSettings.AppSettings["ReportsPath"] + "BacksideContractNote.rpt";
        rptDoc.Load(strPath);
        CrystalReportViewer1.ReportSource = rptDoc;
    }
}
