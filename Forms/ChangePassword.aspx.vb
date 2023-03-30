Imports System.Data
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Web.UI.Control
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Partial Class Forms_ChangePassword
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim blnPsw As Boolean = True
    Dim hashedBytes(16) As Byte
    Dim OldpasswordhashedBytes() As Byte
    Dim ConfirmhashedBytes(16) As Byte
    Dim sqlConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            'objCommon.OpenConn()
            If IsPostBack = False Then
                Hid_loginname.Value = Session("UserName")
                SetAttributes()
                SetPasswordFields()
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
        If sqlConn.State = ConnectionState.Open Then sqlConn.Close()
    End Sub
    Private Sub SetAttributes()
        btn_Save.Attributes.Add("onclick", "return  Validation();")
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        'If CBool(objCommon.CheckDuplicate(clsCommonFuns.sqlConn, "UserMaster", "UserName", txt_loginname.Text)) = False Then
        Dim strHtml As String
        If CheckOldPassword() = False Then
            Dim msg1 As String = "Old Password is not correct."
            strHtml = "alert('" + msg1 + "');"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msges", strHtml, True)
            Exit Sub
        End If
        If CheckPrevPasswords() = False Then
            'If ISArrayEqual(OldpasswordhashedBytes, hashedBytes) = False Then
            Dim msg2 As String = "Previous Password can not be entered."
            strHtml = "alert('" + msg2 + "');"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msges", strHtml, True)
            Exit Sub
        End If
        SetSaveUpdate("ID_INSERT_PasswordDetail")
        Dim msg As String = "Password Saved Successfully."
        strHtml = "alert('" + msg + "');"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msges", strHtml, True)
    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            Dim strUrl As String
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            If UpdateUserPassword(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()
            strUrl = "SelectYear.aspx"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msges", "window.location='" & strUrl & "'", True)

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function UpdateUserPassword(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = "ID_UPDATE_UserPassword"
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            'sqlComm.Connection = clsCommonFuns.sqlConn
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.SmallInt, 4, "I", , , Session("UserId"))
            If ConfirmhashedBytes IsNot Nothing Then
                objCommon.SetCommandParameters(sqlComm, "@Password", Data.SqlDbType.Binary, 16, "I", , , ConfirmhashedBytes)
            End If
            objCommon.SetCommandParameters(sqlComm, "@PasswordChangeDate", SqlDbType.SmallDateTime, 4, "I", , , Date.Today)
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            'sqlComm.Connection = clsCommonFuns.sqlConn
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@ChangePasswordId", SqlDbType.SmallInt, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@UserId", Data.SqlDbType.BigInt, 16, "I", , , Val(Session("UserId")))
            If hashedBytes IsNot Nothing Then
                objCommon.SetCommandParameters(sqlComm, "@Password", Data.SqlDbType.Binary, 16, "I", , , hashedBytes)
            End If
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@ChangePasswordId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function CheckOldPassword() As Boolean
        Try
            Dim dt As DataTable
            Dim objPwdDB As New PasswordDatabase.clsDatabase
            Dim objPwdEncrypt As New PasswordDatabase.clsEncryption
            Dim OldhashedBytes(16) As Byte
            Dim CurrenthashedBytes(16) As Byte
            OpenConn()

            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_UserMaster", Session("UserId"), "UserId")
            If dt.Rows.Count > 0 Then
                OldhashedBytes = dt.Rows(0).Item("Password")
                Hid_loginname.Value = dt.Rows(0).Item("NameOfUser")
            End If

            dt = objPwdDB.GetConfigTable(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
            If dt.Rows.Count > 0 Then
                CurrenthashedBytes = objPwdEncrypt.GetEncryptedPassword(Trim(txt_OldPassword.Text), dt.Rows(0).Item("EncryptionType"))
            End If
            For i As Integer = 0 To OldhashedBytes.Length - 1
                If CurrenthashedBytes.GetValue(i).ToString <> OldhashedBytes.GetValue(i).ToString Then
                    Return False
                End If
            Next
            Return True
        Catch ex As Exception
        Finally
            CloseConn()
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
                Hid_LastPasswordCnt.Value = Val(dt.Rows(0).Item("LastPasswordCnt").ToString)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function CheckPrevPasswords() As Boolean
        Try
            OpenConn()
            Dim dt As DataTable
            Dim i As Integer
            Dim OldpasswordhashedBytes() As Byte
            Dim objPwdDB As New PasswordDatabase.clsDatabase
            Dim objPwdEncrypt As New PasswordDatabase.clsEncryption

            Dim intLstPwdCnt As Int16
            dt = objPwdDB.GetConfigTable(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
            If dt.Rows.Count > 0 Then
                ConfirmhashedBytes = objPwdEncrypt.GetEncryptedPassword(Trim(txt_ConfirmPassword.Text), dt.Rows(0).Item("EncryptionType"))
            End If
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_PasswordDetail", Session("UserId"), "UserId")
            intLstPwdCnt = Val(Hid_LastPasswordCnt.Value)
            If intLstPwdCnt > dt.Rows.Count Then
                intLstPwdCnt = dt.Rows.Count
            End If
            For i = 0 To intLstPwdCnt - 1
                OldpasswordhashedBytes = dt.Rows(i).Item("Password")
                If (ISArrayEqual(OldpasswordhashedBytes, ConfirmhashedBytes) = True) Then
                    Return False
                End If
            Next
            Return True
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Function

    Private Function ISArrayEqual(ByVal arrPwd1 As Array, ByVal arrPwd2 As Array) As Boolean
        Try
            For i As Integer = 0 To arrPwd1.Length - 1
                If (arrPwd1.GetValue(i).ToString <> arrPwd2.GetValue(i).ToString) Then
                    Return False
                End If
            Next
            Return True
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
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
