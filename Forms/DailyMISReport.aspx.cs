using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using System.Linq;
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

using DocumentFormat.OpenXml.Packaging;

public partial class DailyMISReport : System.Web.UI.Page
{
    ClsCommon objComm = new ClsCommon();
    clsCommonFuns objclscomm = new clsCommonFuns();
    SqlConnection sqlconn = new SqlConnection();

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
                SetAttribute();
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void SetAttribute()
    {
        txt_FromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_FromDate.Attributes.Add("onkeypress", "OnlyDate();");
        txt_FromDate.Attributes.Add("onblur", "CheckDate(this,false);");
        txt_ToDate.Attributes.Add("onkeypress", "OnlyDate();");
        txt_ToDate.Attributes.Add("onblur", "CheckDate(this,false);");
        txt_ToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        DataSet ds = new DataSet();
        List<SqlParameter> lstParam = new List<SqlParameter>();
        ds = objComm.FillDetails(lstParam, "ID_FILL_CompanyMasterRpt");
        cbo_Company.DataSource = ds.Tables[0];
        cbo_Company.DataTextField = "CompName";
        cbo_Company.DataValueField = "CompId";

        cbo_Company.DataBind();
        //cbo_Company.Items.Insert(0, new ListItem("All Record"));
        //cbo_Company.Items.Add(new ListItem("Select", "0", true));

    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                string FromDate = txt_FromDate.Text;
                string ToDate = txt_ToDate.Text;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                List<SqlParameter> lstParam = new List<SqlParameter>();
                GetTransactionDetails("");
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void GetTransactionDetails(string strReportType)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string dealtranstype = "";
        List<SqlParameter> lstParam = new List<SqlParameter>();
        try
        {
            string dealTransTypeValue = ddlDealTransType.SelectedItem.Value;
            string ReportType = ddlDealTrasReportType.SelectedItem.Text;

            if (dealTransTypeValue == "0")
            {
                dealtranstype = "";
            }
            else if (dealTransTypeValue == "1")
            {
                dealtranstype = "T";
            }
            else
            {
                dealtranstype = "B";
            }
            switch (ReportType)
            {

                case "Deal Acknowledgement":
                    lstParam.Add(new SqlParameter("@DealTransType", dealtranstype));
                    lstParam.Add(new SqlParameter("@CompId", cbo_Company.SelectedItem.Value));
                    //lstParam.Add(new SqlParameter("@YearId", Convert.ToInt32(Session["YearId"])));
                    lstParam.Add(new SqlParameter("@FromDate", txt_FromDate.Text));
                    lstParam.Add(new SqlParameter("@ToDate", txt_ToDate.Text));
                    ds = objComm.FillDetails(lstParam, "ID_RPT_DealsAckReport");
                    dt = ds.Tables[0];
                    ExportToExcel_DealsAckReport(dt);
                    break;
                case "Daily Transaction Report":
                    lstParam.Add(new SqlParameter("@DealTransType", dealtranstype));
                    lstParam.Add(new SqlParameter("@CompId", cbo_Company.SelectedItem.Value));
                    lstParam.Add(new SqlParameter("@YearId", Convert.ToInt32(Session["YearId"])));
                    lstParam.Add(new SqlParameter("@FromDate", txt_FromDate.Text));
                    lstParam.Add(new SqlParameter("@ToDate", txt_ToDate.Text));
                    ds = objComm.FillDetails(lstParam, "ID_FILL_MISProfitRpt");
                    dt = ds.Tables[0];
                    ExportToExcel_MISProfitRpt(dt);
                    break;
                    default:
                    break;
            }

        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Key", "alert('" + ex.ToString() + "');", true);
        }
        finally
        {
        }
    }

   
    public void ExportToExcel_DealsAckReport(DataTable dt)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws = wb.Worksheets.Add("sheet1");
                    
