
Partial Class Forms_BranchDetail
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
            ''Response.Filter = New WhitespaceFilter(Response.Filter)
            If IsPostBack = False Then
                'objCommon.OpenConn() 'remove this
                vws_BranchName.Columns.Add("BranchName")
                vws_BranchName.Columns.Add("BranchPhoneNo")
                vws_BranchName.Columns.Add("FaxNo")
                vws_BranchName.Columns.Add("BranchId")

                vws_BranchName.ColumnWidths.Add(250)
                vws_BranchName.ColumnWidths.Add(150)
                vws_BranchName.ColumnWidths.Add(150)
                vws_BranchName.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
