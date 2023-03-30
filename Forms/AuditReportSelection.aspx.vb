Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports RKLib.ExportData
Imports ClosedXML.Excel
Imports DocumentFormat.OpenXml.Spreadsheet
Imports DocumentFormat.OpenXml
Partial Class Forms_AuditReportSelection
    Inherits System.Web.UI.Page
    Dim sqlconn As SqlConnection
    Dim objCommon As New clsCommonFuns
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
            'txt_ToDate.Text = System.DateTime.Now().ToString("dd/MM/yyyy")
            'txt_FromDate.Text = System.DateTime.Now().AddDays(-1).ToString("dd/MM/yyyy")
            If IsPostBack = False Then
                setcontrols()
                txt_ToDate.Text = System.DateTime.Now().ToString("dd/MM/yyyy")
                txt_FromDate.Text = System.DateTime.Now().AddDays(-1).ToString("dd/MM/yyyy")

                txt_FromDate.Attributes.Add("onblur", "CheckDate(this,false);")
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "hid", "HidRow();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "test7773", "BindDate();", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage('Validation', '" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "', 175, 450);", True)
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
    Private Sub setcontrols()
        cbo_RetailWDM.Attributes.Add("onchange", "HidRow();")
        'Srh_Customer.Columns.Add("CustomerName")
        'Srh_Customer.Columns.Add("PanNumber")
        'Srh_Customer.Columns.Add("CustomerId")

        'Srh_DealSlipNo.Columns.Add("DealSlipNo")
        'Srh_DealSlipNo.Columns.Add("CONVERT(VARCHAR,DealDate,103) As DealDate")
        'Srh_DealSlipNo.Columns.Add("DealSlipId")

        'Srh_security.Columns.Add("SecurityName")
        'Srh_security.Columns.Add("NSDLACNumber As ISIN")
        'Srh_security.Columns.Add("SecurityId")

        'srh_TransCode.Columns.Add("WDMDealNumber")
        'srh_TransCode.Columns.Add("CONVERT(VARCHAR,DealDate,103) As DealDate")
        'srh_TransCode.Columns.Add("DealEntryId")
        'srh_WDMTransCode.Columns.Add("DealSlipNo")
        'srh_WDMTransCode.Columns.Add("CONVERT(VARCHAR,DealDate,103) As DealDate")
        'srh_WDMTransCode.Columns.Add("DealSlipId")
        btn_Print.Attributes.Add("onclick", "return Validation();")

    End Sub

    Protected Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Try
            Dim dtRpt As DataTable = New DataTable()
            Dim dtRptSecurity As DataTable = New DataTable()
            Dim dtRptCashFlow As DataTable = New DataTable()
            Dim dtRptSecInfo As DataTable = New DataTable()

            Hid_ReportType.Value = "RetailAudit"
            Dim AuditId As Integer
            If cbo_RetailWDM.SelectedValue = "C" And Srh_Customer.SelectedId <> "" Then
                AuditId = Srh_Customer.SelectedId
            ElseIf cbo_RetailWDM.SelectedValue = "S" And Srh_security.SelectedId <> "" Then
                AuditId = Srh_security.SelectedId
            ElseIf cbo_RetailWDM.SelectedValue = "D" And Srh_DealSlipNo.SelectedId <> "" Then
                AuditId = Srh_DealSlipNo.SelectedId
            ElseIf cbo_RetailWDM.SelectedValue = "W" Then
                AuditId = srh_TransCode.SelectedId
            ElseIf cbo_RetailWDM.SelectedValue = "T" Then
                AuditId = srh_WDMTransCode.SelectedId
            End If
            If cbo_RetailWDM.SelectedValue <> "S" Then
                dtRpt = GetReportTable("ID_FILL_AUDIT_Rpt")
            ElseIf cbo_RetailWDM.SelectedValue = "S" Then
                dtRptSecurity = GetReportTable("ID_FILL_AUDIT_Rpt")
                Session("MISSecurityReport") = dtRptSecurity
                dtRptCashFlow = GetReportTable("ID_RPT_SecurityCashFlowAudit")
                Session("MISCashFlowReport") = dtRptCashFlow
                dtRptSecInfo = GetReportTable("ID_RPT_SecurityInfoAudit")
                Session("MISDetailsReport") = dtRptSecInfo
            End If
            If cbo_RetailWDM.SelectedValue = "S" Then
                If dtRptSecurity.Rows.Count = 0 And dtRptCashFlow.Rows.Count = 0 And dtRptSecInfo.Rows.Count = 0 Then
                    Dim strHtml As String
                    strHtml = "AlertMessage('Validation', 'Sorry!!! No Records available to show report', 175, 450);"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "CallOnNoRecords();", True)
                Else
                    If dtRptSecurity IsNot Nothing And dtRptSecurity.Rows.Count > 0 Then
                        objMIS.ExportToExcel_MISSecurityAuditReport(dtRptSecurity, Trim(txt_FromDate.Text), Trim(txt_ToDate.Text), Trim(cbo_RetailWDM.SelectedItem.Text))
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "CallOnNoRecords();", True)
                    End If
                End If
            Else
                If dtRpt IsNot Nothing And dtRpt.Rows.Count > 0 Then
                    If cbo_RetailWDM.SelectedValue = "C" Then
                        objMIS.ExportToExcel_AuditReport(dtRpt, Trim(txt_FromDate.Text), Trim(txt_ToDate.Text), Trim(cbo_RetailWDM.SelectedItem.Text))
                    Else
                        objMIS.ExportToExcel_AuditReport(dtRpt, Trim(txt_FromDate.Text), Trim(txt_ToDate.Text), Trim(cbo_RetailWDM.SelectedItem.Text))
                    End If
                Else
                    Dim strHtml As String
                    strHtml = "AlertMessage('Validation', 'Sorry!!! No Records available to show report', 175, 450);"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "CallOnNoRecords();", True)
                End If
            End If
            'Response.Redirect("ViewReports.aspx?Rpt=" & Hid_ReportType.Value & "&AuditType=" & cbo_RetailWDM.SelectedValue & "&AuditId=" & AuditId, False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage('Validation','" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "', 175, 450);", True)
        End Try
    End Sub
    Public Sub ExportToExcel_MISDealDateReportNew(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String, ByVal FilterName As String)
        Try
            Dim strdtTime As String = DateTime.Now.ToString("h:mm:ss tt")
            strdtTime = strdtTime.Replace("PM", "")
            strdtTime = strdtTime.Replace("AM", "")
            Dim strFileName As String = FilterName & "_Audit Report_" & strdtTime
            Dim sheetName As String = FilterName & "_Audit Report"
            Dim ds As DataSet = New DataSet()

            If dt IsNot Nothing Then

                If dt.Rows.Count > 0 Then

                    Using wb As XLWorkbook = New XLWorkbook()
                        Dim ws As IXLWorksheet = wb.Worksheets.Add(dt, sheetName)
                        ws.Tables.Table(0).ShowAutoFilter = False
                        HttpContext.Current.Response.Clear()
                        HttpContext.Current.Response.Buffer = True
                        HttpContext.Current.Response.Charset = ""
                        HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" & strFileName & ".xlsx")
                        HttpContext.Current.Response.Charset = ""
                        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)


                        Using MyMemoryStream As MemoryStream = New MemoryStream()
                            wb.SaveAs(MyMemoryStream)
                            MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream)
                            HttpContext.Current.Response.Flush()
                            HttpContext.Current.Response.[End]()
                        End Using
                    End Using
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage('Validation', '" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "', 175, 450);", True)
        End Try
    End Sub

    Private Function GetReportTable(ByVal strProcName As String) As DataTable
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
                objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)
                objCommon.SetCommandParameters(sqlComm, "@ToDate", SqlDbType.SmallDateTime, 8, "I", , , datTo)
                objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("compid")))
                objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                objCommon.SetCommandParameters(sqlComm, "@AuditType", SqlDbType.Char, 1, "I", , , cbo_RetailWDM.SelectedValue)
                If cbo_RetailWDM.SelectedValue = "C" And Srh_Customer.SelectedId <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@Customerid", SqlDbType.Int, 4, "I", , , Val(Srh_Customer.SelectedId))
                End If
                If cbo_RetailWDM.SelectedValue = "S" And Srh_security.SelectedId <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@Securityid", SqlDbType.Int, 4, "I", , , Val(Srh_security.SelectedId))
                End If
                If cbo_RetailWDM.SelectedValue = "D" And Srh_DealSlipNo.SelectedId <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@Dealslipid", SqlDbType.Int, 4, "I", , , Val(Srh_DealSlipNo.SelectedId))
                End If
                If cbo_RetailWDM.SelectedValue = "W" And srh_TransCode.SelectedId <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@DealEntryId", SqlDbType.Int, 4, "I", , , Val(srh_TransCode.SelectedId))
                End If
                .ExecuteNonQuery()
            End With
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dtfill)
            Session("RptData") = dtfill
            Return dtfill
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage('Validation', '" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "', 175, 450);", True)
        Finally
            CloseConn()
        End Try
    End Function
    Protected Sub cbo_RetailWDM_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RetailWDM.SelectedIndexChanged
        Try
            If cbo_RetailWDM.SelectedValue = "C" Then
                Srh_security.SelectedId = ""
                Srh_security.SelectedFieldName = ""
                Srh_DealSlipNo.SelectedId = ""
                Srh_DealSlipNo.SelectedFieldName = ""
            ElseIf cbo_RetailWDM.SelectedValue = "S" Then
                Srh_Customer.SelectedId = ""
                Srh_Customer.SelectedFieldName = ""
                Srh_DealSlipNo.SelectedId = ""
                Srh_DealSlipNo.SelectedFieldName = ""
            ElseIf cbo_RetailWDM.SelectedValue = "D" Then
                Srh_security.SelectedId = ""
                Srh_security.SelectedFieldName = ""
                Srh_Customer.SelectedId = ""
                Srh_Customer.SelectedFieldName = ""
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "hid", "BindDate();", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage('Validation', '" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "', 175, 450);", True)
        End Try
    End Sub

    Protected Sub Srh_Customer_ButtonClick() Handles Srh_Customer.ButtonClick

    End Sub
End Class
