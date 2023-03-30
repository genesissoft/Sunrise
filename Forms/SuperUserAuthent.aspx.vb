Imports System.Data


Partial Class Forms_SuperUserAuthent
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")

            If Not Page.IsPostBack Then

                Dim strPwdType As String = Request.QueryString("pwdtype")

                btnCancel.Attributes.Add("onclick", "return CloseFormCancel();")
                Page.SetFocus(txt_Password)
                ViewState("pwdtype") = strPwdType
                hdn_pwdflag.Value = ""

                Dim dt As DataTable = Session("SPassword")
                Dim DvSPassword As DataView = New DataView(dt)

                With DvSPassword
                    .RowFilter = "SUserFor ='" & ViewState("pwdtype") & "'"
                End With

                hid_pwd.Value = ""
                If (DvSPassword.Count > 0) Then
                    hid_pwd.Value = objCommon.DeCrypt(Trim(DvSPassword(0)("SPassword")))
                End If

            End If


        Catch ex As Exception

        End Try
       
    End Sub

    'Protected Sub btn_Ok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Ok.Click
    '    Dim OldhashedBytes(16) As Byte
    '    Dim CurrenthashedBytes(16) As Byte
    '    Dim strSql As String = ""
    '    Dim objPwdDB As New PasswordDatabase.clsDatabase
    '    Dim objPwdEncrypt As New PasswordDatabase.clsEncryption
    '    Dim bFlag As String = String.Empty

    '    Try

    '        Dim dt As DataTable = Session("SPassword")
    '        Dim DvSPassword As DataView = New DataView(dt)

    '        With DvSPassword
    '            .RowFilter = "SUserFor ='" & ViewState("pwdtype") & "'"
    '        End With

    '        If (DvSPassword.Count > 0) Then
    '            OldhashedBytes = DvSPassword(0)("SPassword")
    '            hid_pwd.Value = OldhashedBytes.ToString()
    '            ' hid_pwd.Value = "a"
    '        End If

    '        dt = objPwdDB.GetConfigTable(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
    '        If dt.Rows.Count > 0 Then
    '            CurrenthashedBytes = objPwdEncrypt.GetEncryptedPassword(Trim(txt_Password.Text), dt.Rows(0).Item("EncryptionType"))
    '        End If
    '        For i As Integer = 0 To OldhashedBytes.Length - 1
    '            If CurrenthashedBytes.GetValue(i).ToString <> OldhashedBytes.GetValue(i).ToString Then
    '                bFlag = "N"
    '                Exit For
    '            End If
    '        Next

    '        If Not (bFlag = "N") Then
    '            bFlag = "T"
    '        End If


    '        Dim strStartUpScript As String = "fnUnloadHandler_1('" & bFlag & "');"

    '        If (Not ClientScript.IsStartupScriptRegistered(Me.GetType(), strStartUpScript)) Then
    '            ClientScript.RegisterStartupScript(Me.GetType(), "okclose", strStartUpScript, True)
    '        End If

    '        '  End If
    '        ' End If
    '    Catch ex As Exception
    '        ' Response.Write(ex.Message)
    '    Finally
    '        ' sqlConn.Close()
    '    End Try
    'End Sub

  
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

    End Sub
End Class
