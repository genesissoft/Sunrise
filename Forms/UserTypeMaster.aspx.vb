Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_UserTypeMaster
    Inherits System.Web.UI.Page
    Dim lstItem As ListItem
    Dim sqlConn As New SqlConnection
    Dim objCommon As New clsCommonFuns

    Protected Sub Page_AbortTransaction(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.AbortTransaction

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            'If clsCommonFuns.sqlConn Is Nothing Then objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            'srh_BusinessType.SelectCheckbox.Visible = False
            'srh_BusinessType.SelectCheckbox.Checked = False
            'srh_BusinessType.SelectLinkButton.Enabled = True

            lnk_MerchantBanking.Visible = False
            lnk_PMS.Visible = False
            lnk_WDM.Visible = False

            If IsPostBack = False Then
                If Request.QueryString("Id") <> "" Then
                    Dim strId As String = objCommon.DecryptText(HttpUtility.UrlDecode(Request.QueryString("Id")))
                    ViewState("Id") = Val(strId)
                    FillFields()
                    btn_Save.Visible = False
                    btn_Update.Visible = True
                    LinkbuttontVF()
                Else
                    FillAccessDataGrid()
                    btn_Save.Visible = True
                    btn_Update.Visible = False

                End If
                SetAttributes()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "select", "CheckBranch();", True)
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
        If sqlConn.State = ConnectionState.Open Then
            sqlConn.Close()
            sqlConn = Nothing
        End If
    End Sub
    Private Sub SetAttributes()
        Try
            OpenConn()
            txt_UserType.Attributes.Add("onblur", "ConvertUCase(this);")
            btn_Save.Attributes.Add("onclick", "return CommonValidation();")
            'btn_Update.Attributes.Add("onclick", "return CommonValidation();")
            rdo_Checker.Attributes.Add("onclick", "CheckBranch();")
            'objCommon.FillControl(cbo_DealerName, sqlConn, "ID_FILL_User", "NameOfUser", "UserId", Val(ViewState("Id")), "UsertypeId")
        Catch ex As Exception
        Finally
            CloseConn()
        End Try

    End Sub
    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        'CHANGE
        Try
            OpenConn()
            If CBool(objCommon.CheckDuplicate(sqlConn, "UserTypeMaster", "UserTypeName", txt_UserType.Text)) = False Then

                Dim msg As String = "This User Type Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_UserTypeMaster")

        Catch ex As Exception

        Finally
            CloseConn()
        End Try


    End Sub


    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        'CHANGEViewState("Id")
        Try
            OpenConn()
            If objCommon.CheckDuplicate(sqlConn, "UserTypeMaster", "UserTypeName", Trim(txt_UserType.Text), "UserTypeId", Val(ViewState("Id"))) = False Then
                Dim msg As String = "This  User Type Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_UPDATE_UserTypeMaster")
        Catch ex As Exception

        Finally
            CloseConn()
        End Try

    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            If DeleteDetails(sqlTrans) = False Then Exit Sub
            If DeleteUserTypeSetting(sqlTrans) = False Then Exit Sub
            If SaveUserTypeSetting(sqlTrans) = False Then Exit Sub
            If SaveBusinessType(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()
            ' Response.Redirect("UserTypeDetail.aspx", False)
            Response.Redirect("UserTypeDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
                objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.SmallInt, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.SmallInt, 4, "I", , , ViewState("Id"))
            End If
            'objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@UserTypeName", SqlDbType.VarChar, 100, "I", , , Trim(txt_UserType.Text))
            objCommon.SetCommandParameters(sqlComm, "@Checker", SqlDbType.Char, 1, "I", , , Trim(rdo_Checker.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@UserTypeSection", SqlDbType.Char, 1, "I", , , Trim(rbl_UserTypeSection.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@ForBranch", SqlDbType.Char, 1, "I", , , Trim(rdo_Branch.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@RestrictedAccess", SqlDbType.Char, 1, "I", , , Trim(rdo_RestrictedAccess.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@UserTypeId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    'Private Function SaveUserTypeSetting(ByRef sqlTrans As SqlTransaction) As Boolean
    '    Try
    '        Dim sqlComm As New SqlCommand
    '        Dim dgItem As DataGridItem
    '        Dim intUserTypeId As Integer
    '        Dim strFormName As String
    '        Dim chrAccess As Char
    '        Dim I As Int16
    '        With sqlComm
    '            .Connection = clsCommonFuns.sqlConn
    '            .Transaction = sqlTrans
    '            .CommandType = CommandType.StoredProcedure
    '            .CommandText = "ID_INSERT_UserTypeSettings"
    '            For I = 0 To gv_Access.Items.Count - 1
    '                dgItem = gv_Access.Items(I)
    '                strFormName = (CType(dgItem.FindControl("lbl_FormName"), Label).Text)
    '                chrAccess = (CType(dgItem.FindControl("rdo_Incetive1"), RadioButtonList).Text)
    '                .Parameters.Clear()
    '                objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
    '                objCommon.SetCommandParameters(sqlComm, "@FormName", SqlDbType.VarChar, 500, "I", , , strFormName)
    '                objCommon.SetCommandParameters(sqlComm, "@Access", SqlDbType.Char, 1, "I", , , chrAccess)
    '                objCommon.SetCommandParameters(sqlComm, "@UserTypeSettingId", SqlDbType.Int, 4, "O")
    '                objCommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
    '                .ExecuteNonQuery()
    '            Next
    '        End With
    '        Return True
    '    Catch ex As Exception
    '        sqlTrans.Rollback()
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '        Return False
    '    End Try
    'End Function


    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("UserTypeDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("UserTypeDetail.aspx")
            End If


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillFields()
        'CHANGE 
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_UserTypeMaster", ViewState("Id"), "UserTypeId")
            If dt.Rows.Count > 0 Then
                txt_UserType.Text = Trim(dt.Rows(0).Item("UserTypeName") & "")
                rdo_Checker.SelectedValue = Trim(dt.Rows(0).Item("Checker") & "")
                rbl_UserTypeSection.SelectedValue = Trim(dt.Rows(0).Item("UserTypeSection") & "")
                rdo_Branch.SelectedValue = Trim(dt.Rows(0).Item("ForBranch") & "")
                rdo_RestrictedAccess.SelectedValue = Trim(dt.Rows(0).Item("RestrictedAccess") & "")

                dt = objCommon.FillDataTable(sqlConn, "ID_FILL_UserFormAccessTable1", ViewState("Id"), "UserTypeId")
                gv_Access.DataSource = dt
                gv_Access.DataBind()
                'dt = objCommon.FillDetailsGrid(gv_Access, "ID_FILL_UserFormAccessTable1", "UserTypeId", Val(ViewState("Id") & ""))
                Session("UserTypeSettingTable") = dt

                dt = objCommon.FillControl(srh_BusinessType.SelectListBox, sqlConn, "ID_FILL_UserBusinessTypeMaster", "BusinessType", "BusinessTypeId", ViewState("Id"), "UserTypeId")
                lstItem = srh_BusinessType.SelectListBox.Items.FindByText("")
                If lstItem IsNot Nothing Then
                    srh_BusinessType.SelectListBox.Items.Remove(lstItem)
                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try


    End Sub
    Private Function DeleteUserTypeSetting(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_UserFormAccessTable"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    'Private Sub FillAccessDataGrid()
    '    'CHANGE 
    '    Dim dt As DataTable
    '    dt = objCommon.FillDetailsGrid(gv_Access, "ID_FILL_FormAccessTable", "FormAccessId", Val(ViewState("Id") & ""))
    '    Session("FormAccessTable") = dt
    'End Sub

    Private Function SaveBusinessType(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_UserBussinessTable"
            sqlComm.Connection = sqlConn
            'If srh_BusinessType.SelectCheckbox.Checked = True Then Return True
            For I As Int16 = 0 To srh_BusinessType.SelectListBox.Items.Count - 1
                If Val(srh_BusinessType.SelectListBox.Items(I).Value) <> 0 Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@BusinessTypeId", SqlDbType.Int, 4, "I", , , srh_BusinessType.SelectListBox.Items(I).Value)
                    objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@UserBussinessId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Sub LinkbuttontVF()
        Try
            For I As Int16 = 0 To srh_BusinessType.SelectListBox.Items.Count - 1

                If Val(srh_BusinessType.SelectListBox.Items(I).Value) <> 0 Then
                    If (srh_BusinessType.SelectListBox.Items(I).Value = 4) Then
                        lnk_PMS.Visible = True
                    End If
                    If (srh_BusinessType.SelectListBox.Items(I).Value = 5) Then
                        lnk_WDM.Visible = True
                    End If
                    If (srh_BusinessType.SelectListBox.Items(I).Value = 2) Then
                        lnk_MerchantBanking.Visible = True
                    End If
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function DeleteDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_UserBussinessTable"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub srh_BusinessType_linkbutton() Handles srh_BusinessType.LinkClick
        Try
            LinkbuttontVF()
        Catch ex As Exception
        End Try
    End Sub


    Private Function SaveUserTypeSetting(ByRef sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dgItem As DataGridItem
            Dim strFormName As String
            Dim chrAccess As Char
            Dim strUrl As String
            Dim IntMenuId As Integer

            Dim I As Int16
            With sqlComm
                .Connection = sqlConn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_INSERT_UserFormAccessTable"
                For I = 0 To gv_Access.Items.Count - 1
                    dgItem = gv_Access.Items(I)
                    strFormName = (CType(dgItem.FindControl("lbl_Name"), Label).Text)
                    strUrl = (CType(dgItem.FindControl("lbl_URL"), Label).Text)
                    IntMenuId = (CType(dgItem.FindControl("lbl_MenuID"), Label).Text)
                    chrAccess = (CType(dgItem.FindControl("rdo_Incetive1"), RadioButtonList).Text)

                    .Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@Name", SqlDbType.VarChar, 500, "I", , , strFormName)

                    objCommon.SetCommandParameters(sqlComm, "@URL", SqlDbType.VarChar, 500, "I", , , strUrl)
                    objCommon.SetCommandParameters(sqlComm, "@MenuId", SqlDbType.VarChar, 500, "I", , , IntMenuId)

                    objCommon.SetCommandParameters(sqlComm, "@UserAccess", SqlDbType.Char, 1, "I", , , chrAccess)
                    objCommon.SetCommandParameters(sqlComm, "@FromAccessId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
                    .ExecuteNonQuery()
                Next
            End With
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            Return False
        End Try
    End Function

    Private Sub FillAccessDataGrid()
        'CHANGE
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = sqlConn
            sqlComm.CommandText = "ID_FILL_MenuMasterDefault"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.ExecuteNonQuery()
            Dim sqlDa As New SqlDataAdapter()
            Dim dt As New DataTable()
            Dim dt1 As New DataTable()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            Session("DefaultSetting") = dt
            gv_Access.DataSource = dt
            gv_Access.DataBind()
        Catch ex As Exception
        Finally
            CloseConn()
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
