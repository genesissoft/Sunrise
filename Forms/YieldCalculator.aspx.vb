Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports YieldCalc
Imports System.Reflection
Imports System.IO
Imports RKLib.ExportData
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html
Imports iTextSharp.text.html.simpleparser
Imports ClosedXML.Excel
Imports System.Web.Services
Partial Class Forms_YieldCalculator
    Inherits System.Web.UI.Page
    Dim arrIssueDetailIds() As String
    Dim dblTotalFaceValue As Decimal
    Dim intUserId As Integer

    Dim MatDate() As Date
    Dim MatAmt() As Double
    Dim CoupDate() As Date
    Dim CoupRate() As Double
    Dim CallDate() As Date
    Dim CallAmt() As Double
    Dim PutDate() As Date
    Dim PutAmt() As Double
    Dim strCoupFlag As String
    Dim datYTM As Date
    Dim decFaceValue As Decimal
    Dim datIssue As Date
    Dim datInterest As Date
    Dim datBookClosure As Date
    Dim decRate As Decimal
    Dim blnNonGovernment As Boolean
    Dim blnRateActual As Boolean
    Dim blnDMAT As Boolean
    Dim intBKDiff As Integer
    Dim dblPepCoupRate As Double
    Dim blnCompRate As Boolean
    Dim blnCloseButton As Boolean
    Dim sqlConn As SqlConnection
    Dim objCommon As New clsCommonFuns
    Dim BrokenInt As Boolean
    Dim InterestOnHoliday As Boolean
    Dim InterestOnSat As Boolean
    Dim MaturityOnHoliday As Boolean
    Dim MaturityOnSat As Boolean
    Dim Rbl_StepUp_down As String = ""
    Dim SecurityId As Integer

    Dim dblCashFlowAmt As Decimal
    Dim dblCashFlowAccInterest As Double
    Dim dblCashFlowTotalConsideration As Double
    Dim dblCashFlowSettAmt As Double
    Dim dblStampDutyAmt As Decimal
    Dim dblNoOfBond As Decimal
    Dim Quantity As Decimal
    Dim strShutperiod As String
    Dim strAmountInwords As String
    Dim objMIS As MISReports
    Private Sub Forms_YieldCalculatorNew_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If

            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            SetAttributes()
            If IsPostBack = False Then
                txt_Dealdate.Attributes.Add("onkeypress", "OnlyDate();")
                txt_Dealdate.Text = Format(Now, "dd/MM/yyyy")
                txt_valuedate.Attributes.Add("onkeypress", "OnlyDate();")
                txt_valuedate.Text = Format(Now, "dd/MM/yyyy")
                SetControls()
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CheckCashFlow", "CheckCashFlow();", True)
        Catch ex As Exception
            Response.Write(ex.Message)
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
        If sqlConn.State = ConnectionState.Open Then
            sqlConn.Close()
            sqlConn = Nothing
        End If
    End Sub

    Private Sub btn_Reset_Click(sender As Object, e As EventArgs) Handles btn_Reset.Click
        Try
            ClearFields_LeftPane()
            ClearFields_RightPane()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub


    Private Sub SetAttributes()
        'in user control
        'rdo_YXM.Items(0).Attributes.Add("onclick", "CheckCashFlow()")
        'rdo_YXM.Items(1).Attributes.Add("onclick", "CheckCashFlow()")
        'rdo_YXM.Items(2).Attributes.Add("onclick", "CheckCashFlow()")
        rdo_SemiAnn.Items(0).Attributes.Add("onclick", "ChangeSemiAnn(true);")
        rdo_SemiAnn.Items(1).Attributes.Add("onclick", "ChangeSemiAnn(true);")
        rdo_Yield.Attributes.Add("onclick", "MakeDisable();")
        rdo_MatToRate.Attributes.Add("onclick", "MakeEnable('M',true);")
        rdo_CallToRate.Attributes.Add("onclick", "MakeEnable('C',true);")
        rdo_PutToRate.Attributes.Add("onclick", "MakeEnable('P',true);")
        btn_CalYield.Attributes.Add("onclick", "return ValidateCalculation();")
        txt_YTMSemi.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_YTMAnn.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_YTCSemi.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_YTCAnn.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_YTPSemi.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_YTPAnn.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_Rate.Attributes.Add("onkeypress", "OnlyDecimal();")
        btn_CashFlowExcel.Attributes.Add("onclick", "return ValidateCashFlow();")
        EnableDisable()

    End Sub

    Private Sub EnableDisable()
        txt_YTPSemi.Enabled = False
        txt_YTPAnn.Enabled = False
        txt_YTCAnn.Enabled = False
        txt_YTCSemi.Enabled = False
        txt_YTMSemi.Enabled = False
        txt_YTMAnn.Enabled = False
    End Sub

    Private Sub SetControls()
        Try
            OpenConn()
            objCommon.FillControl(cbo_securitytype, sqlConn, "ID_FILL_SecurityTypeMaster_YieldCal", "SecurityTypeName", "SecurityTypeId")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub cbo_SecurityType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_securitytype.SelectedIndexChanged
        srh_NameofSecurity.SearchTextBox.Text = ""
        srh_NameofSecurity.SelectedId = ""
    End Sub

    Protected Sub Srh_NameofSecurity_ButtonClick() Handles srh_NameofSecurity.ButtonClick
        Try
            FillSecurityDetails()
            FillMaturityPutCallCouponDetails()
            FillOptions()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillSecurityDetails()
        Try
            Dim dt As DataTable
            OpenConn()

            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityMaster_YieldCal", srh_NameofSecurity.SelectedId, "SecurityId",, txt_valuedate.Text, "AsOn")
            If dt.Rows.Count > 0 Then
                cbo_securitytype.SelectedValue = Trim(dt.Rows(0).Item("SecurityTypeId").ToString)
                Hid_SecurityId.Value = srh_NameofSecurity.SelectedId

                srh_NameofSecurity.SearchTextBox.Text = dt.Rows(0).Item("SecurityName").ToString
                txt_isin.Text = dt.Rows(0).Item("NSDLAcNumber").ToString
                lit_isin.Text = dt.Rows(0).Item("NSDLAcNumber").ToString
                txt_couponrate.Text = dt.Rows(0).Item("CouponMaturity").ToString
                lit_name.Text = (dt.Rows(0).Item("SecurityIssuer") & "")

                lit_allotmentdate.Text = (dt.Rows(0).Item("IssueDate") & "")
                lit_mastertype.Text = (dt.Rows(0).Item("SecurityTypeName") & "")

                lit_facevalue.Text = objCommon.DecimalFormat2((dt.Rows(0).Item("NSDLFaceValue") & ""))
                lit_redemptionrate.Text = (dt.Rows(0).Item("MaturityDate") & "")
                lit_intpayfrequency.Text = (dt.Rows(0).Item("InterestFrequency") & "")
                lit_issuerating.Text = (dt.Rows(0).Item("Rating") & "")
                lit_shutperiod.Text = (dt.Rows(0).Item("ShutPeriod") & "")
                Hid_CurrSecFaceValue.Value = Val(dt.Rows(0).Item("CurrSecFaceValue") & "")
                Hid_IsStaggered.Value = Trim(dt.Rows(0).Item("IsStaggered") & "")
                rdo_YXM.SelectedValue = Trim(dt.Rows(0).Item("YXM") & "")
                If Hid_IsStaggered.Value = "True" Then
                    txt_quantum.Enabled = False
                    cbo_Amount.SelectedValue = 1
                    cbo_Amount.Enabled = False
                Else
                    txt_quantum.Enabled = True
                    cbo_Amount.Enabled = True
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub



    Private Sub FillMaturityPutCallCouponDetails()
        Try
            Dim dt As DataTable

            OpenConn()
            Dim i As Int16
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_Securitydetails_YIELDCAL", srh_NameofSecurity.SelectedId, "SecurityId")
            If dt.Rows.Count > 0 Then

                lit_intpaydate.Text = (dt.Rows(i).Item("IPDates") & "")

                Hid_NatureOfInstrument.Value = (dt.Rows(i).Item("NatureOfInstrument") & "")
                For i = 0 To dt.Rows.Count - 1
                    Hid_TypeFlag.Value = Trim(dt.Rows(i).Item("TypeFlag") & "")
                    If Hid_TypeFlag.Value = "M" Then

                        lit_maturevalue.Text = lit_maturevalue.Text & dt.Rows(i).Item(0).ToString() & "-" & (Format(Val(dt.Rows(i).Item(1)), "############")) & ", "
                    ElseIf Hid_TypeFlag.Value = "I" Then
                        lit_couponrate.Text = lit_couponrate.Text & IIf(dt.Rows(i).Item(0).ToString() = "30/12/9999", "", dt.Rows(i).Item(0).ToString()) & "-" & dt.Rows(i).Item(1).ToString() & ", "

                    ElseIf Hid_TypeFlag.Value = "C" Then
                        lit_putcalldate.Text = lit_putcalldate.Text & dt.Rows(i).Item(0).ToString() & "-" & (Format(Val(dt.Rows(i).Item(1)), "############")) & ", "
                        lit_putcalltype.Text = "Call"
                    ElseIf Hid_TypeFlag.Value = "P" Then
                        lit_putcalldate.Text = lit_putcalldate.Text & dt.Rows(i).Item(0).ToString() & "-" & (Format(Val(dt.Rows(i).Item(1)), "############")) & ", "
                        lit_putcalltype.Text = "Put"
                    End If
                Next
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub


    Private Sub ClearFields_LeftPane()
        Try
            cbo_securitytype.SelectedIndex = 0
            txt_isin.Text = ""
            srh_NameofSecurity.SearchTextBox.Text = ""
            srh_NameofSecurity.SelectedId = ""
            srh_NameofSecurity.SelectedFieldText = ""
            srh_NameofSecurity.SelectedFieldId = ""

            Srh_NameOFClient.SearchTextBox.Text = ""
            Srh_NameOFClient.SelectedId = ""
            Srh_NameOFClient.SelectedFieldText = ""
            Srh_NameOFClient.SelectedFieldId = ""

            txt_couponrate.Text = ""
            txt_Dealdate.Text = ""
            txt_valuedate.Text = ""
            'cbo_calculationmode.SelectedIndex = 0
            txt_quantum.Text = ""
            cbo_Amount.SelectedValue = "1"
            'txt_priceyield.Text = ""
            txt_lastipdate.Text = ""
            txt_accrued.Text = ""
            txt_nofodays.Text = ""
            txt_SettlementAmt.Text = ""
            txt_StampDuty.Text = ""
            txt_TC.Text = ""
            txt_PrinciAmt.Text = ""
            txt_YTCAnn.Text = ""
            txt_YTCSemi.Text = ""
            txt_YTMSemi.Text = ""
            txt_YTPAnn.Text = ""
            txt_YTPSemi.Text = ""
            txt_YTMAnn.Text = ""
            txt_NoOfBonds.Text = ""
            txt_Dealdate.Text = Format(Now, "dd/MM/yyyy")
            txt_valuedate.Text = Format(Now, "dd/MM/yyyy")
            cbo_Amount.Enabled = True
            txt_quantum.Enabled = True
            txt_Rate.Text = ""
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub ClearFields_RightPane()
        Try
            lit_mastertype.Text = ""
            lit_isin.Text = ""
            lit_couponrate.Text = ""
            lit_name.Text = ""
            lit_redemptionrate.Text = ""
            lit_allotmentdate.Text = ""
            lit_intpayfrequency.Text = ""
            lit_intpaydate.Text = ""
            lit_putcalltype.Text = ""
            lit_putcalldate.Text = ""
            lit_shutperiod.Text = ""
            lit_stepupdown.Text = ""
            lit_facevalue.Text = ""
            lit_maturevalue.Text = ""
            lit_issuerating.Text = ""
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub



    Private Sub btn_CalYield_Click(sender As Object, e As EventArgs) Handles btn_CalYield.Click
        Dim intSecurityId As Integer
        Try
            intSecurityId = Val(Hid_SecurityId.Value)
            FillOptions()
            Dim I As Single
            MatDate = FillDateArrays(Hid_MatDate.Value)
            MatAmt = FillAmtArrays(Hid_MatAmt.Value)
            CallDate = FillDateArrays(Hid_CallDate.Value)
            CallAmt = FillAmtArrays(Hid_CallAmt.Value)
            CoupDate = FillDateArrays(Hid_CoupDate.Value)
            CoupRate = FillAmtArrays(Hid_CoupRate.Value)
            PutDate = FillDateArrays(Hid_PutDate.Value)
            PutAmt = FillAmtArrays(Hid_PutAmt.Value)
            'FillSettDate()
            SetValues()


            If Hid_RateAmtFlag.Value = "A" Then
                For I = 0 To CoupRate.Length - 1
                    CoupRate(I) = (CoupRate(I) / decFaceValue) * 100
                Next
            End If
            Select Case rdo_YXM.SelectedValue
                Case "Y"
                    FillYieldOptions()
                Case "X"
                    FillXIRROptions()
                Case "M"
                    'FillMMYOptions()
                    FillMMYOptionsNew()

            End Select
            FillAccruedDetails_CashFlow()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillYieldOptions()
        Try
            Dim datMaturity As Date
            Dim datCoupon As Date
            Dim datCall As Date
            Dim datPut As Date
            Dim decMaturityAmt As Decimal
            Dim decCouponRate As Decimal
            Dim decCallAmt As Decimal
            Dim decPutAmt As Decimal
            Dim dblResult As Double
            Dim strSemiAnnFlag As String

            With objCommon
                If MatDate.Length <= 0 Then
                    datMaturity = Date.MinValue
                Else
                    datMaturity = MatDate(0)
                End If
                If CallDate.Length <= 0 Then
                    datCall = Date.MinValue
                Else
                    datCall = CallDate(0)
                End If
                If PutDate.Length <= 0 Then
                    datPut = Date.MinValue
                Else
                    datPut = PutDate(0)
                End If
                If CoupDate.Length <= 0 Then
                    datCoupon = Date.MinValue
                Else
                    datCoupon = CoupDate(0)
                End If
                If MatAmt.Length <= 0 Then
                    decMaturityAmt = 0
                Else
                    decMaturityAmt = MatAmt(0)
                End If
                If CallAmt.Length <= 0 Then
                    decCallAmt = 0
                Else
                    decCallAmt = CallAmt(0)
                End If
                If PutAmt.Length <= 0 Then
                    decPutAmt = 0
                Else
                    decPutAmt = PutAmt(0)
                End If
                If CoupRate.Length <= 0 Then
                    decCouponRate = 0
                Else
                    decCouponRate = CoupRate(0)
                End If
            End With

            If rdo_Yield.Checked = True Then
                GlobalFuns.CalculateYield(txt_valuedate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                               datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "Y", 0, "")
                With objCommon
                    txt_YTMAnn.Text = .DecimalFormat4(dblYTMAnn)
                    txt_YTCAnn.Text = .DecimalFormat4(dblYTCAnn)
                    txt_YTPAnn.Text = .DecimalFormat4(dblYTPAnn)
                    If Val(Hid_Frequency.Value & "") > 1 Then
                        txt_YTMSemi.Text = .DecimalFormat4(dblYTMSemi)
                        txt_YTCSemi.Text = .DecimalFormat4(dblYTCSemi)
                        txt_YTPSemi.Text = .DecimalFormat4(dblYTPSemi)
                    End If
                End With
            ElseIf rdo_MatToRate.Checked = True Then
                If rdo_SemiAnn.SelectedValue = "A" Then
                    txt_YTMAnn.Text = objCommon.DecimalFormat4(txt_YTMAnn.Text)
                    strSemiAnnFlag = "A"
                Else
                    txt_YTMSemi.Text = objCommon.DecimalFormat4(txt_YTMSemi.Text)
                    strSemiAnnFlag = "S"
                End If
                dblResult = IIf(rdo_SemiAnn.SelectedValue = "A", Val(txt_YTMAnn.Text), Val(txt_YTMSemi.Text))
                GlobalFuns.CalculateYield(txt_valuedate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                                datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "M", dblResult, strSemiAnnFlag)
                txt_Rate.Text = objCommon.DecimalFormat4(decMarketRate)
                GlobalFuns.CalculateYield(txt_valuedate.Text, decFaceValue, decMarketRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                               datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "Y", 0, "")
            ElseIf rdo_CallToRate.Checked = True Then
                If rdo_SemiAnn.SelectedValue = "A" Then
                    txt_YTCAnn.Text = objCommon.DecimalFormat4(txt_YTCAnn.Text)
                    strSemiAnnFlag = "A"
                Else
                    txt_YTCSemi.Text = objCommon.DecimalFormat4(txt_YTCSemi.Text)
                    strSemiAnnFlag = "S"
                End If
                dblResult = IIf(rdo_SemiAnn.SelectedValue = "A", Val(txt_YTCAnn.Text), Val(txt_YTCSemi.Text))
                GlobalFuns.CalculateYield(txt_valuedate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                                datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "C", dblResult, strSemiAnnFlag)
                txt_Rate.Text = objCommon.DecimalFormat4(decMarketRate)
                dblYTCAnn = objCommon.DecimalFormat4(txt_YTCAnn.Text)
                GlobalFuns.CalculateYield(txt_valuedate.Text, decFaceValue, decMarketRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                               datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "Y", 0, "")
            ElseIf rdo_PutToRate.Checked = True Then
                If rdo_SemiAnn.SelectedValue = "A" Then
                    txt_YTPAnn.Text = objCommon.DecimalFormat4(txt_YTPAnn.Text)
                    strSemiAnnFlag = "A"
                Else
                    txt_YTPSemi.Text = objCommon.DecimalFormat4(txt_YTPSemi.Text)
                    strSemiAnnFlag = "S"
                End If
                dblResult = IIf(rdo_SemiAnn.SelectedValue = "A", Val(txt_YTPAnn.Text), Val(txt_YTPSemi.Text))
                GlobalFuns.CalculateYield(txt_valuedate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                                datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "P", dblResult, strSemiAnnFlag)
                txt_Rate.Text = objCommon.DecimalFormat4(decMarketRate)
                GlobalFuns.CalculateYield(txt_valuedate.Text, decFaceValue, decMarketRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                               datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "Y", 0, "")
            End If

            With objCommon
                If rdo_MatToRate.Checked = True And rdo_SemiAnn.SelectedValue = "A" Then
                Else
                    txt_YTMAnn.Text = .DecimalFormat4(dblYTMAnn)
                End If
                If rdo_CallToRate.Checked = True And rdo_SemiAnn.SelectedValue = "A" Then
                Else
                    txt_YTCAnn.Text = .DecimalFormat4(dblYTCAnn)
                End If
                If rdo_PutToRate.Checked = True And rdo_SemiAnn.SelectedValue = "A" Then
                Else
                    txt_YTPAnn.Text = .DecimalFormat4(dblYTPAnn)
                End If
                If Val(Hid_Frequency.Value & "") > 1 Then
                    If rdo_MatToRate.Checked = True And rdo_SemiAnn.SelectedValue = "S" Then
                    Else
                        txt_YTMSemi.Text = .DecimalFormat4(dblYTMSemi)
                    End If
                    If rdo_CallToRate.Checked = True And rdo_SemiAnn.SelectedValue = "S" Then
                    Else
                        txt_YTCSemi.Text = .DecimalFormat4(dblYTCSemi)
                    End If
                    If rdo_PutToRate.Checked = True And rdo_SemiAnn.SelectedValue = "S" Then
                    Else
                        txt_YTPSemi.Text = .DecimalFormat4(dblYTPSemi)
                    End If
                End If
            End With
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    Private Sub FillXIRROptions()
        Try
            If rdo_Yield.Checked = True Then
                If Rbl_StepUp_down = "" Then

                    CalculateXIRRYield(SecurityId, txt_valuedate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_RateActual.SelectedValue, "N")
                Else

                    CalculateXIRRYield(SecurityId, txt_valuedate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_RateActual.SelectedValue, "N")
                End If

            ElseIf rdo_MatToRate.Checked = True Then
                dblYTMAnn = Val(txt_YTMAnn.Text)
                dblYTMSemi = Val(txt_YTMSemi.Text)
                If Rbl_StepUp_down = "" Then
                    CalculateXIRRPrice(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "N", "M")
                Else

                    CalculateXIRRPrice(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "N", "M")
                End If
                txt_Rate.Text = Math.Round(dblPTM, 4)

                CalculateXIRRYield(SecurityId, txt_valuedate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_RateActual.SelectedValue, "N")
            ElseIf rdo_CallToRate.Checked = True Then
                dblYTCAnn = Val(txt_YTCAnn.Text)
                dblYTCSemi = Val(txt_YTCSemi.Text)
                If Rbl_StepUp_down = "" Then

                    CalculateXIRRPrice(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "N", "C")
                Else

                    CalculateXIRRPrice(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "N", "C")
                End If
                txt_Rate.Text = Math.Round(dblPTC, 4)

                CalculateXIRRYield(SecurityId, txt_valuedate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_RateActual.SelectedValue, "N")
            ElseIf rdo_PutToRate.Checked = True Then
                dblYTPAnn = Val(txt_YTPAnn.Text)
                dblYTPSemi = Val(txt_YTPSemi.Text)
                If Rbl_StepUp_down = "" Then

                    CalculateXIRRPrice(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "N", "P")
                Else

                    CalculateXIRRPrice(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "N", "P")
                End If
                txt_Rate.Text = Math.Round(dblPTP, 4)

                CalculateXIRRYield(SecurityId, txt_valuedate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_RateActual.SelectedValue, "N")
            End If

            With objCommon
                If rdo_MatToRate.Checked = True And rdo_SemiAnn.SelectedValue = "A" Then
                Else
                    txt_YTMAnn.Text = .DecimalFormat4(dblYTMAnn)
                End If
                If rdo_CallToRate.Checked = True And rdo_SemiAnn.SelectedValue = "A" Then
                Else
                    txt_YTCAnn.Text = .DecimalFormat4(dblYTCAnn)
                End If
                If rdo_PutToRate.Checked = True And rdo_SemiAnn.SelectedValue = "A" Then
                Else
                    txt_YTPAnn.Text = .DecimalFormat4(dblYTPAnn)
                End If
                If Val(Hid_Frequency.Value & "") > 1 Then
                    If rdo_MatToRate.Checked = True And rdo_SemiAnn.SelectedValue = "S" Then
                    Else
                        txt_YTMSemi.Text = .DecimalFormat4(dblYTMSemi)
                    End If
                    If rdo_CallToRate.Checked = True And rdo_SemiAnn.SelectedValue = "S" Then
                    Else
                        txt_YTCSemi.Text = .DecimalFormat4(dblYTCSemi)
                    End If
                    If rdo_PutToRate.Checked = True And rdo_SemiAnn.SelectedValue = "S" Then
                    Else
                        txt_YTPSemi.Text = .DecimalFormat4(dblYTPSemi)
                    End If
                End If
            End With
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(4), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillMMYOptionsNew()
        Try
            If rdo_Yield.Checked = True Then
                If Rbl_StepUp_down = "" Then

                    CalculateMMYYield(SecurityId, txt_valuedate.Text, Val(txt_Rate.Text), rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "N")
                Else

                    CalculateMMYYield(SecurityId, txt_valuedate.Text, Val(txt_Rate.Text), rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "N")
                End If
                With objCommon
                    txt_YTMAnn.Text = Math.Round(dblYTMAnn, 4)
                    txt_YTCAnn.Text = Math.Round(dblYTCAnn, 4)
                    txt_YTPAnn.Text = Math.Round(dblYTPAnn, 4)
                    If Val(Hid_Frequency.Value & "") > 1 Then
                        txt_YTMSemi.Text = Math.Round(dblYTMSemi, 4)
                        txt_YTCSemi.Text = Math.Round(dblYTCSemi, 4)
                        txt_YTPSemi.Text = Math.Round(dblYTPSemi, 4)
                    End If
                End With
            ElseIf rdo_MatToRate.Checked = True Then
                dblYTMAnn = Val(txt_YTMAnn.Text)
                dblYTMSemi = Val(txt_YTMSemi.Text)
                If Rbl_StepUp_down = "" Then

                    CalculateMMYPrice(SecurityId, txt_valuedate.Text, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "M", "N")
                Else

                    CalculateMMYPrice(SecurityId, txt_valuedate.Text, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "M", "N")
                End If

                txt_Rate.Text = Math.Round(dblPTM, 4)
            ElseIf rdo_CallToRate.Checked = True Then
                dblYTCAnn = Val(txt_YTCAnn.Text)
                dblYTCSemi = Val(txt_YTCSemi.Text)
                If Rbl_StepUp_down = "" Then

                    CalculateMMYPrice(SecurityId, txt_valuedate.Text, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "C", "N")
                Else

                    CalculateMMYPrice(SecurityId, txt_valuedate.Text, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "C", "N")
                End If

                txt_Rate.Text = Math.Round(dblPTC, 4)
            End If
        Catch ex As Exception

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(4), " ") & "');", True)
        End Try
    End Sub

    Private Function FillDateArrays(ByVal strValue As String) As Date()
        Try
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            Throw ex
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            Throw ex
        End Try
    End Function

    Private Sub SetValues()
        Try
            With objCommon
                'datYTM = .DateFormat(txt_SettDate.Text)
                datYTM = .DateFormat(txt_valuedate.Text)
                datInterest = Hid_InterestDate.Value
                datBookClosure = Hid_BookClosureDate.Value
                blnNonGovernment = IIf(Hid_GovernmentFlag.Value = "N", True, False)
                blnRateActual = IIf(rdo_RateActual.SelectedValue = "R", True, False)
                blnDMAT = IIf(rdo_PhysicalDMAT.SelectedValue = "P", False, True)
                decFaceValue = .DecimalFormat4(Val(Hid_FaceValue.Value))
                decRate = .DecimalFormat4(Val(txt_Rate.Text))
                datIssue = Hid_Issue.Value
                datBookClosure = IIf(blnDMAT = True, Hid_DMATBkDate.Value, Hid_BookClosureDate.Value)
                intBKDiff = CalculateBookClosureDiff(datBookClosure, rdo_PhysicalDMAT.SelectedValue, datInterest, blnNonGovernment)
                'blnCompRate = chk_CompRate.Checked
                dblYTMAnn = 0
                dblYTCAnn = 0
                dblYTPAnn = 0
                dblYTMSemi = 0
                dblYTCSemi = 0
                dblYTPSemi = 0
            End With
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillOptions()
        Try
            Dim dsSecurity As DataSet
            Dim dtSecurity As DataTable
            Dim strSecurityNature As String = ""
            Dim I As Int32
            Dim datInfo As Date

            Hid_MatDate.Value = ""
            Hid_MatAmt.Value = ""
            Hid_CoupDate.Value = ""
            Hid_CoupRate.Value = ""
            Hid_CallDate.Value = ""
            Hid_CallAmt.Value = ""
            Hid_PutDate.Value = ""
            Hid_PutAmt.Value = ""

            SecurityId = srh_NameofSecurity.SelectedId
            Hid_SecurityId.Value = SecurityId
            'dsSecurity = objCommon.GetDataSet(SqlDataSourceSecurity)
            OpenConn()
            dtSecurity = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", SecurityId, "SecurityId")
            If dtSecurity IsNot Nothing Then
                For I = 0 To dtSecurity.Rows.Count - 1
                    With dtSecurity.Rows(I)
                        SecurityId = Val(.Item("SecurityId") & "")
                        chrMaxActFlag = Trim(.Item("MaxActualFlag") & "")
                        strSecurityNature = Trim(.Item("NatureOfInstrument") & "")
                        Hid_Issuer.Value = Trim(.Item("SecurityIssuer") & "")
                        Hid_Security.Value = Trim(.Item("SecurityName") & "")

                        Hid_BookClosureDate.Value = IIf(Trim(.Item("BookClosureDate") & "") = "", Date.MinValue, .Item("BookClosureDate"))
                        Hid_InterestDate.Value = IIf(Trim(.Item("FirstInterestDate") & "") = "", Date.MinValue, .Item("FirstInterestDate"))
                        Hid_DMATBkDate.Value = IIf(Trim(.Item("DMATBookClosureDate") & "") = "", Date.MinValue, .Item("DMATBookClosureDate"))
                        Hid_Issue.Value = IIf(Trim(.Item("IssueDate") & "") = "", Date.MinValue, .Item("IssueDate"))
                        datInfo = IIf(Trim(.Item("SecurityInfoDate") & "") = "", Date.MinValue, .Item("SecurityInfoDate"))
                        Hid_GovernmentFlag.Value = Trim(.Item("GovernmentFlag") & "")
                        Hid_Frequency.Value = GetFrequency(Trim(.Item("FrequencyOfInterest") & ""))
                        Hid_FaceValue.Value = Val(.Item("FaceValue") & "")
                        Hid_RateAmtFlag.Value = Trim(.Item("CouponOn") & "")
                        'BrokenInt = (.Item("BrokenInterest"))
                        InterestOnHoliday = (.Item("InterestOnHoliday"))
                        InterestOnSat = (.Item("InterestOnSat"))
                        MaturityOnHoliday = (.Item("MaturityOnHoliday"))
                        MaturityOnSat = (.Item("MaturityOnSat"))
                        If (Request.QueryString("StepUp") & "") = "Y" And Trim(.Item("TypeFlag") & "") = "I" Then
                            FillSecurityInfoDetails(datInfo, Val(.Item("SecurityInfoAmt1") & ""), Trim(.Item("TypeFlag") & ""))
                        Else
                            FillSecurityInfoDetails(datInfo, Val(.Item("SecurityInfoAmt") & ""), Trim(.Item("TypeFlag") & ""))
                        End If

                        'If CheckSecurity() = False Or Val(Hid_Frequency.Value) = 0 Then
                        '    rdo_YXM.Items(0).Selected = False
                        '    'rdo_YXM.Items(0).Enabled = False
                        '    rdo_YXM.Items(1).Selected = True
                        'End If
                        'DisableRadio("M", rdo_MatToRate)
                        DisableRadio("C", rdo_CallToRate)
                        DisableRadio("P", rdo_PutToRate)
                        If Val(Hid_Frequency.Value) <= 1 Then
                            rdo_SemiAnn.Items(1).Enabled = False
                        Else
                            rdo_SemiAnn.Items(1).Enabled = True
                        End If
                    End With
                Next
                Dim strMatDate() As String
                Hid_NatureOfInstrument.Value = strSecurityNature
                If strSecurityNature = "P" Then
                    Hid_CoupDate.Value += CStr(#12/31/9999#) & "!"
                    Hid_CoupRate.Value += dblPepCoupRate & "!"
                Else
                    strMatDate = Split(Hid_MatDate.Value, "!")
                    If Val(Hid_Frequency.Value) <> 0 Then
                        If strMatDate(0) <> "" Then
                            chk_CombineIPMat.Visible = CheckLastIPMaturity(Hid_InterestDate.Value, CDate(strMatDate(UBound(strMatDate) - 1)), Hid_Frequency.Value)
                        End If

                    Else
                        chk_CombineIPMat.Visible = False
                    End If
                End If


                If Hid_GovernmentFlag.Value = "G" Then
                    rbl_Days.Items(0).Enabled = True
                    rbl_Days.Items(1).Enabled = False
                    rbl_Days.Items(2).Enabled = False
                    rbl_Days.Items(0).Selected = True
                Else
                    rbl_Days.Items(0).Enabled = False
                    rbl_Days.Items(1).Enabled = True
                    rbl_Days.Items(2).Enabled = True
                    rbl_Days.Items(0).Selected = False
                    'rbl_Days.Items(1).Selected = True
                End If
            End If

        Catch ex As Exception

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
                        Hid_CoupRate.Value += InfoAmt & "!"
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

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function CheckSecurity() As Boolean
        Try
            Dim I As Int16
            Dim strTypes() As String
            'Dim dsInfo As DataSet
            Dim dt As DataTable
            strTypes = Split("M!I!C!P", "!")
            For I = 0 To strTypes.Length - 1
                Hid_TypeFlag.Value = strTypes(I)

                OpenConn()
                dt = objCommon.FillDataTable(sqlConn, "ID_Check_SecurityInfo", SecurityId, "SecurityId", , Hid_TypeFlag.Value, "TypeFlag")


                If Val(dt.Rows(0).Item(0) & "") > 1 Then
                    Return False
                End If
            Next
            Return True
        Catch ex As Exception

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Function

    Private Sub DisableRadio(ByVal strTypeFlag As String, ByVal rdo As RadioButton)
        Try
            'Dim dsInfo As DataSet
            Dim dtInfo As DataTable
            Hid_TypeFlag.Value = strTypeFlag
            'dsInfo = objCommon.GetDataSet(SqlDataSourceSecurityInfo)
            OpenConn()

            dtInfo = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", SecurityId, "SecurityId", , Hid_TypeFlag.Value, "TypeFlag")
            If dtInfo.Rows.Count > 0 Then
                If Val(dtInfo.Rows(0).Item(0) & "") = 0 Then
                    rdo.Enabled = True
                End If
            Else
                rdo.Enabled = False
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub


    Private Sub FillAccruedDetails()
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            Dim sqlda As New SqlDataAdapter
            Dim dt As New DataTable
            Dim sqldv As New DataView
            Dim RateActual As String = "R"


            sqlComm.Connection = sqlConn
            sqlComm.CommandText = "ID_Fill_QuoteEntry_AccruedDetails"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Parameters.Clear()

            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 4, "I", , , Val(srh_NameofSecurity.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@SettlementDate", SqlDbType.Date, 4, "I", , , objCommon.DateFormat(txt_valuedate.Text))
            objCommon.SetCommandParameters(sqlComm, "@TotalFaceValue", SqlDbType.Decimal, 20, "I", , , (Val(txt_quantum.Text) * Val(cbo_Amount.SelectedValue)))
            objCommon.SetCommandParameters(sqlComm, "@StepUp", SqlDbType.Char, 1, "I", , , "N")
            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(dt)

            If dt.Rows.Count > 0 Then
                If RateActual = "R" Then
                    txt_accrued.Text = RoundToTwo(Val(dt.Rows(0).Item("AccruedInterest") & ""))
                Else
                    txt_accrued.Text = RoundToTwo(Val(dt.Rows(0).Item("AccruedInterest") & ""))
                End If
                txt_lastipdate.Text = Trim((dt.Rows(0).Item("LastIPDate") & ""))
                txt_nofodays.Text = Val(dt.Rows(0).Item("AccruedDays") & "")
                dblNoOfBond = Val(dt.Rows(0).Item("NoOfBond") & "")
                strShutperiod = Trim((dt.Rows(0).Item("Shutperiod") & ""))
                txt_SettlementAmt.Text = Val(dt.Rows(0).Item("TotalConsideration") & "")
                txt_StampDuty.Text = Val(dt.Rows(0).Item("StampDutyAmt") & "")
                txt_TC.Text = Val(dt.Rows(0).Item("SettlementAmt") & "")
                txt_PrinciAmt.Text = Val(dt.Rows(0).Item("SettlementAmt") & "")
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function RoundToTwo(ByVal dec As Decimal) As Decimal
        Try
            Dim rounded As Decimal = Decimal.Round(dec, 2)
            rounded = Format(rounded, "###################0.00")
            Return rounded
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function


    Private Function RoundToFour(ByVal dec As Decimal) As Decimal
        Try
            Dim rounded As Decimal = Decimal.Round(dec, 4)
            rounded = Format(rounded, "###################0.0000")
            Return rounded
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Sub btn_CashFlowExcel_Click(sender As Object, e As EventArgs) Handles btn_CashFlowExcel.Click
        Dim intSecurityId As Integer
        Try

            Hid_CashFlowRpt.Value = "Excel"
            'If (rdo_YXM.SelectedValue = "X" And chk_CashFlow.Checked = True) Then
            intSecurityId = Val(Hid_SecurityId.Value)
            FillOptions()
            Dim I As Single
            MatDate = FillDateArrays(Hid_MatDate.Value)
            MatAmt = FillAmtArrays(Hid_MatAmt.Value)
            CallDate = FillDateArrays(Hid_CallDate.Value)
            CallAmt = FillAmtArrays(Hid_CallAmt.Value)
            CoupDate = FillDateArrays(Hid_CoupDate.Value)
            CoupRate = FillAmtArrays(Hid_CoupRate.Value)
            PutDate = FillDateArrays(Hid_PutDate.Value)
            PutAmt = FillAmtArrays(Hid_PutAmt.Value)
            'FillSettDate()
            SetValues()
            FillAccruedDetails()

            If Hid_RateAmtFlag.Value = "A" Then
                For I = 0 To CoupRate.Length - 1
                    CoupRate(I) = (CoupRate(I) / decFaceValue) * 100
                Next
            End If

            FillXIRROptions_CashFlow()
            'End If

        Catch ex As Exception
            Dim strError As String = ex.Message
        End Try
    End Sub

    Private Sub btn_CashFlowPdf_Click(sender As Object, e As EventArgs) Handles btn_CashFlowPdf.Click

        Dim intSecurityId As Integer
        Try

            Hid_CashFlowRpt.Value = "PDF"
            'If (rdo_YXM.SelectedValue = "X" And chk_CashFlow.Checked = True) Then
            intSecurityId = Val(Hid_SecurityId.Value)
            FillOptions()
            Dim I As Single
            MatDate = FillDateArrays(Hid_MatDate.Value)
            MatAmt = FillAmtArrays(Hid_MatAmt.Value)
            CallDate = FillDateArrays(Hid_CallDate.Value)
            CallAmt = FillAmtArrays(Hid_CallAmt.Value)
            CoupDate = FillDateArrays(Hid_CoupDate.Value)
            CoupRate = FillAmtArrays(Hid_CoupRate.Value)
            PutDate = FillDateArrays(Hid_PutDate.Value)
            PutAmt = FillAmtArrays(Hid_PutAmt.Value)
            'FillSettDate()
            SetValues()
            FillAccruedDetails()

            If Hid_RateAmtFlag.Value = "A" Then
                For I = 0 To CoupRate.Length - 1
                    CoupRate(I) = (CoupRate(I) / decFaceValue) * 100
                Next
            End If

            FillXIRROptions_CashFlow()
            'End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillXIRROptions_CashFlow()
        Dim dbYTMAnn As Double
        Dim dbYTCAnn As Double
        Dim dbYTPAnn As Double
        Dim dbYTMSemi As Double
        Dim dbYTCSemi As Double
        Dim dbYTPSemi As Double
        Try
            If rdo_Yield.Checked = True Then
                If Rbl_StepUp_down = "" Then

                    CalculateXIRRYield_CashFlow(SecurityId, txt_valuedate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_RateActual.SelectedValue, "N")
                Else

                    CalculateXIRRYield_CashFlow(SecurityId, txt_valuedate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_RateActual.SelectedValue, "N")
                End If

            ElseIf rdo_MatToRate.Checked = True Then
                dblYTMAnn = Val(txt_YTMAnn.Text)
                dblYTMSemi = Val(txt_YTMSemi.Text)
                If Rbl_StepUp_down = "" Then
                    'CalculateXIRRPrice(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, Trim(Request.QueryString("StepUp") & ""), "M")
                    CalculateXIRRPrice_CashFlow(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "N", "M")
                Else
                    'CalculateXIRRPrice(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, Rbl_StepUp_down, "M")
                    CalculateXIRRPrice_CashFlow(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "N", "M")
                End If
                txt_Rate.Text = Math.Round(dblPTM, 4)
                'CalculateXIRRYield(SecurityId, txt_valuedate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_RateActual.SelectedValue, Rbl_StepUp_down)
                CalculateXIRRYield_CashFlow(SecurityId, txt_valuedate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_RateActual.SelectedValue, "N")
            ElseIf rdo_CallToRate.Checked = True Then
                dblYTCAnn = Val(txt_YTCAnn.Text)
                dblYTCSemi = Val(txt_YTCSemi.Text)
                If Rbl_StepUp_down = "" Then
                    'CalculateXIRRPrice(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, Trim(Request.QueryString("StepUp") & ""), "C")
                    CalculateXIRRPrice_CashFlow(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "N", "C")
                Else
                    'CalculateXIRRPrice(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, Rbl_StepUp_down, "C")
                    CalculateXIRRPrice_CashFlow(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "N", "C")
                End If
                txt_Rate.Text = Math.Round(dblPTC, 4)
                'CalculateXIRRYield(SecurityId, txt_valuedate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_RateActual.SelectedValue, Rbl_StepUp_down)
                CalculateXIRRYield_CashFlow(SecurityId, txt_valuedate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_RateActual.SelectedValue, "N")
            ElseIf rdo_PutToRate.Checked = True Then
                dblYTPAnn = Val(txt_YTPAnn.Text)
                dblYTPSemi = Val(txt_YTPSemi.Text)
                If Rbl_StepUp_down = "" Then
                    'CalculateXIRRPrice(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, Trim(Request.QueryString("StepUp") & ""), "P")
                    CalculateXIRRPrice_CashFlow(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "N", "P")
                Else
                    'CalculateXIRRPrice(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, Rbl_StepUp_down, "P")
                    CalculateXIRRPrice_CashFlow(SecurityId, txt_valuedate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "N", "P")
                End If
                txt_Rate.Text = Math.Round(dblPTP, 4)
                'CalculateXIRRYield(SecurityId, txt_valuedate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_RateActual.SelectedValue, Rbl_StepUp_down)
                CalculateXIRRYield_CashFlow(SecurityId, txt_valuedate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_RateActual.SelectedValue, "N")
            End If

            With objCommon
                If rdo_MatToRate.Checked = True And rdo_SemiAnn.SelectedValue = "A" Then
                Else
                    dbYTMAnn = .DecimalFormat4(dblYTMAnn)
                End If
                If rdo_CallToRate.Checked = True And rdo_SemiAnn.SelectedValue = "A" Then
                Else
                    dbYTCAnn = .DecimalFormat4(dblYTCAnn)
                End If
                If rdo_PutToRate.Checked = True And rdo_SemiAnn.SelectedValue = "A" Then
                Else
                    dbYTPAnn = .DecimalFormat4(dblYTPAnn)
                End If
                If Val(Hid_Frequency.Value & "") > 1 Then
                    If rdo_MatToRate.Checked = True And rdo_SemiAnn.SelectedValue = "S" Then
                    Else
                        dbYTMSemi = .DecimalFormat4(dblYTMSemi)
                    End If
                    If rdo_CallToRate.Checked = True And rdo_SemiAnn.SelectedValue = "S" Then
                    Else
                        dbYTCSemi = .DecimalFormat4(dblYTCSemi)
                    End If
                    If rdo_PutToRate.Checked = True And rdo_SemiAnn.SelectedValue = "S" Then
                    Else
                        dbYTPSemi = .DecimalFormat4(dblYTPSemi)
                    End If
                End If
            End With

            Hid_CashAmount.Value = strCashAmount

            Hid_CashDate.Value = strCashDate

            FillGrid(Hid_CashAmount.Value, Hid_CashDate.Value, dbYTMAnn, dbYTCAnn, dbYTPAnn, dbYTMSemi, dbYTCSemi, dbYTPSemi)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(4), " ") & "');", True)
        End Try
    End Sub

    'Public Sub CalculateXIRRYield_CashFlow(
    '                         ByVal SecurityId As Integer,
    '                         ByVal SettementDate As String,
    '                         ByVal Rate As Double,
    '                         ByVal FrequencyOfInterest As Integer,
    '                         ByVal ShowCashflow As Boolean,
    '                         ByVal RateOrActual As String,
    '                         Optional ByVal StepUp As String = "N",
    '                         Optional ByVal YieldType As String = "M")

    '    Dim lstParam As New List(Of SqlParameter)()
    '    Dim ds As New DataSet
    '    Dim dtYTM As DataTable
    '    Dim dtYTC As DataTable
    '    Dim dtYTP As DataTable
    '    Dim I As Integer
    '    Dim strUrl As String
    '    Dim strAmount As String = ""
    '    Dim strDate As String = ""
    '    Dim objComm As New ClsCommon
    '    Try
    '        lstParam.Add(New SqlParameter("@SecurityId", SecurityId))
    '        lstParam.Add(New SqlParameter("@Dated", SettementDate))
    '        lstParam.Add(New SqlParameter("@PurchaseRate", Rate))
    '        lstParam.Add(New SqlParameter("@RateOrActual", RateOrActual))
    '        lstParam.Add(New SqlParameter("@StepUp", StepUp))
    '        lstParam.Add(New SqlParameter("@YieldType", YieldType))

    '        ds = objComm.FillDetails(lstParam, "WDM_Fill_SecurityYieldCashFlow")
    '        If ds.Tables.Count >= 3 Then
    '            I = 0
    '            dtYTM = ds.Tables(0)
    '            If dtYTM.Rows.Count > 0 Then
    '                ReDim XirrDate(dtYTM.Rows.Count - 1)
    '                ReDim XirrAmt(dtYTM.Rows.Count - 1)

    '                For Each row As DataRow In dtYTM.Rows
    '                    XirrDate(I) = row("IPDate")
    '                    XirrAmt(I) = row("IPAmount")
    '                    strDate = strDate & CDate(row("IPDate")) & "!"
    '                    strAmount = strAmount & row("IPAmount") & "!"
    '                    I = I + 1
    '                Next

    '                dblYTMAnn = FinancialFunc.XIRR(XirrAmt, XirrDate, 0.1)
    '                dblYTMSemi = FinancialFunc.nominal(dblYTMAnn, 2)
    '                dblYTMAnn = dblYTMAnn * 100
    '                dblYTMSemi = dblYTMSemi * 100

    '                If ShowCashflow Then
    '                    Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)
    '                    strAmount = strAmount.Substring(0, strAmount.Length - 1)
    '                    strDate = strDate.Substring(0, strDate.Length - 1)
    '                    FillGrid(strAmount, strDate)
    '                    'strUrl = "CashFlow.aspx?Amount=" & strAmount & "&Date=" & strDate
    '                    'ScriptManager.RegisterClientScriptBlock(page, GetType(Page), "MyScript", "window.open('" & strUrl & "','_blank','rs=yes,top=100,left=500,width=300,height=500');", True)
    '                End If
    '            End If

    '            I = 0
    '            dtYTC = ds.Tables(1)
    '            If dtYTC.Rows.Count > 0 Then
    '                ReDim XirrDate(dtYTC.Rows.Count - 1)
    '                ReDim XirrAmt(dtYTC.Rows.Count - 1)
    '                strAmount = ""
    '                strDate = ""
    '                For Each row As DataRow In dtYTC.Rows
    '                    XirrDate(I) = row("IPDate")
    '                    XirrAmt(I) = row("IPAmount")
    '                    strDate = strDate & CDate(row("IPDate")) & "!"
    '                    strAmount = strAmount & row("IPAmount") & "!"
    '                    I = I + 1
    '                Next

    '                dblYTCAnn = FinancialFunc.XIRR(XirrAmt, XirrDate, 0.1)
    '                dblYTCSemi = FinancialFunc.nominal(dblYTCAnn, 2)
    '                dblYTCAnn = dblYTCAnn * 100
    '                dblYTCSemi = dblYTCSemi * 100

    '                If ShowCashflow And dtYTM.Rows.Count = 0 Then
    '                    Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)
    '                    strAmount = strAmount.Substring(0, strAmount.Length - 1)
    '                    strDate = strDate.Substring(0, strDate.Length - 1)
    '                    strUrl = "CashFlow.aspx?Amount=" & strAmount & "&Date=" & strDate
    '                    ScriptManager.RegisterClientScriptBlock(page, GetType(Page), "MyScript", "window.open('" & strUrl & "','_blank','rs=yes,top=100,left=500,width=300,height=500');", True)
    '                End If
    '            End If

    '            I = 0
    '            dtYTP = ds.Tables(2)
    '            If dtYTP.Rows.Count > 0 Then
    '                ReDim XirrDate(dtYTP.Rows.Count - 1)
    '                ReDim XirrAmt(dtYTP.Rows.Count - 1)
    '                strAmount = ""
    '                strDate = ""
    '                For Each row As DataRow In dtYTP.Rows
    '                    XirrDate(I) = row("IPDate")
    '                    XirrAmt(I) = row("IPAmount")
    '                    strDate = strDate & CDate(row("IPDate")) & "!"
    '                    strAmount = strAmount & row("IPAmount") & "!"
    '                    I = I + 1
    '                Next

    '                dblYTPAnn = FinancialFunc.XIRR(XirrAmt, XirrDate, 0.1)
    '                dblYTPSemi = FinancialFunc.nominal(dblYTPAnn, 2)
    '                dblYTPAnn = dblYTPAnn * 100
    '                dblYTPSemi = dblYTPSemi * 100

    '                If ShowCashflow And dtYTM.Rows.Count = 0 And dtYTC.Rows.Count = 0 Then
    '                    Dim page As Page = TryCast(HttpContext.Current.CurrentHandler, Page)
    '                    strAmount = strAmount.Substring(0, strAmount.Length - 1)
    '                    strDate = strDate.Substring(0, strDate.Length - 1)
    '                    strUrl = "CashFlow.aspx?Amount=" & strAmount & "&Date=" & strDate
    '                    ScriptManager.RegisterClientScriptBlock(page, GetType(Page), "MyScript", "window.open('" & strUrl & "','_blank','rs=yes,top=100,left=500,width=300,height=500');", True)
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        'objUtil.WritErrorLog(PgName, "CalculateXIRRYield", "Error in CalculateXIRRYield", "", ex)
    '        Throw ex
    '        GC.Collect()
    '        GC.WaitForPendingFinalizers()
    '        GC.Collect()
    '    Finally
    '        GC.Collect()
    '        GC.WaitForPendingFinalizers()
    '        GC.Collect()
    '    End Try
    'End Sub

    Private Sub FillGrid(ByVal Amount As String, ByVal Dates As String, ByVal dblYTMAnn As Double, ByVal dblYTCAnn As Double, ByVal dblYTPAnn As Double, ByVal dblYTMsemi As Double, ByVal dblYTCsemi As Double, ByVal dblYTPsemi As Double)
        Try
            Dim dc As DataColumn
            Dim dt As New DataTable
            Dim I As Integer
            Dim dtRow As DataRow
            Dim arrDate() As String
            Dim arrAmt() As String
            dc = AddColumnInfo("Date")
            dt.Columns.Add(dc)
            dc = AddColumnInfo("Amount")
            dt.Columns.Add(dc)
            'arrDate = Split(Request.QueryString("Date"), "!")
            'arrAmt = Split(Request.QueryString("Amount"), "!")
            arrDate = Split(Dates, "!")
            arrAmt = Split(Amount, "!")
            For I = 0 To arrDate.Length - 1
                If Trim(arrDate(I)) = CStr(Date.MinValue) Then Exit For
                dtRow = dt.NewRow
                dtRow.Item(0) = Format(CDate(arrDate(I)), "dd MMM yyyy")
                If Val(dblNoOfBond) = 0 Then
                    dtRow.Item(1) = (Format(objCommon.DecimalFormat(arrAmt(I) & ""), "################0.00"))
                Else
                    dtRow.Item(1) = (Format(objCommon.DecimalFormat(arrAmt(I) * Val(txt_NoOfBonds.Text) & ""), "################0.00"))
                End If

                dt.Rows.Add(dtRow)
            Next

            If dt.Rows.Count > 0 Then
                If (Hid_CashFlowRpt.Value = "Excel") Then
                    FillExcel(dt, dblYTMAnn, dblYTCAnn, dblYTPAnn, dblYTMsemi, dblYTCsemi, dblYTPsemi)
                    'ExportExcel(dt, dblYTMAnn, dblYTCAnn, dblYTPAnn, dblYTMsemi, dblYTCsemi, dblYTPsemi)
                Else
                    FillPDF(dt, dblYTMAnn, dblYTCAnn, dblYTPAnn, dblYTMsemi, dblYTCsemi, dblYTPsemi)
                End If
            End If
            'grdView.DataSource = dt
            'grdView.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function AddColumnInfo(ByVal strParam As String) As DataColumn
        Try
            Dim dc As DataColumn
            dc = New DataColumn
            dc.ColumnName = strParam
            dc.DataType = GetType(String)
            Return dc
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Public Sub SetBorder(ByVal ws As IXLWorksheet, ByVal DataCell As String)
        ws.Cell(DataCell).Style.Border.TopBorder = XLBorderStyleValues.Thin
        ws.Cell(DataCell).Style.Border.BottomBorder = XLBorderStyleValues.Thin
        ws.Cell(DataCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin
        ws.Cell(DataCell).Style.Border.RightBorder = XLBorderStyleValues.Thin
    End Sub

    Public Sub SetText(ByVal ws As IXLWorksheet, ByVal DataCell As String)
        ws.Cell(DataCell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
        ws.Cell(DataCell).Style.Font.FontName = "Calibri"
        ws.Cell(DataCell).Style.Font.FontSize = 12
    End Sub

    Private Function ColumnIndexToColumnLetter(colIndex As Integer) As String
        Dim div As Integer = colIndex
        Dim colLetter As String = String.Empty
        Dim modnum As Integer = 0

        While div > 0
            modnum = (div - 1) Mod 26
            colLetter = Chr(65 + modnum) & colLetter
            div = CInt((div - modnum) \ 26)
        End While

        Return colLetter
    End Function

    Public Sub ExportExcel(ByVal dtprint As DataTable, ByVal dblYTMAnn As Double, ByVal dblYTCAnn As Double, ByVal dblYTPAnn As Double, ByVal dblYTMsemi As Double, ByVal dblYTCsemi As Double, ByVal dblYTPsemi As Double)
        Dim tw As New System.IO.StringWriter()
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        Dim dgGrid As New DataGrid()
        Dim dt As DataTable = New DataTable()
        Dim intBondQty As Integer = 0
        Dim RowIndex As Integer = 2
        Dim ColIndex As Integer = 1
        Try
            FillAccruedDetails_CashFlow()
            OpenConn()

            CloseConn()
            intBondQty = objCommon.DecimalFormat((Val(txt_quantum.Text) * Val(cbo_Amount.SelectedValue)) / Val(lit_facevalue.Text))
            intBondQty = Format(intBondQty, "################0.00")
            Dim strPath As String = ""

            Dim strDirectory As String = ConfigurationManager.AppSettings("offerPath") & "\CashFlow"
            If (Directory.Exists(strDirectory) = False) Then
                Directory.CreateDirectory(strDirectory)
            End If

            strPath = strDirectory & "\CashFlow_" & System.DateTime.Now.ToString("h:mm:ss tt").Replace(":", "") + ".xlsx"
            'strPath = strDirectory & "\CashFlow_" & strAbbr & Srh_NameOFClient.SearchTextBox.Text & "_" & srh_NameofSecurity.SearchTextBox.Text + ".xlsx"
            If dtprint.Rows.Count > 0 Then
                Using wb As New XLWorkbook()
                    Dim ws As IXLWorksheet = wb.Worksheets.Add(dt, "CashFlow")



                    Dim DataCell As String = ""
                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "To,"
                    SetText(ws, DataCell)
                    ws.Range("A7:C7").Merge()
                    RowIndex = RowIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "" & Trim(Srh_NameOFClient.SearchTextBox.Text.ToString)
                    SetText(ws, DataCell)
                    ws.Range("A8:B8").Merge()
                    RowIndex = RowIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Dear Sir/Madam,"
                    SetText(ws, DataCell)
                    ws.Range("A9:B9").Merge()
                    RowIndex = RowIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Please find Cash Flow and Quotation for:"
                    SetText(ws, DataCell)
                    ws.Range("A10:B10").Merge()
                    RowIndex = RowIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "ISIN:"
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)
                    ColIndex = ColIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = txt_isin.Text.ToString()
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)
                    RowIndex = RowIndex + 1
                    ColIndex = ColIndex - 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Name of Security:"
                    Dim SecurityRowIndex As Integer = RowIndex
                    SetText(ws, DataCell)
                    ColIndex = ColIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = srh_NameofSecurity.SearchTextBox.Text
                    'SetBorder(ws, DataCell)
                    'SetText(ws, DataCell)
                    ws.Cell(DataCell).Style.Alignment.WrapText = True


                    RowIndex = RowIndex + 1
                    ColIndex = ColIndex - 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Deal Date:"
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)
                    ColIndex = ColIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = Format(objCommon.DateFormat(txt_Dealdate.Text), "dd MMM yyyy")
                    ws.Cell(DataCell).Style.NumberFormat.Format = "dd-MMM-yy"
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)

                    RowIndex = RowIndex + 1
                    ColIndex = ColIndex - 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Settlement Date:"
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)
                    ColIndex = ColIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = Format(objCommon.DateFormat(txt_valuedate.Text), "dd MMM yyyy")
                    ws.Cell(DataCell).Style.NumberFormat.Format = "dd-MMM-yy"
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)

                    If Trim(strShutperiod) <> "" Then
                        RowIndex = RowIndex + 1
                        ColIndex = ColIndex - 1

                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = "Record Date:"
                        'SetBorder(ws, DataCell)
                        SetText(ws, DataCell)
                        ColIndex = ColIndex + 1

                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = Format(objCommon.DateFormat(strShutperiod), "dd MMM yyyy")
                        ws.Cell(DataCell).Style.NumberFormat.Format = "dd-MMM-yy"
                        'SetBorder(ws, DataCell)
                        SetText(ws, DataCell)
                    End If

                    RowIndex = RowIndex + 1
                    ColIndex = ColIndex - 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Face Value:"
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)
                    ColIndex = ColIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = Format(lit_facevalue.Text.ToString(), "################0.00")
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)

                    RowIndex = RowIndex + 1
                    ColIndex = ColIndex - 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Quantity:"
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)
                    ColIndex = ColIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = Format(txt_NoOfBonds.Text, "################0.00")
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)

                    RowIndex = RowIndex + 1
                    ColIndex = ColIndex - 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Total Face Value:"
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)
                    ColIndex = ColIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = txt_quantum.Text
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)

                    RowIndex = RowIndex + 1
                    ColIndex = ColIndex - 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Rate:"
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)

                    ColIndex = ColIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = txt_Rate.Text
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)

                    If Val(dblYTMAnn) <> 0 Then
                        RowIndex = RowIndex + 1
                        ColIndex = ColIndex - 1

                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = "YTM Ann:"
                        'SetBorder(ws, DataCell)
                        SetText(ws, DataCell)
                        ColIndex = ColIndex + 1

                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = Convert.ToString(dblYTMAnn)
                        'SetBorder(ws, DataCell)
                        SetText(ws, DataCell)
                    End If

                    If Val(dblYTMsemi) <> 0 Then
                        RowIndex = RowIndex + 1
                        ColIndex = ColIndex - 1

                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = "YTM Semi:"
                        'SetBorder(ws, DataCell)
                        SetText(ws, DataCell)
                        ColIndex = ColIndex + 1

                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = Convert.ToString(dblYTMsemi)
                        'SetBorder(ws, DataCell)
                        SetText(ws, DataCell)
                    End If

                    If Val(dblYTCAnn) <> 0 Then
                        RowIndex = RowIndex + 1
                        ColIndex = ColIndex - 1

                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = "YTC Ann:"
                        'SetBorder(ws, DataCell)
                        SetText(ws, DataCell)
                        ColIndex = ColIndex + 1

                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = Convert.ToString(dblYTCAnn)
                        'SetBorder(ws, DataCell)
                        SetText(ws, DataCell)
                    End If

                    If Val(dblYTCsemi) <> 0 Then
                        RowIndex = RowIndex + 1
                        ColIndex = ColIndex - 1

                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = "YTC Semi:"
                        'SetBorder(ws, DataCell)
                        SetText(ws, DataCell)
                        ColIndex = ColIndex + 1

                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = Convert.ToString(dblYTCsemi)
                        'SetBorder(ws, DataCell)
                        SetText(ws, DataCell)
                    End If

                    If Val(dblYTPAnn) <> 0 Then
                        RowIndex = RowIndex + 1
                        ColIndex = ColIndex - 1

                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = "YTP Ann:"
                        'SetBorder(ws, DataCell)
                        SetText(ws, DataCell)
                        ColIndex = ColIndex + 1

                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = Convert.ToString(dblYTPAnn)
                        'SetBorder(ws, DataCell)
                        SetText(ws, DataCell)
                    End If

                    If Val(dblYTPsemi) <> 0 Then
                        RowIndex = RowIndex + 1
                        ColIndex = ColIndex - 1

                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = "YTP Semi:"
                        'SetBorder(ws, DataCell)
                        SetText(ws, DataCell)
                        ColIndex = ColIndex + 1

                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = Convert.ToString(dblYTPsemi)
                        'SetBorder(ws, DataCell)
                        SetText(ws, DataCell)
                    End If

                    RowIndex = RowIndex + 1
                    ColIndex = ColIndex - 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Amount:"
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)
                    ColIndex = ColIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = Convert.ToString(dblCashFlowAmt)
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)

                    RowIndex = RowIndex + 1
                    ColIndex = ColIndex - 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Accrued Interest:"
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)
                    ColIndex = ColIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = Convert.ToString(dblCashFlowAccInterest)
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)

                    RowIndex = RowIndex + 1
                    ColIndex = ColIndex - 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Settlement Amount:"
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)
                    ColIndex = ColIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = Convert.ToString(objCommon.DecimalFormat2(dblCashFlowTotalConsideration))
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)

                    RowIndex = RowIndex + 1
                    ColIndex = ColIndex - 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Stamp Duty:"
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)
                    ColIndex = ColIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = Convert.ToString(objCommon.DecimalFormat(dblStampDutyAmt))
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)

                    RowIndex = RowIndex + 1
                    ColIndex = ColIndex - 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Total Amount Payable:"
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)
                    ColIndex = ColIndex + 1

                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = Convert.ToString(dblCashFlowSettAmt)
                    'SetBorder(ws, DataCell)
                    SetText(ws, DataCell)

                    RowIndex = RowIndex + 2
                    ColIndex = ColIndex - 1
                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Date"
                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(36, 64, 98)
                    ws.Cell(DataCell).Style.Font.FontColor = XLColor.White
                    ws.Cell(DataCell).Style.Font.Bold = True
                    SetBorder(ws, DataCell)
                    SetText(ws, DataCell)

                    ColIndex = ColIndex + 1
                    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ws.Cell(DataCell).Value = "Amount"
                    ws.Cell(DataCell).Style.Fill.BackgroundColor = XLColor.FromArgb(36, 64, 98)
                    ws.Cell(DataCell).Style.Font.FontColor = XLColor.White
                    ws.Cell(DataCell).Style.Font.Bold = True
                    SetBorder(ws, DataCell)
                    SetText(ws, DataCell)


                    Dim I As Integer
                    For I = 0 To dtprint.Rows.Count - 1
                        RowIndex = RowIndex + 1
                        ColIndex = ColIndex - 1
                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = Convert.ToString(dtprint.Rows(I)("Date").ToString())
                        ws.Cell(DataCell).Style.NumberFormat.Format = "dd-MMM-yy"
                        SetBorder(ws, DataCell)
                        SetText(ws, DataCell)

                        ColIndex = ColIndex + 1
                        DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                        ws.Cell(DataCell).Value = Convert.ToString(dtprint.Rows(I)("Amount").ToString())
                        SetBorder(ws, DataCell)
                        SetText(ws, DataCell)
                    Next

                    'RowIndex = RowIndex + 2
                    'ColIndex = ColIndex - 1
                    'DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    'ws.Cell(DataCell).Value = "Disclaimer: These are indicative rates and subject to market conditions."
                    'ws.Range("B" & RowIndex & ":C" & RowIndex).Merge()
                    'SetText(ws, DataCell)


                    'RowIndex = RowIndex + 2
                    ''ColIndex = ColIndex - 1
                    'DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    'ws.Cell(DataCell).Value = strUserName
                    'ws.Range("B" & RowIndex & ":C" & RowIndex).Merge()
                    ''RowIndex = RowIndex + 1
                    'If Trim(strMobileNo) <> "" Then
                    '    RowIndex = RowIndex + 1
                    '    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    '    ws.Cell(DataCell).Value = strMobileNo
                    '    ws.Cell(DataCell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    '    ws.Range("B" & RowIndex & ":C" & RowIndex).Merge()
                    '    SetText(ws, DataCell)
                    'End If

                    'If Trim(strEmailId) <> "" Then
                    '    RowIndex = RowIndex + 1
                    '    DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    '    ws.Cell(DataCell).Value = strEmailId
                    '    ws.Cell(DataCell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    '    ws.Range("B" & RowIndex & ":C" & RowIndex).Merge()
                    '    SetText(ws, DataCell)
                    'End If
                    'RowIndex = RowIndex + 3
                    'DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    'ws.Cell(DataCell).Value = strcopyright '"Disclaimer: These are indicative rates and subject to market conditions."
                    'ws.Range("B" & RowIndex & ":C" & RowIndex).Merge()
                    'SetText(ws, DataCell)
                    ''RowIndex = RowIndex + 1
                    ''ColIndex = ColIndex - 1
                    ''DataCell = ColumnIndexToColumnLetter(ColIndex) & RowIndex.ToString()
                    ''ws.Cell(DataCell).Value = strcopyright
                    ''ws.Cell(DataCell).Style.Font.Bold = True
                    'ws.Columns().AdjustToContents()
                    'ws.Row(SecurityRowIndex).Height = Convert.ToInt32(Hid_SecurityHeight.Value)
                    'ws.Row(SecurityRowIndex).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    'ws.Column(3).Width = Convert.ToInt32(Hid_SecurityWidth.Value)
                    'ws.Cell(SecurityRowIndex, 3).Style.Alignment.WrapText = True

                    ws.ShowGridLines = False
                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename=" & "CashFlow.xlsx")
                    Using MyMemoryStream As New MemoryStream()
                        wb.SaveAs(MyMemoryStream)
                        MyMemoryStream.WriteTo(Response.OutputStream)
                        Response.Flush()
                        Response.End()
                    End Using
                End Using
            End If
        Catch ex As Exception
            Dim file As System.IO.StreamWriter
            file = My.Computer.FileSystem.OpenTextFileWriter("c:\test.txt", True)
            file.WriteLine(ex.Message + "  " + System.DateTime.Now.ToString())
            file.Close()
        End Try
    End Sub

    Private Sub FillExcel(ByVal dtprint As DataTable, ByVal dblYTMAnn As Double, ByVal dblYTCAnn As Double, ByVal dblYTPAnn As Double, ByVal dblYTMsemi As Double, ByVal dblYTCsemi As Double, ByVal dblYTPsemi As Double)
        Dim tw As New System.IO.StringWriter()
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        Dim dgGrid As New DataGrid()
        Dim intBondQty As Decimal
        objMIS = New MISReports()
        Try
            'dtprint = GetData()
            FillAccruedDetails_CashFlow()
            intBondQty = objCommon.DecimalFormat((Val(txt_quantum.Text) * Val(cbo_Amount.SelectedValue)) / Val(lit_facevalue.Text))
            intBondQty = Format(intBondQty, "################0.00")
            If dtprint.Rows.Count > 0 Then
                dgGrid.DataSource = dtprint
                '   Report Header
                objMIS.DownloadCashFlow_EXCEL(dtprint, srh_NameofSecurity.SearchTextBox.Text, txt_isin.Text, Format(objCommon.DateFormat(txt_Dealdate.Text), "dd MMM yyyy"), Format(objCommon.DateFormat(txt_valuedate.Text), "dd MMM yyyy"), (Val(txt_quantum.Text) * Val(cbo_Amount.SelectedValue)), txt_NoOfBonds.Text, txt_Rate.Text, txt_YTMAnn.Text, txt_YTCAnn.Text, txt_YTPAnn.Text, txt_lastipdate.Text, txt_nofodays.Text, txt_PrinciAmt.Text, Convert.ToString(dblCashFlowAccInterest), Convert.ToString(dblCashFlowTotalConsideration), Convert.ToString(dblStampDutyAmt), Convert.ToString(dblCashFlowSettAmt), strAmountInwords, Hid_NatureOfInstrument.Value)

            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "msg", "alert('No record found')", True)
            End If
        Catch ex As Exception

        End Try
    End Sub



    Private Sub FillExcel18Nov2022(ByVal dtprint As DataTable, ByVal dblYTMAnn As Double, ByVal dblYTCAnn As Double, ByVal dblYTPAnn As Double, ByVal dblYTMsemi As Double, ByVal dblYTCsemi As Double, ByVal dblYTPsemi As Double)
        Dim tw As New System.IO.StringWriter()
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        Dim dgGrid As New DataGrid()
        Dim intBondQty As Decimal
        Try
            'dtprint = GetData()
            FillAccruedDetails_CashFlow()
            intBondQty = objCommon.DecimalFormat((Val(txt_quantum.Text) * Val(cbo_Amount.SelectedValue)) / Val(lit_facevalue.Text))
            intBondQty = Format(intBondQty, "################0.00")
            If dtprint.Rows.Count > 0 Then
                dgGrid.DataSource = dtprint
                '   Report Header

                hw.WriteLine("<font size='3' align='center'>Dear Sir/Madam,</font>")
                hw.WriteLine("<br/><br/>")
                hw.WriteLine("<font size='3' align='center'>Please find Cash Flow and Quotation for:</font>")
                hw.WriteLine("<font size='3' align='center'></font>")
                hw.WriteLine("<table border ='2'")
                hw.WriteLine("<tr align='left'><td  style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>Name of Security</td><td align='right' style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "';width:500px!important;'>" + srh_NameofSecurity.SearchTextBox.Text + "</td></tr>")
                hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>ISIN </td><td align='right' style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + txt_isin.Text.ToString() + "</td></tr>")
                hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>Deal Date</td><td align='right' style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Format(objCommon.DateFormat(txt_Dealdate.Text), "dd MMM yyyy") + "</td></tr>")
                hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>Settlement Date</td><td align='right' style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Format(objCommon.DateFormat(txt_valuedate.Text), "dd MMM yyyy") + "</td></tr>")
                'If Trim(strShutperiod) <> "" Then
                '    hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>Record Date  </td><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Format(objCommon.DateFormat(strShutperiod), "dd MMM yyyy") + "</td></tr>")
                'End If

                'hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>Face Value </td><td align='right' style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + lit_facevalue.Text.ToString + "</td></tr>")

                hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>Quantum</td><td align='right'  style='color:blue;font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + (Val(txt_quantum.Text) * Val(cbo_Amount.SelectedValue)).ToString("#,##0.00") + "</td></tr>")
                hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>Quantity  </td><td align='right' style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Convert.ToString(txt_NoOfBonds.Text) + "</td></tr>")

                hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>Rate</td><td align='right' style='color:blue;font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Val(txt_Rate.Text).ToString("#,##0.0000") + "</td></tr>")
                'If Val(txt_YTMAnn.Text) <> 0 Then
                '    hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>YTM Ann </td><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Convert.ToString(txt_YTMAnn.Text) + "%" + "</td></tr>")
                '    'hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>YTM Ann </td><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + dblYTMAnn  + "%" + "</td></tr>")
                'End If
                If Val(txt_YTMAnn.Text) <> 0 Then
                    hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>YTM Ann</td><td align='right' style='color:blue;font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Val(txt_YTMAnn.Text).ToString("#,##0.0000") + "   %" + "</td></tr>")
                End If
                If Val(txt_YTMSemi.Text) <> 0 Then
                    hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>YTM Semi </td><td align='right' style='color:blue;font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Val(txt_YTMSemi.Text).ToString("#,##0.0000") + "   %" + "</td></tr>")
                End If
                If Val(txt_YTCAnn.Text) <> 0 Then
                    hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>YTC Ann </td><td align='right' style='color:blue;font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Val(txt_YTCAnn.Text).ToString("#,##0.0000") + "   %" + "</td></tr>")
                End If

                If Val(txt_YTCSemi.Text) <> 0 Then
                    hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>YTC Semi  </td><td align='right' style='color:blue;font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Val(txt_YTCSemi.Text).ToString("#,##0.0000") + "   %" + "</td></tr>")
                End If

                If Val(txt_YTPAnn.Text) <> 0 Then
                    hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>YTP Ann </td><td align='right' style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Val(txt_YTPAnn.Text).ToString("#,##0.0000") + "   %" + "</td></tr>")
                End If


                If Val(txt_YTPSemi.Text) <> 0 Then
                    hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>YTP Semi  </td><td align='right' style='color:blue;font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Val(txt_YTPSemi.Text).ToString("#,##0.0000") + "   %" + "</td></tr>")
                End If
                hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>Last IP Date</td><td align='right' style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + txt_lastipdate.Text.ToString() + "</td></tr>")
                hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>No of Days </td><td align='right' style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + txt_nofodays.Text.ToString() + "</td></tr>")

                hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>Principal Amount </td><td align='right' style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Convert.ToDouble(txt_PrinciAmt.Text).ToString("#,##0.00") + "</td></tr>")
                hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>Accrued Interest   </td><td align='right' style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Convert.ToDouble(dblCashFlowAccInterest).ToString("#,##0.00") + "</td></tr>")
                hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>Total Consideration   </td><td align='right' style='color:blue;font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + dblCashFlowTotalConsideration.ToString("#,##0.00") + "</td></tr>")
                hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>Stamp Duty   </td><td align='right' style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Convert.ToDouble(dblStampDutyAmt).ToString("#,##0.00") + "</td></tr>")
                hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>Settlement Amount    </td><td align='right' style='color:blue;font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Convert.ToDouble(dblCashFlowSettAmt).ToString("#,##0.00") + "</td></tr>")
                hw.WriteLine("<tr align='left'><td style='font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>Amount in Words</td><td align='left' style='color:blue;font-family:Calibri;font-weight:bold;font-size:" + Hid_ExcelPDFFont.Value + "'>" + Convert.ToString(strAmountInwords) + "</td></tr>")
                hw.WriteLine("<br/><br/>")
                'hw.WriteLine("<tr/>'&nbsp;'<tr/>")

                'hw.WriteLine("<tr align='left'><td><font size='3' align='center'>Cash Flow</td></tr>")
                hw.WriteLine("<br></br>")
                hw.WriteLine("</table>")
                hw.WriteLine("<table><tr><td></td></tr><tr><td style ='font-weight:bold'><font size='3' align='center'>Cash Flow</font></td></tr></table>")
                hw.WriteLine("<br></br>")
                hw.AddAttribute(HtmlTextWriterAttribute.Width, "1000")

                dgGrid.HeaderStyle.Font.Bold = True
                dgGrid.DataBind()
                dgGrid.Width = 9000
                dgGrid.GridLines = GridLines.Both


                dgGrid.RenderControl(hw)
                '   Write the HTML back to the browser.
                Response.ClearHeaders()

                Response.AddHeader("content-disposition", "attachment;filename=CashFlow.xls")

                Response.ContentType = "application/vnd.ms-excel"
                Me.EnableViewState = False

                Response.Write(tw.ToString())
                Response.End()
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "msg", "alert('No record found')", True)
            End If
        Catch ex As Exception

        End Try
    End Sub



    Private Sub FillPDF(ByVal dt As DataTable, ByVal dblYTMAnn As Double, ByVal dblYTCAnn As Double, ByVal dblYTPAnn As Double, ByVal dblYTMsemi As Double, ByVal dblYTCsemi As Double, ByVal dblYTPsemi As Double)

        Dim pdfDoc As Document = New Document(PageSize.A4.Rotate(), 10, 10, 10, 10)

        Try
            Dim intBondQty As Decimal

            FillAccruedDetails_CashFlow()
            intBondQty = objCommon.DecimalFormat2(Val(txt_quantum.Text) * Val(cbo_Amount.SelectedValue) / Val(lit_facevalue.Text))

            PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream)
            pdfDoc.Open()

            Dim font8 As Font = FontFactory.GetFont("CALIBRI", 10, 0)
            Dim fontDisclaimer As Font = FontFactory.GetFont("CALIBRI", 8, 6, Color.RED)
            Dim fontBlue As Font = FontFactory.GetFont("CALIBRI", 10, 6, Color.BLUE)



            Dim PdfTable As PdfPTable = New PdfPTable(2)
            PdfTable.WidthPercentage = 60 ' Table size is set to 100% of the page
            PdfTable.HorizontalAlignment = 1 'Left aLign
            PdfTable.SpacingAfter = 10
            pdfDoc.SetMargins(20.0F, 20.0F, 20.0F, 20.0F)



            Dim sglTblHdWidths(1) As Single
            sglTblHdWidths(0) = 30
            sglTblHdWidths(1) = 70
            PdfTable.SetWidths(sglTblHdWidths)
            Dim PdfPCell As PdfPCell = Nothing

            PdfPCell = New PdfPCell(New Phrase(New Chunk("Dear Sir / Madam,", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfPCell.Border = 0
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfPCell.Border = 0
            PdfTable.AddCell(PdfPCell)

            Dim FontColour = New Color(193, 36, 67)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("DISCLAIMER : This Cash Flow has been prepared on the basis of IM / NSDL details available with us. Incase of any change / alteration / modification we are not liable for the same. Kindly confirm and cross check at your end then consider the same.", fontDisclaimer)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfPCell.Border = 0
            PdfPCell.Colspan = 2
            PdfTable.AddCell(PdfPCell)

            'PdfPCell = New PdfPCell(New Phrase(New Chunk("", font8)))
            'PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            'PdfPCell.Padding = 4
            'PdfPCell.Border = 0
            'PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfPCell.Border = 0
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfPCell.Border = 0
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("Security Name", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)
            PdfPCell.Border = Rectangle.TOP_BORDER + Rectangle.BOTTOM_BORDER

            PdfPCell = New PdfPCell(New Phrase(New Chunk(srh_NameofSecurity.SearchTextBox.Text, font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)


            PdfPCell = New PdfPCell(New Phrase(New Chunk("ISIN", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk(txt_isin.Text, font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("Deal Date", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk(Format(objCommon.DateFormat(txt_Dealdate.Text.ToString()), "dd MMM yyyy"), font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("Settlement Date", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk(Format(objCommon.DateFormat(txt_valuedate.Text.ToString()), "dd MMM yyyy"), font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("Quantum", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk((Val(txt_quantum.Text) * Val(cbo_Amount.SelectedValue)).ToString("#,##0"), fontBlue)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("Quantity", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk(txt_NoOfBonds.Text, font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("Rate", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk(Convert.ToDouble(txt_Rate.Text).ToString("#,##0.0000"), fontBlue)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            If Val(txt_YTMAnn.Text) <> 0 And Hid_NatureOfInstrument.Value = "NP" Then
                PdfPCell = New PdfPCell(New Phrase(New Chunk("YTM Ann", font8)))
                PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
                PdfPCell.Padding = 4
                PdfTable.AddCell(PdfPCell)

                PdfPCell = New PdfPCell(New Phrase(New Chunk(Convert.ToDouble(txt_YTMAnn.Text).ToString("#,##0.0000") + " %", fontBlue)))
                PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                PdfPCell.Padding = 4
                PdfTable.AddCell(PdfPCell)
            End If

            If Val(txt_YTCAnn.Text) <> 0 And Hid_NatureOfInstrument.Value = "P" Then
                PdfPCell = New PdfPCell(New Phrase(New Chunk("YTC Ann", font8)))
                PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
                PdfPCell.Padding = 4
                PdfTable.AddCell(PdfPCell)

                PdfPCell = New PdfPCell(New Phrase(New Chunk(Convert.ToDouble(txt_YTCAnn.Text).ToString("#,##0.0000") + " %", fontBlue)))
                PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                PdfPCell.Padding = 4
                PdfTable.AddCell(PdfPCell)
            End If

            If Val(txt_YTPAnn.Text) <> 0 Then
                PdfPCell = New PdfPCell(New Phrase(New Chunk("YTP Ann", font8)))
                PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
                PdfPCell.Padding = 4
                PdfTable.AddCell(PdfPCell)

                PdfPCell = New PdfPCell(New Phrase(New Chunk(Convert.ToDouble(txt_YTPAnn.Text).ToString("#,##0.0000") + " %", fontBlue)))
                PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                PdfPCell.Padding = 4
                PdfTable.AddCell(PdfPCell)
            End If

            PdfPCell = New PdfPCell(New Phrase(New Chunk("Last IP Date", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk(txt_lastipdate.Text, font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("No of Days", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfTable.AddCell(PdfPCell)

            If (dblCashFlowAccInterest < 0) Then
                PdfPCell = New PdfPCell(New Phrase(New Chunk("-" & txt_nofodays.Text, font8)))
                PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                PdfPCell.Padding = 4
                PdfTable.AddCell(PdfPCell)
            Else
                PdfPCell = New PdfPCell(New Phrase(New Chunk(txt_nofodays.Text, font8)))
                PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                PdfPCell.Padding = 4
                PdfTable.AddCell(PdfPCell)
            End If
            PdfPCell = New PdfPCell(New Phrase(New Chunk("Principal Amount", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk(Convert.ToDouble(dblCashFlowAmt).ToString("#,##0.00"), font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            If (dblCashFlowAccInterest < 0) Then
                PdfPCell = New PdfPCell(New Phrase(New Chunk("Ex Interest", font8)))
                PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
                PdfPCell.Padding = 4
                PdfTable.AddCell(PdfPCell)
            Else
                PdfPCell = New PdfPCell(New Phrase(New Chunk("Accrued Interest", font8)))
                PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
                PdfPCell.Padding = 4
                PdfTable.AddCell(PdfPCell)
            End If

            PdfPCell = New PdfPCell(New Phrase(New Chunk(Convert.ToDouble(dblCashFlowAccInterest).ToString("#,##0.00"), font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("Total Consideration", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk(Convert.ToDouble(dblCashFlowTotalConsideration).ToString("#,##0.00"), fontBlue)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)



            PdfPCell = New PdfPCell(New Phrase(New Chunk("Stamp Duty", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk(Convert.ToDouble(dblStampDutyAmt).ToString("#,##0.00"), font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("Settlement Amount", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk(Convert.ToDouble(dblCashFlowSettAmt).ToString("#,##0.00"), fontBlue)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("Amount In Words", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk(Convert.ToString(strAmountInwords), fontBlue)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)
            pdfDoc.Add(PdfTable)

            PdfTable = New PdfPTable(2)
            PdfTable.WidthPercentage = 60 ' Table size is set to 100% of the page
            PdfTable.HorizontalAlignment = 1 'Left aLign
            PdfTable.SpacingAfter = 10
            pdfDoc.SetMargins(20.0F, 20.0F, 20.0F, 20.0F)


            Dim sglTblHdWidths2(1) As Single
            sglTblHdWidths2(0) = 70
            sglTblHdWidths2(1) = 70
            PdfTable.SetWidths(sglTblHdWidths2)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("Cash Flow", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfPCell.Border = 0
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfPCell.Border = 0
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfPCell.Border = 0
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfPCell.Border = 0
            PdfTable.AddCell(PdfPCell)


            PdfPCell = New PdfPCell(New Phrase(New Chunk("Date", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            PdfPCell = New PdfPCell(New Phrase(New Chunk("Amount", font8)))
            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            PdfPCell.Padding = 4
            PdfTable.AddCell(PdfPCell)

            If dt Is Nothing = False Then
                For rows As Integer = 0 To dt.Rows.Count - 1
                    For column As Integer = 0 To dt.Columns.Count - 1
                        If column = 1 Then
                            PdfPCell = New PdfPCell(New Phrase(New Chunk(Convert.ToDouble(dt.Rows(rows)(column)).ToString("#,##0.00"), font8)))
                            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                        Else
                            PdfPCell = New PdfPCell(New Phrase(New Chunk(dt.Rows(rows)(column).ToString(), font8)))
                        End If

                        PdfPCell.Padding = 4
                        PdfTable.AddCell(PdfPCell)

                    Next
                Next
                pdfDoc.Add(PdfTable)
            End If

            pdfDoc.Close()
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment; filename=CashFlow_" & Now.Date.Day.ToString() & Now.Date.Month.ToString() & Now.Date.Year.ToString() & Now.Date.Hour.ToString() & Now.Date.Minute.ToString() & Now.Date.Second.ToString() & Now.Date.Millisecond.ToString() & ".pdf")
            System.Web.HttpContext.Current.Response.Write(pdfDoc)
            Response.Flush()
            Response.[End]()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Public Sub SetBorder(ByVal cell As iTextSharp.text.Cell)
        cell.Border = Rectangle.BOX
    End Sub

    Public Sub SetBorder_(ByVal cell As iTextSharp.text.Cell)
        cell.BorderColorLeft = Color.BLACK
        cell.BorderColorRight = Color.BLACK
        cell.BorderColorTop = Color.BLACK
        cell.BorderColorBottom = Color.BLACK
        cell.BorderWidthLeft = 1

        '  cell.BorderWidthRight = 1
        cell.BorderWidthTop = 1
        ' cell.BorderWidthBottom = 1
    End Sub
    Private Sub FillAccruedDetails_CashFlow()
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            Dim sqlda As New SqlDataAdapter
            Dim dt As New DataTable
            Dim sqldv As New DataView
            Dim RateActual As String = "R"


            sqlComm.Connection = sqlConn
            sqlComm.CommandText = "ID_Fill_QuoteEntry_AccruedDetails"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Parameters.Clear()

            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 4, "I", , , Val(srh_NameofSecurity.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@SettlementDate", SqlDbType.Date, 4, "I", , , objCommon.DateFormat(txt_valuedate.Text))
            'objCommon.SetCommandParameters(sqlComm, "@TotalFaceValue", SqlDbType.Decimal, 20, "I", , , Val(HDAcc_FaceValue.Value) * Val(HDAcc_Multiple.Value))
            objCommon.SetCommandParameters(sqlComm, "@TotalFaceValue", SqlDbType.Decimal, 20, "I", , , Val(txt_quantum.Text) * Val(cbo_Amount.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@Rate", SqlDbType.Decimal, 20, "I", , , Val(txt_Rate.Text))

            objCommon.SetCommandParameters(sqlComm, "@StepUp", SqlDbType.Char, 1, "I", , , "N")
            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(dt)

            If dt.Rows.Count > 0 Then
                '    Hid_IntDays.Value = Val(dt.Rows(0).Item("AccruedDays") & "")
                If RateActual = "R" Then
                    'txt_Amount.Text = RoundToTwo(Val(HDAcc_FaceValue.Value) * Val(HDAcc_Multiple.Value) * RoundToFour(Val(HDAcc_Rate.Value)) / 100)
                    dblCashFlowAmt = objCommon.DecimalFormat2(RoundToTwo(Val(txt_quantum.Text) * Val(cbo_Amount.SelectedValue) * RoundToFour(Val(txt_Rate.Text)) / 100))
                    'txt_AddInterest.Text = RoundToTwo(Val(dt.Rows(0).Item("AccruedInterest") & ""))
                    dblCashFlowAccInterest = RoundToTwo(Val(dt.Rows(0).Item("AccruedInterest") & ""))
                    dblStampDutyAmt = objCommon.DecimalFormat2(RoundToTwo(Val(dt.Rows(0).Item("StampDutyAmt") & "")))
                Else
                    'txt_Amount.Text = RoundToTwo((txt_Amount.Text + txt_AddInterest.Text) / Val(Hid_FaceValue.Value) * Val(HDAcc_FaceValue.Value) * Val(HDAcc_Multiple.Value))
                    dblCashFlowAmt = objCommon.DecimalFormat2(RoundToTwo(Val(txt_quantum.Text) * Val(cbo_Amount.SelectedValue)))
                    'txt_AddInterest.Text = RoundToTwo(Val(dt.Rows(0).Item("AccruedInterest") & ""))
                    dblCashFlowAccInterest = RoundToTwo(Val(dt.Rows(0).Item("AccruedInterest") & ""))
                    dblStampDutyAmt = objCommon.DecimalFormat2(RoundToTwo(Val(dt.Rows(0).Item("StampDutyAmt") & "")))
                End If
                dblNoOfBond = Val(dt.Rows(0).Item("NoOfBond") & "")
                dblCashFlowTotalConsideration = RoundToTwo(Val(dt.Rows(0).Item("TotalConsideration") & ""))
                dblCashFlowSettAmt = RoundToTwo(Val(dt.Rows(0).Item("SettlementAmt") & "")) ''(txt_quantum.Text) + RoundToTwo(Val(dt.Rows(0).Item("AccruedInterest") & "")))
                strAmountInwords = Trim(dt.Rows(0).Item("AmountInWords") & "")
                If RateActual = "R" Then
                    txt_accrued.Text = RoundToTwo(Val(dt.Rows(0).Item("AccruedInterest") & ""))
                Else
                    txt_accrued.Text = RoundToTwo(Val(dt.Rows(0).Item("AccruedInterest") & ""))
                End If
                txt_lastipdate.Text = Trim((dt.Rows(0).Item("LastIPDate") & ""))
                txt_nofodays.Text = Val(dt.Rows(0).Item("AccruedDays") & "")
                dblNoOfBond = Val(dt.Rows(0).Item("NoOfBond") & "")
                strShutperiod = Trim((dt.Rows(0).Item("Shutperiod") & ""))
                txt_SettlementAmt.Text = RoundToTwo(Val(dt.Rows(0).Item("TotalConsideration") & ""))
                txt_StampDuty.Text = RoundToTwo(Val(dt.Rows(0).Item("StampDutyAmt") & ""))
                txt_TC.Text = RoundToTwo(Val(dt.Rows(0).Item("SettlementAmt") & ""))
                txt_PrinciAmt.Text = objCommon.DecimalFormat2(RoundToTwo(Val(dblCashFlowAmt)))
            End If


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub txt_valuedate_TextChanged(sender As Object, e As EventArgs) Handles txt_valuedate.TextChanged
        FillSecurityDetails()
    End Sub

    <WebMethod(EnableSession:=True)>
    Public Shared Function GetCurrentFaceValue(ByVal securityId As String, ByVal valuedate As String) As String
        Dim strData As String = ""
        Dim dsData As New DataSet

        Dim lstParam As New List(Of SqlParameter)()
        Try
            lstParam.Add(New SqlParameter("@SecurityId", securityId))
            lstParam.Add(New SqlParameter("@AsOn", valuedate))
            dsData = objComm.FillDetails(lstParam, "ID_GET_CurrentFaceValue")
            If dsData Is Nothing = False And dsData.Tables.Count > 0 Then
                strData = Convert.ToString(dsData.Tables(0).Rows(0)("Data").ToString())

            End If
        Catch ex As Exception

        End Try
        Return strData
    End Function
End Class
