Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports log4net
Partial Class Forms_ViewContactsAppEntry
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
        If IsPostBack = False Then
            If Val(Request.QueryString("Id")) <> 0 Then
                ViewState("Id") = Val(Request.QueryString("Id"))
                FillDetails()
            Else
                Response.Write("Contact Not Loaded")
            End If
        End If
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
    Private Sub FillDetails()
        Try
            Dim DT As DataTable
            Dim DR As DataRow
            OpenConn()
            DT = objCommon.FillDataTable(sqlConn, "MB_FILL_ViewAppEntryInvContacts", Val(ViewState("Id")), "InvContactId")
            DR = DT.Rows(0)
            lbl_ContactName.Text = DR("TitleName").ToString() & " " & DR("ContactPerson").ToString()
            lbl_Designation.Text = DR("Designation").ToString()
            lbl_DirectNo.Text = DR("PhoneNo").ToString()
            lbl_MobileNo.Text = DR("MobileNo").ToString()
            lbl_BoardNo.Text = DR("BoardNo").ToString()
            lbl_Interaction.Text = DR("InteractionVal").ToString()
            lbl_Section.Text = DR("SectionVal").ToString()
            lbl_Branch.Text = DR("Branch").ToString()
            lbl_Instrument.Text = setInstruments(DR("Instruments").ToString())
            lbl_AdditionalContact.Text = DR("AdditionalContacts").ToString().Replace("%5Cn", "</br>")
            If DR("AdditionalContacts").ToString() = "" Then
                lbl_AdditionalContact.Text = "&nbsp;"
            End If

        Catch ex As Exception
            'errorinfo.send_error(ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function setInstruments(ByVal Instr As String) As String
        Dim InstrNames As String = ""
        Dim instrumnets() As String = {"CP", "NCD", "Public Deposit", "Capital Gain Bond", "FD"}
        If Trim(Instr) <> "" Then
            For I As Int32 = 0 To 4
                If Instr.Chars(I) = "1" Then
                    InstrNames = InstrNames + instrumnets(I) & ","
                End If
            Next
            Return InstrNames
        Else
            Return ""
        End If
    End Function
End Class
