Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports RKLib.ExportData
Partial Class Forms_ViewStockDetails
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
            If IsPostBack = False Then
                SetAttributes()
                btn_Show_Click(sender, e)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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

    Private Sub SetAttributes()
        Try
            OpenConn()
            txt_ForDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
            txt_ForDate.Attributes.Add("onkeypress", "OnlyDate();")
            txt_ForDate.Attributes.Add("onblur", "CheckDate(this,false);")
            'Srh_security.Columns.Add("SecurityName")
            'Srh_security.Columns.Add("SecurityId")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Protected Sub btn_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Show.Click
        FillDailyMarketValueGrid("SecurityId DESC", 0)
    End Sub

    Private Sub FillDailyMarketValueGrid(ByVal strSort As String, Optional ByVal intPageIndex As Int16 = 0)
        Try
            OpenConn()
            Dim sqlda As New SqlDataAdapter
            Dim sqldt As New DataTable
            Dim sqldv As New DataView
            sqlComm.Connection = sqlConn
            sqlComm.CommandTimeout = "100"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_FILL_ViewStockDetails_New"
            sqlComm.Parameters.Clear()

            If Trim(txt_ForDate.Text) <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@ForDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ForDate.Text))
            End If
            objCommon.SetCommandParameters(sqlComm, "@Compid", SqlDbType.Int, 4, "I", , , Val(Session("Compid")))
            If Val(Session("UserTypeId")) <> 1 Then
                objCommon.SetCommandParameters(sqlComm, "@BranchId", SqlDbType.Int, 4, "I", , , Val(Session("BranchId")))
            End If

            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            'objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , Val(Srh_security.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId")))
            objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(sqldt)

            If sqldt.Rows.Count > 0 Then
                Dim StockFaceValue As Double = Convert.ToDouble(sqldt.Compute("SUM(StockFaceValue)", String.Empty))

                Dim StockConsideration As Double = Convert.ToDouble(sqldt.Compute("SUM(StockSettlementAmt)", String.Empty))
                TStockFaceValue.Text = StockFaceValue
                'TPhysicalStock.Text = PhysicalStock
                TStockConsideration.Text = StockConsideration
                TStockFaceValue.Font.Bold = True
                TPhysicalStock.Font.Bold = True
                TStockConsideration.Font.Bold = True
            Else
                TStockFaceValue.Text = "0"
                TPhysicalStock.Text = "0"
                TStockConsideration.Text = "0"
                TStockFaceValue.Font.Bold = True
                TPhysicalStock.Font.Bold = True
                TStockConsideration.Font.Bold = True
            End If
            sqldv = sqldt.DefaultView
            sqldv.Sort = strSort
            dg_dme.DataSource = sqldv
            dg_dme.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Protected Sub dg_dme_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_dme.ItemDataBound
        Try
            Dim IntSecurityId As Integer

            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                Dim img As ImageButton
                img = CType(e.Item.FindControl("img_Select"), ImageButton)
                IntSecurityId = Val(TryCast(e.Item.DataItem, DataRowView).Row.Item("SecurityId").ToString)
                img.Attributes.Add("onclick", "SelectOption(this,'" & IntSecurityId & "');")
                e.Item.ToolTip = Trim(TryCast(e.Item.DataItem, DataRowView).Row.Item("StockStatus") & "")
            End If

        Catch ex As Exception
        End Try
    End Sub



    Protected Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Try
            ExportTradingStock()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub ExportTradingStock()

        Dim tw As New System.IO.StringWriter()
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        Dim dgGrid As New DataGrid()
        Dim strHead As String
        Dim dt As DataTable

        dgGrid.ShowFooter = True
        dgGrid.DataSource = GetReportTable("ID_FILL_ViewStockDetails_New") ' getData()
        dt = dgGrid.DataSource
        dt.Columns.Remove("SecurityId")
        Dim strComp As String

        strComp = dt.Rows(0).Item("CompName") & ""
        dt.Columns.Remove("Compname")

        If dt.Rows.Count > 0 Then
            hw.WriteLine("<b><u><font size='2' align='center'></font></u></b><br>")
            hw.WriteLine("<b><u><font size='3' align='center'>" & "Summary Stock for  " & strComp & "</font></u></b><br>")
            hw.WriteLine("<br>")

            dgGrid.HeaderStyle.Font.Bold = True
            dgGrid.GridLines = GridLines.Both
            dgGrid.ItemStyle.HorizontalAlign = HorizontalAlign.Center
            dgGrid.ItemStyle.Font.Name = "Cambria"
            dgGrid.ItemStyle.Font.Size = 10
            dgGrid.FooterStyle.Font.Bold = True
            dgGrid.DataBind()
            dgGrid.RenderControl(hw)
            Response.ClearHeaders()
            Response.AddHeader("content-disposition", "attachment;filename=ViewStockReport.xls")
            Response.ContentType = "application/vnd.ms-excel"
            Me.EnableViewState = False
            Response.Write(tw.ToString())
            Response.End()
        Else
            Dim strHtml As String
            strHtml = "alert('Sorry!!! No Records available to show report');"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
        End If

    End Sub
    Private Function GetReportTable(ByVal strProcName As String) As DataTable
        Try
            'If clsCommonFuns.sqlConn.State = ConnectionState.Closed Then clsCommonFuns.sqlConn.Open()
            OpenConn()
            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand

            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = strProcName
                If Trim(txt_ForDate.Text) <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@ForDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ForDate.Text))

                End If
                objCommon.SetCommandParameters(sqlComm, "@Compid", SqlDbType.Int, 4, "I", , , Val(Session("Compid")))
                'objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId")))
                objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
                objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 4, "I", , , Val(Srh_security.SelectedId))
                objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")

                .ExecuteNonQuery()
            End With
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dtfill)

            If dtfill.Rows.Count > 0 Then
                Dim StockFaceValue As Double = Convert.ToDouble(dtfill.Compute("SUM(StockFaceValue)", String.Empty))

                Dim StockConsideration As Double = Convert.ToDouble(dtfill.Compute("SUM(StockSettlementAmt)", String.Empty))
                TStockFaceValue.Text = StockFaceValue
                'TPhysicalStock.Text = PhysicalStock
                TStockConsideration.Text = StockConsideration
                TStockFaceValue.Font.Bold = True
                TPhysicalStock.Font.Bold = True
                TStockConsideration.Font.Bold = True
            Else
                TStockFaceValue.Text = "0"
                TPhysicalStock.Text = "0"
                TStockConsideration.Text = "0"
                TStockFaceValue.Font.Bold = True
                TPhysicalStock.Font.Bold = True
                TStockConsideration.Font.Bold = True
            End If
            'sqldv = sqldt.DefaultView
            'sqldv.Sort = strSort
            dg_dme.DataSource = dtfill
            dg_dme.DataBind()
            Return dtfill
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Function


    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            sqlConn.Dispose()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub

    Private Sub btn_ShowSecurity_Click(sender As Object, e As EventArgs) Handles btn_ShowSecurity.Click
        GetReportTable("ID_FILL_ViewStockDetails_New")
    End Sub

    Private Sub btn_ShowSecurityall_Click(sender As Object, e As EventArgs) Handles btn_ShowSecurityall.Click
        Srh_security.SelectedId = 0
        Srh_security.SearchTextBox.Text = ""
        FillDailyMarketValueGrid("SecurityId DESC", 0)

    End Sub
End Class
