
Partial Class Forms_ServiceTaxDetail
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

                vws_SerciceTax.Columns.Add("CONVERT(VARCHAR,FromDate,103) AS FromDate")
                vws_SerciceTax.Columns.Add("Tax")
                vws_SerciceTax.Columns.Add("Cess")
                vws_SerciceTax.Columns.Add("ECess")
                vws_SerciceTax.Columns.Add("TaxID")
                vws_SerciceTax.ColumnWidths.Add(150)
                vws_SerciceTax.ColumnWidths.Add(150)
                vws_SerciceTax.ColumnWidths.Add(150)
                vws_SerciceTax.ColumnWidths.Add(150)
                vws_SerciceTax.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub vws_SerciceTax_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles vws_SerciceTax.RowDataBound
        Dim imgBtnEdit As ImageButton
        If e.Row.RowType = DataControlRowType.DataRow Then
            imgBtnEdit = CType(e.Row.FindControl("imgBtn_Edit"), ImageButton)
            imgBtnEdit.Visible = False

        End If
    End Sub
End Class
