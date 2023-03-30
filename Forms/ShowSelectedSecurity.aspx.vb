Imports System.Data
Imports System.Data.SqlClient
Partial Class ShowSelectedSecurity
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
            If Session("SelectedTempSecurityTable") Is Nothing Then
                Dim msg As String = "Please Select Atlest One Security"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)

            Else
                dt2 = CType(Session("SelectedTempSecurityTable"), DataTable)
                objSrh.SetGrid(dg_SelectedSecurity, dt2)
                dg_SelectedSecurity.DataSource = dt2
                dg_SelectedSecurity.DataBind()

            End If


           
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    

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


    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

    End Sub
End Class
