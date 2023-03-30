
Partial Class Forms_CategoryDetail
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
            
                vws_CategoryDetail.Columns.Add("CategoryName")
                vws_CategoryDetail.Columns.Add("CustomerTypeName")
                'vws_CategoryDetail.Columns.Add("CM.CustomerTypeId")
                vws_CategoryDetail.Columns.Add("CategoryId")


                vws_CategoryDetail.ColumnWidths.Add(250)
                vws_CategoryDetail.ColumnWidths.Add(120)
                'vws_CategoryDetail.ColumnWidths.Add(50)
                vws_CategoryDetail.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
