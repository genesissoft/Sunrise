Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_ClientContactPerson
    Inherits System.Web.UI.Page
    Dim sqlConn As New SqlConnection
    Dim objCommon As New clsCommonFuns

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        Try
            Dim strValues() As String
            'srh_BusniessType.SelectCheckbox.Visible = False
            'srh_BusniessType.SelectCheckbox.Checked = False
            'srh_BusniessType.SelectLinkButton.Enabled = True
            'srh_Dealer.SelectCheckbox.Visible = False
            'srh_Dealer.SelectCheckbox.Checked = False
            'srh_Dealer.SelectLinkButton.Enabled = True
            'srh_SearchRD.SelectCheckbox.Visible = False
            'srh_SearchRD.SelectCheckbox.Checked = False
            'srh_SearchRD.SelectLinkButton.Enabled = True
            If (IsPostBack = False) Then
                SetAttributes()
                If (Request.QueryString("CustomerId") & "") <> "" Then
                    Hid_CustomerId.Value = Request.QueryString("CustomerId").ToString()
                End If
                'Dim intId As Int16 = IIf(Request.QueryString("MercBanking") = "true", 1, 0)
                If Request.QueryString("MercBanking") = "true" Then
                    row_LstBustype.Visible = False
                End If
                FillCombo()
                If Trim(Request.QueryString("Values") & "") <> "" Then
                    strValues = Split(Trim(Request.QueryString("Values") & ""), "!")

                    txt_ContactPerson.Text = strValues(0)
                    txt_Designation.Text = strValues(1)
                    txt_PhoneNo1.Text = strValues(2)
                    txt_MobileNo.Text = strValues(3)
                    txt_EmailId.Text = strValues(4)
                    'Dim strBusinessType() As String = Split(strValues(5), ",")
                    'Dim strBusinessTypeId() As String = Split(strValues(6), ",")
                    'Dim strDealer() As String = Split(strValues(7), ",")
                    'Dim strDealerId() As String = Split(strValues(8), ",")

                    'For I As Int16 = 0 To strBusinessType.Length - 1
                    '    If strBusinessType(I) <> "" Then
                    '        srh_BusniessType.SelectListBox.Items.Add(New ListItem(strBusinessType(I), strBusinessTypeId(I)))
                    '    End If
                    'Next
                    'For I As Int16 = 0 To strDealer.Length - 1
                    '    If strDealer(I) <> "" Then
                    '        srh_Dealer.SelectListBox.Items.Add(New ListItem(strDealer(I), strDealerId(I)))
                    '    End If
                    'Next
                    txt_PhoneNo2.Text = strValues(5)
                    txt_FaxNo1.Text = strValues(6)
                    txt_FaxNo2.Text = strValues(7)
                    cbo_ContactInteraction.SelectedValue = Val(strValues(8))
                    rdoList_Section.SelectedValue = strValues(9)


                    'cbo_ContactInteraction.SelectedValue = strValues(15)
                    'txt_Branch.Text = strValues(16)


                    'Dim strResearchDocName() As String = Split(strValues(13), ",")
                    'Dim strResearchDocId() As String = Split(strValues(14), ",")
                    'For I As Int16 = 0 To strResearchDocName.Length - 1
                    '    If strResearchDocName(I) <> "" Then
                    '        srh_SearchRD.SelectListBox.Items.Add(New ListItem(strResearchDocName(I), strResearchDocId(I)))
                    '    End If
                    'Next

                    btn_Cancel.Visible = True
                    btn_Update.Visible = False
                End If
                btn_Update.Visible = False

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

    Private Sub FillCombo()
        Try
            Dim ds As New DataSet()
            ds = objComm.FillAllCombo("ID_Fill_INTERACTIONTYPE")
            cbo_ContactInteraction.DataSource = ds.Tables(0)
            cbo_ContactInteraction.DataValueField = "InteractionTypeId"
            cbo_ContactInteraction.DataTextField = "InteractionType"
            cbo_ContactInteraction.DataBind()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub SetAttributes()
        txt_ContactPerson.Attributes.Add("onblur", "ConvertUCase(this);")
        'btn_Save.Attributes.Add("onclick", "return Validation();")
    End Sub


    'Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
    '    Try
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "close", "RetValues('" & Hid_Ids.Value & "',0);", True)
    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        ' btn_Save_Click(sender, e)
    End Sub

    Private Sub A()
        Try
            OpenConn()
            Dim dt As DataTable
            Dim lstItem As ListItem
            dt = objCommon.FillControl(srh_BusniessType.SelectListBox, sqlConn, "ID_FILL_BusinessTypeMaster", "BusinessType", "BusinessTypeId")
            lstItem = srh_BusniessType.SelectListBox.Items.FindByText("")
            If lstItem IsNot Nothing Then
                srh_BusniessType.SelectListBox.Items.Remove(lstItem)
            End If

        Catch ex As Exception

        Finally
            CloseConn()
        End Try



    End Sub

    Private Sub FillCustomerDetailsGrid()
        Try
            OpenConn()
            Dim dt As DataTable
            Dim lstItem As ListItem
            dt = objCommon.FillControl(srh_BusniessType.SelectListBox, sqlConn, "ID_FILL_ClientBusniessDetails", "BusinessType", "BusinessTypeId", Val(Hid_CustomerId.Value), "CustomerId")
            lstItem = srh_BusniessType.SelectListBox.Items.FindByText("")
            If lstItem IsNot Nothing Then
                srh_BusniessType.SelectListBox.Items.Remove(lstItem)
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
