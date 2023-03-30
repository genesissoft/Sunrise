
Partial Class Forms_SecurityList
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
                btn_AccruedInterest.Attributes.Add("onclick", "return ViewAccruedInterest();")
                btn_ShowSecurity.Attributes.Add("onclick", "return ViewSecurity();")
                btn_CurrentRate.Attributes.Add("onclick", "return ViewCurrentRate();")
                btn_YieldCalc.Attributes.Add("onclick", "return ViewYieldCalc();")
                vws_SecurityName.AddButton.Visible = False
                vws_SecurityName.Columns.Add("SecurityTypeName")
                vws_SecurityName.Columns.Add("SecurityIssuer")
                vws_SecurityName.Columns.Add("SecurityName")
                vws_SecurityName.Columns.Add("CONVERT(VARCHAR,MaturityDate,103) As MaturityDate")
                vws_SecurityName.Columns.Add("CONVERT(VARCHAR,CallDate,103) As CallDate")
                vws_SecurityName.Columns.Add("CouponRate")
                vws_SecurityName.Columns.Add("NSDLAcNumber AS ISINNo")
                vws_SecurityName.Columns.Add("SecurityId")
                vws_SecurityName.ColumnWidths.Add(250)
                vws_SecurityName.ColumnWidths.Add(250)
                vws_SecurityName.ColumnWidths.Add(250)
                vws_SecurityName.ColumnWidths.Add(50)
                vws_SecurityName.ColumnWidths.Add(50)
                vws_SecurityName.ColumnWidths.Add(50)
                vws_SecurityName.ColumnWidths.Add(50)
                vws_SecurityName.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
     
    End Sub
End Class
