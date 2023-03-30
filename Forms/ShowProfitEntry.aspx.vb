Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_ShowProfitEntry
    Inherits System.Web.UI.Page

    Dim dt As DataTable
    Dim objCommon As New clsCommonFuns
    Dim intSellDealId As Integer
    Dim sqlDa As New SqlDataAdapter
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


    Private Sub FillGrid(Optional ByVal intPageIndex As Int16 = 0)
        Try
            OpenConn()
            dt = objCommon.FillControl(dgProfit, sqlConn, "ID_Fill_ShowProfitEntry", , , Session("userTypeId"), "userTypeId")
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub dgProfit_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgProfit.ItemDataBound
        Try
            Dim imgButton As ImageButton
            Dim intSellDealSlipId As Integer
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                intSellDealSlipId = Val(CType(e.Item.FindControl("lbl_SellDealSlipId"), Label).Text)
                imgButton = CType(e.Item.FindControl("imgBtn_Edit"), ImageButton)
                imgButton.Attributes.Add("onclick", "return Update(" & e.Item.ItemIndex & "," & intSellDealSlipId & ");")
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dgProfit_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgProfit.PageIndexChanged
        'Try
        '    dgProfit.CurrentPageIndex = 0
        '    FillGrid(e.NewPageIndex)
        'Catch ex As Exception
        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        'End Try
    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            sqlConn.Dispose()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub

End Class
