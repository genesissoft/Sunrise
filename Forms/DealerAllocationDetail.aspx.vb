Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_DealerAllocationDetail
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

        Dim strValues() As String

        If (Request.QueryString("Id") & "") <> "" Then 'customerid
            Hid_DealerAllocationId.Value = Request.QueryString("Id").ToString()
        End If

        If (Request.QueryString("AddEdit") & "") <> "" Then
            Dim AddEdit As String = Request.QueryString("AddEdit").ToString()
            If (AddEdit = "Add") Then
                btn_Update.Visible = False
            End If
        End If

        If (IsPostBack = False) Then
            btn_Save.Attributes.Add("onclick", "return Validation();")
            Dim dt As DataTable
            Dim dt1 As DataTable
            OpenConn()
            dt = objCommon.FillControl(cbo_ContactInteraction, sqlConn, "ID_FILL_InteractionType_CP", "InteractionType", "InteractionTypeId")
            dt1 = objCommon.FillControl(cbo_Dealer, sqlConn, "ID_FILL_Dealer", "NameOfUser", "UserId")
            CloseConn()

            If Trim(Request.QueryString("Values") & "") <> "" Then

                strValues = Split(Trim(Request.QueryString("Values") & ""), "!")
                Dim strDealerBusinessType() As String = Split(strValues(3), ",")

                cbo_Dealer.SelectedValue = strValues(1)
                cbo_DealerType.SelectedValue = strValues(2)

                'If (cbo_DealerType.SelectedItem.Text = "Management" Or cbo_DealerType.SelectedItem.Text = "Mid-Management") Then
                '    row_LstBustype.Visible = False
                'End If


                For I As Int16 = 0 To strDealerBusinessType.Length - 1
                    If strDealerBusinessType(I) <> "" Then
                        If (strDealerBusinessType.Length = 1) Then
                            If (strDealerBusinessType(0).ToString() = "CP") Then
                                chk_BusinessType.Items(0).Selected = True
                            ElseIf (strDealerBusinessType(0).ToString() = "NCD") Then
                                chk_BusinessType.Items(1).Selected = True
                            ElseIf (strDealerBusinessType(0).ToString() = "Public Deposit") Then
                                chk_BusinessType.Items(2).Selected = True
                            ElseIf (strDealerBusinessType(0).ToString() = "Capital Gain Bond") Then
                                chk_BusinessType.Items(3).Selected = True
                            ElseIf (strDealerBusinessType(0).ToString() = "FD") Then
                                chk_BusinessType.Items(4).Selected = True
                            End If
                        Else
                            If (strDealerBusinessType(I).ToString() = "CP") Then
                                chk_BusinessType.Items(0).Selected = True
                            ElseIf (strDealerBusinessType(I).ToString() = "NCD") Then
                                chk_BusinessType.Items(1).Selected = True
                            ElseIf (strDealerBusinessType(I).ToString() = "Public Deposit") Then
                                chk_BusinessType.Items(2).Selected = True
                            ElseIf (strDealerBusinessType(I).ToString() = "Capital Gain Bond") Then
                                chk_BusinessType.Items(3).Selected = True
                            ElseIf (strDealerBusinessType(I).ToString() = "FD") Then
                                chk_BusinessType.Items(4).Selected = True
                            End If

                        End If
                        'srh_DealerBusniessType.SelectListBox.Items.Add(New ListItem(strDealerBusinessType(I), strDealerBusinessTypeId(I)))
                    End If
                Next
                cbo_ContactInteraction.SelectedValue = strValues(5)
                txt_FromDate.Text = strValues(6)
                txt_ToDate.Text = strValues(7)
                btn_Cancel.Visible = True
                btn_Update.Visible = False
            End If

        End If
        txt_FromDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_FromDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_ToDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_ToDate.Attributes.Add("onblur", "CheckDate(this,false);")
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

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "close", "RetValues('" & Hid_Ids.Value & "',0);", True)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub
End Class
