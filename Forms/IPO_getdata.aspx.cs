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
using System.Web.Services;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Web.Script;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

public partial class Forms_IPO_getdata : System.Web.UI.Page
{
    string strPageName = "";
    string strReturn = "Oops, some error occurred.";

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D);
        Response.Expires = -1500;
        Response.CacheControl = "no-cache";
        Response.AppendHeader("Cache-Control", "no-store");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        try
        {
            if (!string.IsNullOrEmpty(Request.QueryString["pagename"]))
            {
                strPageName = Request.QueryString["pagename"];
            }

            if (strPageName == "issuemaster")
            {
                strReturn = SaveIssueMaster();
            }
            else if (strPageName == "issuedetails")
            {
                strReturn = SaveIssueDetails();
            }
            else if (strPageName == "fillissuedetails")
            {
                strReturn = FillIssueDescription();
            }
            else if (strPageName == "verticalsecuritydetails")
            {
                strReturn = FillVertical();
            }
            else if (strPageName == "stocksecuritydetails")
            {
                strReturn = FillStock();
            }
            else if (strPageName == "accrueddetails")
            {
                strReturn = FillAccruedDetails();
            }
            else if (strPageName == "financialstockdetails")
            {
                strReturn = FillFinancialStockDetails();
            }
            else if (strPageName == "otherstockdetails")
            {
                strReturn = FillOtherStockDetails();
            }
            else if (strPageName == "validatelimit")
            {
                strReturn = FillVerticalStock();
            }
            else if (strPageName == "updateratingspread")
            {
                strReturn = UpdateRatingSpread();
            }
            else if (strPageName == "validatestock")
            {
                strReturn = FillStockCheck();
            }
            else if (strPageName == "validateavailablestock")
            {
                strReturn = ValidateAvailableStock();
            }
            else if (strPageName == "getsettlementdate")
            {
                strReturn = GetSettlementDate();
            }
            
            Response.Write(strReturn);
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private string FillStockCheck()
    {
        List<SqlParameter> lstParam = new List<SqlParameter>();
        DataSet ds = new DataSet();
        ClsCommon objComm = new ClsCommon();
        string strData;
        try
        {

            lstParam.Add(new SqlParameter("@PortFolioID", Request.QueryString["PortfolioId"]));
            lstParam.Add(new SqlParameter("@Securityid", Request.QueryString["SecurityId"]));


            ds = objComm.FillDetails(lstParam, "ID_Check_Stock");

            strData = objComm.Trim(ds.Tables[0].Rows[0][0].ToString());

            return strData;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }

    private string FillStock()
    {
        List<SqlParameter> lstParam = new List<SqlParameter>();
        DataSet ds = new DataSet();
        ClsCommon objComm = new ClsCommon();
        string strMsg;
        try
        {
            lstParam.Add(new SqlParameter("@CompId", Request.QueryString["compid"]));
            lstParam.Add(new SqlParameter("@SecurityId", Request.QueryString["securityid"]));
            lstParam.Add(new SqlParameter("@AsOn", Request.QueryString["asondate"]));
            ds = objComm.FillDetails(lstParam, "CRM_Fill_StockSecDetails");

            strMsg = objComm.Trim(ds.Tables[0].Rows[0]["Data"].ToString());

            return strMsg;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }

    private string SaveIssueMaster()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        SqlCommand sqlComm = new SqlCommand();
        string strMsg;
        try
        {
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = "IPO_InsertUpdateIssueMaster";
            sqlComm.Parameters.Clear();

            if (!string.IsNullOrEmpty(Request.QueryString["IssueId"]))
            {
                sqlComm.Parameters.Add("IssueId", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.QueryString["IssueId"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["IssueType"]))
            {
                sqlComm.Parameters.Add("IssueType", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.Params["IssueType"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["IssueName"]))
            {
                sqlComm.Parameters.Add("IssueName", SqlDbType.VarChar, 150).Value = Request.Params["IssueName"];
            }
            if (!string.IsNullOrEmpty(Request.Params["IssuerId"]))
            {
                sqlComm.Parameters.Add("IssuerId", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.Params["IssuerId"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["InstrumentType"]))
            {
                sqlComm.Parameters.Add("InstrumentType", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.Params["InstrumentType"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["Seniority"]))
            {
                sqlComm.Parameters.Add("Seniority", SqlDbType.VarChar, 10).Value = Request.Params["Seniority"];
            }
            if (!string.IsNullOrEmpty(Request.Params["Listing"]))
            {
                sqlComm.Parameters.Add("Listing", SqlDbType.VarChar, 50).Value = Request.Params["Listing"];
            }
            if (!string.IsNullOrEmpty(Request.Params["IssueStatus"]))
            {
                sqlComm.Parameters.Add("IssueStatus", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.Params["IssueStatus"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["EffortBasis"]))
            {
                sqlComm.Parameters.Add("EffortBasis", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.Params["EffortBasis"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["Ragistrar"]))
            {
                sqlComm.Parameters.Add("Ragistrar", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.Params["Ragistrar"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["Depository"]))
            {
                sqlComm.Parameters.Add("Depository", SqlDbType.VarChar, 50).Value = Request.Params["Depository"];
            }
            if (!string.IsNullOrEmpty(Request.Params["Trustee"]))
            {
                sqlComm.Parameters.Add("Trustee", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.Params["Trustee"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["OurStatus"]))
            {
                sqlComm.Parameters.Add("OurStatus", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.Params["OurStatus"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["Nomenclature"]))
            {
                sqlComm.Parameters.Add("Nomenclature", SqlDbType.VarChar, 50).Value = Request.Params["Nomenclature"];
            }
            if (!string.IsNullOrEmpty(Request.Params["MinApplicationSize"]))
            {
                sqlComm.Parameters.Add("MinApplicationSize", SqlDbType.Decimal).Value = Convert.ToDecimal(Request.Params["MinApplicationSize"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["InterestOnAppMoney"]))
            {
                sqlComm.Parameters.Add("InterestOnAppMoney", SqlDbType.Decimal).Value = Convert.ToDecimal(Request.Params["InterestOnAppMoney"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["InterestOnRefundMoney"]))
            {
                sqlComm.Parameters.Add("InterestOnRefundMoney", SqlDbType.Decimal).Value = Convert.ToDecimal(Request.Params["InterestOnRefundMoney"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["DefaultInterestRate"]))
            {
                sqlComm.Parameters.Add("DefaultInterestRate", SqlDbType.Decimal).Value = Convert.ToDecimal(Request.Params["DefaultInterestRate"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["Series"]))
            {
                sqlComm.Parameters.Add("Series", SqlDbType.VarChar, 50).Value = Request.Params["Series"];
            }
            if (!string.IsNullOrEmpty(Request.Params["OpeningDate"]))
            {
                sqlComm.Parameters.Add("@OpeningDate", SqlDbType.VarChar, 10).Value = Request.Params["OpeningDate"];
            }
            if (!string.IsNullOrEmpty(Request.Params["ClosingDate"]))
            {
                sqlComm.Parameters.Add("@ClosingDate", SqlDbType.VarChar, 10).Value = Request.Params["ClosingDate"];
            }
            if (!string.IsNullOrEmpty(Request.Params["AllotmentDate"]))
            {
                sqlComm.Parameters.Add("@AllotmentDate", SqlDbType.VarChar, 10).Value = Request.Params["AllotmentDate"];
            }
            if (!string.IsNullOrEmpty(Request.Params["IssuePrice"]))
            {
                sqlComm.Parameters.Add("@IssuePrice", SqlDbType.Decimal).Value = Convert.ToDecimal(Request.Params["IssuePrice"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["IssuePriceMultiple"]))
            {
                sqlComm.Parameters.Add("@IssuePriceMultiple", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.Params["IssuePriceMultiple"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["FaceValue"]))
            {
                sqlComm.Parameters.Add("@FaceValue", SqlDbType.Decimal).Value = Convert.ToDecimal(Request.Params["FaceValue"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["FaceValueMutiple"]))
            {
                sqlComm.Parameters.Add("@FaceValueMutiple", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.Params["FaceValueMutiple"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["IssueSize"]))
            {
                sqlComm.Parameters.Add("@IssueSize", SqlDbType.Decimal).Value = Convert.ToDecimal(Request.Params["IssueSize"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["IssueSizeMultiple"]))
            {
                sqlComm.Parameters.Add("@IssueSizeMultiple", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.Params["IssueSizeMultiple"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["GreenShoe"]))
            {
                sqlComm.Parameters.Add("@GreenShoe", SqlDbType.Char, 1).Value = Request.Params["GreenShoe"];
            }
            if (!string.IsNullOrEmpty(Request.Params["GreenShoeType"]))
            {
                sqlComm.Parameters.Add("@GreenShoeType", SqlDbType.Char, 1).Value = Request.Params["GreenShoeType"];
            }
            if (!string.IsNullOrEmpty(Request.Params["GreenShoeSize"]))
            {
                sqlComm.Parameters.Add("@GreenShoeSize", SqlDbType.Decimal).Value = Convert.ToDecimal(Request.Params["GreenShoeSize"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["GreenShoeSizeMultiple"]))
            {
                sqlComm.Parameters.Add("@GreenShoeSizeMultiple", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.Params["GreenShoeSizeMultiple"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["FinalIssueAmt"]))
            {
                sqlComm.Parameters.Add("@FinalIssueAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(Request.Params["FinalIssueAmt"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["FinalIssueAmtMutiple"]))
            {
                sqlComm.Parameters.Add("@FinalIssueAmtMutiple", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.Params["FinalIssueAmtMutiple"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["UnderwrittenAmt"]))
            {
                sqlComm.Parameters.Add("@UnderwrittenAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(Request.Params["UnderwrittenAmt"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["UnderwrittenAmtMutiple"]))
            {
                sqlComm.Parameters.Add("@UnderwrittenAmtMutiple", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.Params["UnderwrittenAmtMutiple"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["ProcurementAmt"]))
            {
                sqlComm.Parameters.Add("@ProcurementAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(Request.Params["ProcurementAmt"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["ProcurementAmtMutiple"]))
            {
                sqlComm.Parameters.Add("@ProcurementAmtMutiple", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.Params["ProcurementAmtMutiple"]);
            }

            if (!string.IsNullOrEmpty(Request.Params["Rating"]))
            {
                XmlDocument xmlDoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + Request.Params["Rating"] + "}", "root");
                sqlComm.Parameters.Add("@RatingDetails_Xml", SqlDbType.Xml).Value = xmlDoc.OuterXml;
            }
            if (!string.IsNullOrEmpty(Session["UserId"].ToString()))
            {
                sqlComm.Parameters.Add("UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(Session["UserId"]);
            }
            sqlConn.Open();
            strMsg = sqlComm.ExecuteScalar().ToString();
            return strMsg;
        }
        catch (Exception ex)
        {
            throw ex;
            //Response.Write(ex.Message);
        }
        finally
        {
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
            }
            sqlConn = null;
        }
    }

    private string SaveIssueDetails()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        SqlCommand sqlComm = new SqlCommand();
        string strMsg;
        try
        {
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = "IPO_InsertUpdateIssuedetails";
            sqlComm.Parameters.Clear();

            if (!string.IsNullOrEmpty(Request.QueryString["IssueId"]))
            {
                sqlComm.Parameters.Add("IssueId", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.QueryString["IssueId"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["IssueDetailsId"]))
            {
                sqlComm.Parameters.Add("@IssueDetailsId", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.QueryString["IssueDetailsId"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["IssueDescription"]))
            {
                sqlComm.Parameters.Add("@IssueDescription", SqlDbType.VarChar, 200).Value = Request.Params["IssueDescription"];
            }
            if (!string.IsNullOrEmpty(Request.Params["ISINNumber"]))
            {
                sqlComm.Parameters.Add("@ISINNumber", SqlDbType.VarChar, 20).Value = Request.Params["ISINNumber"];
            }
            if (!string.IsNullOrEmpty(Request.Params["FrequencyOfInterest"]))
            {
                sqlComm.Parameters.Add("@FrequencyOfInterest", SqlDbType.Char, 1).Value = Request.Params["FrequencyOfInterest"];
            }
            if (!string.IsNullOrEmpty(Request.Params["FirstInterestDate"]))
            {
                sqlComm.Parameters.Add("@FirstInterestDate", SqlDbType.VarChar, 10).Value = Request.Params["FirstInterestDate"];
            }
            if (!string.IsNullOrEmpty(Request.Params["CouponType"]))
            {
                sqlComm.Parameters.Add("@CouponType", SqlDbType.Char, 1).Value = Request.Params["CouponType"];
            }
            if (!string.IsNullOrEmpty(Request.Params["IPDates"]))
            {
                sqlComm.Parameters.Add("@IPDates", SqlDbType.VarChar, 50).Value = Request.Params["IPDates"];
            }
            if (!string.IsNullOrEmpty(Request.Params["InterestPaymentMode"]))
            {
                sqlComm.Parameters.Add("@InterestPaymentMode", SqlDbType.VarChar, 20).Value = Request.Params["InterestPaymentMode"];
            }
            if (!string.IsNullOrEmpty(Request.Params["Tenor"]))
            {
                sqlComm.Parameters.Add("@Tenor", SqlDbType.Char, 1).Value = Request.Params["Tenor"];
            }
            if (!string.IsNullOrEmpty(Request.Params["Remarks"]))
            {
                sqlComm.Parameters.Add("@Remarks", SqlDbType.VarChar, 500).Value = Request.Params["Remarks"];
            }
            if (!string.IsNullOrEmpty(Request.Params["IssueDetailsInfo"]))
            {
                XmlDocument xmlDoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + Request.Params["IssueDetailsInfo"] + "}", "root");
                sqlComm.Parameters.Add("@IssueDetailsInfo_Xml", SqlDbType.Xml).Value = xmlDoc.OuterXml;
            }
            if (!string.IsNullOrEmpty(Session["UserId"].ToString()))
            {
                sqlComm.Parameters.Add("UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(Session["UserId"]);
            }
            sqlConn.Open();
            strMsg = sqlComm.ExecuteScalar().ToString();
            return strMsg;
        }
        catch (Exception ex)
        {
            throw ex;
            //Response.Write(ex.Message);
        }
        finally
        {
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
            }
            sqlConn = null;
        }
    }

    private string FillIssueDescription()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        SqlCommand sqlComm = new SqlCommand();
        string strMsg;
        try
        {
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = "IPO_Fill_IssueDescription";
            sqlComm.Parameters.Clear();

            if (!string.IsNullOrEmpty(Request.QueryString["IssueId"]))
            {
                sqlComm.Parameters.Add("IssueId", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.QueryString["IssueId"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["IssueDetailsId"]))
            {
                sqlComm.Parameters.Add("@IssueDetailsId", SqlDbType.Int, 4).Value = Convert.ToInt32(Request.QueryString["IssueDetailsId"]);
            }
            if (!string.IsNullOrEmpty(Request.Params["type"]))
            {
                sqlComm.Parameters.Add("@Type", SqlDbType.Char, 1).Value = Request.QueryString["type"];
            }
            if (!string.IsNullOrEmpty(Session["UserId"].ToString()))
            {
                sqlComm.Parameters.Add("UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(Session["UserId"]);
            }
            sqlConn.Open();
            strMsg = sqlComm.ExecuteScalar().ToString();
            return strMsg;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
            }
            sqlConn = null;
        }
    }

    private string FillVertical()
    {
        List<SqlParameter> lstParam = new List<SqlParameter>();
        DataSet ds = new DataSet();
        ClsCommon objComm = new ClsCommon();
        string strMsg;
        try
        {
            lstParam.Add(new SqlParameter("@AsOnDate", Request.QueryString["asondate"]));
            lstParam.Add(new SqlParameter("@CompId", Request.QueryString["compid"]));
            lstParam.Add(new SqlParameter("@VerticalId", Request.QueryString["verticalid"]));
            lstParam.Add(new SqlParameter("@SecurityId", Request.QueryString["securityid"]));

            ds = objComm.FillDetails(lstParam, "TS_Fill_StockSecurityDetails");

            strMsg = objComm.Trim(ds.Tables[0].Rows[0]["Data"].ToString());

            return strMsg;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }

    private string FillAccruedDetails()
    {
        List<SqlParameter> lstParam = new List<SqlParameter>();
        DataSet ds = new DataSet();
        ClsCommon objComm = new ClsCommon();
        string strData;
        try
        {
            lstParam.Add(new SqlParameter("@SecurityId", Request.QueryString["securityid"]));
            lstParam.Add(new SqlParameter("@Dated", Request.QueryString["dated"]));
            lstParam.Add(new SqlParameter("@FaceValue", Request.QueryString["facevalue"]));
            ds = objComm.FillDetails(lstParam, "TS_Fill_TransferStockAccruedDetails");

            strData = objComm.Trim(ds.Tables[0].Rows[0]["Data"].ToString());

            return strData;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }

    private string FillFinancialStockDetails()
    {
        List<SqlParameter> lstParam = new List<SqlParameter>();
        DataSet ds = new DataSet();
        ClsCommon objComm = new ClsCommon();
        string strData;
        try
        {
            lstParam.Add(new SqlParameter("@CompId", Request.QueryString["compid"]));
            if (!string.IsNullOrEmpty(Request.QueryString["verticalid"]))
            {
                lstParam.Add(new SqlParameter("@VerticalId", Request.QueryString["verticalid"]));
            }
            lstParam.Add(new SqlParameter("@SecurityId", Request.QueryString["securityid"]));
            lstParam.Add(new SqlParameter("@AsOn", Request.QueryString["asondate"]));
            ds = objComm.FillDetails(lstParam, "TS_Fill_FinancialStockDetails");

            strData = objComm.Trim(ds.Tables[0].Rows[0]["Data"].ToString());

            return strData;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }

    private string FillOtherStockDetails()
    {
        List<SqlParameter> lstParam = new List<SqlParameter>();
        DataSet ds = new DataSet();
        ClsCommon objComm = new ClsCommon();
        string strData;
        try
        {
            lstParam.Add(new SqlParameter("@CompId", Request.QueryString["compid"]));
            if (!string.IsNullOrEmpty(Request.QueryString["verticalid"]))
            {
                lstParam.Add(new SqlParameter("@VerticalId", Request.QueryString["verticalid"]));
            }
            lstParam.Add(new SqlParameter("@SecurityId", Request.QueryString["securityid"]));
            lstParam.Add(new SqlParameter("@AsOn", Request.QueryString["asondate"]));
            ds = objComm.FillDetails(lstParam, "TS_Fill_OtherStockDetails");

            strData = objComm.Trim(ds.Tables[0].Rows[0]["Data"].ToString());

            return strData;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }

    private string FillVerticalStock()
    {
        List<SqlParameter> lstParam = new List<SqlParameter>();
        DataSet ds = new DataSet();
        ClsCommon objComm = new ClsCommon();
        string strData;
        try
        {
            if (!string.IsNullOrEmpty(Session["CompId"].ToString()))
            {
                lstParam.Add(new SqlParameter("@CompId", Convert.ToInt32(Session["CompId"])));
            }
            lstParam.Add(new SqlParameter("@VerticalId", Request.QueryString["verticalid"]));
            lstParam.Add(new SqlParameter("@FaceValue", Request.QueryString["facevalue"]));
            lstParam.Add(new SqlParameter("@AsOn", Request.QueryString["dated"]));
            if (!string.IsNullOrEmpty(Request.QueryString["rate"]))
            {
                lstParam.Add(new SqlParameter("@Rate", Request.QueryString["rate"]));
            }
            if (!string.IsNullOrEmpty(Request.QueryString["securityid"]))
            {
                lstParam.Add(new SqlParameter("@SecurityId", Request.QueryString["securityid"]));
            }
            if (!string.IsNullOrEmpty(Request.QueryString["noofbond"]))
            {
                lstParam.Add(new SqlParameter("@NoofBond", Request.QueryString["noofbond"]));
            }
            if (!string.IsNullOrEmpty(Request.QueryString["fvornoofbond"]))
            {
                lstParam.Add(new SqlParameter("@FaceValueorNoOfBond", Request.QueryString["fvornoofbond"]));
            }

            ds = objComm.FillDetails(lstParam, "TS_Fill_VerticalStock");

            strData = objComm.Trim(ds.Tables[0].Rows[0]["Data"].ToString());

            return strData;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }

    private string UpdateRatingSpread()
    {
        List<SqlParameter> lstParam = new List<SqlParameter>();
        DataSet ds = new DataSet();
        ClsCommon objComm = new ClsCommon();
        string strData = "";
        try
        {
            lstParam.Add(new SqlParameter("@AsOn", Request.Params["dated"]));
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(Request.Params["data"], (typeof(DataTable)));
            lstParam.Add(new SqlParameter("@SpreadData", dt));
            if (!string.IsNullOrEmpty(Session["UserId"].ToString()))
            {
                lstParam.Add(new SqlParameter("@UserId", Convert.ToInt32(Session["UserId"])));
            }

            strData = objComm.InsertUpdateDeleteDetailsMsg(lstParam, "TS_InsertUpdate_RatingSpread");

            return strData;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }

    private string ValidateAvailableStock()
    {
        List<SqlParameter> lstParam = new List<SqlParameter>();
        DataSet ds = new DataSet();
        ClsCommon objComm = new ClsCommon();
        string strData = "";
        try
        {
            lstParam.Add(new SqlParameter("@DealerId", !string.IsNullOrEmpty(Request.Params["dealerid"]) ? Request.Params["dealerid"] : null));

            if (!string.IsNullOrEmpty(Request.Params["portfolioid"]))
            {
                lstParam.Add(new SqlParameter("@PortfolioId", Request.Params["portfolioid"]));
            }
            if (!string.IsNullOrEmpty(Request.Params["securityid"]))
            {
                lstParam.Add(new SqlParameter("@SecurityId", Request.Params["securityid"]));
            }
            if (!string.IsNullOrEmpty(Request.Params["dated"]))
            {
                lstParam.Add(new SqlParameter("@AsOn", Request.Params["dated"]));
            }
            if (!string.IsNullOrEmpty(Request.Params["dealslipid"]))
            {
                lstParam.Add(new SqlParameter("@DealSlipId", Request.Params["dealslipid"]));
            }
            if (!string.IsNullOrEmpty(Request.Params["facevalue"]))
            {
                lstParam.Add(new SqlParameter("@FaceValue", Request.Params["facevalue"]));
            }
            if (!string.IsNullOrEmpty(Session["CompId"].ToString()))
            {
                lstParam.Add(new SqlParameter("@CompId", Convert.ToInt32(Session["CompId"])));
            }

            ds = objComm.FillDetails(lstParam, "Id_Check_AvailableStock");

            strData = objComm.Trim(ds.Tables[0].Rows[0]["Data"].ToString());
            return strData;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }

    private string GetSettlementDate()
    {
        List<SqlParameter> lstParam = new List<SqlParameter>();
        DataSet ds = new DataSet();
        ClsCommon objComm = new ClsCommon();
        string strData = "";
        try
        {
            lstParam.Add(new SqlParameter("@DealDate", !string.IsNullOrEmpty(Request.Params["dealdate"]) ? Request.Params["dealdate"] : null));
            lstParam.Add(new SqlParameter("@SettlementType", !string.IsNullOrEmpty(Request.Params["settlementtype"]) ? Request.Params["settlementtype"] : null));

            ds = objComm.FillDetails(lstParam, "ID_Fill_SettlementDate");

            strData = objComm.Trim(ds.Tables[0].Rows[0]["Data"].ToString());
            return strData;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }

//    //private void FillSettDate()
//{
//    try
//    {
//            ClsCommon objComm = new ClsCommon();
//            Int16 intLoop = 0;
//        int count = 0;
//        DateTime incDate;

//        incDate = objComm.DateFormat(Request.Params["dealdate"]);
//        while (count < cbo_SettDay.SelectedValue)
//        {
//            incDate = DateTime.DateAdd(DateInterval.Day, 1, incDate);
//            if (checkdate(incDate) == true)
//                count = count - 1;
//            count = count + 1;
//        }
//        txt_SettmentDate.Text = Strings.Format(incDate, "dd/MM/yyyy");
//    }
//    catch (Exception ex)
//    {
//        objUtil.WritErrorLog(PgName, "FillSettDate", "Error in FillSettDate", "", ex);
//        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "msg", "AlertMessage(Validation,'" + ex.Message.Replace("'", " ").Replace(Strings.Chr(13), " ").Replace(Strings.Chr(10), " ") + "',175,450);", true);
//    }
//}

    

}