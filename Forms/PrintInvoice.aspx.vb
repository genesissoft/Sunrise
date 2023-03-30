Imports DocumentFormat.OpenXml
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports iTextSharp.text
'Imports iTextSharp.text.pdf
Imports iTextSharp.text.Element
Imports System.Net
Imports System.Net.Mail
Imports ClosedXML.Excel
Imports System.Drawing
'Imports myApp.ns.pages
Imports log4net
Imports System.Xml
Imports System.Xml.Serialization

Imports System.Collections.Generic
'Imports System.Linq
Imports System.Text
Imports System.Object
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet
Imports DocumentFormat.OpenXml.Presentation

'Imports System.Windows.Forms
Imports iTextSharp.text.pdf
Imports System.FormatException
Imports DocumentFormat.OpenXml.Drawing.Spreadsheet
Imports DocumentFormat.OpenXml.Math
Imports System.Decimal
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class Forms_PrintInvoice
    Inherits System.Web.UI.Page
    Dim objUtil As New Util
    Dim objRpt As New MISReports
    Dim rptDoc As New ReportDocument
    Dim sqlconn As SqlConnection
    Dim objCommon As New clsCommonFuns


    Private Sub Forms_PrintInvoice_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try

            If IsPostBack = False Then
                SetAttributes()
                txt_FromDate.Text = DateTime.Now.ToString("dd/MM/yyyy")
                txt_FromDate.Attributes.Add("onkeypress", "OnlyDate();")
                txt_FromDate.Attributes.Add("onblur", "CheckDate(this,false);")

                txt_ToDate.Attributes.Add("onkeypress", "OnlyDate();")
                txt_ToDate.Attributes.Add("onblur", "CheckDate(this,false);")
                'txt_ToDate.Text = Format(DateAdd(DateInterval.Day, 1, Today), "dd/MM/yyyy")
                txt_ToDate.Text = DateTime.Now.ToString("dd/MM/yyyy")
            End If

            Hid_ReportType.Value = Trim(Request.QueryString("Rpt") & "")


        Catch ex As Exception

        End Try
    End Sub

    Private Sub OpenConn()
        If sqlconn Is Nothing Then
            sqlconn = New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
            sqlconn.Open()
        ElseIf sqlconn.State = ConnectionState.Closed Then
            sqlconn.ConnectionString = ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString()
            sqlconn.Open()
        End If
    End Sub

    Private Sub CloseConn()
        If sqlconn Is Nothing Then Exit Sub
        If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
    End Sub

    Private Sub SetAttributes()
        Try
            OpenConn()

            btn_Excel.Attributes.Add("onclick", "return  Validation();")
            btn_PDF.Attributes.Add("onclick", "return  Validation();")
            Col_Headers.InnerHtml = "Debit Note"
        Catch ex As Exception
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub btn_PDF_Click(sender As Object, e As EventArgs) Handles btn_PDF.Click
        Try
            OpenConn()
            Wordexport()
        Catch ex As Exception
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub Wordexport()
        Try
            Dim rptDoc As ReportDocument

            rptDoc = BindReport()
            'If rptDoc IsNot Nothing Then
            '    ExportReport(rptDoc)
            '    rptDoc = Nothing
            'End If

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            'rptDoc.Dispose()
        End Try
    End Sub

    Private Sub ExportReport(ByVal crReport As ReportDocument)
        'declare a memorystream object that will hold out output 
        Try
            Dim oStream As MemoryStream
            Response.Clear()
            Response.ClearHeaders()
            Response.Buffer = True

            crReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report")



            Response.Flush()
            Response.End()
        Catch ex As Exception
        Finally
        End Try
    End Sub
    Private Function BindReport(Optional ByVal blnSepPages As Boolean = False) As ReportDocument
        Try
            Dim strPath As String
            Dim dtRpt As DataTable
            ''*****Mehul*****17/Aug/2019***"\"****strpath Set***
            strPath = ConfigurationManager.AppSettings("ReportsPath").ToString & "\" & "DebitNoteDetailGST.rpt"
            rptDoc.Load(strPath)
            dtRpt = GetReportTable()
            If dtRpt.Rows.Count = 0 Then
                Dim strHtml As String
                strHtml = "alert('Sorry!!! No Records available to show report');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
                Exit Function
            End If
            rptDoc.SetDataSource(dtRpt)
            'strReportRange = " From " & Trim(Request.QueryString("FromDate") & "") & " To " & Trim(Request.QueryString("ToDate") & "") & ""
            'rptDoc.SummaryInfo.ReportComments = strReportRange


            rptDoc.RecordSelectionFormula = clsCommonFuns.strRecordSelection
            clsCommonFuns.strRecordSelection = ""
            rptDoc = objCommon.GetReport(rptDoc)
            rptDoc.VerifyDatabase()
            rptDoc.Refresh()
            'If Trim(Request.QueryString("Rpt") & "") <> "DealDetail" Then
            CrystalReportViewer1.SeparatePages = False
            CrystalReportViewer1.ReportSource = rptDoc
            CrystalReportViewer1.DataBind()
            CrystalReportViewer1.RefreshReport()
            'End If
            Dim oStream As MemoryStream
            Response.Clear()
            Response.ClearHeaders()
            Response.Buffer = True
            Dim strRptName As String = srh_BrokerName.SearchTextBox.Text

            rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, strRptName)



            Response.Flush()
            Response.End()

            Return rptDoc
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            rptDoc.Dispose()
        End Try
    End Function

    Private Function GetReportTable() As DataTable
        Try

            Dim sqlcomm As New SqlCommand
            Dim sqlda As New SqlDataAdapter
            Dim dtfill As New DataTable
            With sqlcomm
                .Connection = sqlconn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_FILL_RetailDebitNoteRpt"
                .Parameters.Clear()
                objCommon.SetCommandParameters(sqlcomm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                objCommon.SetCommandParameters(sqlcomm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                objCommon.SetCommandParameters(sqlcomm, "@BrokerId", SqlDbType.Int, 4, "I", , , Val(srh_BrokerName.SelectedId))
                objCommon.SetCommandParameters(sqlcomm, "@Fromdate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_FromDate.Text))
                objCommon.SetCommandParameters(sqlcomm, "@Todate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ToDate.Text))
                objCommon.SetCommandParameters(sqlcomm, "@RetailDebitRefNo", SqlDbType.Char, 10, "I", , , Trim(Srh_RetailDebitNo.SearchTextBox.Text))

                .ExecuteNonQuery()
                sqlda.SelectCommand = sqlcomm
                sqlda.Fill(dtfill)
                Session("DebitNote") = dtfill
            End With
            Return dtfill

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Sub btn_Excel_Click(sender As Object, e As EventArgs) Handles btn_Excel.Click
        Try
            OpenConn()
            GetReportTable()
            Dim dt As New DataTable()
            dt = Session("DebitNote")
            DebitExcel()
        Catch ex As Exception
            'Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try
    End Sub
    Public Sub DebitExcel()
        Try

            Dim strDestination As String
            Dim strOldCustName As String = ""
            Dim intCnt As Int16 = -1
            Dim arrContact(0) As String
            Dim arrIndex(0) As Int16
            Dim strCustCont As String = ""
            Dim strFileOfferSave As String = ""
            Dim J As Int16
            Dim srEnd As StreamReader = Nothing
            Dim CustomerName As String = ""
            Dim intRowIndex As Int16
            Dim fv As Int64
            Dim BrokerageAmt As Decimal


            Dim dta As DataTable = TryCast(Session("DebitNote"), DataTable)
            If dta.Rows.Count > 0 Then
                For b As Int32 = 0 To dta.Rows.Count
                    Dim strppp As String = (Server.MapPath("").Replace("Forms", "Temp") & "\DebitExcel.xlsx")
                    Dim workbook = New XLWorkbook(strppp)
                    Dim worksheet = workbook.Worksheet(1)

                    Dim myRange As ClosedXML.Excel.IXLCell
                    Dim strPath As String = ""
                    Dim sImagePath As String = strFileOfferSave

                    Dim strFileName As String = (Server.MapPath("").Replace("Forms", "Temp") & "\Report12.xlsx")
                    strDestination = strFileName
                    Dim custname As String = ""
                    If IsInArray(J, arrIndex) Then
                        intRowIndex = 5
                    End If

                    intRowIndex = 7
                    myRange = worksheet.Cell(intRowIndex - 5, 8)
                    myRange.Style.Font.Bold = True
                    myRange = worksheet.Cell(intRowIndex - 1, 8)
                    myRange.Style.Font.Bold = True
                    intRowIndex = 2

                    myRange = worksheet.Cell(intRowIndex, 1)
                    myRange.WorksheetColumn.Width = 5
                    Dim Range As String = ""
                    intRowIndex = intRowIndex + 2
                    Range = "E3:E3"
                    worksheet.Range(Range).Merge()
                    myRange = worksheet.Cell(3, 5)
                    myRange.Style.Font.Bold = True
                    myRange.Style.Font.FontSize = 14
                    myRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    worksheet.Cell(3, 5).Value = Trim(dta.Rows(b).Item("DistributorName") & ",")
                    myRange = worksheet.Cell(3, 13)
                    myRange.Style.Font.Bold = True
                    intRowIndex = intRowIndex + 1

                    myRange = worksheet.Cell(3, 6)
                    myRange.Style.Font.Bold = True
                    myRange.Style.Font.FontSize = 14
                    myRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    worksheet.Cell(4, 5).Value = Trim(dta.Rows(b).Item("CustomerAddress1") & ",")
                    myRange = worksheet.Cell(4, 10)
                    myRange.Style.Font.Bold = True
                    intRowIndex = intRowIndex + 1

                    myRange = worksheet.Cell(3, 6)
                    myRange.Style.Font.Bold = True
                    myRange.Style.Font.FontSize = 14
                    myRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    worksheet.Cell(3, 7).Value = Trim(dta.Rows(b).Item("CustomerAddress2") & ",")
                    myRange = worksheet.Cell(3, 10)
                    myRange.Style.Font.Bold = True
                    intRowIndex = intRowIndex + 1

                    myRange = worksheet.Cell(3, 8)
                    myRange.Style.Font.Bold = True
                    myRange.Style.Font.FontSize = 14
                    myRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    worksheet.Cell(3, 8).Value = Trim(dta.Rows(b).Item("CustomerCity") & ",") & "" & Trim(dta.Rows(b).Item("CustomerPinCode") & ",")
                    myRange = worksheet.Cell(3, 10)
                    myRange.Style.Font.Bold = True
                    intRowIndex = intRowIndex + 1

                    myRange = worksheet.Cell(3, 9)
                    myRange.Style.Font.Bold = True
                    myRange.Style.Font.FontSize = 14
                    myRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    worksheet.Cell(3, 9).Value = "PAN :" & "" & Trim(dta.Rows(b).Item("CustomerPAN") & ",")
                    myRange = worksheet.Cell(3, 10)
                    myRange.Style.Font.Bold = True
                    intRowIndex = intRowIndex + 1




                    If dta.Rows(b).Item("CompName") & "" <> "" Then
                        intRowIndex = intRowIndex + 1
                        myRange = worksheet.Cell(intRowIndex, 2)
                        myRange.Style.Font.Bold = False
                        worksheet.Cell(intRowIndex, 2).Value = Trim(dta.Rows(b).Item("CompName") & ",")
                        myRange = worksheet.Cell(intRowIndex, 7)
                        myRange.Style.Font.Bold = True
                        'myRange.Style.Font.FontSize = 15
                        worksheet.Cell(intRowIndex, 7).Value = "Invoice No :" & Trim(dta.Rows(b).Item("RefNo") & "")
                        custname = Trim(dta.Rows(b).Item("CompName") & "") & ","
                    End If



                    If dta.Rows(b).Item("CompanyAddress1") & "" <> "" Then
                        intRowIndex = intRowIndex + 1
                        myRange = worksheet.Cell(intRowIndex, 2)
                        myRange.Style.Font.Bold = False
                        myRange = worksheet.Cell(intRowIndex, 7)
                        myRange.Style.Font.Bold = True
                        worksheet.Cell(intRowIndex, 2).Value = Trim(dta.Rows(b).Item("CompanyAddress1") & "")
                        worksheet.Cell(intRowIndex, 7).Value = "Invoice Date :" & Trim(dta.Rows(b).Item("ToDate") & "")

                    End If
                    If dta.Rows(b).Item("CompanyAddress2") & "" <> "" Then
                        intRowIndex = intRowIndex + 1
                        myRange = worksheet.Cell(intRowIndex, 2)
                        myRange.Style.Font.Bold = False
                        myRange = worksheet.Cell(intRowIndex, 13)

                        worksheet.Cell(intRowIndex, 2).Value = Trim(dta.Rows(b).Item("CompanyAddress2") & "")
                        'worksheet.Cell(intRowIndex, 13).Value = "Service Accounting Code: 997152"

                    End If
                    If dta.Rows(b).Item("CompanyCity") & "" <> "" Then
                        intRowIndex = intRowIndex + 1
                        myRange = worksheet.Cell(intRowIndex, 2)
                        myRange.Style.Font.Bold = False
                        myRange = worksheet.Cell(intRowIndex, 13)

                        worksheet.Cell(intRowIndex, 2).Value = Trim(dta.Rows(b).Item("CompanyCity") & ",") & Trim(dta.Rows(b).Item("CompanyPinCode") & "")
                        'worksheet.Cell(intRowIndex, 13).Value = "Service Accounting Code: 997152"

                    End If

                    If dta.Rows(b).Item("PANNo") & "" <> "" Then
                        intRowIndex = intRowIndex + 1
                        myRange = worksheet.Cell(intRowIndex, 2)
                        myRange.Style.Font.Bold = False
                        myRange = worksheet.Cell(intRowIndex, 13)

                        worksheet.Cell(intRowIndex, 2).Value = "PAN :" & Trim(dta.Rows(b).Item("PANNo") & ",")


                    End If
                    intRowIndex = intRowIndex + 1
                    myRange = worksheet.Cell(intRowIndex, 6)
                    myRange.Style.Font.Underline = XLFontUnderlineValues.Single
                    myRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    worksheet.Cell(intRowIndex, 2).Value = "PARTICULARS"
                    intRowIndex = intRowIndex + 1

                    worksheet.Cell(intRowIndex, 2).Value = "Being our brokrage for arranging investors for secondary deal "
                    intRowIndex = intRowIndex + 1




                    intRowIndex = intRowIndex + 1
                    myRange = worksheet.Cell(intRowIndex, 7)
                    myRange.Style.Font.Bold = True
                    worksheet.Cell(intRowIndex, 2).Value = "Details of Transactions:"
                    myRange = worksheet.Cell(intRowIndex, 8)
                    myRange.Style.Font.Bold = True
                    myRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    worksheet.Cell(intRowIndex, 8).Value = "(All Amount in Rs.)"
                    intRowIndex = intRowIndex + 1




                    worksheet.Cell(intRowIndex, 2).Value = "SR NO."
                    worksheet.Cell(intRowIndex, 3).Value = "DEAL DATE"
                    worksheet.Cell(intRowIndex, 4).Value = "STOCK NAME"
                    worksheet.Cell(intRowIndex, 5).Value = "QTM in (Lacs)"
                    worksheet.Cell(intRowIndex, 6).Value = "BROKRAGE AMT"

                    For D As Int32 = 0 To dta.Rows.Count - 1

                        intRowIndex = intRowIndex + 1
                        myRange = worksheet.Cell(intRowIndex, 2)
                        SetTableStyle(myRange, "")
                        myRange.WorksheetColumn.Width = 10
                        For y As Int16 = 2 To 8
                            myRange = worksheet.Cell(intRowIndex, y)
                            myRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        Next
                        'worksheet.Cell(intRowIndex, 2).Style.NumberFormat.Format = "dd-MMM-yy"
                        worksheet.Cell(intRowIndex, 2).Value = Trim(dta.Rows(D).Item("SrNo") & "")
                        myRange = worksheet.Cell(intRowIndex, 3)
                        SetTableStyle(myRange, "")
                        myRange.WorksheetColumn.Width = 10

                        worksheet.Cell(intRowIndex, 3).Style.NumberFormat.Format = "dd-MMM-yy"
                        worksheet.Cell(intRowIndex, 3).Value = Trim(dta.Rows(D).Item("DealDate") & "")
                        myRange = worksheet.Cell(intRowIndex, 3)
                        SetTableStyle(myRange, "")
                        'myRange.WorksheetColumn.Width = 35
                        worksheet.Cell(intRowIndex, 3).Value = Trim(dta.Rows(D).Item("SecurityName") & "")


                        worksheet.Cell(intRowIndex, 4).Style.NumberFormat.Format = "[>=10000000] ##\,##\,##\,##0.00;[>=100000] ##\,##\,##0.00;##,##0.00"
                        myRange = worksheet.Cell(intRowIndex, 4)
                        myRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                        SetTableStyle(myRange, "")
                        myRange.WorksheetColumn.Width = 20
                        worksheet.Cell(intRowIndex, 4).Value = Val(dta.Rows(D).Item("FaceValue") & "")

                        worksheet.Cell(intRowIndex, 5).Style.NumberFormat.Format = "[>=10000000] ##\,##\,##\,##0.00;[>=100000] ##\,##\,##0.00;##,##0.00"
                        myRange = worksheet.Cell(intRowIndex, 5)
                        myRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                        SetTableStyle(myRange, "")
                        myRange.WorksheetColumn.Width = 20
                        worksheet.Cell(intRowIndex, 5).Value = Val(dta.Rows(D).Item("Amount") & "")

                        fv = fv + Val(dta.Rows(D).Item("FaceValue") & "")

                        BrokerageAmt = BrokerageAmt + Val(dta.Rows(D).Item("Amount") & "")


                    Next
                    intRowIndex = intRowIndex + 1

                    For y As Int16 = 2 To 8
                        myRange = worksheet.Cell(intRowIndex, y)
                        myRange.Style.Font.Bold = True
                        SetTableStyle(myRange, "H")
                    Next

                    Range = "B" & intRowIndex & ":" & "F" & intRowIndex
                    worksheet.Range(Range).Merge()

                    myRange = worksheet.Cell(intRowIndex, 7)
                    myRange.Style.Font.Bold = True
                    myRange = worksheet.Cell(intRowIndex, 8)
                    myRange.Style.Font.Bold = True
                    worksheet.Cell(intRowIndex, 8).Style.NumberFormat.Format = "[>=10000000] ##\,##\,##\,##0.00;[>=100000] ##\,##\,##0.00;##,##0.00"
                    worksheet.Cell(intRowIndex, 7).Value = "Total Amount"
                    worksheet.Cell(intRowIndex, 8).Value = BrokerageAmt
                    SetTableStyle(myRange, "")
                    intRowIndex = intRowIndex + 1



                    worksheet.Cell(intRowIndex, 3).Value = "Total Amount (in words) : " & AmtInWord(BrokerageAmt)
                    worksheet.Cell(intRowIndex, 8).Style.NumberFormat.Format = "[>=10000000] ##\,##\,##\,##0.00;[>=100000] ##\,##\,##0.00;##,##0.00"
                    worksheet.Cell(intRowIndex, 8).Value = (BrokerageAmt)

                    For t As Int16 = 2 To 8
                        myRange = worksheet.Cell(intRowIndex, t)
                        myRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin

                    Next
                    intRowIndex = intRowIndex + 1





                    myRange = worksheet.Cell(intRowIndex, 7)
                    myRange.Style.Font.Bold = True
                    worksheet.Cell(intRowIndex, 7).Value = "Authorised Signatory"
                    intRowIndex = intRowIndex + 2
                    myRange = worksheet.Cell(intRowIndex, 2)
                    myRange.Style.Font.Bold = True


                    worksheet.Cell(intRowIndex, 2).Value = "CREST FINSERV LIMITED"



                    intRowIndex = intRowIndex + 1

                    worksheet.Cell(intRowIndex, 2).Value = "Formerly known as Tullett Prebon (India) Ltd & Prebon Yamane (India) Ltd."



                    intRowIndex = intRowIndex + 1
                    myRange = worksheet.Cell(intRowIndex, 6)
                    myRange.IsMerged()
                    worksheet.Cell(intRowIndex, 2).Value = "Reg. Add :4th Floor, Kalpataru Heritage,127, M.G. Road, Fort,Mumbai - 400 001. Telephone : 2262 4341 Facsimile : 2262 4350, Email: wdm@crestfinserv.com"
                    intRowIndex = intRowIndex + 1
                    worksheet.Cell(intRowIndex, 2).Value = "CIN: U65990MH1995PLC091626"
                    workbook.SaveAs(strDestination & "\" & dta.Rows(b).Item("DistributorName") & "" & ".xlsx")

                    HttpContext.Current.Response.Clear()
                    HttpContext.Current.Response.Buffer = True
                    HttpContext.Current.Response.Charset = ""
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" & dta.Rows(b).Item("DistributorName") & "" & ".xlsx")
                    HttpContext.Current.Response.Charset = ""
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    Dim MyMemoryStream As MemoryStream = New MemoryStream()
                    'MemoryStream(MyMemoryStream = New MemoryStream())
                    workbook.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream)
                    HttpContext.Current.Response.Flush()
                    HttpContext.Current.Response.End()

                Next
            Else
                Dim strHtml As String
                strHtml = "alert('Sorry!!! No Records available to show report');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
            End If

        Catch ex As Exception

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub SetTableStyle(ByRef myRange As ClosedXML.Excel.IXLCell, Optional ByVal str As String = "")
        Try
            myRange.IsMerged()
            With myRange.Style.Border
                .BottomBorder = XLBorderStyleValues.Thin
                .LeftBorder = XLBorderStyleValues.Thin
                .RightBorder = XLBorderStyleValues.Thin
                .TopBorder = XLBorderStyleValues.Thin

            End With
            If str = "H" Then
                'With myRange.Style.Fill
                '    .BackgroundColor = XLColor.AliceBlue
                'End With
                With myRange.Style.Font
                    .Bold = True
                End With
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function IsInArray(ByVal FindValue As Object, ByVal arrSearch As Object) As Boolean
        Dim blnExist As Boolean = False
        If Not IsArray(arrSearch) Then Return blnExist
        If Array.IndexOf(arrSearch, FindValue) >= 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Function strReplicate(ByVal str As String, ByVal intD As Integer) As String
        'This fucntion padded "0" after the number to evaluate hundred, thousand and on....
        'using this function you can replicate any Charactor with given string.
        Dim i As Integer
        strReplicate = ""
        For i = 1 To intD
            strReplicate = strReplicate + str
        Next
        Return strReplicate
    End Function

    Function AmtInWord(ByVal Num As Decimal) As String
        'I have created this function for converting amount in indian rupees (INR). 
        'You can manipulate as you wish like decimal setting, Doller (any currency) Prefix.

        Dim strNum As String
        Dim strNumDec As String
        Dim StrWord As String
        strNum = Num

        If InStr(1, strNum, ".") <> 0 Then
            strNumDec = Mid(strNum, InStr(1, strNum, ".") + 1)

            If Len(strNumDec) = 1 Then
                strNumDec = strNumDec + "0"
            End If
            If Len(strNumDec) > 2 Then
                strNumDec = Mid(strNumDec, 1, 2)
            End If

            strNum = Mid(strNum, 1, InStr(1, strNum, ".") - 1)
            StrWord = IIf(CDbl(strNum) = 1, " INR ", " INR ") + NumToWord(CDbl(strNum)) + IIf(CDbl(strNumDec) > 0, " and Paise" + cWord3(CDbl(strNumDec)), "")
        Else
            StrWord = IIf(CDbl(strNum) = 1, " INR ", " INR ") + NumToWord(CDbl(strNum))
        End If
        AmtInWord = StrWord & " Only"
        Return AmtInWord
    End Function

    Function NumToWord(ByVal Num As Decimal) As String
        'I divided this function in two part.
        '1. Three or less digit number.
        '2. more than three digit number.
        Dim strNum As String
        Dim StrWord As String
        strNum = Num

        If Len(strNum) <= 3 Then
            StrWord = cWord3(CDbl(strNum))
        Else
            StrWord = cWordG3(CDbl(Mid(strNum, 1, Len(strNum) - 3))) + " " + cWord3(CDbl(Mid(strNum, Len(strNum) - 2)))
        End If
        NumToWord = StrWord
    End Function
    Function cWordG3(ByVal Num As Decimal) As String
        '2. more than three digit number.
        Dim strNum As String = ""
        Dim StrWord As String = ""
        Dim readNum As String = ""
        strNum = Num
        If Len(strNum) Mod 2 <> 0 Then
            readNum = CDbl(Mid(strNum, 1, 1))
            If readNum <> "0" Then
                StrWord = retWord(readNum)
                readNum = CDbl("1" + strReplicate("0", Len(strNum) - 1) + "000")
                StrWord = StrWord + " " + retWord(readNum)
            End If
            strNum = Mid(strNum, 2)
        End If
        While Not Len(strNum) = 0
            readNum = CDbl(Mid(strNum, 1, 2))
            If readNum <> "0" Then
                StrWord = StrWord + " " + cWord3(readNum)
                readNum = CDbl("1" + strReplicate("0", Len(strNum) - 2) + "000")
                StrWord = StrWord + " " + retWord(readNum)
            End If
            strNum = Mid(strNum, 3)
        End While
        cWordG3 = StrWord
        Return cWordG3
    End Function
    Function cWord3(ByVal Num As Decimal) As String
        '1. Three or less digit number.
        Dim strNum As String = ""
        Dim StrWord As String = ""
        Dim readNum As String = ""
        If Num < 0 Then Num = Num * -1
        strNum = Num

        If Len(strNum) = 3 Then
            readNum = CDbl(Mid(strNum, 1, 1))
            StrWord = retWord(readNum) + " Hundred"
            strNum = Mid(strNum, 2, Len(strNum))
        End If

        If Len(strNum) <= 2 Then
            If CDbl(strNum) >= 0 And CDbl(strNum) <= 20 Then
                StrWord = StrWord + " " + retWord(CDbl(strNum))
            Else
                StrWord = StrWord + " " + retWord(CDbl(Mid(strNum, 1, 1) + "0")) + " " + retWord(CDbl(Mid(strNum, 2, 1)))
            End If
        End If

        strNum = CStr(Num)
        cWord3 = StrWord
        Return cWord3
    End Function


    Function retWord(ByVal Num As Decimal) As String
        'This two dimensional array store the primary word convertion of number.

        retWord = ""
        Dim ArrWordList(,) As Object = {{0, ""}, {1, "One"}, {2, "Two"}, {3, "Three"}, {4, "Four"},
                                        {5, "Five"}, {6, "Six"}, {7, "Seven"}, {8, "Eight"}, {9, "Nine"},
                                        {10, "Ten"}, {11, "Eleven"}, {12, "Twelve"}, {13, "Thirteen"}, {14, "Fourteen"},
                                        {15, "Fifteen"}, {16, "Sixteen"}, {17, "Seventeen"}, {18, "Eighteen"}, {19, "Nineteen"},
                                        {20, "Twenty"}, {30, "Thirty"}, {40, "Forty"}, {50, "Fifty"}, {60, "Sixty"},
                                        {70, "Seventy"}, {80, "Eighty"}, {90, "Ninety"}, {100, "Hundred"}, {1000, "Thousand"},
                                        {100000, "Lakh"}, {10000000, "Crore"}}

        Dim i As Integer
        For i = 0 To UBound(ArrWordList)
            If Num = ArrWordList(i, 0) Then
                retWord = ArrWordList(i, 1)
                Exit For
            End If
        Next
        Return retWord
    End Function
End Class
