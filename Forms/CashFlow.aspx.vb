Imports System.Data
Imports System.Data.SqlClient

Imports Microsoft.ApplicationBlocks.ExceptionManagement
Partial Class Forms_CashFlow
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim errorPage As String = Convert.ToString(ConfigurationManager.AppSettings("errorPageUrl"))
    Dim sqlConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'objCommon.OpenConn()
            If IsPostBack = False Then
                FillGrid()
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
    Private Function AddColumnInfo(ByVal strParam As String) As DataColumn
        Try
            Dim dc As DataColumn
            dc = New DataColumn
            dc.ColumnName = strParam
            dc.DataType = GetType(String)
            Return dc
        Catch ex As Exception
             Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Sub FillGrid()
        Try
            Dim dc As DataColumn
            Dim dt As New DataTable
            Dim I As Integer
            Dim dtRow As DataRow
            Dim arrDate() As String
            Dim arrAmt() As String
            dc = AddColumnInfo("Date")
            dt.Columns.Add(dc)
            dc = AddColumnInfo("Amount")
            dt.Columns.Add(dc)
            arrDate = Split(Request.QueryString("Date"), "!")
            arrAmt = Split(Request.QueryString("Amount"), "!")
            For I = 0 To arrDate.Length - 1
                If Trim(arrDate(I)) = CStr(Date.MinValue) Then Exit For
                dtRow = dt.NewRow
                dtRow.Item(0) = Format(CDate(arrDate(I)), "dd/MM/yyyy")
                dtRow.Item(1) = Format(objCommon.DecimalFormat(arrAmt(I) & ""), "################0.000000")
                dt.Rows.Add(dtRow)
            Next
            grdView.DataSource = dt
            grdView.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
