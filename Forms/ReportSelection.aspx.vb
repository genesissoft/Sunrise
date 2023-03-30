Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports RKLib.ExportData


Partial Class Forms_ReportSelection
    Inherits System.Web.UI.Page
    Dim sqlconn As SqlConnection
    Dim objCommon As New clsCommonFuns

    Dim date1 As DateTime
    Dim date2 As DateTime
    Dim Fvinlacs As Double = 0
    Dim Settamt As Double = 0
    Dim Brokerage As Double = 0
    Dim chekbox As CheckBox
    Dim oXL As Excel.Application
    Dim rptDoc As New ReportDocument
    Dim oSheet As Excel.Worksheet
    Dim strFilePath As String
    Dim dtRpt As DataTable
    Dim ScriptOpenModalDialog As String = "javascript:OpenModalDialog('{0}','{1}','{2}');"
    Dim objMIS As New MISReports

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Val(Session("UserId") & "") = 0 Then
            Response.Redirect("Login.aspx", False)
            Exit Sub
        End If
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500

        If IsPostBack = False Then
            Hid_ReportType.Value = Trim(Request.QueryString("Rpt") & "")

        End If
        'srh_Customer.SelectLinkButton.Enabled = True
        'srh_Security.SelectLinkButton.Enabled = True
        'srh_Staff.SelectLinkButton.Enabled = True
        'srh_category.SelectLinkButton.Enabled = True
        'srh_exchange.SelectLinkButton.Enabled = True
        'srh_DealSlipNo.SelectLinkButton.Enabled = True

        OpenConn()
        'Srh_NameOFClient.Columns.Add("CustomerName")
        'Srh_NameOFClient.Columns.Add("CustomerCity")
        'Srh_NameOFClient.Columns.Add("CustomerPhone")
        'Srh_NameOFClient.Columns.Add("CustomerId")

        'srh_WDMSecurity.Columns.Add("SecurityName")
        'srh_WDMSecurity.Columns.Add("SecurityId")
        Try

            If IsPostBack = False Then
                Page.SetFocus(btn_Print)
                Session("CustomerIds") = ""
                Session("DebitRefNo") = ""
                Session("DebitRefNoNormal") = ""
                Session("AdvisoryRefNo") = ""
                Session("RetailDebitRefNo") = ""
                Session("ADDCustids") = ""

                txt_FromDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
                txt_FromDate.Attributes.Add("onkeypress", "OnlyDate();")
                txt_Intcost.Attributes.Add("onkeypress", "OnlyDecimal();")
                txt_FromDate.Attributes.Add("onblur", "CheckDate(this,false);")


                txt_ToDate.Attributes.Add("onkeypress", "OnlyDate();")
                txt_ToDate.Attributes.Add("onblur", "CheckDate(this,false);")
                txt_ToDate.Text = Format(DateAdd(DateInterval.Day, 1, Today), "dd/MM/yyyy")
                Hid_ReportType.Value = Trim(Request.QueryString("Rpt") & "")
                cbo_Months.SelectedValue = Month(Date.Now)
                cbo_Year.SelectedValue = Year(Date.Now)

                If Hid_ReportType.Value = "CustomerInfo" Then
                    row_customer.Visible = True
                    row_customer2.Visible = True
                    row_FromDate.Visible = False
                    row_ToDate.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_Intcost.Visible = False
                Else
                    row_FromDate.Visible = True
                    row_ToDate.Visible = True
                End If


                If Hid_ReportType.Value = "DebitNote" Then
                    row_FromDate.Visible = True
                    row_ToDate.Visible = True
                    row_customer.Visible = False
                    row_customer2.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    Tr_NameOFClient.Visible = False
                    lbl_According.Visible = False
                    row_Month.Visible = False
                    lbl_print.Visible = False
                    rdo_Selection.Visible = False
                    rdo_DateType.Visible = False
                    tr2.Visible = False
                    tr_DebitRefNo.Visible = True
                    tr_Mis.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_DealTransType.Visible = False
                    Session("CustomerIds") = ""
                    Session("DebitRefNo") = ""
                    Session("DebitRefNoNormal") = ""
                    Session("AdvisoryRefNo") = ""

                    rdo_MisReport.Visible = False
                    tr_Mis.Visible = False

                    row_Intcost.Visible = False

                    Session("CustomerIds") = ""
                    Session("DebitRefNo") = ""
                    Session("DebitRefNoNormal") = ""
                    Session("AdvisoryRefNo") = ""


                    row_Intcost.Visible = False


                ElseIf Hid_ReportType.Value = "ClientWiseMonthly" Then
                    row_customer.Visible = False
                    row_customer2.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    Tr_NameOFClient.Visible = False
                    lbl_According.Visible = True
                    row_Month.Visible = True
                    lbl_print.Visible = True
                    rdo_Selection.Visible = True
                    rdo_DateType.Visible = True
                    row_Intcost.Visible = False

                    'tr2.Visible = True

                ElseIf Hid_ReportType.Value = "StampDutyDetail" Or Hid_ReportType.Value = "StampDutySummary" Then
                    row_customer.Visible = False
                    row_customer2.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    rdo_Selection.Visible = True
                    row_Month.Visible = True
                    lbl_print.Visible = True
                    row_customer.Visible = False
                    row_customer2.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    Tr_NameOFClient.Visible = False
                    rdo_DateType.Visible = False
                    lbl_According.Visible = False
                    row_Intcost.Visible = False
                ElseIf Hid_ReportType.Value = "AdvisoryReport" Or Hid_ReportType.Value = "AdvisoryLatterReport" Then
                    row_FromDate.Visible = True
                    row_ToDate.Visible = True
                    row_customer.Visible = False
                    row_customer2.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    Tr_NameOFClient.Visible = False
                    lbl_According.Visible = False
                    row_Month.Visible = False
                    lbl_print.Visible = False
                    rdo_Selection.Visible = False
                    rdo_DateType.Visible = False
                    tr2.Visible = False
                    tr_DebitRefNo.Visible = False
                    tr_AdvisoryRefNo.Visible = True
                    row_Intcost.Visible = False

                    Session("CustomerIds") = ""
                    Session("DebitRefNo") = ""
                    Session("DebitRefNoNormal") = ""
                    Session("AdvisoryRefNo") = ""
                ElseIf Hid_ReportType.Value = "RetailDebitRpt" Then

                    row_customer.Visible = False
                    row_customer2.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    Tr_NameOFClient.Visible = False
                    lbl_According.Visible = False
                    row_Month.Visible = False
                    lbl_print.Visible = False
                    rdo_Selection.Visible = False
                    rdo_DateType.Visible = False
                    tr2.Visible = False
                    tr_DebitRefNo.Visible = False
                    row_RetailDebitNote.Visible = True
                    row_Intcost.Visible = False
                    cbo_DealTransType.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_RetailDebitType.Visible = False
                ElseIf Hid_ReportType.Value = "RetailCreditRpt" Then
                    row_FromDate.Visible = False
                    row_ToDate.Visible = False
                    row_customer.Visible = False
                    row_customer2.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    Tr_NameOFClient.Visible = False
                    lbl_According.Visible = False
                    row_Month.Visible = False
                    lbl_print.Visible = False
                    rdo_Selection.Visible = False
                    rdo_DateType.Visible = False
                    tr2.Visible = False
                    tr_DebitRefNo.Visible = False
                    row_RetailCreditRef.Visible = True
                    row_Intcost.Visible = False
                    cbo_DealTransType.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_RetailCreditType.Visible = False
                ElseIf Hid_ReportType.Value = "Consolidated" Or Hid_ReportType.Value = "ConsolidatedRpt" Or Hid_ReportType.Value = "ClientBrokSummary" Or Hid_ReportType.Value = "SecTypSumm" Then

                    Tr_NameOFClient.Visible = False
                    lbl_According.Visible = False
                    row_Month.Visible = False
                    lbl_print.Visible = False
                    rdo_Selection.Visible = False
                    rdo_DateType.Visible = False
                    row_DealType.Visible = True
                    row_customer.Visible = False
                    row_customer2.Visible = False
                    row_FromDate.Visible = True
                    row_ToDate.Visible = True
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_Intcost.Visible = False
                    row_DealType.Visible = False
                    row_WDMSecurity.Visible = True
                ElseIf Hid_ReportType.Value = "WDMTransRepts" Then
                    row_FromDate.Visible = True
                    row_ToDate.Visible = True
                    rdo_MisReport.Visible = False
                    tr_Mis.Visible = False
                    cbo_DealTransType.Visible = False
                    row_DealTransType.Visible = False
                    row_customer.Visible = False
                    tr2.Visible = False
                    row_Intcost.Visible = False
                    Dealtrns.Visible = False
                    row_WDMTransRepts.Visible = True
                    row_DealTransType.Visible = False
                Else
                    row_customer.Visible = False
                    row_customer2.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    lbl_According.Visible = False
                    row_Month.Visible = False
                    lbl_print.Visible = False
                    Tr_NameOFClient.Visible = False
                    rdo_Selection.Visible = False
                    rdo_DateType.Visible = False
                    row_customer.Visible = False
                    row_customer2.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_Month.Visible = False
                    lbl_print.Visible = False
                    Tr_NameOFClient.Visible = False
                    rdo_DateType.Visible = False
                    tr_Broker.Visible = False
                    row_WDMTransRepts.Visible = False
                    'row_Intcost.Visible = False

                End If

                If Hid_ReportType.Value = "DealwiseBrokerage" Then
                    row_Staff.Visible = True
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_Intcost.Visible = False
                    row_Staff.Visible = True
                    tr_Broker.Visible = True

                ElseIf Hid_ReportType.Value = "StaffwiseTrans" Or Hid_ReportType.Value = "StaffwiseTransP" Then
                    row_Staff.Visible = True
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_Intcost.Visible = False
                    row_Security.Visible = True
                    row_customer.Visible = True
                    row_customer2.Visible = False
                ElseIf Hid_ReportType.Value = "BrokerWiseTrans" Then
                    row_Staff.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_Intcost.Visible = False
                    row_Security.Visible = True
                    row_customer.Visible = False
                    row_customer2.Visible = False
                    tr_Broker.Visible = True

                ElseIf Hid_ReportType.Value = "SecuritywiseTrans" Or Hid_ReportType.Value = "SecwiseTrans" Or Hid_ReportType.Value = "SecwiseTransP" Then
                    row_Security.Visible = True
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_customer.Visible = True
                    row_customer2.Visible = True
                    row_Intcost.Visible = False
                ElseIf Hid_ReportType.Value = "CategorywiseTrans" Or Hid_ReportType.Value = "CategorywiseTransP" Then
                    row_Category.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_Security.Visible = True
                    row_CustomerType.Visible = False
                    row_SecurityType.Visible = True
                    row_Intcost.Visible = False
                ElseIf Hid_ReportType.Value = "ExchangewiseTrans" Then
                    row_Exchangewise.Visible = True
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_Intcost.Visible = False
                ElseIf Hid_ReportType.Value = "InflowOutFlowPayment" Then
                    row_InflowOutflow.Visible = False
                    row_InflowOutflowPayment.Visible = True
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_FromDate.Visible = True

                    row_Intcost.Visible = False
                ElseIf Hid_ReportType.Value = "InflowOutflowSecurity" Then
                    row_InflowOutflow.Visible = True
                    row_InflowOutflowPayment.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_FromDate.Visible = True

                    row_Intcost.Visible = False
                ElseIf Hid_ReportType.Value = "FinancialDelivery" Or Hid_ReportType.Value = "FinancialDeliveryDetails" Then
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_Intcost.Visible = False
                ElseIf Hid_ReportType.Value = "BrokeragePaidRecd" Or Hid_ReportType.Value = "ClientwiseBrokerage" Or Hid_ReportType.Value = "DealwiseBrokerage" Or Hid_ReportType.Value = "DetailClientBrokerage" Then
                    tr_Broker.Visible = True
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_Intcost.Visible = False
                ElseIf Hid_ReportType.Value = "WDMSecuritywiseTrans" Then
                    row_Security.Visible = False
                    row_DealTransType.Visible = False
                    row_DealTranschkAll.Visible = False
                    row_WDMDealType.Visible = True
                    row_customer.Visible = False
                    row_customer2.Visible = False
                    row_Intcost.Visible = False
                    row_WDMSecurity.Visible = True


                End If
            Else
                If rdo_Selection.SelectedValue = "M" Then
                    Getdate()
                End If
            End If

            If Hid_ReportType.Value = "ClientWiseMonthly" Then
                btn_Print.Attributes.Add("onclick", "return  ValidationS();")
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "show", "DateMonthSelection();", True)
                rdo_Selection.Attributes.Add("onclick", "DateMonthSelection();")
            End If

            If Hid_ReportType.Value = "AdvisoryReport" Or Hid_ReportType.Value = "AdvisoryLatterReport" Then
                btn_Print.Attributes.Add("onclick", "return  ValidationS();")
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "show", "DateMonthSelection();", True)
                rdo_Selection.Attributes.Add("onclick", "DateMonthSelection();")
            End If

            If Hid_ReportType.Value = "StampDutyDetail" Or Hid_ReportType.Value = "StampDutySummary" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "show1", "DateMonthSelection();", True)
                rdo_Selection.Attributes.Add("onclick", "DateMonthSelection();")
                TR_exchange.Visible = True
                row_Intcost.Visible = False
            Else
                TR_exchange.Visible = False

            End If

            If Hid_ReportType.Value = "CustomerDocStatus" Then
                row_FromDate.Visible = False
                row_ToDate.Visible = False
                row_Category.Visible = False
                tr2.Visible = True
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False

                row_customer.Visible = False
                row_customer2.Visible = False
                row_Intcost.Visible = False
            End If


            If Hid_ReportType.Value = "MisReport" Then
                row_FromDate.Visible = True
                row_ToDate.Visible = True
                rdo_MisReport.Visible = True
                tr_Mis.Visible = True
                cbo_DealTransType.Visible = False
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
                row_customer.Visible = False
                row_customer2.Visible = False
                tr2.Visible = False
                row_Intcost.Visible = False
                Dealtrns.Visible = False


            End If

            If Hid_ReportType.Value = "ClientwiseBrokerage" Or Hid_ReportType.Value = "CustomerInfo" Or Hid_ReportType.Value = "DetailClientBrokerage" Then
                row_customer.Visible = True
                row_customer2.Visible = True
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
                row_Intcost.Visible = False
            End If
            If Hid_ReportType.Value = "ClientwiseTrans" Or Hid_ReportType.Value = "ClientwiseTransP" Or Hid_ReportType.Value = "WDMClientwiseTrans" Then
                row_Security.Visible = True
                row_customer.Visible = True
                row_customer2.Visible = True
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
                row_Intcost.Visible = False

            End If

            If Hid_ReportType.Value = "AccountProfit" Or Hid_ReportType.Value = "AccountPurchase" Or Hid_ReportType.Value = "AccountAccuredInterest" Or Hid_ReportType.Value = "AccountSell" Or Hid_ReportType.Value = "AccountPurchase" Then
                row_DealSlipNo.Visible = False
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
                row_Intcost.Visible = False
                row_customer.Visible = False
                row_customer2.Visible = False
            End If
            If Hid_ReportType.Value = "DailyPurchaseSale" Or Hid_ReportType.Value = "RetailMIS" Then
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False

                row_Intcost.Visible = False
            End If




            If Hid_ReportType.Value = "InflowOutflow" Or Hid_ReportType.Value = "InflowOutflowBnk" Or Hid_ReportType.Value = "MergeDeals" Or Hid_ReportType.Value = "StockRegisterRpt" Or Hid_ReportType.Value = "DailyTransaction" Then
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
            End If
            If Hid_ReportType.Value = "WithWithoutrokerage" Then
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
                row_WithWithoutbrok.Visible = True
            End If
            If Hid_ReportType.Value = "DebitNoteNormal" Or Hid_ReportType.Value = "DebitNoteLetterNormal" Then
                row_FromDate.Visible = False
                row_ToDate.Visible = False
                row_customer.Visible = False
                row_customer2.Visible = False
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
                Tr_NameOFClient.Visible = False
                lbl_According.Visible = False
                row_Month.Visible = False
                lbl_print.Visible = False
                rdo_Selection.Visible = False
                rdo_DateType.Visible = False
                tr2.Visible = False
                tr_DebitRefNoNormal.Visible = True
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
                row_Intcost.Visible = False
                Session("CustomerIds") = ""
                Session("DebitRefNo") = ""
                Session("DebitRefNoNormal") = ""
                Session("AdvisoryRefNo") = ""
                Session("RetailDebitRefNo") = ""
            End If
            If Hid_ReportType.Value = "CustomerwiseBrokerage" Then
                row_FromDate.Visible = True
                row_ToDate.Visible = True
                row_customer.Visible = True
                row_customer2.Visible = True
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
                Tr_NameOFClient.Visible = False
                lbl_According.Visible = False
                row_Month.Visible = False
                lbl_print.Visible = False
                rdo_Selection.Visible = False
                rdo_DateType.Visible = False
                tr2.Visible = False
                tr_DebitRefNoNormal.Visible = False
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
                row_Intcost.Visible = False

            End If

            If Hid_ReportType.Value = "DebitNoteDetailNormal" Or Hid_ReportType.Value = "CancelledDeals" Then
                row_FromDate.Visible = True
                row_ToDate.Visible = True
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
                row_Intcost.Visible = False
                row_CustomerType.Visible = False
                row_customer.Visible = False
                row_customer2.Visible = False
            End If
            If Hid_ReportType.Value = "ExchangewiseTrans" Or Hid_ReportType.Value = "StaffwiseTrans" Or Hid_ReportType.Value = "ClientwiseTrans" Or Hid_ReportType.Value = "SecuritywiseTrans" Or Hid_ReportType.Value = "CategorywiseTrans" Or Hid_ReportType.Value = "CategorywiseTransP" Or
             Hid_ReportType.Value = "ClientwiseBrokerage" Or Hid_ReportType.Value = "DealwiseBrokerage" Or Hid_ReportType.Value = "CustomerDocStatus" Or Hid_ReportType.Value = "StaffwiseTransP" Or Hid_ReportType.Value = "DetailClientBrokerage" _
        Or Hid_ReportType.Value = "WDMClientwiseTrans" Or Hid_ReportType.Value = "CustomerCtc" Or Hid_ReportType.Value = "BrokerWiseTrans" Then

                btn_Print.Attributes.Add("onclick", "return  Validation();")

            End If
            If Hid_ReportType.Value = "RetailDebitRpt" Or Hid_ReportType.Value = "RetailCreditRpt" Then
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
            End If
            If Hid_ReportType.Value = "ActivInactiv" Then
                row_ActiveInactive.Visible = True
                row_ActiveInactive2.Visible = True
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
                row_FromDate.Visible = False
                row_ToDate.Visible = False

            End If

            If Hid_ReportType.Value = "MISDetailTrading" Or Hid_ReportType.Value = "MISGeneralSettlementdt" Or Hid_ReportType.Value = "MISGeneralDealdt" Then
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
                row_MISDealTransType.Visible = False
                row_CustomerType.Visible = False
                row_Intcost.Visible = True
                row_CustomerType.Visible = False
                row_customer.Visible = False
                row_customer2.Visible = False
                tr2.Visible = False
                btn_Print.Attributes.Add("onclick", "return  Validationnew();")
            End If
            If Hid_ReportType.Value = "IPDateReport" Then
                row_FromDate.Visible = True
                row_ToDate.Visible = True
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
                row_MISDealTransType.Visible = False
                row_CustomerType.Visible = False
                row_customer2.Visible = False
                row_Intcost.Visible = False
            End If
            If Hid_ReportType.Value = "MISStockExcelRpt" Then
                row_DealTransType.Visible = False
                row_DealTranschkAll.Visible = False
                row_MISDealTransType.Visible = True
                row_CustomerType.Visible = True
                row_Intcost.Visible = True
                btn_Print.Attributes.Add("onclick", "return  Validationnew();")
            End If
            If Hid_ReportType.Value = "MISClosedPosition" Then
                row_FromDate.Visible = False
            End If
            InitializeSelection()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
    Private Sub InitializeSelection()
        Try
            If Hid_ReportType.Value = "DailyPurchaseSale" Then
                Col_Headers.InnerHtml = "Daily Purchase Sale"

            ElseIf Hid_ReportType.Value = "ClientwiseTrans" Then
                Col_Headers.InnerHtml = "Clientwise Transaction"

            ElseIf Hid_ReportType.Value = "SecuritywiseTrans" Then
                Col_Headers.InnerHtml = "Securitywise Transaction"

            ElseIf Hid_ReportType.Value = "CategorywiseTrans" Then
                Col_Headers.InnerHtml = "Categorywise Transaction"

            ElseIf Hid_ReportType.Value = "CategorywiseTransP" Then
                Col_Headers.InnerHtml = "Categorywise Transaction"

            ElseIf Hid_ReportType.Value = "ConsolidatedRpt" Then
                Col_Headers.InnerHtml = "ConsolidatedRpt"

            ElseIf Hid_ReportType.Value = "PendingDematDelivery" Then
                Col_Headers.InnerHtml = "Pending Demat Delivery"

            ElseIf Hid_ReportType.Value = "MismatchedDMATDelivery" Then
                Col_Headers.InnerHtml = "Mismatched DMAT Delivery"

            ElseIf Hid_ReportType.Value = "NSDLTONSDL" Then
                Col_Headers.InnerHtml = "NSDL TO NSDL"

            ElseIf Hid_ReportType.Value = "NSDLTOCSDL" Then
                Col_Headers.InnerHtml = "NSDL TO CSDL"

            ElseIf Hid_ReportType.Value = "mismatchedPurchase" Then
                Col_Headers.InnerHtml = "Mismatched Purchase"


            ElseIf Hid_ReportType.Value = "RetailMIS" Then
                Col_Headers.InnerHtml = "Transaction Report"



            ElseIf Hid_ReportType.Value = "InflowOutFlowPayment" Then
                Col_Headers.InnerHtml = "Inflow Outflow Payment"

            ElseIf Hid_ReportType.Value = "InflowOutflowSecurity" Then
                Col_Headers.InnerHtml = "Inflow Outflow Security"
            ElseIf Hid_ReportType.Value = "InflowOutflow" Then
                Col_Headers.InnerHtml = "Inflow Outflow"

            ElseIf Hid_ReportType.Value = "InflowOutflowBnk" Then
                Col_Headers.InnerHtml = "Inflow Outflow Bankwise"
            ElseIf Hid_ReportType.Value = "DailyTransaction" Then
                Col_Headers.InnerHtml = "Daily Transaction"

            ElseIf Hid_ReportType.Value = "CustomerwiseBrokerage" Then
                Col_Headers.InnerHtml = "Customerwise Brokerage"

            ElseIf Hid_ReportType.Value = "DealwiseBrokerage" Then
                Col_Headers.InnerHtml = "Dealerwise Brokerage"
            ElseIf Hid_ReportType.Value = "StockRegisterRpt" Then
                Col_Headers.InnerHtml = "Stock Register Report"
            ElseIf Hid_ReportType.Value = "ClientwiseBrokerage" Or Hid_ReportType.Value = "DetailClientBrokerage" Then
                Col_Headers.InnerHtml = "Clientwise Brokerage"

            ElseIf Hid_ReportType.Value = "MISGeneralSettlementdt" Then
                Col_Headers.InnerHtml = "Mis General Settlementdate"

            ElseIf Hid_ReportType.Value = "MISGeneralDealdt" Then
                Col_Headers.InnerHtml = "Mis General Dealdate"

            ElseIf Hid_ReportType.Value = "MISStockExcelRpt" Then
                Col_Headers.InnerHtml = "Stock Excel Report"

            ElseIf Hid_ReportType.Value = "MISDetailTrading" Then
                Col_Headers.InnerHtml = "MisDetail"

            ElseIf Hid_ReportType.Value = "DebitNote" Then
                Col_Headers.InnerHtml = "WDM Debit Note"

            ElseIf Hid_ReportType.Value = "DebitNoteNormal" Then
                Col_Headers.InnerHtml = "Debit Note"

            ElseIf Hid_ReportType.Value = "DebitNoteLetterNormal" Then
                Col_Headers.InnerHtml = "Debit Note Letter"

            ElseIf Hid_ReportType.Value = "DebitNoteDetailNormal" Then
                Col_Headers.InnerHtml = "Debit Note Detail"


            ElseIf Hid_ReportType.Value = "ClientWiseMonthly" Then
                Col_Headers.InnerHtml = "Client Wise Monthly"

            ElseIf Hid_ReportType.Value = "TestAdvisoryReport" Then
                Col_Headers.InnerHtml = "Test Advisory Report"

            ElseIf Hid_ReportType.Value = "AdvisoryReport" Then
                Col_Headers.InnerHtml = "Advisory Report"

            ElseIf Hid_ReportType.Value = "AdvisoryLatterReport" Then
                Col_Headers.InnerHtml = "Advisory Letter"

            ElseIf Hid_ReportType.Value = "MisReport" Then
                Col_Headers.InnerHtml = "Mis Report"


            ElseIf Hid_ReportType.Value = "StampDutyDetail" Then
                Col_Headers.InnerHtml = "Stamp Duty Detail"

            ElseIf Hid_ReportType.Value = "StampDutySummary" Then
                Col_Headers.InnerHtml = "Stamp Duty Summary"

            ElseIf Hid_ReportType.Value = "AccountAccuredInterest" Then
                Col_Headers.InnerHtml = "Account Accured Interest"

            ElseIf Hid_ReportType.Value = "AccountPurchase" Then
                Col_Headers.InnerHtml = "Account Purchase"

            ElseIf Hid_ReportType.Value = "AccountSell" Then
                Col_Headers.InnerHtml = "Account Sell"
            ElseIf Hid_ReportType.Value = "AccountProfit" Then
                Col_Headers.InnerHtml = "Account Profit"
            ElseIf Hid_ReportType.Value = "BrokeragePaidRecd" Then
                Col_Headers.InnerHtml = "Brokerage Paid/Received"
            ElseIf Hid_ReportType.Value = "StaffwiseTrans" Or Hid_ReportType.Value = "StaffwiseTransP" Then
                Col_Headers.InnerHtml = "Staffwise Transaction"

            ElseIf Hid_ReportType.Value = "BrokerWiseTrans" Then
                Col_Headers.InnerHtml = "Brokerwise Transaction"
            ElseIf Hid_ReportType.Value = "ExchangewiseTrans" Then
                Col_Headers.InnerHtml = "Exchangewise Transaction"
            ElseIf Hid_ReportType.Value = "CancelledWDMDeals" Then
                Col_Headers.InnerHtml = "Cancelled WDM Deals"

            ElseIf Hid_ReportType.Value = "MergeDeals" Then
                Col_Headers.InnerHtml = "MergeDeal Report"
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub Getdate()
        Try
            Dim dateFrom As DateTime
            Dim dateTo As DateTime
            Dim m As Int16
            Dim Y As Int16

            m = cbo_Months.SelectedValue
            Y = cbo_Year.SelectedValue
            dateFrom = DateSerial(Y, m, 1)
            dateTo = DateSerial(Y, m + 1, 1 - 1)
            txt_FromDate.Text = Format(dateFrom, "dd/MM/yyyy")
            txt_ToDate.Text = Format(dateTo, "dd/MM/yyyy")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    Protected Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Try


            If Hid_ReportType.Value = "MISGeneralDealdt" Then
                dtRpt = GetReportTable("ID_RPT_MISDetailReport")

                If dtRpt IsNot Nothing And dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISDealDateReport(dtRpt, Trim(txt_FromDate.Text), Trim(txt_ToDate.Text), Val(Session("CompId")), txt_Intcost.Text)
                Else
                    Dim strHtml As String
                    strHtml = "alert('Sorry!!! No Records available to show report');"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                    Exit Sub
                End If

            End If

            If Hid_ReportType.Value = "MISClosedPosition" Then
                dtRpt = GetReportTable("ID_RPT_MISClosedPosition")
                Session("ClosedPosition") = dtRpt
                dtRpt = GetReportTable("ID_RPT_PerformanceReport")
                Session("PerformanceReport") = dtRpt
                dtRpt = GetReportTable("ID_FILL_ViewDealStockOpenPosition")
                Session("OpenPosition") = dtRpt


                If dtRpt IsNot Nothing And dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISClosedPositionReport(dtRpt, Trim(txt_FromDate.Text), Session("CompName"))
                Else
                    Dim strHtml As String
                    strHtml = "alert('Sorry!!! No Records available to show report');"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                    Exit Sub
                End If

            End If
            If Hid_ReportType.Value = "RetailMIS" Then
                BuildReportCond()
                rptDoc = BindReportS(, "RetailMIS.rpt", "ID_FILL_RetailMIS")
                If rptDoc Is Nothing Then Exit Sub
                'GenerateTrans()
                ExportReport(rptDoc, ExportFormatType.ExcelRecord, True)
            End If
            If (Hid_ReportType.Value = "MISDetailTrading") Then
                BuildReportCond()
                ConvertToExcel("ID_FILL_MISDetailsTradingRpt")

            ElseIf Hid_ReportType.Value = "ExchangewiseTrans" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_ExchangewiseTrans_Rept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISExchangeWiseTransactionReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "StaffwiseTrans" Or Hid_ReportType.Value = "StaffwiseTransP" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_StaffwiseTrans_Rept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISStaffWiseTransactionReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If

            ElseIf Hid_ReportType.Value = "BrokerWiseTrans" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_BrokeriseTrans_Rept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISBrokerWiseTransactionReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "ClientwiseTrans" Or Hid_ReportType.Value = "ClientwiseTransP" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_ClientwiseTrans_Rept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISClientWiseTransactionReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "SecuritywiseTrans" Or Hid_ReportType.Value = "SecwiseTransP" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_SecuritywiseTrans_Rept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISSecurityWiseTransactionReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "CategorywiseTrans" Or Hid_ReportType.Value = "CategorywiseTransP" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_RPT_CategorywiseTrans")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISCategoryWiseTransactionReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "InflowOutFlowPayment" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_InflowOutFlow_Rept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISInFlowOutFlowPaymentReport(dtRpt, rbl_TransactionPayment.SelectedValue, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "InflowOutflowSecurity" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_InflowOutFlow_Rept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISInFlowOutFlowSecurityReport(dtRpt, rbl_Transaction.SelectedValue, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "InflowOutflowBnk" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_InflwOutFlwBnk_Rpt")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISInFlowOutFlowBankReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "AccountAccuredInterest" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_AccountAccuredInterest_Rept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISAccAccuredIntReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "AccountPurchase" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_AccountPurSell_Rept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISAccountPurchaseReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "AccountSell" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_AccountPurSell_Rept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISAccountSellReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "AccountProfit" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_AccountProfit_Rept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISAccountProfitReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "ClientwiseBrokerage" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_ClientwiseBrokerage_Rept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISClientWiseBrokerageReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "DealwiseBrokerage" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_DealwiseBrokerage_Rept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISDealerwiseBrokerageReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "WithWithoutrokerage" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_BrokeragerRept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISBrokerageReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "DetailClientBrokerage" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_DetailClientwiseBrokerage")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISClientwiseBrokReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "CustomerwiseBrokerage" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_ClientwiseBrokeragerRept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISCustomerwiseBrokReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "DailyPurchaseSale" Then
                BuildReportCond()
                dtRpt = GetReportTable("ID_FILL_DailyPurchaseSell_Rept")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_DailyPurchaseSellReport(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                End If
            ElseIf Hid_ReportType.Value = "DebitNote" Then
                BuildReportCond()
                Hid_PageName.Value = "DebitNote.aspx"
                If rdo_InvDebit.SelectedValue = "D" Then
                    If rdo_PrintAsDebInv.SelectedValue = "D" Then
                        Response.Redirect("ViewReports.aspx?Fromdate=" & txt_FromDate.Text & "&Todate=" & txt_ToDate.Text & "&Rpt=" & Hid_ReportType.Value & "&PrintSTCode=" & chk_STAccCode.SelectedValue & "&DebitInvFlag=" & rdo_InvDebit.SelectedValue & "&PrintAs=" & rdo_PrintAsDebInv.SelectedValue & "&PageName=" & Hid_PageName.Value, False)
                    Else
                        Response.Redirect("ViewReports.aspx?Fromdate=" & txt_FromDate.Text & "&Todate=" & txt_ToDate.Text & "&Rpt=" & Hid_ReportType.Value & "&DebitInvFlag=" & rdo_InvDebit.SelectedValue & "&PrintAddr=" & rdo_WDMInvAddr.SelectedValue & "&PrintAs=" & rdo_PrintAsDebInv.SelectedValue & "&PageName=" & Hid_PageName.Value, False)
                    End If
                End If
            ElseIf Hid_ReportType.Value = "AdvisoryReport" Then
                BuildReportCond()
                Response.Redirect("ViewReports.aspx?&Rpt=" & Hid_ReportType.Value & "&Fromdate=" & txt_FromDate.Text & "&Todate=" & txt_ToDate.Text, False)
            ElseIf Hid_ReportType.Value = "AdvisoryLatterReport" Then
                BuildReportCond()
                Response.Redirect("ViewReports.aspx?&Rpt=" & Hid_ReportType.Value & "&Fromdate=" & txt_FromDate.Text & "&Todate=" & txt_ToDate.Text, False)
            'ElseIf Hid_ReportType.Value = "MisReport" Then
            '    Response.Redirect("ViewReports.aspx?Fromdate=" & txt_FromDate.Text & "&Todate=" & txt_ToDate.Text & "&Rpt=" & Hid_ReportType.Value & "&MISFlag=" & rdo_MisReport.SelectedValue, False)
            ElseIf Hid_ReportType.Value = "WDMTransRepts" Then
                If cbo_WDMTransRepts.SelectedValue = "D" Or cbo_WDMTransRepts.SelectedValue = "V" Or cbo_WDMTransRepts.SelectedValue = "K" Or cbo_WDMTransRepts.SelectedValue = "T" Or cbo_WDMTransRepts.SelectedValue = "R" Or cbo_WDMTransRepts.SelectedValue = "L" Or cbo_WDMTransRepts.SelectedValue = "O" Or cbo_WDMTransRepts.SelectedValue = "W" Then
                    Response.Redirect("ViewReports.aspx?Fromdate=" & txt_FromDate.Text & "&Todate=" & txt_ToDate.Text & "&Rpt=" & Hid_ReportType.Value & "&WDMTransFlag=" & cbo_WDMTransRepts.SelectedValue, False)
                ElseIf cbo_WDMTransRepts.SelectedValue = "S" Then
                    BuildReportCond()
                    Response.Redirect("ViewReports.aspx?Fromdate=" & txt_FromDate.Text & "&Todate=" & txt_ToDate.Text & "&Rpt=" & Hid_ReportType.Value & "&WDMTransFlag=" & cbo_WDMTransRepts.SelectedValue & "&DealType=" & cbo_ConsoDealType.SelectedValue, False)

                End If
            ElseIf Trim(Request.QueryString("Rpt") & "") = "MisReport" Then
                If rdo_MisReport.SelectedValue = "C" Then

                    BuildReportCond()
                    dtRpt = GetReportTable("Id_RPT_MisBroker")
                    If dtRpt.Rows.Count > 0 Then
                        objMIS.ExportToExcel_WDMMisBrokerage(dtRpt, txt_FromDate.Text, txt_ToDate.Text, "MIS_Brokerage")
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                    End If
                ElseIf rdo_MisReport.SelectedValue = "D" Then

                    BuildReportCond()
                    dtRpt = GetReportTable("Id_RPT_MisDealer")
                    If dtRpt.Rows.Count > 0 Then
                        objMIS.ExportToExcel_WDMMisBrokerage(dtRpt, txt_FromDate.Text, txt_ToDate.Text, "MIS_Dealer")
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                    End If
                ElseIf rdo_MisReport.SelectedValue = "A" Then
                    BuildReportCond()
                    dtRpt = GetReportTable("ID_RPT_MISCons")
                    If dtRpt.Rows.Count > 0 Then
                        objMIS.ExportToExcel_WDMMisBrokerage(dtRpt, txt_FromDate.Text, txt_ToDate.Text, "MIS_Consultancy")
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                    End If
                End If
            Else

                Response.Redirect("ViewReports.aspx?Fromdate=" & txt_FromDate.Text & "&Todate=" & txt_ToDate.Text & "&Rpt=" & Hid_ReportType.Value & "&DealTransType=" & cbo_DealTransType.SelectedValue, False)
            End If

            If Hid_ReportType.Value = "RetailMIS" Then
                GenerateTrans()
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function getData() As DataSet
        Try

            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand
            Dim sqlds As New DataSet

            sqlComm.Connection = sqlconn
            sqlComm.CommandType = CommandType.StoredProcedure

            sqlComm.CommandText = "Id_RPT_ConsolidatedReport"

            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(sqlds)
            Return sqlds
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally

        End Try
    End Function

    Private Sub Excel()

        Try
            'ConvertToExcel("JRVReport")
            Dim tw As New System.IO.StringWriter()
            Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            Dim dgGrid As New DataGrid()
            dgGrid.DataSource = getData()
            '   Report Header
            'hw.WriteLine("<b><u><font size='3' align='center'> Consolidate Report </font></u></b>")
            '   Get the HTML for the control.
            dgGrid.HeaderStyle.Font.Bold = True
            dgGrid.DataBind()
            dgGrid.RenderControl(hw)
            '   Write the HTML back to the browser.
            Response.ClearHeaders()
            Response.AddHeader("content-disposition", "attachment;filename=report.xls")
            Response.ContentType = "application/vnd.ms-excel"
            Me.EnableViewState = False
            Response.Write(tw.ToString())
            Response.End()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub ConvertToExcel(ByVal strProcName As String)
        Try
            Dim dt As DataTable
            Dim ds As New DataSet
            Dim tw As New System.IO.StringWriter()
            Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            Dim dgGrid As New DataGrid()
            Response.ClearHeaders()
            dt = GetReportTable(strProcName)
            dgGrid.DataSource = dt
            If dt.Rows.Count > 0 Then
                Dim objExport As New RKLib.ExportData.Export("Web")
                objExport.ExportDetails(dt, Export.ExportFormat.Excel, "Report.xls")
            Else
                Dim strHtml As String
                strHtml = "alert('Sorry!!! No Records available to show report');"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try



    End Sub



    Private Function GetReportTable(ByVal strProcName As String) As DataTable
        Try
            Dim datFrom As Date
            Dim datTo As Date
            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand
            sqlComm.CommandTimeout = 0
            'Dim intColIndex As Int16

            datFrom = objCommon.DateFormat(txt_FromDate.Text)
            datTo = objCommon.DateFormat(txt_ToDate.Text)
            'openconnection()
            With sqlComm
                .Connection = sqlconn
                .CommandType = CommandType.StoredProcedure
                .CommandText = strProcName

                If (Hid_ReportType.Value = "MISStockExcelRpt") Or Hid_ReportType.Value = "MISGeneralDealdt" Or Hid_ReportType.Value = "MISDetailTrading" Or Hid_ReportType.Value = "MISGeneralSettlementdt" Or Hid_ReportType.Value = "MISGeneralDealdt" Then
                    objCommon.SetCommandParameters(sqlComm, "@IntCost", SqlDbType.Decimal, 9, "I", , , Val(objCommon.DecimalFormat(txt_Intcost.Text) & ""))
                    'objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , cbo_MISDealTransType.SelectedValue)
                End If

                If Session("strWhereClause") <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@Cond", SqlDbType.VarChar, 1000, "I", , , Session("strWhereClause"))
                End If

                objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)
                objCommon.SetCommandParameters(sqlComm, "@ToDate", SqlDbType.SmallDateTime, 8, "I", , , datTo)

                If Hid_ReportType.Value = "ExchangewiseTrans" Or Hid_ReportType.Value = "StaffwiseTrans" Or Hid_ReportType.Value = "StaffwiseTransP" Or
                    Hid_ReportType.Value = "StaffwiseTrans" Or Hid_ReportType.Value = "SecuritywiseTrans" Or Hid_ReportType.Value = "SecwiseTransP" Or
                    Hid_ReportType.Value = "CategorywiseTrans" Or Hid_ReportType.Value = "CategorywiseTransP" Or Hid_ReportType.Value = "ClientwiseTrans" Or
                    Hid_ReportType.Value = "ClientwiseTransP" Or Hid_ReportType.Value = "BrokerWiseTrans" Then
                    objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , cbo_DealTransType.SelectedValue)
                    objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId") & ""))
                    objCommon.SetCommandParameters(sqlComm, "@CheckAll", SqlDbType.VarChar, 10, "I", , , srh_Staff.SelectCheckBox.Checked)
                End If
                If Hid_ReportType.Value = "StaffwiseTrans" Then
                    objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId") & ""))
                End If
                If Hid_ReportType.Value = "InflowOutFlowPayment" Then
                    objCommon.SetCommandParameters(sqlComm, "@TransType", SqlDbType.Char, 1, "I", , , rbl_TransactionPayment.SelectedValue)
                    objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , cbo_DealTransType.SelectedValue)
                    objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId") & ""))
                End If
                If Hid_ReportType.Value = "InflowOutflowSecurity" Then
                    objCommon.SetCommandParameters(sqlComm, "@TransType", SqlDbType.Char, 1, "I", , , rbl_Transaction.SelectedValue)
                    objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , cbo_DealTransType.SelectedValue)
                    objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId") & ""))
                End If
                If Hid_ReportType.Value = "InflowOutflowBnk" Or Hid_ReportType.Value = "AccountAccuredInterest" Or Hid_ReportType.Value = "AccountProfit" Or Hid_ReportType.Value = "AccountPurchase" Or Hid_ReportType.Value = "AccountSell" Then
                    objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId") & ""))
                    objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId") & ""))
                End If


                If Hid_ReportType.Value = "ClientwiseBrokerage" Or Hid_ReportType.Value = "DealwiseBrokerage" Or Hid_ReportType.Value = "DetailClientBrokerage" Then
                    objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , cbo_DealTransType.SelectedValue)
                End If
                If Hid_ReportType.Value = "WithWithoutrokerage" Or Hid_ReportType.Value = "CustomerwiseBrokerage" Then
                    objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , cbo_DealTransType.SelectedValue)
                    objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId") & ""))
                    objCommon.SetCommandParameters(sqlComm, "@CheckAll", SqlDbType.VarChar, 10, "I", , , srh_Staff.SelectCheckBox.Checked)
                    objCommon.SetCommandParameters(sqlComm, "@BrokFlag", SqlDbType.Char, 1, "I", , , rdo_WithWithoutbrok.SelectedValue)
                End If
                .ExecuteNonQuery()
            End With
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dtfill)

            Return dtfill
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally

        End Try
    End Function

    Private Sub BuildReportCond()
        Try
            Dim strCond As String = ""
            Dim strRpt As String
            strRpt = Hid_ReportType.Value
            Session("strWhereClause") = ""

            If Hid_ReportType.Value = "CustomerInfo" Then
                strCond += BuildConditionStr(srh_CustomerName.SelectCheckBox, srh_CustomerName.SelectListBox, "CM.CustomerId")
                strCond += BuildConditionStr(srh_CustomerTypeName.SelectCheckBox, srh_CustomerTypeName.SelectListBox, "CTM.CustomerTypeId")

            ElseIf Hid_ReportType.Value = "ClientwiseBrokerage" Or Hid_ReportType.Value = "DetailClientBrokerage" Then
                strCond += BuildConditionStr(srh_CustomerName.SelectCheckBox, srh_CustomerName.SelectListBox, "CM.CustomerId")
                strCond += BuildConditionStr(srh_CustomerTypeName.SelectCheckBox, srh_CustomerTypeName.SelectListBox, "CTM.CustomerTypeId")
                strCond += BuildDSEBrokerStr(srh_Broker.SelectCheckBox, srh_Broker.SelectListBox)
                strCond += BuildDealTransTyp()
            ElseIf Hid_ReportType.Value = "ClientwiseTrans" Or Hid_ReportType.Value = "ClientwiseTransP" Then
                strCond += BuildConditionStr(srh_CustomerName.SelectCheckBox, srh_CustomerName.SelectListBox, "CM.CustomerId")
                strCond += BuildConditionStr(srh_CustomerTypeName.SelectCheckBox, srh_CustomerTypeName.SelectListBox, "CTM.CustomerTypeId")
                strCond += BuildConditionStr(srh_Security.SelectCheckBox, srh_Security.SelectListBox, "S.SecurityId")
                strCond += BuildDealTransTyp()
            ElseIf Hid_ReportType.Value = "WDMClientwiseTrans" Or Hid_ReportType.Value = "CustomerwiseBrokerage" Then
                strCond += BuildConditionStr(srh_CustomerName.SelectCheckBox, srh_CustomerName.SelectListBox, "CM.CustomerId")
                strCond += BuildConditionStr(srh_CustomerTypeName.SelectCheckBox, srh_CustomerTypeName.SelectListBox, "CTM.CustomerTypeId")
                strCond += BuildConditionStr(srh_Security.SelectCheckBox, srh_Security.SelectListBox, "S.SecurityId")
                strCond += BuildConditionStr(srh_SecurityType.SelectCheckBox, srh_SecurityType.SelectListBox, "ST.SecurityTypeId")

            ElseIf Hid_ReportType.Value = "DebitNote" Then

                strCond = BuildDebitRefStr(srh_DebitRefNo.SelectCheckBox, srh_DebitRefNo.SelectListBox)

            ElseIf Hid_ReportType.Value = "DebitNoteNormal" Or Hid_ReportType.Value = "DebitNoteLetterNormal" Then

                strCond = BuildNormalDebitRefStr(srh_DebitRefNoNormal.SelectCheckBox, srh_DebitRefNoNormal.SelectListBox)


            ElseIf Hid_ReportType.Value = "AdvisoryReport" Or Hid_ReportType.Value = "AdvisoryLatterReport" Then

                strCond = BuildAdvStr(Srh_AdvisoryRefNo.SelectCheckBox, Srh_AdvisoryRefNo.SelectListBox)


            ElseIf Hid_ReportType.Value = "RetailDebitRpt" Then
                strCond = BuildRetailDebitRefStr(srh_RetailDebit.SelectCheckBox, srh_RetailDebit.SelectListBox)
            ElseIf Hid_ReportType.Value = "RetailCreditRpt" Then
                strCond = BuildRetailCreditRefStr(srh_RetailCreditRef.SelectCheckBox, srh_RetailCreditRef.SelectListBox)


            ElseIf Hid_ReportType.Value = "BrokeragePaidRecd" Then
                strCond += BuildBrokerStr(srh_Broker.SelectCheckBox, srh_Broker.SelectListBox)
            ElseIf Hid_ReportType.Value = "RetCrmDelearWiseDetail" Or Hid_ReportType.Value = "RetCrmCustWiseDetail" Then
                strCond += BuildConditionStr(srh_Staff.SelectCheckBox, srh_Staff.SelectListBox, "UserId")
                strCond += BuildConditionStr(srh_CustomerName.SelectCheckBox, srh_CustomerName.SelectListBox, "II.CustomerId")
                strCond += BuildConditionStr(srh_CustomerType.SelectCheckBox, srh_CustomerType.SelectListBox, "ITM.CustomerTypeId")
            ElseIf Hid_ReportType.Value = "DealwiseBrokerage" Then

                strCond += BuildConditionStr(srh_Staff.SelectCheckBox, srh_Staff.SelectListBox, "UM.UserId")
                strCond += BuildDSEBrokerStr(srh_Broker.SelectCheckBox, srh_Broker.SelectListBox)
                strCond += BuildDealTransTyp()
            ElseIf Hid_ReportType.Value = "BrokerWiseTrans" Then

                strCond += BuildDSEBrokerStr(srh_Broker.SelectCheckBox, srh_Broker.SelectListBox)
                strCond += BuildDealTransTyp()
            ElseIf Hid_ReportType.Value = "StaffwiseTrans" Or Hid_ReportType.Value = "StaffwiseTransP" Then
                strCond += BuildConditionStr(srh_Staff.SelectCheckBox, srh_Staff.SelectListBox, "UserId")
                strCond += BuildConditionStr(srh_CustomerName.SelectCheckBox, srh_CustomerName.SelectListBox, "CustomerId")
                strCond += BuildConditionStr(srh_CustomerTypeName.SelectCheckBox, srh_CustomerTypeName.SelectListBox, "CT.CustomerTypeId")
                strCond += BuildConditionStr(srh_Security.SelectCheckBox, srh_Security.SelectListBox, "SecurityId")
                strCond += BuildDealTransTyp()
            ElseIf Hid_ReportType.Value = "SecuritywiseTrans" Or Hid_ReportType.Value = "SecwiseTrans" Or Hid_ReportType.Value = "SecwiseTransP" Then
                strCond += BuildConditionStr(srh_Security.SelectCheckBox, srh_Security.SelectListBox, "S.SecurityId")
                strCond += BuildConditionStr(srh_CustomerName.SelectCheckBox, srh_CustomerName.SelectListBox, "C.CustomerId")
                strCond += BuildDealTransTyp()
            ElseIf Hid_ReportType.Value = "ExchangewiseTrans" Then
                strCond += BuildConditionStr(srh_exchange.SelectCheckBox, srh_exchange.SelectListBox, "EM.ExchangeId")
                strCond += BuildDealTransTyp()
            ElseIf Hid_ReportType.Value = "CategorywiseTrans" Or Hid_ReportType.Value = "CategorywiseTransP" Then
                strCond += BuildConditionStr(srh_Security.SelectCheckBox, srh_Security.SelectListBox, "SM.SecurityId")
                strCond += BuildConditionStr(srh_CustomerType.SelectCheckBox, srh_CustomerType.SelectListBox, "CT.CustomerTypeId")
                strCond += BuildConditionStr(srh_SecurityType.SelectCheckBox, srh_SecurityType.SelectListBox, "SM.SecurityTypeId")
                strCond += BuildDealTransTyp()
            ElseIf Hid_ReportType.Value = "CustomerDocStatus" Then
                strCond += BuildConditionStr(srh_Customer.SelectCheckBox, srh_Customer.SelectListBox, "C.CustomerId")
            ElseIf Hid_ReportType.Value = "AccountProfit" Then
                ' strCond += BuildAccountProfitStr(srh_DealSlipNo.SelectCheckBox, srh_DealSlipNo.SelectListBox, "DealSlipID")
                strCond += BuildDealTransTyp()
            ElseIf Hid_ReportType.Value = "MISDetailTrading" Or Hid_ReportType.Value = "MISGeneralSettlementdt" Or Hid_ReportType.Value = "MISGeneralDealdt" Or Hid_ReportType.Value = "MISStockExcelRpt" Then
                strCond += BuildMISStr(srh_CustomerType.SelectCheckBox, srh_CustomerType.SelectListBox)
            ElseIf Hid_ReportType.Value = "CustomerDealsSummary" Then

                strCond += BuildCustomrDealsSumm(srh_CustomerType.SelectCheckBox, srh_CustomerType.SelectListBox, "CT.CustomerTypeId")
                strCond += BuildDealTransTyp()
            ElseIf Hid_ReportType.Value = "CustomerCtc" Then
                strCond += BuildConditionStr(srh_CustomerType.SelectCheckBox, srh_CustomerType.SelectListBox, "CTM.CustomerTypeId")

            ElseIf Hid_ReportType.Value = "PendingDematDelivery" Or Hid_ReportType.Value = "MismatchedDMATDelivery" Or Hid_ReportType.Value = "mismatchedPurchase" Or Hid_ReportType.Value = "FinancialDeliveryDetails" _
            Or Hid_ReportType.Value = "AccountAccuredInterest" Or Hid_ReportType.Value = "AccountPurchase" Or Hid_ReportType.Value = "AccountSell" Or Hid_ReportType.Value = "DailyPurchaseSale" Or Hid_ReportType.Value = "InflowOutflowSecurity" Or Hid_ReportType.Value = "InflowOutFlowPayment" Or Hid_ReportType.Value = "InflowOutflowBnk" Or Hid_ReportType.Value = "RetailMIS" Then
                strCond += BuildDealTransTyp()
                'If Val(Session("UserTypeId")) <> 1 Then
                If Trim(Session("RestrictedAccess") & "") = "Y" Then
                        strCond += strCond & " AND DSE.UserId =" & Val(Session("UserId"))
                    End If

                End If

            Session("strWhereClause") = strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function BuildDBBrokerStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox) As String
        Try
            Dim strCond As String = ""
            Dim strIds As String = ""

            If chkbox.Checked = False Then
                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strIds += lstbox.Items(I).Value & ","
                    End If
                Next
                'strCond = 
                strCond += " AND ( Brokerid IN (" & Mid(strIds, 1, Len(strIds) - 1) & ")"
                strCond += " OR Brokerid IN (" & Mid(strIds, 1, Len(strIds) - 1) & ") ) "
                Session("Brokerid") = Mid(strIds, 1, Len(strIds) - 1)

            End If
            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function BuildAdvStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox) As String
        Try
            Dim strCond As String = ""
            Dim strIds As String = ""

            Session("AdvisoryRefNo") = "0"
            Session("AdvisoryLetterRefNo") = "0"


            If chkbox.Checked = False Then
                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strIds += lstbox.Items(I).Value & ","
                    End If
                Next
                'strCond = 
                strCond += " AND ( Brokerid IN (" & Mid(strIds, 1, Len(strIds) - 1) & ")"
                strCond += " OR Brokerid IN (" & Mid(strIds, 1, Len(strIds) - 1) & ") ) "

                Session("AdvisoryRefNo") = ""
                Session("AdvisoryLetterRefNo") = ""
                Session("AdvisoryRefNo") = Mid(strIds, 1, Len(strIds) - 1)
            End If
            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function BuildBrokerStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox) As String
        Try
            Dim strCond As String = ""
            Dim strIds As String = ""

            If chkbox.Checked = False Then
                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strIds += lstbox.Items(I).Value & ","
                    End If
                Next
                'strCond = 
                Session("Brokerids") = Mid(strIds, 1, Len(strIds) - 1)

            End If
            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function BuildCustomrDealsSumm(ByVal chkbox As CheckBox, ByVal lstbox As ListBox, ByVal strFieldName As String, Optional ByVal chrFlag As Char = "") As String
        Try
            Dim strCond As String = ""
            Dim strIds As String = ""

            If chkbox.Checked = False Then
                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strIds += lstbox.Items(I).Value & ","
                    End If
                Next
                strCond += " AND ( CM.CustomerTypeId IN (" & Mid(strIds, 1, Len(strIds) - 1) & ")"
                strCond += " AND CM1.CustomerTypeId  IN (" & Mid(strIds, 1, Len(strIds) - 1) & ") ) "
            End If
            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function BuildAdviLetterStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox) As String
        Try
            Dim strCond As String = ""
            Dim strIds As String = ""
            Session("AdvisoryRefNo") = "0"

            If chkbox.Checked = False Then
                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strIds += lstbox.Items(I).Value & ","
                    End If
                Next
                'strCond = 

                Session("AdvisoryRefNo") = ""
                Session("AdvisoryLetterRefNo") = ""

                Session("AdvisoryLetterRefNo") = Mid(strIds, 1, Len(strIds) - 1)
            End If
            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function BuildConditionStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox, ByVal strFieldName As String, Optional ByVal chrFlag As Char = "") As String
        Try
            Dim strCond As String = ""
            Dim strOpt As String '= " WHERE "

            If Hid_ReportType.Value = "CustomerInfo" Or Hid_ReportType.Value = "CustomerCtc" Then
                strOpt = " WHERE "
            Else
                strOpt = " AND "
            End If
            If chkbox.Checked = False Then
                If strCond <> "" Then strOpt = " AND "
                If Hid_ReportType.Value = "StaffwiseTrans" Or Hid_ReportType.Value = "StaffwiseTransP" Then
                    'strCond = " IN ("
                    strCond = strOpt & "DSE." & strFieldName & " IN ("
                Else
                    strCond = strOpt & strFieldName & " IN ("
                End If

                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strCond += lstbox.Items(I).Value & ","
                    End If
                Next
                'If Val(Session("Usertypeid")) <> 1 Then
                '    If Hid_ReportType.Value = "PendingDematDelivery" Or Hid_ReportType.Value = "MismatchedDMATDelivery" Or Hid_ReportType.Value = "mismatchedPurchase" Or Hid_ReportType.Value = "FinancialDeliveryDetails" _
                '            Or Hid_ReportType.Value = "AccountAccuredInterest" Or Hid_ReportType.Value = "AccountPurchase" Or Hid_ReportType.Value = "AccountSell" Or Hid_ReportType.Value = "DailyPurchaseSale" Or Hid_ReportType.Value = "InflowOutflowSecurity" Or Hid_ReportType.Value = "InflowOutFlowPayment" Or Hid_ReportType.Value = "RetailMIS" Then
                '        strCond += strCond & " AND DSE.UserId"
                '    End If
                'End If


                strCond = Mid(strCond, 1, Len(strCond) - 1) & ")"
            End If
            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function BuildDBCustStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox) As String
        Try
            Dim strCond As String = ""
            Dim strIds As String = ""

            If chkbox.Checked = False Then
                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strIds += lstbox.Items(I).Value & ","
                    End If
                Next
                'strCond = 
                strCond += " AND ( BuyerCustId IN (" & Mid(strIds, 1, Len(strIds) - 1) & ")"
                strCond += " OR SellerCustId IN (" & Mid(strIds, 1, Len(strIds) - 1) & ") ) "
                Session("CustomerIds") = Mid(strIds, 1, Len(strIds) - 1)
            End If
            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function BuildDSEBrokerStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox) As String
        Try
            Dim strCond As String = ""
            Dim strIds As String = ""

            If chkbox.Checked = False Then
                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strIds += lstbox.Items(I).Value & ","
                    End If
                Next

                strCond += " AND ( BrockPaidTo IN (" & Mid(strIds, 1, Len(strIds) - 1) & ")"
                strCond += " OR BrockRecvForm IN (" & Mid(strIds, 1, Len(strIds) - 1) & ") ) "

            End If
            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function


    Private Function BuildMISStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox) As String
        Try
            Dim strCond As String = ""
            Dim strIds As String = ""

            If chkbox.Checked = False Then
                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strIds += lstbox.Items(I).Value & ","
                    End If
                Next

                strCond += " AND ( CM1.CustomerTypeId IN (" & Mid(strIds, 1, Len(strIds) - 1) & ")"
                strCond += " OR CM2.CustomerTypeId IN (" & Mid(strIds, 1, Len(strIds) - 1) & ") ) "

            End If
            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function BuildDealTransTyp() As String
        Try
            Dim strCond As String = ""
            Dim strIds As String = ""


            For Each lstItem As ListItem In cbo_DealTransType.Items
                If lstItem.Selected = True Then
                    strIds += "'" & lstItem.Value & "'" & ","
                End If
            Next
            If Hid_ReportType.Value = "AccountProfit" Then
                strCond += " AND DSE.DealTransType IN (" & Mid(strIds, 1, Len(strIds) - 1) & ")"
            Else
                strCond += " AND DealTransType IN (" & Mid(strIds, 1, Len(strIds) - 1) & ")"

            End If

            Session("strWherClause") = strCond
            Return strCond

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function BuildDebitRefStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox) As String
        Try
            Dim strCond As String = ""
            Dim strIds As String = ""
            Session("DebitRefNo") = "0"
            If chkbox.Checked = False Then
                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strIds += lstbox.Items(I).Value & ","
                    End If
                Next
                Session("DebitRefNo") = ""
                Session("DebitRefNo") = Mid(strIds, 1, Len(strIds) - 1)
            End If
            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function BuildNormalDebitRefStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox) As String
        Try
            Dim strCond As String = ""
            Dim strIds As String = ""
            Session("DebitRefNoNormal") = "0"
            If chkbox.Checked = False Then
                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strIds += lstbox.Items(I).Value & ","
                    End If
                Next
                Session("DebitRefNoNormal") = ""
                Session("DebitRefNo") = ""
                Session("DebitRefNoNormal") = Mid(strIds, 1, Len(strIds) - 1)
            End If
            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
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
    Private Function BuildRetailCreditRefStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox) As String
        Try
            Dim strCond As String = ""
            Dim strIds As String = ""
            Session("RetCreditRefNo") = "0"
            If chkbox.Checked = False Then
                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strIds += lstbox.Items(I).Value & ","
                    End If
                Next
                Session("RetCreditRefNo") = ""
                Session("RetCreditRefNo") = Mid(strIds, 1, Len(strIds) - 1)
            End If
            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function



    Private Function BuildAccountProfitStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox, ByVal strFieldName As String, Optional ByVal chrFlag As Char = "") As String
        Try
            Dim strCond As String = ""
            Dim strCond1 As String = ""
            Dim strIds As String = ""
            Dim strIds1 As String = ""
            Dim strDealslipNo As String = ""

            'If chkbox.Checked = False Then
            strCond = " AND  PR.PurchaseDealSlipId IN ("
            strCond1 = " AND  PR.SellDealSlipId IN ("
            For I As Int16 = 0 To lstbox.Items.Count - 1
                If lstbox.Items(I).Value <> "" Then

                    strDealslipNo = lstbox.Items(I).Text
                    If Mid(strDealslipNo, 2, Len(strDealslipNo) - 7) = "P" Then
                        strIds += lstbox.Items(I).Value & ","

                    ElseIf Mid(strDealslipNo, 2, Len(strDealslipNo) - 7) = "S" Then
                        strIds1 += lstbox.Items(I).Value & ","

                    End If
                End If
            Next
            If strIds <> "" Then
                strCond = strCond & Mid(strIds, 1, Len(strIds) - 1) & ")"

            End If
            If strIds1 <> "" Then
                strCond1 = strCond1 & Mid(strIds1, 1, Len(strIds1) - 1) & ")"

            End If
            If strIds <> "" And strIds1 = "" Then
                strCond = strCond
            ElseIf strIds1 <> "" And strIds = "" Then
                strCond = strCond1
            Else
                strCond = strCond + strCond1
            End If

            'End If
            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function


    Protected Sub cbo_Months_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Months.SelectedIndexChanged
        Getdate()
    End Sub

    Protected Sub cbo_Year_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Year.SelectedIndexChanged
        Getdate()
    End Sub

    Private Sub GenerateTrans()

        Dim tw As New System.IO.StringWriter()
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        Dim dgGrid As New DataGrid()
        Dim strHead As String
        dgGrid.ShowFooter = True
        If Hid_ReportType.Value = "RetailMIS" Then
            dgGrid.DataSource = GetReportTable("ID_FILL_RetailMIS")
        End If


        AddHandler dgGrid.ItemDataBound, AddressOf GridItemDataBound
        dgGrid.HeaderStyle.Font.Bold = True
        dgGrid.FooterStyle.Font.Bold = True
        dgGrid.DataBind()
        dgGrid.RenderControl(hw)

        Response.ClearHeaders()
        Response.AddHeader("content-disposition", "attachment;filename=Report.xls")
        Response.ContentType = "application/vnd.ms-excel"
        Me.EnableViewState = False
        Response.Write(tw.ToString())
        Response.End()
    End Sub

    Public Sub GridItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Fvinlacs = Fvinlacs + Val(TryCast(e.Item.DataItem, DataRowView).Row.Item("FaceValue").ToString)
            Settamt = Settamt + Val(TryCast(e.Item.DataItem, DataRowView).Row.Item("Profit").ToString)

        End If
        If e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(0).Text = "Total"
            e.Item.Cells(8).Text = Format(Fvinlacs, "################0.00")

        End If
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

            If Hid_ReportType.Value = "StockRegisterRpt" Then
                dtRpt = GetReportTable("ID_FILL_StockRegister")
            End If
            If Hid_ReportType.Value = "CustomerDealsSummary" Then
                dtRpt = GetReportTable("ID_FILL_CustomerDealsSummaryRpt")
            End If
            If Hid_ReportType.Value = "RetailMIS" Then
                dtRpt = GetReportTable("ID_FILL_RetailMIS")
            End If
            If Hid_ReportType.Value = "CustomerCtc" Then
                dtRpt = GetReportTable("ID_FILL_MFReport")
            End If
            If Hid_ReportType.Value = "MISGeneralDealdt" Then
                dtRpt = GetReportTable("ID_RPT_MISCosting")
            End If
            If (dtRpt.Rows.Count = 0) Then
                Dim msg As String = "No Entries Done"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
                Exit Function
            Else

                rptDoc.SetDataSource(dtRpt)
                strReportRange = txt_FromDate.Text & "  TO  " & txt_ToDate.Text & ""
                rptDoc.SummaryInfo.ReportComments = strReportRange

            End If
            Return rptDoc
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function


    Private Sub ExportReport(ByVal crReport As ReportDocument, ByVal type As ExportFormatType,
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
            .AddHeader("content-disposition", "attachment;filename=Report.xls")
            .WriteFile(strFilePath)
            .Flush()
            .End()
        End With
        clsCommonFuns.sqlConn.Close()
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            Session("strWhereClause") = ""
            If sqlconn IsNot Nothing Then
                sqlconn.Dispose()
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub



    Protected Sub rdo_InvDebit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdo_InvDebit.SelectedIndexChanged
        Try
            If rdo_InvDebit.SelectedValue = "I" Then
                row_WDMInvoiceAddr.Visible = True
                row_STAccCode.Visible = False
                rdo_WDMInvAddr.SelectedValue = "N"
            Else
                row_WDMInvoiceAddr.Visible = False
                row_STAccCode.Visible = True
                chk_STAccCode.SelectedValue = "N"
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub rdo_PrintAsDebInv_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdo_PrintAsDebInv.SelectedIndexChanged
        Try
            If rdo_PrintAsDebInv.SelectedValue = "I" Then
                row_WDMInvoiceAddr.Visible = True
                row_STAccCode.Visible = False
                rdo_WDMInvAddr.SelectedValue = "N"
            Else
                row_WDMInvoiceAddr.Visible = False
                row_STAccCode.Visible = True
                chk_STAccCode.SelectedValue = "N"
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub chk_DealTranschkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_DealTranschkAll.CheckedChanged
        Try
            If chk_DealTranschkAll.Checked = True Then
                For Each lstItem As ListItem In cbo_DealTransType.Items
                    lstItem.Selected = True
                Next
            Else
                For Each lstItem As ListItem In cbo_DealTransType.Items
                    lstItem.Selected = False
                Next
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub ExportReport1(ByVal crReport As ReportDocument, ByVal type As ExportFormatType,
                             Optional ByVal blnDiskExport As Boolean = False)
        Response.Clear()
        Response.ClearHeaders()
        Response.Buffer = True
        If blnDiskExport = True Then
            Dim strFilePath As String

            strFilePath = Server.MapPath("").Replace("Forms", "Temp") & "\Report.xls"
            If File.Exists(strFilePath) = True Then
                File.Delete(strFilePath)
            End If
            crReport.ExportToDisk(type, strFilePath)

            'strFilePath = Server.MapPath("").Replace("Forms", "Temp") & "\InvestorWise.xls"
        Else
            crReport.ExportToHttpResponse(type, Response, True, "Report")
        End If

        clsCommonFuns.sqlConn.Close()
    End Sub

End Class