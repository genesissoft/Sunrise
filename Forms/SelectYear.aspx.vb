Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_SelectYear
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim LastIPDate As Date
    Dim dblPepCoupRate As Double
    Dim intDays As Int16
    Dim dt As DataTable
    Dim sqlConn As SqlConnection
    Dim strYearText As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.AddHeader("Cache-Control", "no-cache")
        Response.AddHeader("Cache-Control", "no-store")
        If (Session("username") = "") Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If
        'objCommon.OpenConn()
        Try
            If IsPostBack = False Then
                SetControls()
                FillDetails()
             
                FillSuperUserPassword()

            End If
            Page.SetFocus(btn_Ok)
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
            btn_Ok.Attributes.Add("onclick", "return Validation();")

            objCommon.FillControl(cbo_Company, sqlConn, "ID_FILL_CompanyMaster1", "CompName", "CompId", Val(Session("UserId")), "UserId")
            Dim c As Integer = cbo_Company.Items.Count
            If (c <= 1) Then
                btn_Ok.Enabled = False
            End If
            Dim lstitemCompany As ListItem = cbo_Company.Items.FindByText("")
            cbo_Company.Items.Remove(lstitemCompany)
            cbo_Company.Enabled = True
            Dim dt As DataTable
            dt = objCommon.FillControl(cbo_SelectYear, sqlConn, "ID_FILL_YEAR", "YearText", "YearId")
            Dim lstItemYear As ListItem = cbo_SelectYear.Items.FindByText("")
            cbo_SelectYear.Items.Remove(lstItemYear)
            If dt.Rows.Count > 0 Then

                If DateTime.Today.Month <= 3 Then
                    Dim CurrentYear As String = DateTime.Today.Year.ToString()
                    Dim FindYear As String = CurrentYear - 1
                    Dim CurrFinanYr = FindYear + "-" + CurrentYear
                    Dim CheckYear As ListItem = cbo_SelectYear.Items.FindByText(CurrFinanYr)
                    If Not CheckYear Is Nothing Then cbo_SelectYear.SelectedValue = CheckYear.Value
                    lbl_StartDate.Text = Format(dt.Rows(cbo_SelectYear.SelectedIndex).Item("StartDate"), "dd/MM/yyyy")
                    lbl_EndDate.Text = Format(dt.Rows(cbo_SelectYear.SelectedIndex).Item("EndDate"), "dd/MM/yyyy")
                Else
                    Dim CurrentYear As String = DateTime.Today.Year.ToString()
                    Dim NextYear As String = CType((DateTime.Today.Year + 1), String)
                    Dim FindYear As String = CurrentYear + "-" + NextYear
                    Dim CheckYear As ListItem = cbo_SelectYear.Items.FindByText(FindYear)
                    If Not CheckYear Is Nothing Then cbo_SelectYear.SelectedValue = CheckYear.Value
                    lbl_StartDate.Text = Format(dt.Rows(cbo_SelectYear.SelectedIndex).Item("StartDate"), "dd/MM/yyyy")
                    lbl_EndDate.Text = Format(dt.Rows(cbo_SelectYear.SelectedIndex).Item("EndDate"), "dd/MM/yyyy")
                End If

            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub FillDetails()
        Try
            Dim dt As DataTable
            OpenConn()

            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_YEAR", cbo_SelectYear.SelectedValue, "YearId")
            If dt.Rows.Count > 0 Then
                lbl_StartDate.Text = Format(dt.Rows(0).Item("StartDate"), "dd/MM/yyyy")
                lbl_EndDate.Text = Format(dt.Rows(0).Item("EndDate"), "dd/MM/yyyy")
                strYearText = Trim(dt.Rows(0).Item("YearText") & "")
            End If
            Dim CurrentYear As String = DateTime.Today.Year.ToString()
            Dim NextYear As String = CType((DateTime.Today.Year + 1), String)
            Dim FindYear As String = CurrentYear + "-" + NextYear
            Dim strYr As String = Right(strYearText, 4)
            Dim strdttime As String = Now.Year





        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub FillSuperUserPassword()
        Try
            Dim dt As DataTable
            OpenConn()

            dt = objCommon.FillDataTable(sqlConn, "MB_FILL_SuperUserType")
            Session("SPassword") = dt

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Protected Sub cbo_SelectYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SelectYear.SelectedIndexChanged
        Try
            FillDetails()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Ok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Ok.Click
        Try
            Session("CompId") = Val(cbo_Company.SelectedValue)
            Session("CompanyId") = Val(cbo_Company.SelectedValue)
            Session("YearId") = Val(cbo_SelectYear.SelectedValue)
            Session("CompName") = Trim(cbo_Company.SelectedItem.Text)
            Session("YearText") = Trim(cbo_SelectYear.SelectedItem.Text)
            Session("Year") = Trim(cbo_SelectYear.SelectedItem.Text)
            Response.Redirect("YieldCalculator.aspx", False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    


    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            Response.Redirect("Login.aspx", False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            'sqlConn.Dispose()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
