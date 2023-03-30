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

public partial class Forms_MultiDealCon : System.Web.UI.Page
{
    SqlConnection sqlconn = new SqlConnection();
    ReportDocument rptDoc = new ReportDocument();
    string strPath = string.Empty;
    string strRptName = "DealConfirmation.rpt";
    clsCommonFuns objCommon = new clsCommonFuns();

   

    protected void Page_Load(object sender, EventArgs e)
    {
       
        DateTime dt2;


        if (!IsPostBack)
        {
            Hid_ReportType.Value = "MultiDealCon";
            Col_Headers.InnerHtml = "Multi Deal Confirmation";
            txt_FromDate.Text = DateTime.Today.Date.ToString("dd/MM/yyyy");
            DateTime dt = DateTime.Now.AddDays(1);
            txt_ToDate.Text = dt.ToString("dd/MM/yyyy");
            Session.Remove("dt");
        }
      
        dt2 = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

     
        if (Session["dt"] != null)
        {
            strPath = ConfigurationSettings.AppSettings["ReportsPath"] + "\\" + strRptName;
            sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
            rptDoc.Load(strPath);
            rptDoc.SetDataSource(Session["dt"] as DataTable);
            //rptDoc.DataDefinition.FormulaFields["NewCompDate"].Text = "\"" + strNewCompDate + "\""; ;
            rptDoc.DataDefinition.FormulaFields["strPrintLH"].Text = "\"" + rdo_LetterHead.SelectedValue + "\"";
            rptDoc.DataDefinition.FormulaFields["PrintDealNo"].Text = "\"" + rdo_DealNoPrint .SelectedValue + "\"";
            rptDoc.DataDefinition.FormulaFields["strPrintSignStamp"].Text = "\"" + rdo_PrintSignStamp .SelectedValue + "\"";
            rptDoc.DataDefinition.FormulaFields["DealconHeader"].Text = "\"" + rdo_DealconHeader.SelectedValue + "\"";
            rptDoc.DataDefinition.FormulaFields["PrintsignNote"].Text = "\"" + rdo_PrintsignNote.SelectedValue + "\"";
            rptDoc.DataDefinition.FormulaFields["strPrintDP"].Text = "\"" + rdo_DPDetails .SelectedValue + "\"";
            rptDoc.DataDefinition.FormulaFields["PrintYield"].Text = "\"" + rdo_PrintYield.SelectedValue + "\"";


            DealConfirmation.ReportSource = rptDoc;
            
        }

    }

    public void BuildReportCond()
    {
        try
        {
            string strCond = "";
            string strRpt = "";
            strRpt = Hid_ReportType.Value;
            Session["strWhereClause"] = "";
            strCond = BuildContractNoStr(srh_Customer .SelectCheckBox, srh_Customer .SelectListBox);
            Session["strWhereClause"] = strCond;
            BindReport();
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }
    }

    public string BuildContractNoStr(CheckBox chkbox, ListBox lstbox)
    {
        string str = string.Empty;
        try
        {
            string strCond = "";
            string strIds = "";
            Session["CustomerId"] = null;
            //if (chkbox.Checked == false)
            //{
            foreach (ListItem item in lstbox.Items)
            {
                if (item.Value != "")
                {
                    strIds += item.Value + ",";
                }
            }
            Session["CustomerId"] = strIds.TrimEnd(',');
            //}
            str = Session["CustomerId"].ToString();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        return str;
    }

    public void BindReport()
    {
        try
        {
            string strRptName = "";
            SqlDataAdapter sqlDa = new SqlDataAdapter();
            DataTable dt = new DataTable();
            string lstCustomerIds = "";
            strRptName = "DealConfirmation.rpt";
            strPath = ConfigurationSettings.AppSettings["ReportsPath"] + "\\" + strRptName;
            sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
            SqlCommand sqlComm = new SqlCommand("dbo.Id_RPT_BuyDealConfirmation", sqlconn);
            sqlconn.Open();
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Parameters.Clear();
            sqlComm.Parameters.Add("@fromdate", SqlDbType.SmallDateTime ).Value = objCommon.DateFormat (txt_FromDate.Text);
            sqlComm.Parameters.Add("@todate", SqlDbType.SmallDateTime ).Value = objCommon.DateFormat(txt_ToDate.Text);
            if (srh_Customer.SelectCheckBox.Checked == false)
            {
                if (srh_Customer.SelectListBox.Items.Count > 0)
                {
                    for (int i = 0; i < srh_Customer.SelectListBox.Items.Count; i++)
                    {
                        lstCustomerIds += srh_Customer.SelectListBox.Items[i].Value.ToString() + ",";
                    }
                }
            }
            //lstCustomerIds = lstCustomerIds.TrimEnd(',');
            sqlComm.Parameters.Add("@CustomerIds", SqlDbType.VarChar).Value = lstCustomerIds;
            sqlComm.Parameters.Add("@CompID", SqlDbType.Int).Value = Convert.ToInt32(Session["CompID"]);
            sqlComm.Parameters.Add("@YearID", SqlDbType.Int).Value = Convert.ToInt32(Session["YearId"]);
            sqlComm.Parameters.Add("@Transtype", SqlDbType.Char ).Value = Convert.ToString (rdo_TransType .SelectedValue );
            sqlDa.SelectCommand = sqlComm;
            sqlDa.Fill(dt);

            if ((dt.Rows.Count == 0))
            {
                string msg = "No Records Found";
                string strHtml;
                strHtml = "alert('" + msg + "');";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "msg", strHtml, true);
                return;
            }

            DealConfirmation.Visible = true;
            sqlconn.Close();
            if (dt.Rows.Count > 0)
            {
                //strRptName = "DealConfirmation.rpt";
                //strPath = ConfigurationSettings.AppSettings["ReportsPath"] + "\\" + strRptName;
                //Session["dt"] = dt;
                //rptDoc.Load(strPath);
                //rptDoc.SetDataSource(dt);
                //CrystalReportViewer1.ReportSource = rptDoc;

                Session["dt"] = dt;
                rptDoc.Load(strPath);
                rptDoc.SetDataSource(dt);
                //rptDoc.DataDefinition.FormulaFields["NewCompDate"].Text = "\"" + strNewCompDate + "\"";
                rptDoc.DataDefinition.FormulaFields["strPrintLH"].Text = "\"" + rdo_LetterHead .SelectedValue  + "\"";
                rptDoc.DataDefinition.FormulaFields["PrintDealNo"].Text = "\"" + rdo_DealNoPrint.SelectedValue + "\"";
                rptDoc.DataDefinition.FormulaFields["strPrintSignStamp"].Text = "\"" + rdo_PrintSignStamp.SelectedValue + "\"";
                rptDoc.DataDefinition.FormulaFields["DealconHeader"].Text = "\"" + rdo_DealconHeader.SelectedValue + "\"";
                rptDoc.DataDefinition.FormulaFields["PrintsignNote"].Text = "\"" + rdo_PrintsignNote.SelectedValue + "\"";
                rptDoc.DataDefinition.FormulaFields["strPrintDP"].Text = "\"" + rdo_DPDetails.SelectedValue + "\"";
                rptDoc.DataDefinition.FormulaFields["PrintYield"].Text = "\"" + rdo_PrintYield.SelectedValue + "\"";
                DealConfirmation.ReportSource = rptDoc;
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }
    }

    protected void btn_Print_Click(object sender, EventArgs e)
    {
        //BuildReportCond();
        BindReport();
    }
}
