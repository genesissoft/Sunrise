Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_DematSlipGeneration
    Inherits System.Web.UI.Page
    Dim sqlComm As New SqlCommand
    Dim objCommon As New clsCommonFuns
    Dim dsmenu As DataSet
    Dim dsDPDetails As DataSet
    Dim newTable As DataTable
    Dim DpDetailsTable As DataTable
    Dim trmenu As DataRow
    Dim chkBox As CheckBox
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
            'rbl_ReportType.Items(0).Attributes.Add("onclick", "SelectType();")
            'rbl_ReportType.Items(1).Attributes.Add("onclick", "SelectType();")
            'rbl_ReportType.Items(2).Attributes.Add("onclick", "SelectType();")


            If IsPostBack = False Then
                SetControls()
                btn_Print.Visible = False
                btn_Print.Attributes.Add("onclick", "return  validation();")
                btn_Show.Attributes.Add("onclick", "return  ValidateCustomer();")


                Hid_ReportType.Value = Trim(Request.QueryString("Rpt") & "")
                If Hid_ReportType.Value = "NSDLTOCSDL" Then
                    srh_NameOFClient.SearchTextBox.Visible = False
                    srh_NameOFClient.SearchButton.Visible = False
                    srh_NameOFClient.Mandatory = False
                    lbl_CustLabel.visible = False
                    Hid_CustomerId.Value = ""
                    Col_Headers.InnerHtml = "NSDL TO CSDL"
                Else
                    Col_Headers.InnerHtml = "NSDL TO NSDL"
                End If
                txt_ForDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
                txt_ForDate.Attributes.Add("onkeypress", "OnlyDate();")
                txt_ForDate.Attributes.Add("onblur", "CheckDate(this,false);")
                'btn_Show_Click(sender, e)
            End If

            'Page.ClientScript.RegisterStartupScript(Me.GetType, "Type", "SelectType();", True)
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

    Private Sub SetControls()
        Try
            OpenConn()
            objCommon.FillControl(cbo_DPName, sqlConn, "ID_FILL_DMATDealSlipGenr", "DPName", "DMatId", Session("CompId"), "CompId")
            'srh_NameOFClient.Columns.Add("CustomerName")
            'srh_NameOFClient.Columns.Add("CustomerId")
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
        
    End Sub

    Protected Sub btn_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Show.Click
        FillGrid("DealSlipId DESC", 0)


    End Sub

    Private Sub FillGrid(ByVal strSort As String, Optional ByVal intPageIndex As Int16 = 0)
        Try
            Dim sqlda As New SqlDataAdapter
            Dim sqldt As New DataTable
            Dim sqldv As New DataView
            OpenConn()
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_FILL_DematSlipGeneration"
            sqlComm.Parameters.Clear()

            If Trim(txt_ForDate.Text) <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@SettmentDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ForDate.Text))
            End If
            If Hid_CustomerId.Value <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , srh_NameOFClient.SelectedId)
            End If

            If rdo_DISLtr.SelectedValue = "D" Or rdo_DISLtr.SelectedValue = "N" Or rdo_DISLtr.SelectedValue = "A" Then
                If cbo_DPName.SelectedValue <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@DMatId", SqlDbType.Int, 4, "I", , , cbo_DPName.SelectedValue)
                Else
                    objCommon.SetCommandParameters(sqlComm, "@DMatId", SqlDbType.Int, 4, "I", , , DBNull.Value)
                End If
            End If
            ' objCommon.SetCommandParameters(sqlComm, "@DMatId", SqlDbType.Int, 4, "I", , , cbo_DPName.SelectedValue)



            If Hid_ReportType.Value = "NSDLTONSDL" Then
                objCommon.SetCommandParameters(sqlComm, "@Flag", SqlDbType.Char, 1, "I", , , "N")
            Else
                objCommon.SetCommandParameters(sqlComm, "@Flag", SqlDbType.Char, 1, "I", , , "C")
            End If
            If rbl_ReportType.SelectedValue = "NI" Then
                objCommon.SetCommandParameters(sqlComm, "@ReptType", SqlDbType.Char, 1, "I", , , "A")
            Else
                objCommon.SetCommandParameters(sqlComm, "@ReptType", SqlDbType.Char, 1, "I", , , rbl_ReportType.SelectedValue)
            End If
            objCommon.SetCommandParameters(sqlComm, "@SlipType", SqlDbType.Char, 1, "I", , , rdo_DISLtr.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(sqldt)
            sqldv = sqldt.DefaultView
            sqldv.Sort = strSort
            dg_dme.DataSource = sqldv
            dg_dme.DataBind()

            If sqldt.Rows.Count > 0 Then
                With sqldt.Rows(0)
                    Hid_DealSlipId.value = (.Item("DealSlipId") & "")

                End With
                btn_Print.Visible = True

            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub srh_NameOFClient_ButtonClick() Handles srh_NameOFClient.ButtonClick
        FillCustomerName()
    End Sub

    Private Sub FillCustomerName()
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_CustomerMaster", srh_NameOFClient.SelectedId, "CustomerId")
            If dt.Rows.Count > 0 Then
                srh_NameOFClient.SearchTextBox.Text = Trim(dt.Rows(0).Item("CustomerName") & "")
                Hid_CustomerId.value = srh_NameOFClient.SelectedId
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub GetIds()
        Try
            Dim i As Integer
            Dim dtGrid As DataGridItem
            Dim intId As Integer
            Dim chkBox As CheckBox
            For i = 0 To dg_dme.Items.Count - 1
                chkBox = CType(dg_dme.Items(i).FindControl("chk_ItemChecked"), CheckBox)
                If chkBox.Checked = True Then
                    dtGrid = dg_dme.Items(i)
                    intId = Val(CType(dg_dme.Items(i).FindControl("lbl_DealSlipID"), Label).Text)
                    Hid_Intids.Value += intId & ","
                End If
            Next
            If Hid_Intids.Value <> "" Then
                Hid_Intids.Value = Left(Hid_Intids.Value, Len(Hid_Intids.Value) - 1)
                Session("intids") = Hid_Intids.Value
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Try
            Dim strBank As String = rdo_DISLtr.SelectedValue
            If rdo_DISLtr.SelectedValue = "N" Or rdo_DISLtr.SelectedValue = "A" Then
                Hid_ReportType.Value = "NSDLIndusInd"
                Session("intids") = Hid_DealSlipIds.Value
            Else
                Hid_ReportType.Value = Hid_ReportType.Value
                Session("intids") = Hid_DealSlipIds.Value
            End If
            Response.Redirect("ViewReports.aspx?Rpt=" & Hid_ReportType.Value & "&DealSlipId=" & Hid_Intids.Value & "&Fromdate=" & txt_ForDate.Text & "&DematRptType=" & rbl_ReportType.SelectedValue & "&DPName=" & cbo_DPName.SelectedItem.Text & "&strBank=" & rdo_DISLtr.SelectedValue, False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dg_dme_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_dme.ItemDataBound
        Try
            Dim intDealSlipID As Integer
            Dim intCustomerId As Integer
            Dim intImgId As Integer
            'Dim dt As DataTable
            Dim dtGrid As DataTable
            'Dim dtRow As DataRow
            Dim chkBoxHead As CheckBox


            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                e.Item.ID = "itm" & e.Item.ItemIndex
                intCustomerId = Val(CType(e.Item.FindControl("lbl_CustomerId"), Label).Text)
                intDealSlipID = Val(CType(e.Item.FindControl("lbl_DealSlipID"), Label).Text)
                ' intImgId = Val(CType(e.Item.FindControl("lbl_ImgId"), Label).Text)
                chkBox = CType(e.Item.FindControl("chk_ItemChecked"), CheckBox)
                If rdo_DISLtr.SelectedValue = "D" Or rdo_DISLtr.SelectedValue = "N" Or rdo_DISLtr.SelectedValue = "A" Then
                    Hid_Intids.Value += Val(CType(e.Item.FindControl("lbl_Dmatinfoid"), Label).Text) & "!"
                Else
                    Hid_Intids.Value += Val(CType(e.Item.FindControl("lbl_DealSlipID"), Label).Text) & "!"
                End If
                'If chkBox.Checked = True Then
                '    'Hid_Intids.Value += Val(CType(e.Item.FindControl("lbl_DealSlipID"), Label).Text) & "!"
                '    ' CType(e.Item.FindControl("txt_SecurityType"), TextBox).BackColor = Drawing.Color.FromName("#D1E4F8")
                'End If
            Else
                If (e.Item.ItemType = ListItemType.Header) Then
                    chkBoxHead = CType(e.Item.FindControl("chk_AllItems"), CheckBox)

                    'CType(e.Item.FindControl("chk_AllItems"), CheckBox).BackColor = Drawing.Color.FromName("#D1E4F8")

                End If
                'Hid_Intids.Value += dtGrid.Rows(e.Item.ItemIndex).Item("DealSlipIdId") & "!"
                'Hid_Intids.Value = Left(Hid_Intids.Value, Len(Hid_Intids.Value) - 1)
                'Session("intids") = Hid_Intids.Value
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    'Private Sub OpenSqlConn()
    '    sqlConn = New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
    '    sqlConn.Open()
    'End Sub

    'Private Sub CloseSqlConn()
    '    If sqlConn Is Nothing Then Exit Sub
    '    If sqlConn.State = ConnectionState.Open Then sqlConn.Close()
    'End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub

    Protected Sub rbl_ReportType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbl_ReportType.SelectedIndexChanged
        Try
            If rbl_ReportType.SelectedValue = "A" Then
                row_Cust.Visible = True
                row_DPName.Visible = True
            Else
                row_Cust.Visible = False
                'row_DPName.Visible = False
            End If
            dg_dme.DataSource = Nothing
            dg_dme.DataMember = Nothing
            dg_dme.DataBind()
            cbo_DPName.SelectedIndex = 0
            srh_NameOFClient.SearchTextBox.Text = ""
        Catch ex As Exception

        End Try
    End Sub
End Class
