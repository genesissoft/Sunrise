using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Collections.Generic;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Globalization;

public partial class Forms_ScriptEvaluationReport : System.Web.UI.Page
{
    ClsCommon objComm = new ClsCommon();
    clsCommonFuns objclscomm = new clsCommonFuns();
    SqlConnection sqlconn = new SqlConnection();
    clsCommonFuns objCommon = new clsCommonFuns();
    SqlCommand sqlComm = new SqlCommand();
    DataTable dtRptSale = new DataTable();
    DataTable dtRptPurchase = new DataTable();
    DataTable dtRptOpenPosition = new DataTable();

    MISReports objMISRpt = new MISReports(); 

    protected void Page_PreInit(object sender, EventArgs e)
    {
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D);
        Response.Expires = -1500;
        Response.CacheControl = "no-cache";
        Response.AppendHeader("Cache-Control", "no-store");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {           
            if (!Page.IsPostBack)
            { 
                txt_FromDate.Text =DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") ;
                txt_ToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");         
            }
        }
        catch (Exception ex)
        {
            throw; 
        }
    }

    private void OpenConn()
    {
        if ((sqlconn == null))
        {
            sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
            sqlconn.Open();
        }
        else if ((sqlconn.State == ConnectionState.Closed))
        {
            sqlconn.ConnectionString = ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString();
            sqlconn.Open();
        }
    }
    private void CloseConn()
    {
        if ((sqlconn == null))
        {
            return;
        }
        if ((sqlconn.State == ConnectionState.Open))
        {
            sqlconn.Close();
        }

    }
    private DataTable ViewReport(string strProcName)
    {
        try {
            string strcon = "";
            SqlDataAdapter sqlda = new SqlDataAdapter();
            DataTable sqldt = new DataTable();
            DataView sqldv = new DataView();
            DateTime dateFor;
            DateTime dateTo;
            dateFor = objCommon.DateFormat(txt_FromDate.Text);
            dateTo = objCommon.DateFormat(txt_ToDate.Text);
            OpenConn();
            sqlComm.Connection = sqlconn;
            sqlComm.CommandTimeout = 1000;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = strProcName;
            sqlComm.Parameters.Clear();
            if ((strProcName == "ID_RPT_MISPurchaseReport") || strProcName == "ID_RPT_MISSaleReport") 
            {   
                sqlComm.Parameters.Add("@ForDate", SqlDbType.SmallDateTime, 8).Value = dateFor;
                sqlComm.Parameters.Add("@ToDate", SqlDbType.SmallDateTime, 8).Value = dateTo;
            }
            else {
                sqlComm.Parameters.Add("@ForDate", SqlDbType.SmallDateTime, 8).Value = dateTo;
            }
            sqlComm.Parameters.Add("@compid", SqlDbType.Int, 4).Value = Session["CompId"];
            sqlComm.Parameters.Add("@YearId", SqlDbType.Int, 4).Value = Session["YearId"];
            sqlda.SelectCommand = sqlComm;
            sqlda.Fill(sqldt);
            return sqldt;
        }
        catch (Exception ex) 
        {
            throw ex;
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", ("alert(\'" + (ex.Message.Replace("\'", " ").Replace('\r', " ").Replace('\n', " ") + "\');")), true);
        }
        //finally {
        //    CloseConn();
        //}
        
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            dtRptSale = ViewReport("ID_RPT_MISSaleReport");
            Session["MISSaleReport"] = dtRptSale;
            dtRptPurchase = ViewReport("ID_RPT_MISPurchaseReport");
            Session["MISPurchaseReport"] = dtRptPurchase;
            dtRptOpenPosition = ViewReport("ID_RPT_MISOpenPositionReport");
            Session["MISOpenPositionReport"] = dtRptOpenPosition;
            if (dtRptSale.Rows.Count == 0 && dtRptPurchase.Rows.Count == 0 && dtRptOpenPosition.Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('No Data found...');", true);
            }
            else
            {
                ExportToExcel_MISClosedPositionReport(dtRptOpenPosition, txt_FromDate.Text.ToString(), Session["CompName"].ToString());       
            }
            
        }
        catch(Exception ex)
        {
            throw ex; 
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('" + ex.Message.Replace("'", " ").Replace(Strings.Chr(13), " ").Replace(Strings.Chr(10), " ") + "');", true);
            
        }
        
    }
    public void ExportToExcel_MISClosedPositionReport(DataTable dt, string FromDate, String CompName)
    {
        try
        {
            XLWorkbook wb = new XLWorkbook();
            string sheetName = "";
            for (int Y = 0; Y < 6; Y++)
            {   
                //*********************************************************************
                //*****************************Sale Report*****************************
                //*********************************************************************
                if (Y == 1)
                {
                    dt = (DataTable)HttpContext.Current.Session["MISSaleReport"];
                    if (dt == null)
                    {
                        sheetName = "Sale Report";
                        IXLWorksheet ws = wb.Worksheets.Add(sheetName);
                    }
                    else
                    {
                        try
                        {
                            if (dt != null)
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    dt.Columns.RemoveAt(0); //Remove the Serial No Column****
                                    dt.AcceptChanges();
                                    sheetName = "Sale Report";
                                    IXLWorksheet ws = wb.Worksheets.Add(dt,sheetName);
                                    ws.Table(0).ShowAutoFilter = false;
                                  
                                }
                                else
                                {
                                    //ClientScript.RegisterStartupScript(this.GetType(), "Key", "alert('No Data Found.');", true);
                                }
                            }
                            else
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "Key", "alert('No Data Found.');", true);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                //***************************************************************************
                //***************************************************************************
                //***************************************************************************
                //*************************Op Position Interest******************************
                //***************************************************************************
                //***************************************************************************
                //***************************************************************************0
                else if (Y == 2)
                {
                    dt = (DataTable)HttpContext.Current.Session["MISOpenPositionReport"];
                    if (dt == null)
                    {
                        sheetName = "Op Position Interest";
                        IXLWorksheet ws = wb.Worksheets.Add(dt, sheetName);
                        ws.Table(0).ShowAutoFilter = false;
                    }
                    else
                    {
                        try
                        {
                            if (dt != null)
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    sheetName = "Op Position Interest";
                                    IXLWorksheet ws = wb.Worksheets.Add(dt,sheetName);
                                    ws.Table(0).ShowAutoFilter = false; 
                                    
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }


                    }
                }
                //***************************************************************************
                //***************************************************************************
                //***************************************************************************
                //*************************Purchase Reports**********************************
                //***************************************************************************
                //***************************************************************************
                //***************************************************************************
                else if (Y == 0)
                {
                    dt = (DataTable)HttpContext.Current.Session["MISPurchaseReport"];
                    if (dt == null)
                    {
                        sheetName = "Purchase Report";
                        IXLWorksheet ws = wb.Worksheets.Add(sheetName);
                        ws.Table(0).ShowAutoFilter = false;
                    }
                    else
                    {
                        try
                        {
                            if (dt != null)
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    //dt.Columns.RemoveAt(0); //****Serial No Column Remove in DataTable
                                    //dt.AcceptChanges(); 
                                    sheetName = "Purchase Report";
                                    IXLWorksheet ws = wb.Worksheets.Add(dt,sheetName);
                                    ws.Table(0).ShowAutoFilter = false;
                                 
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            // Clear All the Session Here.....            
            HttpContext.Current.Session["MISSaleReport"] = null;
            HttpContext.Current.Session["MISPurchaseReport"] = null;
            HttpContext.Current.Session["MISOpenPositionReport"] = null;

            //Code to save the file
            string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
            strdtTime = strdtTime.Replace("PM", "");
            strdtTime = strdtTime.Replace("AM", "");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=MISStockReport" + strdtTime + ".xlsx");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }
        catch (Exception ex)
        {
            //ScriptManager.RegisterStartupScript("", this.GetType(), "test", "alert('A');", true);
        }

        // HttpContext.Current.Response.Redirect("~/Forms/Login.aspx", false);
    }
    public string ReturnIndianFormat(Double Amount)
    {
        string ReturnString = "";
        if (Amount < 100000)
        {
            ReturnString = @"##,##0.00";
        }
        else if (Amount < 10000000)
        {
            ReturnString = @"#\,##\,##0.00";
        }
        else if (Amount < 1000000000)
        {
            ReturnString = @"#\,##\,##\,##0.00";
        }
        else if (Amount < 1000000000)
        {
            ReturnString = @"#\,##\,##\,##0.00";
        }
        else if (Amount < 100000000000)
        {
            ReturnString = @"#\,##\,##\,##\,##0.00";
        }
        else
        {
            ReturnString = @"#\,##\,##\,##\,##\,##0.00";
        }
        return ReturnString;

    }
}
