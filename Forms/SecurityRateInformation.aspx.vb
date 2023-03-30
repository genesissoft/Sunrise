Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class Forms_SecurityRateInformation
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
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
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            'objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            If IsPostBack = False Then

                intCondCnt = 0

                SetControls()
                EnsureChildControls()
                FillGrid()
                SetLinkButtons()
            Else

                If blnShowAll = True Then
                    Dim txtBox As TextBox = pnl_Search.FindControl("txt_Search0")
                    If txtBox IsNot Nothing Then txtBox.Text = ""
                End If
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub SetControls()
        Columns.Add("STM.SecurityTypeName")
        Columns.Add("Abbreviation")
        Columns.Add("SecurityIssuer")
        Columns.Add("SecurityName")
        Columns.Add("MaturityDate")
        Columns.Add("CallDate")
        Columns.Add("CouponRate")
        Columns.Add("SecurityId")
        ColumnWidths.Add("100")
        ColumnWidths.Add("100")
        ColumnWidths.Add("100")
        ColumnWidths.Add("100")
        ColumnWidths.Add("100")
        ColumnWidths.Add("100")
        ColumnWidths.Add("100")
        ColumnWidths.Add("50")

        Hid_ColList.Value = objSearch.ConvertListToString(Columns, ",")
        Hid_ColWidths.Value = objSearch.ConvertListToString(ColumnWidths, ",")
        Hid_ColText.Value = objSearch.ConvertListToString(Columns, "!")
    End Sub
   
    'Private Sub FillBlankDPGrids()
    '    Try
    '        Dim dtSelectionGrid As New DataTable
    '        dtSelectionGrid.Columns.Add("Security Name", GetType(String))
    '        dtSelectionGrid.Columns.Add("Rate", GetType(String))
    '        dtSelectionGrid.Columns.Add("Quantity", GetType(String))
    '        dtSelectionGrid.Columns.Add("RatingRemarks", GetType(String))
    '        dtSelectionGrid.Columns.Add("Yield_XIRR", GetType(String))
    '        dtSelectionGrid.Columns.Add("Physical_DMAT_SGL", GetType(String))
    '        dtSelectionGrid.Columns.Add("Rate_Actual", GetType(String))
    '        dtSelectionGrid.Columns.Add("Equal_Actual", GetType(String))
    '        dtSelectionGrid.Columns.Add("365_366", GetType(String))
    '        dtSelectionGrid.Columns.Add("Years", GetType(String))
    '        dtSelectionGrid.Columns.Add("YTMSemi", GetType(String))
    '        dtSelectionGrid.Columns.Add("PutCallDate", GetType(String))
    '        dtSelectionGrid.Columns.Add("CurrentYield", GetType(String))
    '        Session("SelectionTable") = dtSelectionGrid
    '        dg_Selection.DataSource = dtSelectionGrid
    '        dg_Selection.DataBind()
    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Sub

    Private Sub FillSecurityGrid()
        Try
            Dim dt As DataTable
            dt = objCommon.FillDetailsGrid(dg_Selection, "ID_FILL_SecurityFaxGenaration", "SecurityId", Val(ViewState("Id") & ""))
            Session("SecurityTable") = dt
            'dg_Selection.CurrentPageIndex = intPageIndex
            dg_Selection.DataSource = dt
            dg_Selection.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "", Optional ByVal intPageIndex As Int16 = 0)
        Try
            Dim objSearch As New clsSearch
            Dim strCondFld As String = ""
            Dim intCondVal As Int16 = 0
            dtGrid = objSearch.FillGrid(dg_Selection, cbo, txt, Hid_ColList.Value, 2, strCondFld, intCondVal, , "ID_SEARCH_SecurityFields", strSort, intPageIndex)
            FillFieldCombo(cbo, dtGrid)
        Catch ex As Exception
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
                For J As Int16 = 0 To dt.Columns.Count - 3
                    If arrCols(J).ToUpper.IndexOf("AS") <> -1 Then
                        arrCols(J) = arrCols(J).Substring(0, arrCols(J).ToUpper.IndexOf("AS"))
                    End If
                    cbo_Name.Items.Add(New ListItem(dt.Columns(J).ColumnName, arrCols(J)))
                Next
            Next
        Catch ex As Exception
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
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Search.Click
        lnk_And_Click(sender, e)
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
       
    End Sub
End Class
