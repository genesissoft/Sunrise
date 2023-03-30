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
//using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Collections.Generic;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Globalization;

/// <summary>
/// Summary description for MISReports
/// </summary>
public class MISReports
{

    //
    // TODO: Add constructor logic here
    //
    clsCommonFuns objCommon = new clsCommonFuns();
    public void ExportToExcel_SecurityEvaluation(DataTable dt, string ForDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws = wb.Worksheets.Add("sheet1");
                    // ws.Cell("A1").Value = "fdsgfgfdgf";
                    //add columns
                    string Range = "";
                    ws.Column("A:S").AdjustToContents();
                    Range = "A1:S1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Range(Range).Style.Font.FontName = "Verdana";


                    ws.Range(Range).Style.Font.FontColor = XLColor.White;

                    Range = "A2:X2";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 10;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Range(Range).Style.Font.FontName = "Verdana";
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(212, 239, 223); ;
                    ws.Range(Range).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(Range).Value = "Valuation Report As On - " + ForDate;
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;

                    string[] cols = new string[dt.Columns.Count];
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }
                    char StartCharCols = 'A';
                    int StartIndexCols = 4;

                    //   ws.AutoFilter.Clear();
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        if (i == cols.Length)
                        {
                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                            ws.Cell(DataCell).Value = cols[i - 1];
                            ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                            ws.Cell(DataCell).Style.Font.Bold = true;
                            ws.Cell(DataCell).Style.Font.FontSize = 9;
                            ws.Cell(DataCell).Style.Font.FontName = "Verdana";
                            ws.Cell(DataCell).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
                            ws.Cell(DataCell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                        }
                        else
                        {
                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                            ws.Cell(DataCell).Value = cols[i - 1];
                            ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                            ws.Cell(DataCell).Style.Font.Bold = true;
                            ws.Cell(DataCell).Style.Font.FontSize = 9;
                            ws.Cell(DataCell).Style.Font.FontName = "Verdana";
                            ws.Cell(DataCell).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            ws.Cell(DataCell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            StartCharCols++;
                        }
                    }
                    //char StartCharData = 'A';
                    int StartIndexData = 4;

                    DataView view = new DataView(dt);
                    DataTable distinctValues = new DataTable();
                    distinctValues = view.ToTable(true, "SecurityTypeName");
                    double GrandMarketValue = 0.0;
                    double GrandFaceValue = 0.0;
                    double GrandBookValue = 0.0;
                    double GrandInterestValue = 0.0;


                    foreach (DataRow row in distinctValues.Rows)
                    {
                        char StartCharData = 'A';
                        //Range = StartCharData.ToString() + StartIndexData.ToString();
                        //ws.Range(Range).Merge();
                        //ws.Range(Range).Style.Font.FontSize = 11;
                        //ws.Range(Range).Style.Font.Bold = true;
                        //ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        //ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        //ws.Range(Range).Style.Fill.BackgroundColor = XLColor.DeepSkyBlue;
                        //ws.Range(Range).Style.Font.FontColor = XLColor.White;
                        //ws.Range(Range).SetValue(Convert.ToString(row["TradeDate"]));

                        DataTable dtTarget = new DataTable();
                        dtTarget = dt.Clone();
                        DataRow[] rowsToCopy;
                        string filterdate2 = Convert.ToString(row["SecurityTypeName"]);
                        rowsToCopy = dt.Select("[SecurityTypeName]='" + filterdate2 + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }
                        StartIndexData++;
                        DataTable dtP = new DataTable();
                        dtP = dtTarget;

                        double TotalFaceValue = 0.0;
                        double TotalBookValue = 0.0;
                        double TotalMarketValue = 0.0;
                        double TotalInterestValue = 0.0;
                        //double TotalStampDuty = 0.0;
                        //double TotalStampDutyRebate = 0.0;
                        //double TotalSGST = 0.0;
                        //double TotalCGST = 0.0;
                        //double TotalIGST = 0.0;

                        char StartCharDataCol = char.MinValue;
                        for (int i = 0; i < dtP.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtP.Columns.Count; j++)
                            {

                                string DataCell = StartCharData.ToString() + StartIndexData;
                                string a = dtP.Rows[i][j].ToString();
                                a = a.Replace("&nbsp;", " ");
                                a = a.Replace("&amp;", "&");

                                if (StartCharData.ToString() == "H")
                                {

                                    TotalFaceValue += Convert.ToDouble(a);

                                }
                                if (StartCharData.ToString() == "J")
                                {
                                    TotalBookValue += Convert.ToDouble(a);
                                }
                                if (StartCharData.ToString() == "L")
                                {
                                    TotalMarketValue += Convert.ToDouble(a);
                                }

                                if (StartCharData.ToString() == "W")
                                {
                                    TotalInterestValue += Convert.ToDouble(a);
                                }
                                //if (StartCharData.ToString() == "O")
                                //{
                                //    TotalSGST += Convert.ToDouble(a);
                                //}
                                //if (StartCharData.ToString() == "P")
                                //{
                                //    TotalCGST += Convert.ToDouble(a);
                                //}
                                //if (StartCharData.ToString() == "Q")
                                //{
                                //    TotalIGST += Convert.ToDouble(a);
                                //}
                                //if (StartCharData.ToString() == "R")
                                //{
                                //    TotalStampDutyRebate += Convert.ToDouble(a);
                                //}
                                //if (StartCharData.ToString() == "S")
                                //{
                                //    TotalBrokerage += Convert.ToDouble(a);
                                //}

                                //check if value is of integer type
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
                                    ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.00";
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
                                ws.Cell(DataCell).Style.Font.FontName = "Verdana";
                                ws.Cell(DataCell).Style.Font.FontSize = 9;
                                ws.Cell(DataCell).WorksheetColumn().AdjustToContents();
                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                //ws.Cell(DataCell).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                StartCharData++;
                            }
                            StartCharData = 'A';
                            StartIndexData++;
                        }
                        for (int j = 0; j < dtP.Columns.Count; j++)
                        {
                            Range = StartCharData + StartIndexData.ToString();
                            ws.Range(Range).Style.Font.FontSize = 10;
                            ws.Range(Range).Style.Font.Bold = true;
                            ws.Range(Range).Style.Font.FontName = "Verdana";
                            ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                            ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(230, 238, 215);
                            ws.Range(Range).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            StartCharData++;
                        }
                        ws.Cell("A" + StartIndexData).SetValue(filterdate2);
                        Range = "A" + StartIndexData + ":" + "G" + StartIndexData;
                        ws.Range(Range).Merge();
                        ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        GrandFaceValue += TotalFaceValue;
                        GrandBookValue += TotalBookValue;
                        GrandMarketValue += TotalMarketValue;
                        GrandInterestValue += TotalInterestValue;


                        ws.Cell("H" + StartIndexData).Value = TotalFaceValue;
                        ws.Cell("H" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                        Range = "H" + StartIndexData + ":" + "H" + StartIndexData;
                        ws.Range(Range).Merge();

                        ws.Cell("J" + StartIndexData).Value = TotalBookValue;
                        ws.Cell("J" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                        Range = "J" + StartIndexData + ":" + "J" + StartIndexData;
                        ws.Range(Range).Merge();

                        ws.Cell("L" + StartIndexData).Value = TotalMarketValue;
                        ws.Cell("L" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                        ws.Cell("W" + StartIndexData).Value = TotalInterestValue;
                        ws.Cell("W" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                        //ws.Cell("O" + StartIndexData).Value = TotalSGST;
                        //ws.Cell("O" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                        //ws.Cell("P" + StartIndexData).Value = TotalCGST;
                        //ws.Cell("P" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                        //ws.Cell("Q" + StartIndexData).Value = TotalIGST;
                        //ws.Cell("Q" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                        //ws.Cell("R" + StartIndexData).Value = TotalStampDutyRebate;
                        //ws.Cell("R" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                        //ws.Cell("S" + StartIndexData).Value = TotalBrokerage;
                        //ws.Cell("S" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                    }
                    StartIndexData++;
                    char StartCharData1 = 'A';
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Range = StartCharData1 + StartIndexData.ToString();
                        ws.Range(Range).Style.Font.FontSize = 10;
                        ws.Range(Range).Style.Font.Bold = true;
                        ws.Range(Range).Style.Font.FontName = "Verdana";
                        ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(214, 224, 236);
                        ws.Range(Range).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        StartCharData1++;
                    }

                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    Range = "A" + StartIndexData + ":" + "G" + StartIndexData;
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    ws.Cell("H" + StartIndexData).Value = GrandFaceValue;
                    ws.Cell("H" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                    ws.Cell("H" + StartIndexData).WorksheetColumn().Width = 25;
                    Range = "H" + StartIndexData + ":" + "H" + StartIndexData;
                    ws.Range(Range).Merge();

                    ws.Cell("J" + StartIndexData).Value = GrandBookValue;
                    ws.Cell("J" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                    ws.Cell("J" + StartIndexData).WorksheetColumn().Width = 25;
                    Range = "J" + StartIndexData + ":" + "J" + StartIndexData;
                    ws.Range(Range).Merge();


                    ws.Cell("L" + StartIndexData).Value = GrandMarketValue;
                    ws.Cell("L" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                    ws.Cell("W" + StartIndexData).Value = GrandInterestValue;
                    ws.Cell("W" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                    //ws.Cell("O" + StartIndexData).Value = GrandSGST;
                    //ws.Cell("O" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                    //ws.Cell("P" + StartIndexData).Value = GrandCGST;
                    //ws.Cell("P" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                    //ws.Cell("Q" + StartIndexData).Value = GrandIGST;
                    //ws.Cell("Q" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                    //ws.Cell("R" + StartIndexData).Value = GrandStampDutyRebate;
                    //ws.Cell("R" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                    //ws.Cell("S" + StartIndexData).Value = GrandBrokerage;
                    //ws.Cell("S" + StartIndexData).Style.NumberFormat.Format = "#,##0.00";
                    //Code to save the file
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ScriptValuation" + strdtTime + ".xlsx");
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



    public void ExportToExcel_IPDateReport(DataTable dt, string FromDate, String ToDate, string RptType)
    {
        XLWorkbook wb = new XLWorkbook();
        IXLWorksheet ws;
        string repttype;
        //Add new sheet
        if (RptType == "B")
        {

            repttype = "Back Dated ";
        }
        else
        {

            repttype = "Post Dated ";
        }
        ws = wb.Worksheets.Add(dt, repttype);
        ws.Table(0).ShowAutoFilter = false;
        int dtCount = 0;
        double Totfacevalue = 0.0;
        double Totaccrdintrst = 0.0;
        dtCount = dt.Rows.Count + 2;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Totfacevalue += Convert.ToDouble(dt.Rows[i]["Facevalue"]);
            Totaccrdintrst += Convert.ToDouble(dt.Rows[i]["AccruedInterest"]);
        }

        ws.Cell("A" + dtCount).SetValue("Grand Total");
        ws.Cell("F" + dtCount).SetValue(Totfacevalue);
        ws.Cell("G" + dtCount).SetValue(Totaccrdintrst);

        ws.Cell("A" + dtCount).Style.Font.Bold = true;
        ws.Cell("F" + dtCount).Style.Font.Bold = true;
        ws.Cell("G" + dtCount).Style.Font.Bold = true;

        string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
        strdtTime = strdtTime.Replace("PM", "");
        strdtTime = strdtTime.Replace("AM", "");
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=IPDateReport" + strdtTime + ".xlsx");

        using (MemoryStream MyMemoryStream = new MemoryStream())
        {
            wb.SaveAs(MyMemoryStream);
            MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
    public void ExportToExcel_MISDealDateReport(DataTable dt, string FromDate, String ToDate, int CompId, int intcost)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("MIS");
                    //Add new sheet



                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 3;
                    //   ws.AutoFilter.Clear();
                    char dblStartCharCols = 'A';
                    // filling header with formatting and colors
                    int count = 0;
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        if (i == cols.Length)
                        {
                            if (count > 0)
                            {
                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            }
                            else
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            }
                        }
                        else
                        {
                            if (StartCharCols.ToString() == "[" || count > 0)
                            {
                                count = 1;
                                if (i % 27 == 0)
                                {
                                    StartCharCols = 'A';
                                    if (i % 54 == 0)
                                    {
                                        dblStartCharCols++;
                                    }
                                }
                                //dblStartCharCols++;
                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }
                            else
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }
                        }
                    }

                    int Ccout = 0;
                    char StartCharData = 'A';
                    int StartIndexData = 4;
                    dblStartCharCols = 'A';


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (StartCharData.ToString() == "[" || Ccout > 0)
                            {
                                Ccout = 1;
                                if (j % 26 == 0)
                                {
                                    StartCharData = 'A';
                                    if (j % 52 == 0)
                                    {
                                        dblStartCharCols++;
                                    }
                                }

                                string DataCell = dblStartCharCols.ToString() + StartCharData.ToString() + StartIndexData;
                                string a = dt.Rows[i][j].ToString();
                                a = a.Replace("&nbsp;", " ");
                                a = a.Replace("&amp;", "&");
                                //check if value is of integer type
                                int val = 0;
                                double val2 = 0.0;
                                DateTime dt2 = DateTime.Now;
                                if (int.TryParse(a, out val))
                                {
                                    //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                    ws.Cell(DataCell).Value = val;
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
                            else
                            {
                                string DataCell = StartCharData.ToString() + StartIndexData;
                                string a = dt.Rows[i][j].ToString();
                                a = a.Replace("&nbsp;", " ");
                                a = a.Replace("&amp;", "&");
                                //check if value is of integer type
                                int val = 0;
                                double val2 = 0.0;
                                DateTime dt2 = DateTime.Now;
                                if (int.TryParse(a, out val))
                                {
                                    //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                    ws.Cell(DataCell).Value = val;
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
                        }
                        StartCharData = 'A';
                        StartIndexData++;
                        Ccout = 0;
                    }
                    string Range = "";
                    Range = "A1:E1";
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("C" + 1).SetValue("From " + FromDate + " To " + ToDate);
                    ws.Cell("D" + 1).SetValue("Report Date :  " + System.DateTime.Now.ToShortDateString());
                    ws.Cell("A" + 1).SetValue("MIS Transaction Report");
                    //ws.Cell("E" + StartIndexData).SetValue(facevalue);
                    //ws.Cell("G" + StartIndexData).SetValue(finalAmount);
                    //ws.Cell("E" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //ws.Cell("G" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //ws.Cell("A" + StartIndexData).SetValue("Total");
                    //ws.Cell("C" + StartIndexData).WorksheetColumn().Width = 35;
                    //ws.Cell("D" + StartIndexData).WorksheetColumn().Width = 35;
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=MIS_" + strdtTime + ".xlsx");
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



    public void ExportToExcel_MISClosedPositionReportNew(DataTable dt, string FromDate, String CompName)
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
                    dt = (DataTable)HttpContext.Current.Session["ClosedPosition"];
                    if (dt == null)
                    {
                        sheetName = "Closed Position";
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
                                    sheetName = "Closed Position";
                                    IXLWorksheet ws = wb.Worksheets.Add(sheetName);
                                    string[] cols = new string[dt.Columns.Count];
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        string a = dt.Columns[c].ToString();
                                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                                    }
                                    string StartCharCols = "A";
                                    int StartIndexCols = 1;
                                    for (int i = 1; i <= cols.Length; i++)
                                    {
                                        if (i == cols.Length)
                                        {
                                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                            ws.Cell(DataCell).Value = cols[i - 1];
                                            ws.Cell(DataCell).Style.Font.Bold = true;
                                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                            ws.Cell(DataCell).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                                            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                        }
                                        else
                                        {
                                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                            ws.Cell(DataCell).Value = cols[i - 1];
                                            ws.Cell(DataCell).Style.Font.Bold = true;
                                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                            ws.Cell(DataCell).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                                            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                            StartCharCols = ColumnIndexToColumnLetter(i + 1);
                                        }
                                    }
                                    string StartCharData = "A";
                                    int StartIndexData = 2;
                                    char StartCharDataCol = char.MinValue;
                                    for (int p = 0; p < dt.Rows.Count; p++)
                                    {
                                        for (int j = 0; j < dt.Columns.Count; j++)
                                        {
                                            string DataCell = StartCharData.ToString() + StartIndexData;
                                            string a = dt.Rows[p][j].ToString();
                                            a = a.Replace("&nbsp;", " ");
                                            a = a.Replace("&amp;", "&");
                                            int val = 0;
                                            double val2 = 0.0;
                                            DateTime dt2 = DateTime.Now;
                                            if (int.TryParse(a, out val))
                                            {
                                                ws.Cell(DataCell).Value = val;
                                                ws.Cell(DataCell).SetValue(a);
                                                ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val);
                                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                            }
                                            if (double.TryParse(a, out val2))
                                            {
                                                ws.Cell(DataCell).Value = val2;
                                                ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val2);
                                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                            }
                                            else if (DateTime.TryParse(a, out dt2))
                                            {
                                                ws.Cell(DataCell).Value = dt2.ToShortDateString();
                                                ws.Cell(DataCell).SetValue(a);
                                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                            }
                                            else
                                            {
                                                ws.Cell(DataCell).SetValue(a);
                                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                            }
                                            StartCharData = ColumnIndexToColumnLetter(j + 2);
                                            ws.Columns().AdjustToContents();
                                        }
                                        StartCharData = "A";
                                        StartIndexData++;

                                    }



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
                else if (Y == 0)
                {
                    dt = (DataTable)HttpContext.Current.Session["OpenPosition"];
                    if (dt == null)
                    {
                        sheetName = "Open Position";
                        IXLWorksheet ws = wb.Worksheets.Add(dt, sheetName);
                    }
                    else
                    {
                        try
                        {
                            if (dt != null)
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    sheetName = "Open Position";
                                    IXLWorksheet ws = wb.Worksheets.Add(sheetName);
                                    string[] cols = new string[dt.Columns.Count];
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        string a = dt.Columns[c].ToString();
                                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                                    }
                                    string StartCharCols = "A";
                                    int StartIndexCols = 1;

                                    for (int i = 1; i <= cols.Length; i++)
                                    {
                                        if (i == cols.Length)
                                        {
                                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                            ws.Cell(DataCell).Value = cols[i - 1];
                                            ws.Cell(DataCell).Style.Font.Bold = true;
                                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                            ws.Cell(DataCell).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                                            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                        }
                                        else
                                        {
                                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                            ws.Cell(DataCell).Value = cols[i - 1];
                                            ws.Cell(DataCell).Style.Font.Bold = true;
                                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                            ws.Cell(DataCell).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                                            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                            //StartCharCols++;
                                            StartCharCols = ColumnIndexToColumnLetter(i + 1);
                                        }
                                    }
                                    string StartCharData = "A";
                                    int StartIndexData = 2;
                                    char StartCharDataCol = char.MinValue;
                                    for (int p = 0; p < dt.Rows.Count; p++)
                                    {
                                        for (int j = 0; j < dt.Columns.Count; j++)
                                        {
                                            string DataCell = StartCharData.ToString() + StartIndexData;
                                            string a = dt.Rows[p][j].ToString();
                                            a = a.Replace("&nbsp;", " ");
                                            a = a.Replace("&amp;", "&");
                                            int val = 0;
                                            double val2 = 0.0;
                                            DateTime dt2 = DateTime.Now;
                                            if (int.TryParse(a, out val))
                                            {
                                                ws.Cell(DataCell).Value = val;
                                                ws.Cell(DataCell).SetValue(a);
                                                ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val);
                                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                            }
                                            if (double.TryParse(a, out val2))
                                            {
                                                ws.Cell(DataCell).Value = val2;
                                                ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val2);
                                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                            }
                                            else if (DateTime.TryParse(a, out dt2))
                                            {
                                                ws.Cell(DataCell).Value = dt2.ToShortDateString();
                                                ws.Cell(DataCell).SetValue(a);
                                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                            }
                                            else
                                            {
                                                ws.Cell(DataCell).SetValue(a);
                                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                            }
                                            StartCharData = ColumnIndexToColumnLetter(j + 2);
                                            ws.Columns().AdjustToContents();
                                        }
                                        StartCharData = "A";
                                        StartIndexData++;
                                    }

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
                else if (Y == 2)
                {
                    dt = (DataTable)HttpContext.Current.Session["PerformanceReport"];
                    if (dt == null)
                    {
                        sheetName = "Summary";
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
                                    sheetName = "Summary";
                                    IXLWorksheet ws = wb.Worksheets.Add(sheetName);
                                    string[] cols = new string[dt.Columns.Count];
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        string a = dt.Columns[c].ToString();
                                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                                    }
                                    string StartCharCols = "A";
                                    int StartIndexCols = 1;
                                    for (int i = 1; i <= cols.Length; i++)
                                    {
                                        if (i == cols.Length)
                                        {
                                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                            ws.Cell(DataCell).Value = cols[i - 1];
                                            ws.Cell(DataCell).Style.Font.Bold = true;
                                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                            ws.Cell(DataCell).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                                            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                        }
                                        else
                                        {
                                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                            ws.Cell(DataCell).Value = cols[i - 1];
                                            ws.Cell(DataCell).Style.Font.Bold = true;
                                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                            ws.Cell(DataCell).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                                            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                            //StartCharCols++;
                                            StartCharCols = ColumnIndexToColumnLetter(i + 1);
                                        }
                                    }
                                    string StartCharData = "A";
                                    int StartIndexData = 2;
                                    char StartCharDataCol = char.MinValue;
                                    for (int p = 0; p < dt.Rows.Count; p++)
                                    {
                                        for (int j = 0; j < dt.Columns.Count; j++)
                                        {
                                            string DataCell = StartCharData.ToString() + StartIndexData;
                                            string a = dt.Rows[p][j].ToString();
                                            a = a.Replace("&nbsp;", " ");
                                            a = a.Replace("&amp;", "&");
                                            int val = 0;
                                            double val2 = 0.0;
                                            DateTime dt2 = DateTime.Now;
                                            if (int.TryParse(a, out val))
                                            {
                                                ws.Cell(DataCell).Value = val;
                                                ws.Cell(DataCell).SetValue(a);
                                                ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val);
                                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                            }
                                            if (double.TryParse(a, out val2))
                                            {
                                                ws.Cell(DataCell).Value = val2;
                                                ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val2);
                                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                            }
                                            else if (DateTime.TryParse(a, out dt2))
                                            {
                                                ws.Cell(DataCell).Value = dt2.ToShortDateString();
                                                ws.Cell(DataCell).SetValue(a);
                                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                            }
                                            else
                                            {
                                                ws.Cell(DataCell).SetValue(a);
                                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                            }
                                            StartCharData = ColumnIndexToColumnLetter(j + 2);
                                            ws.Columns().AdjustToContents();
                                        }
                                        StartCharData = "A";
                                        StartIndexData++;
                                    }
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
            HttpContext.Current.Session["ClosedPosition"] = null;
            HttpContext.Current.Session["OpenPosition"] = null;
            HttpContext.Current.Session["PerformanceReport"] = null;

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

    public string ColumnIndexToColumnLetter(int colIndex)
    {
        int div = colIndex;
        string colLetter = String.Empty;
        int mod = 0;

        while (div > 0)
        {
            mod = (div - 1) % 26;
            colLetter = (char)(65 + mod) + colLetter;
            div = (int)((div - mod) / 26);
        }
        return colLetter;
    }
    public void ExportToExcel_MISClosedPositionReport(DataTable dt, string FromDate, String CompName)
    {
        try
        {
            XLWorkbook wb = new XLWorkbook();
            string sheetName = "";
            //DataTable dt = new DataTable();


            for (int Y = 0; Y < 6; Y++)
            {
                if (Y == 1)
                {
                    dt = (DataTable)HttpContext.Current.Session["ClosedPosition"];
                    if (dt == null)
                    {
                        sheetName = "Closed Position";
                        IXLWorksheet ws = wb.Worksheets.Add(dt, sheetName);
                    }
                    else
                    {
                        try
                        {
                            if (dt != null)
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    // XLWorkbook wb = new XLWorkbook();
                                    sheetName = "Closed Position";
                                    IXLWorksheet ws = wb.Worksheets.Add(dt, sheetName);
                                    //Add new sheet



                                    //fetching columns from dt
                                    string[] cols = new string[dt.Columns.Count];
                                    //adding the  columns
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        string a = dt.Columns[c].ToString();
                                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                                    }

                                    //row 1 header
                                    char StartCharCols = 'A';
                                    int StartIndexCols = 1;
                                    //   ws.AutoFilter.Clear();
                                    char dblStartCharCols = 'A';
                                    // filling header with formatting and colors
                                    int count = 0;
                                    for (int i = 1; i <= cols.Length; i++)
                                    {
                                        if (i == cols.Length)
                                        {
                                            if (count > 0)
                                            {
                                                //wrong
                                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();

                                                ws.Cell(DataCell).Value = cols[i - 1];
                                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                                ws.Cell(DataCell).Style.Font.Bold = true;
                                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                            }
                                            else
                                            {
                                            }
                                        }
                                        else
                                        {
                                            if (StartCharCols.ToString() == "[" || count > 0)
                                            {
                                                count = 1;
                                                if (i % 27 == 0)
                                                {
                                                    //fine
                                                    StartCharCols = 'A';
                                                    if (i % 54 == 0)
                                                    {
                                                        dblStartCharCols++;
                                                    }
                                                }
                                                //dblStartCharCols++;
                                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                                ws.Cell(DataCell).Value = cols[i - 1];
                                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                                ws.Cell(DataCell).Style.Font.Bold = true;
                                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                                StartCharCols++;
                                            }
                                            else
                                            {
                                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                                ws.Cell(DataCell).Value = cols[i - 1];
                                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                                ws.Cell(DataCell).Style.Font.Bold = true;
                                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                                StartCharCols++;
                                            }
                                        }
                                    }


                                    int Ccout = 0;
                                    char StartCharData = 'A';
                                    int StartIndexData = 2;
                                    dblStartCharCols = 'A';

                                    char StartCharDataCol = char.MinValue;
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        for (int j = 0; j < dt.Columns.Count; j++)
                                        {
                                            if (StartCharData.ToString() == "[" || Ccout > 0)
                                            {
                                                Ccout = 1;
                                                if (j % 26 == 0)
                                                {
                                                    StartCharData = 'A';
                                                    if (j % 52 == 0)
                                                    {
                                                        dblStartCharCols++;
                                                    }
                                                }

                                                string DataCell = dblStartCharCols.ToString() + StartCharData.ToString() + StartIndexData;
                                                string a = dt.Rows[i][j].ToString();
                                                a = a.Replace("&nbsp;", " ");
                                                a = a.Replace("&amp;", "&");
                                                //check if value is of integer type
                                                int val = 0;
                                                DateTime dt2 = DateTime.Now;
                                                if (int.TryParse(a, out val))
                                                {
                                                    //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                                    ws.Cell(DataCell).Value = objCommon.DateFormat(dt2.ToShortDateString());
                                                    ws.Cell(DataCell).SetValue(objCommon.DateFormat(dt2.ToShortDateString()));
                                                }
                                                else if (StartCharData.ToString() == "G")
                                                {
                                                    if (a == "")
                                                    {
                                                        ws.Cell(DataCell).SetValue(a);
                                                    }
                                                    else
                                                    {
                                                        ws.Cell(DataCell).Value = Convert.ToDecimal(a);
                                                        ws.Cell(DataCell).SetValue(Convert.ToDecimal(a));
                                                        //TotalAccInt += Convert.ToDouble(a);
                                                    }
                                                }
                                                //check if datetime type
                                                //else if (DateTime.TryParse(a, out dt2))
                                                //{
                                                //    ws.Cell(DataCell).Value = objCommon.DateFormat(dt2.ToShortDateString());
                                                //    ws.Cell(DataCell).SetValue(objCommon.DateFormat(dt2.ToShortDateString()));
                                                //}

                                                else if (StartCharData.ToString() == "G" || StartCharData.ToString() == "H" || StartCharData.ToString() == "I" || StartCharData.ToString() == "Q" || StartCharData.ToString() == "R")
                                                {
                                                    if (a == "")
                                                    {
                                                        ws.Cell(DataCell).SetValue(a);
                                                    }
                                                    else
                                                    {
                                                        ws.Cell(DataCell).Value = Convert.ToDecimal(a);
                                                        ws.Cell(DataCell).SetValue(Convert.ToDecimal(a));
                                                        //TotalAccInt += Convert.ToDouble(a);
                                                    }
                                                }
                                                else if (StartCharData.ToString() == "S" || StartCharData.ToString() == "T" || StartCharData.ToString() == "U" || StartCharData.ToString() == "W" || StartCharData.ToString() == "X")
                                                {
                                                    if (a == "")
                                                    {
                                                        ws.Cell(DataCell).SetValue(a);
                                                    }
                                                    else
                                                    {
                                                        ws.Cell(DataCell).Value = Convert.ToDecimal(a);
                                                        ws.Cell(DataCell).SetValue(Convert.ToDecimal(a));
                                                        //TotalAccInt += Convert.ToDouble(a);
                                                    }
                                                }
                                                else if (StartCharData.ToString() == "Y" || StartCharData.ToString() == "Z")
                                                {
                                                    if (a == "")
                                                    {
                                                        ws.Cell(DataCell).SetValue(a);
                                                    }
                                                    else
                                                    {
                                                        ws.Cell(DataCell).Value = Convert.ToDecimal(a);
                                                        ws.Cell(DataCell).SetValue(Convert.ToDecimal(a));
                                                        //TotalAccInt += Convert.ToDouble(a);
                                                    }
                                                }
                                                else if (DataCell.ToString() == "AA" + StartIndexData || DataCell.ToString() == "AB" + StartIndexData || DataCell.ToString() == "AC" + StartIndexData)
                                                {
                                                    if (a == "")
                                                    {
                                                        ws.Cell(DataCell).SetValue(a);
                                                    }
                                                    else
                                                    {
                                                        ws.Cell(DataCell).Value = Convert.ToDecimal(a);
                                                        ws.Cell(DataCell).SetValue(Convert.ToDecimal(a));
                                                        ws.Cell(DataCell).Value = cols[i - 1];
                                                        ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                                        ws.Cell(DataCell).Style.Font.Bold = true;
                                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                                                        ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                                        //TotalAccInt += Convert.ToDouble(a);
                                                    }
                                                }
                                                else
                                                {
                                                    ws.Cell(DataCell).SetValue(a);
                                                }

                                                StartCharData++;
                                            }
                                            else
                                            {
                                                string DataCell = StartCharData.ToString() + StartIndexData;
                                                string a = dt.Rows[i][j].ToString();
                                                a = a.Replace("&nbsp;", " ");
                                                a = a.Replace("&amp;", "&");
                                                //check if value is of integer type
                                                int val = 0;
                                                DateTime dt2 = DateTime.Now;
                                                if (int.TryParse(a, out val))
                                                {
                                                    //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                                    ws.Cell(DataCell).Value = val;
                                                }
                                                else if (StartCharData.ToString() == "G")
                                                {
                                                    if (a == "")
                                                    {
                                                        ws.Cell(DataCell).SetValue(a);
                                                    }
                                                    else
                                                    {
                                                        ws.Cell(DataCell).Value = Convert.ToDecimal(a);
                                                        ws.Cell(DataCell).SetValue(Convert.ToDecimal(a));
                                                        //TotalAccInt += Convert.ToDouble(a);
                                                    }
                                                }
                                                //check if datetime type
                                                //else if (DateTime.TryParse(a, out dt2))
                                                //{
                                                //    //ws.Cell(DataCell).Value = dt2.ToShortDateString();
                                                //    ws.Cell(DataCell).Value = objCommon.DateFormat(dt2.ToShortDateString());
                                                //    ws.Cell(DataCell).SetValue(objCommon.DateFormat(dt2.ToShortDateString()));
                                                //}
                                                else if (StartCharData.ToString() == "H" || StartCharData.ToString() == "I" || StartCharData.ToString() == "Q" || StartCharData.ToString() == "R")
                                                {
                                                    if (a == "")
                                                    {
                                                        ws.Cell(DataCell).SetValue(a);
                                                    }
                                                    else
                                                    {
                                                        ws.Cell(DataCell).Value = Convert.ToDecimal(a);
                                                        ws.Cell(DataCell).SetValue(Convert.ToDecimal(a));
                                                        //TotalAccInt += Convert.ToDouble(a);
                                                    }
                                                }
                                                else if (StartCharData.ToString() == "S" || StartCharData.ToString() == "T" || StartCharData.ToString() == "U" || StartCharData.ToString() == "W" || StartCharData.ToString() == "X")
                                                {
                                                    if (a == "")
                                                    {
                                                        ws.Cell(DataCell).SetValue(a);
                                                    }
                                                    else
                                                    {
                                                        ws.Cell(DataCell).Value = Convert.ToDecimal(a);
                                                        ws.Cell(DataCell).SetValue(Convert.ToDecimal(a));
                                                        //TotalAccInt += Convert.ToDouble(a);
                                                    }
                                                }
                                                else if (StartCharData.ToString() == "Y" || StartCharData.ToString() == "Z")
                                                {
                                                    if (a == "")
                                                    {
                                                        ws.Cell(DataCell).SetValue(a);
                                                    }
                                                    else
                                                    {
                                                        ws.Cell(DataCell).Value = Convert.ToDecimal(a);
                                                        ws.Cell(DataCell).SetValue(Convert.ToDecimal(a));
                                                        //TotalAccInt += Convert.ToDouble(a);
                                                    }
                                                }
                                                else if (DataCell.ToString() == "AA" + StartIndexData || DataCell.ToString() == "AB" + StartIndexData || DataCell.ToString() == "AC" + StartIndexData)
                                                {
                                                    if (a == "")
                                                    {
                                                        ws.Cell(DataCell).SetValue(a);
                                                    }
                                                    else
                                                    {
                                                        ws.Cell(DataCell).Value = Convert.ToDecimal(a);
                                                        ws.Cell(DataCell).SetValue(Convert.ToDecimal(a));
                                                        ws.Cell(DataCell).Value = cols[i - 1];
                                                        ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                                        ws.Cell(DataCell).Style.Font.Bold = true;
                                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                                                        ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                                        //TotalAccInt += Convert.ToDouble(a);
                                                    }
                                                }
                                                else
                                                {
                                                    ws.Cell(DataCell).SetValue(a);
                                                }

                                                StartCharData++;
                                            }
                                        }
                                        StartCharData = 'A';
                                        StartIndexData++;
                                        Ccout = 0;
                                    }

                                    //Code to save the file
                                    //string Range = "";
                                    //string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                                    //strdtTime = strdtTime.Replace("PM", "");
                                    //strdtTime = strdtTime.Replace("AM", "");

                                    //Range = "A1:D1";
                                    //ws.Range(Range).Style.Font.FontSize = 11;
                                    //ws.Range(Range).Style.Font.Bold = true;
                                    //ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                                    //ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    //ws.Range(Range).Style.Fill.BackgroundColor = XLColor.LightGray;
                                    //  ws.Cell("A" + 1).SetValue(" From " + FromDate + " To " + " For Company " + CompName);

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

                        //for (int i = 0; i < dt.Rows.Count; i++)
                        //{

                        //}



                    }
                }
                else if (Y == 0)
                {
                    dt = (DataTable)HttpContext.Current.Session["OpenPosition"];
                    if (dt == null)
                    {
                        sheetName = "Open Position";
                        IXLWorksheet ws = wb.Worksheets.Add(dt, sheetName);
                    }
                    else
                    {
                        sheetName = "Open Position";
                        IXLWorksheet ws = wb.Worksheets.Add(dt, sheetName);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                        }


                    }
                }
                else if (Y == 2)
                {
                    dt = (DataTable)HttpContext.Current.Session["PerformanceReport"];
                    if (dt == null)
                    {
                        sheetName = "Summary";
                        IXLWorksheet ws = wb.Worksheets.Add(dt, sheetName);
                    }
                    else
                    {

                        try
                        {
                            if (dt != null)
                            {
                                if (dt.Rows.Count > 0)
                                {

                                    //XLWorkbook wb = new XLWorkbook();
                                    sheetName = "Summary";
                                    IXLWorksheet ws = wb.Worksheets.Add(dt, sheetName);
                                    //IXLWorksheet ws;
                                    //ws = wb.Worksheets.Add("Security Evaluation Report");

                                    //Summary Table

                                    //string Range = "B2:D2";
                                    //ws.Range(Range).Merge();
                                    //ws.Cell("B2").Value = "Open Position Summary";
                                    //ws.Cell("B2").Style.Font.Bold = true;

                                    //Range = "B3:D3";
                                    //ws.Range(Range).Merge();
                                    //ws.Cell("B3").Value = "Particulars";
                                    //ws.Cell("B3").Style.Font.Bold = true;

                                    //ws.Cell("E3").Value = "Amount";
                                    //ws.Cell("E3").Style.Font.Bold = true;

                                    //Range = "B4:D4";
                                    //ws.Range(Range).Merge();
                                    //ws.Cell("B4").Value = "Open Position as on (Rs.in Crs) ";

                                    //Range = "B5:D5";
                                    //ws.Range(Range).Merge();
                                    //ws.Cell("B5").Value = "Open position Loss in Rs.";

                                    //ws.Cell("E2").Value = "";
                                    //Range = "B6:D6";
                                    //ws.Range(Range).Merge();
                                    //ws.Cell("B6").Value = "Open position Gain in Rs.";

                                    //ws.Cell("B3").Style.Fill.BackgroundColor = XLColor.Redwood;
                                    //ws.Cell("B3").Style.Font.FontColor = XLColor.White;

                                    //ws.Cell("E3").Style.Fill.BackgroundColor = XLColor.Redwood;
                                    //ws.Cell("E3").Style.Font.FontColor = XLColor.White;

                                    //ws.Cell("E3").Style.Fill.BackgroundColor = XLColor.Redwood;
                                    //ws.Cell("E3").Style.Font.FontColor = XLColor.White;

                                    //ws.Cell("E5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                    //ws.Cell("E6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                    ////fetching columns from dt

                                    //Range = "B2:E6";
                                    //ws.Range(Range).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                    //ws.Range(Range).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                    //ws.Range(Range).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                    //ws.Range(Range).Style.Border.BottomBorder = XLBorderStyleValues.Thin;


                                    string[] cols = new string[dt.Columns.Count];
                                    //adding the  columns
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        string a = dt.Columns[c].ToString();
                                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                                    }
                                    char StartCharCols = 'A';
                                    int StartIndexCols = 1;
                                    //   ws.AutoFilter.Clear();

                                    // filling header with formatting and colors
                                    for (int i = 1; i <= cols.Length; i++)
                                    {
                                        if (i == cols.Length)
                                        {
                                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                            ws.Cell(DataCell).Value = cols[i - 1];
                                            ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                            ws.Cell(DataCell).Style.Font.Bold = true;
                                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                            ws.Cell(DataCell).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                                        }
                                        else
                                        {
                                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                            if (i == 1 || i == 5 || i == 6)
                                            {
                                                ws.Cell(DataCell).Value = cols[i - 1];
                                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 6;
                                                ws.Cell(DataCell).Style.Font.Bold = true;
                                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                                ws.Cell(DataCell).Style.Alignment.WrapText = true;
                                                ws.Cell(DataCell).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                                            }
                                            else
                                            {
                                                ws.Cell(DataCell).Value = cols[i - 1];
                                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                                ws.Cell(DataCell).Style.Font.Bold = true;
                                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SteelBlue;
                                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                                ws.Cell(DataCell).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                                            }
                                            StartCharCols++;
                                        }
                                    }

                                    ////
                                    //char StartCharData = 'B';
                                    //int StartIndexData = 10;
                                    //double TotalFV = 0.0;
                                    //double TotalAccInt = 0.0;
                                    //char StartCharDataCol = char.MinValue;
                                    //for (int i = 0; i < dt.Rows.Count; i++)
                                    //{
                                    //    for (int j = 0; j < dt.Columns.Count; j++)
                                    //    {
                                    //        string DataCell = StartCharData.ToString() + StartIndexData;
                                    //        string a = dt.Rows[i][j].ToString();
                                    //        a = a.Replace("&nbsp;", " ");
                                    //        a = a.Replace("&amp;", "&");
                                    //        //check if value is of integer type
                                    //        int val = 0;
                                    //        DateTime dt2 = DateTime.Now;
                                    //        if (StartCharData.ToString() == "E" || StartCharData.ToString() == "F" || StartCharData.ToString() == "G" || StartCharData.ToString() == "H" || StartCharData.ToString() == "I" || StartCharData.ToString() == "J")
                                    //        {
                                    //            if (a == "")
                                    //            {
                                    //                ws.Cell(DataCell).SetValue(a);
                                    //            }
                                    //            else
                                    //            {
                                    //                if (StartCharData.ToString() == "G")
                                    //                {
                                    //                    TotalPurValue += Convert.ToDouble(a);
                                    //                }
                                    //                if (StartCharData.ToString() == "I")
                                    //                {
                                    //                    TotalMTMGain += Convert.ToDouble(a);
                                    //                }
                                    //                if (StartCharData.ToString() == "J")
                                    //                {
                                    //                    TotalMTMLoss += Convert.ToDouble(a);
                                    //                }

                                    //                ws.Cell(DataCell).Value = Convert.ToDouble(a);
                                    //            }
                                    //        }
                                    //        else if (StartCharData.ToString() == "D")
                                    //        {
                                    //            if (a == "")
                                    //            {
                                    //                ws.Cell(DataCell).SetValue(a);
                                    //            }
                                    //            else
                                    //            {
                                    //                if (DateTime.TryParse(a, out dt2))
                                    //                {
                                    //                    ws.Cell(DataCell).Value = dt2.ToShortDateString();

                                    //                }
                                    //            }
                                    //        }
                                    //        else
                                    //        {
                                    //            ws.Cell(DataCell).SetValue(a);
                                    //        }
                                    //        StartCharData++;
                                    //    }
                                    //    StartCharData = 'B';
                                    //    StartIndexData++;
                                    //}
                                    //ws.Column(4).Style.DateFormat.SetFormat("dd/mm/yyyy");
                                    //ws.Column(6).Style.NumberFormat.Format = "#,##0.0000";
                                    //ws.Column(8).Style.NumberFormat.Format = "#,##0.0000";
                                    //ws.Cell("F" + StartIndexData).SetValue("Total");
                                    //ws.Cell("F" + StartIndexData).Style.Font.Bold = true;

                                    //ws.Cell("G" + StartIndexData).SetValue(TotalPurValue);
                                    //ws.Cell("G" + StartIndexData).Style.Font.Bold = true;

                                    //ws.Cell("I" + StartIndexData).SetValue(TotalMTMGain);
                                    //ws.Cell("I" + StartIndexData).Style.Font.Bold = true;

                                    //ws.Cell("J" + StartIndexData).SetValue(TotalMTMLoss);
                                    //ws.Cell("J" + StartIndexData).Style.Font.Bold = true;
                                    //if (TotalPurValue > 0)
                                    //{
                                    //    ws.Cell("E4").SetValue(TotalPurValue / 10000000);
                                    //}

                                    //ws.Cell("E5").SetValue("(" + Convert.ToString(TotalMTMLoss) + ")");
                                    //ws.Cell("E6").SetValue(Convert.ToString(TotalMTMGain));

                                    //Range = "";
                                    //Range = "B9:J" + StartIndexData;
                                    //ws.Range(Range).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                    //ws.Range(Range).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                    //ws.Range(Range).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                    //ws.Range(Range).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                    ////ws.Cell("I10").FormulaA1 = "\"The total value is: \" & SUM(F10:G10)";

                                    //ws.Row(9).Height = 30;
                                    //ws.Column(1).Width = 2;
                                    //ws.Column(3).Width = 50;
                                    //ws.ShowGridLines = false;
                                    //ws.Column(2).AdjustToContents();
                                    //ws.Column(4).AdjustToContents();
                                    //ws.Column(5).Width = 10;
                                    //// ws.Column(5).Style.Alignment.SetVertical(XLAlignmentHorizontalValues.Right );
                                    //ws.Column(6).AdjustToContents();
                                    //ws.Column(7).AdjustToContents();
                                    //ws.Column(8).AdjustToContents();
                                    //ws.Column(9).AdjustToContents();
                                    //ws.Column(10).AdjustToContents();
                                    //Code to save the file
                                    //HttpContext.Current.Response.Clear();
                                    //HttpContext.Current.Response.Buffer = true;
                                    //HttpContext.Current.Response.Charset = "";
                                    //HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=OpenPositionSummary.xlsx");
                                    //HttpContext.Current.Response.Charset = "";
                                    //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                    //using (MemoryStream MyMemoryStream = new MemoryStream())
                                    //{
                                    //    wb.SaveAs(MyMemoryStream);
                                    //    MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                                    //    HttpContext.Current.Response.Flush();
                                    //    HttpContext.Current.Response.SuppressContent = true;
                                    //}
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }


                    }
                }



            }

            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.Buffer = true;
            //HttpContext.Current.Response.Charset = "";
            //HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=MISStockReport" + strdtTime + ".xlsx");
            //HttpContext.Current.Response.Charset = "";
            //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //using (MemoryStream MyMemoryStream = new MemoryStream())
            //{
            //    wb.SaveAs(MyMemoryStream);
            //    MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
            //    HttpContext.Current.Response.Flush();
            //    HttpContext.Current.Response.End();
            //}

            // Clear All the Session Here.....
            HttpContext.Current.Session["ClosedPosition"] = null;
            HttpContext.Current.Session["PerformanceReport"] = null;
            HttpContext.Current.Session["OpenPosition"] = null;



            //Code to save the file
            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.Buffer = true;
            //HttpContext.Current.Response.Charset = "";
            //HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Stock_Report.xlsx");
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


    public void ExportToExcel_DealsUpdate(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("Updated Deals");
                    //Add new sheet



                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 3;
                    //   ws.AutoFilter.Clear();
                    char dblStartCharCols = 'A';
                    // filling header with formatting and colors
                    int count = 0;
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        if (i == cols.Length)
                        {
                            if (count > 0)
                            {
                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            }
                            else
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            }
                        }
                        else
                        {
                            if (StartCharCols.ToString() == "[" || count > 0)
                            {
                                count = 1;
                                if (i % 27 == 0)
                                {
                                    StartCharCols = 'A';
                                    if (i % 54 == 0)
                                    {
                                        dblStartCharCols++;
                                    }
                                }
                                //dblStartCharCols++;
                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }
                            else
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }
                        }
                    }

                    int Ccout = 0;
                    char StartCharData = 'A';
                    int StartIndexData = 4;
                    dblStartCharCols = 'A';

                    char StartCharDataCol = char.MinValue;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (StartCharData.ToString() == "[" || Ccout > 0)
                            {
                                Ccout = 1;
                                if (j % 26 == 0)
                                {
                                    StartCharData = 'A';
                                    if (j % 52 == 0)
                                    {
                                        dblStartCharCols++;
                                    }
                                }

                                string DataCell = dblStartCharCols.ToString() + StartCharData.ToString() + StartIndexData;
                                string a = dt.Rows[i][j].ToString();
                                a = a.Replace("&nbsp;", " ");
                                a = a.Replace("&amp;", "&");
                                //check if value is of integer type
                                int val = 0;
                                DateTime dt2 = DateTime.Now;
                                if (int.TryParse(a, out val))
                                {
                                    //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                    ws.Cell(DataCell).Value = val;
                                }
                                //check if datetime type
                                //else if (DateTime.TryParse(a, out dt2))
                                //{
                                //    ws.Cell(DataCell).Value = dt2.ToShortDateString();
                                //}
                                ws.Cell(DataCell).SetValue(a);
                                StartCharData++;
                            }
                            else
                            {
                                string DataCell = StartCharData.ToString() + StartIndexData;
                                string a = dt.Rows[i][j].ToString();
                                a = a.Replace("&nbsp;", " ");
                                a = a.Replace("&amp;", "&");
                                //check if value is of integer type
                                int val = 0;
                                DateTime dt2 = DateTime.Now;
                                if (int.TryParse(a, out val))
                                {
                                    //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                    ws.Cell(DataCell).Value = val;
                                }
                                //check if datetime type
                                //else if (DateTime.TryParse(a, out dt2))
                                //{
                                //    ws.Cell(DataCell).Value = dt2.ToShortDateString();
                                //}
                                ws.Cell(DataCell).SetValue(a);
                                StartCharData++;
                            }
                        }
                        StartCharData = 'A';
                        StartIndexData++;
                        Ccout = 0;
                    }

                    //Code to save the file
                    //string Range = "";
                    //Range = "A1:D1";
                    //ws.Range(Range).Style.Font.FontSize = 11;
                    //ws.Range(Range).Style.Font.Bold = true;
                    //ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    //ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    //ws.Range(Range).Style.Fill.BackgroundColor = XLColor.LightGray;
                    //ws.Cell("A" + 1).SetValue("Deals Updated From " + FromDate + " To " + ToDate);
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=DealsUpdate.xlsx");
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
                else
                {
                    // ClientScript.RegisterStartupScript(this.GetType(), "Key", "alert('No Data Found.');", true);
                }
            }
            else
            {
                //  ClientScript.RegisterStartupScript(this.GetType(), "Key", "alert('No Data Found.');", true);
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void ExportToExcel_ABML_MIS()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)HttpContext.Current.Session["Retail_MIS"];
            XLWorkbook wb = new XLWorkbook();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    IXLWorksheet ws = wb.Worksheets.Add("MIS");
                    //dt.Columns.Remove("DealDate");

                    string Range = "";

                    /*Merging Header*/
                    Range = "A2:A2";
                    //ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    //ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Value = "GAIN / LOSS STATEMENT OF ABML - DEBT INSTRUMENT";
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.White;
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    //ws.Cell(2, 2).Style.Border.LeftBorder = XLBorderStyleValues.Thick;

                    Range = "B4:C4";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Value = "Security Details";
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.White;
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    //ws.Cell(4, 2).Style.Border.LeftBorder = XLBorderStyleValues.Thick;

                    Range = "F4:G4";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Value = "Buying Leg";
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.White;
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;

                    Range = "M4:N4";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Value = "Selling Leg";
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.White;
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;

                    string[] cols = new string[dt.Columns.Count];
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }
                    char StartCharCols = 'A';
                    int StartIndexCols = 5;

