Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_SearchSong
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            FillBlankGrid()
        End If
    End Sub
    Private Sub FillBlankGrid()
        Try
            Dim DtInfoGrid As New DataTable
            DtInfoGrid.Columns.Add("Title", GetType(String))
            DtInfoGrid.Columns.Add("Artist", GetType(String))
            DtInfoGrid.Columns.Add("Album", GetType(String))
            DtInfoGrid.Columns.Add("Genre", GetType(String))
            Session("FinalcialTable") = DtInfoGrid
            dgProduct.DataSource = DtInfoGrid
            dgProduct.DataBind()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class
