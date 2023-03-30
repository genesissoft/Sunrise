<%@ WebHandler Language="C#" Class="getdata" %>
using System;
using System.Web;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web.SessionState;

using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using System.Web.Script.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

public class getdata : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public string Cols;
    public string Tabs;
    public string Proc;
    public string ManualQry;
    public int Id;
    public string Sord;
    public string Sidx;
    public int Rows;
    public int Pages;
    public bool _Search;
    public string SearchQuery;
    public string strOperator;
    public string strCPCD;

    SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
    SqlCommand sqlComm = new SqlCommand();
    DataTable dt = new DataTable();
    DataTable dtSecurity = new DataTable();
    ClsCommon objComm = new ClsCommon();
    public void ProcessRequest(HttpContext context)
    {
        context.Response.AddHeader("Pragma", "no-cache");
        context.Response.AddHeader("Cache-Control", "no-cache");
        context.Response.AddHeader("Cache-Control", "no-store");
        context.Response.ContentType = "text/plain";

        try
        {
            string strPageName = string.IsNullOrEmpty(context.Request.QueryString["pagename"]) ? "" : context.Request.QueryString["PageName"];
            string strTableName = string.IsNullOrEmpty(context.Request.QueryString["tablename"]) ? "" : context.Request.QueryString["TableName"];
            string strOpern = string.IsNullOrEmpty(context.Request.QueryString["oper"]) ? "" : context.Request.QueryString["oper"];
            string strTask = "";

            if (strPageName != "")
            {
                strTask = strPageName;
            }
            if (strTableName != "")
            {
                strTask = strTask + "_" + strTableName;
            }

            if (strOpern != "")
            {
                strTask = strTask + "_" + strOpern;
            }
            else
            {
                strTask = strTask + "_" + "get";
            }

            switch (strTask)
            {

                case "SecurityMarketRate_get":
                    context.Response.Write(FillData("TS_Fill_SecurityMarketRateDetails"));
                    break;
                case "RatingSpread_get":
                    context.Response.Write(FillData("TS_Fill_SecurityRatingSpreadDetails"));
                    break;

                case "SelldownTracking_get":
                    context.Response.Write(FillData("TS_Fill_SellDownTrackingDetails"));
                    break;

                case "search_NameOfSecurity_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_NameOfSecurity_data_get":
                    context.Response.Write(FillData("ID_SEARCH_SecurityName_"));
                    break;

                case "search_Security__model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_Security__data_get":
                    context.Response.Write(FillData("ID_SEARCH_Security_"));
                    break;

                case "search_RedeemedSecurity_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_RedeemedSecurity_data_get":
                    context.Response.Write(FillData("ID_SEARCH_RedeemedSecurity_"));
                    break;

                case "search_NameOfIssuer_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_NameOfIssuer_data_get":
                    context.Response.Write(FillData("ID_SEARCH_SecurityIssuer_"));
                    break;

                case "search_NameOfIssuerRDM_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_NameOfIssuerRDM_data_get":
                    context.Response.Write(FillData("ID_SEARCH_SecurityIssuerRDM_"));
                    break;

                case "search_NameOFClient_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_NameOFClient_data_get":
                    context.Response.Write(FillData("ID_SEARCH_CustomerMaster_"));
                    break;
                case "search_StateName_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_CustodianName_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_CustodianName_data_get":
                    context.Response.Write(FillData("ID_SEARCH_CustodianMaster_"));
                    break;
                case "search_StateName_data_get":
                    context.Response.Write(FillData("ID_SEARCH_StateMaster_"));
                    break;
                case "search_FinancialDelivery_TransCode_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_FinancialDelivery_TransCode_data_get":
                    context.Response.Write(FillData("ID_SEARCH_FinancialInfo_"));
                    break;

                case "search_DMatDelivery_TransCode_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_DMatDelivery_TransCode_data_get":
                    context.Response.Write(FillData("ID_SEARCH_DMatDealSlipEntry_"));
                    break;

                case "search_WDM_TransCode_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_WDM_TransCode_data_get":
                    context.Response.Write(FillData("ID_SEARCH_WDMCencelDeal_"));
                    break;

                case "search_WDM_WDMTransCode_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_WDM_WDMTransCode_data_get":
                    context.Response.Write(FillData("ID_SEARCH_WDMPrintDeals_"));
                    break;

                case "search_CustomerAddress_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_CustomerAddress_data_get":
                    context.Response.Write(FillData("ID_SEARCH_ClientCustMultipleAddress_"));
                    break;

                case "search_BrokerName_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_BrokerName_data_get":
                    context.Response.Write(FillData("ID_SEARCH_BrokerMaster_"));
                    break;

                case "search_DealSlipNoAudit_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_DealSlipNoAudit_data_get":
                    context.Response.Write(FillData("ID_SEARCH_AuditDeals_"));
                    break;

                case "search_CustomerNameAudit_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_CustomerNameAudit_data_get":
                    context.Response.Write(FillData("ID_SEARCH_AuditCustomerMaster_"));
                    break;

                case "search_SecurityNameAudit_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_SecurityNameAudit_data_get":
                    context.Response.Write(FillData("ID_SEARCH_AuditSecurityMaster_"));
                    break;

                case "search_WDM_TransCodeAudit_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_WDM_TransCodeAudit_data_get":
                    context.Response.Write(FillData("ID_SEARCH_WDMDeals_"));
                    break;

                case "search_WDMTransCode_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_WDMTransCode_data_get":
                    context.Response.Write(FillData("ID_SEARCH_WDMPrintDeal_"));
                    break;

                case "search_WDMDirectDeal_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_WDMDirectDeal_data_get":
                    context.Response.Write(FillData("ID_SEARCH_WDMDirectDeal_"));
                    break;
                case "search_WDMDebitRefNo_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_WDMDebitRefNo_data_get":
                    context.Response.Write(FillData("ID_SEARCH_WDMDebitRefNo_"));
                    break;

                case "search_PrintDeals_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_PrintDeals_data_get":
                    context.Response.Write(FillData("ID_SEARCH_PrintDeals_"));
                    break;

                case "search_MergeDealNo_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_MergeDealNo_data_get":
                    context.Response.Write(FillData("ID_SEARCH_mergedealentry_"));
                    break;

                case "search_CancelDealNumber_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_CancelDealNumber_data_get":
                    context.Response.Write(FillData("ID_SEARCH_CancelDealnew_"));
                    break;

                case "search_DeleteDMatDealSlip_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_DeleteDMatDealSlip_data_get":
                    context.Response.Write(FillData("ID_SEARCH_DeleteDMatDealSlip_"));
                    break;

                case "search_DeleteFinancialDealSlip_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_DeleteFinancialDealSlip_data_get":
                    context.Response.Write(FillData("ID_SEARCH_DeleteFinanDealSlip_"));
                    break;
                case "search_BTBDealSlipNo_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_BTBDealSlipNo_data_get":
                    context.Response.Write(FillData("ID_SEARCH_BTBDealSlipNo_"));
                    break;
                case "search_BTBDealSlipNoOrderByDealDate_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_BTBDealSlipNoOrderByDealDate_data_get":
                    context.Response.Write(FillData("ID_SEARCH_BTBDealSlipNoOrdByDealdate_"));
                    break;
                case "search_CustomerMasterNew_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_CustomerMasterNew_data_get":
                    context.Response.Write(FillData("ID_SEARCH_CustomerMasterNew_"));
                    break;
                case "search_ConvertToPending_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;
                case "AcknowledgedDeals_get":
                    context.Response.Write(FillData("ID_FILL_AcknowledgedDeals"));
                    break;
                case "AcknowledgedDeals_delete":
                    context.Response.Write(DeletedRecord(context.Request.QueryString["id"], "ID_Delete_DealAckDetails"));
                    break;
                case "details_ipo_delete":
                    context.Response.Write(DeletedRecord(context.Request.QueryString["id"], "Trust_Delete_CreditNoteDetails_IPO"));
                    break;
                case "search_ConvertToPending_data_get":
                    context.Response.Write(FillData("ID_SEARCH_ConvertToPending"));
                    break;

                case "search_RetailDebitDeals_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_RetailDebitDeals_data_get":
                    context.Response.Write(FillData("ID_SEARCH_RetailDebitRefNo"));
                    break;

                case "search_CancelRetailDebitNo_model_get":
                    context.Response.Write(GetColModel(strTask));
                    break;

                case "search_CancelRetailDebitNo_data_get":
                    context.Response.Write(FillData("ID_SEARCH_CancelRetDebitNote"));
                    break;
                default:
                    context.Response.Write("");
                    break;
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("{\"msg\":\"<div class='error'>Some error occurred while uploading the file.</div>\",\"type\":\"I\"}");
        }
        finally
        {

        }
    }

    private string FillData(string strProc)
    {
        Int32 intTotalRecords = 0;
        List<SqlParameter> lstParam = new List<SqlParameter>();

        try
        {
            Id = 0;
            Sidx = Convert.ToString(HttpContext.Current.Request.Params["sidx"].ToString());
            Sord = Convert.ToString(HttpContext.Current.Request.Params["sord"].ToString());
            Rows = Convert.ToInt32(HttpContext.Current.Request.Params["rows"].ToString());
            Pages = Convert.ToInt32(HttpContext.Current.Request.Params["page"].ToString());
            _Search = Convert.ToBoolean(HttpContext.Current.Request.Params["_search"].ToString());
            //SearchQuery = string.IsNullOrEmpty(HttpContext.Current.Request.Params["filters"]) ? "" : HttpContext.Current.Request.Params["filters"];
            SearchQuery = string.IsNullOrEmpty(HttpContext.Current.Request.Params["SQLQuery"]) ? "" : HttpContext.Current.Request.Params["SQLQuery"];
            SearchQuery = SearchQuery.Replace("'", "");
            SearchQuery = SearchQuery.Replace("\"", "'");
            if (SearchQuery != "")
            {
                _Search = true;
            }


            string SortExp = (Sidx != "") ? Sidx + " " + Sord : "";
            int Start = ((Pages - 1) * Rows);

            GetFillParameter(ref lstParam, strProc, Id, Start, Rows, SortExp, _Search, SearchQuery);
            SqlParameter[] dbParam = lstParam.ToArray();

            sqlComm.Connection = sqlConn;
            sqlComm.CommandTimeout = 10000;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = strProc;
            sqlComm.Parameters.Clear();
            sqlComm.Parameters.AddRange(dbParam);

            SqlDataAdapter sqlDa = new SqlDataAdapter(sqlComm);

            sqlDa.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                if (strProc == "CRM_Fill_StockReportAll")
                {
                    //CalculateYield(ref dt);
                }
                intTotalRecords = Convert.ToInt32(dt.Rows[0]["CNT"]);
            }

            return (JsonForJqgrid(dt, Rows, intTotalRecords, Pages));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void GetFillParameter(ref List<SqlParameter> lstparam, string strProc, Int32 Id, Int32 Start, Int32 Rows, string SortExp, bool _Search, string SearchQuery)
    {
        try
        {
            lstparam.Add(new SqlParameter("@UserId", Convert.ToInt32(HttpContext.Current.Session["UserId"])));
            lstparam.Add(new SqlParameter("@id", Id));
            lstparam.Add(new SqlParameter("@start", Start));
            lstparam.Add(new SqlParameter("@rows", Rows));
            lstparam.Add(new SqlParameter("@sortExp", SortExp));
            lstparam.Add(new SqlParameter("@_search", _Search));
            lstparam.Add(new SqlParameter("@query", SearchQuery));
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["condition"]))
            {
                lstparam.Add(new SqlParameter("@condition", HttpContext.Current.Request.QueryString["condition"].ToString()));
            }
            switch (strProc)
            {

                case "TS_Fill_SellDownTrackingDetails":
                    lstparam.Add(new SqlParameter("@OpeningDate", objComm.Trim(HttpContext.Current.Request.Params["fromdate"]) != "" ? HttpContext.Current.Request.Params["fromdate"] : null));
                    lstparam.Add(new SqlParameter("@ClosingDate", objComm.Trim(HttpContext.Current.Request.Params["todate"]) != "" ? HttpContext.Current.Request.Params["todate"] : null));
                    lstparam.Add(new SqlParameter("@TempCompId", objComm.Trim(HttpContext.Current.Request.Params["tempcompid"]) != "" ? HttpContext.Current.Request.Params["tempcompid"] : null));
                    break;
                case "ID_FILL_AcknowledgedDeals":
                    lstparam.Add(new SqlParameter("@CompId", objComm.Trim((HttpContext.Current.Session["CompId"]).ToString()) != "" ? Convert.ToString(HttpContext.Current.Session["CompId"]) : null));
                    lstparam.Add(new SqlParameter("@YearId", objComm.Trim((HttpContext.Current.Session["YearId"]).ToString()) != "" ? Convert.ToString(HttpContext.Current.Session["YearId"]) : null));
                    lstparam.Add(new SqlParameter("@UserTypeId", objComm.Trim((HttpContext.Current.Session["UserTypeId"]).ToString()) != "" ? Convert.ToString(HttpContext.Current.Session["UserTypeId"]) : null));
                    //lstparam.Add(new SqlParameter("@UserId", objComm.Trim((HttpContext.Current.Session["UserId"]).ToString()) != "" ? Convert.ToString(HttpContext.Current.Session["UserId"]) : null));
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private static string JsonForJqgrid(DataTable dt, int pageSize, int totalRecords, int page)
    {
        try
        {
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.Append("\"total\":" + totalPages + ",\"page\":" + page + ",\"records\":" + (totalRecords) + ",\"rows\"");
            jsonBuilder.Append(":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{\"id\":" + (i) + ",\"cell\":[");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(Convert.ToString(dt.Rows[i][j].ToString().Replace("\"", "").Replace("\\", "\\\\")).Trim());

                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("]},");
            }

            if (dt.Rows.Count != 0)
            {
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }
            //jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
        catch (Exception ex)
        {
            return "false";
            throw ex;
        }
    }

    private string DeletedRecord(string Id, string strProc)
    {

        string strMsg = "";
        try
        {
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = strProc;
            sqlComm.Parameters.Clear();
            sqlComm.Parameters.Add("@RecordId", SqlDbType.BigInt, 8).Value = Convert.ToInt64(Id);
            //sqlComm.Parameters.Add("@LoginId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session["LoginId"]);
            if (sqlConn.State == ConnectionState.Closed)
            {
                sqlConn.Open();
            }
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
        }
    }

    private string GetColModel(string pagename)
    {
        string strColumnName = "", strColumnModel = "";

        switch (pagename)
        {
            case "search_applicationentry_wdm_IssuerOfSecurity_model_get":
                strColumnName = "[\"SN\",\"\",\"Security Issuer\"]";

                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 15, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityIssuer\", \"index\": \"SecurityIssuer\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityIssuer\", \"index\": \"SecurityIssuer\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_NameOfSecurity_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Security Name\",\"Security Type Name\",\"Security Issuer\",\"Coupon Rate\",\"ISIN\",\"MaturityDate\",\"CallDate\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityName\", \"index\": \"SecurityName\", \"width\": 150, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityTypeName\", \"index\": \"SecurityTypeName\", \"width\": 70, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityIssuer\", \"index\": \"SecurityIssuer\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"CouponRate\", \"index\": \"CouponRate\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"ISIN\", \"index\": \"ISIN\", \"width\": 70, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"MaturityDate\", \"index\": \"MaturityDate\", \"width\": 70, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"CallDate\", \"index\": \"CallDate\", \"width\": 60, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";

                break;
            case "search_Security__model_get":
                strColumnName = "[\"SN\",\"Id\",\"Security Name\",\"Security Type Name\",\"Security Issuer\",\"Coupon Rate\",\"ISIN\",\"MaturityDate\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityName\", \"index\": \"SecurityName\", \"width\": 150, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityTypeName\", \"index\": \"SecurityTypeName\", \"width\": 70, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityIssuer\", \"index\": \"SecurityIssuer\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"CouponRate\", \"index\": \"CouponRate\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"ISIN\", \"index\": \"ISIN\", \"width\": 70, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"MaturityDate\", \"index\": \"MaturityDate\", \"width\": 60, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";

                break;
            case "search_RedeemedSecurity_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Security Name\",\"Security Type Name\",\"Security Issuer\",\"Coupon Rate\",\"ISIN\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityName\", \"index\": \"SecurityName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityTypeName\", \"index\": \"SecurityTypeName\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityIssuer\", \"index\": \"SecurityIssuer\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"CouponRate\", \"index\": \"CouponRate\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"ISIN\", \"index\": \"ISIN\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";

                break;
            case "search_NameOfIssuer_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Issuer Name\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityIssuer\", \"index\": \"SecurityIssuer\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_NameOfIssuerRDM_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Issuer Name\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityIssuer\", \"index\": \"SecurityIssuer\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_CustodianName_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Custodian Name\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"CustodianName\", \"index\": \"CustodianName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;
            //case "search_NameOFClient_model_get":
            //    strColumnName = "[\"SN\",\"Id\",\"Customer Name\",\"Customer City\",\"Customer Phone\"]";
            //    strColumnModel = "[";
            //    strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
            //    strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
            //    strColumnModel = strColumnModel + "{ \"name\": \"CustomerName\", \"index\": \"CustomerName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
            //    strColumnModel = strColumnModel + "{ \"name\": \"CustomerCity\", \"index\": \"CustomerCity\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
            //    strColumnModel = strColumnModel + "{ \"name\": \"CustomerPhone\", \"index\": \"CustomerPhone\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
            //    strColumnModel = strColumnModel + "]";
            //    break;

            case "search_NameOFClient_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Customer Name\",\"Customer Type\",\"Customer City\",\"PAN\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"CustomerName\", \"index\": \"CustomerName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"CustomerTypeName\", \"index\": \"CustomerTypeName\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"CustomerCity\", \"index\": \"CustomerCity\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"PANNumber\", \"index\": \"PANNumber\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;


            case "search_StateName_model_get":
                strColumnName = "[\"SN\",\"Id\",\"State Name\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"StateName\", \"index\": \"StateName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                //strColumnModel = strColumnModel + "{ \"name\": \"StampDuty\", \"index\": \"StampDuty\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_FinancialDelivery_TransCode_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Deal Slip No\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealSlipNo\", \"index\": \"DealSlipNo\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_DMatDelivery_TransCode_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Deal Slip No\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealSlipNo\", \"index\": \"DealSlipNo\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_WDM_TransCode_model_get":
                strColumnName = "[\"SN\",\"Id\",\"WDM Deal Number\",\"Buy Sr No\",\"Sell Sr No\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"WDMDealNumber\", \"index\": \"WDMDealNumber\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"BuySrNo\", \"index\": \"BuySrNo\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SellSrNo\", \"index\": \"SellSrNo\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_WDM_WDMTransCode_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Deal Slip No\",\"Sr No\",\"Customer Name\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealSlipNo\", \"index\": \"DealSlipNo\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SrNo\", \"index\": \"SrNo\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"CustomerName\", \"index\": \"CustomerName\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_CustomerAddress_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Address1\",\"Address2\",\"City\",\"Pin Code\",\"Phone No\",\"Fax No\",\"Email Id\",\"Address\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"Address1\", \"index\": \"Address1\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"Address2\", \"index\": \"Address2\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"City\", \"index\": \"City\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"PinCode\", \"index\": \"PinCode\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"Phone\", \"index\": \"Phone\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"FaxNo\", \"FaxNo\": \"PinCode\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"EmailId\", \"index\": \"EmailId\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"Address\", \"index\": \"Address\", \"width\": 100, \"sorttype\": \"text\", \"hidden\": true, \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_BrokerName_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Broker Name\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"BrokerName\", \"index\": \"BrokerName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_DealSlipNoAudit_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Deal Slip No\",\"Deal Date\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealSlipNo\", \"index\": \"DealSlipNo\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealDate\", \"index\": \"DealDate\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_CustomerNameAudit_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Customer Name\",\"PAN Number\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"CustomerName\", \"index\": \"CustomerName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"PANNumber\", \"index\": \"PANNumber\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_SecurityNameAudit_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Security Name\",\"ISIN\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityName\", \"index\": \"SecurityName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"ISIN\", \"index\": \"ISIN\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_WDM_TransCodeAudit_model_get":
                strColumnName = "[\"SN\",\"Id\",\"WDM Deal Number\",\"Deal Date\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"WDMDealNumber\", \"index\": \"WDMDealNumber\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealDate\", \"index\": \"DealDate\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_WDMTransCode_model_get":
                strColumnName = "[\"SN\",\"Id\",\"WDM Deal Number\",\"Deal Date\",\"Security Name\", \"Buy Sr No\",\"Sell Sr No\", \"Exchange Name\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"WDMDealNumber\", \"index\": \"WDMDealNumber\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealDate\", \"index\": \"DealDate\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityName\", \"index\": \"SecurityName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"BuySrNo\", \"index\": \"BuySrNo\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SellSrNo\", \"index\": \"SellSrNo\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"ExchangeName\", \"index\": \"ExchangeName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;
            case "search_WDMDirectDeal_model_get":
                strColumnName = "[\"SN\",\"Id\",\"WDM Deal Number\",\"Deal Date\",\"Customer Name\", \"Security Name\",\"Exchange Name\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"WDMDealNumber\", \"index\": \"WDMDealNumber\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealDate\", \"index\": \"DealDate\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"CustomerName\", \"index\": \"CustomerName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityName\", \"index\": \"SecurityName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"ExchangeName\", \"index\": \"ExchangeName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;
            case "search_WDMDebitRefNo_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Debit RefNo\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"DebitRefNo\", \"index\": \"DebitRefNo\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;
            case "search_PrintDeals_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Deal Slip No\",\"Trade Date\",\"Customer Name\", \"Security Name\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealSlipNo\", \"index\": \"DealSlipNo\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"TradeDate\", \"index\": \"TradeDate\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"CustomerName\", \"index\": \"CustomerName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityName\", \"index\": \"SecurityName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_MergeDealNo_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Deal No\",\"Merge Deal No\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealNo\", \"index\": \"DealpNo\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"MergedealNo\", \"index\": \"MergedealNo\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;
            case "search_CancelDealNumber_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Deal Slip No\",\"Customer Name\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealSlipNo\", \"index\": \"DealSlipNo\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"CustomerName\", \"index\": \"CustomerName\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;
            case "search_DeleteDMatDealSlip_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Deal Slip No\",\"Customer Name\",\"Settlement Date\", \"Face Value\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealSlipNo\", \"index\": \"DealSlipNo\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"CustomerName\", \"index\": \"CustomerName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SettlementDate\", \"index\": \"SettlementDate\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"FaceValue\", \"index\": \"FaceValue\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;
            case "search_DeleteFinancialDealSlip_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Deal Slip No\",\"Customer Name\",\"Settlement Date\", \"Face Value\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealSlipNo\", \"index\": \"DealSlipNo\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"CustomerName\", \"index\": \"CustomerName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SettlementDate\", \"index\": \"SettlementDate\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"FaceValue\", \"index\": \"FaceValue\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;
            case "search_BTBDealSlipNo_model_get":
                strColumnName = "[\"SN\",\"Id\",\"DealSlipNo\",\"FinancialDealType\",\"CustomerName\",\"SecurityName\",\"DealDate\",\"SettlementDate\",\"Rate\",\"RemainingFaceValue\", \"DealDateTime\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealSlipNo\", \"index\": \"DealSlipNo\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"FinancialDealType\", \"index\": \"FinancialDealType\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"CustomerName\", \"index\": \"CustomerName\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityName\", \"index\": \"SecurityName\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealDate\", \"index\": \"DealDate\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SettlementDate\", \"index\": \"SettlementDate\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"Rate\", \"index\": \"Rate\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"RemainingFaceValue\", \"index\": \"RemainingFaceValue\", \"width\": 100, \"sorttype\": \"text\", \"hidden\": false, \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealDateTime\", \"index\": \"DealDateTime\", \"width\": 100, \"sorttype\": \"text\", \"hidden\": true, \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;
            case "search_BTBDealSlipNoOrderByDealDate_model_get":
                strColumnName = "[\"SN\",\"Id\",\"DealSlipNo\",\"FinancialDealType\",\"CustomerName\",\"SecurityName\",\"DealDate\",\"SettlementDate\",\"Rate\",\"RemainingFaceValue\", \"DealDateTime\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealSlipNo\", \"index\": \"DealSlipNo\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"FinancialDealType\", \"index\": \"FinancialDealType\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"CustomerName\", \"index\": \"CustomerName\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SecurityName\", \"index\": \"SecurityName\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealDate\", \"index\": \"DealDate\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"SettlementDate\", \"index\": \"SettlementDate\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"Rate\", \"index\": \"Rate\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"RemainingFaceValue\", \"index\": \"RemainingFaceValue\", \"width\": 100, \"sorttype\": \"text\", \"hidden\": false, \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealDateTime\", \"index\": \"DealDateTime\", \"width\": 100, \"sorttype\": \"text\", \"hidden\": true, \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;
            case "search_CustomerMasterNew_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Customer Name\"]";

                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 15, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"CustomerName\", \"index\": \"CustomerName\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;
            case "search_ConvertToPending_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Deal Slip No\",\"Customer Name\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"DealSlipNo\", \"index\": \"DealSlipNo\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"CustomerName\", \"index\": \"CustomerName\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;

            case "search_RetailDebitDeals_model_get":
                strColumnName = "[\"SN\",\"Id\",\"Invoice No\",\"Broker Name\",\"Invoice Date\", \"Amount\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"RefNo\", \"index\": \"RefNo\", \"width\": 50, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"BrokerName\", \"index\": \"BrokerName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"InvoiceDate\", \"index\": \"InvoiceDate\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"BrokerageAmount\", \"index\": \"BrokerageAmount\", \"width\": 80, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";
                strColumnModel = strColumnModel + "]";
                break;
            case "search_CancelRetailDebitNo_model_get":
                strColumnName = "[\"SN\",\"Id\",\"RefNo\",\"Distributor Name\",\"FromDate\",\"ToDate\"]";
                strColumnModel = "[";
                strColumnModel = strColumnModel + "{\"name\": \"row\", \"index\": \"row\", \"width\": 10, \"sorttype\": \"int\", \"align\": \"center\", \"sortable\": false, \"search\": false},";
                strColumnModel = strColumnModel + "{ \"name\": \"Id\", \"index\": \"Id\", \"width\": 0, \"search\": false, \"hidden\": true, \"key\": true },";
                strColumnModel = strColumnModel + "{ \"name\": \"RefNo\", \"index\": \"RefNo\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"BrokerName\", \"index\": \"BrokerName\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"FromDate\", \"index\": \"FromDate\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } },";
                strColumnModel = strColumnModel + "{ \"name\": \"ToDate\", \"index\": \"ToDate\", \"width\": 100, \"sorttype\": \"text\", \"sortable\": true, \"search\": true, \"searchrules\": { \"required\": true } }";

                strColumnModel = strColumnModel + "]";
                break;
            default:
                break;
        }


        return "{\"colName\":" + strColumnName + ",\"colModel\":" + strColumnModel + "}";
    }

    private void CalculateYield(ref DataTable dt)
    {
        string strFlag = "";
        int SecurityId = 0;
        Int16 FrequencyOfInterest = 0;
        double Rate = 0;
        DataSet ds = new DataSet();
        clsCommonFuns objClsComm = new clsCommonFuns();
        List<SqlParameter> lstParam = new List<SqlParameter>();

        try
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strFlag = Convert.ToString(dt.Rows[i]["TypeFlag"]);
                SecurityId = Convert.ToInt32(dt.Rows[i]["SecurityId"]);
                Rate = (string.IsNullOrEmpty(dt.Rows[i]["AveragePrice"].ToString()) != true) ? Convert.ToDouble(dt.Rows[i]["AveragePrice"]) : 0;
                FrequencyOfInterest = Convert.ToInt16(dt.Rows[i]["FrequencyOfInterest"]);
                if (Rate > 0)
                {
                    if (strFlag == "G")
                    {
                        int intMaturity = 0, intCoupn = 0, intCall = 0;
                        DateTime YTMDate = System.DateTime.Today;
                        DateTime MaturityDate = DateTime.MinValue, CouponDate = DateTime.MinValue, CallDate = DateTime.MinValue, PutDate = DateTime.MinValue;
                        double FaceValue = 0, MaturityAmt = 0, CouponRate = 0, CallAmt = 0, PutAmt = 0;

                        lstParam.Clear();
                        lstParam.Add(new SqlParameter("@SecurityId", SecurityId));
                        ds = objComm.FillDetails(lstParam, "CRM_FILL_SecurityInfoDetails");
                        if (ds.Tables.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                FaceValue = Double.Parse(row["FaceValue"].ToString());
                                if (row["TypeFlag"].ToString() == "I")
                                {
                                    CouponDate = DateTime.Parse(row["Date"].ToString());
                                    CouponRate = Double.Parse(row["Amount"].ToString());
                                    intCoupn += 1;
                                }
                                else if (row["TypeFlag"].ToString() == "M")
                                {
                                    MaturityDate = DateTime.Parse(row["Date"].ToString());
                                    MaturityAmt = Double.Parse(row["Amount"].ToString());
                                    intMaturity += 1;
                                }
                                else if (row["TypeFlag"].ToString() == "C" && intCall == 0 && DateTime.Parse(row["Date"].ToString()) > YTMDate)
                                {
                                    CallDate = DateTime.Parse(row["Date"].ToString());
                                    CallAmt = Double.Parse(row["Amount"].ToString());
                                    intCall += 1;
                                }
                            }
                        }
                        if (intCoupn == 1 && (intMaturity == 1 || (intMaturity == 0 && intCall == 1)))
                        {
                            GlobalFuns.CalculateYield(YTMDate, FaceValue, Rate, false, true, MaturityDate, MaturityAmt, CouponDate, CouponRate, CallDate, CallAmt, PutDate, PutAmt, FrequencyOfInterest, "Y", 0, "Y");
                        }
                        else if (intCoupn == 0 || intCoupn > 1 || intMaturity > 1)
                        {
                            GlobalFuns.CalculateXIRRYield(SecurityId, System.DateTime.Now.ToString("dd/MM/yyyy"), Rate, FrequencyOfInterest, false, "R", "N", "M");
                        }
                    }
                    else
                    {
                        GlobalFuns.CalculateXIRRYield(SecurityId, System.DateTime.Now.ToString("dd/MM/yyyy"), Rate, FrequencyOfInterest, false, "R", "N", "M");
                    }
                    dt.Rows[i]["YTMAnn"] = objClsComm.DecimalFormat(GlobalFuns.dblYTMAnn);
                    dt.Rows[i]["YTMSemi"] = objClsComm.DecimalFormat(GlobalFuns.dblYTMSemi);
                    dt.Rows[i]["YTCAnn"] = objClsComm.DecimalFormat(GlobalFuns.dblYTCAnn);
                    dt.Rows[i]["YTCSemi"] = objClsComm.DecimalFormat(GlobalFuns.dblYTCSemi);
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}