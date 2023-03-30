Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_ServiceTaxMaster
    Inherits System.Web.UI.Page
    Dim objcommon As New clsCommonFuns
    Dim sqlConn As New SqlConnection


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Val(Session("userid") & "") = 0 Then
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
                    Dim strId As String = objcommon.DecryptText(HttpUtility.UrlDecode(Request.QueryString("Id")))
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
        txt_ServTax.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_Cess.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_ECess.Attributes.Add("onkeypress", "OnlyDecimal();")
        btn_Save.Attributes.Add("onclick", "return validation();")
        btn_Update.Attributes.Add("onclick", "return validation();")
        txt_FromDate.Attributes.Add("onkeypress", "return OnlyDate();")
        'txt_FromDate.Text = Format(Now, "dd/MM/yyyy")
        'txt_FromDate.ReadOnly = True
    End Sub
    Private Sub FillFields()
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objcommon.FillDataTable(sqlConn, "ID_FILL_TaxMaster", ViewState("Id"), "TaxId")
            If dt.Rows.Count > 0 Then
                txt_FromDate.Text = dt.Rows(0).Item("FromDate") & ""
                txt_ServTax.Text = dt.Rows(0).Item("Tax") & ""
                txt_Cess.Text = dt.Rows(0).Item("Cess") & ""
                txt_ECess.Text = dt.Rows(0).Item("ECess") & ""
                txt_SBCess.Text = dt.Rows(0).Item("SBCess") & ""
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
            If CBool(ISNameexists("ID_CHECK_ServiceTax", sqlConn, txt_FromDate)) = False Then
                Dim msg As String = "Date cannot be less than or equal to the previous dates"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            Setsaveupdate("ID_INSERT_TaxMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Sub Setsaveupdate(ByVal strproc As String)
        Try
            Dim sqltrans As SqlTransaction
            'sqltrans = sqlConn.BeginTransaction
            OpenConn()
            sqltrans = sqlConn.BeginTransaction
            If SaveUpdate(sqltrans, strproc) = False Then Exit Sub
            sqltrans.Commit()
            Response.Redirect("ServiceTaxDetail.aspx", False)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function SaveUpdate(ByVal sqltrans As SqlTransaction, ByVal strproc As String) As Boolean
        Try
            Dim sqlcomm As New SqlCommand
            sqlcomm.CommandText = strproc
            sqlcomm.CommandType = CommandType.StoredProcedure
            sqlcomm.Transaction = sqltrans
            sqlcomm.Connection = sqlConn
            sqlcomm.Parameters.Clear()
            If Val(ViewState("Id")) = 0 Then
                objcommon.SetCommandParameters(sqlcomm, "@TaxID", SqlDbType.Int, 4, "O")
            Else
                objcommon.SetCommandParameters(sqlcomm, "@TaxID", SqlDbType.BigInt, 4, "I", , , ViewState("Id"))
            End If
            objcommon.SetCommandParameters(sqlcomm, "@FromDate", SqlDbType.SmallDateTime, 9, "I", , , objcommon.DateFormat(txt_FromDate.Text))
            objcommon.SetCommandParameters(sqlcomm, "@Tax", SqlDbType.Float, 9, "I", , , Val(txt_ServTax.Text))
            objcommon.SetCommandParameters(sqlcomm, "@Cess", SqlDbType.Float, 9, "I", , , Val(txt_Cess.Text))
            objcommon.SetCommandParameters(sqlcomm, "@ECess", SqlDbType.Float, 9, "I", , , Val(txt_ECess.Text))
            objcommon.SetCommandParameters(sqlcomm, "@SBCess", SqlDbType.Float, 9, "I", , , Val(txt_SBCess.Text))
            objcommon.SetCommandParameters(sqlcomm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlcomm.ExecuteNonQuery()
            ViewState("Id") = sqlcomm.Parameters("@TaxID").Value
            Return True
        Catch ex As Exception
            sqltrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        Try
            OpenConn()
            If CBool(ISNameexists("ID_CHECK_ServiceTax", sqlConn, txt_FromDate, ViewState("Id"))) = False Then
                Dim msg As String = "Date cannot be less than or equal to the previous dates"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            Setsaveupdate("ID_UPDATE_TaxMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            Response.Redirect("ServiceTaxDetail.aspx", False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Public Function ISNameexists(ByVal strproc As String, ByVal sqlConn1 As SqlConnection, ByVal txtname As TextBox, Optional ByVal intid As Int16 = 0, Optional ByVal intOptid As Int16 = 0, Optional ByVal intyearId As Int16 = 0) As Boolean
        Try
            Dim dt As New DataTable
            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter


            With sqlComm
                sqlComm.CommandText = strproc
                sqlComm.CommandType = CommandType.StoredProcedure
                sqlComm.Connection = sqlConn
                .Parameters.Clear()
                If intid <> 0 Then
                    objcommon.SetCommandParameters(sqlComm, "@intid", SqlDbType.BigInt, 8, "I", , , intid)
                End If
                If intOptid <> 0 Then
                    objcommon.SetCommandParameters(sqlComm, "@intOptid", SqlDbType.BigInt, 8, "I", , , intOptid)
                End If
                If intyearId <> 0 Then
                    objcommon.SetCommandParameters(sqlComm, "@intyearId", SqlDbType.BigInt, 8, "I", , , intyearId)
                End If
                objcommon.SetCommandParameters(sqlComm, "@txtname", SqlDbType.SmallDateTime, 9, "I", , , objcommon.DateFormat(txtname.Text))
                objcommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
                objcommon.SetCommandParameters(sqlComm, "@Valid", SqlDbType.Bit, 1, "O")
                .ExecuteNonQuery()
            End With
            If CBool(sqlComm.Parameters("@Valid").Value) = False Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
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
