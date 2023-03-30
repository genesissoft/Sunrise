Imports System.Data
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Web.UI.Control
Imports Microsoft.ApplicationBlocks.ExceptionManagement

Partial Class Forms_SuperChangePassword
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim blnPsw As Boolean = True
    Dim hashedBytes(16) As Byte
    Dim OldpasswordhashedBytes() As Byte
    Dim ConfirmhashedBytes(16) As Byte

    Dim Confirmhashedstr As String
    Dim Oldpasswordhashedstr As String
    Dim hashedString As String

    Dim sqlConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            'Response.Cache.SetCacheability(HttpCacheability.NoCache)
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
                Hid_loginname.Value = Session("UserName")
                SetAttributes()
                'SetPasswordFields()
                FillPasswordTypeCombo()

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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
            Exit Sub
        End If

        SetSaveUpdate()
        Dim msg As String = "Password Save Successfully."
        strHtml = "alert('" + msg + "');"
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msges", strHtml, True)
    End Sub
    Private Sub SetSaveUpdate()
        Try
            Dim sqlTrans As SqlTransaction
            Dim strUrl As String
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction

            If UpdateUserPassword(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()

            If ((Session("UserTypeId") = "1") Or (Session("UserTypeId") = "40") Or (Session("UserTypeId") = "43") Or (Session("UserTypeId") = "44") Or (Session("UserTypeId") = "66") Or (Session("UserTypeId") = "63") Or (Session("UserTypeId") = "67")) Then
                strUrl = "SelectYear.aspx"
            Else
                strUrl = "QuoteDetails.aspx"
            End If
            Dim dt As DataTable

            dt = objCommon.FillDataTable(sqlConn, "MB_FILL_SuperUserType")
            Session("SPassword") = dt

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "show", "window.location='" & strUrl & "'", True)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function UpdateUserPassword(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = "ID_UPDATE_SUserPassword"
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            'sqlComm.Connection = clsCommonFuns.sqlConn
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@SUserId", SqlDbType.SmallInt, 4, "I", , , Val(Session("SUserId")))
            If hashedString IsNot Nothing Then
                objCommon.SetCommandParameters(sqlComm, "@SPassword", Data.SqlDbType.VarChar, 20, "I", , , hashedString)
            End If

            objCommon.SetCommandParameters(sqlComm, "@SPasswordChangeDate", SqlDbType.SmallDateTime, 4, "I", , , Date.Today)
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
            objCommon.SetCommandParameters(sqlComm, "@SChangePasswordId", SqlDbType.SmallInt, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@SUserId", Data.SqlDbType.BigInt, 16, "I", , , Val(Session("SUserId")))
            If hashedString IsNot Nothing Then
                objCommon.SetCommandParameters(sqlComm, "@SPassword", Data.SqlDbType.VarChar, 20, "I", , , Confirmhashedstr)
            End If
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@SChangePasswordId").Value
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
            Dim currPwd As String = String.Empty
            Dim oldPwd As String = String.Empty

            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "MB_FILL_SuperUserType")

            'Session("SPassword") = dt
            Dim DvSPassword As DataView = New DataView(dt)

            With DvSPassword
                .RowFilter = "SUserFor ='" & cbo_PasswordType.SelectedValue & "'"
            End With


            If DvSPassword.Count > 0 Then
                If IsDBNull(DvSPassword(0)("SPassword")) Then
                Else
                    Oldpasswordhashedstr = DvSPassword(0)("SPassword").ToString()
                End If
                If IsDBNull(DvSPassword(0)("SUserForDesc")) Then
                Else
                    Hid_loginname.Value = DvSPassword(0)("SUserForDesc")
                End If
                Session("SUserId") = DvSPassword(0)("SUserId")
            End If


            Confirmhashedstr = objCommon.Crypt(Trim(txt_OldPassword.Text))
            hashedString = objCommon.Crypt(Trim(txt_NewPassword.Text))

            If (Oldpasswordhashedstr <> Confirmhashedstr) Then
                Return False
            End If

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
    Private Sub FillPasswordTypeCombo()
        Try
            OpenConn()
            objCommon.FillControl(cbo_PasswordType, sqlConn, "MB_FILL_SuperUserType", "SUserForDesc", "SUserFor")
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    'Private Function CheckPrevPasswords() As Boolean
    '    Try
    '        OpenConn()
    '        Dim dt As DataTable
    '        Dim i As Integer
    '        Dim OldpasswordhashedBytes() As Byte
    '        Dim objPwdDB As New PasswordDatabase.clsDatabase
    '        Dim objPwdEncrypt As New PasswordDatabase.clsEncryption

    '        Dim intLstPwdCnt As Int16
    '        dt = objPwdDB.GetConfigTable(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
    '        'If dt.Rows.Count > 0 Then
    '        '    ConfirmhashedBytes = objPwdEncrypt.GetEncryptedPassword(Trim(txt_ConfirmPassword.Text), dt.Rows(0).Item("EncryptionType"))
    '        'End If

    '        Confirmhashedstr = objCommon.Crypt(txt_ConfirmPassword.Text)

    '        dt = objCommon.FillDataTable(sqlConn, "Id_FILL_SPasswordDetail", Session("SUserId"), "SUserId")
    '        intLstPwdCnt = Val(Hid_LastPasswordCnt.Value)
    '        If Not (dt Is Nothing) Then
    '            If intLstPwdCnt > dt.Rows.Count Then
    '                intLstPwdCnt = dt.Rows.Count
    '            End If
    '        End If

    '        If Not (dt Is Nothing) Then
    '            For i = 0 To intLstPwdCnt - 1
    '                'OldpasswordhashedBytes = dt.Rows(i).Item("SPassword")
    '                Oldpasswordhashedstr = Convert.ToString(dt.Rows(i).Item("SPassword"))
    '                'If (ISArrayEqual(OldpasswordhashedBytes, ConfirmhashedBytes) = True) Then
    '                '    Return False
    '                'End If
    '                If Oldpasswordhashedstr = Confirmhashedstr Then
    '                    Return False
    '                End If
    '            Next
    '        End If

    '        Return True
    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    Finally
    '        CloseConn()
    '    End Try
    'End Function

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
            sqlConn.Dispose()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub
End Class
