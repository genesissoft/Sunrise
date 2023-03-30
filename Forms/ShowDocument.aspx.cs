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
//using System.Xml.Linq;
using System.IO;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Collections.Generic;

public partial class Forms_ShowDocument : System.Web.UI.Page
{
    string strReportType = "";
    ClsCommon objComm = new ClsCommon();
    string condition = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
                SqlCommand sqlComm = new SqlCommand();

                sqlComm.Connection = sqlConn;
                sqlComm.CommandType = CommandType.Text;
                sqlComm.Parameters.Clear();
                if (string.IsNullOrEmpty(Request.QueryString["Type"]))
                {
                    sqlComm.CommandText = "select DocumentData As FileData,DocumentName As FileName,FileType From IssueDocDetails where IssueDocId=" + Request.QueryString["Id"].Trim();
                }
                else if (Request.QueryString["Type"].Trim() == "CDD")
                {
                    sqlComm.CommandText = "Select FileData,FileName,FileType From CustomerDocumentDetails Where CustomerDocumentId=" + Request.QueryString["Id"].Trim();
                }
                else if (Request.QueryString["Type"].Trim() == "SDD")
                {
                    sqlComm.CommandText = "Select FileData,FileName,FileType From SecurityDocumentDetails Where SecurityDocumentId=" + Request.QueryString["Id"].Trim();
                }
                else if (Request.QueryString["Type"].Trim() == "BDD")
                {
                    sqlComm.CommandText = "Select FileData ,FileName ,FileType From BrokerDocumentDetails Where BrokerDocumentId=" + Request.QueryString["Id"].Trim();
                }
                else if (Request.QueryString["Type"].Trim() == "DealAck")
                {
                    sqlComm.CommandText = "Select imgData as FileData ,imgTitle As FileName ,imgType as FileType From DealAckDocuments Where Dealslipid=" + Request.QueryString["Id"].Trim();
                }
                sqlConn.Open();
                SqlDataReader dr = sqlComm.ExecuteReader();
                dr.Read();

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = dr["FileName"].ToString();
                if (Request.QueryString["Type"].Trim() == "DealAck")
                {
                    Response.AddHeader("content-disposition", "attachment;filename=" + dr["FileName"].ToString()+ ".pdf");
                }

                else
                {
                    Response.AddHeader("content-disposition", "attachment;filename=" + dr["FileName"].ToString());
                }

                    
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite((byte[])dr["FileData"]);
                Response.End();

