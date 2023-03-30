Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_SecurityCategoryMaster
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As New SqlConnection

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

            If IsPostBack = False Then
                SetAttributes()
                SetControls()
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
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
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
        txt_SecurityCategory.Attributes.Add("onblur", "ConvertUCase(this);")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
    End Sub
    Private Sub SetControls()
        Try
            OpenConn()
             objCommon.FillControl(cbo_SecurityType, sqlConn, "ID_FILL_SecurityTypeMaster", "SecurityTypeName", "SecurityTypeId")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            'If objCommon.CheckDuplicate(sqlConn, "SecurityCategoryMaster", "SecurityCategory", Trim(txt_SecurityCategory.Text)) = False Then
            '    Dim msg As String = "This Category Name Already Exist"
            '    Dim strHtml As String
            '    strHtml = "alert('" + msg + "');"
            '    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
            '    Exit Sub
            'End If
            SetSaveUpdate("ID_INSERT_SecurityCategoryMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
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
            Response.Redirect("SecurityCategoryDetail.aspx", False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
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
                objCommon.SetCommandParameters(sqlComm, "@SecurityCatId", SqlDbType.Int, 2, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@SecurityCatId", SqlDbType.Int, 2, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@SecurityCategory", SqlDbType.VarChar, 100, "I", , , Trim(txt_SecurityCategory.Text))
            objCommon.SetCommandParameters(sqlComm, "@SecurityTypeId", SqlDbType.Int, 4, "I", , , Val(cbo_SecurityType.SelectedValue))

            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@SecurityCatId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Private Sub FillFields()
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityCategoryMaster", ViewState("Id"), "SecurityCatId")
            If dt.Rows.Count > 0 Then
                txt_SecurityCategory.Text = Trim(dt.Rows(0).Item("SecurityCategory") & "")
                cbo_SecurityType.SelectedValue = Trim(dt.Rows(0).Item("SecurityTypeId") & "")

            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        Try
            OpenConn()
            'If objCommon.CheckDuplicate(sqlConn, "SecurityCategoryMaster", "SecurityCategory", Trim(txt_SecurityCategory.Text), "SecurityCatId", Val(ViewState("Id"))) = False Then
            '    Dim msg As String = "This Category Name Already Exist"
            '    Dim strHtml As String
            '    strHtml = "alert('" + msg + "');"
            '    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
            '    Exit Sub
            'End If
            SetSaveUpdate("ID_UPDATE_SecurityCategoryMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub btn_Cancel_Click(sender As Object, e As EventArgs) Handles btn_Cancel.Click
        If Val(ViewState("Id")) <> 0 Then
            Response.Redirect("SecurityCategoryDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Else
            Response.Redirect("SecurityCategoryDetail.aspx")
        End If
    End Sub
End Class
