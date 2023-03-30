Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class Forms_AddBulk
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As New SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim bAdd As String = String.Empty
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
         
            If IsPostBack = False Then
                SetControls()
                bAdd = Request.QueryString("bAdd")
                Hid_bFlag.Value = bAdd

                Dim int_perc As Decimal = Request.QueryString("percent")
                ViewState("percent") = int_perc
                hid_percent.Value = int_perc
                SetAttributes()
            End If

            If (bAdd = "'A'") Then
                Hid_NSDLFaceValue.Value = Request.QueryString("Hid_NSDLFaceValue")
            ElseIf (bAdd = "'E'") Then
                Dim strVal As String = Request.QueryString("str")
                Page.ClientScript.RegisterStartupScript(Me.GetType, "CalBonds", "TotalNoOfBond();", True)
                If IsPostBack = False Then FillFields(strVal)
            End If
            'Page.ClientScript.RegisterStartupScript(Me.GetType, "TotalNoOfBond", "TotalNoOfBond();", True)
            'txt_Roundoff.Attributes.Add("onchange", "CalcRoundofsettAMT();")

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
        Try
            txt_Amount.Attributes.Add("onchange", "CalcBrokAmt();")
            txt_Amount.Attributes.Add("onblur", "TotalNoOfBond();")
            cbo_Amount.Attributes.Add("onchange", "TotalNoOfBond();")
            Hid_NSDLFaceValue.Value = Request.QueryString("Hid_NSDLFaceValue")
            btn_ShowCustomer.Attributes.Add("onclick", "return ShowCustomerMaster();")
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillFields(ByVal strVal As String)
        Dim strData() As String
        Dim FaceVal() As String

        Try


            strData = strVal.Split("*")

            Hid_Index.Value = strData(0)
            FaceVal = strData(6).ToString().Split("!")
            txt_Brok.Text = strData(5)
            txt_Amount.Text = FaceVal(0)
            cbo_Amount.SelectedValue = FaceVal(1)
            'Srh_NameOFClient.SearchTextBox.Text = strData(3).Replace("$", "&")
            'Fill Customer Name
            FillCustomerName(strData(2))
            hid_CustId.Value = strData(2)
            txt_NoOfBonds.Text = strVal(7)

            'strVal = e.Row.DataItemIndex
            'strVal = strVal + "," + dr.Item("SrNo")
            'strVal = strVal + "," + dr.Item("CustomerId")
            'strVal = strVal + "," + dr.Item("Customer")
            'strVal = strVal + "," + dr.Item("FaceValue")
            'strVal = strVal + "," + dr.Item("BrokerageAmt")
            'strVal = strVal + "," + dr.Item("FaceVal_WMult")

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally

        End Try
    End Sub

    Private Sub SetControls()
        Try
            OpenConn()
            'Srh_NameOFClient.Columns.Add("CustomerName")
            'Srh_NameOFClient.Columns.Add("CustomerTypeName")
            'Srh_NameOFClient.Columns.Add("CustomerCity")
            'Srh_NameOFClient.Columns.Add("CustomerPhone")
            'Srh_NameOFClient.Columns.Add("CustomerId")
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    'Private Sub FillCustName(ByVal CustId)
    '    Try
    '        OpenConn()
    '        Dim dt As DataTable
    '        dt = objCommon.FillDataTable(sqlConn, "Id_FILL_DealSlipEntry", CustId, "CustomerId")
    '        If dt.Rows.Count > 0 Then
    '            Hid_Amt.Value = Trim(dt.Rows(0).Item("Amount") & "")
    '            Hid_AddInterest.Value = Trim(dt.Rows(0).Item("InterestAmt") & "")
    '            Hid_IntDays.Value = Trim(dt.Rows(0).Item("InterestDays") & "")
    '            Hid_InterestFromTo.Value = Trim(dt.Rows(0).Item("InterestFromToDates") & "")
    '            Hid_SettlementAmt.Value = Trim(dt.Rows(0).Item("Settamtbeforeroundoff") & "")
    '        End If
    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    Finally
    '        CloseConn()
    '    End Try
    'End Sub
    Private Sub FillCustomerName(ByVal CustId)
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_CustomerMaster", CustId, "CustomerId")
            If dt.Rows.Count > 0 Then
                Srh_NameOFClient.SelectedId = CustId
                Srh_NameOFClient.SearchTextBox.Text = Trim(dt.Rows(0).Item("CustomerName") & "")
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Protected Sub Srh_NameOFClient_ButtonClick() Handles Srh_NameOFClient.ButtonClick
        Try
            Hid_CId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Convert.ToString(Srh_NameOFClient.SelectedId)))
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
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
End Class