                if (sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                }
            }
            else if (Request.QueryString["Type"].Trim() != "")
            {
                strReportType = Request.QueryString["Type"].Trim();
                switch (strReportType)
                {
                    case "StockVerticalsSummary":
                    case "StockVerticalsDetails":
                    case "StockDetails":
                    case "TransferStock":
                    case "StockUpdate":
                    case "TransferSell":
                    case "FinancialVerticalSummary":
                    case "OtherStockVerticalSummary":
                        ExportDetails();
                        break;

                    case "DealDetails":
                        ExportDealDetails();
                        break;

                    case "HoldingExposure":
                        ExportExposureDetails();
                        break;

                    case "AgingInventory":
                        ExportAgingInventory();
                        break;
                    case "SellDownTracking":
                        ExportSelldownTracking();
                        break;

                    case "CompanyStockDetails":
                        ExportCompanyStockDetails();
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            //Response.Write(ex.Message);
        }
    }

    private void ExportAgingInventory()
    {
        DataSet ds = new DataSet();
        DataTable dtGesc = new DataTable();
        DataTable dtNonGesc = new DataTable();
        DataTable dtGescSummary = new DataTable();
        DataTable dtNonGescSummary = new DataTable();

        List<SqlParameter> lstParam = new List<SqlParameter>();
        string strFileName = "";
        int row = 1;
        int col = 1;
        int i;
        string[] Column = { "FV", "PC", "Quantity", "MTM", "PV01", "CreditPV01", "TC", "CC", "CTC", "CR" };
        string[] GSec = { "Rates (Face Value)", "Rates (Pur Consideration)", "Rates (Quantity)", "Rates (MTM)", "PV01-", "Credit PV01-", "Total Consideration", "Current Consideration", "Current Total Consideration", "Coupon Received" };
        string[] GNonSec = { "Credit (Face Value)", "Credit (Pur Consideration)", "Credit (Quantity)", "Credit (MTM)", "PV01-", "Credit PV01-", "Total Consideration", "Current Consideration", "Current Total Consideration", "Coupon Received" };

        try
        {
            lstParam.Add(new SqlParameter("@OpeningDate", objComm.Trim(Request.QueryString["fromdate"]) != "" ? objComm.Trim(Request.QueryString["fromdate"]) : null));
            lstParam.Add(new SqlParameter("@ClosingDate", objComm.Trim(Request.QueryString["todate"]) != "" ? objComm.Trim(Request.QueryString["todate"]) : null));
            lstParam.Add(new SqlParameter("@TempCompId", objComm.Trim(Request.QueryString["compid"]) != "" ? objComm.Trim(Request.QueryString["compid"]) : null));
            ds = objComm.FillDetails(lstParam, "TS_Export_AgingInventoryDetails");
            dtGescSummary = ds.Tables[0];
            dtNonGescSummary = ds.Tables[1];
            dtGesc = ds.Tables[2];
            dtNonGesc = ds.Tables[3];
            strFileName = "Aging of Inventory";

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet wsSummary = wb.Worksheets.Add("Summary");
                wsSummary.Column(1).Width = 25;
                wsSummary.Column(2).Width = 18;
                wsSummary.Column(3).Width = 18;
                wsSummary.Column(4).Width = 18;
                wsSummary.Column(5).Width = 18;
                wsSummary.Column(6).Width = 18;
                wsSummary.Column(7).Width = 18;
                wsSummary.Column(1).Style.Font.Bold = true;

                wsSummary.Cell(row, 1).Value = "Holding Time";
                wsSummary.Cell(row, 2).Value = ">Less Than 1 Wk";
                wsSummary.Cell(row, 3).Value = ">1 Wk - < 1 Mon";
                wsSummary.Cell(row, 4).Value = ">1 Mon - < 3 Mon";
                wsSummary.Cell(row, 5).Value = ">3 Mon - < 6 Mon";
                wsSummary.Cell(row, 6).Value = "Greater Than 6 Mon";
                wsSummary.Cell(row, 7).Value = "Total";

                wsSummary.Range("A" + row.ToString() + ":G" + row.ToString()).Style.Font.Bold = true;
                wsSummary.Range("A" + row.ToString() + ":G" + row.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                row++;


                wsSummary.Cell(row, 2).Value = "G-Sec";
                wsSummary.Cell(row, 2).Style.Font.Bold = true;
                wsSummary.Cell(row, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wsSummary.Range("B" + row.ToString() + ":G" + row.ToString()).Merge();
                wsSummary.Range("A" + row.ToString() + ":G" + row.ToString()).Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);
                row++;

                wsSummary.Range(row, 2, row + Column.Length, 7).Style.NumberFormat.Format = "#,##0.00";

                i = 0;
                foreach (string value in GSec)
                {
                    wsSummary.Cell(row, 1).Value = value;
                    if (!(dtGesc.Compute("SUM(" + Column[i] + "1)", "Range=1") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 2).Value = dtGesc.Compute("SUM(" + Column[i] + "1)", "Range=1");
                    }
                    else
                    {
                        wsSummary.Cell(row, 2).Value = 0;
                    }

                    if (!(dtGesc.Compute("SUM(" + Column[i] + "2)", "Range=2") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 3).Value = dtGesc.Compute("SUM(" + Column[i] + "2)", "Range=2");
                    }
                    else
                    {
                        wsSummary.Cell(row, 3).Value = 0;
                    }

                    if (!(dtGesc.Compute("SUM(" + Column[i] + "3)", "Range=3") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 4).Value = dtGesc.Compute("SUM(" + Column[i] + "3)", "Range=3");
                    }
                    else
                    {
                        wsSummary.Cell(row, 4).Value = 0;
                    }

                    if (!(dtGesc.Compute("SUM(" + Column[i] + "4)", "Range=4") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 5).Value = dtGesc.Compute("SUM(" + Column[i] + "4)", "Range=4");
                    }
                    else
                    {
                        wsSummary.Cell(row, 5).Value = 0;
                    }

                    if (!(dtGesc.Compute("SUM(" + Column[i] + "5)", "Range=5") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 6).Value = dtGesc.Compute("SUM(" + Column[i] + "5)", "Range=5");
                    }
                    else
                    {
                        wsSummary.Cell(row, 6).Value = 0;
                    }

                    if (value == "PV01-")
                    {
                        wsSummary.Range(row, 2, row, 7).Style.Font.FontColor = XLColor.Red;
                        wsSummary.Range(row + 2, 2, row + 2, 7).Style.Font.FontColor = XLColor.Red;
                        row++;
                    }
                    row++;
                    i++;
                }

                row++;

                wsSummary.Cell(row, 2).Value = "Non G-Sec";
                wsSummary.Cell(row, 2).Style.Font.Bold = true;
                wsSummary.Cell(row, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wsSummary.Range("B" + row.ToString() + ":G" + row.ToString()).Merge();
                wsSummary.Range("A" + row.ToString() + ":G" + row.ToString()).Style.Fill.BackgroundColor = XLColor.FromArgb(217, 151, 149);
                row++;

                wsSummary.Range(row, 2, row + Column.Length, 7).Style.NumberFormat.Format = "#,##0.00";

                i = 0;
                foreach (string value in GNonSec)
                {
                    wsSummary.Cell(row, 1).Value = value;
                    if (!(dtNonGesc.Compute("SUM(" + Column[i] + "1)", "Range=1") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 2).Value = dtNonGesc.Compute("SUM(" + Column[i] + "1)", "Range=1");
                    }
                    else
                    {
                        wsSummary.Cell(row, 2).Value = 0;
                    }

                    if (!(dtNonGesc.Compute("SUM(" + Column[i] + "2)", "Range=2") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 3).Value = dtNonGesc.Compute("SUM(" + Column[i] + "2)", "Range=2");
                    }
                    else
                    {
                        wsSummary.Cell(row, 3).Value = 0;
                    }

                    if (!(dtNonGesc.Compute("SUM(" + Column[i] + "3)", "Range=3") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 4).Value = dtNonGesc.Compute("SUM(" + Column[i] + "3)", "Range=3");
                    }
                    else
                    {
                        wsSummary.Cell(row, 4).Value = 0;
                    }

                    if (!(dtNonGesc.Compute("SUM(" + Column[i] + "4)", "Range=4") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 5).Value = dtNonGesc.Compute("SUM(" + Column[i] + "4)", "Range=4");
                    }
                    else
                    {
                        wsSummary.Cell(row, 5).Value = 0;
                    }

                    if (!(dtNonGesc.Compute("SUM(" + Column[i] + "5)", "Range=5") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 6).Value = dtNonGesc.Compute("SUM(" + Column[i] + "5)", "Range=5");
                    }
                    else
                    {
                        wsSummary.Cell(row, 6).Value = 0;
                    }

                    if (value == "PV01-")
                    {
                        wsSummary.Range(row, 2, row, 7).Style.Font.FontColor = XLColor.Red;
                        wsSummary.Range(row + 2, 2, row + 2, 7).Style.Font.FontColor = XLColor.Red;
                        row++;
                    }
                    row++;
                    i++;
                }

                wsSummary.Column(7).Style.Font.Bold = true;
                wsSummary.Cell(3, 7).FormulaA1 = "SUM(B3,C3,D3,E3,F3)";
                wsSummary.Cell(4, 7).FormulaA1 = "SUM(B4,C4,D4,E4,F4)";
                wsSummary.Cell(5, 7).FormulaA1 = "SUM(B5,C5,D5,E5,F5)";
                wsSummary.Cell(6, 7).FormulaA1 = "SUM(B6,C6,D6,E6,F6)";
                wsSummary.Cell(7, 7).FormulaA1 = "SUM(B7,C7,D7,E7,F7)";

                wsSummary.Cell(9, 7).FormulaA1 = "SUM(B9,C9,D9,E9,F9)";
                wsSummary.Cell(10, 7).FormulaA1 = "SUM(B10,C10,D10,E10,F10)";
                wsSummary.Cell(11, 7).FormulaA1 = "SUM(B11,C11,D11,E11,F11)";
                wsSummary.Cell(12, 7).FormulaA1 = "SUM(B12,C12,D12,E12,F12)";
                wsSummary.Cell(13, 7).FormulaA1 = "SUM(B13,C13,D13,E13,F13)";

                wsSummary.Cell(16, 7).FormulaA1 = "SUM(B16,C16,D16,E16,F16)";
                wsSummary.Cell(17, 7).FormulaA1 = "SUM(B17,C17,D17,E17,F17)";
                wsSummary.Cell(18, 7).FormulaA1 = "SUM(B18,C18,D18,E18,F18)";
                wsSummary.Cell(19, 7).FormulaA1 = "SUM(B19,C19,D19,E19,F19)";
                wsSummary.Cell(20, 7).FormulaA1 = "SUM(B20,C20,D20,E20,F20)";

                wsSummary.Cell(22, 7).FormulaA1 = "SUM(B22,C22,D22,E22,F22)";
                wsSummary.Cell(23, 7).FormulaA1 = "SUM(B23,C23,D23,E23,F23)";
                wsSummary.Cell(24, 7).FormulaA1 = "SUM(B24,C24,D24,E24,F24)";
                wsSummary.Cell(25, 7).FormulaA1 = "SUM(B25,C25,D25,E25,F25)";
                wsSummary.Cell(26, 7).FormulaA1 = "SUM(B26,C26,D26,E26,F26)";

                //Start Summay
                row = 1;
                IXLWorksheet wsGSecSummmary = wb.Worksheets.Add("G-Sec");

                col = 7;
                wsGSecSummmary.Cell(row, col).Value = "Less Than 1 Wk";
                wsGSecSummmary.Range(row, col, row, col + 9).Merge();
                col = col + 10;

                wsGSecSummmary.Cell(row, col).Value = ">1 Wk - < 1 Mon";
                wsGSecSummmary.Range(row, col, row, col + 9).Merge();
                col = col + 10;

                wsGSecSummmary.Cell(row, col).Value = ">1 Mon - < 3 Mon";
                wsGSecSummmary.Range(row, col, row, col + 9).Merge();
                col = col + 10;

                wsGSecSummmary.Cell(row, col).Value = ">3 Mon - < 6 Mon";
                wsGSecSummmary.Range(row, col, row, col + 9).Merge();
                col = col + 10;

                wsGSecSummmary.Cell(row, col).Value = "Greater Than 6 Mon";
                wsGSecSummmary.Range(row, col, row, col + 9).Merge();
                col = col + 10;

                wsGSecSummmary.Row(row).Style.Font.Bold = true;
                wsGSecSummmary.Row(row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                row++;
                wsGSecSummmary.Row(row).Style.Font.Bold = true;
                wsGSecSummmary.Cell(row, 1).Value = "Company Name";
                wsGSecSummmary.Column(1).Width = 40;
                wsGSecSummmary.Cell(row, 2).Value = "Security Name";
                wsGSecSummmary.Column(2).Width = 60;
                wsGSecSummmary.Cell(row, 3).Value = "ISIN";
                wsGSecSummmary.Column(3).Width = 15;
                wsGSecSummmary.Cell(row, 4).Value = "Rating";
                wsGSecSummmary.Column(4).Width = 15;
                wsGSecSummmary.Cell(row, 5).Value = "Pre IP Date";
                wsGSecSummmary.Column(5).Width = 15;
                wsGSecSummmary.Cell(row, 6).Value = "Interest Frequency";
                wsGSecSummmary.Column(6).Width = 15;
                i = 0;
                col = 7;
                for (i = 1; i <= 5; i++)
                {
                    wsGSecSummmary.Cell(row, col).Value = "Face Value";
                    wsGSecSummmary.Column(col).Width = 15;
                    wsGSecSummmary.Cell(row, col + 1).Value = "Pur Consideration";
                    wsGSecSummmary.Column(col + 1).Width = 20;
                    wsGSecSummmary.Cell(row, col + 2).Value = "Quantity";
                    wsGSecSummmary.Column(col + 2).Width = 15;
                    wsGSecSummmary.Cell(row, col + 3).Value = "Total Consideration";
                    wsGSecSummmary.Column(col + 3).Width = 20;
                    wsGSecSummmary.Cell(row, col + 4).Value = "Current Consideration";
                    wsGSecSummmary.Column(col + 4).Width = 20;
                    wsGSecSummmary.Cell(row, col + 5).Value = "MTM";
                    wsGSecSummmary.Column(col + 5).Width = 15;
                    wsGSecSummmary.Cell(row, col + 6).Value = "Total Current Consideration";
                    wsGSecSummmary.Column(col + 6).Width = 20;
                    wsGSecSummmary.Cell(row, col + 7).Value = "Coupon Received";
                    wsGSecSummmary.Column(col + 7).Width = 20;
                    wsGSecSummmary.Cell(row, col + 8).Value = "PV01-";
                    wsGSecSummmary.Column(col + 8).Width = 15;
                    wsGSecSummmary.Cell(row, col + 9).Value = "Credit PV01-";
                    wsGSecSummmary.Column(col + 9).Width = 15;
                    wsGSecSummmary.Range(row + 1, col + 8, row + dtGesc.Rows.Count + 1, col + 9).Style.Font.FontColor = XLColor.Red;

                    if (i == 1)
                    {
                        wsGSecSummmary.Range(row - 1, col, row, col + 9).Style.Fill.BackgroundColor = XLColor.FromArgb(242, 221, 220);
                    }
                    else if (i == 2)
                    {
                        wsGSecSummmary.Range(row - 1, col, row, col + 9).Style.Fill.BackgroundColor = XLColor.FromArgb(215, 228, 188);
                    }
                    else if (i == 3)
                    {
                        wsGSecSummmary.Range(row - 1, col, row, col + 9).Style.Fill.BackgroundColor = XLColor.FromArgb(204, 192, 218);
                    }
                    else if (i == 4)
                    {
                        wsGSecSummmary.Range(row - 1, col, row, col + 9).Style.Fill.BackgroundColor = XLColor.FromArgb(182, 221, 232);
                    }
                    else if (i == 5)
                    {
                        wsGSecSummmary.Range(row - 1, col, row, col + 9).Style.Fill.BackgroundColor = XLColor.FromArgb(252, 213, 180);
                    }
                    wsGSecSummmary.Range(row - 1, col, row, col + 9).Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
                    wsGSecSummmary.Range(row - 1, col, row, col + 9).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                    wsGSecSummmary.Range(row - 1, col, row, col + 9).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
                    wsGSecSummmary.Range(row - 1, col, row, col + 9).Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
                    col = col + 10;
                }

                row++;
                dtGescSummary.Columns.Remove("Range");

                wsGSecSummmary.Cell(row, 1).InsertTable(dtGescSummary);
                wsGSecSummmary.Tables.Table(0).ShowAutoFilter = false;
                wsGSecSummmary.Tables.Table(0).Theme = XLTableTheme.None;
                wsGSecSummmary.SheetView.FreezeRows(1);
                wsGSecSummmary.SheetView.FreezeRows(2);
                wsGSecSummmary.Row(3).Hide();

                row++;
                wsGSecSummmary.Range(row, 6, row + dtGescSummary.Rows.Count, dtGescSummary.Columns.Count).Style.NumberFormat.Format = "#,##0.00";
                row = row + ((dtGescSummary.Rows.Count > 0) ? dtGescSummary.Rows.Count : 1);
                wsGSecSummmary.Table(0).ShowTotalsRow = true;
                wsGSecSummmary.Table(0).Field(0).TotalsRowLabel = "Total:";
                for (i = 6; i <= dtGescSummary.Columns.Count - 1; i++)
                {
                    wsGSecSummmary.Table(0).Field(i).TotalsRowFunction = XLTotalsRowFunction.Sum;
                }
                wsGSecSummmary.Row(row).Style.Font.Bold = true;


                row = 1;
                IXLWorksheet wsNonGSecSummary = wb.Worksheets.Add("Non G-Sec");

                col = 7;
                wsNonGSecSummary.Cell(row, col).Value = "Less Than 1 Wk";
                wsNonGSecSummary.Range(row, col, row, col + 9).Merge();
                col = col + 10;

                wsNonGSecSummary.Cell(row, col).Value = ">1 Wk - < 1 Mon";
                wsNonGSecSummary.Range(row, col, row, col + 9).Merge();
                col = col + 10;

                wsNonGSecSummary.Cell(row, col).Value = ">1 Mon - < 3 Mon";
                wsNonGSecSummary.Range(row, col, row, col + 9).Merge();
                col = col + 10;

                wsNonGSecSummary.Cell(row, col).Value = ">3 Mon - < 6 Mon";
                wsNonGSecSummary.Range(row, col, row, col + 9).Merge();
                col = col + 10;

                wsNonGSecSummary.Cell(row, col).Value = "Greater Than 6 Mon";
                wsNonGSecSummary.Range(row, col, row, col + 9).Merge();
                col = col + 10;

                wsNonGSecSummary.Row(row).Style.Font.Bold = true;
                wsNonGSecSummary.Row(row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                row++;
                wsNonGSecSummary.Row(row).Style.Font.Bold = true;
                wsNonGSecSummary.Cell(row, 1).Value = "Company Name";
                wsNonGSecSummary.Column(1).Width = 40;
                wsNonGSecSummary.Cell(row, 2).Value = "Security Name";
                wsNonGSecSummary.Column(2).Width = 60;
                wsNonGSecSummary.Cell(row, 3).Value = "ISIN";
                wsNonGSecSummary.Column(3).Width = 15;
                wsNonGSecSummary.Cell(row, 4).Value = "Rating";
                wsNonGSecSummary.Column(4).Width = 15;
                wsNonGSecSummary.Cell(row, 5).Value = "Pre IP Date";
                wsNonGSecSummary.Column(5).Width = 15;
                wsNonGSecSummary.Cell(row, 6).Value = "Interest Frequency";
                wsNonGSecSummary.Column(6).Width = 15;
                i = 0;
                col = 7;
                for (i = 1; i <= 5; i++)
                {
                    wsNonGSecSummary.Cell(row, col).Value = "Face Value";
                    wsNonGSecSummary.Column(col).Width = 15;
                    wsNonGSecSummary.Cell(row, col + 1).Value = "Pur Consideration";
                    wsNonGSecSummary.Column(col + 1).Width = 20;
                    wsNonGSecSummary.Cell(row, col + 2).Value = "Quantity";
                    wsNonGSecSummary.Column(col + 2).Width = 15;
                    wsNonGSecSummary.Cell(row, col + 3).Value = "Total Consideration";
                    wsNonGSecSummary.Column(col + 3).Width = 20;
                    wsNonGSecSummary.Cell(row, col + 4).Value = "Current Consideration";
                    wsNonGSecSummary.Column(col + 4).Width = 20;
                    wsNonGSecSummary.Cell(row, col + 5).Value = "MTM";
                    wsNonGSecSummary.Column(col + 5).Width = 15;
                    wsNonGSecSummary.Cell(row, col + 6).Value = "Total Current Consideration";
                    wsNonGSecSummary.Column(col + 6).Width = 20;
                    wsNonGSecSummary.Cell(row, col + 7).Value = "Coupon Received";
                    wsNonGSecSummary.Column(col + 7).Width = 20;
                    wsNonGSecSummary.Cell(row, col + 8).Value = "PV01-";
                    wsNonGSecSummary.Column(col + 8).Width = 15;
                    wsNonGSecSummary.Cell(row, col + 9).Value = "Credit PV01-";
                    wsNonGSecSummary.Column(col + 9).Width = 15;
                    wsNonGSecSummary.Range(row + 1, col + 8, row + dtGesc.Rows.Count + 1, col + 9).Style.Font.FontColor = XLColor.Red;

                    if (i == 1)
                    {
                        wsNonGSecSummary.Range(row - 1, col, row, col + 9).Style.Fill.BackgroundColor = XLColor.FromArgb(242, 221, 220);
                    }
                    else if (i == 2)
                    {
                        wsNonGSecSummary.Range(row - 1, col, row, col + 9).Style.Fill.BackgroundColor = XLColor.FromArgb(215, 228, 188);
                    }
                    else if (i == 3)
                    {
                        wsNonGSecSummary.Range(row - 1, col, row, col + 9).Style.Fill.BackgroundColor = XLColor.FromArgb(204, 192, 218);
                    }
                    else if (i == 4)
                    {
                        wsNonGSecSummary.Range(row - 1, col, row, col + 9).Style.Fill.BackgroundColor = XLColor.FromArgb(182, 221, 232);
                    }
                    else if (i == 5)
                    {
                        wsNonGSecSummary.Range(row - 1, col, row, col + 9).Style.Fill.BackgroundColor = XLColor.FromArgb(252, 213, 180);
                    }
                    wsNonGSecSummary.Range(row - 1, col, row, col + 9).Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
                    wsNonGSecSummary.Range(row - 1, col, row, col + 9).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                    wsNonGSecSummary.Range(row - 1, col, row, col + 9).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
                    wsNonGSecSummary.Range(row - 1, col, row, col + 9).Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
                    col = col + 10;
                }

                row++;
                dtNonGescSummary.Columns.Remove("Range");

                wsNonGSecSummary.Cell(row, 1).InsertTable(dtNonGescSummary);
                wsNonGSecSummary.Tables.Table(0).ShowAutoFilter = false;
                wsNonGSecSummary.Tables.Table(0).Theme = XLTableTheme.None;
                wsNonGSecSummary.SheetView.FreezeRows(1);
                wsNonGSecSummary.SheetView.FreezeRows(2);
                wsNonGSecSummary.Row(3).Hide();

                row++;
                wsNonGSecSummary.Range(row, 6, row + dtNonGescSummary.Rows.Count, dtNonGescSummary.Columns.Count).Style.NumberFormat.Format = "#,##0.00";
                row = row + ((dtNonGescSummary.Rows.Count > 0) ? dtNonGescSummary.Rows.Count : 1);
                wsNonGSecSummary.Table(0).ShowTotalsRow = true;
                wsNonGSecSummary.Table(0).Field(0).TotalsRowLabel = "Total:";
                for (i = 6; i <= dtNonGescSummary.Columns.Count - 1; i++)
                {
                    wsNonGSecSummary.Table(0).Field(i).TotalsRowFunction = XLTotalsRowFunction.Sum;
                }
                wsNonGSecSummary.Row(row).Style.Font.Bold = true;

                //End Summary

                row = 1;
                IXLWorksheet wsGSec = wb.Worksheets.Add("G-Sec (Details)");

                col = 9;
                wsGSec.Cell(row, col).Value = "Less Than 1 Wk";
                wsGSec.Range(row, col, row, col + 11).Merge();
                col = col + 12;

                wsGSec.Cell(row, col).Value = ">1 Wk - < 1 Mon";
                wsGSec.Range(row, col, row, col + 11).Merge();
                col = col + 12;

                wsGSec.Cell(row, col).Value = ">1 Mon - < 3 Mon";
                wsGSec.Range(row, col, row, col + 11).Merge();
                col = col + 12;

                wsGSec.Cell(row, col).Value = ">3 Mon - < 6 Mon";
                wsGSec.Range(row, col, row, col + 11).Merge();
                col = col + 12;

                wsGSec.Cell(row, col).Value = "Greater Than 6 Mon";
                wsGSec.Range(row, col, row, col + 11).Merge();
                col = col + 12;

                wsGSec.Row(row).Style.Font.Bold = true;
                wsGSec.Row(row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                row++;
                wsGSec.Row(row).Style.Font.Bold = true;
                wsGSec.Cell(row, 1).Value = "Company Name";
                wsGSec.Column(1).Width = 40;
                wsGSec.Cell(row, 2).Value = "Security Name";
                wsGSec.Column(2).Width = 60;
                wsGSec.Cell(row, 3).Value = "ISIN";
                wsGSec.Column(3).Width = 15;
                wsGSec.Cell(row, 4).Value = "Deal No";
                wsGSec.Column(4).Width = 15;
                wsGSec.Cell(row, 5).Value = "Deal Date";
                wsGSec.Column(5).Width = 15;
                wsGSec.Cell(row, 6).Value = "Rating";
                wsGSec.Column(6).Width = 15;
                wsGSec.Cell(row, 7).Value = "Pre IP Date";
                wsGSec.Column(7).Width = 15;
                wsGSec.Cell(row, 8).Value = "Interest Frequency";
                wsGSec.Column(8).Width = 15;
                i = 0;
                col = 9;
                for (i = 1; i <= 5; i++)
                {
                    wsGSec.Cell(row, col).Value = "Face Value";
                    wsGSec.Column(col).Width = 15;
                    wsGSec.Cell(row, col + 1).Value = "Rate";
                    wsGSec.Column(col + 1).Width = 15;
                    wsGSec.Cell(row, col + 2).Value = "Pur Consideration";
                    wsGSec.Column(col + 2).Width = 20;
                    wsGSec.Cell(row, col + 3).Value = "Quantity";
                    wsGSec.Column(col + 3).Width = 15;
                    wsGSec.Cell(row, col + 4).Value = "Total Consideration";
                    wsGSec.Column(col + 4).Width = 20;
                    wsGSec.Cell(row, col + 5).Value = "Market Rate";
                    wsGSec.Column(col + 5).Width = 15;
                    wsGSec.Cell(row, col + 6).Value = "Current Consideration";
                    wsGSec.Column(col + 6).Width = 20;
                    wsGSec.Cell(row, col + 7).Value = "MTM";
                    wsGSec.Column(col + 7).Width = 15;
                    wsGSec.Cell(row, col + 8).Value = "Total Current Consideration";
                    wsGSec.Column(col + 8).Width = 20;
                    wsGSec.Cell(row, col + 9).Value = "Coupon Received";
                    wsGSec.Column(col + 9).Width = 20;
                    wsGSec.Cell(row, col + 10).Value = "PV01-";
                    wsGSec.Column(col + 10).Width = 15;
                    wsGSec.Cell(row, col + 11).Value = "Credit PV01-";
                    wsGSec.Column(col + 11).Width = 15;
                    wsGSec.Range(row + 1, col + 10, row + dtGesc.Rows.Count + 1, col + 11).Style.Font.FontColor = XLColor.Red;

                    if (i == 1)
                    {
                        wsGSec.Range(row - 1, col, row, col + 11).Style.Fill.BackgroundColor = XLColor.FromArgb(242, 221, 220);
                    }
                    else if (i == 2)
                    {
                        wsGSec.Range(row - 1, col, row, col + 11).Style.Fill.BackgroundColor = XLColor.FromArgb(215, 228, 188);
                    }
                    else if (i == 3)
                    {
                        wsGSec.Range(row - 1, col, row, col + 11).Style.Fill.BackgroundColor = XLColor.FromArgb(204, 192, 218);
                    }
                    else if (i == 4)
                    {
                        wsGSec.Range(row - 1, col, row, col + 11).Style.Fill.BackgroundColor = XLColor.FromArgb(182, 221, 232);
                    }
                    else if (i == 5)
                    {
                        wsGSec.Range(row - 1, col, row, col + 11).Style.Fill.BackgroundColor = XLColor.FromArgb(252, 213, 180);
                    }
                    wsGSec.Range(row - 1, col, row, col + 11).Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
                    wsGSec.Range(row - 1, col, row, col + 11).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                    wsGSec.Range(row - 1, col, row, col + 11).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
                    wsGSec.Range(row - 1, col, row, col + 11).Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
                    col = col + 12;
                }

                row++;
                dtGesc.Columns.Remove("Range");

                wsGSec.Cell(row, 1).InsertTable(dtGesc);
                wsGSec.Tables.Table(0).ShowAutoFilter = false;
                wsGSec.Tables.Table(0).Theme = XLTableTheme.None;
                wsGSec.SheetView.FreezeRows(1);
                wsGSec.SheetView.FreezeRows(2);
                wsGSec.Row(3).Hide();

                row++;
                wsGSec.Range(row, 6, row + dtGesc.Rows.Count, dtGesc.Columns.Count).Style.NumberFormat.Format = "#,##0.00";
                row = row + ((dtGesc.Rows.Count > 0) ? dtGesc.Rows.Count : 1);
                wsGSec.Table(0).ShowTotalsRow = true;
                wsGSec.Table(0).Field(0).TotalsRowLabel = "Total:";
                for (i = 8; i <= dtGesc.Columns.Count - 1; i++)
                {
                    if (i != 9 && i != 13 && i != 21 && i != 25 && i != 33 && i != 37 && i != 45 && i != 49 && i != 57 && i != 61)
                    {
                        wsGSec.Table(0).Field(i).TotalsRowFunction = XLTotalsRowFunction.Sum;
                    }
                }
                wsGSec.Row(row).Style.Font.Bold = true;


                row = 1;
                IXLWorksheet wsNonGSec = wb.Worksheets.Add("Non G-Sec (Details)");

                col = 9;
                wsNonGSec.Cell(row, col).Value = "Less Than 1 Wk";
                wsNonGSec.Range(row, col, row, col + 11).Merge();
                col = col + 12;

                wsNonGSec.Cell(row, col).Value = ">1 Wk - < 1 Mon";
                wsNonGSec.Range(row, col, row, col + 11).Merge();
                col = col + 12;

                wsNonGSec.Cell(row, col).Value = ">1 Mon - < 3 Mon";
                wsNonGSec.Range(row, col, row, col + 11).Merge();
                col = col + 12;

                wsNonGSec.Cell(row, col).Value = ">3 Mon - < 6 Mon";
                wsNonGSec.Range(row, col, row, col + 11).Merge();
                col = col + 12;

                wsNonGSec.Cell(row, col).Value = "Greater Than 6 Mon";
                wsNonGSec.Range(row, col, row, col + 11).Merge();
                col = col + 12;

                wsNonGSec.Row(row).Style.Font.Bold = true;
                wsNonGSec.Row(row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                row++;
                wsNonGSec.Row(row).Style.Font.Bold = true;
                wsNonGSec.Cell(row, 1).Value = "Company Name";
                wsNonGSec.Column(1).Width = 40;
                wsNonGSec.Cell(row, 2).Value = "Security Name";
                wsNonGSec.Column(2).Width = 60;
                wsNonGSec.Cell(row, 3).Value = "ISIN";
                wsNonGSec.Column(3).Width = 15;
                wsNonGSec.Cell(row, 4).Value = "Deal No";
                wsNonGSec.Column(4).Width = 15;
                wsNonGSec.Cell(row, 5).Value = "Deal Date";
                wsNonGSec.Column(5).Width = 15;
                wsNonGSec.Cell(row, 6).Value = "Rating";
                wsNonGSec.Column(6).Width = 15;
                wsNonGSec.Cell(row, 7).Value = "Pre IP Date";
                wsNonGSec.Column(7).Width = 15;
                wsNonGSec.Cell(row, 8).Value = "Interest Frequency";
                wsNonGSec.Column(8).Width = 15;

                i = 0;
                col = 9;
                for (i = 1; i <= 5; i++)
                {
                    wsNonGSec.Cell(row, col).Value = "Face Value";
                    wsNonGSec.Column(col).Width = 15;
                    wsNonGSec.Cell(row, col + 1).Value = "Rate";
                    wsNonGSec.Column(col + 1).Width = 15;
                    wsNonGSec.Cell(row, col + 2).Value = "Pur Consideration";
                    wsNonGSec.Column(col + 2).Width = 20;
                    wsNonGSec.Cell(row, col + 3).Value = "Quantity";
                    wsNonGSec.Column(col + 3).Width = 15;
                    wsNonGSec.Cell(row, col + 4).Value = "Total Consideration";
                    wsNonGSec.Column(col + 4).Width = 20;
                    wsNonGSec.Cell(row, col + 5).Value = "Market Rate";
                    wsNonGSec.Column(col + 5).Width = 15;
                    wsNonGSec.Cell(row, col + 6).Value = "Current Consideration";
                    wsNonGSec.Column(col + 6).Width = 20;
                    wsNonGSec.Cell(row, col + 7).Value = "MTM";
                    wsNonGSec.Column(col + 7).Width = 15;
                    wsNonGSec.Cell(row, col + 8).Value = "Total Current Consideration";
                    wsNonGSec.Column(col + 8).Width = 20;
                    wsNonGSec.Cell(row, col + 9).Value = "Coupon Received";
                    wsNonGSec.Column(col + 9).Width = 20;
                    wsNonGSec.Cell(row, col + 10).Value = "PV01-";
                    wsNonGSec.Column(col + 10).Width = 15;
                    wsNonGSec.Cell(row, col + 11).Value = "Credit PV01-";
                    wsNonGSec.Column(col + 11).Width = 15;
                    wsNonGSec.Range(row + 1, col + 10, row + dtNonGesc.Rows.Count + 1, col + 11).Style.Font.FontColor = XLColor.Red;

                    if (i == 1)
                    {
                        wsNonGSec.Range(row - 1, col, row, col + 11).Style.Fill.BackgroundColor = XLColor.FromArgb(242, 221, 220);
                    }
                    else if (i == 2)
                    {
                        wsNonGSec.Range(row - 1, col, row, col + 11).Style.Fill.BackgroundColor = XLColor.FromArgb(215, 228, 188);
                    }
                    else if (i == 3)
                    {
                        wsNonGSec.Range(row - 1, col, row, col + 11).Style.Fill.BackgroundColor = XLColor.FromArgb(204, 192, 218);
                    }
                    else if (i == 4)
                    {
                        wsNonGSec.Range(row - 1, col, row, col + 11).Style.Fill.BackgroundColor = XLColor.FromArgb(182, 221, 232);
                    }
                    else if (i == 5)
                    {
                        wsNonGSec.Range(row - 1, col, row, col + 11).Style.Fill.BackgroundColor = XLColor.FromArgb(252, 213, 180);
                    }
                    wsNonGSec.Range(row - 1, col, row, col + 11).Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
                    wsNonGSec.Range(row - 1, col, row, col + 11).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                    wsNonGSec.Range(row - 1, col, row, col + 11).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
                    wsNonGSec.Range(row - 1, col, row, col + 11).Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
                    col = col + 12;
                }

                row++;
                dtNonGesc.Columns.Remove("Range");

                wsNonGSec.Cell(row, 1).InsertTable(dtNonGesc);
                wsNonGSec.Tables.Table(0).ShowAutoFilter = false;
                wsNonGSec.Tables.Table(0).Theme = XLTableTheme.None;
                wsNonGSec.SheetView.FreezeRows(1);
                wsNonGSec.SheetView.FreezeRows(2);
                wsNonGSec.Row(3).Hide();

                row++;
                wsNonGSec.Range(row, 6, row + dtNonGesc.Rows.Count, dtNonGesc.Columns.Count).Style.NumberFormat.Format = "#,##0.00";
                row = row + ((dtNonGesc.Rows.Count > 0) ? dtNonGesc.Rows.Count : 1);
                wsNonGSec.Table(0).ShowTotalsRow = true;
                wsNonGSec.Table(0).Field(0).TotalsRowLabel = "Total:";
                for (i = 8; i <= dtNonGesc.Columns.Count - 1; i++)
                {
                    if (i != 9 && i != 13 && i != 21 && i != 25 && i != 33 && i != 37 && i != 45 && i != 49 && i != 57 && i != 61)
                    {
                        wsNonGSec.Table(0).Field(i).TotalsRowFunction = XLTotalsRowFunction.Sum;
                    }
                }
                wsNonGSec.Row(row).Style.Font.Bold = true;
                string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                strdtTime = strdtTime.Replace("PM", "");
                strdtTime = strdtTime.Replace("AM", "");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName + strdtTime + ".xlsx");
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
        }
        catch (Exception ex)
        {
            //throw ex;
        }
        finally
        {
        }
    }

    private void ExportDetails()
    {
        DataSet ds = new DataSet();
        List<SqlParameter> lstParam = new List<SqlParameter>();
        string strFileName = "";

        try
        {
            switch (strReportType)
            {
                case "StockVerticalsSummary":
                    lstParam.Add(new SqlParameter("@AsOn", objComm.Trim(Request.QueryString["dated"]) != "" ? objComm.Trim(Request.QueryString["dated"]) : null));
                    lstParam.Add(new SqlParameter("@VerticalId", objComm.Val(HttpContext.Current.Request.Params["verticalid"]) > 0 ? HttpContext.Current.Request.Params["verticalid"] : null));
                    ds = objComm.FillDetails(lstParam, "TS_Export_StockVerticalSummary");
                    strFileName = Request.QueryString["rptName"].Trim().Replace("/", " ");
                    break;

                case "StockVerticalsDetails":
                    lstParam.Add(new SqlParameter("@AsOn", objComm.Trim(Request.QueryString["dated"]) != "" ? objComm.Trim(Request.QueryString["dated"]) : null));
                    lstParam.Add(new SqlParameter("@VerticalId", objComm.Val(HttpContext.Current.Request.Params["verticalid"]) > 0 ? HttpContext.Current.Request.Params["verticalid"] : null));
                    ds = objComm.FillDetails(lstParam, "TS_Export_StockVerticalDetails");
                    strFileName = Request.QueryString["rptName"].Trim().Replace("/", " ");
                    break;

                case "StockDetails":
                    lstParam.Add(new SqlParameter("@AsOn", objComm.Trim(Request.QueryString["dated"]) != "" ? objComm.Trim(Request.QueryString["dated"]) : null));
                    //ds = objComm.FillDetails(lstParam, "TS_Export_StockReportAll");
                    ds = (DataSet)Session["StockData"];
                    strFileName = "Stock Details";
                    break;

                case "TransferStock":
                    lstParam.Add(new SqlParameter("@FromVerticalId", objComm.Val(HttpContext.Current.Request.Params["FromVerticalId"]) > 0 ? HttpContext.Current.Request.Params["FromVerticalId"] : null));
                    lstParam.Add(new SqlParameter("@ToVerticalId", objComm.Val(HttpContext.Current.Request.Params["ToVerticalId"]) > 0 ? HttpContext.Current.Request.Params["ToVerticalId"] : null));
                    lstParam.Add(new SqlParameter("@SecurityName", objComm.Trim(Request.QueryString["securityname"]) != "" ? objComm.Trim(Request.QueryString["securityname"]) : null));
                    lstParam.Add(new SqlParameter("@AsOn", objComm.Trim(Request.QueryString["dated"]) != "" ? objComm.Trim(Request.QueryString["dated"]) : null));
                    ds = objComm.FillDetails(lstParam, "TS_Export_TransferStock");
                    strFileName = "Transfer Stock";
                    break;

                case "StockUpdate":
                    lstParam.Add(new SqlParameter("@AsOn", objComm.Trim(Request.QueryString["dated"]) != "" ? objComm.Trim(Request.QueryString["dated"]) : null));
                    lstParam.Add(new SqlParameter("@query", condition));
                    ds = objComm.FillDetails(lstParam, "TS_Export_StockUpdate");
                    strFileName = "Stock Update";
                    break;

                case "TransferSell":
                    lstParam.Add(new SqlParameter("@AsOn", objComm.Trim(Request.QueryString["dated"]) != "" ? objComm.Trim(Request.QueryString["dated"]) : null));
                    lstParam.Add(new SqlParameter("@VerticalId", objComm.Val(HttpContext.Current.Request.Params["verticalid"]) > 0 ? HttpContext.Current.Request.Params["verticalid"] : null));
                    ds = objComm.FillDetails(lstParam, "TS_Export_TransferSellDetails");
                    strFileName = "Transfer Sell Details";
                    break;

                case "FinancialVerticalSummary":
                    lstParam.Add(new SqlParameter("@AsOn", objComm.Trim(Request.QueryString["dated"]) != "" ? objComm.Trim(Request.QueryString["dated"]) : null));
                    lstParam.Add(new SqlParameter("@VerticalId", objComm.Val(HttpContext.Current.Request.Params["verticalid"]) > 0 ? HttpContext.Current.Request.Params["verticalid"] : null));
                    ds = objComm.FillDetails(lstParam, "TS_Export_FinancialStockSummary");
                    strFileName = "Financial Stock Summary";
                    break;

                case "OtherStockVerticalSummary":
                    lstParam.Add(new SqlParameter("@AsOn", objComm.Trim(Request.QueryString["dated"]) != "" ? objComm.Trim(Request.QueryString["dated"]) : null));
                    lstParam.Add(new SqlParameter("@VerticalId", objComm.Val(HttpContext.Current.Request.Params["verticalid"]) > 0 ? HttpContext.Current.Request.Params["verticalid"] : null));
                    ds = objComm.FillDetails(lstParam, "TS_Export_OtherStockVerticalSummary");
                    strFileName = "Other Stock Summary";
                    break;

                default:
                    break;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                //wb.Worksheets.Add(ds.Tables[0], strFileName);
                IXLWorksheet ws = wb.Worksheets.Add(ds.Tables[0], strFileName);
                //ws.Tables.Table(0).ShowAutoFilter = false;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName + ".xlsx");
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
        }
        catch (Exception ex)
        {
            //throw ex;
        }
        finally
        {
        }
    }

    private void ExportDealDetails()
    {
        DataSet ds = new DataSet();
        List<SqlParameter> lstParam = new List<SqlParameter>();
        string strFileName = "";
        int row = 1;

        try
        {
            lstParam.Add(new SqlParameter("@AsOn", objComm.Trim(Request.QueryString["dated"]) != "" ? objComm.Trim(Request.QueryString["dated"]) : null));
            lstParam.Add(new SqlParameter("@DealSlipId", objComm.Val(HttpContext.Current.Request.Params["dealslipid"]) > 0 ? HttpContext.Current.Request.Params["dealslipid"] : null));
            ds = objComm.FillDetails(lstParam, "TS_Export_DealTransactionDetails");
            strFileName = "Deal Details";

            using (XLWorkbook wb = new XLWorkbook())
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    IXLWorksheet ws = wb.Worksheets.Add(strFileName);
                    ws.Cell(row, 1).Value = "Deal Detail Report";
                    ws.Cell(row, 1).Style.Font.Bold = true;
                    ws.Cell(row, 1).Style.Font.FontSize = 15;
                    ws.Cell(row, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Range("A" + row.ToString() + ":J" + row.ToString()).Merge();

                    ws.Column(1).Width = 15;
                    ws.Column(2).Width = 15;
                    ws.Column(3).Width = 15;
                    ws.Column(4).Width = 15;
                    ws.Column(5).Width = 15;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 15;
                    ws.Column(10).Width = 18;
                    ws.Column(11).Width = 18;
                    ws.Column(12).Width = 18;

                    row = row + 2;
                    ws.Cell(row, 1).Value = "As on";
                    ws.Cell(row, 2).Value = ds.Tables[0].Rows[0]["AsOn"].ToString();

                    row = row + 1;
                    ws.Cell(row, 1).Value = "Stock Type";
                    ws.Cell(row, 2).Value = ds.Tables[0].Rows[0]["StockType"].ToString();

                    row = row + 1;
                    ws.Cell(row, 1).Value = "ISIN";
                    ws.Cell(row, 2).Value = ds.Tables[0].Rows[0]["NSDLAcNumber"].ToString();

                    row = row + 1;
                    ws.Cell(row, 1).Value = "Security Name";
                    ws.Cell(row, 2).Value = ds.Tables[0].Rows[0]["SecurityName"].ToString();

                    row = row + 1;
                    ws.Cell(row, 1).Value = "Issuer Name";
                    ws.Cell(row, 2).Value = ds.Tables[0].Rows[0]["IssuerName"].ToString();

                    ws.Range("A1" + ":B" + row.ToString()).Style.Font.Bold = true;

                    ds.Tables[0].Columns.Remove("AsOn");
                    ds.Tables[0].Columns.Remove("StockType");
                    ds.Tables[0].Columns.Remove("SecurityName");
                    ds.Tables[0].Columns.Remove("NSDLAcNumber");
                    ds.Tables[0].Columns.Remove("IssuerName");

                    row = row + 2;
                    ws.Cell(row, 1).Value = "Purchase Details";
                    ws.Cell(row, 1).Style.Font.Bold = true;
                    row = row + 1;
                    ws.Cell(row, 1).InsertTable(ds.Tables[0]);
                    ws.Range(row, 1, row, ds.Tables[0].Columns.Count).Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);
                    ws.Range(row, 6, row + ds.Tables[0].Rows.Count, 10).Style.NumberFormat.Format = "#,##0.00";
                    ws.Tables.Table(0).ShowAutoFilter = false;
                    ws.Tables.Table(0).Theme = XLTableTheme.None;
                    ws.Tables.Table(0).Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
                    ws.Tables.Table(0).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                    ws.Tables.Table(0).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
                    ws.Tables.Table(0).Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
                    ws.Tables.Table(0).Style.Border.SetTopBorderColor(XLColor.FromArgb(194, 214, 154));
                    ws.Tables.Table(0).Style.Border.SetBottomBorderColor(XLColor.FromArgb(194, 214, 154));
                    ws.Tables.Table(0).Style.Border.SetLeftBorderColor(XLColor.FromArgb(194, 214, 154));
                    ws.Tables.Table(0).Style.Border.SetRightBorderColor(XLColor.FromArgb(194, 214, 154));


                    row = row + ((ds.Tables[0].Rows.Count > 0) ? ds.Tables[0].Rows.Count + 2 : 3);
                    ws.Cell(row, 1).Value = "Transfer Details";
                    ws.Cell(row, 1).Style.Font.Bold = true;
                    row = row + 1;
                    ws.Cell(row, 1).InsertTable(ds.Tables[1]);
                    ws.Range(row, 1, row, ds.Tables[1].Columns.Count).Style.Fill.BackgroundColor = XLColor.FromArgb(230, 185, 184);
                    ws.Range(row, 7, row + ds.Tables[1].Rows.Count, 11).Style.NumberFormat.Format = "#,##0.00";
                    //if (ds.Tables[1].Rows.Count > 0)
                    //{
                    //    row = row + ds.Tables[1].Rows.Count + 1;
                    //    ws.Row(row).Style.Font.Bold = true;
                    //    ws.Cell(row, 7).Value = ds.Tables[1].Compute("Sum([Tranfer Face Value])", "").ToString();
                    //    ws.Cell(row, 10).Value = ds.Tables[1].Compute("Sum([Total Consideration])", "").ToString();
                    //    ws.Cell(row, 11).Value = ds.Tables[1].Compute("Sum([P&L])", "").ToString();
                    //}
                    ws.Tables.Table(1).ShowAutoFilter = false;
                    ws.Tables.Table(1).Theme = XLTableTheme.None;
                    ws.Tables.Table(1).Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
                    ws.Tables.Table(1).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                    ws.Tables.Table(1).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
                    ws.Tables.Table(1).Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
                    ws.Tables.Table(1).Style.Border.SetTopBorderColor(XLColor.FromArgb(230, 185, 184));
                    ws.Tables.Table(1).Style.Border.SetBottomBorderColor(XLColor.FromArgb(230, 185, 184));
                    ws.Tables.Table(1).Style.Border.SetLeftBorderColor(XLColor.FromArgb(230, 185, 184));
                    ws.Tables.Table(1).Style.Border.SetRightBorderColor(XLColor.FromArgb(230, 185, 184));


                    row = row + ((ds.Tables[1].Rows.Count > 0) ? ds.Tables[1].Rows.Count + 2 : 3);
                    ws.Cell(row, 1).Value = "Sell Details";
                    ws.Cell(row, 1).Style.Font.Bold = true;
                    row = row + 1;
                    ws.Cell(row, 1).InsertTable(ds.Tables[2]);
                    ws.Range(row, 1, row, ds.Tables[2].Columns.Count).Style.Fill.BackgroundColor = XLColor.FromArgb(250, 192, 144);
                    ws.Range(row, 8, row + ds.Tables[2].Rows.Count, 12).Style.NumberFormat.Format = "#,##0.00";
                    //if (ds.Tables[2].Rows.Count > 0)
                    //{
                    //    row = row + ds.Tables[2].Rows.Count + 1;
                    //    ws.Row(row).Style.Font.Bold = true;
                    //    ws.Cell(row, 8).Value = ds.Tables[2].Compute("Sum([Face Value])", "").ToString();
                    //    ws.Cell(row, 11).Value = ds.Tables[2].Compute("Sum([Total Consideration])", "").ToString();
                    //    ws.Cell(row, 12).Value = ds.Tables[2].Compute("Sum([P&L])", "").ToString();
                    //}
                    ws.Tables.Table(2).ShowAutoFilter = false;
                    ws.Tables.Table(2).Theme = XLTableTheme.None;
                    ws.Tables.Table(2).Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
                    ws.Tables.Table(2).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                    ws.Tables.Table(2).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
                    ws.Tables.Table(2).Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
                    ws.Tables.Table(2).Style.Border.SetTopBorderColor(XLColor.FromArgb(250, 192, 144));
                    ws.Tables.Table(2).Style.Border.SetBottomBorderColor(XLColor.FromArgb(250, 192, 144));
                    ws.Tables.Table(2).Style.Border.SetLeftBorderColor(XLColor.FromArgb(250, 192, 144));
                    ws.Tables.Table(2).Style.Border.SetRightBorderColor(XLColor.FromArgb(250, 192, 144));
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName + ".xlsx");
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
        }
        catch (Exception ex)
        {
            //throw ex;
        }
        finally
        {
        }
    }

    private void ExportExposureDetails()
    {
        DataSet ds = new DataSet();
        DataTable dtGesc = new DataTable();
        DataTable dtNonGesc = new DataTable();
        List<SqlParameter> lstParam = new List<SqlParameter>();
        string strFileName = "";
        int row = 1;
        int col = 1;
        int i;
        string[] Column = { "FV", "PC", "Quantity", "MTM", "PV01", "TC", "CC", "CTC", "CR" };
        string[] GSec = { "Rates (Face Value)", "Rates (Pur Consideration)", "Rates (Quantity)", "Rates (MTM)", "PV01", "Total Consideration", "Current Consideration", "Current Total Consideration", "Coupon Received" };
        string[] GNonSec = { "Credit (Face Value)", "Credit (Pur Consideration)", "Credit (Quantity)", "Credit (MTM)", "PV01", "Total Consideration", "Current Consideration", "Current Total Consideration", "Coupon Received" };
        try
        {
            lstParam.Add(new SqlParameter("@AsOn", objComm.Trim(Request.QueryString["dated"]) != "" ? objComm.Trim(Request.QueryString["dated"]) : null));
            ds = objComm.FillDetails(lstParam, "Id_Export_HoldingExposureDetails");
            strFileName = "Aging of Inventory";

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet wsSummary = wb.Worksheets.Add("Summary");
                wsSummary.Column(1).Width = 25;
                wsSummary.Column(2).Width = 20;
                wsSummary.Column(3).Width = 20;
                wsSummary.Column(4).Width = 20;
                wsSummary.Column(5).Width = 20;
                wsSummary.Column(6).Width = 20;
                wsSummary.Column(1).Style.Font.Bold = true;

                wsSummary.Cell(row, 1).Value = "Holding Time";
                wsSummary.Cell(row, 2).Value = ">Less Than 1 Wk";
                wsSummary.Cell(row, 3).Value = ">1 Wk - < 1 Mon";
                wsSummary.Cell(row, 4).Value = ">1 Mon - < 3 Mon";
                wsSummary.Cell(row, 5).Value = ">3 Mon - < 6 Mon";
                wsSummary.Cell(row, 6).Value = "Greater Than 6 Mon";

                wsSummary.Range("A" + row.ToString() + ":F" + row.ToString()).Style.Font.Bold = true;
                wsSummary.Range("A" + row.ToString() + ":F" + row.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                row++;


                wsSummary.Cell(row, 2).Value = "G - Sec";
                wsSummary.Cell(row, 2).Style.Font.Bold = true;
                wsSummary.Cell(row, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wsSummary.Range("B" + row.ToString() + ":F" + row.ToString()).Merge();
                wsSummary.Range("A" + row.ToString() + ":F" + row.ToString()).Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);
                row++;

                wsSummary.Range(row, 2, row + Column.Length, 6).Style.NumberFormat.Format = "#,##0.00";

                i = 0;
                dtGesc = ds.Tables[0];
                foreach (string value in GSec)
                {
                    wsSummary.Cell(row, 1).Value = value;
                    if (!(dtGesc.Compute("SUM(" + Column[i] + "1)", "Range=1") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 2).Value = dtGesc.Compute("SUM(" + Column[i] + "1)", "Range=1");
                    }
                    if (!(dtGesc.Compute("SUM(" + Column[i] + "2)", "Range=2") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 3).Value = dtGesc.Compute("SUM(" + Column[i] + "2)", "Range=2");
                    }
                    if (!(dtGesc.Compute("SUM(" + Column[i] + "3)", "Range=3") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 4).Value = dtGesc.Compute("SUM(" + Column[i] + "3)", "Range=3");
                    }
                    if (!(dtGesc.Compute("SUM(" + Column[i] + "4)", "Range=4") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 5).Value = dtGesc.Compute("SUM(" + Column[i] + "4)", "Range=4");
                    }
                    if (!(dtGesc.Compute("SUM(" + Column[i] + "5)", "Range=5") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 6).Value = dtGesc.Compute("SUM(" + Column[i] + "5)", "Range=5");
                    }
                    if (value == "PV01")
                    {
                        row++;
                    }
                    row++;
                    i++;
                }

                row++;

                wsSummary.Cell(row, 2).Style.Font.Bold = true;
                wsSummary.Cell(row, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wsSummary.Range("B" + row.ToString() + ":F" + row.ToString()).Merge();
                wsSummary.Range("A" + row.ToString() + ":F" + row.ToString()).Style.Fill.BackgroundColor = XLColor.FromArgb(217, 151, 149);
                row++;

                wsSummary.Range(row, 2, row + Column.Length, 6).Style.NumberFormat.Format = "#,##0.00";

                i = 0;
                dtNonGesc = ds.Tables[1];
                foreach (string value in GNonSec)
                {
                    wsSummary.Cell(row, 1).Value = value;
                    if (!(dtNonGesc.Compute("SUM(" + Column[i] + "1)", "Range=1") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 2).Value = dtNonGesc.Compute("SUM(" + Column[i] + "1)", "Range=1");
                    }
                    if (!(dtNonGesc.Compute("SUM(" + Column[i] + "2)", "Range=2") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 3).Value = dtNonGesc.Compute("SUM(" + Column[i] + "2)", "Range=2");
                    }
                    if (!(dtNonGesc.Compute("SUM(" + Column[i] + "3)", "Range=3") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 4).Value = dtNonGesc.Compute("SUM(" + Column[i] + "3)", "Range=3");
                    }
                    if (!(dtNonGesc.Compute("SUM(" + Column[i] + "4)", "Range=4") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 5).Value = dtNonGesc.Compute("SUM(" + Column[i] + "4)", "Range=4");
                    }
                    if (!(dtNonGesc.Compute("SUM(" + Column[i] + "5)", "Range=5") == DBNull.Value))
                    {
                        wsSummary.Cell(row, 6).Value = dtNonGesc.Compute("SUM(" + Column[i] + "5)", "Range=5");
                    }
                    if (value == "PV01")
                    {
                        row++;
                    }
                    row++;
                    i++;
                }

                row = 1;
                IXLWorksheet wsGSec = wb.Worksheets.Add("G-Sec");
                wsGSec.Column("A").Width = 50;
                wsGSec.Columns("B,C,D,E,F,I,K,M,P,Q,T,V,X,AA,AE,AG,AI,AL,AM,AP,AT,AW,AX,BA,BC,BE,BH").Width = 15;
                wsGSec.Columns("H,J,L,N,O,S,U,W,Y,Z,AB,AD,AF,AH,AK,AO,AQ,AR,AS,AU,AV,AZ,BB,BD,BF,BG").Width = 20;

                col = 6;
                wsGSec.Cell(row, col).Value = "Less Than 1 Wk";
                wsGSec.Range(row, col, row, col + 10).Merge();
                col = col + 11;

                wsGSec.Cell(row, col).Value = ">1 Wk - < 1 Mon";
                wsGSec.Range(row, col, row, col + 10).Merge();
                col = col + 11;

                wsGSec.Cell(row, col).Value = ">1 Mon - < 3 Mon";
                wsGSec.Range(row, col, row, col + 10).Merge();
                col = col + 11;

                wsGSec.Cell(row, col).Value = ">3 Mon - < 6 Mon";
                wsGSec.Range(row, col, row, col + 10).Merge();
                col = col + 11;

                wsGSec.Cell(row, col).Value = "Greater Than 6 Mon";
                wsGSec.Range(row, col, row, col + 10).Merge();
                col = col + 11;

                wsGSec.Row(row).Style.Font.Bold = true;
                wsGSec.Range(row, 6, row, col).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);


                row++;
                wsGSec.Row(row).Style.Font.Bold = true;
                wsGSec.Cell(row, 1).Value = "Security Name";
                wsGSec.Cell(row, 2).Value = "Deal Date";
                wsGSec.Cell(row, 3).Value = "Rating";
                wsGSec.Cell(row, 4).Value = "Pre IP Date";
                wsGSec.Cell(row, 5).Value = "Interest Frequency";

                wsGSec.Cell(row, 6).Value = "Face Value";
                wsGSec.Cell(row, 7).Value = "Rate";
                wsGSec.Cell(row, 8).Value = "Pur Consideration";
                wsGSec.Cell(row, 9).Value = "Quantity";
                wsGSec.Cell(row, 10).Value = "Total Consideration";
                wsGSec.Range(row, 6, row, 10).Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);

                wsGSec.Cell(row, 11).Value = "Market Rate";
                wsGSec.Cell(row, 12).Value = "Current Consideration";
                wsGSec.Cell(row, 13).Value = "MTM";
                wsGSec.Cell(row, 14).Value = "Total Current Consideration";
                wsGSec.Cell(row, 15).Value = "Coupon Received";
                wsGSec.Cell(row, 16).Value = "PV01";
                wsGSec.Range(row, 11, row, 16).Style.Fill.BackgroundColor = XLColor.FromArgb(217, 151, 149);

                wsGSec.Cell(row, 17).Value = "Face Value";
                wsGSec.Cell(row, 18).Value = "Rate";
                wsGSec.Cell(row, 19).Value = "Pur Consideration";
                wsGSec.Cell(row, 20).Value = "Quantity";
                wsGSec.Cell(row, 21).Value = "Total Consideration";
                wsGSec.Range(row, 17, row, 21).Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);

                wsGSec.Cell(row, 22).Value = "Market Rate";
                wsGSec.Cell(row, 23).Value = "Current Consideration";
                wsGSec.Cell(row, 24).Value = "MTM";
                wsGSec.Cell(row, 25).Value = "Total Current Consideration";
                wsGSec.Cell(row, 26).Value = "Coupon Received";
                wsGSec.Cell(row, 27).Value = "PV01";
                wsGSec.Range(row, 22, row, 27).Style.Fill.BackgroundColor = XLColor.FromArgb(217, 151, 149);

                wsGSec.Cell(row, 28).Value = "Face Value";
                wsGSec.Cell(row, 29).Value = "Rate";
                wsGSec.Cell(row, 30).Value = "Pur Consideration";
                wsGSec.Cell(row, 31).Value = "Quantity";
                wsGSec.Cell(row, 32).Value = "Total Consideration";
                wsGSec.Range(row, 28, row, 32).Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);

                wsGSec.Cell(row, 33).Value = "Market Rate";
                wsGSec.Cell(row, 34).Value = "Current Consideration";
                wsGSec.Cell(row, 35).Value = "MTM";
                wsGSec.Cell(row, 36).Value = "Total Current Consideration";
                wsGSec.Cell(row, 37).Value = "Coupon Received";
                wsGSec.Cell(row, 38).Value = "PV01";
                wsGSec.Range(row, 33, row, 38).Style.Fill.BackgroundColor = XLColor.FromArgb(217, 151, 149);

                wsGSec.Cell(row, 39).Value = "Face Value";
                wsGSec.Cell(row, 40).Value = "Rate";
                wsGSec.Cell(row, 41).Value = "Pur Consideration";
                wsGSec.Cell(row, 42).Value = "Quantity";
                wsGSec.Cell(row, 43).Value = "Total Consideration";
                wsGSec.Range(row, 39, row, 43).Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);

                wsGSec.Cell(row, 44).Value = "Market Rate";
                wsGSec.Cell(row, 45).Value = "Current Consideration";
                wsGSec.Cell(row, 46).Value = "MTM";
                wsGSec.Cell(row, 47).Value = "Total Current Consideration";
                wsGSec.Cell(row, 48).Value = "Coupon Received";
                wsGSec.Cell(row, 49).Value = "PV01";
                wsGSec.Range(row, 44, row, 49).Style.Fill.BackgroundColor = XLColor.FromArgb(217, 151, 149);

                wsGSec.Cell(row, 50).Value = "Face Value";
                wsGSec.Cell(row, 51).Value = "Rate";
                wsGSec.Cell(row, 52).Value = "Pur Consideration";
                wsGSec.Cell(row, 53).Value = "Quantity";
                wsGSec.Cell(row, 54).Value = "Total Consideration";
                wsGSec.Range(row, 50, row, 54).Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);

                wsGSec.Cell(row, 55).Value = "Market Rate";
                wsGSec.Cell(row, 56).Value = "Current Consideration";
                wsGSec.Cell(row, 57).Value = "MTM";
                wsGSec.Cell(row, 58).Value = "Total Current Consideration";
                wsGSec.Cell(row, 59).Value = "Coupon Received";
                wsGSec.Cell(row, 60).Value = "PV01";
                wsGSec.Range(row, 55, row, 60).Style.Fill.BackgroundColor = XLColor.FromArgb(217, 151, 149);

                row++;
                dtGesc.Columns.Remove("Range");

                //foreach (DataRow row in dtGesc.Rows)
                //{
                //    for (i = 0; i <= dtGesc.Columns.Count - 1; i++)
                //    {
                //        wsGSec.Cell(row, 1) = row[i].ToString();
                //    }
                //}

                wsGSec.Cell(row, 1).InsertTable(dtGesc);
                wsGSec.Tables.Table(0).ShowAutoFilter = false;
                wsGSec.Tables.Table(0).Theme = XLTableTheme.None;
                wsGSec.Row(3).Hide();
                row++;
                wsGSec.Range(row, 6, row + dtGesc.Rows.Count, dtGesc.Columns.Count).Style.NumberFormat.Format = "#,##0.00";

                row = 1;
                IXLWorksheet wsNonGSec = wb.Worksheets.Add("Non G-Sec");
                wsNonGSec.Column("A").Width = 50;
                wsNonGSec.Columns("B,C,D,E,F,I,K,M,P,Q,T,V,X,AA,AE,AG,AI,AL,AM,AP,AT,AW,AX,BA,BC,BE,BH").Width = 15;
                wsNonGSec.Columns("H,J,L,N,O,S,U,W,Y,Z,AB,AD,AF,AH,AK,AO,AQ,AR,AS,AU,AV,AZ,BB,BD,BF,BG").Width = 20;

                col = 6;
                wsNonGSec.Cell(row, col).Value = "Less Than 1 Wk";
                wsNonGSec.Range(row, col, row, col + 10).Merge();
                col = col + 11;

                wsNonGSec.Cell(row, col).Value = ">1 Wk - < 1 Mon";
                wsNonGSec.Range(row, col, row, col + 10).Merge();
                col = col + 11;

                wsNonGSec.Cell(row, col).Value = ">1 Mon - < 3 Mon";
                wsNonGSec.Range(row, col, row, col + 10).Merge();
                col = col + 11;

                wsNonGSec.Cell(row, col).Value = ">3 Mon - < 6 Mon";
                wsNonGSec.Range(row, col, row, col + 10).Merge();
                col = col + 11;

                wsNonGSec.Cell(row, col).Value = "Greater Than 6 Mon";
                wsNonGSec.Range(row, col, row, col + 10).Merge();
                col = col + 11;

                wsNonGSec.Row(row).Style.Font.Bold = true;
                wsNonGSec.Range(row, 6, row, col).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                row++;
                wsNonGSec.Row(row).Style.Font.Bold = true;
                wsNonGSec.Cell(row, 1).Value = "Security Name";
                wsNonGSec.Cell(row, 2).Value = "Deal Date";
                wsNonGSec.Cell(row, 3).Value = "Rating";
                wsNonGSec.Cell(row, 4).Value = "Pre IP Date";
                wsNonGSec.Cell(row, 5).Value = "Interest Frequency";

                wsNonGSec.Cell(row, 6).Value = "Face Value";
                wsNonGSec.Cell(row, 7).Value = "Rate";
                wsNonGSec.Cell(row, 8).Value = "Pur Consideration";
                wsNonGSec.Cell(row, 9).Value = "Quantity";
                wsNonGSec.Cell(row, 10).Value = "Total Consideration";
                wsNonGSec.Range(row, 6, row, 10).Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);

                wsNonGSec.Cell(row, 11).Value = "Market Rate";
                wsNonGSec.Cell(row, 12).Value = "Current Consideration";
                wsNonGSec.Cell(row, 13).Value = "MTM";
                wsNonGSec.Cell(row, 14).Value = "Total Current Consideration";
                wsNonGSec.Cell(row, 15).Value = "Coupon Received";
                wsNonGSec.Cell(row, 16).Value = "PV01";
                wsNonGSec.Range(row, 11, row, 16).Style.Fill.BackgroundColor = XLColor.FromArgb(217, 151, 149);

                wsNonGSec.Cell(row, 17).Value = "Face Value";
                wsNonGSec.Cell(row, 18).Value = "Rate";
                wsNonGSec.Cell(row, 19).Value = "Pur Consideration";
                wsNonGSec.Cell(row, 20).Value = "Quantity";
                wsNonGSec.Cell(row, 21).Value = "Total Consideration";
                wsNonGSec.Range(row, 17, row, 21).Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);

                wsNonGSec.Cell(row, 22).Value = "Market Rate";
                wsNonGSec.Cell(row, 23).Value = "Current Consideration";
                wsNonGSec.Cell(row, 24).Value = "MTM";
                wsNonGSec.Cell(row, 25).Value = "Total Current Consideration";
                wsNonGSec.Cell(row, 26).Value = "Coupon Received";
                wsNonGSec.Cell(row, 27).Value = "PV01";
                wsNonGSec.Range(row, 22, row, 27).Style.Fill.BackgroundColor = XLColor.FromArgb(217, 151, 149);

                wsNonGSec.Cell(row, 28).Value = "Face Value";
                wsNonGSec.Cell(row, 29).Value = "Rate";
                wsNonGSec.Cell(row, 30).Value = "Pur Consideration";
                wsNonGSec.Cell(row, 31).Value = "Quantity";
                wsNonGSec.Cell(row, 32).Value = "Total Consideration";
                wsNonGSec.Range(row, 28, row, 32).Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);

                wsNonGSec.Cell(row, 33).Value = "Market Rate";
                wsNonGSec.Cell(row, 34).Value = "Current Consideration";
                wsNonGSec.Cell(row, 35).Value = "MTM";
                wsNonGSec.Cell(row, 36).Value = "Total Current Consideration";
                wsNonGSec.Cell(row, 37).Value = "Coupon Received";
                wsNonGSec.Cell(row, 38).Value = "PV01";
                wsNonGSec.Range(row, 33, row, 38).Style.Fill.BackgroundColor = XLColor.FromArgb(217, 151, 149);

                wsNonGSec.Cell(row, 39).Value = "Face Value";
                wsNonGSec.Cell(row, 40).Value = "Rate";
                wsNonGSec.Cell(row, 41).Value = "Pur Consideration";
                wsNonGSec.Cell(row, 42).Value = "Quantity";
                wsNonGSec.Cell(row, 43).Value = "Total Consideration";
                wsNonGSec.Range(row, 39, row, 43).Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);

                wsNonGSec.Cell(row, 44).Value = "Market Rate";
                wsNonGSec.Cell(row, 45).Value = "Current Consideration";
                wsNonGSec.Cell(row, 46).Value = "MTM";
                wsNonGSec.Cell(row, 47).Value = "Total Current Consideration";
                wsNonGSec.Cell(row, 48).Value = "Coupon Received";
                wsNonGSec.Cell(row, 49).Value = "PV01";
                wsNonGSec.Range(row, 44, row, 49).Style.Fill.BackgroundColor = XLColor.FromArgb(217, 151, 149);

                wsNonGSec.Cell(row, 50).Value = "Face Value";
                wsNonGSec.Cell(row, 51).Value = "Rate";
                wsNonGSec.Cell(row, 52).Value = "Pur Consideration";
                wsNonGSec.Cell(row, 53).Value = "Quantity";
                wsNonGSec.Cell(row, 54).Value = "Total Consideration";
                wsNonGSec.Range(row, 50, row, 54).Style.Fill.BackgroundColor = XLColor.FromArgb(194, 214, 154);

                wsNonGSec.Cell(row, 55).Value = "Market Rate";
                wsNonGSec.Cell(row, 56).Value = "Current Consideration";
                wsNonGSec.Cell(row, 57).Value = "MTM";
                wsNonGSec.Cell(row, 58).Value = "Total Current Consideration";
                wsNonGSec.Cell(row, 59).Value = "Coupon Received";
                wsNonGSec.Cell(row, 60).Value = "PV01";
                wsNonGSec.Range(row, 55, row, 60).Style.Fill.BackgroundColor = XLColor.FromArgb(217, 151, 149);

                row++;
                dtNonGesc.Columns.Remove("Range");
                wsNonGSec.Cell(row, 1).InsertTable(dtNonGesc);
                wsNonGSec.Tables.Table(0).ShowAutoFilter = false;
                wsNonGSec.Tables.Table(0).Theme = XLTableTheme.None;
                wsNonGSec.Row(3).Hide();

                row++;
                wsGSec.Range(row, 6, row + dtNonGesc.Rows.Count, dtNonGesc.Columns.Count).Style.NumberFormat.Format = "#,##0.00";


                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName + ".xlsx");
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
        }
        catch (Exception ex)
        {
            //throw ex;
        }
        finally
        {
        }
    }



    private void ExportSelldownTracking()
    {
        DataSet ds = new DataSet();
        DataTable dtGesc = new DataTable();
        DataTable dtNonGesc = new DataTable();
        List<SqlParameter> lstParam = new List<SqlParameter>();
        string strFileName = "";
        try
        {
            lstParam.Add(new SqlParameter("@OpeningDate", objComm.Trim(Request.QueryString["fromdate"]) != "" ? objComm.Trim(Request.QueryString["fromdate"]) : null));
            lstParam.Add(new SqlParameter("@ClosingDate", objComm.Trim(Request.QueryString["todate"]) != "" ? objComm.Trim(Request.QueryString["todate"]) : null));
            lstParam.Add(new SqlParameter("@TempCompId", objComm.Trim(Request.QueryString["compid"]) != "" ? objComm.Trim(Request.QueryString["compid"]) : null));
            ds = objComm.FillDetails(lstParam, "TS_Export_SellDownTracking");
            strFileName = "Sell Down Tracking";

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet wsGSec = wb.Worksheets.Add(ds.Tables[0], "G-Sec");
                IXLWorksheet wsNonGSec = wb.Worksheets.Add(ds.Tables[1], "Non G-Sec");

                //wsGSec.Tables.Table(0).ShowAutoFilter = false;
                //wsGSec.Tables.Table(0).Theme = XLTableTheme.None;
                string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                strdtTime = strdtTime.Replace("PM", "");
                strdtTime = strdtTime.Replace("AM", "");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName + strdtTime + ".xlsx");
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
        }
        catch (Exception ex)
        {
            //throw ex;
        }
        finally
        {
        }
    }
    private void ExportCompanyStockDetails()
    {
        DataSet ds = new DataSet();
        DataTable dtSummary = new DataTable();
        DataTable dtDetails = new DataTable();
        List<SqlParameter> lstParam = new List<SqlParameter>();
        string strFileName = "";
        int row = 1;
        int col = 1;
        int i;

        try
        {
            lstParam.Add(new SqlParameter("@FromDate", objComm.Trim(Request.QueryString["fromdate"]) != "" ? objComm.Trim(Request.QueryString["fromdate"]) : null));
            lstParam.Add(new SqlParameter("@ToDate", objComm.Trim(Request.QueryString["todate"]) != "" ? objComm.Trim(Request.QueryString["todate"]) : null));
            lstParam.Add(new SqlParameter("@TempCompId", objComm.Trim(Request.QueryString["compid"]) != "" ? objComm.Trim(Request.QueryString["compid"]) : null));
            ds = objComm.FillDetails(lstParam, "TS_Export_CompanyStockDetails");
            strFileName = "Stock Details";

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet wsSummary = wb.Worksheets.Add("Summary");
                dtSummary = ds.Tables[0];

                col = 8;
                wsSummary.Cell(row, col).Value = "Opening Stock";
                wsSummary.Range(row, col, row, col + 9).Merge();
                wsSummary.Range(row, col, row + 1, col + 9).Style.Fill.BackgroundColor = XLColor.FromArgb(242, 221, 220);
                col = col + 10;

                wsSummary.Cell(row, col).Value = "Purchases";
                wsSummary.Range(row, col, row, col + 7).Merge();
                wsSummary.Range(row, col, row + 1, col + 7).Style.Fill.BackgroundColor = XLColor.FromArgb(215, 228, 188);
                col = col + 8;

                wsSummary.Cell(row, col).Value = "Sales";
                wsSummary.Range(row, col, row, col + 8).Merge();
                wsSummary.Range(row, col, row + 1, col + 8).Style.Fill.BackgroundColor = XLColor.FromArgb(204, 192, 218);
                col = col + 9;

                wsSummary.Cell(row, col).Value = "Closing Stock";
                wsSummary.Range(row, col, row, col + 10).Merge();
                wsSummary.Range(row, col, row + 1, col + 10).Style.Fill.BackgroundColor = XLColor.FromArgb(182, 221, 232);
                col = col + 11;

                wsSummary.Cell(row, col).Value = "Gross Profit";
                wsSummary.Range(row, col, row, col + 8).Merge();
                wsSummary.Range(row, col, row + 1, col + 8).Style.Fill.BackgroundColor = XLColor.FromArgb(252, 213, 180);
                col = col + 8;

                wsSummary.Row(row).Style.Font.Bold = true;
                wsSummary.Row(row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wsSummary.Range(row, 8, row + 1, col).Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
                wsSummary.Range(row, 8, row + 1, col).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                wsSummary.Range(row, 8, row + 1, col).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
                wsSummary.Range(row, 8, row + 1, col).Style.Border.SetRightBorder(XLBorderStyleValues.Thin);

                row++;
                wsSummary.Columns(1, 54).Width = 18;
                wsSummary.Row(row).Style.Font.Bold = true;
                wsSummary.Cell(row, 1).Value = "Company Name";
                wsSummary.Column(1).Width = 40;
                wsSummary.Cell(row, 2).Value = "ISIN";
                wsSummary.Column(2).Width = 15;
                wsSummary.Cell(row, 3).Value = "Security Name";
                wsSummary.Column(3).Width = 60;
                wsSummary.Cell(row, 4).Value = "Coupon Rate";
                wsSummary.Column(4).Width = 15;
                wsSummary.Cell(row, 5).Value = "Call/Maturity Date";
                wsSummary.Cell(row, 6).Value = "IP Date";
                wsSummary.Column(4).Width = 15;
                wsSummary.Cell(row, 7).Value = "Interest Frequency";

                //Opening
                wsSummary.Cell(row, 8).Value = "Face Value";
                wsSummary.Cell(row, 9).Value = "Rate";
                wsSummary.Cell(row, 10).Value = "Principle Value";
                wsSummary.Cell(row, 11).Value = "Deal Interest";
                wsSummary.Cell(row, 12).Value = "Roundoff";
                wsSummary.Cell(row, 13).Value = "Other Charges";
                wsSummary.Cell(row, 14).Value = "Sett Brokerage Charges";
                wsSummary.Cell(row, 15).Value = "Total Consideration";
                wsSummary.Cell(row, 16).Value = "Interest From Pre IP";
                wsSummary.Cell(row, 17).Value = "Acc Interest";

                //Purchase
                wsSummary.Cell(row, 18).Value = "Face Value";
                wsSummary.Cell(row, 19).Value = "Rate";
                wsSummary.Cell(row, 20).Value = "Principle Value";
                wsSummary.Cell(row, 21).Value = "Deal Interest";
                wsSummary.Cell(row, 22).Value = "Roundoff";
                wsSummary.Cell(row, 23).Value = "Other Charges";
                wsSummary.Cell(row, 24).Value = "Sett Brokerage Charges";
                wsSummary.Cell(row, 25).Value = "Total Consideration";

                //Sell
                wsSummary.Cell(row, 26).Value = "Face Value";
                wsSummary.Cell(row, 27).Value = "Rate";
                wsSummary.Cell(row, 28).Value = "Principle Value";
                wsSummary.Cell(row, 29).Value = "Deal Interest";
                wsSummary.Cell(row, 30).Value = "Roundoff";
                wsSummary.Cell(row, 31).Value = "Other Charges";
                wsSummary.Cell(row, 32).Value = "Sett Brokerage Charges";
                wsSummary.Cell(row, 33).Value = "Total Consideration";
                wsSummary.Cell(row, 34).Value = "Coupon Received";

                //Closing
                wsSummary.Cell(row, 35).Value = "Face Value";
                wsSummary.Cell(row, 36).Value = "Rate";
                wsSummary.Cell(row, 37).Value = "Principle Value";
                wsSummary.Cell(row, 38).Value = "Deal Interest";
                wsSummary.Cell(row, 39).Value = "Roundoff";
                wsSummary.Cell(row, 40).Value = "Other Charges";
                wsSummary.Cell(row, 41).Value = "Sett Brokerage Charges";
                wsSummary.Cell(row, 42).Value = "Total Consideration";
                wsSummary.Cell(row, 43).Value = "Coupon Received";
                wsSummary.Cell(row, 44).Value = "Interest From Pre IP";
                wsSummary.Cell(row, 45).Value = "Acc Interest";

                //Gross
                wsSummary.Cell(row, 46).Value = "Face Value";
                wsSummary.Cell(row, 47).Value = "Principle Value";
                wsSummary.Cell(row, 48).Value = "Deal Interest";
                wsSummary.Cell(row, 49).Value = "Roundoff";
                wsSummary.Cell(row, 50).Value = "Other Charges";
                wsSummary.Cell(row, 51).Value = "Sett Brokerage Charges";
                wsSummary.Cell(row, 52).Value = "Total Consideration";
                wsSummary.Cell(row, 53).Value = "Coupon Received";
                wsSummary.Cell(row, 54).Value = "Acc Interest";

                

                row++;
                wsSummary.SheetView.FreezeRows(1);
                wsSummary.SheetView.FreezeRows(2);
                wsSummary.Range(row, 8, row + dtSummary.Rows.Count, dtSummary.Columns.Count).Style.NumberFormat.Format = "#,##0.00";

                foreach (DataRow drow in dtSummary.Rows)
                {
                    col = 1;
                    foreach (DataColumn dcol in dtSummary.Columns)
                    {
                        wsSummary.Cell(row, col).Value = objComm.Trim(drow[dcol.ColumnName].ToString());
                        col++;
                    }
                    row++;
                }

                wsSummary.Cell(row, 1).Value = "Total:";
                string strColLetter = "";
                for (i = 8; i <= dtSummary.Columns.Count; i++)
                {
                    if (i != 9 && i != 19 && i != 27 && i != 36)
                    {
                        strColLetter = wsSummary.Column(i).ColumnLetter();
                        wsSummary.Cell(row, i).FormulaA1 = "=SUM(" + strColLetter + "3" + ":" + strColLetter + (row - 1).ToString() + ")";
                    }
                }
                wsSummary.Row(row).Style.Font.Bold = true;

                //Details

                row = 1;
                IXLWorksheet wsDetails = wb.Worksheets.Add("Details");
                dtDetails = ds.Tables[1];

                col = 8;
                wsDetails.Cell(row, col).Value = "Opening Stock";
                wsDetails.Range(row, col, row, col + 13).Merge();
                wsDetails.Range(row, col, row + 1, col + 13).Style.Fill.BackgroundColor = XLColor.FromArgb(242, 221, 220);
                col = col + 14;

                wsDetails.Cell(row, col).Value = "Purchases";
                wsDetails.Range(row, col, row, col + 11).Merge();
                wsDetails.Range(row, col, row + 1, col + 11).Style.Fill.BackgroundColor = XLColor.FromArgb(215, 228, 188);
                col = col + 12;

                wsDetails.Cell(row, col).Value = "Sales";
                wsDetails.Range(row, col, row, col + 12).Merge();
                wsDetails.Range(row, col, row + 1, col + 12).Style.Fill.BackgroundColor = XLColor.FromArgb(204, 192, 218);
                col = col + 13;

                wsDetails.Cell(row, col).Value = "Closing Stock";
                wsDetails.Range(row, col, row, col + 11).Merge();
                wsDetails.Range(row, col, row + 1, col + 11).Style.Fill.BackgroundColor = XLColor.FromArgb(182, 221, 232);
                col = col + 12;

                wsDetails.Cell(row, col).Value = "Gross Profit";
                wsDetails.Range(row, col, row, col + 8).Merge();
                wsDetails.Range(row, col, row + 1, col + 8).Style.Fill.BackgroundColor = XLColor.FromArgb(252, 213, 180);
                col = col + 8;

                wsDetails.Row(row).Style.Font.Bold = true;
                wsDetails.Row(row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                wsDetails.Range(row, 8, row + 1, col).Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
                wsDetails.Range(row, 8, row + 1, col).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                wsDetails.Range(row, 8, row + 1, col).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
                wsDetails.Range(row, 8, row + 1, col).Style.Border.SetRightBorder(XLBorderStyleValues.Thin);

                row++;
                wsDetails.Columns(1, 67).Width = 15;
                wsDetails.Row(row).Style.Font.Bold = true;
                wsDetails.Cell(row, 1).Value = "Company Name";
                wsDetails.Column(1).Width = 40;
                wsDetails.Cell(row, 2).Value = "ISIN";
                wsDetails.Column(2).Width = 15;
                wsDetails.Cell(row, 3).Value = "Security Name";
                wsDetails.Column(3).Width = 60;
                wsDetails.Cell(row, 4).Value = "Coupon Rate";
                wsDetails.Cell(row, 5).Value = "Call/Maturity Date";
                wsDetails.Cell(row, 6).Value = "IP Date";
                wsDetails.Cell(row, 7).Value = "Interest Frequency";

                //Opening
                wsDetails.Cell(row, 8).Value = "Deal No";
                wsDetails.Cell(row, 9).Value = "Deal Date";
                wsDetails.Cell(row, 10).Value = "Settlement Date";
                wsDetails.Cell(row, 11).Value = "Client Name";
                wsDetails.Column(11).Width = 40;
                wsDetails.Cell(row, 12).Value = "Face Value";
                wsDetails.Cell(row, 13).Value = "Rate";
                wsDetails.Cell(row, 14).Value = "Principle Value";
                wsDetails.Cell(row, 15).Value = "Deal Interest";
                wsDetails.Cell(row, 16).Value = "Roundoff";
                wsDetails.Cell(row, 17).Value = "Other Charges";
                wsDetails.Cell(row, 18).Value = "Sett Brokerage Charges";
                wsDetails.Cell(row, 19).Value = "Total Consideration";
                wsDetails.Cell(row, 20).Value = "Interest From Pre IP";
                wsDetails.Cell(row, 21).Value = "Acc Interest";

                //Purchase
                wsDetails.Cell(row, 22).Value = "Deal No";
                wsDetails.Cell(row, 23).Value = "Deal Date";
                wsDetails.Cell(row, 24).Value = "Settlement Date";
                wsDetails.Cell(row, 25).Value = "Client Name";
                wsDetails.Column(25).Width = 40;
                wsDetails.Cell(row, 26).Value = "Face Value";
                wsDetails.Cell(row, 27).Value = "Rate";
                wsDetails.Cell(row, 28).Value = "Principle Value";
                wsDetails.Cell(row, 29).Value = "Deal Interest";
                wsDetails.Cell(row, 30).Value = "Roundoff";
                wsDetails.Cell(row, 31).Value = "Other Charges";
                wsDetails.Cell(row, 32).Value = "Sett Brokerage Charges";
                wsDetails.Cell(row, 33).Value = "Total Consideration";

                //Sell
                wsDetails.Cell(row, 34).Value = "Deal No";
                wsDetails.Cell(row, 35).Value = "Deal Date";
                wsDetails.Cell(row, 36).Value = "Settlement Date";
                wsDetails.Cell(row, 37).Value = "Client Name";
                wsDetails.Column(37).Width = 40;
                wsDetails.Cell(row, 38).Value = "Face Value";
                wsDetails.Cell(row, 39).Value = "Rate";
                wsDetails.Cell(row, 40).Value = "Principle Value";
                wsDetails.Cell(row, 41).Value = "Deal Interest";
                wsDetails.Cell(row, 42).Value = "Roundoff";
                wsDetails.Cell(row, 43).Value = "Other Charges";
                wsDetails.Cell(row, 44).Value = "Sett Brokerage Charges";
                wsDetails.Cell(row, 45).Value = "Total Consideration";
                wsDetails.Cell(row, 46).Value = "Coupon Received";

                //Closing
                wsDetails.Cell(row, 47).Value = "Face Value";
                wsDetails.Cell(row, 48).Value = "Rate";
                wsDetails.Cell(row, 49).Value = "Principle Value";
                wsDetails.Cell(row, 50).Value = "Deal Interest";
                wsDetails.Cell(row, 51).Value = "Roundoff";
                wsDetails.Cell(row, 52).Value = "Other Charges";
                wsDetails.Cell(row, 53).Value = "Sett Brokerage Charges";
                wsDetails.Cell(row, 54).Value = "Total Consideration";
                wsDetails.Cell(row, 55).Value = "Coupon Received";
                wsDetails.Cell(row, 56).Value = "Interest From Pre IP";
                wsDetails.Cell(row, 57).Value = "Acc Interest";
                wsDetails.Cell(row, 58).Value = "Last IP Date";

                //Gross
                wsDetails.Cell(row, 59).Value = "Face Value";
                wsDetails.Cell(row, 60).Value = "Principle Value";
                wsDetails.Cell(row, 61).Value = "Deal Interest";
                wsDetails.Cell(row, 62).Value = "Roundoff";
                wsDetails.Cell(row, 63).Value = "Other Charges";
                wsDetails.Cell(row, 64).Value = "Sett Brokerage Charges";
                wsDetails.Cell(row, 65).Value = "Total Consideration";
                wsDetails.Cell(row, 66).Value = "Coupon Received";
                wsDetails.Cell(row, 67).Value = "Acc Interest";

          

                row++;
                wsDetails.SheetView.FreezeRows(1);
                wsDetails.SheetView.FreezeRows(2);
                wsDetails.Range("L3", "U" + (dtDetails.Rows.Count + 3).ToString()).Style.NumberFormat.Format = "#,##0.00";
                wsDetails.Range("Z3", "AG" + (dtDetails.Rows.Count + 3).ToString()).Style.NumberFormat.Format = "#,##0.00";
                wsDetails.Range("AL3", "BO" + (dtDetails.Rows.Count + 3).ToString()).Style.NumberFormat.Format = "#,##0.00";

                foreach (DataRow drow in dtDetails.Rows)
                {
                    col = 1;
                    foreach (DataColumn dcol in dtDetails.Columns)
                    {
                        wsDetails.Cell(row, col).Value = objComm.Trim(drow[dcol.ColumnName].ToString());
                        col++;
                    }
                    row++;
                }

                string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                strdtTime = strdtTime.Replace("PM", "");
                strdtTime = strdtTime.Replace("AM", "");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName + strdtTime + ".xlsx");
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
        }
        catch (Exception ex)
        {
            //throw ex;
        }
        finally
        {
        }
    }
}
