Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class Forms_ShowFullScreen
    Inherits System.Web.UI.Page

    Dim PgName As String = "$ShowFullScreen$"
    Dim objUtil As New Util

    'Dim objCommon As New ClsCommon
    Dim objCommon As New clsCommonFuns
    Dim datEndDate As Date
    Dim datRegDate As Date
    Dim sqlConn As SqlConnection
    Dim decCPAmt, decCDAmt, decSLRAmt, decNONSLRAmt, decTotAmt As Decimal
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            If IsPostBack = False Then
                If Session("UserName") = "" Then
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                End If
                If Request.QueryString("RptName") = "DealRpt" Then
                    row_DealRpt.Visible = True
                    FillDataGrid_DealRpt()
                    'ElseIf Request.QueryString("RptName") = "WeeklyRpt" Then
                    '    row_WeeklyRpt.Visible = True
                    '    FillDataGrid_WeeklyRpt()
                ElseIf Request.QueryString("RptName") = "MeetingRpt" Then
                    row_MeetingRpt.Visible = True
                    FillDataGrid_MeetingRpt()
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
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
    Private Sub FillDataGrid_DealRpt()
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Try
            OpenConn()
            sqlComm.CommandText = "CRM_FILL_DEALDETAILS"
            sqlComm.CommandType = CommandType.StoredProcedure
            OpenConn()
            sqlComm.Connection = sqlConn 'obj.DateFormatMMDDYY
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@Fromdate", SqlDbType.VarChar, 20, "I", , , objCommon.DateFormatMMDDYY(Request.QueryString("FromDate")))
            objCommon.SetCommandParameters(sqlComm, "@Todate", SqlDbType.VarChar, 20, "I", , , objCommon.DateFormatMMDDYY(Request.QueryString("ToDate")))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(Session("UserId")))
            If Trim(Request.QueryString("Selection") & "") = "Customer" Then
                objCommon.SetCommandParameters(sqlComm, "@StrCustomerValues", SqlDbType.VarChar, 20, "I", , , Trim(Request.QueryString("DealerId")))
            Else
                objCommon.SetCommandParameters(sqlComm, "@StrCustomerValues", SqlDbType.VarChar, 20, "I", , , "")
            End If
            If Trim(Request.QueryString("Selection") & "") = "Dealer" Then
                objCommon.SetCommandParameters(sqlComm, "@StrDealerValues", SqlDbType.VarChar, 20, "I", , , Trim(Request.QueryString("DealerId")))
            Else
                objCommon.SetCommandParameters(sqlComm, "@StrDealerValues", SqlDbType.VarChar, 20, "I", , , DBNull.Value)
            End If
            'objCommon.SetCommandParameters(sqlComm, "@StrSelVal", SqlDbType.VarChar, 2, "I", , , "CU")
            If Convert.ToString(Session("UserType")).ToLower() <> "administrator" Then
                objCommon.SetCommandParameters(sqlComm, "@UserType", SqlDbType.Char, 1, "I", , , "U")
            Else
                objCommon.SetCommandParameters(sqlComm, "@UserType", SqlDbType.Char, 1, "I", , , "A")
            End If
            'objCommon.SetCommandParameters(sqlComm, "@PageName", SqlDbType.VarChar, 20, "I", , , "AdminRpt")

            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            dg_DealRpt.DataSource = dt
            dg_DealRpt.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillDataGrid_DealRpt", "Error in FillDataGrid_DealRpt", "", ex)
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try
    End Sub

    'Private Sub FillDataGrid_WeeklyRpt()
    '    Dim sqlComm As New SqlCommand
    '    Dim sqlDa As New SqlDataAdapter
    '    Dim dt As New DataTable
    '    Dim strCond As String = ""
    '    If Trim(Request.QueryString("Selection") & "") = "Dealer" Then
    '        strCond = " AND UM.UserId IN (" & Request.QueryString("DealerId") & ") "
    '    Else
    '        strCond = " AND CE.ClientId IN (" & Request.QueryString("DealerId") & ") "
    '    End If

    '    Try
    '        OpenConn()
    '        sqlComm.CommandText = "CRM_RPT_WEEKLYUPDATE"
    '        sqlComm.CommandType = CommandType.StoredProcedure
    '        OpenConn()
    '        sqlComm.Connection = sqlConn 'obj.DateFormatMMDDYY
    '        sqlComm.Parameters.Clear()
    '        objCommon.SetCommandParameters(sqlComm, "@Fromdate", SqlDbType.VarChar, 20, "I", , , objCommon.DateFormatMMDDYY(Request.QueryString("FromDate")))
    '        objCommon.SetCommandParameters(sqlComm, "@Todate", SqlDbType.VarChar, 20, "I", , , objCommon.DateFormatMMDDYY(Request.QueryString("ToDate")))
    '        objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(Session("UserId")))
    '        objCommon.SetCommandParameters(sqlComm, "@StrValues", SqlDbType.VarChar, 2000, "I", , , strCond)
    '        'objCommon.SetCommandParameters(sqlComm, "@StrSelVal", SqlDbType.VarChar, 2, "I", , , "CU")
    '        If Convert.ToString(Session("UserType")).ToLower() <> "administrator" Then
    '            objCommon.SetCommandParameters(sqlComm, "@UserType", SqlDbType.Char, 1, "I", , , "U")
    '        Else
    '            objCommon.SetCommandParameters(sqlComm, "@UserType", SqlDbType.Char, 1, "I", , , "A")
    '        End If
    '        'objCommon.SetCommandParameters(sqlComm, "@PageName", SqlDbType.VarChar, 20, "I", , , "AdminRpt")
    '        'objCommon.SetCommandParameters(sqlComm, "@ShowPage", SqlDbType.VarChar, 10, "I", , , "Full")
    '        sqlComm.ExecuteNonQuery()
    '        sqlDa.SelectCommand = sqlComm
    '        sqlDa.Fill(dt)
    '        dg_WeeklyRpt.DataSource = dt
    '        dg_WeeklyRpt.DataBind()
    '    Catch ex As Exception
    '        objUtil.WritErrorLog(PgName, "FillDataGrid_WeeklyRpt", "Error in FillDataGrid_WeeklyRpt", "", ex)
    '        Response.Write(ex.Message)
    '    Finally
    '        CloseConn()
    '    End Try
    'End Sub

    '  AND (clientDIFF='CU' AND CE.ClientId IN (8600,3560)) AND CE.UserId IN (148)
    Private Sub FillDataGrid_MeetingRpt()
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim strCond As String = ""
        If Trim(Request.QueryString("Selection") & "") = "Dealer" Then
            strCond = " AND CE.UserId IN (" & Request.QueryString("DealerId") & ") "
        Else
            strCond = " AND CE.ClientId IN(" & Request.QueryString("DealerId") & ") "
            dg_MeetingRpt.Columns(2).HeaderText = "Dealer Name"
        End If
        Try
            OpenConn()
            sqlComm.CommandText = "CRM_INTERACTION_ADMINRPT"
            sqlComm.CommandType = CommandType.StoredProcedure
            OpenConn()
            sqlComm.Connection = sqlConn 'obj.DateFormatMMDDYY
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@Fromdate", SqlDbType.VarChar, 20, "I", , , objCommon.DateFormatMMDDYY(Request.QueryString("FromDate")))
            objCommon.SetCommandParameters(sqlComm, "@Todate", SqlDbType.VarChar, 20, "I", , , objCommon.DateFormatMMDDYY(Request.QueryString("ToDate")))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@StrValues", SqlDbType.VarChar, 200, "I", , , strCond)
            'objCommon.SetCommandParameters(sqlComm, "@StrSelVal", SqlDbType.VarChar, 2, "I", , , "CU")
            If Convert.ToString(Session("UserType")).ToLower() <> "administrator" Then
                objCommon.SetCommandParameters(sqlComm, "@UserType", SqlDbType.Char, 1, "I", , , "U")
            Else
                objCommon.SetCommandParameters(sqlComm, "@UserType", SqlDbType.Char, 1, "I", , , "A")
            End If
            'objCommon.SetCommandParameters(sqlComm, "@ShowPage", SqlDbType.VarChar, 10, "I", , , "Full")
            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            dg_MeetingRpt.DataSource = dt
            dg_MeetingRpt.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillDataGrid_MeetingRpt", "Error in FillDataGrid_MeetingRpt", "", ex)
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try
    End Sub


    Protected Sub dg_DealRpt_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_DealRpt.ItemDataBound
        Dim strMatch As String
        Dim intMM, intSLR, intNONSLR, intCPPVT As Integer
        Dim lblTotCP, lblTotCD, lblTotSLR, lblTotNONSLR, lblTotGrand As Label
        Dim lblUserName, lblCustomerName As Label
        Try
            If e.Item.ItemType = ListItemType.Header Then
                If Trim(Request.QueryString("Selection") & "") = "Dealer" Then
                    e.Item.Cells(0).Text = "Customer Name"
                Else
                    e.Item.Cells(0).Text = "Dealer Name"
                End If
            End If
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                'strMatch = Trim(CType(e.Item.FindControl("lbl_Match"), Label).Text)
                'intMM = Val(CType(e.Item.FindControl("lbl_MMC"), Label).Text)
                'intSLR = Val(CType(e.Item.FindControl("lbl_SLRC"), Label).Text)
                'intNONSLR = Val(CType(e.Item.FindControl("lbl_NONSLRC"), Label).Text)
                'intCPPVT = Val(CType(e.Item.FindControl("lbl_CPPVTC"), Label).Text)

                lblCustomerName = CType(e.Item.FindControl("lbl_CustomerName"), Label)
                lblUserName = CType(e.Item.FindControl("lbl_UserName"), Label)
                If Trim(Request.QueryString("Selection") & "") = "Dealer" Then
                    lblCustomerName.Visible = True
                Else
                    lblUserName.Visible = True
                End If

                decCPAmt += Convert.ToDecimal(CType(e.Item.FindControl("lbl_CPAmt"), Label).Text)
                decCDAmt += Convert.ToDecimal(CType(e.Item.FindControl("lbl_CDAmt"), Label).Text)
                decSLRAmt += Convert.ToDecimal(CType(e.Item.FindControl("lbl_SLRAmt"), Label).Text)
                decNONSLRAmt += Convert.ToDecimal(CType(e.Item.FindControl("lbl_NONSLRAmt"), Label).Text)
                decTotAmt += Convert.ToDecimal(CType(e.Item.FindControl("lbl_Total"), Label).Text)
                'If strMatch = "N" Then
                '    'e.Item.BackColor = Drawing.Color.Yellow
                '    e.Item.BackColor = Drawing.Color.FromName("#F2F29E")
                '    'e.Item.BackColor = Drawing.Color.FromName("#E6E6A3")
                'Else
                '    If intMM <> 0 Then '#8AE6A3
                '        e.Item.Cells(1).BackColor = Drawing.Color.FromName("#B8E6A3")
                '        e.Item.Cells(2).BackColor = Drawing.Color.FromName("#B8E6A3")
                '    End If
                '    If intSLR <> 0 Then
                '        e.Item.Cells(3).BackColor = Drawing.Color.FromName("#B8E6A3")
                '    End If
                '    If intNONSLR <> 0 Then
                '        e.Item.Cells(4).BackColor = Drawing.Color.FromName("#B8E6A3")
                '    End If
                '    If intCPPVT <> 0 Then
                '        e.Item.Cells(5).BackColor = Drawing.Color.FromName("#B8E6A3")
                '        e.Item.Cells(6).BackColor = Drawing.Color.FromName("#B8E6A3")
                '    End If

                'End If

            End If

            If e.Item.ItemType = ListItemType.Footer Then
                lblTotCP = CType(e.Item.FindControl("lbl_TotCPAmt"), Label)
                lblTotCD = CType(e.Item.FindControl("lbl_TotCDAmt"), Label)
                lblTotSLR = CType(e.Item.FindControl("lbl_TotSLRAmt"), Label)
                lblTotNONSLR = CType(e.Item.FindControl("lbl_TotNONSLRAmt"), Label)
                lblTotGrand = CType(e.Item.FindControl("lbl_GrandTotal"), Label)
                lblTotCP.Text = decCPAmt
                lblTotCD.Text = decCDAmt
                lblTotSLR.Text = decSLRAmt
                lblTotNONSLR.Text = decNONSLRAmt
                lblTotGrand.Text = decTotAmt
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_DealRpt_ItemDataBound", "Error in dg_DealRpt_ItemDataBound", "", ex)
            Response.Write(ex.Message)
        End Try
    End Sub
End Class
