Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_CustomerTypeMaster
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim lstItem As ListItem
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


            srh_EmpalementDocuments.SelectCheckBox.Visible = False
            srh_EmpalementDocuments.SelectCheckBox.Checked = False
            'srh_EmpalementDocuments.SelectLinkButton.Enabled = True

            srh_KYCDocuments.SelectCheckBox.Visible = False
            srh_KYCDocuments.SelectCheckBox.Checked = False
            'srh_KYCDocuments.SelectLinkButton.Enabled = True

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
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "s1", "SelectDocType();", True)
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
        Try
            txt_CustomerType.Attributes.Add("onblur", "ConvertUCase(this);")
            txt_CustomerType.Attributes.Add("onblur", "ConvertUCase(this);")
            btn_Save.Attributes.Add("onclick", "return Validation();")
            btn_Update.Attributes.Add("onclick", "return Validation();")
            rdo_DocType.Items(0).Attributes.Add("onclick", "SelectDocType()")
            rdo_DocType.Items(1).Attributes.Add("onclick", "SelectDocType()")
            rdo_DocType.Items(2).Attributes.Add("onclick", "SelectDocType()")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

      
    End Sub
    Private Sub SetControls()
        Try
            OpenConn()
            objCommon.FillControl(cbo_Category, sqlConn, "ID_FILL_CustomerTypeCategory", "CustomerTypeCategory", "CustomerTypeCatId")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        'CHANGE
        Try
            OpenConn()
            If objCommon.CheckDuplicate(sqlConn, "CustomerTypeMaster", "CustomerTypeName", Trim(txt_CustomerType.Text)) = False Then
                Dim msg As String = "This Customer Type Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_CustomerTypeMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
       
    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        'CHANGE
        Try
            OpenConn()
            If objCommon.CheckDuplicate(sqlConn, "CustomerTypeMaster", "CustomerTypeName", Trim(txt_CustomerType.Text), "CustomerTypeId", Val(ViewState("Id"))) = False Then
                Dim msg As String = "This Customer Type Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_UPDATE_CustomerTypeMaster")
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
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub

            If rdo_DocType.SelectedValue = "B" Then
                If DeleteDetails(sqlTrans) = False Then Exit Sub
                If SaveKycDocuments(sqlTrans) = False Then Exit Sub
                If DeleteEmpDetails(sqlTrans) = False Then Exit Sub
                If SaveEmpDocuments(sqlTrans) = False Then Exit Sub
            ElseIf rdo_DocType.SelectedValue = "K" Then
                If DeleteDetails(sqlTrans) = False Then Exit Sub
                If SaveKycDocuments(sqlTrans) = False Then Exit Sub
            Else
                If DeleteEmpDetails(sqlTrans) = False Then Exit Sub
                If SaveEmpDocuments(sqlTrans) = False Then Exit Sub
            End If
            sqlTrans.Commit()
            Response.Redirect("CustomerTypeDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
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
                objCommon.SetCommandParameters(sqlComm, "@CustomerTypeId", SqlDbType.SmallInt, 2, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@CustomerTypeId", SqlDbType.SmallInt, 2, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@CustomerTypeName", SqlDbType.VarChar, 100, "I", , , Trim(txt_CustomerType.Text))
            objCommon.SetCommandParameters(sqlComm, "@DocType", SqlDbType.Char, 1, "I", , , Trim(rdo_DocType.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@CustomerTypeCatId", SqlDbType.Int, 4, "I", , , Val(cbo_Category.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@Abbreviation", SqlDbType.VarChar, 100, "I", , , Trim(txt_CustomerAbbr.Text))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@CustomerTypeId").Value

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    'Private Function SaveDocuments(ByVal sqlTrans As SqlTransaction) As Boolean
    '    Try
    '        Dim sqlComm As New SqlCommand
    '        sqlComm.Transaction = sqlTrans
    '        sqlComm.CommandType = CommandType.StoredProcedure
    '        sqlComm.CommandText = "ID_INSERT_CustomerDocuments"
    '        sqlComm.Connection = clsCommonFuns.sqlConn
    '        For I As Int16 = 0 To chkList_Documents.Items.Count - 1
    '            If chkList_Documents.Items(I).Selected = True Then
    '                sqlComm.Parameters.Clear()
    '                objCommon.SetCommandParameters(sqlComm, "@DocumentTypeId", SqlDbType.Int, 4, "I", , , chkList_Documents.Items(I).Value)
    '                objCommon.SetCommandParameters(sqlComm, "@CustomerTypeId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
    '                objCommon.SetCommandParameters(sqlComm, "@CustomerDocId", SqlDbType.Int, 4, "O")
    '                objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
    '                sqlComm.ExecuteNonQuery()
    '            End If
    '        Next
    '        Return True
    '    Catch ex As Exception
    '        sqlTrans.Rollback()
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Function

    Private Function SaveEmpDocuments(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_CustomerEmptypedocument"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To srh_EmpalementDocuments.SelectListBox.Items.Count - 1
                If Val(srh_EmpalementDocuments.SelectListBox.Items(I).Value) <> 0 Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@DocumentId", SqlDbType.Int, 4, "I", , , srh_EmpalementDocuments.SelectListBox.Items(I).Value)
                    objCommon.SetCommandParameters(sqlComm, "@CustomerTypeId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@CustomerEmpDocId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")

                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function SaveKycDocuments(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_CustomerDocumentsNormal1"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To srh_KYCDocuments.SelectListBox.Items.Count - 1
                If Val(srh_KYCDocuments.SelectListBox.Items(I).Value) <> 0 Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@DocumentId", SqlDbType.Int, 4, "I", , , srh_KYCDocuments.SelectListBox.Items(I).Value)
                    objCommon.SetCommandParameters(sqlComm, "@CustomerTypeId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@CustomerDocId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")

                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function DeleteDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_CustomerTypeDocumentsNormal"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@CustomerTypeId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function DeleteEmpDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_CustomerTypeEmpDocuments"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@CustomerTypeId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("CustomerTypeDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("CustomerTypeDetail.aspx")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillFields()
        'CHANGE 
        Try
            OpenConn()
            Dim dt As DataTable
            Dim dt1 As DataTable
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_CustomerTypeMaster", ViewState("Id"), "CustomerTypeId")
            If dt.Rows.Count > 0 Then
                txt_CustomerType.Text = Trim(dt.Rows(0).Item("CustomerTypeName") & "")
                rdo_DocType.SelectedValue = Trim(dt.Rows(0).Item("DocType") & "")
                txt_CustomerAbbr.Text = Trim(dt.Rows(0).Item("Abbreviation") & "")
            End If

            If rdo_DocType.SelectedValue = "B" Then
                objCommon.FillControl(srh_KYCDocuments.SelectListBox, sqlConn, "ID_FILL_DocumentTypes1", "DocumentName", "DocumentId", ViewState("Id"), "CustomerTypeId")
                lstItem = srh_KYCDocuments.SelectListBox.Items.FindByText("")
                If lstItem IsNot Nothing Then
                    srh_KYCDocuments.SelectListBox.Items.Remove(lstItem)
                End If

                objCommon.FillControl(srh_EmpalementDocuments.SelectListBox, sqlConn, "ID_FILL_EmpDocumentTypes1", "DocumentName", "DocumentId", ViewState("Id"), "CustomerTypeId")
                lstItem = srh_EmpalementDocuments.SelectListBox.Items.FindByText("")
                If lstItem IsNot Nothing Then
                    srh_EmpalementDocuments.SelectListBox.Items.Remove(lstItem)
                End If
            ElseIf rdo_DocType.SelectedValue = "K" Then
                objCommon.FillControl(srh_KYCDocuments.SelectListBox, sqlConn, "ID_FILL_DocumentTypes1", "DocumentName", "DocumentId", ViewState("Id"), "CustomerTypeId")
                lstItem = srh_KYCDocuments.SelectListBox.Items.FindByText("")
                If lstItem IsNot Nothing Then
                    srh_KYCDocuments.SelectListBox.Items.Remove(lstItem)
                End If
            Else
                objCommon.FillControl(srh_EmpalementDocuments.SelectListBox, sqlConn, "ID_FILL_EmpDocumentTypes1", "DocumentName", "DocumentId", ViewState("Id"), "CustomerTypeId")
                lstItem = srh_EmpalementDocuments.SelectListBox.Items.FindByText("")
                If lstItem IsNot Nothing Then
                    srh_EmpalementDocuments.SelectListBox.Items.Remove(lstItem)
                End If
            End If
            objCommon.FillControl(cbo_Category, sqlConn, "ID_FILL_CustomerTypeCategory", "CustomerTypeCategory", "CustomerTypeCatId")
            If dt.Rows.Count > 0 Then
                cbo_Category.SelectedValue = Val(dt.Rows(0).Item("CustomerTypeCatId") & "")
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
