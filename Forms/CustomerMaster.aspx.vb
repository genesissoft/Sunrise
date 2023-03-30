Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_CustomerMaster
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim dsmenu As DataSet
    Dim dsDPDetails As DataSet
    Dim newTable As DataTable
    Dim DpDetailsTable As DataTable
    Dim trmenu As DataRow
    Dim lstItem As ListItem
    Dim sqlConn As New SqlConnection

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

            srh_EmpalementDocuments.SelectCheckbox.Visible = False
            srh_EmpalementDocuments.SelectCheckbox.Checked = False
            srh_EmpalementDocuments.SelectLinkButton.Enabled = True

            srh_KYCDocuments.SelectCheckbox.Visible = False
            srh_KYCDocuments.SelectCheckbox.Checked = False
            srh_KYCDocuments.SelectLinkButton.Enabled = True

            ' srh_DocumentTypeName

            'srh_KYCDocuments.SelectCheckbox.Visible = False
            'srh_KYCDocuments.SelectCheckbox.Checked = False
            'srh_KYCDocuments.SelectLinkButton.Enabled = True

            srh_BusniessType.SelectCheckbox.Visible = False
            srh_BusniessType.SelectCheckbox.Checked = False
            srh_BusniessType.SelectLinkButton.Enabled = True
            If IsPostBack = False Then
                SetAttributes()
                SetControls()
                FillBlankGrids()
                If Val(Request.QueryString("Id") & "") <> 0 Then
                    ViewState("Id") = Val(Request.QueryString("Id") & "")
                    Hid_CustomerId.Value = ViewState("Id")
                    FillFields()
                    FillCustomerDetailsGrid()
                    BtnTF()
                    CatBtnTF()
                    btn_Save.Visible = False
                    btn_Update.Visible = True
                Else

                    btn_Save.Visible = True
                    btn_Update.Visible = False
                    row_CoOpBank.Visible = False
                    ' btn_Save.Visible = True
                    row_BankDetails.Visible = False
                    row_Insurance.Visible = False
                    row_MFDetails.Visible = False
                    row_OtherDetails.Visible = False
                    row_PFDetails.Visible = False
                    row_MerchantbankProfile.Visible = False
                    Page.ClientScript.RegisterStartupScript(Me.GetType, "s21", "Visiblefalse();", True)
                End If
            Else
                FillContactDetails()

            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "ShowCustodian", "ShowCustodian();", True)

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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

            'srh_HeadCustomer.Columns.Add("CustomerName")
            'srh_HeadCustomer.Columns.Add("CustomerId")
            'srh_Custodian.Columns.Add("CustodianName")
            'srh_Custodian.Columns.Add("CustodianId")
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub SetAttributes()
        txt_empalmentdate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_empalmentdate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_CustomerName.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_Address1.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_Address2.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_PinCode.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_City.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_PhoneNo.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_FaxNo.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_PANNo.Attributes.Add("onblur", "ConvertUCase(this);")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
        btn_AddDetails.Attributes.Add("onclick", "return ValidateDetails();")
        btn_Address.Attributes.Add("onclick", "return AddAddress();")
        btn_SaveEditPFDetails.Attributes.Add("onclick", "return ShowDetails('Cust','1000px','800px','225px');")
        btn_CoOpBank.Attributes.Add("onclick", "return ShowDetails('CustCoOpBankDetails','1000px','800px','225px');")
        btn_SaveEditMFDetails.Attributes.Add("onclick", "return ShowDetails('CustMutualFundDetails','1000px','800px','225px');")
        btn_SaveEditBankDetails.Attributes.Add("onclick", "return ShowDetails('CustBankDetails','1000px','800px','225px');")
        btn_SaveEditOtherDetails.Attributes.Add("onclick", "return ShowDetails('CustOtherDetails','1000px','800px','225px');")
        btn_SaveEditInsurance.Attributes.Add("onclick", "return ShowDetails('CustInsuranceDetails','1000px','800px','225px');")
        btn_MerchantbankProfile.Attributes.Add("onclick", "return ShowDetails('CustMerchatBankDetails','1000px','800px','50px');")
        Btn_PMSProfile.Attributes.Add("onclick", "return ShowDetails('PMSClientProfile','1000px','800px','50px');")

        'btn_signature.Attributes.Add("onclick", "return showPurchaseOrder();")

    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        ''If CBool(objCommon.CheckDuplicate(clsCommonFuns.sqlConn, "CustomerMaster", "CustomerName", txt_CustomerName.Text, "CustomerTypeId", cbo_CustomerType.SelectedValue, "CategoryId", cbo_Category.SelectedValue)) = True Then
        ''    Dim msg As String = "This Customer Name Already Exist"
        ''    Dim strHtml As String
        ''    strHtml = "alert('" + msg + "');"
        ''    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
        ''    Exit Sub
        ''End If
        'Dim dt As DataTable
        'If CBool(objCommon.ISNameexists("FA_CHECK_CUSTOMERNAME",sqlConn, txt_CustomerName, , Val((cbo_CustomerType.SelectedValue) & ""))) = False Then
        '    Dim msg As String = "This Customer Name Already Exist"
        '    Dim strHtml As String
        '    strHtml = "alert('" + msg + "');"
        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
        '    Exit Sub
        'End If
        Try
            OpenConn()
            If CBool(objCommon.CheckDuplicate(sqlConn, "CustomerMaster", "CustomerName", txt_CustomerName.Text)) = False Then
                Dim msg As String = "This Customer Name Already Exist"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            SetSaveUpdate("ID_INSERT_CustomerMaster")
            'ShowDetailForms()       
            BtnTF()
            CatBtnTF()
            btn_Save.Visible = False
            btn_Update.Visible = True
            cbo_Category.Enabled = False
            'cbo_CustomerType.Enabled = False
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

        'Response.Redirect("ClientProfileDetail.aspx?Id=" & ViewState("Id"), False)
    End Sub

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
            If objCommon.CheckDuplicate(sqlConn, "CustomerMaster", "CustomerName", Trim(txt_CustomerName.Text), "CustomerId", Val(ViewState("Id"))) = False Then
                Dim msg As String = "This Customer Name Already Exist"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
                Exit Sub
            End If
            If SetSaveUpdate("ID_UPDATE_CustomerMaster") = False Then Exit Sub
            Response.Redirect("ClientProfileDetail.aspx?Id=" & ViewState("Id"), False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            Response.Redirect("ClientProfileDetail.aspx", False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            'If SaveContacts(sqlTrans) = False Then Exit Function
            If SaveSGLTrans(sqlTrans) = False Then Exit Function
            If SaveDPDetails(sqlTrans) = False Then Exit Function
            If SaveSignaturyDetails(sqlTrans) = False Then Exit Function


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

            'If SaveCustDoc(sqlTrans) = False Then Exit Function
            'If SaveCustDoc(sqlTrans) = False Then Exit Function


            If SaveClientBusinessType(sqlTrans) = False Then Exit Function
            If SaveCustomerBusinessType(sqlTrans) = False Then Exit Function
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

            sqlTrans.Commit()

            Return True
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
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
                    objCommon.SetCommandParameters(sqlComm, "@DecType", SqlDbType.Char, 1, "I", , , Val(Hid_DecType.Value))
                    objCommon.SetCommandParameters(sqlComm, "@CustDocId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
                    objCommon.SetCommandParameters(sqlComm, "@DecType", SqlDbType.Char, 1, "I", , , Val(Hid_DecType.Value))
                    objCommon.SetCommandParameters(sqlComm, "@CustDocId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.SmallInt, 4, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.SmallInt, 4, "I", , , ViewState("Id"))
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
            objCommon.SetCommandParameters(sqlComm, "@State", SqlDbType.VarChar, 100, "I", , , Trim(txt_State.Text))
            objCommon.SetCommandParameters(sqlComm, "@CustomerPhone", SqlDbType.VarChar, 100, "I", , , Trim(txt_PhoneNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@CustomerFax", SqlDbType.VarChar, 100, "I", , , Trim(txt_FaxNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@PANNumber", SqlDbType.VarChar, 100, "I", , , Trim(txt_PANNo.Text))
            objCommon.SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , Trim(txt_EmailId.Text))
            objCommon.SetCommandParameters(sqlComm, "@AccessLevel", SqlDbType.Char, 1, "I", , , cbo_Accessible.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@HeadSelect", SqlDbType.Char, 1, "I", , , rdo_Headselect.SelectedValue)
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
            objCommon.SetCommandParameters(sqlComm, "@EmpalmentFrequency", SqlDbType.Char, 1, "I", , , Trim(cbo_FrequencyEmpalment.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@CustomerId").Value
            Hid_CustomerId.Value = ViewState("Id")

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillBlankSGLGrids()
        Try
            Dim dtSGLGrid As New DataTable
            dtSGLGrid.Columns.Add("SGLTransWith", GetType(String))
            dtSGLGrid.Columns.Add("CustomerSGLId", GetType(String))


            Session("SGLTable") = dtSGLGrid
            dg_SGL.DataSource = dtSGLGrid
            dg_SGL.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillBlankDPGrids()
        Try
            Dim dtDPGrid As New DataTable
            dtDPGrid.Columns.Add("DpName", GetType(String))
            dtDPGrid.Columns.Add("DpId", GetType(String))
            dtDPGrid.Columns.Add("ClientId", GetType(String))
            dtDPGrid.Columns.Add("CustomerDPId", GetType(String))
            Session("DPTable") = dtDPGrid
            dg_DP.DataSource = dtDPGrid
            dg_DP.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            'dtBAnkGrid.Columns.Add("CustBankId", GetType(String))
            Session("BankTable") = dtBAnkGrid
            dg_Bank.DataSource = dtBAnkGrid
            dg_Bank.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            dtContactGrid.Columns.Add("ContactDetailId", GetType(String))
            Session("ContactTable") = dtContactGrid
            dg_Contact.DataSource = dtContactGrid
            dg_Contact.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
                dt.Rows(e.Item.ItemIndex).Item("BusniessTypeIds") = (strRetValues(5))
                dt.Rows(e.Item.ItemIndex).Item("BusinessTypeNames") = Trim(strRetValues(6))
                dt.Rows(e.Item.ItemIndex).Item("UserIds") = (strRetValues(7))
                dt.Rows(e.Item.ItemIndex).Item("NameOfUsers") = Trim(strRetValues(8))
                dt.Rows(e.Item.ItemIndex).Item("PhoneNo2") = strRetValues(9)
                dt.Rows(e.Item.ItemIndex).Item("FaxNo1") = strRetValues(10)
                dt.Rows(e.Item.ItemIndex).Item("FaxNo2") = strRetValues(11)

            Else
                dt.Rows.RemoveAt(e.Item.ItemIndex)
            End If
            Session("contactTable") = dt
            FillContactDetails()

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    'Private Sub SetContactGrid(ByVal objGrid As DataGrid, ByVal e As DataGridCommandEventArgs, ByVal strSessionName As String)
    '    Try
    '        Dim dtGrid As DataTable
    '        dtGrid = TryCast(Session("ContactTable"), DataTable).Copy
    '        If e.CommandName.ToUpper() = "ADD" Or e.CommandName.ToUpper() = "UPDATE" Then
    '            Dim dr As DataRow
    '            If e.CommandName.ToUpper() = "ADD" Then
    '                dr = dtGrid.NewRow
    '            Else
    '                dr = dtGrid.Rows(e.Item.ItemIndex)
    '            End If
    '            dr("ContactPerson") = Trim(TryCast(e.Item.FindControl("txt_ContactPerson"), TextBox).Text)
    '            dr("Designation") = Trim(TryCast(e.Item.FindControl("txt_Designation"), TextBox).Text)
    '            dr("PhoneNo") = Trim(TryCast(e.Item.FindControl("txt_PhoneNo"), TextBox).Text)
    '            dr("MobileNo") = Trim(TryCast(e.Item.FindControl("txt_MobileNo"), TextBox).Text)
    '            dr("EmailId") = Trim(TryCast(e.Item.FindControl("txt_EmailId"), TextBox).Text)
    '            dr("Dept") = Trim(TryCast(e.Item.FindControl("txt_Dept"), TextBox).Text)
    '            If e.CommandName.ToUpper() = "ADD" Then
    '                dtGrid.Rows.Add(dr)
    '            End If
    '            objGrid.EditItemIndex = -1
    '        ElseIf e.CommandName.ToUpper() = "DELETE" Then
    '            dtGrid.Rows.RemoveAt(e.Item.ItemIndex)
    '        ElseIf e.CommandName.ToUpper() = "EDIT" Then
    '            objGrid.EditItemIndex = e.Item.ItemIndex
    '        ElseIf e.CommandName.ToUpper() = "CANCEL" Then
    '            objGrid.EditItemIndex = -1
    '        End If
    '        Session("ContactTable") = dtGrid
    '        objGrid.DataSource = dtGrid
    '        objGrid.DataBind()
    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Sub

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
                    If dg_Bank.Items(I).FindControl("deletebtn").Visible = True Then
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
                    'objCommon.SetCommandParameters(sqlComm, "@CustBankId", SqlDbType.Int, 4, "O")
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
                    If dg_SGL.Items(I).FindControl("deletebtn").Visible = True Then
                        sqlComm.CommandText = "ID_INSERT_CustomerSGLDetail"
                        objCommon.SetCommandParameters(sqlComm, "@CustSGLId", SqlDbType.Int, 4, "O")
                    Else
                        sqlComm.CommandText = "ID_UPDATE_CustomerSGLDetail"

                        objCommon.SetCommandParameters(sqlComm, "@CustSGLId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("CustSGLId"))
                    End If

                    objCommon.SetCommandParameters(sqlComm, "@SGLTransWith", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("SGLTransWith"))
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                    'objCommon.SetCommandParameters(sqlComm, "@CustSGLId ", SqlDbType.Int, 4, "O")
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
                    If dg_DP.Items(I).FindControl("deletebtn").Visible = True Then
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function


    Private Function DeleteCustomerDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_ClientCustomerDetailTable"
            'sqlComm.CommandText = "ID_DELETE_CustomerDetailTable"
            sqlComm.Connection = sqlConn
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , Val(ViewState("Id")))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Sub FillFields()
        'CHANGE 
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_ClientCustomerMaster", ViewState("Id"), "CustomerId")
            If dt.Rows.Count > 0 Then
                cbo_CustomerType.SelectedValue = Val(dt.Rows(0).Item("CustomerTypeId") & "")

                objCommon.FillControl(cbo_Category, sqlConn, "ID_FILL_CategoryMaster", "CategoryName", "CategoryId", cbo_CustomerType.SelectedValue, "CustomerTypeId")
                cbo_CustomerGroup.SelectedValue = Val(dt.Rows(0).Item("GroupId") & "")
                cbo_Category.SelectedValue = Val(dt.Rows(0).Item("CategoryId") & "")
                cbo_Category.Enabled = False
                cbo_CustomerType.Enabled = False
                txt_CustomerName.Text = Trim(dt.Rows(0).Item("CustomerName") & "")
                txt_CustPrefix.Text = Trim(dt.Rows(0).Item("CustPrefix") & "")
                txt_Address1.Text = Trim(dt.Rows(0).Item("CustomerAddress1") & "")
                txt_Address2.Text = Trim(dt.Rows(0).Item("CustomerAddress2") & "")
                txt_PinCode.Text = Trim(dt.Rows(0).Item("CustomerPinCode") & "")
                txt_City.Text = Trim(dt.Rows(0).Item("CustomerCity") & "")
                txt_State.Text = Trim(dt.Rows(0).Item("State") & "")
                txt_Country.Text = Trim(dt.Rows(0).Item("Country") & "")
                txt_PhoneNo.Text = Trim(dt.Rows(0).Item("CustomerPhone") & "")
                txt_FaxNo.Text = Trim(dt.Rows(0).Item("CustomerFax") & "")
                txt_PANNo.Text = Trim(dt.Rows(0).Item("PANNumber") & "")
                txt_EmailId.Text = Trim(dt.Rows(0).Item("EmailId") & "")
                txt_empalmentdate.Text = Trim(dt.Rows(0).Item("EmpalmentDate") & "")
                cbo_FrequencyEmpalment.SelectedValue = Trim(dt.Rows(0).Item("EmpalmentFrequency") & "")
                rdo_Headselect.SelectedValue = Trim(dt.Rows(0).Item("HeadSelect") & "")
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
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub



    'Private Function SaveCustDoc(ByVal sqlTrans As SqlTransaction) As Boolean
    '    Try
    '        Dim sqlComm As New SqlCommand
    '        sqlComm.Transaction = sqlTrans
    '        sqlComm.CommandType = CommandType.StoredProcedure
    '        sqlComm.CommandText = "ID_INSERT_CustDoc"
    '        sqlComm.Connection = clsCommonFuns.sqlConn
    '        For I As Int16 = 0 To srh_KYCDocuments.SelectListBox.Items.Count - 1
    '            If Val(srh_KYCDocuments.SelectListBox.Items(I).Value) <> 0 Then
    '                sqlComm.Parameters.Clear()
    '                objCommon.SetCommandParameters(sqlComm, "@DocumentTypeId", SqlDbType.Int, 4, "I", , , srh_KYCDocuments.SelectListBox.Items(I).Value)
    '                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
    '                objCommon.SetCommandParameters(sqlComm, "@CustDocId", SqlDbType.Int, 4, "O")
    '                objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
    '                objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
    '                sqlComm.ExecuteNonQuery()
    '            End If
    '        Next
    '        Return True
    '    Catch ex As Exception
    '        sqlTrans.Rollback()
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Function
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    'Protected Sub cbo_CustomerType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CustomerType.SelectedIndexChanged


    'End Sub
    Private Function BtnTF()
        Try
            'Dim a As Char
            btn_Cancel.Visible = True
            row_CoOpBank.Visible = False
            ' btn_Save.Visible = True
            row_BankDetails.Visible = False
            row_Insurance.Visible = False
            row_MFDetails.Visible = False
            row_OtherDetails.Visible = False
            row_PFDetails.Visible = False
            row_MerchantbankProfile.Visible = True
            'btn_Update.Visible = True
            If (cbo_CustomerType.SelectedItem.Text = "BANK") Then
                row_BankDetails.Visible = True
            ElseIf (cbo_CustomerType.SelectedItem.Text = "INSURANCE") Then
                row_Insurance.Visible = True
            ElseIf (cbo_CustomerType.SelectedItem.Text = "MUTUAL FUND") Then
                row_MFDetails.Visible = True
            ElseIf (cbo_CustomerType.SelectedItem.Text = "EXEMPT TRUST FUNDS") Then
                row_PFDetails.Visible = True
            Else
                row_OtherDetails.Visible = True
            End If
        Catch ex As Exception

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function CatBtnTF()
        Try
            If (cbo_CustomerType.SelectedItem.Text = "BANK") Then
                row_BankDetails.Visible = True
                If (cbo_Category.SelectedItem.Text = "CO OP BANKS") Then
                    row_CoOpBank.Visible = True
                    row_BankDetails.Visible = "false"
                End If
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub cbo_Category_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Category.SelectedIndexChanged
        If (Hid_CustomerId.Value <> "") Then

            CatBtnTF()


        End If

    End Sub

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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_AddDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddDetails.Click
        Try
            Dim strRetValues() As String
            Dim dt As DataTable
            Dim dr As DataRow


            strRetValues = Split(Hid_RetValues.Value, "!")
            dt = Session("contactTable")
            dr = dt.NewRow
            dr.Item("ContactPerson") = Trim(strRetValues(0) & "")
            dr.Item("Designation") = Trim(strRetValues(1))
            dr.Item("PhoneNo1") = Trim(strRetValues(2))
            dr.Item("MobileNo") = strRetValues(3)
            dr.Item("EmailId") = strRetValues(4)
            If strRetValues(6) <> "" Then
                dr.Item("BusinessTypeNames") = IIf(Right(strRetValues(6), 1) = ",", Left(strRetValues(6), strRetValues(6).Length - 1), strRetValues(6))
            End If

            If strRetValues(5) <> "" Then
                dr.Item("BusniessTypeIds") = IIf(Right(strRetValues(5), 1) = ",", Left(strRetValues(5), strRetValues(5).Length - 1), strRetValues(5))
            End If

            If strRetValues(7) <> "" Then
                dr.Item("UserIds") = IIf(Right(strRetValues(7), 1) = ",", Left(strRetValues(7), strRetValues(7).Length - 1), strRetValues(7))
            End If

            If strRetValues(8) <> "" Then
                dr.Item("NameOfUsers") = IIf(Right(strRetValues(8), 1) = ",", Left(strRetValues(8), strRetValues(8).Length - 1), strRetValues(8))
            End If
            dr.Item("PhoneNo2") = Trim(strRetValues(9))
            dr.Item("FaxNo1") = Trim(strRetValues(10))
            dr.Item("FaxNo2") = Trim(strRetValues(11))
            dr.Item("ContactDetailId") = 0
            dt.Rows.Add(dr)
            Session("contactTable") = dt
            FillContactDetails()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dg_Contact_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_Contact.ItemDataBound
        Try
            Dim img As New HtmlImage
            Dim lnkBtnEdit As LinkButton
            Dim lnkbtnDelete As LinkButton
            Dim intContactDetailId As Integer
            Dim dtGrid As DataTable

            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                e.Item.ID = "itm" & e.Item.ItemIndex

                lnkbtnDelete = CType(e.Item.FindControl("imgBtn_Del"), LinkButton)
                lnkbtnDelete.Attributes.Add("onclick", "return Deletedetails()")
                lnkBtnEdit = CType(e.Item.FindControl("imgBtn_Edit"), LinkButton)
                lnkBtnEdit.Attributes.Add("onclick", "return UpdateContactDetails('" & e.Item.ItemIndex & "')")
                dtGrid = CType(Session("contactTable"), DataTable)
                Hid_BusinessTypeId.Value += Trim(CType(e.Item.FindControl("lbl_BusniessTypeIds"), Label).Text) & "!"
                Hid_BusinessTypeNames.Value += Trim(CType(e.Item.FindControl("lbl_BusinessTypeNames"), TextBox).Text) & "!"
                Hid_UserIds.Value += Trim(CType(e.Item.FindControl("lbl_UserIds"), Label).Text) & "!"
                Hid_NameOfUsers.Value += Trim(CType(e.Item.FindControl("lbl_NameOfUsers"), TextBox).Text) & "!"
                Hid_PhoneNo2.Value += Trim(CType(e.Item.FindControl("lbl_PhoneNo2"), Label).Text) & "!"
                Hid_FaxNo1.Value += Trim(CType(e.Item.FindControl("lbl_FaxNo1"), Label).Text) & "!"
                Hid_FaxNo2.Value += Trim(CType(e.Item.FindControl("lbl_FaxNo2"), Label).Text) & "!"
                intContactDetailId = Val(CType(e.Item.FindControl("lbl_ContactDetailId"), Label).Text)
                'Hid_ContactDetailId.Value = Val(CType(e.Item.FindControl("lbl_ContactDetailId"), Label).Text)
                Hid_ContactDetailIds.Value += Val(CType(e.Item.FindControl("lbl_ContactDetailId"), Label).Text) & "!"
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function SaveAddresses(ByVal sqlTrans As SqlTransaction) As Boolean
        Try

            Dim sqlComm As New SqlCommand

            Dim dgItem As DataGridItem

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
                    objCommon.SetCommandParameters(sqlComm, "@ProfileType", SqlDbType.Char, 2, "I", , , "CM")
                    objCommon.SetCommandParameters(sqlComm, "@BusniessType", SqlDbType.Char, 2, "I", , , "NO")
                    'objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 200, "O")
                    sqlComm.ExecuteNonQuery()

                    If objCommon.SaveClientAddBusniessDetails(sqlTrans, Trim(dt.Rows(i).Item("BusniessTypeIds") & ""), Val(sqlComm.Parameters("@ClientCustAddressId").Value), strCustomerId) = False Then Return False
                    'End If
                Next
            End If
            Return True

        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType, "s1", "SelectDocType();", True)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub cbo_CustomerType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CustomerType.SelectedIndexChanged
        Try
            OpenConn()
            objCommon.FillControl(cbo_Category, sqlConn, "ID_FILL_CategoryMaster", "CategoryName", "CategoryId", cbo_CustomerType.SelectedValue, "CustomerTypeId")
            If (Hid_CustomerId.Value <> "") Then
                BtnTF()
            End If
            FillDocType()
            'FillListBox()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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

        End Try
    End Sub


    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
        Catch ex As Exception

        End Try
    End Sub
End Class
