
Partial Class Forms_BrokerDetail
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
                vws_Broker.Columns.Add("BrokerName")
                vws_Broker.Columns.Add("BrokerCode")
                vws_Broker.Columns.Add("NSEBrokerCode")
                vws_Broker.Columns.Add("BSEBrokerCode")
                'vws_Broker.Columns.Add("AD.AdvisoryName")

                vws_Broker.Columns.Add("BM.BrokerId")
                vws_Broker.ColumnWidths.Add(250)
                vws_Broker.ColumnWidths.Add(100)
                vws_Broker.ColumnWidths.Add(100)
                vws_Broker.ColumnWidths.Add(100)
                'vws_Broker.ColumnWidths.Add(250)

                vws_Broker.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

  
End Class
 

