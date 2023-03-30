Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_SGLMaster
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
        txt_SGLBankName.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_AccountNo.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_Address1.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_Address2.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_City.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_ContactPerson.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_SGLBranch.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_FaxNo.Attributes.Add("onkeypress", "OnlyInteger();")
        txt_Pincode.Attributes.Add("onkeypress", "OnlyInteger();")
        txt_MoblieNo.Attributes.Add("onkeypress", "OnlyInteger();")
        txt_PhoneNo.Attributes.Add("onkeypress", "OnlyInteger();")
        txt_RTGScode.Attributes.Add("onblur", "ConvertUCase(this);")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")

    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        'CHANGE
        Try
            OpenConn()
            If objCommon.CheckDuplicate(sqlConn, "SGLMaster", "BankName", Trim(txt_SGLBankName.Text), , , "CompId", Session("CompId")) = False Then
                Dim msg As String = "This Bank Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_SGLMaster")
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
            If objCommon.CheckDuplicate(sqlConn, "SGLMaster", "BankName", Trim(txt_SGLBankName.Text), "SGLId", Val(ViewState("Id"))) = False Then
                Dim msg As String = "This Bank Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_UPDATE_SGLMaster")
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
            sqlTrans.Commit()
            Response.Redirect("SGLMasterDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
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
                objCommon.SetCommandParameters(sqlComm, "@SGLId", SqlDbType.SmallInt, 2, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@SGLId", SqlDbType.SmallInt, 2, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@BankName", SqlDbType.VarChar, 100, "I", , , Trim(txt_SGLBankName.Text))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@Branch", SqlDbType.VarChar, 100, "I", , , Trim(txt_SGLBranch.Text))
            objCommon.SetCommandParameters(sqlComm, "@AccountNo", SqlDbType.VarChar, 100, "I", , , Trim(txt_AccountNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@ContactPerson", SqlDbType.VarChar, 100, "I", , , Trim(txt_ContactPerson.Text))
            objCommon.SetCommandParameters(sqlComm, "@Address1", SqlDbType.VarChar, 100, "I", , , Trim(txt_Address1.Text))
            objCommon.SetCommandParameters(sqlComm, "@Address2", SqlDbType.VarChar, 100, "I", , , Trim(txt_Address2.Text))
            objCommon.SetCommandParameters(sqlComm, "@City", SqlDbType.VarChar, 100, "I", , , Trim(txt_City.Text))
            objCommon.SetCommandParameters(sqlComm, "@Pincode", SqlDbType.VarChar, 100, "I", , , Trim(txt_Pincode.Text))
            objCommon.SetCommandParameters(sqlComm, "@PhoneNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_PhoneNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@FaxNo", SqlDbType.VarChar, 100, "I", , , Trim(txt_FaxNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@MobileNo", SqlDbType.VarChar, 100, "I", , , Trim(txt_MoblieNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@GiltAccNo", SqlDbType.VarChar, 100, "I", , , Trim(txt_GiltAccNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@SGLEmail", SqlDbType.VarChar, 200, "I", , , Trim(txt_EmailId.Text))
            objCommon.SetCommandParameters(sqlComm, "@SGLRTGScode", SqlDbType.VarChar, 15, "I", , , Trim(txt_RTGScode.Text))
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@SGLId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("SGLMasterDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("SGLMasterDetail.aspx")
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
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_SGLMaster", ViewState("Id"), "SGLId")
            If dt.Rows.Count > 0 Then
                txt_SGLBankName.Text = Trim(dt.Rows(0).Item("BankName") & "")
                txt_SGLBranch.Text = Trim(dt.Rows(0).Item("Branch") & "")
                txt_AccountNo.Text = Trim(dt.Rows(0).Item("AccountNo") & "")
                txt_ContactPerson.Text = Trim(dt.Rows(0).Item("ContactPerson") & "")
                txt_Address1.Text = Trim(dt.Rows(0).Item("Address1") & "")
                txt_Address2.Text = Trim(dt.Rows(0).Item("Address2") & "")
                txt_City.Text = Trim(dt.Rows(0).Item("City") & "")
                txt_Pincode.Text = Trim(dt.Rows(0).Item("Pincode") & "")
                txt_PhoneNo.Text = Trim(dt.Rows(0).Item("PhoneNo") & "")
                txt_FaxNo.Text = Trim(dt.Rows(0).Item("FaxNo") & "")
                txt_MoblieNo.Text = Trim(dt.Rows(0).Item("MobileNo") & "")
                txt_GiltAccNo.Text = Trim(dt.Rows(0).Item("GiltAccNo") & "")
                txt_EmailId.Text = Trim(dt.Rows(0).Item("SGLEmail") & "")
                txt_RTGScode.Text = Trim(dt.Rows(0).Item("SGLRTGScode") & "")
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
            sqlConn.Dispose()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
