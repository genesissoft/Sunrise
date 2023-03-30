Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_QuoteDetails
    Inherits System.Web.UI.Page
    Dim sqlConn As SqlConnection
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
            'Response.Filter = New WhitespaceFilter(Response.Filter)
            If IsPostBack = False Then
                'objCommon.OpenConn() 'remove this
                'Dim dg As DataGrid = vws_QuoteEntry.FindControl("")

                vws_QuoteEntry.Columns.Add("SecurityName")
                vws_QuoteEntry.Columns.Add("CONVERT(VARCHAR,QuoteDate,103)AS QuoteDate")
                vws_QuoteEntry.Columns.Add("Rate")
                vws_QuoteEntry.Columns.Add("(Q.FaceValue * Q.Multiple) as FaceValue")
                vws_QuoteEntry.Columns.Add("ISNULL(CM.CustomerName,TM.CustomerName) As CustomerName")
                'vws_QuoteEntry.Columns.Add("CASE QuoteType WHEN 'S' THEN 'SELL' WHEN 'P' THEN 'PURCHASE' END AS QuoteType")
                'vws_QuoteEntry.Columns.Add("CASE QuoteType WHEN 'S' THEN 'SELL' ELSE 'PURCHASE' END AS QuoteType")
                vws_QuoteEntry.Columns.Add("NameOfUser As DealerName")
                vws_QuoteEntry.Columns.Add("QuoteId")

                vws_QuoteEntry.ColumnWidths.Add(350)
                vws_QuoteEntry.ColumnWidths.Add(100)
                vws_QuoteEntry.ColumnWidths.Add(100)
                vws_QuoteEntry.ColumnWidths.Add(100)
                vws_QuoteEntry.ColumnWidths.Add(350)
                vws_QuoteEntry.ColumnWidths.Add(100)
                'vws_QuoteEntry.ColumnWidths.Add(100)
                vws_QuoteEntry.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub OpenConn()
        If sqlConn Is Nothing Then
            sqlConn = New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
            sqlConn.Open()
        ElseIf sqlConn.State = ConnectionState.Closed Then
            sqlConn.ConnectionString = ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString()
            sqlConn.Open()
        End If
    End Sub

    Private Sub CloseConn()
        If sqlConn Is Nothing Then Exit Sub
        If sqlConn.State = ConnectionState.Open Then sqlConn.Close()
    End Sub
    Protected Sub vws_QuoteEntry_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles vws_QuoteEntry.RowDataBound
        Try
            Dim imgBtnDelete As ImageButton
            Dim imgBtnUpdate As ImageButton
            If e.Row.RowType = DataControlRowType.DataRow Then
                If IsDBNull(TryCast(e.Row.DataItem, DataRowView).Row.Item("DealerName")) Then Exit Sub
                Dim strName As String = TryCast(e.Row.DataItem, DataRowView).Row.Item("DealerName")
                If strName <> Trim(Session("NameOfUser") & "") Then
                    imgBtnDelete = CType(e.Row.FindControl("imgBtn_Delete"), ImageButton)
                    imgBtnUpdate = CType(e.Row.FindControl("imgBtn_Edit"), ImageButton)
                    imgBtnDelete.Visible = False
                    'imgBtnUpdate.Visible = False
                End If
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
