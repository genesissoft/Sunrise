Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_StockInfo
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
                If IsPostBack = False Then
                    Hid_ID.Value = Val(Request.QueryString("Id")).ToString()
                    FillGrid()
                End If
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function FillGrid()
        Try
            Dim dt As DataTable
            Dim objSrh As New clsSearch
            dt = objCommon.FillDetailsGrid(dg_StockDtl, "ID_FILL_StockInfo", "SecurityId", Val(Hid_ID.Value & ""))
            dg_StockDtl.DataSource = dt
            dg_StockDtl.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

End Class
