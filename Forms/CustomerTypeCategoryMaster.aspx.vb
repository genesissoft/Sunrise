Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_CustomerTypeCategoryMaster
    Inherits System.Web.UI.Page
    Dim sqlConn As New SqlConnection
    Dim objCommon As New clsCommonFuns
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'If Val(Session("UserId") & "") = 0 Or clsCommonFuns.sqlConn Is Nothing Then
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
                Page.SetFocus(txt_CustTypCategory)
                If Request.QueryString("Id") <> "" Then
                    Dim strId As String = objCommon.DecryptText(HttpUtility.UrlDecode(Request.QueryString("Id")))
                    ViewState("Id") = Val(strId)
                    FillFields()
                    btn_Save.Visible = False
                    btn_Update.Visible = True
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
        txt_CustTypCategory.Attributes.Add("onblur", "ConvertUCase(this);")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
    End Sub
    Private Sub FillFields()
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable1(sqlConn, "ID_FILL_CustTypeCategoryMaster", ViewState("Id"), "CustomerTypeCatId")
            If dt.Rows.Count > 0 Then
                txt_CustTypCategory.Text = Trim(dt.Rows(0).Item("CustomerTypeCategory") & "")
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
            If objCommon.CheckDuplicate(sqlConn, "CustomerTypeCategoryMaster", "CustomerTypeCategory", Trim(txt_CustTypCategory.Text), , , ) = False Then
                Dim msg As String = "This Category Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_CustTypeCategoryMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Response.Redirect("CustomerTypCatDetails.aspx", False)
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
                objCommon.SetCommandParameters(sqlComm, "@CustomerTypeCatId", SqlDbType.Int, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@CustomerTypeCatId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@CustomerTypeCategory", SqlDbType.VarChar, 100, "I", , , Trim(txt_CustTypCategory.Text))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@CustomerTypeCatId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        Try
            OpenConn()
            If objCommon.CheckDuplicate(sqlConn, "CustomerTypeCategoryMaster", "CustomerTypeCategory", Trim(txt_CustTypCategory.Text), "CustomerTypeCatId", Val(ViewState("Id"))) = False Then
                Dim msg As String = "This Category Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_UPDATE_CustTypeCategoryMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Protected Sub btn_Back_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Back.Click
        Try
            Response.Redirect("CustomerTypCatDetails.aspx", False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
