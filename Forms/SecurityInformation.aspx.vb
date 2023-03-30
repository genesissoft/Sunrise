Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports log4net

Partial Class Forms_SecurityInformation
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim PgName As String = "$SecurityInformation$"
    Dim dsmenu As DataSet
    Dim dsDPDetails As DataSet
    Dim newTable As DataTable
    Dim DpDetailsTable As DataTable
    Dim trmenu As DataRow
    Dim cbo() As DropDownList
    Dim txt() As TextBox
    Dim dtGrid As DataTable
    Public Shared intCondCnt As Int16
    Dim objSearch As New clsSearch
    Dim _Columns As List(Of String)
    Dim _ColumnWidths As List(Of Int16)
    Dim blnShowAll As Boolean
    Dim intSecId As Integer
    Dim intSelIndex As Integer = -1

    Dim MatDate() As Date
    Dim MatAmt() As Double
    Dim CoupDate() As Date
    Dim CoupRate() As Double
    Dim CallDate() As Date
    Dim CallAmt() As Double
    Dim PutDate() As Date
    Dim PutAmt() As Double
    Dim strCoupFlag As String
    Dim datYTM As Date
    Dim decFaceValue As Decimal
    Dim datIssue As Date
    Dim datInterest As Date
    Dim datBookClosure As Date
    Dim decRate As Decimal
    Dim blnNonGovernment As Boolean
    Dim blnRateActual As Boolean
    Dim blnDMAT As Boolean
    Dim intBKDiff As Integer
    Dim dblPepCoupRate As Double
    Dim blnCompRate As Boolean
    Dim blnCloseButton As Boolean
    Dim sqlConn As SqlConnection
    Dim NatureOFInstrument As String
    Dim objUtil As New Util

    Public Property SecurityId() As Integer
        Get
            Return intSecId
        End Get
        Set(ByVal value As Integer)
            intSecId = value
        End Set
    End Property

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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If IsPostBack = True Then
            SetCondCount()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'If Val(Session("UserId") & "") = 0 Then
            '    Response.Redirect("Login.aspx", False)
            '    Exit Sub
            'End If
            'objCommon.OpenConn()

            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            btn_CalRate.Attributes.Add("onclick", "return ShowYieldCalculation();")

            If IsPostBack = False Then

                intCondCnt = 0
                SetControls()
                SetAttribute()
                EnsureChildControls()
                Hid_FaxQuoteId.Value = Val(Request.QueryString("FaxQuoteId") & "")
                If Trim(Request.QueryString("rowIndex") & "") <> "" Then
                    Hid_RowIndex.Value = Trim(Request.QueryString("rowIndex") & "")
                    Hid_FaxQuoteId.Value = Val(Request.QueryString("FaxQuoteId") & "")

                    GetDataTableValues()
                    btn_Add.Visible = False
                    btn_update.Visible = True
                    btn_Sumbit.Visible = False
                    pnl_Search.Visible = False
                    btn_Search.Visible = False
                    btn_ShowAll.Visible = False
                    FillSecurityGrid()
                Else
                    btn_Add.Visible = True
                    btn_update.Visible = False
                    ' btn_Sumbit.Visible = True
                    FillGrid()
                End If

                SetLinkButtons()
                SetTempSecurityTable()
                txt_Date.Text = Format(DateAndTime.Today, "dd/MM/yyyy")

                dblYTMAnn = 0
                dblYTMAnn = 0
                dblYTCAnn = 0
                dblYTPAnn = 0
                dblYTMSemi = 0
                dblYTCSemi = 0
                dblYTPSemi = 0

            Else
                If blnShowAll = True Then
                    Dim txtBox As TextBox = pnl_Search.FindControl("txt_Search0")
                    If txtBox IsNot Nothing Then txtBox.Text = ""
                    FillGrid()
                End If
            End If
            lbl_Save.Visible = False

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub SetAttribute()
        txt_Date.Attributes.Add("onkeypress", "OnlyDate();")
        txt_Date.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_Date.Attributes.Add("onblur", "CheckDate(this);")
        btn_Close.Attributes.Add("onclick", "return Close();")
        txt_Date.Text = Format(Now, "dd/MM/yyyy")
        txt_LotSize.Attributes.Add("onblur", "ConvertUCase(this);")
        txt_RatingRemark.Attributes.Add("onblur", "ConvertUCase(this);")
        btn_update.Attributes.Add("onclick", "return RateValidation();")
        btn_Add.Attributes.Add("onclick", "return RateValidation();")
        txt_Rate.Attributes.Add("onkeypress", "OnlyDecimal();")
        btn_Show.Attributes.Add("onclick", "return OpenViewWindow();")
        btn_Search.Attributes.Add("onclick", "return SaveValues()")
        txt_SecurityName.Enabled = False
        'btn_Sumbit.Attributes.Add("onclick", "return YieldValidation();")

    End Sub

    Private Sub SetControls()

        ' original
        'Columns.Add("SecurityName")
        'Columns.Add("ST.SecurityTypeName")
        'Columns.Add("Abbreviation")
        'Columns.Add("SecurityIssuer")

        'Columns.Add("MaturityDate")
        'Columns.Add("CallDate")
        'Columns.Add("CouponRate")
        'Columns.Add("FaceValue")

        'Columns.Add("NSDLAcNumber")
        'Columns.Add("CASE WHEN S.SecurityId IS NOT NULL THEN 'T' ELSE 'Q' END AS ColorId")
        'Columns.Add("S.SecurityId")
        'Columns.Add("S.SecurityTypeId")


        'ColumnWidths.Add("100")
        'ColumnWidths.Add("100")
        'ColumnWidths.Add("100")
        'ColumnWidths.Add("100")

        'ColumnWidths.Add("100")
        'ColumnWidths.Add("100")
        'ColumnWidths.Add("100")
        'ColumnWidths.Add("100")
        'ColumnWidths.Add("100")

        'ColumnWidths.Add("50")
        'ColumnWidths.Add("50")
        'ColumnWidths.Add("50")

        Columns.Add("SecurityName")
        Columns.Add("ST.SecurityTypeName")
        Columns.Add("Abbreviation")
        Columns.Add("SecurityIssuer")

        Columns.Add("dbo.ID_GET_SecurityRating(SecurityId)")
        Columns.Add("MaturityDate")
        Columns.Add("CallDate")
        Columns.Add("CouponRate")

        Columns.Add("FaceValue")
        Columns.Add("NSDLAcNumber")
        'Columns.Add("CASE WHEN S.SecurityId IS NOT NULL THEN 'T' ELSE 'Q' END AS ColorId")
        Columns.Add("S.SecurityId")
        Columns.Add("S.SecurityTypeId")
        Columns.Add("OrderId")
        Columns.Add("CreditRating")


        ColumnWidths.Add("100")
        ColumnWidths.Add("100")
        ColumnWidths.Add("100")
        ColumnWidths.Add("100")

        ColumnWidths.Add("100")
        ColumnWidths.Add("100")
        ColumnWidths.Add("100")
        ColumnWidths.Add("100")

        ColumnWidths.Add("100")
        ColumnWidths.Add("50")
        ColumnWidths.Add("50")
        ColumnWidths.Add("50")
        ColumnWidths.Add("50")
        ColumnWidths.Add("50")


        Hid_ColList.Value = objSearch.ConvertListToString(Columns, ",")
        Hid_ColWidths.Value = objSearch.ConvertListToString(ColumnWidths, ",")
        Hid_ColText.Value = objSearch.ConvertListToString(Columns, "!")

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
    Private Sub FillSecurityGrid()
        Try
            Dim dt As DataTable

            If (Val(Hid_SecurityId.Value) = 0) Then
                dt = objCommon.FillDetailsGrid(dg_Selection, "ID_FILL_SecurityFaxGenaration", "SecurityId", Val(ViewState("Id")), "Compid", Val(Session("Compid")) & "")
            Else
                dt = objCommon.FillDetailsGrid(dg_Selection, "ID_FILL_SecurityFaxGenaration", "SecurityId", Val(Hid_SecurityId.Value & ""))

                'dt = objCommon.FillDetailsGrid(dg_Selection, "ID_FILL_SecurityFaxGenaration", "SecurityId", Val(Hid_SecurityId.Value), "Compid", Val(Session("Compid")) & "")
            End If

            'If (Hid_SecurityId.Value = "") Then
            '    dt = objCommon.FillDetailsGrid(dg_Selection, "ID_FILL_SecurityFaxGenaration", "SecurityId", Val(ViewState("Id") & ""))
            'Else
            '    dt = objCommon.FillDetailsGrid(dg_Selection, "ID_FILL_SecurityFaxGenaration", "SecurityId", Val(Hid_SecurityId.Value))
            'End If

            'Hid_SecurityId.Value = ""
            If dt.Rows.Count > 0 Then
                txt_SecurityName.Text = Trim(dt.Rows(0).Item("SecurityName") & "")
            End If

            Session("SecurityTable") = dt
            dg_Selection.DataSource = dt
            dg_Selection.DataBind()

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillSecurityGrid", "Error in FillSecurityGrid", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally

        End Try
    End Sub

    Private Sub BuildCondControls()
        Try
            Dim tbl As Table
            Dim row As TableRow
            Dim cell As TableCell
            Dim cbo_Search As DropDownList
            Dim txt_Search As TextBox
            Dim lnk_And As LinkButton
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

                lnk_And = New LinkButton
                lnk_And.ID = "btn_And" & I
                lnk_And.Text = "And"
                lnk_And.Attributes.Add("onclick", "SaveValues('" & Me.ClientID & "');")
                AddHandler lnk_And.Click, AddressOf lnk_And_Click
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
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "BuildCondControls", "Error in BuildCondControls", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Public Sub lnk_And_Click(ByVal sender As Object, ByVal e As EventArgs)
        FillGrid(Hid_DefaultSort.Value)
        SetComboValues()
    End Sub

    Protected Overrides Sub CreateChildControls()
        BuildCondControls()
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
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetComboValues", "Error in SetComboValues", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub SetCombos()
        Try
            'Dim arrColName() As String
            'arrColName = Split(Hid_ColText.Value, "!")
            'For I As Int16 = 0 To intCondCnt
            '    Dim cbo As DropDownList = pnl_Search.FindControl("cbo_Search" & I)
            '    If arrColName IsNot Nothing Then
            '        For J As Int16 = 0 To arrColName.Length - 1
            '            Dim K As Int16 = cbo.Items.Count - 1
            '            While K >= 0
            '                If cbo.Items(K).Value = arrColName(J) Then
            '                    cbo.Items.RemoveAt(K)
            '                End If
            '                K = K - 1
            '            End While
            '        Next
            '    Else
            '        ReDim Preserve arrColName(I)
            '        arrColName(I) = cbo.SelectedValue
            '    End If
            'Next
            SetLinkButtons()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetCombos", "Error in SetCombos", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "", Optional ByVal intPageIndex As Int16 = 0)
        Try
            Dim objSearch As New clsSearch
            Dim strCondFld As String = ""
            Dim intCondVal As Int16 = 0
            dtGrid = objSearch.FillGrid(dg_Selection, cbo, txt, Hid_ColList.Value, 2, strCondFld, intCondVal, , "ID_SEARCH_SecurityFields", strSort, intPageIndex, , , True)

            'changed by poonam for colorid grid
            dtGrid.Columns.Remove("ColorId")

            Session("SecurityTable") = dtGrid
            FillFieldCombo(cbo, dtGrid)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillGrid", "Error in FillGrid", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillFieldCombo(ByVal cbo() As DropDownList, ByVal dt As DataTable)
        Try
            Dim arrCols() As String
            arrCols = Split(Hid_ColText.Value, "!")
            For I As Int16 = 0 To cbo.Length - 1
                Dim cbo_Name As DropDownList = cbo(I)
                cbo_Name.Items.Clear()
                For J As Int16 = 0 To dt.Columns.Count - 4
                    If arrCols(J).ToUpper.IndexOf("AS") <> -1 Then
                        arrCols(J) = arrCols(J).Substring(0, arrCols(J).ToUpper.IndexOf("AS"))
                    End If
                    cbo_Name.Items.Add(New ListItem(dt.Columns(J).ColumnName, arrCols(J)))
                Next
            Next
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillFieldCombo", "Error in FillFieldCombo", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Search.Click
        lnk_And_Click(sender, e)
    End Sub

    Protected Sub dg_Selection_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dg_Selection.PageIndexChanging
        Try
            FillGrid(Hid_DefaultSort.Value, e.NewPageIndex)
            '  SetComboValues()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Selection_PageIndexChanging", "Error in dg_Selection_PageIndexChanging", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dg_Selection_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dg_Selection.RowCommand
        Try
            Dim imgBtn As ImageButton
            Dim gvRow As GridViewRow
            Dim dr As DataRow
            Dim dt As DataTable
            dt = Session("SecurityTable")
            If e.CommandName = "EditRow" Then
                imgBtn = TryCast(e.CommandSource, ImageButton)
                gvRow = imgBtn.Parent.Parent
                ViewState("EditIndex") = gvRow.DataItemIndex
                ViewState("ContactEditFlag") = True
                dr = dt.Rows(gvRow.DataItemIndex)
                Hid_SecurityId.Value = dr.Item("SecurityId")
                Hid_SecurityTypeId.Value = dr.Item("SecurityTypeId")
                Hid_SecurityName.Value = dr.Item("SecurityName")
                Hid_OrderId.Value = Val(dr.Item("OrderId") & "")
                Hid_CreditRating.Value = Val(dr.Item("CreditRating") & "")
            ElseIf e.CommandName = "DeleteRow" Then
                imgBtn = TryCast(e.CommandSource, ImageButton)
                gvRow = imgBtn.Parent.Parent
            ElseIf e.CommandName = "SelectRow" Then
                imgBtn = TryCast(e.CommandSource, ImageButton)
                gvRow = imgBtn.Parent.Parent
                dr = dt.Rows(gvRow.DataItemIndex)
                Hid_Security.Value = dr.Item("SecurityName")
                txt_SecurityName.Text = dr.Item("SecurityName")
                Hid_SecurityId.Value = dr.Item("SecurityId")
                Hid_SecurityTypeId.Value = dr.Item("SecurityTypeId")
                txt_RatingRemark.Text = (dr.Item("Rating") & "")
                Hid_OrderId.Value = Val(dr.Item("OrderId") & "")
                Hid_CreditRating.Value = Val(dr.Item("CreditRating") & "")
                intSelIndex = gvRow.DataItemIndex
                FillGrid()
                'ElseIf e.CommandName = "ShowStock" Then
                '    imgBtn = TryCast(e.CommandSource, ImageButton)
                '    gvRow = imgBtn.Parent.Parent
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Selection_RowCommand", "Error in dg_Selection_RowCommand", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillTempSecurityTable()
        Try

            FillGrid()

            Dim dr As DataRow
            Dim dt As DataTable
            Dim dv As DataView

            dt = Session("SelectedTempSecurityTable")
            dv = New DataView(dt)
            dv.RowFilter = "SecurityId=" & Hid_SecurityId.Value

            If (Hid_SecurityId.Value = "") Then
                Dim msg As String = "Please Select Atleast One Security"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
                Exit Sub
            Else

                If dblYTMAnn = 0 And dblYTCAnn = 0 Then
                    Dim msg As String = "Please Calculate Rate !"
                    Dim strHtml As String
                    strHtml = "alert('" + msg + "');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
                    Exit Sub
                End If
                If dv.Count > 0 Then
                    Dim msg As String = "This Security is already added !"
                    Dim strHtml As String
                    strHtml = "alert('" + msg + "');"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
                    Exit Sub
                End If

                dr = dt.NewRow()
                dr("FaxQuoteId") = Val(Hid_FaxQuoteId.Value)
                dr("SecurityId") = Hid_SecurityId.Value
                dr("SecurityName") = txt_SecurityName.Text
                dr("Date") = txt_Date.Text
                dr("Rate") = txt_Rate.Text
                dr("LotSize") = txt_LotSize.Text
                dr("RatingRemark") = txt_RatingRemark.Text
                dr("Days") = rdo_Days.SelectedValue
                dr("DaysOptions") = rdo_DaysOptions.SelectedValue
                dr("PhysicalDMAT") = rdo_PhysicalDMAT.SelectedValue
                dr("IPCalc") = rdo_IPCalc.SelectedValue
                dr("RateActual") = rdo_RateActual.SelectedValue
                dr("YXM") = rdo_YXM.SelectedValue
                dr("YTMAnn") = Math.Round(dblYTMAnn, 4)
                dr("YTCAnn") = Math.Round(dblYTCAnn, 4)
                dr("YTPAnn") = Math.Round(dblYTPAnn, 4)
                dr("YTMSemi") = Math.Round(dblYTMSemi, 4)
                dr("YTCSemi") = Math.Round(dblYTCSemi, 4)
                dr("YTPSemi") = Math.Round(dblYTPSemi, 4)
                dr("SecurityTypeId") = Hid_SecurityTypeId.Value
                If Hid_SecurityTypeId.Value = 54 Or Hid_SecurityTypeId.Value = 56 Or Hid_SecurityTypeId.Value = 66 Then
                    dr("SecurityTypeName") = "CENTRAL GOVERNMENT / STATE GOVERNMENT & TREASURY BILLS ( 45% TO 50%)"
                Else
                    dr("SecurityTypeName") = "CORPORATE BONDS ( 35% TO 45%)"
                End If
                dr("OrderId") = Hid_OrderId.Value
                dr("Semi_Ann_Flag") = Hid_Semi_Ann_Flag.Value
                dr("CombineIPMat") = Hid_CombineIPMat.Value
                dr("Rate_Actual_Flag") = Hid_Rate_Actual_Flag.Value
                dr("Equal_Actual_Flag") = Hid_Equal_Actual_Flag.Value
                dr("IntDays") = Hid_IntDays.Value
                dr("FirstYrAllYr") = Hid_FirstYrAllYr.Value

              
                dt.Rows.Add(dr)
                Session("SelectedTempSecurityTable") = dt
            End If

            ClearField()
            lbl_Save.Visible = True
            btn_Sumbit.Visible = True

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillTempSecurityTable", "Error in FillTempSecurityTable", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub ClearField()
        Hid_SecurityId.Value = ""
        Hid_SecurityName.Value = ""
        'txt_Date.Text = ""
        txt_Rate.Text = ""
        txt_LotSize.Text = ""
        rdo_PhysicalDMAT.SelectedValue = "D"
        rdo_IPCalc.SelectedValue = "E"
        txt_RatingRemark.Text = ""
        rdo_Days.SelectedValue = "365"
        rdo_Days.SelectedValue = "F"
    End Sub

    Public Sub btn_Sumbit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Sumbit.Click
        SubmitData(False)

    End Sub

    Private Sub SetTempSecurityTable()
        Try
            'Dim dr As DataRow
            Dim dt As New DataTable
            dt.Columns.Add("FaxQuoteId")
            dt.Columns.Add("SecurityId")
            dt.Columns.Add("SecurityName")
            dt.Columns.Add("Date")
            dt.Columns.Add("Rate")
            dt.Columns.Add("LotSize")
            dt.Columns.Add("RatingRemark")
            dt.Columns.Add("Days")
            dt.Columns.Add("DaysOptions")
            dt.Columns.Add("PhysicalDMAT")
            dt.Columns.Add("IPCalc")
            dt.Columns.Add("RateActual")
            dt.Columns.Add("YXM")

            dt.Columns.Add("YTMAnn")
            dt.Columns.Add("YTCAnn")
            dt.Columns.Add("YTPAnn")
            dt.Columns.Add("YTMSemi")
            dt.Columns.Add("YTCSemi")
            dt.Columns.Add("YTPSemi")
            dt.Columns.Add("SecurityTypeId")
            dt.Columns.Add("SecurityTypeName")
            dt.Columns.Add("SPriority")
            dt.Columns.Add("OrderId")
            dt.Columns.Add("CreditRating")
            dt.Columns.Add("Semi_Ann_Flag")
            dt.Columns.Add("CombineIPMat")
            dt.Columns.Add("Rate_Actual_Flag")
            dt.Columns.Add("Equal_Actual_Flag")
            dt.Columns.Add("IntDays")
            dt.Columns.Add("FirstYrAllYr")

            Session("SelectedTempSecurityTable") = dt
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetTempSecurityTable", "Error in SetTempSecurityTable", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Add.Click

        FillTempSecurityTable()
        'If txt_Rate.Text = "" Or Val(txt_Rate.Text) = 0 Then
        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Rate cannot be zero or blank')", True)
        'End If
    End Sub


    'TO FIND ID FROM GRID VIEW
    Protected Sub dg_Selection_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dg_Selection.RowDataBound
        Try
            Dim IntSecurityId As Integer
            Dim strSecurityName As String
            If e.Row.RowType = ListItemType.AlternatingItem Or e.Row.RowType = ListItemType.Item Then
                Dim img As ImageButton
                ' Dim LinkSecName As LinkButton

                img = CType(e.Row.FindControl("img_Select"), ImageButton)
                'e.Row.ToolTip = Trim(TryCast(e.Row.DataItem, DataRowView).Row.Item("StockStatus") & "")
                IntSecurityId = Val(TryCast(e.Row.DataItem, DataRowView).Row.Item("SecurityId").ToString)
                strSecurityName = TryCast(e.Row.DataItem, DataRowView).Row.Item("SecurityName").ToString
                ' LinkSecName = CType(e.Row.FindControl("lnk_SecurityTypeName"), LinkButton)
                IntSecurityId = Val(TryCast(e.Row.DataItem, DataRowView).Row.Item("SecurityId").ToString)
                ' LinkSecName.Attributes.Add("onclick", "return ShowStockInfo('" & IntSecurityId & "');")
                'LinkSecName.Attributes.Add("onclick", "return SelectOption(this,'" & IntSecurityId & "','" & strSecurityName & "');")
                'e.Row.ToolTip = Trim(TryCast(e.Row.DataItem, DataRowView).Row.Item("StockStatus") & "")

                If TryCast(e.Row.DataItem, DataRowView).Row.Item("ColorId").ToString = "Q" Then
                    e.Row.BackColor = Drawing.Color.FromName("#93E069")
                    CType(e.Row.FindControl("txt_SecurityIssuer"), TextBox).BackColor = Drawing.Color.FromName("#93E069")
                    CType(e.Row.FindControl("txt_SecurityName"), TextBox).BackColor = Drawing.Color.FromName("#93E069")
                Else
                    e.Row.BackColor = Drawing.Color.FromName("#EAEBF4")
                    CType(e.Row.FindControl("txt_SecurityIssuer"), TextBox).BackColor = Drawing.Color.FromName("#EAEBF4")
                    CType(e.Row.FindControl("txt_SecurityName"), TextBox).BackColor = Drawing.Color.FromName("#EAEBF4")
                End If
                If intSelIndex = e.Row.RowIndex Then
                    e.Row.BackColor = Drawing.Color.FromName("#D1E4F8")
                    img.ImageUrl = "~/Images/images.jpg"
                    CType(e.Row.FindControl("txt_SecurityIssuer"), TextBox).BackColor = Drawing.Color.FromName("#D1E4F8")
                    CType(e.Row.FindControl("txt_SecurityName"), TextBox).BackColor = Drawing.Color.FromName("#D1E4F8")
                End If
                If Trim(Request.QueryString("rowIndex") & "") <> "" Then
                    img.Visible = False
                End If
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Selection_RowDataBound", "Error in dg_Selection_RowDataBound", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Show.Click
        Try
            ' Page.ClientScript.RegisterStartupScript(Me.GetType, "show", "OpenViewWindow()", True)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Public Shared Function mergeDTs(ByVal dt1 As DataTable, ByVal dt2 As DataTable) As DataTable
        Dim dtResult As DataTable = dt1.Clone()
        For Each dr As DataRow In dt1.Rows
            dtResult.Rows.Add(dr.ItemArray)
        Next
        For Each dr As DataRow In dt2.Rows
            dtResult.Rows.Add(dr.ItemArray)
        Next
        Return dtResult
    End Function

    Private Sub GetDataTableValues()
        Try
            Dim dt As DataTable
            Dim dr As DataRow
            Dim dtNew As DataTable
            dt = Session("TempSecurityTable")
            dtNew = dt.Copy()
         
            If dtNew.Rows.Count > 0 Then
                dtNew.DefaultView.Sort = "OrderId Asc,CreditRating Asc"
                dtNew = dtNew.DefaultView.ToTable()
                dr = dtNew.Rows(Hid_RowIndex.Value)
                Hid_SecurityId.Value = (dr("SecurityId") & "")
                Hid_SecurityName.Value = (dr("SecurityName") & "")
                txt_Date.Text = (dr.Item("Date") & "")
                txt_LotSize.Text = (dr.Item("LotSize") & "")
                txt_Rate.Text = (dr.Item("Rate") & "")
                txt_RatingRemark.Text = (dr.Item("RatingRemark") & "")
                rdo_Days.SelectedValue = (dr.Item("Days") & "")
                rdo_DaysOptions.SelectedValue = (dr.Item("DaysOptions") & "")
                rdo_IPCalc.SelectedValue = (dr.Item("IPCalc") & "")
                rdo_PhysicalDMAT.SelectedValue = (dr.Item("PhysicalDMAT") & "")
                rdo_RateActual.SelectedValue = (dr.Item("RateActual") & "")
                rdo_YXM.SelectedValue = (dr.Item("YXM") & "")
                Hid_SecurityTypeId.Value = (dr("SecurityTypeId") & "")
                Hid_OrderId.Value = (dr("OrderId") & "")
                Hid_CreditRating.Value = Val(dr("CreditRating") & "")
                Hid_Semi_Ann_Flag.Value = (dr("Semi_Ann_Flag") & "")
                Hid_Semi_Ann_Flag.Value = (dr("Semi_Ann_Flag") & "")
                Hid_CombineIPMat.Value = (dr("CombineIPMat") & "")
                Hid_Rate_Actual_Flag.Value = (dr("Rate_Actual_Flag") & "")
                Hid_Equal_Actual_Flag.Value = (dr("Equal_Actual_Flag") & "")
                Hid_IntDays.Value = (dr("IntDays") & "")
                Hid_FirstYrAllYr.Value = (dr("FirstYrAllYr") & "")
            End If
        
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetDataTableValues", "Error in GetDataTableValues", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_update.Click
        Try
            Dim dr As DataRow
            Dim dt As DataTable
            dt = Session("SelectedTempSecurityTable")
            If (Hid_SecurityId.Value = "") Then
                Dim msg As String = "Please Select Atlest One Security"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
                Exit Sub
            Else
                'dt.Rows.RemoveAt(Hid_RowIndex.Value)
                CalcYieldXIRR()
                dr = dt.NewRow()
                dr("FaxQuoteId") = Val(Hid_FaxQuoteId.Value)
                dr("SecurityId") = Hid_SecurityId.Value
                dr("SecurityName") = Hid_SecurityName.Value
                dr("Date") = txt_Date.Text
                dr("Rate") = txt_Rate.Text
                dr("LotSize") = txt_LotSize.Text
                dr("RatingRemark") = txt_RatingRemark.Text
                dr("Days") = rdo_Days.SelectedValue
                dr("DaysOptions") = rdo_DaysOptions.SelectedValue
                dr("PhysicalDMAT") = rdo_PhysicalDMAT.SelectedValue
                dr("IPCalc") = rdo_IPCalc.SelectedValue
                dr("RateActual") = rdo_RateActual.SelectedValue
                dr("YXM") = rdo_YXM.SelectedValue

                dr("YTMAnn") = (dblYTMAnn)
                dr("YTCAnn") = dblYTCAnn
                dr("YTPAnn") = dblYTPAnn
                dr("YTMSemi") = dblYTMSemi
                dr("YTCSemi") = dblYTCSemi
                dr("YTPSemi") = dblYTPSemi

                dr("Semi_Ann_Flag") = (Hid_Semi_Ann_Flag.Value)
                dr("CombineIPMat") = (Hid_CombineIPMat.Value)
                dr("Rate_Actual_Flag") = (Hid_Rate_Actual_Flag.Value)
                dr("Equal_Actual_Flag") = (Hid_Equal_Actual_Flag.Value)
                dr("IntDays") = (Hid_IntDays.Value)
                dr("FirstYrAllYr") = (Hid_FirstYrAllYr.Value)


                dt.Rows.Add(dr)
                Session("SelectedTempSecurityTable") = dt
                SubmitData(True)
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_update_Click", "Error in btn_update_Click", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub SubmitData(ByVal blnUpdate As Boolean)
        Try
            Dim dt1 As DataTable
            Dim dt2 As DataTable
            Dim dt3 As DataTable
            dt1 = Session("TempSecurityTable")
            If blnUpdate = True Then dt1.Rows.RemoveAt(Hid_RowIndex.Value)
            dt2 = Session("SelectedTempSecurityTable")

            dt3 = mergeDTs(dt1, dt2)
            Session("TempSecurityTable") = dt3
            Page.ClientScript.RegisterStartupScript(Me.GetType, "select", "Close('Submit');", True)

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SubmitData", "Error in SubmitData", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillOptions()
        Try
            ' Dim dsSecurity As DataSet
            Dim dtSecurity As DataTable
            Dim strSecurityNature As String = ""
            Dim I As Int32
            Dim datInfo As Date
            OpenConn()
            'Hid_SecurityId.Value = 
            SecurityId = val(Hid_SecurityId.Value)
            'dsSecurity = objCommon.GetDataSet(SqlDataSourceSecurity)
            dtSecurity = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", SecurityId, "SecurityId")
            For I = 0 To dtSecurity.Rows.Count - 1
                With dtSecurity.Rows(I)
                    SecurityId = Val(.Item("SecurityId") & "")
                    chrMaxActFlag = Trim(.Item("MaxActualFlag") & "")
                    strSecurityNature = Trim(.Item("NatureOfInstrument") & "")
                    Hid_Issuer.Value = Trim(.Item("SecurityIssuer") & "")
                    Hid_Security.Value = Trim(.Item("SecurityName") & "")

                    Hid_BookClosureDate.Value = IIf(Trim(.Item("BookClosureDate") & "") = "", Date.MinValue, .Item("BookClosureDate"))
                    Hid_InterestDate.Value = IIf(Trim(.Item("FirstInterestDate") & "") = "", Date.MinValue, .Item("FirstInterestDate"))
                    Hid_DMATBkDate.Value = IIf(Trim(.Item("DMATBookClosureDate") & "") = "", Date.MinValue, .Item("DMATBookClosureDate"))
                    Hid_Issue.Value = IIf(Trim(.Item("IssueDate") & "") = "", Date.MinValue, .Item("IssueDate"))
                    datInfo = IIf(Trim(.Item("SecurityInfoDate") & "") = "", Date.MinValue, .Item("SecurityInfoDate"))
                    Hid_GovernmentFlag.Value = Trim(.Item("GovernmentFlag") & "")
                    Hid_Frequency.Value = GetFrequency(Trim(.Item("FrequencyOfInterest") & ""))
                    Hid_FaceValue.Value = Val(.Item("FaceValue") & "")
                    Hid_RateAmtFlag.Value = Trim(.Item("CouponOn") & "")
                    FillSecurityInfoDetails(datInfo, Val(.Item("SecurityInfoAmt") & ""), Trim(.Item("TypeFlag") & ""))
                    If CheckSecurity() = False Or Val(Hid_Frequency.Value) = 0 Then
                        rdo_YXM.Items(0).Selected = False
                        rdo_YXM.Items(0).Enabled = False
                        rdo_YXM.Items(1).Selected = True
                    End If
                End With
            Next
            Dim strMatDate() As String
            If strSecurityNature = "P" Then
                Hid_CoupDate.Value += CStr(#12/31/9999#) & "!"
                Hid_CoupRate.Value += dblPepCoupRate & "!"
            Else
                strMatDate = Split(Hid_MatDate.Value, "!")
            End If
            If Trim(Hid_MatDate.Value) = "" Then
                rdo_YXM.Items(2).Enabled = False
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillOptions", "Error in FillOptions", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function CheckSecurity() As Boolean
        Try
            Dim I As Int16
            Dim strTypes() As String
            'Dim dsInfo As DataSet
            Dim dt As DataTable
            OpenConn()
            strTypes = Split("M!I!C!P", "!")
            For I = 0 To strTypes.Length - 1
                Hid_TypeFlagSec.Value = strTypes(I)
                'dsInfo = objCommon.GetDataSet(SqlDataSourceSecurityInfo)
                dt = objCommon.FillDataTable(sqlConn, "ID_Check_SecurityInfo", SecurityId, "SecurityId", , Hid_TypeFlagSec.Value, "TypeFlag")
                'dsInfo = objCommon.FillDetailsGrid(dg_Maturity, "ID_Check_SecurityInfo", "SecurityId", Val(ViewState("Id") & ""), "M", "TypeFlag")

                If Val(dt.Rows(0).Item(0) & "") > 1 Then
                    Return False
                End If
            Next
            Return True
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CheckSecurity", "Error in CheckSecurity", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Function
    Private Sub FillSecurityInfoDetails(ByVal InfoDate As Date, ByVal InfoAmt As Decimal, ByVal TypeFlag As String)
        Try
            Select Case TypeFlag
                Case "M"
                    Hid_MatDate.Value += InfoDate & "!"
                    Hid_MatAmt.Value += InfoAmt & "!"
                Case "I"
                    If InfoDate <> Date.MinValue Then
                        Hid_CoupDate.Value += InfoDate & "!"
                        Hid_CoupRate.Value += InfoAmt & "!"
                    Else
                        dblPepCoupRate = InfoAmt
                    End If
                Case "C"
                    Hid_CallDate.Value += InfoDate & "!"
                    Hid_CallAmt.Value += InfoAmt & "!"
                Case "P"
                    Hid_PutDate.Value += InfoDate & "!"
                    Hid_PutAmt.Value += InfoAmt & "!"
            End Select
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillSecurityInfoDetails", "Error in FillSecurityInfoDetails", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function GetFrequency(ByVal strFrequency As String) As Int16
        Try
            Select Case UCase(strFrequency)
                Case "Y"
                    Return 1
                Case "H"
                    Return 2
                Case "Q"
                    Return 4
                Case "M"
                    Return 12
                Case "N"
                    Return 0
            End Select
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetFrequency", "Error in GetFrequency", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Sub DisableRadio(ByVal strTypeFlag As String, ByVal rdo As RadioButton)
        Try
            'Dim dsInfo As DataSet
            Dim dtInfo As DataTable
            Hid_TypeFlagSec.Value = strTypeFlag
            OpenConn()
            'dsInfo = objCommon.GetDataSet(SqlDataSourceSecurityInfo)
            dtInfo = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", SecurityId, "SecurityId", , Hid_TypeFlagSec.Value, "TypeFlag")
            If dtInfo.Rows.Count > 0 Then
                If Val(dtInfo.Rows(0).Item(0) & "") = 0 Then
                    rdo.Enabled = True
                End If
            Else
                rdo.Enabled = False
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "DisableRadio", "Error in DisableRadio", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function FillDateArrays(ByVal strValue As String) As Date()
        Try
            Dim strDate() As String
            Dim arrDate() As Date
            Dim I As Int32

            strDate = Split(strValue, "!")
            ReDim arrDate(strDate.Length - 2)
            'arrDate(0) = Date.MinValue
            For I = 0 To strDate.Length - 2
                If strDate(I) <> "" Then arrDate(I) = CDate(strDate(I))
            Next
            Return arrDate
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillDateArrays", "Error in FillDateArrays", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            Return Nothing
        End Try
    End Function

    Private Function FillAmtArrays(ByVal strValue As String) As Double()
        Try
            Dim strDate() As String
            Dim arrDouble() As Double
            Dim I As Int32

            strDate = Split(strValue, "!")
            ReDim arrDouble(strDate.Length - 2)
            For I = 0 To strDate.Length - 2
                If strDate(I) <> "" Then arrDouble(I) = CDbl(strDate(I))
            Next
            Return arrDouble
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillAmtArrays", "Error in FillAmtArrays", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            Return Nothing
        End Try
    End Function
    Private Sub SetValues()
        Try
            With objCommon
                datYTM = .DateFormat(txt_Date.Text)
                datInterest = Hid_InterestDate.Value
                datBookClosure = Hid_BookClosureDate.Value
                blnNonGovernment = IIf(Hid_GovernmentFlag.Value = "N", True, False)
                blnRateActual = IIf(rdo_RateActual.SelectedValue = "R", True, False)
                blnDMAT = IIf(rdo_PhysicalDMAT.SelectedValue = "P", False, True)
                decFaceValue = .DecimalFormat(Val(Hid_FaceValue.Value))
                decRate = .DecimalFormat(Val(txt_Rate.Text))
                datIssue = Hid_Issue.Value
                datBookClosure = IIf(blnDMAT = True, Hid_DMATBkDate.Value, Hid_BookClosureDate.Value)
                intBKDiff = CalculateBookClosureDiff(datBookClosure, rdo_PhysicalDMAT.SelectedValue, datInterest, blnNonGovernment)
                'blnCompRate = chk_CompRate.Checked
                dblYTMAnn = 0
                dblYTCAnn = 0
                dblYTPAnn = 0
                dblYTMSemi = 0
                dblYTCSemi = 0
                dblYTPSemi = 0
            End With
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetValues", "Error in SetValues", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillYieldOptions()
        Try
            Dim datMaturity As Date
            Dim datCoupon As Date
            Dim datCall As Date
            Dim datPut As Date
            Dim decMaturityAmt As Decimal
            Dim decCouponRate As Decimal
            Dim decCallAmt As Decimal
            Dim decPutAmt As Decimal
            ' Dim dblResult As Double
            '  Dim strSemiAnnFlag As String

            With objCommon
                If MatDate.Length <= 0 Then
                    datMaturity = Date.MinValue
                Else
                    datMaturity = MatDate(0)
                End If
                If CallDate.Length <= 0 Then
                    datCall = Date.MinValue
                Else
                    datCall = CallDate(0)
                End If
                If PutDate.Length <= 0 Then
                    datPut = Date.MinValue
                Else
                    datPut = PutDate(0)
                End If
                If CoupDate.Length <= 0 Then
                    datCoupon = Date.MinValue
                Else
                    datCoupon = CoupDate(0)
                End If
                If MatAmt.Length <= 0 Then
                    decMaturityAmt = 0
                Else
                    decMaturityAmt = MatAmt(0)
                End If
                If CallAmt.Length <= 0 Then
                    decCallAmt = 0
                Else
                    decCallAmt = CallAmt(0)
                End If
                If PutAmt.Length <= 0 Then
                    decPutAmt = 0
                Else
                    decPutAmt = PutAmt(0)
                End If
                If CoupRate.Length <= 0 Then
                    decCouponRate = 0
                Else
                    decCouponRate = CoupRate(0)
                End If
            End With
            'decCouponRate = GetCouponRate(decCouponRate)
            'CalculateYield(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt, _
            '               datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""))

            GlobalFuns.CalculateYield(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, datMaturity, decMaturityAmt, _
                           datCoupon, decCouponRate, datCall, decCallAmt, datPut, decPutAmt, Val(Hid_Frequency.Value & ""), "Y", 0, "")


        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillYieldOptions", "Error in FillYieldOptions", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillXIRROptions()
        Try
            Dim dblMarketValue As Double
            Dim J As Int32
            Dim K As Int32 = 1
            Dim blnShow As Boolean = False

            If Val(Hid_Frequency.Value) = 0 Then
                ReDim XirrDate(23)
                ReDim XirrAmt(23)
                dblMarketValue = IIf(rdo_RateActual.SelectedValue = "R", Val(txt_Rate.Text) * Val(Hid_FaceValue.Value) / 100, Val(txt_Rate.Text))
                XirrAmt(0) = -dblMarketValue
                XirrDate(0) = objCommon.DateFormat(txt_Date.Text)
                For J = 0 To UBound(MatAmt)
                    If MatDate(J) > XirrDate(0) Then
                        blnShow = True
                        Exit For
                    End If
                Next
                For J = J To UBound(MatAmt)
                    XirrAmt(K) = MatAmt(J)
                    XirrDate(K) = MatDate(J)
                    K = K + 1
                Next

                K = 1
                blnShow = False
                ' ****************************************************************************************************
                ' Code for YTC(Call XIRR) calculation
                For J = 0 To UBound(CallAmt)
                    If CallDate(J) > XirrDate(0) Then
                        blnShow = True
                        Exit For
                    End If
                Next
                For J = J To UBound(CallAmt)
                    XirrAmt(K) = CallAmt(J)
                    XirrDate(K) = CallDate(J)
                    K = K + 1
                Next


                ' ****************************************************************************************************
                K = 1
                blnShow = False
                ' ****************************************************************************************************
                ' Code for YTP(Put XIRR) calculation
                For J = 0 To UBound(PutAmt)
                    If PutDate(J) > XirrDate(0) Then
                        blnShow = True
                        Exit For
                    End If
                Next
                For J = J To UBound(PutAmt)
                    XirrAmt(K) = PutAmt(J)
                    XirrDate(K) = PutDate(J)
                    K = K + 1
                Next

                ' ****************************************************************************************************

            Else
                If MatDate.Length > 0 Then
                    CntXirr = 0
                    CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, MatDate, MatAmt, _
                                  CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Val(Hid_Frequency.Value & ""), blnCompRate, rdo_IPCalc.SelectedValue, False, rdo_Days.SelectedValue, rdo_DaysOptions.SelectedValue)
                    GetXIRRResult(Val(Hid_Frequency.Value), dblYTMAnn, dblYTMSemi)
                End If
                ' ****************************************************************************************************
                ' Code for YTC(Call XIRR) calculation
                For J = 0 To UBound(CallAmt)
                    If CallDate(J) > datYTM Then Exit For
                Next
                If J <> UBound(CallDate) + 1 Then
                    CallDate(0) = CallDate(J)
                    CallAmt(0) = CallAmt(J)
                    ReDim Preserve CallDate(0)
                    ReDim Preserve CallAmt(0)
                    CntXirr = 0
                    CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, CallDate, CallAmt, _
                                  CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Hid_Frequency.Value, blnCompRate, rdo_IPCalc.SelectedValue, False, rdo_Days.SelectedValue, rdo_DaysOptions.SelectedValue)
                    GetXIRRResult(Val(Hid_Frequency.Value), dblYTCAnn, dblYTCSemi)

                End If
                ' ****************************************************************************************************

                ' ****************************************************************************************************
                ' Code for YTP(Put XIRR) calculation
                For J = 0 To UBound(PutAmt)
                    If PutDate(J) > datYTM Then Exit For
                Next
                If J <> UBound(PutDate) + 1 Then
                    PutDate(0) = PutDate(J)
                    PutAmt(0) = PutAmt(J)
                    ReDim Preserve PutDate(0)
                    ReDim Preserve PutAmt(0)
                    CntXirr = 0
                    CalculateXIRR(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual, PutDate, PutAmt, _
                                  CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Hid_Frequency.Value, blnCompRate, rdo_IPCalc.SelectedValue, False, rdo_Days.SelectedValue, rdo_DaysOptions.SelectedValue)
                    GetXIRRResult(Val(Hid_Frequency.Value), dblYTPAnn, dblYTPSemi)
                End If
                ' ****************************************************************************************************

            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillXIRROptions", "Error in FillXIRROptions", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub CalcYieldXIRR()
        Try
            FillOptions()
            Dim I As Single
            MatDate = FillDateArrays(Hid_MatDate.Value)
            MatAmt = FillAmtArrays(Hid_MatAmt.Value)
            CallDate = FillDateArrays(Hid_CallDate.Value)
            CallAmt = FillAmtArrays(Hid_CallAmt.Value)
            CoupDate = FillDateArrays(Hid_CoupDate.Value)
            CoupRate = FillAmtArrays(Hid_CoupRate.Value)
            PutDate = FillDateArrays(Hid_PutDate.Value)
            PutAmt = FillAmtArrays(Hid_PutAmt.Value)
            SetValues()
            If Hid_RateAmtFlag.Value = "A" Then
                For I = 0 To CoupRate.Length - 1
                    CoupRate(I) = (CoupRate(I) / decFaceValue) * 100
                Next
            End If
            Select Case rdo_YXM.SelectedValue
                Case "Y"
                    FillYieldOptions()
                Case "X"
                    FillXIRROptions()
            End Select
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalcYieldXIRR", "Error in CalcYieldXIRR", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            'sqlConn.Dispose()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class

