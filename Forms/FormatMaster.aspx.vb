Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_FormatMaster
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
            srh_Client.SelectCheckBox.Visible = False
            srh_Client.SelectCheckBox.Checked = False
            If IsPostBack = False Then
                SetAttributes()
                'objCommon.FillControl(lst_SelectFields, clsCommonFuns.sqlConn, "ID_FILL_ClientType", "FaxField", "FieldId")
                'Dim lstItem As ListItem
                'lstItem = chkList_Fields.Items.FindByText("")
                'If lstItem IsNot Nothing Then
                '    chkList_Fields.Items.Remove(lstItem)
                'End If
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
            Else

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
        txt_FormatName.Attributes.Add("onblur", "ConvertUCase(this);")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        'lnk_SelectFields.Attributes.Add("onclick", "return ShowSelectFields()")
    End Sub
    Private Sub FillFields()
        Try
            OpenConn()
            Dim dt As DataTable
            Dim lstItem As ListItem
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_FormatMaster", ViewState("Id"), "FormatId")
            If dt.Rows.Count > 0 Then
                txt_FormatName.Text = Trim(dt.Rows(0).Item("FormatName") & "")
                'For I As Int16 = 0 To dt.Rows.Count - 1
                '    lstItem = chkList_Fields.Items.FindByValue(dt.Rows(I).Item("FieldId").ToString())
                '    If lstItem IsNot Nothing Then
                '        lstItem.Selected = True
                '    End If
                'Next
            End If
            objCommon.FillControl(srh_Client.SelectListBox, sqlConn, "ID_FILL_FormatClients", "CustomerName", "CustomerId", ViewState("Id"), "FormatId")
            lstItem = srh_Client.SelectListBox.Items.FindByText("")
            If lstItem IsNot Nothing Then
                srh_Client.SelectListBox.Items.Remove(lstItem)
            End If
            'If srh_Client.SelectListBox.Items.Count > 0 Then
            '    srh_Client.SelectCheckbox.Checked = False
            'Else
            '    srh_Client.SelectCheckbox.Checked = True
            'End If

            dt = objCommon.FillControl(lst_SelectFields, sqlConn, "ID_FILL_FormatFields", "FaxField", "FieldId", ViewState("Id"), "FormatId")
            lstItem = lst_SelectFields.Items.FindByText("")
            If lstItem IsNot Nothing Then
                lst_SelectFields.Items.Remove(lstItem)
            End If
            'lstItem = srh_SelectFields.SelectListBox.Items.FindByText("")
            'If lstItem IsNot Nothing Then
            '    srh_SelectFields.SelectListBox.Items.Remove(lstItem)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
       
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            If objCommon.CheckDuplicate(sqlConn, "FormatMaster", "FormatName", Trim(txt_FormatName.Text)) = False Then
                Dim msg As String = "This Format Name Already Exist"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_FormatMaster")
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
            If DeleteDetails(sqlTrans) = False Then Exit Sub
            If SaveFormatCustomers(sqlTrans) = False Then Exit Sub
            If SaveFields(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()
            Response.Redirect("FormatDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function SaveFormatCustomers(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            'If srh_Client.SelectCheckbox.Checked = True Then Return True
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_FormatCustomers"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To srh_Client.SelectListBox.Items.Count - 1
                If Val(srh_Client.SelectListBox.Items(I).Value) <> 0 Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , srh_Client.SelectListBox.Items(I).Value)
                    objCommon.SetCommandParameters(sqlComm, "@FormatId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@FormatCustId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
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
            sqlComm.CommandText = "ID_DELETE_OfferLetterFields"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@FormatId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    'Private Function SaveFields(ByVal sqlTrans As SqlTransaction) As Boolean
    '    Try
    '        Dim sqlComm As New SqlCommand
    '        sqlComm.Transaction = sqlTrans
    '        sqlComm.CommandType = CommandType.StoredProcedure
    '        sqlComm.CommandText = "ID_INSERT_OfferLetterFields"
    '        sqlComm.Connection = sqlConn

    '        For I As Int16 = 0 To chkList_Fields.Items.Count - 1
    '            If chkList_Fields.Items(I).Selected = True Then
    '                sqlComm.Parameters.Clear()
    '                objCommon.SetCommandParameters(sqlComm, "@FieldId", SqlDbType.Int, 4, "I", , , chkList_Fields.Items(I).Value)
    '                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
    '                objCommon.SetCommandParameters(sqlComm, "@FormatId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
    '                objCommon.SetCommandParameters(sqlComm, "@FormatCustId", SqlDbType.Int, 4, "O")
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


    Private Function SaveFields(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_OfferLetterFields"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To lst_SelectFields.Items.Count - 1
                If Val(lst_SelectFields.Items(I).Value) <> 0 Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@FieldId", SqlDbType.Int, 4, "I", , , lst_SelectFields.Items(I).Value)

                    objCommon.SetCommandParameters(sqlComm, "@FormatId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@FormatCustId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    'objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@FormatId", SqlDbType.SmallInt, 2, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@FormatId", SqlDbType.SmallInt, 2, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@FormatName", SqlDbType.VarChar, 50, "I", , , Trim(txt_FormatName.Text))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@FormatId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("FormatDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("FormatDetail.aspx")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        Try
            OpenConn()
            If objCommon.CheckDuplicate(sqlConn, "FormatMaster", "FormatName", Trim(txt_FormatName.Text), "FormatId", Val(ViewState("Id"))) = False Then
                Dim msg As String = "This Format Name Already Exist"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_UPDATE_FormatMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
        
    End Sub

    Protected Sub lnk_SelectFields_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_SelectFields.Click
        Dim arrsplitFields As Array
        Dim strRetFields() As String
        Dim strRetValues() As String
        Dim j As Integer
        arrsplitFields = Split(Hid_FieldId.Value, "!")
        If arrsplitFields(0) = "" Then
        Else
            strRetValues = Split(arrsplitFields(0), ",")
            strRetFields = Split(arrsplitFields(1), ",")


            lst_SelectFields.Items.Clear()
            For I As Int16 = 0 To strRetValues.Length - 1
                lst_SelectFields.Items.Add(New ListItem(strRetFields(I), strRetValues(I)))
            Next
        End If

    End Sub

    Private Function BuildReportCond() As String
        Try
            Dim strCond As String = ""
            Dim a As String = Hid_FieldId.Value
            Dim b As String = a.LastIndexOf(",")
            Dim c As String = Mid(Hid_FieldId.Value, 1, b)
            If Hid_FieldId.Value <> "" Then
                strCond += " WHERE FieldId in " & "(" & c & ")"
            End If
            Return strCond
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function


    Private Sub FillList(Optional ByVal intPageIndex As Int16 = 0)
        Try
            Dim sqlDa As New SqlDataAdapter
            Dim dtFill As New DataTable
            Dim Sqlcomm As New SqlCommand

            With Sqlcomm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_FILL_CustFaxFields"
                .Parameters.Clear()
                objCommon.SetCommandParameters(Sqlcomm, "@Cond", SqlDbType.VarChar, 1000, "I", , , BuildReportCond())
                objCommon.SetCommandParameters(Sqlcomm, "@RET_CODE", SqlDbType.Int, 4, "O")
                .ExecuteNonQuery()
            End With
            sqlDa.SelectCommand = Sqlcomm
            sqlDa.Fill(dtFill)
            'If dtFill.Rows.Count > 0 Then
            '    Hid_OrderType.Value = Trim(dtFill.Rows(0).Item("OrderType"))
            'End If
            lst_SelectFields.DataSource = dtFill
            lst_SelectFields.DataTextField = "FaxField"
            lst_SelectFields.DataValueField = "FieldId"
            lst_SelectFields.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally

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
