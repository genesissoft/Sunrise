
Partial Class Forms_SecurityTypeDetail
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
                'objCommon.OpenConn()
                vws_SecurityTypeName.Columns.Add("SecurityTypeName")
                'vws_SecurityTypeName.Columns.Add("CASE TypeFlag WHEN 'N' THEN 'NON GOVERNMENT' ELSE 'GOVERNMENT' END AS TypeFlag")
                vws_SecurityTypeName.Columns.Add("TypeFlag")
                vws_SecurityTypeName.Columns.Add("OrderId")
                vws_SecurityTypeName.Columns.Add("SecurityTypeId")
                vws_SecurityTypeName.ColumnWidths.Add(250)
                vws_SecurityTypeName.ColumnWidths.Add(200)
                vws_SecurityTypeName.ColumnWidths.Add(100)
                vws_SecurityTypeName.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
