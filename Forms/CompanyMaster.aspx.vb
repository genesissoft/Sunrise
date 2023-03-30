Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_CompanyMaster
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As SqlConnection
    Dim bytHeader() As Byte
    Dim bytFooter() As Byte
    Dim strHeaderType As String
    Dim strFooterType As String
    Dim strHeaderFileName As String
    Dim strFooterFileName As String
    Dim lngHeaderLen As Long
    Dim lngFooterLen As Long

    Dim intWidth As Integer = 175
    Dim intHeight As Integer = 450
    Dim strErrorHeader As String = "Validation"

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
                    If Hid_prevHeaderFileName.Value <> "" Then
                        btn_ShowHeader.Visible = True
                        row_HeaderMsg.Visible = False
                    Else
                        btn_ShowHeader.Visible = False
                        row_HeaderMsg.Visible = True
                    End If
                    If Hid_prevFooterFileName.Value <> "" Then
                        btn_showFooter.Visible = True
                        row_Footermsg.Visible = False
                    Else
                        btn_showFooter.Visible = False
                        row_Footermsg.Visible = True
                    End If
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
        txt_CompName.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_PANNo.Attributes.Add("onblur", "ConvertUCase(this);")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
        txt_TDAccountNo.Attributes.Add("onblur", "ConvertUCase(this);")
        file_Header.Attributes.Add("onchange", "ImageChange('Hid_UpdateHeaderFlag')")
        File_Footer.Attributes.Add("onchange", "ImageChange('Hid_UpdateFooterFlag')")
    End Sub

    Private Sub FillFields()
        'CHANGE 
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_CompanyMaster", ViewState("Id"), "CompId")
            If dt.Rows.Count > 0 Then
                txt_CompName.Text = Trim(dt.Rows(0).Item("CompName") & "")
                txt_confirm.Text = Trim(dt.Rows(0).Item("ConfirmationText") & "")
                txt_PANNo.Text = Trim(dt.Rows(0).Item("PANNo") & "")
                txt_PhoneNo.Text = Trim(dt.Rows(0).Item("PhoneNo") & "")
                txt_FaxNo.Text = Trim(dt.Rows(0).Item("FaxNo") & "")
                txt_City.Text = Trim(dt.Rows(0).Item("City") & "")
                txt_PinCode.Text = Trim(dt.Rows(0).Item("PinNo") & "")
                txt_Address.Text = Trim(dt.Rows(0).Item("Adderess") & "")
                txt_Address2.Text = Trim(dt.Rows(0).Item("Address1") & "")
                txt_ServiceTaxNo.Text = Trim(dt.Rows(0).Item("ServiceTaxNo") & "")
                txt_GSTNo.Text = Trim(dt.Rows(0).Item("CompanyGST") & "")
                txt_TDAccountNo.Text = Trim(dt.Rows(0).Item("TDAccountNo") & "")
                Hid_prevHeaderFileName.Value = Trim(dt.Rows(0).Item("HeaderFileName") & "")
                Hid_prevFooterFileName.Value = Trim(dt.Rows(0).Item("FooterFileName") & "")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try


    End Sub
    Private Function Add_HeaderFile() As Boolean
        Try
            Dim strHeader() As String
            Dim strFooter() As String
            bytHeader = file_Header.FileBytes
            bytFooter = File_Footer.FileBytes
            If bytHeader.Length <> 0 Then
                strHeaderType = file_Header.PostedFile.ContentType
                strHeader = Split(file_Header.PostedFile.FileName, "\")
                strHeaderFileName = strHeader(UBound(strHeader))
                Hid_HeaderFileName.Value = strHeaderFileName
                lngHeaderLen = file_Header.PostedFile.ContentLength
                strHeader = Split(file_Header.PostedFile.FileName, ".")
            End If
            If bytFooter.Length <> 0 Then
                strFooterType = File_Footer.PostedFile.ContentType
                strFooter = Split(File_Footer.PostedFile.FileName, "\")
                strFooterFileName = strFooter(UBound(strFooter))
                Hid_FooterFileName.Value = strFooterFileName
                lngFooterLen = File_Footer.PostedFile.ContentLength
                strFooter = Split(File_Footer.PostedFile.FileName, ".")

            End If

            Return True
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
        End Try
    End Function

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            If objCommon.CheckDuplicate(sqlConn, "CompanyMaster", "CompName", Trim(txt_CompName.Text)) = False Then
                Dim msg As String = "This Company Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_CompanyMaster")

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try


    End Sub

    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub

            sqlTrans.Commit()
            Response.Redirect("CompanyDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
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
                objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.SmallInt, 2, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.SmallInt, 2, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@CompName", SqlDbType.VarChar, 100, "I", , , Trim(txt_CompName.Text))
            objCommon.SetCommandParameters(sqlComm, "@ConfirmationText", SqlDbType.VarChar, 200, "I", , , Trim(txt_confirm.Text))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@PANNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_PANNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@PhoneNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_PhoneNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@FaxNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_FaxNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@City", SqlDbType.VarChar, 50, "I", , , Trim(txt_City.Text))
            objCommon.SetCommandParameters(sqlComm, "@PinNo", SqlDbType.VarChar, 50, "I", , , Trim(txt_PinCode.Text))
            objCommon.SetCommandParameters(sqlComm, "@Adderess", SqlDbType.VarChar, 500, "I", , , Trim(txt_Address.Text))
            objCommon.SetCommandParameters(sqlComm, "@Address1", SqlDbType.VarChar, 500, "I", , , Trim(txt_Address2.Text))
            objCommon.SetCommandParameters(sqlComm, "@ServiceTaxNo", SqlDbType.VarChar, 100, "I", , , Trim(txt_ServiceTaxNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@Checked", SqlDbType.Char, 1, "I", , , 0)
            objCommon.SetCommandParameters(sqlComm, "@CompanyGST", SqlDbType.VarChar, 100, "I", , , Trim(txt_GSTNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@TDAccountNo", SqlDbType.VarChar, 100, "I", , , Trim(txt_TDAccountNo.Text))
            If Add_HeaderFile() = True Then
                If Hid_UpdateHeaderFlag.Value = "TRUE" Then
                    objCommon.SetCommandParameters(sqlComm, "@HeaderData", SqlDbType.Image, 0, "I", , , bytHeader)
                    objCommon.SetCommandParameters(sqlComm, "@HeaderType", SqlDbType.VarChar, 50, "I", , , strHeaderType)
                    objCommon.SetCommandParameters(sqlComm, "@HeaderLength", SqlDbType.Int, 4, "I", , , lngHeaderLen)
                    objCommon.SetCommandParameters(sqlComm, "@HeaderFileName", SqlDbType.VarChar, 100, "I", , , strHeaderFileName)
                    objCommon.SetCommandParameters(sqlComm, "@UpdateHeaderFlag", SqlDbType.Bit, 1, "I", , , True)
                Else
                    objCommon.SetCommandParameters(sqlComm, "@UpdateHeaderFlag", SqlDbType.Bit, 1, "I", , , False)
                End If
                If Hid_UpdateFooterFlag.Value = "TRUE" Then
                    objCommon.SetCommandParameters(sqlComm, "@FooterType", SqlDbType.VarChar, 50, "I", , , strFooterType)
                    objCommon.SetCommandParameters(sqlComm, "@FooterLength", SqlDbType.Int, 4, "I", , , lngFooterLen)
                    objCommon.SetCommandParameters(sqlComm, "@FooterFileName", SqlDbType.VarChar, 100, "I", , , strFooterFileName)
                    objCommon.SetCommandParameters(sqlComm, "@FooterData", SqlDbType.Image, 0, "I", , , bytFooter)
                    objCommon.SetCommandParameters(sqlComm, "@UpdateFooterFlag", SqlDbType.Bit, 1, "I", , , True)
                Else
                    objCommon.SetCommandParameters(sqlComm, "@UpdateFooterFlag", SqlDbType.Bit, 1, "I", , , False)
                End If
            End If
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@CompId").Value

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("CompanyDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("CompanyDetail.aspx")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        Try
            OpenConn()
            If objCommon.CheckDuplicate(sqlConn, "CompanyMaster", "CompName", Trim(txt_CompName.Text), "CompId", Val(ViewState("Id"))) = False Then
                Dim msg As String = "This Company Name Already Exist"
                Dim strHtml As String
                strHtml = "AlertMessage(Validation,'" + msg + "', 175,450);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_UPDATE_CompanyMaster")

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

    Private Sub btn_showFooter_Click(sender As Object, e As EventArgs) Handles btn_showFooter.Click
        Try
            OpenConn()
            ShowImage("Footer")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Private Sub btn_ShowHeader_Click(sender As Object, e As EventArgs) Handles btn_ShowHeader.Click
        Try
            OpenConn()
            ShowImage("Header")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub ShowImage(ByVal strType As String)
        Try
            ' OpenConn()
            Dim dt As DataTable
            Dim arrContent() As Byte
            dt = GetDetails()
            'OpenConn()
            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                arrContent = CType(dr.Item(strType & "Data"), Byte())
                Dim conType As String = dr.Item(strType & "Type").ToString()
                Response.Clear()
                Response.Charset = ""
                Response.ClearHeaders()
                Response.ContentType = conType
                Response.AddHeader("content-disposition", "attachment;filename=" & dr.Item(strType & "FileName"))
                Response.BinaryWrite(arrContent)
                Response.Flush()
                'Response.End()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            'CloseConn()
        End Try
    End Sub

    Private Function GetDetails() As DataTable
        Try
            'OpenConn()
            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand
            'Dim sqlconn As New SqlConnection
            dtfill = objCommon.FillDataTable(sqlConn, "ID_FILL_CompanyMaster", ViewState("Id"), "compid")
            Return dtfill
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            'CloseConn()
        End Try
    End Function
End Class
