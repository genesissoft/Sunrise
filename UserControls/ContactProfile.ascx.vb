Imports System.Data
Imports System.Reflection
Imports System.Collections.Generic
Imports System.ComponentModel
Imports GlobalFuns
Imports System.Data.SqlClient
Imports log4net
Partial Class UserControls_ContactProfile
    Inherits System.Web.UI.UserControl
    Dim PgName As String = "$ContactProfile$"
    Dim intProfileType As Int16
    Dim intBusinessType As Int16
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As New SqlConnection
    Dim objUtil As New Util

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
    Enum enumProfileType
        Others = 1
        MutualFund = 2
        Insurance = 3
        Bank = 4
        PF = 5
        CoopBank = 6
        CustomerMaster = 7
    End Enum
    Enum enumBusinessType
        Normal = 1
        MerchantBanking = 2
        PMS = 3
    End Enum
    <Category("Data"), Description("Get and Set value of ProfileType"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False), DefaultValue(enumProfileType.Others)> _
    Public Property ProfileType() As enumProfileType
        Get
            If Val(ViewState("ProfileType")) = 0 Then
                ViewState("ProfileType") = 1
            End If
            Return ViewState("ProfileType")
        End Get
        Set(ByVal value As enumProfileType)
            ViewState("ProfileType") = value
        End Set
    End Property

    <Category("Data"), Description("Get and Set value of BusinessType"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False), DefaultValue(enumBusinessType.Normal)> _
    Public Property BusinessType() As enumBusinessType
        Get
            If Val(ViewState("BusinessType")) = 0 Then
                ViewState("BusinessType") = 1
            End If
            Return ViewState("BusinessType")
        End Get
        Set(ByVal value As enumBusinessType)
            ViewState("BusinessType") = value
        End Set
    End Property

    Public Property CustomerId() As Integer
        Get
            Return ViewState("CustomerId")
        End Get
        Set(ByVal value As Integer)
            ViewState("CustomerId") = value
        End Set
    End Property
    Public Property merchantbanking() As Boolean
        Get
            Return ViewState("merchantbanking")
        End Get
        Set(ByVal value As Boolean)
            ViewState("merchantbanking") = value

        End Set
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property dgcontact() As DataGrid
        Get
            Return dg_Contact
        End Get
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property dgAddress() As DataGrid
        Get
            Return dg_Address
        End Get
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property dgSource() As DataGrid
        Get
            Return dg_Source
        End Get
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'objCommon.OpenConn()
            If IsPostBack = False Then
                SetAttributes()
                'FillBlankGrids()
                FillContactDetails()
                FillAddressDetails()
                FillSourceDetails()
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub SetAttributes()
        btn_AddContact.Attributes.Add("onclick", "return AddContact('" & Me.ClientID & "','" & merchantbanking.ToString.ToLower & "')")
        btn_AddAddress.Attributes.Add("onclick", "return AddAddress('" & Me.ClientID & "','" & merchantbanking.ToString.ToLower & "')")
        btn_AddSourceDetail.Attributes.Add("onclick", "return AddSource('" & Me.ClientID & "','" & merchantbanking.ToString.ToLower & "')")
    End Sub

    Private Sub FillBlankGrids()
        Try
            OpenConn()
            FillBlankContactGrids()
            FillBlankAddresses()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillBlankGrids", "Error in FillBlankGrids", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
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
            dtContactGrid.Columns.Add("Interaction", GetType(String))
            dtContactGrid.Columns.Add("Branch", GetType(String))
            Session("ContactTable") = dtContactGrid
            dg_Contact.DataSource = dtContactGrid
            dg_Contact.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillBlankContactGrids", "Error in FillBlankContactGrids", "", ex)
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
            dt.Columns.Add("ProfileType", GetType(String))
            dt.Columns.Add("BusniessType", GetType(String))
            Session("CustAddressTable") = dt
            dg_Address.DataSource = dt
            dg_Address.DataBind()

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillBlankAddresses", "Error in FillBlankAddresses", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillContactDetails()
        Try
            OpenConn()
            Dim dt As DataTable

            If IsPostBack = True Then
                dt = Session("contactTable")
            Else
                Dim strProfileType As String = objCommon.ProfileTypes(ProfileType)
                Dim strBussType As String = objCommon.BusinessTypes(BusinessType)
                'dt = objCommon.FillDetailsGrid(dgcontact, "ID_FILL_ClientContactDetails", "CustomerId", CustomerId, "ProfileType", strProfileType, "BusniessType", strBussType)
                dt = objCommon.FillDetailsGrid(dgcontact, "ID_FILL_ClientContactDetails1", "CustomerId", CustomerId, "ProfileType", strProfileType, "BusniessType", strBussType)
            End If
            Session("contactTable") = dt
            ClearOptions()
            dgcontact.DataSource = dt
            dgcontact.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillContactDetails", "Error in FillContactDetails", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
        Hid_ResearchDocId.Value = ""
        Hid_ResearchDocName.Value = ""
        Hid_Interaction.Value = ""
        Hid_Branch.Value = ""
    End Sub


    Private Sub clearfields()
        Hid_AddCustomerId.Value = ""
        Hid_AddBusinessTypeId.Value = ""
        Hid_AddBusinessTypeNames.Value = ""
    End Sub
    Private Sub SourceClearOptions()
        Hid_BusTypeId.Value = ""
        Hid_SourceId.Value = ""
        Hid_SourceCustomerId.Value = ""

    End Sub

    Protected Sub dg_Contact_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_Contact.ItemCommand
        Try
            Dim strRetValues() As String
            Dim dt As DataTable
            Dim _FlagRet As Boolean


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
                If Trim(strRetValues(12)) = True Then
                    dt.Rows(e.Item.ItemIndex).Item("SectionType") = "F"
                Else
                    dt.Rows(e.Item.ItemIndex).Item("SectionType") = "B"
                End If
                dt.Rows(e.Item.ItemIndex).Item("CustomerId") = CustomerId
                dt.Rows(e.Item.ItemIndex).Item("ResearchDocId") = (strRetValues(13))
                dt.Rows(e.Item.ItemIndex).Item("ResearchDocName") = Trim(strRetValues(14))
                dt.Rows(e.Item.ItemIndex).Item("Interaction") = Trim(strRetValues(15))
                dt.Rows(e.Item.ItemIndex).Item("Branch") = Trim(strRetValues(16))



            Else
                If objCommon.DeleteClientContact(Hid_ContactDetailId.Value) = False Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Record is in use ');", True)
                    Exit Sub
                Else
                    dt.Rows.RemoveAt(e.Item.ItemIndex)
                End If


            End If
            Session("contactTable") = dt
            FillContactDetails()

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Contact_ItemCommand", "Error in dg_Contact_ItemCommand", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dg_Contact_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_Contact.ItemDataBound
        Try
            Dim img As New HtmlImage
            Dim lnkBtnEdit As ImageButton
            Dim lnkbtnDelete As ImageButton
            'Dim intContactDetailId As Integer
            Dim dtGrid As DataTable
            Dim txtContactPerson As TextBox
            Dim ContatDetailId As Integer

            Dim txtBusinessTypeNames As TextBox

            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then

                txtContactPerson = CType(e.Item.FindControl("txt_ContactPerson"), TextBox)
                txtBusinessTypeNames = CType(e.Item.FindControl("lbl_BusinessTypeNames"), TextBox)
                txtContactPerson.Attributes.Add("onKeyPress", "return OnlyScroll()")
                txtBusinessTypeNames.Attributes.Add("onKeyPress", "return OnlyScroll()")

                'Amit
                ContatDetailId = Val(CType(e.Item.FindControl("lbl_ContatDetailId"), Label).Text)
                lnkbtnDelete = CType(e.Item.FindControl("imgBtn_Delete"), ImageButton)
                lnkbtnDelete.Attributes.Add("onclick", "return Deletedetails1('" & ContatDetailId & "','" & Me.ClientID & "')")

                lnkBtnEdit = CType(e.Item.FindControl("imgBtn_Edit"), ImageButton)
                lnkBtnEdit.Attributes.Add("onclick", "return UpdateContactDetails('" & e.Item.ItemIndex & "','" & Me.ClientID & "',this,'" & merchantbanking.ToString.ToLower & "')")
                dtGrid = CType(Session("contactTable"), DataTable)
                Hid_BusinessTypeId.Value += Trim(CType(e.Item.FindControl("lbl_BusniessTypeIds"), Label).Text) & "!"
                Hid_BusinessTypeNames.Value += Trim(CType(e.Item.FindControl("lbl_BusinessTypeNames"), TextBox).Text) & "!"
                Hid_UserIds.Value += Trim(CType(e.Item.FindControl("lbl_UserIds"), Label).Text) & "!"
                Hid_NameOfUsers.Value += Trim(CType(e.Item.FindControl("lbl_NameOfUsers"), TextBox).Text) & "!"
                Hid_PhoneNo2.Value += Trim(CType(e.Item.FindControl("lbl_PhoneNo2"), Label).Text) & "!"
                Hid_FaxNo1.Value += Trim(CType(e.Item.FindControl("lbl_FaxNo1"), Label).Text) & "!"
                Hid_FaxNo2.Value += Trim(CType(e.Item.FindControl("lbl_FaxNo2"), Label).Text) & "!"
                Hid_SectionType.Value += Trim(CType(e.Item.FindControl("lbl_SectionType"), Label).Text) & "!"
                Hid_Interaction.Value += Trim(CType(e.Item.FindControl("lbl_Interaction"), Label).Text) & "!"
                Hid_Branch.Value += Trim(CType(e.Item.FindControl("lbl_Branch"), Label).Text) & "!"
                Hid_ResearchDocId.Value += Trim(CType(e.Item.FindControl("lbl_ResearchDocId"), Label).Text) & "!"
                Hid_ResearchDocName.Value += Trim(CType(e.Item.FindControl("lbl_ResearchDocName"), TextBox).Text) & "!"

                'Amit
                If (Val(CType(e.Item.FindControl("lbl_ContactId"), Label).Text) <> 0) Then
                    lnkbtnDelete.Visible = False
                End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Contact_ItemDataBound", "Error in dg_Contact_ItemDataBound", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_AddContact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddContact.Click
        Try
            OpenConn()
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
            dr.Item("Interaction") = Trim(strRetValues(15))
            dr.Item("Branch") = Trim(strRetValues(16))
            If Trim(strRetValues(12)) = True Then
                dr.Item("SectionType") = "F"
            Else
                dr.Item("SectionType") = "B"
            End If
            If strRetValues(14) <> "" Then
                dr.Item("ResearchDocName") = IIf(Right(strRetValues(14), 1) = ",", Left(strRetValues(14), strRetValues(14).Length - 1), strRetValues(14))
            End If

            If strRetValues(13) <> "" Then
                dr.Item("ResearchDocId") = IIf(Right(strRetValues(13), 1) = ",", Left(strRetValues(13), strRetValues(13).Length - 1), strRetValues(13))
            End If

            dr.Item("CustomerId") = CustomerId

            dt.Rows.Add(dr)
            Session("contactTable") = dt
            FillContactDetails()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Contact_ItemDataBound", "Error in dg_Contact_ItemDataBound", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub btn_AddAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddAddress.Click
        Try
            OpenConn()
            Dim strRetValues() As String
            Dim dt As DataTable
            Dim dr As DataRow

            strRetValues = Split(Hid_RetValues.Value, "!")
            dt = Session("CustAddressTable")
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
            dr.Item("CustomerId") = CustomerId
            If strRetValues(12) <> "" Then
                dr.Item("BusinessTypeNames") = IIf(Right(strRetValues(12), 1) = ",", Left(strRetValues(12), strRetValues(12).Length - 1), strRetValues(12))

            End If

            If strRetValues(11) <> "" Then
                dr.Item("BusniessTypeIds") = IIf(Right(strRetValues(11), 1) = ",", Left(strRetValues(11), strRetValues(11).Length - 1), strRetValues(11))
            End If

            dt.Rows.Add(dr)
            Session("CustAddressTable") = dt
            FillAddressDetails()

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_AddAddress_Click", "Error in btn_AddAddress_Click", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub FillAddressDetails()
        Try
            Dim dt As DataTable
            OpenConn()
            If IsPostBack = True Then
                dt = Session("CustAddressTable")
            Else
                Dim strProfileType As String = objCommon.ProfileTypes(ProfileType)
                Dim strBussType As String = objCommon.BusinessTypes(BusinessType)
                dt = objCommon.FillDetailsGrid(dg_Address, "Id_FILL_ClientCustMultipleAddress", "CustomerId", CustomerId, "ProfileType", strProfileType, "BusniessType", strBussType)
            End If


            Session("CustAddressTable") = dt
            clearfields()
            dg_Address.DataSource = dt
            dg_Address.DataBind()



        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillAddressDetails", "Error in FillAddressDetails", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub dg_Address_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_Address.ItemCommand
        Try
            Dim strRetValues() As String
            Dim dt As DataTable


            dt = CType(Session("CustAddressTable"), DataTable)
            If e.CommandName = "Edit" Then
                strRetValues = Split(Hid_RetValues.Value, "!")
                dt.Rows(e.Item.ItemIndex).Item("CustomerBranchName") = strRetValues(0)
                dt.Rows(e.Item.ItemIndex).Item("Address1") = strRetValues(1)
                dt.Rows(e.Item.ItemIndex).Item("Address2") = strRetValues(2)
                dt.Rows(e.Item.ItemIndex).Item("City") = strRetValues(3)
                dt.Rows(e.Item.ItemIndex).Item("PinCode") = strRetValues(4)
                dt.Rows(e.Item.ItemIndex).Item("State") = strRetValues(5)
                dt.Rows(e.Item.ItemIndex).Item("Country") = strRetValues(6)
                dt.Rows(e.Item.ItemIndex).Item("Phone") = strRetValues(7)
                dt.Rows(e.Item.ItemIndex).Item("FaxNo") = strRetValues(8)
                dt.Rows(e.Item.ItemIndex).Item("EmailId") = strRetValues(9)
                dt.Rows(e.Item.ItemIndex).Item("CustomerId") = CustomerId
                dt.Rows(e.Item.ItemIndex).Item("BusniessTypeIds") = (strRetValues(12))
                dt.Rows(e.Item.ItemIndex).Item("BusinessTypeNames") = Trim(strRetValues(13))
            Else
                dt.Rows.RemoveAt(e.Item.ItemIndex)
            End If
            Session("CustAddressTable") = dt
            FillAddressDetails()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub dg_Address_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_Address.ItemDataBound
        Try
            Dim img As New HtmlImage
            Dim lnkBtnEdit As ImageButton
            Dim lnkbtnDelete As ImageButton
            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then

                lnkbtnDelete = CType(e.Item.FindControl("imgBtn_Delete"), ImageButton)
                lnkbtnDelete.Attributes.Add("onclick", "return Deletedetails()")
                lnkBtnEdit = CType(e.Item.FindControl("imgBtn_Edit"), ImageButton)
                lnkBtnEdit.Attributes.Add("onclick", "return UpdateAddressDetails(this," & e.Item.ItemIndex & ",'" & Me.ClientID & "','" & merchantbanking.ToString.ToLower & "')")
                Hid_AddCustomerId.Value += Val(CType(e.Item.FindControl("lbl_CustomerId"), Label).Text) & "!"
                Hid_AddBusinessTypeId.Value += Trim(CType(e.Item.FindControl("lbl_BusniessTypeIds"), Label).Text) & "!"
                Hid_AddBusinessTypeNames.Value += Trim(CType(e.Item.FindControl("lbl_BusinessTypeNames"), TextBox).Text) & "!"

            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Address_ItemDataBound", "Error in dg_Address_ItemDataBound", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_AddSourceDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddSourceDetail.Click
        Try
            Dim strRetValues() As String
            Dim dt As DataTable
            Dim dr As DataRow


            strRetValues = Split(Hid_RetValues.Value, "!")
            dt = Session("SourceTable")
            dr = dt.NewRow
            dr.Item("SourceName") = Trim(strRetValues(0) & "")
            dr.Item("BusinessType") = Trim(strRetValues(1))
            'If Trim(strRetValues(2)) = True Then
            '    dr.Item("FeesType") = "F"
            'Else
            '    dr.Item("FeesType") = "V"
            'End If
            'dr.Item("FixedParticulars") = Trim(strRetValues(3))
            dr.Item("CustomerId") = CustomerId
            strRetValues(2) = CustomerId
            dr.Item("SourceId") = Val(strRetValues(3) & "")
            dr.Item("BusinessTypeId") = Val(strRetValues(4))

            dt.Rows.Add(dr)
            Session("SourceTable") = dt
            FillSourceDetails()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_AddSourceDetail_Click", "Error in btn_AddSourceDetail_Click", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillSourceDetails()
        Try
            OpenConn()
            Dim dt As DataTable

            If IsPostBack = True Then
                dt = Session("SourceTable")
            Else
                Dim strProfileType As String = objCommon.ProfileTypes(ProfileType)
                Dim strBussType As String = objCommon.BusinessTypes(BusinessType)
                dt = objCommon.FillDetailsGrid(dgSource, "ID_FILL_ClientSourceDetails", "CustomerId", CustomerId, "ProfileType", strProfileType, "BusniessType", strBussType)
            End If
            Session("SourceTable") = dt
            SourceClearOptions()
            dg_Source.DataSource = dt
            dg_Source.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_AddSourceDetail_Click", "Error in btn_AddSourceDetail_Click", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub dg_Source_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_Source.ItemCommand
        Try
            Dim strRetValues() As String
            Dim dt As DataTable


            dt = CType(Session("SourceTable"), DataTable)
            If e.CommandName = "Edit" Then
                strRetValues = Split(Hid_RetValues.Value, "!")
                dt.Rows(e.Item.ItemIndex).Item("SourceName") = strRetValues(0)
                dt.Rows(e.Item.ItemIndex).Item("BusinessType") = strRetValues(1)
                'dt.Rows(e.Item.ItemIndex).Item("FeesType") = strRetValues(2)
                'dt.Rows(e.Item.ItemIndex).Item("FixedParticulars") = strRetValues(3)
                dt.Rows(e.Item.ItemIndex).Item("SourceId") = (strRetValues(3))
                dt.Rows(e.Item.ItemIndex).Item("BusinessTypeId") = Trim(strRetValues(4))
                'If Trim(strRetValues(2)) = True Then
                '    dt.Rows(e.Item.ItemIndex).Item("FeesType") = "F"
                'Else
                '    dt.Rows(e.Item.ItemIndex).Item("FeesType") = "V"
                'End If
                dt.Rows(e.Item.ItemIndex).Item("CustomerId") = CustomerId
            Else
                dt.Rows.RemoveAt(e.Item.ItemIndex)
            End If
            Session("SourceTable") = dt
            FillSourceDetails()

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Source_ItemCommand", "Error in dg_Source_ItemCommand", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dg_Source_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_Source.ItemDataBound
        Try
            Dim img As New HtmlImage
            Dim lnkBtnEdit As ImageButton
            Dim lnkbtnDelete As ImageButton
            Dim dtGrid As DataTable


            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                'e.Item.ID = "itm" & e.Item.ItemIndex
                lnkbtnDelete = CType(e.Item.FindControl("imgBtn_Delete"), ImageButton)
                lnkbtnDelete.Attributes.Add("onclick", "return Deletedetails()")
                lnkBtnEdit = CType(e.Item.FindControl("imgBtn_Edit"), ImageButton)
                lnkBtnEdit.Attributes.Add("onclick", "return UpdateSource('" & e.Item.ItemIndex & "','" & Me.ClientID & "',this,'" & merchantbanking.ToString.ToLower & "')")
                dtGrid = CType(Session("SourceTable"), DataTable)
                Hid_BusTypeId.Value += Trim(CType(e.Item.FindControl("lbl_BusinessTypeId"), Label).Text) & "!"
                Hid_SourceId.Value += Trim(CType(e.Item.FindControl("lbl_SourceId"), Label).Text) & "!"
                Hid_SourceCustomerId.Value = Trim(CType(e.Item.FindControl("lbl_CustomerId"), Label).Text)


            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Source_ItemDataBound", "Error in dg_Source_ItemDataBound", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

   
End Class
