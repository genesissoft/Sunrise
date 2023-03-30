Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Partial Class Forms_AccuredInterest
    Inherits System.Web.UI.Page
    Dim MatDate() As Date
    Dim MatAmt() As Double
    Dim CoupDate() As Date
    Dim CoupRate() As Double
    Dim objCommon As New clsCommonFuns
    Dim MonthDays() As Int32 = {0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}
    Dim sqlConn As SqlConnection
    Dim Rbl_StepUp_down As String = ""
    Dim strPage As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OpenConn()
        Try
            Dim str As String = Convert.ToString(Session("URL"))
            If Trim(Session("UserTypeSection") & "") <> "F" Then
                objCommon.SetPageTitle(Page, Trim(Session("CompName") & ""), Trim(Session("YearText") & ""))
            End If
            If IsPostBack = False Then
                SetAttributes()
                FillOptions()
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

    Private Sub SetAttributes()

        txt_Date.Attributes.Add("onblur", "CheckDate(document.getElementById('txt_Date'));")
        txt_Date.Attributes.Add("onkeypress", "OnlyDate();")
        txt_Date.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_Rate.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_FaceValue.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_Amount.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_AddInterest.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_SettAmt.Attributes.Add("onkeypress", "OnlyDecimal();")
        btn_CalInterest.Attributes.Add("onclick", "return Validate();")
        'btn_Close.Attributes.Add("onclick", "Close();")
    End Sub

    Private Sub FillOptions()
        Try

            OpenConn()
            Dim dtSecurity As DataTable
            Dim datInfo As Date
            Dim I As Int32
            Hid_SecurityId.Value = Val(Request.QueryString("Id") & "")
            'dsSecurity = objCommon.GetDataSet(SqlDataSourceSecurity)
            dtSecurity = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Hid_SecurityId.Value, "SecurityId")
            For I = 0 To dtSecurity.Rows.Count - 1
                With dtSecurity.Rows(I)
                    chrMaxActFlag = Trim(.Item("MaxActualFlag") & "")
                    'txt_Date.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
                    txt_Date.Text = IIf(Trim(Request.QueryString("Date") & "") = "", Format(DateAndTime.Today, "dd/MM/yyyy"), Trim(Request.QueryString("Date") & ""))
                    txt_Rate.Text = RoundToFour(Val(Request.QueryString("Rate") & ""))
                    txt_IPDates.Text = Trim(.Item("IPDates") & "")
                    Hid_Frequency.Value = GetFrequency(Trim(.Item("FrequencyOfInterest") & ""))
                    datInfo = IIf(Trim(.Item("SecurityInfoDate") & "") = "", Date.MinValue, .Item("SecurityInfoDate"))
                    Hid_FaceValue.Value = Val(.Item("FaceValue") & "")
                    txt_FaceValue.Text = Val(Request.QueryString("FaceValue") & "")
                    cbo_FaceValue.SelectedValue = Val(Request.QueryString("Multiple") & "")
                    'txt_Issuer.Text = Trim(.Item("SecurityIssuer") & "")
                    'txt_Security.Text = Trim(.Item("SecurityName") & "")
                    Hid_BookClosureDate.Value = IIf(Trim(.Item("BookClosureDate") & "") = "", Date.MinValue, .Item("BookClosureDate"))
                    Hid_InterestDate.Value = IIf(Trim(.Item("FirstInterestDate") & "") = "", Date.MinValue, .Item("FirstInterestDate"))
                    Hid_DMATBkDate.Value = IIf(Trim(.Item("DMATBookClosureDate") & "") = "", Date.MinValue, .Item("DMATBookClosureDate"))
                    Hid_Issue.Value = IIf(Trim(.Item("IssueDate") & "") = "", Date.MinValue, .Item("IssueDate"))
                    Hid_GovernmentFlag.Value = Trim(.Item("GovernmentFlag") & "")
                    FillSecurityInfoDetails(datInfo, Val(.Item("SecurityInfoAmt") & ""), Trim(.Item("TypeFlag") & ""))
                    rdo_Days.SelectedValue = IIf(Trim(.Item("GovernmentFlag") & "") = "N", 365, 360)
                    Hid_NSDLFaceValue.Value = Val(.Item("NSDLFaceValue") & "")
                End With
            Next
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub FillSecurityInfoDetails(ByVal InfoDate As Date, ByVal InfoAmt As Decimal, ByVal TypeFlag As String)
        Try
            Select Case TypeFlag
                Case "M"
                    Hid_MatDate.Value += InfoDate & "!"
                    Hid_MatAmt.Value += InfoAmt & "!"
                Case "I"
                    Hid_CoupDate.Value += InfoDate & "!"
                    Hid_CoupRate.Value += InfoAmt & "!"
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

    Protected Sub btn_CalInterest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CalInterest.Click
        Try
            strPage = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            If Convert.ToString(Session("URL")) = "QuoteEntry.aspx" Then
                Rbl_StepUp_down = Convert.ToString(Session("StepUpDown"))
            Else
                Rbl_StepUp_down = Trim(Request.QueryString("StepUp") & "")
            End If

            FillAccruedDetails()
            'OpenConn()
            'Dim datYTM As Date
            'Dim datIssue As Date
            'Dim datInterest As Date
            'Dim datBookClosure As Date
            'Dim decFaceValue As Decimal
            'Dim decRate As Decimal
            'Dim blnNonGovernment As Boolean
            'Dim blnRateActual As Boolean
            'Dim blnDMAT As Boolean
            'Dim intBKDiff As Integer
            'Dim FinalAmount As Double
            'Dim IntDate As Date
            'Dim AddLess As String
            'Dim AddLessNoofDays As Integer
            'Dim Ratio As Double
            'Dim IntAmount As Double
            'Dim intDays As Int16

            'If Val(Hid_Frequency.Value) = 0 Then
            '    txt_AddInterest.Text = 0
            '    txt_Amount.Text = RoundToTwo(Val(txt_FaceValue.Text) * Val(cbo_FaceValue.SelectedValue) * txt_Rate.Text / 100)
            '    txt_SettAmt.Text = RoundToTwo(Val(txt_FaceValue.Text) * Val(cbo_FaceValue.SelectedValue) * txt_Rate.Text / 100)
            'Else
            '    MatDate = FillDateArrays(Hid_MatDate.Value)
            '    MatAmt = FillAmtArrays(Hid_MatAmt.Value)
            '    CoupDate = FillDateArrays(Hid_CoupDate.Value)
            '    CoupRate = FillAmtArrays(Hid_CoupRate.Value)
            '    With objCommon
            '        datYTM = .DateFormat(txt_Date.Text)
            '        datInterest = Hid_InterestDate.Value
            '        datBookClosure = Hid_BookClosureDate.Value
            '        blnNonGovernment = IIf(Hid_GovernmentFlag.Value = "N", True, False)
            '        blnRateActual = IIf(rdo_RateActual.SelectedValue = "R", True, False)
            '        blnDMAT = IIf(rdo_PhysicalDMAT.SelectedValue = "D", True, False)
            '        decFaceValue = .DecimalFormat(txt_FaceValue.Text)
            '        decRate = .DecimalFormat(txt_Rate.Text)
            '        datIssue = Hid_Issue.Value
            '        datBookClosure = IIf(blnDMAT = True, Hid_DMATBkDate.Value, Hid_BookClosureDate.Value)
            '        intBKDiff = CalculateBookClosureDiff(datBookClosure, rdo_PhysicalDMAT.SelectedValue, datInterest, blnNonGovernment)
            '        If blnNonGovernment = False Then
            '            intDays = 360
            '        Else
            '            intDays = rdo_Days.SelectedValue
            '        End If
            '    End With
            '    CalculateAccuredInterest(datYTM, Val(Hid_NSDLFaceValue.Value), decRate, blnNonGovernment, blnRateActual, _
            '                  MatDate, MatAmt, CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Hid_Frequency.Value, _
            '                  intDays, FinalAmount, IntDate, AddLess, AddLessNoofDays, Ratio, IntAmount, "Y")
            '    If rdo_RateActual.SelectedValue = "R" Then
            '        txt_Amount.Text = RoundToTwo(Val(txt_FaceValue.Text) * Val(cbo_FaceValue.SelectedValue) * txt_Rate.Text / 100)
            '        txt_AddInterest.Text = RoundToTwo(IntAmount * Val(txt_FaceValue.Text) * Val(cbo_FaceValue.SelectedValue))
            '    Else
            '        txt_Amount.Text = RoundToTwo(FinalAmount / Val(Hid_FaceValue.Value) * Val(txt_FaceValue.Text) * Val(cbo_FaceValue.SelectedValue))
            '        txt_AddInterest.Text = RoundToTwo(IntAmount * Val(txt_FaceValue.Text) * cbo_FaceValue.SelectedValue)
            '    End If
            '    If AddLess = "L" Then
            '        txt_SettAmt.Text = Val(txt_Amount.Text) - Val(txt_AddInterest.Text)
            '    Else
            '        txt_SettAmt.Text = Val(txt_Amount.Text) + Val(txt_AddInterest.Text)
            '    End If
            '    'txt_Amount.Text = Format(txt_Amount.Text, "##,##,##,##,###.00")
            '    'txt_AddInterest.Text = Format(txt_AddInterest.Text, "##,##,##,##,###.00")
            '    'txt_SettAmt.Text = Format(txt_SettAmt.Text, "##,##,##,##,###.00")
            '    lbl_AddInterest.Visible = True
            '    lbl_SettAmt.Visible = True
            '    If Val(txt_AddInterest.Text) = 0 Then
            '        lbl_AddInterest.Text = ""
            '        lbl_SettAmt.Text = ""
            '    End If
            '    If AddLess = "A" Then
            '        row_Interest.TagName = "Add Interest:"
            '        lbl_AddInterest.Text = "(" & Format(IntDate, "dd/MM/yyyy") & " - " & txt_Date.Text & ")"
            '        lbl_SettAmt.Text = "(" & AddLessNoofDays & " Days)"
            '    Else
            '        row_Interest.TagName = "Less Interest:"
            '        Hid_IntDays.Value = -1 * Val(AddLessNoofDays & "")
            '        lbl_AddInterest.Text = "(" & txt_Date.Text & " - " & Format(IntDate, "dd/MM/yyyy") & ")"
            '        lbl_SettAmt.Text = "(" & Hid_IntDays.Value & " Days)"
            '        txt_AddInterest.Text = "-" & txt_AddInterest.Text
            '        txt_AddInterest.Attributes.Add("style", "color:red")
            '        lbl_SettAmt.Attributes.Add("style", "color:red")
            '    End If
            'End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
        End Try
    End Function

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            Session("URL") = ""
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub

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
        End Try
    End Function

    Private Sub FillAccruedDetails()
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            Dim sqlda As New SqlDataAdapter
            Dim dt As New DataTable
            Dim sqldv As New DataView

            sqlComm.Connection = sqlConn
            sqlComm.CommandText = "ID_Fill_QuoteEntry_AccruedDetails"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Parameters.Clear()

            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 4, "I", , , Val(Request.QueryString("Id") & ""))
            objCommon.SetCommandParameters(sqlComm, "@SettlementDate", SqlDbType.Date, 4, "I", , , objCommon.DateFormat(txt_Date.Text))
            objCommon.SetCommandParameters(sqlComm, "@TotalFaceValue", SqlDbType.Decimal, 20, "I", , , Val(txt_FaceValue.Text) * Val(cbo_FaceValue.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@StepUp", SqlDbType.Char, 1, "I", , , Rbl_StepUp_down)
            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(dt)

            If dt.Rows.Count > 0 Then
                '    Hid_AddInterest.Value = Val(dt.Rows(0).Item("AccruedInterest") & "")
                Hid_IntDays.Value = Val(dt.Rows(0).Item("AccruedDays") & "")
                'Hid_InterestFromTo.Value = Trim(dt.Rows(0).Item("AccruedDates") & "")
                'Hid_Amtshow.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)) * Val(txt_Rate.Text) / 100)
                'Hid_ShowInterest.Value = RoundToTwo(IntAmount * Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue))
                'Hid_Amt.Value = RoundToTwo(Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue) * Val(txt_Rate.Text) / 100)
                'Hid_SettlementAmt.Value = Val(Hid_Amtshow.Value) + Val(Hid_AddInterest.Value)


                '    If Val(dt.Rows(0).Item("AccruedInterest") & "") < 0 Then
                '        Hid_IntDays.Value = -1 * Val(dt.Rows(0).Item("AccruedDays") & "")
                '        Hid_SettlementAmt.Value = Val(Hid_Amt.Value) + Val(Hid_AddInterest.Value)
                '        Hid_AddInterest.Value = Val(Hid_AddInterest.Value)
                '        lbl_InterestDays.Text = Hid_IntDays.Value
                '        lbl_InterestAmt.Text = Hid_AddInterest.Value
                '        lbl_InterestDays.ForeColor = Drawing.Color.Red
                '        lbl_SettlementAmt.Text = Hid_SettlementAmt.Value

                '    End If

                'If Val(dt.Rows(0).Item("AccruedDays") & "") > 0 Then
                '    row_Interest.TagName = "Add Interest:"
                '    lbl_AddInterest.Text = Trim(dt.Rows(0).Item("AccruedDates") & "")
                '    lbl_SettAmt.Text = RoundToTwo((Val(txt_Amount.Text)) * Val(txt_Rate.Text) / 100) + Val(dt.Rows(0).Item("AccruedInterest") & "")
                'Else
                '    row_Interest.TagName = "Less Interest:"
                '    Hid_IntDays.Value = Val(dt.Rows(0).Item("AccruedDays") & "")
                '    lbl_AddInterest.Text = Val(dt.Rows(0).Item("AccruedInterest") & "")
                '    lbl_SettAmt.Text = "(" & Hid_IntDays.Value & " Days)"
                '    txt_AddInterest.Text = "-" & txt_AddInterest.Text
                '    txt_AddInterest.Attributes.Add("style", "color:red")
                '    lbl_SettAmt.Attributes.Add("style", "color:red")
                'End If
                If rdo_RateActual.SelectedValue = "R" Then
                    txt_Amount.Text = RoundToTwo(Val(txt_FaceValue.Text) * Val(cbo_FaceValue.SelectedValue) * RoundToFour(Val(txt_Rate.Text)) / 100)
                    txt_AddInterest.Text = RoundToTwo(Val(dt.Rows(0).Item("AccruedInterest") & ""))
                Else
                    txt_Amount.Text = RoundToTwo((txt_Amount.Text + txt_AddInterest.Text) / Val(Hid_FaceValue.Value) * Val(txt_FaceValue.Text) * Val(cbo_FaceValue.SelectedValue))
                    txt_AddInterest.Text = RoundToTwo(Val(dt.Rows(0).Item("AccruedInterest") & ""))
                End If
              
                txt_SettAmt.Text = Val(txt_Amount.Text) + Val(txt_AddInterest.Text)

                'txt_Amount.Text = Format(txt_Amount.Text, "##,##,##,##,###.00")
                'txt_AddInterest.Text = Format(txt_AddInterest.Text, "##,##,##,##,###.00")
                'txt_SettAmt.Text = Format(txt_SettAmt.Text, "##,##,##,##,###.00")
                lbl_AddInterest.Visible = True
                lbl_SettAmt.Visible = True
                If Val(txt_AddInterest.Text) = 0 Then
                    lbl_AddInterest.Text = ""
                    lbl_SettAmt.Text = ""
                End If
                If Val(dt.Rows(0).Item("AccruedDays") & "") > 0 Then
                    row_Interest.TagName = "Add Interest:"
                    lbl_AddInterest.Text = Trim(dt.Rows(0).Item("AccruedDates") & "")
                    lbl_SettAmt.Text = "(" & Val(dt.Rows(0).Item("AccruedDays") & "") & " Days)"
                Else
                    row_Interest.TagName = "Less Interest:"
                    Hid_IntDays.Value = Val(dt.Rows(0).Item("AccruedDays") & "")
                    lbl_AddInterest.Text = Trim(dt.Rows(0).Item("AccruedDates") & "")
                    lbl_SettAmt.Text = "(" & Hid_IntDays.Value & " Days)"
                    txt_AddInterest.Text = "-" & txt_AddInterest.Text
                    txt_AddInterest.Attributes.Add("style", "color:red")
                    lbl_SettAmt.Attributes.Add("style", "color:red")
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
End Class
