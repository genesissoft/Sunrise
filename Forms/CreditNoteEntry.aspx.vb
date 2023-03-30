Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_CreditNoteEntry
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
            'If  sqlConn Is Nothing Then objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")

            If IsPostBack = False Then
                SetAttributes()
                SetControls()
                'FilTDS()
                'If Session("usertypeid") = 76 Then
                '    ReadonlyFields()
                'End If
                If Val(Request.QueryString("AppEntryId") & "") <> 0 Then
                    FillApplicationFields()
                    FillTDS()
                End If
                If Val(Request.QueryString("Id") & "") <> 0 Then
                    ViewState("Id") = Val(Request.QueryString("Id") & "")
                    FillFields()

                    Srh_ApplNo.SearchButton.Visible = False
                    Srh_ChannApplNo.SearchButton.Visible = False
                    rbl_Through.Enabled = False
                    btn_ReCalculate.Visible = True

                    btn_Save.Visible = False
                    btn_Update.Visible = True
                Else
                    GetCreditNoteNo()
                End If
            Else

            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "show", "AppType();", True)
            Page.ClientScript.RegisterStartupScript(Me.GetType, "check4", "CheckRateAmt('fnt_Perc1','txt_TotAmount');", True)
            Page.ClientScript.RegisterStartupScript(Me.GetType, "check5", "CheckRateAmt('fnt_Perc2','txt_GrossAmt');", True)
            Page.ClientScript.RegisterStartupScript(Me.GetType, "check6", "CheckRateAmt('fnt_Perc3','txt_TDS');", True)
            Page.ClientScript.RegisterStartupScript(Me.GetType, "check7", "CheckRateAmt('fnt_Perc4','txt_NetAmt');", True)

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
    Private Sub ReadonlyFields()
        Try
            txt_TDS.ReadOnly = False
            txt_TotAmount.ReadOnly = False
            txt_NetAmt.ReadOnly = False
            txt_GrossAmt.ReadOnly = False
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub SetAttributes()

        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")

        txt_CreditDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_CreditDate.Attributes.Add("onblur", "CheckDate(this,false);")


        txt_ChequeDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_ChequeDate.Attributes.Add("onblur", "CheckDate(this,false);")


        txt_TotAmount.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_GrossAmt.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_NetAmt.Attributes.Add("onkeypress", "OnlyDecimal();")
        rbl_Through.Attributes.Add("onclick", "AppType();")

    End Sub
    Private Sub SetControls()

        'Srh_ApplNo.Columns.Add("ApplicationNo")
        'Srh_ApplNo.Columns.Add("InvestorName")
        'Srh_ApplNo.Columns.Add("IssueDesc")
        'Srh_ApplNo.Columns.Add("AppEntryId")

        'Srh_ApplNo.Columns.Add("ApplicationNo")
        'Srh_ApplNo.Columns.Add("AppEntryId")



        'Srh_ChannApplNo.Columns.Add("ApplicationNo")
        'Srh_ChannApplNo.Columns.Add("InvestorName")
        'Srh_ChannApplNo.Columns.Add("IssueDesc")
        'Srh_ChannApplNo.Columns.Add("AppEntryId")


        txt_CreditDate.Text = Format(Today, "dd/MM/yyyy")
        txt_ChequeDate.Text = Format(Today, "dd/MM/yyyy")

    End Sub

    Private Sub FilTDS()

        Dim dt As New DataTable
        Dim sqlda As New SqlDataAdapter
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "MB_FILL_TDSCredit"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@CreditNoteDate", SqlDbType.SmallDateTime, 2, "I", , , objCommon.DateFormat(txt_CreditDate.Text))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(dt)
            If dt.Rows.Count > 0 Then
                txt_TDS.Text = Trim(dt.Rows(0).Item("TDSPercentage") & "")
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub FillApplicationFields()

        Try

            Dim dt As DataTable
            OpenConn()
            If Val(Request.QueryString("AppEntryId") & "") <> 0 Then
                dt = objCommon.FillDataTable(sqlConn, "MB_FILL_CreditAppliFill", Request.QueryString("AppEntryId"), "AppEntryId")
            Else
                If rbl_Through.SelectedValue = "C" Then
                    dt = objCommon.FillDataTable(sqlConn, "MB_FILL_CreditAppliFill", Srh_ChannApplNo.SelectedId, "AppEntryId")
                Else
                    dt = objCommon.FillDataTable(sqlConn, "MB_FILL_CreditAppliFill", Srh_ApplNo.SelectedId, "AppEntryId")
                End If

            End If

            If dt.Rows.Count > 0 Then

                'Hid_ChannelPartnerId.value = Val(dt.Rows(0).Item("ChannelPartnerId").ToString)

                'If Val(dt.Rows(0).Item("ChannelPartnerId").ToString) <> 0 Then
                'rbl_Through.selectdvalue = Trim(dt.Rows(0).Item("Through").ToString)
                '    rbl_Through.Enable = False

                'End If



                If Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "") <> 0 Or Val(dt.Rows(0).Item("ChannelPartInc2Amount") & "") <> 0 Then
                    'Hid_ChannelPartInc1.SelectedValue = Trim(dt.Rows(0).Item("ChannelPartInc1") & "")
                    'Hid_ChannelPartInc1.Text = Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "")

                    'Hid_ChannelPartInc2.SelectedValue = Trim(dt.Rows(0).Item("ChannelPartInc2") & "")
                    'Hid_ChannelPartInc2.Text = Val(dt.Rows(0).Item("ChannelPartInc2Amount") & "")
                End If

                If Val(dt.Rows(0).Item("DiscountAmount") & "") <> 0 Or Val(dt.Rows(0).Item("IncentiveAmt") & "") <> 0 Or Val(dt.Rows(0).Item("IncentiveAmt3") & "") <> 0 Then


                End If

                Srh_ChannApplNo.SearchTextBox.Text = dt.Rows(0).Item("ApplicationNo").ToString
                Srh_ApplNo.SearchTextBox.Text = dt.Rows(0).Item("ApplicationNo").ToString

                txt_ChannelPartnerName.Text = dt.Rows(0).Item("ChannelPartnerName").ToString


                Hid_IssuerId.Value = Val(dt.Rows(0).Item("IssuerId") & "")
                txt_IssuerName.Text = dt.Rows(0).Item("IssuerName").ToString
                Hid_IssueId.Value = Val(dt.Rows(0).Item("IssueId") & "")
                txt_InvestoreName.Text = dt.Rows(0).Item("SchemeName").ToString
                txt_IssueName.Text = dt.Rows(0).Item("IssueDesc").ToString

                Hid_IncentiveType.Value = Trim(dt.Rows(0).Item("IncentiveType") & "")
                Hid_AppAmtQty.Value = Val(dt.Rows(0).Item("AppAmtQty") & "")
                Hid_AppAmtMultiple.Value = Val(dt.Rows(0).Item("AppAmtMultiple") & "")
                Hid_ChannelPartInc1.Value = Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "")
                Hid_IncentiveAmt.Value = Val(dt.Rows(0).Item("IncentiveAmt") & "")
                Hid_TotAllotedAmt.Value = Val(dt.Rows(0).Item("AllotedAmt") & "") * Val(dt.Rows(0).Item("AllotedAmtMultiple") & "") / 10000000


                Hid_Instrument.Value = Trim(dt.Rows(0).Item("Instrument") & "")


                If (Val(dt.Rows(0).Item("IncentiveAmt") & "") <> 0 And Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "") = 0) Then
                    If Trim(dt.Rows(0).Item("IncentiveType") & "") = "R" Then
                        Hid_IncePer.Value = Val(dt.Rows(0).Item("AllotedAmt") & "") * Val(dt.Rows(0).Item("AllotedAmtMultiple") & "") * Val(dt.Rows(0).Item("IncentiveAmt") & "") / 100
                        'txt_TotAmount.Text = (Val(dt.Rows(0).Item("AllotedAmt") & "") * Val(dt.Rows(0).Item("AllotedAmtMultiple") & "") * Val(Hid_IncePer.Value) / 100) / 10000000
                        txt_TotAmount.Text = Val(Hid_IncePer.Value)
                    Else
                        Hid_IncePer.Value = Val(dt.Rows(0).Item("IncentiveAmt") & "")
                        'txt_TotAmount.Text = (Val(dt.Rows(0).Item("AllotedAmt") & "") * Val(dt.Rows(0).Item("AllotedAmtMultiple") & "") * Val(Hid_IncePer.Value))
                        txt_TotAmount.Text = Val(dt.Rows(0).Item("IncentiveAmt") & "")
                    End If
                ElseIf (Val(dt.Rows(0).Item("IncentiveAmt") & "") = 0 And Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "") <> 0) Then

                    If Trim(dt.Rows(0).Item("ChannelPartInc1") & "") = "R" Then
                        Hid_IncePer.Value = Val(dt.Rows(0).Item("AllotedAmt") & "") * Val(dt.Rows(0).Item("AllotedAmtMultiple") & "") * Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "") / 100
                        'txt_TotAmount.Text = (Val(dt.Rows(0).Item("AllotedAmt") & "") * Val(dt.Rows(0).Item("AllotedAmtMultiple") & "") * Val(Hid_IncePer.Value) / 100) / 10000000
                        txt_TotAmount.Text = Val(Hid_IncePer.Value)
                    Else
                        Hid_IncePer.Value = Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "")
                        'txt_TotAmount.Text = (Val(dt.Rows(0).Item("AllotedAmt") & "") * Val(dt.Rows(0).Item("AllotedAmtMultiple") & "") * Val(Hid_IncePer.Value))
                        txt_TotAmount.Text = Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "")
                    End If

                ElseIf (Val(dt.Rows(0).Item("IncentiveAmt") & "") <> 0 And Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "") <> 0) Then
                    'rbl_Through.SelectedValue = Trim(dt.Rows(0).Item("Through").ToString)
                    rbl_Through.Enabled = False
                    If Trim(dt.Rows(0).Item("Through").ToString) = "C" Then
                        If Trim(dt.Rows(0).Item("ChannelPartInc1") & "") = "R" Then
                            Hid_IncePer.Value = Val(dt.Rows(0).Item("AllotedAmt") & "") * Val(dt.Rows(0).Item("AllotedAmtMultiple") & "") * Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "") / 100
                            'txt_TotAmount.Text = (Val(dt.Rows(0).Item("AllotedAmt") & "") * Val(dt.Rows(0).Item("AllotedAmtMultiple") & "") * Val(Hid_IncePer.Value) / 100) / 10000000
                            txt_TotAmount.Text = Val(Hid_IncePer.Value)
                        Else
                            Hid_IncePer.Value = Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "")
                            'txt_TotAmount.Text = (Val(dt.Rows(0).Item("AllotedAmt") & "") * Val(dt.Rows(0).Item("AllotedAmtMultiple") & "") * Val(Hid_IncePer.Value))
                            txt_TotAmount.Text = Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "")
                        End If

                    Else
                        If Trim(dt.Rows(0).Item("IncentiveType") & "") = "R" Then
                            Hid_IncePer.Value = Val(dt.Rows(0).Item("AllotedAmt") & "") * Val(dt.Rows(0).Item("AllotedAmtMultiple") & "") * Val(dt.Rows(0).Item("IncentiveAmt") & "") / 100
                            'txt_TotAmount.Text = (Val(dt.Rows(0).Item("AllotedAmt") & "") * Val(dt.Rows(0).Item("AllotedAmtMultiple") & "") * Val(Hid_IncePer.Value) / 100) / 10000000
                            txt_TotAmount.Text = Val(Hid_IncePer.Value)
                        Else
                            Hid_IncePer.Value = Val(dt.Rows(0).Item("IncentiveAmt") & "")
                            'txt_TotAmount.Text = (Val(dt.Rows(0).Item("AllotedAmt") & "") * Val(dt.Rows(0).Item("AllotedAmtMultiple") & "") * Val(Hid_IncePer.Value))
                            txt_TotAmount.Text = Val(dt.Rows(0).Item("IncentiveAmt") & "")
                        End If


                    End If

                End If



                'txt_TotAmount.Text = (Val(dt.Rows(0).Item("AllotedAmt") & "") * Val(dt.Rows(0).Item("AllotedAmtMultiple") & "") * Val(Hid_IncePer.Value) / 100) / 10000000
                txt_GrossAmt.Text = Val(txt_TotAmount.Text)

            End If

            If (Val(dt.Rows(0).Item("IncentiveAmt") & "") <> 0 And Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "") = 0) Then
                If (dt.Rows(0).Item("Nomenclature") & "") <> "" Then
                    txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_IncentiveAmt.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " in " + (dt.Rows(0).Item("Nomenclature") & "") + " " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                Else
                    txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_IncentiveAmt.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " in " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                End If


            ElseIf (Val(dt.Rows(0).Item("IncentiveAmt") & "") = 0 And Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "") <> 0) Then
                If (dt.Rows(0).Item("Nomenclature") & "") <> "" Then
                    txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_ChannelPartInc1.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " in " + (dt.Rows(0).Item("Nomenclature") & "") + " " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                Else
                    txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_ChannelPartInc1.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " in " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                End If

            ElseIf (Val(dt.Rows(0).Item("IncentiveAmt") & "") <> 0 And Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "") <> 0) Then
                If rbl_Through.SelectedValue = "C" Then
                    If (dt.Rows(0).Item("Nomenclature") & "") <> "" Then
                        txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_ChannelPartInc1.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " in " + (dt.Rows(0).Item("Nomenclature") & "") + " " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                    Else
                        txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_ChannelPartInc1.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " in " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                    End If
                Else
                    If (dt.Rows(0).Item("Nomenclature") & "") <> "" Then
                        txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_IncentiveAmt.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " in " + (dt.Rows(0).Item("Nomenclature") & "") + " " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                    Else
                        txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_IncentiveAmt.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " " + " in " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                    End If
                End If

            End If

        Catch ex As Exception

        Finally
            CloseConn()
        End Try




    End Sub
    Private Sub FillTDS()

        Try
            Dim dt As DataTable
            OpenConn()
            If Val(Request.QueryString("AppEntryId") & "") <> 0 Then
                dt = objCommon.FillDataTable(sqlConn, "MB_FILL_CREDITTDS", Request.QueryString("AppEntryId"), "AppEntryId")
            Else

                If rbl_Through.SelectedValue = "C" Then
                    dt = objCommon.FillDataTable(sqlConn, "MB_FILL_CREDITTDS", Srh_ChannApplNo.SelectedId, "AppEntryId")
                Else
                    dt = objCommon.FillDataTable(sqlConn, "MB_FILL_CREDITTDS", Srh_ApplNo.SelectedId, "AppEntryId")
                End If


            End If
            dt = objCommon.FillDataTable(sqlConn, "MB_FILL_CREDITTDS", Srh_ApplNo.SelectedId, "AppEntryId")
            If dt.Rows.Count > 0 Then
                Hid_TDSRate.Value = Val(dt.Rows(0).Item("TDSPercentage").ToString)

                If rbl_TDSapplicable.SelectedValue = "Y" Then
                    txt_TDS.Text = objCommon.DecimalFormat(Val(txt_GrossAmt.Text)) * objCommon.DecimalFormat(Val(Hid_TDSRate.Value)) / 100
                Else
                    txt_TDS.Text = 0
                End If



            Else
                txt_TDS.Text = 0
            End If

            txt_NetAmt.Text = objCommon.DecimalFormat(Val(txt_GrossAmt.Text)) - objCommon.DecimalFormat(Val(txt_TDS.Text))

        Catch ex As Exception

        Finally
            CloseConn()
        End Try

       
    End Sub

    Protected Sub Srh_ApplNo_ButtonClick() Handles Srh_ApplNo.ButtonClick
        FillApplicationFields()
        FillTDS()
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        GetCreditNoteNo()
        If SetSaveUpdate("MB_INSERT_CreditNoteEntry") = True Then
            If rbl_Through.SelectedValue = "C" Then
                Page.ClientScript.RegisterStartupScript(Me.GetType, "open", "ReportViewForChannel()", True)
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType, "open", "ReportView()", True)
            End If
        End If

    End Sub
    Private Function SetSaveUpdate(ByVal strProc As String, Optional ByVal blnRedirect As Boolean = True) As Boolean
        Try
            OpenConn()
            Dim sqlTrans As SqlTransaction
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Function

            sqlTrans.Commit()
            If blnRedirect = False Then
                Response.Redirect("CreditNoteDetails.aspx?Id=" & ViewState("Id"), False)
            End If
            Return True
            'Response.Redirect("CreditNoteDetails.aspx?Id=" & ViewState("Id"), False)
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
            sqlComm.Parameters.Clear()

            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@CreditNoteId", SqlDbType.BigInt, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@CreditNoteId", SqlDbType.BigInt, 4, "I", , , ViewState("Id"))
            End If
            If Val(Request.QueryString("AppEntryId") & "") <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@AppEntryId", SqlDbType.Int, 4, "I", , , Val(Request.QueryString("AppEntryId")))
            Else

                If rbl_Through.SelectedValue = "C" Then
                    objCommon.SetCommandParameters(sqlComm, "@AppEntryId", SqlDbType.Int, 4, "I", , , Val(Srh_ChannApplNo.SelectedId))
                Else
                    objCommon.SetCommandParameters(sqlComm, "@AppEntryId", SqlDbType.Int, 4, "I", , , Val(Srh_ApplNo.SelectedId))
                End If
            End If
            objCommon.SetCommandParameters(sqlComm, "@InvestorChanPart", SqlDbType.Char, 1, "I", , , Trim(rbl_Through.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@CreditNoteDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_CreditDate.Text))
            objCommon.SetCommandParameters(sqlComm, "@NetAmt", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_NetAmt.Text)))
            objCommon.SetCommandParameters(sqlComm, "@GrossAmt", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_GrossAmt.Text)))
            objCommon.SetCommandParameters(sqlComm, "@TDSAmt", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_TDS.Text)))
            objCommon.SetCommandParameters(sqlComm, "@Remark", SqlDbType.VarChar, 1000, "I", , , Trim(txt_Remark.Text))
            objCommon.SetCommandParameters(sqlComm, "@CreditNoteNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_CreditNoteNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@TotalAmount", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(Val(txt_TotAmount.Text)))
            objCommon.SetCommandParameters(sqlComm, "@CompanyId", SqlDbType.Int, 4, "I", , , Session("CompanyId"))
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Session("YearId"))
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Session("CompId"))
            objCommon.SetCommandParameters(sqlComm, "@ChequeNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_ChequeNo.Text))
            If txt_ChequeDate.Text <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@ChequeDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ChequeDate.Text))

            End If
            objCommon.SetCommandParameters(sqlComm, "@DrawnOn", SqlDbType.VarChar, 50, "I", , , Trim(txt_DrawnOn.Text))

            objCommon.SetCommandParameters(sqlComm, "@TDSapplicable", SqlDbType.Char, 1, "I", , , Trim(rbl_TDSapplicable.SelectedValue))

            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@CreditNoteId").Value
            Hid_CreditNoteId.Value = ViewState("Id")
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        If SetSaveUpdate("MB_UPDATE_CreditNoteEntry") = True Then
            If rbl_Through.SelectedValue = "C" Then
                Page.ClientScript.RegisterStartupScript(Me.GetType, "open", "ReportViewForChannel()", True)
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType, "open", "ReportView()", True)
            End If
        End If

    End Sub
    Private Sub FillFields()

        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "MB_FILL_CreditNoteEntry", ViewState("Id"), "CreditNoteId")
            If dt.Rows.Count > 0 Then
                rbl_Through.SelectedValue = Trim(dt.Rows(0).Item("InvestorChanPart").ToString)

                If rbl_Through.SelectedValue = "C" Then
                    Srh_ChannApplNo.SelectedId = Val(dt.Rows(0).Item("AppEntryId") & "")
                    Srh_ChannApplNo.SearchTextBox.Text = dt.Rows(0).Item("ApplicationNo").ToString
                Else
                    Srh_ApplNo.SelectedId = Val(dt.Rows(0).Item("AppEntryId") & "")
                    Srh_ApplNo.SearchTextBox.Text = dt.Rows(0).Item("ApplicationNo").ToString
                End If
                txt_CreditDate.Text = Format(dt.Rows(0).Item("CreditNoteDate"), "dd/MM/yyyy")
                txt_CreditNoteNo.Text = Trim(dt.Rows(0).Item("CreditNoteNo") & "")
                If Trim(dt.Rows(0).Item("ChequeDate") & "") <> "" Then
                    txt_ChequeDate.Text = Trim(dt.Rows(0).Item("ChequeDate") & "")
                End If
                txt_ChequeDate.Text = Trim(dt.Rows(0).Item("ChequeDate") & "")
                txt_ChequeNo.Text = dt.Rows(0).Item("ChequeNo").ToString
                txt_DrawnOn.Text = Trim(dt.Rows(0).Item("DrawnOn") & "")

                Hid_IssuerId.Value = Val(dt.Rows(0).Item("IssuerId") & "")
                txt_IssuerName.Text = dt.Rows(0).Item("IssuerName").ToString
                Hid_IssueId.Value = Val(dt.Rows(0).Item("IssueId") & "")
                txt_InvestoreName.Text = dt.Rows(0).Item("SchemeName").ToString
                txt_IssueName.Text = dt.Rows(0).Item("IssueDesc").ToString
                txt_TDS.Text = Val(dt.Rows(0).Item("TDSAmt") & "")
                txt_TotAmount.Text = Val(dt.Rows(0).Item("TotalAmount") & "")
                txt_NetAmt.Text = Val(dt.Rows(0).Item("NetAmt") & "")
                txt_GrossAmt.Text = Val(dt.Rows(0).Item("GrossAmt") & "")
                Hid_IncentiveAmt.Value = Val(dt.Rows(0).Item("IncentiveAmt") & "")
                Hid_ChannelPartInc1.Value = Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "")
                rbl_TDSapplicable.SelectedValue = Trim(dt.Rows(0).Item("TDSapplicable").ToString)
                Hid_TotAllotedAmt.Value = Val(dt.Rows(0).Item("AllotedAmt") & "") * Val(dt.Rows(0).Item("AllotedAmtMultiple") & "") / 10000000
                If (Val(dt.Rows(0).Item("IncentiveAmt") & "") <> 0 And Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "") = 0) Then
                    If (dt.Rows(0).Item("Nomenclature") & "") <> "" Then
                        txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_IncentiveAmt.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " in " + (dt.Rows(0).Item("Nomenclature") & "") + " " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                    Else
                        txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_IncentiveAmt.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " in " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                    End If


                ElseIf (Val(dt.Rows(0).Item("IncentiveAmt") & "") = 0 And Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "") <> 0) Then
                    If (dt.Rows(0).Item("Nomenclature") & "") <> "" Then
                        txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_ChannelPartInc1.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " in " + (dt.Rows(0).Item("Nomenclature") & "") + " " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                    Else
                        txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_ChannelPartInc1.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " in " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                    End If

                ElseIf (Val(dt.Rows(0).Item("IncentiveAmt") & "") <> 0 And Val(dt.Rows(0).Item("ChannelPartInc1Amount") & "") <> 0) Then
                    If rbl_Through.SelectedValue = "C" Then
                        If (dt.Rows(0).Item("Nomenclature") & "") <> "" Then
                            txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_ChannelPartInc1.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " in " + (dt.Rows(0).Item("Nomenclature") & "") + " " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                        Else
                            txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_ChannelPartInc1.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " in " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                        End If


                    Else
                        If (dt.Rows(0).Item("Nomenclature") & "") <> "" Then
                            txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_IncentiveAmt.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " in " + (dt.Rows(0).Item("Nomenclature") & "") + " " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                        Else
                            txt_Remark.Text = "Being refund of incentive" + " @ " + Hid_IncentiveAmt.Value + " % for your investment of Rs. " + Hid_TotAllotedAmt.Value + " Crores" + " " + " in " + StrConv(txt_IssueName.Text, VbStrConv.ProperCase) + " Issue."
                        End If


                    End If


                End If



                'FillApplicationFields()
                FillTDS()
                'txt_Remark.Text = Trim(dt.Rows(0).Item("Remark") & "")

            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try


    End Sub

    Protected Sub btn_Back_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Back.Click
        Try
            Response.Redirect("CreditNoteDetails.aspx", False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub GetCreditNoteNo()
        Try
            Dim CreditNoteNo As String
            Dim fstyr As String
            Dim lstyr As String
            Dim yr As String
            fstyr = Mid(Session("Year"), 3, 2)
            lstyr = Mid(Session("Year"), 8, 3)
            yr = fstyr + "-" + lstyr
            OpenConn()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "MB_FILL_DebitNoteNo_craditnote"
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@NextNo", SqlDbType.VarChar, 50, "O")
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Session("YearId"))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Hid_CreditNoteNo.Value = Trim(sqlComm.Parameters("@NextNo").Value & "")
            CreditNoteNo = "T" + "/" + "MB" + "/" + "CN" + "/" + yr + "/" + Hid_CreditNoteNo.Value
            txt_CreditNoteNo.Text = CreditNoteNo
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub Srh_ChannApplNo_ButtonClick() Handles Srh_ChannApplNo.ButtonClick
        FillApplicationFields()
        FillTDS()
    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub rbl_TDSapplicable_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbl_TDSapplicable.SelectedIndexChanged
        Try
            FillTDS()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

        End Try

    End Sub

    Protected Sub btn_ReCalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ReCalculate.Click
        Try
            FillApplicationFields()
            FillTDS()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

        End Try
    End Sub
End Class
