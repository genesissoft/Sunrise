Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_AddressMultiple
    Inherits System.Web.UI.Page
    Dim sqlConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Val(Session("UserId") & "") = 0 Then
            Response.Redirect("Login.aspx", False)
            Exit Sub
        End If
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        Try
            Dim strValues() As String
          
            srh_BusniessType.SelectCheckbox.Visible = False
            srh_BusniessType.SelectCheckbox.Checked = False
            srh_BusniessType.SelectLinkButton.Enabled = True
         
            If (IsPostBack = False) Then
                SetAttributes()
                If (Request.QueryString("CustomerId") & "") <> "" Then
                    Hid_CustomerId.Value = Request.QueryString("CustomerId").ToString()
                End If
                If Request.QueryString("MercBanking") = "true" Then
                    row_LstBustype.Visible = False
                    Hid_PageName.Value = "MercBanking"
                End If
                
                If Trim(Request.QueryString("Values") & "") <> "" Then
                    strValues = Split(Trim(Request.QueryString("Values") & ""), "!")
                    txt_CustBranchname.Text = strValues(0)
                    txt_Address1.Text = strValues(1)
                    txt_Address2.Text = strValues(2)
                    txt_City.Text = strValues(3)
                    txt_Pincode.Text = strValues(4)
                    txt_State.Text = strValues(5)
                    txt_Country.Text = strValues(6)
                    txt_PhoneNo.Text = strValues(7)
                    txt_FaxNo.Text = strValues(8)
                    txt_Email.Text = strValues(9)
                    Hid_CustomerId.Value = Request.QueryString("CustomerId").ToString()
                    Dim strBusinessType() As String = Split(strValues(10), ",")
                    Dim strBusinessTypeId() As String = Split(strValues(11), ",")
                    For I As Int16 = 0 To strBusinessType.Length - 1
                        If strBusinessType(I) <> "" Then
                            srh_BusniessType.SelectListBox.Items.Add(New ListItem(strBusinessType(I), strBusinessTypeId(I)))
                        End If
                    Next
                    Hid_ClientCustAddressId.Value = strValues(12)
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
    Private Sub SetAttributes()
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
        txt_CustBranchname.Attributes.Add("onblur", "ConvertUCase(this);")
    End Sub
    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "close", "RetValues();", True)
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "close", "RetValues('" & Hid_Ids.Value & "',0);", True)

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        btn_Save_Click(sender, e)
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
