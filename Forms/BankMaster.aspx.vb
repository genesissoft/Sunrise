Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_BankMaster
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As New SqlConnection

    'bankname
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
                'CHANGE
                Hid_CompId.Value = Val(Session("CompId"))
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
        txt_AccountNumber.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_Location.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_MobNo.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_Email.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_BankName.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_Branch.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_ContactPerson.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_IFSCcode.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_RTGSCode.Attributes.Add("onblur", "ConvertUCase(this);")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
    End Sub

    Private Sub FillFields()
        'CHANGE 
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable1(sqlConn, "ID_FILL_BankMaster", ViewState("Id"), "BankId")
            If dt.Rows.Count > 0 Then
                txt_AccountNumber.Text = Trim(dt.Rows(0).Item("AccountNumber") & "")
                txt_Location.Text = Trim(dt.Rows(0).Item("Location") & "")
                txt_MobNo.Text = Trim(dt.Rows(0).Item("MobNo") & "")
                txt_Email.Text = Trim(dt.Rows(0).Item("Email") & "")
                txt_BankName.Text = Trim(dt.Rows(0).Item("BankName") & "")
                txt_Branch.Text = Trim(dt.Rows(0).Item("Branch") & "")
                txt_ContactPerson.Text = Trim(dt.Rows(0).Item("ContactPerson") & "")
                txt_FaxNo.Text = Trim(dt.Rows(0).Item("FaxNo") & "")
                txt_PhoneNo.Text = Trim(dt.Rows(0).Item("PhoneNo") & "")
                txt_IFSCcode.Text = Trim(dt.Rows(0).Item("IFSCCode") & "")
                txt_RTGSCode.Text = Trim(dt.Rows(0).Item("RTGScode") & "")
                If Convert.ToString((dt.Rows(0).Item("DefaultBank"))) = "" Then
                    chkDefault.Checked = False
                Else
                    chkDefault.Checked = dt.Rows(0).Item("DefaultBank")

                End If

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
            If objCommon.CheckDuplicate(sqlConn, "BankMaster", "AccountNumber", Trim(txt_AccountNumber.Text), , , "CompId", Session("CompId")) = False Then
                Dim msg As String = "This Account Number Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_BankMaster")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            'OpenConn()
            'sqlTrans = clsCommonFuns.sqlConn.BeginTransaction
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            sqlTrans.Commit()
            'CloseConn()
            Response.Redirect("BankDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
                objCommon.SetCommandParameters(sqlComm, "@BankId", SqlDbType.SmallInt, 2, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@BankId", SqlDbType.SmallInt, 2, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@BankName", SqlDbType.VarChar, 50, "I", , , Trim(txt_BankName.Text))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@Branch", SqlDbType.VarChar, 50, "I", , , Trim(txt_Branch.Text))
            objCommon.SetCommandParameters(sqlComm, "@AccountNumber", SqlDbType.VarChar, 15, "I", , , Trim(txt_AccountNumber.Text))
            objCommon.SetCommandParameters(sqlComm, "@ContactPerson", SqlDbType.VarChar, 50, "I", , , Trim(txt_ContactPerson.Text))
            objCommon.SetCommandParameters(sqlComm, "@PhoneNo", SqlDbType.VarChar, 15, "I", , , Trim(txt_PhoneNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@FaxNo", SqlDbType.VarChar, 15, "I", , , Trim(txt_FaxNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@Location", SqlDbType.VarChar, 15, "I", , , Trim(txt_Location.Text))
            objCommon.SetCommandParameters(sqlComm, "@MobNo", SqlDbType.VarChar, 15, "I", , , Trim(txt_MobNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@Email", SqlDbType.VarChar, 15, "I", , , Trim(txt_Email.Text))
            objCommon.SetCommandParameters(sqlComm, "@IFSCCode", SqlDbType.VarChar, 15, "I", , , Trim(txt_IFSCcode.Text))
            objCommon.SetCommandParameters(sqlComm, "@RTGScode", SqlDbType.VarChar, 15, "I", , , Trim(txt_RTGSCode.Text))
            objCommon.SetCommandParameters(sqlComm, "@DefaultBank", SqlDbType.Bit, 1, "I", , , chkDefault.Checked)
            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Hid_CompId.Value))
            End If
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@BankId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("BankDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("BankDetail.aspx")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        Try
            OpenConn()
            If objCommon.CheckDuplicate(sqlConn, "BankMaster", "AccountNumber", Trim(txt_AccountNumber.Text), "BankId", ViewState("Id"), "CompId", Session("CompId")) = False Then
                Dim msg As String = "This Account Number Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_UPDATE_BankMaster")
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





