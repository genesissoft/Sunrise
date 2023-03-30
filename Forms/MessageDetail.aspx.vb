
Partial Class Forms_MessageDetail
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

            If IsPostBack = False Then
                'objCommon.OpenConn()
                vws_Message.Columns.Add("Message")
                vws_Message.Columns.Add("CONVERT(VARCHAR,RegisteredDate,103)As RegisteredDate")
                vws_Message.Columns.Add("CONVERT(VARCHAR,EndDate,103)as EndDate")
                vws_Message.Columns.Add("MessageId")
                vws_Message.ColumnWidths.Add(200)
                vws_Message.ColumnWidths.Add(150)
                vws_Message.ColumnWidths.Add(150)
                vws_Message.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
