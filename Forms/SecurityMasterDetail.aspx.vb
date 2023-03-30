
Partial Class Forms_SecurityMasterDetail
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
                vws_SecurityMaster.Columns.Add("SecurityName")
                vws_SecurityMaster.Columns.Add("SecurityTypeName")
                vws_SecurityMaster.Columns.Add("SecurityIssuer")
                vws_SecurityMaster.Columns.Add("NSDLAcNumber AS [ISIN No]")
                vws_SecurityMaster.Columns.Add("CONVERT(VARCHAR,CallDate,103)AS CallDate")
                vws_SecurityMaster.Columns.Add("CONVERT(VARCHAR,MaturityDate,103)AS MaturityDate")

                vws_SecurityMaster.Columns.Add("YEAR(MaturityDate) AS MaturityYear")
                'vws_SecurityMaster.Columns.Add("LEFT(datename(mm,(MaturityDate)),3) + '-' + Convert(varchar,year(MaturityDate))AS MaturityMthYr")
                'vws_SecurityMaster.Columns.Add("CONVERT(VARCHAR,CallDate,103) AS Calldate")
                vws_SecurityMaster.Columns.Add("IPDates")


                vws_SecurityMaster.Columns.Add("SecurityId")

                vws_SecurityMaster.ColumnWidths.Add(250)
                vws_SecurityMaster.ColumnWidths.Add(150)
                vws_SecurityMaster.ColumnWidths.Add(150)
                vws_SecurityMaster.ColumnWidths.Add(100)
                vws_SecurityMaster.ColumnWidths.Add(100)
                vws_SecurityMaster.ColumnWidths.Add(100)
                'vws_SecurityMaster.ColumnWidths.Add(100)
                vws_SecurityMaster.ColumnWidths.Add(100)
                vws_SecurityMaster.ColumnWidths.Add(100)

                vws_SecurityMaster.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
