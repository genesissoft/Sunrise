
Partial Class Forms_FinancialDeliveryInformation
    Inherits System.Web.UI.Page

    Protected Sub vws_DematDelivery_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles vws_DematDelivery.RowDataBound
        Try
            Dim imgDelete As ImageButton
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim strDealslipNo As String
                strDealslipNo = Trim(CType(e.Row.DataItem, Data.DataRowView).Row.Item("DealSlipNo") & "")
                If Session("UserTypeId") <> 107 Then
                    If strDealslipNo.ToUpper.IndexOf("PP") <> -1 Or strDealslipNo.ToUpper.IndexOf("PS") <> -1 Then
                        e.Row.Visible = False
                    End If
                End If
                imgDelete = CType(e.Row.FindControl("imgBtn_Delete"), ImageButton)
                imgDelete.Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub

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
                vws_DematDelivery.Columns.Add("DealSlipNo")
                vws_DematDelivery.Columns.Add("TransType")
                vws_DematDelivery.Columns.Add("CustomerName")
                vws_DematDelivery.Columns.Add("CONVERT(VARCHAR,DealDate,103) As DealDate")
                vws_DematDelivery.Columns.Add("CONVERT(VARCHAR,SettmentDate,103) As SettmentDate")
                vws_DematDelivery.Columns.Add("FI.DealSlipId")
                vws_DematDelivery.ColumnWidths.Add(100)
                vws_DematDelivery.ColumnWidths.Add(50)
                vws_DematDelivery.ColumnWidths.Add(250)
                vws_DematDelivery.ColumnWidths.Add(50)
                vws_DematDelivery.ColumnWidths.Add(50)
                vws_DematDelivery.ColumnWidths.Add(50)

            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
       
    End Sub
End Class
