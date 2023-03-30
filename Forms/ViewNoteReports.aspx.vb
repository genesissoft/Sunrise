Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class Forms_ViewNoteReports
    Inherits System.Web.UI.Page
    Dim sqlconn As SqlConnection
    Dim objCommon As New clsCommonFuns
    Dim strRptName As String
    Dim strProcName As String
    Dim rptDoc As New ReportDocument

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If  sqlConn Is Nothing Then objCommon.OpenConn()
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.AddHeader("Cache-Control", "no-cache")
        Response.AddHeader("Cache-Control", "no-store")
        Try
            If Page.IsPostBack = False Then
               

                If Trim(Request.QueryString("ReportSelection") & "") = "IS" Or Trim(Request.QueryString("ReportSelection") & "") = "IN" Then
                    Wordexport()
                Else
                    GetReportName()
                    BindReport()
                End If
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
    Private Sub GetReportName()
        Try
            Select Case Trim(Request.QueryString("Note") & "")

                Case "DebitNoteEntry"
                    Me.Page.Title = "Debit Note Entry Report"

                    strRptName = "DebitNoteEntry.rpt"
                    strProcName = "MB_FILL_DebitNoteEntryRpt"

                Case "ArrDebitNoteEntry"
                    Me.Page.Title = "Arranger Debit Note Entry Report"

                    strRptName = "ArrabgerDebitEntry.rpt"
                    strProcName = "MB_FILL_ArrangerDebitRpt"
                Case "CreditNoteEntry"

                    Me.Page.Title = "Credit Note Entry Report"

                    strRptName = "CreditNoteEntry.rpt"
                    strProcName = "MB_FILL_CreditNoteEntryRpt"

                Case "ChanepatCreditNoteEntry"

                    Me.Page.Title = "Credit Note Entry Report"

                    strRptName = "CPCreditNoteEntry.rpt"
                    strProcName = "MB_FILL_CreditNoteEntryRpt"

                Case "ApplicationEntryIssuer"

                    Me.Page.Title = "Deal Confirmation Issuer"

                    strRptName = "DealConfirmationIssuer.rpt"
                    strProcName = "MB_FILL_DealConfirmationIssuer"
                Case "ApplicationEntryInvestor"

                    Me.Page.Title = "Deal Confirmation Investor"

                    strRptName = "DealConfirmationInvestor.rpt"
                    strProcName = "MB_FILL_DealConfirmationInvestor"

                Case "GenerateIssueDealSlip"

                    Me.Page.Title = "Deal Slip No For Private Placement Of Issue"

                    strRptName = "DealSlipPrivateIssue.rpt"
                    strProcName = "MB_FILL_DealslipnoforprivateissueRpt"
                Case "IssueInvestorLetter"

                    If Trim(Request.QueryString("ReportSelection") & "") = "IS" Then
                        Me.Page.Title = "Issuer Letter"
                        strRptName = "ThanksLetterIssuer.rpt"
                        strProcName = "MB_FILL_IssuerLetterRpt"
                    ElseIf Trim(Request.QueryString("ReportSelection") & "") = "IN" Then
                        Me.Page.Title = "Investor Letter"
                        'strRptName = "ThnxLetterInvestor.rpt"
                        strRptName = "ThanksLetterInvestor.rpt"
                        strProcName = "MB_FILL_InvestorLetterRpt"
                    End If

                Case "IssueCheckList"
                    Me.Page.Title = "IssueCheckList"
                    strRptName = "IssueCheckList.rpt"
                    strProcName = "MB_FILL_IssueListNormalRpt"

            End Select
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function BindReport(Optional ByVal blnSepPages As Boolean = False) As ReportDocument
        Try
            Dim strPath As String
            Dim dtRpt As DataTable
            strPath = ConfigurationManager.AppSettings("ReportsPath").ToString & strRptName
            rptDoc.Load(strPath)
            dtRpt = GetReportTable()
            rptDoc.SetDataSource(dtRpt)
            'strReportRange = " From " & Trim(Request.QueryString("FromDate") & "") & " To " & Trim(Request.QueryString("ToDate") & "") & ""
            'rptDoc.SummaryInfo.ReportComments = strReportRange
            rptDoc.RecordSelectionFormula = strRecordSelection
            strRecordSelection = ""
            rptDoc = objCommon.GetReport("", rptDoc)
            rptDoc.VerifyDatabase()
            rptDoc.Refresh()
            CrystalReportViewer1.SeparatePages = False
            CrystalReportViewer1.ReportSource = rptDoc
            CrystalReportViewer1.DataBind()
            CrystalReportViewer1.RefreshReport()
            Return rptDoc
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            'rptDoc.Dispose()
        End Try
    End Function

    Private Function GetReportTable() As DataTable
        Try
            Dim sqlcomm As New SqlCommand
            Dim sqlda As New SqlDataAdapter
            Dim dtfill As New DataTable
            OpenConn()
            sqlcomm.Connection = sqlconn
            With sqlcomm
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = "300"
                .CommandText = strProcName
                .Parameters.Clear()
                If Trim(Request.QueryString("Note") & "") = "DebitNoteEntry" Or Trim(Request.QueryString("Note") & "") = "ArrDebitNoteEntry" Then
                    objCommon.SetCommandParameters(sqlcomm, "@DebitNoteId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("DebitNoteId") & ""))
                ElseIf Trim(Request.QueryString("Note") & "") = "CreditNoteEntry" Then
                    objCommon.SetCommandParameters(sqlcomm, "@CreditNoteId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("CreditNoteId") & ""))
                ElseIf Trim(Request.QueryString("Note") & "") = "ChanepatCreditNoteEntry" Then
                    objCommon.SetCommandParameters(sqlcomm, "@CreditNoteId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("CreditNoteId") & ""))

                ElseIf Trim(Request.QueryString("Note") & "") = "GenerateIssueDealSlip" Then
                    objCommon.SetCommandParameters(sqlcomm, "@IssueDealSlipId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("IssueDealSlipId") & ""))
                ElseIf Trim(Request.QueryString("Note") & "") = "ApplicationEntryIssuer" Then
                    objCommon.SetCommandParameters(sqlcomm, "@AppEntryId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("AppEntryId") & ""))
                ElseIf Trim(Request.QueryString("Note") & "") = "ApplicationEntryInvestor" Then
                    objCommon.SetCommandParameters(sqlcomm, "@AppEntryId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("AppEntryId") & ""))
                ElseIf Trim(Request.QueryString("Note") & "") = "IssueInvestorLetter" Then

                    If Trim(Request.QueryString("ReportSelection") & "") = "IS" Then
                        If Val(Request.QueryString("IssueId") & "") <> 0 Then
                            objCommon.SetCommandParameters(sqlcomm, "@IssueId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("IssueId") & ""))
                        End If
                        If Val(Request.QueryString("IssuerContactId") & "") <> 0 Then
                            objCommon.SetCommandParameters(sqlcomm, "@IssuerContactId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("IssuerContactId") & ""))
                        End If
                    ElseIf Trim(Request.QueryString("ReportSelection") & "") = "IN" Then
                        If Val(Request.QueryString("AppEntryId") & "") <> 0 Then
                            objCommon.SetCommandParameters(sqlcomm, "@AppEntryId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("AppEntryId") & ""))
                        End If
                        If Val(Request.QueryString("InvSchemeId") & "") <> 0 Then
                            objCommon.SetCommandParameters(sqlcomm, "@InvSchemeId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("InvSchemeId") & ""))
                        End If
                        If Val(Request.QueryString("InvestorId") & "") <> 0 Then
                            objCommon.SetCommandParameters(sqlcomm, "@InvestorId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("InvestorId") & ""))
                        End If
                        If Val(Request.QueryString("InvContactId") & "") <> 0 Then
                            objCommon.SetCommandParameters(sqlcomm, "@InvContactId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("InvContactId") & ""))
                        End If
                    End If

                ElseIf Trim(Request.QueryString("Note") & "") = "IssueCheckList" Then
                    objCommon.SetCommandParameters(sqlcomm, "@ListIssueNormalId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("ListIssueNormalId") & ""))

                End If

                .ExecuteNonQuery()
                sqlda.SelectCommand = sqlcomm
                sqlda.Fill(dtfill)
            End With
            Return dtfill
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Function

    Private Sub Wordexport()
        Try
            Dim rptDoc As ReportDocument
            GetReportName()
            rptDoc = BindReport()
            ExportReport(rptDoc)
            rptDoc = Nothing
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            'rptDoc.Dispose()
        End Try
    End Sub
    Private Sub ExportReport(ByVal crReport As ReportDocument)
        'declare a memorystream object that will hold out output 
        Dim oStream As MemoryStream
        Response.Clear()
        Response.ClearHeaders()
        Response.Buffer = True
        crReport.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report")
        Try
            Response.Flush()
            Response.End()
        Catch ex As Exception
        Finally
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            If rptDoc IsNot Nothing Then
                rptDoc.Close()
                rptDoc.Dispose()
                GC.Collect()
                CloseConn()
                sqlconn.Dispose()
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
