Imports System.Data
Imports System.Data.SqlClient
Imports System.Reflection
Imports System.ComponentModel
Imports System.Collections.Generic
Imports log4net

Partial Class UserControls_ViewScreen
    Inherits System.Web.UI.UserControl
    Implements INamingContainer
    Dim _Columns As List(Of String)
    Dim PgName As String = "$ViewScreen$"
    Dim _ColumnWidths As List(Of Int16)
    Dim objCommon As New clsCommonFuns
    Dim cbo() As DropDownList
    Dim txt() As TextBox
    Dim dtGrid As DataTable
    Dim blnShowAll As Boolean
    Dim blnCondExist As Boolean
    Dim blnSrhOnly As Boolean
    Dim blnCheckComp As Boolean
    Dim blnChgColor As Boolean
    Dim strTableName As String
    Public Shared intCondCnt As Int16
    Dim blnCheckYearCompany As Boolean
    Dim blnCheckUser As Boolean
    Dim blnCheckUserType As Boolean
    Dim sqlConn As SqlConnection
    Public Event RowDataBound As GridViewRowEventHandler
    Public Event RowCommand As GridViewCommandEventHandler
    Dim objUtil As New Util
    Dim gridColCount As Integer = 0
    Dim ArrColCount As Integer = 0


    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property PageName() As String
        Get
            Return ViewState("PageName")
        End Get
        Set(ByVal value As String)
            ViewState("PageName") = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property CheckUserTypeId() As String
        Get
            Return ViewState("CheckUserTypeId")
        End Get
        Set(ByVal value As String)
            ViewState("CheckUserTypeId") = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property SelectedFieldName() As String
        Get
            Return ViewState("SelectedFieldName")
        End Get
        Set(ByVal value As String)
            ViewState("SelectedFieldName") = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property SelectProcName() As String
        Get
            Return ViewState("ProcName")
        End Get
        Set(ByVal value As String)
            ViewState("ProcName") = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property DeleteProcName() As String
        Get
            Return ViewState("DelProcName")
        End Get
        Set(ByVal value As String)
            ViewState("DelProcName") = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property DeleteFieldName() As String
        Get
            Return ViewState("DelFieldName")
        End Get
        Set(ByVal value As String)
            ViewState("DelFieldName") = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property DefaultSort() As String
        Get
            Return ViewState("DefaultSort")
        End Get
        Set(ByVal value As String)
            ViewState("DefaultSort") = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property TableAlias() As String
        Get
            Return ViewState("TableAlias")
        End Get
        Set(ByVal value As String)
            ViewState("TableAlias") = value
        End Set
    End Property

    <Category("Data"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property UserIdFieldName() As String
        Get
            Return ViewState("UserIdFieldName")
        End Get
        Set(ByVal value As String)
            ViewState("UserIdFieldName") = value
        End Set
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property CheckCompany() As Boolean
        Get
            Return blnCheckComp
        End Get
        Set(ByVal value As Boolean)
            blnCheckComp = value
        End Set
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property CheckUser() As Boolean
        Get
            Return blnCheckUser
        End Get
        Set(ByVal value As Boolean)
            blnCheckUser = value
        End Set
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property CheckUserType() As Boolean
        Get
            Return blnCheckUserType
        End Get
        Set(ByVal value As Boolean)
            blnCheckUserType = value
        End Set
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property ConditionExist() As Boolean
        Get
            Return blnCondExist
        End Get
        Set(ByVal value As Boolean)
            blnCondExist = value
        End Set
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property Columns() As List(Of String)
        Get
            If _Columns Is Nothing Then
                _Columns = New List(Of String)()
            End If
            Return _Columns
        End Get
        Set(ByVal value As List(Of String))
            _Columns = value
        End Set
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property ColumnWidths() As List(Of Int16)
        Get
            If _ColumnWidths Is Nothing Then
                _ColumnWidths = New List(Of Int16)()
            End If
            Return _ColumnWidths
        End Get
        Set(ByVal value As List(Of Int16))
            _ColumnWidths = value
        End Set
    End Property

    <Category("Url"), Browsable(True), Description("Select the Url for the page to which the site will redirect after the password is changed successfully"), _
     UrlProperty()> _
    Public Property NavigateUrl() As String
        Get
            Return TryCast(ViewState("NavigateUrl"), String)
        End Get
        Set(ByVal value As String)
            ViewState("NavigateUrl") = value
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

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property SearchOnly() As Boolean
        Get
            Return blnSrhOnly
        End Get
        Set(ByVal value As Boolean)
            blnSrhOnly = value
        End Set
    End Property
    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public Property CheckYearCompany() As Boolean
        Get
            Return blnCheckYearCompany
        End Get
        Set(ByVal value As Boolean)
            blnCheckYearCompany = value
        End Set
    End Property

    <Category("Behavior"), Description("The fields collection"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(False), Bindable(False)> _
    Public ReadOnly Property AddButton() As Button
        Get
            Return btn_Add
        End Get
    End Property

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If IsPostBack = True Then
                SetCondCount()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        'Response.Filter = New WhitespaceFilter(Response.Filter)
        Try
            'objCommon.OpenConn()
            Me.Page.Title = "View " & PageName & " Details"
            If IsPostBack = False Then
                Session("gridColCount") = ""
                Session("ArrColCount") = ""
                col_Header.InnerText = "View " & PageName & " Details"
                SetControls()
                SetAttributes()
                intCondCnt = 0
                EnsureChildControls()
                If Request.QueryString("Id") <> "" Then
                    Hid_SelectedId.Value = HttpUtility.UrlDecode(objCommon.DecryptText(Request.QueryString("Id")))
                    FillGrid(Hid_DefaultSort.Value)
                Else
                    FillGrid()
                End If
                SetLinkButtons()
            Else
                If blnShowAll = True Then
                    Dim txtBox As TextBox = pnl_Search.FindControl("txt_Search0")
                    If txtBox IsNot Nothing Then txtBox.Text = ""

                End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex)
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
    Private Sub SetControls()
        Dim objSearch As New clsSearch
        If DefaultSort <> "" Then
            Hid_DefaultSort.Value = DefaultSort
        Else
            Hid_DefaultSort.Value = SelectedFieldName & " ASC "
        End If

        'Hid_DefaultSort.Value = SelectedFieldName & " ASC"
        Hid_ColList.Value = objSearch.ConvertListToString(Columns, ",")
        Hid_ColWidths.Value = objSearch.ConvertListToString(ColumnWidths, ",")
        Hid_ColText.Value = objSearch.ConvertListToString(Columns, "!")
        If SearchOnly = True Then
            gv_Details.Columns(0).Visible = True
            gv_Details.Columns(1).Visible = False
            gv_Details.Columns(2).Visible = False

        End If

    End Sub

    Private Sub SetAttributes()
        Try
            btn_Search.Attributes.Add("onclick", "return ValidateSearch('" & Me.ClientID & "');")
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetAttributes", "Error in SetAttributes", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Protected Sub gv_Details_RowCommand(ByVal sender As Object, ByVal e As WebControls.GridViewCommandEventArgs) Handles gv_Details.RowCommand
        Try
            Dim gvRow As GridViewRow
            Dim imgBtn As ImageButton
            Dim strId As String
            Dim sqlTrans As SqlTransaction
            Dim strUrl As String
            Dim strPage As String

            strPage = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            If e.CommandName = "EditRow" Then
                imgBtn = TryCast(e.CommandSource, ImageButton)
                gvRow = imgBtn.Parent.Parent
                strId = gvRow.Cells(gvRow.Cells.Count - 2).Text
                strId = HttpUtility.UrlEncode(objCommon.EncryptText(strId))
                strUrl = NavigateUrl & "?Id=" & strId & "&Page=" & strPage & "&PageIndex=" & gv_Details.PageIndex
                Response.Redirect(strUrl, False)
            ElseIf e.CommandName = "DeleteRow" Then
                imgBtn = TryCast(e.CommandSource, ImageButton)
                gvRow = imgBtn.Parent.Parent
                strId = gvRow.Cells(gvRow.Cells.Count - 2).Text
                OpenConn()
                sqlTrans = sqlConn.BeginTransaction

                ' If (strPage = "DealSlipDetail.aspx" And DeleteBrokDealEntry(sqlTrans, strId)) Then

                'Exit Sub
                'End If

                If strPage = "MergeDealDetails.aspx" Then
                    If DeleteMergeDeal(sqlTrans, strId) = False Then Exit Sub
                ElseIf strPage = "SecurityMasterDetail.aspx" Then
                    If CheckSecurityDealExist(sqlTrans, strId) = False Then
                        Dim msg As String = "Deal has been entered for this security .Cant delete!"
                        Dim strHtml As String
                        strHtml = "alert('" + msg + "');"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                        Exit Sub
                    Else
                        If Delete(sqlTrans, strId) = False Then Exit Sub
                    End If

                ElseIf strPage = "IssuerMasterDetail_RDM.aspx" Then
                    If CheckEntryExist(sqlTrans, strId) = False Then
                        Dim msg As String = "Entries exist for this Issuer ! Cant Delete!"
                        Dim strHtml As String
                        strHtml = "alert('" + msg + "');"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                        Exit Sub
                    Else
                        If Delete(sqlTrans, strId) = False Then Exit Sub
                    End If
                ElseIf strPage = "CustomerTypeDetail.aspx" Then
                    If CheckEntryExist(sqlTrans, strId) = False Then
                        Dim msg As String = "Entries exist for this Customer Type ! Cant Delete!"
                        Dim strHtml As String
                        strHtml = "alert('" + msg + "');"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                        Exit Sub
                    Else
                        If Delete(sqlTrans, strId) = False Then Exit Sub
                    End If
                ElseIf strPage = "BrokerDetail.aspx" Then
                    If CheckEntryExist(sqlTrans, strId) = False Then
                        Dim msg As String = "Entries exist for this Broker ! Cant Delete!"
                        Dim strHtml As String
                        strHtml = "alert('" + msg + "');"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                        Exit Sub
                    Else
                        If DeleteBrokerMaster(sqlTrans, strId) = False Then Exit Sub
                    End If



                ElseIf strPage = "DealSlipDetail.aspx" Then

                    'If DeleteBrokDealEntry(sqlTrans, strId) = False Then

                    If CheckDealDone(sqlTrans, strId) = False Then
                        Dim msg As String = "This deal has been generated.Cant delete!"
                        Dim strHtml As String
                        strHtml = "alert('" + msg + "');"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                        Exit Sub
                    ElseIf CheckBrokDealDone(sqlTrans, strId) = False Then
                        Dim msg As String = "The corresponding Purchase/Sell deal has been generated.Cant delete!"
                        Dim strHtml As String
                        strHtml = "alert('" + msg + "');"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
                        Exit Sub
                    Else
                        If Delete(sqlTrans, strId) = False Then Exit Sub
                    End If
                    'End If

                Else
                    If Delete(sqlTrans, strId) = False Then Exit Sub
                End If
                sqlTrans.Commit()
                CloseConn()
                FillGrid(Hid_DefaultSort.Value)
            End If

            SetComboValues()
            RaiseEvent RowCommand(sender, e)

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "gv_Details_RowCommand", "Error in gv_Details_RowCommand", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try



    End Sub
    Private Function CheckEntryExist(ByVal sqlTrans As SqlTransaction, ByVal intId As Integer) As Boolean
        Try
            Dim a As Integer
            Dim sqlComm As New SqlCommand
            Dim strPage As String
            strPage = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_CHECK_MasterEntriesExist"
            sqlComm.Connection = sqlConn

            objCommon.SetCommandParameters(sqlComm, "@Id", SqlDbType.Int, 4, "I", , , Val(intId))

            objCommon.SetCommandParameters(sqlComm, "@PageName", SqlDbType.VarChar, 100, "I", , , strPage)
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@Valid", SqlDbType.Bit, 1, "O")
            sqlComm.ExecuteNonQuery()
            a = Val((sqlComm.Parameters("@Valid").Value))
            Return CBool(sqlComm.Parameters("@Valid").Value)
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "CheckDealDone", "Error in CheckDealDone", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function CheckIssuerEntryExist(ByVal sqlTrans As SqlTransaction, ByVal intId As Integer) As Boolean
        Try
            Dim a As Integer
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_CHECK_SecurityIssuerEntryExist"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@RDMIssuerId", SqlDbType.Int, 4, "I", , , Val(intId))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@Valid", SqlDbType.Bit, 1, "O")
            sqlComm.ExecuteNonQuery()
            a = Val((sqlComm.Parameters("@Valid").Value))
            Return CBool(sqlComm.Parameters("@Valid").Value)
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "CheckDealDone", "Error in CheckDealDone", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function CheckSecurityDealExist(ByVal sqlTrans As SqlTransaction, ByVal intId As Integer) As Boolean
        Try
            Dim a As Integer
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_CHECK_SecurityDealExist"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , Val(intId))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@Valid", SqlDbType.Bit, 1, "O")
            sqlComm.ExecuteNonQuery()
            a = Val((sqlComm.Parameters("@Valid").Value))
            Return CBool(sqlComm.Parameters("@Valid").Value)
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "CheckDealDone", "Error in CheckDealDone", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function DeleteMergeDeal(ByVal sqlTrans As SqlTransaction, ByVal intId As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_MergeDealEntry"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@MergedealNo", SqlDbType.VarChar, 30, "I", , , intId)
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "DeleteMergeDeal", "Error in DeleteMergeDeal", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function DeleteBrokDealEntry(ByVal sqlTrans As SqlTransaction, ByVal intId As String) As Boolean
        Try
            Dim a As Integer
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_CHECK_BrokDealEntry"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(intId))
            objCommon.SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@StrMessage", SqlDbType.VarChar, 100, "O")
            objCommon.SetCommandParameters(sqlComm, "@Valid", SqlDbType.Bit, 1, "O")
            sqlComm.ExecuteNonQuery()
            a = Val((sqlComm.Parameters("@Valid").Value))
            Return CBool(sqlComm.Parameters("@Valid").Value)
            'Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "DeleteBrokDealEntry", "Error in DeleteBrokDealEntry", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function DeleteBrokerMaster(ByVal sqlTrans As SqlTransaction, ByVal intId As String) As Boolean
        Try
            Dim a As Integer
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_BrokerMaster"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.Int, 4, "I", , , Val(intId))
            objCommon.SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@StrMessage", SqlDbType.VarChar, 100, "O")

            sqlComm.ExecuteNonQuery()

            'Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "DeleteBrokDealEntry", "Error in DeleteBrokDealEntry", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
        Return True
    End Function
    Private Function CheckDealDone(ByVal sqlTrans As SqlTransaction, ByVal intId As Integer) As Boolean
        Try
            Dim a As Integer
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_CHECK_DealDone"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(intId))
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Session("Yearid"))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@Valid", SqlDbType.Bit, 1, "O")
            sqlComm.ExecuteNonQuery()
            a = Val((sqlComm.Parameters("@Valid").Value))
            Return CBool(sqlComm.Parameters("@Valid").Value)
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "CheckDealDone", "Error in CheckDealDone", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function CheckBrokDealDone(ByVal sqlTrans As SqlTransaction, ByVal intId As Integer) As Boolean
        Try
            Dim a As Integer
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_CHECK_BrokDealDone"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(intId))
            objCommon.SetCommandParameters(sqlComm, "@Valid", SqlDbType.Bit, 1, "O")
            'objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            a = Val((sqlComm.Parameters("@Valid").Value))
            Return CBool(sqlComm.Parameters("@Valid").Value)
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "CheckBrokDealDone", "Error in CheckBrokDealDone", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub gv_Details_PageIndexChanging(ByVal sender As Object, ByVal e As WebControls.GridViewPageEventArgs) Handles gv_Details.PageIndexChanging
        Try
            FillGrid(Hid_DefaultSort.Value, e.NewPageIndex)
            SetComboValues()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "gv_Details_PageIndexChanging", "Error in gv_Details_PageIndexChanging", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub gv_Details_RowDataBound(ByVal sender As Object, ByVal e As WebControls.GridViewRowEventArgs) Handles gv_Details.RowDataBound
        Try
            Dim intGridCols As Int16
            Dim dv As DataView
            dv = CType(gv_Details.DataSource, DataView)
            If dv IsNot Nothing Then intGridCols = dv.Table.Columns.Count

            If e.Row.RowType <> DataControlRowType.Pager Then
                'e.Row.Cells(intGridCols).Visible = False
                e.Row.Cells(intGridCols + 1).Attributes.Add("style", "display:none")
                e.Row.Cells(intGridCols + 2).Attributes.Add("style", "display:none")
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim imgBtnEdit As ImageButton
                Dim imgBtnDelete As ImageButton
                Dim imgBtnSelect As ImageButton

                imgBtnEdit = CType(e.Row.FindControl("imgBtn_Edit"), ImageButton)
                imgBtnDelete = CType(e.Row.FindControl("imgBtn_Delete"), ImageButton)
                imgBtnSelect = CType(e.Row.FindControl("imgBtn_Select"), ImageButton)
                If e.Row.Cells(3).Text = "&nbsp;" Then
                    imgBtnSelect.Visible = False
                    imgBtnEdit.Visible = False
                    imgBtnDelete.Visible = False
                Else
                    imgBtnDelete.Attributes.Add("onclick", "return CheckDelete('" & Me.ClientID & "');")
                    imgBtnSelect.Attributes.Add("onclick", "return SelectOption(this,'" & e.Row.Cells(e.Row.Cells.Count - 2).Text & "','" & Me.ClientID & "')")
                End If
                If Hid_ColWidths.Value <> "" Then
                    Dim arrColWidth() As String = Split(Hid_ColWidths.Value, ",")
                    For I As Int16 = 3 To intGridCols
                        e.Row.Cells(I).Width = arrColWidth(I - 3)
                    Next
                End If
                If Val(dv.Table.Rows(e.Row.RowIndex).Item("Selected") & "") = 1 Then
                    'e.Row.BackColor = Drawing.Color.LightBlue
                    'e.Row.BackColor = Drawing.Color.FromName("#E1E1C3")
                    e.Row.BackColor = Drawing.Color.FromName("#EBFFEE")
                End If
            End If
            If e.Row.RowType = DataControlRowType.Header Then
                Dim lnkBtn As LinkButton
                For I As Int16 = 2 To intGridCols
                    lnkBtn = CType(e.Row.Cells(3).Controls(0), LinkButton)
                    lnkBtn.Attributes.Add("onclick", "SaveValues('" & Me.ClientID & "');")
                Next
            End If
            If e.Row.RowType = DataControlRowType.Pager Then
                Dim lnkBtn As Object
                For I As Int16 = 1 To gv_Details.PageCount
                    Dim strIdText As String
                    strIdText = IIf(I < 10, "0" & I, I)
                    lnkBtn = e.Row.FindControl("ctl" & strIdText)
                    If Not (lnkBtn Is Nothing) Then
                        lnkBtn.Attributes.Add("onclick", "SaveValues('" & Me.ClientID & "');")
                    End If
                Next
            End If
            RaiseEvent RowDataBound(sender, e)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "gv_Details_RowDataBound", "Error in gv_Details_RowDataBound", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "", Optional ByVal intPageIndex As Int16 = 0)
        Try
            Dim objSearch As New clsSearch
            Dim strCondFld As String = ""
            Dim intCondVal As Int16 = 0

            If CheckCompany = True Then
                strCondFld = "CompId"
                intCondVal = Val(Session("CompId") & "")
            ElseIf CheckUserType = True Then
                If Val(Session("UserTypeId") & "") <> ViewState("CheckUserTypeId") Then
                    If CheckUser = True Then
                        strCondFld = TableAlias & ".UserId"
                        intCondVal = Val(Session("UserId") & "")
                    End If
                End If
            ElseIf CheckUser = True Then
                'If Val(Session("UserTypeId") & "") <> 1 Then

                If Trim(Session("RestrictedAccess") & "") = "Y" Then
                        If UserIdFieldName = "" Then
                            strCondFld = TableAlias & ".UserId"
                        Else
                            strCondFld = TableAlias & "." & UserIdFieldName
                        End If
                        intCondVal = Val(Session("UserId") & "")
                    End If
                End If


            If IsPostBack = False Then
                dtGrid = objSearch.FillGrid(gv_Details, cbo, txt, Hid_ColList.Value, 2, strCondFld, intCondVal, ,
                            SelectProcName, strSort, intPageIndex, DeleteFieldName, Val(Hid_SelectedId.Value & ""), ConditionExist, CheckYearCompany, Val(Session("YearId") & ""), Val(Session("CompId") & ""))
            Else
                dtGrid = objSearch.FillGrid(gv_Details, cbo, txt, Hid_ColList.Value, 2, strCondFld, intCondVal, ,
                            SelectProcName, strSort, intPageIndex, , , ConditionExist, CheckYearCompany, Val(Session("YearId") & ""), Val(Session("CompId") & ""))
            End If

            If dtGrid IsNot Nothing Then
                FillFieldCombo(cbo, dtGrid)
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillGrid", "Error in FillGrid", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_ShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ShowAll.Click
        Try
            FillGrid(Hid_DefaultSort.Value)
            Dim arrCols() As String
            arrCols = Split(Hid_ColText.Value, "!")
            Dim lnkBtn As LinkButton = pnl_Search.FindControl("btn_And" & 0)
            If arrCols.Length = 2 Then
                If lnkBtn IsNot Nothing Then lnkBtn.Visible = False
            Else
                If lnkBtn IsNot Nothing Then lnkBtn.Visible = True
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_ShowAll_Click", "Error in btn_ShowAll_Click", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub gv_Details_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_Details.Sorting
        Try
            FillGrid(e.SortExpression & " " & GetSortDirection(e.SortExpression))
            SetComboValues()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "gv_Details_Sorting", "Error in gv_Details_Sorting", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Search.Click
        Try
            FillGrid(Hid_DefaultSort.Value)
            SetComboValues()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_Search_Click", "Error in btn_Search_Click", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function GetSortDirection(ByVal column As String) As String
        Try
            Dim sortDirection = "ASC"
            Dim sortExpression = TryCast(ViewState("SortExpression"), String)
            If sortExpression IsNot Nothing Then
                If sortExpression = column Then
                    Dim lastDirection = TryCast(ViewState("SortDirection"), String)
                    If lastDirection IsNot Nothing AndAlso lastDirection = "ASC" Then
                        sortDirection = "DESC"
                    End If
                End If
            End If
            ViewState("SortDirection") = sortDirection
            ViewState("SortExpression") = column
            Return sortDirection
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetSortDirection", "Error in GetSortDirection", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim strPage As String = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Dim strUrl As String
        strUrl = NavigateUrl & "?Page=" & strPage
        Response.Redirect(strUrl, False)
    End Sub

    Private Sub BuildCondControls()
        Try
            Dim tbl As Table
            Dim row As TableRow
            Dim cell As TableCell
            Dim cbo_Search As DropDownList
            Dim txt_Search As TextBox

            Dim arrFieldNames() As String = Split(Hid_FieldNames.Value, "!")
            Dim arrFieldValues() As String = Split(Hid_FieldValues.Value, "!")

            ReDim cbo(intCondCnt)
            ReDim txt(intCondCnt)
            For I As Int16 = 0 To intCondCnt
                cbo_Search = New DropDownList
                cbo_Search.ID = "cbo_Search" & I
                cbo_Search.CssClass = "ComboBoxCSS"
                cbo(I) = cbo_Search

                txt_Search = New TextBox
                txt_Search.ID = "txt_Search" & I
                txt_Search.CssClass = "TextBoxCSS"
                txt(I) = txt_Search
                Dim lnk_And As LinkButton
                lnk_And = New LinkButton
                lnk_And.ID = "btn_And" & I
                lnk_And.Text = "And"
                lnk_And.Attributes.Add("onclick", "SaveValues('" & Me.ClientID & "');")
                AddHandler lnk_And.Click, AddressOf lnk_And_Click

                If Session("gridColCount").ToString() = Session("ArrColCount").ToString() And Session("ArrColCount").ToString() <> "" Then
                    lnk_And.Enabled = False
                End If

                Dim ctrl() As Control = {cbo_Search, txt_Search, lnk_And}

                cell = New TableCell
                cell.CssClass = "LabelCSS"
                cell.Text = "Search Field:"
                row = New TableRow
                row.Cells.Add(cell)
                For J As Int16 = 0 To 2
                    cell = New TableCell
                    cell.Controls.Add(ctrl(J))
                    row.Cells.Add(cell)
                Next

                tbl = New Table
                tbl.Rows.Add(row)
                pnl_Search.Controls.Add(tbl)
            Next

            For I As Int16 = 0 To intCondCnt - 1
                Dim lnkBtn As LinkButton = pnl_Search.FindControl("btn_And" & I)
                If lnkBtn IsNot Nothing Then lnkBtn.Visible = False
            Next
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "BuildCondControls", "Error in BuildCondControls", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillFieldCombo(ByVal cbo() As DropDownList, ByVal dt As DataTable)
        Try
            Dim arrCols() As String
            arrCols = Split(Hid_ColText.Value, "!")
            For I As Int16 = 0 To cbo.Length - 1
                Dim cbo_Name As DropDownList = cbo(I)
                cbo_Name.Items.Clear()
                For J As Int16 = 0 To dt.Columns.Count - 3
                    If arrCols(J).ToUpper.IndexOf("AS ") <> -1 Then
                        arrCols(J) = arrCols(J).Substring(0, arrCols(J).ToUpper.IndexOf("AS "))
                    End If
                    cbo_Name.Items.Add(New ListItem(dt.Columns(J).ColumnName, arrCols(J)))
                Next
            Next
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillFieldCombo", "Error in FillFieldCombo", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Overrides Sub CreateChildControls()
        BuildCondControls()
    End Sub

    Private Sub SetCondCount()
        Try
            Dim strCtrlId As String
            strCtrlId = Page.Request.Params("__EVENTTARGET")
            If String.IsNullOrEmpty(strCtrlId) = False Then
                If strCtrlId.IndexOf("btn_And") <> -1 Then
                    intCondCnt = intCondCnt + 1
                    Exit Sub
                End If
            End If
            For Each strKey As String In Page.Request.Form.Keys
                If String.IsNullOrEmpty(strKey) = False Then
                    If strKey.IndexOf("btn_ShowAll") <> -1 Then
                        intCondCnt = 0
                        blnShowAll = True
                    End If
                End If
            Next
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetCondCount", "Error in SetCondCount", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Public Sub lnk_And_Click(ByVal sender As Object, ByVal e As EventArgs)
        FillGrid(Hid_DefaultSort.Value)
        SetComboValues()

    End Sub

    Private Function Delete(ByVal sqlTrans As SqlTransaction, ByVal intId As Integer) As Boolean
        Dim sqlComm As New SqlCommand
        Try
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Transaction = sqlTrans

            sqlComm.CommandText = "ID_DELETE_All"


            objCommon.SetCommandParameters(sqlComm, "@TableName", SqlDbType.VarChar, 50, "I", , , TableName)
            objCommon.SetCommandParameters(sqlComm, "@DelFldName", SqlDbType.VarChar, 50, "I", , , DeleteFieldName)
            objCommon.SetCommandParameters(sqlComm, "@DelFldValue", SqlDbType.BigInt, 8, "I", , , Val(intId))
            objCommon.SetCommandParameters(sqlComm, "@IntFlag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@StrMessage", SqlDbType.VarChar, 100, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "Delete", "Error in Delete", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('Can not Delete this " & SelectedFieldName & ", Client Exist');", True)
        End Try
    End Function

    Private Sub SetCombos()
        Try
            Dim arrColName() As String
            For I As Int16 = 0 To intCondCnt
                Dim cbo As DropDownList = pnl_Search.FindControl("cbo_Search" & I)
                If arrColName IsNot Nothing Then
                    For J As Int16 = 0 To arrColName.Length - 1
                        Dim K As Int16 = cbo.Items.Count - 1
                        While K >= 0
                            If cbo.Items(K).Value = arrColName(J) Then
                                cbo.Items.RemoveAt(K)
                            End If
                            K = K - 1
                        End While
                    Next
                End If
                ReDim Preserve arrColName(I)
                arrColName(I) = cbo.SelectedValue
            Next
            SetLinkButtons()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetCombos", "Error in SetCombos", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub SetComboValues()
        Try
            Dim cbo As DropDownList
            Dim arrValues() As String = Split(Hid_FieldValues.Value, "!")
            For I As Int16 = 0 To intCondCnt
                If I < arrValues.Length - 1 Then
                    cbo = pnl_Search.FindControl("cbo_Search" & I)
                    cbo.SelectedValue = arrValues(I)
                End If
            Next
            SetCombos()
            '  ArrColCount = arrValues.Length

            Session("ArrColCount") = arrValues.Length

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetComboValues", "Error in SetComboValues", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub SetLinkButtons()
        Try
            Dim cbo As DropDownList = pnl_Search.FindControl("cbo_Search" & intCondCnt)
            If cbo.Items.Count = 1 Then
                Dim lnkBtn As LinkButton = pnl_Search.FindControl("btn_And" & intCondCnt)
                If lnkBtn IsNot Nothing Then lnkBtn.Visible = False
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetLinkButtons", "Error in SetLinkButtons", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

End Class