Imports System.Data
Imports System.Reflection
Imports System.Collections.Generic
Imports System.ComponentModel
Imports GlobalFuns
Imports System.Data.SqlClient
Partial Class UserControls_YieldCalculater
    Inherits System.Web.UI.UserControl
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
    ' Dim objCommon As New clsCommonFuns
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

    Public Property SecurityId() As Integer
        Get
            Return ViewState("SecurityId")
        End Get
        Set(ByVal value As Integer)
            ViewState("SecurityId") = value
        End Set
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property cboFaceValue() As DropDownList
        Get
            Return Cbo_FaceValue
        End Get
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property TextBoxRate() As TextBox
        Get
            Return txt_Rate
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property TextBoxFaceValue() As TextBox
        Get
            Return txt_FaceValue
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property TextBoxYTMSemi() As TextBox
        Get
            Return txt_YTMSemi
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property TextBoxYTCSemi() As TextBox
        Get
            Return txt_YTCSemi
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property TextBoxYTPSemi() As TextBox
        Get
            Return txt_YTPSemi
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property TextBoxYTMAnn() As TextBox
        Get
            Return txt_YTMAnn
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property TextBoxYTCAnn() As TextBox
        Get
            Return txt_YTCAnn
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property TextBoxYTPAnn() As TextBox
        Get
            Return txt_YTPAnn
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property TextBoxYTMDate() As TextBox
        Get
            Return txt_YTMDate
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property RdoIPCalc() As RadioButtonList
        Get
            Return rdo_IPCalc
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property RdoRateActual() As RadioButtonList
        Get
            Return rdo_RateActual
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property RdoPhysicalDMAT() As RadioButtonList
        Get
            Return rdo_PhysicalDMAT
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property RdoSemiAnn() As RadioButtonList
        Get
            Return rdo_SemiAnn
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property RdoYXM() As RadioButtonList
        Get
            Return rdo_YXM
        End Get
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property RdoDays() As RadioButtonList
        Get
            Return rbl_Days
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property RdoDaysOptions() As RadioButtonList
        Get
            Return rbl_DaysOptions
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property RdoYield() As RadioButton
        Get
            Return rdo_Yield
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property RdoMatToRate() As RadioButton
        Get
            Return rdo_MatToRate
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property RdoMatCallToRate() As RadioButton
        Get
            Return rdo_CallToRate
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property RdoPutToRate() As RadioButton
        Get
            Return rdo_PutToRate
        End Get
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property chkCombineIPMat() As CheckBox
        Get
            Return chk_CombineIPMat
        End Get
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property ShowCloseButton() As Boolean
        Get
            Return btn_Ret.Visible
        End Get
        Set(ByVal value As Boolean)
            btn_Ret.Visible = value
        End Set
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property TextBoxSettDate() As TextBox
        Get
            Return txt_SettDate
        End Get
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property cboSettDay() As DropDownList
        Get
            Return cbo_SettDay
        End Get
    End Property


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

                        If CheckSecurity() = False Or Val(Hid_Frequency.Value) = 0 Then
                            rdo_YXM.Items(0).Selected = False
                            'rdo_YXM.Items(0).Enabled = False
                            rdo_YXM.Items(1).Selected = True
                        End If
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
                If Trim(Hid_MatDate.Value) = "" Then

                    'rdo_YXM.Items(2).Enabled = False
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
    Private Function CheckSecurity() As Boolean
        Try
            Dim I As Int16
            Dim strTypes() As String
            'Dim dsInfo As DataSet
            Dim dt As DataTable
            strTypes = Split("M!I!C!P", "!")
            For I = 0 To strTypes.Length - 1
                Hid_TypeFlag.Value = strTypes(I)
                'dsInfo = objCommon.GetDataSet(SqlDataSourceSecurityInfo)
                OpenConn()
                dt = objCommon.FillDataTable(sqlConn, "ID_Check_SecurityInfo", SecurityId, "SecurityId", , Hid_TypeFlag.Value, "TypeFlag")
                'dsInfo = objCommon.FillDetailsGrid(dg_Maturity, "ID_Check_SecurityInfo", "SecurityId", Val(ViewState("Id") & ""), "M", "TypeFlag")

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

    Protected Sub btn_CalYield_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CalYield.Click
        Try
            If Request.QueryString("Page") = "QuoteDetails.aspx" Then
                Dim RBL As RadioButtonList = CType(Me.Parent.FindControl("rdo_TaxFree"), RadioButtonList)
                Rbl_StepUp_down = RBL.SelectedValue
            End If

            If SecurityId = 0 Then Exit Sub
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
            'lblCalcDate.Text = "Calc Date: " & datYTM
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
                datYTM = .DateFormat(txt_SettDate.Text)
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
            'decCouponRate = GetCouponRate(decCouponRate)
            'CalculateYield(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt, _
            '               datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""))
            If rdo_Yield.Checked = True Then
                GlobalFuns.CalculateYield(txt_SettDate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
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
                GlobalFuns.CalculateYield(txt_SettDate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                                datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "M", dblResult, strSemiAnnFlag)
                txt_Rate.Text = objCommon.DecimalFormat4(decMarketRate)
                GlobalFuns.CalculateYield(txt_SettDate.Text, decFaceValue, decMarketRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
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
                GlobalFuns.CalculateYield(txt_SettDate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                                datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "C", dblResult, strSemiAnnFlag)
                txt_Rate.Text = objCommon.DecimalFormat4(decMarketRate)
                dblYTCAnn = objCommon.DecimalFormat4(txt_YTCAnn.Text)
                GlobalFuns.CalculateYield(txt_SettDate.Text, decFaceValue, decMarketRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
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
                GlobalFuns.CalculateYield(txt_SettDate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                                datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "P", dblResult, strSemiAnnFlag)
                txt_Rate.Text = objCommon.DecimalFormat4(decMarketRate)
                GlobalFuns.CalculateYield(txt_SettDate.Text, decFaceValue, decMarketRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt,
                               datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "Y", 0, "")
            End If

            With objCommon
                If rdo_MatToRate.Checked = True And RdoSemiAnn.SelectedValue = "A" Then
                Else
                    txt_YTMAnn.Text = .DecimalFormat4(dblYTMAnn)
                End If
                If rdo_CallToRate.Checked = True And RdoSemiAnn.SelectedValue = "A" Then
                Else
                    txt_YTCAnn.Text = .DecimalFormat4(dblYTCAnn)
                End If
                If rdo_PutToRate.Checked = True And RdoSemiAnn.SelectedValue = "A" Then
                Else
                    txt_YTPAnn.Text = .DecimalFormat4(dblYTPAnn)
                End If
                If Val(Hid_Frequency.Value & "") > 1 Then
                    If rdo_MatToRate.Checked = True And RdoSemiAnn.SelectedValue = "S" Then
                    Else
                        txt_YTMSemi.Text = .DecimalFormat4(dblYTMSemi)
                    End If
                    If rdo_CallToRate.Checked = True And RdoSemiAnn.SelectedValue = "S" Then
                    Else
                        txt_YTCSemi.Text = .DecimalFormat4(dblYTCSemi)
                    End If
                    If rdo_PutToRate.Checked = True And RdoSemiAnn.SelectedValue = "S" Then
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
                    CalculateXIRRYield(SecurityId, txt_SettDate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, RdoRateActual.SelectedValue, Trim(Request.QueryString("StepUp") & ""))
                Else
                    CalculateXIRRYield(SecurityId, txt_SettDate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, RdoRateActual.SelectedValue, Rbl_StepUp_down)
                End If

                'With objCommon
                '    txt_YTMAnn.Text = Math.Round(dblYTMAnn, 4)
                '    txt_YTCAnn.Text = Math.Round(dblYTCAnn, 4)
                '    txt_YTPAnn.Text = Math.Round(dblYTPAnn, 4)
                '    If Val(Hid_Frequency.Value & "") > 1 Then
                '        txt_YTMSemi.Text = Math.Round(dblYTMSemi, 4)
                '        txt_YTCSemi.Text = Math.Round(dblYTCSemi, 4)
                '        txt_YTPSemi.Text = Math.Round(dblYTPSemi, 4)
                '    End If
                'End With
            ElseIf rdo_MatToRate.Checked = True Then
                dblYTMAnn = Val(txt_YTMAnn.Text)
                dblYTMSemi = Val(txt_YTMSemi.Text)
                If Rbl_StepUp_down = "" Then
                    CalculateXIRRPrice(SecurityId, txt_SettDate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, Trim(Request.QueryString("StepUp") & ""), "M")
                Else
                    CalculateXIRRPrice(SecurityId, txt_SettDate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, Rbl_StepUp_down, "M")
                End If
                txt_Rate.Text = Math.Round(dblPTM, 4)
                CalculateXIRRYield(SecurityId, txt_SettDate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, RdoRateActual.SelectedValue, Rbl_StepUp_down)
            ElseIf rdo_CallToRate.Checked = True Then
                dblYTCAnn = Val(txt_YTCAnn.Text)
                dblYTCSemi = Val(txt_YTCSemi.Text)
                If Rbl_StepUp_down = "" Then
                    CalculateXIRRPrice(SecurityId, txt_SettDate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, Trim(Request.QueryString("StepUp") & ""), "C")
                Else
                    CalculateXIRRPrice(SecurityId, txt_SettDate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, Rbl_StepUp_down, "C")
                End If
                txt_Rate.Text = Math.Round(dblPTC, 4)
                CalculateXIRRYield(SecurityId, txt_SettDate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, RdoRateActual.SelectedValue, Rbl_StepUp_down)
            ElseIf rdo_PutToRate.Checked = True Then
                dblYTPAnn = Val(txt_YTPAnn.Text)
                dblYTPSemi = Val(txt_YTPSemi.Text)
                If Rbl_StepUp_down = "" Then
                    CalculateXIRRPrice(SecurityId, txt_SettDate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, Trim(Request.QueryString("StepUp") & ""), "P")
                Else
                    CalculateXIRRPrice(SecurityId, txt_SettDate.Text, Val(Hid_Frequency.Value), chk_CashFlow.Checked, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, Rbl_StepUp_down, "P")
                End If
                txt_Rate.Text = Math.Round(dblPTP, 4)
                CalculateXIRRYield(SecurityId, txt_SettDate.Text, Val(txt_Rate.Text), Val(Hid_Frequency.Value), chk_CashFlow.Checked, RdoRateActual.SelectedValue, Rbl_StepUp_down)
            End If
            'With objCommon
            '    txt_YTMAnn.Text = Math.Round(dblYTMAnn, 4)
            '    txt_YTCAnn.Text = Math.Round(dblYTCAnn, 4)
            '    txt_YTPAnn.Text = Math.Round(dblYTPAnn, 4)
            '    If Val(Hid_Frequency.Value & "") > 1 Then
            '        txt_YTMSemi.Text = Math.Round(dblYTMSemi, 4)
            '        txt_YTCSemi.Text = Math.Round(dblYTCSemi, 4)
            '        txt_YTPSemi.Text = Math.Round(dblYTPSemi, 4)
            '    End If
            'End With

            With objCommon
                If rdo_MatToRate.Checked = True And RdoSemiAnn.SelectedValue = "A" Then
                Else
                    txt_YTMAnn.Text = .DecimalFormat4(dblYTMAnn)
                End If
                If rdo_CallToRate.Checked = True And RdoSemiAnn.SelectedValue = "A" Then
                Else
                    txt_YTCAnn.Text = .DecimalFormat4(dblYTCAnn)
                End If
                If rdo_PutToRate.Checked = True And RdoSemiAnn.SelectedValue = "A" Then
                Else
                    txt_YTPAnn.Text = .DecimalFormat4(dblYTPAnn)
                End If
                If Val(Hid_Frequency.Value & "") > 1 Then
                    If rdo_MatToRate.Checked = True And RdoSemiAnn.SelectedValue = "S" Then
                    Else
                        txt_YTMSemi.Text = .DecimalFormat4(dblYTMSemi)
                    End If
                    If rdo_CallToRate.Checked = True And RdoSemiAnn.SelectedValue = "S" Then
                    Else
                        txt_YTCSemi.Text = .DecimalFormat4(dblYTCSemi)
                    End If
                    If rdo_PutToRate.Checked = True And RdoSemiAnn.SelectedValue = "S" Then
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
                    CalculateMMYYield(SecurityId, txt_SettDate.Text, Val(txt_Rate.Text), rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, (Request.QueryString("StepUp") & ""))
                Else
                    CalculateMMYYield(SecurityId, txt_SettDate.Text, Val(txt_Rate.Text), rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, Rbl_StepUp_down)
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
                    CalculateMMYPrice(SecurityId, txt_SettDate.Text, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "M", (Request.QueryString("StepUp") & ""))
                Else
                    CalculateMMYPrice(SecurityId, txt_SettDate.Text, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "M", Rbl_StepUp_down)
                End If

                txt_Rate.Text = Math.Round(dblPTM, 4)
            ElseIf rdo_CallToRate.Checked = True Then
                dblYTCAnn = Val(txt_YTCAnn.Text)
                dblYTCSemi = Val(txt_YTCSemi.Text)
                If Rbl_StepUp_down = "" Then
                    CalculateMMYPrice(SecurityId, txt_SettDate.Text, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "C", (Request.QueryString("StepUp") & ""))
                Else
                    CalculateMMYPrice(SecurityId, txt_SettDate.Text, rdo_SemiAnn.SelectedValue, rdo_RateActual.SelectedValue, "C", Rbl_StepUp_down)
                End If

                txt_Rate.Text = Math.Round(dblPTC, 4)
            End If
        Catch ex As Exception

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(4), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillMMYOptions()
        Try
            Dim dblResult As Double
            If rdo_Yield.Checked = True Then
                CalculateMMY(txt_SettDate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, MatDate, MatAmt,
                                                CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Val(Hid_Frequency.Value), Val(Hid_MMYRate.Value) / 100, rbl_Days.SelectedValue, rbl_DaysOptions.SelectedValue)
                txt_YTMAnn.Text = objCommon.DecimalFormat4(dblYTMAnn)
                If chk_CashFlow.Checked = True Then ShowCashFlow()
            Else
                dblYTMAnn = Val(txt_YTMAnn.Text & "")
                dblResult = CalculateMMYMarketRate(txt_SettDate.Text, decFaceValue, decRate, blnNonGovernment, blnRateActual, MatDate, MatAmt,
                                    CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Val(Hid_Frequency.Value), Val(Hid_MMYRate.Value) / 100, rbl_Days.SelectedValue, rbl_DaysOptions.SelectedValue)
                txt_Rate.Text = objCommon.DecimalFormat4(dblResult)
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub ShowCashFlow()
        Try
            Dim strHtml As String
            Dim strAmt As String
            Dim strDate As String
            Dim I As Int32
            For I = 0 To XirrAmt.Length - 1
                strAmt += XirrAmt(I) & "!"
            Next
            For I = 0 To XirrDate.Length - 1
                strDate += XirrDate(I) & "!"
            Next
            strHtml += "ShowCashFlow('CashFlow.aspx','" & strAmt & "','" & strDate & "',270,600)"
            If Trim(Request.QueryString("page") & "") = "QuoteDetails.aspx" Then
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "cash", strHtml, True)
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType, "cash", strHtml, True)

            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'txt_YTMDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
            Hid_StepUp.Value = (Request.QueryString("StepUp") & "")
            If Trim(Request.QueryString("page") & "") = "DealSlipEntry.aspx" Then
                txt_YTMDate.Text = Trim(Request.QueryString("DealDate") & "")
                cbo_SettDay.SelectedValue = Trim(Request.QueryString("SettDay") & "")
                txt_SettDate.Text = Trim(Request.QueryString("SettDate") & "")
                txt_YTMDate.Enabled = False
                cbo_SettDay.Enabled = False
                txt_SettDate.Enabled = False
                'img_Calendar.Visible = False
            ElseIf Trim(Request.QueryString("SettDate") & "") <> "" Then
                txt_YTMDate.Text = Trim(Request.QueryString("SettDate") & "")
                cbo_SettDay.SelectedValue = "0"
                txt_SettDate.Text = Trim(Request.QueryString("SettDate") & "")
            Else
                txt_YTMDate.Text = Today.ToString("dd/MM/yyyy")
                cbo_SettDay.SelectedValue = "1"
                FillSettDate()
            End If

            'Dim RBL As RadioButtonList = CType(Me.Parent.FindControl("rdo_TaxFree"), RadioButtonList)
            'Rbl_StepUp_down = RBL.SelectedValue
            'Hid_StepUp.Value = Rbl_StepUp_down
            'If Trim(Request.QueryString("Page") & "") <> "DealSlipEntry.aspx" Then
            '    FillSettDate()
            'End If
        End If
        SetAttributes()
        FillOptions()
        Session("uc_parentid") = Me.ClientID

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CheckCashFlow", "CheckCashFlow('" & Me.ClientID & "');", True)
    End Sub

    Private Sub FillSettDate()
        Try
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "FillSettlementDate();", True)
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Sub FillSettDate_old()
        Try
            Dim intLoop As Int16 = 0
            Dim count As Integer = 0
            Dim incDate As Date

            incDate = objCommon.DateFormat(txt_YTMDate.Text)
            While count < cbo_SettDay.SelectedValue
                incDate = DateAdd(DateInterval.Day, 1, incDate)
                If checkdate(incDate) = True Then
                    count = count - 1
                End If
                count = count + 1
            End While
            txt_SettDate.Text = incDate.ToString("dd/MM/yyyy") ' String.Format(incDate, "dd/MM/yyyy")
            datYTM = String.Format(incDate, "dd/MM/yyyy")
        Catch ex As Exception

            Response.Write(ex.Message)
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

            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try
    End Function
    Private Sub SetAttributes()
        rdo_YXM.Items(0).Attributes.Add("onclick", "CheckCashFlow('" & Me.ClientID & "',true)")
        rdo_YXM.Items(1).Attributes.Add("onclick", "CheckCashFlow('" & Me.ClientID & "',true)")
        rdo_YXM.Items(2).Attributes.Add("onclick", "CheckCashFlow('" & Me.ClientID & "',true)")
        rdo_SemiAnn.Items(0).Attributes.Add("onclick", "ChangeSemiAnn('" & Me.ClientID & "',true);")
        rdo_SemiAnn.Items(1).Attributes.Add("onclick", "ChangeSemiAnn('" & Me.ClientID & "',true);")
        rdo_Yield.Attributes.Add("onclick", "MakeDisable('" & Me.ClientID & "');")
        rdo_MatToRate.Attributes.Add("onclick", "MakeEnable('" & Me.ClientID & "','M',true);")
        rdo_CallToRate.Attributes.Add("onclick", "MakeEnable('" & Me.ClientID & "','C',true);")
        rdo_PutToRate.Attributes.Add("onclick", "MakeEnable('" & Me.ClientID & "','P',true);")
        btn_CalYield.Attributes.Add("onclick", "return ValidateCalculation('" & Me.ClientID & "');")
        btn_CalInterest.Attributes.Add("onclick", "return ShowAccuredInterest('" & Me.ClientID & "')")
        btn_CalCurrRate.Attributes.Add("onclick", "return OpenCurrentRate('" & Me.ClientID & "')")
        txt_YTMSemi.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_YTMAnn.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_YTCSemi.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_YTCAnn.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_YTPSemi.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_YTPAnn.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_YTMDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_YTMDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_Rate.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_FaceValue.Attributes.Add("onkeypress", "OnlyDecimal();")
        btn_Ret.Attributes.Add("onclick", "Close('" & Me.ClientID & "');")
        'img_Calendar.Attributes.Add("onclick", "displayDatePicker('" & Me.ClientID & "_txt_YTMDate',this);")
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

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Session("OfferRate") = ""
    End Sub

    Protected Sub cbo_SettDay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SettDay.SelectedIndexChanged
        Try
            FillSettDate()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub btn_CalInterest_Click(sender As Object, e As EventArgs) Handles btn_CalInterest.Click

    End Sub
End Class
