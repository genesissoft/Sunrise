Imports System.Data.SqlClient
Imports System.Data

Partial Class Forms_ContractNoteTrading
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'objCommon.OpenConn()
        Try
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")

            'rdo_RateAmount.Items(0).Attributes.Add("onclick", "CheckRateAmount()")
            'rdo_RateAmount.Items(1).Attributes.Add("onclick", "CheckRateAmount()")


            rdo_TradeType.Items(0).Attributes.Add("onclick", "CheckRepo();")
            rdo_TradeType.Items(1).Attributes.Add("onclick", "CheckRepo();")
            Hid_WDMDealNumber.Value = Left(Request.QueryString("WDMDealNumber"), 1)

            row_buysell.Visible = True
            If (IsPostBack = False) Then
                'btn_SaveandPrint.Attributes.Add("onclick", "return Validation();")
                SetAttributes()
                'RSetTimeCombos()
                Hid_DealEntryId.Value = Val(Request.QueryString("DealEntryId") & "")
                ViewState("Id") = Val(Request.QueryString("DealEntryId") & "")
                FillContractNoteFields()
                'CalcDuty()

                Page.ClientScript.RegisterStartupScript(Me.GetType, "s2", "CalcDutybyJS();", True)

            End If

            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "CheckRateAmount", "CheckRateAmount();", True)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "CheckRepo", "CheckRepo();", True)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub


    Private Sub CalcDuty()
        Try
            Dim SubTotal As Decimal
            lbl_StampDuty.Text = objCommon.FormatCurr((Val(Hid_SettlementAmount.Value) * Val(txt_StampDuty.Text)) / 100)
            If rdo_ContNotefor.SelectedValue = "B" Then
                SubTotal = (Val(Hid_BuyBroker.Value) - Val(lbl_StampDuty.Text))
            Else
                SubTotal = (Val(Hid_SellBroker.Value) - Val(lbl_StampDuty.Text))
            End If
            lbl_ServiceTax.Text = objCommon.FormatCurr((SubTotal * Val(txt_ServiceTax.Text)) / 100)
            lbl_Educationcess.Text = objCommon.FormatCurr((Val(lbl_ServiceTax.Text) * Val(txt_Educationcess.Text)) / 100)
            lbl_HEducationcess.Text = objCommon.FormatCurr((Val(lbl_ServiceTax.Text) * Val(txt_HEducationcess.Text)) / 100)

            Hid_StampDutyAMT.Value = Val(lbl_StampDuty.Text)
            Hid_ServiceTaxAMT.Value = Val(lbl_ServiceTax.Text)
            Hid_EducationcessAMT.Value = Val(lbl_Educationcess.Text)
            Hid_HEducationcessAMT.Value = Val(lbl_HEducationcess.Text)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub SetAttributes()
        txt_TradeDate.ReadOnly = True
        txt_TradeDate.Attributes.Add("onkeypress", "OnlyDate();")
        'txt_TradeDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_TradeDate.Text = Format(Now, "dd/MM/yyyy")
        txt_RepoRate.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_RepoPeriod.Attributes.Add("onkeypress", "OnlyInteger();")
        'txt_TradeDate.ReadOnly = False
        btn_SaveandPrint.Attributes.Add("onclick", "return Validation();")
        btn_ShowBackside.Attributes.Add("onclick", "return ShowBacksideReportr();")

        txt_StampDuty.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_ServiceTax.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_Educationcess.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_HEducationcess.Attributes.Add("onkeypress", "OnlyDecimal();")
        Dim strOrderNo As String
        strOrderNo = Format(Now, "yyyy/MM/dd")
        strOrderNo = Replace(strOrderNo, "/", "")
        strOrderNo = strOrderNo & "000"
        txt_OrderNo.Text = strOrderNo

    End Sub
    'Private Sub RSetTimeCombos()
    '    Try
    '        Dim i As Integer
    '        'Hid_txtHr.Value = cbo_hr.SelectedValue
    '        'Hid_txtMin.Value = cbo_minute.SelectedValue
    '        cbo_hr.Items.Clear()
    '        cbo_minute.Items.Clear()
    '        For i = 1 To 24
    '            If i < 10 Then
    '                'cbo_hr.Items.Add(New ListItem(Trim("0" & i), Val(i)))
    '                cbo_hr.Items.Add(New ListItem(Trim("0" & i), Trim("0" & i)))
    '            Else
    '                cbo_hr.Items.Add(New ListItem(Val(i), Val(i)))
    '            End If
    '        Next
    '        For i = 0 To 59
    '            If i < 10 Then
    '                cbo_minute.Items.Add(New ListItem(Trim("0" & i), Val("0" & i)))
    '            Else
    '                cbo_minute.Items.Add(New ListItem(Val(i), Val(i)))
    '            End If
    '        Next
    '    Catch ex As Exception
    '        Response.Write(ex.Message)
    '    End Try
    'End Sub

    Protected Sub btn_SaveandPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_SaveandPrint.Click
        If SetSaveUpdate("ID_UPDATE_WDMContractNote", False) = True Then
            Page.ClientScript.RegisterStartupScript(Me.GetType, "OpenReport", "OpenReport('WDMContractNote')", True)
        End If
    End Sub
    Private Function SetSaveUpdate(ByVal strProc As String, Optional ByVal blnRedirect As Boolean = True) As Boolean
        Try

            Dim sqlTrans As SqlTransaction
            sqlTrans = clsCommonFuns.sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Function
            sqlTrans.Commit()

            Return True
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = clsCommonFuns.sqlConn
            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@DealEntryId", SqlDbType.Int, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@DealEntryId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
            End If
            If rdo_ContNotefor.SelectedValue = "B" Then
                objCommon.SetCommandParameters(sqlComm, "@PurOrderNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_OrderNo.Text))
            Else
                objCommon.SetCommandParameters(sqlComm, "@SellOrderNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_OrderNo.Text))
            End If
            objCommon.SetCommandParameters(sqlComm, "@TradeNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_TradeNo.Text))

            'objCommon.SetCommandParameters(sqlComm, "@SecurityTypeCode", SqlDbType.VarChar, 50, "I", , , Trim(txt_TradeNo.Text))
            'objCommon.SetCommandParameters(sqlComm, "@SecurityCode", SqlDbType.VarChar, 50, "I", , , Trim(txt_TradeNo.Text))

            objCommon.SetCommandParameters(sqlComm, "@TradeType", SqlDbType.Char, 1, "I", , , Trim(rdo_TradeType.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@BuyerSellerContract", SqlDbType.Char, 1, "I", , , Trim(rdo_ContNotefor.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@Issue", SqlDbType.VarChar, 50, "I", , , Trim(txt_Couponrate.Text))
            If Trim(rdo_TradeType.SelectedValue) = "R" Then
                objCommon.SetCommandParameters(sqlComm, "@RepoPeriod", SqlDbType.SmallInt, 4, "I", , , Val(txt_RepoPeriod.Text))
                objCommon.SetCommandParameters(sqlComm, "@RepoRate", SqlDbType.Decimal, 9, "I", , , Val(txt_RepoRate.Text))
            Else
                objCommon.SetCommandParameters(sqlComm, "@RepoPeriod", SqlDbType.SmallInt, 4, "I", , , DBNull.Value)
                objCommon.SetCommandParameters(sqlComm, "@RepoRate", SqlDbType.Decimal, 9, "I", , , DBNull.Value)
            End If
            objCommon.SetCommandParameters(sqlComm, "@Tradedate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_TradeDate.Text))
            If Trim(txt_OrderAttributes.Text) <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@OrderAttributes", SqlDbType.VarChar, 50, "I", , , Trim(txt_OrderAttributes.Text))
            Else
                objCommon.SetCommandParameters(sqlComm, "@OrderAttributes", SqlDbType.VarChar, 50, "I", , , DBNull.Value)
            End If
            objCommon.SetCommandParameters(sqlComm, "@ConstituentRefNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_ConstRefNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@ParticipantCode", SqlDbType.VarChar, 50, "I", , , Trim(txt_ParticipantCode.Text))
            'objCommon.SetCommandParameters(sqlComm, "@StampDutyOn", SqlDbType.Char, 1, "I", , , Trim(rdo_RateAmount.SelectedValue))
            'objCommon.SetCommandParameters(sqlComm, "@StampDuty", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_StampDuty.Text)))
            'objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")



            objCommon.SetCommandParameters(sqlComm, "@StampDuty", SqlDbType.Decimal, 9, "I", , , Val(txt_StampDuty.Text))
            objCommon.SetCommandParameters(sqlComm, "@ServiceTax", SqlDbType.Decimal, 9, "I", , , Val(txt_ServiceTax.Text))
            objCommon.SetCommandParameters(sqlComm, "@Educationcess", SqlDbType.Decimal, 9, "I", , , Val(txt_Educationcess.Text))
            objCommon.SetCommandParameters(sqlComm, "@HEducationcess", SqlDbType.Decimal, 9, "I", , , Val(txt_HEducationcess.Text))

            If rdo_ContNotefor.SelectedValue = "B" Then
                objCommon.SetCommandParameters(sqlComm, "@PurStampDutyAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_StampDutyAMT.Value))
                objCommon.SetCommandParameters(sqlComm, "@PurServiceTaxAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_ServiceTaxAMT.Value))
                objCommon.SetCommandParameters(sqlComm, "@PurEducationcessAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_EducationcessAMT.Value))
                objCommon.SetCommandParameters(sqlComm, "@PurHEducationcessAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_HEducationcessAMT.Value))
            Else
                objCommon.SetCommandParameters(sqlComm, "@SellStampDutyAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_StampDutyAMT.Value))
                objCommon.SetCommandParameters(sqlComm, "@SellServiceTaxAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_ServiceTaxAMT.Value))
                objCommon.SetCommandParameters(sqlComm, "@SellEducationcessAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_EducationcessAMT.Value))
                objCommon.SetCommandParameters(sqlComm, "@SellHEducationcessAmt", SqlDbType.Decimal, 9, "I", , , Val(Hid_HEducationcessAMT.Value))
            End If




            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@DealEntryId").Value

            'Hid_DealEntryId.Value = ViewState("Id")
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Sub FillContractNoteFields()
        Try
            Dim dt As DataTable
            Dim strTime() As String
            Dim DealTime() As String
            Dim StrBuyer As String
            Dim StrSeller As String
            dt = objCommon.FillDataTable(clsCommonFuns.sqlConn, "ID_FILL_WDMContractNote", ViewState("Id"), "DealEntryId")
            If dt.Rows.Count > 0 Then
                lbl_Exchange.Text = Trim(dt.Rows(0).Item("ExchangeName") & "")
                rdo_ContNotefor.SelectedValue = Trim(dt.Rows(0).Item("BuyerSellerContract") & "")
                rdo_TradeType.SelectedValue = Trim(dt.Rows(0).Item("TradeType") & "")
                txt_Couponrate.Text = Trim(dt.Rows(0).Item("Issue") & "")
                If Trim(dt.Rows(0).Item("SecurityTypeCode") & "") = "CP" Or Trim(dt.Rows(0).Item("SecurityTypeCode") & "") = "CD" Then
                    txt_SecurityTypeCode.Text = "----"
                Else
                    txt_SecurityTypeCode.Text = Trim(dt.Rows(0).Item("SecurityTypeCode") & "")
                End If
                'txt_SecurityTypeCode.Text = Trim(dt.Rows(0).Item("SecurityTypeCode") & "")
                txt_SecurityCode.Text = Trim(dt.Rows(0).Item("SecurityCode") & "")
                txt_TradeDate.Text = Trim(dt.Rows(0).Item("DealDate") & "")


                If rdo_ContNotefor.SelectedValue = "B" Then
                    If Trim(dt.Rows(0).Item("SecurityTypeCode") & "") = "CP" Or Trim(dt.Rows(0).Item("SecurityTypeCode") & "") = "CD" Then
                        txt_OrderNo.Text = ""
                    Else
                        txt_OrderNo.Text = Trim(dt.Rows(0).Item("PurOrderNo") & "")
                    End If

                ElseIf rdo_ContNotefor.SelectedValue <> "B" Then
                    If Trim(dt.Rows(0).Item("SecurityTypeCode") & "") = "CP" Or Trim(dt.Rows(0).Item("SecurityTypeCode") & "") = "CD" Then
                        txt_OrderNo.Text = ""
                    Else
                        txt_OrderNo.Text = Trim(dt.Rows(0).Item("SellOrderNo") & "")
                    End If
                End If

                If Trim(dt.Rows(0).Item("TradeNo") & "") <> "" Then
                    txt_TradeNo.Text = Trim(dt.Rows(0).Item("TradeNo") & "")
                End If
                txt_OrderAttributes.Text = Trim(dt.Rows(0).Item("OrderAttributes") & "")
                rdo_TradeType.SelectedValue = Trim(dt.Rows(0).Item("TradeType") & "")
                If rdo_TradeType.SelectedValue = "R" Then
                    txt_RepoPeriod.Text = Trim(dt.Rows(0).Item("RepoPeriod") & "")
                    txt_RepoRate.Text = Trim(dt.Rows(0).Item("RepoRate") & "")

                Else
                    txt_RepoPeriod.Text = ""
                    txt_RepoRate.Text = ""
                End If
                txt_ConstRefNo.Text = Trim(dt.Rows(0).Item("ConstituentRefNo") & "")
                txt_ParticipantCode.Text = Trim(dt.Rows(0).Item("ParticipantCode") & "")
                Hid_Exchange.Value = Trim(dt.Rows(0).Item("ExchangeName") & "")
                StrBuyer = Trim(dt.Rows(0).Item("BuyBrokerName") & "")
                StrSeller = Trim(dt.Rows(0).Item("SellBrokerName") & "")



                If StrBuyer.ToUpper.IndexOf("TRUST FINANCIAL") <> -1 Then
                    rdo_ContNotefor.SelectedIndex = 0
                End If
                If StrSeller.ToUpper.IndexOf("TRUST FINANCIAL") <> -1 Then
                    rdo_ContNotefor.SelectedIndex = 1
                End If



                If StrBuyer.ToUpper.IndexOf("TRUST FINANCIAL") <> -1 And StrSeller.ToUpper.IndexOf("TRUST FINANCIAL") <> -1 Then
                    rdo_ContNotefor.Enabled = True
                Else
                    rdo_ContNotefor.Enabled = False
                End If


                Hid_SettlementAmount.Value = Trim(dt.Rows(0).Item("SettlementAmount") & "")
                Hid_BuyBroker.Value = Trim(dt.Rows(0).Item("BuyerBrokerageAmt") & "")
                Hid_SellBroker.Value = Trim(dt.Rows(0).Item("SellerBrokerageAmt") & "")
                If Trim(dt.Rows(0).Item("StampDuty") & "") <> "" Then
                    txt_StampDuty.Text = Trim(dt.Rows(0).Item("StampDuty") & "")
                    txt_ServiceTax.Text = Trim(dt.Rows(0).Item("ServiceTax") & "")
                    txt_Educationcess.Text = Trim(dt.Rows(0).Item("Educationcess") & "")
                    txt_HEducationcess.Text = Trim(dt.Rows(0).Item("HEducationcess") & "")
                End If
                If rdo_ContNotefor.SelectedValue = "B" Then
                    lbl_StampDuty.Text = Trim(dt.Rows(0).Item("PurStampDutyAmt") & "")
                    lbl_ServiceTax.Text = Trim(dt.Rows(0).Item("PurServiceTaxAmt") & "")
                    lbl_Educationcess.Text = Trim(dt.Rows(0).Item("PurEducationcessAmt") & "")
                    lbl_HEducationcess.Text = Trim(dt.Rows(0).Item("PurHEducationcessAmt") & "")
                Else
                    lbl_StampDuty.Text = Trim(dt.Rows(0).Item("SellStampDutyAmt") & "")
                    lbl_ServiceTax.Text = Trim(dt.Rows(0).Item("SellServiceTaxAmt") & "")
                    lbl_Educationcess.Text = Trim(dt.Rows(0).Item("SellEducationcessAmt") & "")
                    lbl_HEducationcess.Text = Trim(dt.Rows(0).Item("SellHEducationcessAmt") & "")
                End If
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

    End Sub
End Class