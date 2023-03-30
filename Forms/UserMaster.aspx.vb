Imports System.Data
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Web.UI.Control
Imports Microsoft.ApplicationBlocks.ExceptionManagement

Partial Class Forms_UserMaster
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As SqlConnection
    Dim lstItem As ListItem

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'If Val(Session("UserId") & "") = 0 Then
            '    Response.Redirect("Login.aspx", False)
            '    Exit Sub
            'End If
            'objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            'srh_CategoryName.SelectCheckbox.Visible = False
            'srh_CategoryName.SelectCheckbox.Checked = False
            'srh_CategoryName.SelectLinkButton.Enabled = True
            srh_Company.SelectCheckBox.Visible = False
            srh_Company.SelectCheckBox.Checked = False
            If IsPostBack = False Then
                txt_JoiningDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
                txt_JoiningDate.Attributes.Add("onkeypress", "OnlyDate();")
                SetPasswordFields()
                SetAttributes()
                Page.SetFocus(cbo_usertype)
                Page.Form.DefaultButton = btn_Save.UniqueID
                OpenConn()
                objCommon.FillControl(cbo_usertype, sqlConn, "Id_FILL_UserTypeMaster", "UserTypeName", "UserTypeId")
                objCommon.FillControl(cbo_Branch, sqlConn, "ID_FILL_BranchMaster", "BranchName", "BranchId")
                objCommon.FillControl(cbo_ManagedBy, sqlConn, "ID_FILL_Dealer", "NameOfUser", "UserId")
                CloseConn()
                If Request.QueryString("Id") <> "" Then
                    Dim strId As String = objCommon.DecryptText(HttpUtility.UrlDecode(Request.QueryString("Id")))
                    ViewState("Id") = Val(strId)
                    FillFields()
                    Page.Form.DefaultButton = btn_Update.UniqueID
                    btn_Save.Visible = False
                    btn_Update.Visible = True
                    'Btn_ChangePassword.Visible = True
                    'txt_Password.Visible = False
                    ''lbl_Password.Visible = False
                    'txt_ConfirmPassword.Visible = False
                    ''lbl_ConfirmPassword.Visible = False
                    'Session("UserName") = Trim(txt_nameofuser.Text)
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "show", "ChangePassword('none','');", True)

                Else
                    btn_Save.Visible = True
                    btn_Update.Visible = False
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "show", "ChangePassword('','none');", True)
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
        txt_nameofuser.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_PhoneNo.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_MobileNo.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_FaxNo.Attributes.Add("onblur", "ConvertUCase(this);")
        btn_Save.Attributes.Add("onclick", "return  Validation();")
        btn_Update.Attributes.Add("onclick", "return  Validation();")
        Btn_ChangePassword.Attributes.Add("onclick", "return ChangePassword('','none');")
        txt_JoiningDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_JoiningDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_JoiningDate.Text = Format(Now, "dd/MM/yyyy")
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        'CHANGE
        Try
            OpenConn()
            If CBool(objCommon.CheckDuplicate(sqlConn, "UserMaster", "UserName", txt_loginname.Text)) = False Then
                Dim msg As String = "This Login Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_UserMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click


        Try
            OpenConn()
            If objCommon.CheckDuplicate(sqlConn, "UserMaster", "UserName", Trim(txt_loginname.Text), "UserId", Val(ViewState("Id"))) = False Then
                Dim msg As String = "This Login Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_UPDATE_UserMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

        Finally
            CloseConn()
        End Try


    End Sub

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("UserDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("UserDetail.aspx")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            If DeleteDetails(sqlTrans) = False Then Exit Sub
            If SaveDealerCompany(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()
            Response.Redirect("UserDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim objPwdDB As New PasswordDatabase.clsDatabase
            Dim objPwdEncrypt As New PasswordDatabase.clsEncryption
            Dim hashedBytes() As Byte
            If Val(ViewState("Id") & "") = 0 Or Hid_ChangePwd.Value = "none" Then
                Dim dt As DataTable = objPwdDB.GetConfigTable(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
                If dt.Rows.Count > 0 Then
                    hashedBytes = objPwdEncrypt.GetEncryptedPassword(Trim(txt_Password.Text), dt.Rows(0).Item("EncryptionType"))
                End If
            End If
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.SmallInt, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.SmallInt, 4, "I", , , ViewState("Id"))
            End If
            If hashedBytes IsNot Nothing Then
                objCommon.SetCommandParameters(sqlComm, "@Password", Data.SqlDbType.Binary, 16, "I", , , hashedBytes)
            End If
            objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Trim(cbo_usertype.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@ManagebyId", SqlDbType.Int, 4, "I", , , Val(cbo_ManagedBy.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@JoiningDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_JoiningDate.Text))
            objCommon.SetCommandParameters(sqlComm, "@UserName", SqlDbType.VarChar, 50, "I", , , Trim(txt_loginname.Text))
            objCommon.SetCommandParameters(sqlComm, "@UserCode", SqlDbType.VarChar, 10, "I", , , Trim(txt_EmployeeCode.Text))
            objCommon.SetCommandParameters(sqlComm, "@UniqueCode", SqlDbType.VarChar, 10, "I", , , Trim(txt_UniqueCode.Text))
            objCommon.SetCommandParameters(sqlComm, "@NameOfUser", SqlDbType.VarChar, 100, "I", , , Trim(txt_nameofuser.Text))
            objCommon.SetCommandParameters(sqlComm, "@BranchId", SqlDbType.Int, 4, "I", , , Trim(cbo_Branch.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@PhoneNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_PhoneNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@MobileNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_MobileNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@FaxNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_FaxNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@Status", SqlDbType.Char, 1, "I", , , cbo_Status.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , Trim(txt_EmailId.Text))
            objCommon.SetCommandParameters(sqlComm, "@CheckerValidDatetime", SqlDbType.DateTime, 8, "I", , , Date.MaxValue)
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            'objCommon.SetCommandParameters(sqlComm, "@StrMessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@UserId").Value

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Sub FillFields()
        'CHANGE 
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_UserMaster", ViewState("Id"), "UserId")
            If dt.Rows.Count > 0 Then
                cbo_usertype.SelectedValue = Trim(dt.Rows(0).Item("UserTypeId") & "")
                txt_loginname.Text = Trim(dt.Rows(0).Item("UserName") & "")
                txt_nameofuser.Text = Trim(dt.Rows(0).Item("NameOfUser") & "")
                txt_EmployeeCode.Text = Trim(dt.Rows(0).Item("UserCode") & "")
                txt_UniqueCode.Text = Trim(dt.Rows(0).Item("UniqueCode") & "")
                Session("UserName") = txt_nameofuser.Text
                cbo_Branch.SelectedValue = Trim(dt.Rows(0).Item("BranchId") & "")
                cbo_ManagedBy.SelectedValue = Trim(dt.Rows(0).Item("ManagebyId") & "")
                If Trim(dt.Rows(0).Item("JoiningDate") & "") <> "" Then
                    txt_JoiningDate.Text = Format(dt.Rows(0).Item("JoiningDate"), "dd/MM/yyyy")
                End If
                cbo_Status.SelectedValue = Trim(dt.Rows(0).Item("Status") & "")
                txt_PhoneNo.Text = Trim(dt.Rows(0).Item("PhoneNo") & "")
                txt_MobileNo.Text = Trim(dt.Rows(0).Item("MobileNo") & "")
                txt_FaxNo.Text = Trim(dt.Rows(0).Item("FaxNo") & "")
                txt_EmailId.Text = Trim(dt.Rows(0).Item("EmailId") & "")
            End If
            dt = objCommon.FillControl(srh_CategoryName.SelectListBox, sqlConn, "ID_FILL_UserDealerCategory", "DealerCategoryName", "DealerCategoryId", ViewState("Id"), "UserId")
            lstItem = srh_CategoryName.SelectListBox.Items.FindByText("")
            If lstItem IsNot Nothing Then
                srh_CategoryName.SelectListBox.Items.Remove(lstItem)
            End If

            dt = objCommon.FillControl(srh_Company.SelectListBox, sqlConn, "ID_FILL_UserCompanyDetails", "CompName", "CompanyId", ViewState("Id"), "UserId")
            lstItem = srh_Company.SelectListBox.Items.FindByText("")
            If lstItem IsNot Nothing Then
                srh_Company.SelectListBox.Items.Remove(lstItem)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try


    End Sub

    Private Function SaveDealerCompany(ByVal sqlTrans As SqlTransaction) As Boolean
        Try

            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_UserCompanyDetails"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To srh_Company.SelectListBox.Items.Count - 1
                If Val(srh_Company.SelectListBox.Items(I).Value) <> 0 Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@CompanyId", SqlDbType.Int, 4, "I", , , srh_Company.SelectListBox.Items(I).Value)
                    objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@UserCompanyDetailId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally

        End Try

    End Function
    Private Sub SetPasswordFields()
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable1(sqlConn, "ID_FILL_PasswordConfiguration")
            If dt.Rows.Count > 0 Then
                Hid_MinChars.Value = Val(dt.Rows(0).Item("MinChars").ToString)
                Hid_Format.Value = Trim(dt.Rows(0).Item("Format").ToString)
                Hid_ContainsLogin.Value = Trim(dt.Rows(0).Item("ContainsLogin").ToString)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function SaveDealerCategory(ByVal sqlTrans As SqlTransaction) As Boolean
        Try

            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_UserDealerDetails"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To srh_CategoryName.SelectListBox.Items.Count - 1
                If Val(srh_CategoryName.SelectListBox.Items(I).Value) <> 0 Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@DealerCategoryId", SqlDbType.Int, 4, "I", , , srh_CategoryName.SelectListBox.Items(I).Value)
                    objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@UserDealerId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally

        End Try

    End Function
    Private Function DeleteDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_UserDealers"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Protected Sub Btn_ChangePassword_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_ChangePassword.Click
        row_btn.Visible = False
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
