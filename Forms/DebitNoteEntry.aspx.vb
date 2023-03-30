Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_DebitNoteEntry
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim lstItem As ListItem
    Dim IssueId As Integer
    Dim sqlConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
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

            'rdo_RoundOff.Items(0).Attributes.Add("onclick", "calcRevTax();")
            'rdo_RoundOff.Items(1).Attributes.Add("onclick", "calcRevTax();")

            If IsPostBack = False Then
                SetAttributes()
                SetControls()

                'Hid_YearId.Value = (Session("YearId"))
                If Val(Request.QueryString("Id") & "") <> 0 Then
                    ViewState("Id") = Val(Request.QueryString("Id") & "")
                    FillServiceTax()
                    FillFields()
                    srh_IssueDesc.SearchButton.Visible = False
                    srh_IssuerName.SearchButton.Visible = False
                    btn_Save.Visible = False
                    btn_Update.Visible = True
                    btn_ReCalculate.Visible = True
                Else
                    GetDebitNoteNo()
                End If
            Else
                'FillAppliAllotmTotals()
                'Setquantity()
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType, "check4", "CheckRateAmt('fnt_Perc4','txt_TotAppliamt');", True)
            Page.ClientScript.RegisterStartupScript(Me.GetType, "check5", "CheckRateAmt('fnt_Perc6','txt_totAlloAmt');", True)

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
        
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
        'txt_ServiceTaxRate.Attributes.Add("onblur", "calcTax();")
        'txt_CessRate.Attributes.Add("onblur", "calcTax();")
        'txt_SecoCessRate.Attributes.Add("onblur", "calcTax();")
        txt_TDSDeducted.Attributes.Add("onblur", "calcnetamtreceived();")
        'txt_totFees.Attributes.Add("onblur", "calcRevTax();")
        txt_DebitDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_DebitDate.Attributes.Add("onblur", "CheckDate(this,false);")

        txt_ServiceTaxRate.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_CessRate.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_SecoCessRate.Attributes.Add("onkeypress", "OnlyDecimal();")

        txt_Chequedate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_Chequedate.Attributes.Add("onblur", "CheckDate(this,false);")

        txt_DepositDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_DepositDate.Attributes.Add("onblur", "CheckDate(this,false);")

        txt_TDSDeducted.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_Netamtreceived.Attributes.Add("onkeypress", "OnlyDecimal();")

    End Sub
    Private Sub SetControls()
        'srh_IssuerName.Columns.Add("IssuerName")
        'srh_IssuerName.Columns.Add("IssuerTypeName")
        'srh_IssuerName.Columns.Add("City")
        'srh_IssuerName.Columns.Add("IM.IssuerId")
        'srh_IssueDesc.Columns.Add("IssueName")
        'srh_IssueDesc.Columns.Add("IE.IssueId")
        txt_DebitDate.Text = Format(Today, "dd/MM/yyyy")

    End Sub

    Private Sub FillAppliAllotmTotals()
        Dim dt As DataTable
        Try
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "MB_FILL_TotalApplAmt", srh_IssueDesc.SelectedId, "IssueId")
            If dt.Rows.Count > 0 Then
                If (Val(dt.Rows(0).Item("TotalApplAmt") & "") Or Val(dt.Rows(0).Item("TotalAllotAmt") & "")) = 0 Then

                    Dim msg As String = "Total Application Amt or total allotment amt is zero Please take another Issue."
                    Dim strHtml As String
                    strHtml = "alert('" + msg + "');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
                    txt_totAlloAmt.Text = ""
                    txt_TotAppliamt.Text = ""
                    'txt_Fees.Text = ""
                    'txt_ServiceTaxRate.Text = objCommon.DecimalFormat(dt.Rows(0).Item("ServiceTax") & "")
                    Exit Sub

                Else
                    txt_TotAppliamt.Text = objCommon.DecimalFormat(dt.Rows(0).Item("TotalApplAmt") & "")
                    txt_totAlloAmt.Text = objCommon.DecimalFormat(dt.Rows(0).Item("TotalAllotAmt") & "")
                    'txt_Fees.Text = Val(Hid_Fees.Value)
                    'txt_ServiceTaxRate.Text = objCommon.DecimalFormat(dt.Rows(0).Item("ServiceTax") & "")

                End If

            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

      
    End Sub

    Private Sub FillFees()
        Dim dt As DataTable
        Dim dt1 As DataTable
        Dim dt2 As DataTable
        Dim dtRem As DataTable
        Dim dblMobilFees As Double = 0
        Dim dblIssueFee As Double = 0
        Dim dblfees As Double
        Dim i As Integer
        Dim j As Integer
        Dim dblrate As Decimal
        Dim dblrate1 As Double
        Dim stra As String = ""
        Dim strRemk As String = ""
        Dim dblfeesMob As Double
        Dim dblfeesLamp As Double
        Dim FeesOn As Char
        Dim dblFee As Double
        Dim dblServFee As Double
        Try
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "MB_FILL_FEES", srh_IssueDesc.SelectedId, "IssueId")
            If dt.Rows.Count > 0 Then
                Hid_NomanClature.Value = Trim(dt.Rows(0).Item("NomanClature") & "")
                Hid_IssuerName.Value = Trim(dt.Rows(0).Item("IssuerName") & "")
                Hid_ServTaxInEx.Value = Trim(dt.Rows(0).Item("ServTaxInEx") & "")
                dblFee = objCommon.DecimalFormat(Val(dt.Rows(i).Item("Fees") & ""))
            End If
            If Trim(dt.Rows(0).Item("ServTaxInEx") & "") = "Y" Then
                txt_totFees.ReadOnly = False
            End If
            FeesOn = Trim(dt.Rows(i).Item("FeesOn") & "")
            For i = 0 To dt.Rows.Count - 1
                If Trim(dt.Rows(i).Item("FeesOn") & "") = "M" And Val(dt.Rows(0).Item("InstrumentId") & "") <> 35 Then
                    dblMobilFees = dblMobilFees + (objCommon.DecimalFormat(Val(txt_totAlloAmt.Text)) * objCommon.DecimalFormat(dt.Rows(i).Item("Fees") & "")) / 100
                    dblrate = objCommon.DecimalFormat(Val(dt.Rows(i).Item("Fees") & ""))
                    dblfeesMob = objCommon.DecimalFormat(Val(dt.Rows(i).Item("Fees") & ""))
                    dblfeesLamp = objCommon.DecimalFormat(Val(dt.Rows(i).Item("FeeAmount") & ""))
                ElseIf Trim(dt.Rows(i).Item("FeesOn") & "") = "M" And Val(dt.Rows(0).Item("InstrumentId") & "") = 35 Then
                    dblMobilFees = dblMobilFees + objCommon.DecimalFormat(Val(dt.Rows(i).Item("Fees1") & ""))
                    dblrate = objCommon.DecimalFormat(Val(dt.Rows(i).Item("Fees") & ""))
                    dblfeesMob = objCommon.DecimalFormat(Val(dt.Rows(i).Item("Fees") & ""))
                    dblfeesLamp = objCommon.DecimalFormat(Val(dt.Rows(i).Item("FeeAmount") & ""))
                ElseIf Trim(dt.Rows(i).Item("FeesOn") & "") = "I" Then
                    dblIssueFee = dblIssueFee + ((Val(dt.Rows(i).Item("IssueSizeQty") & "") * Val(dt.Rows(i).Item("IssueSizeMultiple") & "")) + (Val(dt.Rows(i).Item("GreenShoeQty") & "") * Val(dt.Rows(i).Item("GreenShoeMultiple") & ""))) * objCommon.DecimalFormat(Val(dt.Rows(i).Item("Fees") & "")) / Val(dt.Rows(i).Item("TotalArranger") & "") * 100
                    dblrate = objCommon.DecimalFormat(Val(dt.Rows(i).Item("Fees") & ""))
                    dblfeesLamp = objCommon.DecimalFormat(Val(dt.Rows(i).Item("FeeAmount") & ""))

                ElseIf Trim(dt.Rows(i).Item("FeesOn") & "") = "L" Then
                    dblIssueFee = dblIssueFee + ((Val(dt.Rows(i).Item("IssueSizeQty") & "") * Val(dt.Rows(i).Item("IssueSizeMultiple") & "")) + (Val(dt.Rows(i).Item("GreenShoeQty") & "") * Val(dt.Rows(i).Item("GreenShoeMultiple") & ""))) * objCommon.DecimalFormat(Val(dt.Rows(i).Item("Fees") & "")) / Val(dt.Rows(i).Item("TotalArranger") & "") * 100
                    dblrate = objCommon.DecimalFormat(Val(dt.Rows(i).Item("Fees") & ""))
                    dblfeesLamp = objCommon.DecimalFormat(Val(dt.Rows(i).Item("FeeAmount") & ""))
                End If
            Next
            If dt.Rows.Count > 1 Then
                If Val(dt.Rows(0).Item("InstrumentId") & "") = 35 And FeesOn <> "L" Then
                    'txt_Fees.Text = Val(dt.Rows(0).Item("Fees1") & "")
                    txt_Fees.Text = dblMobilFees
                ElseIf Val(dt.Rows(0).Item("InstrumentId") & "") = 35 And FeesOn = "L" Then
                    txt_Fees.Text = dblfeesLamp
                ElseIf FeesOn = "L" Then
                    txt_Fees.Text = dblfeesLamp
                Else
                    txt_totFees.Text = dblMobilFees
                End If

                If Val(dt.Rows(0).Item("InstrumentId") & "") = 36 Then
                    txt_Fees.Text = Val(dt.Rows(0).Item("NCDFees") & "")
                    txt_totFees.Text = Val(dt.Rows(0).Item("NCDFees") & "")
                End If
            End If
            If Val(dt.Rows(0).Item("InstrumentId") & "") = 35 And FeesOn <> "L" Then
                txt_Fees.Text = dblMobilFees
                'txt_Fees.Text = Val(dt.Rows(0).Item("Fees1") & "")

            ElseIf Val(dt.Rows(0).Item("InstrumentId") & "") = 35 And FeesOn = "L" Then
                txt_Fees.Text = dblfeesLamp
            ElseIf FeesOn = "L" Then
                txt_Fees.Text = dblfeesLamp
            ElseIf dt.Rows.Count = 1 Then
                txt_Fees.Text = (Val(txt_totAlloAmt.Text) * Val(dt.Rows(0).Item("Fees") & "")) / 100
                If Trim(dt.Rows(0).Item("ServTaxInEx") & "") = "Y" Then
                    txt_totFees.Text = (Val(txt_totAlloAmt.Text) * Val(dt.Rows(0).Item("Fees") & "")) / 100
                End If
            End If
            If (Val(dt.Rows(0).Item("InstrumentId") & "") = 35 And FeesOn <> "L") Then
                txt_totFees.Text = dblMobilFees
                'txt_totFees.Text = Val(dt.Rows(0).Item("Fees1") & "")
            End If
            If FeesOn <> "L" And Val(dt.Rows(0).Item("InstrumentId") & "") <> 35 And Val(dt.Rows(0).Item("InstrumentId") & "") <> 36 Then
                txt_totFees.Text = objCommon.DecimalFormat((Val(txt_totAlloAmt.Text) * Val(dt.Rows(0).Item("Fees") & "")) / 100)
                txt_Fees.Text = Val(txt_totFees.Text)
            ElseIf FeesOn = "L" Then
                txt_totFees.Text = dblfeesLamp
            End If
            Hid_Fee1.Value = Val(txt_totFees.Text)
            If Hid_ServTaxInEx.Value = "Y" Then
                'txt_totFees.Attributes.Add("onblur", "calcRevTax();")
                'txt_Fees.Attributes.Add("onblur", "calcRevTax();")
                rdo_RoundOff.Items(0).Attributes.Add("onclick", "calcRevTax();")
                rdo_RoundOff.Items(1).Attributes.Add("onclick", "calcRevTax();")

                Page.ClientScript.RegisterStartupScript(Me.GetType, "ab", "calcRevTax();", True)
            Else
                'txt_totFees.Attributes.Add("onblur", "calcTax();")
                'txt_Fees.Attributes.Add("onblur", "calcTax();")
                rdo_RoundOff.Items(0).Attributes.Add("onclick", "calcTax();")
                rdo_RoundOff.Items(1).Attributes.Add("onclick", "calcTax();")

                Page.ClientScript.RegisterStartupScript(Me.GetType, "df", "calcTax();", True)
            End If
            dblfees = objCommon.DecimalFormat(Val(txt_Fees.Text))
            Hid_remark.Value = Val(txt_totAlloAmt.Text) / 10000000
            dt1 = objCommon.FillDataTable(sqlConn, "MB_FILL_TotalApplAmtNEW", srh_IssueDesc.SelectedId, "IssueId")
            For j = 0 To dt1.Rows.Count - 1
                If dt1.Rows.Count = 1 Then
                    txt_Remark.Text = "Being fees @ " & dblrate & " % for arranging Rs." & Val(Hid_remark.Value) & " crores" & "  for " & srh_IssuerName.SearchTextBox.Text & ", " & Trim(Hid_NomanClature.Value) & " on rate of " & dblrate & "%"
                Else
                    Hid_TotAllAmt1.Value = Val(dt1.Rows(j).Item("TotalAllotAmt")) / 10000000
                    dblrate1 = objCommon.DecimalFormat(Val(dt1.Rows(j).Item("Fees") & ""))
                    stra = stra & "Rs. " & Val(Hid_TotAllAmt1.Value) & " crores @ " & dblrate1 & " % and "
                    Hid_MultiReamrk.Value = Mid(stra, 1, Len(stra) - Len(Right(stra, 4)))
                End If
            Next
            dtRem = objCommon.FillDataTable(sqlConn, "MB_FILL_Narration", srh_IssueDesc.SelectedId, "IssueId")
            If dtRem.Rows.Count = 1 Then
                If Trim(dtRem.Rows(0).Item("ServTaxInEx") & "") = "Y" Then
                    If Val(Hid_ServFees.Value) <> 0 Then
                        dblServFee = objCommon.DecimalFormat(Hid_ServFees.Value) * 100 / objCommon.DecimalFormat(txt_totAlloAmt.Text)
                        dblServFee = objCommon.DecimalFormat(dblServFee)
                    End If
                End If

            End If
            If FeesOn = "L" Then
                txt_Remark.Text = "Being fees for arranging funds Rs." & Val(txt_TotAppliamt.Text) / 10000000 & " Crore" & "   for  " & srh_IssuerName.SearchTextBox.Text
            ElseIf Val(dtRem.Rows(0).Item("InstrumentId") & "") = 35 Then
                txt_Remark.Text = "Being fees @ " & dblfeesMob & " % for arranging Rs." & Trim(dtRem.Rows(0).Item("CPNarration") & "") & "  for  " & Trim(dtRem.Rows(0).Item("Instrument") & "") & " For " & Trim(dtRem.Rows(0).Item("Tenor") & "") & "." & Trim(dtRem.Rows(0).Item("CPNarration1") & "")
            ElseIf Trim(dtRem.Rows(0).Item("ServTaxInEx") & "") = "Y" And Val(Hid_ServFees.Value) <> 0 And Val(dtRem.Rows(0).Item("InstrumentId") & "") <> 36 Then
                txt_Remark.Text = "Being fees @ " & dblFee & " % for arranging Rs." & Trim(dtRem.Rows(0).Item("Narration") & "") & Val(Hid_remark.Value) & " crores" & "  for " & srh_IssuerName.SearchTextBox.Text & ", " & Trim(Hid_NomanClature.Value)
            Else
                txt_Remark.Text = "Being fees @" & Trim(dtRem.Rows(0).Item("MultiFees") & " ") & "for arranging " & Trim(dtRem.Rows(0).Item("MultipleAlltmnt") & " ") & " Crores ,respectively for " & srh_IssueDesc.SearchTextBox.Text & " " & Trim(Hid_NomanClature.Value)
            End If
            If FeesOn = "L" Then
                txt_Remark.Text = "Being fees for arranging funds Rs." & Val(txt_TotAppliamt.Text) / 10000000 & " Crore" & "   for  " & srh_IssuerName.SearchTextBox.Text & Trim(dtRem.Rows(0).Item("Instrument") & "")
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try


    End Sub
    Protected Sub srh_IssueDesc_ButtonClick() Handles srh_IssueDesc.ButtonClick
        Try
            OpenConn()
            objCommon.FillControl(cbo_ContPerson, sqlConn, "MB_FILL_IssuerContactperDebit", "ContactPerson", "IssuerContactId", Val(srh_IssuerName.SelectedId), "IssuerId")
            CloseConn()
            FillAppliAllotmTotals()
            FillFees()
            FillServiceTax()
            'Page.ClientScript.RegisterStartupScript(Me.GetType, "df", "FillTax();", True)

        Catch ex As Exception

        End Try
        
    End Sub
    'Private Sub FillServiceTax()
    '    Try
    '        Dim dt As DataTable
    '        OpenConn()
    '        dt = objCommon.FillDataTable(sqlConn, "MB_FILL_IssueServiceTax", 0, "")
    '        If dt.Rows.Count > 0 Then
    '            txt_ServiceTaxRate.Text = dt.Rows(0).Item("Tax").ToString
    '            txt_CessRate.Text = dt.Rows(0).Item("Cess").ToString
    '            txt_SecoCessRate.Text = dt.Rows(0).Item("ECess").ToString
    '        End If
    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    Finally
    '        CloseConn()
    '    End Try
    'End Sub
    Private Sub FillServiceTax()
        Try
            Dim dt As New DataTable
            OpenConn()
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_FILL_ServiceTax"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@SettlementDate", SqlDbType.SmallDateTime, 9, "I", , , objCommon.DateFormat(txt_DebitDate.Text))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            If dt.Rows.Count > 0 Then
                txt_ServiceTaxRate.Text = dt.Rows(0).Item("Tax").ToString
                txt_CessRate.Text = dt.Rows(0).Item("Cess").ToString
                txt_SecoCessRate.Text = dt.Rows(0).Item("ECess").ToString
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        GetDebitNoteNo()
        If SetSaveUpdate("MB_INSERT_DebitNoteEntry") = True Then
            Page.ClientScript.RegisterStartupScript(Me.GetType, "open", "ReportView()", True)
        End If
    End Sub

    Protected Sub btn_Back_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Back.Click
        Try
            Response.Redirect("DebitNoteDetails.aspx", False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function SetSaveUpdate(ByVal strProc As String, Optional ByVal blnRedirect As Boolean = True) As Boolean

        Try
            OpenConn()
            Dim sqlTrans As SqlTransaction
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Function
            sqlTrans.Commit()
            If blnRedirect = False Then
                Response.Redirect("DebitNoteDetails.aspx?Id=" & ViewState("Id"), False)
            End If
            Return True

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Function
    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try

            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn


            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@DebitNoteId", SqlDbType.BigInt, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@DebitNoteId", SqlDbType.BigInt, 4, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@IssuerId", SqlDbType.Int, 4, "I", , , Val(srh_IssuerName.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@IssueId", SqlDbType.Int, 4, "I", , , Val(srh_IssueDesc.SelectedId))
            If Val(cbo_ContPerson.SelectedValue) = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@IssuerContactId", SqlDbType.Int, 4, "I", , , DBNull.Value)
            Else
                objCommon.SetCommandParameters(sqlComm, "@IssuerContactId", SqlDbType.Int, 4, "I", , , Val(cbo_ContPerson.SelectedValue))
            End If
            objCommon.SetCommandParameters(sqlComm, "@DebitNoteDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_DebitDate.Text))
            objCommon.SetCommandParameters(sqlComm, "@ServiceTax", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_ServiceTaxRate.Text)))
            objCommon.SetCommandParameters(sqlComm, "@Cess", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_CessRate.Text)))
            objCommon.SetCommandParameters(sqlComm, "@SecondaryCess", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_SecoCessRate.Text)))
            objCommon.SetCommandParameters(sqlComm, "@TotalFees", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_totFees.Text)))
            objCommon.SetCommandParameters(sqlComm, "@Fees", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_Fees.Text)))
            objCommon.SetCommandParameters(sqlComm, "@FeesAmt", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_Fees.Text)))

            objCommon.SetCommandParameters(sqlComm, "@HECessAmt", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_CessAmt.Text)))
            objCommon.SetCommandParameters(sqlComm, "@ECessAmt", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_SecoCessAmt.Text)))
            objCommon.SetCommandParameters(sqlComm, "@ServiceTaxAmt", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_ServiceTaxAmt.Text)))


            objCommon.SetCommandParameters(sqlComm, "@Remark", SqlDbType.VarChar, 2000, "I", , , Trim(txt_Remark.Text))
            objCommon.SetCommandParameters(sqlComm, "@DebitNoteNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_DebitNoteNo.Text))

            objCommon.SetCommandParameters(sqlComm, "@CompanyId", SqlDbType.Int, 4, "I", , , Session("CompanyId"))
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Session("YearId"))


            If Trim(txt_Chequedate.Text).ToString <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@Chequedate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_Chequedate.Text))
            End If

            objCommon.SetCommandParameters(sqlComm, "@ChequeNo", SqlDbType.VarChar, 10, "I", , , (txt_ChequeNo.Text))
            If Trim(txt_DepositDate.Text).ToString <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@DepositDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_DepositDate.Text))
            End If

            If Trim(Hid_Netamtreceived.Value).ToString <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@NetReceivedAmt", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(Hid_Netamtreceived.Value)))
            Else

                objCommon.SetCommandParameters(sqlComm, "@NetReceivedAmt", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_Netamtreceived.Text)))
            End If

            If Trim(txt_TDSDeducted.Text).ToString <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@TDSDeducted", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_TDSDeducted.Text)))
            End If
            objCommon.SetCommandParameters(sqlComm, "@Depositedin", SqlDbType.VarChar, 50, "I", , , (txt_depositedin.Text))
            If Trim(txt_DeliveryDate.Text).ToString <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@DeliveryDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_DeliveryDate.Text))
            End If
            If Trim(txt_ReceiptdateFees.Text).ToString <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@FeeReceiptDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ReceiptdateFees.Text))
            End If

            If Trim(txt_AccountClouserDate.Text).ToString <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@AccountClosueDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_AccountClouserDate.Text))
            End If


            objCommon.SetCommandParameters(sqlComm, "@FormOfReceipt", SqlDbType.Char, 1, "I", , , Trim(rdo_ReceiptForm.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@DebitFor", SqlDbType.Char, 1, "I", , , "I")
            objCommon.SetCommandParameters(sqlComm, "@RoundOff", SqlDbType.Char, 1, "I", , , Trim(rdo_RoundOff.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@DispatchallIncentivetoallInvestor", SqlDbType.Char, 1, "I", , , Trim(rdo_DispatchIncentive.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Session("CompId"))

            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@DebitNoteId").Value
            Hid_DebitNoteId.Value = ViewState("Id")
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        If SetSaveUpdate("MB_UPDATE_DebitNoteEntry") = True Then
            Page.ClientScript.RegisterStartupScript(Me.GetType, "open", "ReportView()", True)
        End If

    End Sub
    Private Sub FillFields()

        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "MB_FILL_DebitNoteEntry", ViewState("Id"), "DebitNoteId")
            If dt.Rows.Count > 0 Then
                srh_IssuerName.SelectedId = Val(dt.Rows(0).Item("IssuerId") & "")
                srh_IssuerName.SearchTextBox.Text = dt.Rows(0).Item("IssuerName").ToString
                srh_IssueDesc.SelectedId = Val(dt.Rows(0).Item("IssueId") & "")
                srh_IssueDesc.SearchTextBox.Text = dt.Rows(0).Item("IssueName").ToString
                txt_DebitDate.Text = Format(dt.Rows(0).Item("DebitNoteDate"), "dd/MM/yyyy")
                txt_ServiceTaxRate.Text = objCommon.DecimalFormat(dt.Rows(0).Item("ServiceTax") & "")
                txt_CessRate.Text = objCommon.DecimalFormat(dt.Rows(0).Item("Cess") & "")
                txt_SecoCessRate.Text = objCommon.DecimalFormat(dt.Rows(0).Item("SecondaryCess") & "")
                txt_Remark.Text = Trim(dt.Rows(0).Item("Remark") & "")
                objCommon.FillControl(cbo_ContPerson, sqlConn, "MB_FILL_IssuerContactperDebit", "ContactPerson", "IssuerContactId", srh_IssuerName.SelectedId, "IssuerId")
                cbo_ContPerson.SelectedValue = Val(dt.Rows(0).Item("IssuerContactId") & "")
                txt_DebitNoteNo.Text = Trim(dt.Rows(0).Item("DebitNoteNo") & "")
                If Val(dt.Rows(0).Item("HECessAmt") & "") <> 0.0 Then
                    txt_CessAmt.Text = objCommon.DecimalFormat(dt.Rows(0).Item("HECessAmt") & "")
                End If
                If Val(dt.Rows(0).Item("ECessAmt") & "") <> 0.0 Then
                    txt_SecoCessAmt.Text = objCommon.DecimalFormat(dt.Rows(0).Item("ECessAmt") & "")
                End If
                If Val(dt.Rows(0).Item("ServiceTaxAmt") & "") <> 0.0 Then
                    txt_ServiceTaxAmt.Text = objCommon.DecimalFormat(dt.Rows(0).Item("ServiceTaxAmt") & "")
                End If
                If Val(dt.Rows(0).Item("TotFees") & "") <> 0.0 Then
                    txt_totFees.Text = objCommon.DecimalFormat(dt.Rows(0).Item("TotFees") & "")
                    txt_DebitNoteAmt.Text = objCommon.DecimalFormat(dt.Rows(0).Item("TotFees") & "")
                End If

                If Val(dt.Rows(0).Item("FeesAmt") & "") <> 0.0 Then
                    txt_Fees.Text = objCommon.DecimalFormat(dt.Rows(0).Item("FeesAmt") & "")
                    Hid_ServFees.Value = objCommon.DecimalFormat(dt.Rows(0).Item("FeesAmt") & "")
                End If

                If Trim(dt.Rows(0).Item("Chequedate").ToString) <> "" Then
                    txt_Chequedate.Text = Format(dt.Rows(0).Item("Chequedate"), "dd/MM/yyyy")
                End If
                If Trim(dt.Rows(0).Item("DepositDate").ToString) <> "" Then
                    txt_DepositDate.Text = Format(dt.Rows(0).Item("DepositDate"), "dd/MM/yyyy")
                End If
                If Trim(dt.Rows(0).Item("DeliveryDate").ToString) <> "" Then
                    txt_DeliveryDate.Text = Format(dt.Rows(0).Item("DeliveryDate"), "dd/MM/yyyy")
                End If

                If Trim(dt.Rows(0).Item("FeeReceiptDate").ToString) <> "" Then
                    txt_ReceiptdateFees.Text = Format(dt.Rows(0).Item("FeeReceiptDate"), "dd/MM/yyyy")
                End If

                If Trim(dt.Rows(0).Item("FormOfReceipt").ToString) <> "" Then
                    rdo_ReceiptForm.SelectedValue = (dt.Rows(0).Item("FormOfReceipt"))
                End If
                If Trim(dt.Rows(0).Item("RoundOff").ToString) <> "" Then
                    rdo_RoundOff.SelectedValue = (dt.Rows(0).Item("RoundOff"))
                End If


                If Trim(dt.Rows(0).Item("DispatchallIncentivetoallInvestor").ToString) <> "" Then
                    rdo_DispatchIncentive.SelectedValue = (dt.Rows(0).Item("DispatchallIncentivetoallInvestor"))
                End If

                If Trim(dt.Rows(0).Item("AccountClosueDate").ToString) <> "" Then
                    txt_AccountClouserDate.Text = Format(dt.Rows(0).Item("AccountClosueDate"), "dd/MM/yyyy")
                End If


                If dt.Rows(0).Item("NetReceivedAmt").ToString <> "" Then
                    txt_Netamtreceived.Text = objCommon.DecimalFormat(dt.Rows(0).Item("NetReceivedAmt") & "")


                End If
                If dt.Rows(0).Item("TDSDeducted").ToString <> "" Then
                    txt_TDSDeducted.Text = objCommon.DecimalFormat(dt.Rows(0).Item("TDSDeducted") & "")
                End If


                txt_ChequeNo.Text = Trim(dt.Rows(0).Item("ChequeNo") & "")
                txt_depositedin.Text = Trim(dt.Rows(0).Item("Depositedin") & "")
               

                FillAppliAllotmTotals()
                'FillFees()
                'If Hid_ServTaxInEx.Value = "Y" Then
                '    'Page.ClientScript.RegisterStartupScript(Me.GetType, "ab", "calcRevTax();", True)
                '    rdo_RoundOff.Items(0).Attributes.Add("onclick", "calcRevTax();")
                '    rdo_RoundOff.Items(1).Attributes.Add("onclick", "calcRevTax();")
                '    'txt_totFees.Attributes.Add("onblur", "calcRevTax();")
                '    'txt_Fees.Attributes.Add("onblur", "calcRevTax();")
                'Else
                '    'Page.ClientScript.RegisterStartupScript(Me.GetType, "df", "calcTax();", True)
                '    rdo_RoundOff.Items(0).Attributes.Add("onclick", "calcTax();")
                '    rdo_RoundOff.Items(1).Attributes.Add("onclick", "calcTax();")
                '    'txt_totFees.Attributes.Add("onblur", "calcTax();")
                '    'txt_Fees.Attributes.Add("onblur", "calcTax();")
                'End If

                rdo_RoundOff.Items(0).Attributes.Add("onclick", "roundoff();")
                rdo_RoundOff.Items(1).Attributes.Add("onclick", "roundoff();")


            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Sub CalcTax()
        Try
            Dim servicetax As Decimal
            Dim Cess As Decimal
            Dim secondaeyCess As Decimal

            servicetax = (objCommon.DecimalFormat(Val(txt_Fees.Text)) * objCommon.DecimalFormat(Val(txt_ServiceTaxRate.Text))) / 100
            txt_ServiceTaxAmt.Text = servicetax
            Cess = (objCommon.DecimalFormat(Val(txt_Fees.Text)) * objCommon.DecimalFormat(Val(txt_CessRate.Text))) / 100
            txt_CessAmt.Text = Cess
            secondaeyCess = (objCommon.DecimalFormat(Val(txt_Fees.Text)) * objCommon.DecimalFormat(Val(txt_SecoCessRate.Text))) / 100
            txt_SecoCessAmt.Text = objCommon.DecimalFormat(Val(secondaeyCess))
            txt_totFees.Text = objCommon.DecimalFormat(Val(servicetax)) + objCommon.DecimalFormat(Val(Cess)) + objCommon.DecimalFormat(Val(secondaeyCess)) + objCommon.DecimalFormat(Val(txt_Fees.Text))
            txt_DebitNoteAmt.Text = objCommon.DecimalFormat(Val(servicetax)) + objCommon.DecimalFormat(Val(Cess)) + objCommon.DecimalFormat(Val(secondaeyCess)) + objCommon.DecimalFormat(Val(txt_Fees.Text))
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub GetDebitNoteNo()
        Try
            Dim DebitNoteNo As String
            Dim fstyr As String
            Dim lstyr As String
            Dim yr As String
            fstyr = Mid(Session("Year"), 3, 2)
            lstyr = Mid(Session("Year"), 8, 2)
            yr = fstyr + "-" + lstyr
            Dim sqlComm As New SqlCommand
            OpenConn()
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "MB_FILL_DebitNoteNoNew"
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@NextNo", SqlDbType.VarChar, 50, "O")
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Session("YearId"))
            objCommon.SetCommandParameters(sqlComm, "@DebitFor", SqlDbType.Char, 1, "I", , , "I")
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Hid_DebitNoteNo.Value = Trim(sqlComm.Parameters("@NextNo").Value & "")

            DebitNoteNo = "T" + "/" + "MB" + "/" + "IN" + "/" + yr + "/" + Hid_DebitNoteNo.Value
            txt_DebitNoteNo.Text = DebitNoteNo
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

        End Try

    End Sub


    Protected Sub btn_ReCalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ReCalculate.Click
        Try
            FillAppliAllotmTotals()
            FillFees()
            If Hid_ServTaxInEx.Value = "Y" Then
                Page.ClientScript.RegisterStartupScript(Me.GetType, "ab", "calcRevTax();", True)
                rdo_RoundOff.Items(0).Attributes.Add("onclick", "calcRevTax();")
                rdo_RoundOff.Items(1).Attributes.Add("onclick", "calcRevTax();")
                txt_totFees.Attributes.Add("onblur", "calcRevTax();")
                txt_Fees.Attributes.Add("onblur", "calcRevTax();")
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType, "df", "calcTax();", True)
                rdo_RoundOff.Items(0).Attributes.Add("onclick", "calcTax();")
                rdo_RoundOff.Items(1).Attributes.Add("onclick", "calcTax();")
                txt_totFees.Attributes.Add("onblur", "calcTax();")
                txt_Fees.Attributes.Add("onblur", "calcTax();")
            End If

            'If SetSaveUpdate("MB_UPDATE_DebitNoteEntry") = True Then
            '    Page.ClientScript.RegisterStartupScript(Me.GetType, "open", "ReportView();", True)
            'End If
        Catch ex As Exception

        End Try
    End Sub
End Class
