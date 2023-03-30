
Partial Class Forms_UserTypeDetail
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

                vws_UserTypeName.Columns.Add("UserTypeName")
                'vws_UserTypeName.Columns.Add("UserTypeSection")
                'vws_UserTypeName.Columns.Add("Checker")
                vws_UserTypeName.Columns.Add("UserTypeId")

                vws_UserTypeName.ColumnWidths.Add(300)
                'vws_UserTypeName.ColumnWidths.Add(150)
                'vws_UserTypeName.ColumnWidths.Add(150)
                vws_UserTypeName.ColumnWidths.Add(50)
                vws_UserTypeName.ColumnWidths.Add(50)


            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub vws_UserTypeName_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles vws_UserTypeName.RowDataBound
        Try
            Dim imgBtnDelete As ImageButton
            If e.Row.RowType = DataControlRowType.DataRow Then
                imgBtnDelete = CType(e.Row.FindControl("imgBtn_Delete"), ImageButton)
                imgBtnDelete.Visible = False
                'vws_UserTypeName.AddButton.Visible = False
            End If


        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
