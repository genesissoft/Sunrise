Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_SubCategoryMaster
    Inherits System.Web.UI.Page
    Dim objcomm As New clsCommonFuns
    Dim sqlConn As New SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'If Val(Session("UserId") & "") = 0 Then
            '    Response.Redirect("Login.aspx", False)
            '    Exit Sub
            'End If
            'objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            If IsPostBack = False Then
                'Hid_CompId.Value = Val(Session("CompId"))
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
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "select", "CheckType();", True)
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
        txt_Subcategory.Attributes.Add("onblur", "ConvertUCase(this);")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
        rbl_UserTypeSection.Attributes.Add("onclick", "CheckType();")
    End Sub
    Private Sub showrows()
        If (rbl_UserTypeSection.SelectedValue = "T") Then
            cbo_CustomerType.Visible = False
            cbo_Category.Visible = True
        ElseIf (rbl_UserTypeSection.SelectedValue = "T") Then
            cbo_CustomerType.Visible = True
            cbo_Category.Visible = False
        End If
    End Sub
    Private Sub SetControls()
        Try
            OpenConn()
            objCommon.FillControl(cbo_Category, sqlConn, "ID_FILL_CategoryMaster", "CategoryName", "CategoryId")
            objCommon.FillControl(cbo_CustomerType, sqlConn, "ID_FILL_CustomerTypeMaster", "CustomerTypeName", "CustomerTypeId")

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            If CBool(objCommon.CheckDuplicate(sqlConn, "SubCategoryMaster", "SubCategoryName", txt_Subcategory.Text)) = False Then
                Dim msg As String = "This Sub Category Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_SubCategoryMaster")
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
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            sqlTrans.Commit()
            Response.Redirect("SubCategoryDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
                objCommon.SetCommandParameters(sqlComm, "@SubCategoryId", SqlDbType.SmallInt, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@SubCategoryId", SqlDbType.SmallInt, 4, "I", , , ViewState("Id"))
            End If
            'objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@SubCategoryName", SqlDbType.VarChar, 100, "I", , , Trim(txt_Subcategory.Text))
            objCommon.SetCommandParameters(sqlComm, "@Type", SqlDbType.Char, 1, "I", , , Trim(rbl_UserTypeSection.SelectedValue))
            If rbl_UserTypeSection.SelectedValue = "T" Then
                objCommon.SetCommandParameters(sqlComm, "@TypeCatId", SqlDbType.Int, 4, "I", , , Val(cbo_CustomerType.SelectedValue))
            End If
            If rbl_UserTypeSection.SelectedValue = "C" Then
                objCommon.SetCommandParameters(sqlComm, "@TypeCatId", SqlDbType.Int, 4, "I", , , Val(cbo_Category.SelectedValue))
            End If
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@SubCategoryId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("SubCategoryDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("SubCategoryDetail.aspx")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillFields()
        'CHANGE 
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable1(sqlConn, "Id_FILL_SubCategoryMaster", ViewState("Id"), "SubCategoryId")
            If dt.Rows.Count > 0 Then
                txt_Subcategory.Text = Trim(dt.Rows(0).Item("SubCategoryName") & "")
                rbl_UserTypeSection.SelectedValue = Trim(dt.Rows(0).Item("Type") & "")
                If rbl_UserTypeSection.SelectedValue = "C" Then
                    cbo_Category.SelectedValue = Val(dt.Rows(0).Item("TypeCatId") & "")
                End If
                If rbl_UserTypeSection.SelectedValue = "T" Then
                    cbo_CustomerType.SelectedValue = Val(dt.Rows(0).Item("TypeCatId") & "")
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        Try
            OpenConn()
            If CBool(objCommon.CheckDuplicate(sqlConn, "SubCategoryMaster", "SubCategoryName", txt_Subcategory.Text, "SubCategoryId", Val(ViewState("Id")))) = False Then
                Dim msg As String = "This Sub Category Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_UPDATE_SubCategoryMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

        'showrows()
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            sqlConn.Dispose()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
