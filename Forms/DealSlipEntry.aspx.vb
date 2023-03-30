Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Imports Newtonsoft.Json
Imports System.Collections.Generic
Partial Class Forms_DealSlipEntry
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim PgName As String = "$DealslipEntry$"
    Dim objUtil As New Util
    Dim dblPepCoupRate As Double
    Dim LastIPDate As Date = Date.MinValue
    Dim sqlConn As SqlConnection
    Dim PurSettDate As Date
    Dim chrIntFlag As Char
    Dim decFaceValue As Decimal
    Dim secFaceValue As Decimal
    Dim decRate As Decimal
    Dim blnNonGovernment As Boolean
    Dim blnRateActual As Boolean
    Dim MatDate() As Date
    Dim MatAmt() As Double
    Dim CoupDate() As Date
    Dim CoupRate() As Double
    Dim intBKDiff As Integer
    Dim datIssue As Date
    Dim datInterest As Date
    Dim datYTM As Date
    Dim dblIntReceivable As Double
    Dim CostMemoFlag As Char
    Dim Costdealconf As String
    Dim PurDealslipId As Integer
    Dim PurIntreceivabl_IDs As ArrayList = New ArrayList()
    Dim SellIntreceivabl_IDs As ArrayList = New ArrayList()
    Dim tmp_IntReceivable As Double
    Dim intrec As Decimal = 0
    Dim DealTime As Date
    Dim cIntr As Decimal
    Dim InterestOnHoliday As Boolean
    Dim InterestOnSat As Boolean
    Dim MaturityOnHoliday As Boolean
    Dim MaturityOnSat As Boolean
    Dim StockAvailable As Char
    Dim strStampDutyDate As String = ConfigurationManager.AppSettings("StampDutyDate")
    Dim sqlComm As New SqlCommand

    Dim param As New SqlParameter

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Session("username") = "") Then
                Response.Redirect("Login.aspx")
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
            txt_DealSlipNo.Enabled = False
            Hid_UserTypeId.Value = Session("UserTypeId")
            rdo_PurMethod.Items(0).Attributes.Add("onclick", "Showaddlnk();")
            rdo_PurMethod.Items(1).Attributes.Add("onclick", "Showaddlnk();")
            rbl_DealType.Items(0).Attributes.Add("onclick", "CheckDealType();")
            rbl_DealType.Items(1).Attributes.Add("onclick", "CheckDealType();")



            If (Trim(Request.QueryString("page") & "") = "DealSlipDetail.aspx") And rbl_TypeOFTranction.SelectedValue = "S" Then
                rbl_DealSlipType.Enabled = True
            Else
                rbl_DealSlipType.Enabled = False
            End If

            'srh_ContactPerson.SelectCheckBox.Enabled = False
            Costdealconf = ""

            srh_ContactPerson.SelectCheckBox.Visible = False
            srh_ContactPerson.SelectCheckBox.Checked = False
            'srh_ContactPerson.SelectLinkButton.Enabled = True

            lbl_TotalSettlementAmt.ReadOnly = True

            Hid_CompId.Value = Session("CompId")

            If IsPostBack = False Then
                Hid_MinDate.Value = Session("StartDate")
                Hid_MaxDate.Value = Session("EndDate")
                Page.SetFocus(rbl_TypeOFTranction)
                srh_SelectAddress.SearchTextBox.TextMode = TextBoxMode.MultiLine
                srh_CountSelectAddress.SearchTextBox.TextMode = TextBoxMode.MultiLine
                SetAttributes()
                SetControls()
                cbo_Demat.DataBind()
                cbo_Demat.Items.Insert(0, "")
                btn_ConvertToDeal.Visible = False
                If (Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Or
               Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Or Trim(Request.QueryString("page") & "") = "PendingCostMemo.aspx") Or
               Trim(Request.QueryString("Flag") & "") = "C" Or Hid_CostMemoFlag.Value = "C" Then
                    row_HoldingCost.Visible = False
                    row_IntraCost.Visible = False
                Else
                    row_HoldingCost.Visible = False
                    row_IntraCost.Visible = False
                End If

                If (Request.QueryString("PageName") & "") = "QuoteEntry.aspx" Then
                    If Val(Request.QueryString("SecurityId") & "") <> 0 Then
                        Hid_QuoteId.Value = Val(Request.QueryString("QuoteId") & "")
                        txt_Amount.Text = Val(Request.QueryString("FaceValue") & "")
                        cbo_Amount.SelectedValue = Val(Request.QueryString("Multiple") & "")
                        txt_Rate.Text = Val(Request.QueryString("Rate") & "")
                        FillQuoteFields(Val(Request.QueryString("QuoteId") & ""))

                    End If
                    SetAttributes()
                    SetTimeCombos()
                    GetCurrentTime()
                    btn_Save.Visible = True
                    btn_Update.Visible = False
                    btn_savegenerateDeal.Visible = False
                Else

                End If
                If Request.QueryString("Id") <> "" Then
                    Dim strId As String = objCommon.DecryptText(HttpUtility.UrlDecode(Request.QueryString("Id")))
                    ViewState("Id") = Val(strId)
                    Hid_DealSlipId.Value = Val(strId)
                    rbl_TypeOFTranction.Enabled = False
                    If Trim(Request.QueryString("page") & "") <> "DealSlipDetail.aspx" Then
                        rdo_TaxFree.Enabled = False
                    End If

                    FillFields()

                    If cbo_DealTransType.SelectedValue = "F" Then
                        If rbl_TypeOFTranction.SelectedValue = "P" Then
                            rdo_FinancialDealType.Items(1).Text = "To Sell"
                        Else
                            rdo_FinancialDealType.Items(1).Text = "From Purchase"
                        End If
                    End If

                    If Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Then
                        Hid_Page.Value = Trim(Request.QueryString("page") & "")
                        btn_Cancel.Visible = True
                        btn_Save.Visible = False
                        btn_Update.Visible = False
                        btn_savegenerateDeal.Visible = True
                        btn_ConvertToDeal.Visible = False
                        If rbl_TypeOFTranction.SelectedValue = "P" Then
                            If CheckSaleDeal() = False Then
                                ReadOnlyFields()
                            End If
                        Else
                            ReadOnlyFields()
                        End If

                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "testaa", "showAddLinkRemove();", True)
                        Hid_PendngFlag.Value = "Pending"
                        If cbo_DealTransType.SelectedValue = "D" Or cbo_DealTransType.SelectedValue = "T" Then
                            row_DealType.Visible = True
                        Else
                            row_DealType.Visible = False
                        End If
                        'End If
                    ElseIf Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                        Hid_Page.Value = Trim(Request.QueryString("page") & "")
                        btn_savegenerateDeal.Visible = True
                        btn_ConvertToDeal.Visible = False
                        btn_Save.Visible = False
                        btn_Update.Visible = False
                        rbl_DealType.Enabled = False
                        rdo_PurMethod.Enabled = False
                        If rbl_TypeOFTranction.SelectedValue = "P" Then
                            If CheckSaleDeal() = False Then
                                ReadOnlyFields()
                            End If
                        Else
                            ReadOnlyFields()
                        End If
                        Hid_PendngFlag.Value = "Generated"
                        If cbo_DealTransType.SelectedValue = "D" Or cbo_DealTransType.SelectedValue = "T" Then
                            row_DealType.Visible = False
                        Else
                            row_DealType.Visible = False
                        End If

                    Else
                        Hid_Page.Value = 0
                        btn_savegenerateDeal.Visible = False
                        btn_Save.Visible = False
                        btn_Update.Visible = True


                    End If
                    If Trim(Request.QueryString("page") & "") = "PendingCostMemo.aspx" Then
                        Hid_CostMemoPageName.Value = Trim(Request.QueryString("page") & "")
                        Hid_Page.Value = 0
                        btn_savegenerateDeal.Visible = False
                        btn_Update.Visible = False
                        btn_Save.Visible = False
                        btn_ConvertToDeal.Visible = True
                        btn_dealconf.Visible = True
                        ReadOnlyFieldsCheckerview()

                        'If Hid_UserTypeId.Value <> "1" Then
                        ReadOnlyFields()
                        'End If

                        CheckStock()
                        If cbo_DealTransType.SelectedValue = "D" Or cbo_DealTransType.SelectedValue = "T" Then
                            row_DealType.Visible = True
                        Else
                            row_DealType.Visible = False
                        End If
                    Else
                        btn_ConvertToDeal.Visible = False
                    End If
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "testaa", "showAddLinkRemove();", True)

                Else
                    btn_Save.Visible = True
                    btn_Update.Visible = False
                    btn_savegenerateDeal.Visible = False
                End If
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "select", "CheckDealType();", True)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "test", "RefDealSlipNoe();", True)
                If Val(Request.QueryString("Id") & "") <> 0 Then
                    FillIntestField()
                End If
            Else
                LabelError.Visible = False
                UploadTempImage()
            End If


            If (Hid_dealdone.Value) = "True" Then
                btn_Update.Visible = False
                btn_Cancel.Visible = True
                If rbl_TypeOFTranction.SelectedValue = "P" Then
                    If CheckSaleDeal() = False Then
                        ReadOnlyFields()
                    End If
                Else
                    ReadOnlyFields()
                End If
                rbl_DealSlipType.Enabled = False

                ' for round off readonly 
                If rbl_TypeOFTranction.SelectedValue = "P" Then
                    If CheckSellDeal("ID_CHECK_Selldeal") = True Then
                        txt_Roundoff.ReadOnly = True
                        rdo_roundoff.Enabled = False

                        txt_SettOtherChrgs.ReadOnly = True
                    End If
                End If


            End If
            If cbo_DealTransType.SelectedValue = "B" And rbl_TypeOFTranction.SelectedValue = "S" And Trim(Request.QueryString("page") & "") = "DealSlipDetail.aspx" Then
                btn_Update.Visible = False
            End If

            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "TotalNoOfBond", "TotalNoOfBond();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "select", "CheckDealType();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "test", "RefDealSlipNoe();", True)

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "DMAT", "CheckPhysicalDMAT(false);", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "visible", "CheckVisibleFalse();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "test", "RefDealSlipNoe();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "test1", "ReferenceBy();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "securityType", "securityType();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "show", "ReferenceBy();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "xxx", "CheckDealType();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "xx", "Showaddlnk();", True)
            If Trim(Request.QueryString("Flag") & "") = "C" Then
                If (rbl_TypeOFTranction.SelectedValue = "S" And cbo_DealTransType.SelectedValue <> "B") Then
                    btn_Save.Visible = False
                    btn_Update.Visible = False
                    btn_savegenerateDeal.Visible = False
                    'rbl_DealSlipType.Enabled = False
                    Session("PageNameCheckerView") = "Dealslipentry.aspx"
                    ReadOnlyFieldsCheckerview()
                    rbl_DealSlipType.Enabled = True
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "testaa", "showAddLinkRemove();", True)
                End If
            End If
            If Hid_T.Value = "1" Then
                cbo_SettDay_SelectedIndexChanged(sender, e)
                Hid_T.Value = "0"
            End If

            If Trim(Request.QueryString("page") & "") = "PendingCostMemo.aspx" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "test123", "CostMemoReadonly();", True)
            End If
            If Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "testaa", "showAddLinkRemove();", True)
            End If
            If Trim(Request.QueryString("Flag") & "") = "C" Then
                If Val(Hid_Page.Value) = 0 Then
                    Session("PageNameCheckerView") = ""
                End If
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "tt", "Brokerage();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "test1203", "CalcRoundofsettAMT();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "test7773", "BindDate();", True)
            If Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" And rbl_TypeOFTranction.SelectedValue = "S" Then
                btnRemoveMarking.Visible = True
                row_RemoveMarking.Visible = True
            Else
                btnRemoveMarking.Visible = False
                row_RemoveMarking.Visible = False
            End If
            If Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" And rbl_TypeOFTranction.SelectedValue = "S" Then
                rbl_DealType.SelectedValue = "M"
                rbl_DealType.Enabled = False
            End If
            'Hid_MinDate.Value = Session("StartDate")
            'Hid_MaxDate.Value = Session("EndDate")
            'If Trim(Request.QueryString("page") & "") = "PendingCostMemo.aspx" Or Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
            '    row_UploadSignedAck.Visible = True
            'Else
            '    row_UploadSignedAck.Visible = False
            'End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Sub GetCurrentTime()
        cbo_hr.SelectedValue = 0 'Hour(Now)
        cbo_minute.SelectedValue = 0 ' Val(Minute(Now))
        cbo_DealSecond.SelectedValue = 0 ' Val(Second(Now))
    End Sub
    Private Sub SetTimeCombos()
        Try
            Dim i As Integer
            cbo_hr.Items.Clear()
            cbo_minute.Items.Clear()
            For i = 0 To 24
                If i < 10 Then
                    cbo_hr.Items.Add(New ListItem(Trim("0" & i), i))
                Else
                    cbo_hr.Items.Add(New ListItem(Val(i), Val(i)))
                End If
            Next
            For i = 0 To 59
                If i < 10 Then
                    cbo_minute.Items.Add(New ListItem(Trim("0" & i), i))
                Else
                    cbo_minute.Items.Add(New ListItem(Val(i), Val(i)))
                End If
            Next

            For i = 0 To 59
                If i < 10 Then
                    cbo_DealSecond.Items.Add(New ListItem(Trim("0" & i), i))
                Else
                    cbo_DealSecond.Items.Add(New ListItem(Val(i), Val(i)))
                End If
            Next

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetTimeCombos", "Error in SetTimeCombos", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Sub SetControls()
        Try
            BrokerCombo()

            'srh_IssuerOfSecurity.Columns.Add("SecurityIssuer")
            'srh_IssuerOfSecurity.Columns.Add("SecurityIssuer")

            'Srh_NameofSecurity.Columns.Add("SecurityName")
            'Srh_NameofSecurity.Columns.Add("SecurityTypeName")
            'Srh_NameofSecurity.Columns.Add("SecurityIssuer")
            'Srh_NameofSecurity.Columns.Add("NSDLAcNumber As ISIN")
            'Srh_NameofSecurity.Columns.Add("SecurityId")


            'Srh_NameOFClient.Columns.Add("CustomerName")
            'Srh_NameOFClient.Columns.Add("CustomerTypeName")
            'Srh_NameOFClient.Columns.Add("PANNumber")
            'Srh_NameOFClient.Columns.Add("CustomerPhone")
            'Srh_NameOFClient.Columns.Add("CustomerId")

            'Srh_ReferenceBy.Columns.Add("CustomerName")
            'Srh_ReferenceBy.Columns.Add("CustomerTypeName")
            'Srh_ReferenceBy.Columns.Add("CustomerCity")
            'Srh_ReferenceBy.Columns.Add("CustomerPhone")
            'Srh_ReferenceBy.Columns.Add("CustomerId")

            'srh_BrokNameOfSeller.Columns.Add("CustomerName")
            'srh_BrokNameOfSeller.Columns.Add("CustomerTypeName")
            'srh_BrokNameOfSeller.Columns.Add("CustomerCity")
            'srh_BrokNameOfSeller.Columns.Add("CustomerPhone")
            'srh_BrokNameOfSeller.Columns.Add("CustomerId")


            'srh_SelectAddress.Columns.Add("Address1")
            'srh_SelectAddress.Columns.Add("Address2")
            'srh_SelectAddress.Columns.Add("Phone")
            'srh_SelectAddress.Columns.Add("City")
            'srh_SelectAddress.Columns.Add("EmailId")
            'srh_SelectAddress.Columns.Add("FaxNo")
            'srh_SelectAddress.Columns.Add("AddId")

            'srh_CountSelectAddress.Columns.Add("Address1")
            'srh_CountSelectAddress.Columns.Add("Address2")
            'srh_CountSelectAddress.Columns.Add("Phone")
            'srh_CountSelectAddress.Columns.Add("City")
            'srh_CountSelectAddress.Columns.Add("EmailId")
            'srh_CountSelectAddress.Columns.Add("FaxNo")
            'srh_CountSelectAddress.Columns.Add("AddId")

            'srh_BTBDealSlipNo.Columns.Add("DealSlipNo")
            'srh_BTBDealSlipNo.Columns.Add("FinancialDealType")
            'srh_BTBDealSlipNo.Columns.Add("CM.CustomerName")
            'srh_BTBDealSlipNo.Columns.Add("SM.SecurityName")
            'srh_BTBDealSlipNo.Columns.Add("CONVERT(VARCHAR,DealDate,103) As DealDate")
            'srh_BTBDealSlipNo.Columns.Add("CONVERT(VARCHAR,SettmentDate,103) As SettlementDate ")
            'srh_BTBDealSlipNo.Columns.Add("Rate")
            'srh_BTBDealSlipNo.Columns.Add("RemainingFaceValue")
            'srh_BTBDealSlipNo.Columns.Add("CONVERT(SMALLDATETIME,DealDate,103) As DealDateTime")


            'srh_BTBDealSlipNo.Columns.Add("DealSlipID")
            'srh_RefDealSlipNo.Columns.Add("DealSlipNo")
            'srh_RefDealSlipNo.Columns.Add("SM.SecurityName")
            'srh_RefDealSlipNo.Columns.Add("DealDate")
            'srh_RefDealSlipNo.Columns.Add("SettmentDate")
            'srh_RefDealSlipNo.Columns.Add("Rate")
            'srh_RefDealSlipNo.Columns.Add("DealSlipID")

            'srh_BrokingBTBDealSlipNo.Columns.Add("DealSlipNo")
            'srh_BrokingBTBDealSlipNo.Columns.Add("CM.CustomerName")
            'srh_BrokingBTBDealSlipNo.Columns.Add("SM.SecurityName")
            'srh_BrokingBTBDealSlipNo.Columns.Add("CONVERT(VARCHAR,DealDate,103) As DealDate")
            'srh_BrokingBTBDealSlipNo.Columns.Add("CONVERT(VARCHAR,SettmentDate,103) As SettlementDate ")
            'srh_BrokingBTBDealSlipNo.Columns.Add("Rate")
            'srh_BrokingBTBDealSlipNo.Columns.Add("RemainingFaceValue")
            'srh_BrokingBTBDealSlipNo.Columns.Add("DealSlipID")


            OpenConn()

            Dim dt As DataTable = objCommon.FillControl(cbo_SecurityType, sqlConn, "ID_FILL_SecurityTypeMaster1", "SecurityTypeName", "SecurityTypeId")
            Dim dt1 As DataTable = objCommon.FillControl(cbo_CustomerType, sqlConn, "ID_FILL_CustomerTypeMaster1", "CustomerTypeName", "CustomerTypeId")
            Dim dt2 As DataTable = objCommon.FillControl(cbo_BrokCustomerType, sqlConn, "ID_FILL_CustomerTypeMaster1", "CustomerTypeName", "CustomerTypeId")
            Dim dt3 As DataTable = objCommon.FillControl(cbo_refCustType, sqlConn, "ID_FILL_CustomerTypeMaster1", "CustomerTypeName", "CustomerTypeId")

            'Dim dtScheme As DataTable = objCommon.FillControl(cbo_CustomerScheme, sqlConn, "Id_FILL_ClientCustomerScheme", "SchemeName", "CustomerSchemeId")

            'Dim dtBrokScheme As DataTable = objCommon.FillControl(cbo_BrokCustomerScheme, sqlConn, "Id_FILL_ClientCustomerScheme", "SchemeName", "CustomerSchemeId")
            objCommon.FillControl(cbo_Company, sqlConn, "ID_FILL_CompanyMaster1", "CompName", "CompId")
            Dim lstItem1 As ListItem = cbo_Company.Items.FindByValue(Trim(Session("CompId")))
            If lstItem1 IsNot Nothing Then cbo_Company.SelectedValue = lstItem1.Value
            objCommon.FillControl(cbo_Exchange, sqlConn, "ID_FILL_ExchangeMaster1", "ExchangeName", "ExchangeId")
            objCommon.FillControl(cbo_SGLWith, sqlConn, "ID_FILL_SGLBankMaster1", "BankName", "SGLId", Val(cbo_Company.SelectedValue), "CompId")
            objCommon.FillControl(cbo_Bank, sqlConn, "ID_FILL_BankMaster1", "BankAccInfo", "BankId", Val(cbo_Company.SelectedValue), "CompId")
            'objCommon.FillControl(cbo_OurBank, sqlConn, "ID_FILL_BankMaster1NEW", "BankName", "BankId", Val(cbo_Company.SelectedValue), "CompId")
            dt = objCommon.FillControl(cbo_Demat, sqlConn, "ID_FILL_DMATMaster1", "DematClientInfo", "DMatId", Val(cbo_Company.SelectedValue), "CompId")
            If cbo_ModeOfPayment.SelectedValue = "N" Or cbo_ModeOfPayment.SelectedValue = "B" Or cbo_ModeOfPayment.SelectedValue = "L" Or cbo_DealTransType.SelectedValue = "B" Then
                cbo_Demat.SelectedValue = 0
            Else
                Dim lstItem6 As ListItem = cbo_Demat.Items.FindByText(Trim("INDUSIND BANK LTD  -  10289266"))
                If lstItem6 IsNot Nothing Then cbo_Demat.SelectedValue = lstItem6.Value
                Hid_Demat.Value = cbo_Demat.SelectedValue
            End If
            objCommon.FillControl(cbo_ReportedBy, sqlConn, "ID_FILL_BrokerMasterNew", "BrokerName", "BrokerId")
            objCommon.FillControl(cbo_ConsBroker, sqlConn, "ID_FILL_BrokerMasterNew", "BrokerName", "BrokerId")
            objCommon.FillControl(cbo_DealerName, sqlConn, "ID_FILL_Dealer", "NameOfUser", "UserId")
            Dim lstItem As ListItem = cbo_DealerName.Items.FindByText(Trim(Session("NameOfUser")))
            If lstItem IsNot Nothing Then cbo_DealerName.SelectedValue = lstItem.Value
            objCommon.FillControl(cbo_SellerDealerName, sqlConn, "ID_FILL_Dealer", "NameOfUser", "UserId")
            objCommon.FillControl(cbo_ReferenceByDealer, sqlConn, "ID_FILL_Dealer", "NameOfUser", "UserId")


            'Dim lstItem5 As ListItem = cbo_DealerName.Items.FindByText(Trim(Session("NameOfUser")))
            'If lstItem5 IsNot Nothing Then dfcbo_DealerName.SelectedValue = lstItem5.Value

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetControls", "Error in SetControls", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub SetAttributes()

        btn_SGLFedFormat.Attributes.Add("onclick", "return OpenReport('SGLFedFormat');")
        btn_SGLHDFCFormat.Attributes.Add("onclick", "return OpenReport('SGLHDFCFormat');")
        'lnk_add.Attributes.Add("onclick", "return AddMultiplepucahse();")
        txt_DealDate.Attributes.Add("onkeypress", "OnlyDate();")
        'txt_DealDate.Attributes.Add("onblur", "CheckDate(this,false);CallServerSide();")
        'txt_DealDate.Attributes.Add("onchange", "CheckDate(this,false);CallServerSide();")
        txt_DealDate.Text = Format(Now, "dd/MM/yyyy")

        txt_SettmentDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_SettmentDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_SettmentDate.Text = Format(Now, "dd/MM/yyyy")
        txt_DealSlipNo.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_ContactPerson.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_CountContactPerson.Attributes.Add("onblur", "ConvertUCase(this);")
        'txt_BrokeragePaidTo.Attributes.Add("onblur", "ConvertUCase(this);")
        'txt_BrokeragereceivedFrom.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_CancelRemark.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_Comment.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_PreviousInterest.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_CouponPaid.Attributes.Add("onkeypress", "OnlyDecimal();")
        btn_Save.Attributes.Add("onclick", "return  ValidationForSaveUpdate();")
        btn_Update.Attributes.Add("onclick", "return  ValidationForSaveUpdate();")
        lbl_Dealar.Text = Trim(Session("NameOfUser"))
        'cbo_Amount.Attributes.Add("onchange", "TotalNoOfBond();")
        'cbo_Company.Attributes.Add("onchange", "TotalNoOfBond();")
        cbo_DealTransType.Attributes.Add("onchange", "CheckDealType();")
        cbo_SecurityType.Attributes.Add("onchange", "securityType();")
        rbl_DealType.Attributes.Add("onclick", "CheckDealType();")
        'txt_Amount.Attributes.Add("onblur", "TotalNoOfBond();")
        rdo_PhysicalDMAT.Attributes.Add("onclick", "CheckPhysicalDMAT(true);")
        rbl_RefDealSlip.Attributes.Add("onclick", "RefDealSlipNoe();")
        rbl_Reference.Attributes.Add("onclick", "ReferenceBy();")
        chk_Brokerage1.Attributes.Add("onclick", "Brokerage();")
        txt_BrokeragePaid.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_Brokeragereceived.Attributes.Add("onkeypress", "OnlyDecimal();")
        rbl_TypeOFTranction.Attributes.Add("onclick", "CheckDealType();")
        txt_Amount.Attributes.Add("onkeypress", "OnlyDecimal();")
        btn_savegenerateDeal.Attributes.Add("onclick", "return Validation();")
        btn_ShowSecurity.Attributes.Add("onclick", "return ShowSecurityMaster();")
        btn_ShowPur.Attributes.Add("onclick", "return ShowPurDeal();")
        btn_ShowBrokPurdeal.Attributes.Add("onclick", "return ShowPurDeal();")
        btn_ShowCustomer.Attributes.Add("onclick", "return ShowCustomerMaster();")
        txt_Conchargespaid.Attributes.Add("onkeypress", "OnlyDecimalAndMinus();")
        txt_Conchargesreceived.Attributes.Add("onkeypress", "OnlyDecimalAndMinus();")
        rdo_FinancialDealType.Attributes.Add("onclick", "CheckDealType()")
        'btn_CalRate.Attributes.Add("onclick", "return ShowYieldCalculation();")
        txt_Roundoff.Text = 0.0
        lbl_Msg.Attributes.Add("style", "color:green")
        rdo_PurMethod.Attributes.Add("onclick", "return ClearText();")
        txt_Roundoff.Attributes.Add("onchange", "CalcRoundofsettAMT();")

        txt_SettOtherChrgs.Attributes.Add("onchange", "CalcRoundofsettAMT();")
        txt_DealerAmt.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_SettOtherChrgs.Attributes.Add("onkeypress", "OnlyDecimal();")
        If Session("UserTypeId") <> 107 Then
            Dim LstItem As ListItem
            LstItem = cbo_DealTransType.Items.FindByText("Proprietory")
            If LstItem IsNot Nothing Then
                cbo_DealTransType.Items.Remove(LstItem)
            End If
        End If
        If Trim(Request.QueryString("page") & "") = "DealSlipDetail.aspx" Then
            Dim LstItemMT As ListItem
            Dim LstItemMD As ListItem
            LstItemMT = cbo_DealTransType.Items.FindByText("MB Trading")
            LstItemMD = cbo_DealTransType.Items.FindByText("MB Distribution")
            If LstItemMT IsNot Nothing And LstItemMD IsNot Nothing Then
                cbo_DealTransType.Items.Remove(LstItemMT)
                cbo_DealTransType.Items.Remove(LstItemMD)
            End If
        End If
        SetTimeCombos()
        GetCurrentTime()
    End Sub
    Private Function CheckSaleDeal() As Boolean
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            Dim Count As Integer
            'Dim DealSlipNo As String
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_FILL_CheckSaleDeal"
            sqlComm.Connection = sqlConn
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@Cnt", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@DealSlipNo", SqlDbType.VarChar, 500, "O")
            sqlComm.ExecuteNonQuery()
            Count = sqlComm.Parameters.Item("@Cnt").Value
            'DealSlipNo = sqlComm.Parameters.Item("@DealSlipNo").Value
            If Count > 0 Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Function
    Private Sub FillIntestField()
        Try

            If Hid_dealdone.Value = "False" Or Hid_CostMemoFlag.Value = "C" Then
                FillSecurityDetailsDeal()
                If Val(Hid_Frequency.Value) <> 0 Then
                    'FillAccuredInterestOptions()
                    FillAccruedDetails()
                Else
                    If rdo_SelectOpt.SelectedValue = "B" Then
                        If Val(Hid_Frequency.Value) = 0 Then
                            Hid_Amt.Value = Val(txt_NoOfBonds.Text) * Val(txt_Rate.Text)
                            Hid_AddInterest.Value = 0
                        Else
                            Hid_Amt.Value = (Val(txt_Rate.Text) * Val(Hid_IssuePrice.Value) * Val(txt_NoOfBonds.Text)) / 100
                        End If
                        lbl_Amount.Text = Format(Trim(Hid_Amt.Value & ""), "Standard")
                        Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                    ElseIf rdo_SelectOpt.SelectedValue = "F" Then
                        Hid_Amt.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)) * Val(txt_Rate.Text) / 100)
                        Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                    End If
                    lbl_Amount.Text = Format(Trim(Hid_Amt.Value & ""), "Standard")
                End If
            Else
                FillInterestFieldFromdatabase()
            End If
            If Hid_CostMemoFlag.Value = "C" Then
                FillInterestFieldFromdatabase()
            End If
            'FillInterestFieldFromdatabase()
            'lbl_Amount.Text = Format(Trim(Hid_Amt.Value & ""), "Standard")
            'txt_InterestAmt.Text = Format(Trim(Hid_AddInterest.Value & ""), "Standard")
            'lbl_InterestDays.Text = Hid_IntDays.Value
            'lbl_InterestFromToDates.Text = Trim(Hid_InterestFromTo.Value & "")

            lbl_SettlementAmt.Text = Format(Trim(Hid_SettlementAmt.Value & ""), "Standard")
            If (Val(Hid_IntDays.Value) < 0) Then
                If Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Then
                    rdo_PreviousdealType.SelectedValue = "Y"
                    rdo_PreviousdealType.Enabled = False
                ElseIf Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                    rdo_PreviousdealType.SelectedValue = "Y"
                    rdo_PreviousdealType.Enabled = False
                End If
                tr_calcXInt.Visible = True
                tr_PreviousdealType.Visible = True
                lbl_InterestDays.Attributes.Add("style", "color:red")
                txt_InterestAmt.Attributes.Add("style", "color:red")
                lbl_InterestFromToDates.Attributes.Add("style", "color:red")
            Else

                If Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Then
                    rdo_PreviousdealType.Enabled = False
                ElseIf Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                    rdo_PreviousdealType.Enabled = False
                End If


                If rdo_PreviousdealType.SelectedValue = "Y" And rdo_calcXInt.SelectedValue = "N" Then
                    tr_PreviousdealType.Visible = True
                    tr_calcXInt.Visible = True
                Else
                    tr_PreviousdealType.Visible = False
                End If
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillIntestField", "Error in FillIntestField", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub


    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        CalcAmt()
        SetSaveUpdate("ID_INSERT_DealSlipEntry", False, True)
        OrderNoMsg()

        If cbo_DealTransType.SelectedValue = "B" And rbl_TypeOFTranction.SelectedValue = "P" Then
            srh_BrokingBTBDealSlipNo.SelectedId = Val(ViewState("Id"))
            fillBrokingfields()
            rbl_TypeOFTranction.SelectedValue = "S"
            ViewState("Id") = ""
            SetSaveUpdate("ID_INSERT_DealSlipEntry", False, True)
            OrderNoMsg()
            Hid_DealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_DealSlipId.Value))
            Hid_PurBrokDealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_PurBrokDealSlipId.Value))
            Hid_SellBrokDealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_SellBrokDealSlipId.Value))
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "open123", "DealTicketReportViewBrok()", True)
        Else
            Hid_DealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_DealSlipId.Value))
            Hid_PurBrokDealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_PurBrokDealSlipId.Value))
            Hid_SellBrokDealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_SellBrokDealSlipId.Value))
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "open", "DealTicketReportView()", True)
        End If
    End Sub
    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click

        Try
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "testaa", "TotalNoOfBond();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "testaa1", "TotalFaceValue();", True)

            CalcAmt()
            If Val(Hid_Frequency.Value) <> 0 Then
                'FillAccuredInterestOptions()
            Else
                If rdo_SelectOpt.SelectedValue = "B" Then
                    If Val(Hid_Frequency.Value) = 0 Then
                        Hid_Amt.Value = Val(txt_NoOfBonds.Text) * Val(txt_Rate.Text)
                        Hid_AddInterest.Value = 0
                    Else
                        Hid_Amt.Value = (Val(txt_Rate.Text) * Val(Hid_IssuePrice.Value) * Val(txt_NoOfBonds.Text)) / 100
                    End If
                    Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                ElseIf rdo_SelectOpt.SelectedValue = "F" Then
                    Hid_Amt.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)) * Val(txt_Rate.Text) / 100)
                    Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                End If
            End If

            SetSaveUpdate("ID_UPDATE_DealSlipEntry", False, False)

            If cbo_DealTransType.SelectedValue = "B" And rbl_TypeOFTranction.SelectedValue = "P" And Trim(Request.QueryString("page") & "") = "DealSlipDetail.aspx" Then
                SetSaveUpdate("ID_UPDATE_BSDealSlipEntry", False, False)
                Dim dt As DataTable
                OpenConn()
                dt = objCommon.FillDataTable1(sqlConn, "ID_FILL_BTBId", Val(Hid_PurBrokDealSlipId.Value), "DealSlipId")
                If dt IsNot Nothing Then
                    If dt.Rows.Count > 0 Then
                        Hid_SellBrokDealSlipId.Value = Trim(dt.Rows(0).Item("DealslipId") & "")
                    End If
                End If
                Hid_DealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_DealSlipId.Value))
                Hid_PurBrokDealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_PurBrokDealSlipId.Value))
                Hid_SellBrokDealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_SellBrokDealSlipId.Value))
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "open", "DealTicketReportViewBrok()", True)
            End If
            Hid_DealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_DealSlipId.Value))
            Hid_PurBrokDealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_PurBrokDealSlipId.Value))
            Hid_SellBrokDealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_SellBrokDealSlipId.Value))
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "open", "DealTicketReportView()", True)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_Update_Click", "Error in btn_Update_Click", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try


    End Sub

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try

            If rdo_PurMethod.SelectedValue = "M" Then
                Delete()
            End If

            If Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Then

                Response.Redirect("PendingDealSlip.aspx", False)
            ElseIf Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                Response.Redirect("GeneratedDealSlip.aspx", False)

            ElseIf Trim(Request.QueryString("page") & "") = "QuoteEntry.aspx" Then
                Response.Redirect("QuoteEntry.aspx", False)
            Else
                Response.Redirect("DealSlipDetail.aspx", False)
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_Cancel_Click", "Error in btn_Cancel_Click", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Function SetSaveUpdate(ByVal strProc As String, Optional ByVal blnRedirect As Boolean = True,
                                   Optional ByVal blnGenDealNo As Boolean = False)
        Try

            Dim sqlTrans As SqlTransaction
            OpenConn()
            ViewState("DealPurchaseTbl") = CType(objCommon.FillDataTable(sqlConn, "ID_FILL_DealPurchaseDetailsTblStr"), DataTable)
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction
            If (rbl_TypeOFTranction.SelectedValue = "S" And Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx") Then
                If CheckTotalStock(sqlTrans) Then
                    StockAvailable = "N"
                    Dim strHtml As String
                    Dim msg As String = "Not Enough stock available."
                    strHtml = "alert('" + msg + "');"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "msg", strHtml, True)
                    Exit Function
                End If

            End If
            'If Trim(Request.QueryString("page") & "") = "PendingCostMemo.aspx" Or Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
            '    If Doc2SQLServer(sqlTrans) = False Then Exit Function
            'End If

            If CheckProfitEntry(sqlTrans) = False Then Exit Function
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Function
            If DeletePurchaseDeal(sqlTrans) = False Then Exit Function
            If rdo_PurMethod.SelectedValue = "M" And rbl_DealType.SelectedValue = "M" Then
                If (Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Or Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx") Then
                    If SavePurchaseDealno(sqlTrans) = False Then Exit Function
                End If
            End If
            If rbl_TypeOFTranction.SelectedValue = "S" And
                (Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Or
                    Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx") Then
                'If DeleteProfit(sqlTrans) = False Then Exit Function
                If SaveProfit(sqlTrans, "ID_INSERT_PROFITCALC") = False Then Exit Function
                If rbl_DealType.SelectedValue = "F" Then
                    'If SaveIntReceivable(sqlTrans) = False Then Exit Function
                Else
                    If rbl_DealType.SelectedValue = "M" Then
                        If Val(srh_BTBDealSlipNo.SelectedId) = 0 Then
                            'If SaveIntReceivable(sqlTrans) = False Then Exit Function
                            If SaveIntReceivableNew(sqlTrans) = False Then Exit Function
                        Else
                            If SaveIntRecSingle(sqlTrans) = False Then Exit Function
                        End If
                    End If
                End If



            End If


            'Purchase Profit updation

            If ((rbl_TypeOFTranction.SelectedValue = "P" And rdo_FinancialDealType.SelectedIndex = 0 _
                       And (Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx")) Or (rbl_TypeOFTranction.SelectedValue = "P" And rdo_FinancialDealType.SelectedIndex = 1 And cbo_DealTransType.SelectedValue = "F" _
                                 And (Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx"))) Then
                If CheckProfit(sqlTrans) = True Then
                    If SavePurchaseProfitTD(sqlTrans, "ID_UPDATE_PurchaseProfit") = False Then Exit Function
                End If
            End If
            sqlTrans.Commit()
            'If Trim(Request.QueryString("page") & "") = "PendingCostMemo.aspx" Then
            '    SetCheckerFalse()
            'End If
            If blnRedirect = True Then
                If Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Then
                    Response.Redirect("PendingDealSlip.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
                Else
                    Response.Redirect("DealSlipDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
                End If
            End If
            Return True
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetSaveUpdate", "Error in SetSaveUpdate", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Function


    Private Function DeletePurchaseDeal(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_DealPurchaseDetails"
            sqlComm.Connection = sqlConn
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@SellDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "DeletePurchaseDeal", "Error in DeletePurchaseDeal", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Function DeleteSellDeal(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_DealPurchaseDetailsFinancial"
            sqlComm.Connection = sqlConn
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@PurchaseDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "DeleteSellDeal", "Error in DeleteSellDeal", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim strSettmentDate As String = Trim(txt_SettmentDate.Text)
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            sqlComm.Parameters.Clear()
            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            End If
            objCommon.SetCommandParameters(sqlComm, "@TransType", SqlDbType.Char, 1, "I", , , Trim(rbl_TypeOFTranction.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@PurMethod", SqlDbType.Char, 1, "I", , , Trim(rdo_PurMethod.SelectedValue))

            If Hid_CostMemoPageName.Value = "PendingCostMemo.aspx" Then
                If Costdealconf = "" Then
                    objCommon.SetCommandParameters(sqlComm, "@DealSlipType", SqlDbType.Char, 1, "I", , , Trim("D"))
                    objCommon.SetCommandParameters(sqlComm, "@DealSlipNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_DealSlipNo.Text))
                    objCommon.SetCommandParameters(sqlComm, "@CostMemoNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_DealSlipNo.Text))
                    objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , Trim(cbo_DealTransType.SelectedValue))
                Else
                    objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , Trim(cbo_DealTransType.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@DealSlipType", SqlDbType.Char, 1, "I", , , Trim(rbl_DealSlipType.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@DealSlipNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_DealSlipNo.Text))
                    objCommon.SetCommandParameters(sqlComm, "@CMDealConfFlag", SqlDbType.Bit, 1, "I", , , 1)
                End If
            Else

                If Hid_CostMemoFlag.Value = "C" And Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Then
                    objCommon.SetCommandParameters(sqlComm, "@CostMemoNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_DealSlipNo.Text))
                End If
                objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , Trim(cbo_DealTransType.SelectedValue))
                objCommon.SetCommandParameters(sqlComm, "@DealSlipType", SqlDbType.Char, 1, "I", , , Trim(rbl_DealSlipType.SelectedValue))
                objCommon.SetCommandParameters(sqlComm, "@DealSlipNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_DealSlipNo.Text))

            End If

            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , Val(Srh_NameofSecurity.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , Val(Srh_NameOFClient.SelectedId))

            If cbo_DealTransType.SelectedValue = "B" Then
                objCommon.SetCommandParameters(sqlComm, "@BrokCustomerId", SqlDbType.Int, 4, "I", , , Val(srh_BrokNameOfSeller.SelectedId))
            Else
                objCommon.SetCommandParameters(sqlComm, "@BrokCustomerId", SqlDbType.Int, 4, "I", , , DBNull.Value)
            End If

            objCommon.SetCommandParameters(sqlComm, "@FaceValue", SqlDbType.Decimal, 9, "I", , , Val(txt_Amount.Text))
            objCommon.SetCommandParameters(sqlComm, "@FaceValueMultiple", SqlDbType.BigInt, 8, "I", , , Val(cbo_Amount.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@NoOfBond", SqlDbType.Int, 4, "I", , , Val(txt_NoOfBonds.Text))

            objCommon.SetCommandParameters(sqlComm, "@DealDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_DealDate.Text))
            objCommon.SetCommandParameters(sqlComm, "@SettmentDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_SettmentDate.Text))
            objCommon.SetCommandParameters(sqlComm, "@Rate", SqlDbType.Decimal, 16, "I", , , Val(txt_Rate.Text))
            objCommon.SetCommandParameters(sqlComm, "@ModeofDelivery", SqlDbType.Char, 1, "I", , , Trim(rdo_PhysicalDMAT.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@AccIntDays", SqlDbType.Char, 1, "I", , , Trim(rdo_AccIntDays.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@ModeOFPayment", SqlDbType.Char, 1, "I", , , Trim(cbo_ModeOfPayment.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(cbo_Company.SelectedValue))

            If Val(cbo_Bank.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@bankId", SqlDbType.Int, 4, "I", , , Val(cbo_Bank.SelectedValue))
            End If

            If Val(cbo_SGLWith.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@SGLId", SqlDbType.Int, 4, "I", , , Val(cbo_SGLWith.SelectedValue))
            End If
            If Val(txt_BrokeragePaid.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@Brockpaid", SqlDbType.Decimal, 16, "I", , , Val(txt_BrokeragePaid.Text))
            End If
            If Val(txt_Brokeragereceived.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@BrockReceived", SqlDbType.Decimal, 16, "I", , , Val(txt_Brokeragereceived.Text))
            End If
            If Val(txt_SellBrokeragePaid.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@countbrokpaid", SqlDbType.Decimal, 16, "I", , , Val(txt_SellBrokeragePaid.Text))
            End If

            If Val(txt_SellBrokeragereceived.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@countbrokrecive ", SqlDbType.Decimal, 16, "I", , , Val(txt_SellBrokeragereceived.Text))
            End If
            objCommon.SetCommandParameters(sqlComm, "@SelectMethod", SqlDbType.Char, 1, "I", , , Trim(rbl_DealType.SelectedValue))
            If cbo_DealTransType.SelectedValue = "B" Then
                objCommon.SetCommandParameters(sqlComm, "@BTBDealSlipId", SqlDbType.Int, 4, "I", , , Val(srh_BrokingBTBDealSlipNo.SelectedId))
            Else
                objCommon.SetCommandParameters(sqlComm, "@BTBDealSlipId", SqlDbType.Int, 4, "I", , , Val(srh_BTBDealSlipNo.SelectedId))
            End If
            objCommon.SetCommandParameters(sqlComm, "@RefDealSlip", SqlDbType.Char, 1, "I", , , Trim(rbl_RefDealSlip.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@RefDealSlipId", SqlDbType.Int, 4, "I", , , Val(srh_RefDealSlipNo.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@CancelDeal", SqlDbType.Bit, 1, "I", , , 0)
            objCommon.SetCommandParameters(sqlComm, "@CancelRemark", SqlDbType.VarChar, 500, "I", , , txt_CancelRemark.Text)
            objCommon.SetCommandParameters(sqlComm, "@BrockEntry", SqlDbType.Bit, 1, "I", , , Val(chk_Brokerage1.Checked))
            If Val(Hid_QuoteId.Value) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@QuoteId", SqlDbType.Int, 4, "I", , , Val(Hid_QuoteId.Value))
            End If
            'If (Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Or _
            '    Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Or Trim(Request.QueryString("page") & "") = "PendingCostMemo.aspx") Or _
            '    Trim(Request.QueryString("Flag") & "") = "C" Or Hid_CostMemoFlag.Value = "C" Then
            'objCommon.SetCommandParameters(sqlComm, "@DealDone", SqlDbType.Bit, 1, "I", , , 1)
            objCommon.SetCommandParameters(sqlComm, "@Amount", SqlDbType.Decimal, 9, "I", , , Val(Hid_Amt.Value))
            objCommon.SetCommandParameters(sqlComm, "@InterestAmt", SqlDbType.Decimal, 9, "I", , , Val(txt_InterestAmt.Text.Replace(",", "")))
            objCommon.SetCommandParameters(sqlComm, "@InterestDays", SqlDbType.Int, 1, "I", , , Val(lbl_InterestDays.Text))
            objCommon.SetCommandParameters(sqlComm, "@InterestFromToDates", SqlDbType.VarChar, 50, "I", , , Trim(lbl_InterestFromToDates.Text))
            objCommon.SetCommandParameters(sqlComm, "@Settamtbeforeroundoff", SqlDbType.Decimal, 9, "I", , , Val(Hid_Amt.Value) + Val(txt_InterestAmt.Text.Replace(",", "")))
            objCommon.SetCommandParameters(sqlComm, "@Roundoff", SqlDbType.Decimal, 9, "I", , , Val(txt_Roundoff.Text))
            objCommon.SetCommandParameters(sqlComm, "@HCIntAmt", SqlDbType.Decimal, 9, "I", , , Val(txt_HoldingCost.Text))
            objCommon.SetCommandParameters(sqlComm, "@IDIntAmt", SqlDbType.Decimal, 9, "I", , , Val(txt_IntraDayCost.Text))
            If Val(txt_Roundoff.Text & "") <> 0.0 Then
                If rdo_roundoff.SelectedValue = "Y" Then
                    If chk_SettCharges.Checked = True Then
                        If rbl_TypeOFTranction.SelectedValue = "P" Then
                            objCommon.SetCommandParameters(sqlComm, "@SettlementAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_Amt.Value) + Val(txt_InterestAmt.Text.Replace(",", "")) + Val(txt_Roundoff.Text) + Val(txt_SettOtherChrgs.Text))
                        Else
                            objCommon.SetCommandParameters(sqlComm, "@SettlementAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_Amt.Value) + Val(txt_InterestAmt.Text.Replace(",", "")) + Val(txt_Roundoff.Text) - Val(txt_SettOtherChrgs.Text))

                        End If
                    Else
                        objCommon.SetCommandParameters(sqlComm, "@SettlementAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_Amt.Value) + Val(txt_InterestAmt.Text.Replace(",", "")) + Val(txt_Roundoff.Text))

                    End If
                Else
                    If chk_SettCharges.Checked = True Then
                        If rbl_TypeOFTranction.SelectedValue = "P" Then
                            objCommon.SetCommandParameters(sqlComm, "@SettlementAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_Amt.Value) + Val(txt_InterestAmt.Text.Replace(",", "")) - Val(txt_Roundoff.Text) + Val(txt_SettOtherChrgs.Text))
                        Else
                            objCommon.SetCommandParameters(sqlComm, "@SettlementAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_Amt.Value) + Val(txt_InterestAmt.Text.Replace(",", "")) - Val(txt_Roundoff.Text) - Val(txt_SettOtherChrgs.Text))

                        End If
                    Else
                        objCommon.SetCommandParameters(sqlComm, "@SettlementAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_Amt.Value) + Val(txt_InterestAmt.Text.Replace(",", "")) - Val(txt_Roundoff.Text))

                    End If
                End If
            Else
                If chk_SettCharges.Checked = True Then
                    If rbl_TypeOFTranction.SelectedValue = "P" Then
                        objCommon.SetCommandParameters(sqlComm, "@SettlementAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_Amt.Value) + Val(txt_InterestAmt.Text.Replace(",", "")) + Val(txt_SettOtherChrgs.Text))
                    Else
                        objCommon.SetCommandParameters(sqlComm, "@SettlementAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_Amt.Value) + Val(txt_InterestAmt.Text.Replace(",", "")) - Val(txt_SettOtherChrgs.Text))

                    End If
                Else
                    objCommon.SetCommandParameters(sqlComm, "@SettlementAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_Amt.Value) + Val(txt_InterestAmt.Text.Replace(",", "")))

                End If
            End If


            'End If
            If Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Or
               Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                objCommon.SetCommandParameters(sqlComm, "@DealDone", SqlDbType.Bit, 1, "I", , , 1)

                objCommon.SetCommandParameters(sqlComm, "@RemainingFaceValue", SqlDbType.BigInt, 8, "I", , , DBNull.Value)
            Else
                objCommon.SetCommandParameters(sqlComm, "@DealDone", SqlDbType.Bit, 1, "I", , , 0)
                If Trim(cbo_DealTransType.SelectedValue) = "F" And Trim(rbl_TypeOFTranction.SelectedValue) = "P" And Trim(rdo_FinancialDealType.SelectedValue) = "N" Then
                    objCommon.SetCommandParameters(sqlComm, "@RemainingFaceValue", SqlDbType.BigInt, 8, "I", , , 0)
                Else
                    objCommon.SetCommandParameters(sqlComm, "@RemainingFaceValue", SqlDbType.BigInt, 8, "I", , , (Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)))
                End If
            End If
            If (Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Or Trim(Request.QueryString("page") & "") = "PendingCostMemo.aspx") Then
                objCommon.SetCommandParameters(sqlComm, "@Flag", SqlDbType.Bit, 8, "I", , , 1)
            End If

            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")

            If Val(cbo_CustDemate.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@CustDPId", SqlDbType.Int, 4, "I", , , Val(cbo_CustDemate.SelectedValue))
            End If

            If Val(cbo_CounterCustSGLWith.SelectedValue) <> 0 And cbo_DealTransType.SelectedValue = "B" And rdo_PhysicalDMAT.SelectedValue = "S" Then
                objCommon.SetCommandParameters(sqlComm, "@CounterpartySGLid", SqlDbType.Int, 4, "I", , , Val(cbo_CounterCustSGLWith.SelectedValue))
            Else
                objCommon.SetCommandParameters(sqlComm, "@CounterpartySGLid", SqlDbType.Int, 4, "I", , , DBNull.Value)
            End If
            If Val(cbo_CustSGL.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@CustSGLId", SqlDbType.Int, 4, "I", , , Val(cbo_CustSGL.SelectedValue))
            End If
            If Val(Cbo_CounterCustDemat.SelectedValue) <> 0 And cbo_DealTransType.SelectedValue = "B" Then
                objCommon.SetCommandParameters(sqlComm, "@CounterPartyDmatid", SqlDbType.Int, 4, "I", , , Val(Cbo_CounterCustDemat.SelectedValue))
            Else
                objCommon.SetCommandParameters(sqlComm, "@CounterPartyDmatid", SqlDbType.Int, 4, "I", , , DBNull.Value)
            End If

            If Val(cbo_CustomerBank.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@CustBankId", SqlDbType.Int, 4, "I", , , Val(cbo_CustomerBank.SelectedValue))
            End If
            'tttt
            If Val(cbo_SellerCustomerBank.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@cuntcustbank", SqlDbType.Int, 4, "I", , , Val(cbo_SellerCustomerBank.SelectedValue))
            End If

            If Val(cbo_Exchange.SelectedValue) <> 0 Or Val(cbo_Exchange.SelectedValue) <> 3 Then
                If Val(cbo_Demat.SelectedValue) <> 0 Then
                    objCommon.SetCommandParameters(sqlComm, "@DMatId", SqlDbType.Int, 4, "I", , , Val(cbo_Demat.SelectedValue))
                End If
            End If

            objCommon.SetCommandParameters(sqlComm, "@ContactPersons", SqlDbType.VarChar, 500, "I", , , Trim(txt_ContactPerson.Text))
            objCommon.SetCommandParameters(sqlComm, "@CountContactPersons", SqlDbType.VarChar, 500, "I", , , Trim(txt_CountContactPerson.Text))
            If LastIPDate <> Date.MinValue Then
                objCommon.SetCommandParameters(sqlComm, "@LastIpDate", SqlDbType.DateTime, 4, "I", , , LastIPDate)
            End If
            If Val(cbo_Exchange.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@ExchangeId", SqlDbType.Int, 4, "I", , , Val(cbo_Exchange.SelectedValue))
            End If
            objCommon.SetCommandParameters(sqlComm, "@FORemark", SqlDbType.VarChar, 500, "I", , , Trim(txt_CancelRemark.Text))
            If Val(cbo_ReportedBy.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.Int, 4, "I", , , Val(cbo_ReportedBy.SelectedValue))
            Else
                objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.Int, 4, "I", , , DBNull.Value)
            End If
            'Cons
            If Val(cbo_ConsBroker.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@ConsBrokerid", SqlDbType.Int, 4, "I", , , Val(cbo_ConsBroker.SelectedValue))
            Else
                objCommon.SetCommandParameters(sqlComm, "@ConsBrokerid", SqlDbType.Int, 4, "I", , , DBNull.Value)
            End If
            'objCommon.SetCommandParameters(sqlComm, "@Conspaid", SqlDbType.Decimal, 9, "I", , , (txt_Conchargespaid.Text))
            'objCommon.SetCommandParameters(sqlComm, "@ConsRec", SqlDbType.Decimal, 9, "I", , , (txt_Conchargesreceived.Text))
            objCommon.SetCommandParameters(sqlComm, "@Conspaid", SqlDbType.Decimal, 9, "I", , , Val(txt_Conchargespaid.Text))
            objCommon.SetCommandParameters(sqlComm, "@ConsRec", SqlDbType.Decimal, 9, "I", , , Val(txt_Conchargesreceived.Text))
            If Val(cbo_BrokeragereceivedFrom.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@BrockRecvForm", SqlDbType.Int, 4, "I", , , Val(cbo_BrokeragereceivedFrom.SelectedValue))
            Else
                objCommon.SetCommandParameters(sqlComm, "@BrockRecvForm", SqlDbType.Int, 4, "I", , , DBNull.Value)
            End If
            If Val(cbo_BrokeragePaidTo.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@BrockPaidTo", SqlDbType.Int, 4, "I", , , Val(cbo_BrokeragePaidTo.SelectedValue))
            Else
                objCommon.SetCommandParameters(sqlComm, "@BrockPaidTo", SqlDbType.Int, 4, "I", , , DBNull.Value)
            End If
            objCommon.SetCommandParameters(sqlComm, "@SettTurms", SqlDbType.Char, 1, "I", , , Trim(cbo_SettTurms.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@PreviousInterest", SqlDbType.Decimal, 9, "I", , , Val(txt_PreviousInterest.Text))

            objCommon.SetCommandParameters(sqlComm, "@CouponPaid", SqlDbType.Decimal, 9, "I", , , Val(txt_CouponPaid.Text))

            If (Val(srh_SelectAddress.SelectedId) = 0) Then
                objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "I", , , DBNull.Value)
            Else
                objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "I", , , Val(srh_SelectAddress.SelectedId))
            End If

            If (Val(srh_CountSelectAddress.SelectedId) = 0) Then
                objCommon.SetCommandParameters(sqlComm, "@CounterCustAddressId", SqlDbType.Int, 4, "I", , , DBNull.Value)
            Else
                objCommon.SetCommandParameters(sqlComm, "@CounterCustAddressId", SqlDbType.Int, 4, "I", , , Val(srh_CountSelectAddress.SelectedId))
            End If

            objCommon.SetCommandParameters(sqlComm, "@WdmDeal", SqlDbType.Char, 1, "I", , , "D")

            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(cbo_DealerName.SelectedValue))

            If cbo_SellerDealerName.SelectedValue = "" Then
                objCommon.SetCommandParameters(sqlComm, "@SellDealer", SqlDbType.BigInt, 8, "I", , , DBNull.Value)
            Else
                objCommon.SetCommandParameters(sqlComm, "@SellDealer", SqlDbType.BigInt, 8, "I", , , Val(cbo_SellerDealerName.SelectedValue))
            End If

            objCommon.SetCommandParameters(sqlComm, "@NilComm", SqlDbType.Char, 1, "I", , , rdo_NilComm.SelectedValue)



            objCommon.SetCommandParameters(sqlComm, "@Reference", SqlDbType.Char, 1, "I", , , Trim(rbl_Reference.SelectedValue))
            If Srh_ReferenceBy.SearchTextBox.Text = "" Then
                objCommon.SetCommandParameters(sqlComm, "@ReferenceById", SqlDbType.Int, 4, "I", , , DBNull.Value)
            Else
                objCommon.SetCommandParameters(sqlComm, "@ReferenceById", SqlDbType.Int, 4, "I", , , Val(Srh_ReferenceBy.SelectedId))
            End If


            objCommon.SetCommandParameters(sqlComm, "@SettDays", SqlDbType.TinyInt, 4, "I", , , Val(cbo_SettDay.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@EntryUserId", SqlDbType.BigInt, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@FinancialDealType", SqlDbType.Char, 1, "I", , , Trim(rdo_FinancialDealType.SelectedValue))


            objCommon.SetCommandParameters(sqlComm, "@RefDealerId", SqlDbType.BigInt, 8, "I", , , Val(cbo_ReferenceByDealer.SelectedValue))

            objCommon.SetCommandParameters(sqlComm, "@FaceValueorNoOfBond", SqlDbType.Char, 1, "I", , , Trim(rdo_SelectOpt.SelectedValue))

            objCommon.SetCommandParameters(sqlComm, "@SettleNo", SqlDbType.VarChar, 500, "I", , , Trim(txt_SettleNo.Text))

            If Trim(Request.QueryString("Flag") & "") = "C" Then
                If rbl_DealSlipType.SelectedValue = "C" Then
                    objCommon.SetCommandParameters(sqlComm, "@CostMemoFlag", SqlDbType.Char, 1, "I", , , "C")
                Else
                    objCommon.SetCommandParameters(sqlComm, "@CostMemoFlag", SqlDbType.Char, 1, "I", , , "D")
                End If
            Else
                If Trim(Request.QueryString("page") & "") = "PendingCostMemo.aspx" Then
                    objCommon.SetCommandParameters(sqlComm, "@CostMemoFlag", SqlDbType.Char, 1, "I", , , "C")
                Else
                    If Hid_CostMemoFlag.Value = "C" Then
                        objCommon.SetCommandParameters(sqlComm, "@CostMemoFlag", SqlDbType.Char, 1, "I", , , "C")
                    Else
                        objCommon.SetCommandParameters(sqlComm, "@CostMemoFlag", SqlDbType.Char, 1, "I", , , "D")
                    End If
                End If
            End If

            ' objCommon.SetCommandParameters(sqlComm, "@CostMemoFlag", SqlDbType.Char, 1, "I", , , rbl_DealSlipType.SelectedValue)


            objCommon.SetCommandParameters(sqlComm, "@NextNo", SqlDbType.VarChar, 500, "O")
            objCommon.SetCommandParameters(sqlComm, "@Convincumfromex", SqlDbType.Char, 1, "I", , , Trim(rdo_calcXInt.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@Dealisexintdeal", SqlDbType.Char, 1, "I", , , Trim(rdo_PreviousdealType.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@Roundoffposorneg", SqlDbType.Char, 1, "I", , , Trim(rdo_roundoff.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@Comment", SqlDbType.VarChar, 500, "I", , , Trim(txt_Comment.Text))
            If chk_SettCharges.Checked = True Then

                objCommon.SetCommandParameters(sqlComm, "@OtherCharges", SqlDbType.Decimal, 9, "I", , , Val(txt_SettOtherChrgs.Text))
            End If
            objCommon.SetCommandParameters(sqlComm, "@DealerAmt", SqlDbType.Decimal, 9, "I", , , Val(txt_DealerAmt.Text))
            If Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Or
                          Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                If cbo_DealTransType.SelectedValue = "D" Or cbo_DealTransType.SelectedValue = "T" Then
                    objCommon.SetCommandParameters(sqlComm, "@DealType", SqlDbType.Char, 1, "I", , , Trim(cbo_DealType.SelectedValue))
                Else
                    objCommon.SetCommandParameters(sqlComm, "@DealType", SqlDbType.Char, 1, "I", , , DBNull.Value)
                End If
            End If
            DealTime = Today & " " & cbo_hr.SelectedValue & ":" & cbo_minute.SelectedValue & ":" & cbo_DealSecond.SelectedValue
            objCommon.SetCommandParameters(sqlComm, "@DealTime", SqlDbType.DateTime, 9, "I", , , DealTime)
            objCommon.SetCommandParameters(sqlComm, "@TaxFree", SqlDbType.Char, 1, "I", , , Trim(rdo_TaxFree.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@Yield", SqlDbType.Decimal, 9, "I", , , Val(txt_Yield.Text))
            objCommon.SetCommandParameters(sqlComm, "@YTCAnn", SqlDbType.Decimal, 9, "I", , , Val(Hid_YTC.Value))
            objCommon.SetCommandParameters(sqlComm, "@YTPAnn", SqlDbType.Decimal, 9, "I", , , Val(Hid_YTP.Value))
            objCommon.SetCommandParameters(sqlComm, "@DealAck", SqlDbType.Char, 1, "I", , , Trim(rdo_DealAck.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@RedeemedDeal", SqlDbType.Char, 1, "I", , , Trim(rdo_RedeemedSec.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@CommChecked", SqlDbType.Char, 1, "I", , , Trim(rdo_CommChecked.SelectedValue))
            If (Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Or
                      Trim(Request.QueryString("page") & "") = "PendingCostMemo.aspx" Or Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx") And objCommon.DateFormat(strSettmentDate) >= objCommon.DateFormat(strStampDutyDate) Then
                objCommon.SetCommandParameters(sqlComm, "@StampDutyDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(strStampDutyDate))
            End If
            objCommon.SetCommandParameters(sqlComm, "@TCSApplicable", SqlDbType.Char, 1, "I", , , Trim(rdo_TCSApplicable.SelectedValue))
            If tr_StaggMat.Visible = True Then
                objCommon.SetCommandParameters(sqlComm, "@CalcInParFV", SqlDbType.Char, 1, "I", , , Trim(rdo_StaggMat.SelectedValue))
            Else
                objCommon.SetCommandParameters(sqlComm, "@CalcInParFV", SqlDbType.Char, 1, "I", , , "F")
            End If
            objCommon.SetCommandParameters(sqlComm, "@MarketType", SqlDbType.Char, 1, "I", , , Trim(rdo_MarketType.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@TDSApplicable", SqlDbType.Char, 1, "I", , , Trim(rdo_TDSApplicable.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@CutOffRate", SqlDbType.Decimal, 9, "I", , , Val(txt_CutOffRate.Text))
            objCommon.SetCommandParameters(sqlComm, "@BrokerageRateAmt", SqlDbType.Char, 1, "I", , , Trim(rdo_BrokerageRateAmt.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@BrokerageRate", SqlDbType.Decimal, 9, "I", , , Val(txt_BrokerageRate.Text))
            objCommon.SetCommandParameters(sqlComm, "@CutOffYield", SqlDbType.Decimal, 9, "I", , , Val(txt_CutOffYield.Text))
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = Val(sqlComm.Parameters("@DealSlipID").Value)
            txt_DealSlipNo.Text = sqlComm.Parameters("@NextNo").Value
            Hid_bond.Value = ""
            Hid_DealSlipId.Value = Val(ViewState("Id"))

            If cbo_DealTransType.SelectedValue = "B" And rbl_TypeOFTranction.SelectedValue = "S" Then
                Hid_SellBrokDealSlipId.Value = Val(ViewState("Id"))
            End If
            If cbo_DealTransType.SelectedValue = "B" And rbl_TypeOFTranction.SelectedValue = "P" Then
                Hid_PurBrokDealSlipId.Value = Val(ViewState("Id"))
            End If

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveUpdate", "Error in SaveUpdate", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Function SavePurchaseDealno(ByRef sqlTrans As SqlTransaction) As Boolean
        Try
            If rdo_PurMethod.SelectedValue = "S" Then
                Return True
            End If

            Dim sqlComm As New SqlCommand
            Dim I As Int16
            Dim dt As DataTable = ViewState("DealPurchaseTbl")
            Dim dr As DataRow

            If Hid_Marked.Value <> "" Then
                Dim dtMarked As DataTable = CType(JsonConvert.DeserializeObject(Hid_Marked.Value, (GetType(DataTable))), DataTable)
                If dtMarked.Rows.Count > 0 Then
                    For Each row As DataRow In dtMarked.Rows
                        dr = dt.NewRow
                        dr("SellDealSlipId") = Val(ViewState("Id"))
                        dr("PurchaseDealSlipId") = Val(row("DealSlipId") & "")
                        dr("PurRemainingFaceValue") = Val(row("FaceValue") & "")
                        dr("TempPurRemainingFaceValue") = Val(row("FaceValue") & "")

                        If Val(row("NoofBond") & "") > 0 Then
                            dr("NoofBond") = Val(row("NoofBond") & "")
                        End If
                        dt.Rows.Add(dr)
                    Next

                    sqlComm.Transaction = sqlTrans
                    sqlComm.CommandType = CommandType.StoredProcedure
                    sqlComm.Connection = sqlConn

                    sqlComm.CommandText = "ID_INSERT_DealPurchaseDetailsOPT"
                    sqlComm.Parameters.Clear()
                    sqlComm.Parameters.Add("@DealPurTbl", SqlDbType.Structured).Value = dt
                    sqlComm.ExecuteNonQuery()
                End If
            End If


            'Dim sqlComm As New SqlCommand
            'Dim arrRemainingFaceValues As Array
            'Dim dt As DataTable
            'dt = ViewState("DealPurchaseTbl")
            'arrRemainingFaceValues = Split(Hid_RemainingFaceValue.Value, ",")

            'Dim I As Int16
            'Dim dr As DataRow

            'For I = 0 To lst_addmultiple.Items.Count - 1
            '    If Val(lst_addmultiple.Items(I).Value) <> 0 Then
            '        dr = dt.NewRow
            '        If Trim(arrRemainingFaceValues(I)) <> "" Then
            '            dr("SellDealSlipId") = Val(ViewState("Id"))
            '            dr("PurchaseDealSlipId") = Val(lst_addmultiple.Items(I).Value)
            '            dr("PurRemainingFaceValue") = Val(arrRemainingFaceValues(I))
            '            dr("TempPurRemainingFaceValue") = Val(arrRemainingFaceValues(I))
            '            dt.Rows.Add(dr)
            '        End If
            '    End If
            'Next

            'sqlComm.Transaction = sqlTrans
            'sqlComm.CommandType = CommandType.StoredProcedure
            'sqlComm.Connection = sqlConn
            'sqlComm.CommandText = "ID_INSERT_DealPurchaseDetailsOPT"
            'sqlComm.Parameters.Clear()
            'sqlComm.Parameters.Add("@DealPurTbl", SqlDbType.Structured).Value = dt
            'sqlComm.ExecuteNonQuery()

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SavePurchaseDealno", "Error in SavePurchaseDealno", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
            Return False
        End Try
    End Function


    Private Function CheckStock()
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            Dim intCnt As Integer
            sqlComm.CommandText = "ID_CHECK_STOCKCOSTMEMO"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , Val(Srh_NameofSecurity.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@FaceValue", SqlDbType.Decimal, 9, "I", , , Val(txt_Amount.Text))
            objCommon.SetCommandParameters(sqlComm, "@FaceValueMultiple", SqlDbType.BigInt, 8, "I", , , Val(cbo_Amount.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , Trim(cbo_DealTransType.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
            objCommon.SetCommandParameters(sqlComm, "@SettmentDate", SqlDbType.SmallDateTime, 9, "I", , , objCommon.DateFormat(txt_SettmentDate.Text))
            objCommon.SetCommandParameters(sqlComm, "@NoofBond", SqlDbType.Int, 4, "I", , , Val(txt_NoOfBonds.Text))
            intCnt = Val(sqlComm.ExecuteScalar())
            If intCnt = 0 Then
                btn_ConvertToDeal.Visible = False
            Else
                btn_ConvertToDeal.Visible = True
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CheckStock", "Error in CheckStock", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Function

    Private Function CheckTotalStock(ByVal sqltrans As SqlTransaction) As Boolean
        Try
            'OpenConn()
            Dim sqlComm As New SqlCommand
            Dim intCnt As Integer
            sqlComm.CommandText = "ID_CHECK_STOCKCOSTMEMO"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            sqlComm.Transaction = sqltrans
            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , Val(Srh_NameofSecurity.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@FaceValue", SqlDbType.Decimal, 9, "I", , , Val(txt_Amount.Text))
            objCommon.SetCommandParameters(sqlComm, "@FaceValueMultiple", SqlDbType.BigInt, 8, "I", , , Val(cbo_Amount.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , Trim(cbo_DealTransType.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
            objCommon.SetCommandParameters(sqlComm, "@SettmentDate", SqlDbType.SmallDateTime, 9, "I", , , objCommon.DateFormat(txt_SettmentDate.Text))
            objCommon.SetCommandParameters(sqlComm, "@NoofBond", SqlDbType.Int, 4, "I", , , Val(txt_NoOfBonds.Text))
            intCnt = Val(sqlComm.ExecuteScalar())

            If intCnt = 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CheckStock", "Error in CheckStock", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally

        End Try
    End Function

    Private Function CheckProfitEntry(ByRef sqlTrans As SqlTransaction)
        Try

            Dim sqlComm As New SqlCommand

            sqlComm.CommandText = "ID_CHECK_ProfitEntry"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            sqlComm.Transaction = sqlTrans
            objCommon.SetCommandParameters(sqlComm, "@SellDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@PurchaseDealSlipId", SqlDbType.Int, 4, "I", , , Val(srh_BTBDealSlipNo.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@Valid", SqlDbType.Int, 4, "O")
            'intCnt = Val(sqlComm.ExecuteScalar())
            If sqlComm.ExecuteScalar <= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "CheckProfitEntry", "Error in CheckProfitEntry", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)

        End Try
    End Function

    Private Function CheckProfit(ByRef sqlTrans As SqlTransaction)
        Try

            Dim sqlComm As New SqlCommand
            Dim intCnt As Integer
            sqlComm.CommandText = "ID_CHECK_Profit"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            sqlComm.Transaction = sqlTrans
            objCommon.SetCommandParameters(sqlComm, "@PurchaseDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@Valid", SqlDbType.Int, 4, "O")
            If sqlComm.ExecuteScalar >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "CheckProfit", "Error in CheckProfit", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)

        End Try
    End Function
    Private Sub OrderNoMsg()
        Try
            If (Trim(txt_DealSlipNo.Text) <> "") Then
                Dim strHtml As String
                Dim msg As String = "Deal No " + txt_DealSlipNo.Text + " is Generated"
                strHtml = "alert('" + msg + "');"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "msg", strHtml, True)
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "OrderNoMsg", "Error in OrderNoMsg", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub FillInterestFieldFromdatabase()
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_DealSlipEntry", Val(ViewState("Id")), "DealSlipID")
            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    Hid_Amt.Value = Trim(dt.Rows(0).Item("Amount") & "")
                    Hid_AddInterest.Value = Trim(dt.Rows(0).Item("InterestAmt") & "")
                    Hid_IntDays.Value = Trim(dt.Rows(0).Item("InterestDays") & "")
                    Hid_InterestFromTo.Value = Trim(dt.Rows(0).Item("InterestFromToDates") & "")
                    Hid_SettlementAmt.Value = Trim(dt.Rows(0).Item("Settamtbeforeroundoff") & "")
                    txt_Roundoff.Text = dt.Rows(0).Item("Roundoff") & ""
                    rdo_roundoff.SelectedValue = dt.Rows(0).Item("Roundoffposorneg") & ""
                End If
                FillSecurityDetailsDeal()
                If Val(dt.Rows(0).Item("AccruedInterest") & "") < 0 Then
                    Hid_AddInterest.Value = Val(dt.Rows(0).Item("AccruedInterest") & "")
                    Hid_IntDays.Value = -1 * Val(dt.Rows(0).Item("AccruedDays") & "")
                    Hid_InterestFromTo.Value = Trim(dt.Rows(0).Item("AccruedDates") & "")
                    If rdo_SelectOpt.SelectedValue = "B" Then
                        If Hid_Frequency.Value = 0 Then
                            Hid_Amt.Value = Val(txt_NoOfBonds.Text) * Val(txt_Rate.Text)
                            Hid_AddInterest.Value = 0
                        Else

                            Hid_Amt.Value = (Val(txt_Rate.Text) * Val(txt_NoOfBonds.Text))
                        End If

                        Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                    End If
                    'Hid_Amt.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)) * Val(txt_Rate.Text) / 100)
                    'lbl_TotalSettlementAmt.Text = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                    lbl_Amount.Text = Format(Trim(Hid_Amt.Value & ""), "Standard")
                    lbl_SettlementAmt.Text = objCommon.DecimalFormat(Val(dt.Rows(0).Item("AccruedInterest") & "") + Val(Hid_Amt.Value))
                    Hid_SettlementAmt.Value = Val(dt.Rows(0).Item("AccruedInterest") & "") + Val(Hid_Amt.Value)
                Else
                    Hid_AddInterest.Value = Val(dt.Rows(0).Item("AccruedInterest") & "")
                    Hid_IntDays.Value = Val(dt.Rows(0).Item("AccruedDays") & "")
                    Hid_InterestFromTo.Value = Trim(dt.Rows(0).Item("AccruedDates") & "")
                    If rdo_SelectOpt.SelectedValue = "B" Then
                        If Hid_Frequency.Value = 0 Then
                            Hid_Amt.Value = Val(txt_NoOfBonds.Text) * Val(txt_Rate.Text)
                            Hid_AddInterest.Value = 0
                        Else

                            Hid_Amt.Value = (Val(txt_Rate.Text) * Val(txt_NoOfBonds.Text))
                        End If
                        lbl_Amount.Text = Format(Trim(Hid_Amt.Value & ""), "Standard")
                        Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                    End If
                    'Hid_Amt.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)) * Val(txt_Rate.Text) / 100)
                    'lbl_TotalSettlementAmt.Text = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                    lbl_SettlementAmt.Text = objCommon.DecimalFormat(Val(dt.Rows(0).Item("AccruedInterest") & "") + Val(Hid_Amt.Value))
                    Hid_SettlementAmt.Value = Val(dt.Rows(0).Item("AccruedInterest") & "") + Val(Hid_Amt.Value)
                    lbl_TotalSettlementAmt.Text = objCommon.DecimalFormat(Val(dt.Rows(0).Item("AccruedInterest") & "") + Val(Hid_Amt.Value) + Val(dt.Rows(0).Item("StampDutyAmt") & "") + Val(dt.Rows(0).Item("TCSAmount") & ""))
                    lbl_StampDuty.Text = Val(dt.Rows(0).Item("StampDutyAmt") & "")
                    lbl_TCSAmount.Text = Val(dt.Rows(0).Item("TCSAmount") & "")
                    lbl_TDSAmount.Text = Val(dt.Rows(0).Item("TDSAmount") & "")
                    lbl_TotalSettlementAmt.Text = Format(Trim(lbl_TotalSettlementAmt.Text & ""), "Standard")
                End If
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillInterestFieldFromdatabase", "Error in FillInterestFieldFromdatabase", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub FillFields()
        Try
            'CHANGE 
            OpenConn()
            Dim dt As DataTable
            Dim lstItem As ListItem
            Dim DealTime() As String
            Dim lstParam As New List(Of SqlParameter)()

            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_DealSlipEntry", Val(ViewState("Id")), "DealSlipID")
            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    If Val(dt.Rows(0).Item("ImgId") & "") = 0 Then
                        row_Doc.Visible = False
                        row_UploadDoc.Visible = True
                    Else
                        row_UploadDoc.Visible = True
                        row_Doc.Visible = True
                    End If
                    If Val(dt.Rows(0).Item("QuoteId") & "") <> 0 Then
                        Hid_QuoteId.Value = Val(dt.Rows(0).Item("QuoteId") & "")
                    End If
                    Hid_Yield.Value = Val(dt.Rows(0).Item("Yield") & "")
                    txt_Yield.Text = Val(dt.Rows(0).Item("Yield") & "")
                    Hid_YTC.Value = Val(dt.Rows(0).Item("YTCAnn") & "")
                    Hid_YTP.Value = Val(dt.Rows(0).Item("YTPAnn") & "")
                    lbl_IPDates.Text = Trim(dt.Rows(0).Item("IPDates") & "")
                    rbl_TypeOFTranction.SelectedValue = Trim(dt.Rows(0).Item("TransType") & "")
                    rbl_DealSlipType.SelectedValue = Trim(dt.Rows(0).Item("DealSlipType") & "")
                    cbo_DealTransType.SelectedValue = Trim(dt.Rows(0).Item("DealTransType") & "")
                    txt_DealSlipNo.Text = Trim(dt.Rows(0).Item("DealSlipNo") & "")

                    If cbo_DealTransType.SelectedValue = "B" Then
                        lst_addmultiple.Visible = False
                    End If
                    If (rbl_DealSlipType.SelectedValue = "C") Then
                        Hid_CostMemoNo.Value = Trim(dt.Rows(0).Item("DealSlipNo") & "")
                        row_BackOffice.Visible = True
                    End If
                    Srh_NameofSecurity.SelectedId = Val(dt.Rows(0).Item("SecurityId") & "")
                    Hid_SecId.Value = HttpUtility.HtmlEncode(objCommon.EncryptText(Trim(dt.Rows(0).Item("SecurityId") & "")))
                    Hid_SecurityId.Value = Val(dt.Rows(0).Item("SecurityId") & "")
                    txt_Roundoff.Text = Val(dt.Rows(0).Item("Roundoff") & "")
                    Hid_Frequency.Value = GetFrequency(Trim(dt.Rows(0).Item("FrequencyOfInterest") & ""))

                    Srh_NameOFClient.SelectedId = Val(dt.Rows(0).Item("CustomerId") & "")
                    objCommon.FillControl(cbo_BrokeragePaidTo, sqlConn, "ID_FILL_BrokerMasterNew", "BrokerName", "BrokerId", Val(Srh_NameOFClient.SelectedId), "Customerid")
                    objCommon.FillControl(cbo_BrokeragereceivedFrom, sqlConn, "ID_FILL_BrokerMasterNew", "BrokerName", "BrokerId", Val(Srh_NameOFClient.SelectedId), "Customerid")
                    Hid_CustId.Value = HttpUtility.HtmlEncode(objCommon.EncryptText(Trim(dt.Rows(0).Item("CustomerId") & "")))
                    Srh_NameOFClient.SearchTextBox.Text = dt.Rows(0).Item("CustomerName").ToString
                    If cbo_DealTransType.SelectedValue = "B" Then
                        srh_BrokNameOfSeller.SelectedId = Val(dt.Rows(0).Item("BrokCustomerId") & "")
                        srh_BrokNameOfSeller.SearchTextBox.Text = dt.Rows(0).Item("BrokCustName").ToString
                        cbo_BrokCustomerType.SelectedValue = Val(dt.Rows(0).Item("BrokCustomerTypeId") & "")
                    End If
                    If Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Or Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Or Trim(Request.QueryString("page") & "") = "PendingCostMemo.aspx" Then
                        If cbo_DealTransType.SelectedValue = "D" Or cbo_DealTransType.SelectedValue = "T" Then
                            row_DealType.Visible = True
                            cbo_DealType.SelectedValue = Trim(dt.Rows(0).Item("DealType") & "")
                        Else
                            row_DealType.Visible = False
                        End If
                    End If

                    txt_Amount.Text = Val(dt.Rows(0).Item("FaceValue") & "")
                    cbo_Amount.SelectedValue = Val(dt.Rows(0).Item("FaceValueMultiple") & "")
                    txt_NoOfBonds.Text = ((dt.Rows(0).Item("NoOfBond") & ""))
                    Hid_bond.Value = Val(dt.Rows(0).Item("NoOfBond") & "")
                    txt_DealDate.Text = Format(dt.Rows(0).Item("DealDate"), "dd/MM/yyyy")
                    txt_DealDate.ReadOnly = True
                    'IMG1.Visible = False
                    txt_SettmentDate.Text = Format(dt.Rows(0).Item("SettmentDate"), "dd/MM/yyyy")
                    cbo_SettDay.SelectedValue = Val(dt.Rows(0).Item("SettDays") & "")
                    txt_Rate.Text = Trim(dt.Rows(0).Item("Rate") & "")
                    If rbl_TypeOFTranction.SelectedValue = "P" Then
                        txt_SettleNo.Text = Trim(dt.Rows(0).Item("SettleNo") & "")
                    Else
                        txt_SettleNo.Text = ""
                    End If

                    rdo_PhysicalDMAT.SelectedValue = Trim(dt.Rows(0).Item("ModeofDelivery") & "")
                    rdo_roundoff.SelectedValue = Trim(dt.Rows(0).Item("Roundoffposorneg") & "")
                    txt_DealerAmt.Text = Val(dt.Rows(0).Item("DealerAmt") & "")
                    txt_CutOffRate.Text = Val(dt.Rows(0).Item("CutOffRate") & "")
                    Hid_TCSApplicable.Value = Trim(dt.Rows(0).Item("PrintTCS") & "")
                    rdo_TCSApplicable.SelectedValue = Trim(dt.Rows(0).Item("PrintTCS") & "")
                    rdo_TDSApplicable.SelectedValue = Trim(dt.Rows(0).Item("TDSApplicable") & "")
                    rdo_MarketType.SelectedValue = Trim(dt.Rows(0).Item("MarketType") & "")
                    If Trim(dt.Rows(0).Item("AccIntDays") & "") Then
                        rdo_AccIntDays.SelectedValue = Trim(dt.Rows(0).Item("AccIntDays") & "")
                        Hid_AccIntDays.Value = Trim(dt.Rows(0).Item("AccIntDays") & "")
                    End If
                    cbo_ModeOfPayment.SelectedValue = Trim(dt.Rows(0).Item("ModeOFPayment") & "")

                    cbo_Company.SelectedValue = Val(dt.Rows(0).Item("CompId") & "")
                    objCommon.FillControl(cbo_SGLWith, sqlConn, "ID_FILL_SGLBankMaster1", "BankName", "SGLId", Val(cbo_Company.SelectedValue), "CompId")
                    objCommon.FillControl(cbo_Bank, sqlConn, "ID_FILL_BankMaster1", "BankAccInfo", "BankId", Val(cbo_Company.SelectedValue), "CompId")
                    cbo_Bank.SelectedValue = Val(dt.Rows(0).Item("bankId") & "")

                    If Trim(dt.Rows(0).Item("DealTime1") & "") <> "" Then
                        DealTime = Split(Trim(dt.Rows(0).Item("DealTime1")), ":")
                        If DealTime.Length > 1 Then
                            cbo_hr.SelectedValue = Val(Right(DealTime(0), 2))
                            cbo_minute.SelectedValue = Val(Left(DealTime(1), 2))
                            cbo_DealSecond.SelectedValue = Val(Left(DealTime(2), 2))
                        End If
                    End If
                    cbo_SGLWith.SelectedValue = Val(dt.Rows(0).Item("SGLId") & "")
                    objCommon.FillControl(cbo_Exchange, sqlConn, "ID_FILL_ExchangeMaster1", "ExchangeName", "ExchangeId")
                    cbo_Exchange.SelectedValue = Val(dt.Rows(0).Item("ExchangeId") & "")
                    Hid_DealerName.Value = Trim(dt.Rows(0).Item("DealerName") & "")
                    lbl_Dealar.Text = Trim(Hid_DealerName.Value)
                    'txt_BrokeragePaidTo.Text = Trim(dt.Rows(0).Item("BrockPaidTo") & "")
                    txt_BrokeragePaid.Text = Trim(dt.Rows(0).Item("Brockpaid") & "")
                    txt_SellBrokeragePaid.Text = Trim(dt.Rows(0).Item("countbrokpaid") & "")
                    'txt_BrokeragereceivedFrom.Text = Trim(dt.Rows(0).Item("BrockRecvForm") & "")
                    txt_Brokeragereceived.Text = Trim(dt.Rows(0).Item("BrockReceived") & "")
                    txt_SellBrokeragereceived.Text = Trim(dt.Rows(0).Item("countbrokrecive") & "")
                    objCommon.FillControl(cbo_ReferenceByDealer, sqlConn, "ID_FILL_Dealer", "NameOfUser", "UserId")
                    cbo_ReferenceByDealer.SelectedValue = Val(dt.Rows(0).Item("RefDealerId") & "")
                    objCommon.FillControl(cbo_DealerName, sqlConn, "ID_FILL_Dealer", "NameOfUser", "UserId")
                    cbo_DealerName.SelectedValue = Val(dt.Rows(0).Item("UserId") & "")
                    txt_Comment.Text = Trim(dt.Rows(0).Item("Comment") & "")
                    lbl_PAN.Text = Trim(dt.Rows(0).Item("PANNumber") & "")
                    objCommon.FillControl(cbo_SellerDealerName, sqlConn, "ID_FILL_Dealer", "NameOfUser", "UserId")
                    cbo_SellerDealerName.SelectedValue = Val(dt.Rows(0).Item("SellDealer") & "")
                    Hid_UserId.Value = Val(dt.Rows(0).Item("UserId") & "")
                    cbo_DealerName.SelectedValue = Val(dt.Rows(0).Item("UserId") & "")
                    rbl_DealType.SelectedValue = Trim(dt.Rows(0).Item("SelectMethod") & "")
                    rdo_PurMethod.SelectedValue = Trim(dt.Rows(0).Item("PurMethod") & "")
                    rdo_BrokerageRateAmt.SelectedValue = Trim(dt.Rows(0).Item("BrokerageRateAmt") & "")
                    txt_BrokerageRate.Text = Trim(dt.Rows(0).Item("BrokerageRate") & "")
                    txt_CutOffYield.Text = Trim(dt.Rows(0).Item("CutOffYield") & "")

                    If rdo_BrokerageRateAmt.SelectedValue = "A" Then
                        txt_BrokerageRate.Enabled = False
                    Else
                        txt_BrokerageRate.Enabled = True
                    End If

                    If rdo_BrokerageRateAmt.SelectedValue = "R" Then
                        txt_BrokeragePaid.Enabled = False
                    Else
                        txt_BrokeragePaid.Enabled = True
                    End If

                    If cbo_DealTransType.SelectedValue = "B" Then
                            srh_BrokingBTBDealSlipNo.SelectedId = Val(dt.Rows(0).Item("BTBDealSlipId") & "")
                            srh_BrokingBTBDealSlipNo.SearchTextBox.Text = dt.Rows(0).Item("BTBdealslipno").ToString
                            Hid_BTOBid.Value = Val(dt.Rows(0).Item("BTBDealSlipId") & "")
                        Else
                            srh_BTBDealSlipNo.SelectedId = Val(dt.Rows(0).Item("BTBDealSlipId") & "")
                            srh_BTBDealSlipNo.SearchTextBox.Text = dt.Rows(0).Item("BTBdealslipno").ToString
                            Hid_BTOBid.Value = Val(dt.Rows(0).Item("BTBDealSlipId") & "")
                        End If

                        Hid_BTOBid.Value = HttpUtility.HtmlEncode(objCommon.EncryptText(Trim(dt.Rows(0).Item("BTBDealSlipId") & "")))
                        If Trim(dt.Rows(0).Item("DealTransType") & "") = "B" Then
                            If Val(srh_BrokingBTBDealSlipNo.SelectedId) <> 0 Then
                                btn_ShowBrokPurdeal.Visible = True
                            End If
                        Else
                            If Val(srh_BTBDealSlipNo.SelectedId) <> 0 Then
                                btn_ShowPur.Visible = True
                            End If

                        End If
                        rbl_RefDealSlip.SelectedValue = Trim(dt.Rows(0).Item("RefDealSlip") & "")
                        srh_RefDealSlipNo.SelectedId = Val(dt.Rows(0).Item("RefDealSlipId") & "")
                        srh_RefDealSlipNo.SearchTextBox.Text = dt.Rows(0).Item("RefDealSlipNo").ToString
                        rdo_CommChecked.SelectedValue = dt.Rows(0).Item("CommChecked").ToString
                        Hid_dealdone.Value = dt.Rows(0).Item("DealDone")
                        If Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                            If dt.Rows(0).Item("DealDone") Then
                                txt_InterestAmt.Enabled = False
                            End If
                        End If

                        If dt.Rows(0).Item("DealDone") Then
                            If dt.Rows(0).Item("CommChecked").ToString = "Y" Then
                                If Val(Session("UserTypeId")) = 1 Then
                                    cbo_ConsBroker.Enabled = True
                                    txt_Conchargespaid.Enabled = True
                                    txt_Conchargesreceived.Enabled = True
                                    rdo_CommChecked.Enabled = True
                                Else
                                    cbo_ConsBroker.Enabled = False
                                    txt_Conchargespaid.Enabled = False
                                    txt_Conchargesreceived.Enabled = False
                                    rdo_CommChecked.Enabled = False
                                End If
                            Else
                                cbo_ConsBroker.Enabled = True
                                txt_Conchargespaid.Enabled = True
                                txt_Conchargesreceived.Enabled = True
                                rdo_CommChecked.Enabled = True
                            End If

                        End If
                        If dt.Rows(0).Item("DealDone") Then
                            If Val(Session("UserTypeId")) = 1 Then
                                rdo_CommChecked.Enabled = True
                            Else
                                rdo_CommChecked.Enabled = False
                            End If
                        Else
                            rdo_CommChecked.Enabled = False
                        End If
                        txt_CancelRemark.Text = Trim(dt.Rows(0).Item("FORemark") & "")
                        If Trim(dt.Rows(0).Item("DealTransType") & "") = "B" Then
                            cbo_Demat.SelectedValue = ""
                        Else
                            objCommon.FillControl(cbo_Demat, sqlConn, "ID_FILL_DMATMaster1", "DematClientInfo", "DMatId", Val(cbo_Company.SelectedValue), "CompId")
                            cbo_Demat.SelectedValue = Val(dt.Rows(0).Item("DMatId") & "")
                        End If

                        objCommon.FillControl(cbo_CustDemate, sqlConn, "ID_FILL_CustomerdPDetails", "DPClient", "CustDPId", Val(Srh_NameOFClient.SelectedId), "CustomerId")
                        cbo_CustDemate.SelectedValue = Val(dt.Rows(0).Item("CustDPId") & "")
                        objCommon.FillControl(cbo_CustSGL, sqlConn, "ID_FILL_CustomerSGLDetails", "SGLTransWith", "CustSGLId", Val(Srh_NameOFClient.SelectedId), "CustomerId")
                        cbo_CustSGL.SelectedValue = Val(dt.Rows(0).Item("CustSGLId") & "")



                        objCommon.FillControl(Cbo_CounterCustDemat, sqlConn, "ID_FILL_CustomerdPDetails", "DPClient", "CustDPId", Val(srh_BrokNameOfSeller.SelectedId), "CustomerId")
                        Cbo_CounterCustDemat.SelectedValue = Val(dt.Rows(0).Item("CounterPartyDmatid") & "")
                        objCommon.FillControl(cbo_CounterCustSGLWith, sqlConn, "ID_FILL_CustomerSGLDetails", "SGLTransWith", "CustSGLId", Val(srh_BrokNameOfSeller.SelectedId), "CustomerId")
                        cbo_CounterCustSGLWith.SelectedValue = Val(dt.Rows(0).Item("CounterpartySGLid") & "")

                        objCommon.FillControl(cbo_CustomerType, sqlConn, "ID_FILL_CustomerTypeMaster1", "CustomerTypeName", "CustomerTypeId")
                        cbo_CustomerType.SelectedValue = Val(dt.Rows(0).Item("CustomerTypeId") & "")
                        rdo_TaxFree.SelectedValue = Trim(dt.Rows(0).Item("TaxFree") & "")
                        rdo_DealAck.SelectedValue = Trim(dt.Rows(0).Item("DealAck") & "")
                        If cbo_DealTransType.SelectedValue = "B" Then
                            objCommon.FillControl(cbo_BrokCustomerType, sqlConn, "ID_FILL_CustomerTypeMaster1", "CustomerTypeName", "CustomerTypeId")
                            cbo_BrokCustomerType.SelectedValue = Val(dt.Rows(0).Item("BrokCustomerTypeId") & "")
                        End If
                        If rbl_Reference.SelectedValue = "Y" Then
                            objCommon.FillControl(cbo_refCustType, sqlConn, "ID_FILL_CustomerTypeMaster1", "CustomerTypeName", "CustomerTypeId")
                            cbo_refCustType.SelectedValue = Val(dt.Rows(0).Item("RefCustomerTypeId") & "")
                        End If

                        objCommon.FillControl(cbo_CustomerBank, sqlConn, "ID_FILL_CustomerBankDetailS", "BankAcc", "CustBankId", Val(Srh_NameOFClient.SelectedId), "CustomerId")
                        cbo_CustomerBank.SelectedValue = Val(dt.Rows(0).Item("CustBankId") & "")
                        'tttt
                        objCommon.FillControl(cbo_SellerCustomerBank, sqlConn, "ID_FILL_CustomerBankDetailS", "BankAcc", "CustBankId", Val(srh_BrokNameOfSeller.SelectedId), "CustomerId")
                        cbo_SellerCustomerBank.SelectedValue = Val(dt.Rows(0).Item("cuntcustbank") & "")

                        If Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                            lstParam.Clear()
                            lstParam.Add(New SqlParameter("@DealSlipId", Val(ViewState("Id"))))
                            Dim dsProfit As DataSet = objComm.FillDetails(lstParam, "ID_Fill_PurchaseDetails")
                            lst_addmultiple.Items.Clear()
                            If dsProfit.Tables.Count > 0 Then
                                Hid_Marked.Value = Trim(dsProfit.Tables(0).Rows(0)("Data") & "")
                                If Hid_Marked.Value <> "" Then
                                    Dim dtMarked As DataTable = CType(JsonConvert.DeserializeObject(Hid_Marked.Value, (GetType(DataTable))), DataTable)

                                    If dtMarked.Rows.Count > 0 Then
                                        For Each row As DataRow In dtMarked.Rows
                                            lst_addmultiple.Items.Add(New ListItem(Trim(row("DealSlipNo") & ""), Trim(row("DealSlipId") & "")))
                                        Next
                                        btnRemoveMarking.Visible = True
                                    End If
                                End If
                            End If
                        End If

                        txt_ContactPerson.Text = Trim(dt.Rows(0).Item("ContactPersons") & "")
                        txt_CountContactPerson.Text = Trim(dt.Rows(0).Item("CountContactPersons") & "")
                        LastIPDate = IIf(Trim(dt.Rows(0).Item("LastIpDate") & "") = "", Date.MinValue, dt.Rows(0).Item("LastIpDate")) 'dt.Rows(0).Item("LastIpDate")
                        cbo_ReportedBy.SelectedValue = Val(dt.Rows(0).Item("BrokerId") & "")
                        cbo_BrokeragereceivedFrom.SelectedValue = Val(dt.Rows(0).Item("BrockRecvForm") & "")
                        cbo_BrokeragePaidTo.SelectedValue = Val(dt.Rows(0).Item("BrockPaidTo") & "")
                        cbo_SettTurms.SelectedValue = Trim(dt.Rows(0).Item("SettTurms") & "")
                        txt_PreviousInterest.Text = Val(dt.Rows(0).Item("PreviousInterest") & "")
                        txt_CouponPaid.Text = Val(dt.Rows(0).Item("CouponPaid") & "")
                        srh_SelectAddress.SearchTextBox.Text = dt.Rows(0).Item("Address1") & " " & dt.Rows(0).Item("Address2") & " " & dt.Rows(0).Item("City") & "-" & dt.Rows(0).Item("PinCode")
                        rdo_RedeemedSec.SelectedValue = Trim(dt.Rows(0).Item("RedeemedDeal") & "")
                        srh_CountSelectAddress.SearchTextBox.Text = dt.Rows(0).Item("CAddress1") & " " & dt.Rows(0).Item("CAddress2") & " " & dt.Rows(0).Item("CCity") & "-" & dt.Rows(0).Item("CPinCode")

                        srh_SelectAddress.SelectedId = Val(dt.Rows(0).Item("ClientCustAddressId") & "")
                        srh_CountSelectAddress.SelectedId = Val(dt.Rows(0).Item("CounterCustAddressId") & "")
                        cbo_ConsBroker.SelectedValue = Val(dt.Rows(0).Item("ConsBrokerid") & "")
                        txt_Conchargespaid.Text = Trim(dt.Rows(0).Item("Conspaid") & "")
                        txt_Conchargesreceived.Text = Trim(dt.Rows(0).Item("ConsRec") & "")
                        chk_Brokerage1.Checked = dt.Rows(0).Item("BrockEntry")
                        CostMemoFlag = Trim(dt.Rows(0).Item("CostMemoFlag") & "")
                        Hid_CostMemoFlag.Value = Trim(dt.Rows(0).Item("CostMemoFlag") & "")

                        If (Trim(Request.QueryString("page") & "") = "DealSlipDetail.aspx") And rbl_TypeOFTranction.SelectedValue = "S" Then
                            rbl_DealSlipType.Enabled = True
                        Else
                            rbl_DealSlipType.Enabled = False
                        End If
                        rbl_DealType.Enabled = True
                        cbo_DealTransType.Enabled = False
                        'srh_BTBDealSlipNo.SearchButton.Visible = False
                        rbl_RefDealSlip.Enabled = False
                        '  srh_RefDealSlipNo.SearchButton.Visible = False
                        If Trim(Request.QueryString("page") & "") <> "DealSlipDetail.aspx" Then
                            If rdo_PurMethod.SelectedValue = "M" Then
                                txt_Amount.ReadOnly = True
                                cbo_Amount.Enabled = False
                                txt_NoOfBonds.ReadOnly = True
                            End If
                        End If

                        If cbo_DealTransType.SelectedValue = "F" Then
                            If cbo_DealTransType.SelectedValue = "F" And rbl_TypeOFTranction.SelectedValue = "P" Then
                                Hid_RemainingFaceValue.Value = Trim(dt.Rows(0).Item("SellRemFaceValue") & "")
                                Hid_SingleRemainFV.Value = Val(dt.Rows(0).Item("SingleFinRemFaceValue") & "")

                                If Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Then
                                    Hid_SingleRemainFV.Value = Val(dt.Rows(0).Item("SellRemFaceValueFinacial1") & "")

                                ElseIf Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                                    Hid_SingleRemainFV.Value = Val(dt.Rows(0).Item("SellRemFaceValueFinacial") & "")

                                Else
                                    Hid_SingleRemainFV.Value = Val(dt.Rows(0).Item("SellRemFaceValueFinacial1") & "")
                                    'Hid_SingleRemainFV.Value = Val(dt.Rows(0).Item("SellRemFaceValueFinacial") & "")
                                End If
                            End If
                        Else


                            If Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Then
                                Hid_SingleRemainFV.Value = Val(dt.Rows(0).Item("SellRemFaceValueFinacial") & "")
                            ElseIf Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                                Hid_SingleRemainFV.Value = Val(dt.Rows(0).Item("SellRemFaceValueFinacial") & "")
                            Else
                                Hid_SingleRemainFV.Value = Val(dt.Rows(0).Item("SellRemFaceValueFinacial") & "")
                                'Hid_SingleRemainFV.Value = Val(dt.Rows(0).Item("SellRemFaceValueFinacial") & "")
                            End If

                            Hid_RemainingFaceValue.Value = Trim(dt.Rows(0).Item("PurcRemFaceValue") & "")
                            'Hid_SingleRemainFV.Value = Val(dt.Rows(0).Item("SingleRemFaceValue") & "")
                        End If

                    End If

                End If
            Hid_RemainingFaceValue.Value = Trim(dt.Rows(0).Item("PurcRemFaceValue") & "")
            Hid_SingleRemainFV.Value = Val(dt.Rows(0).Item("SingleRemFaceValue") & "")
            rdo_NilComm.SelectedValue = Trim(dt.Rows(0).Item("NilComm") & "")

            rbl_Reference.SelectedValue = Trim(dt.Rows(0).Item("Reference") & "")
            Srh_ReferenceBy.SelectedId = Val(dt.Rows(0).Item("ReferenceById") & "")
            Srh_ReferenceBy.SearchTextBox.Text = dt.Rows(0).Item("ReferenceByName").ToString


            cbo_refCustType.SelectedValue = Val(dt.Rows(0).Item("RefCustomerTypeId") & "")
            rdo_FinancialDealType.SelectedValue = Trim(dt.Rows(0).Item("FinancialDealType") & "")
            rdo_SelectOpt.SelectedValue = Trim(dt.Rows(0).Item("FaceValueorNoOfBond") & "")

            If cbo_DealTransType.SelectedValue = "B" And rbl_TypeOFTranction.SelectedValue = "S" Then
                srh_BrokNameOfSeller.SearchButton.Visible = False
                Srh_NameOFClient.SearchButton.Visible = False
            End If
            rdo_PreviousdealType.SelectedValue = Trim(dt.Rows(0).Item("Dealisexintdeal") & "")
            rdo_calcXInt.SelectedValue = Trim(dt.Rows(0).Item("Convincumfromex") & "")
            If Val(dt.Rows(0).Item("OtherCharges") & "") <> 0 Then
                chk_SettCharges.Checked = True

                txt_SettOtherChrgs.Text = Val(dt.Rows(0).Item("OtherCharges") & "")
            End If
            'End If
            'End If

            If (Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Or
                 Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Or Trim(Request.QueryString("page") & "") = "PendingCostMemo.aspx") Or
                  Hid_CostMemoFlag.Value = "C" Then
                row_HoldingCost.Visible = False
                row_IntraCost.Visible = False

            Else
                row_HoldingCost.Visible = False
                row_IntraCost.Visible = False

            End If
            If Val(dt.Rows(0).Item("HCostIntAmt") & "") <> 0 Or Val(dt.Rows(0).Item("IDCostIntAmt") & "") <> 0 Then
                chk_SettCharges.Checked = True

                txt_HoldingCost.Text = Val(dt.Rows(0).Item("HCostIntAmt") & "")
                txt_IntraDayCost.Text = Val(dt.Rows(0).Item("IDCostIntAmt") & "")

            End If
            Hid_AddInterest.Value = Val(dt.Rows(0).Item("AccruedInterest") & "")
            Hid_IntDays.Value = Val(dt.Rows(0).Item("AccruedDays") & "")
            Hid_InterestFromTo.Value = Trim(dt.Rows(0).Item("AccruedDates") & "")
            Hid_StampDutyAmt.Value = Val(dt.Rows(0).Item("StampDutyAmt") & "")
            Hid_TCSAmount.Value = Val(dt.Rows(0).Item("TCSAmount") & "")
            FillSecurityDetails()
            If Trim(Request.QueryString("page") & "") <> "DealSlipDetail.aspx" Then
                If rbl_TypeOFTranction.SelectedValue = "P" Then
                    If CheckSaleDeal() = False Then
                        ReadOnlyFields()
                    End If
                Else
                    ReadOnlyFields()
                End If
            End If

            'Hid_Amtshow.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)) * Val(txt_Rate.Text) / 100)
            If rdo_SelectOpt.SelectedValue = "B" Then
                If Val(Hid_Frequency.Value) = 0 Then
                    Hid_Amt.Value = Val(txt_NoOfBonds.Text) * Val(txt_Rate.Text)
                    Hid_AddInterest.Value = 0
                Else
                    Hid_Amt.Value = (Val(txt_Rate.Text) * Val(txt_NoOfBonds.Text))
                End If
                Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
            ElseIf rdo_SelectOpt.SelectedValue = "F" Then
                Hid_Amt.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)) * Val(txt_Rate.Text) / 100)
                Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
            End If

            Hid_Amtshow.Value = Hid_Amt.Value
            Hid_SettlementAmt.Value = Val(Hid_Amtshow.Value) + Val(Hid_AddInterest.Value)

            'If (cbo_SecurityType.SelectedItem.Text.Contains("PREFERENCE")) Then
            If Session("UserTypeId") = 1 Then

                If Val(dt.Rows(0).Item("AccruedInterest") & "") < 0 Then
                    lbl_Amount.Text = Format(Trim(Hid_Amt.Value & ""), "Standard")
                    Hid_IntDays.Value = -1 * Val(dt.Rows(0).Item("AccruedDays") & "")
                    Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                    Hid_AddInterest.Value = objCommon.DecimalFormat((Val(Hid_AddInterest.Value)))
                    lbl_InterestDays.Text = Hid_IntDays.Value
                    txt_InterestAmt.Text = Hid_AddInterest.Value
                    lbl_InterestDays.ForeColor = Drawing.Color.Red
                    txt_InterestAmt.ForeColor = Drawing.Color.Red
                    lbl_SettlementAmt.Text = Hid_SettlementAmt.Value
                    lbl_InterestFromToDates.Text = Hid_InterestFromTo.Value
                    lbl_TotalSettlementAmt.Text = objCommon.DecimalFormat(Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value) + Val(dt.Rows(0).Item("StampDutyAmt") & "") + Val(dt.Rows(0).Item("TCSAmount") & ""))
                    lbl_StampDuty.Text = Val(dt.Rows(0).Item("StampDutyAmt") & "")
                    lbl_TCSAmount.Text = Val(dt.Rows(0).Item("TCSAmount") & "")
                    lbl_TDSAmount.Text = Val(dt.Rows(0).Item("TDSAmount") & "")

                Else
                    lbl_Amount.Text = Format(Trim(Hid_Amt.Value & ""), "Standard")
                    txt_InterestAmt.Text = Format(Trim(Hid_AddInterest.Value & ""), "Standard")
                    lbl_InterestDays.Text = Hid_IntDays.Value
                    lbl_InterestFromToDates.Text = Trim(Hid_InterestFromTo.Value & "")
                    Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                    lbl_SettlementAmt.Text = Format(Trim(Hid_SettlementAmt.Value & ""), "Standard")
                    lbl_InterestFromToDates.Text = Hid_InterestFromTo.Value

                    lbl_StampDuty.Text = Val(dt.Rows(0).Item("StampDutyAmt") & "")
                    lbl_TCSAmount.Text = Val(dt.Rows(0).Item("TCSAmount") & "")
                    lbl_TDSAmount.Text = Val(dt.Rows(0).Item("TDSAmount") & "")
                    lbl_TotalSettlementAmt.Text = objCommon.DecimalFormat(Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value) + Val(dt.Rows(0).Item("StampDutyAmt") & "") + Val(dt.Rows(0).Item("TCSAmount") & "") - Val(dt.Rows(0).Item("TDSAmount") & ""))
                End If
            Else
                If Val(dt.Rows(0).Item("AccruedInterest") & "") < 0 Then
                    lbl_Amount.Text = Format(Trim(Hid_Amt.Value & ""), "Standard")
                    Hid_IntDays.Value = -1 * Val(dt.Rows(0).Item("AccruedDays") & "")
                    Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                    lbl_InterestDays.Text = Hid_IntDays.Value
                    txt_InterestAmt.ForeColor = Drawing.Color.Red
                    lbl_InterestDays.ForeColor = Drawing.Color.Red
                    lbl_InterestFromToDates.Text = Hid_InterestFromTo.Value
                    lbl_TotalSettlementAmt.Text = objCommon.DecimalFormat(Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value) + Val(dt.Rows(0).Item("StampDutyAmt") & "") + Val(dt.Rows(0).Item("TCSAmount") & "") - Val(dt.Rows(0).Item("TDSAmount") & ""))
                    Hid_AddInterest.Value = Format(Trim(Hid_AddInterest.Value & ""), "Standard")
                    txt_InterestAmt.Text = Format(Trim(Hid_AddInterest.Value & ""), "Standard")
                    lbl_StampDuty.Text = Val(dt.Rows(0).Item("StampDutyAmt") & "")
                    lbl_TCSAmount.Text = Val(dt.Rows(0).Item("TCSAmount") & "")
                    lbl_SettlementAmt.Text = Format(Trim(Hid_SettlementAmt.Value & ""), "Standard")
                Else
                    lbl_InterestDays.Text = Hid_IntDays.Value
                    lbl_InterestFromToDates.Text = Trim(Hid_InterestFromTo.Value & "")
                    Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                    lbl_InterestFromToDates.Text = Hid_InterestFromTo.Value
                    lbl_TotalSettlementAmt.Text = objCommon.DecimalFormat(Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value) + Val(dt.Rows(0).Item("StampDutyAmt") & "") + Val(dt.Rows(0).Item("TCSAmount") & "") - Val(dt.Rows(0).Item("TDSAmount") & ""))
                    lbl_Amount.Text = Format(Trim(Hid_Amt.Value & ""), "Standard")
                    txt_InterestAmt.Text = Format(Trim(Hid_AddInterest.Value & ""), "Standard")
                    lbl_StampDuty.Text = Val(dt.Rows(0).Item("StampDutyAmt") & "")
                    lbl_TCSAmount.Text = Val(dt.Rows(0).Item("TCSAmount") & "")
                    lbl_TDSAmount.Text = Val(dt.Rows(0).Item("TDSAmount") & "")
                    lbl_SettlementAmt.Text = Format(Trim(Hid_SettlementAmt.Value & ""), "Standard")
                End If
            End If
            lbl_TotalSettlementAmt.Text = Format(Trim(lbl_TotalSettlementAmt.Text & ""), "Standard")
            rdo_AccIntDays.SelectedValue = Hid_AccIntDays.Value

            'PR
            If rdo_StaggMat.SelectedValue = "P" Then
                FillSecurityFaceValue()
            Else
                Hid_NSDLFaceValue.Value = Hid_NSDLORGFacevalue.Value
            End If




        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillFields", "Error in FillFields", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Public Sub FillSecurityFaceValue()
        Try
            OpenConn()
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand
            Dim dt As New DataTable
            Hid_NSDLFaceValue.Value = Hid_NSDLORGFacevalue.Value
            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_FILL_MaturityAmtRedmd"
                objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , Val(Srh_NameofSecurity.SelectedId))
                objCommon.SetCommandParameters(sqlComm, "@SettlementDate", SqlDbType.SmallDateTime, 8, "I", , , objCommon.DateFormat(txt_SettmentDate.Text))
                .ExecuteNonQuery()
            End With
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    Hid_NSDLFaceValue.Value = Val(Hid_NSDLFaceValue.Value) - Val(dt.Rows(0).Item("MatAmtRedeemd") & "")
                End If
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType, "TotalNoOfBond", "TotalNoOfBond();", True)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillSecurityFaceValue", "Error in FillSecurityFaceValue", "", ex)
            'errorinfo.send_error(ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub FillSecurityDetails()
        Try
            Dim ds As DataSet
            Dim dt As DataTable
            Dim lstParam As New List(Of SqlParameter)()
            OpenConn()

            'dt = objCommon.FillDataTable(sqlConn, "ID_FILL_Securitydetails", Val(Srh_NameofSecurity.SelectedId), "SecurityId")
            lstParam.Add(New SqlParameter("@SecurityId", Val(Srh_NameofSecurity.SelectedId)))
            If Trim(txt_SettmentDate.Text) <> "" Then
                lstParam.Add(New SqlParameter("@AsOn", Trim(txt_SettmentDate.Text)))
            End If
            ds = objComm.FillDetails(lstParam, "ID_FILL_DealSlipSecurityDetails")
            dt = ds.Tables(0)
            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    cbo_SecurityType.SelectedValue = Val(dt.Rows(0).Item("SecurityTypeId").ToString)
                    srh_IssuerOfSecurity.SearchTextBox.Text = dt.Rows(0).Item("SecurityIssuer").ToString
                    'srh_IssuerOfSecurity.SelectedId = dt.Rows(0).Item("SecurityIssuer").ToString
                    srh_IssuerOfSecurity.SelectedFieldText = dt.Rows(0).Item("SecurityIssuer").ToString
                    Srh_NameofSecurity.SearchTextBox.Text = dt.Rows(0).Item("SecurityName").ToString
                    Srh_NameofSecurity.SelectedFieldText = dt.Rows(0).Item("SecurityName").ToString
                    Hid_NSDLFaceValue.Value = Val(dt.Rows(0).Item("NSDLFaceValue").ToString)
                    Hid_SecurityFV.Value = Val(dt.Rows(0).Item("FaceValue").ToString)
                    Hid_FirstInterestDate.Value = Trim(dt.Rows(0).Item("FirstInterestDate") & "")
                    Hid_CouponRate.Value = Val(dt.Rows(0).Item("CouponRate") & "")
                    Hid_SecFaceValue.Value = Val(dt.Rows(0).Item("FaceValue").ToString)

                    'Hid_MaturityAmt.Value = Val(dt.Rows(0).Item("MaturityAmt") & "")
                    Hid_IssuePrice.Value = Val(dt.Rows(0).Item("FaceValue") & "")
                    txt_ISIN.Text = Trim(dt.Rows(0).Item("NSDLAcNumber") & "")
                    Hid_NorCompoundInt.Value = Trim(dt.Rows(0).Item("NormCompoundInt") & "")
                    Hid_CurrSecFaceValue.Value = Val(dt.Rows(0).Item("CurrSecFaceValue") & "")

                    If Trim(dt.Rows(0).Item("AccIntDays") & "") = "365" Then
                        rdo_AccIntDays.SelectedValue = 3
                    Else
                        rdo_AccIntDays.SelectedValue = 2
                    End If


                    InterestOnHoliday = ((dt.Rows(0).Item("InterestOnHoliday") & ""))
                    Hid_IntOnHoliday.Value = ((dt.Rows(0).Item("InterestOnHoliday") & ""))

                    InterestOnSat = ((dt.Rows(0).Item("InterestOnSat") & ""))
                    Hid_IntOnSat.Value = ((dt.Rows(0).Item("InterestOnSat") & ""))

                    MaturityOnHoliday = ((dt.Rows(0).Item("MaturityOnHoliday") & ""))
                    Hid_MatIntOnHoliday.Value = ((dt.Rows(0).Item("MaturityOnHoliday") & ""))

                    MaturityOnSat = ((dt.Rows(0).Item("MaturityOnSat") & ""))
                    Hid_MatIntOnSat.Value = ((dt.Rows(0).Item("MaturityOnSat") & ""))
                    If dt.Rows(0).Item("IsStaggered") Then
                        Hid_RateAmt.Value = "1"
                        tr_StaggMat.Visible = False
                        rdo_StaggMat.Enabled = False

                    Else

                        tr_StaggMat.Visible = False
                    End If


                    dgRedemptionDetails.DataSource = ds.Tables(1)
                    dgRedemptionDetails.DataBind()
                    If ds.Tables(1).Rows.Count > 0 Then
                        trRedemption.Visible = True
                        lbl_redem.Visible = True
                        txt_Amount.Enabled = False
                        cbo_Amount.Enabled = False

                        txt_Amount.Text = Val(txt_NoOfBonds.Text) * Val(Hid_CurrSecFaceValue.Value)
                        cbo_Amount.SelectedValue = "1"
                    Else
                        trRedemption.Visible = False
                        txt_Amount.Enabled = True
                        lbl_redem.Visible = False
                        cbo_Amount.Enabled = True
                    End If

                    'If (cbo_SecurityType.SelectedItem.Text.Contains("PREFERENCE")) Then
                    If Session("UserTypeId") = 1 Then
                        txt_InterestAmt.Enabled = True
                    Else
                        txt_InterestAmt.Enabled = False
                    End If

                End If

            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillSecurityDetails", "Error in FillSecurityDetails", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub FillBTBDEALFields()
        Try
            OpenConn()
            'CHANGE 
            Dim dt As DataTable

            If cbo_DealTransType.SelectedValue = "F" And rbl_TypeOFTranction.SelectedValue = "P" Then
                'dt = objCommon.FillDataTable(sqlConn, "Id_FILL_DealSlipEntry", Val(srh_FinancialDealSlipNo.SelectedId), "DealSlipID")
            Else
                dt = objCommon.FillDataTable(sqlConn, "Id_FILL_DealSlipEntry", Val(srh_BTBDealSlipNo.SelectedId), "DealSlipID")
            End If
            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    Srh_NameofSecurity.SelectedId = Val(dt.Rows(0).Item("SecurityId") & "")
                    FillSecurityDetails()
                    rdo_AccIntDays.SelectedValue = Hid_AccIntDays.Value
                    If Trim(cbo_DealTransType.SelectedValue) = "T" Or Trim(cbo_DealTransType.SelectedValue) = "B" Or Trim(cbo_DealTransType.SelectedValue) = "D" Or Trim(cbo_DealTransType.SelectedValue) = "F" Or Trim(cbo_DealTransType.SelectedValue) = "M" Or Trim(cbo_DealTransType.SelectedValue) = "O" Then
                        Hid_SingleRemainFV.Value = Val(dt.Rows(0).Item("RemainingFaceValue") & "")

                        If Val(dt.Rows(0).Item("RemainingFaceValue") & "") < Math.Round((txt_Amount.Text * cbo_Amount.SelectedValue), 4) Then
                            Dim strHtml As String
                            Dim msg As String = "Purchase Remaining Face value cannot be less than Sell Face Value"
                            strHtml = "alert('" + msg + "');"
                            'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msges", strHtml, True)
                            srh_BTBDealSlipNo.SearchTextBox.Text = ""
                        End If
                    Else

                    End If
                End If





            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillBTBDEALFields", "Error in FillBTBDEALFields", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function GetDealSlipNo(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandText = "ID_SEARCH_DealSlipNo"
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@TransType", SqlDbType.Char, 1, "I", , , Trim(rbl_TypeOFTranction.SelectedValue))
            If Hid_CostMemoPageName.Value = "PendingCostMemo.aspx" Then
                objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , Trim("D"))
            Else
                objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , Trim(cbo_DealTransType.SelectedValue))
            End If
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(cbo_Company.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
            objCommon.SetCommandParameters(sqlComm, "@NextNo", SqlDbType.VarChar, 50, "O")
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            txt_DealSlipNo.Text = Trim(sqlComm.Parameters("@NextNo").Value & "")
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "GetDealSlipNo", "Error in GetDealSlipNo", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)

            Return False
        End Try
    End Function

    Protected Sub Srh_NameofSecurity_ButtonClick() Handles Srh_NameofSecurity.ButtonClick
        Try

            FillSecurityDetails()
            Hid_SecId.Value = HttpUtility.HtmlEncode(objCommon.EncryptText(Convert.ToString(Srh_NameofSecurity.SelectedId)))
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "testaa", "showAddLinkRemove();", True)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Srh_NameofSecurity_ButtonClick", "Error in Srh_NameofSecurity_ButtonClick", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub FillQuoteFields(ByVal intQuoteId As Integer)
        Try
            OpenConn()
            Dim dt As DataTable

            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_QuoteList", intQuoteId, "QuoteId")
            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    rbl_TypeOFTranction.SelectedValue = (dt.Rows(0).Item("QuoteType") & "")
                    Srh_NameofSecurity.SelectedId = Val(dt.Rows(0).Item("SecurityId") & "")
                    Srh_NameofSecurity.SearchTextBox.Text = Trim(dt.Rows(0).Item("SecurityName") & "")
                    FillSecurityDetails()
                    Srh_NameOFClient.SelectedId = Val(dt.Rows(0).Item("CustomerId") & "")
                    Srh_NameOFClient.SearchTextBox.Text = dt.Rows(0).Item("CustomerName").ToString
                End If
            End If

            'FillSecurityDetails()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillQuoteFields", "Error in FillQuoteFields", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub


    Protected Sub cbo_Company_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Company.SelectedIndexChanged
        Try
            OpenConn()
            objCommon.FillControl(cbo_SGLWith, sqlConn, "ID_FILL_SGLBankMaster1", "BankName", "SGLId", Val(cbo_Company.SelectedValue), "CompId")
            objCommon.FillControl(cbo_Bank, sqlConn, "ID_FILL_BankMaster1", "BankAccInfo", "BankId", Val(cbo_Company.SelectedValue), "CompId")
            'objCommon.FillControl(cbo_OurBank, sqlConn, "ID_FILL_BankMaster1NEW", "BankName", "BankId", Val(cbo_Company.SelectedValue), "CompId")
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "cbo_Company_SelectedIndexChanged", "Error in cbo_Company_SelectedIndexChanged", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub btn_savegenerateDeal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_savegenerateDeal.Click
        Try
            If rdo_PurMethod.SelectedValue = "M" Then
                Delete()
            End If
            FillSecurityDetailsDeal()
            If Val(Hid_Frequency.Value) <> 0 Then
                FillAccuredInterestOptions()
                FillAccruedDetails()
            Else
            End If
            If rdo_SelectOpt.SelectedValue = "B" Then
                If Val(Hid_Frequency.Value) = 0 Then
                    Hid_Amt.Value = Val(txt_NoOfBonds.Text) * Val(txt_Rate.Text)
                    Hid_AddInterest.Value = 0
                Else
                    Hid_Amt.Value = (Val(txt_Rate.Text) * Val(txt_NoOfBonds.Text))
                End If
                Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
            ElseIf rdo_SelectOpt.SelectedValue = "F" Then
                Hid_Amt.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)) * Val(txt_Rate.Text) / 100)
                Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
            End If



            SetSaveUpdate("ID_UPDATE_DealSlipEntry", False, False)
            Hid_DealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_DealSlipId.Value))
            If StockAvailable <> "N" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "open", "DealReportView()", True)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ReadOnlyFields()
        Try
            rbl_TypeOFTranction.Enabled = False
            rbl_TypeOFTranction.Font.Bold = True
            If (Trim(Request.QueryString("page") & "") = "DealSlipDetail.aspx") And rbl_TypeOFTranction.SelectedValue = "S" Then
                rbl_DealSlipType.Enabled = True
            Else
                rbl_DealSlipType.Enabled = False
            End If

            If Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                If Hid_dealdone.Value Then
                    txt_InterestAmt.Enabled = False
                End If
            End If
            If (Trim(Request.QueryString("page") & "") <> "DealSlipDetail.aspx") Then
                rbl_DealSlipType.Font.Bold = True
                If Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                    rbl_DealType.Enabled = False
                Else
                    rbl_DealType.Enabled = True
                End If

                rbl_DealType.Font.Bold = True
                btn_CalRate.Visible = True
                'rbl_RefDealSlip.Enabled = False
                'rbl_RefDealSlip.Font.Bold = True

                'rdo_FinancialDealType.Enabled = False
                'rdo_FinancialDealType.Font.Bold = True
                cbo_SettDay.Enabled = False
                'rdo_SelectOpt.Enabled = False

                'rdo_PurMethod.Enabled = False
                'lnk_add.Enabled = False

                cbo_SecurityType.Enabled = False
                cbo_CustomerType.Enabled = False
                rdo_MarketType.Enabled = False
                rdo_MarketType.Font.Bold = True
                'srh_BTBDealSlipNo.SearchButton.Visible = False
                srh_IssuerOfSecurity.SearchButton.Attributes.Add("style", "display:none")
                Srh_NameofSecurity.SearchButton.Attributes.Add("style", "display:none")
                Srh_NameOFClient.Attributes.Add("style", "display:none")
                srh_BrokNameOfSeller.SearchButton.Attributes.Add("style", "display:none")

                'srh_IssuerOfSecurity.SearchTextBox.Width = "180"
                'Srh_NameofSecurity.SearchTextBox.Width = "180"
                'Srh_NameOFClient.SearchTextBox.Width = "180"
                'srh_BrokNameOfSeller.SearchTextBox.Width = "180"

                cbo_BrokCustomerType.Enabled = False
                'srh_BTBDealSlipNo.SearchButton.Visible = False
                srh_IssuerOfSecurity.SearchTextBox.Enabled = False
                srh_IssuerOfSecurity.SearchTextBox.Font.Bold = True
                Srh_NameofSecurity.SearchTextBox.Enabled = False
                Srh_NameofSecurity.SearchTextBox.Font.Bold = True
                Srh_NameOFClient.SearchTextBox.Enabled = False
                Srh_NameOFClient.SearchTextBox.Font.Bold = True
                srh_BrokNameOfSeller.SearchTextBox.Enabled = False
                srh_BrokNameOfSeller.SearchTextBox.Font.Bold = True
                chk_Brokerage1.Enabled = True
                txt_DealDate.Enabled = False
                txt_DealDate.Font.Bold = True
                rdo_RedeemedSec.Enabled = False
                'txt_CancelRemark.Enabled = False
                'txt_CancelRemark.Font.Bold = True


                'IMG1.Visible = False
                txt_SettmentDate.Enabled = False
                txt_SettmentDate.Font.Bold = True
                txt_DealSlipNo.Enabled = False
                txt_DealSlipNo.Font.Bold = True
                txt_Rate.Enabled = False
                txt_Rate.Font.Bold = True

                txt_Amount.Enabled = False
                txt_Amount.Font.Bold = True
                cbo_Amount.Enabled = False
                cbo_Amount.Font.Bold = True
                txt_NoOfBonds.Enabled = False
                txt_NoOfBonds.Font.Bold = True
                lbl_Dealar.Enabled = False
                lbl_Dealar.Font.Bold = True
                cbo_Company.Enabled = False
                cbo_Company.Font.Bold = True

                row_Bpto.Visible = True
                row_Bp.Visible = True
                row_BrFrom.Visible = True
                row_Br.Visible = True


                rdo_FinancialDealType.Enabled = False
                txt_ContactPerson.Enabled = True
                rdo_SelectOpt.Enabled = False

                rdo_PurMethod.SelectedValue = "M"
                rdo_PurMethod.Enabled = False



            End If





        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "ReadOnlyFields", "Error in ReadOnlyFields", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Sub ReadOnlyFieldsCheckerview()
        Try
            rbl_TypeOFTranction.Enabled = False
            rbl_TypeOFTranction.Font.Bold = True
            'rbl_DealSlipType.Enabled = False
            'rbl_DealSlipType.Font.Bold = True
            rbl_DealType.Enabled = False
            rbl_DealType.Font.Bold = True
            rbl_RefDealSlip.Enabled = False
            rbl_RefDealSlip.Font.Bold = True

            rdo_FinancialDealType.Enabled = False
            rdo_FinancialDealType.Font.Bold = True
            cbo_SettDay.Enabled = False
            rdo_SelectOpt.Enabled = False

            rdo_PurMethod.Enabled = False
            lnk_add.Enabled = False

            cbo_SecurityType.Enabled = False
            cbo_CustomerType.Enabled = False
            srh_BTBDealSlipNo.SearchButton.Visible = False
            srh_IssuerOfSecurity.SearchButton.Attributes.Add("style", "display:none")
            Srh_NameofSecurity.SearchButton.Attributes.Add("style", "display:none")
            Srh_NameOFClient.SearchButton.Attributes.Add("style", "display:none")
            srh_BrokNameOfSeller.SearchButton.Attributes.Add("style", "display:none")
            cbo_BrokCustomerType.Enabled = False
            srh_BTBDealSlipNo.SearchButton.Attributes.Add("style", "display:none")
            srh_IssuerOfSecurity.SearchTextBox.Enabled = False
            srh_IssuerOfSecurity.SearchTextBox.Font.Bold = True
            Srh_NameofSecurity.SearchTextBox.Enabled = False
            Srh_NameofSecurity.SearchTextBox.Font.Bold = True
            Srh_NameOFClient.SearchTextBox.Enabled = False
            Srh_NameOFClient.SearchTextBox.Font.Bold = True
            srh_BrokNameOfSeller.SearchTextBox.Enabled = False
            srh_BrokNameOfSeller.SearchTextBox.Font.Bold = True
            chk_Brokerage1.Enabled = False
            txt_DealDate.Enabled = False
            txt_DealDate.Font.Bold = True

            txt_CancelRemark.Enabled = False
            txt_CancelRemark.Font.Bold = True


            'IMG1.Visible = False
            txt_SettmentDate.Enabled = False
            txt_SettmentDate.Font.Bold = True
            txt_DealSlipNo.Enabled = False
            txt_DealSlipNo.Font.Bold = True
            txt_Rate.Enabled = False
            txt_Rate.Font.Bold = True

            txt_Amount.Enabled = False
            txt_Amount.Font.Bold = True
            cbo_Amount.Enabled = False
            cbo_Amount.Font.Bold = True
            txt_NoOfBonds.Enabled = False
            txt_NoOfBonds.Font.Bold = True
            lbl_Dealar.Enabled = False
            lbl_Dealar.Font.Bold = True
            cbo_Company.Enabled = False
            cbo_Company.Font.Bold = True
            chk_Brokerage1.Checked = True
            row_Bpto.Visible = True
            row_Bp.Visible = True
            row_BrFrom.Visible = True
            row_Br.Visible = True

            txt_BrokeragePaid.Enabled = False
            txt_BrokeragePaid.Font.Bold = True
            txt_Brokeragereceived.Enabled = False
            txt_Brokeragereceived.Font.Bold = True
            txt_ContactPerson.Enabled = False
            txt_ContactPerson.Font.Bold = True
            cbo_SettTurms.Enabled = False
            cbo_SettTurms.Font.Bold = True
            cbo_DealerName.Enabled = False
            cbo_DealerName.Font.Bold = True
            cbo_ReportedBy.Enabled = False
            cbo_ReportedBy.Font.Bold = True
            cbo_ReferenceByDealer.Enabled = False
            cbo_ReferenceByDealer.Font.Bold = True
            rbl_Reference.Enabled = False
            'Srh_ReferenceBy.SearchTextBox.Enabled = False
            'Srh_ReferenceBy.SearchButton.Visible = False
            'srh_SelectAddress.SearchTextBox.Enabled = False
            'srh_SelectAddress.SearchButton.Visible = False

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "ReadOnlyFieldsCheckerview", "Error in ReadOnlyFieldsCheckerview", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub VisibleFalseFields()
        Try
            row_BackOffice.Visible = False
        Catch ex As Exception
        End Try
    End Sub

    Private Sub FillAccuredInterestOptions()
        Try


            Dim datBookClosure As Date = Date.MinValue
            Dim blnDMAT As Boolean = True
            Dim FinalAmt As Double = 0
            Dim IntDate As Date = Date.MinValue
            Dim AddLess As String = ""
            Dim AddLessNoofDays As Integer = 0
            Dim Ratio As Double = 0
            Dim IntAmount As Double = 0
            Dim intDays As Int16 = 0
            Dim intIncludeInt As Int16 = 0
            Dim tmp_ExInt As Char

            MatDate = FillDateArrays(Hid_MatDate.Value)
            MatAmt = FillAmtArrays(Hid_MatAmt.Value)
            CoupDate = FillDateArrays(Hid_CoupDate.Value)
            CoupRate = FillAmtArrays(Hid_CoupRate.Value)

            With objCommon

                datYTM = .DateFormat(txt_SettmentDate.Text)
                datInterest = Hid_InterestDate.Value
                datBookClosure = Hid_BookClosureDate.Value
                blnNonGovernment = IIf(Hid_GovernmentFlag.Value = "N", True, False)
                blnRateActual = True

                secFaceValue = Val(Hid_SecFaceValue.Value)

                If rdo_SelectOpt.Items(0).Selected Then
                    decFaceValue = (txt_NoOfBonds.Text * Hid_NSDLFaceValue.Value)
                    txt_Amount.Text = decFaceValue / Val(cbo_Amount.SelectedValue)

                Else
                    'decFaceValue = (Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue))
                    'txt_NoOfBonds.Text = decFaceValue / Val(Hid_NSDLFaceValue.Value)
                End If

                'decFaceValue = (Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue))
                decRate = .DecimalFormat4(Val(txt_Rate.Text))
                datIssue = Hid_Issue.Value
                datBookClosure = IIf(blnDMAT = True, Hid_DMATBkDate.Value, Hid_BookClosureDate.Value)
                intBKDiff = CalculateBookClosureDiff(datBookClosure, "D", datInterest, blnNonGovernment)

                If blnNonGovernment = False Then
                    intDays = 360
                Else
                    If rdo_AccIntDays.SelectedValue = 2 Then
                        intDays = 366
                    Else
                        intDays = 365
                    End If

                End If
            End With
            Hid_Quantity.Value = ((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue))) / decFaceValue

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillAccuredInterestOptions", "Error in FillAccuredInterestOptions", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub CalcCompoundInt()
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            Dim sqlda As New SqlDataAdapter
            Dim sqldt As New DataTable
            Dim sqldv As New DataView
            datIssue = Hid_Issue.Value

            sqlComm.Connection = sqlConn
            sqlComm.CommandText = "ID_Fill_CompoundInt"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 8, "I", , , Val(Srh_NameofSecurity.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@IssueDate", SqlDbType.SmallDateTime, 4, "I", , , datIssue)
            objCommon.SetCommandParameters(sqlComm, "@SettmentDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_SettmentDate.Text))
            objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@ActualDays", SqlDbType.Int, 4, "I", , , Val(rdo_AccIntDays.SelectedItem.Text))


            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(sqldt)
            If sqldt.Rows.Count > 0 Then
                cIntr = sqldt.Rows(0).Item("CompoundInt")
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalcCompoundInt", "Error in CalcCompoundInt", "", ex)

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function FillDateArrays(ByVal strValue As String) As Date()
        Try
            Dim cmd
            cmd = Server.CreateObject("ADODB.Command")
            cmd.CommandTimeout = 120
            Dim strDate() As String
            Dim arrDate() As Date
            Dim I As Int32
            strDate = Split(strValue, "!")
            ReDim arrDate(strDate.Length - 2)
            'arrDate(0) = Date.MinValue
            For I = 0 To strDate.Length - 2
                If strDate(I) <> "" Then arrDate(I) = CDate(strDate(I))
            Next
            Return arrDate
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillDateArrays", "Error in FillDateArrays", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Private Function FillAmtArrays(ByVal strValue As String) As Double()
        Try
            Dim strDate() As String
            Dim arrDouble() As Double
            Dim I As Int32
            strDate = Split(strValue, "!")
            ReDim arrDouble(strDate.Length - 2)
            For I = 0 To strDate.Length - 2
                If strDate(I) <> "" Then arrDouble(I) = CDbl(strDate(I))
            Next
            Return arrDouble
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillAmtArrays", "Error in FillAmtArrays", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Function RoundToTwo(ByVal dec As Decimal) As Decimal
        Try
            Dim rounded As Decimal = Decimal.Round(dec, 2)
            rounded = Format(rounded, "###################0.00")
            Return rounded
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "RoundToTwo", "Error in RoundToTwo", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Private Sub FillSecurityDetailsDeal()
        Try
            OpenConn()
            Dim dt As DataTable
            Dim strSecurityNature As String = ""
            Dim I As Int32
            Dim datInfo As Date
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Val(Srh_NameofSecurity.SelectedId), "SecurityId")
            If dt IsNot Nothing Then
                For I = 0 To dt.Rows().Count - 1
                    With dt.Rows(I)
                        chrMaxActFlag = Trim(.Item("MaxActualFlag") & "")
                        strSecurityNature = Trim(.Item("NatureOfInstrument") & "")
                        srh_IssuerOfSecurity.SearchTextBox.Text = Trim(.Item("SecurityIssuer") & "")

                        Srh_NameofSecurity.SearchTextBox.Text = Trim(.Item("SecurityName") & "")
                        Hid_BookClosureDate.Value = IIf(Trim(.Item("BookClosureDate") & "") = "", Date.MinValue, .Item("BookClosureDate"))
                        Hid_InterestDate.Value = IIf(Trim(.Item("FirstInterestDate") & "") = "", Date.MinValue, .Item("FirstInterestDate"))
                        Hid_DMATBkDate.Value = IIf(Trim(.Item("DMATBookClosureDate") & "") = "", Date.MinValue, .Item("DMATBookClosureDate"))
                        Hid_Issue.Value = IIf(Trim(.Item("IssueDate") & "") = "", Date.MinValue, .Item("IssueDate"))
                        datInfo = IIf(Trim(.Item("SecurityInfoDate") & "") = "", Date.MinValue, .Item("SecurityInfoDate"))
                        Hid_GovernmentFlag.Value = Trim(.Item("GovernmentFlag") & "")
                        Hid_Frequency.Value = GetFrequency(Trim(.Item("FrequencyOfInterest") & ""))
                        Hid_NorCompoundInt.Value = Trim(dt.Rows(0).Item("NormCompoundInt") & "")

                        If Val(.Item("NSDLFaceValue") & "") <> 0 Then
                            Hid_FaceValue.Value = Val(.Item("NSDLFaceValue") & "")
                            Hid_IssuePrice.Value = Val(dt.Rows(0).Item("NSDLFaceValue") & "")
                        Else
                            Hid_FaceValue.Value = Val(.Item("FaceValue") & "")
                            Hid_IssuePrice.Value = Val(dt.Rows(0).Item("FaceValue") & "")
                        End If
                        InterestOnHoliday = ((dt.Rows(0).Item("InterestOnHoliday") & ""))
                        Hid_IntOnHoliday.Value = ((dt.Rows(0).Item("InterestOnHoliday") & ""))

                        InterestOnSat = ((dt.Rows(0).Item("InterestOnSat") & ""))
                        Hid_IntOnSat.Value = ((dt.Rows(0).Item("InterestOnSat") & ""))

                        MaturityOnHoliday = ((dt.Rows(0).Item("MaturityOnHoliday") & ""))
                        Hid_MatIntOnHoliday.Value = ((dt.Rows(0).Item("MaturityOnHoliday") & ""))

                        MaturityOnSat = ((dt.Rows(0).Item("MaturityOnSat") & ""))
                        Hid_MatIntOnSat.Value = ((dt.Rows(0).Item("MaturityOnSat") & ""))
                        'FillSecurityInfoDetails(datInfo, Val(.Item("SecurityInfoAmt") & ""), Trim(.Item("TypeFlag") & ""))

                        If rdo_TaxFree.SelectedValue = "Y" Then
                            FillSecurityInfoDetails(datInfo, Val(.Item("SecurityInfoAmt1") & ""), Trim(.Item("TypeFlag") & ""))
                        Else
                            FillSecurityInfoDetails(datInfo, Val(.Item("SecurityInfoAmt") & ""), Trim(.Item("TypeFlag") & ""))
                        End If

                    End With
                Next
                If strSecurityNature = "P" Then
                    Hid_CoupDate.Value += CStr(#12/31/9999#) & "!"
                    Hid_CoupRate.Value += dblPepCoupRate & "!"
                End If
                If Hid_GovernmentFlag.Value = "G" Then
                    'Hid_Days.Visible = False
                End If
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillSecurityDetailsDeal", "Error in FillSecurityDetailsDeal", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function GetFrequency(ByVal strFrequency As String) As Int16
        Try
            Select Case UCase(strFrequency)
                Case "Y"
                    Return 1
                Case "H"
                    Return 2
                Case "Q"
                    Return 4
                Case "M"
                    Return 12
                Case "N"
                    Return 0
            End Select
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetFrequency", "Error in GetFrequency", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Sub FillSecurityInfoDetails(ByVal InfoDate As Date, ByVal InfoAmt As Decimal, ByVal TypeFlag As String)
        Try
            Select Case TypeFlag
                Case "M"
                    Hid_MatDate.Value += InfoDate & "!"
                    Hid_MatAmt.Value += InfoAmt & "!"
                Case "I"
                    If InfoDate <> Date.MinValue Then
                        Hid_CoupDate.Value += InfoDate & "!"
                        If rdo_TaxFree.SelectedValue = "N" Then
                            Hid_CoupRate.Value += InfoAmt & "!"
                        Else
                            Hid_CoupRate1.Value += InfoAmt & "!"
                        End If
                    Else
                        dblPepCoupRate = InfoAmt
                    End If
                Case "C"
                    Hid_CallDate.Value += InfoDate & "!"
                    Hid_CallAmt.Value += InfoAmt & "!"
                Case "P"
                    Hid_PutDate.Value += InfoDate & "!"
                    Hid_PutAmt.Value += InfoAmt & "!"
            End Select
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillSecurityInfoDetails", "Error in FillSecurityInfoDetails", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Function SaveBlockedStockEntry(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            'CHANGE 
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn

            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@BlockedStockId", SqlDbType.SmallInt, 2, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@BlockedStockId", SqlDbType.SmallInt, 2, "I", , , Val(ViewState("Id")))
            End If
            'objCommon.SetCommandParameters(sqlComm, "@StockFaceValue", SqlDbType.BigInt, 8, "I", , , )
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Hid_UserId.Value))
            objCommon.SetCommandParameters(sqlComm, "@BlockedFaceValue", SqlDbType.BigInt, 8, "I", , , Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@Type", SqlDbType.Char, 1, "I", , , "H")
            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , Val(Srh_NameofSecurity.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@BlockedStockId").Value

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveBlockedStockEntry", "Error in SaveBlockedStockEntry", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Protected Sub cbo_SecurityType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SecurityType.SelectedIndexChanged
        srh_IssuerOfSecurity.SearchTextBox.Text = ""
        Srh_NameofSecurity.SearchTextBox.Text = ""
        txt_NoOfBonds.Text = ""
        txt_Amount.Text = ""
        If cbo_DealTransType.SelectedValue = "B" Then
            lst_addmultiple.Visible = False
        End If

        'If (cbo_SecurityType.SelectedItem.Text.Contains("PREFERENCE")) Then
        If Session("UserTypeId") = 1 Then
            txt_InterestAmt.Enabled = True
        Else
            txt_InterestAmt.Enabled = False
        End If


    End Sub

    Protected Sub srh_IssuerOfSecurity_ButtonClick() Handles srh_IssuerOfSecurity.ButtonClick
        Srh_NameofSecurity.SearchTextBox.Text = ""
        GetSecurityIssuer()
        If cbo_DealTransType.SelectedValue = "B" Then
            lst_addmultiple.Visible = False
        End If

    End Sub

    Protected Sub srh_BrokNameOfSeller_ButtonClick() Handles srh_BrokNameOfSeller.ButtonClick
        Try
            FillCustomerDetails()
            OpenConn()
            Dim dt As DataTable
            Dim dtBrokScheme As DataTable
            dt = objCommon.FillControl(Cbo_CounterCustDemat, sqlConn, "ID_FILL_CustomerdPDetails", "DPClient", "CustDPId", Val(srh_BrokNameOfSeller.SelectedId), "CustomerId")
            If cbo_CustDemate.Items.Count = 2 Then
                cbo_CustDemate.SelectedIndex = 1
            End If
            dt = objCommon.FillControl(cbo_CounterCustSGLWith, sqlConn, "ID_FILL_CustomerSGLDetails", "SGLTransWith", "CustSGLId", Val(srh_BrokNameOfSeller.SelectedId), "CustomerId")

            If cbo_CustSGL.Items.Count = 2 Then
                cbo_CustSGL.SelectedIndex = 1
            End If

            dt = objCommon.FillControl(cbo_SellerCustomerBank, sqlConn, "ID_FILL_CustomerBankDetailS", "BankName", "CustBankId", Val(srh_BrokNameOfSeller.SelectedId), "CustomerId")
            If cbo_SellerCustomerBank.Items.Count = 2 Then
                cbo_SellerCustomerBank.SelectedIndex = 1
            End If

            'dtBrokScheme = objCommon.FillControl(cbo_BrokCustomerScheme, sqlConn, "Id_FILL_ClientCustomerScheme", "SchemeName", "CustomerSchemeId", Val(srh_BrokNameOfSeller.SelectedId), "CustomerId")
            'If dtBrokScheme.Rows.Count >= 1 Then
            '    row_BrokCustomerScheme.Visible = True
            '    'cbo_BrokCustomerScheme.SelectedValue = Val(dtBrokScheme.Rows(0).Item("CustomerSchemeId") & "")
            'Else
            '    row_BrokCustomerScheme.Visible = False
            'End If

            txt_CountContactPerson.Text = ""
            FillFieldsNewCount()
            srh_SelectAddress.SelectedId = 0
            srh_SelectAddress.SearchTextBox.Text = ""

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "srh_BrokNameOfSeller_ButtonClick", "Error in srh_BrokNameOfSeller_ButtonClick", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Protected Sub NameOFClient_ButtonClick() Handles Srh_NameOFClient.ButtonClick
        Try
            FillCustomerDetails()
            OpenConn()
            Dim dt As DataTable
            Dim dtScheme As DataTable

            objCommon.FillControl(cbo_CustDemate, sqlConn, "ID_FILL_CustomerdPDetails", "DPClient", "CustDPId", Val(Srh_NameOFClient.SelectedId), "CustomerId")
            If cbo_CustDemate.Items.Count = 2 Then
                cbo_CustDemate.SelectedIndex = 1
            End If
            dt = objCommon.FillControl(cbo_CustSGL, sqlConn, "ID_FILL_CustomerSGLDetails", "SGLTransWith", "CustSGLId", Val(Srh_NameOFClient.SelectedId), "CustomerId")

            If cbo_CustSGL.Items.Count = 2 Then
                cbo_CustSGL.SelectedIndex = 1
            End If

            dt = objCommon.FillControl(cbo_CustomerBank, sqlConn, "ID_FILL_CustomerBankDetailS", "BankAcc", "CustBankId", Val(Srh_NameOFClient.SelectedId), "CustomerId")
            If cbo_CustomerBank.Items.Count = 2 Then
                cbo_CustomerBank.SelectedIndex = 1
            End If
            'dtScheme = objCommon.FillControl(cbo_CustomerScheme, sqlConn, "Id_FILL_ClientCustomerScheme", "SchemeName", "CustomerSchemeId", Val(Srh_NameOFClient.SelectedId), "CustomerId")
            'If dtScheme.Rows.Count >= 1 Then
            '    row_CustomerScheme.Visible = True
            '    'cbo_CustomerScheme.SelectedValue = Val(dtScheme.Rows(0).Item("CustomerSchemeId") & "")
            'Else
            '    row_CustomerScheme.Visible = False
            'End If




            txt_ContactPerson.Text = ""
            srh_SelectAddress.SelectedId = 0
            srh_SelectAddress.SearchTextBox.Text = ""
            FillFieldsNew()
            Hid_CustId.Value = HttpUtility.HtmlEncode(objCommon.EncryptText(Convert.ToString(Srh_NameOFClient.SelectedId)))
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "testaa", "showAddLinkRemove();", True)

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "NameOFClient_ButtonClick", "Error in NameOFClient_ButtonClick", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub FillCustomerDetails()
        Try
            OpenConn()
            Dim dt As DataTable

            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_ClientCustomerMaster", Val(Srh_NameOFClient.SelectedId), "CustomerId")
            lbl_PAN.Text = Trim(dt.Rows(0).Item("PANNumber") & "")
            If Val(dt.Rows(0).Item("CustomerDealerName") & "") <> 0 Then
                cbo_DealerName.SelectedValue = Val(dt.Rows(0).Item("CustomerDealerName") & "")
            End If
            'dtScheme = objCommon.FillDataTable(sqlConn, "Id_FILL_ClientCustomerScheme", Val(Srh_NameOFClient.SelectedId), "CustomerId")
            If cbo_DealTransType.SelectedValue <> "B" Then
                If dt IsNot Nothing Then
                    If dt.Rows.Count > 0 Then
                        cbo_CustomerType.SelectedValue = Val(dt.Rows(0).Item("CustomerTypeId") & "")
                    End If
                End If
            End If
            'If dtScheme IsNot Nothing Then
            '    If dtScheme.Rows.Count > 0 Then
            '        row_CustomerScheme.Visible = True
            '        cbo_CustomerScheme.SelectedValue = Val(dtScheme.Rows(0).Item("CustomerSchemeId") & "")
            '    End If
            'End If

            objCommon.FillControl(cbo_BrokeragePaidTo, sqlConn, "ID_FILL_BrokerMasterNew", "BrokerName", "BrokerId", Val(Srh_NameOFClient.SelectedId), "Customerid")
            objCommon.FillControl(cbo_BrokeragereceivedFrom, sqlConn, "ID_FILL_BrokerMasterNew", "BrokerName", "BrokerId", Val(Srh_NameOFClient.SelectedId), "Customerid")

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillCustomerDetails", "Error in FillCustomerDetails", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Protected Sub srh_BTBDealSlipNo_ButtonClick() Handles srh_BTBDealSlipNo.ButtonClick
        Try
            lbl_Msg.Text = ""
            FillBTBDEALFields()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "testaa", "showAddLinkRemove();", True)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "srh_BTBDealSlipNo_ButtonClick", "Error in srh_BTBDealSlipNo_ButtonClick", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try

    End Sub
    Private Function SaveProfit(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@SellDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            If strProc = "ID_INSERT_PROFITCALC_Financial_SALE" Then
                objCommon.SetCommandParameters(sqlComm, "@FinancialDealType", SqlDbType.Char, 1, "I", , , Trim(rdo_FinancialDealType.SelectedValue))
            End If
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveProfit", "Error in SaveProfit", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
            Return False
        End Try
    End Function
    Private Function SavePurchaseProfit(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@PurcDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SavePurchaseProfit", "Error in SavePurchaseProfit", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
            Return False
        End Try
    End Function
    Private Function SavePurchaseProfitTD(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@PurchaseDealslipid", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SavePurchaseProfitTD", "Error in SavePurchaseProfitTD", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
            Return False
        End Try
    End Function

    Private Sub GetSecurityIssuer()
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_GET_SecurityName"
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@SecurityIssuer", SqlDbType.VarChar, 500, "I", , , Trim(srh_IssuerOfSecurity.SearchTextBox.Text))
            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 8, "O")
            objCommon.SetCommandParameters(sqlComm, "@SecurityName", SqlDbType.VarChar, 50, "O")
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            If Trim(sqlComm.Parameters("@SecurityName").Value) <> "" Then
                Srh_NameofSecurity.SelectedId = Val(sqlComm.Parameters("@SecurityId").Value & "")
                Srh_NameofSecurity.SelectedFieldText = Trim(sqlComm.Parameters("@SecurityName").Value & "")
                Srh_NameofSecurity_ButtonClick()
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetSecurityIssuer", "Error in GetSecurityIssuer", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Protected Sub btn_ConvertToDeal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ConvertToDeal.Click
        Try
            'GetDealSlipNo()


            FillSecurityDetailsDeal()
            If Val(Hid_Frequency.Value) <> 0 Then
                'FillAccuredInterestOptions()
                FillAccruedDetails()

            End If
            If rdo_SelectOpt.SelectedValue = "B" Then
                If Val(Hid_Frequency.Value) = 0 Then
                    Hid_Amt.Value = Val(txt_NoOfBonds.Text) * Val(txt_Rate.Text)
                    Hid_AddInterest.Value = 0
                Else
                    Hid_Amt.Value = (Val(txt_Rate.Text) * Val(txt_NoOfBonds.Text))
                End If
                Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
            ElseIf rdo_SelectOpt.SelectedValue = "F" Then
                Hid_Amt.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)) * Val(txt_Rate.Text) / 100)
                Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
            End If
            SetSaveUpdate("ID_UPDATE_DealSlipEntry", False, False)
            Response.Redirect("PendingDealSlip.aspx")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Protected Sub srh_SelectAddress_ButtonClick() Handles srh_SelectAddress.ButtonClick
        Try
            Dim Id As String = ""
            Id = srh_SelectAddress.SelectedId
            srh_SelectAddress.SearchTextBox.Text = srh_SelectAddress.SelectedFieldText
            Hid_MultiAddrId.Value = Id
            If Hid_MultiAddrId.Value <> "" Then
                FillMultiAddrContact()
            Else
                FillFieldsNew()
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "srh_SelectAddress_ButtonClick", "Error in srh_SelectAddress_ButtonClick", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub lnk_add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_add.Click
        lbl_Msg.Text = ""
        Delete()
        FillListBox()
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "testaa", "showAddLinkRemove();", True)
    End Sub
    Private Sub FillListBox()
        Try

            Dim I As Integer = 1
            Dim dt As DataTable = CType(JsonConvert.DeserializeObject(Hid_Marked.Value, (GetType(DataTable))), DataTable)

            Hid_PurchaseDealSlipId.Value = ""
            Hid_DealSlipNo.Value = ""
            Hid_RemainingFaceValue.Value = ""

            lst_addmultiple.Items.Clear()
            For Each row As DataRow In dt.Rows
                lst_addmultiple.Items.Add(New ListItem(Trim(row("DealSlipNo") & ""), Trim(row("DealSlipId") & "")))
            Next

            If lst_addmultiple.Items.Count > 0 Then
                srh_IssuerOfSecurity.SearchButton.Attributes.Add("style", "display:none")
                srh_IssuerOfSecurity.SearchTextBox.Enabled = False
                Srh_NameofSecurity.SearchButton.Attributes.Add("style", "display:none")
                Srh_NameofSecurity.SearchTextBox.Enabled = False
                txt_Amount.ReadOnly = True
                cbo_Amount.Enabled = False
                cbo_DealTransType.Enabled = False
                cbo_SecurityType.Enabled = False
                cbo_CustomerType.Enabled = False
            Else
                srh_IssuerOfSecurity.SearchButton.Attributes.Add("style", "display:")
                srh_IssuerOfSecurity.SearchTextBox.Enabled = True
                Srh_NameofSecurity.SearchButton.Attributes.Add("style", "display:")
                Srh_NameofSecurity.SearchTextBox.Enabled = True
                txt_Amount.ReadOnly = False
                cbo_Amount.Enabled = True
                cbo_DealTransType.Enabled = False
                cbo_SecurityType.Enabled = True
                cbo_CustomerType.Enabled = True
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillListBox", "Error in FillListBox", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try

    End Sub
    Protected Sub srh_ContactPerson_linkbutton() Handles srh_ContactPerson.LinkClick
        Try
            txt_ContactPerson.Text = ""
            Dim itm As ListItem = srh_ContactPerson.SelectListBox.Items.FindByText("")
            If itm IsNot Nothing Then srh_ContactPerson.SelectListBox.Items.Remove(itm)
            For Each lstItem As ListItem In srh_ContactPerson.SelectListBox.Items
                txt_ContactPerson.Text = txt_ContactPerson.Text & lstItem.Text & " / "
            Next
            If txt_ContactPerson.Text.Length > 0 Then
                txt_ContactPerson.Text = Left(txt_ContactPerson.Text, txt_ContactPerson.Text.Length - 3)
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "srh_ContactPerson_linkbutton", "Error in srh_ContactPerson_linkbutton", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
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


    Private Function GetPurchaseSettDate(ByVal sqlTrans As SqlTransaction, ByVal intdealslipid As Integer) As Boolean
        Try
            Dim sqlComm As New SqlCommand

            sqlComm.CommandText = "ID_FILL_PurcSettAmt"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            sqlComm.Transaction = sqlTrans
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(intdealslipid))
            objCommon.SetCommandParameters(sqlComm, "@PurcSettDate", SqlDbType.SmallDateTime, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Char, 1, "O")
            sqlComm.ExecuteNonQuery()
            PurSettDate = sqlComm.Parameters("@PurcSettDate").Value
            chrIntFlag = sqlComm.Parameters("@IntFlag").Value

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "GetPurchaseSettDate", "Error in GetPurchaseSettDate", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Sub CalcAmt()
        FillSecurityDetailsDeal()
        If Val(Hid_Frequency.Value) <> 0 Then
            'FillAccuredInterestOptions()
            FillAccruedDetails()
        Else
            Hid_Amt.Value = Val(txt_NoOfBonds.Text) * Val(txt_Rate.Text)
            Hid_SettlementAmt.Value = Val(txt_NoOfBonds.Text) * Val(txt_Rate.Text)

        End If
        If rdo_SelectOpt.SelectedValue = "B" Then
            If Val(Hid_Frequency.Value) = 0 Then
                Hid_Amt.Value = Val(txt_NoOfBonds.Text) * Val(txt_Rate.Text)
                Hid_AddInterest.Value = 0
            Else
                Hid_Amt.Value = (Val(txt_Rate.Text) * Val(txt_NoOfBonds.Text))
            End If
            Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
        ElseIf rdo_SelectOpt.SelectedValue = "F" Then
            Hid_Amt.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)) * Val(txt_Rate.Text) / 100)
            Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
        End If



    End Sub
    Private Sub FillAccruedDetails()
        Try
            OpenConn()
            Dim dt As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = "ID_Fill_QuoteEntry_AccruedDetails"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn

            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 50, "I", , , Val(Srh_NameofSecurity.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@SettlementDate", SqlDbType.SmallDateTime, 9, "I", , , objCommon.DateFormat(txt_SettmentDate.Text))
            objCommon.SetCommandParameters(sqlComm, "@TotalFaceValue", SqlDbType.Decimal, 18, "I", , , Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@Rate", SqlDbType.Decimal, 9, "I", , , Val(txt_Rate.Text))
            objCommon.SetCommandParameters(sqlComm, "@StepUp", SqlDbType.Char, 1, "I", , , Trim(rdo_TaxFree.SelectedValue))
            sqlComm.ExecuteNonQuery()

            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            If dt.Rows.Count > 0 Then
                Hid_AddInterest.Value = Val(dt.Rows(0).Item("AccruedInterest") & "")
                Hid_IntDays.Value = Val(dt.Rows(0).Item("AccruedDays") & "")
                Hid_InterestFromTo.Value = Trim(dt.Rows(0).Item("AccruedDates") & "")
                Hid_Amtshow.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)) * Val(txt_Rate.Text) / 100)
                txt_InterestAmt.Text = Val(dt.Rows(0).Item("AccruedInterest") & "")

                If rdo_SelectOpt.SelectedValue = "B" Then
                    If Hid_Frequency.Value = 0 Then
                        Hid_Amt.Value = Val(txt_NoOfBonds.Text) * Val(txt_Rate.Text)
                        Hid_AddInterest.Value = 0
                    Else
                        'Hid_Amt.Value = (Val(txt_Rate.Text) * Val(Hid_IssuePrice.Value) * Val(txt_NoOfBonds.Text)) / 100
                        Hid_Amt.Value = (Val(txt_Rate.Text) * Val(txt_NoOfBonds.Text))
                    End If

                    Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                ElseIf rdo_SelectOpt.SelectedValue = "F" Then
                    If cbo_SecurityType.SelectedValue = 16 Then
                        Hid_Amt.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue) * Val(txt_Rate.Text) / 100))
                    Else
                        Hid_Amt.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)) * Val(txt_Rate.Text) / 100)
                    End If

                    Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                End If
                Hid_Amtshow.Value = Hid_Amt.Value
                ' Hid_Amt.Value = RoundToTwo(Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue) * Val(txt_Rate.Text) / 100)
                Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                'If (Not cbo_SecurityType.SelectedItem.Text.Contains("PREFERENCE")) Then
                If Session("UserTypeId") <> 1 Then
                    If Val(dt.Rows(0).Item("AccruedInterest") & "") < 0 Then
                        Hid_IntDays.Value = -1 * Val(dt.Rows(0).Item("AccruedDays") & "")
                        Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                        'lbl_TotalSettlementAmt.Text = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value) + Val(dt.Rows(0).Item("StampDutyAmt") & "") + Val(dt.Rows(0).Item("TCSAmount") & "") - Val(dt.Rows(0).Item("TDSAmount") & "")
                        lbl_TotalSettlementAmt.Text = objCommon.DecimalFormat(Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value))
                        lbl_InterestDays.Text = Hid_IntDays.Value
                        txt_InterestAmt.Text = Hid_AddInterest.Value
                        txt_InterestAmt.ForeColor = Drawing.Color.Red
                        lbl_InterestDays.ForeColor = Drawing.Color.Red
                        lbl_SettlementAmt.Text = Format(Trim(Hid_SettlementAmt.Value & ""), "Standard")
                        Hid_AddInterest.Value = Format(Trim(Hid_AddInterest.Value & ""), "Standard")
                        lbl_StampDuty.Text = Val(Hid_StampDutyAmt.Value)
                        lbl_TCSAmount.Text = Val(Hid_TCSAmount.Value)

                    Else

                        lbl_InterestDays.Text = Hid_IntDays.Value
                        lbl_InterestFromToDates.Text = Trim(Hid_InterestFromTo.Value & "")
                        Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                        lbl_InterestFromToDates.Text = Hid_InterestFromTo.Value
                        lbl_TotalSettlementAmt.Text = objCommon.DecimalFormat(Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)) '+ Val(dt.Rows(0).Item("StampDutyAmt") & "") + Val(dt.Rows(0).Item("TCSAmount") & "")
                        lbl_Amount.Text = Format(Trim(Hid_Amt.Value & ""), "Standard")
                        txt_InterestAmt.Text = Format(Trim(Hid_AddInterest.Value & ""), "Standard")
                        lbl_SettlementAmt.Text = Format(Trim(Hid_SettlementAmt.Value & ""), "Standard")
                        lbl_StampDuty.Text = Val(Hid_StampDutyAmt.Value)
                        lbl_TCSAmount.Text = Val(Hid_TCSAmount.Value)
                    End If
                Else
                    If Val(dt.Rows(0).Item("AccruedInterest") & "") < 0 Then
                        Hid_IntDays.Value = -1 * Val(dt.Rows(0).Item("AccruedDays") & "")
                        Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(dt.Rows(0).Item("AccruedInterest") & "")
                        lbl_TotalSettlementAmt.Text = objCommon.DecimalFormat(Val(Hid_Amt.Value) + Val(dt.Rows(0).Item("AccruedInterest") & "")) '+ Val(dt.Rows(0).Item("StampDutyAmt") & "") + Val(dt.Rows(0).Item("TCSAmount") & "")
                        lbl_InterestDays.Text = Hid_IntDays.Value
                        lbl_InterestDays.ForeColor = Drawing.Color.Red
                        txt_InterestAmt.ForeColor = Drawing.Color.Red
                        lbl_SettlementAmt.Text = Format(Trim(Hid_SettlementAmt.Value & ""), "Standard")
                        lbl_StampDuty.Text = Val(Hid_StampDutyAmt.Value)
                        lbl_TCSAmount.Text = Val(Hid_TCSAmount.Value)
                    Else

                        lbl_InterestDays.Text = Hid_IntDays.Value
                        lbl_InterestFromToDates.Text = Trim(Hid_InterestFromTo.Value & "")
                        Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(dt.Rows(0).Item("AccruedInterest") & "")
                        lbl_InterestFromToDates.Text = Hid_InterestFromTo.Value
                        lbl_TotalSettlementAmt.Text = objCommon.DecimalFormat(Val(Hid_Amt.Value) + Val(dt.Rows(0).Item("AccruedInterest") & "")) '+ Val(Hid_StampDutyAmt.Value)
                        lbl_Amount.Text = Format(Trim(Hid_Amt.Value & ""), "Standard")
                        lbl_SettlementAmt.Text = Format(Trim(Hid_SettlementAmt.Value & ""), "Standard")
                        lbl_StampDuty.Text = Val(Hid_StampDutyAmt.Value)
                        lbl_TCSAmount.Text = Val(Hid_TCSAmount.Value)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub


    Protected Sub srh_BrokingBTBDealSlipNo_ButtonClick() Handles srh_BrokingBTBDealSlipNo.ButtonClick
        fillBrokingfields()
        brokingreadonlyfields()

    End Sub
    Private Sub brokingreadonlyfields()
        rbl_TypeOFTranction.Enabled = False
        rbl_DealSlipType.Enabled = False
        cbo_DealTransType.Enabled = False
        '  srh_RefDealSlipNo.SearchButton.Visible = False
        'srh_FinancialDealSlipNo.SearchButton.Visible = False
        txt_DealSlipNo.ReadOnly = True
        cbo_SecurityType.Enabled = False
        cbo_CustomerType.Enabled = False
        srh_IssuerOfSecurity.SearchButton.Attributes.Add("style", "display:none")
        Srh_NameofSecurity.SearchButton.Attributes.Add("style", "display:none")
        lst_addmultiple.Visible = False
        srh_BrokingBTBDealSlipNo.SearchButton.Visible = True
        Srh_NameOFClient.SearchButton.Attributes.Add("style", "display:none")
        srh_BrokNameOfSeller.SearchButton.Attributes.Add("style", "display:none")
        cbo_BrokCustomerType.Enabled = False
        'txt_ContactPerson.ReadOnly = True
        'srh_SelectAddress.SearchButton.Visible = False
        txt_DealDate.ReadOnly = True
        txt_SettmentDate.ReadOnly = True
        rdo_SelectOpt.Enabled = False
        txt_Amount.ReadOnly = True
        cbo_Amount.Enabled = False
        txt_NoOfBonds.ReadOnly = True
        txt_Rate.ReadOnly = True
        rdo_PhysicalDMAT.Enabled = False
        rdo_AccIntDays.Enabled = False
        'IMG1.Visible = False
        'IMG2.Visible = False
    End Sub
    Private Sub fillBrokingfields()
        Try
            Dim dt As DataTable
            OpenConn()
            Dim cmd
            cmd = Server.CreateObject("ADODB.Command")
            cmd.CommandTimeout = 120
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_DealSlipEntry", Val(srh_BrokingBTBDealSlipNo.SelectedId), "DealSlipID")
            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    cbo_SettDay.SelectedValue = Val(dt.Rows(0).Item("SettDays") & "")
                    rbl_DealSlipType.SelectedValue = Trim(dt.Rows(0).Item("DealSlipType") & "")

                    cbo_DealTransType.SelectedValue = Trim(dt.Rows(0).Item("DealTransType") & "")

                    If (rbl_DealSlipType.SelectedValue = "C") Then
                        Hid_CostMemoNo.Value = Trim(dt.Rows(0).Item("DealSlipNo") & "")
                    End If
                    Srh_NameofSecurity.SelectedId = Val(dt.Rows(0).Item("SecurityId") & "")
                    Hid_SecurityId.Value = Val(dt.Rows(0).Item("SecurityId") & "")
                    Srh_NameOFClient.SelectedId = Val(dt.Rows(0).Item("BrokCustomerId") & "")
                    Srh_NameOFClient.SearchTextBox.Text = dt.Rows(0).Item("BrokCustName").ToString
                    Srh_NameOFClient.SelectedFieldText = dt.Rows(0).Item("BrokCustName").ToString
                    If cbo_DealTransType.SelectedValue = "B" Then
                        srh_BrokNameOfSeller.SelectedId = Val(dt.Rows(0).Item("CustomerId") & "")
                        srh_BrokNameOfSeller.SearchTextBox.Text = dt.Rows(0).Item("CustomerName").ToString
                        srh_BrokNameOfSeller.SelectedFieldText = dt.Rows(0).Item("CustomerName").ToString
                        cbo_BrokCustomerType.SelectedValue = Val(dt.Rows(0).Item("BrokCustomerTypeId") & "")
                    End If
                    If rbl_Reference.SelectedValue = "Y" Then
                        objCommon.FillControl(cbo_refCustType, sqlConn, "ID_FILL_CustomerTypeMaster1", "CustomerTypeName", "CustomerTypeId")
                        cbo_refCustType.SelectedValue = Val(dt.Rows(0).Item("RefCustomerTypeId") & "")
                    End If

                    txt_Amount.Text = Val(dt.Rows(0).Item("FaceValue") & "")
                    cbo_Amount.SelectedValue = Val(dt.Rows(0).Item("FaceValueMultiple") & "")
                    txt_NoOfBonds.Text = Val(dt.Rows(0).Item("NoOfBond") & "")
                    Hid_bond.Value = Val(dt.Rows(0).Item("NoOfBond") & "")
                    txt_DealDate.Text = Format(dt.Rows(0).Item("DealDate"), "dd/MM/yyyy")
                    txt_SettmentDate.Text = Format(dt.Rows(0).Item("SettmentDate"), "dd/MM/yyyy")
                    txt_Rate.Text = Val(dt.Rows(0).Item("Rate") & "")
                    rdo_PhysicalDMAT.SelectedValue = Trim(dt.Rows(0).Item("ModeofDelivery") & "")

                    cbo_ModeOfPayment.SelectedValue = Trim(dt.Rows(0).Item("ModeOFPayment") & "")
                    cbo_Company.SelectedValue = Val(dt.Rows(0).Item("CompId") & "")
                    If Trim(dt.Rows(0).Item("AccIntDays") & "") Then
                        rdo_AccIntDays.SelectedValue = Trim(dt.Rows(0).Item("AccIntDays") & "")
                        Hid_AccIntDays.Value = Trim(dt.Rows(0).Item("AccIntDays") & "")
                    End If

                    Hid_DealerName.Value = Trim(dt.Rows(0).Item("DealerName") & "")
                    lbl_Dealar.Text = Trim(Hid_DealerName.Value)

                    objCommon.FillControl(cbo_DealerName, sqlConn, "ID_FILL_Dealer", "NameOfUser", "UserId")
                    Hid_UserId.Value = Val(dt.Rows(0).Item("UserId") & "")
                    cbo_DealerName.SelectedValue = Val(dt.Rows(0).Item("UserId") & "")
                    rbl_DealType.SelectedValue = Trim(dt.Rows(0).Item("SelectMethod") & "")
                    rdo_PurMethod.SelectedValue = Trim(dt.Rows(0).Item("PurMethod") & "")

                    rbl_RefDealSlip.SelectedValue = Trim(dt.Rows(0).Item("RefDealSlip") & "")
                    srh_BTBDealSlipNo.SelectedId = Val(dt.Rows(0).Item("BTBDealSlipId") & "")
                    srh_BTBDealSlipNo.SearchTextBox.Text = dt.Rows(0).Item("BTBdealslipno").ToString
                    srh_RefDealSlipNo.SelectedId = Val(dt.Rows(0).Item("RefDealSlipId") & "")
                    srh_RefDealSlipNo.SearchTextBox.Text = dt.Rows(0).Item("RefDealSlipNo").ToString

                    chk_Brokerage1.Checked = dt.Rows(0).Item("BrockEntry")
                    Hid_dealdone.Value = Val(dt.Rows(0).Item("DealDone"))
                    txt_CancelRemark.Text = Trim(dt.Rows(0).Item("FORemark") & "")
                    LastIPDate = IIf(Trim(dt.Rows(0).Item("LastIpDate") & "") = "", Date.MinValue, dt.Rows(0).Item("LastIpDate")) 'dt.Rows(0).Item("LastIpDate")
                    cbo_SettTurms.SelectedValue = Trim(dt.Rows(0).Item("SettTurms") & "")
                    txt_PreviousInterest.Text = Val(dt.Rows(0).Item("PreviousInterest") & "")
                    txt_CouponPaid.Text = Val(dt.Rows(0).Item("CouponPaid") & "")


                    rbl_DealSlipType.Enabled = False
                    rbl_DealType.Enabled = False
                    cbo_DealTransType.Enabled = False

                    rbl_RefDealSlip.Enabled = False
                    ' srh_RefDealSlipNo.SearchButton.Visible = False
                    Hid_RemainingFaceValue.Value = Trim(dt.Rows(0).Item("PurcRemFaceValue") & "")
                    Hid_SingleRemainFV.Value = Val(dt.Rows(0).Item("SingleRemFaceValue") & "")

                    rdo_NilComm.SelectedValue = Trim(dt.Rows(0).Item("NilComm") & "")



                    If rdo_PhysicalDMAT.SelectedValue = "S" Then

                        If Val(dt.Rows(0).Item("SGLId") & "") <> 0 Then
                            objCommon.FillControl(cbo_SGLWith, sqlConn, "ID_FILL_SGLBankMaster1", "BankName", "SGLId", Val(cbo_Company.SelectedValue), "CompId")
                            cbo_SGLWith.SelectedValue = Val(dt.Rows(0).Item("SGLId") & "")
                        Else
                            objCommon.FillControl(cbo_SGLWith, sqlConn, "ID_FILL_SGLBankMaster1", "BankName", "SGLId", Val(cbo_Company.SelectedValue), "CompId")
                        End If

                    ElseIf Val(dt.Rows(0).Item("DMatId") & "") <> 0 Then
                        objCommon.FillControl(cbo_Demat, sqlConn, "ID_FILL_DMATMaster1", "DematClientInfo", "DMatId", Val(cbo_Company.SelectedValue), "CompId")
                        'cbo_Demat.SelectedValue = Val(dt.Rows(0).Item("DMatId") & "")
                    Else
                        objCommon.FillControl(cbo_Demat, sqlConn, "ID_FILL_DMATMaster1", "DematClientInfo", "DMatId", Val(cbo_Company.SelectedValue), "CompId")
                        objCommon.FillControl(cbo_SGLWith, sqlConn, "ID_FILL_SGLBankMaster1", "BankName", "SGLId", Val(cbo_Company.SelectedValue), "CompId")
                    End If



                    If rdo_PhysicalDMAT.SelectedValue = "D" Then
                        objCommon.FillControl(Cbo_CounterCustDemat, sqlConn, "ID_FILL_CustomerdPDetails", "DPClient", "CustDPId", Val(srh_BrokNameOfSeller.SelectedId), "CustomerId")
                        objCommon.FillControl(cbo_CustDemate, sqlConn, "ID_FILL_CustomerdPDetails", "DPClient", "CustDPId", Val(Srh_NameOFClient.SelectedId), "CustomerId")

                        If Val(dt.Rows(0).Item("CounterPartyDmatid") & "") <> 0 Then
                            cbo_CustDemate.SelectedValue = Val(dt.Rows(0).Item("CounterPartyDmatid") & "")
                        End If

                        If Val(dt.Rows(0).Item("CustDPId") & "") <> 0 Then
                            objCommon.FillControl(cbo_CustDemate, sqlConn, "ID_FILL_CustomerdPDetails", "DPClient", "CustDPId", Val(Srh_NameOFClient.SelectedId), "CustomerId")
                            Cbo_CounterCustDemat.SelectedValue = Val(dt.Rows(0).Item("CustDPId") & "")
                        End If


                    Else
                        objCommon.FillControl(cbo_CustSGL, sqlConn, "ID_FILL_CustomerSGLDetails", "SGLTransWith", "CustSGLId", Val(Srh_NameOFClient.SelectedId), "CustomerId")
                        objCommon.FillControl(cbo_CounterCustSGLWith, sqlConn, "ID_FILL_CustomerSGLDetails", "SGLTransWith", "CustSGLId", Val(srh_BrokNameOfSeller.SelectedId), "CustomerId")
                        If Val(dt.Rows(0).Item("CustSGLId") & "") <> 0 Then
                            cbo_CounterCustSGLWith.SelectedValue = Val(dt.Rows(0).Item("CustSGLId") & "")
                        End If

                        If Val(dt.Rows(0).Item("CounterpartySGLid") & "") <> 0 Then
                            cbo_CustSGL.SelectedValue = Val(dt.Rows(0).Item("CounterpartySGLid") & "")
                        End If


                    End If


                    If Val(dt.Rows(0).Item("bankId") & "") <> 0 Then
                        objCommon.FillControl(cbo_Bank, sqlConn, "ID_FILL_BankMaster1", "BankAccInfo", "BankId", Val(cbo_Company.SelectedValue), "CompId")
                        cbo_Bank.SelectedValue = Val(dt.Rows(0).Item("bankId") & "")
                    End If



                    If Val(dt.Rows(0).Item("CustBankId") & "") <> 0 Then
                        objCommon.FillControl(cbo_CustomerBank, sqlConn, "ID_FILL_CustomerBankDetailS", "BankAcc", "CustBankId", Val(Srh_NameOFClient.SelectedId), "CustomerId")
                        'cbo_CustomerBank.SelectedValue = Val(dt.Rows(0).Item("CustBankId") & "")
                        If cbo_CustomerBank.Items.Count = 2 Then
                            cbo_CustomerBank.SelectedIndex = 1
                        End If

                    End If


                    If Val(dt.Rows(0).Item("cuntcustbank") & "") <> 0 Then
                        objCommon.FillControl(cbo_SellerCustomerBank, sqlConn, "ID_FILL_CustomerBankDetailS", "BankAcc", "CustBankId", Val(srh_BrokNameOfSeller.SelectedId), "CustomerId")
                        'cbo_SellerCustomerBank.SelectedValue = Val(dt.Rows(0).Item("cuntcustbank") & "")
                        If cbo_SellerCustomerBank.Items.Count = 2 Then
                            cbo_SellerCustomerBank.SelectedIndex = 1
                        End If


                    End If
                    FillSecurityDetails()
                    rdo_AccIntDays.SelectedValue = Hid_AccIntDays.Value
                    If txt_ContactPerson.Text <> "" Then
                        FillFieldsNew()
                    End If
                    If txt_CountContactPerson.Text <> "" Then
                        FillFieldsNewCount()
                    End If


                End If
            End If


        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "fillBrokingfields", "Error in fillBrokingfields", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub BrokerCombo()
        Try

            OpenConn()

            'If Val(ViewState("Id")) = 0 Then

            objCommon.FillControl(cbo_BrokeragePaidTo, sqlConn, "ID_FILL_BrokerMasterNew", "BrokerName", "BrokerId", Val(Srh_NameOFClient.SelectedId), "Customerid")
                objCommon.FillControl(cbo_BrokeragereceivedFrom, sqlConn, "ID_FILL_BrokerMasterNew", "BrokerName", "BrokerId", Val(Srh_NameOFClient.SelectedId), "Customerid")
            'Else
            '    objCommon.FillControl(cbo_BrokeragereceivedFrom, sqlConn, "ID_FILL_BrokerMasterNew", "BrokerName", "BrokerId")
            '    objCommon.FillControl(cbo_BrokeragePaidTo, sqlConn, "ID_FILL_BrokerMasterNew", "BrokerName", "BrokerId")
            'End If




        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "BrokerCombo", "Error in BrokerCombo", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Protected Sub cbo_DealTransType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DealTransType.SelectedIndexChanged
        Try

            BrokerCombo()
            If cbo_DealTransType.SelectedValue = "B" Then
                cbo_Demat.SelectedValue = ""
            Else
                Dim lstItem6 As ListItem = cbo_Demat.Items.FindByText(Trim("INDUSIND BANK LTD  -  10289266"))
                If lstItem6 IsNot Nothing Then cbo_Demat.SelectedValue = lstItem6.Value
            End If
            If cbo_DealTransType.SelectedValue = "F" Then
                If rbl_TypeOFTranction.SelectedValue = "P" Then
                    rdo_FinancialDealType.Items(1).Text = "To Sell"
                Else
                    rdo_FinancialDealType.Items(1).Text = "From Purchase"
                End If
            End If

            If cbo_DealTransType.SelectedValue = "B" Then
                If rbl_TypeOFTranction.SelectedValue = "P" Then
                    rbl_TypeOFTranction.SelectedValue = "P"
                    rbl_TypeOFTranction.Enabled = False
                    txt_Roundoff.Enabled = False
                Else
                    rbl_TypeOFTranction.SelectedValue = "P"
                    rbl_TypeOFTranction.Enabled = False
                    txt_Roundoff.Enabled = False
                    Dim strHtml As String
                    Dim msg As String = "You cannot enter broking sell deal"
                    strHtml = "alert('" + msg + "');"
                    'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msges", strHtml, True)
                End If
            Else
                rbl_TypeOFTranction.Enabled = True
                txt_Roundoff.Enabled = True
            End If
            If cbo_DealTransType.SelectedValue = "D" Or cbo_DealTransType.SelectedValue = "T" Then
                rdo_FinancialDealType.SelectedValue = "N"
            End If
            OpenConn()
            objCommon.FillControl(cbo_BrokCustomerType, sqlConn, "ID_FILL_CustomerTypeMaster1", "CustomerTypeName", "CustomerTypeId")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub cbo_SettDay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SettDay.SelectedIndexChanged
        Try

            'FillSettDate()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try

    End Sub
    Private Sub FillSettDate()
        Try
            Dim intLoop As Int16 = 0
            Dim count As Integer = 0
            Dim incDate As Date

            incDate = objCommon.DateFormat(txt_DealDate.Text)
            While count < cbo_SettDay.SelectedValue
                incDate = DateAdd(DateInterval.Day, 1, incDate)
                If checkdate(incDate) = True Then
                    count = count - 1
                End If
                count = count + 1
            End While
            txt_SettmentDate.Text = Format(incDate, "dd/MM/yyyy")
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillSettDate", "Error in FillSettDate", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Function checkdate(ByVal incDate As Date) As Boolean
        Try

            Dim Sqlcomm As New SqlCommand
            OpenConn()
            With Sqlcomm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_FILL_HolidaysNew"
                .Parameters.Clear()
                objCommon.SetCommandParameters(Sqlcomm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                objCommon.SetCommandParameters(Sqlcomm, "@Month", SqlDbType.Int, 4, "I", , , Val(incDate.Month))
                objCommon.SetCommandParameters(Sqlcomm, "@HolidayDate", SqlDbType.DateTime, 4, "I", , , incDate)
                objCommon.SetCommandParameters(Sqlcomm, "@RET_CODE", SqlDbType.Int, 4, "O")
            End With
            If Sqlcomm.ExecuteScalar > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "checkdate", "Error in checkdate", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Function

    Private Function DeleteProfit(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_Profit"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@SellDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "DeleteProfit", "Error in DeleteProfit", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Function DeleteProfitFinancial(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_ProfitFinancial"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@SellDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "DeleteProfitFinancial", "Error in DeleteProfitFinancial", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function


    Private Function DeletePurchaseProfit(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_PurchaseProfit"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@PurcDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "DeletePurchaseProfit", "Error in DeletePurchaseProfit", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Private Function DeletePurchaseProfitFinancial(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_PurchaseProfitUpdate"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@PurcDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "DeletePurchaseProfitFinancial", "Error in DeletePurchaseProfitFinancial", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Private Function CheckSellDeal(ByVal strProc As String) As Boolean
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            Dim intCnt As Integer
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@PurDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            intCnt = Val(sqlComm.ExecuteScalar())
            If intCnt = 0 Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CheckSellDeal", "Error in CheckSellDeal", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Function

    Protected Sub btn_dealconf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_dealconf.Click

        Try
            Costdealconf = "CostMemoDealConf"

            FillSecurityDetailsDeal()
            If Val(Hid_Frequency.Value) <> 0 Then
                'FillAccuredInterestOptions()
                FillAccruedDetails()
            End If
            If rdo_SelectOpt.SelectedValue = "B" Then
                If Val(Hid_Frequency.Value) = 0 Then
                    Hid_Amt.Value = Val(txt_NoOfBonds.Text) * Val(txt_Rate.Text)
                    Hid_AddInterest.Value = 0
                Else
                    Hid_Amt.Value = (Val(txt_Rate.Text) * Val(Hid_IssuePrice.Value) * Val(txt_NoOfBonds.Text)) / 100
                End If
                Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
            ElseIf rdo_SelectOpt.SelectedValue = "F" Then
                Hid_Amt.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)) * Val(txt_Rate.Text) / 100)
                Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
            End If
            SetSaveUpdate("ID_UPDATE_DealSlipEntry", False, False)

            Hid_DealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_DealSlipId.Value))
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "open", "DealReportView()", True)

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_dealconf_Click", "Error in btn_dealconf_Click", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)

        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Page_Unload", "Error in Page_Unload", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub


    Private Sub FillFieldsNew()
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_ContactPerson", Val(Srh_NameOFClient.SelectedId), "Customerid")
            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    If txt_ContactPerson.Text = "" Then
                        txt_ContactPerson.Text = Trim(dt.Rows(0).Item("ContactPerson") & "")
                    ElseIf txt_ContactPerson.Text <> "" And txt_ContactPerson.Text = Trim(dt.Rows(0).Item("ContactPerson") & "") Then
                        txt_ContactPerson.Text = Trim(dt.Rows(0).Item("ContactPerson") & "")
                    End If
                End If
            End If

            Hid_CustomerName.Value = txt_ContactPerson.Text
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillFieldsNew", "Error in FillFieldsNew", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Private Sub FillFieldsNewCount()
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_ContactPerson", Val(srh_BrokNameOfSeller.SelectedId), "Customerid")
            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    If txt_CountContactPerson.Text = "" Then
                        txt_CountContactPerson.Text = Trim(dt.Rows(0).Item("ContactPerson") & "")
                    ElseIf ((txt_CountContactPerson.Text <> "") And (txt_CountContactPerson.Text = Trim(dt.Rows(0).Item("ContactPerson") & ""))) Then
                        txt_CountContactPerson.Text = Trim(dt.Rows(0).Item("ContactPerson") & "")
                    End If
                End If
            End If

            Hid_CountCustomerName.Value = txt_CountContactPerson.Text
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillFieldsNewCount", "Error in FillFieldsNewCount", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Sub FillCountMAddrContact()
        Try
            OpenConn()
            Dim dt As DataTable

            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_MultiAddrContactPerson", Hid_MultiAddrId.Value, "ClientCustAddressId")
            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    If Hid_CountCustomerName.Value <> "" Then
                        txt_CountContactPerson.Text = Hid_CountCustomerName.Value
                    Else
                        txt_CountContactPerson.Text = ""
                    End If
                    If txt_CountContactPerson.Text = "" Then
                        txt_CountContactPerson.Text = Trim(dt.Rows(0).Item("ContactPerson") & "")
                    ElseIf txt_CountContactPerson.Text <> "" And Trim(dt.Rows(0).Item("ContactPerson") & "") <> "" Then
                        txt_CountContactPerson.Text = txt_CountContactPerson.Text & " , " & Trim(dt.Rows(0).Item("ContactPerson") & "")
                    End If
                End If
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillCountMAddrContact", "Error in FillCountMAddrContact", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Sub FillMultiAddrContact()
        Try
            OpenConn()
            Dim dt As DataTable

            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_MultiAddrContactPerson", Hid_MultiAddrId.Value, "ClientCustAddressId")
            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    If Hid_CustomerName.Value <> "" Then
                        txt_ContactPerson.Text = Hid_CustomerName.Value
                    Else
                        txt_ContactPerson.Text = ""
                    End If
                    If txt_ContactPerson.Text = "" Then
                        txt_ContactPerson.Text = Trim(dt.Rows(0).Item("ContactPerson") & "")
                    ElseIf txt_ContactPerson.Text <> "" And Trim(dt.Rows(0).Item("ContactPerson") & "") <> "" Then
                        txt_ContactPerson.Text = txt_ContactPerson.Text & " , " & Trim(dt.Rows(0).Item("ContactPerson") & "")
                    End If
                End If
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillMultiAddrContact", "Error in FillMultiAddrContact", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Sub DeleteBrokingSellDeal()
        Try

            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable(sqlConn, "ID_Delete_brokingsellDealslipid", Val(ViewState("Id")), "Dealslipid")
            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    ViewState("Id") = Val(dt.Rows(0).Item("dealslipid") & "")
                End If
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "DeleteBrokingSellDeal", "Error in DeleteBrokingSellDeal", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub srh_CountContactPerson_linkbutton() Handles srh_CountContactPerson.LinkClick
        Try
            txt_CountContactPerson.Text = ""
            Dim itm As ListItem = srh_CountContactPerson.SelectListBox.Items.FindByText("")
            If itm IsNot Nothing Then srh_CountContactPerson.SelectListBox.Items.Remove(itm)
            For Each lstItem As ListItem In srh_CountContactPerson.SelectListBox.Items
                txt_CountContactPerson.Text = txt_CountContactPerson.Text & lstItem.Text & " / "
            Next
            If txt_CountContactPerson.Text.Length > 0 Then
                txt_CountContactPerson.Text = Left(txt_CountContactPerson.Text, txt_CountContactPerson.Text.Length - 3)
            End If
            Hid_CountCustomerName.Value = txt_CountContactPerson.Text
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "srh_CountContactPerson_linkbutton", "Error in srh_CountContactPerson_linkbutton", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub srh_CountSelectAddress_ButtonClick() Handles srh_CountSelectAddress.ButtonClick
        Try
            Dim strValues() As String
            strValues = srh_CountSelectAddress.SelectedValues
            If strValues(0) = "&nbsp;" Then
                strValues(0) = ""
            End If
            If strValues(1) = "&nbsp;" Then
                strValues(1) = ""
            End If
            If strValues(2) = "&nbsp;" Then
                strValues(2) = ""
            End If
            If strValues(3) = "&nbsp;" Then
                strValues(3) = ""
            End If
            If strValues(6) = "&nbsp;" Then
                strValues(6) = ""
            End If
            If strValues(7) = "&nbsp;" Then
                strValues(7) = ""
            End If
            Hid_MultiAddrId.Value = strValues(7)
            If Hid_MultiAddrId.Value <> "" Then
                FillCountMAddrContact()
            Else
                FillFieldsNewCount()
            End If

            srh_CountSelectAddress.SearchTextBox.Text = strValues(0) & " " & strValues(1) & " " & strValues(2) & "-" & strValues(3)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "srh_CountSelectAddress_ButtonClick", "Error in srh_CountSelectAddress_ButtonClick", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub rdo_calcXInt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdo_calcXInt.SelectedIndexChanged
        Try


            FillSecurityDetailsDeal()
            If Val(Hid_Frequency.Value) <> 0 Then
                'FillAccuredInterestOptions()
                FillAccruedDetails()
            End If
            If rdo_SelectOpt.SelectedValue = "B" Then
                If Val(Hid_Frequency.Value) = 0 Then
                    Hid_Amt.Value = Val(txt_NoOfBonds.Text) * Val(txt_Rate.Text)
                    Hid_AddInterest.Value = 0
                Else
                    Hid_Amt.Value = (Val(txt_Rate.Text) * Val(Hid_IssuePrice.Value) * Val(txt_NoOfBonds.Text)) / 100
                End If
                Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
            ElseIf rdo_SelectOpt.SelectedValue = "F" Then
                Hid_Amt.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)) * Val(txt_Rate.Text) / 100)
                Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
            End If
            lbl_Amount.Text = Format(Trim(Hid_Amt.Value & ""), "Standard")
            txt_InterestAmt.Text = Format(Trim(Hid_AddInterest.Value & ""), "Standard")
            lbl_InterestDays.Text = Hid_IntDays.Value
            lbl_InterestFromToDates.Text = Trim(Hid_InterestFromTo.Value & "")

            lbl_SettlementAmt.Text = Format(Trim(Hid_SettlementAmt.Value & ""), "Standard")
            'lbl_SettlementAmt.Text = Format(Trim(Hid_SettlementAmt.Value & ""), "Standard")
            'lbl_SettlementAmt.Text = Format(Trim(Hid_SettlementAmt.Value & ""), "Standard")
            Hid_TotalSettlementAmt.Value = Hid_SettlementAmt.Value + txt_Roundoff.Text
            'lbl_TotalSettlementAmt.Text = Hid_TotalSettlementAmt.Value
            'lbl_TotalSettlementAmt.Text = Format(Trim(Hid_SettlementAmt.Value & ""), "Standard")
            txt_Roundoff.Attributes.Add("onchange", "CalcRoundofsettAMT();")

            txt_SettOtherChrgs.Attributes.Add("onchange", "CalcRoundofsettAMT();")
            'lbl_TotalSettlementAmt.Text = Hid_TotalSettlementAmt.Value
            If (Val(Hid_IntDays.Value) < 0) Then
                If Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Then
                    rdo_PreviousdealType.SelectedValue = "Y"
                    rdo_PreviousdealType.Enabled = False
                ElseIf Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                    rdo_PreviousdealType.SelectedValue = "Y"
                    rdo_PreviousdealType.Enabled = False
                End If
                tr_calcXInt.Visible = True
                tr_PreviousdealType.Visible = True
                'lbl_Amount.Attributes.Add("style", "color:red")
                lbl_InterestDays.Attributes.Add("style", "color:red")
                txt_InterestAmt.Attributes.Add("style", "color:red")
                lbl_InterestFromToDates.Attributes.Add("style", "color:red")
                'lbl_SettlementAmt.Attributes.Add("style", "color:red")
            Else

                lbl_InterestDays.Attributes.Add("style", "color:black")
                txt_InterestAmt.Attributes.Add("style", "color:black")
                lbl_InterestFromToDates.Attributes.Add("style", "color:black")

                If Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Then
                    rdo_PreviousdealType.Enabled = False
                ElseIf Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                    rdo_PreviousdealType.Enabled = False
                End If
                If rdo_PreviousdealType.SelectedValue = "Y" And rdo_calcXInt.SelectedValue = "N" Then
                    tr_PreviousdealType.Visible = True
                    tr_calcXInt.Visible = True
                Else
                    tr_PreviousdealType.Visible = False
                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Function Delete()
        Dim sqlComm As New SqlCommand
        Try
            OpenConn()
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.Text
            sqlComm.CommandText = "delete from TempPurdeal"
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Delete", "Error in Delete", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Function

    Protected Sub cbo_Exchange_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Exchange.SelectedIndexChanged
        Try
            If (cbo_Exchange.SelectedItem.Text = "NSE" Or cbo_Exchange.SelectedItem.Text = "BSE") And (cbo_ModeOfPayment.SelectedValue = "N" _
            Or cbo_ModeOfPayment.SelectedValue = "B" Or cbo_ModeOfPayment.SelectedValue = "L") Or cbo_DealTransType.SelectedValue = "B" Then
                cbo_Demat.SelectedIndex = 0
            Else
                Dim Lstitem As ListItem = cbo_Demat.Items.FindByText("INDUSIND BANK LTD")
                If Not Lstitem Is Nothing Then cbo_Demat.SelectedValue = Lstitem.Value

                cbo_Demat.Items.Insert(0, "")
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType, "testaa", "showAddLinkRemove();", True)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "cbo_Exchange_SelectedIndexChanged", "Error in cbo_Exchange_SelectedIndexChanged", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub




    Private Function SaveIntReceivable(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim I As Int16
            Dim strRetValues() As String
            Dim arrDealSlipNos As Array
            Dim arrDealSlipIDs As Array
            Dim arrCoupon As Array
            Dim dt As New DataTable
            Dim dr As DataRow
            Dim j As Integer
            sqlComm.Connection = sqlConn
            sqlComm.CommandText = "ID_UPDATE_IntReceivable"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Transaction = sqlTrans
            If Hid_RetValues.Value <> "" Then
                strRetValues = Split(Hid_RetValues.Value, "!")
                dt.Columns.Add("DealSlipNo")
                dt.Columns.Add("DealSlipID")
                dt.Columns.Add("RemainingFaceValue")
                dt.Columns.Add("Coupon")
                'dt.Columns.Add("CouponPaid")
                dr = dt.NewRow
                dr.Item("DealSlipID") = Trim(strRetValues(0))
                Hid_PurchaseDealSlipId.Value = Trim(strRetValues(0))
                dr.Item("DealSlipNo") = Trim(strRetValues(1))
                Hid_DealSlipNo.Value = Trim(strRetValues(1))
                dr.Item("RemainingFaceValue") = Trim(strRetValues(2))
                Hid_RemainingFaceValue.Value = Trim(strRetValues(2))
                dr.Item("Coupon") = Trim(strRetValues(3))
                'Hid_Coupon.Value = Trim(strRetValues(3))
                dt.Rows.Add(dr)
                Session("DealPurchaseDetailsNew") = dt
                arrDealSlipIDs = Split(Hid_PurchaseDealSlipId.Value, ",")
                arrDealSlipNos = Split(Hid_DealSlipNo.Value, ",")


                lst_addmultiple.Items.Clear()
                'For j = 0 To UBound(arrDealSlipIDs)
                '    If Trim(arrDealSlipIDs(j)) <> "" Then
                '        lst_addmultiple.Items.Add(New ListItem(Trim(arrDealSlipNos(j)), Val(arrDealSlipIDs(j))))
                '        'IntReceivable.Add(New String() {lst_addmultiple.Items(I).Value, Val(arrCoupon(j))})

                '    End If
                'Next
                For j = 0 To (arrDealSlipIDs).Length - 1
                    If Trim(arrDealSlipIDs(j)) <> "" Then
                        sqlComm.Parameters.Clear()
                        objCommon.SetCommandParameters(sqlComm, "@SellDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
                        objCommon.SetCommandParameters(sqlComm, "@PurchaseDealSlipId", SqlDbType.Int, 4, "I", , , Val(arrDealSlipIDs(j)))
                        objCommon.SetCommandParameters(sqlComm, "@TaxFree", SqlDbType.Char, 1, "I", , , Trim(rdo_TaxFree.SelectedValue))
                        sqlComm.ExecuteNonQuery()
                        'objCommon.SetCommandParameters(sqlComm, "@InterestReceivable", SqlDbType.Decimal, 9, "I", , , Val(arrCoupon(j)))
                        'objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 8, "I", , , Val(Srh_NameofSecurity.SelectedId))
                        'sqlComm.ExecuteNonQuery()
                    End If
                    'intrec = intrec + Val(arrCoupon(j))

                Next

                txt_PreviousInterest.Text = Val(intrec)

            Else
                If fillIntRec(sqlTrans) = False Then Exit Function
                Dim dt1 As DataTable = Session("PurIntReceivable")
                For k As Integer = 0 To dt1.Rows.Count - 1
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@SellDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
                    objCommon.SetCommandParameters(sqlComm, "@PurchaseDealSlipId", SqlDbType.Int, 4, "I", , , Val(dt1.Rows(k).Item("PurchaseDealSlipId")))
                    objCommon.SetCommandParameters(sqlComm, "@TaxFree", SqlDbType.Char, 1, "I", , , Trim(rdo_TaxFree.SelectedValue))
                    sqlComm.ExecuteNonQuery()
                    'objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 8, "I", , , Val(Srh_NameofSecurity.SelectedId))
                    'objCommon.SetCommandParameters(sqlComm, "@InterestReceivable", SqlDbType.Decimal, 9, "I", , , Val(arrCoupon(k)))

                    'sqlComm.ExecuteNonQuery()
                    'intrec = intrec + Val(arrCoupon(k))

                Next
                txt_PreviousInterest.Text = Val(intrec)

            End If
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveIntReceivable", "Error in SaveIntReceivable", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Function SaveIntReceivableNew(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As New DataTable

            sqlComm.Connection = sqlConn
            sqlComm.CommandText = "ID_UPDATE_IntReceivable"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Transaction = sqlTrans

            If Hid_Marked.Value <> "" Then
                Dim dtMarked As DataTable = CType(JsonConvert.DeserializeObject(Hid_Marked.Value, (GetType(DataTable))), DataTable)
                If dtMarked.Rows.Count > 0 Then
                    For Each row As DataRow In dtMarked.Rows
                        sqlComm.Parameters.Clear()
                        objCommon.SetCommandParameters(sqlComm, "@SellDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
                        objCommon.SetCommandParameters(sqlComm, "@PurchaseDealSlipId", SqlDbType.Int, 4, "I", , , Val(row("DealSlipId") & ""))
                        objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 8, "I", , , Val(Srh_NameofSecurity.SelectedId))
                        objCommon.SetCommandParameters(sqlComm, "@TaxFree", SqlDbType.Char, 1, "I", , , Trim(rdo_TaxFree.SelectedValue))
                        sqlComm.ExecuteNonQuery()
                    Next
                End If
            End If

            txt_PreviousInterest.Text = Val(intrec)

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveIntReceivable", "Error in SaveIntReceivable", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Function fillIntRec(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlda As New SqlDataAdapter
            Dim sqldt As New DataTable
            Dim sqldv As New DataView
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Transaction = sqlTrans

            sqlComm.CommandText = "ID_FILL_DealPurchaseDetails"

            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@SellDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@ret_code", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(sqldt)
            Session("PurIntReceivable") = sqldt
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveIntReceivable", "Error in SaveIntReceivable", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Function SaveIntRecSingle(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim I As Int16
            sqlComm.Connection = sqlConn
            sqlComm.CommandText = "ID_UPDATE_IntReceivable"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Transaction = sqlTrans

            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@SellDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@PurchaseDealSlipId", SqlDbType.Int, 4, "I", , , Val(srh_BTBDealSlipNo.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@InterestReceivable", SqlDbType.Decimal, 9, "I", , , Val(tmp_IntReceivable))
            objCommon.SetCommandParameters(sqlComm, "@TaxFree", SqlDbType.Char, 1, "I", , , Trim(rdo_TaxFree.SelectedValue))
            sqlComm.ExecuteNonQuery()

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveIntRecSingle", "Error in SaveIntRecSingle", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function


    Protected Sub Btn_Remove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_Remove.Click
        Try
            OpenConn()
            Dim sqlTrans As SqlTransaction
            sqlTrans = sqlConn.BeginTransaction
            If DeleteMarking(sqlTrans) = False Then Exit Sub
            If cbo_DealTransType.SelectedValue = "F" And rbl_TypeOFTranction.SelectedValue = "P" And rdo_FinancialDealType.SelectedValue = "N" Then
                'srh_FinancialDealSlipNo.SearchTextBox.Text = ""
            Else
                srh_BTBDealSlipNo.SearchTextBox.Text = ""
            End If

            sqlTrans.Commit()
            lbl_Msg.Text = "Marking removed successfully"
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Btn_Remove_Click", "Error in Btn_Remove_Click", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function DeleteMarking(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandText = "ID_Remove_FIFOMarking"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Transaction = sqlTrans
            sqlComm.Parameters.Clear()

            objCommon.SetCommandParameters(sqlComm, "@SellDealSlipId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            sqlComm.ExecuteNonQuery()
            lbl_Msg.Text = "Marking removed successfully"
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "DeleteMarking", "Error in DeleteMarking", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Protected Sub cbo_CustomerType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CustomerType.SelectedIndexChanged
        Try
            Srh_NameOFClient.SearchTextBox.Text = ""
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "cbo_CustomerType_SelectedIndexChanged", "Error in cbo_CustomerType_SelectedIndexChanged", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub cbo_BrokCustomerType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BrokCustomerType.SelectedIndexChanged
        Try
            srh_BrokNameOfSeller.SearchTextBox.Text = ""
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "cbo_BrokCustomerType_SelectedIndexChanged", "Error in cbo_BrokCustomerType_SelectedIndexChanged", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub Srh_ReferenceBy_ButtonClick() Handles Srh_ReferenceBy.ButtonClick
        Try
            FillCustomerDetails()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Srh_ReferenceBy_ButtonClick", "Error in Srh_ReferenceBy_ButtonClick", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub



    Private Sub btnRemoveMarking_Click(sender As Object, e As EventArgs) Handles btnRemoveMarking.Click
        Dim lstParam As New List(Of SqlParameter)()
        If rbl_DealType.SelectedValue = "M" Then
            If lst_addmultiple.Items.Count > 0 Then
                Dim dtMarked As DataTable = CType(JsonConvert.DeserializeObject(Hid_Marked.Value, (GetType(DataTable))), DataTable)

                lstParam.Add(New SqlParameter("@SellDealSlipId", Val(ViewState("Id"))))
                lstParam.Add(New SqlParameter("@CompId", Val(Session("CompId"))))
                lstParam.Add(New SqlParameter("@Marking", dtMarked))
                lbl_Msg.Text = objComm.InsertUpdateDeleteDetailsMsg(lstParam, "ID_Remove_MultipleMarking")
                If lbl_Msg.Text = "Marking removed successfully." Then
                    Hid_Marked.Value = ""
                    lst_addmultiple.Items.Clear()
                    btnRemoveMarking.Visible = False
                End If
            End If
        Else
            OpenConn()
            Dim sqlTrans As SqlTransaction
            sqlTrans = sqlConn.BeginTransaction
            If DeleteMarking(sqlTrans) = False Then Exit Sub


            sqlTrans.Commit()
            lbl_Msg.Text = "Marking removed successfully"
        End If

    End Sub

    Private Sub txt_DealDate_TextChanged(sender As Object, e As EventArgs) Handles txt_DealDate.TextChanged
        Try
            OpenConn()
            FillSecurityDetails()
        Catch ex As Exception
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub rdo_TCSApplicable_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdo_TCSApplicable.SelectedIndexChanged
        Try
            If rdo_TCSApplicable.SelectedValue = "Y" Then
                rdo_TDSApplicable.SelectedValue = "N"
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub rdo_TDSApplicable_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdo_TDSApplicable.SelectedIndexChanged
        Try
            If rdo_TDSApplicable.SelectedValue = "Y" Then
                rdo_TCSApplicable.SelectedValue = "N"
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Function Doc2SQLServer(ByVal sqlTrans As SqlTransaction)
        Try
            'OpenConn()

            With sqlComm
                .Connection = sqlConn
                sqlComm.Transaction = sqlTrans
                .Parameters.Clear()
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_INSERT_UPLOADDealAck"
                param = New Data.SqlClient.SqlParameter("@Dealslipid", Data.SqlDbType.Int)
                param.Value = ViewState("Id")
                .Parameters.Add(param)
                .ExecuteNonQuery()
                LabelError.Visible = True
                LabelError.Text = "File uploaded successfully."
            End With
            Return True
        Catch ex As Exception
            Response.Write(ex.Message)
            Return False
        Finally
            'CloseConn()
        End Try
    End Function

    Private Sub btn_view_Click(sender As Object, e As EventArgs) Handles btn_view.Click
        Try
            ShowImage("")
        Catch ex As Exception

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            'CloseConn()
        End Try
    End Sub
    Private Sub ShowImage(ByVal strType As String)
        Try
            ' OpenConn()
            Dim dt As DataTable
            Dim arrContent() As Byte
            dt = GetDetails()

            'OpenConn()
            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                arrContent = CType(dr.Item(strType & "imgData"), Byte())
                Dim conType As String = dr.Item(strType & "imgType").ToString()
                Response.Clear()
                Response.Charset = ""
                Response.ClearHeaders()
                Response.ContentType = conType
                Response.AddHeader("content-disposition", "attachment;filename=" & dr.Item(strType & "FileName") & ".PDF")
                Response.BinaryWrite(arrContent)
                Response.Flush()

                'Dim dr As DataRow = dt.Rows(0)
                'arrContent = CType(dr.Item(strType & "imgData"), Byte())
                'Dim conType As String = dr.Item(strType & "imgType").ToString()
                'Response.Clear()
                'Response.Charset = ""
                'Response.ClearHeaders()
                'Response.ContentType = conType
                'Response.AddHeader("content-disposition", "attachment;filename=img.pdf")
                'Response.BinaryWrite(arrContent)

                If Response.IsClientConnected Then
                    Response.Flush()
                    Response.[End]()
                End If

                'Response.Flush()
                'Response.End()
            End If
        Catch ex As Exception

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            'CloseConn()
        End Try
    End Sub

    Private Function GetDetails() As DataTable
        Try
            OpenConn()
            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand
            'Dim sqlconn As New SqlConnection
            dtfill = objCommon.FillDataTable(sqlConn, "ID_FILL_UPLOADDealDoc", ViewState("Id"), "DealSlipId")
            Return dtfill
        Catch ex As Exception

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Function
    Private Function UploadTempImage()
        Dim intLength As Integer
        Dim arrContent As Byte()
        If FilePicker.PostedFile Is Nothing Then
            LabelError.Text = "No file specified."
            LabelError.Visible = False
            Exit Function
        Else
            Dim fileName As String = FilePicker.PostedFile.FileName
            Hid_uploadImagePath.Value = fileName
            If fileName Is Nothing Or fileName = "" Then Exit Function
            Dim strFile As String = Right(fileName, fileName.Length - fileName.LastIndexOf("\") - 1)
            strFile = Left(strFile, strFile.LastIndexOf("."))
            Dim ext As String = fileName.Substring(fileName.LastIndexOf("."))
            ext = ext.ToLower
            Dim imgType = FilePicker.PostedFile.ContentType

            Hid_ImageContentType.Value = FilePicker.PostedFile.ContentType

            If ext = ".jpg" Then
            ElseIf ext = ".tif" Then
            ElseIf ext = ".bmp" Then
            ElseIf ext = ".gif" Then
            ElseIf ext = "jpg" Then
            ElseIf ext = "bmp" Then
            ElseIf ext = "gif" Then
            ElseIf ext = "tif" Then
            ElseIf ext = ".pdf" Then

            Else
                LabelError.Text = "Only gif, bmp, or jpg format files supported."
                Exit Function
            End If
            intLength = Convert.ToInt32(FilePicker.PostedFile.InputStream.Length)
            ReDim arrContent(intLength)
            FilePicker.PostedFile.InputStream.Read(arrContent, 0, intLength)
            If TempDoc2SQLServer(txt_DealSlipNo.Text, arrContent, intLength, imgType) = True Then
                LabelError.Visible = True
                LabelError.Text = "Document uploaded successfully."
            Else
                LabelError.Text = "An error occured while uploading Image... Please try again."

            End If
        End If
    End Function
    Protected Function TempDoc2SQLServer(ByVal title As String, ByVal Content As Byte(), ByVal Length As Integer, ByVal strType As String) As Boolean
        Try
            OpenConn()
            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_INSERT_TEMPUPLOADIMAGE"
                param = New Data.SqlClient.SqlParameter("@content", Data.SqlDbType.Image)
                param.Value = Content
                .Parameters.Add(param)
                param = New Data.SqlClient.SqlParameter("@length", Data.SqlDbType.BigInt)
                param.Value = Length
                .Parameters.Add(param)
                param = New Data.SqlClient.SqlParameter("@title", Data.SqlDbType.VarChar)
                param.Value = title
                .Parameters.Add(param)
                param = New Data.SqlClient.SqlParameter("@type", Data.SqlDbType.VarChar)
                param.Value = strType
                .Parameters.Add(param)
                param = New Data.SqlClient.SqlParameter("@tempimgdate", Data.SqlDbType.SmallDateTime)
                param.Value = Today.Date
                .Parameters.Add(param)
                param = New Data.SqlClient.SqlParameter("@DealSlipId", Data.SqlDbType.Int)
                param.Value = ViewState("Id")
                .Parameters.Add(param)
                .ExecuteNonQuery()
            End With
            Return True
        Catch ex As Exception
            Response.Write(ex.Message)

            Return False
        Finally
            CloseConn()
        End Try
    End Function

    Private Sub txt_BrokerageRate_TextChanged(sender As Object, e As EventArgs) Handles txt_BrokerageRate.TextChanged
        Try
            If Val(txt_BrokerageRate.Text) > 0 And rdo_BrokerageRateAmt.SelectedValue = "R" Then
                txt_BrokeragePaid.Text = Val(txt_BrokerageRate.Text) * (Val(txt_Amount.Text) * cbo_Amount.SelectedValue) / 100
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub rdo_BrokerageRateAmt_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdo_BrokerageRateAmt.SelectedIndexChanged
        Try
            'txt_Conchargespaid.Text = 0

            If rdo_BrokerageRateAmt.SelectedValue = "R" Then
                txt_BrokeragePaid.Text = 0
                txt_BrokerageRate.Text = 0
                txt_BrokeragePaid.Enabled = False
                txt_BrokerageRate.Enabled = True
            Else
                txt_BrokerageRate.Text = 0
                txt_BrokeragePaid.Text = 0
                txt_BrokerageRate.Enabled = False
                txt_BrokeragePaid.Enabled = True
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class


