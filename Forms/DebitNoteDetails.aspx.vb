Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_DebitNoteDetails
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

            If IsPostBack = False Then
                vws_DebitNote.Columns.Add("IssuerName")
                vws_DebitNote.Columns.Add("DebitNoteNo")

                vws_DebitNote.Columns.Add("IssueName")
                vws_DebitNote.Columns.Add("DebitNoteId")
                vws_DebitNote.ColumnWidths.Add(250)
                vws_DebitNote.ColumnWidths.Add(150)

                vws_DebitNote.ColumnWidths.Add(250)
                vws_DebitNote.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
