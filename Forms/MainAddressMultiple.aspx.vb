Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_MainAddressMultiple
    Inherits System.Web.UI.Page
    Dim sqlConn As SqlConnection
    Dim objCommon As New clsCommonFuns

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        Try
            Dim strValues() As String

            'srh_BusniessType.SelectCheckbox.Visible = False
            'srh_BusniessType.SelectCheckbox.Checked = False
            'srh_BusniessType.SelectLinkButton.Enabled = True

            If (IsPostBack = False) Then
                SetAttributes()
                FillBlankContactGrids()
                If (Request.QueryString("CustomerId") & "") <> "" Then
                    Hid_CustomerId.Value = Request.QueryString("CustomerId").ToString()
                End If
                If (Request.QueryString("custMAddrId") & "") <> "" Then
                    ViewState("custMAddrId") = Request.QueryString("custMAddrId").ToString()
                End If
                If (Request.QueryString("tempId") & "") <> "" Then
                    Session("TempId") = Request.QueryString("tempId").ToString()
                    Hid_TempId.Value = Request.QueryString("tempId").ToString()
                End If
                If (Request.QueryString("ClientCustId") & "") <> "" Then
                    Hid_ClientCustMulId.Value = Request.QueryString("ClientCustId").ToString()
                    Session("ClientCustMulId") = Request.QueryString("ClientCustId").ToString()
                End If

                If Request.QueryString("MercBanking") = "true" Then
                    row_LstBustype.Visible = False
                    Hid_PageName.Value = "MercBanking"
                End If

                If Trim(Request.QueryString("Values") & "") <> "" Then
                    strValues = Split(Trim(Request.QueryString("Values") & ""), "!")
                    txt_CustBranchname.Text = strValues(0)
                    txt_Address1.Text = strValues(1)
                    txt_Address2.Text = strValues(2)
                    txt_City.Text = strValues(3)
                    txt_Pincode.Text = strValues(4)
                    txt_State.Text = strValues(5)
                    txt_Country.Text = strValues(6)
                    txt_PhoneNo.Text = strValues(7)
                    txt_FaxNo.Text = strValues(8)
                    txt_Email.Text = strValues(9)
                    Hid_CustomerId.Value = Request.QueryString("CustomerId").ToString()
                    Dim strBusinessType() As String = Split(strValues(10), ",")
                    Dim strBusinessTypeId() As String = Split(strValues(11), ",")
                    For I As Int16 = 0 To strBusinessType.Length - 1
                        If strBusinessType(I) <> "" Then
                            srh_BusniessType.SelectListBox.Items.Add(New ListItem(strBusinessType(I), strBusinessTypeId(I)))
                        End If
                    Next
                    Hid_ClientCustAddressId.Value = strValues(12)
                    btn_Cancel.Visible = True
                    btn_Update.Visible = False
                    FillContactsGrid()
                End If
                btn_Update.Visible = False

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
        btn_Save.Attributes.Add("onclick", "return Validation();")
        'btn_Update.Attributes.Add("onclick", "return Validation();")
        txt_CustBranchname.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_Address1.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_Address2.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_Pincode.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_City.Attributes.Add("onblur", "ConvertUCase(this);")
        btn_AddContact.Attributes.Add("onclick", "return AddContact('" & Me.ClientID & "');")
    End Sub
    Private Sub FillBlankContactGrids()
        Try
            Dim dtContactGrid As New DataTable
            dtContactGrid.Columns.Add("ContactPerson", GetType(String))
            dtContactGrid.Columns.Add("Designation", GetType(String))
            dtContactGrid.Columns.Add("PhoneNo1", GetType(String))
            dtContactGrid.Columns.Add("MobileNo", GetType(String))
            dtContactGrid.Columns.Add("EmailId", GetType(String))
            dtContactGrid.Columns.Add("BusinessTypeNames", GetType(String))
            dtContactGrid.Columns.Add("BusniessTypeIds", GetType(String))
            dtContactGrid.Columns.Add("NameOfUsers", GetType(String))
            dtContactGrid.Columns.Add("UserIds", GetType(String))
            dtContactGrid.Columns.Add("PhoneNo2", GetType(String))
            dtContactGrid.Columns.Add("FaxNo1", GetType(String))
            dtContactGrid.Columns.Add("FaxNo2", GetType(String))
            dtContactGrid.Columns.Add("CustomerId", GetType(String))
            dtContactGrid.Columns.Add("ProfileType", GetType(String))
            dtContactGrid.Columns.Add("BusniessType", GetType(String))
            dtContactGrid.Columns.Add("SectionType", GetType(String))
            dtContactGrid.Columns.Add("TempId", GetType(String))
            dtContactGrid.Columns.Add("ClientCustAddressId", GetType(String))


            Session("MAddrcontactTable") = dtContactGrid
            dg_Contact.DataSource = dtContactGrid
            dg_Contact.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillContactsGrid()
        Try
            OpenConn()
            Dim dt As DataTable
            'If ViewState("custMAddrId") = 0 Then
            If Val(ViewState("TempId")) <> 0 Then
                dt = objCommon.FillDetailsGrid(dg_Contact, "ID_FILL_MulAddrCtcDet", "CustomerId", Hid_CustomerId.Value, "TempId", Session("TempId"))
            Else
                dt = objCommon.FillDetailsGrid(dg_Contact, "ID_FILL_MulAddrCtcDet", "CustomerId", Hid_CustomerId.Value, "ClientCustAddressId", Hid_ClientCustMulId.Value)
            End If
            Session("MAddrcontactTable") = dt
            dg_Contact.DataSource = dt
            dg_Contact.DataBind()
            ' dg_Contact.Columns(7).ItemStyle.CssClass = "hidden"
            'HideShowColumns(dg_Contact)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            Dim sqlTrans As SqlTransaction
            sqlTrans = sqlConn.BeginTransaction
            If DeleteClientContact(sqlTrans) = False Then Exit Sub
            If SaveContactNew(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()

            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "close", "RetValues();", True)
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "msg", "RetValues();", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            If Val(Hid_ClientCustMulId.Value) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "I", , , Hid_ClientCustMulId.Value)
            Else
                objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "I", , , DBNull.Value)
            End If
            objCommon.SetCommandParameters(sqlComm, "@TempId", SqlDbType.Int, 4, "I", , , Val(ViewState("TempId")))
            objCommon.SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@StrMessage", SqlDbType.VarChar, 100, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally

        End Try
    End Function

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        btn_Save_Click(sender, e)
    End Sub

    Protected Sub btn_AddContact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddContact.Click
        Try
            OpenConn()
            Dim strRetValues() As String
            Dim dt As DataTable
            Dim dr As DataRow

            Dim strRowIndex As String = Hid_RowIndex.Value
            If strRowIndex <> "" Then
                Dim intRowIndex As Integer = Convert.ToInt32(strRowIndex)
                dt = Session("MAddrcontactTable")
                strRetValues = Split(Hid_RetValues.Value, "!")
                dt.Rows(intRowIndex).Item("ContactPerson") = strRetValues(0)
                dt.Rows(intRowIndex).Item("Designation") = strRetValues(1)
                dt.Rows(intRowIndex).Item("PhoneNo1") = strRetValues(2)
                dt.Rows(intRowIndex).Item("MobileNo") = strRetValues(3)
                dt.Rows(intRowIndex).Item("EmailId") = (strRetValues(4))
                dt.Rows(intRowIndex).Item("PhoneNo2") = strRetValues(5)
                dt.Rows(intRowIndex).Item("FaxNo1") = strRetValues(6)
                dt.Rows(intRowIndex).Item("FaxNo2") = strRetValues(7)
                If Trim(strRetValues(8)) = False Then
                    dt.Rows(intRowIndex).Item("SectionType") = "F"
                Else
                    dt.Rows(intRowIndex).Item("SectionType") = "B"
                End If
                dt.Rows(intRowIndex).Item("CustomerId") = Hid_CustomerId.Value
            Else
                strRetValues = Split(Hid_RetValues.Value, "!")
                dt = Session("MAddrcontactTable")
                dr = dt.NewRow
                dr.Item("ContactPerson") = Trim(strRetValues(0) & "")
                dr.Item("Designation") = Trim(strRetValues(1))
                dr.Item("PhoneNo1") = Trim(strRetValues(2))
                dr.Item("MobileNo") = strRetValues(3)
                dr.Item("EmailId") = strRetValues(4)
                dr.Item("PhoneNo2") = Trim(strRetValues(5))
                dr.Item("FaxNo1") = Trim(strRetValues(6))
                dr.Item("FaxNo2") = Trim(strRetValues(7))

                If Trim(strRetValues(8)) = True Then
                    dr.Item("SectionType") = "F"
                Else
                    dr.Item("SectionType") = "B"
                End If
                dr.Item("CustomerId") = Request.QueryString("CustomerId").ToString()
                dr.Item("ClientCustAddressId") = Val(Hid_ClientCustMulId.Value)
                dr.Item("TempId") = Val(ViewState("TempId"))
                dt.Rows.Add(dr)
            End If
            Session("MAddrcontactTable") = dt
            dg_Contact.DataSource = dt
            dg_Contact.DataBind()
            FillContactDetails()
            Hid_RowIndex.Value = ""
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub FillContactDetails()
        Try
            OpenConn()
            Dim dt As DataTable

            If IsPostBack = True Then
                dt = Session("MAddrcontactTable")
            Else
                dt = objCommon.FillDetailsGrid(dg_Contact, "ID_FILL_ClientContactDetails", "CustomerId", Request.QueryString("CustomerId"))
            End If
            Session("MAddrcontactTable") = dt
            ClearOptions()
            dg_Contact.DataSource = dt
            dg_Contact.DataBind()
            dg_Contact.Columns(7).Visible = False
            'HideShowColumns(dg_Contact)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub ClearOptions()
        Hid_BusinessTypeId.Value = ""
        Hid_BusinessTypeNames.Value = ""
        Hid_UserIds.Value = ""
        Hid_NameOfUsers.Value = ""
        Hid_PhoneNo2.Value = ""
        Hid_FaxNo1.Value = ""
        Hid_FaxNo2.Value = ""
        Hid_SectionType.Value = ""


    End Sub
    Public Function SaveContactNew(ByVal sqlTrans As SqlTransaction) As Boolean

        Try
            Dim sqlComm As New SqlCommand
            Dim dt As New DataTable
            Dim dgItem As DataGridItem

            Dim strContactPer As String
            Dim strDesig As String
            Dim strPh1 As String
            Dim strPh2 As String
            Dim strFax1 As String
            Dim strFax2 As String
            Dim strMobile As String
            Dim strEmail As String
            Dim strSectionType As String
            Dim datTempId As Integer

            With sqlComm
                .Connection = sqlConn
                .Transaction = sqlTrans
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_INSERT_MultiAddrClientContact"
                dt = Session("MAddrcontactTable")
                For I As Int16 = 0 To dg_Contact.Items.Count - 1
                    dgItem = dg_Contact.Items(I)
                    strContactPer = Trim(CType(dgItem.FindControl("txt_ContactPerson"), TextBox).Text)
                    strDesig = Trim(CType(dgItem.FindControl("lbl_Designation"), Label).Text)
                    strPh1 = Trim(CType(dgItem.FindControl("lbl_PhoneNo1"), Label).Text)
                    strPh2 = Trim(CType(dgItem.FindControl("lbl_PhoneNo2"), Label).Text)
                    strFax1 = Trim(CType(dgItem.FindControl("lbl_FaxNo1"), Label).Text)
                    strFax2 = Trim(CType(dgItem.FindControl("lbl_FaxNo2"), Label).Text)
                    strMobile = Trim(CType(dgItem.FindControl("lbl_MobileNo"), Label).Text)
                    strEmail = Trim(CType(dgItem.FindControl("lbl_EmailId"), Label).Text)

                    'strCustomerId = Trim(CType(dgItem.FindControl("lbl_CustomerId"), Label).Text)
                    'strClientCustAddressId = Trim(CType(dgItem.FindControl("txt_ContactPerson"), Label).Text)
                    datTempId = Trim(CType(dgItem.FindControl("lbl_tempId"), Label).Text)
                    strSectionType = Trim(CType(dgItem.FindControl("lbl_SectionType"), Label).Text)
                    'If dt.Rows(I).Item("ContactPerson").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    'objCommon.SetCommandParameters(sqlComm, "@ContactPerson", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("ContactPerson"))
                    'objCommon.SetCommandParameters(sqlComm, "@Designation", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("Designation"))
                    'objCommon.SetCommandParameters(sqlComm, "@PhoneNo1", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("PhoneNo1"))
                    'objCommon.SetCommandParameters(sqlComm, "@PhoneNo2", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("PhoneNo2"))
                    'objCommon.SetCommandParameters(sqlComm, "@FaxNo1", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("FaxNo1"))
                    'objCommon.SetCommandParameters(sqlComm, "@FaxNo2", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("FaxNo2"))
                    'objCommon.SetCommandParameters(sqlComm, "@MobileNo", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("MobileNo"))
                    'objCommon.SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , dt.Rows(I).Item("EmailId"))
                    'If Val(Hid_CustomerId.Value) <> 0 Then
                    '    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , Val(Hid_CustomerId.Value))
                    'End If
                    'If Val(Hid_ClientCustMulId.Value) <> 0 Then
                    '    objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "I", , , Hid_ClientCustMulId.Value)
                    'Else
                    '    objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "I", , , DBNull.Value)
                    'End If
                    'objCommon.SetCommandParameters(sqlComm, "@TempId", SqlDbType.Int, 4, "I", , , Val(ViewState("TempId")))
                    'objCommon.SetCommandParameters(sqlComm, "@SectionType", SqlDbType.Char, 1, "I", , , dt.Rows(I).Item("SectionType"))
                    'objCommon.SetCommandParameters(sqlComm, "@MAContactDetailId", SqlDbType.Int, 4, "O")
                    'objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    'objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")

                    objCommon.SetCommandParameters(sqlComm, "@ContactPerson", SqlDbType.VarChar, 100, "I", , , strContactPer)
                    objCommon.SetCommandParameters(sqlComm, "@Designation", SqlDbType.VarChar, 100, "I", , , strDesig)
                    objCommon.SetCommandParameters(sqlComm, "@PhoneNo1", SqlDbType.VarChar, 100, "I", , , strPh1)
                    objCommon.SetCommandParameters(sqlComm, "@PhoneNo2", SqlDbType.VarChar, 100, "I", , , strPh2)
                    objCommon.SetCommandParameters(sqlComm, "@FaxNo1", SqlDbType.VarChar, 100, "I", , , strFax1)
                    objCommon.SetCommandParameters(sqlComm, "@FaxNo2", SqlDbType.VarChar, 100, "I", , , strFax2)
                    objCommon.SetCommandParameters(sqlComm, "@MobileNo", SqlDbType.VarChar, 100, "I", , , strMobile)
                    objCommon.SetCommandParameters(sqlComm, "@EmailId", SqlDbType.VarChar, 100, "I", , , strEmail)
                    If Val(Hid_CustomerId.Value) <> 0 Then
                        objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , Hid_CustomerId.Value)
                    End If
                    If Val(Hid_ClientCustMulId.Value) <> 0 Then
                        objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "I", , , Hid_ClientCustMulId.Value)
                    Else
                        objCommon.SetCommandParameters(sqlComm, "@ClientCustAddressId", SqlDbType.Int, 4, "I", , , DBNull.Value)
                    End If
                    objCommon.SetCommandParameters(sqlComm, "@TempId", SqlDbType.Int, 4, "I", , , datTempId)
                    objCommon.SetCommandParameters(sqlComm, "@SectionType", SqlDbType.Char, 1, "I", , , strSectionType)
                    objCommon.SetCommandParameters(sqlComm, "@MAContactDetailId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
                    sqlComm.ExecuteNonQuery()
                    'End If
                    Hid_MACustAddrCtcid.Value += Val(sqlComm.Parameters.Item("@MAContactDetailId").Value & "") & "!"
                Next
                Return True
            End With
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            'CloseConn()
        End Try
    End Function

    Protected Sub dg_Contact_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_Contact.ItemCommand
        Try
            Dim strRetValues() As String
            Dim dt As DataTable


            dt = CType(Session("MAddrcontactTable"), DataTable)
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
                If Trim(strRetValues(8)) = False Then
                    dt.Rows(e.Item.ItemIndex).Item("SectionType") = "F"
                Else
                    dt.Rows(e.Item.ItemIndex).Item("SectionType") = "B"
                End If
                'dt.Rows(e.Item.ItemIndex).Item("SectionType") = strRetValues(8)
                dt.Rows(e.Item.ItemIndex).Item("CustomerId") = Hid_CustomerId.Value
                'dt.Rows(e.Item.ItemIndex).Item("TempId") = ViewState("TempId")
                'dt.Rows(e.Item.ItemIndex).Item("ResearchDocId") = (strRetValues(13))
                'dt.Rows(e.Item.ItemIndex).Item("ResearchDocName") = Trim(strRetValues(14))



            Else
                dt.Rows.RemoveAt(e.Item.ItemIndex)
            End If
            Session("MAddrcontactTable") = dt
            FillContactDetails()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dg_Contact_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_Contact.ItemDataBound
        Try

            Dim img As New HtmlImage
            Dim lnkBtnEdit As ImageButton
            Dim lnkbtnDelete As ImageButton
            Dim intContactDetailId As Integer
            Dim dtGrid As DataTable
            Dim txtContactPerson As TextBox
            Dim txtBusinessTypeNames As TextBox
            Dim ImgEdit1 As HtmlInputButton

            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                'e.Item.ID = "itm" & e.Item.ItemIndex

                txtContactPerson = CType(e.Item.FindControl("txt_ContactPerson"), TextBox)
                'txtAddress1 = CType(e.Item.FindControl("txt_Address1"), TextBox)
                'txtAddress2 = CType(e.Item.FindControl("txt_Address2"), TextBox)
                txtBusinessTypeNames = CType(e.Item.FindControl("lbl_BusinessTypeNames"), TextBox)

                txtContactPerson.Attributes.Add("onKeyPress", "return OnlyScroll()")
                'txtAddress1.Attributes.Add("onKeyPress", "return OnlyScroll()")
                'txtAddress2.Attributes.Add("onKeyPress", "return OnlyScroll()")
                'txtBusinessTypeNames.Attributes.Add("onKeyPress", "return OnlyScroll()")


                lnkbtnDelete = CType(e.Item.FindControl("imgBtn_Delete"), ImageButton)
                lnkbtnDelete.Attributes.Add("onclick", "return Deletedetails()")
                lnkBtnEdit = CType(e.Item.FindControl("imgBtn_Edit"), ImageButton)
                ImgEdit1 = CType(e.Item.FindControl("imgBtn_Edit1"), HtmlInputButton)
                'lnkBtnEdit.Attributes.Add("onclick", "return UpdateContactDetails('" & e.Item.ItemIndex & "','" & Me.ClientID & "')")
                lnkBtnEdit.Attributes.Add("onclick", "return UpdateContactDetails('" & e.Item.ItemIndex & "','" & Me.ClientID & "',this,'" & "');return false;")
                ImgEdit1.Attributes.Add("onclick", "return UpdateContactDetails(this, '" & e.Item.ItemIndex & "');")
                dtGrid = CType(Session("MAddrcontactTable"), DataTable)
                'Hid_BusinessTypeId.Value += Trim(CType(e.Item.FindControl("lbl_BusniessTypeIds"), Label).Text) & "!"
                'Hid_BusinessTypeNames.Value += Trim(CType(e.Item.FindControl("lbl_BusinessTypeNames"), TextBox).Text) & "!"
                'Hid_UserIds.Value += Trim(CType(e.Item.FindControl("lbl_UserIds"), Label).Text) & "!"
                'Hid_NameOfUsers.Value += Trim(CType(e.Item.FindControl("lbl_NameOfUsers"), TextBox).Text) & "!"
                Hid_PhoneNo2.Value += Trim(CType(e.Item.FindControl("lbl_PhoneNo2"), Label).Text) & "!"
                Hid_FaxNo1.Value += Trim(CType(e.Item.FindControl("lbl_FaxNo1"), Label).Text) & "!"
                Hid_FaxNo2.Value += Trim(CType(e.Item.FindControl("lbl_FaxNo2"), Label).Text) & "!"
                Hid_SectionType.Value += Trim(CType(e.Item.FindControl("lbl_SectionType"), Label).Text) & "!"
                'Hid_MACustAddrCtcid.Value = Trim(CType(e.Item.FindControl("lbl_ContactDetailId"), Label).Text) & "!"

                'Hid_ResearchDocId.Value += Trim(CType(e.Item.FindControl("lbl_ResearchDocId"), Label).Text) & "!"
                'Hid_ResearchDocName.Value += Trim(CType(e.Item.FindControl("lbl_ResearchDocName"), TextBox).Text) & "!"
                ''Hid_ContactDetailId.Value = Val(CType(e.Item.FindControl("lbl_ContactDetailId"), Label).Text)

            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            sqlConn.Dispose()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
