
Partial Class Forms_ConfigurePassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Configuration1.ConnectionString = ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString
    End Sub
End Class
