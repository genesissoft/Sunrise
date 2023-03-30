Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_ClientTypeMaster
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
            ''Response.Filter = New WhitespaceFilter(Response.Filter)
            If IsPostBack = False Then
                SetAttributes()
                Page.SetFocus(txt_ClientType)
                If Val(Request.QueryString("Id") & "") <> 0 Then
                    ViewState("Id") = Val(Request.QueryString("Id") & "")
                    FillFields()
                    txt_ClientType.Focus()
                    btn_Save.Visible = False
                    btn_Update.Visible = True

                End If
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
        txt_ClientType.Attributes.Add("onblur", "ConvertUCase(this);")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
        btn_Update.Visible = False
    End Sub
    Private Sub FillFields()
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable1(sqlConn, "Id_FILL_ClientTypeMaster", ViewState("Id"), "ClientTypeId")
            If dt.Rows.Count > 0 Then
                txt_ClientType.Text = Trim(dt.Rows(0).Item("ClientType") & "")
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "')")
        Finally
            CloseConn()
        End Try
     
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            If CBool(objCommon.CheckDuplicate(sqlConn, "ClientTypeMaster", "ClientType", txt_ClientType.Text)) = False Then
                Dim msg As String = "This Client Type Already Exist"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_ClientTypeMaster")
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "')")
        Finally
            CloseConn()
        End Try
      
    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        Try
            OpenConn()
            If objCommon.CheckDuplicate(sqlConn, "ClientTypeMaster", "ClientType", Trim(txt_ClientType.Text), "ClientTypeId", Val(ViewState("Id"))) = False Then
                Dim msg As String = "This Client Type Already Exist"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_UPDATE_ClientTypeMaster")
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "')")
        Finally
            CloseConn()
        End Try
     
    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            'sqlTrans = clsCommonFuns.sqlConn.BeginTransaction
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            sqlTrans.Commit()
            Response.Redirect("ClientTypeDetail.aspx?Id=" & ViewState("Id"), False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
                objCommon.SetCommandParameters(sqlComm, "@ClientTypeId", SqlDbType.SmallInt, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@ClientTypeId", SqlDbType.SmallInt, 4, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@ClientType", SqlDbType.VarChar, 100, "I", , , Trim(txt_ClientType.Text))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@ClientTypeId").Value

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            Response.Redirect("ClientTypeDetail.aspx", False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

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
