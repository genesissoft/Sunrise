Imports System.Data
Imports System.Reflection
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Data.SqlClient

Partial Class UserControls_SelectTempCustomer
    Inherits System.Web.UI.UserControl

    Dim PgName As String = "#searchlistbox#"
    Dim strPageName As String
    Dim strTableName As String
    Dim strProcName As String
    Dim strSelectedFieldId As String
    Dim strSelFieldName As String
    Dim strFieldName As String
    Dim chkbox As CheckBox

    Dim txtBox As TextBox
    Dim lnkbutton As LinkButton
    Dim lstbox As ListBox
    Dim lblVal As Label
    Dim btn As HtmlAnchor
    Dim arrColList() As String
    Dim intWidth As Int16
    Dim intFormWidth As Int16
    Dim intFormHeight As Int16
    Dim strCondFldName As String
    Dim strCondFldName1 As String
    Dim strCondFldName2 As String
    Dim strCondProperty As String
    Dim intSourceType As Int16
    Dim arrSelValues() As String
    Dim blnPostback As Boolean
    Dim blnCondExist As Boolean
    Dim strCondCtrlId As String
    Dim strCondCtrlId1 As String
    Dim strCondCtrlId2 As String
    Dim strSelvalueName As String

    Dim strCondFldValue As String
    Dim objCtrlCond As Object
    Dim _Columns As List(Of String)
    Dim ConditionalControl As Object
    Dim ConditionalControl1 As Object
    Dim ConditionalControl2 As Object
    Dim blnCheckYearCompany As Boolean
    Dim blnCheckCompany As Boolean
    Dim _Tables As List(Of String)
    Dim intTabIndex As Int16
    Dim PrimaryKey As String
    Dim SecondaryKey As String
    Dim _ColWidth As List(Of String)
    Dim _ColVisibility As List(Of String)
    Dim ColumnName As String
    Dim ColWidth As String
    Dim ColVis As String
    Dim DisPlayName As String
    Dim _DisplayName As List(Of String)

    Dim extraParams As String = ""

    'Public Event ButtonClick()
    Public Event ButtonClick As EventHandler
    Public Event LinkClick()

    Dim sqlConn As SqlConnection

    Enum enmSourceType
        Query = 1
        StoredProcedure = 2
    End Enum

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False), DefaultValue(enmSourceType.StoredProcedure)> _
    Public Property SourceType() As enmSourceType
        Get
            If intSourceType = 0 Then
                intSourceType = 2
            End If
            Return intSourceType
        End Get
        Set(ByVal value As enmSourceType)
            intSourceType = value
        End Set
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property SelectLinkButton() As LinkButton
        Get
            Return lnkbutton
        End Get
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property PageName() As String
        Get
            Return strPageName
        End Get
        Set(ByVal value As String)
            strPageName = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property TableName() As String
        Get
            Return strTableName
        End Get
        Set(ByVal value As String)
            strTableName = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property ProcName() As String
        Get
            Return strProcName
        End Get
        Set(ByVal value As String)
            strProcName = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property SelectedFieldId() As String
        Get
            Return strSelectedFieldId
        End Get
        Set(ByVal value As String)
            strSelectedFieldId = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property SelectedFieldName() As String
        Get
            Return strSelFieldName
        End Get
        Set(ByVal value As String)
            strSelFieldName = value
        End Set
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property SelectCheckbox() As CheckBox
        Get
            Return chkbox
        End Get
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property FieldName() As String
        Get
            Return strFieldName
        End Get
        Set(ByVal value As String)
            strFieldName = value
        End Set
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property AllFields() As HiddenField
        Get
            Return Hid_AllFields
        End Get
    End Property

    'declare  button 
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property SearchButton() As HtmlAnchor
        Get
            Return btn
        End Get
    End Property

    'ClientFunctionName     
    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property ClientFunctionName() As String
        Get
            Return ViewState("ClientFunctionName")
        End Get
        Set(ByVal value As String)
            ViewState("ClientFunctionName") = value
        End Set
    End Property

    'For setting tab index
    <Category("Layout"), Description("The fields collection"), DefaultValue(-1), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property TabIndex() As Int16
        Get
            If intTabIndex = 0 Then
                intTabIndex = -1
            End If
            Return intTabIndex
        End Get
        Set(ByVal value As Int16)
            intTabIndex = value
        End Set
    End Property

    <Category("Layout"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False), DefaultValue(400)> _
    Public Property FormHeight() As Int16
        Get
            If intFormHeight = 0 Then
                intFormHeight = 350
            End If
            Return intFormHeight
        End Get
        Set(ByVal value As Int16)
            intFormHeight = value
        End Set
    End Property

    'Like if you want all security according to type then pass (SM.SecurityTypeId) 
    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property ConditionalFieldName() As String
        Get
            Return strCondFldName
        End Get
        Set(ByVal value As String)
            strCondFldName = value
        End Set
    End Property

    'Like if you want all security according to type then pass id of control id like cbo_SecurityType
    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property ConditionalFieldId() As String
        Get
            Return strCondCtrlId
        End Get
        Set(ByVal value As String)
            strCondCtrlId = value
        End Set
    End Property

    'Like if you want all security according to type then pass (SM.SecurityTypeId) 
    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property ConditionalFieldName1() As String
        Get
            Return strCondFldName1
        End Get
        Set(ByVal value As String)
            strCondFldName1 = value
        End Set
    End Property

    'Like if you want all security according to type then pass (SM.SecurityTypeId)
    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property ConditionalFieldId1() As String
        Get
            Return strCondFldName1
        End Get
        Set(ByVal value As String)
            strCondFldName1 = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property ConditionalFieldName2() As String
        Get
            Return strCondFldName2
        End Get
        Set(ByVal value As String)
            strCondFldName1 = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property ConditionalFieldId2() As String
        Get
            Return strCondCtrlId2
        End Get
        Set(ByVal value As String)
            strCondCtrlId2 = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property SelectedValues() As String()
        Get
            Return arrSelValues
        End Get
    End Property

    'If you want to set postback property then use this property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property AutoPostback() As Boolean
        Get
            Return blnPostback
        End Get
        Set(ByVal value As Boolean)
            blnPostback = value
        End Set
    End Property

    ' if you want to control as Mandatory then set this true
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property Mandatory() As Boolean
        Get
            Return fnt_Mandatory.Visible
        End Get
        Set(ByVal value As Boolean)
            fnt_Mandatory.Visible = value
        End Set
    End Property

    'for returning selected fied id like 0% rbi have id 454
    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property SelectedId() As String
        Get
            Return Hid_SelectedId.Value
        End Get
        Set(ByVal value As String)
            Hid_SelectedId.Value = value
        End Set
    End Property

    'if you want to give external condition like wdm deal ='W' for trading deal then set this true other wise it show error in query
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property ConditionExist() As Boolean
        Get
            Return blnCondExist
        End Get
        Set(ByVal value As Boolean)
            blnCondExist = value
        End Set
    End Property

    'for returning selected fied name like 0% rbi  
    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property SelectedFieldText() As String
        Get
            Return Hid_SelectedFieldText.Value
        End Get
        Set(ByVal value As String)
            Hid_SelectedFieldText.Value = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property SelectedValueName() As String
        Get
            Return strSelvalueName
        End Get
        Set(ByVal value As String)
            strSelvalueName = value
        End Set
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(True)> _
    Public Property ShowAll() As Boolean
        Get
            Return ViewState("ShowAll")
        End Get
        Set(ByVal value As Boolean)
            ViewState("ShowAll") = value
        End Set
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property SearchTextBox() As TextBox
        Get
            Return txtBox
        End Get
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property SelectListBox() As ListBox
        Get
            Return lstbox
        End Get
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property GetLableText() As Label
        Get
            Return lblVal
        End Get
    End Property

    'to set this value befor master page
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'txtBox = txt_Name
        'lstbox = lst_Select
        btn = btn_Search
        'chkbox = chk_Select
    End Sub

    Protected Sub btn_Post_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Post.Click
        Try
            Session("retval") = ""
            Session("retval") = Hid_RetValues_fax.Value
            RaiseEvent LinkClick()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub Page_LinkClick() Handles Me.LinkClick

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                '  lst_Select.TabIndex = intTabIndex
                Hid_PageName.Value = PageName
                Hid_TableName.Value = TableName
                Hid_ProcName.Value = ProcName
                Hid_SelectedFieldId.Value = SelectedFieldId
                Hid_SelectedFieldName.Value = SelectedFieldName
                Hid_ConditionalFieldName.Value = ConditionalFieldName
                Hid_ControlId.Value = btn_Search.Parent.ClientID
                Hid_Postback.Value = AutoPostback.ToString()
                'Hid_SelectedFieldText.Value = txt_Name.Text
            Else
                Dim arrId() As String = Split(Hid_SelectedId.Value, "!")
                Dim arrName() As String = Split(Hid_SelectedFieldText.Value, "!")
                'lst_Select.Items.Clear()
                'For I As Int16 = 0 To arrId.Length - 1
                '    lst_Select.Items.Add(New ListItem(arrName(I), arrId(I)))
                'Next
                'txt_Name.Text = Hid_SelectedFieldText.Value
            End If

            'if you have set conditional fild name 
            If Trim(ConditionalFieldId & "") <> "" Then
                If Me.Page.Master Is Nothing Then
                    ConditionalControl = Me.Page.Controls(0).FindControl(Trim(ConditionalFieldId & ""))
                Else
                    ConditionalControl = Me.Page.Controls(0).FindControl("ContentPlaceHolder1").FindControl(Trim(ConditionalFieldId & ""))
                End If
                If ConditionalControl Is Nothing Then
                    Hid_ConditionalFieldValue.Value = Trim(ConditionalFieldId & "")
                Else
                    GetControlValue(ConditionalControl, Hid_ConditionalFieldValue, "")
                End If
                'very imp function
            End If

            If Trim(ConditionalFieldId1 & "") <> "" Then
                If Me.Page.Master Is Nothing Then
                    ConditionalControl1 = Me.Page.Controls(0).FindControl(Trim(ConditionalFieldId1 & ""))
                Else
                    ConditionalControl1 = Me.Page.Controls(0).FindControl("ContentPlaceHolder1").FindControl(Trim(ConditionalFieldId1 & ""))
                End If
                If ConditionalControl1 Is Nothing Then
                    Hid_ConditionalFieldValue1.Value = Trim(ConditionalFieldId1 & "")
                Else
                    GetControlValue(ConditionalControl1, Hid_ConditionalFieldValue1, "1")
                End If
                'very imp function
            End If

            If Trim(ConditionalFieldId2 & "") <> "" Then
                If Me.Page.Master Is Nothing Then
                    ConditionalControl2 = Me.Page.Controls(0).FindControl(Trim(ConditionalFieldId2 & ""))
                Else
                    ConditionalControl2 = Me.Page.Controls(0).FindControl("ContentPlaceHolder1").FindControl(Trim(ConditionalFieldId2 & ""))
                End If
                'very imp function
                If ConditionalControl2 Is Nothing Then
                    Hid_ConditionalFieldValue2.Value = Trim(ConditionalFieldId2 & "")
                Else
                    GetControlValue(ConditionalControl2, Hid_ConditionalFieldValue2, "1")
                End If
            End If

            Hid_ExtraParams.Value = extraParams

            'To get page name of control
            Dim strPage As String = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            'if you want to add validation before open search form
            Dim strFunName As String
            strFunName = "OpenSearchForm"

            Hid_StrPage.Value = strPage

            Hid_MyId.Value = Me.ID.ToString()

            Hid_SrcType.Value = SourceType
            Hid_TableName.Value = TableName
            Hid_SelectedFieldName.Value = SelectedFieldName
            Hid_ProcName.Value = Trim(ProcName & "")
            Hid_SearchFieldValue.Value = SelectedValueName
            Hid_ConditionalFieldName.Value = ConditionalFieldName
            Hid_ConditionalFieldName1.Value = ConditionalFieldName1
            Hid_ConditionalFieldName2.Value = ConditionalFieldName2
            Hid_PageName.Value = strPage
            Hid_CondExist.Value = ConditionExist.ToString.ToLower
            Hid_CompYear.Value = blnCheckYearCompany.ToString.ToLower
            Hid_ShowAll.Value = ShowAll.ToString.ToLower
            'Hid_AllFields.Value = show

            ' Hid_QueryString.Value = SourceType & "!" + btn_Search.Parent.ClientID + "!" & SelectedFieldName + "!" & _
            '                        ProcName & "!" & ConditionalFieldName & "!" & ConditionExist.ToString.ToLower & "!" & _
            '                        blnCheckYearCompany.ToString.ToLower & "!" & blnCheckCompany.ToString.ToLower & "!" & _
            '                        ConditionalFieldName1 + "!" & ConditionalFieldName2 & "!" & strPage & "!" & PrimaryKey & "!" & extraParams & "!" & SecondaryKey

            'btn_Search.Attributes.Add("onclick", "return show('" & Hid_MyId.Value & "')")

            'If Mandatory = True Then
            '    SearchTextBox.CssClass = SearchTextBox.CssClass + " Mandatory"
            '    btn_Del.Style.Add("display", "none")
            'End If

        Catch ex As Exception
            'Util.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ") & "');", True)
        End Try
    End Sub

    Private Function GetControlValue(ByVal CondCtrl As Object, ByVal Hid_CondFldVal As HiddenField, ByVal strTmp As String) As String
        'According to what you want your record like if you fill data according to securitytype 
        ' if coditional field id (cbo_securitytype) means it is dropdownlist
        Try
            'Dim strClick As String
            If TypeOf CondCtrl Is DropDownList Then
                'First time 
                Hid_CondFldVal.Value = CType(CondCtrl, DropDownList).SelectedValue
                ' if i change drop down value security type then grid data should be autometicaly fill on onchange event of conditional control (cbo_securitytype)
                'btn.Parent.ClientID is id of perticular page
                'CondCtrl.ClientId - it is new id of conditional control
                CondCtrl.Attributes.Add("onchange", "FillConditionalValues" & strTmp & "('" & btn.Parent.ClientID & "','" & CondCtrl.ClientID & "',0);")
            ElseIf TypeOf CondCtrl Is TextBox Then
                Hid_CondFldVal.Value = CType(CondCtrl, TextBox).Text
                CondCtrl.Attributes.Add("onchange", "FillConditionalValues" & strTmp & "('" & btn.Parent.ClientID & "','" & CondCtrl.ClientID & "');")
            ElseIf TypeOf CondCtrl Is HiddenField Then
                Hid_CondFldVal.Value = CType(CondCtrl, HiddenField).Value
                extraParams = extraParams & CondCtrl.ClientID & "#"
            ElseIf TypeOf CondCtrl Is Label Then
                Hid_CondFldVal.Value = CType(CondCtrl, Label).Text
            ElseIf TypeOf CondCtrl Is RadioButtonList Then
                Hid_CondFldVal.Value = CType(CondCtrl, RadioButtonList).SelectedValue
                For I As Int16 = 0 To CondCtrl.Items.Count - 1
                    CondCtrl.Items(I).Attributes.Add("onclick", "FillConditionalValues" & strTmp & "('" & btn.Parent.ClientID & "','" & CondCtrl.ClientID & "_" & I & "');")
                Next
            ElseIf TypeOf CondCtrl Is UserControl Then
                Hid_CondFldVal.Value = CondCtrl.SelectedId
                Dim ctrlHid As HiddenField = CType(CondCtrl.FindControl("Hid_SelectedId"), HiddenField)
                Dim ctrlBtn As HtmlImage = CType(CondCtrl.FindControl("btn_Search"), HtmlImage)
                extraParams = extraParams & ctrlHid.ClientID & "#"
                'strClick = Trim(ctrlBtn.Attributes("onclick") & "").Replace("OpenSearchForm", "OpenSearchWithFill" & strTmp)
                'strClick = Mid(strClick, 1, strClick.Length - 2) & ",'" & btn.Parent.ClientID & "','" & ctrlHid.ClientID & "');"
                'ctrlBtn.Attributes.Add("onclick", strClick)
            End If
            Return Hid_CondFldVal.Value
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ") & "');", True)
            Return ""
        End Try
    End Function

End Class
