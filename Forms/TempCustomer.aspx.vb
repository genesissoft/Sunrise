Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_TempCustomer
    Inherits System.Web.UI.Page
    Dim sqlConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        Try
            If (IsPostBack = False) Then
                SetAttributes()
                If Trim(Request.QueryString("AddCustomer") & "") <> "" Then
                    btn_Update.Visible = False
                End If
                If Val(Request.QueryString("CustomerId") & "") <> 0 Then
                    ViewState("Id") = Val(Request.QueryString("CustomerId").ToString())
                    Session("Cust_Id") = Val(Request.QueryString("CustomerId").ToString())
                    btn_Save.Visible = False
                    FillField()
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
        txt_CustomerName.Attributes.Add("onblur", "ConvertUCase(this)")
        txt_ContactPerson.Attributes.Add("onblur", "ConvertUCase(this)")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
    End Sub
    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            SaveUpdate("ID_INSERT_TempCustomer") 'ReturnValues

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "close", "Close('');", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub SaveUpdate(ByVal strProc As String)
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            sqlComm.Parameters.Clear()
            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@CustomerName", SqlDbType.VarChar, 100, "I", , , Trim(txt_CustomerName.Text))
            objCommon.SetCommandParameters(sqlComm, "@ContactPerson", SqlDbType.VarChar, 100, "I", , , Trim(txt_ContactPerson.Text))
            objCommon.SetCommandParameters(sqlComm, "@PhoneNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_PhoneNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@MobileNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_MobileNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@Email", SqlDbType.VarChar, 50, "I", , , Trim(txt_EMailId.Text))
            objCommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        SaveUpdate("ID_UPDATE_TempCustomer")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "update", "Close('Update');", True)
    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            sqlConn.Dispose()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub
    Private Function FillField()
        Try

            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter
            Dim dtFill As New DataTable
            Dim dvFill As DataView

            OpenConn()

            'Dim sqlrd As SqlDataReader

            sqlComm.Connection = sqlConn
            With sqlComm
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_FILL_TempCustomer"
                .Parameters.Clear()
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , ViewState("Id"))
                objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(Session("UserId")))
                objCommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
                .ExecuteNonQuery()
                sqlDa.SelectCommand = sqlComm
                sqlDa.Fill(dtFill)
                dvFill = dtFill.DefaultView
            End With
            'sqlrd = sqlComm.ExecuteReader
            If dtFill.Rows.Count > 0 Then
                txt_CustomerName.Text = Trim(dtFill.Rows(0).Item("CustomerName") & "")
                txt_ContactPerson.Text = Trim(dtFill.Rows(0).Item("ContactPerson") & "")
                txt_PhoneNo.Text = Trim(dtFill.Rows(0).Item("PhoneNo") & "")
                txt_MobileNo.Text = Trim(dtFill.Rows(0).Item("MobileNo") & "")
                txt_EMailId.Text = Trim(dtFill.Rows(0).Item("EmailId") & "")
            End If
            'sqlrd.Close()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
        Return ""
    End Function

End Class
