Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports log4net


Partial Class Forms_AdminReport
    Inherits System.Web.UI.Page

    Dim PgName As String = "$AdminReport$"
    Dim objUtil As New Util

    'Dim objCommon As New clsCommon
    Dim objCommon As New clsCommonFuns

    Dim datEndDate As Date
    Dim datRegDate As Date
    Dim sqlConn As SqlConnection
    Dim fileName As String = ""
    Dim fileExtension As String = ""
    Dim documentType As String = ""
    Dim documentBinary As Byte() = New Byte(0) {}
    Dim fileSize As Integer
    Dim dtEmail As New DataTable
    Public strUserName As String
    Dim decCPAmt, decCDAmt, decSLRAmt, decNONSLRAmt, decTotAmt As Decimal
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            Hid_Selection.Value = Trim(Request.QueryString("Value") & "")
            If Hid_Selection.Value = "Dealer" Then
                'row_Dealerwise.Visible = True
                cbo_Dealer.Visible = True
                lbl_Name.Text = "Dealer Name:"
            Else
                cbo_Customer.Visible = True
                lbl_Name.Text = "Customer Name:"
            End If

            If IsPostBack = False Then
                If (Request.QueryString("Value") & "") = "Dealer" Then
                    FillDealerName()
                Else
                    FillCustomerName()
                End If
                btn_Go.Attributes.Add("onclick", "return Validation();")
                txt_FromDate.Text = DateTime.Now.ToString("dd/MM/yyyy")
                txt_ToDate.Text = DateTime.Now.ToString("dd/MM/yyyy")

            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex)
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
    Private Sub FillDealerName()
        Try

            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter
            Dim dt As New DataTable
            sqlComm.CommandText = "CRM_FILL_UserMaster_Admin"
            sqlComm.CommandType = CommandType.StoredProcedure
            OpenConn()
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            cbo_Dealer.DataSource = dt
            cbo_Dealer.DataTextField = "NameOfUser"
            cbo_Dealer.DataValueField = "UserId"
            cbo_Dealer.DataBind()
            cbo_Dealer.Items.Add(New ListItem("Select", "0"))
            cbo_Dealer.SelectedValue = "0"
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillDealerName", "Error in FillDealerName", "", ex)
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub FillCustomerName()
        Try

            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter
            Dim dt As New DataTable
            sqlComm.CommandText = "CRM_FILL_CustomerMaster_Admin"
            sqlComm.CommandType = CommandType.StoredProcedure
            OpenConn()
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            cbo_Customer.DataSource = dt
            cbo_Customer.DataTextField = "CustomerName"
            cbo_Customer.DataValueField = "CustomerId"
            cbo_Customer.DataBind()
            cbo_Customer.Items.Add(New ListItem("Select", "0"))
            cbo_Customer.SelectedValue = "0"
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillCustomerName", "Error in FillCustomerName", "", ex)
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try
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
            objCommon.SetCommandParameters(sqlComm, "@Fromdate", SqlDbType.VarChar, 20, "I", , , objCommon.DateFormatMMDDYY(Hid_FromDate.Value))
            objCommon.SetCommandParameters(sqlComm, "@Todate", SqlDbType.VarChar, 20, "I", , , objCommon.DateFormatMMDDYY(Hid_ToDate.Value))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(Session("UserId")))
            If Val(cbo_Customer.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@StrCustomerValues", SqlDbType.VarChar, 20, "I", , , Trim(cbo_Customer.SelectedValue))
            Else
                objCommon.SetCommandParameters(sqlComm, "@StrCustomerValues", SqlDbType.VarChar, 20, "I", , , DBNull.Value)
            End If
            If Val(cbo_Dealer.SelectedValue) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@StrDealerValues", SqlDbType.VarChar, 20, "I", , , Trim(cbo_Dealer.SelectedValue))
            Else
                objCommon.SetCommandParameters(sqlComm, "@StrDealerValues", SqlDbType.VarChar, 20, "I", , , DBNull.Value)
            End If
            'objCommon.SetCommandParameters(sqlComm, "@StrSelVal", SqlDbType.VarChar, 2, "I", , , "CU")
            If Convert.ToString(Session("UserType")).ToLower() <> "administrator" Then
                objCommon.SetCommandParameters(sqlComm, "@UserType", SqlDbType.Char, 1, "I", , , "U")
            Else
                objCommon.SetCommandParameters(sqlComm, "@UserType", SqlDbType.Char, 1, "I", , , "A")
            End If
            'objCommon.SetCommandParameters(sqlComm, "@PageName", SqlDbType.VarChar, 50, "I", , , "AdminRpt")
            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            If dt.Rows.Count > 0 Then
                btn_FullDealRpt.Visible = True
                dg_DealRpt.DataSource = dt
                dg_DealRpt.DataBind()
            Else
                btn_FullDealRpt.Visible = False
                dg_DealRpt.DataSource = dt
                dg_DealRpt.DataBind()
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillDataGrid_DealRpt", "Error in FillDataGrid_DealRpt", "", ex)
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try
    End Sub

  
    Private Sub FillDataGrid_MeetingRpt()
        Dim sqlComm As New SqlCommand
        Dim sqlDa As New SqlDataAdapter
        Dim dt As New DataTable
        Dim strCond As String = ""
        If Val(cbo_Dealer.SelectedValue) <> 0 Then
            strCond = " AND CE.UserId IN (" & cbo_Dealer.SelectedValue & ") "
        End If
        If Val(cbo_Customer.SelectedValue) <> 0 Then
            strCond = " AND CE.ClientId IN (" & cbo_Customer.SelectedValue & ") "
            dg_MeetingRpt.Columns(2).HeaderText = "Dealer Name"
        End If
        Try
            OpenConn()
            sqlComm.CommandText = "CRM_INTERACTION_ADMINRPT"
            sqlComm.CommandType = CommandType.StoredProcedure
            OpenConn()
            sqlComm.Connection = sqlConn 'obj.DateFormatMMDDYY
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@Fromdate", SqlDbType.VarChar, 20, "I", , , objCommon.DateFormatMMDDYY(Hid_FromDate.Value))
            objCommon.SetCommandParameters(sqlComm, "@Todate", SqlDbType.VarChar, 20, "I", , , objCommon.DateFormatMMDDYY(Hid_ToDate.Value))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(Session("UserId")))
            If strCond <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@StrValues", SqlDbType.VarChar, 200, "I", , , strCond)
            Else
                objCommon.SetCommandParameters(sqlComm, "@StrValues", SqlDbType.VarChar, 200, "I", , , DBNull.Value)
            End If
            'objCommon.SetCommandParameters(sqlComm, "@StrSelVal", SqlDbType.VarChar, 2, "I", , , "CU")
            If Convert.ToString(Session("UserType")).ToLower() <> "administrator" Then
                objCommon.SetCommandParameters(sqlComm, "@UserType", SqlDbType.Char, 1, "I", , , "U")
            Else
                objCommon.SetCommandParameters(sqlComm, "@UserType", SqlDbType.Char, 1, "I", , , "A")
            End If

            sqlComm.ExecuteNonQuery()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            If dt.Rows.Count > 0 Then
                btn_FullMeetingRpt.Visible = True
                dg_MeetingRpt.DataSource = dt
                dg_MeetingRpt.DataBind()
            Else
                btn_FullMeetingRpt.Visible = False
                dg_MeetingRpt.DataSource = dt
                dg_MeetingRpt.DataBind()
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillDataGrid_MeetingRpt", "Error in FillDataGrid_MeetingRpt", "", ex)
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try
    End Sub
    Protected Sub btn_Go_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Go.Click
        Try
            txt_FromDate.Text = Hid_FromDate.Value
            txt_ToDate.Text = Hid_ToDate.Value
            FillDataGrid_DealRpt()
            'FillDataGrid_WeeklyRpt()
            FillDataGrid_MeetingRpt()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_Go_Click", "Error in btn_Go_Click", "", ex)
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub dg_DealRpt_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_DealRpt.ItemDataBound
        Dim strMatch As String
        Dim intCP, intCD, intSLR, intNONSLR As Integer
        Dim lblTotCP, lblTotCD, lblTotSLR, lblTotNONSLR, lblTotGrand As Label
        Dim lblUserName, lblCustomerName As Label
        Try
            If e.Item.ItemType = ListItemType.Header Then
                If Hid_Selection.Value = "Dealer" Then
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

                If Hid_Selection.Value = "Dealer" Then
                    lblCustomerName.Visible = True 
                Else
                    lblUserName.Visible = True
                End If
                decCPAmt += Convert.ToDecimal(CType(e.Item.FindControl("lbl_CPAmt"), Label).Text)
                decCDAmt += Convert.ToDecimal(CType(e.Item.FindControl("lbl_CDAmt"), Label).Text)
                decSLRAmt += Convert.ToDecimal(CType(e.Item.FindControl("lbl_SLRAmt"), Label).Text)
                decNONSLRAmt += Convert.ToDecimal(CType(e.Item.FindControl("lbl_NONSLRAmt"), Label).Text)
                decTotAmt += Convert.ToDecimal(CType(e.Item.FindControl("lbl_Total"), Label).Text)

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
