
Partial Class Forms_MakerView
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            MakerView1.UserId = Session("UserId")
            MakerView1.FindControl("btn_Submit").Visible = False
            'MakerView1.FindControl("dg_MC").width = 700

        Catch ex As Exception

        End Try
    End Sub
End Class
