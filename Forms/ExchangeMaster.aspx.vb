Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_ExchangeMaster
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
        txt_address1.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_address2.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_city.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_ExchangeName.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_ContactPerson.Attributes.Add("onblur", "ConvertUCase(this);")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
    End Sub
    Private Sub FillFields()
        'CHANGE 
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_ExchangeMaster", ViewState("Id"), "ExchangeId")
            If dt.Rows.Count > 0 Then
                txt_ExchangeName.Text = Trim(dt.Rows(0).Item("ExchangeName") & "")
                txt_ContactPerson.Text = Trim(dt.Rows(0).Item("ContactPerson") & "")
                txt_address1.Text = Trim(dt.Rows(0).Item("Address1") & "")
                txt_address2.Text = Trim(dt.Rows(0).Item("Address2") & "")
                txt_pincode.Text = Trim(dt.Rows(0).Item("Pincode") & "")
                txt_phoneno.Text = Trim(dt.Rows(0).Item("PhoneNo") & "")
                txt_fax.Text = Trim(dt.Rows(0).Item("FaxNo") & "")
                txt_email.Text = Trim(dt.Rows(0).Item("EmailId") & "")
                txt_city.Text = Trim(dt.Rows(0).Item("City") & "")
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
            If objCommon.CheckDuplicate(sqlConn, "ExchangeMaster", "ExchangeName", Trim(txt_ExchangeName.Text)) = False Then
                Dim msg As String = "This Exchange Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_ExchangeMaster")
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
            sqlTrans.Commit()
            Response.Redirect("ExchangeDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
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
                objCommon.SetCommandParameters(sqlComm, "@ExchangeId", SqlDbType.SmallInt, 2, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@ExchangeId", SqlDbType.SmallInt, 2, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@ExchangeName", SqlDbType.VarChar, 50, "I", , , Trim(txt_ExchangeName.Text))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@ContactPerson", SqlDbType.VarChar, 50, "I", , , Trim(txt_ContactPerson.Text))
            objCommon.SetCommandParameters(sqlComm, "@Address1", SqlDbType.VarChar, 100, "I", , , Trim(txt_address1.Text))
            objCommon.SetCommandParameters(sqlComm, "@Address2", SqlDbType.VarChar, 100, "I", , , Trim(txt_address2.Text))
            objCommon.SetCommandParameters(sqlComm, "@Pincode", SqlDbType.VarChar, 10, "I", , , Trim(txt_pincode.Text))
            objCommon.SetCommandParameters(sqlComm, "@City", SqlDbType.VarChar, 50, "I", , , Trim(txt_city.Text))
            objCommon.SetCommandParameters(sqlComm, "@FaxNo", SqlDbType.VarChar, 20, "I", , , Trim(txt_fax.Text))
            objCommon.SetCommandParameters(sqlComm, "@PhoneNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_phoneno.Text))
            objCommon.SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , Trim(txt_email.Text))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@ExchangeId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            Response.Redirect("ExchangeDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        Try
            OpenConn()
            If objCommon.CheckDuplicate(sqlConn, "ExchangeMaster", "ExchangeName", Trim(txt_ExchangeName.Text), "ExchangeId", Val(ViewState("Id"))) = False Then
                Dim msg As String = "This Exchange Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_UPDATE_ExchangeMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
