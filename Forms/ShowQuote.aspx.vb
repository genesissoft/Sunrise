Imports System.Data
Imports System.Data.SqlClient
Partial Class ShowQuote
    Inherits System.Web.UI.Page
    Dim dt3 As DataTable


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            If IsPostBack = False Then
                btn_Close.Attributes.Add("onclick", "return Close();")
                FillGrid()
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function FillGrid()
        Try
            Dim dt1 As DataTable
            Dim dt2 As DataTable
            Dim objSrh As New clsSearch
            dt1 = CType(Session("FaxQuoteTable"), DataTable)
            objSrh.SetGrid(dg_Quote, dt1)
            dg_Quote.DataSource = dt1
            dg_Quote.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    

    Protected Sub dg_Quote_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dg_Quote.RowDataBound
        Try
            Dim IntSecurityId As Integer
            'Dim strSecurityName As String
            If e.Row.RowType = ListItemType.AlternatingItem Or e.Row.RowType = ListItemType.Item Then
                Dim img As HtmlImage
                img = CType(e.Row.FindControl("img_Select"), HtmlImage)
                IntSecurityId = Val(TryCast(e.Row.DataItem, DataRowView).Row.Item("FaxQuoteId").ToString)
                img.Attributes.Add("onclick", "SelectOption(this,'" & IntSecurityId & "');")
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    'Private Sub DeleteGridRows(ByVal strSessionName As String, ByVal intIndex As Int16, ByVal grdView As GridView)
    '    Try
    '        Dim dt As DataTable
    '        dt3 = Session(FaxQuoteTable)
    '        dt3.Rows.RemoveAt(intIndex)
    '        Session(strSessionName) = dt3
    '        grdView.DataSource = dt3
    '        grdView.DataBind()
    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Sub

    'Protected Sub dg_SelectedSecurity_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dg_SelectedSecurity.RowDataBound
    '    Try
    '        If e.Row.RowType = DataControlRowType.DataRow Then
    '            Dim imgBtnEdit As ImageButton
    '            Dim imgBtnDelete As ImageButton

    '            'imgBtnEdit = CType(e.Row.FindControl("imgBtn_Edit"), ImageButton)
    '            imgBtnDelete = CType(e.Row.FindControl("imgBtn_Delete"), ImageButton)
    '            If e.Row.Cells(3).Text = "&nbsp;" Then
    '                'imgBtnEdit.Visible = False
    '                imgBtnDelete.Visible = False
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Sub

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


    Protected Sub btn_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close.Click

    End Sub

    Protected Sub btn_Sumbit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Sumbit.Click

        Page.ClientScript.RegisterStartupScript(Me.GetType, "select", "Submit();", True)

    End Sub


    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

    End Sub
End Class