                    int count = 0;
                    char dblStartCharCols = 'A';

                    for (int i = 1; i <= cols.Length; i++)
                    {
                        if (i == cols.Length)
                        {
                            if (count > 0)
                            {
                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                                if (i <= 5)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                }
                                else if (i > 5 && i <= 12)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.Yellow;
                                }
                                else if (i > 12 && i <= 19)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightGreen;
                                }
                                else if (i > 19 && i <= 22)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SkyBlue;
                                }
                                else if (i > 22)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                }
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            }
                            else
                            {
                                if (StartCharCols.ToString() == "[" || count > 0)
                                {
                                    count = 1;
                                    if (i % 27 == 0)
                                    {
                                        StartCharCols = 'A';
                                        if (i % 54 == 0)
                                        {
                                            dblStartCharCols++;
                                        }
                                    }
                                    string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                    ws.Cell(DataCell).Value = cols[i - 1];
                                    ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                    ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                    ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                    ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                    ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                    //ws.Cell(DataCell).Style.Font.Bold = true;
                                    if (i <= 5)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                    }
                                    else if (i > 5 && i <= 12)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.Yellow;
                                    }
                                    else if (i > 12 && i <= 19)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightGreen;
                                    }
                                    else if (i > 19 && i <= 22)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SkyBlue;
                                    }
                                    else if (i > 22)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                    }
                                    ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                }
                                else
                                {
                                    string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                    ws.Cell(DataCell).Value = cols[i - 1];
                                    ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                    ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                    ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                    ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                    ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                    //ws.Cell(DataCell).Style.Font.Bold = true;
                                    if (i <= 5)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                    }
                                    else if (i > 5 && i <= 12)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.Yellow;
                                    }
                                    else if (i > 12 && i <= 19)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightGreen;
                                    }
                                    else if (i > 19 && i <= 22)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SkyBlue;
                                    }
                                    else if (i > 22)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                    }
                                    ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                }

                            }
                        }
                        else
                        {
                            if (StartCharCols.ToString() == "[" || count > 0)
                            {
                                count = 1;
                                if (i % 27 == 0)
                                {
                                    StartCharCols = 'A';
                                    if (i % 54 == 0)
                                    {
                                        dblStartCharCols++;
                                    }
                                }
                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                //ws.Cell(DataCell).Style.Font.Bold = true;
                                if (i <= 5)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                }
                                else if (i > 5 && i <= 12)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.Yellow;
                                }
                                else if (i > 12 && i <= 19)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightGreen;
                                }
                                else if (i > 19 && i <= 22)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SkyBlue;
                                }
                                else if (i > 22)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                }
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            }
                            else
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                //ws.Cell(DataCell).Style.Font.Bold = true;
                                if (i <= 5)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                }
                                else if (i > 5 && i <= 12)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.Yellow;
                                }
                                else if (i > 12 && i <= 19)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightGreen;
                                }
                                else if (i > 19 && i <= 22)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SkyBlue;
                                }
                                else if (i > 22)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                }
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            }
                        }
                        StartCharCols++;
                    }


                    int Ccout = 0;
                    char StartCharData = 'A';
                    int StartIndexData = 6;
                    dblStartCharCols = 'A';

                    char StartCharDataCol = char.MinValue;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (StartCharData.ToString() == "[" || Ccout > 0)
                            {
                                Ccout = 1;
                                if (j % 26 == 0)
                                {
                                    StartCharData = 'A';
                                    if (j % 52 == 0)
                                    {
                                        dblStartCharCols++;
                                    }
                                }
                                string DataCell = dblStartCharCols.ToString() + StartCharData.ToString() + StartIndexData;
                                string a = dt.Rows[i][j].ToString();
                                a = a.Replace("&nbsp;", " ");
                                a = a.Replace("&amp;", "&");

                                ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                if (StartCharData.ToString() == "E" || StartCharData.ToString() == "L" || StartCharData.ToString() == "S")
                                {
                                    ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                }
                                else
                                {
                                    ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                }

                                ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                //check if value is of integer type
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

                                //                            ws.Cell("AA" + StartIndexData.ToString()).FormulaA1 = "{G" + StartIndexData.ToString() + "H" + StartIndexData.ToString() + "}";

                                StartCharData++;
                            }
                            else
                            {
                                string DataCell = StartCharData.ToString() + StartIndexData;
                                string a = dt.Rows[i][j].ToString();
                                a = a.Replace("&nbsp;", " ");
                                a = a.Replace("&amp;", "&");

                                ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                if (StartCharData.ToString() == "E" || StartCharData.ToString() == "L" || StartCharData.ToString() == "S")
                                {
                                    ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                }
                                else
                                {
                                    ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                }

                                ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                //check if value is of integer type
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
                        }
                        StartCharData = 'A';
                        StartIndexData++;
                        Ccout = 0;
                    }

                    StartIndexData++;
                    ws.Cell("S" + StartIndexData.ToString()).SetValue("Total");
                    ws.Cell("T" + StartIndexData.ToString()).SetValue(HttpContext.Current.Session["TMISCapitalGain"]);
                    ws.Cell("Z" + StartIndexData.ToString()).SetValue(HttpContext.Current.Session["NetIntt.Income"]);
                    ws.Cell("AA" + StartIndexData.ToString()).SetValue(HttpContext.Current.Session["TMISTotalNettgain"]);

                    ws.Cell("S" + StartIndexData.ToString()).Style.Font.Bold = true;
                    ws.Cell("T" + StartIndexData.ToString()).Style.Font.Bold = true;
                    ws.Cell("Z" + StartIndexData.ToString()).Style.Font.Bold = true;
                    ws.Cell("AA" + StartIndexData.ToString()).Style.Font.Bold = true;
                }
            }



            dt = (DataTable)HttpContext.Current.Session["Retail_OpenPositions"];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    IXLWorksheet ws = wb.Worksheets.Add("Open Positions");
                    //dt.Columns.Remove("DealDate");

                    string Range = "";

                    /*Merging Header*/
                    Range = "A1:A1";
                    //ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    //ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Value = "OPEN POSITION STATEMENT OF ABML - DEBT INSTRUMENT";
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.White;
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    //ws.Cell(2, 2).Style.Border.LeftBorder = XLBorderStyleValues.Thick;

                    Range = "B4:C4";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Value = "Security Details";
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.White;
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    //ws.Cell(4, 2).Style.Border.LeftBorder = XLBorderStyleValues.Thick;

                    Range = "F4:G4";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Value = "Buying Leg";
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.White;
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;

                    Range = "M4:N4";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Value = "Selling Leg";
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.White;
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;

                    string[] cols = new string[dt.Columns.Count];
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }
                    char StartCharCols = 'A';
                    int StartIndexCols = 5;

                    int count = 0;
                    char dblStartCharCols = 'A';

                    for (int i = 1; i <= cols.Length; i++)
                    {
                        if (i == cols.Length)
                        {
                            if (count > 0)
                            {
                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                //ws.Cell(DataCell).Style.Font.Bold = true;
                                if (i <= 5)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                }
                                else if (i > 5 && i <= 12)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.Yellow;
                                }
                                else if (i > 12 && i <= 19)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightGreen;
                                }
                                else if (i > 19 && i <= 22)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SkyBlue;
                                }
                                else if (i > 22)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                }
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            }
                            else
                            {
                                if (StartCharCols.ToString() == "[" || count > 0)
                                {
                                    count = 1;
                                    if (i % 27 == 0)
                                    {
                                        StartCharCols = 'A';
                                        if (i % 54 == 0)
                                        {
                                            dblStartCharCols++;
                                        }
                                    }
                                    string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                    ws.Cell(DataCell).Value = cols[i - 1];
                                    ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                    ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                    ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                    ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                    ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                    //ws.Cell(DataCell).Style.Font.Bold = true;
                                    if (i <= 5)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                    }
                                    else if (i > 5 && i <= 12)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.Yellow;
                                    }
                                    else if (i > 12 && i <= 19)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightGreen;
                                    }
                                    else if (i > 19 && i <= 22)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SkyBlue;
                                    }
                                    else if (i > 22)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                    }
                                    ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                }
                                else
                                {
                                    string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                    ws.Cell(DataCell).Value = cols[i - 1];
                                    ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                    ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                    ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                    ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                    ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                    //ws.Cell(DataCell).Style.Font.Bold = true;
                                    if (i <= 5)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                    }
                                    else if (i > 5 && i <= 12)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.Yellow;
                                    }
                                    else if (i > 12 && i <= 19)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightGreen;
                                    }
                                    else if (i > 19 && i <= 22)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SkyBlue;
                                    }
                                    else if (i > 22)
                                    {
                                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                    }
                                    ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                }

                            }
                        }
                        else
                        {
                            if (StartCharCols.ToString() == "[" || count > 0)
                            {
                                count = 1;
                                if (i % 27 == 0)
                                {
                                    StartCharCols = 'A';
                                    if (i % 54 == 0)
                                    {
                                        dblStartCharCols++;
                                    }
                                }
                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                //ws.Cell(DataCell).Style.Font.Bold = true;
                                if (i <= 5)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                }
                                else if (i > 5 && i <= 12)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.Yellow;
                                }
                                else if (i > 12 && i <= 19)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightGreen;
                                }
                                else if (i > 19 && i <= 22)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SkyBlue;
                                }
                                else if (i > 22)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                }
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            }
                            else
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                //ws.Cell(DataCell).Style.Font.Bold = true;
                                if (i <= 5)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                }
                                else if (i > 5 && i <= 12)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.Yellow;
                                }
                                else if (i > 12 && i <= 19)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.LightGreen;
                                }
                                else if (i > 19 && i <= 22)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SkyBlue;
                                }
                                else if (i > 22)
                                {
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.SandyBrown;
                                }
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            }
                        }
                        StartCharCols++;
                    }


                    int Ccout = 0;
                    char StartCharData = 'A';
                    int StartIndexData = 6;
                    dblStartCharCols = 'A';

                    char StartCharDataCol = char.MinValue;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (StartCharData.ToString() == "[" || Ccout > 0)
                            {
                                Ccout = 1;
                                if (j % 26 == 0)
                                {
                                    StartCharData = 'A';
                                    if (j % 52 == 0)
                                    {
                                        dblStartCharCols++;
                                    }
                                }
                                string DataCell = dblStartCharCols.ToString() + StartCharData.ToString() + StartIndexData;
                                string a = dt.Rows[i][j].ToString();
                                a = a.Replace("&nbsp;", " ");
                                a = a.Replace("&amp;", "&");

                                ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                if (StartCharData.ToString() == "E" || StartCharData.ToString() == "L" || StartCharData.ToString() == "S")
                                {
                                    ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                }
                                else
                                {
                                    ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                }

                                ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                //check if value is of integer type
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

                                //                            ws.Cell("AA" + StartIndexData.ToString()).FormulaA1 = "{G" + StartIndexData.ToString() + "H" + StartIndexData.ToString() + "}";

                                StartCharData++;
                            }
                            else
                            {
                                string DataCell = StartCharData.ToString() + StartIndexData;
                                string a = dt.Rows[i][j].ToString();
                                a = a.Replace("&nbsp;", " ");
                                a = a.Replace("&amp;", "&");

                                ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                if (StartCharData.ToString() == "E" || StartCharData.ToString() == "L" || StartCharData.ToString() == "S")
                                {
                                    ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                }
                                else
                                {
                                    ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                }

                                ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                //check if value is of integer type
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
                        }
                        StartCharData = 'A';
                        StartIndexData++;
                        Ccout = 0;
                    }

                    StartIndexData++;
                    ws.Cell("S" + StartIndexData.ToString()).SetValue("Total");
                    ws.Cell("T" + StartIndexData.ToString()).SetValue(HttpContext.Current.Session["TOPCapitalGain"]);
                    ws.Cell("Z" + StartIndexData.ToString()).SetValue(HttpContext.Current.Session["TOPNettIntIncome"]);
                    ws.Cell("AA" + StartIndexData.ToString()).SetValue(HttpContext.Current.Session["TOPTotalNettgain"]);

                    ws.Cell("S" + StartIndexData.ToString()).Style.Font.Bold = true;
                    ws.Cell("T" + StartIndexData.ToString()).Style.Font.Bold = true;
                    ws.Cell("Z" + StartIndexData.ToString()).Style.Font.Bold = true;
                    ws.Cell("AA" + StartIndexData.ToString()).Style.Font.Bold = true;
                }
            }

            /* Summary Report*/
            dt = (DataTable)HttpContext.Current.Session["Summary"];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    IXLWorksheet ws = wb.Worksheets.Add("Summary");
                    //dt.Columns.Remove("DealDate");

                    string Range = "";

                    string[] cols = new string[dt.Columns.Count];
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }
                    char StartCharCols = 'B';
                    int StartIndexCols = 4;

                    int count = 0;
                    char dblStartCharCols = 'B';

                    for (int i = 1; i <= cols.Length; i++)
                    {
                        if (i == cols.Length)
                        {
                            if (count > 0)
                            {
                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            }
                            else
                            {
                                if (StartCharCols.ToString() == "[" || count > 0)
                                {
                                    count = 1;
                                    if (i % 27 == 0)
                                    {
                                        StartCharCols = 'B';
                                        if (i % 54 == 0)
                                        {
                                            dblStartCharCols++;
                                        }
                                    }
                                    string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                    ws.Cell(DataCell).Value = cols[i - 1];
                                    ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                    ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                    ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thick;
                                    ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                    ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                                    ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                }
                                else
                                {
                                    string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                    ws.Cell(DataCell).Value = cols[i - 1];
                                    ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                    ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                    ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thick;
                                    ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                    ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                                    ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                }

                            }
                        }
                        else
                        {
                            if (StartCharCols.ToString() == "[" || count > 0)
                            {
                                count = 1;
                                if (i % 27 == 0)
                                {
                                    StartCharCols = 'B';
                                    if (i % 54 == 0)
                                    {
                                        dblStartCharCols++;
                                    }
                                }
                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            }
                            else
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            }
                        }
                        StartCharCols++;
                    }


                    int Ccout = 0;
                    char StartCharData = 'B';
                    int StartIndexData = 5;
                    dblStartCharCols = 'B';

                    char StartCharDataCol = char.MinValue;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (StartCharData.ToString() == "[" || Ccout > 0)
                            {
                                Ccout = 1;
                                if (j % 26 == 0)
                                {
                                    StartCharData = 'B';
                                    if (j % 52 == 0)
                                    {
                                        dblStartCharCols++;
                                    }
                                }
                                string DataCell = dblStartCharCols.ToString() + StartCharData.ToString() + StartIndexData;
                                string a = dt.Rows[i][j].ToString();
                                a = a.Replace("&nbsp;", " ");
                                a = a.Replace("&amp;", "&");

                                ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                                //check if value is of integer type
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

                                //                            ws.Cell("AA" + StartIndexData.ToString()).FormulaA1 = "{G" + StartIndexData.ToString() + "H" + StartIndexData.ToString() + "}";

                                StartCharData++;
                            }
                            else
                            {
                                string DataCell = StartCharData.ToString() + StartIndexData;
                                string a = dt.Rows[i][j].ToString();
                                a = a.Replace("&nbsp;", " ");
                                a = a.Replace("&amp;", "&");

                                ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thick;
                                ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                                //check if value is of integer type
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
                        }
                        StartCharData = 'B';
                        StartIndexData++;
                        Ccout = 0;
                    }
                }
            }

            //Code to save the file
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Debt Performance MIS.xlsx");

            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                HttpContext.Current.Response.Flush();
                //HttpContext.Current.Response.End();
                HttpContext.Current.Response.SuppressContent = true;
            }
        }
        catch (Exception ex)
        {

        }
    }



    public void ExportToExcel_MISExchangeWiseTransactionReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("Exchangewise Transaction Report");
                    //Add new sheet



                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;





                    DataTable dtExchangeId = new DataTable();
                    System.Data.DataView view = new System.Data.DataView(dt);
                    dtExchangeId = view.ToTable(true, "ExchangeId");
                    string ExchangeId = "";
                    string ExchangeName = "";
                    double SellProfit = 0.0/*H*/, PurchaseProfit = 0.0/*I*/, FaceValue = 0.0/*K*/;
                    double SettlementAmt = 0.0/*L*/, Profit = 0.0/*M*/, HOProfit = 0.0/*O*/;

                    double TotalSellProfit = 0.0/*I*/, TotalPurchaseProfit = 0.0/*J*/, TotalFaceValue = 0.0/*L*/;
                    double TotalSettlementAmt = 0.0/*M*/, TotalProfit = 0.0/*N*/, TotalHOProfit = 0.0/*O*/;

                    foreach (DataRow row in dtExchangeId.Rows)
                    {
                        SellProfit = 0;
                        PurchaseProfit = 0;
                        FaceValue = 0;
                        SettlementAmt = 0;
                        Profit = 0;
                        HOProfit = 0;
                        int Index = StartIndexData;

                        DataTable dtTarget = new DataTable();
                        dtTarget = dt.Clone();
                        ExchangeId = Convert.ToString(row["ExchangeId"]);

                        DataRow[] rowsToCopy;
                        rowsToCopy = dt.Select("ExchangeId='" + ExchangeId + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }
                        int dtTargetC = dtTarget.Rows.Count;
                        StartIndexData++;
                        if (dtTargetC > 0)
                        {
                            // filling header with formatting and colors
                            StartCharCols = 'A';
                            StartIndexCols = StartIndexData;
                            for (int i = 1; i <= cols.Length; i++)
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }

                            ExchangeName = Convert.ToString(dtTarget.Rows[0]["ExchangeName"]);
                            StartIndexData++;
                            for (int i = 0; i < dtTarget.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtTarget.Columns.Count; j++)
                                {
                                    string DataCell = StartCharData.ToString() + StartIndexData;
                                    //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                                    string a = dtTarget.Rows[i][j].ToString();
                                    a = a.Replace("&nbsp;", " ");
                                    a = a.Replace("&amp;", "&");
                                    //check if value is of integer type
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
                                    if (StartCharData.ToString() == "H")
                                    {
                                        if (a != "")
                                        {
                                            SellProfit += Convert.ToDouble(a);
                                            TotalSellProfit += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            SellProfit += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "I")
                                    {
                                        if (a != "")
                                        {
                                            PurchaseProfit += Convert.ToDouble(a);
                                            TotalPurchaseProfit += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            PurchaseProfit += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "K")
                                    {
                                        if (a != "")
                                        {
                                            FaceValue += Convert.ToDouble(a);
                                            TotalFaceValue += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            FaceValue += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "L")
                                    {
                                        if (a != "")
                                        {
                                            SettlementAmt += Convert.ToDouble(a);
                                            TotalSettlementAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            SettlementAmt += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "M")
                                    {
                                        if (a != "")
                                        {
                                            Profit += Convert.ToDouble(a);
                                            TotalProfit += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            Profit += 0;
                                        }
                                    }
                                    //if (StartCharData.ToString() == "O")
                                    //{
                                    //    if (a != "")
                                    //    {
                                    //        HOProfit += Convert.ToDouble(a);
                                    //        TotalHOProfit += Convert.ToDouble(a);
                                    //    }
                                    //    else
                                    //    {
                                    //        HOProfit += 0;
                                    //    }
                                    //}
                                    StartCharData++;
                                }
                                StartCharData = 'A';
                                StartIndexData++;
                            }

                            ws.Cell("A" + StartIndexData).SetValue("Total");
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            ws.Cell("A" + Index).SetValue("ExchangeName:   " + ExchangeName);
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;


                            string[] Array = { "H", "I", "K", "L", "M" };
                            string setval = "";
                            foreach (string value in Array)
                            {
                                if (value == "H")
                                {
                                    setval = Convert.ToString(SellProfit);
                                }
                                if (value == "I")
                                {
                                    setval = Convert.ToString(PurchaseProfit);
                                }
                                if (value == "K")
                                {
                                    setval = Convert.ToString(FaceValue);
                                }
                                if (value == "L")
                                {
                                    setval = Convert.ToString(SettlementAmt);
                                }
                                if (value == "M")
                                {
                                    setval = Convert.ToString(Profit);
                                }
                                //if (value == "O")
                                //{
                                //    setval = Convert.ToString(HOProfit);
                                //}
                                ws.Cell(value + StartIndexData).SetValue(setval);
                                ws.Cell(value + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                ws.Cell(value + StartIndexData).Style.Font.Bold = true;
                            }
                            StartIndexData++;
                            StartIndexData++;
                        }

                        //StartIndexData++;
                    }
                    //StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("H" + StartIndexData).SetValue(TotalSellProfit);
                    ws.Cell("H" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("H" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("I" + StartIndexData).SetValue(TotalPurchaseProfit);
                    ws.Cell("I" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("I" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("K" + StartIndexData).SetValue(TotalFaceValue);
                    ws.Cell("K" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("K" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("L" + StartIndexData).SetValue(TotalSettlementAmt);
                    ws.Cell("L" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("L" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("M" + StartIndexData).SetValue(TotalProfit);
                    ws.Cell("M" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("M" + StartIndexData).Style.Font.Bold = true;

                    //ws.Cell("O" + StartIndexData).SetValue(TotalHOProfit);
                    //ws.Cell("O" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //ws.Cell("N" + StartIndexData).Style.Font.Bold = true;


                    string Range = "";
                    Range = "A1:M1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("Exchangewise Transaction Report                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    ws.Column(6).Hide();
                    ws.Column(8).Hide();
                    ws.Column(9).Hide();

                    ws.Column(13).Hide();
                    ws.Column(14).Hide();
                    ws.Column(15).Hide();
                    ws.Column(16).Hide();
                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ExchangewiseReport" + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISBrokerWiseTransactionReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("Brokerwise Transaction Report");
                    //Add new sheet

                    int DeleteStartIndex = Convert.ToInt32(dt.Rows[0]["DeleteStartIndex"].ToString());
                    int DeleteEndIndex = Convert.ToInt32(dt.Rows[0]["DeleteEndIndex"].ToString());
                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "DealSlipNo", "TransType", "DealDate", "SettmentDate", "SecurityName", "CustomerName", "ISIN", "FaceValue", "NoOfBond", "Rate", "Principal Amount", "Interest Amount", "TotalConsideration", "StampDutyAmt", "TCSAmount", "TDSAmount", "SettlementAmt", "BrokerageAmount", "ModeOfPayment", "ModeOfDelivery", "YTM", "BrokerId", "Broker");

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;





                    DataTable dtUserId = new DataTable();
                    System.Data.DataView view = new System.Data.DataView(dt);
                    dtUserId = view.ToTable(true, "BrokerId");
                    string UserId = "";
                    string DealerName = "", DealerBranch = "";
                    //double SellProfit = 0.0/*H*/, PurchaseProfit = 0.0/*I*/, FaceValue = 0.0/*K*/;
                    //double SettlementAmt = 0.0/*L*/, Profit = 0.0/*M*/, HOProfit = 0.0/*O*/;

                    //double TotalSellProfit = 0.0/*I*/, TotalPurchaseProfit = 0.0/*J*/, TotalFaceValue = 0.0/*L*/;
                    //double TotalSettlementAmt = 0.0/*M*/, TotalProfit = 0.0/*N*/, TotalHOProfit = 0.0/*O*/;
                    double facevalue = 0.0, settamount = 0.0, princiAmt = 0.0, InterestAmt = 0.0, StampDutyAmt = 0.0, TotalConsideration = 0.0, TCSAmount = 0.0, TDSAmount = 0.0,Brokerage=0.0;
                    double totalfacevalue = 0.0, totalsettamount = 0.0, TotalprinciAmt = 0.0, TotalInterestAmt = 0.0, TOtalStampDutyAmt = 0.0, TTotalConsideration = 0.0, TotalTCSAmount = 0.0, TotalTDSAmount = 0.0,TotalBrokerage=0.0;

                    foreach (DataRow row in dtUserId.Rows)
                    {
                        facevalue = 0;
                        princiAmt = 0;
                        InterestAmt = 0;
                        settamount = 0;
                        TotalConsideration = 0;
                        StampDutyAmt = 0;
                        TCSAmount = 0;
                        TDSAmount = 0;
                        Brokerage = 0;
                        int Index = StartIndexData;

                        DataTable dtTarget = new DataTable();
                        dtTarget = dt.Clone();
                        UserId = Convert.ToString(row["BrokerId"]);

                        DataRow[] rowsToCopy;
                        rowsToCopy = dt.Select("BrokerId='" + UserId + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }
                        int dtTargetC = dtTarget.Rows.Count;
                        StartIndexData++;
                        if (dtTargetC > 0)
                        {
                            // filling header with formatting and colors
                            StartCharCols = 'A';
                            StartIndexCols = StartIndexData;
                            for (int i = 1; i <= cols.Length; i++)
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }

                            DealerName = Convert.ToString(dtTarget.Rows[0]["Broker"]);
                            //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                            StartIndexData++;
                            for (int i = 0; i < dtTarget.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtTarget.Columns.Count; j++)
                                {
                                    string DataCell = StartCharData.ToString() + StartIndexData;
                                    //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                                    string a = dtTarget.Rows[i][j].ToString();
                                    a = a.Replace("&nbsp;", " ");
                                    a = a.Replace("&amp;", "&");
                                    //check if value is of integer type
                                    int val = 0;
                                    double val2 = 0.0;
                                    DateTime dt2 = DateTime.Now;
                                    if (int.TryParse(a, out val))
                                    {
                                        //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                        ws.Cell(DataCell).Value = val;
                                        //ws.Cell(DataCell).SetValue(a);
                                        //ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val);
                                        ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    }
                                    else if (double.TryParse(a, out val2))
                                    {
                                        ws.Cell(DataCell).Value = val2;
                                        ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val2);
                                        ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
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


                                    if (StartCharData.ToString() == "H")
                                    {
                                        if (a != "")
                                        {
                                            facevalue += Convert.ToDouble(a);
                                            totalfacevalue += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            facevalue += 0;
                                        }
                                    }
                                    else if (StartCharData.ToString() == "K")
                                    {
                                        if (a != "")
                                        {
                                            princiAmt += Convert.ToDouble(a);
                                            TotalprinciAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            facevalue += 0;
                                        }
                                    }
                                    else if (StartCharData.ToString() == "L")
                                    {
                                        if (a != "")
                                        {
                                            InterestAmt += Convert.ToDouble(a);
                                            TotalInterestAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            facevalue += 0;
                                        }
                                    }
                                    else if (StartCharData.ToString() == "M")
                                    {
                                        if (a != "")
                                        {
                                            TotalConsideration += Convert.ToDouble(a);
                                            TTotalConsideration += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            TotalConsideration += 0;
                                        }
                                    }
                                    else if (StartCharData.ToString() == "N")
                                    {
                                        if (a != "")
                                        {
                                            StampDutyAmt += Convert.ToDouble(a);
                                            TOtalStampDutyAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            StampDutyAmt += 0;
                                        }
                                    }

                                    else if (StartCharData.ToString() == "O")
                                    {
                                        if (a != "")
                                        {
                                            TCSAmount += Convert.ToDouble(a);
                                            TotalTCSAmount += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            TCSAmount += 0;
                                        }
                                    }

                                    else if (StartCharData.ToString() == "P")
                                    {
                                        if (a != "")
                                        {
                                            TDSAmount += Convert.ToDouble(a);
                                            TotalTDSAmount += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            TDSAmount += 0;
                                        }
                                    }
                                    else if (StartCharData.ToString() == "Q")
                                    {
                                        if (a != "")
                                        {
                                            settamount += Convert.ToDouble(a);
                                            totalsettamount += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            settamount += 0;
                                        }
                                    }
                                    else if (StartCharData.ToString() == "R")
                                    {
                                        if (a != "")
                                        {
                                            Brokerage  += Convert.ToDouble(a);
                                            TotalBrokerage  += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            Brokerage += 0;
                                        }
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

                            ws.Cell("A" + StartIndexData).SetValue("Total");
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            ws.Cell("A" + Index).SetValue("Broker Name:   " + DealerName);
                            ws.Cell("A" + Index).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + Index).Style.Font.Bold = true;

                            string[] Array = { "H", "K", "L", "M", "N", "O", "P", "Q","R" };
                            string setval = "";
                            foreach (string value in Array)
                            {
                                if (value == "H")
                                {
                                    setval = Convert.ToString(facevalue);
                                }
                                if (value == "K")
                                {
                                    setval = Convert.ToString(princiAmt);
                                }
                                if (value == "L")
                                {
                                    setval = Convert.ToString(InterestAmt);
                                }
                                if (value == "M")
                                {
                                    setval = Convert.ToString(TotalConsideration);
                                }

                                if (value == "N")
                                {
                                    setval = Convert.ToString(StampDutyAmt);
                                }

                                if (value == "O")
                                {
                                    setval = Convert.ToString(TCSAmount);
                                }

                                if (value == "P")
                                {
                                    setval = Convert.ToString(TDSAmount);
                                }

                                if (value == "Q")
                                {
                                    setval = Convert.ToString(settamount);
                                }
                                if (value == "R")
                                {
                                    setval = Convert.ToString(Brokerage);
                                }

                                ws.Cell(value + StartIndexData).SetValue(setval);
                                ws.Cell(value + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                ws.Cell(value + StartIndexData).Style.Font.Bold = true;
                            }
                            StartIndexData++;
                            StartIndexData++;
                        }

                        //StartIndexData++;
                    }
                    //StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;



                    ws.Cell("H" + StartIndexData).SetValue(totalfacevalue);
                    ws.Cell("H" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("H" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(totalfacevalue);
                    ws.Cell("H" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("K" + StartIndexData).SetValue(TotalprinciAmt);
                    ws.Cell("K" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("K" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalprinciAmt);
                    ws.Cell("K" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("L" + StartIndexData).SetValue(TotalInterestAmt);
                    ws.Cell("L" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("L" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalInterestAmt);
                    ws.Cell("L" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("M" + StartIndexData).SetValue(TTotalConsideration);
                    ws.Cell("M" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("M" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TTotalConsideration);
                    ws.Cell("M" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("N" + StartIndexData).SetValue(TOtalStampDutyAmt);
                    ws.Cell("N" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("N" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TOtalStampDutyAmt);
                    ws.Cell("N" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("O" + StartIndexData).SetValue(TotalTCSAmount);
                    ws.Cell("O" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("O" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalTCSAmount);
                    ws.Cell("O" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("P" + StartIndexData).SetValue(TotalTDSAmount);
                    ws.Cell("P" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("P" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalTDSAmount);
                    ws.Cell("P" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("Q" + StartIndexData).SetValue(totalsettamount);
                    ws.Cell("Q" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("Q" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(totalsettamount);
                    ws.Cell("Q" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("R" + StartIndexData).SetValue(TotalBrokerage );
                    ws.Cell("R" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("R" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalBrokerage);
                    ws.Cell("R" + StartIndexData).Style.Font.Bold = true;


                    string Range = "";
                    Range = "A1:M1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("Brokerwise Transaction Report                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));

                    for (int I = DeleteStartIndex; I < DeleteEndIndex; I++)
                    {
                        ws.Column(DeleteStartIndex).Delete();
                    }

                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Brokerwisereport" + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISStaffWiseTransactionReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("Staffwise Transaction Report");
                    //Add new sheet

                    int DeleteStartIndex = Convert.ToInt32(dt.Rows[0]["DeleteStartIndex"].ToString());
                    int DeleteEndIndex = Convert.ToInt32(dt.Rows[0]["DeleteEndIndex"].ToString());
                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "DealSlipNo", "TransType", "DealDate", "SettmentDate", "SecurityName", "CustomerName", "ISIN", "FaceValue", "NoOfBond", "Rate", "YTM", "Principal Amount", "Interest Amount", "TotalConsideration", "StampDutyAmt", "TCSAmount", "TDSAmount", "SettlementAmt", "ModeOfPayment", "ModeOfDelivery",  "Broker", "BrokerageAmount",  "CutOffRate", "CutOffYield", "UserId", "NameOfUser");

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;
                    DataTable dtUserId = new DataTable();
                    System.Data.DataView view = new System.Data.DataView(dt);
                    dtUserId = view.ToTable(true, "UserId");
                    string UserId = "";
                    string DealerName = "", DealerBranch = "";
                    //double SellProfit = 0.0/*H*/, PurchaseProfit = 0.0/*I*/, FaceValue = 0.0/*K*/;
                    //double SettlementAmt = 0.0/*L*/, Profit = 0.0/*M*/, HOProfit = 0.0/*O*/;

                    //double TotalSellProfit = 0.0/*I*/, TotalPurchaseProfit = 0.0/*J*/, TotalFaceValue = 0.0/*L*/;
                    //double TotalSettlementAmt = 0.0/*M*/, TotalProfit = 0.0/*N*/, TotalHOProfit = 0.0/*O*/;
                    double facevalue = 0.0, settamount = 0.0, princiAmt = 0.0, InterestAmt = 0.0, StampDutyAmt = 0.0, TotalConsideration = 0.0, TCSAmount = 0.0, TDSAmount = 0.0,Brokerage=0.0;
                    double totalfacevalue = 0.0, totalsettamount = 0.0, TotalprinciAmt = 0.0, TotalInterestAmt = 0.0, TOtalStampDutyAmt = 0.0, TTotalConsideration = 0.0, TotalTCSAmount = 0.0, TotalTDSAmount = 0.0, totalBrokerage=0.0;

                    foreach (DataRow row in dtUserId.Rows)
                    {
                        facevalue = 0;
                        princiAmt = 0;
                        InterestAmt = 0;
                        settamount = 0;
                        TotalConsideration = 0;
                        StampDutyAmt = 0;
                        TCSAmount = 0;
                        TDSAmount = 0;
                        Brokerage = 0;
                        int Index = StartIndexData;

                        DataTable dtTarget = new DataTable();
                        dtTarget = dt.Clone();
                        UserId = Convert.ToString(row["UserId"]);

                        DataRow[] rowsToCopy;
                        rowsToCopy = dt.Select("UserId='" + UserId + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }
                        int dtTargetC = dtTarget.Rows.Count;
                        StartIndexData++;
                        if (dtTargetC > 0)
                        {
                            // filling header with formatting and colors
                            StartCharCols = 'A';
                            StartIndexCols = StartIndexData;
                            for (int i = 1; i <= cols.Length; i++)
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }

                            DealerName = Convert.ToString(dtTarget.Rows[0]["NameOfUser"]);
                            //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                            StartIndexData++;
                            for (int i = 0; i < dtTarget.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtTarget.Columns.Count; j++)
                                {
                                    string DataCell = StartCharData.ToString() + StartIndexData;
                                    //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                                    string a = dtTarget.Rows[i][j].ToString();
                                    a = a.Replace("&nbsp;", " ");
                                    a = a.Replace("&amp;", "&");
                                    //check if value is of integer type
                                    int val = 0;
                                    double val2 = 0.0;
                                    DateTime dt2 = DateTime.Now;
                                    if (int.TryParse(a, out val))
                                    {
                                        //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                        ws.Cell(DataCell).Value = val;
                                        //ws.Cell(DataCell).SetValue(a);
                                        //ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val);
                                        ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    }
                                    else if (double.TryParse(a, out val2))
                                    {
                                        ws.Cell(DataCell).Value = val2;
                                        ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val2);
                                        ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
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


                                    if (StartCharData.ToString() == "H")
                                    {
                                        if (a != "")
                                        {
                                            facevalue += Convert.ToDouble(a);
                                            totalfacevalue += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            facevalue += 0;
                                        }
                                    }
                                    else if (StartCharData.ToString() == "L")
                                    {
                                        if (a != "")
                                        {
                                            princiAmt += Convert.ToDouble(a);
                                            TotalprinciAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            facevalue += 0;
                                        }
                                    }
                                    else if (StartCharData.ToString() == "M")
                                    {
                                        if (a != "")
                                        {
                                            InterestAmt += Convert.ToDouble(a);
                                            TotalInterestAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            facevalue += 0;
                                        }
                                    }
                                    else if (StartCharData.ToString() == "N")
                                    {
                                        if (a != "")
                                        {
                                            TotalConsideration += Convert.ToDouble(a);
                                            TTotalConsideration += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            TotalConsideration += 0;
                                        }
                                    }
                                    else if (StartCharData.ToString() == "O")
                                    {
                                        if (a != "")
                                        {
                                            StampDutyAmt += Convert.ToDouble(a);
                                            TOtalStampDutyAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            StampDutyAmt += 0;
                                        }
                                    }

                                    else if (StartCharData.ToString() == "P")
                                    {
                                        if (a != "")
                                        {
                                            TCSAmount += Convert.ToDouble(a);
                                            TotalTCSAmount += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            TCSAmount += 0;
                                        }
                                    }

                                    else if (StartCharData.ToString() == "Q")
                                    {
                                        if (a != "")
                                        {
                                            TDSAmount += Convert.ToDouble(a);
                                            TotalTDSAmount += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            TDSAmount += 0;
                                        }
                                    }
                                    else if (StartCharData.ToString() == "R")
                                    {
                                        if (a != "")
                                        {
                                            settamount += Convert.ToDouble(a);
                                            totalsettamount += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            settamount += 0;
                                        }
                                    }
                                    else if (StartCharData.ToString() == "V")
                                    {
                                        if (a != "")
                                        {
                                            Brokerage  += Convert.ToDouble(a);
                                            totalBrokerage  += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            Brokerage += 0;
                                        }
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

                            ws.Cell("A" + StartIndexData).SetValue("Total");
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            ws.Cell("A" + Index).SetValue("Dealer Name:   " + DealerName);
                            ws.Cell("A" + Index).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + Index).Style.Font.Bold = true;

                            string[] Array = { "H", "L", "M", "N", "O", "P", "Q", "R","V" };
                            string setval = "";
                            foreach (string value in Array)
                            {
                                if (value == "H")
                                {
                                    setval = Convert.ToString(facevalue);
                                }
                                if (value == "L")
                                {
                                    setval = Convert.ToString(princiAmt);
                                }
                                if (value == "M")
                                {
                                    setval = Convert.ToString(InterestAmt);
                                }
                                if (value == "N")
                                {
                                    setval = Convert.ToString(TotalConsideration);
                                }

                                if (value == "O")
                                {
                                    setval = Convert.ToString(StampDutyAmt);
                                }

                                if (value == "P")
                                {
                                    setval = Convert.ToString(TCSAmount);
                                }

                                if (value == "Q")
                                {
                                    setval = Convert.ToString(TDSAmount);
                                }

                                if (value == "R")
                                {
                                    setval = Convert.ToString(settamount);
                                }
                                if (value == "V")
                                {
                                    setval = Convert.ToString(Brokerage);
                                }

                                ws.Cell(value + StartIndexData).SetValue(setval);
                                ws.Cell(value + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                ws.Cell(value + StartIndexData).Style.Font.Bold = true;
                            }
                            StartIndexData++;
                            StartIndexData++;
                        }

                        //StartIndexData++;
                    }
                    //StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;



                    ws.Cell("H" + StartIndexData).SetValue(totalfacevalue);
                    ws.Cell("H" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("H" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(totalfacevalue);
                    ws.Cell("H" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("L" + StartIndexData).SetValue(TotalprinciAmt);
                    ws.Cell("L" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("L" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalprinciAmt);
                    ws.Cell("L" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("M" + StartIndexData).SetValue(TotalInterestAmt);
                    ws.Cell("M" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("M" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalInterestAmt);
                    ws.Cell("M" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("N" + StartIndexData).SetValue(TTotalConsideration);
                    ws.Cell("N" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("N" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TTotalConsideration);
                    ws.Cell("N" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("O" + StartIndexData).SetValue(TOtalStampDutyAmt);
                    ws.Cell("O" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("O" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TOtalStampDutyAmt);
                    ws.Cell("O" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("P" + StartIndexData).SetValue(TotalTCSAmount);
                    ws.Cell("P" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("P" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalTCSAmount);
                    ws.Cell("P" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("Q" + StartIndexData).SetValue(TotalTDSAmount);
                    ws.Cell("Q" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("Q" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalTDSAmount);
                    ws.Cell("Q" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("R" + StartIndexData).SetValue(totalsettamount);
                    ws.Cell("R" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("R" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(totalsettamount);
                    ws.Cell("R" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("V" + StartIndexData).SetValue(totalBrokerage);
                    ws.Cell("V" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("V" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(totalsettamount);
                    ws.Cell("V" + StartIndexData).Style.Font.Bold = true;


                    string Range = "";
                    Range = "A1:M1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("Staffwise Transaction Report                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));

                    for (int I = DeleteStartIndex; I < DeleteEndIndex; I++)
                    {
                        ws.Column(DeleteStartIndex).Delete();
                    }

                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Staffwisereport" + strdtTime + ".xlsx");
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
    public void ExportToExcel_MISClientWiseTransactionReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("Clientwise Transaction Report");
                    //Add new sheet

                    int DeleteStartIndex = Convert.ToInt32(dt.Rows[0]["DeleteStartIndex"].ToString());
                    int DeleteEndIndex = Convert.ToInt32(dt.Rows[0]["DeleteEndIndex"].ToString());
                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "DealSlipNo", "TransType", "DealDate", "SettmentDate", "SecurityName", "ISIN", "FaceValue", "NoOfBond", "Rate", "Principal Amount", "Interest Amount", "TotalConsideration", "StampDutyAmt", "TCSAmount", "TDSAmount", "SettlementAmt", "ModeOfPayment", "ModeOfDelivery", "YTM", "CustomerId", "CustomerName");

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;





                    DataTable dtCustomerId = new DataTable();
                    System.Data.DataView view = new System.Data.DataView(dt);
                    dtCustomerId = view.ToTable(true, "CustomerId");
                    string CustomerId = "";
                    string CustomerName = "", DealerBranch = "";
                    //double SellProfit = 0.0/*H*/, PurchaseProfit = 0.0/*I*/, FaceValue = 0.0/*K*/;
                    //double SettlementAmt = 0.0/*L*/, Profit = 0.0/*M*/, HOProfit = 0.0/*O*/;

                    //double TotalSellProfit = 0.0/*I*/, TotalPurchaseProfit = 0.0/*J*/, TotalFaceValue = 0.0/*L*/;
                    //double TotalSettlementAmt = 0.0/*M*/, TotalProfit = 0.0/*N*/, TotalHOProfit = 0.0/*O*/;
                    double facevalue = 0.0, settamount = 0.0, princiAmt = 0.0, InterestAmt = 0.0, StampDutyAmt = 0.0, TotalConsideration = 0.0, TCSAmount = 0.0, TDSAmount = 0.0;
                    double totalfacevalue = 0.0, totalsettamount = 0.0, TotalprinciAmt = 0.0, TotalInterestAmt = 0.0, TOtalStampDutyAmt = 0.0, TTotalConsideration = 0.0, TotalTCSAmount = 0.0, TotalTDSAmount = 0.0;

                    foreach (DataRow row in dtCustomerId.Rows)
                    {
                        facevalue = 0;
                        princiAmt = 0;
                        InterestAmt = 0;
                        settamount = 0;
                        TotalConsideration = 0;
                        StampDutyAmt = 0;
                        TCSAmount = 0;
                        TDSAmount = 0;
                        int Index = StartIndexData;

                        DataTable dtTarget = new DataTable();
                        dtTarget = dt.Clone();
                        CustomerId = Convert.ToString(row["CustomerId"]);

                        DataRow[] rowsToCopy;
                        rowsToCopy = dt.Select("CustomerId='" + CustomerId + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }
                        int dtTargetC = dtTarget.Rows.Count;
                        StartIndexData++;
                        if (dtTargetC > 0)
                        {
                            // filling header with formatting and colors
                            StartCharCols = 'A';
                            StartIndexCols = StartIndexData;
                            for (int i = 1; i <= cols.Length; i++)
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }

                            CustomerName = Convert.ToString(dtTarget.Rows[0]["CustomerName"]);
                            //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                            StartIndexData++;
                            for (int i = 0; i < dtTarget.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtTarget.Columns.Count; j++)
                                {
                                    string DataCell = StartCharData.ToString() + StartIndexData;
                                    //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                                    string a = dtTarget.Rows[i][j].ToString();
                                    a = a.Replace("&nbsp;", " ");
                                    a = a.Replace("&amp;", "&");
                                    int val = 0;
                                    double val2 = 0.0;
                                    DateTime dt2 = DateTime.Now;
                                    if (int.TryParse(a, out val))
                                    {
                                        //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                        ws.Cell(DataCell).Value = val;
                                        //ws.Cell(DataCell).SetValue(a);
                                        //ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val);
                                        ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    }
                                    else if (double.TryParse(a, out val2))
                                    {
                                        ws.Cell(DataCell).Value = val2;
                                        ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val2);
                                        ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
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


                                    if (StartCharData.ToString() == "G")
                                    {
                                        if (a != "")
                                        {
                                            facevalue += Convert.ToDouble(a);
                                            totalfacevalue += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            facevalue += 0;
                                        }
                                    }
                                    if ((StartCharData.ToString() == "I")|| (StartCharData.ToString() == "S"))
                                    {
                                        if (a != "")
                                        {
                                            ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.0000";

                                        }
                                        else
                                        {

                                        }
                                    }
                                    if (StartCharData.ToString() == "J")
                                    {
                                        if (a != "")
                                        {
                                            princiAmt += Convert.ToDouble(a);
                                            TotalprinciAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            facevalue += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "K")
                                    {
                                        if (a != "")
                                        {
                                            InterestAmt += Convert.ToDouble(a);
                                            TotalInterestAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            facevalue += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "L")
                                    {
                                        if (a != "")
                                        {
                                            TotalConsideration += Convert.ToDouble(a);
                                            TTotalConsideration += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            TotalConsideration += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "M")
                                    {
                                        if (a != "")
                                        {
                                            StampDutyAmt += Convert.ToDouble(a);
                                            TOtalStampDutyAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            StampDutyAmt += 0;
                                        }
                                    }

                                    if (StartCharData.ToString() == "N")
                                    {
                                        if (a != "")
                                        {
                                            TCSAmount += Convert.ToDouble(a);
                                            TotalTCSAmount += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            TCSAmount += 0;
                                        }
                                    }

                                    if (StartCharData.ToString() == "O")
                                    {
                                        if (a != "")
                                        {
                                            TDSAmount += Convert.ToDouble(a);
                                            TotalTDSAmount += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            TDSAmount += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "P")
                                    {
                                        if (a != "")
                                        {
                                            settamount += Convert.ToDouble(a);
                                            totalsettamount += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            settamount += 0;
                                        }
                                    }
                                    
                                        StartCharData++;
                                }
                                StartCharData = 'A';
                                StartIndexData++;
                            }
                            StartIndexData++;
                            ws.Cell("A" + StartIndexData).SetValue("Total");
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            ws.Cell("A" + Index).SetValue("Client Name:   " + CustomerName);
                            ws.Cell("A" + Index).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + Index).Style.Font.Bold = true;

                            string[] Array = { "G", "J", "K", "L", "M", "N", "O", "P" };
                            string setval = "";
                            foreach (string value in Array)
                            {
                                if (value == "G")
                                {
                                    setval = Convert.ToString(facevalue);
                                    
                                }
                                if (value == "J")
                                {
                                    setval = Convert.ToString(princiAmt);
                                }
                                if (value == "K")
                                {
                                    setval = Convert.ToString(InterestAmt);
                                }
                                if (value == "L")
                                {
                                    setval = Convert.ToString(TotalConsideration);
                                }

                                if (value == "M")
                                {
                                    setval = Convert.ToString(StampDutyAmt);
                                }

                                if (value == "N")
                                {
                                    setval = Convert.ToString(TCSAmount);
                                }

                                if (value == "O")
                                {
                                    setval = Convert.ToString(TDSAmount);
                                }

                                if (value == "P")
                                {
                                    setval = Convert.ToString(settamount);
                                }

                                ws.Cell(value + StartIndexData).SetValue(setval);
                                ws.Cell(value + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(Convert .ToDouble (setval));
                                ws.Cell(value + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                ws.Cell(value + StartIndexData).Style.Font.Bold = true;
                            }
                            StartIndexData++;
                            StartIndexData++;
                        }

                        //StartIndexData++;
                    }
                    //StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;



                    ws.Cell("G" + StartIndexData).SetValue(totalfacevalue);
                    ws.Cell("G" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("G" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(totalfacevalue);
                    ws.Cell("G" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("J" + StartIndexData).SetValue(TotalprinciAmt);
                    ws.Cell("J" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("J" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalprinciAmt);
                    ws.Cell("J" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("K" + StartIndexData).SetValue(TotalInterestAmt);
                    ws.Cell("K" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("K" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalInterestAmt);
                    ws.Cell("K" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("L" + StartIndexData).SetValue(TTotalConsideration);
                    ws.Cell("L" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("L" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TTotalConsideration);
                    ws.Cell("L" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("M" + StartIndexData).SetValue(TOtalStampDutyAmt);
                    ws.Cell("M" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("M" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TOtalStampDutyAmt);
                    ws.Cell("M" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("N" + StartIndexData).SetValue(TotalTCSAmount);
                    ws.Cell("N" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("N" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalTCSAmount);
                    ws.Cell("N" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("O" + StartIndexData).SetValue(TotalTDSAmount);
                    ws.Cell("O" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("O" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalTDSAmount);
                    ws.Cell("O" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("P" + StartIndexData).SetValue(totalsettamount);
                    ws.Cell("P" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("P" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(totalsettamount);
                    ws.Cell("P" + StartIndexData).Style.Font.Bold = true;


                    string Range = "";
                    Range = "A1:M1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("Clientwise Transaction Report                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));

                    for (int I = DeleteStartIndex; I < DeleteEndIndex; I++)
                    {
                        ws.Column(DeleteStartIndex).Delete();
                    }

                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ClientwiseReport" + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISSecurityWiseTransactionReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("Securitywise Transaction Report");
                    //Add new sheet

                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "DealSlipNo", "TransType", "DealDate", "SettmentDate", "CustomerName", "FaceValue", "Rate", "FinalAmount", "SecurityId", "SecurityName");

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;

                    DataTable dtSecurityId = new DataTable();
                    System.Data.DataView view = new System.Data.DataView(dt);
                    dtSecurityId = view.ToTable(true, "SecurityId");
                    string SecurityId = "";
                    string SecurityName = "", DealerBranch = "";
                    double SellProfit = 0.0/*H*/, PurchaseProfit = 0.0/*I*/, FaceValue = 0.0/*K*/;
                    double SettlementAmt = 0.0/*L*/, Profit = 0.0/*M*/, HOProfit = 0.0/*O*/;

                    double TotalSellProfit = 0.0/*I*/, TotalPurchaseProfit = 0.0/*J*/, TotalFaceValue = 0.0/*L*/;
                    double TotalSettlementAmt = 0.0/*M*/, TotalProfit = 0.0/*N*/, TotalHOProfit = 0.0/*O*/;

                    foreach (DataRow row in dtSecurityId.Rows)
                    {
                        SellProfit = 0;
                        PurchaseProfit = 0;
                        FaceValue = 0;
                        SettlementAmt = 0;
                        Profit = 0;
                        HOProfit = 0;
                        int Index = StartIndexData;

                        DataTable dtTarget = new DataTable();
                        dtTarget = dt.Clone();
                        SecurityId = Convert.ToString(row["SecurityId"]);

                        DataRow[] rowsToCopy;
                        rowsToCopy = dt.Select("SecurityId='" + SecurityId + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }
                        int dtTargetC = dtTarget.Rows.Count;
                        StartIndexData++;
                        if (dtTargetC > 0)
                        {
                            // filling header with formatting and colors
                            StartCharCols = 'A';
                            StartIndexCols = StartIndexData;
                            for (int i = 1; i <= cols.Length; i++)
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }

                            SecurityName = Convert.ToString(dtTarget.Rows[0]["SecurityName"]);
                            //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                            StartIndexData++;
                            for (int i = 0; i < dtTarget.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtTarget.Columns.Count; j++)
                                {
                                    string DataCell = StartCharData.ToString() + StartIndexData;
                                    //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                                    string a = dtTarget.Rows[i][j].ToString();
                                    a = a.Replace("&nbsp;", " ");
                                    a = a.Replace("&amp;", "&");
                                    //check if value is of integer type
                                    int val = 0;
                                    double val2 = 0.0;
                                    DateTime dt2 = DateTime.Now;
                                    if (int.TryParse(a, out val))
                                    {
                                        //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                        ws.Cell(DataCell).Value = val;
                                        //ws.Cell(DataCell).SetValue(a);
                                        //ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val);
                                        ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    }
                                    else if (double.TryParse(a, out val2))
                                    {
                                        ws.Cell(DataCell).Value = val2;
                                        ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val2);
                                        ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
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



                                    if (StartCharData.ToString() == "F")
                                    {
                                        if (a != "")
                                        {
                                            FaceValue += Convert.ToDouble(a);
                                            TotalFaceValue += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            FaceValue += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "H")
                                    {
                                        if (a != "")
                                        {
                                            SettlementAmt += Convert.ToDouble(a);
                                            TotalSettlementAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            SettlementAmt += 0;
                                        }
                                    }
                                    StartCharData++;
                                }
                                StartCharData = 'A';
                                StartIndexData++;
                            }

                            ws.Cell("A" + StartIndexData).SetValue("Total");
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            ws.Cell("A" + Index).SetValue("SecurityName:   " + SecurityName);
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            string[] Array = { "F", "H" };
                            string setval = "";
                            foreach (string value in Array)
                            {

                                if (value == "F")
                                {
                                    setval = Convert.ToString(FaceValue);
                                }
                                if (value == "H")
                                {
                                    setval = Convert.ToString(SettlementAmt);
                                }

                                ws.Cell(value + StartIndexData).SetValue(setval);
                                ws.Cell(value + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                ws.Cell(value + StartIndexData).Style.Font.Bold = true;
                            }
                            StartIndexData++;
                            StartIndexData++;
                        }

                        //StartIndexData++;
                    }
                    //StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("F" + StartIndexData).SetValue(TotalFaceValue);
                    ws.Cell("F" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("F" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("H" + StartIndexData).SetValue(TotalSettlementAmt);
                    ws.Cell("H" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("H" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:M1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("Securitywise Transaction Report                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    ws.Column(9).Delete();
                    ws.Column(9).Delete();
                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=SecuritywiseReport" + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISCategoryWiseTransactionReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("Categorywise Transaction Report");
                    //Add new sheet

                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "DealSlipNo", "TransType", "DealDate", "SettmentDate", "SecurityName", "CustomerName", "Rate", "FaceValue", "Quantity", "FinalAmount", "CategoryName", "CustomerTypeid");

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;

                    DataTable dtUserId = new DataTable();
                    System.Data.DataView view = new System.Data.DataView(dt);
                    dtUserId = view.ToTable(true, "CustomerTypeid");
                    string UserId = "";
                    string Nameofuser = "", DealerBranch = "";
                    double SellProfit = 0.0/*H*/, PurchaseProfit = 0.0/*I*/, FaceValue = 0.0/*K*/;
                    double SettlementAmt = 0.0/*L*/, Profit = 0.0/*M*/, HOProfit = 0.0/*O*/;

                    double TotalSellProfit = 0.0/*I*/, TotalPurchaseProfit = 0.0/*J*/, TotalFaceValue = 0.0/*L*/;
                    double TotalSettlementAmt = 0.0/*M*/, TotalProfit = 0.0/*N*/, TotalHOProfit = 0.0/*O*/;

                    foreach (DataRow row in dtUserId.Rows)
                    {
                        SellProfit = 0;
                        PurchaseProfit = 0;
                        FaceValue = 0;
                        SettlementAmt = 0;
                        Profit = 0;
                        HOProfit = 0;
                        int Index = StartIndexData;

                        DataTable dtTarget = new DataTable();
                        dtTarget = dt.Clone();
                        UserId = Convert.ToString(row["CustomerTypeid"]);

                        DataRow[] rowsToCopy;
                        rowsToCopy = dt.Select("CustomerTypeid='" + UserId + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }
                        int dtTargetC = dtTarget.Rows.Count;
                        StartIndexData++;
                        if (dtTargetC > 0)
                        {
                            // filling header with formatting and colors
                            StartCharCols = 'A';
                            StartIndexCols = StartIndexData;
                            for (int i = 1; i <= cols.Length; i++)
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }

                            Nameofuser = Convert.ToString(dtTarget.Rows[0]["CategoryName"]);
                            //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                            StartIndexData++;
                            for (int i = 0; i < dtTarget.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtTarget.Columns.Count; j++)
                                {
                                    string DataCell = StartCharData.ToString() + StartIndexData;
                                    //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                                    string a = dtTarget.Rows[i][j].ToString();
                                    a = a.Replace("&nbsp;", " ");
                                    a = a.Replace("&amp;", "&");
                                    //check if value is of integer type
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


                                    if (StartCharData.ToString() == "H")
                                    {
                                        if (a != "")
                                        {
                                            FaceValue += Convert.ToDouble(a);
                                            TotalFaceValue += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            FaceValue += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "J")
                                    {
                                        if (a != "")
                                        {
                                            SettlementAmt += Convert.ToDouble(a);
                                            TotalSettlementAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            SettlementAmt += 0;
                                        }
                                    }
                                    StartCharData++;
                                }
                                StartCharData = 'A';
                                StartIndexData++;
                            }

                            ws.Cell("A" + StartIndexData).SetValue("Total");
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            ws.Cell("A" + Index).SetValue("Category:   " + Nameofuser);
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            string[] Array = { "H", "J" };
                            string setval = "";
                            foreach (string value in Array)
                            {

                                if (value == "H")
                                {
                                    setval = Convert.ToString(FaceValue);
                                }
                                if (value == "J")
                                {
                                    setval = Convert.ToString(SettlementAmt);
                                }

                                ws.Cell(value + StartIndexData).SetValue(setval);
                                ws.Cell(value + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                ws.Cell(value + StartIndexData).Style.Font.Bold = true;
                            }
                            StartIndexData++;
                            StartIndexData++;
                        }

                        //StartIndexData++;
                    }
                    //StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("H" + StartIndexData).SetValue(TotalFaceValue);
                    ws.Cell("H" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("H" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("J" + StartIndexData).SetValue(TotalSettlementAmt);
                    ws.Cell("J" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("j" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:M1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("Categorywise Transaction Report                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    ws.Column(11).Delete();
                    ws.Column(12).Delete();
                    ws.Column(13).Delete();
                    ws.Column(10).Delete();
                    ws.Column(10).Delete();
                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=CategorywiseTransactionReport" + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISInFlowOutFlowPaymentReport(DataTable dt, string Flag, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    if (Flag == "S")
                    {
                        Flag = "Inflow";
                    }
                    else
                    {
                        Flag = "Outflow";
                    }
                    ws = wb.Worksheets.Add(Flag + "PaymentReport");
                    //Add new sheet

                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "DealSlipNo", "SecurityName", "CustomerName", "FaceValue", "SettlementAmt", "ModeOFPayment", "BankName", "SettmentDate");

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;

                    DataTable dtSettmentDate = new DataTable();
                    System.Data.DataView view = new System.Data.DataView(dt);
                    dtSettmentDate = view.ToTable(true, "SettmentDate");
                    string settdate = "";
                    string SettmentDate = "", DealerBranch = "";
                    double SellProfit = 0.0/*H*/, PurchaseProfit = 0.0/*I*/, FaceValue = 0.0/*K*/;
                    double SettlementAmt = 0.0/*L*/, Profit = 0.0/*M*/, HOProfit = 0.0/*O*/;

                    double TotalSellProfit = 0.0/*I*/, TotalPurchaseProfit = 0.0/*J*/, TotalFaceValue = 0.0/*L*/;
                    double TotalSettlementAmt = 0.0/*M*/, TotalProfit = 0.0/*N*/, TotalHOProfit = 0.0/*O*/;

                    foreach (DataRow row in dtSettmentDate.Rows)
                    {
                        SellProfit = 0;
                        PurchaseProfit = 0;
                        FaceValue = 0;
                        SettlementAmt = 0;
                        Profit = 0;
                        HOProfit = 0;
                        int Index = StartIndexData;

                        DataTable dtTarget = new DataTable();
                        dtTarget = dt.Clone();
                        settdate = Convert.ToString(row["SettmentDate"]);

                        DataRow[] rowsToCopy;
                        rowsToCopy = dt.Select("SettmentDate='" + settdate + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }
                        int dtTargetC = dtTarget.Rows.Count;
                        StartIndexData++;
                        if (dtTargetC > 0)
                        {
                            // filling header with formatting and colors
                            StartCharCols = 'A';
                            StartIndexCols = StartIndexData;
                            for (int i = 1; i <= cols.Length; i++)
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }

                            SettmentDate = Convert.ToString(dtTarget.Rows[0]["SettmentDate"]);
                            //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                            StartIndexData++;
                            for (int i = 0; i < dtTarget.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtTarget.Columns.Count; j++)
                                {
                                    string DataCell = StartCharData.ToString() + StartIndexData;
                                    //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                                    string a = dtTarget.Rows[i][j].ToString();
                                    a = a.Replace("&nbsp;", " ");
                                    a = a.Replace("&amp;", "&");
                                    //check if value is of integer type
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

                                    if (StartCharData.ToString() == "D")
                                    {
                                        if (a != "")
                                        {
                                            FaceValue += Convert.ToDouble(a);
                                            TotalFaceValue += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            FaceValue += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "E")
                                    {
                                        if (a != "")
                                        {
                                            SettlementAmt += Convert.ToDouble(a);
                                            TotalSettlementAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            SettlementAmt += 0;
                                        }
                                    }
                                    StartCharData++;
                                }
                                StartCharData = 'A';
                                StartIndexData++;
                            }

                            ws.Cell("A" + StartIndexData).SetValue("Total");
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            ws.Cell("A" + Index).SetValue("SettmentDate:   " + SettmentDate);
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            string[] Array = { "D", "E" };
                            string setval = "";
                            foreach (string value in Array)
                            {

                                if (value == "D")
                                {
                                    setval = Convert.ToString(FaceValue);
                                }
                                if (value == "E")
                                {
                                    setval = Convert.ToString(SettlementAmt);
                                }

                                ws.Cell(value + StartIndexData).SetValue(setval);
                                ws.Cell(value + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                ws.Cell(value + StartIndexData).Style.Font.Bold = true;
                            }
                            StartIndexData++;
                            StartIndexData++;
                        }

                        //StartIndexData++;
                    }
                    //StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("D" + StartIndexData).SetValue(TotalFaceValue);
                    ws.Cell("D" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("D" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("E" + StartIndexData).SetValue(TotalSettlementAmt);
                    ws.Cell("E" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("E" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:M1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue(Flag.ToUpper() + " PAYMENT REPORT                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    ws.Column(9).Delete();
                    ws.Columns().AdjustToContents();

                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + Flag + "PaymentReport" + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISInFlowOutFlowSecurityReport(DataTable dt, string Flag, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    if (Flag == "P")
                    {
                        Flag = "Inflow";
                    }
                    else
                    {
                        Flag = "Outflow";
                    }
                    ws = wb.Worksheets.Add(Flag + "SecurityReport");
                    //Add new sheet

                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "DealSlipNo", "SecurityName", "CustomerName", "FaceValue", "Rate", "NoOfBond", "ISINNo", "SettlementAmt", "BankName", "SettmentDate");

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;

                    DataTable dtSettmentDate = new DataTable();
                    System.Data.DataView view = new System.Data.DataView(dt);
                    dtSettmentDate = view.ToTable(true, "SettmentDate");
                    string settdate = "";
                    string SettmentDate = "", DealerBranch = "";
                    double SellProfit = 0.0/*H*/, PurchaseProfit = 0.0/*I*/, FaceValue = 0.0/*K*/;
                    double SettlementAmt = 0.0/*L*/, Profit = 0.0/*M*/, HOProfit = 0.0/*O*/;

                    double TotalSellProfit = 0.0/*I*/, TotalPurchaseProfit = 0.0/*J*/, TotalFaceValue = 0.0/*L*/;
                    double TotalSettlementAmt = 0.0/*M*/, TotalProfit = 0.0/*N*/, TotalHOProfit = 0.0/*O*/;

                    foreach (DataRow row in dtSettmentDate.Rows)
                    {
                        SellProfit = 0;
                        PurchaseProfit = 0;
                        FaceValue = 0;
                        SettlementAmt = 0;
                        Profit = 0;
                        HOProfit = 0;
                        int Index = StartIndexData;

                        DataTable dtTarget = new DataTable();
                        dtTarget = dt.Clone();
                        settdate = Convert.ToString(row["SettmentDate"]);

                        DataRow[] rowsToCopy;
                        rowsToCopy = dt.Select("SettmentDate='" + settdate + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }
                        int dtTargetC = dtTarget.Rows.Count;
                        StartIndexData++;
                        if (dtTargetC > 0)
                        {
                            // filling header with formatting and colors
                            StartCharCols = 'A';
                            StartIndexCols = StartIndexData;
                            for (int i = 1; i <= cols.Length; i++)
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }

                            SettmentDate = Convert.ToString(dtTarget.Rows[0]["SettmentDate"]);
                            //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                            StartIndexData++;
                            for (int i = 0; i < dtTarget.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtTarget.Columns.Count; j++)
                                {
                                    string DataCell = StartCharData.ToString() + StartIndexData;
                                    //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                                    string a = dtTarget.Rows[i][j].ToString();
                                    a = a.Replace("&nbsp;", " ");
                                    a = a.Replace("&amp;", "&");
                                    //check if value is of integer type
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

                                    if (StartCharData.ToString() == "D")
                                    {
                                        if (a != "")
                                        {
                                            FaceValue += Convert.ToDouble(a);
                                            TotalFaceValue += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            FaceValue += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "H")
                                    {
                                        if (a != "")
                                        {
                                            SettlementAmt += Convert.ToDouble(a);
                                            TotalSettlementAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            SettlementAmt += 0;
                                        }
                                    }
                                    StartCharData++;
                                }
                                StartCharData = 'A';
                                StartIndexData++;
                            }

                            ws.Cell("A" + StartIndexData).SetValue("Total");
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            ws.Cell("A" + Index).SetValue("SettmentDate:   " + SettmentDate);
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            string[] Array = { "D", "H" };
                            string setval = "";
                            foreach (string value in Array)
                            {

                                if (value == "D")
                                {
                                    setval = Convert.ToString(FaceValue);
                                }
                                if (value == "H")
                                {
                                    setval = Convert.ToString(SettlementAmt);
                                }

                                ws.Cell(value + StartIndexData).SetValue(setval);
                                ws.Cell(value + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                ws.Cell(value + StartIndexData).Style.Font.Bold = true;
                            }
                            StartIndexData++;
                            StartIndexData++;
                        }

                        //StartIndexData++;
                    }
                    //StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("D" + StartIndexData).SetValue(TotalFaceValue);
                    ws.Cell("D" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("D" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("H" + StartIndexData).SetValue(TotalSettlementAmt);
                    ws.Cell("H" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("H" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:F1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue(Flag.ToUpper() + " SECURITY REPORT                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    ws.Column(11).Delete();
                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + Flag + "SecurityReport" + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISInFlowOutFlowBankReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("InFlowOutFlowBankReport");
                    //Add new sheet

                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "BankName", "SecurityName", "CounterParty", "FaceValue", "InFlow", "OutFlow", "DealSlipNo", "SettmentDate");

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;

                    DataTable dtSettmentDate = new DataTable();
                    System.Data.DataView view = new System.Data.DataView(dt);
                    dtSettmentDate = view.ToTable(true, "SettmentDate");
                    string settdate = "";
                    string SettmentDate = "", DealerBranch = "";
                    double FaceValue = 0.0, InFlow = 0.0, OutFlow = 0.0;
                    double TotalFV = 0.0, TotalInFlow = 0.0, TotalOutFlow = 0.0;

                    foreach (DataRow row in dtSettmentDate.Rows)
                    {
                        FaceValue = 0;
                        InFlow = 0;
                        OutFlow = 0;
                        int Index = StartIndexData;

                        DataTable dtTarget = new DataTable();
                        dtTarget = dt.Clone();
                        settdate = Convert.ToString(row["SettmentDate"]);

                        DataRow[] rowsToCopy;
                        rowsToCopy = dt.Select("SettmentDate='" + settdate + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }
                        int dtTargetC = dtTarget.Rows.Count;
                        StartIndexData++;
                        if (dtTargetC > 0)
                        {
                            // filling header with formatting and colors
                            StartCharCols = 'A';
                            StartIndexCols = StartIndexData;
                            for (int i = 1; i <= cols.Length; i++)
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }

                            SettmentDate = Convert.ToString(dtTarget.Rows[0]["SettmentDate"]);
                            //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                            StartIndexData++;
                            for (int i = 0; i < dtTarget.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtTarget.Columns.Count; j++)
                                {
                                    string DataCell = StartCharData.ToString() + StartIndexData;
                                    //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                                    string a = dtTarget.Rows[i][j].ToString();
                                    a = a.Replace("&nbsp;", " ");
                                    a = a.Replace("&amp;", "&");
                                    //check if value is of integer type
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

                                    if (StartCharData.ToString() == "D")
                                    {
                                        if (a != "")
                                        {
                                            FaceValue += Convert.ToDouble(a);
                                            TotalFV += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            FaceValue += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "E")
                                    {
                                        if (a != "")
                                        {
                                            InFlow += Convert.ToDouble(a);
                                            TotalInFlow += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            InFlow += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "F")
                                    {
                                        if (a != "")
                                        {
                                            OutFlow += Convert.ToDouble(a);
                                            TotalOutFlow += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            OutFlow += 0;
                                        }
                                    }
                                    StartCharData++;
                                }
                                StartCharData = 'A';
                                StartIndexData++;
                            }

                            ws.Cell("A" + StartIndexData).SetValue("Total");
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            ws.Cell("A" + Index).SetValue("SettmentDate:   " + SettmentDate);
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            string[] Array = { "D", "E", "F" };
                            string setval = "";
                            foreach (string value in Array)
                            {

                                if (value == "D")
                                {
                                    setval = Convert.ToString(FaceValue);
                                }
                                if (value == "E")
                                {
                                    setval = Convert.ToString(InFlow);
                                }
                                if (value == "F")
                                {
                                    setval = Convert.ToString(OutFlow);
                                }
                                ws.Cell(value + StartIndexData).SetValue(setval);
                                ws.Cell(value + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                ws.Cell(value + StartIndexData).Style.Font.Bold = true;
                            }
                            StartIndexData++;
                            StartIndexData++;
                        }

                        //StartIndexData++;
                    }
                    //StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("D" + StartIndexData).SetValue(TotalFV);
                    ws.Cell("D" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("D" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("E" + StartIndexData).SetValue(TotalInFlow);
                    ws.Cell("E" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("E" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("F" + StartIndexData).SetValue(TotalOutFlow);
                    ws.Cell("F" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("F" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:F1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("INFLOW OUTFLOW BANKWISE REPORT                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    ws.Column(8).Delete();
                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=InFlowOutFlowBankwiseReport" + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISAccAccuredIntReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("AccountAccruedIntReport");
                    //Add new sheet

                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "DealNo", "TransType", "SecurityName", "FaceValue", "DealDate", "AccruedInt");

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;
                    double FaceValue = 0.0, totalFaceValue = 0.0, AccruedInt = 0.0, totalAccruedInt = 0.0;

                    // filling header with formatting and colors
                    StartCharCols = 'A';
                    StartIndexCols = StartIndexData;
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                        ws.Cell(DataCell).Value = cols[i - 1];
                        ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                        ws.Cell(DataCell).Style.Font.Bold = true;
                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                        ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                        StartCharCols++;
                    }

                    //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                    StartIndexData++;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string DataCell = StartCharData.ToString() + StartIndexData;
                            //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                            string a = dt.Rows[i][j].ToString();
                            a = a.Replace("&nbsp;", " ");
                            a = a.Replace("&amp;", "&");
                            //check if value is of integer type
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

                            if (StartCharData.ToString() == "D")
                            {
                                if (a != "")
                                {
                                    FaceValue += Convert.ToDouble(a);
                                    totalFaceValue += Convert.ToDouble(a);
                                }
                                else
                                {
                                    FaceValue += 0;
                                }
                            }

                            if (StartCharData.ToString() == "F")
                            {
                                if (a != "")
                                {
                                    AccruedInt += Convert.ToDouble(a);
                                    totalAccruedInt += Convert.ToDouble(a);
                                }
                                else
                                {
                                    AccruedInt += 0;
                                }
                            }
                            StartCharData++;
                        }
                        StartCharData = 'A';
                        StartIndexData++;
                    }

                    StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("D" + StartIndexData).SetValue(totalFaceValue);
                    ws.Cell("D" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("D" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("F" + StartIndexData).SetValue(totalAccruedInt);
                    ws.Cell("F" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("F" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:F1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("ACCOUNT ACCRUED INTEREST REPORT                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=AccountAccruedInterestReport" + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISAccountPurchaseReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("AccountPurchaseReport");
                    //Add new sheet
                    int DeleteStartIndex = Convert.ToInt32(dt.Rows[0]["DeleteStartIndex"].ToString());
                    int DeleteEndIndex = Convert.ToInt32(dt.Rows[0]["DeleteEndIndex"].ToString());
                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    //dt = view_.ToTable(false, "DealNo", "CustomerName", "SecurityName", "DealDate", "SettmentDate", "FaceValue", "Rate", "AccruedInt", "FinalAmount", "TransType");
                    dt = view_.ToTable(false, "Dealslipno", "DealDate", "SettlementDate", "SecurityName", "ISIN", "CustomerName", "FaceValue", "NoOfBond", "Rate","YTM", "Principal Amount", "Interest Amount", "TotalConsideration", "StampDutyAmt", "TCSAmount", "TDSAmount", "SettlementAmt", "ModeOfPayment", "ModeOfDelivery", "TransType");
                    DataTable dtTarget = new DataTable();
                    dtTarget = dt.Clone();
                    DataRow[] rowsToCopy;
                    rowsToCopy = dt.Select("TransType='P'");
                    foreach (DataRow temp in rowsToCopy)
                    {
                        dtTarget.ImportRow(temp);
                    }

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;
                    double facevalue = 0.0, settamount = 0.0, princiAmt = 0.0, InterestAmt = 0.0, StampDutyAmt = 0.0, TotalConsideration = 0.0, TCSAmount = 0.0, TDSAmount = 0.0;
                    double totalfacevalue = 0.0, totalsettamount = 0.0, TotalprinciAmt = 0.0, TotalInterestAmt = 0.0, TOtalStampDutyAmt = 0.0, TTotalConsideration = 0.0, TotalTCSAmount = 0.0, TotalTDSAmount = 0.0;
                    // filling header with formatting and colors
                    StartCharCols = 'A';
                    StartIndexCols = StartIndexData;
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                        ws.Cell(DataCell).Value = cols[i - 1];
                        ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                        ws.Cell(DataCell).Style.Font.Bold = true;
                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                        ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                        StartCharCols++;
                    }

                    //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                    StartIndexData++;
                    dt = dtTarget;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string DataCell = StartCharData.ToString() + StartIndexData;
                            //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                            string a = dt.Rows[i][j].ToString();
                            a = a.Replace("&nbsp;", " ");
                            a = a.Replace("&amp;", "&");
                            //check if value is of integer type
                            int val = 0;
                            double val2 = 0.0;
                            DateTime dt2 = DateTime.Now;
                            if (int.TryParse(a, out val))
                            {
                                //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                ws.Cell(DataCell).Value = val;
                                //ws.Cell(DataCell).SetValue(a);
                                //ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val);
                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            }
                          else  if (double.TryParse(a, out val2))
                            {
                                ws.Cell(DataCell).Value = val2;
                                ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val2);
                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
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

                            if (StartCharData.ToString() == "H")
                            {
                                if (a != "")
                                {
                                    facevalue += Convert.ToDouble(a);
                                    totalfacevalue += Convert.ToDouble(a);
                                }
                                else
                                {
                                    facevalue += 0;
                                }
                            }
                            if (StartCharData.ToString() == "K")
                            {
                                if (a != "")
                                {
                                    princiAmt += Convert.ToDouble(a);
                                    TotalprinciAmt += Convert.ToDouble(a);
                                }
                                else
                                {
                                    facevalue += 0;
                                }
                            }
                            if (StartCharData.ToString() == "L")
                            {
                                if (a != "")
                                {
                                    InterestAmt += Convert.ToDouble(a);
                                    TotalInterestAmt += Convert.ToDouble(a);
                                }
                                else
                                {
                                    facevalue += 0;
                                }
                            }
                            if (StartCharData.ToString() == "M")
                            {
                                if (a != "")
                                {
                                    TotalConsideration += Convert.ToDouble(a);
                                    TTotalConsideration += Convert.ToDouble(a);
                                }
                                else
                                {
                                    TotalConsideration += 0;
                                }
                            }
                            if (StartCharData.ToString() == "N")
                            {
                                if (a != "")
                                {
                                    StampDutyAmt += Convert.ToDouble(a);
                                    TOtalStampDutyAmt += Convert.ToDouble(a);
                                }
                                else
                                {
                                    StampDutyAmt += 0;
                                }
                            }

                            if (StartCharData.ToString() == "O")
                            {
                                if (a != "")
                                {
                                    TCSAmount += Convert.ToDouble(a);
                                    TotalTCSAmount += Convert.ToDouble(a);
                                }
                                else
                                {
                                    TCSAmount += 0;
                                }
                            }

                            if (StartCharData.ToString() == "P")
                            {
                                if (a != "")
                                {
                                    TDSAmount += Convert.ToDouble(a);
                                    TotalTDSAmount += Convert.ToDouble(a);
                                }
                                else
                                {
                                    TDSAmount += 0;
                                }
                            }
                            if (StartCharData.ToString() == "Q")
                            {
                                if (a != "")
                                {
                                    settamount += Convert.ToDouble(a);
                                    totalsettamount += Convert.ToDouble(a);
                                }
                                else
                                {
                                    settamount += 0;
                                }
                            }


                            StartCharData++;
                        }
                        StartCharData = 'A';
                        StartIndexData++;
                    }

                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("H" + StartIndexData).SetValue(totalfacevalue);
                    ws.Cell("H" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //ws.Cell("H" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(totalfacevalue);
                    ws.Cell("H" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("K" + StartIndexData).SetValue(TotalprinciAmt);
                    ws.Cell("K" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("K" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalprinciAmt);
                    ws.Cell("K" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("L" + StartIndexData).SetValue(TotalInterestAmt);
                    ws.Cell("L" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("L" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalInterestAmt);
                    ws.Cell("L" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("M" + StartIndexData).SetValue(TTotalConsideration);
                    ws.Cell("M" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("M" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TTotalConsideration);
                    ws.Cell("M" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("N" + StartIndexData).SetValue(TOtalStampDutyAmt);
                    ws.Cell("N" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("N" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TOtalStampDutyAmt);
                    ws.Cell("N" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("O" + StartIndexData).SetValue(TotalTCSAmount);
                    ws.Cell("O" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("O" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalTCSAmount);
                    ws.Cell("O" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("P" + StartIndexData).SetValue(TotalTDSAmount);
                    ws.Cell("P" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("P" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalTDSAmount);
                    ws.Cell("P" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("Q" + StartIndexData).SetValue(totalsettamount);
                    ws.Cell("Q" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("Q" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(totalsettamount);
                    ws.Cell("Q" + StartIndexData).Style.Font.Bold = true;


                    string Range = "";
                    Range = "A1:F1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("ACCOUNT PURCHASE REPORT                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    for (int I = DeleteStartIndex; I < DeleteEndIndex; I++)
                    {
                        ws.Column(DeleteStartIndex).Delete();
                    }

                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=AccountPurchaseReport" + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISAccountSellReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("AccountSellReport");
                    //Add new sheet
                    int DeleteStartIndex = Convert.ToInt32(dt.Rows[0]["DeleteStartIndex"].ToString());
                    int DeleteEndIndex = Convert.ToInt32(dt.Rows[0]["DeleteEndIndex"].ToString());
                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    //dt = view_.ToTable(false, "DealNo", "CustomerName", "SecurityName", "DealDate", "SettmentDate", "FaceValue", "Rate", "AccruedInt", "FinalAmount", "TransType");
                    dt = view_.ToTable(false, "Dealslipno", "DealDate", "SettlementDate", "SecurityName", "ISIN", "CustomerName", "FaceValue", "NoOfBond", "Rate", "YTM", "Principal Amount", "Interest Amount", "TotalConsideration", "StampDutyAmt", "TCSAmount", "TDSAmount", "SettlementAmt", "ModeOfPayment", "ModeOfDelivery", "TransType");
                    DataTable dtTarget = new DataTable();
                    dtTarget = dt.Clone();
                    DataRow[] rowsToCopy;
                    rowsToCopy = dt.Select("TransType='S'");
                    foreach (DataRow temp in rowsToCopy)
                    {
                        dtTarget.ImportRow(temp);
                    }

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;
                    double facevalue = 0.0, settamount = 0.0, princiAmt = 0.0, InterestAmt = 0.0, StampDutyAmt = 0.0, TotalConsideration = 0.0, TCSAmount = 0.0, TDSAmount = 0.0;
                    double totalfacevalue = 0.0, totalsettamount = 0.0, TotalprinciAmt = 0.0, TotalInterestAmt = 0.0, TOtalStampDutyAmt = 0.0, TTotalConsideration = 0.0, TotalTCSAmount = 0.0, TotalTDSAmount = 0.0;
                    // filling header with formatting and colors
                    StartCharCols = 'A';
                    StartIndexCols = StartIndexData;
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                        ws.Cell(DataCell).Value = cols[i - 1];
                        ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                        ws.Cell(DataCell).Style.Font.Bold = true;
                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                        ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                        StartCharCols++;
                    }

                    //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                    StartIndexData++;
                    dt = dtTarget;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string DataCell = StartCharData.ToString() + StartIndexData;
                            //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                            string a = dt.Rows[i][j].ToString();
                            a = a.Replace("&nbsp;", " ");
                            a = a.Replace("&amp;", "&");
                            //check if value is of integer type
                            int val = 0;
                            double val2 = 0.0;
                            DateTime dt2 = DateTime.Now;
                            if (int.TryParse(a, out val))
                            {
                                //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                ws.Cell(DataCell).Value = val;
                                //ws.Cell(DataCell).SetValue(a);
                                //ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val);
                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            }
                            else if (double.TryParse(a, out val2))
                            {
                                ws.Cell(DataCell).Value = val2;
                                ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val2);
                                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
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

                            if (StartCharData.ToString() == "H")
                            {
                                if (a != "")
                                {
                                    facevalue += Convert.ToDouble(a);
                                    totalfacevalue += Convert.ToDouble(a);
                                }
                                else
                                {
                                    facevalue += 0;
                                }
                            }
                            if (StartCharData.ToString() == "K")
                            {
                                if (a != "")
                                {
                                    princiAmt += Convert.ToDouble(a);
                                    TotalprinciAmt += Convert.ToDouble(a);
                                }
                                else
                                {
                                    facevalue += 0;
                                }
                            }
                            if (StartCharData.ToString() == "L")
                            {
                                if (a != "")
                                {
                                    InterestAmt += Convert.ToDouble(a);
                                    TotalInterestAmt += Convert.ToDouble(a);
                                }
                                else
                                {
                                    facevalue += 0;
                                }
                            }
                            if (StartCharData.ToString() == "M")
                            {
                                if (a != "")
                                {
                                    TotalConsideration += Convert.ToDouble(a);
                                    TTotalConsideration += Convert.ToDouble(a);
                                }
                                else
                                {
                                    TotalConsideration += 0;
                                }
                            }
                            if (StartCharData.ToString() == "N")
                            {
                                if (a != "")
                                {
                                    StampDutyAmt += Convert.ToDouble(a);
                                    TOtalStampDutyAmt += Convert.ToDouble(a);
                                }
                                else
                                {
                                    StampDutyAmt += 0;
                                }
                            }

                            if (StartCharData.ToString() == "O")
                            {
                                if (a != "")
                                {
                                    TCSAmount += Convert.ToDouble(a);
                                    TotalTCSAmount += Convert.ToDouble(a);
                                }
                                else
                                {
                                    TCSAmount += 0;
                                }
                            }

                            if (StartCharData.ToString() == "P")
                            {
                                if (a != "")
                                {
                                    TDSAmount += Convert.ToDouble(a);
                                    TotalTDSAmount += Convert.ToDouble(a);
                                }
                                else
                                {
                                    TDSAmount += 0;
                                }
                            }
                            if (StartCharData.ToString() == "Q")
                            {
                                if (a != "")
                                {
                                    settamount += Convert.ToDouble(a);
                                    totalsettamount += Convert.ToDouble(a);
                                }
                                else
                                {
                                    settamount += 0;
                                }
                            }


                            StartCharData++;
                        }
                        StartCharData = 'A';
                        StartIndexData++;
                    }

                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("H" + StartIndexData).SetValue(totalfacevalue);
                    ws.Cell("H" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //ws.Cell("H" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(totalfacevalue);
                    ws.Cell("H" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("K" + StartIndexData).SetValue(TotalprinciAmt);
                    ws.Cell("K" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("K" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalprinciAmt);
                    ws.Cell("K" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("L" + StartIndexData).SetValue(TotalInterestAmt);
                    ws.Cell("L" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("L" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalInterestAmt);
                    ws.Cell("L" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("M" + StartIndexData).SetValue(TTotalConsideration);
                    ws.Cell("M" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("M" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TTotalConsideration);
                    ws.Cell("M" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("N" + StartIndexData).SetValue(TOtalStampDutyAmt);
                    ws.Cell("N" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("N" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TOtalStampDutyAmt);
                    ws.Cell("N" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("O" + StartIndexData).SetValue(TotalTCSAmount);
                    ws.Cell("O" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("O" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalTCSAmount);
                    ws.Cell("O" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("P" + StartIndexData).SetValue(TotalTDSAmount);
                    ws.Cell("P" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("P" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalTDSAmount);
                    ws.Cell("P" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("Q" + StartIndexData).SetValue(totalsettamount);
                    ws.Cell("Q" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("Q" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(totalsettamount);
                    ws.Cell("Q" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:F1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("ACCOUNT SELL REPORT                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    for (int I = DeleteStartIndex; I < DeleteEndIndex; I++)
                    {
                        ws.Column(DeleteStartIndex).Delete();
                    }
                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=AccountSellReport" + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISAccountProfitReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("AccountProfitReport");
                    //Add new sheet

                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "SecurityName", "ISIN", "FaceValue", "PurcDealNo", "PurcCustomer", "PurcRate", "PurcDealDate", "PurcSettmentDate", "PurcAmt", "PurcSettAmt", "PurchaseMarketType", "SellDealNo", "SellCustomer", "SellRate", "SellDealDate", "SellSettlmentDate", "SellSettAmt", "Profit", "SellMarketType");

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;
                    double FaceValue = 0.0, totalFaceValue = 0.0, PurcSettAmount = 0.0, totalPurcSettAmount = 0.0;
                    double SellSettAmount = 0.0, totalSellSettAmount = 0, profit = 0, totalprofit = 0;
                    // filling header with formatting and colors
                    StartCharCols = 'A';
                    StartIndexCols = StartIndexData;
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                        ws.Cell(DataCell).Value = cols[i - 1];
                        ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                        ws.Cell(DataCell).Style.Font.Bold = true;
                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                        ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                        StartCharCols++;
                    }

                    //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                    StartIndexData++;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string DataCell = StartCharData.ToString() + StartIndexData;
                            //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                            string a = dt.Rows[i][j].ToString();
                            a = a.Replace("&nbsp;", " ");
                            a = a.Replace("&amp;", "&");
                            //check if value is of integer type
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

                            if (StartCharData.ToString() == "C")
                            {
                                if (a != "")
                                {
                                    FaceValue += Convert.ToDouble(a);
                                    totalFaceValue += Convert.ToDouble(a);
                                }
                                else
                                {
                                    FaceValue += 0;
                                }
                            }
                            if (StartCharData.ToString() == "J")
                            {
                                if (a != "")
                                {
                                    PurcSettAmount += Convert.ToDouble(a);
                                    totalPurcSettAmount += Convert.ToDouble(a);
                                }
                                else
                                {
                                    PurcSettAmount += 0;
                                }
                            }
                            if (StartCharData.ToString() == "Q")
                            {
                                if (a != "")
                                {
                                    SellSettAmount += Convert.ToDouble(a);
                                    totalSellSettAmount += Convert.ToDouble(a);
                                }
                                else
                                {
                                    SellSettAmount += 0;
                                }
                            }
                            if (StartCharData.ToString() == "R")
                            {
                                if (a != "")
                                {
                                    profit += Convert.ToDouble(a);
                                    totalprofit += Convert.ToDouble(a);
                                }
                                else
                                {
                                    profit += 0;
                                }
                            }
                            StartCharData++;
                        }
                        StartCharData = 'A';
                        StartIndexData++;
                    }

                    StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("C" + StartIndexData).SetValue(totalFaceValue);
                    ws.Cell("C" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("C" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("I" + StartIndexData).SetValue(totalPurcSettAmount);
                    ws.Cell("I" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("I" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("P" + StartIndexData).SetValue(totalSellSettAmount);
                    ws.Cell("P" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("P" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("Q" + StartIndexData).SetValue(totalprofit);
                    ws.Cell("Q" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("Q" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:F1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("ACCOUNT PROFIT REPORT                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=AccountProfitReport." + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISClientWiseBrokerageReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("ClientwiseBrokerageReport");
                    //Add new sheet

                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "CustomerName", "BrockPaid", "BrockReceived", "Profit");

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;
                    double brokpaid = 0.0, totalbrokpaid = 0.0, brokrcvd = 0.0, totalbrokrcvd = 0.0;
                    double profit = 0.0, totalprofit = 0;
                    // filling header with formatting and colors
                    StartCharCols = 'A';
                    StartIndexCols = StartIndexData;
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                        ws.Cell(DataCell).Value = cols[i - 1];
                        ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                        ws.Cell(DataCell).Style.Font.Bold = true;
                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                        ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                        StartCharCols++;
                    }

                    //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                    StartIndexData++;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string DataCell = StartCharData.ToString() + StartIndexData;
                            //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                            string a = dt.Rows[i][j].ToString();
                            a = a.Replace("&nbsp;", " ");
                            a = a.Replace("&amp;", "&");
                            //check if value is of integer type
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
                            if (StartCharData.ToString() == "B")
                            {
                                if (a != "")
                                {
                                    brokpaid += Convert.ToDouble(a);
                                    totalbrokpaid += Convert.ToDouble(a);
                                }
                                else
                                {
                                    brokpaid += 0;
                                }
                            }

                            if (StartCharData.ToString() == "C")
                            {
                                if (a != "")
                                {
                                    brokrcvd += Convert.ToDouble(a);
                                    totalbrokrcvd += Convert.ToDouble(a);
                                }
                                else
                                {
                                    brokrcvd += 0;
                                }
                            }

                            if (StartCharData.ToString() == "D")
                            {
                                if (a != "")
                                {
                                    profit += Convert.ToDouble(a);
                                    totalprofit += Convert.ToDouble(a);
                                }
                                else
                                {
                                    profit += 0;
                                }
                            }
                            StartCharData++;
                        }
                        StartCharData = 'A';
                        StartIndexData++;
                    }

                    StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("B" + StartIndexData).SetValue(totalbrokpaid);
                    ws.Cell("B" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("B" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("C" + StartIndexData).SetValue(totalbrokrcvd);
                    ws.Cell("C" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("C" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("D" + StartIndexData).SetValue(totalprofit);
                    ws.Cell("D" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("D" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:I1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("CLIENT WISE BROKERAGE REPORT                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ClientwiseBrokerageReport" + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISDealerwiseBrokerageReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("Dealerwise Brokerage Report");
                    //Add new sheet




                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;

                    DataTable dtDealers = new DataTable();
                    System.Data.DataView view = new System.Data.DataView(dt);
                    dtDealers = view.ToTable(true, "DealerId");
                    string DealerId = "";
                    string DealerName = "", DealSlipNo = "", SecurityName = "";
                    double SellAmount = 0.0/*H*/, PurchaseAmount = 0.0/*I*/, FaceValue = 0.0/*K*/;
                    double Brokpaid = 0.0/*L*/, Brokrcvd = 0.0/*M*/, Profit = 0.0/*O*/;

                    double totalsellamount = 0.0/*H*/, totalpurchaseamount = 0.0/*I*/, totalfacevalue = 0.0/*K*/;
                    double totalbrokpaid = 0.0/*L*/, totalbrokrcvd = 0.0/*M*/, totalprofit = 0.0/*O*/;

                    double TotalSellProfit = 0.0/*I*/, TotalPurchaseProfit = 0.0/*J*/, TotalFaceValue = 0.0/*L*/;
                    double TotalSettlementAmt = 0.0/*M*/, TotalProfit = 0.0/*N*/, TotalHOProfit = 0.0/*O*/;

                    foreach (DataRow row in dtDealers.Rows)
                    {
                        SellAmount = 0;
                        PurchaseAmount = 0;
                        FaceValue = 0;

                        DataTable dtTarget = new DataTable();
                        dtTarget = dt.Clone();
                        DealerId = Convert.ToString(row["DealerId"]);

                        DataRow[] rowsToCopy;
                        rowsToCopy = dt.Select("DealerId='" + DealerId + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }
                        System.Data.DataView view_ = new System.Data.DataView(dtTarget);
                        dtTarget = view_.ToTable(false, "CustomerName", "Rate", "DealDate", "SettmentDate", "FaceValue", "Quantity", "PurchaseAmount", "SellAmount", "BrokeragePaid", "BrokerageReceived", "Profit", "DealerName", "DealSlipNo", "SecurityName");
                        int dtTargetC = dtTarget.Rows.Count;

                        //fetching columns from dt
                        string[] cols = new string[dtTarget.Columns.Count];
                        //adding the  columns
                        for (int c = 0; c < dtTarget.Columns.Count; c++)
                        {
                            string a = dtTarget.Columns[c].ToString();
                            cols[c] = dtTarget.Columns[c].ToString().Replace('_', ' ');
                        }
                        DealerName = Convert.ToString(dtTarget.Rows[0]["DealerName"]);
                        ws.Cell("A" + StartIndexData).SetValue("Dealer Name :  " + DealerName);
                        ws.Cell("A" + StartIndexData).Style.Font.Bold = true;
                        string Range1 = "";
                        Range1 = "A" + StartIndexData + ":F" + StartIndexData;
                        ws.Range(Range1).Merge();
                        StartIndexData++;
                        for (int k = 0; k < dtTarget.Rows.Count; k++)
                        {
                            if (dtTargetC > 0)
                            {
                                DealSlipNo = Convert.ToString(dtTarget.Rows[k]["DealSlipNo"]);
                                SecurityName = Convert.ToString(dtTarget.Rows[k]["SecurityName"]);
                                string Range_ = "";
                                Range_ = "A" + StartIndexData + ":F" + StartIndexData;
                                ws.Range(Range_).Merge();
                                ws.Range(Range_).Style.Font.Bold = true;
                                ws.Range(Range_).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                                ws.Range(Range_).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                ws.Cell("A" + StartIndexData).SetValue("DealNo :  " + DealSlipNo + "                    SecurityName :  " + SecurityName);
                                StartIndexData++;
                                // filling header with formatting and colors
                                StartCharCols = 'A';
                                StartIndexCols = StartIndexData;
                                for (int i = 1; i <= cols.Length; i++)
                                {
                                    string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                    ws.Cell(DataCell).Value = cols[i - 1];
                                    ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                    ws.Cell(DataCell).Style.Font.Bold = true;
                                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                    ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                    StartCharCols++;
                                }

                                StartIndexData++;

                                for (int j = 0; j < dtTarget.Columns.Count; j++)
                                {
                                    string DataCell = StartCharData.ToString() + StartIndexData;
                                    //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                                    string a = dtTarget.Rows[k][j].ToString();
                                    a = a.Replace("&nbsp;", " ");
                                    a = a.Replace("&amp;", "&");
                                    //check if value is of integer type
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
                                    if (StartCharData.ToString() == "E")
                                    {
                                        if (a != "")
                                        {
                                            FaceValue += Convert.ToDouble(a);
                                            totalfacevalue += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            FaceValue += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "G")
                                    {
                                        if (a != "")
                                        {
                                            PurchaseAmount += Convert.ToDouble(a);
                                            totalpurchaseamount += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            PurchaseAmount += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "H")
                                    {
                                        if (a != "")
                                        {
                                            SellAmount += Convert.ToDouble(a);
                                            totalsellamount += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            SellAmount += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "I")
                                    {
                                        if (a != "")
                                        {
                                            Brokpaid += Convert.ToDouble(a);
                                            totalbrokpaid += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            Brokpaid += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "J")
                                    {
                                        if (a != "")
                                        {
                                            Brokrcvd += Convert.ToDouble(a);
                                            totalbrokrcvd += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            Brokrcvd += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "K")
                                    {
                                        if (a != "")
                                        {
                                            Profit += Convert.ToDouble(a);
                                            totalprofit += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            Profit += 0;
                                        }
                                    }
                                    StartCharData++;
                                }
                                StartCharData = 'A';
                                StartIndexData++;
                                ws.Cell("A" + StartIndexData).SetValue("Total");
                                ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                ws.Cell("A" + StartIndexData).Style.Font.Bold = true;


                                string[] Array = { "E", "G", "H", "I", "J", "K" };
                                string setval = "";
                                foreach (string value in Array)
                                {

                                    if (value == "E")
                                    {
                                        setval = Convert.ToString(FaceValue);
                                    }
                                    if (value == "G")
                                    {
                                        setval = Convert.ToString(PurchaseAmount);
                                    }
                                    if (value == "H")
                                    {
                                        setval = Convert.ToString(SellAmount);
                                    }
                                    if (value == "I")
                                    {
                                        setval = Convert.ToString(Brokpaid);
                                    }
                                    if (value == "J")
                                    {
                                        setval = Convert.ToString(Brokrcvd);
                                    }
                                    if (value == "K")
                                    {
                                        setval = Convert.ToString(Profit);
                                    }
                                    ws.Cell(value + StartIndexData).SetValue(setval);
                                    ws.Cell(value + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                    ws.Cell(value + StartIndexData).Style.Font.Bold = true;
                                }
                                FaceValue = 0; PurchaseAmount = 0; SellAmount = 0; Brokpaid = 0; Brokrcvd = 0; Profit = 0;
                                StartIndexData++;
                            }
                            StartIndexData++;
                        }
                        StartIndexData++;
                    }
                    //StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("E" + StartIndexData).SetValue(totalfacevalue);
                    ws.Cell("E" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("E" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("G" + StartIndexData).SetValue(totalpurchaseamount);
                    ws.Cell("G" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("G" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("H" + StartIndexData).SetValue(totalsellamount);
                    ws.Cell("H" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("H" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("I" + StartIndexData).SetValue(totalbrokpaid);
                    ws.Cell("I" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("I" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("J" + StartIndexData).SetValue(totalbrokrcvd);
                    ws.Cell("J" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("J" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("K" + StartIndexData).SetValue(totalprofit);
                    ws.Cell("K" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("K" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:M1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("Dealerwise Brokerage Report                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    ws.Column(12).Delete();
                    ws.Column(12).Delete();
                    ws.Column(12).Delete();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=DealerwiseBrokerageReport" + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISBrokerageReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("BrokerageReport");
                    //Add new sheet

                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "CustomerName", "DealSlipNo", "SecurityName", "FaceValue", "DealDate", "SettmentDate", "CounterCustomerName", "ConsultancyCharges");

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;
                    double facevalue = 0.0, consultancycharges = 0.0, brokrcvd = 0.0, totalbrokrcvd = 0.0;
                    double totalfacevalue = 0.0, totalconsultancycharges = 0;
                    // filling header with formatting and colors
                    StartCharCols = 'A';
                    StartIndexCols = StartIndexData;
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                        ws.Cell(DataCell).Value = cols[i - 1];
                        ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                        ws.Cell(DataCell).Style.Font.Bold = true;
                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                        ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                        StartCharCols++;
                    }

                    //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                    StartIndexData++;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string DataCell = StartCharData.ToString() + StartIndexData;
                            //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                            string a = dt.Rows[i][j].ToString();
                            a = a.Replace("&nbsp;", " ");
                            a = a.Replace("&amp;", "&");
                            //check if value is of integer type
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

                            if (StartCharData.ToString() == "D")
                            {
                                if (a != "")
                                {
                                    facevalue += Convert.ToDouble(a);
                                    totalfacevalue += Convert.ToDouble(a);
                                }
                                else
                                {
                                    facevalue += 0;
                                }
                            }
                            if (StartCharData.ToString() == "H")
                            {
                                if (a != "")
                                {
                                    consultancycharges += Convert.ToDouble(a);
                                    totalconsultancycharges += Convert.ToDouble(a);
                                }
                                else
                                {
                                    consultancycharges += 0;
                                }
                            }
                            StartCharData++;
                        }
                        StartCharData = 'A';
                        StartIndexData++;
                    }

                    StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("D" + StartIndexData).SetValue(totalfacevalue);
                    ws.Cell("D" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("D" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("H" + StartIndexData).SetValue(totalconsultancycharges);
                    ws.Cell("H" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("H" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:M1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("BROKERAGE REPORT                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=BrokerageReport" + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISClientwiseBrokReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("Clientwise Brokerage Report");
                    //Add new sheet
                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "DealSlipNo", "CustomerName", "Brokerage", "Profit", "BrokerId", "BrokerName");
                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;

                    DataTable dtBroker = new DataTable();
                    System.Data.DataView view = new System.Data.DataView(dt);
                    dtBroker = view.ToTable(true, "BrokerId");
                    string BrokerId = "";
                    string Broker = "";
                    double brokerage = 0.0, profit = 0.0;
                    double totalbrokerage = 0.0, totalprofit = 0.0;
                    int Index = StartIndexData;
                    foreach (DataRow row in dtBroker.Rows)
                    {
                        Index = StartIndexData;
                        brokerage = 0;
                        profit = 0;
                        DataTable dtTarget = new DataTable();
                        dtTarget = dt.Clone();
                        BrokerId = Convert.ToString(row["BrokerId"]);

                        DataRow[] rowsToCopy;
                        rowsToCopy = dt.Select("BrokerId='" + BrokerId + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }
                        int dtTargetC = dtTarget.Rows.Count;
                        StartIndexData++;
                        if (dtTargetC > 0)
                        {
                            // filling header with formatting and colors
                            StartCharCols = 'A';
                            StartIndexCols = StartIndexData;
                            for (int i = 1; i <= cols.Length; i++)
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }

                            Broker = Convert.ToString(dtTarget.Rows[0]["BrokerName"]);
                            StartIndexData++;
                            for (int i = 0; i < dtTarget.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtTarget.Columns.Count; j++)
                                {
                                    string DataCell = StartCharData.ToString() + StartIndexData;
                                    //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                                    string a = dtTarget.Rows[i][j].ToString();
                                    a = a.Replace("&nbsp;", " ");
                                    a = a.Replace("&amp;", "&");
                                    //check if value is of integer type
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

                                    if (StartCharData.ToString() == "C")
                                    {
                                        if (a != "")
                                        {
                                            brokerage += Convert.ToDouble(a);
                                            totalbrokerage += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            brokerage += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "D")
                                    {
                                        if (a != "")
                                        {
                                            profit += Convert.ToDouble(a);
                                            totalprofit += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            profit += 0;
                                        }
                                    }
                                    StartCharData++;
                                }
                                StartCharData = 'A';
                                StartIndexData++;
                            }

                            ws.Cell("A" + StartIndexData).SetValue("Total");
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            ws.Cell("A" + Index).SetValue("Broker:   " + Broker);
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            string[] Array = { "C", "D" };
                            string setval = "";
                            foreach (string value in Array)
                            {
                                if (value == "C")
                                {
                                    setval = Convert.ToString(brokerage);
                                }
                                if (value == "D")
                                {
                                    setval = Convert.ToString(profit);
                                }
                                ws.Cell(value + StartIndexData).SetValue(setval);
                                ws.Cell(value + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                ws.Cell(value + StartIndexData).Style.Font.Bold = true;
                            }
                            StartIndexData++;
                            StartIndexData++;
                        }
                    }
                    //StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("C" + StartIndexData).SetValue(totalbrokerage);
                    ws.Cell("C" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("C" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("D" + StartIndexData).SetValue(totalprofit);
                    ws.Cell("D" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("D" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:G1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("CLIENTWISE DETAILED BROKERAGE REPORT                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));

                    ws.Column(5).Delete();
                    ws.Column(5).Delete();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ClientwiseDetailedReport" + strdtTime + ".xlsx");
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

    public void ExportToExcel_MISCustomerwiseBrokReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;

                    ws = wb.Worksheets.Add("Customerwise Brokerage Report");
                    //Add new sheet
                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "CustomerName", "DealSlipNo", "SecurityName", "FaceValue", "DealDate", "SettmentDate", "CounterCustomerName", "ConsultancyCharges", "CustomerId");
                    //fetching columns from dt

                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;

                    DataTable dtCustomer = new DataTable();
                    System.Data.DataView view = new System.Data.DataView(dt);
                    dtCustomer = view.ToTable(true, "CustomerId");
                    string CustomerId = "";

                    double facevalue = 0.0, consultancycharges = 0.0;
                    double totalfacevalue = 0.0, totalconsultancycharges = 0.0;

                    foreach (DataRow row in dtCustomer.Rows)
                    {
                        facevalue = 0;
                        consultancycharges = 0;
                        DataTable dtTarget = new DataTable();
                        dtTarget = dt.Clone();
                        CustomerId = Convert.ToString(row["CustomerId"]);

                        DataRow[] rowsToCopy;
                        rowsToCopy = dt.Select("CustomerId='" + CustomerId + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }
                        int dtTargetC = dtTarget.Rows.Count;
                        StartIndexData++;
                        if (dtTargetC > 0)
                        {
                            // filling header with formatting and colors
                            StartCharCols = 'A';
                            StartIndexCols = StartIndexData;
                            for (int i = 1; i <= cols.Length; i++)
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }

                            StartIndexData++;
                            for (int i = 0; i < dtTarget.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtTarget.Columns.Count; j++)
                                {
                                    string DataCell = StartCharData.ToString() + StartIndexData;
                                    //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                                    string a = dtTarget.Rows[i][j].ToString();
                                    a = a.Replace("&nbsp;", " ");
                                    a = a.Replace("&amp;", "&");
                                    //check if value is of integer type
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

                                    if (StartCharData.ToString() == "D")
                                    {
                                        if (a != "")
                                        {
                                            facevalue += Convert.ToDouble(a);
                                            totalfacevalue += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            facevalue += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "H")
                                    {
                                        if (a != "")
                                        {
                                            consultancycharges += Convert.ToDouble(a);
                                            totalconsultancycharges += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            consultancycharges += 0;
                                        }
                                    }
                                    StartCharData++;
                                }
                                StartCharData = 'A';
                                StartIndexData++;
                            }

                            ws.Cell("A" + StartIndexData).SetValue("Total");
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            string[] Array = { "D", "H" };
                            string setval = "";
                            foreach (string value in Array)
                            {
                                if (value == "D")
                                {
                                    setval = Convert.ToString(facevalue);
                                }
                                if (value == "H")
                                {
                                    setval = Convert.ToString(consultancycharges);
                                }
                                ws.Cell(value + StartIndexData).SetValue(setval);
                                ws.Cell(value + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                ws.Cell(value + StartIndexData).Style.Font.Bold = true;
                            }
                            StartIndexData++;
                            StartIndexData++;
                        }
                    }
                    //StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("D" + StartIndexData).SetValue(totalfacevalue);
                    ws.Cell("D" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("D" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("H" + StartIndexData).SetValue(totalconsultancycharges);
                    ws.Cell("H" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("H" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:G1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("CUSTOMERWISE BROKERAGE REPORT                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));

                    ws.Column(9).Delete();
                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=CustomerwiseBrokerageReport" + strdtTime + ".xlsx");
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
    public void ExportToExcel_DailyPurchaseSellReport(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    int DeleteStartIndex = Convert.ToInt32(dt.Rows[0]["DeleteStartIndex"].ToString());
                    int DeleteEndIndex = Convert.ToInt32(dt.Rows[0]["DeleteEndIndex"].ToString());
                    ws = wb.Worksheets.Add("Daily Purchase Sell Report");
                    //Add new sheet
                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "Dealslipno", "DealDate", "SettlementDate", "SecurityName", "ISIN", "CustomerName", "FaceValue", "NoOfBond", "Rate","YTM", "Principal Amount", "Interest Amount", "TotalConsideration", "StampDutyAmt", "TCSAmount", "TDSAmount", "SettlementAmt","Dealer", "ModeOfPayment", "ModeOfDelivery",  "TransType");
                    //fetching columns from dt

                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;

                    DataTable dtPurSell = new DataTable();
                    System.Data.DataView view = new System.Data.DataView(dt);
                    dtPurSell = view.ToTable(true, "TransType");
                    string TransType = "";

                    double facevalue = 0.0, settamount = 0.0, princiAmt = 0.0, InterestAmt = 0.0, StampDutyAmt = 0.0, TotalConsideration = 0.0, TCSAmount = 0.0, TDSAmount = 0.0;
                    double totalfacevalue = 0.0, totalsettamount = 0.0, TotalprinciAmt = 0.0, TotalInterestAmt = 0.0, TOtalStampDutyAmt = 0.0, TTotalConsideration = 0.0, TotalTCSAmount = 0.0, TotalTDSAmount = 0.0;


                    foreach (DataRow row in dtPurSell.Rows)
                    {

                        facevalue = 0;
                        princiAmt = 0;
                        InterestAmt = 0;
                        settamount = 0;
                        TotalConsideration = 0;
                        StampDutyAmt = 0;
                        TCSAmount = 0;
                        TDSAmount = 0;

                        DataTable dtTarget = new DataTable();
                        dtTarget = dt.Clone();
                        TransType = Convert.ToString(row["TransType"]);
                        ws.Cell("A" + StartIndexData).SetValue(TransType);
                        ws.Cell("A" + StartIndexData).Style.Font.Bold = true;
                        DataRow[] rowsToCopy;
                        rowsToCopy = dt.Select("TransType='" + TransType + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }
                        int dtTargetC = dtTarget.Rows.Count;
                        StartIndexData++;
                        if (dtTargetC > 0)
                        {
                            // filling header with formatting and colors
                            StartCharCols = 'A';
                            StartIndexCols = StartIndexData;
                            for (int i = 1; i <= cols.Length; i++)
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }

                            StartIndexData++;
                            for (int i = 0; i < dtTarget.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtTarget.Columns.Count; j++)
                                {
                                    string DataCell = StartCharData.ToString() + StartIndexData;
                                    //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                                    string a = dtTarget.Rows[i][j].ToString();
                                    a = a.Replace("&nbsp;", " ");
                                    a = a.Replace("&amp;", "&");
                                    //check if value is of integer type
                                    int val = 0;
                                    double val2 = 0.0000;
                                    DateTime dt2 = DateTime.Now;
                                    if (int.TryParse(a, out val))
                                    {
                                        //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                        ws.Cell(DataCell).Value = val;
                                        //ws.Cell(DataCell).SetValue(a);
                                        //ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val);
                                        ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    }
                                    else if (double.TryParse(a, out val2))
                                    {
                                        ws.Cell(DataCell).Value = val2;
                                        ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val2);
                                        ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
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



                                    if (StartCharData.ToString() == "H")
                                    {
                                        if (a != "")
                                        {
                                            facevalue += Convert.ToDouble(a);
                                            totalfacevalue += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            facevalue += 0;
                                        }
                                    }
                                    if ((StartCharData.ToString() == "I") || (StartCharData.ToString() == "J"))
                                    {
                                        if (a != "")
                                        {
                                            ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.0000";

                                        }
                                        else
                                        {

                                        }
                                    }
                                    if (StartCharData.ToString() == "K")
                                    {
                                        if (a != "")
                                        {
                                            princiAmt += Convert.ToDouble(a);
                                            TotalprinciAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            facevalue += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "L")
                                    {
                                        if (a != "")
                                        {
                                            InterestAmt += Convert.ToDouble(a);
                                            TotalInterestAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            facevalue += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "M")
                                    {
                                        if (a != "")
                                        {
                                            TotalConsideration += Convert.ToDouble(a);
                                            TTotalConsideration += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            TotalConsideration += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "N")
                                    {
                                        if (a != "")
                                        {
                                            StampDutyAmt += Convert.ToDouble(a);
                                            TOtalStampDutyAmt += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            StampDutyAmt += 0;
                                        }
                                    }

                                    if (StartCharData.ToString() == "O")
                                    {
                                        if (a != "")
                                        {
                                            TCSAmount += Convert.ToDouble(a);
                                            TotalTCSAmount += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            TCSAmount += 0;
                                        }
                                    }

                                    if (StartCharData.ToString() == "P")
                                    {
                                        if (a != "")
                                        {
                                            TDSAmount += Convert.ToDouble(a);
                                            TotalTDSAmount += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            TDSAmount += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "Q")
                                    {
                                        if (a != "")
                                        {
                                            settamount += Convert.ToDouble(a);
                                            totalsettamount += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            settamount += 0;
                                        }
                                    }

                                    //if (double.TryParse(a, out val2))
                                    //{
                                    //    ws.Cell(DataCell).Value = val2;
                                    //    ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.00";
                                    //}
                                    ////check if datetime type
                                    //else if (DateTime.TryParse(a, out dt2))
                                    //{
                                    //    ws.Cell(DataCell).Value = dt2.ToShortDateString();
                                    //    ws.Cell(DataCell).SetValue(a);
                                    //}
                                    //else
                                    //{
                                    //    ws.Cell(DataCell).SetValue(a);
                                    //}
                                    StartCharData++;
                                }
                                StartCharData = 'A';
                                StartIndexData++;
                            }

                            ws.Cell("A" + StartIndexData).SetValue("Total");
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                            string[] Array = { "H",  "K", "L", "M", "N", "O", "P","Q" };
                            string setval = "";
                            foreach (string value in Array)
                            {
                                if (value == "H")
                                {
                                    setval = Convert.ToString(facevalue);
                                }
                                if (value == "K")
                                {
                                    setval = Convert.ToString(princiAmt);
                                }
                                if (value == "L")
                                {
                                    setval = Convert.ToString(InterestAmt);
                                }
                                if (value == "M")
                                {
                                    setval = Convert.ToString(TotalConsideration);
                                }

                                if (value == "N")
                                {
                                    setval = Convert.ToString(StampDutyAmt);
                                }

                                if (value == "O")
                                {
                                    setval = Convert.ToString(TCSAmount);
                                }

                                if (value == "P")
                                {
                                    setval = Convert.ToString(TDSAmount);
                                }

                                if (value == "Q")
                                {
                                    setval = Convert.ToString(settamount);
                                }

                                ws.Cell(value + StartIndexData).SetValue(setval);
                                ws.Cell(value + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                ws.Cell(value + StartIndexData).Style.Font.Bold = true;
                            }
                            StartIndexData++;
                            StartIndexData++;
                        }
                    }

                    for (int I = DeleteStartIndex; I < DeleteEndIndex; I++)
                    {
                        ws.Column(DeleteStartIndex).Delete();
                    }
                    //StartIndexData++;
                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("H" + StartIndexData).SetValue(totalfacevalue);
                    ws.Cell("H" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //ws.Cell("H" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(totalfacevalue);
                    ws.Cell("H" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("K" + StartIndexData).SetValue(TotalprinciAmt);
                    ws.Cell("k" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("K" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalprinciAmt);
                    ws.Cell("K" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("L" + StartIndexData).SetValue(TotalInterestAmt);
                    ws.Cell("L" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("L" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalInterestAmt);
                    ws.Cell("L" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("M" + StartIndexData).SetValue(TTotalConsideration);
                    ws.Cell("M" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("M" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TTotalConsideration);
                    ws.Cell("M" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("N" + StartIndexData).SetValue(TOtalStampDutyAmt);
                    ws.Cell("N" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("N" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TOtalStampDutyAmt);
                    ws.Cell("N" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("O" + StartIndexData).SetValue(TotalTCSAmount);
                    ws.Cell("O" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("O" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalTCSAmount);
                    ws.Cell("O" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("P" + StartIndexData).SetValue(TotalTDSAmount);
                    ws.Cell("P" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("P" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(TotalTDSAmount);
                    ws.Cell("P" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("Q" + StartIndexData).SetValue(totalsettamount);
                    ws.Cell("Q" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("Q" + StartIndexData).Style.NumberFormat.Format = ReturnIndianFormat(totalsettamount);
                    ws.Cell("Q" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:F1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("Daily Purchase Sell Report                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));

                    
                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=DailyPurchaseSellReport" + strdtTime + ".xlsx");
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
    public void ExportToExcel_ViewDealStock_SecurityWise(DataTable dt, string ForDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("View Deal Stock");

                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    //dt = view_.ToTable(false, "SecurityName", "DealSlipNo", "ISIN", "CustomerName", "StockFaceValue", "IPDates", "MaturityDate", "NoOfBonds", "DealDate", "Rate", "Settlementconsideration", "ModeOfDelivery", "DPName", "DPID", "ClientId", "SecurityId", "CompName");
                    dt = view_.ToTable(false, "SecurityName", "DealSlipNo", "ISIN", "CustomerName", "StockFaceValue", "IPDates", "MaturityDate", "NoOfBonds", "DealDate", "Rate", "Settlementconsideration", "RedeemedAmount", "ModeOfDelivery", "SecurityId", "CompName");
                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    string CompName = Convert.ToString(dt.Rows[0]["CompName"].ToString());
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 3;
                    //   ws.AutoFilter.Clear();
                    char dblStartCharCols = 'A';
                    // filling header with formatting and colors
                    int count = 0;
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                        ws.Cell(DataCell).Value = cols[i - 1];
                        ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                        ws.Cell(DataCell).Style.Font.Bold = true;
                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                        ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                        StartCharCols++;
                    }

                    int Ccout = 0;
                    char StartCharData = 'A';

                    int StartIndexData = 4;
                    dblStartCharCols = 'A';

                    DataTable dtSecurityId = new DataTable();
                    System.Data.DataView view = new System.Data.DataView(dt);
                    dtSecurityId = view.ToTable(true, "SecurityId");
                    string SecurityId = "";

                    double NoofBond = 0.0/*I*/, Settlementconsideration = 0.0/*J*/, StockFaceValue = 0.0/*L*/;
                    double SettlementAmt = 0.0/*M*/, Profit = 0.0/*N*/, HOProfit = 0.0/*O*/;

                    double TotalNoofBond = 0.0/*I*/, TotalSettlementconsideration = 0.0/*J*/, TotalStockFaceValue = 0.0/*E*/;
                    double TotalSettlementAmt = 0.0/*M*/, TotalProfit = 0.0/*N*/, TotalHOProfit = 0.0/*O*/;

                    foreach (DataRow row in dtSecurityId.Rows)
                    {
                        StockFaceValue = 0;
                        Settlementconsideration = 0;
                        NoofBond = 0;
                        SettlementAmt = 0;
                        Profit = 0;
                        HOProfit = 0;

                        DataTable dtTarget = new DataTable();
                        dtTarget = dt.Clone();
                        SecurityId = Convert.ToString(row["SecurityId"]);

                        DataRow[] rowsToCopy;
                        rowsToCopy = dt.Select("SecurityId='" + SecurityId + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }
                        int dtTargetC = dtTarget.Rows.Count;
                        if (dtTargetC > 0)
                        {
                            //secname = Convert.ToString(dtTarget.Rows[0]["SecurityName"]);
                            for (int i = 0; i < dtTarget.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtTarget.Columns.Count; j++)
                                {
                                    string DataCell = StartCharData.ToString() + StartIndexData;
                                    //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                                    string a = dtTarget.Rows[i][j].ToString();
                                    a = a.Replace("&nbsp;", " ");
                                    a = a.Replace("&amp;", "&");
                                    //check if value is of integer type
                                    int val = 0;
                                    double val2 = 0.0;
                                    DateTime dt2 = DateTime.Now;
                                    if (int.TryParse(a, out val))
                                    {
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
                                        ws.Cell(DataCell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                    }
                                    else
                                    {
                                        ws.Cell(DataCell).SetValue(a);
                                    }
                                    if (StartCharData.ToString() == "E")
                                    {
                                        if (a != "")
                                        {
                                            StockFaceValue += Convert.ToDouble(a);
                                            TotalStockFaceValue += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            StockFaceValue += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "H")
                                    {
                                        if (a != "")
                                        {
                                            NoofBond += Convert.ToDouble(a);
                                            TotalNoofBond += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            NoofBond += 0;
                                        }
                                    }
                                    if (StartCharData.ToString() == "K")
                                    {
                                        if (a != "")
                                        {
                                            Settlementconsideration += Convert.ToDouble(a);
                                            TotalSettlementconsideration += Convert.ToDouble(a);
                                        }
                                        else
                                        {
                                            Settlementconsideration += 0;
                                        }
                                    }
                                    StartCharData++;
                                }
                                StartCharData = 'A';
                                StartIndexData++;
                            }

                            ws.Cell("A" + StartIndexData).SetValue("Total");
                            ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            ws.Cell("A" + StartIndexData).Style.Font.Bold = true;


                            string[] Array = { "E", "H", "K" };
                            double setval = 0;
                            foreach (string value in Array)
                            {
                                if (value == "E")
                                {
                                    setval = StockFaceValue;
                                }
                                if (value == "H")
                                {
                                    setval = NoofBond;
                                }
                                if (value == "K")
                                {
                                    setval = Settlementconsideration;
                                }

                                ws.Cell(value + StartIndexData).SetValue(setval);
                                ws.Cell(value + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                ws.Cell(value + StartIndexData).Style.Font.Bold = true;
                            }

                            StartIndexData++;
                            StartIndexData++;
                        }

                        //StartIndexData++;
                    }


                    ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A" + StartIndexData).Style.Font.Bold = true;


                    ws.Cell("E" + StartIndexData).SetValue(TotalStockFaceValue);
                    ws.Cell("E" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("E" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("H" + StartIndexData).SetValue(TotalNoofBond);
                    ws.Cell("H" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("H" + StartIndexData).Style.Font.Bold = true;

                    ws.Cell("K" + StartIndexData).SetValue(TotalSettlementconsideration);
                    ws.Cell("K" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell("K" + StartIndexData).Style.Font.Bold = true;
                    ws.Cell("A" + 1).Style.Font.Bold = true;
                    ws.Cell("A" + 1).SetValue(CompName.ToUpper() + " STOCK REPORT AS ON " + ForDate);

                    ws.Columns().AdjustToContents();
                    ws.Range(1, 1, StartIndexData, dt.Columns.Count - 2).Style.NumberFormat.Format = "#,##0.00";
                    ws.Range(1, 8, StartIndexData, 8).Style.NumberFormat.Format = "#,##0";
                    ws.Range(1, 10, StartIndexData, 10).Style.NumberFormat.Format = "#,##0.0000";
                    //ws.Range(1, 1, StartIndexData, dt.Columns.Count - 2).Style.Border.BottomBorder = XLBorderStyleValues.Thick;

                    //ws.Range(1, 1, StartIndexData, dt.Columns.Count - 2).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    //ws.Range(1, 1, StartIndexData, dt.Columns.Count - 2).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    //ws.Range(1, 1, StartIndexData, dt.Columns.Count - 2).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    //ws.Range(1, 1, StartIndexData, dt.Columns.Count - 2).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    ws.Column(14).Delete();
                    ws.Column(14).Delete();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ViewDealStock" + strdtTime + ".xlsx");
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
    public void ExportToExcel_AuditReport(DataTable dt, string FromDate, String ToDate, String ReportType)
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
                    int StartIndexCols = 5;
                    //   ws.AutoFilter.Clear();
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        if (i == cols.Length)
                        {
                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                            ws.Cell(DataCell).Value = cols[i - 1];
                            ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                            ws.Cell(DataCell).Style.Font.Bold = true;
                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                        }
                        else
                        {
                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                            ws.Cell(DataCell).Value = cols[i - 1];
                            ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                            ws.Cell(DataCell).Style.Font.Bold = true;
                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            StartCharCols++;
                        }
                    }
                    char StartCharData = 'A';
                    int StartIndexData = 6;

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

                    //ws.Cell("C" + 1).SetValue("From " + txt_FromDate.Text + " To " + txt_ToDate.Text);
                    if (ReportType == "Customer Master")
                    {
                        ws.Cell("A" + 1).SetValue("Customer Name:  " + Convert.ToString(dt.Rows[0]["CustomerName"]) + "  PAN: " + Convert.ToString(dt.Rows[0]["PanNumber"]));
                        ws.Cell("A" + 2).SetValue("Entered By:  " + Convert.ToString(dt.Rows[0]["EnteredBy"]));
                        ws.Cell("A" + 3).SetValue("Entry Date:  " + Convert.ToString(dt.Rows[0]["CustomerEntryDate"]));
                    }
                    else if (ReportType == "Security Master")
                    {
                        ws.Cell("A" + 1).SetValue("Security Name:  " + Convert.ToString(dt.Rows[0]["SecurityName"]) + "  ISIN: " + Convert.ToString(dt.Rows[0]["NSDLAcNumber"]));
                        ws.Cell("A" + 2).SetValue("Entered By:  " + Convert.ToString(dt.Rows[0]["EnteredBy"]));
                        ws.Cell("A" + 3).SetValue("Entry Date:  " + Convert.ToString(dt.Rows[0]["SecEntryDate"]));
                    }

                    else if (ReportType == "DealSlipentry")
                    {
                        ws.Cell("A" + 1).SetValue("Dealslip No:  " + Convert.ToString(dt.Rows[0]["DealslipNo"]) + "    " + "Cancelled :" + Convert.ToString(dt.Rows[0]["Cancelled"]));
                        ws.Cell("A" + 2).SetValue("Entered By:  " + Convert.ToString(dt.Rows[0]["EnteredBy"]));
                        ws.Cell("A" + 3).SetValue("Entry Date:  " + Convert.ToString(dt.Rows[0]["DealEntryDate"]));
                    }
                    else if (ReportType == "WDMEntry")
                    {
                        ws.Cell("A" + 1).SetValue("Deal No:  " + Convert.ToString(dt.Rows[0]["WDMDealNumber"]));
                        ws.Cell("A" + 2).SetValue("Entered By:  " + Convert.ToString(dt.Rows[0]["EnteredBy"]));
                        ws.Cell("A" + 3).SetValue("Entry Date:  " + Convert.ToString(dt.Rows[0]["DealEntryDate"]));
                    }
                    Range = "A1:D1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.Bold = true;
                    Range = "A2:D2";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.Bold = true;
                    Range = "A3:D3";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.Bold = true;

                    //ws.Cell("A" + 1).SetValue("Deal Acknowledgement Report");
                    Range = "A1:G1";
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    //ws.Range(Range).Style.Fill.BackgroundColor = XLColor.LightGray;
                    ws.Cell("C" + StartIndexData).WorksheetColumn().Width = 35;
                    ws.Cell("D" + StartIndexData).WorksheetColumn().Width = 35;
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    if (ReportType == "Customer Master" || ReportType == "DealSlipentry")
                    {

                        ws.Column(6).Delete();
                        ws.Column(6).Delete();
                        ws.Column(6).Delete();
                        ws.Column(6).Delete();
                        ws.Column(6).Delete();
                    }
                    else
                    {
                        ws.Column(6).Delete();
                        ws.Column(6).Delete();
                        ws.Column(6).Delete();
                        ws.Column(6).Delete();
                    }


                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=AuditReport" + strdtTime + ".xlsx");
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
                else
                {
                    // ClientScript.RegisterStartupScript(this.GetType(), "Key", "alert('No Data Found.');", true);
                }
            }
            else
            {
                //  ClientScript.RegisterStartupScript(this.GetType(), "Key", "alert('No Data Found.');", true);
            }
        }
        catch (Exception ex)
        {

        }


    }
    public void ExportToExcel_MISSecurityAuditReport(DataTable dt, string FromDate, String ToDate, String ReportType)
    {
        try
        {
            XLWorkbook wb = new XLWorkbook();
            string sheetName = "";
            for (int Y = 0; Y < 3; Y++)
            {
                if (Y == 1)
                {
                    dt = (DataTable)HttpContext.Current.Session["MISCashFlowReport"];
                    if (dt == null)
                    {
                        sheetName = "Cash Flow Report";
                        IXLWorksheet ws = wb.Worksheets.Add(sheetName);
                        //ws.Table(0).ShowAutoFilter = false;
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
                                    sheetName = "CashFlow Report";
                                    IXLWorksheet ws = wb.Worksheets.Add(dt, sheetName);
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

                else if (Y == 2)
                {
                    {
                        dt = (DataTable)HttpContext.Current.Session["MISDetailsReport"];
                        if (dt == null)
                        {
                            sheetName = "MISDetailsReport";
                            IXLWorksheet ws = wb.Worksheets.Add(sheetName);
                            //ws.Table(0).ShowAutoFilter = false;    /*Remove AutoFilter From Excel Using ClosedXML C#*/
                        }
                        else
                        {
                            try
                            {
                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        //XLWorkbook wb = new XLWorkbook();
                                        IXLWorksheet ws = wb.Worksheets.Add("MISDetailsReport");

                                        //dynamic column formation
                                        string[] cols = new string[dt.Columns.Count];
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            string a = dt.Columns[c].ToString();
                                            cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                                        }
                                        char StartCharCols = 'A';
                                        int StartIndexCols = 5;
                                        //   ws.AutoFilter.Clear();
                                        for (int i = 1; i <= cols.Length; i++)
                                        {
                                            if (i == cols.Length)
                                            {
                                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                                ws.Cell(DataCell).Value = cols[i - 1];
                                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                                ws.Cell(DataCell).Style.Font.Bold = true;
                                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                            }
                                            else
                                            {
                                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                                ws.Cell(DataCell).Value = cols[i - 1];
                                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                                ws.Cell(DataCell).Style.Font.Bold = true;
                                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                                StartCharCols++;
                                            }
                                        }
                                        char StartCharData = 'A';
                                        int StartIndexData = 6;

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

                                        //ws.Cell("C" + 1).SetValue("From " + txt_FromDate.Text + " To " + txt_ToDate.Text);
                                        if (ReportType == "Customer Master")
                                        {
                                            ws.Cell("A" + 1).SetValue("Customer Name:  " + Convert.ToString(dt.Rows[0]["CustomerName"]) + "  PAN: " + Convert.ToString(dt.Rows[0]["PanNumber"]));
                                            ws.Cell("A" + 2).SetValue("Entered By:  " + Convert.ToString(dt.Rows[0]["EnteredBy"]));
                                            ws.Cell("A" + 3).SetValue("Entry Date:  " + Convert.ToString(dt.Rows[0]["CustomerEntryDate"]));
                                        }
                                        else if (ReportType == "Security Master")
                                        {
                                            ws.Cell("A" + 1).SetValue("Security Name:  " + Convert.ToString(dt.Rows[0]["SecurityName"]) + "  ISIN: " + Convert.ToString(dt.Rows[0]["NSDLAcNumber"]));
                                            ws.Cell("A" + 2).SetValue("Entered By:  " + Convert.ToString(dt.Rows[0]["EnteredBy"]));


                                        }

                                        Range = "A1:D1";
                                        ws.Range(Range).Merge();
                                        ws.Range(Range).Style.Font.Bold = true;
                                        Range = "A2:D2";
                                        ws.Range(Range).Merge();
                                        ws.Range(Range).Style.Font.Bold = true;
                                        Range = "A3:D3";
                                        ws.Range(Range).Merge();
                                        ws.Range(Range).Style.Font.Bold = true;

                                        //ws.Cell("A" + 1).SetValue("Deal Acknowledgement Report");
                                        Range = "A1:G1";
                                        ws.Range(Range).Style.Font.FontSize = 11;
                                        ws.Range(Range).Style.Font.Bold = true;
                                        ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                                        ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                        ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                        ws.Range(Range).Style.Font.FontColor = XLColor.White;
                                        //ws.Range(Range).Style.Fill.BackgroundColor = XLColor.LightGray;
                                        ws.Cell("C" + StartIndexData).WorksheetColumn().Width = 35;
                                        ws.Cell("D" + StartIndexData).WorksheetColumn().Width = 35;
                                        //string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                                        //strdtTime = strdtTime.Replace("PM", "");
                                        //strdtTime = strdtTime.Replace("AM", "");
                                        if (ReportType == "Customer Master")
                                        {

                                            ws.Column(6).Delete();
                                            ws.Column(6).Delete();
                                            ws.Column(6).Delete();
                                            ws.Column(6).Delete();
                                            ws.Column(6).Delete();
                                        }
                                        else
                                        {
                                            ws.Column(6).Delete();
                                            ws.Column(6).Delete();
                                            ws.Column(6).Delete();
                                            ws.Column(6).Delete();
                                        }

                                    }
                                }

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }

                else if (Y == 0)
                {
                    dt = (DataTable)HttpContext.Current.Session["MISSecurityReport"];
                    if (dt == null)
                    {
                        sheetName = "Security Report";
                        IXLWorksheet ws = wb.Worksheets.Add(sheetName);
                        //ws.Table(0).ShowAutoFilter = false;    /*Remove AutoFilter From Excel Using ClosedXML C#*/
                    }
                    else
                    {
                        try
                        {
                            if (dt != null)
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    //XLWorkbook wb = new XLWorkbook();
                                    IXLWorksheet ws = wb.Worksheets.Add("Security Report");

                                    //dynamic column formation
                                    string[] cols = new string[dt.Columns.Count];
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        string a = dt.Columns[c].ToString();
                                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                                    }
                                    char StartCharCols = 'A';
                                    int StartIndexCols = 5;
                                    //   ws.AutoFilter.Clear();
                                    for (int i = 1; i <= cols.Length; i++)
                                    {
                                        if (i == cols.Length)
                                        {
                                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                            ws.Cell(DataCell).Value = cols[i - 1];
                                            ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                            ws.Cell(DataCell).Style.Font.Bold = true;
                                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                        }
                                        else
                                        {
                                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                            ws.Cell(DataCell).Value = cols[i - 1];
                                            ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                            ws.Cell(DataCell).Style.Font.Bold = true;
                                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                            StartCharCols++;
                                        }
                                    }
                                    char StartCharData = 'A';
                                    int StartIndexData = 6;

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

                                    //ws.Cell("C" + 1).SetValue("From " + txt_FromDate.Text + " To " + txt_ToDate.Text);
                                    if (ReportType == "Customer Master")
                                    {
                                        ws.Cell("A" + 1).SetValue("Customer Name:  " + Convert.ToString(dt.Rows[0]["CustomerName"]) + "  PAN: " + Convert.ToString(dt.Rows[0]["PanNumber"]));
                                        ws.Cell("A" + 2).SetValue("Entered By:  " + Convert.ToString(dt.Rows[0]["EnteredBy"]));
                                        ws.Cell("A" + 3).SetValue("Entry Date:  " + Convert.ToString(dt.Rows[0]["CustomerEntryDate"]));
                                    }
                                    else if (ReportType == "Security Master")
                                    {
                                        ws.Cell("A" + 1).SetValue("Security Name:  " + Convert.ToString(dt.Rows[0]["SecurityName"]) + "  ISIN: " + Convert.ToString(dt.Rows[0]["NSDLAcNumber"]));
                                        ws.Cell("A" + 2).SetValue("Entered By:  " + Convert.ToString(dt.Rows[0]["EnteredBy"]));
                                        ws.Cell("A" + 3).SetValue("Entry Date:  " + Convert.ToString(dt.Rows[0]["SecEntryDate"]));


                                    }

                                    Range = "A1:D1";
                                    ws.Range(Range).Merge();
                                    ws.Range(Range).Style.Font.Bold = true;
                                    Range = "A2:D2";
                                    ws.Range(Range).Merge();
                                    ws.Range(Range).Style.Font.Bold = true;
                                    Range = "A3:D3";
                                    ws.Range(Range).Merge();
                                    ws.Range(Range).Style.Font.Bold = true;

                                    //ws.Cell("A" + 1).SetValue("Deal Acknowledgement Report");
                                    Range = "A1:G1";
                                    ws.Range(Range).Style.Font.FontSize = 11;
                                    ws.Range(Range).Style.Font.Bold = true;
                                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                                    //ws.Range(Range).Style.Fill.BackgroundColor = XLColor.LightGray;
                                    ws.Cell("C" + StartIndexData).WorksheetColumn().Width = 35;
                                    ws.Cell("D" + StartIndexData).WorksheetColumn().Width = 35;
                                    //string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                                    //strdtTime = strdtTime.Replace("PM", "");
                                    //strdtTime = strdtTime.Replace("AM", "");
                                    if (ReportType == "Customer Master")
                                    {

                                        ws.Column(6).Delete();
                                        ws.Column(6).Delete();
                                        ws.Column(6).Delete();
                                        ws.Column(6).Delete();
                                        ws.Column(6).Delete();
                                    }
                                    else
                                    {
                                        ws.Column(6).Delete();
                                        ws.Column(6).Delete();
                                        ws.Column(6).Delete();
                                        ws.Column(6).Delete();
                                        ws.Column(6).Delete();
                                    }

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
            HttpContext.Current.Session["MISSecurityReport"] = null;
            HttpContext.Current.Session["MISDetailsReport"] = null;
            HttpContext.Current.Session["MISCashFlowReport"] = null;

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
    public void ExportToExcel_MeetingDetails(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("CRM Meeting Details");
                    //Add new sheet

                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "UserName", "CustomerName", "Contacts", "Designation", "ExpectedDate");

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;
                    double FaceValue = 0.0, totalFaceValue = 0.0, PurcSettAmount = 0.0, totalPurcSettAmount = 0.0;
                    double SellSettAmount = 0.0, totalSellSettAmount = 0, profit = 0, totalprofit = 0;
                    // filling header with formatting and colors
                    StartCharCols = 'A';
                    StartIndexCols = StartIndexData;
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                        ws.Cell(DataCell).Value = cols[i - 1];
                        ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                        ws.Cell(DataCell).Style.Font.Bold = true;
                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                        ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                        StartCharCols++;
                    }

                    //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                    StartIndexData++;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string DataCell = StartCharData.ToString() + StartIndexData;
                            //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                            string a = dt.Rows[i][j].ToString();
                            a = a.Replace("&nbsp;", " ");
                            a = a.Replace("&amp;", "&");
                            //check if value is of integer type
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

                            //if (StartCharData.ToString() == "B")
                            //{
                            //    if (a != "")
                            //    {
                            //        FaceValue += Convert.ToDouble(a);
                            //        totalFaceValue += Convert.ToDouble(a);
                            //    }
                            //    else
                            //    {
                            //        FaceValue += 0;
                            //    }
                            //}
                            //if (StartCharData.ToString() == "G")
                            //{
                            //    if (a != "")
                            //    {
                            //        PurcSettAmount += Convert.ToDouble(a);
                            //        totalPurcSettAmount += Convert.ToDouble(a);
                            //    }
                            //    else
                            //    {
                            //        PurcSettAmount += 0;
                            //    }
                            //}
                            //if (StartCharData.ToString() == "L")
                            //{
                            //    if (a != "")
                            //    {
                            //        SellSettAmount += Convert.ToDouble(a);
                            //        totalSellSettAmount += Convert.ToDouble(a);
                            //    }
                            //    else
                            //    {
                            //        SellSettAmount += 0;
                            //    }
                            //}
                            //if (StartCharData.ToString() == "M")
                            //{
                            //    if (a != "")
                            //    {
                            //        profit += Convert.ToDouble(a);
                            //        totalprofit += Convert.ToDouble(a);
                            //    }
                            //    else
                            //    {
                            //        profit += 0;
                            //    }
                            //}
                            StartCharData++;
                        }
                        StartCharData = 'A';
                        StartIndexData++;
                    }

                    StartIndexData++;
                    //ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    //ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    //ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    //ws.Cell("B" + StartIndexData).SetValue(totalFaceValue);
                    //ws.Cell("B" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //ws.Cell("B" + StartIndexData).Style.Font.Bold = true;

                    //ws.Cell("G" + StartIndexData).SetValue(totalPurcSettAmount);
                    //ws.Cell("G" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //ws.Cell("G" + StartIndexData).Style.Font.Bold = true;

                    //ws.Cell("L" + StartIndexData).SetValue(totalSellSettAmount);
                    //ws.Cell("L" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //ws.Cell("L" + StartIndexData).Style.Font.Bold = true;

                    //ws.Cell("M" + StartIndexData).SetValue(totalprofit);
                    //ws.Cell("M" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //ws.Cell("M" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:F1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("CRM Meeting Details REPORT                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=MeetingDetails" + strdtTime + ".xlsx");
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
    public void ExportToExcel_MISCRM(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("CRM");
                    //Add new sheet

                    System.Data.DataView view_ = new System.Data.DataView(dt);
                    dt = view_.ToTable(false, "EntryDate", "DealerName", "CustomerName", "Person Met", "Designation", "AccompaniedBy", "Last Met", "Status", "TopicDiscussed", "Parameters", "Opportunity", "Summary Of Discussion", "ContactMode");

                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 2;
                    char StartCharData = 'A';
                    int StartIndexData = 3;
                    double FaceValue = 0.0, totalFaceValue = 0.0, PurcSettAmount = 0.0, totalPurcSettAmount = 0.0;
                    double SellSettAmount = 0.0, totalSellSettAmount = 0, profit = 0, totalprofit = 0;
                    // filling header with formatting and colors
                    StartCharCols = 'A';
                    StartIndexCols = StartIndexData;
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                        ws.Cell(DataCell).Value = cols[i - 1];
                        ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                        ws.Cell(DataCell).Style.Font.Bold = true;
                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                        ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                        StartCharCols++;
                    }

                    //DealerBranch = Convert.ToString(dtTarget.Rows[0]["DealerBranch"]);
                    StartIndexData++;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string DataCell = StartCharData.ToString() + StartIndexData;
                            //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                            string a = dt.Rows[i][j].ToString();
                            a = a.Replace("&nbsp;", " ");
                            a = a.Replace("&amp;", "&");
                            //check if value is of integer type
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

                            //if (StartCharData.ToString() == "B")
                            //{
                            //    if (a != "")
                            //    {
                            //        FaceValue += Convert.ToDouble(a);
                            //        totalFaceValue += Convert.ToDouble(a);
                            //    }
                            //    else
                            //    {
                            //        FaceValue += 0;
                            //    }
                            //}
                            //if (StartCharData.ToString() == "G")
                            //{
                            //    if (a != "")
                            //    {
                            //        PurcSettAmount += Convert.ToDouble(a);
                            //        totalPurcSettAmount += Convert.ToDouble(a);
                            //    }
                            //    else
                            //    {
                            //        PurcSettAmount += 0;
                            //    }
                            //}
                            //if (StartCharData.ToString() == "L")
                            //{
                            //    if (a != "")
                            //    {
                            //        SellSettAmount += Convert.ToDouble(a);
                            //        totalSellSettAmount += Convert.ToDouble(a);
                            //    }
                            //    else
                            //    {
                            //        SellSettAmount += 0;
                            //    }
                            //}
                            //if (StartCharData.ToString() == "M")
                            //{
                            //    if (a != "")
                            //    {
                            //        profit += Convert.ToDouble(a);
                            //        totalprofit += Convert.ToDouble(a);
                            //    }
                            //    else
                            //    {
                            //        profit += 0;
                            //    }
                            //}
                            StartCharData++;
                        }
                        StartCharData = 'A';
                        StartIndexData++;
                    }

                    StartIndexData++;
                    //ws.Cell("A" + StartIndexData).SetValue("Grand Total");
                    //ws.Cell("A" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    //ws.Cell("A" + StartIndexData).Style.Font.Bold = true;

                    //ws.Cell("B" + StartIndexData).SetValue(totalFaceValue);
                    //ws.Cell("B" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //ws.Cell("B" + StartIndexData).Style.Font.Bold = true;

                    //ws.Cell("G" + StartIndexData).SetValue(totalPurcSettAmount);
                    //ws.Cell("G" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //ws.Cell("G" + StartIndexData).Style.Font.Bold = true;

                    //ws.Cell("L" + StartIndexData).SetValue(totalSellSettAmount);
                    //ws.Cell("L" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //ws.Cell("L" + StartIndexData).Style.Font.Bold = true;

                    //ws.Cell("M" + StartIndexData).SetValue(totalprofit);
                    //ws.Cell("M" + StartIndexData).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    //ws.Cell("M" + StartIndexData).Style.Font.Bold = true;

                    string Range = "";
                    Range = "A1:F1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Cell("A" + 1).SetValue("CRM REPORT                 From   " + FromDate + "   To   " + ToDate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=CRMEntry" + strdtTime + ".xlsx");
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
    public void ExportToExcel_NSCCL_ICCLUploadReport(DataTable dt, string FromDate, string ToDate)
    {
        try
        {
            XLWorkbook wb = new XLWorkbook();
            string sheetName = "";
            //DataTable dt = new DataTable();


            for (int Y = 0; Y < 2; Y++)
            {
                if (Y == 1)
                {
                    dt = (DataTable)HttpContext.Current.Session["NSCCLUpload"];
                    if (dt == null)
                    {
                        //sheetName = "BalanceStock";
                        //IXLWorksheet ws = wb.Worksheets.Add(dt, sheetName);
                        //XLWorkbook wb = new XLWorkbook();
                        IXLWorksheet ws = wb.Worksheets.Add("sheet1");
                        ws.AutoFilter.Enabled = false;
                        ws.Columns().AdjustToContents();
                    }
                    else
                    {
                        try
                        {
                            if (dt != null)
                            {
                                if (dt.Rows.Count > 0)
                                {

                                    IXLWorksheet ws = wb.Worksheets.Add("NSCCLUpload");
                                    string[] cols = new string[dt.Columns.Count];
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        string a = dt.Columns[c].ToString();
                                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                                    }
                                    char StartCharCols = 'A';
                                    int StartIndexCols = 1;

                                    for (int i = 1; i <= cols.Length; i++)
                                    {
                                        if (i == cols.Length)
                                        {
                                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                            ws.Cell(DataCell).Value = cols[i - 1];
                                            ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                            ws.Cell(DataCell).Style.Font.Bold = true;
                                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                        }
                                        else
                                        {
                                            string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                            ws.Cell(DataCell).Value = cols[i - 1];
                                            ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                            ws.Cell(DataCell).Style.Font.Bold = true;
                                            ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                            ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                            StartCharCols++;
                                        }
                                    }
                                    char StartCharData = 'A';
                                    int StartIndexData = 2;

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
                                            if (StartCharData.ToString() == "C")
                                            {
                                                if (a == "")
                                                {
                                                    ws.Cell(DataCell).SetValue(a);
                                                }
                                                else
                                                {

                                                    ws.Cell(DataCell).Value = Convert.ToString(a);
                                                    ws.Cell(DataCell).SetValue(Convert.ToString(a));

                                                }
                                            }
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
                                    // Range = "A" + StartIndexData + ":" + "G" + StartIndexData;
                                    // ws.Range(Range).Merge();

                                    //ws.Cell("A" + 1).SetValue("Balance Stock Report From  " + FromDate + " To " + ToDate);


                                    //Range = "A1:C1";
                                    //ws.Range(Range).Style.Font.FontSize = 11;
                                    //ws.Range(Range).Style.Font.Bold = true;
                                    //ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                                    //ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    //ws.Range(Range).Style.Fill.BackgroundColor = XLColor.LightGray;
                                    //ws.Cell("C" + StartIndexData).WorksheetColumn().Width = 35;
                                    //ws.Cell("D" + StartIndexData).WorksheetColumn().Width = 35;
                                }//here
                            }

                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }

                else if (Y == 0)
                {
                    dt = (DataTable)HttpContext.Current.Session["ICCLUpload"];
                    if (dt == null)
                    {
                        sheetName = "ICCLUpload";
                        IXLWorksheet ws = wb.Worksheets.Add(dt, sheetName);
                    }
                    else
                    {

                        try
                        {
                            dt = (DataTable)HttpContext.Current.Session["ICCLUpload"];
                            if (dt != null)
                            {
                                if (dt.Rows.Count > 0)
                                {

                                    IXLWorksheet ws = wb.Worksheets.Add("ICCLUpload");
                                    string[] cols = new string[dt.Columns.Count];
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        string a = dt.Columns[c].ToString();
                                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                                    }
                                    char StartCharCols = 'A';
                                    int StartIndexCols = 1;
                                    char dblStartCharCols = 'A';
                                    int count = 0;
                                    for (int i = 1; i <= cols.Length; i++)
                                    {
                                        if (i == cols.Length && StartCharCols.ToString() != "[")
                                        {
                                            if (count > 0)
                                            {
                                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                                ws.Cell(DataCell).Value = cols[i - 1];
                                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                                ws.Cell(DataCell).Style.Font.Bold = true;
                                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                            }
                                            else
                                            {
                                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                                ws.Cell(DataCell).Value = cols[i - 1];
                                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                                ws.Cell(DataCell).Style.Font.Bold = true;
                                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                            }
                                        }
                                        else
                                        {
                                            if (StartCharCols.ToString() == "[" || count > 0)
                                            {
                                                count = 1;
                                                if (i % 27 == 0)
                                                {
                                                    StartCharCols = 'A';
                                                    if (i % 54 == 0)
                                                    {
                                                        dblStartCharCols++;
                                                    }
                                                }
                                                //dblStartCharCols++;
                                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                                ws.Cell(DataCell).Value = cols[i - 1];
                                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                                ws.Cell(DataCell).Style.Font.Bold = true;
                                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                                StartCharCols++;
                                            }
                                            else
                                            {
                                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                                ws.Cell(DataCell).Value = cols[i - 1];
                                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                                ws.Cell(DataCell).Style.Font.Bold = true;
                                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                                StartCharCols++;
                                            }
                                        }
                                    }
                                    char StartCharData = 'A';
                                    int StartIndexData = 2;

                                    char StartCharDataCol = char.MinValue;

                                    string Range = "";
                                    Range = "A" + StartIndexData + ":" + "G" + StartIndexData;
                                    // ws.Range(Range).Merge();

                                    //ws.Cell("A" + 1).SetValue("Profit / Loss Report From  " + FromDate + " To " + ToDate);


                                    //Range = "A1:C1";
                                    //ws.Range(Range).Style.Font.FontSize = 11;
                                    //ws.Range(Range).Style.Font.Bold = true;
                                    //ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                                    //ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    //ws.Range(Range).Style.Fill.BackgroundColor = XLColor.LightGray;
                                    //ws.Cell("C" + StartIndexData).WorksheetColumn().Width = 35;
                                    //ws.Cell("D" + StartIndexData).WorksheetColumn().Width = 35;

                                    int Ccout = 0;
                                    //char StartCharData = 'A';
                                    //int StartIndexData = 4;
                                    dblStartCharCols = 'A';


                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        for (int j = 0; j < dt.Columns.Count; j++)
                                        {
                                            if (StartCharData.ToString() == "[" || Ccout > 0)
                                            {
                                                Ccout = 1;
                                                if (j % 26 == 0)
                                                {
                                                    StartCharData = 'A';
                                                    if (j % 52 == 0)
                                                    {
                                                        dblStartCharCols++;
                                                    }
                                                }

                                                string DataCell = dblStartCharCols.ToString() + StartCharData.ToString() + StartIndexData;
                                                string a = dt.Rows[i][j].ToString();
                                                a = a.Replace("&nbsp;", " ");
                                                a = a.Replace("&amp;", "&");
                                                //check if value is of integer type
                                                int val = 0;
                                                double val2 = 0.0;
                                                DateTime dt2 = DateTime.Now;
                                                if (int.TryParse(a, out val))
                                                {
                                                    //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                                    ws.Cell(DataCell).Value = val;
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
                                            else
                                            {
                                                string DataCell = StartCharData.ToString() + StartIndexData;
                                                string a = dt.Rows[i][j].ToString();
                                                a = a.Replace("&nbsp;", " ");
                                                a = a.Replace("&amp;", "&");
                                                //check if value is of integer type
                                                int val = 0;
                                                double val2 = 0.0;
                                                DateTime dt2 = DateTime.Now;
                                                if (int.TryParse(a, out val))
                                                {
                                                    //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                                    ws.Cell(DataCell).Value = val;
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
                                        }
                                        StartCharData = 'A';
                                        StartIndexData++;
                                        Ccout = 0;
                                    }//here
                                }
                            }

                        }
                        catch (Exception ex)
                        {

                        }


                    }
                }//here



            }


            // Clear All the Session Here.....
            HttpContext.Current.Session["ICCLUpload"] = null;
            HttpContext.Current.Session["NSCCLUpload"] = null;



            //Code to save the file

            string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
            strdtTime = strdtTime.Replace("PM", "");
            strdtTime = strdtTime.Replace("AM", "");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=NSCCL_ICCLUploadReport" + strdtTime + ".xlsx");
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

    }

    public void ExportToExcel_PropDealRpt(DataTable dt, string FromDate, String ToDate)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("Prop Deals");
                    //Add new sheet



                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ');
                    }

                    //row 1 header
                    char StartCharCols = 'A';
                    int StartIndexCols = 3;
                    //   ws.AutoFilter.Clear();
                    char dblStartCharCols = 'A';
                    // filling header with formatting and colors
                    int count = 0;
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        if (i == cols.Length)
                        {
                            if (count > 0)
                            {
                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            }
                            else
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                            }
                        }
                        else
                        {
                            if (StartCharCols.ToString() == "[" || count > 0)
                            {
                                count = 1;
                                if (i % 27 == 0)
                                {
                                    StartCharCols = 'A';
                                    if (i % 54 == 0)
                                    {
                                        dblStartCharCols++;
                                    }
                                }
                                //dblStartCharCols++;
                                string DataCell = dblStartCharCols.ToString() + StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }
                            else
                            {
                                string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                                ws.Cell(DataCell).Value = cols[i - 1];
                                ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                                ws.Cell(DataCell).Style.Font.Bold = true;
                                ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                                ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                                StartCharCols++;
                            }
                        }
                    }

                    int Ccout = 0;
                    char StartCharData = 'A';
                    int StartIndexData = 4;
                    dblStartCharCols = 'A';

                    char StartCharDataCol = char.MinValue;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (StartCharData.ToString() == "[" || Ccout > 0)
                            {
                                Ccout = 1;
                                if (j % 26 == 0)
                                {
                                    StartCharData = 'A';
                                    if (j % 52 == 0)
                                    {
                                        dblStartCharCols++;
                                    }
                                }

                                string DataCell = dblStartCharCols.ToString() + StartCharData.ToString() + StartIndexData;
                                string a = dt.Rows[i][j].ToString();
                                a = a.Replace("&nbsp;", " ");
                                a = a.Replace("&amp;", "&");
                                //check if value is of integer type
                                int val = 0;
                                DateTime dt2 = DateTime.Now;
                                if (int.TryParse(a, out val))
                                {
                                    //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                    ws.Cell(DataCell).Value = val;
                                }
                                //check if datetime type
                                //else if (DateTime.TryParse(a, out dt2))
                                //{
                                //    ws.Cell(DataCell).Value = dt2.ToShortDateString();
                                //}
                                ws.Cell(DataCell).SetValue(a);
                                StartCharData++;
                            }
                            else
                            {
                                string DataCell = StartCharData.ToString() + StartIndexData;
                                string a = dt.Rows[i][j].ToString();
                                a = a.Replace("&nbsp;", " ");
                                a = a.Replace("&amp;", "&");
                                //check if value is of integer type
                                int val = 0;
                                DateTime dt2 = DateTime.Now;
                                if (int.TryParse(a, out val))
                                {
                                    //    ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 15;
                                    ws.Cell(DataCell).Value = val;
                                }
                                //check if datetime type
                                //else if (DateTime.TryParse(a, out dt2))
                                //{
                                //    ws.Cell(DataCell).Value = dt2.ToShortDateString();
                                //}
                                ws.Cell(DataCell).SetValue(a);
                                StartCharData++;
                            }
                        }
                        StartCharData = 'A';
                        StartIndexData++;
                        Ccout = 0;
                    }

                    //Code to save the file
                    string Range = "";
                    Range = "A1:D1";
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
                    ws.Cell("A" + 1).SetValue("Prop Deals From  " + FromDate + " To " + ToDate);
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=PropDeals." + strdtTime + ".xlsx");
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
                else
                {
                    // ClientScript.RegisterStartupScript(this.GetType(), "Key", "alert('No Data Found.');", true);
                }
            }
            else
            {
                //  ClientScript.RegisterStartupScript(this.GetType(), "Key", "alert('No Data Found.');", true);
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void ExportToExcel_WDMMisBrokerage(DataTable dt, string FromDate, String ToDate, String rptName)
    {
        XLWorkbook wb = new XLWorkbook();
        IXLWorksheet ws;
        string repttype;
        //Add new sheet

        ws = wb.Worksheets.Add(dt, "MISBrokerage");
        ws.Table(0).ShowAutoFilter = false;
        int dtCount = 0;
        double Totfacevalue = 0.0;
        double Totaccrdintrst = 0.0;
        dtCount = dt.Rows.Count + 2;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            //Totfacevalue += Convert.ToDouble(dt.Rows[i]["Facevalue"]);
            //Totaccrdintrst += Convert.ToDouble(dt.Rows[i]["AccruedInterest"]);
        }

        //ws.Cell("A" + dtCount).SetValue("Grand Total");
        //ws.Cell("F" + dtCount).SetValue(Totfacevalue);
        //ws.Cell("G" + dtCount).SetValue(Totaccrdintrst);

        //ws.Cell("A" + dtCount).Style.Font.Bold = true;
        //ws.Cell("F" + dtCount).Style.Font.Bold = true;
        //ws.Cell("G" + dtCount).Style.Font.Bold = true;

        string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
        strdtTime = strdtTime.Replace("PM", "");
        strdtTime = strdtTime.Replace("AM", "");
        string str = rptName;
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + str + strdtTime + ".xlsx");

        using (MemoryStream MyMemoryStream = new MemoryStream())
        {
            wb.SaveAs(MyMemoryStream);
            MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }

    public void ExportToExcel_MISInwardOutwardSecurityWiseStockReport(DataTable dt, Char InwardOutwardIndex, Char InwardOutwardLastIndex, string Security, int deleteIndex, string InwardOutwardRowIndex)
    {
        string ISINNo = "";
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws;
                    ws = wb.Worksheets.Add("InwardOutwardSecurity");
                    //Add new sheet
                    ISINNo = Convert.ToString(dt.Rows[1]["ISINNumber"].ToString());
                    char StartCharCols = 'A';
                    int StartIndexCols = 7;
                    string str_ColumnName = "";
                    int qty_Inward = 0, qty_Outward = 0, qty_Closing = 0;
                    decimal value_Inward = 0, value_Outward = 0, value_Closing = 0;
                    char total_qty_Inward = 'G', total_qty_Outward = 'I', total_qty_Closing = 'K';
                    char total_value_Inward = 'H', total_value_Outward = 'J', total_value_Closing = 'L';
                    string fromdate = dt.Rows[0]["FromDate"].ToString();
                    string todate = dt.Rows[0]["ToDate"].ToString();
                    int deleteIndex1 = Convert.ToInt32(dt.Rows[0]["DeleteIndex"].ToString());
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string DataCell = StartCharCols.ToString() + StartIndexCols;
                            //ws.Cell(DataCell).Style.Font.SetFontSize(8);
                            str_ColumnName = dt.Columns[j].ColumnName;
                            string a = dt.Rows[i][j].ToString();
                            a = a.Replace("&nbsp;", " ");
                            a = a.Replace("&amp;", "&");
                            SetBorder(ws, DataCell);
                            //check if value is of integer type
                            int val = 0;
                            double val2 = 0.0;
                            DateTime dt2 = DateTime.Now;
                            if (int.TryParse(a, out val))
                            {
                                // ws.Cell(DataCell).Style.NumberFormat.Format = "#####";
                                ws.Cell(DataCell).Value = val;
                                if (val > 0)
                                {

                                    if (str_ColumnName != "A1" && str_ColumnName != "A3"  && str_ColumnName != "A5")
                                    {
                                        ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.00";
                                        ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    }
                                    else
                                    {
                                        ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    }
                                    
                                     
                                    //ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val);
                                }
                                else { ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; ws.Cell(DataCell).Value = "-"; }
                                //ss ws.Cell(DataCell).SetValue(a);

                            }
                            else if (double.TryParse(a, out val2))
                            {
                                ws.Cell(DataCell).Value = val2;
                                if (val2 > 0)
                                {
                                    ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                                    if (str_ColumnName != "Rate")
                                    {
                                        ws.Cell(DataCell).Style.NumberFormat.Format = ReturnIndianFormat(val2);
                                        ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.00";
                                    }
                                    else {
                                        ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.0000";
                                    }

                                   

                                }
                                else
                                {
                                    ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    ws.Cell(DataCell).Value = "-";
                                }


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
                            if (str_ColumnName == "A1") { if (a != "") { qty_Inward += Convert.ToInt32(a); } }
                            if (str_ColumnName == "A3") { if (a != "") { qty_Outward += Convert.ToInt32(a); } }
                            if (str_ColumnName == "A5") { if (a != "") { qty_Closing += Convert.ToInt32(a); } }

                            if (str_ColumnName == "A2") { if (a != "") { value_Inward += Convert.ToDecimal(a); } }
                            if (str_ColumnName == "A4") { if (a != "") { value_Outward += Convert.ToDecimal(a); } }
                            if (str_ColumnName == "A6") { if (a != "") { value_Closing += Convert.ToDecimal(a); } }
                            StartCharCols++;
                        }
                        StartIndexCols++;
                        StartCharCols = 'A';
                    }

                    int index_Total = StartIndexCols;

                    StartIndexCols = 6;
                    StartCharCols = 'A';
                    //fetching columns from dt
                    string[] cols = new string[dt.Columns.Count];
                    //adding the  columns
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string a = dt.Columns[c].ToString();
                        cols[c] = dt.Columns[c].ToString().Replace('_', ' ').Replace("A1", "Quantity").Replace("A3", "Quantity").Replace("A5", "Quantity").Replace("A2", "Value").Replace("A4", "Value").Replace("A6", "Value");
                    }

                    //row 1 header
                    for (int i = 1; i <= cols.Length; i++)
                    {
                        string DataCell = StartCharCols.ToString() + StartIndexCols.ToString();
                        ws.Cell(DataCell).Value = cols[i - 1];
                        ws.Cell(DataCell).WorksheetColumn().Width = cols[i - 1].ToString().Length + 10;
                        ws.Cell(DataCell).Style.Font.Bold = true;
                        ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(36, 64, 98);
                        ws.Cell(DataCell).Style.Font.FontColor = XLColor.White;
                        ws.Cell(DataCell).Style.Font.FontName = "Cambria";
                        SetBorder(ws, DataCell);
                        StartCharCols++;
                    }


                    StartIndexCols++;
                    index_Total = index_Total + 1;
                    ws.Cell("E" + index_Total).SetValue("Total");
                    ws.Cell("E" + index_Total).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("E" + index_Total).Style.Font.Bold = true;
                    ws.Range("E" + index_Total).Style.Font.FontName = "Cambria";
                    ws.Range("A" + index_Total + ":F" + index_Total).Style.Font.FontColor = XLColor.White;
                    ws.Range("A" + index_Total + ":F" + index_Total).Style.Fill.BackgroundColor = XLColor.FromArgb(36, 64, 98);


                    ws.Cell(Convert.ToString(total_qty_Inward.ToString()) + index_Total).SetValue(Convert.ToString(qty_Inward.ToString()));
                    ws.Cell(Convert.ToString(total_qty_Inward.ToString()) + index_Total).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell(Convert.ToString(total_qty_Inward.ToString()) + index_Total).Style.Font.Bold = true;
                    ws.Range(Convert.ToString(total_qty_Inward.ToString()) + index_Total).Style.Font.FontName = "Cambria";
                    ws.Cell(Convert.ToString(total_qty_Inward.ToString()) + index_Total).Style.Fill.BackgroundColor = XLColor.Green;
                    ws.Cell(Convert.ToString(total_qty_Inward.ToString()) + index_Total).Style.Font.FontColor = XLColor.White;

                    ws.Cell(Convert.ToString(total_qty_Outward.ToString()) + index_Total).SetValue(Convert.ToString(qty_Outward.ToString()));
                    ws.Cell(Convert.ToString(total_qty_Outward.ToString()) + index_Total).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell(Convert.ToString(total_qty_Outward.ToString()) + index_Total).Style.Font.Bold = true;
                    ws.Range(Convert.ToString(total_qty_Outward.ToString()) + index_Total).Style.Font.FontName = "Cambria";
                    ws.Cell(Convert.ToString(total_qty_Outward.ToString()) + index_Total).Style.Fill.BackgroundColor = XLColor.Red;
                    ws.Cell(Convert.ToString(total_qty_Outward.ToString()) + index_Total).Style.Font.FontColor = XLColor.White;

                    int qty_closing1 = qty_Inward - qty_Outward;

                    ws.Cell(Convert.ToString(total_qty_Closing.ToString()) + index_Total).SetValue(Convert.ToString(qty_closing1.ToString()));
                    ws.Cell(Convert.ToString(total_qty_Closing.ToString()) + index_Total).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell(Convert.ToString(total_qty_Closing.ToString()) + index_Total).Style.Font.Bold = true;
                    ws.Range(Convert.ToString(total_qty_Closing.ToString()) + index_Total).Style.Font.FontName = "Cambria";
                    ws.Range(Convert.ToString(total_qty_Closing.ToString()) + index_Total).Style.Fill.BackgroundColor = XLColor.Green;
                    ws.Range(Convert.ToString(total_qty_Closing.ToString()) + index_Total).Style.Font.FontColor = XLColor.White;

                    ws.Cell(Convert.ToString(total_value_Inward.ToString()) + index_Total).SetValue(Convert.ToString(value_Inward.ToString()));
                    ws.Cell(Convert.ToString(total_value_Inward.ToString()) + index_Total).SetValue(value_Inward);
                    ws.Cell(Convert.ToString(total_value_Inward.ToString()) + index_Total).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell(Convert.ToString(total_value_Inward.ToString()) + index_Total).Style.Font.Bold = true;
                    ws.Range(Convert.ToString(total_value_Inward.ToString()) + index_Total).Style.Font.FontName = "Cambria";
                    ws.Range(Convert.ToString(total_value_Inward.ToString()) + index_Total).Style.NumberFormat.Format = "#,##0,0;-0;-;@";
                    ws.Range(Convert.ToString(total_value_Inward.ToString()) + index_Total).Style.Fill.BackgroundColor = XLColor.Green;
                    ws.Range(Convert.ToString(total_value_Inward.ToString()) + index_Total).Style.Font.FontColor = XLColor.White;

                    ws.Cell(Convert.ToString(total_value_Outward.ToString()) + index_Total).SetValue(value_Outward);
                    ws.Cell(Convert.ToString(total_value_Outward.ToString()) + index_Total).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell(Convert.ToString(total_value_Outward.ToString()) + index_Total).Style.Font.Bold = true;
                    ws.Range(Convert.ToString(total_value_Outward.ToString()) + index_Total).Style.Font.FontName = "Cambria";
                    ws.Range(Convert.ToString(total_value_Outward.ToString()) + index_Total).Style.NumberFormat.Format = "#,##0,0;-0;-;@";
                    ws.Range(Convert.ToString(total_value_Outward.ToString()) + index_Total).Style.Fill.BackgroundColor = XLColor.Red;
                    ws.Range(Convert.ToString(total_value_Outward.ToString()) + index_Total).Style.Font.FontColor = XLColor.White;
                    decimal value_Closing1 = value_Inward - value_Outward;


                    if (value_Closing1 <= 0)
                    {
                        ws.Cell(Convert.ToString(total_value_Closing.ToString()) + index_Total).SetValue("0");
                    }
                    else
                    {
                        ws.Cell(Convert.ToString(total_value_Closing.ToString()) + index_Total).SetValue(value_Closing1);
                    }
                    ws.Cell(Convert.ToString(total_value_Closing.ToString()) + index_Total).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    ws.Cell(Convert.ToString(total_value_Closing.ToString()) + index_Total).Style.Font.Bold = true;
                    ws.Range(Convert.ToString(total_value_Closing.ToString()) + index_Total).Style.Font.FontName = "Cambria";
                    ws.Range(Convert.ToString(total_value_Closing.ToString()) + index_Total).Style.NumberFormat.Format = "#,##0,0;-0;-;@";
                    ws.Range(Convert.ToString(total_value_Closing.ToString()) + index_Total).Style.Fill.BackgroundColor = XLColor.Green;
                    ws.Range(Convert.ToString(total_value_Closing.ToString()) + index_Total).Style.Font.FontColor = XLColor.White;


                    string Range = "";
                    Range = "A1:" + InwardOutwardLastIndex + "1";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 11;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(36, 64, 98);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;
                    ws.Range(Range).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    ws.Range(Range).Style.Border.BottomBorderColor = XLColor.White;
                    ws.Range(Range).SetValue("Stock Voucher  INWARD OUTWARD SECURITY WISE STOCK REPORT For ");
                    ws.Range(Range).Style.Font.FontName = "Cambria";


                    StartCharCols = InwardOutwardIndex;
                    int count = 1; string text = ""; char c2;
                    for (Char c = StartCharCols; c < InwardOutwardLastIndex; c++)
                    {
                        c2 = c++;
                        Range = c2 + InwardOutwardRowIndex + ":" + c + InwardOutwardRowIndex;
                        ws.Range(Range).Merge();
                        ws.Range(Range).Style.Font.FontSize = 11;
                        ws.Range(Range).Style.Font.Bold = true;
                        ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        if (count == 2)
                        {
                            ws.Range(Range).Style.Fill.BackgroundColor = XLColor.Red;
                        }
                        else
                        {
                            ws.Range(Range).Style.Fill.BackgroundColor = XLColor.Green;
                        }

                        ws.Range(Range).Style.Font.FontColor = XLColor.White;
                        ws.Range(Range).Style.Font.FontName = "Cambria";
                        ws.Range(Range).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        ws.Range(Range).Style.Border.BottomBorderColor = XLColor.White;
                        if (count == 1) { text = "Inwards"; } else if (count == 2) { text = "Outwards"; } else { text = "Closing"; }
                        ws.Range(Range).SetValue(text);
                        count++;
                    }

                    if (Security == "")
                    {
                        ws.Cell("A" + 2).SetValue("INWARD OUTWARD SECURITY WISE STOCK REPORT                 From   " + fromdate + "   To   " + todate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                        ws.Cell("A" + 2).Style.Font.FontSize = 11;
                        ws.Cell("A" + 2).Style.Font.Bold = true;
                        ws.Cell("A" + 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        ws.Cell("A" + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    }
                    else
                    {
                        ws.Cell("A" + 4).SetValue(" From   " + fromdate + "   To   " + todate + "                 Report Date :  " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                        ws.Cell("A" + 4).Style.Font.FontSize = 11;
                        ws.Cell("A" + 4).Style.Font.Bold = true;

                        ws.Cell("A" + 2).SetValue(Security);
                        ws.Cell("A" + 2).Style.Font.FontSize = 11;
                        ws.Cell("A" + 2).Style.Font.Bold = true;

                        ws.Cell("A" + 3).SetValue("ISIN NO: " + ISINNo);
                        ws.Cell("A" + 3).Style.Font.FontSize = 11;
                        ws.Cell("A" + 3).Style.Font.Bold = true;
                    }
                    Range = "A4:" + InwardOutwardLastIndex + "4";
                    ws.Range(Range).Merge();

                    Range = "A2:" + InwardOutwardLastIndex + "2";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(30, 95, 141);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;

                    Range = "A3:" + InwardOutwardLastIndex + "3";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(30, 95, 141);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;

                    Range = "A4:" + InwardOutwardLastIndex + "4";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(30, 95, 141);
                    ws.Range(Range).Style.Font.FontColor = XLColor.White;

                    Range = "A5:F5";
                    ws.Range(Range).Style.Fill.BackgroundColor = XLColor.FromArgb(30, 95, 141);
                    for (int I = 0; I < deleteIndex; I++)
                    {
                        ws.Column(deleteIndex1).Delete();
                    }

                    //ws.Range("A5:A6").Merge();
                    //ws.Range("B5:B6").Merge();
                    //ws.Range("C5:C6").Merge();
                    //ws.Range("D5:D6").Merge();
                    //ws.Range("E5:E6").Merge();
                    //ws.Range("F5:F6").Merge();
                    ws.Columns().AdjustToContents();
                    string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                    strdtTime = strdtTime.Replace("PM", "");
                    strdtTime = strdtTime.Replace("AM", "");
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=InwardOutwardSecurityWiseStock" + strdtTime + ".xlsx");
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
    public void SetBorder(IXLWorksheet ws, string datacell)
    {
        ws.Cell(datacell).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        ws.Cell(datacell).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        ws.Cell(datacell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        ws.Cell(datacell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
    }
    public void DownloadCashFlow_EXCEL(DataTable dtPrint, string SecurityName, string ISINNumber, string DealDate, string ValueDate, string Quantum, string Quantity, string Rate, string YTMAnn, string YTCAnn, string YTPAnn, string LastIPDate, string NoofDays, string PrincipleAmount, string AccruedInterest, string TotalConsidn, string StampDuty, string SettAmt, string AmtInWords, string NatureOfInstrument)
    {
        try
        {
            XLWorkbook wb = new XLWorkbook();
            IXLWorksheet ws = wb.Worksheets.Add("CashFlow");
            string DataCell;
            int RowIndex = 2;

            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Dear Sir / Madam,";
            ws.Cell(DataCell).Style.Font.FontName = "Calibri";
            ws.Cell(DataCell).Style.Font.FontSize = 12;

            RowIndex++;

            string Range = "B" + Convert.ToString(RowIndex) + ":" + "C" + Convert.ToString(RowIndex);
            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "DISCLAIMER : This Cash Flow has been prepared on the basis of IM / NSDL details available with us. Incase of any change / alteration / modification we are not liable for the same. Kindly confirm and cross check at your end then consider the same.";
            ws.Cell(DataCell).Style.Font.FontName = "Calibri";
            ws.Cell(DataCell).Style.Font.FontSize = 9;
            ws.Cell(DataCell).Style.Font.FontColor = XLColor.Red;
            ws.Range(Range).Merge();
            ws.Range(Range).Style.Alignment.WrapText = true;
            ws.Cell(DataCell).Style.Font.Bold = true;
            ws.Row(RowIndex).Height = 35;
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Cell(DataCell).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            RowIndex++;

            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Name of Security";
            ws.Cell(DataCell).Style.Font.Bold = true;
            SetDoubleBorder(ws, DataCell);

            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = SecurityName;
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Alignment.WrapText = true;
            ws.Cell(DataCell).Style.Font.Bold = true;

            RowIndex++;
            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "ISIN";
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = ISINNumber;
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            RowIndex++;
            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Deal Date";
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;


            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = DealDate;
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;
            ws.Cell(DataCell).Style.NumberFormat.Format = "dd-MMM-yy";

            RowIndex++;
            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Settlement Date";
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;


            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = ValueDate;
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;
            ws.Cell(DataCell).Style.NumberFormat.Format = "dd-MMM-yy";

            RowIndex++;
            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Quantum";
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = Quantum;
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;
            ws.Cell(DataCell).Style.Font.FontColor = XLColor.Blue;
            ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0";

            RowIndex++;
            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Quantity";
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = Quantity;
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            RowIndex++;
            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Rate";
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;


            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = Rate;
            ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.0000";
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;
            ws.Cell(DataCell).Style.Font.FontColor = XLColor.Blue;


            if (Convert.ToDecimal(YTMAnn) > 0 && NatureOfInstrument == "NP")
            {
                RowIndex++;
                DataCell = "B" + Convert.ToString(RowIndex);
                ws.Cell(DataCell).Value = "YTM Ann";
                ws.Cell(DataCell).Style.Font.Bold = true;
                SetDoubleBorder(ws, DataCell);

                DataCell = "C" + Convert.ToString(RowIndex);
                ws.Cell(DataCell).Value = Convert.ToDecimal(YTMAnn) + " %";
                ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.0000";
                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                SetDoubleBorder(ws, DataCell);
                ws.Cell(DataCell).Style.Font.Bold = true;
                ws.Cell(DataCell).Style.Font.FontColor = XLColor.Blue;
            }


            if (Convert.ToDecimal(YTCAnn) > 0 && NatureOfInstrument == "P")
            {
                RowIndex++;
                DataCell = "B" + Convert.ToString(RowIndex);
                ws.Cell(DataCell).Value = "YTC Ann";

                DataCell = "C" + Convert.ToString(RowIndex);
                ws.Cell(DataCell).Value = Convert.ToDecimal(YTCAnn) + " %";
                ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.00";
                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                SetDoubleBorder(ws, DataCell);
                ws.Cell(DataCell).Style.Font.Bold = true;
                ws.Cell(DataCell).Style.Font.FontColor = XLColor.Blue;
            }


            if (Convert.ToDecimal(YTPAnn) > 0)
            {
                RowIndex++;
                DataCell = "B" + Convert.ToString(RowIndex);
                ws.Cell(DataCell).Value = "YTP Ann";

                DataCell = "C" + Convert.ToString(RowIndex);
                ws.Cell(DataCell).Value = Convert.ToDecimal(YTPAnn) + " %";
                ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.00";
                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                SetDoubleBorder(ws, DataCell);
                ws.Cell(DataCell).Style.Font.Bold = true;
                ws.Cell(DataCell).Style.Font.FontColor = XLColor.Blue;
            }

            RowIndex++;
            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Last IP Date";
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = LastIPDate;
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;
            ws.Cell(DataCell).Style.NumberFormat.Format = "dd-MMM-yy";

            RowIndex++;
            DataCell = "B" + Convert.ToString(RowIndex);

            ws.Cell(DataCell).Value = "No of Days";
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            DataCell = "C" + Convert.ToString(RowIndex);

            if (Convert.ToDecimal(AccruedInterest) < 0)
            {
                ws.Cell(DataCell).Value = "-" + NoofDays;
            }
            else
            {
                ws.Cell(DataCell).Value = NoofDays;
            }
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            RowIndex++;
            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Principal Amount";
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = PrincipleAmount;
            ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.00";
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            RowIndex++;
            DataCell = "B" + Convert.ToString(RowIndex);
            if (Convert.ToDecimal(AccruedInterest) < 0)
            {
                ws.Cell(DataCell).Value = "Ex Interest";
            }
            else
            {
                ws.Cell(DataCell).Value = "Accrued Interest";
            }
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = AccruedInterest;
            ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.00";
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            RowIndex++;
            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Total Consideration";
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = TotalConsidn;
            ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.00";
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;
            ws.Cell(DataCell).Style.Font.FontColor = XLColor.Blue;

            RowIndex++;
            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Stamp Duty";
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = StampDuty;
            ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.00";
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            RowIndex++;
            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Settlement Amount";
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = SettAmt;
            ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.00";
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;
            ws.Cell(DataCell).Style.Font.FontColor = XLColor.Blue;

            RowIndex++;
            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Amount in Words";
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = AmtInWords;
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Alignment.WrapText = true;
            ws.Cell(DataCell).Style.Font.Bold = true;

            RowIndex++;
            RowIndex++;

            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Cash Flow";
            ws.Cell(DataCell).Style.Font.Bold = true;

            RowIndex++;
            RowIndex++;

            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Date";
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;

            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "Amount";
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            ws.Cell(DataCell).Style.Font.Bold = true;

            RowIndex++;
            int formulastartindex = RowIndex;
            int formulalastindex = 0;
            for (int I = 0; I < dtPrint.Rows.Count; I++)
            {
                DataCell = "B" + Convert.ToString(RowIndex);
                ws.Cell(DataCell).Value = Convert.ToString(dtPrint.Rows[I][0]);
                SetDoubleBorder(ws, DataCell);
                ws.Cell(DataCell).Style.Font.Bold = true;
                ws.Cell(DataCell).Style.NumberFormat.Format = "dd-MMM-yy";
                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                //ws.Cell(DataCell).Style.Font.Bold = true;

                DataCell = "C" + Convert.ToString(RowIndex);
                ws.Cell(DataCell).Value = Convert.ToString(dtPrint.Rows[I][1]);
                ws.Cell(DataCell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.00";
                ws.Cell(DataCell).Style.Font.Bold = true;
                if (I == 0 || I == 1)
                {
                    ws.Cell(DataCell).Style.Font.FontColor = XLColor.Blue;
                }

                SetDoubleBorder(ws, DataCell);
                //ws.Cell(DataCell).Style.Font.Bold = true;
                RowIndex++;
            }
            formulalastindex = RowIndex;

            //create your formula
            string XIRR = "=XIRR(C" + formulastartindex + ":" + "C" + formulalastindex + "," + "B" + formulastartindex + ":" + "B" + formulalastindex + ")";

            RowIndex++;
            DataCell = "B" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).Value = "XIRR";
            ws.Cell(DataCell).Style.Font.Bold = true;
            ws.Cell(DataCell).Style.Font.FontColor = XLColor.Blue;
            SetDoubleBorder(ws, DataCell);

            DataCell = "C" + Convert.ToString(RowIndex);
            ws.Cell(DataCell).FormulaA1 = XIRR;
            SetDoubleBorder(ws, DataCell);
            ws.Cell(DataCell).Style.Font.Bold = true;
            ws.Cell(DataCell).Style.Font.FontColor = XLColor.Blue;
            ws.Cell(DataCell).Style.NumberFormat.Format = "0.0000%";

            ws.Columns().AdjustToContents();
            ws.Column(2).Width = 20;
            ws.Column(3).Width = 70;
            ws.ShowGridLines = false;
            string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
            strdtTime = strdtTime.Replace("PM", "");
            strdtTime = strdtTime.Replace("AM", "");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=CashFlow" + strdtTime + ".xlsx");
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
    }
    public void ExportToExcel_Invoice(DataTable dt)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    
                    XLWorkbook wb = new XLWorkbook();
                    IXLWorksheet ws = wb.Worksheets.Add("sheet1");
                    string CompName = Convert.ToString(dt.Rows[0]["CompName"]);
                    string CompanyAddress1 = Convert.ToString(dt.Rows[0]["CompanyAddress1"]);
                    string CompanyAddress2 = Convert.ToString(dt.Rows[0]["CompanyAddress2"]);
                    string CompanyCity = Convert.ToString(dt.Rows[0]["CompanyCity"]);
                    string CompanyPinCode = Convert.ToString(dt.Rows[0]["CompanyPinCode"]);

                    string Client = Convert.ToString(dt.Rows[0]["DistributorName"]);
                    string CompanyGST = Convert.ToString(dt.Rows[0]["CompanyGST"]);
                    string PANNo = Convert.ToString(dt.Rows[0]["PANNo"]);
                    string CustomerGST = Convert.ToString(dt.Rows[0]["CustomerGST"]);
                    //string AdvStateCode = Convert.ToString(dt.Rows[0]["AdvStateCode"]);
                    string DebitMonthName = Convert.ToString(dt.Rows[0]["DebitMonth"]);
                    string DebitYear = Convert.ToString(dt.Rows[0]["DebitYear"]);
                    //string PlaceOfSupply = Convert.ToString(dt.Rows[0]["StateName"]);
                    string InvoiceNumber = Convert.ToString(dt.Rows[0]["RefNo"]);
                    string ClientAddress = dt.Rows[0]["CustomerAddress1"].ToString();
                    string ClientAddress1 = dt.Rows[0]["CustomerAddress2"].ToString();
                    string ClientCity = dt.Rows[0]["CustomerCity"].ToString();
                    string ClientPin = dt.Rows[0]["CustomerPinCode"].ToString();
                    double Amount = 0, CGSTAmt = 0, SGSTAmt = 0, IGSTAmt = 0, TotalAmt = 0;
                    //Amount = Convert.ToDouble(dt.Rows[0]["TaxableVal"]);
                    //CGSTAmt = Convert.ToDouble(dt.Rows[0]["CGSTAmt"]);
                    //SGSTAmt = Convert.ToDouble(dt.Rows[0]["SGSTAmt"]);
                    //IGSTAmt = Convert.ToDouble(dt.Rows[0]["IGSTAmt"]);
                    //TotalAmt = Amount + CGSTAmt + SGSTAmt + IGSTAmt;
                    TotalAmt = Convert.ToDouble(dt.Rows[0]["BrokerageAmount"]);
                    //string numberInwords = ConvertNumbertoWords(TotalAmt);
                    //dt.Columns.Remove("DealDate");
                    string TotalFeesInWords = Convert.ToString(dt.Rows[0]["AmtInWords"]);
                    string Range = "";

                    /*Merging Header*/
                    Range = "A4:F4";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 10;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Font.FontName = "Arial";
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Range(Range).Value = Client;
                    ws.Range(Range).Style.Font.FontColor = XLColor.Black;

                    Range = "A5:F5";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 10;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Font.FontName = "Arial";
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Range(Range).Value = ClientAddress;
                    ws.Range(Range).Style.Font.FontColor = XLColor.Black;

                    Range = "A6:F6";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 10;
                    ws.Range(Range).Style.Font.Bold = true;
                    ws.Range(Range).Style.Font.FontName = "Arial";
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Range(Range).Value = ClientAddress1;
                    ws.Range(Range).Style.Font.FontColor = XLColor.Black;


                    ws.Column(1).Width = 12;
                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 12;
                    ws.Column(4).Width = 12;
                    ws.Column(5).Width = 12;
                    ws.Column(6).Width = 12;

                    ws.Cell("A6").Value = "Invoice Date:";
                    //ws.Cell("B2").Value = System.DateTime.Today.ToString("dd-MMM-yy");
                    ws.Cell("B6").Value = dt.Rows[0]["ToDate"];
                    ws.Cell("B6").Style.NumberFormat.Format = "dd-MMM-yy";
                    ws.Cell("B6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("F6").Value = "Invoice No.";
                    ws.Cell("F6").Value = "'" + InvoiceNumber;
                    ws.Cell("F6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A8").Value = "To,";
                   

                    ws.Cell("A9").Value = CompName ;
                    ws.Cell("A10").Value = CompanyAddress1 ;
                    ws.Cell("A11").Value = CompanyAddress2 ;
                    ws.Cell("A12").Value = CompanyCity ;
                    ws.Cell("A12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Cell("A13").Value = CompanyPinCode ;
                    
                    ws.Cell("A13").Value = "PAN:";
                    ws.Cell("B13").Value = PANNo;
                    Range = "A18:F18";
                    ws.Cell(18, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(18, 2).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(18, 3).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(18, 4).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(18, 5).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(18, 6).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    ws.Cell(18, 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(18, 2).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(18, 3).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(18, 4).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(18, 5).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(18, 6).Style.Border.TopBorder = XLBorderStyleValues.Thin;

                    ws.Cell(18, 6).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    ws.Cell(28, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(28, 2).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(28, 3).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(28, 4).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(28, 5).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(28, 6).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    ws.Cell(28, 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(28, 2).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(28, 3).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(28, 4).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(28, 5).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(28, 6).Style.Border.TopBorder = XLBorderStyleValues.Thin;

                    ws.Cell(28, 6).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    ws.Cell("A16").Value = "Dear Madam / Sir";
                    ws.Cell("A18").Value = "Particulars";
                    ws.Cell("F18").Value = "Amount";
                    ws.Cell("A20").Value = "Being information & advisory in respect of Debt Market Instrument ";
                    ws.Cell("A21").Value = "Products & Research thereon for the month of " + DebitMonthName  + " " + DebitYear ;
                    ws.Cell("F20").Value = Amount;
                   

                  
                    ws.Cell("A28").Value = "Total Amount: " + TotalFeesInWords;
                    ws.Cell("A28").Style.Font.Bold = true;
                    ws.Cell("F28").Value = TotalAmt;
                   

                    ws.Cell("A32").Value = "PAN No";
                    ws.Cell("B32").Value = "GSTIN No";
                    Range = "C32:D32";
                    ws.Range(Range).Merge();
                    Range = "C33:D33";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 10;
                    ws.Range(Range).Style.Font.FontName = "Arial";
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Range(Range).Value = "State & Place of Supply";
                    ws.Range(Range).Style.Font.FontColor = XLColor.Black;

                    //Range = "E32:F32";
                    //ws.Range(Range).Merge();
                    //ws.Range(Range).Style.Font.FontSize = 10;
                    //ws.Range(Range).Style.Font.FontName = "Arial";
                    //ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    //ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //ws.Range(Range).Value = "State & Place of Supply";
                    //ws.Range(Range).Style.Font.FontColor = XLColor.Black;
                    //ws.Cell("C32").Value = "Service Accounting Code";

                    Range = "E33:F33";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 10;
                    ws.Range(Range).Style.Font.FontName = "Arial";
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Cell("A33").Value = "AAACS7095J";
                    ws.Cell("B33").Value = "27AAACS7095J1ZW";
                    ws.Cell("C33").Value = "997159";
                    ws.Cell("C33").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Cell("E33").Value = "       27 / Maharashtra";


                    ws.Cell(32, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(32, 2).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(32, 3).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(32, 4).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(32, 5).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(32, 6).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    ws.Cell(32, 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(32, 2).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(32, 3).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(32, 4).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(32, 5).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(32, 6).Style.Border.TopBorder = XLBorderStyleValues.Thin;

                    ws.Cell(32, 1).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(32, 2).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(32, 3).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(32, 4).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(32, 5).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(32, 6).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    ws.Cell(33, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(33, 2).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(33, 3).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(33, 4).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(33, 5).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(33, 6).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    ws.Cell(33, 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(33, 2).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(33, 3).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(33, 4).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(33, 5).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(33, 6).Style.Border.TopBorder = XLBorderStyleValues.Thin;

                    ws.Cell(33, 1).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(33, 2).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(33, 3).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(33, 4).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(33, 5).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(33, 6).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    ws.Cell("A35").Value = "Beneficiary Name: ";
                    ws.Cell("A35").Style.Font.Bold = true;

                    ws.Cell("A36").Value = "A/c No";
                    ws.Cell("A37").Value = "Bank Name";
                    ws.Cell("A38").Value = "IFSC Code ";

                    Range = "B36:C36";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 10;
                    ws.Range(Range).Style.Font.FontName = "Arial";
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    ws.Cell("B36").Value = "'" + "";

                    Range = "B37:C37";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 10;
                    ws.Range(Range).Style.Font.FontName = "Arial";
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Cell("B37").Value = "Kotak Mahindra Bank";

                    Range = "B38:C38";
                    ws.Range(Range).Merge();
                    ws.Range(Range).Style.Font.FontSize = 10;
                    ws.Range(Range).Style.Font.FontName = "Arial";
                    ws.Range(Range).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Range(Range).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Cell("B38").Value = "";

                    ws.Cell(36, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(36, 2).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(36, 3).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    ws.Cell(36, 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(36, 2).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(36, 3).Style.Border.TopBorder = XLBorderStyleValues.Thin;

                    ws.Cell(36, 1).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(36, 2).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(36, 3).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    ws.Cell(37, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(37, 2).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(37, 3).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    ws.Cell(37, 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(37, 2).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(37, 3).Style.Border.TopBorder = XLBorderStyleValues.Thin;

                    ws.Cell(37, 1).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(37, 2).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(37, 3).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    ws.Cell(38, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(38, 2).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Cell(38, 3).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    ws.Cell(38, 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(38, 2).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Cell(38, 3).Style.Border.TopBorder = XLBorderStyleValues.Thin;

                    ws.Cell(38, 1).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(38, 2).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Cell(38, 3).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    ws.Cell("A41").Value = "For ";
                    ws.Cell("A41").Style.Font.Bold = true;

                    ws.Cell("A45").Value = "Authorised Signatory";
                    ws.Cell("A45").Style.Font.Bold = true;
                    //Code to save the file
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Invoice.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.SuppressContent = true;
                    }
                }
                else
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Key", "alert('No Data Found.');", true);
                }
            }
            else
            {
                // ClientScript.RegisterStartupScript(this.GetType(), "Key", "alert('No Data Found.');", true);
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void SetDoubleBorder(IXLWorksheet ws, string datacell)
    {
        ws.Cell(datacell).Style.Border.TopBorder = XLBorderStyleValues.Double;
        ws.Cell(datacell).Style.Border.BottomBorder = XLBorderStyleValues.Double;
        ws.Cell(datacell).Style.Border.LeftBorder = XLBorderStyleValues.Double;
        ws.Cell(datacell).Style.Border.RightBorder = XLBorderStyleValues.Double;
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


