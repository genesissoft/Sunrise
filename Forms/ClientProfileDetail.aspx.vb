Imports System.Data
Imports System.Data.SqlClient

Partial Class Forms_ClientProfileDetail
    Inherits System.Web.UI.Page

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
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            If IsPostBack = False Then
                'objCommon.OpenConn() 'remove this

                vws_CustomerName.Columns.Add("CustomerName")
                vws_CustomerName.Columns.Add("CustomerTypeName as CustomerType ")
                'vws_CustomerName.Columns.Add("CategoryName as Category")

                vws_CustomerName.Columns.Add("CustomerPhone as PhoneNo")
                vws_CustomerName.Columns.Add("CustomerFax as FaxNo")
                vws_CustomerName.Columns.Add("PANNumber as PANNo")
                vws_CustomerName.Columns.Add("CM.CustomerId")

                vws_CustomerName.ColumnWidths.Add(200)
                'vws_CustomerName.ColumnWidths.Add(100)
                vws_CustomerName.ColumnWidths.Add(100)
                vws_CustomerName.ColumnWidths.Add(100)
                vws_CustomerName.ColumnWidths.Add(100)
                vws_CustomerName.ColumnWidths.Add(100)
                vws_CustomerName.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Protected Sub vws_CustomerName_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles vws_CustomerName.RowDataBound
        Try
            If e.Row.RowType <> DataControlRowType.Pager Then
                e.Row.Cells(8).Visible = False
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
