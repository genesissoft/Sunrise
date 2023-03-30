Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Partial Class Forms_SearchCRMEntry
    Inherits System.Web.UI.Page
    Dim intGridCols As Int16
    Dim ProcName As String
    Public tblname As String
    Dim PgName As String = "$Search$"
    Dim objUtil As New Util

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        HttpContext.Current.Response.Cache.SetNoServerCaching()
        HttpContext.Current.Response.Cache.SetNoStore()
        dg_Search.Attributes.Add("bordercolor", "#BBC294")
        ProcName = Trim(Request.QueryString("ProcName") & "")
        tblname = Trim(Request.QueryString("TableName") & "")

        If IsPostBack = False Then
            Try
                Hid_DefaultSort.Value = Trim(Request.QueryString("SelectedFieldName") & "") & " ASC"
                SetAttributes()
                FillGrid(Hid_DefaultSort.Value)
            Catch ex As Exception
                objUtil.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex)
                Response.Write(ex.Message)
            End Try
        End If
    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "", Optional ByVal intPageIndex As Int16 = 0, Optional ByVal StrSearch As String = "")
        Try
            Dim dt As New DataTable
            Dim dv As DataView
            Dim ObjClass As New ClsCommonCRM
            If (tblname.ToString() = "IssuerMaster") Then
                dt = ObjClass.FillDataTableIssureMaster(ProcName, StrSearch)
            Else
                dt = ObjClass.FillDataTable(ProcName, StrSearch)
            End If

            If tblname = "IssuerMaster" Then
                dg_Search.Columns(1).HeaderText = "IssuerName"
            ElseIf tblname = "InvestorMaster" Then
                dg_Search.Columns(1).HeaderText = "InvestorName"
            ElseIf tblname = "SecurityMaster" Then
                dg_Search.Columns(1).HeaderText = "SecurityName"
            Else
                dg_Search.Columns(1).HeaderText = "CustomerName"
            End If

            dv = dt.DefaultView
            If dt.Rows.Count > 0 Then
                dv.Sort = strSort
                If strSort <> "" Then dv.Sort = strSort
            End If
            dg_Search.CurrentPageIndex = intPageIndex
            dg_Search.DataSource = dv.ToTable()
            dg_Search.DataBind()
            intGridCols = dv.ToTable().Columns.Count
            Hid_ColCount.Value = intGridCols - 1
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillGrid", "Error in FillGrid", "", ex)
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub SetAttributes()
        Try
            btn_Close.Attributes.Add("onclick", "return Close();")
            btn_Submit.Attributes.Add("onclick", "return Submit();")
            btn_Search.Attributes.Add("onclick", "return Validation();")
        Catch ex As Exception
            Response.Write(ex.Message)
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Search.Click
        Try
            Dim StrSearch As String = ""
            If tblname = "IssuerMaster" Then
                StrSearch = " AND IssuerName LIKE '" + Trim(txt_Name.Text) + "%'"
            ElseIf tblname = "InvestorMaster" Then
                StrSearch = " AND InvestorName LIKE '" + Trim(txt_Name.Text) + "%'"
            ElseIf tblname = "SecurityMaster" Then
                StrSearch = " Where SecurityName LIKE '" + Trim(txt_Name.Text) + "%'"
            Else
                StrSearch = " AND CustomerName LIKE '" + Trim(txt_Name.Text) + "%'"
            End If
            FillGrid(Hid_DefaultSort.Value, 0, StrSearch)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_Search_Click", "Error in btn_Search_Click", "", ex)
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub dg_Search_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_Search.ItemDataBound
        Try
            Dim _intId As Integer
            Dim imgbtn As Image

            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                _intId = Val(CType(e.Item.FindControl("lbl_Id"), Label).Text)
                imgbtn = CType(e.Item.FindControl("img_Select"), Image)
                imgbtn.Attributes.Add("onclick", "return SelectRow(this,'" & _intId & "')")
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Search_ItemDataBound", "Error in dg_Search_ItemDataBound", "", ex)
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub dg_Search_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dg_Search.PageIndexChanged
        Try
            FillGrid(Hid_DefaultSort.Value, e.NewPageIndex, "")
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Search_PageIndexChanged", "Error in dg_Search_PageIndexChanged", "", ex)
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_ShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ShowAll.Click
        Try
            txt_Name.Text = ""
            FillGrid(Hid_DefaultSort.Value)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_ShowAll_Click", "Error in btn_ShowAll_Click", "", ex)
            Response.Write(ex.Message)
        End Try
    End Sub
End Class
