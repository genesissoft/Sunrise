
Partial Class Forms_StockUpdateDetail
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
                vws_StockUpdate.Columns.Add("SecurityName As Security")
                vws_StockUpdate.Columns.Add("SecurityTypeName As Type")
                vws_StockUpdate.Columns.Add("NSDLAcNumber AS [ISIN No]")
                vws_StockUpdate.Columns.Add("CONVERT(VARCHAR,MaturityDate,103)AS MaturityDate")
                'vws_StockUpdate.Columns.Add("IPDates")
                vws_StockUpdate.Columns.Add("CONVERT(VARCHAR,CallDate,103)AS CallDate")
                'vws_StockUpdate.Columns.Add("CONVERT(VARCHAR,[dbo].[ID_GET_PutDate](SM.SecurityId),103)AS PutDate")

                vws_StockUpdate.Columns.Add("CONVERT(DECIMAL(18,2),((SU.FaceValue * SU.Multiple)/100000)) as QTM")
                vws_StockUpdate.Columns.Add("CONVERT(DECIMAL(18,2),(BookStock/100000)) As BookStock")

                vws_StockUpdate.Columns.Add("CONVERT(DECIMAL(18,4),(Rate)) AS  Rate")
                'vws_StockUpdate.Columns.Add("CONVERT(DECIMAL(18,2),(YTMAnn)) AS YTMAnn")
                'vws_StockUpdate.Columns.Add("CONVERT(DECIMAL(18,2),(YTMSemi)) AS YTMSemi")
                'vws_StockUpdate.Columns.Add("CONVERT(DECIMAL(18,2),(YTCAnn)) AS YTCAnn")
                vws_StockUpdate.Columns.Add("CONVERT(VARCHAR,StockDate,103)AS StockDate")
                vws_StockUpdate.Columns.Add("NameOfUser")
                'vws_StockUpdate.Columns.Add("CONVERT(DECIMAL(18,2),(BlockedStock/100000)) As BlockedStock")
                vws_StockUpdate.Columns.Add("StockUpdtId")

                vws_StockUpdate.ColumnWidths.Add(250)
                vws_StockUpdate.ColumnWidths.Add(150)
                vws_StockUpdate.ColumnWidths.Add(150)
                vws_StockUpdate.ColumnWidths.Add(100)
                'vws_StockUpdate.ColumnWidths.Add(100)
                'vws_StockUpdate.ColumnWidths.Add(100)
                'vws_StockUpdate.ColumnWidths.Add(100)
                'vws_StockUpdate.ColumnWidths.Add(100)
                'vws_StockUpdate.ColumnWidths.Add(100)
                vws_StockUpdate.ColumnWidths.Add(100)
                vws_StockUpdate.ColumnWidths.Add(50)
                vws_StockUpdate.ColumnWidths.Add(50)
                vws_StockUpdate.ColumnWidths.Add(50)
                vws_StockUpdate.ColumnWidths.Add(50)
                vws_StockUpdate.ColumnWidths.Add(50)
                vws_StockUpdate.ColumnWidths.Add(50)
                vws_StockUpdate.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
