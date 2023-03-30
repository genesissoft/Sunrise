
Partial Class Forms_BankDetail
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
                vws_BankName.Columns.Add("BankName")
                vws_BankName.Columns.Add("Branch")
                vws_BankName.Columns.Add("Location")
                vws_BankName.Columns.Add("ContactPerson AS RelationshipManager")
                vws_BankName.Columns.Add("MobNo")
                vws_BankName.Columns.Add("PhoneNo")
                vws_BankName.Columns.Add("AccountNumber")
                vws_BankName.Columns.Add("BankId")
                vws_BankName.ColumnWidths.Add(250)
                vws_BankName.ColumnWidths.Add(100)
                vws_BankName.ColumnWidths.Add(150)
                vws_BankName.ColumnWidths.Add(200)
                vws_BankName.ColumnWidths.Add(50)
                vws_BankName.ColumnWidths.Add(50)
                vws_BankName.ColumnWidths.Add(50)
                vws_BankName.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
