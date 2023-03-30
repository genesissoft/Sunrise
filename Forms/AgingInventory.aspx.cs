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
using System.Collections.Generic;
using Newtonsoft.Json;

public partial class Forms_AgingInventory : System.Web.UI.Page
{
    ClsCommon objComm = new ClsCommon();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(Session["UserName"] as string))
            {
                Response.Redirect("Login.aspx", false);
            }

            //HtmlMeta xuac = new HtmlMeta();
            //xuac.HttpEquiv = "X-UA-Compatible";
            //xuac.Content = "IE=edge";
            //Header.Controls.AddAt(0, xuac);

            if (!Page.IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.ToString("01/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtSellDownFromDate.Text = DateTime.Now.ToString("01/MM/yyyy");
                txtSellDownToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtStockFromDate.Text = DateTime.Now.ToString("01/MM/yyyy");
                txtStockToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                FillDetails();
                setcontrols();
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    public void setcontrols()
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> lstParam = new List<SqlParameter>();
            ds = objComm.FillDetails(lstParam, "ID_FILL_CompanyMasterRpt");
            cboCompanyName.DataSource = ds.Tables[0];
            cboCompanyName.DataTextField = "CompName";
            cboCompanyName.DataValueField = "CompId";
            cboCompanyName.DataBind();
            cboSellDownCompanyName.DataSource = ds.Tables[0];
            cboSellDownCompanyName.DataTextField = "CompName";
            cboSellDownCompanyName.DataValueField = "CompId";
            cboSellDownCompanyName.DataBind();

            cboStockCompanyName.DataSource = ds.Tables[0];
            cboStockCompanyName.DataTextField = "CompName";
            cboStockCompanyName.DataValueField = "CompId";
            cboStockCompanyName.DataBind();
        }

        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    protected void cboCompanyName_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        FillDetails();
    }

    private void FillDetails()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        List<SqlParameter> lstParam = new List<SqlParameter>();

        try
        {
            lstParam.Add(new SqlParameter("@OpeningDate", txtFromDate.Text.Trim()));
            lstParam.Add(new SqlParameter("@ClosingDate", txtToDate.Text.Trim()));
            lstParam.Add(new SqlParameter("@UserId", Session["UserId"].ToString()));
            lstParam.Add(new SqlParameter("@CompId", cboCompanyName.SelectedValue));
            ds = objComm.FillDetails(lstParam, "Id_Fill_AgingInventoryDetails");
            if (ds.Tables.Count > 0)
            {
                Hid_GSecExposure.Value = JsonConvert.SerializeObject(ds.Tables[0]);
                Hid_NonGSecExposure.Value = JsonConvert.SerializeObject(ds.Tables[1]);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        //CalculateYeild();
        //return;

        DataSet ds = new DataSet();
        DataTable dtSecurity = new DataTable();
        DataTable dtSecurityInfo = new DataTable();
        DataTable dtData = new DataTable();
        List<SqlParameter> lstParam = new List<SqlParameter>();
        int i = 0;

        try
        {
            dtData.Columns.Add("SecurityId", typeof(String));
            dtData.Columns.Add("ISINNumber", typeof(String));
            dtData.Columns.Add("Rate", typeof(Decimal));
            dtData.Columns.Add("Yield", typeof(Decimal));
            dtData.Columns.Add("Rate1", typeof(Decimal));
            dtData.Columns.Add("Rate2", typeof(Decimal));

            ds = objComm.FillDetails(lstParam, "Fill_SecurityMarketRate");
            dtSecurity = ds.Tables[0];
            dtSecurityInfo = ds.Tables[1];

            //DateTime YTMDate = System.DateTime.Today;
            DateTime YTMDate = Convert.ToDateTime(txtToDate.Text.Trim());
            foreach (DataRow row in dtSecurity.Rows)
            {
                int SecurityId = 0;
                double Rate = 0, Yield = 0, Rate1 = 0, Rate2 = 0;

                int intMaturity = 0, intCoupn = 0, intCall = 0, FrequencyOfInterest = 0;

                DateTime MaturityDate = DateTime.MinValue, CouponDate = DateTime.MinValue, CallDate = DateTime.MinValue, PutDate = DateTime.MinValue;
                double FaceValue = 0, MaturityAmt = 0, CouponRate = 0, CallAmt = 0, PutAmt = 0, dblResult = 0;
                string strTypeFlag = "", ISIN = "", PriceType = "", AnnOrSemi = "A";

                SecurityId = Int32.Parse(row["SecurityId"].ToString());
                FaceValue = Double.Parse(row["FaceValue"].ToString());
                ISIN = objComm.Trim(row["ISIN"].ToString());
                strTypeFlag = objComm.Trim(row["TypeFlag"].ToString());
                Yield = Double.Parse(row["Yield"].ToString());
                FrequencyOfInterest = Int32.Parse(row["FrequencyOfInterest"].ToString());
                PriceType = objComm.Trim(row["PriceType"].ToString());
                if (strTypeFlag == "G")
                {
                    AnnOrSemi = "S";
                }
                DataRow[] rowsMapFields = dtSecurityInfo.Select("SecurityId=" + SecurityId.ToString());
                for (i = 0; i < rowsMapFields.Length; i++)
                {
                    if (rowsMapFields[i]["TypeFlag"].ToString() == "I")
                    {
                        CouponDate = DateTime.Parse(rowsMapFields[i]["Date"].ToString());
                        CouponRate = Double.Parse(rowsMapFields[i]["Amount"].ToString());
                        intCoupn += 1;
                    }
                    else if (rowsMapFields[i]["TypeFlag"].ToString() == "M")
                    {
                        MaturityDate = DateTime.Parse(rowsMapFields[i]["Date"].ToString());
                        MaturityAmt = Double.Parse(rowsMapFields[i]["Amount"].ToString());
                        intMaturity += 1;
                    }
                    else if (rowsMapFields[i]["TypeFlag"].ToString() == "C" && intCall == 0 && DateTime.Parse(rowsMapFields[i]["Date"].ToString()) > YTMDate)
                    {
                        CallDate = DateTime.Parse(rowsMapFields[i]["Date"].ToString());
                        CallAmt = Double.Parse(rowsMapFields[i]["Amount"].ToString());
                        intCall += 1;
                    }
                }

                if (strTypeFlag == "G" && intCoupn == 1 && (intMaturity == 1 || (intMaturity == 0 && intCall == 1)))
                {
                    if (PriceType == "M")
                    {
                        dblResult = Yield;
                        GlobalFuns.CalculateYield(YTMDate, FaceValue, 0, true, true, MaturityDate, MaturityAmt, CouponDate, CouponRate, CallDate, CallAmt, PutDate, PutAmt, Convert.ToInt16(FrequencyOfInterest), "M", dblResult, strTypeFlag);
                        Rate = Convert.ToDouble(GlobalFuns.decMarketRate);
                    }
                    else if (PriceType == "C")
                    {
                        dblResult = Yield;
                        GlobalFuns.CalculateYield(YTMDate, FaceValue, 0, true, true, MaturityDate, MaturityAmt, CouponDate, CouponRate, CallDate, CallAmt, PutDate, PutAmt, Convert.ToInt16(FrequencyOfInterest), "C", dblResult, strTypeFlag);
                        Rate = Convert.ToDouble(GlobalFuns.decMarketRate);
                    }

                    if (PriceType == "M")
                    {
                        dblResult = Yield - 0.01;
                        GlobalFuns.CalculateYield(YTMDate, FaceValue, 0, true, true, MaturityDate, MaturityAmt, CouponDate, CouponRate, CallDate, CallAmt, PutDate, PutAmt, Convert.ToInt16(FrequencyOfInterest), "M", dblResult, strTypeFlag);
                        Rate1 = Convert.ToDouble(GlobalFuns.decMarketRate);
                    }
                    else if (PriceType == "C")
                    {
                        dblResult = Yield - 0.01;
                        GlobalFuns.CalculateYield(YTMDate, FaceValue, 0, true, true, MaturityDate, MaturityAmt, CouponDate, CouponRate, CallDate, CallAmt, PutDate, PutAmt, Convert.ToInt16(FrequencyOfInterest), "C", dblResult, strTypeFlag);
                        Rate1 = Convert.ToDouble(GlobalFuns.decMarketRate);
                    }

                    if (PriceType == "M")
                    {
                        dblResult = Yield + 0.01;
                        GlobalFuns.CalculateYield(YTMDate, FaceValue, 0, true, true, MaturityDate, MaturityAmt, CouponDate, CouponRate, CallDate, CallAmt, PutDate, PutAmt, Convert.ToInt16(FrequencyOfInterest), "M", dblResult, strTypeFlag);
                        Rate2 = Convert.ToDouble(GlobalFuns.decMarketRate);
                    }
                    else if (PriceType == "C")
                    {
                        dblResult = Yield + 0.01;
                        GlobalFuns.CalculateYield(YTMDate, FaceValue, 0, true, true, MaturityDate, MaturityAmt, CouponDate, CouponRate, CallDate, CallAmt, PutDate, PutAmt, Convert.ToInt16(FrequencyOfInterest), "C", dblResult, strTypeFlag);
                        Rate2 = Convert.ToDouble(GlobalFuns.decMarketRate);
                    }
                }
                else
                {
                    if (PriceType == "M")
                    {
                        GlobalFuns.dblYTMAnn = Yield;
                        GlobalFuns.dblYTMSemi = Yield;
                    }
                    else if (PriceType == "C")
                    {
                        GlobalFuns.dblYTCAnn = Yield;
                        GlobalFuns.dblYTCSemi = Yield;
                    }

                    GlobalFuns.CalculateXIRRPrice(SecurityId, txtToDate.Text.Trim(), FrequencyOfInterest, false, AnnOrSemi, "R", "N",PriceType);
                    if (PriceType == "M")
                    {
                        Rate = GlobalFuns.dblPTM;
                    }
                    else if (PriceType == "C")
                    {
                        Rate = GlobalFuns.dblPTC;
                    }

                    //For PV01
                    if (PriceType == "M")
                    {
                        GlobalFuns.dblYTMAnn = Yield - 0.01;
                        GlobalFuns.dblYTMSemi = Yield - 0.01;
                    }
                    else if (PriceType == "C")
                    {
                        GlobalFuns.dblYTCAnn = Yield - 0.01;
                        GlobalFuns.dblYTCSemi = Yield - 0.01;
                    }

                    GlobalFuns.CalculateXIRRPrice(SecurityId, txtToDate.Text.Trim(), FrequencyOfInterest, false, AnnOrSemi, "R","N", PriceType);
                    if (PriceType == "M")
                    {
                        Rate1 = GlobalFuns.dblPTM;
                    }
                    else if (PriceType == "C")
                    {
                        Rate1 = GlobalFuns.dblPTC;
                    }

                    //For PV01
                    if (PriceType == "M")
                    {
                        GlobalFuns.dblYTMAnn = Yield + 0.01;
                        GlobalFuns.dblYTMSemi = Yield + 0.01;
                    }
                    else if (PriceType == "C")
                    {
                        GlobalFuns.dblYTCAnn = Yield + 0.01;
                        GlobalFuns.dblYTCSemi = Yield + 0.01;
                    }

                    GlobalFuns.CalculateXIRRPrice(SecurityId, txtToDate.Text.Trim(), FrequencyOfInterest, false, AnnOrSemi, "R","N", PriceType);
                    if (PriceType == "M")
                    {
                        Rate2 = GlobalFuns.dblPTM;
                    }
                    else if (PriceType == "C")
                    {
                        Rate2 = GlobalFuns.dblPTC;
                    }
                }

                dtData.Rows.Add(SecurityId, ISIN, Rate, Yield, Rate1, Rate2);
            }
            lstParam.Clear();
            lstParam.Add(new SqlParameter("@Ason", txtToDate.Text.Trim()));
            lstParam.Add(new SqlParameter("@Data", dtData));
            if (objComm.InsertUpdateDetails(lstParam, "Id_UpdateSecurityMarketRate") > 0)
            {
                //blnReturn = true;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void CalculateYeild()
    {
        DataSet ds = new DataSet();
        DataTable dtSecurity = new DataTable();
        DataTable dtSecurityInfo = new DataTable();
        DataTable dtData = new DataTable();
        List<SqlParameter> lstParam = new List<SqlParameter>();
        int i = 0;

        try
        {
            dtData.Columns.Add("SecurityId", typeof(String));
            dtData.Columns.Add("ISINNumber", typeof(String));
            dtData.Columns.Add("Rate", typeof(Decimal));
            dtData.Columns.Add("Yield", typeof(Decimal));
            dtData.Columns.Add("Rate1", typeof(Decimal));
            dtData.Columns.Add("Rate2", typeof(Decimal));

            ds = objComm.FillDetails(lstParam, "Fill_SecurityMarketRate");
            dtSecurity = ds.Tables[0];
            dtSecurityInfo = ds.Tables[1];

            //DateTime YTMDate = System.DateTime.Today;
            DateTime YTMDate = Convert.ToDateTime(txtToDate.Text.Trim());
            foreach (DataRow row in dtSecurity.Rows)
            {
                int SecurityId = 0, DealSlipId = 0;
                double Rate = 0, Yield = 0, Rate1 = 0, Rate2 = 0;

                int intMaturity = 0, intCoupn = 0, intCall = 0, FrequencyOfInterest = 0;

                DateTime MaturityDate = DateTime.MinValue, CouponDate = DateTime.MinValue, CallDate = DateTime.MinValue, PutDate = DateTime.MinValue;
                double FaceValue = 0, MaturityAmt = 0, CouponRate = 0, CallAmt = 0, PutAmt = 0, dblResult = 0;
                string strTypeFlag = "", ISIN = "", PriceType = "", AnnOrSemi = "A", SettmentDate = "", NatureOFInstrument = "";

                SecurityId = Int32.Parse(row["SecurityId"].ToString());
                FaceValue = Double.Parse(row["FaceValue"].ToString());
                ISIN = objComm.Trim(row["ISIN"].ToString());
                strTypeFlag = objComm.Trim(row["TypeFlag"].ToString());
                //Yield = Double.Parse(row["Yield"].ToString());
                FrequencyOfInterest = Int32.Parse(row["FrequencyOfInterest"].ToString());
                PriceType = objComm.Trim(row["PriceType"].ToString());
                Rate = Double.Parse(row["Rate"].ToString());
                SettmentDate = objComm.Trim(row["SettmentDate"].ToString());
                NatureOFInstrument = objComm.Trim(row["NatureOFInstrument"].ToString());
                DealSlipId = Int32.Parse(row["DealSlipId"].ToString());

                DataRow[] rowsMapFields = dtSecurityInfo.Select("SecurityId=" + SecurityId.ToString());
                for (i = 0; i < rowsMapFields.Length; i++)
                {
                    if (rowsMapFields[i]["TypeFlag"].ToString() == "I")
                    {
                        CouponDate = DateTime.Parse(rowsMapFields[i]["Date"].ToString());
                        CouponRate = Double.Parse(rowsMapFields[i]["Amount"].ToString());
                        intCoupn += 1;
                    }
                    else if (rowsMapFields[i]["TypeFlag"].ToString() == "M")
                    {
                        MaturityDate = DateTime.Parse(rowsMapFields[i]["Date"].ToString());
                        MaturityAmt = Double.Parse(rowsMapFields[i]["Amount"].ToString());
                        intMaturity += 1;
                    }
                    else if (rowsMapFields[i]["TypeFlag"].ToString() == "C" && intCall == 0 && DateTime.Parse(rowsMapFields[i]["Date"].ToString()) > YTMDate)
                    {
                        CallDate = DateTime.Parse(rowsMapFields[i]["Date"].ToString());
                        CallAmt = Double.Parse(rowsMapFields[i]["Amount"].ToString());
                        intCall += 1;
                    }
                }

                GlobalFuns.dblYTMAnn = 0;
                GlobalFuns.dblYTCAnn = 0;

                GlobalFuns.CalculateXIRRYield(SecurityId, SettmentDate, Rate, 0, false, "R", "N","M");

                if (NatureOFInstrument == "NP")
                {
                    if (!Double.IsNaN(GlobalFuns.dblYTMAnn))
                    {
                        Yield = GlobalFuns.dblYTMAnn;
                    }
                }
                else
                {
                    if (!Double.IsNaN(GlobalFuns.dblYTCAnn))
                    {
                        Yield = GlobalFuns.dblYTCAnn;
                    }
                }

                dtData.Rows.Add(SecurityId, ISIN, Rate, Yield, 0, DealSlipId);
            }
            lstParam.Clear();
            lstParam.Add(new SqlParameter("@Ason", txtToDate.Text.Trim()));
            lstParam.Add(new SqlParameter("@Data", dtData));
            objComm.InsertUpdateDetails(lstParam, "Id_UpdateSecurityMarketRate");
        }
        catch (Exception)
        {
            throw;
        }
    }
}