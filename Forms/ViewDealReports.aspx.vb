Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class Forms_ViewDealReports
    Inherits System.Web.UI.Page
    Dim sqlconn As SqlConnection
    Dim objCommon As New clsCommonFuns
    Dim strRptName As String
    Dim strProcName As String
    Dim rptDoc As New ReportDocument
    Dim WordPrint As String
    Dim strDealDate As String
    Dim strStampDutyDate As String = ConfigurationManager.AppSettings("StampDutyDateCR")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If (Session("username") = "") Then
        '    Response.Redirect("Login.aspx")
        '    Exit Sub
        'End If
        'objCommon.OpenConn()
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.AddHeader("Cache-Control", "no-cache")
        Response.AddHeader("Cache-Control", "no-store")

        WordPrint = Trim(Request.QueryString("WordPrintFlag") & "")
        Dim str = Trim(Request.QueryString("strPrintDet") & "")
        If Trim(Request.QueryString("Rpt") & "") = "NSDLTONSDL" Or Trim(Request.QueryString("Rpt") & "") = "NSDLTOCSDL" Then
            Hid_Intids.Value = Session("intids")
        End If
        Try
            If Page.IsPostBack = False Then
                If WordPrint = "PW" Or WordPrint = "PP" Or Trim(Request.QueryString("Rpt") & "") = "DealDetail" Or Trim(Request.QueryString("Rpt") & "") = "NSDLTONSDL" Or Trim(Request.QueryString("Rpt") & "") = "CSGL" Then
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
            If Trim(Request.QueryString("Rpt") & "") = "DealDetailReport" Or Trim(Request.QueryString("Rpt") & "") = "DealDetail" Then
                strRptName = "DealDetailNew.rpt"
                strProcName = "ID_Fill_DealDetailRptNew"
            End If
            If Trim(Request.QueryString("DealFlag") & "") = "NSDOM" Then
                strRptName = "NDS_OM.rpt"
                strProcName = "ID_FILL_NDS_OMRept"
            End If
            If Trim(Request.QueryString("DealFlag") & "") = "CSGL" Then
                strRptName = "CSGL.rpt"
                strProcName = "ID_FILL_NDS_OMRept"
            End If
            If Trim(Request.QueryString("DealFlag") & "") = "TCSAnnex" Then
                strRptName = "TCSAnnexure.rpt"
                strProcName = "ID_RPT_TCSAnnexureReport"
            End If

            If Trim(Request.QueryString("Rpt") & "") = "NSDLTONSDL" Then
                If Trim(Request.QueryString("DPName") & "").IndexOf("THE FEDERAL BANK LTD.") <> -1 Then
                    'strRptName = "FEDNSDLTONSDLRPT.rpt"
                    strRptName = "NSDLTONSDLRPT.rpt"
                Else
                    'strRptName = "FEDNSDLTONSDLRPT.rpt"
                    strRptName = "NSDLTONSDLRPT.rpt"
                End If
                strRptName = "NSDLTONSDLRPT.rpt"
                'strRptName = "FEDNSDLTONSDLRPT.rpt"
                strProcName = "ID_Fill_NSDLTONSDLRPT"
            End If
           
            Select Case Trim(Request.QueryString("DealFlag") & "")
                Case "DealConf"
                    If Trim(Request.QueryString("Wdmflag1") & "") = "W" Then
                        Me.Page.Title = "Deal Confirmation Report"
                        If Trim(Request.QueryString("DealType") & "") = "B" Then
                            If Trim(Request.QueryString("ModeofDelivery") & "") = "D" Then
                                If Trim(Request.QueryString("TransType") & "") = "P" Then
                                    strRptName = "WDMTradDealConfirmationPurchasedDmatBrok.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"
                                Else
                                    strRptName = "WDMTradDealConfirmationSaleDmat.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"
                                End If
                            Else
                                If Trim(Request.QueryString("TransType") & "") = "P" Then
                                    strRptName = "WDMTradDealConfirmationBuyingSGLBrok.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"
                                Else
                                    strRptName = "WDMTradDealConfirmationSellingSGL.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"
                                End If

                            End If
                        Else
                            If Trim(Request.QueryString("ModeofDelivery") & "") = "D" Then
                                If Trim(Request.QueryString("TransType") & "") = "P" Then
                                    strRptName = "WDMTradDealConfirmationPurchasedDmat.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"
                                Else
                                    strRptName = "WDMTradDealConfirmationSaleDmat.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"
                                End If
                            Else
                                If Trim(Request.QueryString("TransType") & "") = "P" Then
                                    strRptName = "WDMTradDealConfirmationBuyingSGL.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"
                                Else
                                    strRptName = "WDMTradDealConfirmationSellingSGL.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"
                                End If
                            End If
                        End If
                    Else

                        Me.Page.Title = "Deal Confirmation Report"
                        If Trim(Request.QueryString("DealType") & "") = "B" Then
                            If Trim(Request.QueryString("ModeofDelivery") & "") = "D" Then
                                If Trim(Request.QueryString("TransType") & "") = "P" Then
                            
                                    strRptName = "DealConfirmationPurchasedDmatBrok123.rpt"
                                    'strRptName = "DealConfirmation.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"

                                Else
                                    'strRptName = "DealConfirmation.rpt"
                                    strRptName = "DealConfirmationPurchasedDmatBrok123.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"
                                  
                                End If
                            Else
                                If Trim(Request.QueryString("TransType") & "") = "P" Then

                                    strRptName = "DealConfirmationBuyingSGLBrok.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"
                                Else
                                    strRptName = "DealConfirmationBuyingSGLBrok.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"
                                End If

                            End If
                        Else
                            If Trim(Request.QueryString("ModeofDelivery") & "") = "D" Then
                                If Trim(Request.QueryString("TransType") & "") = "P" Then
                                    'strRptName = "DealConfirmationPurchasedDmat.rpt"
                                    strRptName = "DealConfirmation.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"
                                Else
                                    strRptName = "DealConfirmation.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"
                                End If
                            Else
                                If Trim(Request.QueryString("TransType") & "") = "P" Then
                                    'strRptName = "DealConfirmationBuyingSGL.rpt"
                                    strRptName = "DealConfirmation.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"
                                Else
                                    'strRptName = "DealConfirmationSellingSGL.rpt"
                                    strRptName = "DealConfirmation.rpt"
                                    strProcName = "Id_RPT_BuyDealConfirmation"
                                End If
                            End If
                        End If

                    End If


                    If Trim(Request.QueryString("DealTypeFlag") & "") = "WP" Then
                        If Trim(Request.QueryString("DealslipType") & "") = "P" Then
                            strRptName = "WDMPrimaryDealConfirmation.rpt"
                        Else
                            strRptName = "WDMDealConfirmationPurchasedDmatBrok.rpt"
                        End If

                        strProcName = "Id_RPT_WDMBuyDealConfirmation"
                    End If
                    If Trim(Request.QueryString("DealTypeFlag") & "") = "WS" Then
                        If Trim(Request.QueryString("DealslipType") & "") = "P" Then
                            strRptName = "WDMPrimaryDealConfirmation.rpt"
                        Else
                            strRptName = "WDMDealConfirmationPurchasedDmatBrok.rpt"
                        End If
                        strProcName = "Id_RPT_WDMBuyDealConfirmation"
                    End If

                    If Trim(Request.QueryString("DealTypeFlag") & "") = "MDDC" Then

                        Me.Page.Title = "Deal Confirmation Report"

                        If Trim(Request.QueryString("ModeofDelivery") & "") = "D" Then
                            If Trim(Request.QueryString("TransType") & "") = "P" Then
                                strRptName = "DealConfirmation.rpts"
                                strProcName = "Id_RPT_BuyDealConfirmation"
                            Else
                                strRptName = "MergeDealConfirmationPurchasedDmat.rpt"
                                strProcName = "Id_RPT_BuyDealConfirmation"
                            End If
                        Else
                            If Trim(Request.QueryString("ModeofDelivery") & "") = "S" Then
                                strRptName = "MergeDealConfirmationBuyingSGL.rpt"
                                strProcName = "Id_RPT_BuyDealConfirmation"
                            Else

                            End If
                        End If
                    End If
                    If Trim(Request.QueryString("DealTypeFlag") & "") = "NDSMDDC" Then
                        Me.Page.Title = "Deal Confirmation Report"
                        strRptName = "MergeNDS_OM.rpt"
                        strProcName = "ID_FILL_NDS_OMRept"
                    End If

                    If Trim(Request.QueryString("DealTypeFlag") & "") = "MHDFC" Then
                        Me.Page.Title = "SGL HDFC Format"
                        Trim(Request.QueryString("TransType") & "")
                        strRptName = "SGLFormatHDFC.rpt"
                        strProcName = "ID_Rpt_SGLFederalFormat"
                    End If

                    If Trim(Request.QueryString("DealTypeFlag") & "") = "MFED" Then
                        Me.Page.Title = "SGL Federal Format"
                        Trim(Request.QueryString("TransType") & "")
                        strRptName = "SGLFederalFormat.rpt"
                        strProcName = "ID_Rpt_SGLFederalFormat"
                    End If

                    If Trim(Request.QueryString("DealTypeFlag") & "") = "DSP" Then
                        Me.Page.Title = "Deal Ticket Report"
                        strRptName = "WDMDealSlip_Trust.rpt"
                        strProcName = "Id_RPT_WDMBuyDealConfirmation"
                    End If
                    If Trim(Request.QueryString("DealTypeFlag") & "") = "DSS" Then
                        Me.Page.Title = "Deal Ticket Report"
                        strRptName = "WDMDealSlip_Trust.rpt"
                        strProcName = "Id_RPT_WDMBuyDealConfirmation"
                    End If
                 

                Case "DealTicket"
                    Me.Page.Title = "Deal Ticket Report"

                    If Trim(Request.QueryString("DealType") & "") = "B" Then
                        'If Trim(Request.QueryString("TransType") & "") = "S" Then
                        'strRptName = "DealSlip_Trust.rpt"
                        strRptName = "DealSlip_TrustBroking.rpt"
                        strProcName = "Id_RPT_DMATBuyingBricsSec"
                        'End If
                    Else
                        If Trim(Request.QueryString("TransType") & "") = "P" Then

                           strRptName = "DealSlip_Trust.rpt"
                            strProcName = "Id_RPT_DMATBuyingBricsSec"

                        Else
                            strRptName = "DealSlip_Trust.rpt"
                            strProcName = "Id_RPT_DMATBuyingBricsSec"
                        End If
                    End If

                Case "ContractNote"
                    Me.Page.Title = "ContractNote Report"
                    If Trim(Request.QueryString("ExchangeName") & "") = "NSE" Then
                        strRptName = "ContractNoteNSETradGST.rpt"
                        'strRptName = "ContractNoteBroking.rpt"
                        strProcName = "Id_FILL_ContractNoteBrokingRpt"
                    ElseIf Trim(Request.QueryString("ExchangeName") & "") = "BSE" Then
                        strRptName = "ContractNoteBSENormalGST.rpt"
                        strProcName = "ID_FILL_ContractNoteBSE_RPT"
                    End If
                Case "SGLFedFormat"
                    Me.Page.Title = "SGL Federal Format"
                    Trim(Request.QueryString("TransType") & "")
                    strRptName = "SGLFederalFormat.rpt"
                    strProcName = "ID_Rpt_SGLFederalFormat"

                Case "AxisLetter"
                    Me.Page.Title = "SGL Federal Format"
                    Trim(Request.QueryString("TransType") & "")
                    strRptName = "AxisBankLetter.rpt"
                    strProcName = "ID_FILL_RTGS_Rpt"

                Case "SGLHDFCFormat"
                    Me.Page.Title = "SGL HDFC Format"
                    Trim(Request.QueryString("TransType") & "")
                    strRptName = "SGLFormatHDFC.rpt"
                    strProcName = "ID_Rpt_SGLFederalFormat"


                Case "PhysicalAuthority"
                    Me.Page.Title = "Physical Authority Letter"
                    Trim(Request.QueryString("TransType") & "")
                    Trim(Request.QueryString("Hid_AuthorisedPer") & "")
                    strRptName = "AuthorityLetter_Physical.rpt"
                    strProcName = "ID_Rpt_SGLFederalFormat"


                Case "FinancialAuthority"
                    Me.Page.Title = "Financial Authority Letter"
                    Trim(Request.QueryString("TransType") & "")
                    strRptName = "FinancialAuthorityLetter.rpt"
                    strProcName = "ID_Rpt_SGLFederalFormat"

                Case "PhysicalAuthorityMerge"
                    Me.Page.Title = "Physical Authority Letter"
                    Trim(Request.QueryString("TransType") & "")
                    Trim(Request.QueryString("Hid_AuthorisedPer") & "")
                    strRptName = "AuthorityLetter_Physical_Merge.rpt"
                    strProcName = "ID_Rpt_SGLFederalFormat"


                Case "FinancialAuthorityMerge"
                    Me.Page.Title = "Financial Authority Letter"
                    Trim(Request.QueryString("TransType") & "")
                    strRptName = "FinancialAuthorityLetter.rpt"
                    strProcName = "ID_Rpt_SGLFederalFormat"


                Case "WDMContractNote"
                    Me.Page.Title = "ContractNote Report"
                    If Trim(Request.QueryString("ExchangeName") & "") = "BSE" Then
                        strRptName = "ContractNoteBSE_GST.rpt"
                        strProcName = "ID_FILL_ContractNoteBSE_RPT"
                    ElseIf Trim(Request.QueryString("ExchangeName") & "") = "NSE" Then
                        strRptName = "ContractNoteNSEBrokGST.rpt"
                        'strRptName = "ContractNoteNSE.rpt"
                        strProcName = "ID_FILL_ContractNoteBSE_RPT"

                    End If
            End Select
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function BindReport(Optional ByVal blnSepPages As Boolean = False) As ReportDocument
        Try
            Dim strPath As String
            Dim dtRpt As DataTable
            strDealDate = Hid_DealDate.Value
            strPath = ConfigurationManager.AppSettings("ReportsPath").ToString & strRptName
            rptDoc.Load(strPath)
            dtRpt = GetReportTable()
            rptDoc.SetDataSource(dtRpt)
            'strReportRange = " From " & Trim(Request.QueryString("FromDate") & "") & " To " & Trim(Request.QueryString("ToDate") & "") & ""
            'rptDoc.SummaryInfo.ReportComments = strReportRange
            If Trim(Request.QueryString("AuthorisedPerson") & "") <> "" Then
                rptDoc.DataDefinition.FormulaFields("AuthorisedPerson").Text = """" & Trim(Request.QueryString("AuthorisedPerson") & "") & """"
            End If
            If Trim(Request.QueryString("strPrintDet") & "") <> "" Then
                rptDoc.DataDefinition.FormulaFields("Flag").Text = """" & Trim(Request.QueryString("strPrintDet") & "") & """"
            End If
            If Trim(Request.QueryString("PrintRateAmt") & "") <> "" Then
                rptDoc.DataDefinition.FormulaFields("PrintRateAmt").Text = """" & Trim(Request.QueryString("PrintRateAmt") & "") & """"
            End If

            If strProcName = "Id_RPT_BuyDealConfirmation" Then
                If objCommon.DateFormat(strDealDate) >= strStampDutyDate Then
                    rptDoc.DataDefinition.FormulaFields("StampDutyApplicable").Text = """" & "Y" & """"
                Else
                    rptDoc.DataDefinition.FormulaFields("StampDutyApplicable").Text = """" & "N" & """"
                End If
                If Trim(Request.QueryString("strPrintLH") & "") <> "" Then
                    rptDoc.DataDefinition.FormulaFields("strPrintLH").Text = """" & Trim(Request.QueryString("strPrintLH") & "") & """"
                End If

                If Trim(Request.QueryString("PrintDealNo") & "") <> "" Then
                    rptDoc.DataDefinition.FormulaFields("PrintDealNo").Text = """" & Trim(Request.QueryString("PrintDealNo") & "") & """"
                End If

                If Trim(Request.QueryString("strPrintDP") & "") <> "" Then
                    rptDoc.DataDefinition.FormulaFields("strPrintDP").Text = """" & Trim(Request.QueryString("strPrintDP") & "") & """"
                End If

                If Trim(Request.QueryString("strPrintSignStamp") & "") <> "" Then
                    rptDoc.DataDefinition.FormulaFields("strPrintSignStamp").Text = """" & Trim(Request.QueryString("strPrintSignStamp") & "") & """"
                End If


            End If
            If strRptName = "CSGL.rpt" Then
                If Trim(Request.QueryString("strPrintLH") & "") <> "" Then
                    rptDoc.DataDefinition.FormulaFields("strPrintLH").Text = """" & Trim(Request.QueryString("strPrintLH") & "") & """"
                End If
            End If

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
            Dim DateFrom As Date
            Dim DateTo As Date
            OpenConn()
            sqlcomm.Connection = sqlconn
            With sqlcomm
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = "300"
                .CommandText = strProcName
                .Parameters.Clear()
                If Trim(Request.QueryString("Fromdate") & "") <> "" Then
                    DateFrom = objCommon.DateFormat(Request.QueryString("Fromdate").ToString())
                End If
                If Trim(Request.QueryString("Todate") & "") <> "" Then
                    DateTo = objCommon.DateFormat(Request.QueryString("Todate").ToString())
                End If

                If Trim(Request.QueryString("MergeDealNo") & "") <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@MergeDealNo", SqlDbType.VarChar, 50, "I", , , Trim(Request.QueryString("MergeDealNo") & ""))
                ElseIf strRptName = "ContractNoteNSE.rpt" Or strRptName = "ContractNoteBSE.rpt" Or strRptName = "ContractNoteNSEBrok.rpt" Or strRptName = "ContractNoteBSE_GST.rpt" Or strRptName = "ContractNoteNSEBrokGST.rpt" Then
                    objCommon.SetCommandParameters(sqlcomm, "@DealEntryId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("DealEntryId") & ""))
                    objCommon.SetCommandParameters(sqlcomm, "@BuySell", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("BuySell") & ""))

                Else
                    objCommon.SetCommandParameters(sqlcomm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(HttpUtility.UrlDecode(objCommon.DecryptText(Request.QueryString("DealSlipId"))) & ""))
                End If

                If Trim(Request.QueryString("DealTypeFlag") & "") <> "" And Trim(Request.QueryString("MergeDealNo") & "") = "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@TransType", SqlDbType.VarChar, 1000, "I", , , Right(Trim(Request.QueryString("DealTypeFlag") & ""), 1))
                End If
                'changes for deal report
                If Trim(Request.QueryString("TransType") & "") <> "" And (Trim(Request.QueryString("Rpt") & "") = "DealDetailReport" Or Trim(Request.QueryString("Rpt") & "") = "DealDetail") Then
                    objCommon.SetCommandParameters(sqlcomm, "@TransType", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("TransType") & ""))
                End If

                If Trim(Request.QueryString("DealTransType") & "") <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@DealTransType", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("DealTransType") & ""))
                End If

                If Trim(Request.QueryString("Fromdate") & "") <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@Fromdate", SqlDbType.SmallDateTime, 4, "I", , , DateFrom)
                End If
                If Trim(Request.QueryString("Todate") & "") <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@Todate", SqlDbType.SmallDateTime, 4, "I", , , DateTo)
                End If
                If Trim(Request.QueryString("Rpt") & "") = "NSDLTONSDL" Or Trim(Request.QueryString("Rpt") & "") = "NSDLTOCSDL" Then
                    If Trim(Request.QueryString("DematRptType") & "") IsNot Nothing Then
                        objCommon.SetCommandParameters(sqlcomm, "@DematRptType", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("DematRptType") & ""))
                    End If
                    If Trim(Hid_Intids.Value) <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@DealSlipIds", SqlDbType.VarChar, 1000, "I", , , Hid_Intids.Value)
                    End If
                    objCommon.SetCommandParameters(sqlcomm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                    objCommon.SetCommandParameters(sqlcomm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                  
                End If


                .ExecuteNonQuery()
                sqlda.SelectCommand = sqlcomm
                sqlda.Fill(dtfill)
                If dtfill.Rows.Count > 0 And strRptName = "DealConfirmation.rpt" Then
                    strDealDate = (dtfill.Rows(0).Item("DD") & "")
                End If
            End With
            Return dtfill
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Function


    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            If rptDoc IsNot Nothing Then
                rptDoc.Close()
                rptDoc.Dispose()
                GC.Collect()
                sqlconn.Dispose()
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
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
        If WordPrint = "PP" Or Trim(Request.QueryString("Rpt") & "") = "NSDLTONSDL" Then
            crReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "Report")
        Else
            crReport.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report")
        End If
        Try
            Response.Flush()
            Response.End()
        Catch ex As Exception
        Finally
        End Try
    End Sub

    Private Sub ConvertDealRpt(ByVal strProcName As String)
        Try
            OpenConn()
            Dim rptDoc As ReportDocument
            Dim dt As DataTable
            dt = GetDealReportTable(strProcName)
            Session("GetData") = dt
            rptDoc = BindDealReport()
            If dt.Rows.Count > 0 Then
                ExportReport(rptDoc)
                'Dim objExport As New RKLib.ExportData.Export("Web")
                'objExport.ExportDetails(dt, Export.ExportFormat.Excel, "Report.xls")
            Else
                Dim strHtml As String
                strHtml = "alert('Sorry!!! No Records available to show report');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
            End If
            rptDoc = Nothing
        Catch ex As Exception
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function BindDealReport(Optional ByVal blnSepPages As Boolean = False) As ReportDocument
        Try
            Dim strPath As String
            Dim dtRpt As DataTable
            strPath = ConfigurationManager.AppSettings("ReportsPath").ToString & strRptName
            rptDoc.Load(strPath)
            dtRpt = GetDealReportTable("ID_Fill_DealDetailRptNew")
            rptDoc.SetDataSource(dtRpt)
            'strReportRange = " From " & Trim(Request.QueryString("FromDate") & "") & " To " & Trim(Request.QueryString("ToDate") & "") & ""
            'rptDoc.SummaryInfo.ReportComments = strReportRange
            If Trim(Request.QueryString("AuthorisedPerson") & "") <> "" Then
                rptDoc.DataDefinition.FormulaFields("AuthorisedPerson").Text = """" & Trim(Request.QueryString("AuthorisedPerson") & "") & """"
            End If
            If Trim(Request.QueryString("strPrintDet") & "") <> "" Then
                rptDoc.DataDefinition.FormulaFields("Flag").Text = """" & Trim(Request.QueryString("strPrintDet") & "") & """"
            End If

            rptDoc.RecordSelectionFormula = clsCommonFuns.strRecordSelection
            clsCommonFuns.strRecordSelection = ""
            rptDoc = objCommon.GetReport(rptDoc)
            rptDoc.VerifyDatabase()
            rptDoc.Refresh()
            'CrystalReportViewer1.SeparatePages = False
            'CrystalReportViewer1.ReportSource = rptDoc
            'CrystalReportViewer1.DataBind()
            'CrystalReportViewer1.RefreshReport()
            Return rptDoc
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            'rptDoc.Dispose()
        End Try
    End Function
    Private Function GetDealReportTable(ByVal strProcName As String) As DataTable
        Try
            Dim datFrom As Date
            Dim datTo As Date
            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand

            'Dim intColIndex As Int16

            'datFrom = objCommon.DateFormat(txt_Date.Text)

            'openconnection()
            With sqlComm
                .Connection = sqlconn
                .CommandType = CommandType.StoredProcedure
                .CommandText = strProcName




                objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , Trim(Hid_TransType.Value))
                objCommon.SetCommandParameters(sqlComm, "@TransType", SqlDbType.Char, 1, "I", , , Trim(Hid_dealTransType.Value))
                objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(Hid_DealslipId.Value))
                objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                'objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)

                .ExecuteNonQuery()
            End With
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dtfill)

            Return dtfill
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally

        End Try
    End Function
End Class
