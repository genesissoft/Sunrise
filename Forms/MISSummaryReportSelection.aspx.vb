Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports RKLib.ExportData
Imports CrystalDecisions.Web
Imports GemBox.Spreadsheet
Imports GemBox.Spreadsheet.ExcelFile
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Web.HttpApplicationState
Imports System.Web
Imports Excel
Imports System.Data.OleDb.OleDbConnection
Imports log4net
Imports ClosedXML.Excel
Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Spreadsheet
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Presentation
Imports iTextSharp.text.pdf

Partial Class Forms_MISSummaryReportSelection
    Inherits System.Web.UI.Page
    Dim PgName As String = "$MISSummaryReportSelection$"
    Dim objUtil As New Util
    Dim sqlconn As SqlConnection
    Dim objCommon As New clsCommonFuns
    Dim oXL As Excel.Application
    Dim rptDoc As New ReportDocument
    Dim rptDoc1 As New ReportDocument
    Dim oSheet As Excel.Worksheet
    Dim strFilePath As String
    Dim dtRpt As System.Data.DataTable
    Dim xlApp As Excel.Application
    Dim dtPivot As System.Data.DataTable
    Dim TaskPivotTable As System.Data.DataTable
    Dim objExcel As Excel.Application
    Dim ScriptOpenModalDialog As String = "javascript:OpenModalDialog('{0}','{1}','{2}');"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            If IsPostBack = False Then
                Hid_ReportType.Value = Trim(Request.QueryString("Rpt") & "")
                SetAttributes()
                If Hid_ReportType.Value = "TMISSumm" Then
                    row_MISDetail.Visible = True
                    row_MISDetail1.Visible = False
                    row_FromDate.Visible = True
                    row_ToDate.Visible = False
                    row_SummType.Visible = True
                Else
                    row_MISDetail.Visible = True
                    row_MISDetail1.Visible = False
                    row_FromDate.Visible = False
                    row_ToDate.Visible = False
                End If
            End If

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

    Public Sub SetAttributes()
        OpenConn()
        txt_FromDate.Text = Microsoft.VisualBasic.Strings.Format(DateAndTime.Today, "dd/MM/yyyy")
        txt_FromDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_FromDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_ToDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_ToDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_ToDate.Text = Microsoft.VisualBasic.Strings.Format(DateAdd(DateInterval.Day, 1, Today), "dd/MM/yyyy")


        btn_Print.Attributes.Add("onclick", "return  Validation();")
        objCommon.FillControl(cbo_Company, sqlconn, "ID_FILL_CompanyMaster1", "CompName", "CompId")
        Dim lstItemCO As ListItem
        lstItemCO = cbo_Company.Items.FindByText("WELSPUN GROUP")
        If lstItemCO IsNot Nothing Then
            cbo_Company.Items.Remove(lstItemCO)
        End If
        CloseConn()
    End Sub

    Protected Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Try
            'OpenConn()
            'If Hid_ReportType.Value = "TMISSumm" Then
            pivot()
            'ExportTMISCategorySumm()
            'End If

        Catch ex As Exception
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub BindReport(ByVal strRptName As String, ByVal strProcName As String)
        Try
            Dim strPath As String
            Dim dtRpt As System.Data.DataTable


            rptDoc = New ReportDocument
            strPath = ConfigurationManager.AppSettings("ReportsPath") & "\" & strRptName & ".rpt"
            rptDoc.Load(strPath)


            If Hid_ReportType.Value = "TMISSumm" Then
                dtRpt = GetReportTable("ID_FILL_TMISSummary")
                rptDoc.SetDataSource(dtRpt)
            End If
            'dtRpt = GetReportTable("ID_FILL_RetailMIS")


            rptDoc.ExportToDisk(ExportFormatType.ExcelRecord, Server.MapPath("").Replace("Forms", "Temp") & "\" & strRptName & ".xls")
        Catch ex As Exception
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            rptDoc.Close()
            rptDoc.Dispose()
            GC.Collect()
        End Try
    End Sub

    Private Function BindReportS(Optional ByVal blnSepPages As Boolean = False, _
                                Optional ByVal strRptName As String = "", _
                                Optional ByVal strProcName As String = "") As ReportDocument

        Try
            Dim strPath As String = ""
            Dim strPath1 As String = ""
            'Dim subreportName As String
            'Dim subreportObject As SubreportObject
            Dim subreport As New ReportDocument()


            strPath = ConfigurationSettings.AppSettings("ReportsPath") & "\" & strRptName


            rptDoc.Load(strPath)

            If Hid_ReportType.Value = "TMISSumm" Then
                dtRpt = GetReportTable("ID_FILL_TMISSummary")

            End If

            If (dtRpt.Rows.Count = 0) Then
                Dim msg As String = "No Entries Done"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
                Exit Function
            Else

                rptDoc.SetDataSource(dtRpt)

            End If
            Return rptDoc
        Catch ex As Exception
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Public Sub ExportTMISCategorySumm()
        Dim strFileName As String
        'The full path where the excel file will be stored
        'Dim strFileName As String = AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\")
        strFileName = (Server.MapPath("").Replace("Forms", "Temp") & "\Report.xls")
        'strFileName = strFileName & "\MyExcelFile" & System.DateTime.Now.Ticks.ToString() ".xls"


        Dim objBooks As Excel.Workbooks, objBook As Excel.Workbook
        Dim objSheets As Excel.Sheets
        ', objSheet As Excel.Worksheet oSheet
        Dim objRange As Excel.Range
        'Dim objRange1 As Excel.Range

        Try
            'Creating a new object of the Excel application object
            Dim objExcel = New Excel.Application
            'Hiding the Excel application
            objExcel.Visible = False
            'Hiding all the alert messages occurring during the process
            objExcel.DisplayAlerts = False

            'Adding a collection of Workbooks to the Excel object
            objBook = CType(objExcel.Workbooks.Add(), Excel.Workbook)


            If File.Exists(Server.MapPath("").Replace("Forms", "Temp") & "\Report.xls") Then
                File.Delete(Server.MapPath("").Replace("Forms", "Temp") & "\Report.xls")
            End If


            'Saving the Workbook as a normal workbook format.
            objBook.SaveAs(strFileName, Excel.XlFileFormat.xlWorkbookNormal)

            'Getting the collection of workbooks in an object
            objBooks = objExcel.Workbooks

            'Get the reference to the first sheet in the workbook collection in a variable

            oSheet = CType(objBooks(1).Sheets.Item(1), Excel.Worksheet)
            'oSheet.Name = "Category Summary"
            'objRange = oSheet.Cells
            'SaveReports("CategorySummary", "ID_FILL_TMISSummary", "TMIS Summary")

            'objExcel.Workbooks.Open(Server.MapPath("").Replace("Forms", "Temp") & "\" & "CategorySummary" & ".xls")
            'objExcel.ActiveSheet.Cells.Select()

            'objExcel.Selection.Copy()
            ''FormatBondSummary()
            'objExcel.Windows("Report.xls").Activate()
            'objExcel.ActiveSheet.Paste()
            'oSheet.SaveAs(strFileName)
            ''CategoryWise

            'oSheet = CType(objBook.Sheets.Add, Excel.Worksheet)
            oSheet.Name = "Rating Summary Detail"
            objRange = oSheet.Cells

            'Pivot code started

            Dim Conn As SqlConnection
            Dim datFrom As Date
            Dim datTo As Date
            Dim sqlComm As New SqlCommand
            Dim SB As New StringBuilder
            Dim SB1 As New StringBuilder

            datFrom = objCommon.DateFormat(txt_FromDate.Text)
            datTo = objCommon.DateFormat(txt_ToDate.Text)

            'OpenConn()

            'Query String

            SB.AppendLine("Select CompName As Company, isnull([dbo].[ID_GET_SecurityRating](d.SecurityId),'NA') as Rating , isnull(SecurityCategory,'Others') as Category,Securityname ")
            'SB.AppendLine("[dbo].[ID_GET_CateSummary]" & "(CONVERT(SMALLDATETIME," & "'" & datFrom & "'" & ",103)" & ",d.compid) As Holding")
            'SB.AppendLine("[dbo].[ID_GET_CateSummary]" & "(CONVERT(SMALLDATETIME," & "'" & datFrom & "'" & ",103)" & ",d.compid)")
            'SB.AppendLine("/SUM(CONVERT(DECIMAL(18,2),(SettlementAmt/((d.FaceValue*d.FaceValueMultiple)))*((d.facevalue*d.facevaluemultiple )  -")
            'SB.AppendLine("[dbo].[ID_GET_DealStockProfit](d.DealSlipID,CONVERT(SMALLDATETIME," & "'" & datFrom & "'" & ",103) " & ")))) As Holding")
            SB.AppendLine(",SUM(CONVERT(DECIMAL(18,2),(SettlementAmt/((d.FaceValue*d.FaceValueMultiple)))*((d.facevalue*d.facevaluemultiple )  -")
            SB.AppendLine("[dbo].[ID_GET_DealStockProfit](d.DealSlipID,CONVERT(SMALLDATETIME," & "'" & datFrom & "'" & ",103) " & "))))/10000000  AS [Amount(In Cr.)]")
            SB.AppendLine("from dealslipentry d inner join securitymaster s on s.securityid =d.securityid")
            SB.AppendLine("LEFT JOIN SecurityCategorymaster SC ON SC.SecurityCatId =S.SecurityCatId")
            SB.AppendLine("inner join companymaster co on co.compid =d.compid")
            SB.AppendLine("WHERE   transtype='P'")
            SB.AppendLine("and dealtranstype<>'B' and dealdone=1 and canceldeal = 0")
            SB.AppendLine("and ((d.facevalue * d.facevaluemultiple )  - [dbo].[ID_GET_DealStockProfit](d.DealSlipID,CONVERT(SMALLDATETIME," & "'" & datFrom & "'" & ",103) " & " )) > 0")
            SB.AppendLine("AND DealDate <= CONVERT(SMALLDATETIME," & "'" & datFrom & "'" & ",103)")
            If Val(cbo_Company.SelectedValue) <> 0 Then
                SB.AppendLine("AND d.CompId =" & cbo_Company.SelectedValue)
            End If

            SB.AppendLine("GROUP BY compname,d.SecurityId,SecurityName")
            SB.AppendLine(",NSDLFaceValue,NSDLAcNumber,SecurityCategory,IPDates,[dbo].[ID_GET_SecurityRating](d.SecurityId),d.compid")
            SB.ToString()

            datFrom = objCommon.DateFormat(txt_FromDate.Text)
            datTo = objCommon.DateFormat(txt_ToDate.Text)
            Conn = New SqlConnection
            objExcel.ActiveSheet.Cells(1, 1) = "Category Summary"
            objExcel.ActiveSheet.Cells(1, 1).ColumnWidth = 5
            'Dim xlRange As Excel.Range = CType(ExcelWorksheet, Excel.Worksheet).Range("A4")

            Dim ptCache As Excel.PivotCache = objBook.PivotCaches.Add( _
           SourceType:=Excel.XlPivotTableSourceType.xlExternal)
         
            With ptCache
                '.Connection = "OLEDB;Provider=SQLOLEDB.1;User ID=sa; password=genesis;Data Source=SERVER\SQLEXPRESS;Initial Catalog=eInstadeal_Suvarna;User ID=sa; Password=genesis;Trusted_Connection=no"
                '.Connection = "OLEDB;Provider=SQLOLEDB.1;User ID=sa; password=genesis;Data Source=SERVER\SQLEXPRESS;Initial Catalog=eInstadeal_Suvarna;User ID=sa; Password=genesis;Trusted_Connection=no;Persist Security Info=True;Integrated Security =SSPI"
                .Connection = "OLEDB;Provider=SQLOLEDB.1;User ID=sa; password=pass@123$;Data Source=MUMWTREDB01;Initial Catalog=eInstadeal_Welspun;User ID=sa; Password=pass@123$;Trusted_Connection=no;Persist Security Info=True;Integrated Security =SSPI"
                .CommandText = SB.ToString()
                .CommandType = Excel.XlCmdType.xlCmdSql
                
            End With
            Dim pivotTables As Excel.PivotTables = CType(oSheet.PivotTables(Type.Missing), Excel.PivotTables)
            Dim ptTable As Excel.PivotTable = pivotTables.Add(ptCache, objRange, "PT_Report", Type.Missing, Type.Missing)
            'Dim fieldType As Excel.
            With ptTable
                .ManualUpdate = True
                If rdo_SummType.SelectedValue = "R" Then
                    .PivotFields("Company").Orientation = Excel.XlPivotFieldOrientation.xlColumnField
                    .PivotFields("Rating").Orientation = Excel.XlPivotFieldOrientation.xlRowField
                    .PivotFields("Category").Orientation = Excel.XlPivotFieldOrientation.xlRowField
                Else
                    .PivotFields("Company").Orientation = Excel.XlPivotFieldOrientation.xlColumnField
                    .PivotFields("Category").Orientation = Excel.XlPivotFieldOrientation.xlRowField
                End If
                .PivotFields("SecurityName").Orientation = Excel.XlPivotFieldOrientation.xlRowField
                .PivotFields("Amount(In Cr.)").Orientation = Excel.XlPivotFieldOrientation.xlDataField
                .Format(Excel.XlPivotFormatType.xlReport4)
                .ManualUpdate = False
            End With

            Dim PF As Excel.PivotField
            Dim PI As Excel.PivotItem
            For Each ptTable In oSheet.PivotTables
                For Each PF In ptTable.PivotFields
                    For Each PI In PF.PivotItems
                        PI.ShowDetail = False
                    Next PI
                Next PF
            Next ptTable
            'Pivot code ended
            objExcel.ActiveSheet.Cells.Select()

            'objExcel.Selection.Copy()
            'objExcel.Windows("Report.xls").Activate()
            'objExcel.ActiveSheet.Paste()
            'oSheet.SaveAs(strFileName)
            'objExcel.Workbooks.Open(Server.MapPath("").Replace("Forms", "Temp") & "\" & "TMISSumm" & ".xls")
            'objExcel.ActiveSheet.Cells.Select()
            'objExcel.Selection.Copy()
            'objExcel.Windows("TMISSumm.xls").Activate()
            'objExcel.ActiveSheet.Paste()
            'oSheet.SaveAs(strFileName)
            'objSheets = objBook.Worksheets
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
            'System.Runtime.InteropServices.Marshal.ReleaseComObject(objSheets)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objExcel)
            objBook = Nothing
            objSheets = Nothing

            objExcel = Nothing
            ptTable = Nothing
            ptCache = Nothing
            'closeApplication()
            GC.Collect()


            With HttpContext.Current.Response
                .Clear()
                .Charset = ""
                .ClearHeaders()
                .ContentType = "application/vnd.ms-excel"
                .AddHeader("content-disposition", "attachment;filename=" + cbo_Company.SelectedItem.Text + " " + "MIS Details.xls")
                .WriteFile(strFileName)
                .Flush()
                .End()
            End With

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "ExportTMISCategorySumm", "Error in ExportTMISCategorySumm", "", ex)
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
            CloseConn()
        End Try
    End Sub
    Private Sub SaveReports(ByVal strRptName As String, ByVal strProcName As String, ByVal strSheetName As String)
        Try
            BindReport(strRptName, strProcName)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function GetReportTable(ByVal strProcName As String) As System.Data.DataTable
        Try
            Dim datFrom As Date
            Dim datTo As Date
            Dim dtfill As New System.Data.DataTable
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
                If Val(cbo_Company.SelectedValue) <> 0 Then
                    objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(cbo_Company.SelectedValue))
                End If

                objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
                objCommon.SetCommandParameters(sqlComm, "@ForDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)



                .ExecuteNonQuery()
            End With
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dtfill)
            Session("BondPosition") = dtfill
            Return dtfill
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally

        End Try
    End Function
    Private Sub ExportReport(ByVal crReport As ReportDocument, ByVal type As ExportFormatType, _
                            Optional ByVal blnDiskExport As Boolean = False)
        Response.Clear()
        Response.ClearHeaders()
        Response.Buffer = True

        If blnDiskExport = True Then
            strFilePath = Server.MapPath("").Replace("Forms", "Temp") & "\Report.xls"
            If File.Exists(strFilePath) = True Then
                File.Delete(strFilePath)
            End If
            crReport.ExportToDisk(type, strFilePath)
        Else
            crReport.ExportToHttpResponse(type, Response, True, "Report")
        End If

        With HttpContext.Current.Response
            .Clear()
            .Charset = ""
            .ClearHeaders()
            .ContentType = "application/vnd.ms-excel"
            '.AddHeader("content-disposition", "attachment;filename=Report.xls")
            .AddHeader("content-disposition", "attachment;filename=" + Trim(cbo_Company.SelectedItem.Text) + "-" + Trim(rbl_SecurityType.SelectedItem.Text) + ".xls")
            .WriteFile(strFilePath)
            .Flush()
            .End()
        End With
        'clsCommonFuns.sqlConn.Close()
    End Sub

    Public Sub closeApplication()
        'Dim Processes As System.Diagnostics.Process()
        'Processes = System.Diagnostics.Process.GetProcessesByName("EXCEL")
        'For Each p As System.Diagnostics.Process In Processes
        '    If p.MainWindowTitle.Trim() = "" Then
        '        p.Kill()
        '    End If
        'Next
    End Sub

    Public Sub pivot()

        OpenConn()
        'Dim pastries = New Generic.List(Of Pastry)() {New Pastry("abcd", 100, "2")}
        Dim pastries() As Object = {"Hello World", 12D, 16UI, "A"c}
        Dim strFileName As String
        If File.Exists(Server.MapPath("").Replace("Forms", "Temp") & "\RetailSummary.xlsx") Then
            File.Delete(Server.MapPath("").Replace("Forms", "Temp") & "\RetailSummary.xlsx")
        End If

        strFileName = (Server.MapPath("").Replace("Forms", "Temp") & "\RetailSummary.xlsx")

        dtRpt = GetReportTable("ID_FILL_RetailSummary")
        Dim workbook = New XLWorkbook()
        Dim sheet = workbook.Worksheets.Add("RetailSummary")
        'Dim myRange As ClosedXML.Excel.IXLCell
        Dim source = sheet.Cell(1, 1).InsertTable(dtRpt, "RetailSummary", True)
        Dim range = source.DataRange
        Dim header = sheet.Range(1, 1, 1, 3)
        Dim dataRange = sheet.Range(header.FirstCell(), range.LastCell())
        Dim ptSheet = workbook.Worksheets.Add("PivotTable")


        Dim pt = ptSheet.PivotTables.AddNew("PivotTable", ptSheet.Cell(1, 1), dataRange)

        'pt.RowLabels.Add("CompName")
        'pt.ColumnLabels.Add("SecurityRating")
        'pt.ColumnLabels.Add("SecurityName")
        'pt.ColumnLabels.Add("Category")
        'pt.Values.Add("AmountInvested")

        pt.RowLabels.Add("CompName")
        pt.ColumnLabels.Add("CustomerName")
        pt.ColumnLabels.Add("SecurityRating")
        pt.ColumnLabels.Add("SecurityName")
        pt.Values.Add("AmountInvested")

        workbook.SaveAs(strFileName)
        CloseConn()
        With HttpContext.Current.Response
            .Buffer = True
            .Cache.SetCacheability(HttpCacheability.NoCache)
            .Clear()
            .Charset = ""
            .ClearHeaders()
            .ContentType = "application/vnd.ms-excel"
            .AddHeader("content-disposition", "attachment;filename=RetailSummary.xlsx")
            .WriteFile(strFileName)
            .Flush()
            .End()
        End With

    End Sub
End Class
