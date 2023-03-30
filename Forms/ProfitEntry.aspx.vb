Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_ProfitEntry
    Inherits System.Web.UI.Page
    Dim dt As DataTable
    Dim objCommon As New clsCommonFuns
    Dim intSellDealId As Integer
    Dim sqlConn As SqlConnection


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        Try
            If IsPostBack = False Then
                btn_ChangeProfit.Attributes.Add("onclick", "return Validation();")
                If Val(Request.QueryString("SellDealSlipId") & "") <> 0 Then
                    Hid_DealSlipId.Value = Val(Request.QueryString("SellDealSlipId") & "")
                End If
                FillGrid()
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

    Private Sub FillGrid()
        Try
            OpenConn()
            If Val(Request.QueryString("SellDealSlipId") & "") <> 0 Then
                dt = objCommon.FillControl(dgProfit, sqlConn, "ID_Fill_UpdateProfitEntry", , , Val(Hid_DealSlipId.Value), "DealSlipId")
            Else
                dt = objCommon.FillControl(dgProfit, sqlConn, "ID_Fill_ProfitEntry")
                If IsPostBack = True Then
                    If dt.Rows.Count > 0 Then
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Next", "alert('Profit for another Sale is to be Assigned')", True)
                    End If
                End If
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub dgProfit_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgProfit.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                If intSellDealId = TryCast(e.Item.DataItem, DataRowView).Row.Item("SellDealSlipId") Then
                    e.Item.FindControl("txt_SellProfit").Visible = False
                    e.Item.FindControl("lbl_SellBranch").Visible = False
                    e.Item.FindControl("lbl_SellDealSlipNo").Visible = False
                    e.Item.FindControl("lbl_SellDealSlipId").Visible = False
                End If
                intSellDealId = TryCast(e.Item.DataItem, DataRowView).Row.Item("SellDealSlipId")
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_ChangeProfit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ChangeProfit.Click
        Try
            OpenConn()
            Dim sqlTrans As SqlTransaction
            sqlTrans = sqlConn.BeginTransaction
            If SaveDetails(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Profit", "Redirect('" & Hid_DealSlipNo.Value & "');", True)
            If Val(Request.QueryString("SellDealSlipId") & "") = 0 Then
                FillGrid()
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

        
    End Sub

    Private Function SaveDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try

            Dim sqlComm As New SqlCommand
            Dim dgItem As DataGridItem

            Dim decPurcProfitPercent As Decimal
            Dim decsellProfitPercent As Decimal
            Dim decHoProfitPercent As Decimal
            Dim decTotalProfit As Decimal
            Dim intSellDealSlipId As Integer
            Dim strDealslipNo As String
            Dim intPurchaseDealSlipId As Integer

            Dim i As Integer


            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_UPDATE_Profit"
            sqlComm.Connection = sqlConn

            For i = 0 To dgProfit.Items.Count - 1
                dgItem = dgProfit.Items(i)
                Hid_DealSlipNo.value = Trim(CType(dgItem.FindControl("lbl_SellDealSlipNo"), Label).Text)
                intPurchaseDealSlipId = Val(CType(dgItem.FindControl("lbl_purDealSlipId"), Label).Text)
                intSellDealSlipId = Val(CType(dgItem.FindControl("lbl_SellDealSlipId"), Label).Text)
                decsellProfitPercent = objCommon.DecimalFormat(CType(dgItem.FindControl("txt_SellProfit"), TextBox).Text)
                If i = 0 Then
                    Hid_SellPercent.Value = decsellProfitPercent
                End If

                decPurcProfitPercent = objCommon.DecimalFormat(CType(dgItem.FindControl("txt_PurProfit"), TextBox).Text)
                decHoProfitPercent = objCommon.DecimalFormat(CType(dgItem.FindControl("txt_HOProfit"), TextBox).Text)
                decTotalProfit = objCommon.DecimalFormat(CType(dgItem.FindControl("lbl_TotalProfit"), Label).Text)
                sqlComm.Parameters.Clear()
                objCommon.SetCommandParameters(sqlComm, "@SellDealSlipId", SqlDbType.Int, 4, "I", , , intSellDealSlipId)
                objCommon.SetCommandParameters(sqlComm, "@PurchaseDealSlipId", SqlDbType.Int, 4, "I", , , intPurchaseDealSlipId)
                objCommon.SetCommandParameters(sqlComm, "@PurcProfitPercent", SqlDbType.Decimal, 9, "I", , , decPurcProfitPercent)
                objCommon.SetCommandParameters(sqlComm, "@SellProfitPercent", SqlDbType.Decimal, 9, "I", , , (Hid_SellPercent.Value))
                objCommon.SetCommandParameters(sqlComm, "@HOProfitPercent", SqlDbType.Decimal, 9, "I", , , decHoProfitPercent)
                objCommon.SetCommandParameters(sqlComm, "@TotalProfit", SqlDbType.Decimal, 9, "I", , , decTotalProfit)
                sqlComm.ExecuteNonQuery()

            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
  

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            sqlConn.Dispose()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
