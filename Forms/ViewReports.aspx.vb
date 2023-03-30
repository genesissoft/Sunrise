Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class Forms_ViewReports
    Inherits System.Web.UI.Page
    Dim sqlconn As SqlConnection
    Dim rptDoc As New ReportDocument
    Dim objCommon As New clsCommonFuns
    Dim intZoom As Int16
    Dim strValues() As String
    Dim strRptName As String
    Dim strProcName As String
    Dim objUtil As New Util
    Dim PgName As String = "$ViewReports$"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Val(Session("UserId") & "") = 0 Then
            Response.Redirect("Login.aspx", False)
            Exit Sub
        End If

        'objCommon.OpenConn()
        Try
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            If Not rptDoc Is Nothing Then
                rptDoc.Close()
            End If
            If IsPostBack = False Then

                Hid_ImgIds.Value = Session("ImgIds")
                If Trim(Request.QueryString("Rpt") & "") = "NSDLTONSDL" Or Trim(Request.QueryString("Rpt") & "") = "NSDLTOCSDL" Or Trim(Request.QueryString("Rpt") & "") = "NSDLIndusInd" Then
                    Hid_Intids.Value = Session("intids")
                End If

                If Trim(Request.QueryString("DealSlipId") & "") <> "" Then
                    Hid_dealslipId.Value = Val(Request.QueryString("DealSlipId"))
                    Hid_Transtype.Value = Trim(Request.QueryString("Transtype"))

                End If

                If (Hid_Counter.Value <> "1") Then
                    btn_Print.Visible = False
                    GetReportName()
                    BindReport()

                    'rptDoc = Nothing
                Else
                    btn_Print.Visible = True
                    GetReportName()
                    BindReport()
                End If
                btn_Print.Visible = False

            Else
                GetReportName()
                BindReport()
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex)
            Response.Write(ex.Message)
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

    Private Sub GetReportName()
        Try
         

            If Trim(Request.QueryString("Rpt") & "") = "DailyPortfolio" Then
                strRptName = "DailyPortfolioRept.rpt"
                strProcName = "ID_FILL_DailyPortFolio_REPT"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "RetailDebitRpt" Then
                strRptName = "DebitNoteDetailGST.rpt"
                strProcName = "ID_FILL_RetailDebitNoteRpt"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "DailyPurchaseSale" Then
                strRptName = "DailyPurchaseSaleRept.rpt"
                strProcName = "ID_FILL_DailyPurchaseSell_Rept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "PendingDematDelivery" Then
                strRptName = "PendingDematDelivery.rpt"
                strProcName = "ID_FILL_PendingDematDelivery_RPT"
                'ElseIf Trim(Request.QueryString("Rpt") & "") = "MismatchedDealFaceValue" Then
                '    strRptName = "MismatchedDealFaceValue.rpt"
                '    strProcName = "ID_FILL_MismatchedDealFaceValue_Rept"
                'ElseIf Trim(Request.QueryString("Rpt") & "") = "MismatchedDealSettlementDate" Then
                '    strRptName = "MismatchedDealSettlementDate.rpt"
                '    strProcName = "ID_FILL_MismatchedDealFaceValue_Rept"
                '----------------- Mismatched Report
            ElseIf Trim(Request.QueryString("Rpt") & "") = "MismatchedFinancialDelivery" Then
                strRptName = "MismatchedFinancialDeliveryReport.rpt"
                strProcName = "ID_FILL_MismatchedFinancialDelivery_Rept"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "MismatchedDMATDelivery" Then
                strRptName = "MismatchedDMATDelivery.rpt"
                strProcName = "ID_FILL_MismatchedDMATDelivery_Rept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "ClientwiseBrokerage" Then
                '------------------------------
                strRptName = "ClientwiseBrokerage.rpt"
                strProcName = "ID_FILL_ClientwiseBrokerage_Rept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "DetailClientBrokerage" Then
                '------------------------------
                strRptName = "DetailClientwiseBrokerage.rpt"
                strProcName = "ID_FILL_DetailClientwiseBrokerage"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "ClientwiseTrans" Then
                strRptName = "ClientwiseTransaction.rpt"
                strProcName = "ID_FILL_ClientwiseTrans_Rept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "WDMClientwiseTrans" Then
                strRptName = "WDMClientwiseTransaction.rpt"
                strProcName = "Id_RPT_WDMClientwiseTransRept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "DealStockGraph" Then 'poon
                'strRptName = "a_graph.rpt"
                strRptName = "a_pie.rpt"
                strProcName = "ID_FILL_ViewDealStockDetailsPerSecType"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "LineGraph" Then 'poon
                'strRptName = "a_graph.rpt"
                strRptName = "a_line2.rpt"
                strProcName = "ID_FILL_ViewYieldPerSecurity"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "ClientwiseTransP" Then
                strRptName = "ClientwiseTransProfit.rpt"
                strProcName = "ID_FILL_ClientwiseTrans_Rept"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "DebitNoteNormal" Then
                strRptName = "DebitNoteNewNormal.rpt"
                strProcName = "Id_RPT_WDMdebtnotenewNormal"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "DebitNoteLetterNormal" Then
                strRptName = "DebitNoteLatter.rpt"
                strProcName = "Id_RPT_WDMdebtnotenewNormal"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "DebitNoteDetailNormal" Then
                strRptName = "DebitNoteDetailReport.rpt"
                strProcName = "Id_RPT_debtnotenewNormalDetail"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "DebitNote" Then
                If (Request.QueryString("PageName") & "") = "DebitNote.aspx" Then
                    strRptName = "DebitNoteDetailGST.rpt"
                    strProcName = "Id_RPT_WDMdebtnoteNew1"
            End If


            ElseIf Trim(Request.QueryString("Rpt") & "") = "ClientWiseMonthly" Then
                strRptName = "ClientWiseMonthaly.rpt"
                strProcName = "Id_RPT_ClientWiseMonthaly"


            ElseIf Trim(Request.QueryString("Rpt") & "") = "AdvisoryReport" Then
                strRptName = "AdvisoryNew.rpt"
                strProcName = "Id_RPT_Advisorynote"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "RetailDebitRpt" Then
                strRptName = "RetailDebitNote.rpt"
                strProcName = "ID_FILL_RetailDebitNoteRpt"


            ElseIf Trim(Request.QueryString("Rpt") & "") = "RetailCreditRpt" Then
                strRptName = "RetailCreditNote.rpt"
                strProcName = "ID_FILL_RetailCreditRpt"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "TestAdvisoryReport" Then
                strRptName = "AdvisoryNew.rpt"
                strProcName = "ID_RPT_TestAdvisoryRpt"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "AdvisoryLatterReport" Then
                strRptName = "AdvisoryLatterGST.rpt"
                strProcName = "Id_RPT_Advisorynote"


            ElseIf Trim(Request.QueryString("Rpt") & "") = "StampDutyDetail" Then
                strRptName = "Stampdutydetails.rpt"
                strProcName = "Id_RPT_StampDuty"


            ElseIf Trim(Request.QueryString("Rpt") & "") = "StampDutySummary" Then
                strRptName = "StampdutySummary.rpt"
                strProcName = "Id_RPT_StampDutySummary"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "DealwiseBrokerage" Then
                strRptName = "DealwiseBrokerage.rpt"
                strProcName = "ID_FILL_DealwiseBrokerage_Rept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "ExchangewiseTrans" Then
                strRptName = "ExchangewiseTransaction.rpt"
                strProcName = "ID_FILL_ExchangewiseTrans_Rept"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "CancelledWDMDeals" Then
                strRptName = "CancelledWDMDealEntry_Rpt.rpt"
                strProcName = "ID_FILL_CancelWDMDeals_Rpt"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "CancelledDeals" Then
                strRptName = "CancelDeal.rpt"
                strProcName = "ID_FILL_CancelNormalDeal_Rept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "RetailMIS" Then
                strRptName = "RetailMIS.rpt"
                strProcName = "ID_FILL_RetailMIS"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "CustomerDocStatus" Then
                strRptName = "CustomerDocuments.rpt"
                strProcName = "ID_FILL_CustomerDocuments_Rept"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "SecuritywiseTrans" Then
                strRptName = "SecuritywiseTransaction.rpt"
                strProcName = "ID_FILL_SecuritywiseTrans_Rept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "WDMSecuritywiseTrans" Then
                strRptName = "WDM_SecurityWiseTrans.rpt"
                strProcName = "ID_FILL_WDMSecWiseTransRpt"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "SecwiseTransP" Then
                strRptName = "SecuritywiseTransProfit.rpt"
                strProcName = "ID_FILL_SecuritywiseTrans_Rept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "StaffwiseTrans" Then
                strRptName = "StaffwiseTransaction.rpt"
                strProcName = "ID_FILL_StaffwiseTrans_Rept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "StaffwiseTransP" Then
                strRptName = "StaffwiseTransProfit.rpt"
                strProcName = "ID_FILL_StaffwiseTrans_Rept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "AccountAccuredInterest" Then
                strRptName = "AccountAccuredInterest.rpt"
                strProcName = "ID_FILL_AccountAccuredInterest_Rept"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "AccountPurchase" Then
                strRptName = "AccountPurSell.rpt"
                strProcName = "ID_FILL_AccountPurSell_Rept"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "mismatchedPurchase" Then
                strRptName = "MismatchedPur.rpt"
                strProcName = "ID_FILL_AccountPurSell_Rept"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "AccountSell" Then
                strRptName = "AccountPurSell.rpt"
                strProcName = "ID_FILL_AccountPurSell_Rept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "AccountProfit" Then
                strRptName = "AccountProfit.rpt"
                strProcName = "ID_FILL_AccountProfit_Rept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "BrokeragePaidRecd" Then
                strRptName = "BrokeragePaidRecd.rpt"
                strProcName = "ID_FILL_BrokPaidRecd_Rept"
                'strProcName = "ID_FILL_BrokeragePaidRecd_Rept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "AuthorityLtrPhysical" Then
                strRptName = "AuthorityLetter_Physical.rpt"
                strProcName = "ID_Rpt_SGLFederalFormat"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "AuthorityLtrFinancial" Then
                strRptName = "FinancialAuthorityLetter.rpt"
                strProcName = "ID_Rpt_SGLFederalFormat"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "CustomerInfo" Then
                strRptName = "CustomerInfo.rpt"
                strProcName = "ID_FILL_CustomerInfoRpt"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "SGLFedFormat" Then
                strRptName = "SGLFederalFormat.rpt"
                strProcName = "ID_Rpt_SGLFederalFormat"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "SGLHDFCFormat" Then
                strRptName = "SGLFormatHDFC.rpt"
                strProcName = "ID_Rpt_SGLFederalFormat"


            ElseIf Trim(Request.QueryString("Rpt") & "") = "FinancialDelivery" Then
                strRptName = "FinancialDelivery.rpt"
                strProcName = "ID_FILL_FinancialDelivery_Rept"


            ElseIf Trim(Request.QueryString("Rpt") & "") = "FinancialDeliveryDetails" Then
                strRptName = "FinancialDeliveryDetail.rpt"
                strProcName = "ID_FILL_FinancialDelivery_Rept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "ClientBrokSummary" Then
                strRptName = "ClientwiseBrokSummary.rpt"
                strProcName = "ID_RPT_MISSummary"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "SecTypSumm" Then
                strRptName = "SecTypeWiseSumm.rpt"
                strProcName = "ID_RPT_MISSummary"


            ElseIf Trim(Request.QueryString("Rpt") & "") = "CategorywiseTrans" Then
                strRptName = "CategoryWiseTransaction.rpt"
                strProcName = "ID_RPT_CategorywiseTrans"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "CategorywiseTransP" Then
                strRptName = "CategoryWiseTransP.rpt"
                strProcName = "ID_RPT_CategorywiseTrans"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "InflowOutFlowSec" Then
                strRptName = "InflowOutFlow_Security.rpt"
                strProcName = "ID_FILL_InflowOutFlowSec_Rpt"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "NSDLTONSDL" Then
                If Trim(Request.QueryString("DPName") & "").IndexOf("THE FEDERAL BANK LTD.") <> -1 Then
                    strRptName = "FEDNSDLTONSDLRPT.rpt"
                Else
                    strRptName = "FEDNSDLTONSDLRPT.rpt"
                End If
                'strRptName = "NSDLTONSDLRPT.rpt"
                'strRptName = "FEDNSDLTONSDLRPT.rpt"
                strProcName = "ID_Fill_NSDLTONSDLRPT"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "NSDLIndusInd" Then
                strRptName = "IndusInd_DIS_NSDL.rpt"
                strProcName = "ID_Fill_NSDLTONSDLRPT"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "NSDLTOCSDL" Then
                strRptName = "NSDLTOCSDLRPT.rpt"
                strProcName = "ID_Fill_NSDLTONSDLRPT"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "InflowOutFlowPayment" Then
                strRptName = "InflowOutflowReport.rpt"
                'If Session("CompId") = 2 Then
                '    strProcName = "ID_FILL_WDMInflowOutFlow_Rept"
                'Else
                strProcName = "ID_FILL_InflowOutFlow_Rept"
                'End If


            ElseIf Trim(Request.QueryString("Rpt") & "") = "InflowOutflowSecurity" Then
                strRptName = "InflowOutflowSecurity.rpt"
                strProcName = "ID_FILL_InflowOutFlow_Rept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "InflowOutflow" Then
                strRptName = "InflowOutflow_Rpt.rpt"
                strProcName = "ID_FILL_InflowOutFlow_Rpt"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "InflowOutflowBnk" Then
                strRptName = "InflowOutFlowBank.rpt"
                strProcName = "ID_FILL_InflwOutFlwBnk_Rpt"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "DailyTransaction" Then
                strRptName = "DailyTransaction.rpt"
                strProcName = "Id_RPT_DailyTransactionRpt"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "ConsolidatedRpt" Then
                strRptName = "ConsolidatedReport.rpt"
                strProcName = "Id_RPT_ConsolidatedReport"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "ViewDealStockTrad" Then
                strRptName = "ViewDealStockTrad.rpt"
                strProcName = "ID_FILL_ViewDealStockDetailsExp"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "DealStockGraph" Then 'poon
                'strRptName = "a_graph.rpt"
                strRptName = "a_pie.rpt"
                strProcName = "ID_FILL_ViewDealStockDetailsPerSecType"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "LineGraph" Then 'poon
                'strRptName = "a_graph.rpt"
                strRptName = "a_line2.rpt"
                strProcName = "ID_FILL_ViewYieldPerSecurity"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "ViewDealStockFinNor" Then
                strRptName = "ViedDealStockFinanNormal.rpt"
                strProcName = "ID_FILL_ViewFinancialDealStockDetailsExp_Normal"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "ViewDealStockFin" Then
                strRptName = "ViewDealStockFinan.rpt"
                strProcName = "ID_FILL_ViewFinancialDealStockDetailsExp"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "RetCrmDelearWiseDetail" Then
                strRptName = "RetailCrmDelearWiseDetail.rpt"
                strProcName = "ID_RPT_Crm"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "RetCrmCustWiseDetail" Then
                strRptName = "RetailCrmCustWiseDetail.rpt"
                strProcName = "ID_RPT_Crm"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "RetailAudit" Then
                strRptName = "RetailAudit.rpt"
                strProcName = "ID_FILL_AUDIT_Report"

            ElseIf Trim(Request.QueryString("Rpt") & "") = "CustomerDealsSummary" Then
                strRptName = "CustomerDealsSummary.rpt"
                strProcName = "ID_FILL_CustomerDealsSummaryRpt"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "WDMTransRepts" Then
                If Trim(Request.QueryString("WDMTransFlag") & "") = "D" Then
                    strRptName = "WDMDealerwise.rpt"
                    strProcName = "Id_RPT_MisDealerWise"
                ElseIf Trim(Request.QueryString("WDMTransFlag") & "") = "T" Then
                    strRptName = "MisDealerVolTrad.rpt"
                    strProcName = "Id_RPT_MisDealerVolTrad"
                ElseIf Trim(Request.QueryString("WDMTransFlag") & "") = "R" Then
                    strRptName = "MisDealerVolBrok.rpt"
                    strProcName = "Id_RPT_MisDealerVolBrok"
                ElseIf Trim(Request.QueryString("WDMTransFlag") & "") = "K" Or Trim(Request.QueryString("WDMTransFlag") & "") = "L" Or Trim(Request.QueryString("WDMTransFlag") & "") = "O" Then
                    strRptName = "MisDealerwiseBrok.rpt"
                    strProcName = "Id_RPT_MisDealerBrokerage"
                ElseIf Trim(Request.QueryString("WDMTransFlag") & "") = "S" Then
                    strRptName = "WDMSegmentwise.rpt"
                    strProcName = "Id_RPT_WDMSegmentWise"
                ElseIf Trim(Request.QueryString("WDMTransFlag") & "") = "W" Then
                    strRptName = "WDMDealerWiseSegment.rpt"
                    strProcName = "Id_RPT_MisDealerWiseSegment"
                Else
                    strRptName = "MisDealerVol.rpt"
                    strProcName = "Id_RPT_MisDealerVol"
                End If


            ElseIf Trim(Request.QueryString("Rpt") & "") = "DealDetailReport" Then
                strRptName = "DealDetailNew.rpt"
                strProcName = "ID_Fill_DealDetailRptNew"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "MergeDeals" Then
                strRptName = "MergeDeals_Rpt.rpt"
                strProcName = "Id_RPT_MergeDealReport"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "WithWithoutrokerage" Then
                strRptName = "WithWithoutBrokerage.rpt"
                strProcName = "ID_FILL_BrokeragerRept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "CustomerwiseBrokerage" Then
                strRptName = "CustomerwiseBrokerage.rpt"
                strProcName = "ID_FILL_ClientwiseBrokeragerRept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "QuarterlyRpt" Then
                strRptName = "QuarterlyClientRept.rpt"
                strProcName = "Id_RPT_QuarterlyClientRept"
            ElseIf Trim(Request.QueryString("Rpt") & "") = "RTGSRepts" Then
                strProcName = "ID_FILL_RTGS_Rpt"
                If Trim(Request.QueryString("RptTyp") & "") = "H" Then
                    strRptName = "RTGS_HDFC.rpt"
                ElseIf Trim(Request.QueryString("RptTyp") & "") = "I" Then
                    strRptName = "RTGS_ICICI.rpt"
                ElseIf Trim(Request.QueryString("RptTyp") & "") = "F" Then
                    strRptName = "RTGS_FED.rpt"
                End If
            ElseIf Trim(Request.QueryString("Rpt") & "") = "MisReport" Then
                If Trim(Request.QueryString("MISFlag") & "") = "C" Then
                    strRptName = "MisBrokerage.rpt"
                    strProcName = "Id_RPT_MisBroker"
                ElseIf Trim(Request.QueryString("MISFlag") & "") = "D" Then
                    strRptName = "MisDailyDealer.rpt"
                    strProcName = "Id_RPT_MisDealer"
                ElseIf Trim(Request.QueryString("MISFlag") & "") = "S" Then
                    strRptName = "SubBrokerage.rpt"
                    strProcName = "Id_RPT_MisDealer"
                ElseIf Trim(Request.QueryString("MISFlag") & "") = "A" Then
                    strRptName = "MIS_Consultancy.rpt"
                    strProcName = "ID_RPT_MISCons"
                ElseIf Trim(Request.QueryString("MISFlag") & "") = "B" Then
                    strRptName = "WDMClientwiseSummary.rpt"
                    strProcName = "ID_RPT_WDMClientwiseSummary"
                ElseIf Trim(Request.QueryString("MISFlag") & "") = "R" Then
                    strRptName = "WDMClientTypeSummary.rpt"
                    strProcName = "ID_RPT_WDMClientwiseSummary"

                End If

            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetReportName", "Error in GetReportName", "", ex)
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Function BindReport(Optional ByVal blnSepPages As Boolean = False) As ReportDocument
        Try
            Dim strPath As String
            Dim dtRpt As DataTable
            Dim strReportRange As String
            Dim strReturn As String
            Dim strValue As String
            Dim intValue As Integer
            Dim strDecValue As Integer
            Dim slen As Integer
            OpenConn()

            strPath = ConfigurationSettings.AppSettings("ReportsPath") & "\" & strRptName
            rptDoc.Load(strPath)

            Dim intSettAmt As Double
            intSettAmt = Val(Request.QueryString("SettAmt") & "")
            strValue = Trim(Request.QueryString("SettAmt") & "")
            If strValue.IndexOf(".") <> -1 Then
                Dim start_value As [Double] = (Request.QueryString("SettAmt") & "")
                Dim end_values As String() = (Request.QueryString("SettAmt") & "").ToString().Split(".")
                slen = end_values(1).Length
            Else
                slen = 0
            End If


            If strValue.IndexOf(".") = -1 Then
                strDecValue = 0
            Else
                If slen = 1 Then
                    strDecValue = Right(strValue, 1)
                Else
                    strDecValue = Right(strValue, 2)
                End If

            End If
            Dim intPart As Integer = Math.Truncate(intSettAmt)

            dtRpt = GetReportTable()
            If (dtRpt.Rows.Count = 0) Then
                Dim msg As String = "No Trade Done"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
                Exit Function
            Else
                rptDoc.SetDataSource(dtRpt)
                strReportRange = " From " & Trim(Request.QueryString("FromDate") & "") & " To " & Trim(Request.QueryString("ToDate") & "") & ""
                rptDoc.SummaryInfo.ReportComments = strReportRange
                rptDoc.RecordSelectionFormula = strRecordSelection
                strRecordSelection = ""



                If Trim(Request.QueryString("Rpt") & "") = "AccountPurchase" Then
                    rptDoc.DataDefinition.FormulaFields("ReportType").Text = """P"""

                ElseIf Trim(Request.QueryString("Rpt") & "") = "mismatchedPurchase" Then
                    rptDoc.DataDefinition.FormulaFields("ReportType").Text = """P"""

                ElseIf Trim(Request.QueryString("Rpt") & "") = "AccountSell" Then
                    rptDoc.DataDefinition.FormulaFields("ReportType").Text = """S"""
                End If
                If Trim(Request.QueryString("Rpt") & "") = "DealDetailReport" Then
                    If Trim(Request.QueryString("Transtype")) = "P" Then
                        rptDoc.DataDefinition.FormulaFields("TransType").Text = """P"""
                    Else
                        rptDoc.DataDefinition.FormulaFields("TransType").Text = """S"""
                    End If
                End If
                If Trim(Request.QueryString("Rpt") & "") = "RTGSRepts" Then
                    rptDoc.DataDefinition.FormulaFields("Amount").Text = intPart
                    rptDoc.DataDefinition.FormulaFields("DecimalVal").Text = strDecValue
                    rptDoc.DataDefinition.FormulaFields("SettlementAmt").Text = Val(Request.QueryString("SettAmt") & "")
                    rptDoc.DataDefinition.FormulaFields("Narration").Text = """" & Trim(Request.QueryString("Narration") & "") & """"
                    If Trim(Request.QueryString("RptTyp") & "") = "I" Then
                        rptDoc.DataDefinition.FormulaFields("ContactDetails").Text = """" & Trim(Request.QueryString("CtcDetails")) & """"
                    ElseIf Trim(Request.QueryString("RptTyp") & "") = "F" Then
                        rptDoc.DataDefinition.FormulaFields("KindAtten").Text = """" & Trim(Request.QueryString("KindAttn") & "") & """"
                        rptDoc.DataDefinition.FormulaFields("AccountNo").Text = """" & Trim(Request.QueryString("AccountNo") & "") & """"
                    End If
                End If

                
                If Trim(Request.QueryString("Rpt") & "") = "ViewDealStockTrad" _
                                  Or Trim(Request.QueryString("Rpt") & "") = "ViewDealStockFinNor" Or Trim(Request.QueryString("Rpt") & "") = "ViewDealStockFin" Then
                    rptDoc.DataDefinition.FormulaFields("ForDate").Text = """" & Trim(Request.QueryString("ForDate") & "") & """"
                    rptDoc.DataDefinition.FormulaFields("Interest").Text = """" & Trim(Request.QueryString("Interest") & "") & """"

                End If

                rptDoc = objCommon.GetReport(rptDoc)
                rptDoc.VerifyDatabase()
                rptDoc.Refresh()

                If blnSepPages = True Then
                    CrystalReportViewer1.SeparatePages = False
                Else
                    CrystalReportViewer1.SeparatePages = True
                End If
                If intZoom <> 0 Then
                    CrystalReportViewer1.PageZoomFactor = intZoom
                End If
                CrystalReportViewer1.ReportSource = rptDoc
                CrystalReportViewer1.DataBind()
                CrystalReportViewer1.RefreshReport()
                Return rptDoc
                Hid_Counter.Value = "1"
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "BindReport", "Error in BindReport", "", ex)
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try
    End Function
    Private Function GetReportTable() As DataTable
        Try
            Dim sqlcomm As New SqlCommand
            Dim sqlda As New SqlDataAdapter
            Dim dtfill As New DataTable
            Dim DateFrom As Date
            Dim DateTo As Date
            Dim ForDate As Date
            'Dim i As Integer
            'Dim intimgids As String

            'Dim strusertype As String
            If Trim(Request.QueryString("Fromdate") & "") <> "" Then
                DateFrom = objCommon.DateFormat(Request.QueryString("Fromdate").ToString())
            End If
            If Trim(Request.QueryString("Todate") & "") <> "" Then
                DateTo = objCommon.DateFormat(Request.QueryString("Todate").ToString())
            End If
            If Trim(Request.QueryString("ForDate") & "") <> "" Then
                ForDate = objCommon.DateFormat(Request.QueryString("ForDate").ToString())
            End If


            With sqlcomm
                .Connection = sqlconn
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = "1000"
                .CommandText = strProcName
                .Parameters.Clear()
                ' If Trim(Request.QueryString("Rpt") & "") = "DebitNote" Then

                If Trim(Request.QueryString("Rpt") & "") = "DealStockGraph" Then 'poon
                    objCommon.SetCommandParameters(sqlcomm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                    objCommon.SetCommandParameters(sqlcomm, "@Fordate", SqlDbType.SmallDateTime, 4, "I", , , ForDate)
                    objCommon.SetCommandParameters(sqlcomm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId") & ""))
                ElseIf Trim(Request.QueryString("Rpt") & "") = "LineGraph" Then 'poon
                    objCommon.SetCommandParameters(sqlcomm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                    objCommon.SetCommandParameters(sqlcomm, "@fromdate", SqlDbType.SmallDateTime, 4, "I", , , DateFrom)
                    objCommon.SetCommandParameters(sqlcomm, "@toDate", SqlDbType.SmallDateTime, 4, "I", , , DateTo)
                    objCommon.SetCommandParameters(sqlcomm, "@SecurityId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("SecurityId") & ""))
                    objCommon.SetCommandParameters(sqlcomm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId") & ""))

                ElseIf Trim(Session("DebitRefNo") & "") <> "" And Trim(Request.QueryString("Rpt") & "") = "DebitNote" Then
                    objCommon.SetCommandParameters(sqlcomm, "@DebitRefNo", SqlDbType.VarChar, 20000, "I", , , Trim(Session("DebitRefNo")))
                    objCommon.SetCommandParameters(sqlcomm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                    objCommon.SetCommandParameters(sqlcomm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                    objCommon.SetCommandParameters(sqlcomm, "@DebitInvFlag", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("DebitInvFlag")) & "")
                    If Trim(Request.QueryString("DateTypeFlag") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@DateTypeFlag", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("DateTypeFlag") & ""))
                    End If
                    If Trim(Request.QueryString("Fromdate") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@Fromdate", SqlDbType.SmallDateTime, 4, "I", , , DateFrom)
                    End If
                    If Trim(Request.QueryString("Todate") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@Todate", SqlDbType.SmallDateTime, 4, "I", , , DateTo)
                    End If
                ElseIf Trim(Session("DebitRefNoNormal") & "") <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@DebitRefNoNormal", SqlDbType.VarChar, 4000, "I", , , Trim(Session("DebitRefNoNormal")))
                    objCommon.SetCommandParameters(sqlcomm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                    objCommon.SetCommandParameters(sqlcomm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                ElseIf (Trim(Session("AdvisoryRefNo") & "") <> "" Or Trim(Session("AdvisoryLetterRefNo") & "") <> "") And (Trim(Request.QueryString("Rpt") & "") = "AdvisoryReport" Or Trim(Request.QueryString("Rpt") & "") = "AdvisoryLatterReport") Then
                    'ElseIf Trim(Request.QueryString("Rpt") & "") = "AdvisoryReport" And Trim(Request.QueryString("Rpt") & "") = "AdvisoryLatterReport" Then
                    objCommon.SetCommandParameters(sqlcomm, "@AdvisoryRefNo", SqlDbType.VarChar, 4000, "I", , , Trim(Session("AdvisoryRefNo")))
                    objCommon.SetCommandParameters(sqlcomm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                    objCommon.SetCommandParameters(sqlcomm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                    If Trim(Request.QueryString("Fromdate") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@Fromdate", SqlDbType.SmallDateTime, 4, "I", , , DateFrom)
                    End If
                    If Trim(Request.QueryString("Todate") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@Todate", SqlDbType.SmallDateTime, 4, "I", , , DateTo)
                    End If
                    If Trim(Request.QueryString("DateTypeFlag") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@DateTypeFlag", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("DateTypeFlag") & ""))
                    End If
                ElseIf Trim(Session("RetailDebitRefNo") & "") <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@RetailDebitRefNo", SqlDbType.VarChar, 4000, "I", , , Trim(Session("RetailDebitRefNo")))
                    objCommon.SetCommandParameters(sqlcomm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                    objCommon.SetCommandParameters(sqlcomm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                    If Trim(Request.QueryString("Rpt") & "") = "RetailDebitRpt" Then
                        objCommon.SetCommandParameters(sqlcomm, "@DealTransType", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("DealTransType") & ""))
                    End If
                    If Trim(Request.QueryString("Fromdate") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@Fromdate", SqlDbType.SmallDateTime, 4, "I", , , DateFrom)
                    End If
                    If Trim(Request.QueryString("Todate") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@Todate", SqlDbType.SmallDateTime, 4, "I", , , DateTo)
                    End If
                ElseIf Trim(Session("RetCreditRefNo") & "") <> "" And Trim(Request.QueryString("Rpt") & "") = "RetailCreditRpt" Then
                    objCommon.SetCommandParameters(sqlcomm, "@RetCreditRefNo", SqlDbType.VarChar, 4000, "I", , , Trim(Session("RetCreditRefNo")))
                    objCommon.SetCommandParameters(sqlcomm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                    objCommon.SetCommandParameters(sqlcomm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))

                Else

                    If Trim(Request.QueryString("Fromdate") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@Fromdate", SqlDbType.SmallDateTime, 4, "I", , , DateFrom)
                    End If
                    If Trim(Request.QueryString("Todate") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@Todate", SqlDbType.SmallDateTime, 4, "I", , , DateTo)
                    End If
                    If Trim(Request.QueryString("Fordate") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@Fordate", SqlDbType.SmallDateTime, 4, "I", , , ForDate)
                    End If
                    If (Session("UserTypeId") = "41") Then
                        objCommon.SetCommandParameters(sqlcomm, "@BranchId", SqlDbType.Int, 4, "I", , , Val(Session("BranchId") & ""))
                    End If
                    If Trim(Hid_Intids.Value) <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@DealSlipIds", SqlDbType.VarChar, 1000, "I", , , Hid_Intids.Value)
                    End If
                    If Val(Request.QueryString("CustomerId") & "") <> 0 Then
                        objCommon.SetCommandParameters(sqlcomm, "@Customerid", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("Customerid") & ""))
                    End If

                    If Trim(Session("CustomerIds")) <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@CustomerIds", SqlDbType.VarChar, 4000, "I", , , Trim(Session("CustomerIds")))
                    End If
                    If Trim(Session("Brokerids")) <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@Brokerids", SqlDbType.VarChar, 4000, "I", , , Trim(Session("Brokerids")))
                    End If
                    If Trim(Session("ADDBrokerids")) <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@Brokerid", SqlDbType.VarChar, 4000, "I", , , Trim(Session("ADDBrokerids")))
                    End If
                    If Trim(Session("ADDCustids")) <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@CustomerId", SqlDbType.VarChar, 4000, "I", , , Trim(Session("ADDCustids")))
                    End If
                    If Trim(Request.QueryString("DateTypeFlag") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@DateTypeFlag", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("DateTypeFlag") & ""))
                    End If

                    If Trim(Request.QueryString("DateMonthFlag") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@DateMonthFlag", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("DateMonthFlag") & ""))
                    End If

                    If Trim(Request.QueryString("MonthText") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@MonthText", SqlDbType.VarChar, 10, "I", , , Trim(Request.QueryString("MonthText") & ""))
                    End If

                    If Trim(Request.QueryString("Yeartext1") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@Yeartext1", SqlDbType.VarChar, 10, "I", , , Trim(Request.QueryString("Yeartext1") & ""))
                    End If
                    If Trim(Request.QueryString("TransType") & "") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@TransType", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("TransType") & ""))
                    End If

                    If Request.QueryString("Exchangeid") IsNot Nothing Then
                        objCommon.SetCommandParameters(sqlcomm, "@Exchangeid", SqlDbType.Int, 1, "I", , , Val(Request.QueryString("Exchangeid") & ""))
                    End If
                    'If Trim(Request.QueryString("DealTransType") & "") <> "" Then
                    '    objCommon.SetCommandParameters(sqlcomm, "@DealTransType", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("DealTransType") & ""))
                    'End If
                    If Val(Request.QueryString("DealSlipId") & "") <> 0 Then
                        objCommon.SetCommandParameters(sqlcomm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("DealSlipId") & ""))
                    End If
                    If Session("strWhereClause") <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@Cond", SqlDbType.VarChar, 1000, "I", , , Session("strWhereClause"))
                    End If
                    If Request.QueryString("SecurityId") IsNot Nothing Then
                        objCommon.SetCommandParameters(sqlcomm, "@SecurityId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("SecurityId") & ""))
                    End If
                    If Request.QueryString("DealType") IsNot Nothing Then
                        objCommon.SetCommandParameters(sqlcomm, "@DealType", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("DealType") & ""))
                    End If
                    objCommon.SetCommandParameters(sqlcomm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                    objCommon.SetCommandParameters(sqlcomm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                    If Trim(Request.QueryString("Rpt") & "") = "PendingDematDelivery" Or Trim(Request.QueryString("Rpt") & "") = "MismatchedDMATDelivery" Or Trim(Request.QueryString("Rpt") & "") = "mismatchedPurchase" Or Trim(Request.QueryString("Rpt") & "") = "InflowOutFlowPayment" Or Trim(Request.QueryString("Rpt") & "") = "InflowOutflowSecurity" Or Trim(Request.QueryString("Rpt") & "") = "MISGeneralSettlementdt" Or Trim(Request.QueryString("Rpt") & "") = "ClientwiseTransP" _
                    Or Trim(Request.QueryString("Rpt") & "") = "SecwiseTransP" Or Trim(Request.QueryString("Rpt") & "") = "MISDetailTrading" Or Trim(Request.QueryString("Rpt") & "") = "ExchangewiseTrans" Or Trim(Request.QueryString("Rpt") & "") = "StaffwiseTrans" Or Trim(Request.QueryString("Rpt") & "") = "CategorywiseTrans" Or Trim(Request.QueryString("Rpt") & "") = "FinancialDeliveryDetails" Or Trim(Request.QueryString("Rpt") & "") = "AccountAccuredInterest" Or Trim(Request.QueryString("Rpt") & "") = "AccountPurchase" Or Trim(Request.QueryString("Rpt") & "") = "ClientwiseTrans" Or _
                    Trim(Request.QueryString("Rpt") & "") = "SecuritywiseTrans" Or Trim(Request.QueryString("Rpt") & "") = "AccountProfit" Or Trim(Request.QueryString("Rpt") & "") = "AccountSell" Or Trim(Request.QueryString("Rpt") & "") = "CancelledDeals" Or _
                    Trim(Request.QueryString("Rpt") & "") = "DailyPurchaseSale" Or Trim(Request.QueryString("Rpt") & "") = "InflowOutflowBnk" Or Trim(Request.QueryString("Rpt") & "") = "ViewDealStockTrad" _
                   Or Trim(Request.QueryString("Rpt") & "") = "StaffwiseTransP" Or Trim(Request.QueryString("Rpt") & "") = "RetailMIS" Or Trim(Request.QueryString("Rpt") & "") = "CategorywiseTransP" Then
                        objCommon.SetCommandParameters(sqlcomm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId") & ""))
                    End If
                    If Trim(Request.QueryString("Rpt") & "") = "InflowOutFlowPayment" Or Trim(Request.QueryString("Rpt") & "") = "InflowOutflowSecurity" Or Trim(Request.QueryString("Rpt") & "") = "MISGeneralSettlementdt" Or Trim(Request.QueryString("Rpt") & "") = "CancelledDeals" Or Trim(Request.QueryString("Rpt") & "") = "ClientwiseTransP" _
                   Or Trim(Request.QueryString("Rpt") & "") = "SecwiseTransP" Or Trim(Request.QueryString("Rpt") & "") = "MISDetailTrading" Or Trim(Request.QueryString("Rpt") & "") = "ExchangewiseTrans" Or Trim(Request.QueryString("Rpt") & "") = "StaffwiseTrans" Or Trim(Request.QueryString("Rpt") & "") = "CategorywiseTrans" Or Trim(Request.QueryString("Rpt") & "") = "FinancialDeliveryDetails" Or Trim(Request.QueryString("Rpt") & "") = "ClientwiseTrans" Or
                   Trim(Request.QueryString("Rpt") & "") = "SecuritywiseTrans" Or
                   Trim(Request.QueryString("Rpt") & "") = "DailyPurchaseSale" Or Trim(Request.QueryString("Rpt") & "") = "InflowOutflowBnk" _
                  Or Trim(Request.QueryString("Rpt") & "") = "StaffwiseTransP" Or Trim(Request.QueryString("Rpt") & "") = "CategorywiseTransP" Then
                        objCommon.SetCommandParameters(sqlcomm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId") & ""))
                    End If
                    If Request.QueryString("CheckAll") IsNot Nothing Then
                        objCommon.SetCommandParameters(sqlcomm, "@CheckAll", SqlDbType.VarChar, 10, "I", , , Trim(Request.QueryString("CheckAll") & ""))
                    End If
                    If Request.QueryString("BrokFlag") IsNot Nothing Then
                        objCommon.SetCommandParameters(sqlcomm, "@BrokFlag", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("BrokFlag") & ""))
                    End If
                    If Trim(Request.QueryString("Rpt") & "") = "RTGSRepts" Then
                        objCommon.SetCommandParameters(sqlcomm, "@ModeOFPayment", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("PayMode") & ""))
                    End If
                    If Trim(Request.QueryString("Rpt") & "") = "NSDLTONSDL" Or Trim(Request.QueryString("Rpt") & "") = "NSDLTOCSDL" Or Trim(Request.QueryString("Rpt") & "") = "NSDLIndusInd" Then
                        If Trim(Request.QueryString("DematRptType") & "") IsNot Nothing Then
                            objCommon.SetCommandParameters(sqlcomm, "@DematRptType", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("DematRptType") & ""))
                        End If
                    End If
                    If Trim(Request.QueryString("Rpt") & "") = "WDMTransRepts" Then
                        If Trim(Request.QueryString("WDMTransFlag") & "") = "K" Or Trim(Request.QueryString("WDMTransFlag") & "") = "L" Or Trim(Request.QueryString("WDMTransFlag") & "") = "O" Then
                            objCommon.SetCommandParameters(sqlcomm, "@WDMTransFlag", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("WDMTransFlag") & ""))
                        End If
                    End If
                    If Trim(Request.QueryString("Rpt") & "") = "ConsolidatedRpt" Then
                        objCommon.SetCommandParameters(sqlcomm, "@DealType", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("ConsoDealType") & ""))
                    End If
                    If Trim(Request.QueryString("Rpt") & "") = "RetailAudit" Then
                        objCommon.SetCommandParameters(sqlcomm, "@AuditType", SqlDbType.Char, 1, "I", , , Trim(Request.QueryString("AuditType") & ""))
                        objCommon.SetCommandParameters(sqlcomm, "@AuditId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("AuditId") & ""))
                    End If

                    If Trim(Request.QueryString("Rpt") & "") = "ViewDealStockTrad" _
                    Or Trim(Request.QueryString("Rpt") & "") = "ViewDealStockFinNor" Or Trim(Request.QueryString("Rpt") & "") = "ViewDealStockFin" Then
                        objCommon.SetCommandParameters(sqlcomm, "@SecurityName", SqlDbType.VarChar, 500, "I", , , Trim(Request.QueryString("SecurityName") & ""))
                        objCommon.SetCommandParameters(sqlcomm, "@intFlag", SqlDbType.Int, 4, "O")
                        objCommon.SetCommandParameters(sqlcomm, "@Interest", SqlDbType.Decimal, 9, "I", , , Val(Request.QueryString("Interest") & ""))
                    End If


                End If
                .ExecuteNonQuery()
                sqlda.SelectCommand = sqlcomm
                sqlda.Fill(dtfill)
            End With
            Return dtfill

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetReportTable", "Error in GetReportTable", "", ex)
            Response.Write(ex.Message)
        Finally
            ' If (sqlconn.State = ConnectionState.Open) Then sqlconn.Close()
        End Try
    End Function
    Protected Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Try
            Dim strHtml As String
            BindReport(True)

            'rptDoc = Nothing
            strHtml = "CallPrint('" & CrystalReportViewer1.ClientID & "')"
            Page.ClientScript.RegisterStartupScript(Me.GetType, "open", strHtml, True)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_Print_Click", "Error in btn_Print_Click", "", ex)
            Response.Write(ex.Message)
        Finally
            rptDoc.Dispose()
        End Try
    End Sub

    Protected Sub btn_Back_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Back.Click
        Try
            If Trim(Request.QueryString("Rpt") & "") = "NSDLTONSDL" Or Trim(Request.QueryString("Rpt") & "") = "NSDLTOCSDL" Or Trim(Request.QueryString("Rpt") & "") = "NSDLIndusInd" Then
                Response.Redirect("DematSlipGeneration.aspx?Rpt=" & Trim(Request.QueryString("Rpt") & ""), False)
            ElseIf Trim(Request.QueryString("Rpt") & "") = "RetailAudit" Then
                Response.Redirect("AuditReportSelection.aspx?Rpt=" & Trim(Request.QueryString("Rpt") & ""), False)
            ElseIf Trim(Request.QueryString("Rpt") & "") = "RTGSRepts" Then
                Response.Redirect("RTGS_ReportSelection.aspx?Rpt=" & Trim(Request.QueryString("Rpt") & ""), False)
            ElseIf Trim(Request.QueryString("RptForm") & "") = "RetailDebitForm" Then
                Response.Redirect("RetailDebitNote.aspx?RptForm=" & Trim(Request.QueryString("RptForm") & ""), False)
            ElseIf Trim(Request.QueryString("Rpt") & "") = "ViewDealStockTrad" _
                Or Trim(Request.QueryString("Rpt") & "") = "ViewDealStockFinNor" Or Trim(Request.QueryString("Rpt") & "") = "ViewDealStockFin" Then
                Response.Redirect("ViewDealStock.aspx?Rpt=" & Trim(Request.QueryString("Rpt") & ""), False)
            ElseIf Trim(Request.QueryString("Rpt") & "") = "DealStockGraph" Then
                Response.Redirect("GraphReportSelection.aspx?Rpt=" & Trim(Request.QueryString("Rpt") & ""), False)
            ElseIf Trim(Request.QueryString("Rpt") & "") = "LineGraph" Then
                Response.Redirect("GraphReportSelection.aspx?Rpt=" & Trim(Request.QueryString("Rpt") & ""), False)
            Else
                Response.Redirect("ReportSelection.aspx?Rpt=" & Trim(Request.QueryString("Rpt") & ""), False)
            End If
            ''Response.Redirect("ReportSelection.aspx?Rpt=" & Trim(Request.QueryString("Rpt") & ""), False)

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_Back_Click", "Error in btn_Back_Click", "", ex)
            Response.Write(ex.Message)
        End Try

    End Sub

    Protected Sub btn_Export_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Export.Click
        Try
            Dim rptDoc As ReportDocument
            GetReportName()
            rptDoc = BindReport()
            ExportReport(rptDoc)
            rptDoc = Nothing
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_Export_Click", "Error in btn_Export_Click", "", ex)
            Response.Write(ex.Message)
        Finally
            'rptDoc.Dispose()
        End Try
    End Sub
    Private Sub ExportReport(ByVal crReport As ReportDocument)
        'declare a memorystream object that will hold out output 
        Dim oStream As MemoryStream
        'here's the instance of a valid report, one which we have already Load(ed) 
        'Dim crReport = New ReportDocument()
        '*remember that a valid crystal report has to be loaded before you run this code* 
        Dim strReptName As String = Trim(Request.QueryString("Rpt") & "")
        'clear the response and set Buffer to true 
        Response.Clear()
        Response.ClearHeaders()
        Response.Buffer = True
        Select Case cbo_Export.SelectedItem.Value
            Case "1"
                ' ...Portable Document (PDF)
                'oStream = DirectCast(crReport.ExportToStream(ExportFormatType.PortableDocFormat), MemoryStream)
                'Response.AddHeader("content-disposition", "attachment;filename=Report.pdf")
                'Response.ContentType = "application/pdf"
                'in case you want to export it as an attachment use the line below 
                ' 
                crReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, strReptName)
                '*
                Exit Select
            Case "2"
                ' ...MS Word (DOC) 
                'oStream = DirectCast(crReport.ExportToStream(ExportFormatType.WordForWindows), MemoryStream)
                'Response.AddHeader("content-disposition", "attachment;filename=Report.doc")
                'Response.ContentType = "application/doc"
                crReport.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, strReptName)
                Exit Select
            Case "3"
                ' ...MS Excel (XLS) 
                'oStream = DirectCast(crReport.ExportToStream(ExportFormatType.Excel), MemoryStream)
                'oStream = DirectCast(crReport.ExportToStream(ExportFormatType.ExcelRecord), MemoryStream)
                'Response.AddHeader("content-disposition", "attachment;filename=Report.xls")
                'Response.ContentType = "application/vnd.ms-excel"
                crReport.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, True, strReptName)
                Exit Select
            Case "4"
                ' ...Rich Text (RTF) 
                'oStream = DirectCast(crReport.ExportToStream(ExportFormatType.RichText), MemoryStream)
                'Response.AddHeader("content-disposition", "attachment;filename=Report.rtf")
                'Response.ContentType = "application/rtf"
                crReport.ExportToHttpResponse(ExportFormatType.RichText, Response, True, strReptName)
                Exit Select
        End Select
        Try
            'write report to the Response stream 
            'Response.BinaryWrite(oStream.ToArray())
            'Response.TransmitFile(Server.MapPath("~/ExportFile/Report.doc"))
            Response.Flush()
            Response.End()
        Catch ex As Exception
            'Response.Write(ex.Message)
            'labelErrors.Text = "ERROR: " + Server.HtmlEncode(ex.Message.ToString())
        Finally
            'clear stream 
            'oStream.Flush()
            'oStream.Close()
            'oStream.Dispose()
        End Try
    End Sub
    Protected Sub btn_Mail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Mail.Click
        Try
            Dim strFileSave As String
            Dim objCommonFSO As Object
            strFileSave = ConfigurationManager.AppSettings("MailPath") & "\"
            objCommonFSO = Server.CreateObject("Scripting.FileSystemObject")
            If objCommonFSO.FileExists(strFileSave & "Report.doc") = True Then
                System.IO.File.Delete(strFileSave & "Report.doc")
            End If
            Dim rptDoc As ReportDocument
            GetReportName()
            rptDoc = BindReport()
            rptDoc.ExportToDisk(ExportFormatType.WordForWindows, ConfigurationManager.AppSettings("AttachmentPath") & "\Report.doc")
            rptDoc = Nothing
            Dim strHtml As String
            strHtml = "Mail()"
            Page.ClientScript.RegisterStartupScript(Me.GetType, "open", strHtml, True)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_Mail_Click", "Error in btn_Mail_Click", "", ex)
            Response.Write(ex.Message)
        End Try

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            If rptDoc IsNot Nothing Then
                rptDoc.Close()
                rptDoc.Dispose()
                GC.Collect()
                CloseConn()
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Page_Unload", "Error in Page_Unload", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
