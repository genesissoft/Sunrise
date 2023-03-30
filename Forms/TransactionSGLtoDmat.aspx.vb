Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_TransactionSGLtoDmat
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim arrIssueDetailIds() As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            FillBlankGrid()
        End If
    End Sub
    Private Sub FillBlankGrid()
        Try
            Dim DtInfoGrid As New DataTable
            DtInfoGrid.Columns.Add("PaymentDate", GetType(String))

            Session("FinalcialTable") = DtInfoGrid
            dg_Dematdetails.DataSource = DtInfoGrid
            dg_Dematdetails.DataBind()
        Catch ex As Exception

        End Try
    End Sub
End Class
