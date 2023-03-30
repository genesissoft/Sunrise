Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_DealSlipDetail
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

                vws_DealSlip.Columns.Add("DealSlipNo As No")
                vws_DealSlip.Columns.Add("DSE.FaceValue*DSE.FaceValueMultiple AS FaceValue")
                vws_DealSlip.Columns.Add("convert(varchar,DealDate,103) as DealDate")
                vws_DealSlip.Columns.Add("convert(varchar,SettmentDate,103)  as SetttDate")
                vws_DealSlip.Columns.Add("Rate")
                'vws_DealSlip.Columns.Add("Financialdealtype")

                vws_DealSlip.Columns.Add("CASE TransType WHEN 'P' THEN 'PUR' ELSE 'SELL' END AS Type")

                vws_DealSlip.Columns.Add("SecurityName")
                vws_DealSlip.Columns.Add("CustomerName")

                vws_DealSlip.Columns.Add("DealDone")
                vws_DealSlip.Columns.Add("CMDealConfFlag As C")
                vws_DealSlip.Columns.Add("DealSlipID")

                vws_DealSlip.ColumnWidths.Add(20)
                vws_DealSlip.ColumnWidths.Add(150)
                vws_DealSlip.ColumnWidths.Add(100)
                vws_DealSlip.ColumnWidths.Add(100)
                vws_DealSlip.ColumnWidths.Add(20)
                vws_DealSlip.ColumnWidths.Add(20)
                vws_DealSlip.ColumnWidths.Add(300)
                vws_DealSlip.ColumnWidths.Add(300)
                vws_DealSlip.ColumnWidths.Add(20)
                vws_DealSlip.ColumnWidths.Add(20)
                vws_DealSlip.ColumnWidths.Add(20)
                vws_DealSlip.ColumnWidths.Add(50)


            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    
    Protected Sub vws_DealSlip_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles vws_DealSlip.RowDataBound
        Try
            Dim imgBtnDelete As ImageButton
            Dim imgBtnUpdate As ImageButton
            Dim CMDealConfDone As Boolean
            If e.Row.RowType = DataControlRowType.DataRow Then
                If IsDBNull(TryCast(e.Row.DataItem, DataRowView).Row.Item("DealDone")) Then Exit Sub
                If IsDBNull(TryCast(e.Row.DataItem, DataRowView).Row.Item("C")) Then Exit Sub
                Dim DealDone As Boolean = TryCast(e.Row.DataItem, DataRowView).Row.Item("DealDone")
                Dim intDealslipid As Integer = TryCast(e.Row.DataItem, DataRowView).Row.Item("DealSlipID")
                CMDealConfDone = TryCast(e.Row.DataItem, DataRowView).Row.Item("C")

                If DealDone = True Then
                    imgBtnDelete = CType(e.Row.FindControl("imgBtn_Delete"), ImageButton)
                    imgBtnUpdate = CType(e.Row.FindControl("imgBtn_Edit"), ImageButton)
                    imgBtnDelete.Visible = False
                    e.Row.Cells(13).Visible = False
                End If
                If CMDealConfDone = True Then
                    imgBtnDelete = CType(e.Row.FindControl("imgBtn_Delete"), ImageButton)
                    imgBtnDelete.Attributes.Clear()
                    imgBtnDelete.Attributes.Add("onclick", "return ChkDel('" & e.Row.RowIndex & "','" & intDealslipid & "');")
                End If
                e.Row.Cells(13).Visible = False
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
