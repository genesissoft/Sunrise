Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_ViewCostMemoStatus
    Inherits System.Web.UI.Page
    Dim sqlComm As New SqlCommand
    Dim objCommon As New clsCommonFuns
    Dim dsmenu As DataSet
    Dim dsDPDetails As DataSet
    Dim newTable As DataTable
    Dim DpDetailsTable As DataTable
    Dim trmenu As DataRow
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
            If IsPostBack = False Then
                txt_ForDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
                txt_ForDate.Attributes.Add("onkeypress", "OnlyDate();")
                txt_ForDate.Attributes.Add("onblur", "CheckDate(this,false);")
                '  btn_Save.Attributes.Add("onclick", "return ValidateDate();")
                btn_Show_Click(sender, e)
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


    Protected Sub btn_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Show.Click
        FillCostMemoStatus("DealSlipNo", 0)
    End Sub

    Private Sub FillCostMemoStatus(ByVal strSort As String, Optional ByVal intPageIndex As Int16 = 0)
        Try
            Dim sqlda As New SqlDataAdapter
            Dim sqldt As New DataTable
            Dim sqldv As New DataView
            OpenConn()
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "FA_FILL_CostMemoStatus"
            sqlComm.Parameters.Clear()
            If Trim(txt_ForDate.Text) <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@ForDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ForDate.Text))
            End If
            If (Session("UserTypeId") = "41") Then
                objCommon.SetCommandParameters(sqlComm, "@BranchId", SqlDbType.Int, 4, "I", , , Val(Session("BranchId") & ""))
            End If
            If (Session("UserTypeId") = "39") Then
                objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId") & ""))
            End If
            objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(sqldt)
            sqldv = sqldt.DefaultView
            sqldv.Sort = strSort
            'dg_dme.CurrentPageIndex = intPageIndex
            dg_dme.DataSource = sqldv
            dg_dme.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    'Protected Sub dg_dme_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_dme.ItemDataBound

    '    If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
    '        e.Item.ToolTip = Trim(TryCast(e.Item.DataItem, DataRowView).Row.Item("StockStatus") & "")
    '    End If

    'End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            sqlConn.Dispose()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub
End Class
