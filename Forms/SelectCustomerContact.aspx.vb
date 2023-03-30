Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_SelectCustomerContact
    Inherits System.Web.UI.Page
    Dim sqlConn As SqlConnection
    Dim objCommon As New clsCommonFuns
    Dim intGridCols As Int16

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
                FillList()
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    Private Function BuildCondition() As String
        Try
            Dim strCond As String = ""
            Dim strIds As String
            strIds = Mid(Request.QueryString("Values"), 1, Request.QueryString("Values").Length - 1).Replace("!", ",")
            strCond = " WHERE CM.CustomerId IN (" & strIds & ") "
            Return strCond
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Sub FillList()
        Try
            Dim dt As DataTable
            Dim objSrh As New clsSearch
            Dim strColList As String
            strColList = " Customername + '/' + ContactPerson AS CustomerName, CM.CustomerId, ContactDetailId "

            OpenConn()
            dt = objSrh.FillDataTable(sqlConn, "ID_SEARCH_CustomerContactPerson", strColList, BuildCondition())
            chkList_Select.DataSource = dt
            chkList_Select.DataTextField = "CustomerName"
            chkList_Select.DataValueField = "ContactDetailId"
            chkList_Select.DataBind()
            GetBlankCustIds(dt)
            If chkList_Select.Items.Count = 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "return", "Close('');", True)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub btn_Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Submit.Click
        Try
            Dim strValues As String = ""
            For I As Int16 = 0 To chkList_Select.Items.Count - 1
                If chkList_Select.Items(I).Selected = True Then
                    strValues = strValues & chkList_Select.Items(I).Value & "!"
                End If
            Next
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "return", "Close('" & strValues & "')", True)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub GetBlankCustIds(ByVal dt As DataTable)
        Try
            Dim I As Int16
            Dim J As Int16
            While I <= chkList_Select.Items.Count - 1
                If Val(dt.Rows(J).Item("ContactDetailId") & "") = 0 Then
                    Hid_ReturnValues.Value += dt.Rows(I).Item("CustomerId") & "!"
                    chkList_Select.Items.RemoveAt(I)
                    I = I - 1
                End If
                I = I + 1
                J = J + 1
            End While
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

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
End Class