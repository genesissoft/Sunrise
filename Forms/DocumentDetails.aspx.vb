Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_DocumentDetails
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            'If clsCommonFuns.sqlConn Is Nothing Then objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            ''Response.Filter = New WhitespaceFilter(Response.Filter)
            If IsPostBack = False Then
                vws_DocumentType.Columns.Add("DocumentName")
                vws_DocumentType.Columns.Add("DocumentId")

                vws_DocumentType.ColumnWidths.Add(250)
                vws_DocumentType.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
