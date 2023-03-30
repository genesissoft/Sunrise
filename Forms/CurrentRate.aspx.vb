Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_CurrentRate
    Inherits System.Web.UI.Page
    Dim dblPepCoupRate As Double
    Dim datLstIPDate As Date
    Dim MatDate() As Date
    Dim MatAmt() As Double
    Dim CoupDate() As Date
    Dim CoupRate() As Double
    Dim decFaceValue As Decimal
    Dim sqlConn As SqlConnection
    Dim objCommon As New clsCommonFuns

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            OpenConn()
            If IsPostBack = False Then
                'If Val(Request.QueryString("FaceMultiple") & "") <> 0 Then
                '    cbo_FaceValue.SelectedValue = Val(Request.QueryString("FaceMultiple") & "")
                'End If
                SetAttributes()
                txt_Rate.Text = Val(Request.QueryString("Rate") & "")
                If Trim(Request.QueryString("PurDate") & "") = "" Then
                    txt_PurDate.Text = Format(Today, "dd/MM/yyyy")
                Else
                    txt_PurDate.Text = Trim(Request.QueryString("PurDate") & "")
                End If
                txt_FaceValue.Text = Val(Request.QueryString("FaceValue") & "")
                cbo_FaceValue.SelectedValue = Val(Request.QueryString("Multiple") & "")
                If Val(Request.QueryString("Id") & "") <> 0 Then
                    Hid_SecurityId.Value = Val(Request.QueryString("Id") & "")
                    FillOptions()
                End If
                Session("ParentId") = Trim(Request.QueryString("ParentId"))
                Page.SetFocus(txt_Rate.ClientID)
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
        txt_PurDate.Attributes.Add("onblur", "CheckDate(this);")
        txt_PurDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_CalcDate.Attributes.Add("onblur", "CheckDate(this);")
        txt_CalcDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_FaceValue.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_Rate.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_HoldingCost.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_CalcDate.Text = Format(Today, "dd/MM/yyyy")

        btn_CalcRate.Attributes.Add("onclick", "return Validate()")
    End Sub
    Private Sub FillOptions()
        Try

            Dim strSecurityNature As String = ""
            Dim dtSecurity As DataTable
            Dim I As Int32
            Dim datInfo As Date
            'dsSecurity = objCommon.GetDataSet(SqlDataSourceSecurity)
            dtSecurity = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecurityId.Value, "SecurityId")
            For I = 0 To dtSecurity.Rows.Count - 1
                With dtSecurity.Rows(I)
                    chrMaxActFlag = Trim(.Item("MaxActualFlag") & "")
                    strSecurityNature = Trim(.Item("NatureOfInstrument") & "")
                    txt_Issuer.Text = Trim(.Item("SecurityIssuer") & "")
                    txt_Security.Text = Trim(.Item("SecurityName") & "")
                    Hid_BookClosureDate.Value = IIf(Trim(.Item("BookClosureDate") & "") = "", Date.MinValue, .Item("BookClosureDate"))
                    Hid_InterestDate.Value = IIf(Trim(.Item("FirstInterestDate") & "") = "", Date.MinValue, .Item("FirstInterestDate"))
                    Hid_DMATBkDate.Value = IIf(Trim(.Item("DMATBookClosureDate") & "") = "", Date.MinValue, .Item("DMATBookClosureDate"))
                    Hid_Issue.Value = IIf(Trim(.Item("IssueDate") & "") = "", Date.MinValue, .Item("IssueDate"))
                    datInfo = IIf(Trim(.Item("SecurityInfoDate") & "") = "", Date.MinValue, .Item("SecurityInfoDate"))
                    Hid_GovernmentFlag.Value = Trim(.Item("GovernmentFlag") & "")
                    Hid_Frequency.Value = GetFrequency(Trim(.Item("FrequencyOfInterest") & ""))
                    Hid_FaceValue.Value = Val(.Item("FaceValue") & "")
                    txt_FaceValue.Text = Val(Request.QueryString("FaceValue") & "")
                    cbo_FaceValue.SelectedValue = Val(Request.QueryString("Multiple") & "")
                    Hid_RateAmtFlag.Value = Trim(.Item("CouponOn") & "")
                    FillSecurityInfoDetails(datInfo, Val(.Item("SecurityInfoAmt") & ""), Trim(.Item("TypeFlag") & ""))
                End With
            Next
            If strSecurityNature = "P" Then
                Hid_CoupDate.Value += CStr(#12/31/9999#) & "!"
                Hid_CoupRate.Value += dblPepCoupRate & "!"
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

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
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_CalcRate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CalcRate.Click
        Try
            Dim intDays As Integer
            Dim dblSettAmt As Double
            Dim dblRevSettAmt As Double
            Dim intAccIntDays As Double
            Dim intLstIPDiff As Integer
            Dim datCalc As Date
            Dim dblAccInt As Double
            Dim dblCoupRate As Double
            Dim dblGivenFaceValue As Double
            Dim dblCurrRate As Double

            datCalc = objCommon.DateFormat(txt_CalcDate.Text)
            intDays = GetDateDiff()
            dblSettAmt = GetSettlementAmt()
            If Val(Hid_Frequency.Value) <> 0 Then
                dblCoupRate = objCommon.DecimalFormat(GetCouponRate())
            End If
            dblGivenFaceValue = Val(txt_FaceValue.Text) * Val(cbo_FaceValue.Text)

            intAccIntDays = IIf(Hid_GovernmentFlag.Value = "N", 365, 360)
            dblRevSettAmt = dblSettAmt * (Val(txt_HoldingCost.Text) / 100) / intAccIntDays * intDays + dblSettAmt
            intLstIPDiff = DateDiff(DateInterval.Day, datLstIPDate, datCalc)
            If Val(Hid_Frequency.Value) <> 0 Then
                dblAccInt = (dblGivenFaceValue * dblCoupRate / (100 * intAccIntDays)) * intLstIPDiff
            Else
                dblAccInt = 0
            End If
            dblCurrRate = (dblRevSettAmt - dblAccInt) / dblGivenFaceValue * 100
            txt_Currentrate.Text = objCommon.DecimalFormat(dblCurrRate)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function GetDateDiff() As Integer
        Try
            Dim datPurc As Date
            Dim datCalc As Date
            Dim intDiff As Integer

            datPurc = objCommon.DateFormat(txt_PurDate.Text)
            datCalc = objCommon.DateFormat(txt_CalcDate.Text)
            intDiff = DateDiff(DateInterval.Day, datPurc, datCalc)
            Return intDiff
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function GetSettlementAmt() As Double
        Try
            Dim datYTM As Date
            Dim datIssue As Date
            Dim datInterest As Date
            Dim datBookClosure As Date

            Dim decRate As Decimal
            Dim blnNonGovernment As Boolean
            Dim blnRateActual As Boolean
            Dim blnDMAT As Boolean
            Dim intBKDiff As Integer
            Dim FinalAmount As Double
            Dim IntDate As Date
            Dim AddLess As String
            Dim AddLessNoofDays As Integer
            Dim Ratio As Double
            Dim IntAmount As Double

            Dim intDays As Int16
            Dim dblSettAmt As Double
            Dim dblTotAmt As Double
            Dim dblActualInt As Double

            If Val(Hid_Frequency.Value) = 0 Then
                dblSettAmt = RoundToTwo(Val(txt_FaceValue.Text) * Val(cbo_FaceValue.SelectedValue) * txt_Rate.Text / 100)
            Else
                MatDate = FillDateArrays(Hid_MatDate.Value)
                MatAmt = FillAmtArrays(Hid_MatAmt.Value)
                CoupDate = FillDateArrays(Hid_CoupDate.Value)
                CoupRate = FillAmtArrays(Hid_CoupRate.Value)
                With objCommon
                    datYTM = .DateFormat(txt_PurDate.Text)
                    datInterest = Hid_InterestDate.Value
                    datBookClosure = Hid_BookClosureDate.Value
                    blnNonGovernment = IIf(Hid_GovernmentFlag.Value = "N", True, False)
                    blnRateActual = True
                    blnDMAT = IIf(rdo_PhysicalDMAT.SelectedValue = "D", True, False)
                    decFaceValue = .DecimalFormat(Hid_FaceValue.Value)
                    decRate = .DecimalFormat(txt_Rate.Text)
                    datIssue = Hid_Issue.Value
                    datBookClosure = IIf(blnDMAT = True, Hid_DMATBkDate.Value, Hid_BookClosureDate.Value)
                    intBKDiff = CalculateBookClosureDiff(datBookClosure, rdo_PhysicalDMAT.SelectedValue, datInterest, blnNonGovernment)
                    intDays = IIf(blnNonGovernment = True, 365, 360)
                End With
                CalculateAccuredInterest(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, _
                              MatDate, MatAmt, CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Hid_Frequency.Value, _
                              intDays, FinalAmount, IntDate, AddLess, AddLessNoofDays, Ratio, IntAmount, "Y")
                datLstIPDate = IntDate
                dblTotAmt = RoundToTwo(Val(txt_FaceValue.Text) * Val(cbo_FaceValue.SelectedValue) * txt_Rate.Text / 100)
                dblActualInt = RoundToTwo(IntAmount * Val(txt_FaceValue.Text) * Val(cbo_FaceValue.SelectedValue))
                If AddLess = "L" Then
                    dblSettAmt = dblTotAmt - dblActualInt
                Else
                    dblSettAmt = dblTotAmt + dblActualInt
                End If
            End If
            Return dblSettAmt
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function GetCouponRate() As Double
        Try
            Dim datCalc As Date
            Dim I As Int16
            datCalc = objCommon.DateFormat(txt_CalcDate.Text)
            If Hid_RateAmtFlag.Value = "A" Then
                For I = 0 To CoupRate.Length - 1
                    CoupRate(I) = (CoupRate(I) / decFaceValue) * 100
                Next
            End If
            For I = 0 To CoupRate.Length - 1
                If CoupDate(I) >= datCalc Then
                    Return CoupRate(I)
                End If
            Next
            Return 0
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

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
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function RoundToTwo(ByVal dec As Decimal) As Decimal
        Try
            Dim rounded As Decimal = Decimal.Round(dec, 2)
            rounded = Format(rounded, "###################0.00")
            Return rounded
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
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
End Class
