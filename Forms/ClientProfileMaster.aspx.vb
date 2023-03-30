Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Imports System.IO
Imports System.Collections.Generic
Partial Class Forms_ClientProfileMaster
    Inherits System.Web.UI.Page
    Dim PgName As String = "$ClientProfileMaster$"
    Dim objCommon As New clsCommonFuns
    Dim dsmenu As DataSet
    Dim dsDPDetails As DataSet
    Dim newTable As DataTable
    Dim DpDetailsTable As DataTable
    Dim trmenu As DataRow
    Dim lstItem As ListItem
    Dim sqlConn As New SqlConnection
    Dim objUtil As New Util

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If

            'objCommon.OpenConn()   
            'btn_SaveEditPFDetails.Visible = False
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            'rdo_Headselect.Items(0).Attributes.Add("onclick", "ShowHeadCustomer()")
            'rdo_Headselect.Items(1).Attributes.Add("onclick", "ShowHeadCustomer()")

            Rdo_Custodian.Items(0).Attributes.Add("onclick", "ShowCustodian()")
            Rdo_Custodian.Items(1).Attributes.Add("onclick", "ShowCustodian()")

            If IsPostBack = False Then
                SetAttributes()
                SetControls()
                FillBlankGrids()
                FillBlankDealerGrid()
                FillCombo()
                If Request.QueryString("Id") <> "" Then
                    Dim strId As String = objCommon.DecryptText(HttpUtility.UrlDecode(Request.QueryString("Id")))
                    ViewState("Id") = Val(strId)
                    Hid_CustomerId.Value = ViewState("Id")
                    FillFields()
                    FillCustomerDetailsGrid()

                    btn_Save.Visible = False
                    btn_Update.Visible = True
                    fillListBoxOnUpdate()
                Else

                    btn_Save.Visible = True
                    btn_Update.Visible = False

                    btn_Address.Visible = False
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "s21", "Visiblefalse();", True)
                End If
            Else
                FillContactDetails()

            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowCustodian", "ShowCustodian();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "s1", "SelectDocType();", True)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
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

    Private Sub SetControls()
        Try
            OpenConn()
            objCommon.FillControl(cbo_CustomerType, sqlConn, "ID_FILL_CustomerTypeMaster", "CustomerTypeName", "CustomerTypeId")
            objCommon.FillControl(cbo_CustomerGroup, sqlConn, "Id_FILL_GroupMaster", "GroupName", "GroupId")
            objCommon.FillControl(cbo_Branch, sqlConn, "ID_FILL_BranchMaster", "BranchName", "BranchId")
            '
            If Trim(Session("RestrictedAccess")) = "Y" Then
                objCommon.FillControl(cbo_DealerName, sqlConn, "ID_FILL_CustMasterDealer", "NameOfUser", "UserId", Val(Session("UserId")), "UserId")
            Else
                objCommon.FillControl(cbo_DealerName, sqlConn, "ID_FILL_Dealer", "NameOfUser", "UserId")
            End If


            'srh_HeadCustomer.Columns.Add("CustomerName")
            'srh_HeadCustomer.Columns.Add("CustomerId")
            'srh_Custodian.Columns.Add("CustodianName")
            'srh_Custodian.Columns.Add("CustodianId")
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetControls", "Error in SetControls", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub SetAttributes()

        txt_CustomerName.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_State.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_Country.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_CustomerName.Attributes.Add("onblur", "ConvertUCase(this);")

        txt_empalmentdate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_empalmentdate.Attributes.Add("onchange", "CheckDate(this,false);")

        txt_Address1.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_Address2.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_PinCode.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_City.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_PhoneNo.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_FaxNo.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_PANNo.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_TANNo.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_CustomerCode.Attributes.Add("onblur", "ConvertUCase(this);")
        'txt_PANNo.Attributes.Add("onblur", "fnValidatePAN(this);")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
        'btn_AddDetails.Attributes.Add("onclick", "return ValidateDetails();")
        'btn_Address.Attributes.Add("onclick", "return AddAddress();")
        Add_Dealer.Attributes.Add("onclick", "return ValidateDealer();")

    End Sub
    Private Sub FillCombo()
        Try
            Dim ds As New DataSet()
            ds = objComm.FillAllCombo("ID_Fill_CustomerDocumentsList")
            Hid_DocumentMaster.Value = objComm.Trim(ds.Tables(0).Rows(0)("Document").ToString())
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            'If CBool(objCommon.CheckDuplicate(sqlConn, "CustomerMaster", "CustomerName", txt_CustomerName.Text)) = False Then
            '    Dim msg As String = "This Customer Name Already Exist"
            '    Dim strHtml As String
            '    strHtml = "alert('" + msg + "');"
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
            '    Exit Sub
            'End If
            If Not CheckDuplicate() Then
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_CustomerMaster")

            btn_Save.Visible = False
            btn_Update.Visible = True
            cbo_Category.Enabled = True
            'cbo_CustomerType.Enabled = False
            Response.Redirect("ClientProfileDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_Save_Click", "Error in btn_Save_Click", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try

        'Response.Redirect("ClientProfileDetail.aspx?Id=" & ViewState("Id"), False)
    End Sub
    Private Function CheckDuplicate() As Boolean
        Dim lstParam As New List(Of SqlParameter)()
        Dim ds As DataSet
        Dim blnResult As Boolean = False
        Dim intDuplicate As Integer = 0
        Dim strMessage As String = ""

        Try
            lstParam.Add(New SqlParameter("@CustomerId", Val(ViewState("Id"))))
            lstParam.Add(New SqlParameter("@CustomerNnme", Trim(txt_CustomerName.Text)))
            lstParam.Add(New SqlParameter("@PANNumber", Trim(txt_PANNo.Text)))
            ds = objComm.FillDetails(lstParam, "ID_Check_DuplicateCustomer")

            If ds.Tables(0).Rows.Count > 0 Then
                intDuplicate = Val(ds.Tables(0).Rows(0).Item("TotalDuplicate") & "")
                strMessage = Trim(ds.Tables(0).Rows(0).Item("Message") & "")

                If intDuplicate > 0 Then
                    Dim strHtml As String
                    strHtml = "alert('" + strMessage + "');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
                Else
                    blnResult = True
                End If
            End If

            Return blnResult
        Catch ex As Exception

        End Try
    End Function
    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        'If CBool(objCommon.CheckDuplicate(clsCommonFuns.sqlConn, "CustomerMaster", "CustomerName", txt_CustomerName.Text, "CustomerTypeId", cbo_CustomerType.SelectedValue, "CategoryId", cbo_Category.SelectedValue)) = True Then
        '    Dim msg As String = "This Customer Name Already Exist"
        '    Dim strHtml As String
        '    strHtml = "alert('" + msg + "');"
        '    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)

        '    Exit Sub
        'End If
        'Dim dt As DataTable
        'If CBool(objCommon.ISNameexists("FA_CHECK_CUSTOMERNAME",sqlConn, txt_CustomerName, Val((Hid_CustomerId.Value) & ""), Val((cbo_CustomerType.SelectedValue) & ""), Val(cbo_Category.SelectedValue))) = False Then
        '    Dim msg As String = "This Customer Name Already Exist"
        '    Dim strHtml As String
        '    strHtml = "alert('" + msg + "');"
        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
        '    Exit Sub
        'End If


        ' If CBool(objCommon.CheckDuplicate(clsCommonFuns.sqlConn, "CustomerMaster", "CustomerName", txt_CustomerName.Text)) = False Then
        Try
            OpenConn()
            'If objCommon.CheckDuplicate(sqlConn, "CustomerMaster", "CustomerName", Trim(txt_CustomerName.Text), "CustomerId", Val(ViewState("Id"))) = False Then
            '    Dim msg As String = "This Customer Name Already Exist"
            '    Dim strHtml As String
            '    strHtml = "alert('" + msg + "');"
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
            '    Exit Sub
            'End If
            If Not CheckDuplicate() Then
                Exit Sub
            End If
            If SetSaveUpdate("ID_UPDATE_CustomerMaster") = False Then Exit Sub
            Response.Redirect("ClientProfileDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_Update_Click", "Error in btn_Update_Click", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Function DeleteClientContact(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter
            Dim dtFill As New DataTable
            Dim dvFill As New DataView
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_MultiAddrClientContact"
            sqlComm.Connection = sqlConn
            If Val(Hid_CustomerId.Value) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , Hid_CustomerId.Value)
            End If
            objCommon.SetCommandParameters(sqlComm, "@TempId", SqlDbType.Int, 4, "I", , , Val(Session("TempId")))
            objCommon.SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@StrMessage", SqlDbType.VarChar, 100, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally

        End Try
    End Function
    Public Function SaveContactNew(ByVal sqlTrans As SqlTransaction) As Boolean

        Try
            Dim sqlComm As New SqlCommand
            Dim dt As New DataTable
            Dim dgItem As DataGridItem

            Dim strContactPer As String = ""
            Dim strDesig As String = ""
            Dim strPh1 As String = ""
            Dim strPh2 As String = ""
            Dim strFax1 As String = ""
            Dim strFax2 As String = ""
            Dim strMobile As String = ""
            Dim strEmail As String = ""
            Dim strSectionType As String = ""
            Dim datTempId As Integer

            With sqlComm
                .Connection = sqlConn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_INSERT_MultiAddrClientContact"
                dt = Session("MAddrcontactTable")
                If dt Is Nothing = False Then
                    For I As Int16 = 0 To dt.Rows.Count - 1
                        sqlComm.Parameters.Clear()
                        objCommon.SetCommandParameters(sqlComm, "@ContactPerson", SqlDbType.VarChar, 100, "I", , , Trim(dt.Rows(I).Item("ContactPerson")))
                        objCommon.SetCommandParameters(sqlComm, "@Designation", SqlDbType.VarChar, 100, "I", , , Trim(dt.Rows(I).Item("Designation")))
                        objCommon.SetCommandParameters(sqlComm, "@PhoneNo1", SqlDbType.VarChar, 100, "I", , , Trim(dt.Rows(I).Item("PhoneNo1")))
                        objCommon.SetCommandParameters(sqlComm, "@PhoneNo2", SqlDbType.VarChar, 100, "I", , , Trim(dt.Rows(I).Item("PhoneNo2")))
                        objCommon.SetCommandParameters(sqlComm, "@FaxNo1", SqlDbType.VarChar, 100, "I", , , Trim(dt.Rows(I).Item("FaxNo1")))
                        objCommon.SetCommandParameters(sqlComm, "@FaxNo2", SqlDbType.VarChar, 100, "I", , , Trim(dt.Rows(I).Item("FaxNo2")))
                        objCommon.SetCommandParameters(sqlComm, "@MobileNo", SqlDbType.VarChar, 100, "I", , , Trim(dt.Rows(I).Item("MobileNo")))
                        objCommon.SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , Trim(dt.Rows(I).Item("EmailId")))
                        If Val(Hid_CustomerId.Value) <> 0 Then
                            objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , Hid_CustomerId.Value)
                        End If
                        If Val(Session("ClientCustMulId")) <> 0 Then
                            objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "I", , , Val(Session("ClientCustMulId")))
                        Else
                            objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "I", , , DBNull.Value)
                        End If
                        objCommon.SetCommandParameters(sqlComm, "@TempId", SqlDbType.Int, 4, "I", , , Val(Session("TempId")))
                        objCommon.SetCommandParameters(sqlComm, "@SectionType", SqlDbType.Char, 1, "I", , , Trim(dt.Rows(I).Item("SectionType")))
                        objCommon.SetCommandParameters(sqlComm, "@MAContactDetailId", SqlDbType.Int, 4, "O")
                        objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                        objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
                        sqlComm.ExecuteNonQuery()
                        'End If
                        'Hid_MACustAddrCtcid.Value += Val(sqlComm.Parameters.Item("@MAContactDetailId").Value & "") & "!"
                    Next
                End If
            End With
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            'CloseConn()
        End Try
    End Function
    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("ClientProfileDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("ClientProfileDetail.aspx")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Function SetSaveUpdate(ByVal strProc As String) As Boolean
        Try
            Dim sqlTrans As SqlTransaction
            Dim dt As DataTable
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction

            If SaveUpdate(sqlTrans, strProc) = False Then Exit Function
            If DeleteCustomerDetails(sqlTrans) = False Then Exit Function
            If SaveBanks(sqlTrans) = False Then Exit Function
            If SaveContacts(sqlTrans) = False Then Exit Function
            If SaveSGLTrans(sqlTrans) = False Then Exit Function
            If SaveScheme(sqlTrans) = False Then Exit Function
            If SaveDPDetails(sqlTrans) = False Then Exit Function
            If SaveSignaturyDetails(sqlTrans) = False Then Exit Function
            If SaveDealers(sqlTrans) = False Then Exit Function
            If SaveBrokers(sqlTrans) = False Then Exit Function
            If SaveErstWhile(sqlTrans) = False Then Exit Function
            If Hid_DecType.Value = "B" Then
                'If DeleteDetails(sqlTrans) = False Then Exit Function
                If SaveKycDocuments(sqlTrans) = False Then Exit Function
                'If DeleteEmpDetails(sqlTrans) = False Then Exit Function
                If SaveEmpDocuments(sqlTrans) = False Then Exit Function
            ElseIf Hid_DecType.Value = "K" Then
                'If DeleteDetails(sqlTrans) = False Then Exit Function
                If SaveKycDocuments(sqlTrans) = False Then Exit Function
            Else
                'If DeleteEmpDetails(sqlTrans) = False Then Exit Function
                If SaveEmpDocuments(sqlTrans) = False Then Exit Function
            End If

            If DeleteClientContact(sqlTrans) = False Then Exit Function
            If SaveContactNew(sqlTrans) = False Then Exit Function
            If SaveAddresses(sqlTrans) = False Then Exit Function



            If Rdo_Custodian.SelectedValue = "Y" Then
                If SaveCustodianBanks(sqlTrans) = False Then Exit Function
            Else
                ClearFields()
            End If
            'If UpdateDealSlipContactDetails(sqlTrans) = False Then Exit Function
            'If Rdo_Custodian.SelectedValue = "N" Then
            '    ClearFields()
            'End If
            If SaveKycDocuments_New(sqlTrans) = False Then Exit Function
            sqlTrans.Commit()

            Return True
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetSaveUpdate", "Error in SetSaveUpdate", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Function
    Private Function SaveKycDocuments_New(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim lstParam As List(Of SqlParameter) = New List(Of SqlParameter)
            Dim blnReturn As Boolean = False
            Dim dtDocumnet As DataTable = New DataTable
            Dim filename As String = ""
            Dim ext As String = ""
            Dim contenttype As String = ""
            If Val(ViewState("Id") & "") = 0 Then
                lstParam.Add(New SqlParameter("@CustomerId", 0))
            Else
                lstParam.Add(New SqlParameter("@CustomerId", ViewState("Id")))
            End If
            dtDocumnet.Columns.Add("Id", GetType(Int32))
            dtDocumnet.Columns.Add("DocumentId", GetType(Int32))
            dtDocumnet.Columns.Add("FileName", GetType(String))
            dtDocumnet.Columns.Add("FileData", GetType(System.Byte()))
            dtDocumnet.Columns.Add("FileType", GetType(String))
            dtDocumnet.Columns.Add("FileExtension", GetType(String))
            Dim Id() As String = Hid_CustomerDocumentId.Value.Split(Microsoft.VisualBasic.ChrW(44))
            Dim DocumentId() As String = Hid_DocumentId.Value.Split(Microsoft.VisualBasic.ChrW(44))
            If (Hid_CustomerDocumentId.Value <> "") Then
                Dim i As Integer = 0
                Do While (i < Id.Length)
                    Dim PostedFile As HttpPostedFile = Request.Files(i + 1)
                    Dim datarow As DataRow = dtDocumnet.NewRow
                    datarow("Id") = objComm.Val(Id(i))
                    datarow("DocumentId") = objComm.Val(DocumentId(i))
                    If (PostedFile.ContentLength > 0) Then
                        filename = PostedFile.FileName
                        ext = Path.GetExtension(filename)
                        contenttype = PostedFile.ContentType
                        Dim fs As Stream = PostedFile.InputStream
                        Dim br As BinaryReader = New BinaryReader(fs)
                        Dim bytes() As Byte = br.ReadBytes(CType(fs.Length, Int32))
                        datarow("FileName") = filename
                        datarow("FileData") = bytes
                        datarow("FileType") = contenttype
                        datarow("FileExtension") = ext
                    End If

                    dtDocumnet.Rows.Add(datarow)
                    i = (i + 1)
                Loop
            End If

            lstParam.Add(New SqlParameter("@tblDocumentDetails", dtDocumnet))
            If objComm.InsertUpdateDetails(lstParam, "ID_InsertUpdate_CustomerDocumentsDetails") > 0 Then
                blnReturn = True
            End If
            Return blnReturn
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveKycDocuments", "Error in SaveKycDocuments", "", ex)

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try


    End Function
    Private Function SaveKycDocuments(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_CustomerMasterDocuments"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To srh_KYCDocuments.SelectListBox.Items.Count - 1
                If Val(srh_KYCDocuments.SelectListBox.Items(I).Value) <> 0 Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@DocumentTypeId", SqlDbType.Int, 4, "I", , , srh_KYCDocuments.SelectListBox.Items(I).Value)
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@DecType", SqlDbType.Char, 1, "I", , , Trim("K"))
                    objCommon.SetCommandParameters(sqlComm, "@CustDocId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveKycDocuments", "Error in SaveKycDocuments", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Function SaveEmpDocuments(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_CustomerMasterEmptypedocument"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To srh_EmpalementDocuments.SelectListBox.Items.Count - 1
                If Val(srh_EmpalementDocuments.SelectListBox.Items(I).Value) <> 0 Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@DocumentTypeId", SqlDbType.Int, 4, "I", , , srh_EmpalementDocuments.SelectListBox.Items(I).Value)
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@DecType", SqlDbType.Char, 1, "I", , , Trim("E"))
                    objCommon.SetCommandParameters(sqlComm, "@CustDocId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveEmpDocuments", "Error in SaveEmpDocuments", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            sqlComm.Parameters.Clear()
            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 4, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@CustomerTypeId", SqlDbType.Int, 4, "I", , , Val(cbo_CustomerType.SelectedValue))
            If Val(cbo_Category.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@CategoryId", SqlDbType.Int, 4, "I", , , Val(cbo_Category.SelectedValue))
            End If
            If Val(cbo_CustomerGroup.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@GroupId", SqlDbType.Int, 4, "I", , , Val(cbo_CustomerGroup.SelectedValue))
            End If

            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@CustomerName", SqlDbType.VarChar, 500, "I", , , Trim(txt_CustomerName.Text))
            objCommon.SetCommandParameters(sqlComm, "@CustPrefix", SqlDbType.VarChar, 100, "I", , , Trim(txt_CustPrefix.Text))
            objCommon.SetCommandParameters(sqlComm, "@CustomerAddress1", SqlDbType.VarChar, 100, "I", , , Trim(txt_Address1.Text))
            objCommon.SetCommandParameters(sqlComm, "@CustomerAddress2", SqlDbType.VarChar, 100, "I", , , Trim(txt_Address2.Text))
            objCommon.SetCommandParameters(sqlComm, "@CustomerPinCode", SqlDbType.VarChar, 100, "I", , , Trim(txt_PinCode.Text))
            objCommon.SetCommandParameters(sqlComm, "@CustomerCity", SqlDbType.VarChar, 100, "I", , , Trim(txt_City.Text))
            objCommon.SetCommandParameters(sqlComm, "@Country", SqlDbType.VarChar, 100, "I", , , Trim(txt_Country.Text))
            objCommon.SetCommandParameters(sqlComm, "@State", SqlDbType.VarChar, 100, "I", , , Trim(txt_State.SearchTextBox.Text))
            objCommon.SetCommandParameters(sqlComm, "@StateId", SqlDbType.Int, 4, "I", , , Val(txt_State.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@CustomerPhone", SqlDbType.VarChar, 500, "I", , , Trim(txt_PhoneNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@CustomerFax", SqlDbType.VarChar, 200, "I", , , Trim(txt_FaxNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@PANNumber", SqlDbType.VarChar, 100, "I", , , Trim(txt_PANNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@TANNumber", SqlDbType.VarChar, 100, "I", , , Trim(txt_TANNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , Trim(txt_EmailId.Text))
            objCommon.SetCommandParameters(sqlComm, "@AccessLevel", SqlDbType.Char, 1, "I", , , cbo_Accessible.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@HeadSelect", SqlDbType.Char, 1, "I", , , rdo_Headselect.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@CustomerCode", SqlDbType.VarChar, 100, "I", , , Trim(txt_CustomerCode.Text))
            If rdo_Headselect.SelectedValue = "N" Then
                objCommon.SetCommandParameters(sqlComm, "@HeadCutomerId", SqlDbType.BigInt, 8, "I", , , Val(srh_HeadCustomer.SelectedId))
            Else
                objCommon.SetCommandParameters(sqlComm, "@HeadCutomerId", SqlDbType.BigInt, 8, "I", , , DBNull.Value)
            End If
            objCommon.SetCommandParameters(sqlComm, "@Custodian", SqlDbType.Char, 1, "I", , , Rdo_Custodian.SelectedValue)
            If Rdo_Custodian.SelectedValue = "Y" Then
                objCommon.SetCommandParameters(sqlComm, "@CustodianId", SqlDbType.Int, 4, "I", , , srh_Custodian.SelectedId)
            Else
                objCommon.SetCommandParameters(sqlComm, "@CustodianName", SqlDbType.VarChar, 50, "I", , , DBNull.Value)

            End If
            If Trim(txt_empalmentdate.Text) = "" Then
                objCommon.SetCommandParameters(sqlComm, "@EmpalmentDate", SqlDbType.SmallDateTime, 4, "I", , , DBNull.Value)
            Else
                objCommon.SetCommandParameters(sqlComm, "@EmpalmentDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_empalmentdate.Text))
            End If



            If Val(cbo_Branch.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@BranchId", SqlDbType.Int, 4, "I", , , Val(cbo_Branch.SelectedValue))
            Else
                objCommon.SetCommandParameters(sqlComm, "@BranchId", SqlDbType.Int, 4, "I", , , DBNull.Value)
            End If

            If Trim(cbo_Abbreviation.SelectedItem.Text <> "") Then
                objCommon.SetCommandParameters(sqlComm, "@Abbreviation", SqlDbType.VarChar, 100, "I", , , Trim(cbo_Abbreviation.SelectedItem.Text))
            Else
                objCommon.SetCommandParameters(sqlComm, "@Abbreviation", SqlDbType.VarChar, 100, "I", , , DBNull.Value)
            End If
            objCommon.SetCommandParameters(sqlComm, "@HideShow", SqlDbType.Char, 1, "I", , , rdo_HideShow.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@EmpalmentFrequency", SqlDbType.Char, 1, "I", , , Trim(cbo_FrequencyEmpalment.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@CustomerGST", SqlDbType.VarChar, 100, "I", , , Trim(txt_GSTNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@BillSettType", SqlDbType.Char, 1, "I", , , Trim(rdo_BillSettType.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@CustomerDealer", SqlDbType.Int, 4, "I", , , Val(cbo_DealerName.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@BSECustomerCode", SqlDbType.VarChar, 50, "I", , , Trim(txt_BSECustomerCode.Text))
            objCommon.SetCommandParameters(sqlComm, "@NSECustomerCode", SqlDbType.VarChar, 50, "I", , , Trim(txt_NSECustomerCode.Text))
            objCommon.SetCommandParameters(sqlComm, "@Remarks", SqlDbType.VarChar, 1000, "I", , , Trim(txt_Remarks.Text))

            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@CustomerId").Value
            Hid_CustomerId.Value = ViewState("Id")

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveUpdate", "Error in SaveUpdate", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Function SaveSignaturyDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            dt = Session("SignaturyTable")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_CustomerSignaturyDetail"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("SignaturyName").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@SignaturyName", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("SignaturyName"))
                    objCommon.SetCommandParameters(sqlComm, "@FileBytes", SqlDbType.Image, 0, "I", , , dt.Rows(I).Item("FileBytes"))
                    objCommon.SetCommandParameters(sqlComm, "@FileName", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("FileName"))
                    objCommon.SetCommandParameters(sqlComm, "@ContentType", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("ContentType"))
                    objCommon.SetCommandParameters(sqlComm, "@ContentLength", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("ContentLength"))
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@CustImageId ", SqlDbType.BigInt, 8, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveUpdate", "Error in SaveUpdate", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Private Sub FillBlankGrids()
        Try
            FillBlankSGLGrids()
            FillBlankDPGrids()
            FillBlankBankGrids()
            FillBlankContactGrids()
            FillBlankSignaturyGrids()
            FillBlankAddresses()
            FillBlankCustodianBankGrid()
            FillBlankSchemeGrid()
            FillBlankBrokerGrid()
            FillBlankErstWhileGrid()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub FillBlankErstWhileGrid()
        Try
            Dim dtErstWhileGrid As New DataTable
            dtErstWhileGrid.Columns.Add("CustomerErstWhileName", GetType(String))
            dtErstWhileGrid.Columns.Add("ErstWhileDate", GetType(String))
            dtErstWhileGrid.Columns.Add("CustomerErstWhileId", GetType(String))
            Session("CustomerErstWhile") = dtErstWhileGrid
            dg_ErstWhile.DataSource = dtErstWhileGrid
            dg_ErstWhile.DataBind()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub FillBlankDealerGrid()
        Try
            Dim dtInfoGrid As New DataTable
            dtInfoGrid.Columns.Add("NameOFUser", GetType(String))
            dtInfoGrid.Columns.Add("StartDate", GetType(String))
            dtInfoGrid.Columns.Add("EndDate", GetType(String))
            dtInfoGrid.Columns.Add("CustomerId", GetType(String))
            dtInfoGrid.Columns.Add("CustomerDealerId", GetType(String))
            dtInfoGrid.Columns.Add("CustDealerDetailId", GetType(String))

            Session("CustomerDealerTable") = dtInfoGrid
            dg_Dealer.DataSource = dtInfoGrid
            dg_Dealer.DataBind()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub FillBlankBrokerGrid()
        Try
            Dim dtInfoGrid As New DataTable
            dtInfoGrid.Columns.Add("BrokerName", GetType(String))
            dtInfoGrid.Columns.Add("StartDate", GetType(String))
            dtInfoGrid.Columns.Add("EndDate", GetType(String))
            dtInfoGrid.Columns.Add("CustomerId", GetType(String))
            dtInfoGrid.Columns.Add("CustomerBrokerId", GetType(String))
            dtInfoGrid.Columns.Add("CustBrokerDetailId", GetType(String))

            Session("CustomerBrokerTable") = dtInfoGrid
            dg_Broker.DataSource = dtInfoGrid
            dg_Broker.DataBind()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub FillBlankSGLGrids()
        Try
            Dim dtSGLGrid As New DataTable
            dtSGLGrid.Columns.Add("SGLTransWith", GetType(String))
            dtSGLGrid.Columns.Add("CustomerSGLId", GetType(String))
            dtSGLGrid.Columns.Add("CustSGLId", GetType(String))


            Session("SGLTable") = dtSGLGrid
            dg_SGL.DataSource = dtSGLGrid
            dg_SGL.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillBlankSGLGrids", "Error in FillBlankSGLGrids", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub FillBlankSignaturyGrids()
        Try
            Dim dtSignaturyGrid As New DataTable
            dtSignaturyGrid.Columns.Add("SignaturyName", GetType(String))
            dtSignaturyGrid.Columns.Add("FileBytes", GetType(Byte()))
            dtSignaturyGrid.Columns.Add("FileName", GetType(String))
            dtSignaturyGrid.Columns.Add("ContentType", GetType(String))
            dtSignaturyGrid.Columns.Add("ContentLength", GetType(String))

            Session("SignaturyTable") = dtSignaturyGrid
            dg_Signatury.DataSource = dtSignaturyGrid
            dg_Signatury.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillBlankSignaturyGrids", "Error in FillBlankSignaturyGrids", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub FillBlankDPGrids()
        Try
            Dim dtDPGrid As New DataTable
            dtDPGrid.Columns.Add("DpName", GetType(String))
            dtDPGrid.Columns.Add("DpId", GetType(String))
            dtDPGrid.Columns.Add("ClientId", GetType(String))
            dtDPGrid.Columns.Add("CustomerDPId", GetType(String))
            dtDPGrid.Columns.Add("CustDPId", GetType(String))
            Session("DPTable") = dtDPGrid
            dg_DP.DataSource = dtDPGrid
            dg_DP.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillBlankDPGrids", "Error in FillBlankDPGrids", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub FillBlankBankGrids()
        Try
            Dim dtBAnkGrid As New DataTable
            dtBAnkGrid.Columns.Add("BankName", GetType(String))
            dtBAnkGrid.Columns.Add("AccountNo", GetType(String))
            dtBAnkGrid.Columns.Add("Branch", GetType(String))
            dtBAnkGrid.Columns.Add("RTGSCode", GetType(String))
            dtBAnkGrid.Columns.Add("CustomerBankId", GetType(String))
            dtBAnkGrid.Columns.Add("CustBankId", GetType(String))
            Session("BankTable") = dtBAnkGrid
            dg_Bank.DataSource = dtBAnkGrid
            dg_Bank.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillBlankBankGrids", "Error in FillBlankBankGrids", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Sub FillBlankContactGrids()
        Try
            Dim dtContactGrid As New DataTable
            dtContactGrid.Columns.Add("ContactPerson", GetType(String))
            dtContactGrid.Columns.Add("Designation", GetType(String))
            dtContactGrid.Columns.Add("PhoneNo1", GetType(String))
            dtContactGrid.Columns.Add("MobileNo", GetType(String))
            dtContactGrid.Columns.Add("EmailId", GetType(String))
            'dtContactGrid.Columns.Add("Dept", GetType(String))
            dtContactGrid.Columns.Add("BusinessTypeNames", GetType(String))
            dtContactGrid.Columns.Add("BusniessTypeIds", GetType(String))
            dtContactGrid.Columns.Add("NameOfUsers", GetType(String))
            dtContactGrid.Columns.Add("UserIds", GetType(String))
            dtContactGrid.Columns.Add("PhoneNo2", GetType(String))
            dtContactGrid.Columns.Add("FaxNo1", GetType(String))
            dtContactGrid.Columns.Add("FaxNo2", GetType(String))
            dtContactGrid.Columns.Add("Interaction", GetType(String))
            dtContactGrid.Columns.Add("SectionType", GetType(String))
            dtContactGrid.Columns.Add("ContactDetailId", GetType(String))
            Session("ContactTable") = dtContactGrid
            dg_Contact.DataSource = dtContactGrid
            dg_Contact.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillBlankContactGrids", "Error in FillBlankContactGrids", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub FillBlankSchemeGrid()
        Try
            Dim dtSGLGrid As New DataTable
            dtSGLGrid.Columns.Add("SchemeName", GetType(String))
            dtSGLGrid.Columns.Add("CustomerSchemeId", GetType(String))
            Session("SchemeTable") = dtSGLGrid
            dg_Scheme.DataSource = dtSGLGrid
            dg_Scheme.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillBlankSGLGrids", "Error in FillBlankSGLGrids", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Sub FillBlankAddresses()
        Try
            Dim dt As New DataTable
            dt.Columns.Add("CustomerBranchName", GetType(String))
            dt.Columns.Add("Address1", GetType(String))
            dt.Columns.Add("Address2", GetType(String))
            dt.Columns.Add("City", GetType(String))
            dt.Columns.Add("PinCode", GetType(String))
            dt.Columns.Add("State", GetType(String))
            dt.Columns.Add("Country", GetType(String))
            dt.Columns.Add("Phone", GetType(String))
            dt.Columns.Add("FaxNo", GetType(String))
            dt.Columns.Add("EmailId", GetType(String))
            dt.Columns.Add("CustomerId", GetType(String))
            dt.Columns.Add("BusinessTypeNames", GetType(String))
            dt.Columns.Add("BusniessTypeIds", GetType(String))
            dt.Columns.Add("ClientCustAddId", GetType(String))
            dt.Columns.Add("ClientCustAddressId", GetType(String))
            dt.Columns.Add("TempId", GetType(String))
            'Session("CustAddressTable") = dt
            Session("MainCustAddressTable") = dt
            dg1.DataSource = dt
            dg1.DataBind()
            HideShowColumns(dg1)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillBlankAddresses", "Error in FillBlankAddresses", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub



    Protected Sub btn_Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        AddDetails()
    End Sub
    Private Sub AddDetails()
        Try
            Dim strRetValues() As String
            Dim dt As DataTable
            Dim dr As DataRow

            strRetValues = Split(Hid_RetValues.Value, "!")
            dt = Session("MainCustAddressTable")
            dr = dt.NewRow
            dr.Item("CustomerBranchName") = Trim(strRetValues(0) & "")
            dr.Item("Address1") = Trim(strRetValues(1))
            dr.Item("Address2") = Trim(strRetValues(2))
            dr.Item("City") = Trim(strRetValues(3))
            dr.Item("PinCode") = Trim(strRetValues(4))
            dr.Item("State") = Trim(strRetValues(5))
            dr.Item("Country") = Trim(strRetValues(6))
            dr.Item("Phone") = Trim(strRetValues(7))
            dr.Item("FaxNo") = Trim(strRetValues(8))
            dr.Item("EmailId") = Trim(strRetValues(9))
            dr.Item("CustomerId") = Val(strRetValues(10))
            If strRetValues(14) <> "" Then
                dr.Item("ClientCustAddressId") = Val(strRetValues(14))
            End If

            dr.Item("TempId") = Val(strRetValues(11))
            Hid_TempId.Value = Val(strRetValues(11))
            'If strRetValues(13) <> "" Then
            '    dr.Item("BusinessTypeNames") = IIf(Right(strRetValues(13), 1) = ",", Left(strRetValues(13), strRetValues(13).Length - 1), strRetValues(13))
            'Else
            '    dr.Item("BusinessTypeNames") = ""
            'End If

            If strRetValues(12) <> "" Then
                dr.Item("BusniessTypeIds") = IIf(Right(strRetValues(12), 1) = ",", Left(strRetValues(12), strRetValues(12).Length - 1), strRetValues(12))
            Else
                dr.Item("BusniessTypeIds") = ""
            End If
            dr.Item("ClientCustAddId") = DBNull.Value
            dt.Rows.Add(dr)
            Session("MainCustAddressTable") = dt
            FillDetails()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try

    End Sub

    Private Sub FillDetails()
        Try
            Dim dt As DataTable
            If TypeOf Session("MainCustAddressTable") Is DataTable Then
                dt = Session("MainCustAddressTable")
            Else
                dt = objCommon.FillDetailsGrid(dg1, "Id_FILL_ClientCustMultipleAddress", "CustomerId", Val((Hid_CustomerId.Value) & ""))
            End If
            Session("MainCustAddressTable") = dt
            'ClearOptions()
            dg1.DataSource = dt
            dg1.DataBind()
            HideShowColumns(dg1)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub HideShowColumns(ByVal dg As DataGrid)
        If dg Is Nothing Then
            Return
        End If
        ' Loop through all of the columns in the grid.
        For Each col As DataGridColumn In dg.Columns
            ' Hide the Salary and SS# Columns.
            If col.HeaderText = "TempId" Then
                col.Visible = False
            End If
        Next
    End Sub
    Private Sub FillBlankCustodianBankGrid()

        Try
            Dim dtCustodianbankdetails As New DataTable
            dtCustodianbankdetails.Columns.Add("CustodianBankName", GetType(String))
            dtCustodianbankdetails.Columns.Add("CustodianAccountNo", GetType(String))
            dtCustodianbankdetails.Columns.Add("CustodianBranch", GetType(String))
            dtCustodianbankdetails.Columns.Add("CustodianRTGSCode", GetType(String))
            Session("CustodianBankTable") = dtCustodianbankdetails
            dgCustodianbankdetails.DataSource = dtCustodianbankdetails
            dgCustodianbankdetails.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillBlankCustodianBankGrid", "Error in FillBlankCustodianBankGrid", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try

    End Sub

    Protected Sub dg_SGL_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_SGL.ItemCommand
        SetSGLGrid(dg_SGL, e, "SGLTable")
    End Sub

    Private Sub SetSGLGrid(ByVal objGrid As DataGrid, ByVal e As DataGridCommandEventArgs, ByVal strSessionName As String)
        Try
            Dim dtGrid As DataTable
            dtGrid = TryCast(Session("SGLTable"), DataTable).Copy
            If e.CommandName.ToUpper() = "ADD" Or e.CommandName.ToUpper() = "UPDATE" Then
                Dim dr As DataRow
                If e.CommandName.ToUpper() = "ADD" Then
                    dr = dtGrid.NewRow
                Else
                    dr = dtGrid.Rows(e.Item.ItemIndex)
                End If
                dr("SGLTransWith") = Trim(TryCast(e.Item.FindControl("txt_SGLTransWith"), TextBox).Text)

                If e.CommandName.ToUpper() = "ADD" Then
                    dtGrid.Rows.Add(dr)
                End If
                objGrid.EditItemIndex = -1
            ElseIf e.CommandName.ToUpper() = "DELETE" Then
                dtGrid.Rows.RemoveAt(e.Item.ItemIndex)
            ElseIf e.CommandName.ToUpper() = "EDIT" Then
                objGrid.EditItemIndex = e.Item.ItemIndex
            ElseIf e.CommandName.ToUpper() = "CANCEL" Then
                objGrid.EditItemIndex = -1
            End If
            Session("SGLTable") = dtGrid
            objGrid.DataSource = dtGrid
            objGrid.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetSGLGrid", "Error in SetSGLGrid", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Protected Sub dg_DP_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_DP.ItemCommand
        SetDPGrid(dg_DP, e, "DPTable")
    End Sub

    Private Sub SetDPGrid(ByVal objGrid As DataGrid, ByVal e As DataGridCommandEventArgs, ByVal strSessionName As String)
        Try
            Dim dtGrid As DataTable
            dtGrid = TryCast(Session("DPTable"), DataTable).Copy
            If e.CommandName.ToUpper() = "ADD" Or e.CommandName.ToUpper() = "UPDATE" Then
                Dim dr As DataRow
                If e.CommandName.ToUpper() = "ADD" Then
                    dr = dtGrid.NewRow
                Else
                    dr = dtGrid.Rows(e.Item.ItemIndex)
                End If
                dr("DpName") = Trim(TryCast(e.Item.FindControl("txt_DpName"), TextBox).Text)
                dr("DpId") = Trim(TryCast(e.Item.FindControl("txt_DpId"), TextBox).Text)
                dr("ClientId") = Trim(TryCast(e.Item.FindControl("txt_ClientId"), TextBox).Text)
                If e.CommandName.ToUpper() = "ADD" Then
                    dtGrid.Rows.Add(dr)
                End If
                objGrid.EditItemIndex = -1
            ElseIf e.CommandName.ToUpper() = "DELETE" Then
                dtGrid.Rows.RemoveAt(e.Item.ItemIndex)
            ElseIf e.CommandName.ToUpper() = "EDIT" Then
                objGrid.EditItemIndex = e.Item.ItemIndex
            ElseIf e.CommandName.ToUpper() = "CANCEL" Then
                objGrid.EditItemIndex = -1
            End If
            Session("DPTable") = dtGrid
            objGrid.DataSource = dtGrid
            objGrid.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetDPGrid", "Error in SetDPGrid", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub dg_Bank_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_Bank.ItemCommand
        SetBankGrid(dg_Bank, e, "BankTable")
    End Sub

    Private Sub SetBankGrid(ByVal objGrid As DataGrid, ByVal e As DataGridCommandEventArgs, ByVal strSessionName As String)
        Try
            Dim dtGrid As DataTable
            dtGrid = TryCast(Session("BankTable"), DataTable).Copy
            If e.CommandName.ToUpper() = "ADD" Or e.CommandName.ToUpper() = "UPDATE" Then
                Dim dr As DataRow
                If e.CommandName.ToUpper() = "ADD" Then
                    dr = dtGrid.NewRow
                Else
                    dr = dtGrid.Rows(e.Item.ItemIndex)
                End If
                dr("BankName") = Trim(TryCast(e.Item.FindControl("txt_BankName"), TextBox).Text)
                dr("AccountNo") = Trim(TryCast(e.Item.FindControl("txt_AccountNo"), TextBox).Text)
                dr("Branch") = Trim(TryCast(e.Item.FindControl("txt_Branch"), TextBox).Text)
                dr("RTGSCode") = Trim(TryCast(e.Item.FindControl("txt_RTGSCode"), TextBox).Text)
                If e.CommandName.ToUpper() = "ADD" Then
                    dtGrid.Rows.Add(dr)
                End If
                objGrid.EditItemIndex = -1
            ElseIf e.CommandName.ToUpper() = "DELETE" Then
                dtGrid.Rows.RemoveAt(e.Item.ItemIndex)
            ElseIf e.CommandName.ToUpper() = "EDIT" Then
                objGrid.EditItemIndex = e.Item.ItemIndex
            ElseIf e.CommandName.ToUpper() = "CANCEL" Then
                objGrid.EditItemIndex = -1
            End If
            Session("BankTable") = dtGrid
            objGrid.DataSource = dtGrid
            objGrid.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetBankGrid", "Error in SetBankGrid", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub dg_Contact_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_Contact.ItemCommand
        Try
            Dim strRetValues() As String
            Dim dt As DataTable


            dt = CType(Session("contactTable"), DataTable)
            If e.CommandName = "Edit" Then
                strRetValues = Split(Hid_RetValues.Value, "!")
                dt.Rows(e.Item.ItemIndex).Item("ContactPerson") = strRetValues(0)
                dt.Rows(e.Item.ItemIndex).Item("Designation") = strRetValues(1)
                dt.Rows(e.Item.ItemIndex).Item("PhoneNo1") = strRetValues(2)
                dt.Rows(e.Item.ItemIndex).Item("MobileNo") = strRetValues(3)
                dt.Rows(e.Item.ItemIndex).Item("EmailId") = (strRetValues(4))
                'dt.Rows(e.Item.ItemIndex).Item("BusniessTypeIds") = (strRetValues(5))
                'dt.Rows(e.Item.ItemIndex).Item("BusinessTypeNames") = Trim(strRetValues(6))
                'dt.Rows(e.Item.ItemIndex).Item("UserIds") = (strRetValues(7))
                'dt.Rows(e.Item.ItemIndex).Item("NameOfUsers") = Trim(strRetValues(8))
                dt.Rows(e.Item.ItemIndex).Item("PhoneNo2") = strRetValues(5)
                dt.Rows(e.Item.ItemIndex).Item("FaxNo1") = strRetValues(6)
                dt.Rows(e.Item.ItemIndex).Item("FaxNo2") = strRetValues(7)
                dt.Rows(e.Item.ItemIndex).Item("Interaction") = strRetValues(8)
                dt.Rows(e.Item.ItemIndex).Item("SectionType") = strRetValues(9)
                dt.Rows(e.Item.ItemIndex).Item("Branch") = strRetValues(10)

            Else
                dt.Rows.RemoveAt(e.Item.ItemIndex)
            End If
            Session("contactTable") = dt
            FillContactDetails()

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Contact_ItemCommand", "Error in dg_Contact_ItemCommand", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Function SaveBanks(ByVal sqlTrans As SqlTransaction) As Boolean
        Try

            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            dt = Session("BankTable")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            'sqlComm.CommandText = "ID_INSERT_CustomerBankDetail"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("BankName").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    If Val(dt.Rows(I).Item("CustBankId") & "") = 0 Then
                        sqlComm.CommandText = "ID_INSERT_CustomerBankDetail"
                        objCommon.SetCommandParameters(sqlComm, "@CustBankId", SqlDbType.Int, 4, "O")
                    Else
                        sqlComm.CommandText = "ID_UPDATE_CustomerBankDetail"

                        objCommon.SetCommandParameters(sqlComm, "@CustBankId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("CustBankId"))
                    End If
                    objCommon.SetCommandParameters(sqlComm, "@BankName", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("BankName"))
                    objCommon.SetCommandParameters(sqlComm, "@Branch", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("Branch"))
                    objCommon.SetCommandParameters(sqlComm, "@AccountNo", SqlDbType.VarChar, 30, "I", , , dt.Rows(I).Item("AccountNo"))
                    objCommon.SetCommandParameters(sqlComm, "@RTGSCode", SqlDbType.VarChar, 30, "I", , , dt.Rows(I).Item("RTGSCode"))
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@CustBankUserId", SqlDbType.Int, 4, "I", , , Session("UserId"))
                    'objCommon.SetCommandParameters(sqlComm, "@CustBankId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveBanks", "Error in SaveBanks", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)

        End Try
    End Function
    Private Function SaveCustodianBanks(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            dt = Session("CustodianBankTable")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_CustodianBankDetails"
            sqlComm.Connection = sqlConn

            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("CustodianBankName").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@CustodianBankName", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("CustodianBankName"))
                    objCommon.SetCommandParameters(sqlComm, "@CustodianBranch", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("CustodianBranch"))
                    objCommon.SetCommandParameters(sqlComm, "@CustodianAccountNo", SqlDbType.VarChar, 30, "I", , , dt.Rows(I).Item("CustodianAccountNo"))
                    objCommon.SetCommandParameters(sqlComm, "@CustodianRTGSCode", SqlDbType.VarChar, 30, "I", , , dt.Rows(I).Item("CustodianRTGSCode"))
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@CustodianId", SqlDbType.Int, 4, "I", , , Val(srh_Custodian.SelectedId))
                    objCommon.SetCommandParameters(sqlComm, "@CustodianBankId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveCustodianBanks", "Error in SaveCustodianBanks", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Private Function SaveContacts(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            'Hid_ContactDetailId.Value = ""
            dt = Session("ContactTable")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_ClientContactsDetail"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("ContactPerson").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@ContactPerson", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("ContactPerson"))
                    objCommon.SetCommandParameters(sqlComm, "@Designation", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("Designation"))
                    objCommon.SetCommandParameters(sqlComm, "@PhoneNo1", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("PhoneNo1"))
                    objCommon.SetCommandParameters(sqlComm, "@PhoneNo2", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("PhoneNo2"))
                    objCommon.SetCommandParameters(sqlComm, "@FaxNo1", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("FaxNo1"))
                    objCommon.SetCommandParameters(sqlComm, "@FaxNo2", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("FaxNo2"))
                    objCommon.SetCommandParameters(sqlComm, "@MobileNo", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("MobileNo"))
                    objCommon.SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("EmailId"))
                    objCommon.SetCommandParameters(sqlComm, "@Interaction", SqlDbType.Int, 4, "I", , , Val(dt.Rows(I).Item("Interaction")))
                    objCommon.SetCommandParameters(sqlComm, "@SectionType", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("SectionType"))
                    'objCommon.SetCommandParameters(sqlComm, "@Dept", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("Dept"))
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.SmallInt, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    sqlComm.ExecuteNonQuery()
                    Hid_ContactDetailId.Value += Val(sqlComm.Parameters.Item("@ContactDetailId").Value & "") & "!"

                Else
                    Hid_ContactDetailId.Value += "!"
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveContacts", "Error in SaveContacts", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Function SaveClientBusinessType(ByRef osqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            Dim J As Int16
            Dim arrContactDetailIds As Array
            Dim arrBusinessTypeIds As Array
            Dim arrClientBusniessDetailId As Array

            arrClientBusniessDetailId = Split(Hid_BusinessTypeId.Value, "!")
            arrContactDetailIds = Split(Hid_ContactDetailId.Value, "!")
            dt = Session("ContactTable")
            sqlComm.Transaction = osqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_ClientBusniessDetails"
            sqlComm.Connection = sqlConn
            For J = 0 To arrContactDetailIds.Length - 1

                arrBusinessTypeIds = Split(arrClientBusniessDetailId(J), ",")


                For I As Int16 = 0 To arrBusinessTypeIds.Length - 1
                    If Val(arrBusinessTypeIds(I)) <> 0 Then
                        sqlComm.Parameters.Clear()
                        objCommon.SetCommandParameters(sqlComm, "@BusinessTypeId", SqlDbType.SmallInt, 4, "I", , , Val(arrBusinessTypeIds(I)))
                        objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                        objCommon.SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.SmallInt, 4, "I", , , Val(arrContactDetailIds(J)))
                        objCommon.SetCommandParameters(sqlComm, "@ClientBusniessDetailId", SqlDbType.Int, 4, "O")
                        objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
                        objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 50, "O")
                        sqlComm.ExecuteNonQuery()
                        Hid_ClientBusniessDetailId.Value = Val(sqlComm.Parameters.Item("@ClientBusniessDetailId").Value & "")
                    End If
                Next
            Next
            Return True
        Catch ex As Exception
            osqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveClientBusinessType", "Error in SaveClientBusinessType", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
            Return False
        End Try
    End Function
    Private Function SaveSGLTrans(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            dt = Session("SGLTable")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            'sqlComm.CommandText = "ID_INSERT_CustomerSGLDetail"
            sqlComm.Connection = sqlConn

            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("SGLTransWith").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    If Val(dt.Rows(I).Item("CustSGLId") & "") = 0 Then
                        sqlComm.CommandText = "ID_INSERT_CustomerSGLDetail"
                        objCommon.SetCommandParameters(sqlComm, "@CustSGLId", SqlDbType.Int, 4, "O")
                    Else
                        sqlComm.CommandText = "ID_UPDATE_CustomerSGLDetail"

                        objCommon.SetCommandParameters(sqlComm, "@CustSGLId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("CustSGLId"))
                    End If
                    objCommon.SetCommandParameters(sqlComm, "@SGLTransWith", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("SGLTransWith"))
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@CustSGLUserId", SqlDbType.Int, 4, "I", , , Session("UserId"))
                    'objCommon.SetCommandParameters(sqlComm, "@CustSGLId ", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveSGLTrans", "Error in SaveSGLTrans", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Function SaveScheme(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            dt = Session("SchemeTable")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            'sqlComm.CommandText = "ID_INSERT_CustomerSGLDetail"
            sqlComm.Connection = sqlConn

            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("SchemeName").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    If dg_Scheme.Items(I).FindControl("deletebtn").Visible = True Then
                        sqlComm.CommandText = "ID_INSERT_CustomerSchemeDetails"
                        objCommon.SetCommandParameters(sqlComm, "@CustomerSchemeId", SqlDbType.Int, 4, "O")
                    Else
                        sqlComm.CommandText = "ID_UPDATE_CustomerSchemeDetails"
                        objCommon.SetCommandParameters(sqlComm, "@CustomerSchemeId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("CustomerSchemeId"))
                    End If
                    objCommon.SetCommandParameters(sqlComm, "@SchemeName", SqlDbType.VarChar, 500, "I", , , dt.Rows(I).Item("SchemeName"))
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    'objCommon.SetCommandParameters(sqlComm, "@CustSGLId ", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveScheme", "Error in SaveScheme", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Private Function SaveDPDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            dt = Session("DPTable")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure

            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("DpName").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    If Val(dt.Rows(I).Item("CustDPId") & "") = 0 Then
                        sqlComm.CommandText = "ID_INSERT_CustomerDPDetails"
                        objCommon.SetCommandParameters(sqlComm, "@CustDPId", SqlDbType.Int, 4, "O")
                    Else
                        sqlComm.CommandText = "ID_UPDATE_CustomerDPDetails"

                        objCommon.SetCommandParameters(sqlComm, "@CustDPId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("CustDPId"))
                    End If
                    objCommon.SetCommandParameters(sqlComm, "@DpName", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("DpName"))
                    objCommon.SetCommandParameters(sqlComm, "@DpId", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("DpId"))
                    objCommon.SetCommandParameters(sqlComm, "@ClientId", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("ClientId"))
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@CustDPUserId", SqlDbType.Int, 4, "I", , , Session("UserId"))
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            'old
            'For I As Int16 = 0 To dt.Rows.Count - 1
            '    If dt.Rows(I).Item("DpName").ToString <> "" Then
            '        sqlComm.Parameters.Clear()
            '        objCommon.SetCommandParameters(sqlComm, "@DpName", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("DpName"))
            '        objCommon.SetCommandParameters(sqlComm, "@DpId", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("DpId"))
            '        objCommon.SetCommandParameters(sqlComm, "@ClientId", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("ClientId"))
            '        objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
            '        objCommon.SetCommandParameters(sqlComm, "@CustDPId ", SqlDbType.Int, 4, "O")
            '        objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            '        sqlComm.ExecuteNonQuery()
            '    End If
            'Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveDPDetails", "Error in SaveDPDetails", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Private Function SaveErstWhile(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            dt = Session("CustomerErstWhile")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            'sqlComm.CommandText = "ID_INSERT_CustomerSGLDetail"
            sqlComm.Connection = sqlConn

            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("CustomerErstWhileName").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    'If dg_ErstWhile.Items(I).FindControl("deletebtn").Visible = True Then
                    If Val(dt.Rows(I).Item("CustomerErstWhileId") & "") = 0 Then
                        sqlComm.CommandText = "ID_INSERT_CustomerErstWhileDetail"
                        objCommon.SetCommandParameters(sqlComm, "@CustomerErstWhileId", SqlDbType.Int, 4, "O")
                    Else
                        sqlComm.CommandText = "ID_UPDATE_CustomerErstWhileDetail"
                        objCommon.SetCommandParameters(sqlComm, "@CustomerErstWhileId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("CustomerErstWhileId"))
                    End If
                    objCommon.SetCommandParameters(sqlComm, "@CustomerErstWhileName", SqlDbType.VarChar, 500, "I", , , dt.Rows(I).Item("CustomerErstWhileName"))
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@ErstWhileDate", SqlDbType.SmallDateTime, 9, "I", , , objCommon.DateFormat(dt.Rows(I).Item("ErstWhileDate")))
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function DeleteCustomerDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim SqlDa As New SqlDataAdapter
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_ClientCustomerDetailTable"
            'sqlComm.CommandText = "ID_DELETE_CustomerDetailTable"
            sqlComm.Connection = sqlConn
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            'SqlDa.SelectCommand = sqlComm

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "DeleteCustomerDetails", "Error in DeleteCustomerDetails", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Function DeleteMultiAddrContact(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_MultiAddtContact"
            'sqlComm.CommandText = "ID_DELETE_CustomerDetailTable"
            sqlComm.Connection = sqlConn
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@StrMessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "DeleteMultiAddrContact", "Error in DeleteMultiAddrContact", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Sub FillFields()
        'CHANGE 
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_ClientCustomerMaster", ViewState("Id"), "CustomerId")
            If dt.Rows.Count > 0 Then
                txt_CustomerCode.Text = Trim(dt.Rows(0).Item("CustomerCode") & "")
                cbo_CustomerType.SelectedValue = Val(dt.Rows(0).Item("CustomerTypeId") & "")

                objCommon.FillControl(cbo_Category, sqlConn, "ID_FILL_CategoryMaster", "CategoryName", "CategoryId", cbo_CustomerType.SelectedValue, "CustomerTypeId")
                'objCommon.FillControl(cbo_DealerName, sqlConn, "ID_FILL_Dealer", "NameOfUser", "UserId")

                objCommon.FillControl(cbo_DealerName, sqlConn, "ID_FILL_CustMasterDealer", "NameOfUser", "UserId", Val(Session("UserId")), "UserId")
                cbo_DealerName.SelectedValue = Val(dt.Rows(0).Item("CustomerDealer") & "")
                cbo_CustomerGroup.SelectedValue = Val(dt.Rows(0).Item("GroupId") & "")
                cbo_Category.SelectedValue = Val(dt.Rows(0).Item("CategoryId") & "")
                cbo_Category.Enabled = True
                'cbo_CustomerType.Enabled = False
                txt_CustomerName.Text = Trim(dt.Rows(0).Item("CustomerName") & "")
                txt_CustPrefix.Text = Trim(dt.Rows(0).Item("CustPrefix") & "")
                txt_Address1.Text = Trim(dt.Rows(0).Item("CustomerAddress1") & "")
                txt_Address2.Text = Trim(dt.Rows(0).Item("CustomerAddress2") & "")
                txt_PinCode.Text = Trim(dt.Rows(0).Item("CustomerPinCode") & "")
                txt_City.Text = Trim(dt.Rows(0).Item("CustomerCity") & "")
                txt_State.SearchTextBox.Text = Trim(dt.Rows(0).Item("State") & "")
                txt_State.SelectedId = Trim(dt.Rows(0).Item("StateId") & "")
                txt_Country.Text = Trim(dt.Rows(0).Item("Country") & "")
                txt_PhoneNo.Text = Trim(dt.Rows(0).Item("CustomerPhone") & "")
                txt_FaxNo.Text = Trim(dt.Rows(0).Item("CustomerFax") & "")
                txt_PANNo.Text = Trim(dt.Rows(0).Item("PANNumber") & "")
                txt_TANNo.Text = Trim(dt.Rows(0).Item("TANNumber") & "")
                txt_EmailId.Text = Trim(dt.Rows(0).Item("EmailId") & "")
                txt_empalmentdate.Text = Trim(dt.Rows(0).Item("EmpalmentDate") & "")
                cbo_FrequencyEmpalment.SelectedValue = Trim(dt.Rows(0).Item("EmpalmentFrequency") & "")
                rdo_Headselect.SelectedValue = Trim(dt.Rows(0).Item("HeadSelect") & "")
                txt_GSTNo.Text = Trim(dt.Rows(0).Item("CustomerGST") & "")
                txt_Remarks.Text = Trim(dt.Rows(0).Item("Remarks") & "")
                If rdo_Headselect.SelectedValue = "N" Then
                    srh_HeadCustomer.SelectedId = Val(dt.Rows(0).Item("HeadCutomerId") & "")
                    srh_HeadCustomer.SearchTextBox.Text = dt.Rows(0).Item("HeadCustomer").ToString
                End If
                cbo_Accessible.SelectedValue = Trim(dt.Rows(0).Item("AccessLevel") & "")

                Rdo_Custodian.SelectedValue = Trim(dt.Rows(0).Item("Custodian") & "")
                If Rdo_Custodian.SelectedValue = "Y" Then
                    'txt_Custodian.Text = dt.Rows(0).Item("CustodianName").ToString
                    srh_Custodian.SelectedId = Val(dt.Rows(0).Item("CustodianId") & "")
                    srh_Custodian.SearchTextBox.Text = dt.Rows(0).Item("Custodian1").ToString
                End If
                FillDocType()

                If Val(dt.Rows(0).Item("BranchId") & "") <> 0 Then
                    cbo_Branch.SelectedValue = Val(dt.Rows(0).Item("BranchId") & "")
                End If
                If Convert.ToString(dt.Rows(0).Item("Abbreviation")) <> "" Then
                    cbo_Abbreviation.SelectedValue = Trim(dt.Rows(0).Item("Abbreviation") & "")
                End If
                rdo_HideShow.SelectedValue = Trim(dt.Rows(0).Item("HideShow") & "")
            End If
            rdo_BillSettType.SelectedValue = Trim(dt.Rows(0).Item("BillSettType") & "")
            txt_BSECustomerCode.Text = Trim(dt.Rows(0).Item("BSECustomerCode") & "")
            txt_NSECustomerCode.Text = Trim(dt.Rows(0).Item("NSECustomerCode") & "")
            Hid_DocumentDetails.Value = Trim(dt.Rows(0).Item("DocumentDetails") & "")


        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillFields", "Error in FillFields", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Sub FillCustomerDetailsGrid()
        Try
            Dim dt As DataTable
            Dim lstItem As ListItem
            OpenConn()
            dt = objCommon.FillDetailsGrid(dg_Bank, "ID_FILL_CustomerBankDetails", "CustomerId", Val(ViewState("Id") & ""))

            Session("BankTable") = dt

            dt = objCommon.FillDetailsGrid(dg_Contact, "ID_FILL_ClientContactDetails", "CustomerId", Val(ViewState("Id") & ""))
            Session("ContactTable") = dt

            dt = objCommon.FillDetailsGrid(dg_DP, "ID_FILL_CustomerdPDetails", "CustomerId", Val(ViewState("Id") & ""))
            Session("DPTable") = dt

            dt = objCommon.FillDetailsGrid(dg_SGL, "ID_FILL_CustomerSGLDetails", "CustomerId", Val(ViewState("Id") & ""))
            Session("SGLTable") = dt

            dt = objCommon.FillDetailsGrid(dg_Signatury, "ID_FILL_CustomerSignaturyDetails", "CustomerId", Val(ViewState("Id") & ""))
            Session("SignaturyTable") = dt

            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_ClientCustMultipleAddress", Val(ViewState("Id") & ""), "CustomerId", , "NO", "BusniessType", "CM", "ProfileType")
            'Session("CustAddressTable") = dt
            Session("MainCustAddressTable") = dt

            dt = objCommon.FillDetailsGrid(dgCustodianbankdetails, "ID_FILL_CustodianBankDetails", "CustomerId", Val(ViewState("Id") & ""))
            Session("CustodianBankTable") = dt


            dt = objCommon.FillDetailsGrid(dg_Scheme, "ID_FILL_CustomerSchemeDetails", "CustomerId", Val(ViewState("Id") & ""))
            Session("SchemeTable") = dt

            dt = objCommon.FillDetailsGrid(dg_Dealer, "ID_FILL_CustomerDealerDetails", "CustomerId", Val((ViewState("Id")) & ""))
            Session("CustomerDealerTable") = dt

            dt = objCommon.FillDetailsGrid(dg_Broker, "ID_FILL_CustomerBrokerDetails", "CustomerId", Val((ViewState("Id")) & ""))
            Session("CustomerBrokerTable") = dt

            dt = objCommon.FillDetailsGrid(dg_ErstWhile, "ID_FILL_CustomerErstWhileDetail", "CustomerId", Val(ViewState("Id") & ""))
            Session("CustomerErstWhile") = dt
            dt = objCommon.FillControl(srh_KYCDocuments.SelectListBox, sqlConn, "ID_FILL_CustDoc", "DocumentTypeName", "DocumentTypeId", ViewState("Id"), "CustomerId", "K", "DocType")
            lstItem = srh_KYCDocuments.SelectListBox.Items.FindByText("")
            If lstItem IsNot Nothing Then
                srh_KYCDocuments.SelectListBox.Items.Remove(lstItem)
            End If

            dt = objCommon.FillControl(srh_EmpalementDocuments.SelectListBox, sqlConn, "ID_FILL_CustDoc", "DocumentTypeName", "DocumentTypeId", ViewState("Id"), "CustomerId", "E", "DocType")
            lstItem = srh_EmpalementDocuments.SelectListBox.Items.FindByText("")
            If lstItem IsNot Nothing Then
                srh_EmpalementDocuments.SelectListBox.Items.Remove(lstItem)
            End If


            dt = objCommon.FillControl(srh_BusniessType.SelectListBox, sqlConn, "ID_FILL_CustomerBusinessTypes", "BusinessType", "BusinessTypeId", ViewState("Id"), "CustomerId")
            lstItem = srh_BusniessType.SelectListBox.Items.FindByText("")
            If lstItem IsNot Nothing Then
                srh_BusniessType.SelectListBox.Items.Remove(lstItem)
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillCustomerDetailsGrid", "Error in FillCustomerDetailsGrid", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub DeleteBlankRow(ByRef dt As DataTable, ByVal strColName As String)
        Try
            For I As Int16 = 0 To dt.Rows.Count - 1
                If String.IsNullOrEmpty(dt.Rows(I).Item(strColName).ToString()) Then
                    dt.Rows.RemoveAt(I)
                    Exit Sub
                End If
            Next
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub InsertUpdateDealer(ByVal blnEdit As Boolean)
        Try
            Dim dr As DataRow
            Dim dt As DataTable
            Dim intIndex As Int16
            intIndex = Val(ViewState("RowIndex") & "")
            dt = Session("CustomerDealerTable")
            Dim view As New DataView(dt)
            view.Sort = "CustomerDealerId"

            If blnEdit = False Then
                For I As Int16 = 0 To dt.Rows.Count - 1
                    If (Val(dt.Rows(I).Item("CustomerDealerId"))) = Val(cbo_DealerName.SelectedValue) Then
                        'record exists
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Dealer Already Exists');", True)
                        Exit Sub
                    End If
                Next
                dr = dt.NewRow
            Else
                dr = dt.Rows(intIndex)
            End If
            dr.Item("NameOfUser") = Trim(cbo_DealerName.SelectedItem.Text)

            If txt_StartDate.Text <> "" Then
                dr.Item("StartDate") = Format(objCommon.DateFormat(txt_StartDate.Text), "dd/MM/yyyy")
            End If

            If txt_EndDate.Text <> "" Then
                dr.Item("EndDate") = Format(objCommon.DateFormat(txt_EndDate.Text), "dd/MM/yyyy")
            End If

            dr.Item("CustomerDealerId") = Val(cbo_DealerName.SelectedValue)
            dr.Item("CustDealerDetailId") = 0
            dr.Item("CustomerId") = Val(ViewState("Id"))
            'dt.Rows.Add(dr)

            If blnEdit = False Then dt.Rows.Add(dr)

            DeleteBlankRow(dt, "NameOfUser")
            cbo_DealerName.SelectedIndex = 0
            cbo_DealerName.SelectedItem.Text = ""
            txt_StartDate.Text = ""
            txt_EndDate.Text = ""
            dg_Dealer.DataSource = dt
            dg_Dealer.DataBind()
            Session("CustomerDealerTable") = dt
            ViewState("DealerEditFlag") = False




        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Sub InsertUpdateBroker(ByVal blnEdit As Boolean)
        Try
            Dim dr As DataRow
            Dim dt As DataTable
            Dim intIndex As Int16
            intIndex = Val(ViewState("RowIndex") & "")
            dt = Session("CustomerBrokerTable")
            Dim view As New DataView(dt)
            view.Sort = "CustomerBrokerId"

            If blnEdit = False Then
                For I As Int16 = 0 To dt.Rows.Count - 1
                    If (Val(dt.Rows(I).Item("CustomerBrokerId"))) = Val(srh_Brokername.SelectedId) Then
                        'record exists
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Broker Already Exists');", True)
                        Exit Sub
                    End If
                Next
                dr = dt.NewRow
            Else
                dr = dt.Rows(intIndex)
            End If
            dr.Item("BrokerName") = Trim(srh_Brokername.SearchTextBox.Text)

            If txt_BrokerStartDate.Text <> "" Then
                dr.Item("StartDate") = Format(objCommon.DateFormat(txt_BrokerStartDate.Text), "dd/MM/yyyy")
            End If

            If txt_BrokerEndDate.Text <> "" Then
                dr.Item("EndDate") = Format(objCommon.DateFormat(txt_BrokerEndDate.Text), "dd/MM/yyyy")
            End If

            dr.Item("CustomerBrokerId") = Val(srh_Brokername.SelectedId)
            dr.Item("CustBrokerDetailId") = 0
            dr.Item("CustomerId") = Val(ViewState("Id"))
            'dt.Rows.Add(dr)

            If blnEdit = False Then dt.Rows.Add(dr)

            DeleteBlankRow(dt, "BrokerName")
            srh_Brokername.SelectedId = 0
            srh_Brokername.SearchTextBox.Text = ""
            txt_BrokerStartDate.Text = ""
            txt_BrokerEndDate.Text = ""
            dg_Broker.DataSource = dt
            dg_Broker.DataBind()
            Session("CustomerBrokerTable") = dt
            ViewState("BrokerEditFlag") = False




        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub ShowImage(ByVal intIndex As Int16)
        Try
            Dim dt As DataTable
            Dim arrContent() As Byte
            dt = Session("SignaturyTable")
            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(intIndex)
                arrContent = CType(dr.Item("FileBytes"), Byte())
                Dim conType As String = dr.Item("ContentType").ToString()
                Response.Clear()
                Response.Charset = ""
                Response.ClearHeaders()
                Response.ContentType = conType
                Response.AddHeader("content-disposition", "attachment;filename=" & dr.Item("FileName"))
                Response.BinaryWrite(arrContent)
                Response.Flush()
                Response.End()
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "ShowImage", "Error in ShowImage", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub


    Private Function SaveCustomerBusinessType(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_CustomerBusinessTypes"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To srh_BusniessType.SelectListBox.Items.Count - 1
                If Val(srh_BusniessType.SelectListBox.Items(I).Value) <> 0 Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@BusinessTypeId", SqlDbType.Int, 4, "I", , , srh_BusniessType.SelectListBox.Items(I).Value)
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    objCommon.SetCommandParameters(sqlComm, "@CustBusinessTypeId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveCustomerBusinessType", "Error in SaveCustomerBusinessType", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    'Protected Sub cbo_CustomerType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CustomerType.SelectedIndexChanged





    Private Sub ShowDetailForms()
        Try
            Dim strHtml As String
            If (cbo_CustomerType.SelectedItem.Text = "BANK") Then
                strHtml = "ShowDetails('CustBankDetails','950px','650px','225px');"
            ElseIf (cbo_CustomerType.SelectedItem.Text = "INSURANCE") Then
                strHtml = "ShowDetails('CustInsuranceDetails','950px','650px','225px');"
            ElseIf (cbo_CustomerType.SelectedItem.Text = "MUTUAL FUND") Then
                strHtml = "ShowDetails('CustInsuranceDetails','950px','650px','225px');"
            ElseIf (cbo_CustomerType.SelectedItem.Text = "EXEMPT TRUST FUNDS") Then
                strHtml = "ShowDetails('Cust','950px','650px','225px');"
            Else
                strHtml = "ShowDetails('CustOtherDetails','950px','650px','225px');"
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Open", strHtml, True)

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "ShowDetailForms", "Error in ShowDetailForms", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub dg_Signatury_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_Signatury.ItemCommand
        Try
            Dim dtGrid As DataTable
            dtGrid = Session("SignaturyTable")
            If e.CommandName.ToUpper = "ADD" Then
                Dim dr As DataRow = dtGrid.NewRow
                dr("signaturyName") = CType(e.Item.FindControl("txt_signatury"), TextBox).Text
                dr("FileBytes") = CType(e.Item.FindControl("file_AddFile"), FileUpload).FileBytes
                dr("FileName") = CType(e.Item.FindControl("file_AddFile"), FileUpload).FileName
                dr("ContentType") = CType(e.Item.FindControl("file_AddFile"), FileUpload).PostedFile.ContentType
                dr("ContentLength") = CType(e.Item.FindControl("file_AddFile"), FileUpload).PostedFile.ContentLength
                dtGrid.Rows.Add(dr)
            ElseIf e.CommandName.ToUpper() = "DELETE" Then
                dtGrid.Rows.RemoveAt(e.Item.ItemIndex)
            ElseIf e.CommandName = "Show" Then
                ShowImage(e.Item.ItemIndex)
            End If
            Session("SignaturyTable") = dtGrid
            dg_Signatury.DataSource = dtGrid
            dg_Signatury.DataBind()




        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Signatury_ItemCommand", "Error in dg_Signatury_ItemCommand", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub dg_Signatury_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_Signatury.ItemDataBound
        Try
            Dim lblFileName As Label
            Dim Linkbtn As LinkButton
            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                Linkbtn = CType(e.Item.FindControl("file_AddFile"), LinkButton)
                lblFileName = CType(e.Item.FindControl("lbl_AddFile"), Label)
                If Linkbtn.Text = "" Then
                    Linkbtn.Visible = False
                    lblFileName.Visible = True
                    lblFileName.Text = "Not Available"
                Else
                    Linkbtn.Visible = True
                    lblFileName.Visible = False


                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub SetSignaturyGrid(ByVal objGrid As DataGrid, ByVal e As DataGridCommandEventArgs, ByVal strSessionName As String)
        Try
            Dim dtGrid As DataTable
            dtGrid = TryCast(Session("SignaturyTable"), DataTable).Copy
            If e.CommandName.ToUpper() = "ADD" Or e.CommandName.ToUpper() = "UPDATE" Then
                Dim dr As DataRow
                If e.CommandName.ToUpper() = "ADD" Then
                    dr = dtGrid.NewRow
                Else
                    dr = dtGrid.Rows(e.Item.ItemIndex)
                End If
                dr("signaturyName") = Trim(TryCast(e.Item.FindControl("txt_SGLTransWith"), TextBox).Text)
                If e.CommandName.ToUpper() = "ADD" Then
                    dtGrid.Rows.Add(dr)
                End If
                objGrid.EditItemIndex = -1
            ElseIf e.CommandName.ToUpper() = "DELETE" Then
                dtGrid.Rows.RemoveAt(e.Item.ItemIndex)
            ElseIf e.CommandName.ToUpper() = "EDIT" Then
                objGrid.EditItemIndex = e.Item.ItemIndex
            ElseIf e.CommandName.ToUpper() = "CANCEL" Then
                objGrid.EditItemIndex = -1
            End If
            Session("SGLTable") = dtGrid
            objGrid.DataSource = dtGrid
            objGrid.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetSignaturyGrid", "Error in SetSignaturyGrid", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub btn_AddDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddDetails.Click
        Try
            Dim strRetValues() As String
            Dim dt As DataTable
            Dim dr As DataRow

            Dim strRowIndex As String = Hid_RowIndex.Value
            If Hid_RetValues.Value <> "" Then
                If strRowIndex <> "" Then
                    Dim intRowIndex As Integer = Convert.ToInt32(strRowIndex)
                    dt = Session("contactTable")
                    strRetValues = Split(Hid_RetValues.Value, "!")
                    dt.Rows(intRowIndex).Item("ContactPerson") = strRetValues(0)
                    dt.Rows(intRowIndex).Item("Designation") = strRetValues(1)
                    dt.Rows(intRowIndex).Item("PhoneNo1") = strRetValues(2)
                    dt.Rows(intRowIndex).Item("MobileNo") = strRetValues(3)
                    dt.Rows(intRowIndex).Item("EmailId") = (strRetValues(4))
                    dt.Rows(intRowIndex).Item("PhoneNo2") = strRetValues(5)
                    dt.Rows(intRowIndex).Item("FaxNo1") = strRetValues(6)
                    dt.Rows(intRowIndex).Item("FaxNo2") = strRetValues(7)
                    dt.Rows(intRowIndex).Item("Interaction") = strRetValues(8)
                    dt.Rows(intRowIndex).Item("SectionType") = strRetValues(9)
                    dt.Rows(intRowIndex).Item("Branch") = strRetValues(10)

                Else
                    strRetValues = Split(Hid_RetValues.Value, "!")
                    dt = Session("contactTable")
                    dr = dt.NewRow
                    dr.Item("ContactPerson") = Trim(strRetValues(0) & "")
                    dr.Item("Designation") = Trim(strRetValues(1))
                    dr.Item("PhoneNo1") = Trim(strRetValues(2))
                    dr.Item("MobileNo") = strRetValues(3)
                    dr.Item("EmailId") = strRetValues(4)

                    dr.Item("PhoneNo2") = Trim(strRetValues(5))
                    dr.Item("FaxNo1") = Trim(strRetValues(6))
                    dr.Item("FaxNo2") = Trim(strRetValues(7))
                    dr.Item("Interaction") = Trim(strRetValues(8))
                    dr.Item("SectionType") = Trim(strRetValues(9))
                    dr.Item("ContactDetailId") = 0
                    dt.Rows.Add(dr)

                End If
            End If

            Session("contactTable") = dt
            FillContactDetails()
            Hid_RowIndex.Value = ""

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_AddDetails_Click", "Error in btn_AddDetails_Click", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub FillContactDetails()
        Try
            Dim dt As DataTable
            If TypeOf Session("contactTable") Is DataTable Then
                dt = Session("contactTable")
            Else
                dt = objCommon.FillDetailsGrid(dg_Contact, "ID_FILL_ClientContactDetails", "CustomerId", Val((Hid_CustomerId.Value) & ""))
            End If
            Session("contactTable") = dt
            ClearOptions()
            dg_Contact.DataSource = dt
            dg_Contact.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillContactDetails", "Error in FillContactDetails", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub ClearOptions()
        Try
            Hid_BusinessTypeId.Value = ""
            Hid_ContactDetailId.Value = ""
            Hid_ContactDetailIds.Value = ""
            Hid_ClientBusniessDetailId.Value = ""
            Hid_BusinessTypeNames.Value = ""
            Hid_UserIds.Value = ""
            Hid_NameOfUsers.Value = ""
            Hid_PhoneNo2.Value = ""
            Hid_FaxNo1.Value = ""
            Hid_FaxNo2.Value = ""
            Hid_ClientCustAddressId.Value = ""

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "ClearOptions", "Error in ClearOptions", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub dg_Contact_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_Contact.ItemDataBound
        Try
            Dim img As New HtmlImage
            Dim lnkBtnEdit As LinkButton
            Dim lnkbtnDelete As LinkButton
            Dim intContactDetailId As Integer
            Dim lnkhtmlbtn As HtmlInputButton
            Dim dtGrid As DataTable

            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                e.Item.ID = "itm" & e.Item.ItemIndex

                lnkbtnDelete = CType(e.Item.FindControl("imgBtn_Del"), LinkButton)
                lnkbtnDelete.Attributes.Add("onclick", "return Deletedetails()")
                lnkBtnEdit = CType(e.Item.FindControl("imgBtn_Edit"), LinkButton)
                lnkhtmlbtn = CType(e.Item.FindControl("imgBtn_Edit1"), HtmlInputButton)
                lnkBtnEdit.Attributes.Add("onclick", "return UpdateContactDetails('" & e.Item.ItemIndex & "')")
                lnkhtmlbtn.Attributes.Add("onclick", "return UpdateContactDetails(this,'" & e.Item.ItemIndex & "' );")
                dtGrid = CType(Session("contactTable"), DataTable)
                'Hid_BusinessTypeId.Value += Trim(CType(e.Item.FindControl("lbl_BusniessTypeIds"), Label).Text) & "!"
                'Hid_BusinessTypeNames.Value += Trim(CType(e.Item.FindControl("lbl_BusinessTypeNames"), TextBox).Text) & "!"
                'Hid_UserIds.Value += Trim(CType(e.Item.FindControl("lbl_UserIds"), Label).Text) & "!"
                'Hid_NameOfUsers.Value += Trim(CType(e.Item.FindControl("lbl_NameOfUsers"), TextBox).Text) & "!"
                Hid_PhoneNo2.Value += Trim(CType(e.Item.FindControl("lbl_PhoneNo2"), Label).Text) & "!"
                Hid_FaxNo1.Value += Trim(CType(e.Item.FindControl("lbl_FaxNo1"), Label).Text) & "!"
                Hid_FaxNo2.Value += Trim(CType(e.Item.FindControl("lbl_FaxNo2"), Label).Text) & "!"
                Hid_Interaction.Value += Trim(CType(e.Item.FindControl("lbl_Interaction"), Label).Text) & "!"
                intContactDetailId = Val(CType(e.Item.FindControl("lbl_ContactDetailId"), Label).Text)
                Hid_SectionType.Value += Trim(CType(e.Item.FindControl("lbl_SectionType"), Label).Text) & "!"
                'Hid_ContactDetailId.Value = Val(CType(e.Item.FindControl("lbl_ContactDetailId"), Label).Text)
                Hid_ContactDetailIds.Value += Val(CType(e.Item.FindControl("lbl_ContactDetailId"), Label).Text) & "!"
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Contact_ItemDataBound", "Error in dg_Contact_ItemDataBound", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Function SaveAddresses(ByVal sqlTrans As SqlTransaction) As Boolean
        Try

            Dim sqlComm As New SqlCommand

            'Dim dgItem As DataGridItem

            Dim strCustomerBranchName As String
            Dim strCity As String
            Dim strPinCode As String
            Dim strState As String
            Dim strCountry As String
            Dim strPhone As String
            Dim strFaxNo As String
            Dim strEmailId As String
            Dim strCustomerId As Integer
            Dim strAddress1 As String
            Dim strAddress2 As String
            Dim strTempId As Integer


            Dim i As Integer
            Dim dt As DataTable
            dt = Session("MainCustAddressTable")

            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            'sqlComm.CommandText = "ID_INSERT_ClientCustMultipleAddress"
            sqlComm.Connection = sqlConn
            If Not dt Is Nothing Then
                For i = 0 To dt.Rows.Count - 1
                    'If dt.Rows(i).Item("CustomerBranchName").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    strCustomerBranchName = dt.Rows(i).Item("CustomerBranchName")
                    strCity = dt.Rows(i).Item("City")
                    strPinCode = dt.Rows(i).Item("PinCode")
                    strState = dt.Rows(i).Item("State")
                    strCountry = dt.Rows(i).Item("Country")
                    strPhone = dt.Rows(i).Item("Phone")
                    strFaxNo = dt.Rows(i).Item("FaxNo")
                    strAddress1 = dt.Rows(i).Item("Address1")
                    strAddress2 = dt.Rows(i).Item("Address2")
                    strEmailId = dt.Rows(i).Item("EmailId")
                    strTempId = dt.Rows(i).Item("TempId")
                    strCustomerId = ViewState("Id")
                    sqlComm.Parameters.Clear()
                    If dt.Rows(i).Item("ClientCustAddId").ToString = "" Then
                        sqlComm.CommandText = "ID_INSERT_ClientCustMultipleAddress"
                        objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "O")
                    Else
                        sqlComm.CommandText = "ID_UPDATE_ClientCustMultipleAddress"
                        objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "I", , , dt.Rows(i).Item("ClientCustAddressId"))
                    End If

                    objCommon.SetCommandParameters(sqlComm, "@CustomerBranchName", SqlDbType.VarChar, 100, "I", , , strCustomerBranchName)
                    objCommon.SetCommandParameters(sqlComm, "@City", SqlDbType.VarChar, 20, "I", , , strCity)
                    objCommon.SetCommandParameters(sqlComm, "@PinCode", SqlDbType.VarChar, 15, "I", , , strPinCode)
                    objCommon.SetCommandParameters(sqlComm, "@State", SqlDbType.VarChar, 100, "I", , , strState)
                    objCommon.SetCommandParameters(sqlComm, "@Country", SqlDbType.VarChar, 100, "I", , , strCountry)
                    objCommon.SetCommandParameters(sqlComm, "@Phone", SqlDbType.VarChar, 50, "I", , , strPhone)
                    objCommon.SetCommandParameters(sqlComm, "@FaxNo", SqlDbType.VarChar, 15, "I", , , strFaxNo)
                    objCommon.SetCommandParameters(sqlComm, "@Address1", SqlDbType.VarChar, 100, "I", , , strAddress1)
                    objCommon.SetCommandParameters(sqlComm, "@Address2", SqlDbType.VarChar, 100, "I", , , strAddress2)
                    objCommon.SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , strEmailId)
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , strCustomerId)
                    objCommon.SetCommandParameters(sqlComm, "@TempId", SqlDbType.Int, 4, "I", , , strTempId)
                    objCommon.SetCommandParameters(sqlComm, "@ProfileType", SqlDbType.Char, 2, "I", , , "CM")
                    objCommon.SetCommandParameters(sqlComm, "@BusniessType", SqlDbType.Char, 2, "I", , , "NO")
                    'objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                    sqlComm.ExecuteNonQuery()
                    'If objCommon.SaveClientAddBusniessDetailsNew(sqlConn, sqlTrans, Trim(dt.Rows(i).Item("BusniessTypeIds") & ""), Val(sqlComm.Parameters("@ClientCustAddressId").Value), strCustomerId) = False Then Return False
                    'End If
                    If Val(sqlComm.Parameters.Item("@ClientCustAddressId").Value & "") <> 0 Then
                        Hid_ClientCustAddressId.Value += Val(sqlComm.Parameters.Item("@ClientCustAddressId").Value & "") & "!"
                    End If
                Next

            End If
            Return True

        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveAddresses", "Error in SaveAddresses", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Public Function SaveMultiAddrContact(ByVal sqlTrans As SqlTransaction) As Boolean

        Try
            Dim sqlComm As New SqlCommand
            Dim dt As New DataTable
            Dim sqlda As New SqlDataAdapter
            Dim arrContactDetailIds As Array
            Dim J As Int16
            'OpenConn()
            With sqlComm
                .Connection = sqlConn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = "Id_UPDATE_MultiAddrContactPerson"
                dt = Session("MAddrcontactTable")
                arrContactDetailIds = Split(Hid_ClientCustAddressId.Value, "!")
                For I As Int16 = 0 To dt.Rows.Count - 1
                    For J = 0 To arrContactDetailIds.Length - 1
                        If Val(arrContactDetailIds(J)) <> 0 Then
                            dt.Rows(I).Item("ClientCustAddressId") = Val(arrContactDetailIds(J))
                            sqlComm.Parameters.Clear()
                            objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("ClientCustAddressId"))
                            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
                            sqlComm.ExecuteNonQuery()
                        End If
                    Next
                    sqlda.SelectCommand = sqlComm
                    sqlda.Fill(dt)
                    Session("MAddrcontactTable") = dt
                Next
                Return True
            End With
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            'CloseConn()
        End Try
    End Function

    Protected Sub dgCustodianbankdetails_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgCustodianbankdetails.ItemCommand
        SetCustodianBankGrid(dgCustodianbankdetails, e, "CustodianBankTable")
    End Sub
    Private Sub SetCustodianBankGrid(ByVal objGrid As DataGrid, ByVal e As DataGridCommandEventArgs, ByVal strSessionName As String)
        Try
            Dim dtGrid As DataTable
            dtGrid = TryCast(Session("CustodianBankTable"), DataTable).Copy
            If e.CommandName.ToUpper() = "ADD" Or e.CommandName.ToUpper() = "UPDATE" Then
                Dim dr As DataRow
                If e.CommandName.ToUpper() = "ADD" Then
                    dr = dtGrid.NewRow
                Else
                    dr = dtGrid.Rows(e.Item.ItemIndex)
                End If
                dr("CustodianBankName") = Trim(TryCast(e.Item.FindControl("txt_CustodianBankName"), TextBox).Text)
                dr("CustodianAccountNo") = Trim(TryCast(e.Item.FindControl("txt_CustodianAccountNo"), TextBox).Text)
                dr("CustodianBranch") = Trim(TryCast(e.Item.FindControl("txt_CustodianBranch"), TextBox).Text)
                dr("CustodianRTGSCode") = Trim(TryCast(e.Item.FindControl("txt_CustodianRTGSCode"), TextBox).Text)
                If e.CommandName.ToUpper() = "ADD" Then
                    dtGrid.Rows.Add(dr)
                End If
                objGrid.EditItemIndex = -1
            ElseIf e.CommandName.ToUpper() = "DELETE" Then
                dtGrid.Rows.RemoveAt(e.Item.ItemIndex)
            ElseIf e.CommandName.ToUpper() = "EDIT" Then
                objGrid.EditItemIndex = e.Item.ItemIndex
            ElseIf e.CommandName.ToUpper() = "CANCEL" Then
                objGrid.EditItemIndex = -1
            End If
            Session("CustodianBankTable") = dtGrid
            objGrid.DataSource = dtGrid
            objGrid.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetCustodianBankGrid", "Error in SetCustodianBankGrid", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub ClearFields()
        Try
            Dim dt As DataTable
            dt = CType(Session("CustodianBankTable"), DataTable)
            dt.Rows.Clear()
            dgCustodianbankdetails.DataSource = dt
            dgCustodianbankdetails.DataBind()
            Session("CustodianBankTable") = dt

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try

    End Sub

    Private Function UpdateDealSlipContactDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try

            Dim sqlComm As New SqlCommand

            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_UPDATE_DealSlipContactDetail"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.SmallInt, 4, "I", , , ViewState("Id"))
            objCommon.SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.Int, 4, "I", , , Val(Hid_ContactDetailId.Value))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "UpdateDealSlipContactDetails", "Error in UpdateDealSlipContactDetails", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Protected Sub dg_DP_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_DP.ItemDataBound
        Try
            'objCommon.SetGridRows(e)
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.EditItem Then
                Dim dr As DataRow
                Dim ImgBtnDelete As LinkButton
                ImgBtnDelete = CType(e.Item.FindControl("deletebtn"), LinkButton)
                dr = TryCast(e.Item.DataItem, DataRowView).Row
                If dr("CustomerDPId").ToString <> "" Then
                    ImgBtnDelete.Visible = False
                End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_DP_ItemDataBound", "Error in dg_DP_ItemDataBound", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub dg_SGL_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_SGL.ItemDataBound
        Try
            'objCommon.SetGridRows(e)
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.EditItem Then
                Dim dr As DataRow
                Dim ImgBtnDelete As LinkButton
                ImgBtnDelete = CType(e.Item.FindControl("deletebtn"), LinkButton)
                dr = TryCast(e.Item.DataItem, DataRowView).Row
                If dr("CustomerSGLId").ToString <> "" Then
                    ImgBtnDelete.Visible = False
                End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_SGL_ItemDataBound", "Error in dg_SGL_ItemDataBound", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub dg_Bank_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_Bank.ItemDataBound
        Try
            'objCommon.SetGridRows(e)
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.EditItem Then
                Dim dr As DataRow
                Dim ImgBtnDelete As LinkButton
                ImgBtnDelete = CType(e.Item.FindControl("deletebtn"), LinkButton)
                dr = TryCast(e.Item.DataItem, DataRowView).Row
                If dr("CustomerBankId").ToString <> "" Then
                    ImgBtnDelete.Visible = False
                End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Bank_ItemDataBound", "Error in dg_Bank_ItemDataBound", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Sub FillDocType()
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable1(sqlConn, "ID_FILL_CustomerTypeMaster", cbo_CustomerType.SelectedValue, "CustomerTypeId")
            If dt.Rows.Count > 0 Then
                Hid_DecType.Value = Trim(dt.Rows(0).Item("DocType") & "")
            End If
            Dim ListBox1 As ListBox
            ListBox1 = CType(srh_EmpalementDocuments.FindControl("lst_Select"), ListBox)
            ListBox1.Items.Clear()
            ListBox1 = CType(srh_KYCDocuments.FindControl("lst_Select"), ListBox)
            ListBox1.Items.Clear()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "s1", "SelectDocType();", True)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillDocType", "Error in FillDocType", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub cbo_CustomerType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CustomerType.SelectedIndexChanged
        Try
            OpenConn()
            objCommon.FillControl(cbo_Category, sqlConn, "ID_FILL_CategoryMaster", "CategoryName", "CategoryId", cbo_CustomerType.SelectedValue, "CustomerTypeId")
            If (Hid_CustomerId.Value <> "") Then

            End If
            FillDocType()
            'FillListBox()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "cbo_CustomerType_SelectedIndexChanged", "Error in cbo_CustomerType_SelectedIndexChanged", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub fillListBoxOnUpdate()
        Try

            OpenConn()
            If Hid_DecType.Value = "B" Then
                objCommon.FillControl(srh_KYCDocuments.SelectListBox, sqlConn, "ID_FILL_DocumentTypes_Customer_Kyc", "DocumentName", "DocumentId", ViewState("Id"), "CustomerId")
                lstItem = srh_KYCDocuments.SelectListBox.Items.FindByText("")
                If lstItem IsNot Nothing Then
                    srh_KYCDocuments.SelectListBox.Items.Remove(lstItem)
                End If

                objCommon.FillControl(srh_EmpalementDocuments.SelectListBox, sqlConn, "ID_FILL_DocumentTypes_Customer_Emp", "DocumentName", "DocumentId", ViewState("Id"), "CustomerId")
                lstItem = srh_EmpalementDocuments.SelectListBox.Items.FindByText("")
                If lstItem IsNot Nothing Then
                    srh_EmpalementDocuments.SelectListBox.Items.Remove(lstItem)
                End If
            ElseIf Hid_DecType.Value = "K" Then
                objCommon.FillControl(srh_KYCDocuments.SelectListBox, sqlConn, "ID_FILL_DocumentTypes_Customer_Kyc", "DocumentName", "DocumentId", ViewState("Id"), "CustomerId")
                lstItem = srh_KYCDocuments.SelectListBox.Items.FindByText("")
                If lstItem IsNot Nothing Then
                    srh_KYCDocuments.SelectListBox.Items.Remove(lstItem)
                End If
            Else
                objCommon.FillControl(srh_EmpalementDocuments.SelectListBox, sqlConn, "ID_FILL_DocumentTypes_Customer_Emp", "DocumentName", "DocumentId", ViewState("Id"), "CustomerId")
                lstItem = srh_EmpalementDocuments.SelectListBox.Items.FindByText("")
                If lstItem IsNot Nothing Then
                    srh_EmpalementDocuments.SelectListBox.Items.Remove(lstItem)
                End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "fillListBoxOnUpdate", "Error in fillListBoxOnUpdate", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub fillListBox()
        Try
            Dim dt As DataTable
            Dim dt1 As DataTable
            If Hid_DecType.Value = "B" Then
                dt = objCommon.FillControl(srh_KYCDocuments.SelectListBox, sqlConn, "ID_FILL_DocumentTypes", "DocumentTypeName", "DocumentTypeId", cbo_CustomerType.SelectedValue, "CustomerTypeId")
                lstItem = srh_KYCDocuments.SelectListBox.Items.FindByText("")
                If lstItem IsNot Nothing Then
                    srh_KYCDocuments.SelectListBox.Items.Remove(lstItem)
                End If

                dt1 = objCommon.FillControl(srh_EmpalementDocuments.SelectListBox, sqlConn, "ID_FILL_EmpDocumentTypes", "DocumentTypeName", "DocumentTypeId", cbo_CustomerType.SelectedValue, "CustomerTypeId")
                lstItem = srh_EmpalementDocuments.SelectListBox.Items.FindByText("")
                If lstItem IsNot Nothing Then
                    srh_EmpalementDocuments.SelectListBox.Items.Remove(lstItem)
                End If
            ElseIf Hid_DecType.Value = "K" Then
                dt = objCommon.FillControl(srh_KYCDocuments.SelectListBox, sqlConn, "ID_FILL_DocumentTypes", "DocumentTypeName", "DocumentTypeId", cbo_CustomerType.SelectedValue, "CustomerTypeId")
                lstItem = srh_KYCDocuments.SelectListBox.Items.FindByText("")
                If lstItem IsNot Nothing Then
                    srh_KYCDocuments.SelectListBox.Items.Remove(lstItem)
                End If
            Else
                dt1 = objCommon.FillControl(srh_EmpalementDocuments.SelectListBox, sqlConn, "ID_FILL_EmpDocumentTypes", "DocumentTypeName", "DocumentTypeId", cbo_CustomerType.SelectedValue, "CustomerTypeId")
                lstItem = srh_EmpalementDocuments.SelectListBox.Items.FindByText("")
                If lstItem IsNot Nothing Then
                    srh_EmpalementDocuments.SelectListBox.Items.Remove(lstItem)
                End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "fillListBox", "Error in fillListBox", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub


    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Page_Unload", "Error in Page_Unload", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub dg_Scheme_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_Scheme.ItemCommand
        SetSchemeGrid(dg_Scheme, e, "SchemeTable")
    End Sub
    Private Sub SetSchemeGrid(ByVal objGrid As DataGrid, ByVal e As DataGridCommandEventArgs, ByVal strSessionName As String)
        Try
            Dim dtGrid As DataTable
            dtGrid = TryCast(Session("SchemeTable"), DataTable).Copy
            If e.CommandName.ToUpper() = "ADD" Or e.CommandName.ToUpper() = "UPDATE" Then
                Dim dr As DataRow
                If e.CommandName.ToUpper() = "ADD" Then
                    dr = dtGrid.NewRow
                Else
                    dr = dtGrid.Rows(e.Item.ItemIndex)
                End If
                dr("SchemeName") = Trim(TryCast(e.Item.FindControl("txt_Scheme"), TextBox).Text)

                If e.CommandName.ToUpper() = "ADD" Then
                    dtGrid.Rows.Add(dr)
                End If
                objGrid.EditItemIndex = -1
            ElseIf e.CommandName.ToUpper() = "DELETE" Then
                dtGrid.Rows.RemoveAt(e.Item.ItemIndex)
            ElseIf e.CommandName.ToUpper() = "EDIT" Then
                objGrid.EditItemIndex = e.Item.ItemIndex
            ElseIf e.CommandName.ToUpper() = "CANCEL" Then
                objGrid.EditItemIndex = -1
            End If
            Session("SchemeTable") = dtGrid
            objGrid.DataSource = dtGrid
            objGrid.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetSchemeGrid", "Error in SetSchemeGrid", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub dg_Scheme_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_Scheme.ItemDataBound
        Try
            'objCommon.SetGridRows(e)
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.EditItem Then
                'Dim dr As DataRow
                'Dim ImgBtnDelete As LinkButton
                'ImgBtnDelete = CType(e.Item.FindControl("deletebtn"), LinkButton)
                'dr = TryCast(e.Item.DataItem, DataRowView).Row
                'If dr("CustomerSchemeId").ToString <> "" Then
                '    ImgBtnDelete.Visible = False
                'End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Scheme_ItemDataBound", "Error in dg_Scheme_ItemDataBound", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub Add_Dealer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Add_Dealer.Click
        Try
            InsertUpdateDealer(ViewState("DealerEditFlag"))
            'FillDealerDetails()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub FillDealerDetails()
        Try
            Dim dt As DataTable
            If TypeOf Session("CustomerDealerTable") Is DataTable Then
                dt = Session("CustomerDealerTable")
            Else
                dt = objCommon.FillDetailsGrid(dg_Dealer, "ID_FILL_CustomerDealerDetails", "CustomerId", Val((ViewState("Id")) & ""))
            End If
            Session("CustomerDealerTable") = dt

            dg_Dealer.DataSource = dt
            dg_Dealer.DataBind()
        Catch ex As Exception

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Sub FillBrokerDetails()
        Try
            Dim dt As DataTable
            If TypeOf Session("CustomerBrokerTable") Is DataTable Then
                dt = Session("CustomerBrokerTable")
            Else
                dt = objCommon.FillDetailsGrid(dg_Broker, "ID_FILL_CustomerBrokerDetails", "CustomerId", Val((ViewState("Id")) & ""))
            End If
            Session("CustomerBrokerTable") = dt

            dg_Broker.DataSource = dt
            dg_Broker.DataBind()
        Catch ex As Exception

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub dg_Dealer_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_Dealer.ItemCommand
        Try
            Dim imgBtn As ImageButton
            Dim dgItem As DataGridItem
            Dim dr As DataRow
            Dim dt As DataTable

            dt = CType(Session("CustomerDealerTable"), DataTable)
            If e.CommandName = "Edit" Then
                imgBtn = TryCast(e.CommandSource, ImageButton)
                dgItem = e.Item
                e.Item.BackColor = Drawing.Color.FromName("#CCE3FF")
                ViewState("RowIndex") = dgItem.DataSetIndex
                ViewState("DealerEditFlag") = True
                dr = dt.Rows(dgItem.DataSetIndex)
                cbo_DealerName.SelectedValue = dr.Item("CustomerDealerId")
                If dr.Item("StartDate").ToString <> "" Then
                    txt_StartDate.Text = Trim(dr.Item("StartDate") & "")
                End If

                If dr.Item("StartDate").ToString <> "" Then
                    txt_EndDate.Text = Trim(dr.Item("EndDate") & "")
                End If
            Else
                dt.Rows.RemoveAt(e.Item.ItemIndex)
            End If
            Session("CustomerDealerTable") = dt
            FillDealerDetails()
        Catch ex As Exception

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Function SaveDealers(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable


            dt = Session("CustomerDealerTable")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = ""
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("NameOfUser").ToString <> "" Then
                    sqlComm.Parameters.Clear()

                    If dg_Dealer.Items(I).FindControl("imgBtn_Delete").Visible = True Then
                        sqlComm.CommandText = "ID_INSERT_CustomerDealerDetails"
                        objCommon.SetCommandParameters(sqlComm, "@CustDealerDetailId", SqlDbType.Int, 4, "O")
                    Else
                        sqlComm.CommandText = "ID_UPDATE_CustomerDealerDetails"
                        objCommon.SetCommandParameters(sqlComm, "@CustDealerDetailId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("CustDealerDetailId"))
                    End If
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , Val(ViewState("Id")))
                    If dt.Rows(I).Item("StartDate") & "" <> "" Then
                        objCommon.SetCommandParameters(sqlComm, "@StartDate", SqlDbType.SmallDateTime, 9, "I", , , objCommon.DateFormat(dt.Rows(I).Item("StartDate")))
                    End If
                    If dt.Rows(I).Item("EndDate") & "" <> "" Then
                        objCommon.SetCommandParameters(sqlComm, "@EndDate", SqlDbType.SmallDateTime, 9, "I", , , objCommon.DateFormat(dt.Rows(I).Item("EndDate")))
                    End If
                    objCommon.SetCommandParameters(sqlComm, "@CustomerDealerId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("CustomerDealerId"))
                    objCommon.SetCommandParameters(sqlComm, "@EnteredBy", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
                    objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                End If


                sqlComm.ExecuteNonQuery()
                Hid_DealeretailId.Value += Val(sqlComm.Parameters.Item("@CustDealerDetailId").Value & "") & "!"
                Hid_DealeretailId.Value += "!"

            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Private Function SaveBrokers(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable


            dt = Session("CustomerBrokerTable")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = ""
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("Brokername").ToString <> "" Then
                    sqlComm.Parameters.Clear()

                    If dg_Broker.Items(I).FindControl("imgBtn_Delete").Visible = True Then
                        sqlComm.CommandText = "ID_INSERT_CustomerBrokerDetails"
                        objCommon.SetCommandParameters(sqlComm, "@CustBrokerDetailId", SqlDbType.Int, 4, "O")
                    Else
                        sqlComm.CommandText = "ID_UPDATE_CustomerBrokerDetails"
                        objCommon.SetCommandParameters(sqlComm, "@CustBrokerDetailId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("CustBrokerDetailId"))
                    End If
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , Val(ViewState("Id")))
                    If dt.Rows(I).Item("StartDate") & "" <> "" Then
                        objCommon.SetCommandParameters(sqlComm, "@StartDate", SqlDbType.SmallDateTime, 9, "I", , , objCommon.DateFormat(dt.Rows(I).Item("StartDate")))
                    End If
                    If dt.Rows(I).Item("EndDate") & "" <> "" Then
                        objCommon.SetCommandParameters(sqlComm, "@EndDate", SqlDbType.SmallDateTime, 9, "I", , , objCommon.DateFormat(dt.Rows(I).Item("EndDate")))
                    End If
                    objCommon.SetCommandParameters(sqlComm, "@CustomerBrokerId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("CustomerBrokerId"))
                    objCommon.SetCommandParameters(sqlComm, "@EnteredBy", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
                    objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                End If


                sqlComm.ExecuteNonQuery()
                Hid_BrokerdetailId.Value += Val(sqlComm.Parameters.Item("@CustBrokerDetailId").Value & "") & "!"
                Hid_BrokerdetailId.Value += "!"

            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Private Sub dg_Broker_ItemCommand(source As Object, e As DataGridCommandEventArgs) Handles dg_Broker.ItemCommand
        Try
            Dim imgBtn As ImageButton
            Dim dgItem As DataGridItem
            Dim dr As DataRow
            Dim dt As DataTable

            dt = CType(Session("CustomerBrokerTable"), DataTable)
            If e.CommandName = "Edit" Then
                imgBtn = TryCast(e.CommandSource, ImageButton)
                dgItem = e.Item
                e.Item.BackColor = Drawing.Color.FromName("#CCE3FF")
                ViewState("RowIndex") = dgItem.DataSetIndex
                ViewState("DealerEditFlag") = True
                dr = dt.Rows(dgItem.DataSetIndex)
                srh_Brokername.SelectedId = dr.Item("CustomerBrokerId")
                If dr.Item("StartDate").ToString <> "" Then
                    txt_BrokerStartDate.Text = Trim(dr.Item("StartDate") & "")
                End If

                If dr.Item("StartDate").ToString <> "" Then
                    txt_BrokerEndDate.Text = Trim(dr.Item("EndDate") & "")
                End If
            Else
                dt.Rows.RemoveAt(e.Item.ItemIndex)
            End If
            Session("CustomerBrokerTable") = dt
            FillBrokerDetails()
        Catch ex As Exception

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Sub btn_AddBroker_Click(sender As Object, e As EventArgs) Handles btn_AddBroker.Click
        Try
            InsertUpdateBroker(ViewState("BrokerEditFlag"))

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub dg_ErstWhile_ItemCommand(source As Object, e As DataGridCommandEventArgs) Handles dg_ErstWhile.ItemCommand
        Try
            SetErstWhileGrid(dg_ErstWhile, e, "CustomerErstWhile")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SetErstWhileGrid(ByVal objGrid As DataGrid, ByVal e As DataGridCommandEventArgs, ByVal strSessionName As String)
        Try
            Dim dtGrid As DataTable
            dtGrid = TryCast(Session("CustomerErstWhile"), DataTable).Copy
            If e.CommandName.ToUpper() = "ADD" Or e.CommandName.ToUpper() = "UPDATE" Then
                Dim dr As DataRow
                If e.CommandName.ToUpper() = "ADD" Then
                    dr = dtGrid.NewRow
                Else
                    dr = dtGrid.Rows(e.Item.ItemIndex)
                End If
                dr("CustomerErstWhileName") = Trim(TryCast(e.Item.FindControl("txt_CustomerErstWhile"), TextBox).Text)
                dr("ErstWhileDate") = Trim(TryCast(e.Item.FindControl("txt_CustomerErstWhileDate"), TextBox).Text)
                If e.CommandName.ToUpper() = "ADD" Then
                    dtGrid.Rows.Add(dr)
                End If
                objGrid.EditItemIndex = -1
            ElseIf e.CommandName.ToUpper() = "DELETE" Then
                dtGrid.Rows.RemoveAt(e.Item.ItemIndex)
            ElseIf e.CommandName.ToUpper() = "EDIT" Then
                objGrid.EditItemIndex = e.Item.ItemIndex
            ElseIf e.CommandName.ToUpper() = "CANCEL" Then
                objGrid.EditItemIndex = -1
            End If
            Session("CustomerErstWhile") = dtGrid
            objGrid.DataSource = dtGrid
            objGrid.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
