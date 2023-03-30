Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_GroupMaster
    Inherits System.Web.UI.Page
    Dim sqlConn As New SqlConnection
    Dim objCommon As New clsCommonFuns

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
            'srh_Customers.SelectCheckbox.Visible = True
            'srh_Customers.SelectCheckbox.Checked = True
            'srh_Customers.SelectLinkButton.Enabled = True
            ''Response.Filter = New WhitespaceFilter(Response.Filter)
            If IsPostBack = False Then
                SetAttributes()
                Page.SetFocus(txt_GroupName)
                If Request.QueryString("Id") <> "" Then
                    Dim strId As String = objCommon.DecryptText(HttpUtility.UrlDecode(Request.QueryString("Id")))
                    ViewState("Id") = Val(strId)
                    FillFields()
                    txt_GroupName.Focus()
                    btn_Save.Visible = False
                    btn_Update.Visible = True
                End If
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
        Try
            OpenConn()
            txt_GroupName.Attributes.Add("onblur", "ConvertUCase(this);")
            btn_Save.Attributes.Add("onclick", "return validation();")
            btn_Update.Attributes.Add("onclick", "return validation();")
            btn_Update.Visible = False
            objCommon.FillControl(cbo_CustomerType, sqlConn, "ID_FILL_CustomerTypeMaster", "CustomerTypeName", "CustomerTypeId")
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
            Dim dtGrid As DataTable
            Dim lstItem As ListItem
            dtGrid = objCommon.FillDataTable(sqlConn, "Id_FILL_GroupMaster", ViewState("Id"), "GroupId")
            objCommon.FillControl(cbo_CustomerType, sqlConn, "ID_FILL_CustomerTypeMaster", "CustomerTypeName", "CustomerTypeId")
            'dt = objCommon.FillControl(srh_Customers.SelectListBox, sqlConn, "ID_FILL_GroupCustomers", "CustomerName", "CustomerId", ViewState("Id"), "GroupId")
            If dtGrid.Rows.Count > 0 Then
                txt_GroupCode.Text = Trim(dtGrid.Rows(0).Item("GroupCode") & "")
                txt_GroupName.Text = Trim(dtGrid.Rows(0).Item("GroupName") & "")
                cbo_CustomerType.SelectedValue = Val(dtGrid.Rows(0).Item("CustomerTypeId") & "")
                rdo_HideShow.SelectedValue = Trim(dtGrid.Rows(0).Item("HideShow") & "")
            End If
            'lstItem = srh_Customers.SelectListBox.Items.FindByText("")
            'If lstItem IsNot Nothing Then srh_Customers.SelectListBox.Items.Remove(lstItem)

            'If srh_Customers.SelectListBox.Items.Count > 0 Then
            '    srh_Customers.SelectCheckbox.Checked = False
            'End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            If CBool(objCommon.CheckDuplicate(sqlConn, "GroupMaster", "GroupName", txt_GroupName.Text)) = False Then
                Dim msg As String = "This Group Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_GroupMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
       
    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            'sqlTrans = clsCommonFuns.sqlConn.BeginTransaction
            sqlTrans = sqlConn.BeginTransaction
            OpenConn()
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            If DeleteDetails(sqlTrans) = False Then Exit Sub
            ' If SaveGroupCustomers(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()
            Response.Redirect("GroupDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
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
                objCommon.SetCommandParameters(sqlComm, "@GroupId", SqlDbType.SmallInt, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@GroupId", SqlDbType.SmallInt, 4, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@GroupName", SqlDbType.VarChar, 100, "I", , , Trim(txt_GroupName.Text))
            objCommon.SetCommandParameters(sqlComm, "@CustomerTypeId", SqlDbType.Int, 4, "I", , , Val(cbo_CustomerType.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@HideShow", SqlDbType.Char, 1, "I", , , rdo_HideShow.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@GroupId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        Try
            OpenConn()
            If objCommon.CheckDuplicate(sqlConn, "GroupMaster", "GroupName", Trim(txt_GroupName.Text), "GroupId", Val(ViewState("Id"))) = False Then
                Dim msg As String = "This Group Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_UPDATE_GroupMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
       
    End Sub
    Private Function DeleteDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_GroupCustomers"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@GroupId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    'Private Function SaveGroupCustomers(ByVal sqlTrans As SqlTransaction) As Boolean
    '    Try
    '        If srh_Customers.SelectCheckbox.Checked = True Then Return True
    '        Dim sqlComm As New SqlCommand
    '        sqlComm.Transaction = sqlTrans
    '        sqlComm.CommandType = CommandType.StoredProcedure
    '        sqlComm.CommandText = "ID_INSERT_GroupCustomers"
    '        sqlComm.Connection = clsCommonFuns.sqlConn
    '        For I As Int16 = 0 To srh_Customers.SelectListBox.Items.Count - 1
    '            If Val(srh_Customers.SelectListBox.Items(I).Value) <> 0 Then
    '                sqlComm.Parameters.Clear()
    '                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , srh_Customers.SelectListBox.Items(I).Value)
    '                objCommon.SetCommandParameters(sqlComm, "@GroupId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
    '                objCommon.SetCommandParameters(sqlComm, "@CustomerGroupId", SqlDbType.Int, 4, "O")
    '                objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
    '                objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
    '                sqlComm.ExecuteNonQuery()
    '            End If
    '        Next
    '        Return True
    '    Catch ex As Exception
    '        sqlTrans.Rollback()
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Function

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("GroupDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("GroupDetail.aspx")
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
End Class
