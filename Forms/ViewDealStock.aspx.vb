Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports RKLib.ExportData
Partial Class Forms_ViewDealStock
    Inherits System.Web.UI.Page
    Dim sqlComm As New SqlCommand
    Dim objCommon As New clsCommonFuns
    Dim dsmenu As DataSet
    Dim dsDPDetails As DataSet
    Dim newTable As DataTable
    Dim DpDetailsTable As DataTable
    Dim trmenu As DataRow
    Dim sqlConn As SqlConnection
    Dim rptDoc As New ReportDocument
    Dim strFilePath As String
    Dim dtRpt As DataTable
    Dim objMIS As New MISReports

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            'objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            If IsPostBack = False Then
                SetAttributes()
                FillDailyMarketValueGrid("DealDate DESC", 0)

            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub OpenConn()
        If sqlConn Is Nothing Then
            sqlConn = New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
            sqlConn.Open()
        ElseIf sqlConn.State = ConnectionState.Closed Then
            sqlConn.ConnectionString = ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString()
            sqlConn.Open()
        End If
    End Sub

    Private Sub CloseConn()
        If sqlConn Is Nothing Then Exit Sub
        If sqlConn.State = ConnectionState.Open Then sqlConn.Close()
    End Sub

    Private Sub SetAttributes()
        Try
            OpenConn()
            btn_Show.Attributes.Add("onclick", "return Validation();")
            txt_ForDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
            txt_ForDate.Attributes.Add("onkeypress", "OnlyDate();")
            txt_ForDate.Attributes.Add("onblur", "CheckDate(this,false);")
            txt_Interest.Attributes.Add("onkeypress", "OnlyDecimal();")
            'Srh_security.Columns.Add("SecurityName")
            'Srh_security.Columns.Add("SecurityId")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub FillDailyMarketValueGrid(ByVal strSort As String, Optional ByVal intPageIndex As Int16 = 0)
        Try
            Dim sqlda As New SqlDataAdapter
            Dim sqldt As New DataTable
            Dim sqldv As New DataView
            OpenConn()
            sqlComm.Connection = sqlConn
            sqlComm.CommandTimeout = "100"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_FILL_ViewDealStockDetails"
            sqlComm.Parameters.Clear()

            objCommon.SetCommandParameters(sqlComm, "@ForDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ForDate.Text))
            objCommon.SetCommandParameters(sqlComm, "@SecurityName", SqlDbType.VarChar, 500, "I", , , Trim(Srh_security.SearchTextBox.Text))
            objCommon.SetCommandParameters(sqlComm, "@Compid", SqlDbType.Int, 4, "I", , , Val(Session("Compid")))
            'objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
            objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId")))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(sqldt)
            sqldv = sqldt.DefaultView
            sqldv.Sort = strSort
            dg_dme.DataSource = sqldv
            dg_dme.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub




    Protected Sub dg_dme_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_dme.ItemDataBound
        Try
            Dim IntSecurityId As Integer

            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                Dim img As ImageButton
                img = CType(e.Item.FindControl("img_Select"), ImageButton)
                IntSecurityId = Val(TryCast(e.Item.DataItem, DataRowView).Row.Item("SecurityId").ToString)
                img.Attributes.Add("onclick", "SelectOption(this,'" & IntSecurityId & "');")
                e.Item.ToolTip = Trim(TryCast(e.Item.DataItem, DataRowView).Row.Item("StockStatus") & "")
            End If
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub btn_ShowSecurity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ShowSecurity.Click
        Try
            FillDailyMarketValueGrid("SecurityId DESC", 0)

        Catch ex As Exception
        End Try
    End Sub
    Protected Sub btn_ShowSecurityall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ShowSecurityall.Click
        Try
            Response.Redirect("viewdealstock.aspx")
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Try
            OpenConn()
            'ExportTradingStock()
            Hid_ReportType.Value = "ViewDealStockTrad"

            dtRpt = GetReportTableProp("ID_FILL_ViewDealStockDetailsExp")

            If (dtRpt.Rows.Count = 0) Then
                Dim msg As String = "No Entries Done"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoEntriesDone();", True)
                Exit Sub
            Else
                objMIS.ExportToExcel_ViewDealStock_SecurityWise(dtRpt, txt_ForDate.Text)
            End If

            'rptDoc = BindReportS(, "SecurityWise_DealStock.rpt", "ID_FILL_ViewDealStockDetailsExp")
            'If rptDoc Is Nothing Then Exit Sub
            'ExportReport(rptDoc, ExportFormatType.ExcelRecord, True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub ExportTradingStock()

        Dim tw As New System.IO.StringWriter()
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        Dim dgGrid As New DataGrid()
        Dim strHead As String
        Dim dt As DataTable

        dgGrid.ShowFooter = True
        dgGrid.DataSource = GetReportTableProp("ID_FILL_ViewDealStockDetailsExp") ' getData()

        dt = dgGrid.DataSource

        If dt.Rows.Count > 0 Then
            hw.WriteLine("<b><u><font size='2' align='center'></font></u></b><br>")
            hw.WriteLine("<b><u><font size='5' align='center'>" & strHead & "</font></u></b><br>")
            hw.WriteLine("<br>")

            dgGrid.HeaderStyle.Font.Bold = True
            dgGrid.GridLines = GridLines.Both
            dgGrid.ItemStyle.HorizontalAlign = HorizontalAlign.Center
            dgGrid.ItemStyle.Font.Name = "Cambria"
            dgGrid.ItemStyle.Font.Size = 10
            dgGrid.FooterStyle.Font.Bold = True
            dgGrid.DataBind()
            dgGrid.RenderControl(hw)
            Response.ClearHeaders()
            Response.AddHeader("content-disposition", "attachment;filename=ViewDealStock.xls")
            Response.ContentType = "application/vnd.ms-excel"
            Me.EnableViewState = False
            Response.Write(tw.ToString())
            Response.End()
        Else
            Dim strHtml As String
            strHtml = "alert('Sorry!!! No Records available to show report');"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
        End If

    End Sub


    Private Function GetReportTable(ByVal strProcName As String) As DataTable
        Try
            If sqlConn.State = ConnectionState.Closed Then sqlConn.Open()
            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand
            OpenConn()
            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = strProcName
                If strProcName = "ID_RPT_SegmentRpt" Then
                    objCommon.SetCommandParameters(sqlComm, "@ForDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ForDate.Text))
                    objCommon.SetCommandParameters(sqlComm, "@Compid", SqlDbType.Int, 4, "I", , , Val(Session("Compid")))
                Else
                    objCommon.SetCommandParameters(sqlComm, "@ForDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ForDate.Text))
                    objCommon.SetCommandParameters(sqlComm, "@SecurityName", SqlDbType.VarChar, 500, "I", , , Trim(Srh_security.SearchTextBox.Text))
                    objCommon.SetCommandParameters(sqlComm, "@Compid", SqlDbType.Int, 4, "I", , , Val(Session("Compid")))
                    objCommon.SetCommandParameters(sqlComm, "@Interest", SqlDbType.Decimal, 9, "I", , , Val(txt_Interest.Text))
                    'objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                    objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
                End If

                .ExecuteNonQuery()
            End With
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dtfill)
            Return dtfill
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Function
    Private Function GetReportTableProp(ByVal strProcName As String) As DataTable
        Try
            If sqlConn.State = ConnectionState.Closed Then sqlConn.Open()
            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand
            OpenConn()
            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = strProcName
                objCommon.SetCommandParameters(sqlComm, "@ForDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ForDate.Text))
                objCommon.SetCommandParameters(sqlComm, "@SecurityName", SqlDbType.VarChar, 500, "I", , , Trim(Srh_security.SearchTextBox.Text))
                objCommon.SetCommandParameters(sqlComm, "@Compid", SqlDbType.Int, 4, "I", , , Val(Session("Compid")))
                objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId")))
                objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
                objCommon.SetCommandParameters(sqlComm, "@Interest", SqlDbType.Decimal, 9, "I", , , Val(txt_Interest.Text))
                objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
                .ExecuteNonQuery()
            End With
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dtfill)
            Return dtfill
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Function



    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            sqlConn.Dispose()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub



    Protected Sub btn_ViewTrad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ViewTrad.Click
        Try
            Hid_ReportType.Value = "ViewDealStockTrad"
            Response.Redirect("ViewReports.aspx?Fordate=" & txt_ForDate.Text & "&Interest=" & txt_Interest.Text & "&Rpt=" & Hid_ReportType.Value & "&SecurityName=" & Srh_security.SearchTextBox.Text, False)

        Catch ex As Exception

        End Try
    End Sub


    Private Function BindReportS(Optional ByVal blnSepPages As Boolean = False,
                               Optional ByVal strRptName As String = "",
                               Optional ByVal strProcName As String = "") As ReportDocument
        Try
            Dim strPath As String

            Dim strReportRange As String
            Dim datFrom As Date
            Dim datTo As Date

            'datFrom = Today
            'datTo = Today
            strPath = ConfigurationSettings.AppSettings("ReportsPath") & "\" & strRptName
            rptDoc.Load(strPath)

            dtRpt = GetReportTableProp("ID_FILL_ViewDealStockDetailsExp")

            If (dtRpt.Rows.Count = 0) Then
                Dim msg As String = "No Entries Done"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
                Exit Function
            Else

                rptDoc.SetDataSource(dtRpt)

                'strReportRange = txt_FromDate.Text & "  TO  " & txt_ToDate.Text & ""
                rptDoc.SummaryInfo.ReportComments = strReportRange
                ' rptDoc.DataDefinition.FormulaFields("Period").Text = _
                '"""" & "From " & datFrom.ToString("dd-MMMM-yyyy") & " To " & datTo.ToString("dd-MMMM-yyyy") & """"



            End If
            Return rptDoc
        Catch ex As Exception
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function


    Private Sub ExportReport(ByVal crReport As ReportDocument, ByVal type As ExportFormatType,
                            Optional ByVal blnDiskExport As Boolean = False)
        Response.Clear()
        Response.ClearHeaders()
        Response.Buffer = True
        If blnDiskExport = True Then

            'If cbo_WDMTransRepts.SelectedValue = "E" Then
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
            .AddHeader("content-disposition", "attachment;filename=ViewDealStock.xls")
            .WriteFile(strFilePath)
            .Flush()
            .End()
        End With
        clsCommonFuns.sqlConn.Close()
    End Sub



    Protected Sub btn_Segment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Segment.Click
        Try
            OpenConn()
            dtRpt = GetReportTable("ID_RPT_SegmentRpt")
            'objMIS.ExportToExcel_SegmentReport(dtRpt, Trim(txt_ForDate.Text))
        Catch ex As Exception
        Finally
            CloseConn()
        End Try
    End Sub
End Class
