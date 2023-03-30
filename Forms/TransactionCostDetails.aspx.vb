
Partial Class Forms_TransactionCostDetails
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns

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
                vws_TransactionCost.Columns.Add("CONVERT(VARCHAR,FromDate,103)AS FromDate")
                vws_TransactionCost.Columns.Add("CASE DeliveryMode WHEN 'D' THEN 'Demat' ELSE 'SGL' END AS DeliveryMode")
                vws_TransactionCost.Columns.Add("CASE DeliveryMode WHEN 'D' THEN B.BankName ELSE s.BankName END as BankName")
                vws_TransactionCost.Columns.Add("CASE SecurityType WHEN 'N' THEN 'Non-SLR' ELSE 'SLR' END as SecurityType")
                vws_TransactionCost.Columns.Add("AccountNumber As DmatACCNo")
                vws_TransactionCost.Columns.Add("AccountNo As SGLACNo")
                vws_TransactionCost.Columns.Add("HCostAmount")
                vws_TransactionCost.Columns.Add("IntraDayAmount")
                vws_TransactionCost.Columns.Add("TransactionCostId")
                vws_TransactionCost.ColumnWidths.Add(100)
                vws_TransactionCost.ColumnWidths.Add(100)
                vws_TransactionCost.ColumnWidths.Add(100)
                vws_TransactionCost.ColumnWidths.Add(150)
                vws_TransactionCost.ColumnWidths.Add(100)
                vws_TransactionCost.ColumnWidths.Add(100)
                vws_TransactionCost.ColumnWidths.Add(100)
                vws_TransactionCost.ColumnWidths.Add(100)
                vws_TransactionCost.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
