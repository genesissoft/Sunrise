Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports RKLib.ExportData
Imports CrystalDecisions.Web
Imports GemBox.Spreadsheet
Imports GemBox.Spreadsheet.ExcelFile
Imports System.Linq
Partial Class Forms_MISReportSelection_Retail
    Inherits System.Web.UI.Page
    Dim sqlconn As SqlConnection
    Dim objCommon As New clsCommonFuns
    Dim oXL As Excel.Application
    Dim rptDoc As New ReportDocument
    Dim rptDoc1 As New ReportDocument
    Dim oSheet As Excel.Worksheet
    Dim strFilePath As String
    Dim dtRpt As DataTable
    Dim dtRpt1 As DataTable
    Dim row1 As Integer = 1
    Dim col1 As Integer = 1
    Dim rowEnd As Integer
    Public objExcel As Excel.Application
    Dim CompName As String = ""
    Dim objMIS As New MISReports
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub

            End If

            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            btn_Print.Attributes.Add("onclick", "return  Validation();")
            If IsPostBack = False Then
                Hid_ReportType.Value = Trim(Request.QueryString("Rpt") & "")
                SetAttributes()
                If Hid_ReportType.Value = "InwardOutwardSecurityWise" Then
                    FillInwardOutWardGrid()
                End If
            Else
            End If

            If Hid_ReportType.Value = "InwardOutwardSecurityWise" Then
                trInwardOutwardSecurity.Attributes.Add("style", "display:")
                trInwardOutwardGrid.Attributes.Add("style", "display:")
                srh_Security_.SearchTextBox.Width = Hid_SecurityWidth.Value
                FillDate()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Public Sub FillDate()
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable(sqlconn, "ID_FILL_YEAR", Session("YearId"), "YearId")
            Dim Year As String = ""

            If dt.Rows.Count > 0 Then

                If DateTime.Today.Month <= 3 Then
                    Dim CurrentYear As String = DateTime.Today.Year.ToString()
                    Dim FindYear As String = CurrentYear - 1
                    Dim CurrFinanYr = FindYear + "-" + CurrentYear

                    txt_FromDate.Text = Format(dt.Rows(0).Item("StartDate"), "dd/MM/yyyy")
                    txt_ToDate.Text = Format(dt.Rows(0).Item("EndDate"), "dd/MM/yyyy")
                Else
                    Dim CurrentYear As String = DateTime.Today.Year.ToString()
                    Dim NextYear As String = CType((DateTime.Today.Year + 1), String)
                    Dim FindYear As String = CurrentYear + "-" + NextYear
                    txt_FromDate.Text = Format(dt.Rows(0).Item("StartDate"), "dd/MM/yyyy")
                    txt_ToDate.Text = Format(dt.Rows(0).Item("EndDate"), "dd/MM/yyyy")
                End If

            End If
        Catch ex As Exception
        Finally
            CloseConn()
        End Try

    End Sub
    Private Sub FillInwardOutWardGrid()
        Try
            dtRpt = New DataTable()
            dtRpt = GetReportTable("ID_FILL_InwardOutwardSecurityWiseStock_Rpt")
            If dtRpt.Rows.Count > 0 Then
                Dim I As Integer
                For I = 0 To dtRpt.Columns.Count - 1
                    dtRpt.Rows(0).Item(I) = DBNull.Value
                Next
                dg_InwardOutwrad.DataSource = dtRpt
                dg_InwardOutwrad.DataBind()
            End If
        Catch ex As Exception
            Dim ex_ As String = ex.Message.ToString()
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
    Public Sub SetAttributes()
        txt_FromDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
        txt_FromDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_FromDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_ToDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_ToDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_ToDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
        'btn_Print.Attributes.Add("onclick", "return  Validation();")
        'srh_UserBusinessHead.Columns.Add("NameOfUser")
        'srh_UserBusinessHead.Columns.Add("userid")
        'btn_Print.Attributes.Add("onclick", "return Validation();")
        If Hid_ReportType.Value = "IPDateReport" Then
            row_IPDate.Visible = True
            row_FromDate.Visible = True
            row_ToDate.Visible = True
        ElseIf Hid_ReportType.Value = "RetailDebitRpt" Then
            row_IPDate.Visible = False
            row_FromDate.Visible = True
            row_ToDate.Visible = True
            row_RetailDebitNote.Visible = True
        End If



    End Sub
    Protected Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Try
            Dim strCond As String = ""
            Dim strRpt As String
            strRpt = Hid_ReportType.Value
            Session("strWhereClause") = ""
            If Hid_ReportType.Value = "IPDateReport" Then
                If rdo_IPDate.SelectedValue = "B" Then
                    dtRpt = GetReportTable("ID_RPT_IPDateReport_backdated")
                Else
                    dtRpt = GetReportTable("ID_RPT_IPDateReport")
                End If
                If dtRpt IsNot Nothing And dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_IPDateReport(dtRpt, Trim(txt_FromDate.Text), Trim(txt_ToDate.Text), Trim(rdo_IPDate.SelectedValue))
                Else
                    Dim strHtml As String
                    strHtml = "alert('Sorry!!! No Records available to show report');"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "DealsUpdate" Then
                dtRpt = GetReportTable("ID_RPT_DealsUpdateReport")
                If dtRpt IsNot Nothing And dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_DealsUpdate(dtRpt, Trim(txt_FromDate.Text), Trim(txt_ToDate.Text))
                Else
                    Dim strHtml As String
                    strHtml = "alert('Sorry!!! No Records available to show report');"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "NSCCL_ICCLUpload" Then
                dtRpt = GetReportTable("ID_RPT_UploadNSCCL_ICCL", "NSCCL")
                Session("NSCCLUpload") = dtRpt
                dtRpt1 = GetReportTable("ID_RPT_UploadNSCCL_ICCL", "ICCL")
                Session("ICCLUpload") = dtRpt1

                If dtRpt.Rows.Count > 0 Or dtRpt1.Rows.Count > 0 Then
                    objMIS.ExportToExcel_NSCCL_ICCLUploadReport(dtRpt, Trim(txt_FromDate.Text), Trim(txt_ToDate.Text))
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "PropDealRpt" Then
                dtRpt = GetReportTable("IT_RPT_PropDealReport")
                If dtRpt IsNot Nothing And dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_PropDealRpt(dtRpt, Trim(txt_FromDate.Text), Trim(txt_ToDate.Text))
                Else
                    Dim strHtml As String
                    strHtml = "alert('Sorry!!! No Records available to show report');"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "RetailDebitRpt" Then
                strCond = BuildRetailDebitRefStr(srh_RetailDebit.SelectCheckBox, srh_RetailDebit.SelectListBox)
                Response.Redirect("ViewReports.aspx?&Rpt=" & Hid_ReportType.Value & "&DealTransType=" & "O" & "&Fromdate=" & txt_FromDate.Text & "&Todate=" & txt_ToDate.Text, False)
            ElseIf (Trim(Request.QueryString("Rpt") & "") = "InwardOutwardSecurityWise") Then
                BindInwardOutwardData()
                ' objMIS.ExportToExcel_MISInwardOutwardSecurityWiseStockReport(dtRpt, Convert.ToChar(Hid_InwardOutwardIndex.Value), Convert.ToChar(Hid_InwardOutwardLastIndex.Value), Convert.ToString(srh_Security_.SearchTextBox.Text), Convert.ToInt32(Hid_InwardOutwardDeleteIndex.Value))
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
            End If


        Catch ex As Exception
            Dim ex_ As String = ex.Message.ToString()
        Finally
            'Server.Transfer("MISReportSelection_Retail.aspx", False)
        End Try
    End Sub

    Private Function BuildRetailDebitRefStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox) As String
        Try
            Dim strCond As String = ""
            Dim strIds As String = ""
            Session("RetailDebitRefNo") = "0"
            If chkbox.Checked = False Then
                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strIds += lstbox.Items(I).Value & ","
                    End If
                Next
                Session("RetailDebitRefNo") = ""
                Session("RetailDebitRefNo") = Mid(strIds, 1, Len(strIds) - 1)
            End If
            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function


    Public Sub BindInwardOutwardData()
        dtRpt = New DataTable()
        dtRpt = GetReportTable("ID_FILL_InwardOutwardSecurityWiseStock_Rpt")
        If dtRpt.Rows.Count > 0 Then
            dg_InwardOutwrad.ShowFooter = True
            dg_InwardOutwrad.DataSource = dtRpt

            Dim A1, A2, A3, A4, A5, A6 As Decimal
            For Each row As DataRow In dtRpt.Rows
                If Convert.ToString(row("A1").ToString()) <> "" Then
                    A1 += row("A1")
                End If

                If Convert.ToString(row("A2").ToString()) <> "" Then
                    A2 += row("A2")
                End If

                If Convert.ToString(row("A3").ToString()) <> "" Then
                    A3 += row("A3")
                End If

                If Convert.ToString(row("A4").ToString()) <> "" Then
                    A4 += row("A4")
                End If

                'If Convert.ToString(row("A5").ToString()) <> "" Then
                '    A5 += row("A5")
                'End If

                'If Convert.ToString(row("A6").ToString()) <> "" Then
                '    A6 += row("A6")
                'End If

            Next row

            A5 = A1 - A3
            A6 = A2 - A4

            'Calculate Sum and display in Footer Row
            '     Dim total As Decimal = dtRpt.AsEnumerable().Sum(Function(row) Convert.ToDecimal(If(Not String.IsNullOrEmpty(row.Field(Of Decimal)("A1")), row.Field(Of String)("A1"), "0")))
            'Dim total As Decimal = dtRpt.AsEnumerable().Sum(Function(row) Convert.ToDecimal(If(Not String.IsNullOrEmpty(row.Field(Of String)("Rate")), row.Field(Of String)("Rate"), "0")))

            'dg_InwardOutwrad.FooterRow.Cells(0).Text = "Total"
            'dg_InwardOutwrad.FooterRow.Cells(1).HorizontalAlign = HorizontalAlign.Right
            'dg_InwardOutwrad.FooterRow.Cells(2).Text = total.ToString("N2")

            dg_InwardOutwrad.Columns(0).FooterText = "Total"
            dg_InwardOutwrad.Columns(0).FooterStyle.Font.Bold = True
            dg_InwardOutwrad.Columns(0).FooterStyle.HorizontalAlign = HorizontalAlign.Left

            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A1_FooterIndex.Value)).FooterText = A1.ToString()
            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A1_FooterIndex.Value)).FooterStyle.Font.Bold = True
            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A1_FooterIndex.Value)).FooterStyle.HorizontalAlign = HorizontalAlign.Left

            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A2_FooterIndex.Value)).FooterText = A2.ToString("N2")
            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A2_FooterIndex.Value)).FooterStyle.Font.Bold = True
            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A2_FooterIndex.Value)).FooterStyle.HorizontalAlign = HorizontalAlign.Left

            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A3_FooterIndex.Value)).FooterText = A3.ToString()
            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A3_FooterIndex.Value)).FooterStyle.Font.Bold = True
            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A3_FooterIndex.Value)).FooterStyle.HorizontalAlign = HorizontalAlign.Left

            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A4_FooterIndex.Value)).FooterText = A4.ToString("N2")
            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A4_FooterIndex.Value)).FooterStyle.Font.Bold = True
            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A4_FooterIndex.Value)).FooterStyle.HorizontalAlign = HorizontalAlign.Left

            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A5_FooterIndex.Value)).FooterText = A5.ToString()
            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A5_FooterIndex.Value)).FooterStyle.Font.Bold = True
            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A5_FooterIndex.Value)).FooterStyle.HorizontalAlign = HorizontalAlign.Left

            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A6_FooterIndex.Value)).FooterText = A6.ToString("N2")
            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A6_FooterIndex.Value)).FooterStyle.Font.Bold = True
            dg_InwardOutwrad.Columns(Convert.ToInt32(Hid_A6_FooterIndex.Value)).FooterStyle.HorizontalAlign = HorizontalAlign.Left

            dg_InwardOutwrad.DataBind()
        End If
    End Sub
    Protected Sub OnPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        dg_InwardOutwrad.PageIndex = e.NewPageIndex
        Me.BindInwardOutwardData()
    End Sub
    Protected Sub btn_Export_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Export.Click
        Try
            dtRpt = New DataTable()
            dtRpt = GetReportTable("ID_FILL_InwardOutwardSecurityWiseStock_Rpt")
            If dtRpt.Rows.Count > 0 Then
                objMIS.ExportToExcel_MISInwardOutwardSecurityWiseStockReport(dtRpt, Convert.ToChar(Hid_InwardOutwardIndex.Value), Convert.ToChar(Hid_InwardOutwardLastIndex.Value), Convert.ToString(srh_Security_.SearchTextBox.Text), Convert.ToInt32(Hid_InwardOutwardDeleteIndex.Value), Hid_InwardOutwardRowIndex.Value)
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ConvertToExcel(ByVal strProcName As String)
        Try
            Dim dt As DataTable
            Dim ds As New DataSet
            Dim tw As New System.IO.StringWriter()
            Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            Dim dgGrid As New DataGrid()

            Dim strdtTime As String = DateAndTime.TimeString

            Response.ClearHeaders()
            dt = GetReportTable(strProcName)
            dgGrid.DataSource = dt
            If dt.Rows.Count > 0 Then
                Dim objExport As New RKLib.ExportData.Export("Web")
                objExport.ExportDetails(dt, Export.ExportFormat.Excel, "Report" & "." & strdtTime & ".xls")
            Else
                Dim strHtml As String
                strHtml = "alert('Sorry!!! No Records available to show report');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub SaveReports(ByVal strRptName As String, ByVal strProcName As String, ByVal strSheetName As String)
        Try
            BindReport(strRptName, strProcName)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub BindReport(ByVal strRptName As String, ByVal strProcName As String)
        Try
            Dim strPath As String
            Dim dtRpt As DataTable
            rptDoc = New ReportDocument
            strPath = ConfigurationManager.AppSettings("ReportsPath") & "\" & strRptName & ".rpt"
            rptDoc.Load(strPath)

            If strRptName = "IPDateRpt" Then
                If rdo_IPDate.SelectedValue = "P" Then
                    dtRpt = GetReportTable("ID_RPT_IPDateReport")
                Else
                    dtRpt = GetReportTable("ID_RPT_IPDateReport_backdated")
                End If

            Else
                dtRpt = GetReportTable("ID_Rpt_RETAIL_MIS")
            End If


            rptDoc.SetDataSource(dtRpt)
            rptDoc.ExportToDisk(ExportFormatType.ExcelRecord, Server.MapPath("").Replace("Forms", "Temp") & "\" & strRptName & ".xls")

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            rptDoc.Close()
            rptDoc.Dispose()
            GC.Collect()
        End Try
    End Sub

    Private Function GetReportTable(ByVal strProcName As String, Optional ByVal strExhangeType As String = "") As DataTable
        Try
            OpenConn()
            Dim datFrom As Date
            Dim datTo As Date
            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand
            datFrom = objCommon.DateFormat(txt_FromDate.Text)
            datTo = objCommon.DateFormat(txt_ToDate.Text)
            sqlComm.CommandTimeout = 0
            With sqlComm
                .Connection = sqlconn
                .CommandType = CommandType.StoredProcedure
                .CommandText = strProcName
                .Parameters.Clear()
                If Hid_ReportType.Value = "" Then
                    objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)
                    objCommon.SetCommandParameters(sqlComm, "@Yearid", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                    objCommon.SetCommandParameters(sqlComm, "@ToDate", SqlDbType.SmallDateTime, 8, "I", , , datTo)
                    objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
                ElseIf Hid_ReportType.Value = "DealsUpdate" Then
                    objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)
                    objCommon.SetCommandParameters(sqlComm, "@ToDate", SqlDbType.SmallDateTime, 8, "I", , , datTo)
                ElseIf Hid_ReportType.Value = "IPDateReport" Then
                    objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)
                    objCommon.SetCommandParameters(sqlComm, "@Yearid", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                    objCommon.SetCommandParameters(sqlComm, "@ToDate", SqlDbType.SmallDateTime, 8, "I", , , datTo)
                    objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
                ElseIf Hid_ReportType.Value = "NSCCL_ICCLUpload" Then
                    objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)
                    objCommon.SetCommandParameters(sqlComm, "@Yearid", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                    objCommon.SetCommandParameters(sqlComm, "@ToDate", SqlDbType.SmallDateTime, 8, "I", , , datTo)
                    objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
                    objCommon.SetCommandParameters(sqlComm, "@ExchangeType", SqlDbType.Char, 10, "I", , , strExhangeType)
                ElseIf Hid_ReportType.Value = "PropDealRpt" Then
                    objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)
                    objCommon.SetCommandParameters(sqlComm, "@Yearid", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                    objCommon.SetCommandParameters(sqlComm, "@ToDate", SqlDbType.SmallDateTime, 8, "I", , , datTo)
                    objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))

                ElseIf Hid_ReportType.Value = "InwardOutwardSecurityWise" Then
                    objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)
                    objCommon.SetCommandParameters(sqlComm, "@ToDate", SqlDbType.SmallDateTime, 8, "I", , , datTo)
                    objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
                    objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                    objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId") & ""))
                    objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId") & ""))
                    objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , Val(srh_Security_.SelectedId))

                End If

                .ExecuteNonQuery()
            End With
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dtfill)
            Session("RptData") = dtfill
            Return dtfill
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try
    End Function
    Public Sub ExportToExcelsFSTP()
        Dim strFileName As String
        'The full path where the excel file will be stored
        'Dim strFileName As String = AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\")
        strFileName = (Server.MapPath("").Replace("Forms", "Temp") & "\Reportfs.xls")
        'strFileName = strFileName & "\MyExcelFile" & System.DateTime.Now.Ticks.ToString() ".xls"


        Dim objBooks As Excel.Workbooks, objBook As Excel.Workbook
        Dim objSheets As Excel.Sheets
        ', objSheet As Excel.Worksheet oSheet
        Dim objRange As Excel.Range

        Try
            'Creating a new object of the Excel application object
            objExcel = New Excel.Application
            'Hiding the Excel application
            objExcel.Visible = False
            'Hiding all the alert messages occurring during the process
            objExcel.DisplayAlerts = False

            'Adding a collection of Workbooks to the Excel object
            objBook = CType(objExcel.Workbooks.Add(), Excel.Workbook)


            If File.Exists(Server.MapPath("").Replace("Forms", "Temp") & "\Reportfs.xls") Then
                File.Delete(Server.MapPath("").Replace("Forms", "Temp") & "\Reportfs.xls")
            End If


            'Saving the Workbook as a normal workbook format.
            objBook.SaveAs(strFileName, Excel.XlFileFormat.xlWorkbookNormal)

            'Getting the collection of workbooks in an object
            objBooks = objExcel.Workbooks


            oSheet = CType(objBooks(1).Sheets.Item(1), Excel.Worksheet)
            oSheet.Name = "Stock Marked"
            objRange = oSheet.Cells
            objExcel.Cells.ColumnWidth = 15
            objExcel.Cells(1, 1).Font.Bold = True
            objExcel.Cells(1, 1).Font.size = 12
            SaveReports("StockMarked", "ID_RPT_MIS_FSTP_StockMarked", "Stock Marked")
            objExcel.Workbooks.Open(Server.MapPath("").Replace("Forms", "Temp") & "\" & "StockMarked" & ".xls")
            objExcel.ActiveSheet.Cells.Select()
            objExcel.Selection.Copy()
            objExcel.Windows("Reportfs.xls").Activate()
            objExcel.ActiveSheet.Paste()
            oSheet.SaveAs(strFileName)


            'oSheet = CType(objBooks(1).Sheets.Item(1), Excel.Worksheet)
            oSheet = CType(objBook.Sheets.Add, Excel.Worksheet)
            oSheet.Name = "TP Stock"
            objRange = oSheet.Cells
            objExcel.Cells.ColumnWidth = 15
            SaveReports("FSTPStock", "ID_RPT_MIS_FSTP_Stock", "FSTP Stock")
            objExcel.Workbooks.Open(Server.MapPath("").Replace("Forms", "Temp") & "\" & "FSTPStock" & ".xls")
            objExcel.ActiveSheet.Cells.Select()
            objExcel.Selection.Copy()
            objExcel.Windows("Reportfs.xls").Activate()
            objExcel.ActiveSheet.Paste()
            'Format Bond Sheet
            oSheet.SaveAs(strFileName)



            oSheet = CType(objBook.Sheets.Add, Excel.Worksheet)
            oSheet.Name = "FS Short"
            objRange = oSheet.Cells
            objExcel.Cells.ColumnWidth = 15
            SaveReports("FSShort", "ID_RPT_MIS_FS_Short", "FS Short")
            objExcel.Workbooks.Open(Server.MapPath("").Replace("Forms", "Temp") & "\" & "FSShort" & ".xls")
            objExcel.ActiveSheet.Cells.Select()
            objExcel.Selection.Copy()
            objExcel.Windows("Reportfs.xls").Activate()
            objExcel.ActiveSheet.Paste()
            oSheet.SaveAs(strFileName)
            objSheets = objBook.Worksheets
            oSheet.SaveAs(strFileName)

            '=================================================================
            For Each xlSheet As Excel.Worksheet In objExcel.ActiveWorkbook.Sheets
                If xlSheet.Name.IndexOf("Sheet") <> -1 Then
                    xlSheet.Delete()
                End If
            Next

            objExcel.ActiveWorkbook.Save()
            objExcel.ActiveWorkbook.Close()

            objExcel.Quit()
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objBook)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objSheets)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objExcel)
            objBook = Nothing
            objSheets = Nothing
            objExcel = Nothing
            GC.Collect()


            With HttpContext.Current.Response
                .Clear()
                .Charset = ""
                .ClearHeaders()
                .ContentType = "application/vnd.ms-excel"
                .AddHeader("content-disposition", "attachment;filename=" + "FS_TP_Stock.xls")
                .WriteFile(strFileName)
                .Flush()
                .End()
            End With
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            'Close the Excel application

            If objExcel IsNot Nothing Then
                If objExcel.Workbooks.Count > 0 Then objExcel.ActiveWorkbook.Close()
                objExcel.Quit()
                objExcel = Nothing
                'objExcel.Collect()
            End If

            objExcel = Nothing
            objBooks = Nothing
            objBook = Nothing
            objSheets = Nothing
            oSheet = Nothing
            objRange = Nothing

            'Specifically call the garbage collector.
            System.GC.Collect()
        End Try
    End Sub
    Public Sub ExportToExcelsFSTPAccounts()
        Dim strFileName As String
        Dim strdtTime As String = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt")
        strdtTime = Replace(strdtTime, "/", "")
        strdtTime = Replace(strdtTime, ":", "")
        strdtTime = Replace(strdtTime, " ", "_")
        strdtTime = Replace(strdtTime, "_PM", "")
        strdtTime = Replace(strdtTime, "_AM", "")
        'The full path where the excel file will be stored
        'Dim strFileName As String = AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\")
        strFileName = (Server.MapPath("").Replace("Forms", "Temp") & "\Reportacc.xls")
        'strFileName = strFileName & "\MyExcelFile" & System.DateTime.Now.Ticks.ToString() ".xls"

        CompName = ""
        Dim objBooks As Excel.Workbooks, objBook As Excel.Workbook
        Dim objSheets As Excel.Sheets
        ', objSheet As Excel.Worksheet oSheet
        Dim objRange As Excel.Range

        Try
            'Creating a new object of the Excel application object
            objExcel = New Excel.Application
            'Hiding the Excel application
            objExcel.Visible = False
            'Hiding all the alert messages occurring during the process
            objExcel.DisplayAlerts = False

            'Adding a collection of Workbooks to the Excel object
            objBook = CType(objExcel.Workbooks.Add(), Excel.Workbook)


            If File.Exists(Server.MapPath("").Replace("Forms", "Temp") & "\Reportacc.xls") Then
                File.Delete(Server.MapPath("").Replace("Forms", "Temp") & "\Reportacc.xls")
            End If


            'Saving the Workbook as a normal workbook format.
            objBook.SaveAs(strFileName, Excel.XlFileFormat.xlWorkbookNormal)

            'Getting the collection of workbooks in an object
            objBooks = objExcel.Workbooks


            oSheet = CType(objBooks(1).Sheets.Item(1), Excel.Worksheet)
            oSheet.Name = "Stock Marked"
            objRange = oSheet.Cells
            objExcel.Cells.ColumnWidth = 15
            objExcel.Cells(1, 1).Font.Bold = True
            objExcel.Cells(1, 1).Font.size = 12
            SaveReports("FSTPAccounts", "ID_RPT_MIS_FSTP_ACCOUNTS", "FSTPAccounts")
            objExcel.Workbooks.Open(Server.MapPath("").Replace("Forms", "Temp") & "\" & "FSTPAccounts" & ".xls")
            objExcel.ActiveSheet.Cells.Select()
            objExcel.Selection.Copy()
            objExcel.Windows("Reportacc.xls").Activate()
            objExcel.ActiveSheet.Paste()
            oSheet.SaveAs(strFileName)
            'oSheet = CType(objBooks(1).Sheets.Item(1), Excel.Worksheet)
            oSheet = CType(objBook.Sheets.Add, Excel.Worksheet)
            oSheet.Name = "TP Stock"
            objRange = oSheet.Cells
            objExcel.Cells.ColumnWidth = 15


            SaveReports("FSTPStockAccounts", "ID_RPT_MIS_FSTP_StockAccounts", "FSTP Stock")
            objExcel.Workbooks.Open(Server.MapPath("").Replace("Forms", "Temp") & "\" & "FSTPStockAccounts" + ".xls")
            objExcel.ActiveSheet.Cells.Select()
            objExcel.Selection.Copy()

            objExcel.Windows("Reportacc.xls").Activate()
            objExcel.ActiveSheet.Paste()
            'Format Bond Sheet
            oSheet.SaveAs(strFileName)
            oSheet = CType(objBook.Sheets.Add, Excel.Worksheet)
            oSheet.Name = "FS Short"
            objRange = oSheet.Cells
            objExcel.Cells.ColumnWidth = 15
            SaveReports("ViewdDealStockFinanNormalExp", "ID_FILL_ViewFinancialDealStockFSTPAccounts", "FS Short")
            objExcel.Workbooks.Open(Server.MapPath("").Replace("Forms", "Temp") & "\" & "ViewdDealStockFinanNormalExp" & ".xls")
            objExcel.ActiveSheet.Cells.Select()
            objExcel.Selection.Copy()
            objExcel.Windows("Reportacc.xls").Activate()
            objExcel.ActiveSheet.Paste()
            oSheet.SaveAs(strFileName)
            objSheets = objBook.Worksheets
            oSheet.SaveAs(strFileName)

            oSheet = CType(objBook.Sheets.Add, Excel.Worksheet)
            oSheet.Name = "FS-TP Marking"

            objRange = oSheet.Cells
            objExcel.Cells.ColumnWidth = 15
            SaveReports("FSTPAccountsPurSell", "ID_RPT_MIS_FSTP_ACCOUNTS", "FSTPAccountsPurSell")
            objExcel.Workbooks.Open(Server.MapPath("").Replace("Forms", "Temp") & "\" & "FSTPAccountsPurSell" & ".xls")
            objExcel.ActiveSheet.Cells.Select()
            objExcel.Selection.Copy()
            objExcel.Windows("Reportacc.xls").Activate()
            objExcel.ActiveSheet.Paste()
            oSheet.SaveAs(strFileName)
            'objSheets = objBook.Worksheets
            'oSheet.SaveAs(strFileName)
            'TIAPL


            oSheet = CType(objBook.Sheets.Add, Excel.Worksheet)
            oSheet.Name = "TIAPL Marking"
            CompName = "TIAPL"
            objRange = oSheet.Cells
            objExcel.Cells.ColumnWidth = 15
            SaveReports("TIAPLAccounts", "ID_RPT_ACCOUNTS_TIAPL_TFC", "TIAPLAccounts")
            objExcel.Workbooks.Open(Server.MapPath("").Replace("Forms", "Temp") & "\" & "TIAPLAccounts" & ".xls")
            objExcel.ActiveSheet.Cells.Select()
            objExcel.Selection.Copy()
            objExcel.Windows("Reportacc.xls").Activate()
            objExcel.ActiveSheet.Paste()
            'Format Bond Sheet
            oSheet.SaveAs(strFileName)



            oSheet = CType(objBook.Sheets.Add, Excel.Worksheet)
            oSheet.Name = "TFC Marking"
            CompName = "TFC"
            objRange = oSheet.Cells
            objExcel.Cells.ColumnWidth = 15
            SaveReports("TFCAccounts", "ID_RPT_ACCOUNTS_TIAPL_TFC", "TFCAccounts")
            objExcel.Workbooks.Open(Server.MapPath("").Replace("Forms", "Temp") & "\" & "TFCAccounts" & ".xls")
            objExcel.ActiveSheet.Cells.Select()
            objExcel.Selection.Copy()
            objExcel.Windows("Reportacc.xls").Activate()
            objExcel.ActiveSheet.Paste()
            oSheet.SaveAs(strFileName)
            objSheets = objBook.Worksheets
            oSheet.SaveAs(strFileName)


            'objSheets = objBook.Worksheets
            'oSheet.SaveAs(strFileName)

            '=================================================================
            For Each xlSheet As Excel.Worksheet In objExcel.ActiveWorkbook.Sheets
                If xlSheet.Name.IndexOf("Sheet") <> -1 Then
                    xlSheet.Delete()
                End If
            Next

            objExcel.ActiveWorkbook.Save()
            objExcel.ActiveWorkbook.Close()

            objExcel.Quit()
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objBook)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objSheets)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objExcel)
            objBook = Nothing
            objSheets = Nothing
            objExcel = Nothing
            GC.Collect()


            With HttpContext.Current.Response
                .Clear()
                .Charset = ""
                .ClearHeaders()
                .ContentType = "application/vnd.ms-excel"
                .AddHeader("content-disposition", "attachment;filename=" + "FS_TP_Stock" + strdtTime + ".xls")
                .WriteFile(strFileName)
                .Flush()
                .End()
            End With
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            'Close the Excel application

            If objExcel IsNot Nothing Then
                If objExcel.Workbooks.Count > 0 Then objExcel.ActiveWorkbook.Close()
                objExcel.Quit()
                objExcel = Nothing
                'objExcel.Collect()
            End If

            objExcel = Nothing
            objBooks = Nothing
            objBook = Nothing
            objSheets = Nothing
            oSheet = Nothing
            objRange = Nothing

            GC.Collect()
            'Specifically call the garbage collector.
            System.GC.Collect()
        End Try
    End Sub





End Class
