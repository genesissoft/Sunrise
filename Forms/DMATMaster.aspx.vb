Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_DMATMaster
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
            'Session("CompId") = 9
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
        txt_DPName.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_DPId.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_ClientId.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_CSDLCDSL.Attributes.Add("onblur", "ConvertUCase(this);")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            If CBool(objCommon.CheckDuplicate(sqlConn, "DMATMaster", "ClientId", txt_ClientId.Text, , , "CompId", Session("CompId"))) = False Then
                Dim msg As String = "This Client ID Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_DMATMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        Try
            OpenConn()
            SetSaveUpdate("ID_UPDATE_DMATMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("DMATDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("DMATDetail.aspx")
            End If


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            'sqlTrans = clsCommonFuns.sqlConn.BeginTransaction
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            sqlTrans.Commit()
            Response.Redirect("DMATDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
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
                objCommon.SetCommandParameters(sqlComm, "@DMatId", SqlDbType.SmallInt, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@DMatId", SqlDbType.SmallInt, 4, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@DPName", SqlDbType.VarChar, 100, "I", , , Trim(txt_DPName.Text))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@DPId", SqlDbType.VarChar, 20, "I", , , Trim(txt_DPId.Text))
            objCommon.SetCommandParameters(sqlComm, "@ClientId", SqlDbType.VarChar, 20, "I", , , Trim(txt_ClientId.Text))
            objCommon.SetCommandParameters(sqlComm, "@NSDL_CDSL", SqlDbType.VarChar, 20, "I", , , Trim(txt_CSDLCDSL.Text))
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            objCommon.SetCommandParameters(sqlComm, "@DefaultDMAT", SqlDbType.Bit, 1, "I", , , chkDefault.Checked)
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@DMatId").Value

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Sub FillFields()
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_DMATMaster", ViewState("Id"), "DMatId")
            If dt.Rows.Count > 0 Then
                txt_DPName.Text = Trim(dt.Rows(0).Item("DPName") & "")
                txt_DPId.Text = Trim(dt.Rows(0).Item("DPId") & "")
                txt_ClientId.Text = Trim(dt.Rows(0).Item("ClientId") & "")
                txt_CSDLCDSL.Text = Trim(dt.Rows(0).Item("NSDL_CDSL") & "")
                If Convert.ToString((dt.Rows(0).Item("DefaultDMat"))) = "" Then
                    chkDefault.Checked = False
                Else
                    chkDefault.Checked = dt.Rows(0).Item("DefaultDMat")

                End If
            End If
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
