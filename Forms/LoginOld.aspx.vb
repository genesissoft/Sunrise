Imports System.Security.Cryptography
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.Control
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Imports System
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Xml
Imports System.IO

Partial Class Forms_Login
    Inherits System.Web.UI.Page
    Dim blnValid As Boolean
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'objCommon.CloseConn()
            Login1.ConnectionString = ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString()
            Login1.UserMasterTableName = "UserMaster"
            Login1.LoginNameDatabaseField = "UserName"
            Login1.PasswordDatabaseField = "Password"
            Session.Clear()
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

    Protected Sub Login1_Authenticate(ByVal sender As Object, ByVal e As WebControls.AuthenticateEventArgs) Handles Login1.Authenticate

    End Sub

    Protected Sub Login1_LoggingIn(ByVal sender As Object, ByVal e As WebControls.LoginCancelEventArgs) Handles Login1.LoggingIn

        If Login1.Authenticated = True Then
            Session("UserId") = Login1.ReturnDataTable.Rows(0).Item("UserId")
            Session("UserTypeId") = Login1.ReturnDataTable.Rows(0).Item("UserTypeId")
            Session("UserName") = Login1.ReturnDataTable.Rows(0).Item("UserName")
            Session("NameOfUser") = Login1.ReturnDataTable.Rows(0).Item("NameOfUser")
            Session("BranchId") = Login1.ReturnDataTable.Rows(0).Item("BranchId")
            Session("CheckerDateTime") = IIf(IsDBNull(Login1.ReturnDataTable.Rows(0).Item("CheckerValidDatetime")), Date.MinValue, Login1.ReturnDataTable.Rows(0).Item("CheckerValidDatetime"))
            Session("EmailId") = Login1.ReturnDataTable.Rows(0).Item("EmailId")
            Session("RestrictedAccess") = Login1.ReturnDataTable.Rows(0).Item("RestrictedAccess")

            OpenConn()
            'objCommon.OpenConn()
            'devendra
            'If (Session("UserTypeId") = "1" Or Session("UserTypeId") = "40" Or Session("UserTypeId") = "43" Or Session("UserTypeId") = "44" Or Session("UserTypeId") = "39" Or Session("UserTypeId") = "41" Or Session("UserTypeId") = "42" Or Session("UserTypeId") = "63" Or Session("UserTypeId") = "66") Then
            SetYearId()

            If IsDBNull(Login1.ReturnDataTable.Rows(0).Item("PasswordChangeDate")) Then
                'Login1.DestinationPageUrl = "ChangePassword.aspx?Flag=C"
                Response.Redirect("ChangePassword.aspx?Flag=C", False)
            ElseIf Login1.ReturnDataTable.Rows(0).Item("PasswordChangeDate") <= DateAdd(DateInterval.Day, -1 * 15, Today) Then
                Response.Redirect("ChangePassword.aspx?Flag=C", False)
                ' Login1.DestinationPageUrl = "ChangePassword.aspx?Flag=C"
            Else
                'Login1.DestinationPageUrl = "SelectYear.aspx"
                Response.Redirect("SelectYear.aspx", False)
            End If

            CreateXML()
            CheckLoginDetails()
            'Else
            'Response.Redirect("Login.aspx", False)
        End If
        'End If
    End Sub
    Sub CheckLoginDetails()
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand()
            sqlComm.Connection = sqlConn

            sqlComm.CommandText = "CHECK_User_LoginDetails"
            sqlComm.CommandType = CommandType.StoredProcedure
            objCommon.SetCommandParameters(sqlComm, "@LoginName", SqlDbType.VarChar, 50, "I", , , Session("UserName"))
            sqlComm.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            CloseConn()

        End Try
    End Sub
    Private Sub SetYearId()
        Try
            Dim dt As DataTable

            dt = objCommon.FillDataTable(clsCommonFuns.sqlConn, "ID_FILL_YearId")
            If dt.Rows.Count > 0 Then
                Session("YearId") = dt.Rows(0).Item(0)
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub CreateXML()
        Dim sqlComm As New SqlCommand()
        'sqlComm.Connection = clsCommonFuns.sqlConn
        OpenConn()
        sqlComm.Connection = sqlConn

        sqlComm.CommandText = "ID_Fill_MenuMaster"
        sqlComm.CommandType = CommandType.StoredProcedure
        objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Session("UserTypeId"))
        sqlComm.ExecuteNonQuery()
        Dim sqlDa As New SqlDataAdapter()
        Dim dt As New DataTable()
        Dim dt1 As New DataTable()
        sqlDa.SelectCommand = sqlComm
        sqlDa.Fill(dt)
        dt1 = dt

        Dim writer As New XmlDocument()
        Dim xmlDecl As XmlDeclaration
        xmlDecl = writer.CreateXmlDeclaration("1.0", "UTF-8", "")
        writer.AppendChild(xmlDecl)
        Dim xmlRoot As XmlElement = writer.CreateElement("menu")
        writer.AppendChild(xmlRoot)
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim xmlMenuItem As XmlElement = writer.CreateElement("menuitem")
            xmlRoot.AppendChild(xmlMenuItem)
            For j As Integer = 0 To dt.Columns.Count - 1
                Dim xmlAtt As XmlAttribute = writer.CreateAttribute(dt.Columns(j).ColumnName)
                xmlAtt.Value = dt.Rows(i)(j).ToString()
                xmlMenuItem.Attributes.Append(xmlAtt)
            Next
        Next
        writer.Save(SetMenu)
    End Sub

    Private Function SetMenu() As String

        If Session("UserTypeId") = 1 Or Session("UserTypeId") = 107 Then
            Return Server.MapPath(".") & "\Admin.xml"
        ElseIf Session("UserTypeId") = 39 Then
            Return Server.MapPath(".") & "\DEALER.xml"
        ElseIf Session("UserTypeId") = 40 Then
            Return Server.MapPath(".") & "\BACKOFFICE.xml"
        ElseIf Session("UserTypeId") = 41 Then
            Return Server.MapPath(".") & "\DEALERBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 42 Then
            Return Server.MapPath(".") & "\DEALERMANAGER.xml"
        ElseIf Session("UserTypeId") = 43 Then
            Return Server.MapPath(".") & "\BACKOFFICEBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 44 Then
            Return Server.MapPath(".") & "\BACKOFFICEMANAGER.xml"

        ElseIf Session("UserTypeId") = 66 Then
            Return Server.MapPath(".") & "\ADVANCEDEALER.xml"
        ElseIf Session("UserTypeId") = 63 Then
            Return Server.MapPath(".") & "\COMPLIANCE.xml"
            'New
        ElseIf Session("UserTypeId") = 69 Then
            Return Server.MapPath(".") & "\CHECKER.xml"
        ElseIf Session("UserTypeId") = 70 Then
            Return Server.MapPath(".") & "\MBDEALER.xml"
        ElseIf Session("UserTypeId") = 71 Then
            Return Server.MapPath(".") & "\MBBACKOFFICE.xml"
        ElseIf Session("UserTypeId") = 72 Then
            Return Server.MapPath(".") & "\MBDEALERBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 73 Then
            Return Server.MapPath(".") & "\MBDEALERMANAGER.xml"
        ElseIf Session("UserTypeId") = 74 Then
            Return Server.MapPath(".") & "\MBBACKOFFICEBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 75 Then
            Return Server.MapPath(".") & "\MBBACKOFFICEMANAGER.xml"
        ElseIf Session("UserTypeId") = 76 Then
            Return Server.MapPath(".") & "\MBCOMPLIANCE.xml"
            'ElseIf Session("UserTypeId") = 76 Then
            '    Return Server.MapPath(".") & "\MBCOMPLIANCE.xml"
        ElseIf Session("UserTypeId") = 77 Then
            Return Server.MapPath(".") & "\MBADVANCEDEALER.xml"
        ElseIf Session("UserTypeId") = 78 Then
            Return Server.MapPath(".") & "\MBCHECKER.xml"
        ElseIf Session("UserTypeId") = 79 Then
            Return Server.MapPath(".") & "\WDMDEALER.xml"
        ElseIf Session("UserTypeId") = 80 Then
            Return Server.MapPath(".") & "\WDMBACKOFFICE.xml"
        ElseIf Session("UserTypeId") = 81 Then
            Return Server.MapPath(".") & "\WDMDEALERBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 82 Then
            Return Server.MapPath(".") & "\WDMDEALERMANAGER.xml"
        ElseIf Session("UserTypeId") = 83 Then
            Return Server.MapPath(".") & "\WDMBACKOFFICEBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 84 Then
            Return Server.MapPath(".") & "\WDMBACKOFFICEMANAGER.xml"
        ElseIf Session("UserTypeId") = 85 Then
            Return Server.MapPath(".") & "\WDMCOMPLIANCE.xml"
        ElseIf Session("UserTypeId") = 86 Then
            Return Server.MapPath(".") & "\WDMADVANCEDEALER.xml"
        ElseIf Session("UserTypeId") = 87 Then
            Return Server.MapPath(".") & "\WDMCHECKER.xml"
        ElseIf Session("UserTypeId") = 88 Then
            Return Server.MapPath(".") & "\CRMDEALER.xml"
        ElseIf Session("UserTypeId") = 89 Then
            Return Server.MapPath(".") & "\CRMBACKOFFICE.xml"
        ElseIf Session("UserTypeId") = 90 Then
            Return Server.MapPath(".") & "\CRMDEALERBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 91 Then
            Return Server.MapPath(".") & "\CRMDEALERMANAGER.xml"
        ElseIf Session("UserTypeId") = 92 Then
            Return Server.MapPath(".") & "\CRMBACKOFFICEBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 93 Then
            Return Server.MapPath(".") & "\CRMBACKOFFICEMANAGER.xml"
        ElseIf Session("UserTypeId") = 94 Then
            Return Server.MapPath(".") & "\CRMCOMPLIANCE.xml"
        ElseIf Session("UserTypeId") = 95 Then
            Return Server.MapPath(".") & "\CRMADVANCEDEALER.xml"
        ElseIf Session("UserTypeId") = 96 Then
            Return Server.MapPath(".") & "\CRMCHECKER.xml"
        ElseIf Session("UserTypeId") = 97 Then
            Return Server.MapPath(".") & "\PMSDEALER.xml"
        ElseIf Session("UserTypeId") = 98 Then
            Return Server.MapPath(".") & "\PMSBACKOFFICE.xml"
        ElseIf Session("UserTypeId") = 99 Then
            Return Server.MapPath(".") & "\PMSDEALERBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 100 Then
            Return Server.MapPath(".") & "\PMSDEALERMANAGER.xml"
        ElseIf Session("UserTypeId") = 101 Then
            Return Server.MapPath(".") & "\PMSBACKOFFICEBRANCHMANAGER.xml"
        ElseIf Session("UserTypeId") = 102 Then
            Return Server.MapPath(".") & "\PMSBACKOFFICEMANAGER.xml"
        ElseIf Session("UserTypeId") = 103 Then
            Return Server.MapPath(".") & "\PMSCOMPLIANCE.xml"
        ElseIf Session("UserTypeId") = 104 Then
            Return Server.MapPath(".") & "\PMSADVANCEDEALER.xml"
        ElseIf Session("UserTypeId") = 105 Then
            Return Server.MapPath(".") & "\PMSCHECKER.xml"
        ElseIf Session("UserTypeId") = 67 Then
            Return Server.MapPath(".") & "\USER.xml"
        ElseIf Session("UserTypeId") = 106 Then
            Return Server.MapPath(".") & "\OTHER.xml"
        End If

    End Function

#Region "SetFocus"
    Private Sub SetFocus(ByVal ctrl As System.Web.UI.Control)
        Dim s As String = "<SCRIPT language='javascript'>document.getElementById('" & ctrl.ID & "').focus()</SCRIPT>"
        RegisterStartupScript("focus", s)
    End Sub
#End Region

 
End Class
