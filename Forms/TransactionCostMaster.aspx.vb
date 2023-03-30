Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_TransactionCostMaster
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As New SqlConnection
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
            If IsPostBack = False Then
                SetAttributes()
                If Request.QueryString("Id") <> "" Then
                    Dim strId As String = objCommon.DecryptText(HttpUtility.UrlDecode(Request.QueryString("Id")))
                    ViewState("Id") = Val(strId)
                    FillFields()
                    btn_Save.Visible = False
                    btn_Update.Visible = True
                Else
                    btn_Save.Visible = True
                    btn_Update.Visible = False
                End If
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "DMAT", "CheckPhysicalDMAT();", True)
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
            OpenConn()
            rdo_PhysicalDMAT.Attributes.Add("onclick", "CheckPhysicalDMAT();")
            txt_HCAmount.Attributes.Add("onkeypress", "OnlyDecimal();")
            txt_IDAmount.Attributes.Add("onkeypress", "OnlyDecimal();")
            txt_FromDate.Attributes.Add("onkeypress", "OnlyDate();")
            btn_Save.Attributes.Add("onclick", "return Validation();")
            btn_Update.Attributes.Add("onclick", "return Validation();")
            If rdo_PhysicalDMAT.SelectedValue = "D" Then
                objCommon.FillControl(cbo_Bank, sqlConn, "ID_FILL_BankMaster1", "BankAccInfo", "BankId", Val(Session("CompId")), "CompId")
            Else
                objCommon.FillControl(cbo_SGLWith, sqlConn, "ID_FILL_SGLBankMaster1", "BankName", "SGLId", Val(Session("CompId")), "CompId")
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally

            CloseConn()
        End Try
       
    End Sub

    Private Sub FillFields()
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_TransCostMaster", ViewState("Id"), "TransactionCostId")
            If dt.Rows.Count > 0 Then
                txt_FromDate.Text = Format(dt.Rows(0).Item("FromDate"), "dd/MM/yyyy")
                rdo_PhysicalDMAT.SelectedValue = Trim(dt.Rows(0).Item("DeliveryMode") & "")
                txt_HCAmount.Text = Val(dt.Rows(0).Item("HCostAmount") & "")
                txt_IDAmount.Text = Val(dt.Rows(0).Item("IntraDayAmount") & "")
                rdo_SecurityType.SelectedValue = Trim(dt.Rows(0).Item("SecurityType") & "")
                If rdo_PhysicalDMAT.SelectedValue = "D" Then
                    objCommon.FillControl(cbo_Bank, sqlConn, "ID_FILL_BankMaster1", "BankAccInfo", "BankId", Val(Session("CompId")), "CompId")
                    cbo_Bank.SelectedValue = Val(dt.Rows(0).Item("TCBankId") & "")
                Else
                    objCommon.FillControl(cbo_SGLWith, sqlConn, "ID_FILL_SGLBankMaster1", "BankName", "SGLId", Val(Session("CompId")), "CompId")
                    cbo_SGLWith.SelectedValue = Val(dt.Rows(0).Item("TCBankId") & "")
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
        
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            SetSaveUpdate("ID_INSERT_TransCostMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            sqlTrans.Commit()
            Response.Redirect("TransactionCostDetails.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn

            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@TransactionCostId", SqlDbType.Int, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@TransactionCostId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 9, "I", , , objCommon.DateFormat(txt_FromDate.Text))
            If rdo_PhysicalDMAT.SelectedValue = "D" Then
                If Val(cbo_Bank.SelectedValue) <> 0 Then
                    objCommon.SetCommandParameters(sqlComm, "@TCbankId", SqlDbType.Int, 4, "I", , , Val(cbo_Bank.SelectedValue))
                End If
            Else
                If Val(cbo_SGLWith.SelectedValue) <> 0 Then
                    objCommon.SetCommandParameters(sqlComm, "@TCbankId", SqlDbType.Int, 4, "I", , , Val(cbo_SGLWith.SelectedValue))
                End If
            End If

            objCommon.SetCommandParameters(sqlComm, "@DeliveryMode", SqlDbType.Char, 1, "I", , , Trim(rdo_PhysicalDMAT.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@SecurityType", SqlDbType.Char, 1, "I", , , Trim(rdo_SecurityType.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@HCostAmount", SqlDbType.Decimal, 8, "I", , , Val(txt_HCAmount.Text))
            objCommon.SetCommandParameters(sqlComm, "@IntraDayAmount", SqlDbType.Decimal, 8, "I", , , Val(txt_IDAmount.Text))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@TransactionCostId").Value

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        Try
            OpenConn()
            SetSaveUpdate("ID_UPDATE_TransCostMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("TransactionCostDetails.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("TransactionCostDetails.aspx")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            sqlConn.Dispose()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub rdo_PhysicalDMAT_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdo_PhysicalDMAT.SelectedIndexChanged
        Try
            OpenConn()
            If rdo_PhysicalDMAT.SelectedValue = "D" Then
                objCommon.FillControl(cbo_Bank, sqlConn, "ID_FILL_BankMaster1", "BankAccInfo", "BankId", Val(Session("CompId")), "CompId")
            Else
                objCommon.FillControl(cbo_SGLWith, sqlConn, "ID_FILL_SGLBankMaster1", "BankName", "SGLId", Val(Session("CompId")), "CompId")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
End Class
