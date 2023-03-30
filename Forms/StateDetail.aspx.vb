
Partial Class Forms_StateDetail
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
            'Dim filename As String = Me.Page.ToString().Substring(10, Me.Page.ToString().Substring(10).Length - 5) & ".aspx"
            'If objCommon.CheckUserAccess(Val(Session("UserTypeId") & ""), filename) = False Then
            '    Response.Redirect("Login.aspx")
            'End If
            If IsPostBack = False Then
                'objCommon.OpenConn()
                vws_StateName.Columns.Add("StateName")
                'vws_StateName.Columns.Add("StampDuty")
                vws_StateName.Columns.Add("StateId")
                vws_StateName.ColumnWidths.Add(250)
                'vws_StateName.ColumnWidths.Add(100)
                vws_StateName.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
