Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_DocumentMaster
    Inherits System.Web.UI.Page

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
                Page.SetFocus(txt_Document)
                If Val(Request.QueryString("Id") & "") <> 0 Then
                    ViewState("Id") = Val(Request.QueryString("Id") & "")
                    FillFields()
                    txt_Document.Focus()
                    btn_Save.Visible = False
                    btn_Update.Visible = True

                End If
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    Private Sub SetAttributes()
        txt_Document.Attributes.Add("onblur", "ConvertUCase(this);")
        btn_Save.Attributes.Add("onclick", "return CommonValidation();")
        btn_Update.Attributes.Add("onclick", "return CommonValidation();")
        btn_Update.Visible = False
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        'CHANGE


        If CBool(objCommon.CheckDuplicate(clsCommonFuns.sqlConn, "DocumentTypeMaster", "DocumentTypeName", txt_Document.Text)) = False Then

            Dim msg As String = "This Document Name Already Exist"
            Dim strHtml As String
            strHtml = "alert('" + msg + "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
            Exit Sub
        End If
        SetSaveUpdate("ID_INSERT_DocumentTypeMaster")
    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        'CHANGEViewState("Id")
        If objCommon.CheckDuplicate(clsCommonFuns.sqlConn, "DocumentTypeMaster", "DocumentTypeName", Trim(txt_Document.Text), "DocumentTypeId", Val(ViewState("Id"))) = False Then
            Dim msg As String = "This Document Name Already Exist"
            Dim strHtml As String
            strHtml = "alert('" + msg + "');"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
            Exit Sub
        End If
        SetSaveUpdate("ID_UPDATE_DocumentTypeMaster")
    End Sub

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            Response.Redirect("DocumentDetail .aspx", False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            sqlTrans = clsCommonFuns.sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            sqlTrans.Commit()
            Response.Redirect("DocumentDetail .aspx?Id=" & ViewState("Id"), False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            'CHANGE 
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = clsCommonFuns.sqlConn

            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@DocumentTypeId", SqlDbType.SmallInt, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@DocumentTypeId", SqlDbType.SmallInt, 4, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@DocumentTypeName", SqlDbType.VarChar, 100, "I", , , Trim(txt_Document.Text))
            objCommon.SetCommandParameters(sqlComm, "@DocType", SqlDbType.Char, 1, "I", , , Trim(rdo_DocType.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@DocumentTypeId").Value

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Sub FillFields()
        'CHANGE 
        Dim dt As DataTable
        dt = objCommon.FillDataTable(clsCommonFuns.sqlConn, "Id_FILL_DocumentType", ViewState("Id"), "DocumentTypeId")
        If dt.Rows.Count > 0 Then
            txt_Document.Text = Trim(dt.Rows(0).Item("DocumentTypeName") & "")
            rdo_DocType.SelectedValue = Trim(dt.Rows(0).Item("DocType") & "")

        End If
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
       
    End Sub
End Class
