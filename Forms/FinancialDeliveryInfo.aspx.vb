Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_FinancialDeliveryInfo
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlComm As New SqlCommand
    Dim strValues() As String
    Dim AdjDealSlipId As String
    Dim strFlag As String
    Dim sqlConn As SqlConnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        Try
            If IsPostBack = False Then

                SetAttributes()
                If (Request.QueryString("CustomerId") & "") <> "" Then
                    Hid_CustomerId.Value = Val(Request.QueryString("CustomerId") & "")
                End If
                If (Request.QueryString("DealSlipId") & "") <> "" Then
                    Hid_DealSlipId.Value = Val(Request.QueryString("DealSlipId") & "")

                End If
                If (Request.QueryString("TransType") & "") <> "" Then
                    Hid_TransType.Value = (Request.QueryString("TransType") & "")
                End If
                If (Request.QueryString("BalAmt") & "") <> "" Then
                    txt_Amount.Text = Val(Request.QueryString("BalAmt") & "")
                    txt_Amount.Text = Format(Val(txt_Amount.Text), "############0.00")
                    Hid_BalAmt.Value = Val(Request.QueryString("BalAmt") & "")
                    Hid_BalAmt.Value = Format(Val(Hid_BalAmt.Value), "############0.00")

                End If
                If Trim(Request.QueryString("Values") & "") <> "" Then

                    btn_Clear.Visible = False
                    cbo_FDType.Enabled = False
                    strValues = Split(Trim(Request.QueryString("Values") & ""), "!")
                    cbo_FDType.SelectedValue = strValues(0)
                    txt_Amount.Text = strValues(2)
                    txt_Amount.Text = Format(Val(txt_Amount.Text), "############0.00")
                    If strValues(3) <> "" Then
                        txt_Remark.Text = (strValues(3))
                    End If
                    If strValues(0) = "N" Then
                        txt_PaymntDate.Text = strValues(4)
                        If strValues(11) = "P" Then
                            cbo_Bank.SelectedItem.Text = strValues(5)
                        Else
                            txt_BankName.Text = strValues(5)
                        End If

                        txt_ChqNo.Text = strValues(7)
                        txt_ChequeDate.Text = strValues(8)
                    End If
                    Hid_CustomerId.Value = strValues(9)
                    Hid_DealSlipId.Value = strValues(9)
                    AdjDealSlipId = strValues(10)
                    Hid_BalAmt.Value = (Val(strValues(10)) - 0) + (Val(strValues(1)) - 0)
                    Hid_TransType.Value = strValues(1)

                End If
                FillTransAdjstGrid()
                If AdjDealSlipId <> "" Then
                    Checked(AdjDealSlipId)
                End If
            End If

            Dim strHtml As String
            strHtml = "SelectRow();"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "display", strHtml, True)
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
        Try
            cbo_FDType.Attributes.Add("onchange", "SelectRow();")
            btn_SaveInfo.Attributes.Add("onclick", "return Validation();")
            txt_PaymntDate.Attributes.Add("onkeypress", "OnlyDate();")
            txt_PaymntDate.Attributes.Add("onblur", "CheckDate(this,false);")
            txt_PaymntDate.Text = Format(Now, "dd/MM/yyyy")
            txt_PaymntDate.Attributes.Add("onblur", "CheckDate(this,false);")
            txt_ChequeDate.Attributes.Add("onkeypress", "OnlyDate();")
            txt_ChequeDate.Attributes.Add("onblur", "CheckDate(this,false);")
            txt_Amount.Attributes.Add("onkeypress", "OnlyDecimal();")
            'txt_ChqNo.Attributes.Add("onkeypress", "OnlyInteger();")
            txt_Remark.Attributes.Add("onblur", "ConvertUCase(this)")
            txt_BankName.Attributes.Add("onblur", "ConvertUCase(this)")
            OpenConn()
            objCommon.FillControl(cbo_Bank, sqlConn, "ID_FILL_BankMaster", "BankName", "BankId", Session("CompId"), "CompId")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub FillTransAdjstGrid()
        Try
            Dim sqlda As New SqlDataAdapter
            Dim sqldt As New DataTable
            Dim sqldv As New DataView
            OpenConn()
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "Id_FILL_TransAdjustment"
            sqlComm.Parameters.Clear() 
            objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , Val(Hid_CustomerId.Value))
            objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(Hid_DealSlipId.Value))
            objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(sqldt)
            Session("TransAdjustment") = sqldt
            dg_AdjstTrans.DataSource = Session("TransAdjustment")
            dg_AdjstTrans.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub btn_SaveInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_SaveInfo.Click
        Try
            If cbo_FDType.SelectedValue = "A" Then
                For i As Int16 = 0 To dg_AdjstTrans.Items.Count - 1
                    Dim chk As CheckBox
                    chk = CType(dg_AdjstTrans.Items(i).FindControl("chk_Select"), CheckBox)
                    If chk.Checked = True Then
                        Hid_AdjDealSlipId.Value += Val(CType(dg_AdjstTrans.Items(i).FindControl("lbl_DealSlipId"), Label).Text) & ","
                    End If
                Next
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "close", "RetValues();", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function Checked(ByVal strDealSlipIds As String)
        Try
            Dim dgItem As DataGridItem
            Dim J As Int16
            Dim intDealId As Int16
            Dim strIds() As String
            Dim chk As CheckBox
            strIds = Split(strDealSlipIds, ",")
            For I As Int16 = 0 To strIds.Length - 1
                If strIds(I) <> "" Then
                    For J = 0 To dg_AdjstTrans.Items.Count - 1
                        dgItem = dg_AdjstTrans.Items(J)
                        intDealId = Val(CType(dgItem.FindControl("lbl_DealSlipId"), Label).Text)
                        If intDealId = strIds(I) Then
                            chk = CType(dgItem.FindControl("chk_Select"), CheckBox)
                            chk.Checked = True
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Clear.Click
        cbo_FDType.SelectedIndex = 0
        txt_PaymntDate.Text = ""
        txt_Remark.Text = ""
        txt_BankName.Text = ""
        txt_Amount.Text = ""
        txt_ChqNo.Text = ""
        txt_ChequeDate.Text = ""
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
End Class