                    //dynamic column formation
                    string[] cols = new string[dt.Columns.Count];
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    //   ws.AutoFilter.Clear();
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        if (i == cols.Length)
                        {
                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                            ws.Cell(DataCell).Value = cols[i - 1];
                            ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                            ws.Cell(DataCell).Style.Font.Bold = true;
                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightBlue;
                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.Black;
                        }
                        else
                        {
                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                            ws.Cell(DataCell).Value = cols[i - 1];
                            ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                            ws.Cell(DataCell).Style.Font.Bold = true;
                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightBlue;
                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.Black;
                            StartCharCols++;
                        }
                    }
                    char StartCharData = 'A';
                    int StartIndexData = 3;

                    char StartCharDataCol = char.MinValue;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {

                            string DataCell = StartCharData.ToString() + StartIndexData;
                            string a = dt.Rows[i][j].ToString();
                            a = a.Replace("&nbsp;", " ");
                            a = a.Replace("&amp;", "&");
                           
                            int val = 0;
                            double val2 = 0.0;
                            DateTime dt2 = DateTime.Now;
                            if (int.TryParse(a, out val))
                            {
                                //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                ws.Cell(DataCell).Value = val;
                                ws.Cell(DataCell).SetValue(a);
                            }
                            if (double.TryParse(a, out val2))
                            {
                                ws.Cell(DataCell).Value = val2;
                            }
                            //check if datetime type
                            else if (DateTime.TryParse(a, out dt2))
                            {
                                ws.Cell(DataCell).Value = dt2.ToShortDateString();
                                ws.Cell(DataCell).SetValue(a);
                            }
                            else
                            {
                                ws.Cell(DataCell).SetValue(a);
                            }
                            StartCharData++;
                        }
                        StartCharData = 'A';
                        StartIndexData++;
                    }

                    string Range = "";
                    Range = "A" + StartIndexData + ":" + "G" + StartIndexData;
                    // ws.Range(Range).Merge();

                    ws.Cell("C" + 1).SetValue("From " + txt_FromDate.Text + " To " + txt_ToDate.Text);
                    ws.Cell("D" + 1).SetValue("Report Date :  " + System.DateTime.Now.ToShortDateString());
                    ws.Cell("A" + 1).SetValue("Deal Acknowledgement Report");
                    Range = "A1:G1";
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.LightGreen;
                    ws.Cell("C" + StartIndexData).WorksheetColumn().Width = 35;
                    ws.Cell("D" + StartIndexData).WorksheetColumn().Width = 35;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=DealAckReport.xlsx");
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Key", "alert('No Data Found.');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Key", "alert('No Data Found.');", true);
            }
        }
        catch (Exception ex)
        {

        }
    }


    public void ExportToExcel_MISProfitRpt(DataTable dt)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws = wb.Worksheets.Add("sheet1");

                    //dynamic column formation
                    string[] cols = new string[dt.Columns.Count];
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    //   ws.AutoFilter.Clear();
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        if (i == cols.Length)
                        {
                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                            ws.Cell(DataCell).Value = cols[i - 1];
                            ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                            ws.Cell(DataCell).Style.Font.Bold = true;
                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightBlue;
                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.Black;
                        }
                        else
                        {
                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                            ws.Cell(DataCell).Value = cols[i - 1];
                            ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                            ws.Cell(DataCell).Style.Font.Bold = true;
                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightBlue;
                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.Black;
                            StartCharCols++;
                        }
                    }
                    char StartCharData = 'A';
                    int StartIndexData = 3;

                    char StartCharDataCol = char.MinValue;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {

                            string DataCell = StartCharData.ToString() + StartIndexData;
                            string a = dt.Rows[i][j].ToString();
                            a = a.Replace("&nbsp;", " ");
                            a = a.Replace("&amp;", "&");

                            int val = 0;
                            double val2 = 0.0;
                            DateTime dt2 = DateTime.Now;
                            if (int.TryParse(a, out val))
                            {
                                //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                ws.Cell(DataCell).Value = val;
                                ws.Cell(DataCell).SetValue(a);
                            }
                            if (double.TryParse(a, out val2))
                            {
                                ws.Cell(DataCell).Value = val2;
                            }
                            //check if datetime type
                            else if (DateTime.TryParse(a, out dt2))
                            {
                                ws.Cell(DataCell).Value = dt2.ToShortDateString();
                                ws.Cell(DataCell).SetValue(a);
                            }
                            else
                            {
                                ws.Cell(DataCell).SetValue(a);
                            }
                            StartCharData++;
                        }
                        StartCharData = 'A';
                        StartIndexData++;
                    }

                    string Range = "";
                    Range = "A" + StartIndexData + ":" + "G" + StartIndexData;
                    // ws.Range(Range).Merge();

                    ws.Cell("C" + 1).SetValue("From " + txt_FromDate.Text + " To " + txt_ToDate.Text);
                    ws.Cell("D" + 1).SetValue("Report Date :  " + System.DateTime.Now.ToShortDateString());
                    ws.Cell("A" + 1).SetValue("MIS Profit Report");
                    Range = "A1:G1";
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.LightGreen;
                    ws.Cell("C" + StartIndexData).WorksheetColumn().Width = 35;
                    ws.Cell("D" + StartIndexData).WorksheetColumn().Width = 35;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=MISProfit.xlsx");
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Key", "alert('No Data Found.');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Key", "alert('No Data Found.');", true);
            }
        }
        catch (Exception ex)
        {

        }
    }


    private string GetValue(string strValue)
    {
        string[] arrValues = strValue.Split(',');

        return arrValues.Length > 0 ? arrValues[0] : "";
    }
}
