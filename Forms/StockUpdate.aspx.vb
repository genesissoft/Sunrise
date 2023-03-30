Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_StockUpdate
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim arrIssueDetailIds() As String
    Dim dblTotalFaceValue As Decimal
    Dim intUserId As Integer
    Dim sqlConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
         
            Page.Form.DefaultButton = btn_Save.UniqueID
            If IsPostBack = False Then
                SetAttributes()
                SetTimeCombos()
                SetControls()
                If Val(Request.QueryString("Id") & "") <> 0 Then
                    ViewState("Id") = Val(Request.QueryString("Id") & "")
                    FillFields()
                    btn_Save.Visible = False
                    If intUserId = Val(Session("UserId")) Or Val(intUserId) = 0 Then
                        btn_Update.Visible = True
                    Else
                        btn_Update.Visible = False
                    End If
                Else
                    btn_Save.Visible = True
                    btn_Update.Visible = False
                    btn_GenerateDealSlip.Visible = False
                End If
            Else
                yld_Calc.SecurityId = Val(srh_NameofSecurity.SelectedId)
                If Val(srh_NameofSecurity.SelectedId) <> 0 Then
                    FillSecurityDetails()
                End If
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
        If sqlConn.State = ConnectionState.Open Then
            sqlConn.Close()
            sqlConn = Nothing
        End If
    End Sub

    Private Sub SetAttributes()

        txt_Date.Attributes.Add("onkeypress", "OnlyDate();")
        txt_ValidTill.Attributes.Add("onkeypress", "OnlyDate();")
        btn_ShowSecurity.Attributes.Add("onclick", "return ShowSecurityMaster();")
        btn_Refer.Attributes.Add("onclick", "return Refer();")
        txt_Date.Text = Format(Now, "dd/MM/yyyy hh:mm tt")
        txt_ValidTill.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
        txt_DealerName.Text = Session("NameOfUser")
    End Sub

    Private Sub SetControls()
        Try
            'srh_IssuerOfSecurity.Columns.Add("SecurityIssuer")
            'srh_IssuerOfSecurity.Columns.Add("SecurityIssuer")

            'srh_NameofSecurity.Columns.Add("SecurityName")
            'srh_NameofSecurity.Columns.Add("SecurityTypeName")
            'srh_NameofSecurity.Columns.Add("SecurityIssuer")
            'srh_NameofSecurity.Columns.Add("SecurityId")
            OpenConn()
            objCommon.FillControl(cbo_SecurityType, sqlConn, "ID_FILL_SecurityTypeMaster1", "SecurityTypeName", "SecurityTypeId")

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub


    Private Sub FillFields()
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_StockUpdateMaster", ViewState("Id"), "StockUpdtId")
            If dt.Rows.Count > 0 Then
                cbo_SecurityType.SelectedValue = Val(dt.Rows(0).Item("SecurityTypeId") & "")

                txt_DealerName.Text = Trim(dt.Rows(0).Item("NameOfUser") & "")
                txt_Remark.Text = Trim(dt.Rows(0).Item("Remark") & "")
                If dt.Rows(0).Item("StockDate").ToString() <> "" Then
                    txt_Date.Text = Format(CDate(dt.Rows(0).Item("StockDate").ToString()), "dd/MM/yyyy hh:mm tt")
                End If
                Hid_Rate.Value = (dt.Rows(0).Item("Rate") & "")
                srh_NameofSecurity.SelectedId = Val(dt.Rows(0).Item("SecurityId") & "")
                yld_Calc.RdoIPCalc.SelectedValue = (dt.Rows(0).Item("Equal_Actual_Flag") & "")
                yld_Calc.RdoRateActual.SelectedValue = (dt.Rows(0).Item("Rate_Actual_Flag") & "")
                yld_Calc.RdoPhysicalDMAT.SelectedValue = (dt.Rows(0).Item("Physical_DMAT_Flag") & "")
                yld_Calc.RdoSemiAnn.SelectedValue = (dt.Rows(0).Item("Semi_Ann_Flag") & "")
                yld_Calc.TextBoxYTMDate.Text = (dt.Rows(0).Item("YTMDate") & "")
                yld_Calc.TextBoxRate.Text = (dt.Rows(0).Item("Rate") & "")
                yld_Calc.TextBoxFaceValue.Text = (dt.Rows(0).Item("FaceValue") & "")
                yld_Calc.cboFaceValue.SelectedValue = (dt.Rows(0).Item("Multiple") & "")
                yld_Calc.TextBoxYTMSemi.Text = (dt.Rows(0).Item("YTMSemi") & "")
                yld_Calc.TextBoxYTMAnn.Text = (dt.Rows(0).Item("YTMAnn") & "")
                yld_Calc.TextBoxYTCSemi.Text = (dt.Rows(0).Item("YTCSemi") & "")
                yld_Calc.TextBoxYTCAnn.Text = (dt.Rows(0).Item("YTCAnn") & "")
                yld_Calc.TextBoxYTPSemi.Text = (dt.Rows(0).Item("YTPSemi") & "")
                yld_Calc.TextBoxYTPAnn.Text = (dt.Rows(0).Item("YTPAnn") & "")
                yld_Calc.RdoDays.SelectedValue = (dt.Rows(0).Item("IntDays") & "")
                yld_Calc.RdoDaysOptions.SelectedValue = (dt.Rows(0).Item("FirstYrAllYr") & "")
                intUserId = Val(dt.Rows(0).Item("UserId") & "")
                Hid_UserId.Value = Val(dt.Rows(0).Item("UserId") & "")
                If Val(dt.Rows(0).Item("CombineIPMat")) = 0 Then
                    yld_Calc.chkCombineIPMat.Checked = False
                Else
                    yld_Calc.chkCombineIPMat.Checked = True
                End If
                If (dt.Rows(0).Item("Ytm_Mat_Call_Put_Flag") & "") = "Y" Then
                    yld_Calc.RdoYield.Checked = True
                ElseIf (dt.Rows(0).Item("Ytm_Mat_Call_Put_Flag") & "") = "M" Then
                    yld_Calc.RdoMatToRate.Checked = True
                ElseIf (dt.Rows(0).Item("Ytm_Mat_Call_Put_Flag") & "") = "C" Then
                    yld_Calc.RdoMatCallToRate.Checked = True
                ElseIf (dt.Rows(0).Item("Ytm_Mat_Call_Put_Flag") & "") = "P" Then
                    yld_Calc.RdoPutToRate.Checked = True
                End If

                yld_Calc.SecurityId = srh_NameofSecurity.SelectedId
                FillSecurityDetails()

            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Sub FillSecurityDetails()
        Try
            Dim dt As DataTable
            OpenConn()

            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityMaster1", srh_NameofSecurity.SelectedId, "SecurityId")
            If dt.Rows.Count > 0 Then
                cbo_SecurityType.SelectedValue = Trim(dt.Rows(0).Item("SecurityTypeId").ToString)
                srh_IssuerOfSecurity.SearchTextBox.Text = dt.Rows(0).Item("SecurityIssuer").ToString

                srh_NameofSecurity.SearchTextBox.Text = dt.Rows(0).Item("SecurityName").ToString
                srh_IssuerOfSecurity.SelectedFieldText = dt.Rows(0).Item("SecurityIssuer").ToString

            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        SetSaveUpdate("ID_INSERT_StockUpdateMaster")
    End Sub
    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        If Hid_UserId.Value <> Session("UserId") Then
            If Hid_Rate.Value < yld_Calc.TextBoxRate.Text Then
                Dim msg As String = "Rate cannot be greater than existing one"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
        End If
        SetSaveUpdate("ID_UPDATE_StockUpdateMaster")
    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            sqlTrans.Commit()
            Response.Redirect("StockUpdateDetail.aspx?Id=" & ViewState("Id"), False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            'CHANGE 
            Dim sqlComm As New SqlCommand
            Dim datValid As Date
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            sqlComm.Parameters.Clear()
            datValid = objCommon.DateFormat(txt_ValidTill.Text) & " " & cbo_hr.SelectedValue & ":" & cbo_minute.SelectedValue & " " & cbo_ampm.SelectedValue
            dblTotalFaceValue = Val(yld_Calc.TextBoxFaceValue.Text) * Val(yld_Calc.cboFaceValue.SelectedValue)
            Hid_TotalValue.Value = dblTotalFaceValue / Val(Hid_FaceValue.Value)
            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@StockUpdtId", SqlDbType.Int, 2, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@StockUpdtId", SqlDbType.Int, 2, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , Val(srh_NameofSecurity.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@FaceValue", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(yld_Calc.TextBoxFaceValue.Text))
            objCommon.SetCommandParameters(sqlComm, "@Multiple", SqlDbType.Int, 4, "I", , , Val(yld_Calc.cboFaceValue.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@Qty", SqlDbType.Decimal, 9, "I", , , Val(Hid_TotalValue.Value))
            If txt_Date.Text <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@StockDate", SqlDbType.SmallDateTime, SqlDbType.SmallDateTime, "I", , , Now)
            End If
            objCommon.SetCommandParameters(sqlComm, "@Remark", SqlDbType.VarChar, 100, "I", , , Trim(txt_Remark.Text))
           
            If yld_Calc.RdoYield.Checked = True Then
                objCommon.SetCommandParameters(sqlComm, "@Ytm_Mat_Call_Put_Flag", SqlDbType.Char, 1, "I", , , "Y")
            ElseIf yld_Calc.RdoMatToRate.Checked = True Then
                objCommon.SetCommandParameters(sqlComm, "@Ytm_Mat_Call_Put_Flag", SqlDbType.Char, 1, "I", , , "M")
            ElseIf yld_Calc.RdoMatCallToRate.Checked = True Then
                objCommon.SetCommandParameters(sqlComm, "@Ytm_Mat_Call_Put_Flag", SqlDbType.Char, 1, "I", , , "C")
            ElseIf yld_Calc.RdoPutToRate.Checked = True Then
                objCommon.SetCommandParameters(sqlComm, "@Ytm_Mat_Call_Put_Flag", SqlDbType.Char, 1, "I", , , "P")
            End If

            objCommon.SetCommandParameters(sqlComm, "@Equal_Actual_Flag", SqlDbType.Char, 1, "I", , , yld_Calc.RdoIPCalc.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@Rate_Actual_Flag", SqlDbType.Char, 1, "I", , , yld_Calc.RdoRateActual.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@Physical_DMAT_Flag", SqlDbType.Char, 1, "I", , , Trim(yld_Calc.RdoPhysicalDMAT.SelectedValue) & "")
            objCommon.SetCommandParameters(sqlComm, "@YTM_XIRR_MMY_Flag", SqlDbType.Char, 1, "I", , , (yld_Calc.RdoYXM.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@Semi_Ann_Flag", SqlDbType.Char, 1, "I", , , yld_Calc.RdoSemiAnn.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@Rate", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(yld_Calc.TextBoxRate.Text))
            objCommon.SetCommandParameters(sqlComm, "@YTMDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(yld_Calc.TextBoxYTMDate.Text))
            If Val(yld_Calc.TextBoxYTMSemi.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@YTMSemi", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(yld_Calc.TextBoxYTMSemi.Text))
            End If
            If Val(yld_Calc.TextBoxYTMAnn.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@YTMAnn", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(yld_Calc.TextBoxYTMAnn.Text))
            End If
            If Val(yld_Calc.TextBoxYTCSemi.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@YTCSemi", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(yld_Calc.TextBoxYTCSemi.Text))
            End If

            If Val(yld_Calc.TextBoxYTCAnn.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@YTCAnn", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(yld_Calc.TextBoxYTCAnn.Text))
            End If
            If Val(yld_Calc.TextBoxYTPSemi.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@YTPSemi", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(yld_Calc.TextBoxYTPSemi.Text))
            End If
            If Val(yld_Calc.TextBoxYTPAnn.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@YTPAnn", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat(yld_Calc.TextBoxYTPAnn.Text))
            End If
            If yld_Calc.chkCombineIPMat.Checked = True Then
                objCommon.SetCommandParameters(sqlComm, "@CombineIPMat", SqlDbType.Bit, 1, "I", , , 1)
            Else
                objCommon.SetCommandParameters(sqlComm, "@CombineIPMat", SqlDbType.Bit, 1, "I", , , 0)
            End If

            objCommon.SetCommandParameters(sqlComm, "@IntDays", SqlDbType.Int, 4, "I", , , yld_Calc.RdoDays.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@FirstYrAllYr", SqlDbType.Char, 1, "I", , , yld_Calc.RdoDaysOptions.SelectedValue)

            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@StockUpdtId").Value
            Hid_QuoteId.Value = ViewState("Id")

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            Response.Redirect("StockUpdateDetail.aspx", False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    Private Sub SetTimeCombos()
        Try
            Dim i As Integer
            cbo_hr.Items.Clear()
            cbo_minute.Items.Clear()
            For i = 1 To 12
                If i < 10 Then
                    cbo_hr.Items.Add(New ListItem(Trim("0" & i), Trim("0" & i)))
                Else
                    cbo_hr.Items.Add(New ListItem(Val(i), Val(i)))
                End If
            Next
            'cbo_hr.SelectedValue = 12
            For i = 0 To 59 Step 15
                If i < 10 Then
                    cbo_minute.Items.Add(New ListItem(Trim("0" & i), Trim("0" & i)))
                Else
                    cbo_minute.Items.Add(New ListItem(Val(i), Val(i)))
                End If
            Next
            Dim lstHr = cbo_hr.Items.FindByText("06")
            If Not lstHr Is Nothing Then cbo_hr.SelectedValue = lstHr.Value

            Dim lstampm = cbo_ampm.Items.FindByText("PM")
            If Not lstampm Is Nothing Then cbo_ampm.SelectedValue = lstampm.Value

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub Srh_NameofSecurity_ButtonClick() Handles srh_NameofSecurity.ButtonClick
        Try
            FillSecurityDetails()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_GenerateDealSlip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_GenerateDealSlip.Click
        Try
            Hid_PageName.Value = "QuoteEntry.aspx"
            Response.Redirect("DealSlipEntry.aspx?SecurityId=" & Val(srh_NameofSecurity.SelectedId) & "&PageName=" & Hid_PageName.Value & "&StockUpdtId=" & Val(ViewState("Id")) & "&Rate=" & yld_Calc.TextBoxRate.Text & "&FaceValue=" & yld_Calc.TextBoxFaceValue.Text & "&Multiple=" & yld_Calc.cboFaceValue.SelectedValue, False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub cbo_SecurityType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SecurityType.SelectedIndexChanged
        srh_IssuerOfSecurity.SearchTextBox.Text = ""
        srh_IssuerOfSecurity.SelectedId = ""
        srh_NameofSecurity.SearchTextBox.Text = ""
        srh_NameofSecurity.SelectedId = ""
    End Sub

    Protected Sub srh_IssuerOfSecurity_ButtonClick() Handles srh_IssuerOfSecurity.ButtonClick
        srh_NameofSecurity.SearchTextBox.Text = ""
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

 


End Class
