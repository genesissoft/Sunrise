Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_MessageBoard
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim datEndDate As Date
    Dim datRegDate As Date
    Dim sqlConn As SqlConnection

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

    End Sub
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
            Page.Form.DefaultButton = btn_Save.UniqueID
            If IsPostBack = False Then
                txt_RegisteredDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
                txt_EndDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
                SetAttributes()
                SetTimeCombos()
                If Val(Request.QueryString("Id") & "") <> 0 Then
                    ViewState("Id") = Val(Request.QueryString("Id") & "")
                    FillFields()
                    btn_Save.Visible = False
                    btn_Update.Visible = True
                    Page.Form.DefaultButton = btn_Update.UniqueID
                Else
                    btn_Save.Visible = True
                    btn_Update.Visible = False
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
    Private Sub FillFields()

        Try
            Dim dt As DataTable
            Dim strTime() As String
            Dim dr As DataRow
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_MESSAGEBOARD", ViewState("Id"), "MessageId")
            If dt.Rows.Count > 0 Then
                txt_Message.Text = Trim(dt.Rows(0).Item("Message") & "")
                txt_EndDate.Text = Trim(dt.Rows(0).Item("EndDate") & "")
                txt_RegisteredDate.Text = Trim(dt.Rows(0).Item("RegisteredDate") & "")
                rbl_UserTypeSection.SelectedValue = Trim(dt.Rows(0).Item("UserTypeSection") & "")
                strTime = Split(Trim(dt.Rows(0).Item("EndTime")), ":")
                cbo_hr.SelectedValue = strTime(0)
                cbo_minute.SelectedValue = Left(strTime(1), 2)
                cbo_ampm.SelectedValue = Right(strTime(1), 2)
            End If
            objCommon.FillControl(srh_Branches.SelectListBox, sqlConn, "ID_FILL_MessageBoardBranches", "BranchName", "BranchId", ViewState("Id"), "MessageId")
            objCommon.FillControl(srh_Users.SelectListBox, sqlConn, "ID_FILL_MessageBoardUsers", "NameOfUser", "UserId", ViewState("Id"), "MessageId")
            Dim lstItem As ListItem
            lstItem = srh_Branches.SelectListBox.Items.FindByText("")

            If lstItem IsNot Nothing Then srh_Branches.SelectListBox.Items.Remove(lstItem)
            lstItem = srh_Users.SelectListBox.Items.FindByText("")
            If lstItem IsNot Nothing Then srh_Users.SelectListBox.Items.Remove(lstItem)

            If srh_Branches.SelectListBox.Items.Count > 0 Then
                srh_Branches.SelectCheckbox.Checked = False
            End If
            If srh_Users.SelectListBox.Items.Count > 0 Then
                srh_Users.SelectCheckbox.Checked = False
            End If
        Catch ex As Exception
        Finally
            CloseConn()
        End Try
    End Sub


    Private Sub SetAttributes()
        txt_EndDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_EndDate.Attributes.Add("onblur", "CheckDate(this,false);")
        btn_Save.Attributes.Add("onclick", "return validate();")
        btn_Update.Attributes.Add("onclick", "return validate();")
        txt_Message.Attributes.Add("onblur", "ConvertUCase(this)")
        'btn_Print.Attributes.Add("onclick", "return Validation();")
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            SetValues()
            SetSaveUpdate("ID_INSERT_MessageBoard")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            OpenConn()
            Dim sqlTrans As SqlTransaction
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            If DeleteDetails(sqlTrans) = False Then Exit Sub
            If SaveMsgBoardBranches(sqlTrans) = False Then Exit Sub
            If SaveMsgBoardUsers(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()
            Response.Redirect("MessageDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try


    End Sub
    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            'CHANGE 
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@MessageId", SqlDbType.SmallInt, 2, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@MessageId", SqlDbType.SmallInt, 2, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@Message", SqlDbType.VarChar, 500, "I", , , Trim(txt_Message.Text))
            objCommon.SetCommandParameters(sqlComm, "@RegisteredDate", SqlDbType.SmallDateTime, 4, "I", , , datRegDate)
            objCommon.SetCommandParameters(sqlComm, "@EndDate", SqlDbType.SmallDateTime, 4, "I", , , datEndDate)
            objCommon.SetCommandParameters(sqlComm, "@Userid", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@UserTypeSection", SqlDbType.Char, 1, "I", , , Trim(rbl_UserTypeSection.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@MessageId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function SaveMsgBoardUsers(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            'If srh_Users.SelectCheckbox.Checked = True Then Return True
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_MessageBoardUsers"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To srh_Users.SelectListBox.Items.Count - 1
                If Val(srh_Users.SelectListBox.Items(I).Value) <> 0 Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , srh_Users.SelectListBox.Items(I).Value)
                    objCommon.SetCommandParameters(sqlComm, "@MessageId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@MsgBoardUserId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
                    sqlComm.ExecuteNonQuery()
                End If

            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function SaveMsgBoardBranches(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            'If srh_Branches.SelectCheckbox.Checked = True Then Return True
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_MessageBoardBranches"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To srh_Branches.SelectListBox.Items.Count - 1
                If Val(srh_Branches.SelectListBox.Items(I).Value) <> 0 Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@BranchId", SqlDbType.Int, 4, "I", , , srh_Branches.SelectListBox.Items(I).Value)
                    objCommon.SetCommandParameters(sqlComm, "@MessageId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@MsgBoardBranchId ", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Sub SetValues()
        Try
            txt_Message.Text = Trim(txt_Message.Text)
            datEndDate = objCommon.DateFormat(txt_EndDate.Text) & " " & cbo_hr.SelectedValue & ":" & cbo_minute.SelectedValue & ":00 " & cbo_ampm.SelectedValue
            datRegDate = objCommon.DateFormat(txt_RegisteredDate.Text)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub SetTimeCombos()
        Try
            Dim i As Integer
            cbo_hr.Items.Clear()
            cbo_minute.Items.Clear()
            For i = 1 To 12
                If i < 10 Then
                    cbo_hr.Items.Add(New ListItem(Trim("0" & i), i))
                Else
                    cbo_hr.Items.Add(New ListItem(Val(i), Val(i)))
                End If
            Next
            For i = 0 To 59 Step 15
                If i < 10 Then
                    cbo_minute.Items.Add(New ListItem(Trim("0" & i), Trim("0" & i)))
                Else
                    cbo_minute.Items.Add(New ListItem(Val(i), Val(i)))
                End If
            Next
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    Protected Sub btn_Reset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Reset.Click
        Try
            Response.Redirect("MessageDetail.aspx", False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click

        OpenConn()
        If objCommon.CheckDuplicate(sqlConn, "MessageBoard", "Message", Trim(txt_Message.Text), "MessageId", Val(ViewState("Id"))) = False Then
            Dim msg As String = "This Message Already Exist"
            Dim strHtml As String
            strHtml = "alert('" + msg + "');"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg();", strHtml, True)
            Exit Sub
        End If
        SetValues()
        CloseConn()
        SetSaveUpdate("ID_UPDATE_MessageBoard")
        'SetTimeCombos()
    End Sub
    Private Function DeleteDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_MsgBoardBranchesUsers"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@MessageId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            sqlConn.Dispose()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub
End Class
