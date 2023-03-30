
Partial Class Forms_TransferDPStockDetails
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
                vws_TransferDPStock.Columns.Add("CONVERT(Varchar,T.TransferDate,106) As [Transfer Date]")
                vws_TransferDPStock.Columns.Add("FDP.DPName As [From DP]")
                vws_TransferDPStock.Columns.Add("TDP1.BankName As [From SGL]")
                vws_TransferDPStock.Columns.Add("TDP.BankName As [To SGL]")
                vws_TransferDPStock.Columns.Add("FDP1.DPName As [To DP]")
                vws_TransferDPStock.Columns.Add("SM.SecurityName As [Security Name]")
                vws_TransferDPStock.Columns.Add("T.TransferQty As [Transfer Qty]")
                vws_TransferDPStock.Columns.Add("T.DPTransferId")

                vws_TransferDPStock.ColumnWidths.Add(100)
                vws_TransferDPStock.ColumnWidths.Add(120)
                vws_TransferDPStock.ColumnWidths.Add(120)
                vws_TransferDPStock.ColumnWidths.Add(120)
                vws_TransferDPStock.ColumnWidths.Add(120)
                vws_TransferDPStock.ColumnWidths.Add(250)
                vws_TransferDPStock.ColumnWidths.Add(80)
                vws_TransferDPStock.ColumnWidths.Add(1)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub vws_TransferDPStock_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles vws_TransferDPStock.RowDataBound
        Try

            Dim imgBtnUpdate As ImageButton
            If e.Row.RowType = DataControlRowType.DataRow Then
                imgBtnUpdate = CType(e.Row.FindControl("imgBtn_Edit"), ImageButton)
                imgBtnUpdate.Visible = False

            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
