Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_UserDetail
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'If Val(Session("UserId") & "") = 0 Then
            '    Response.Redirect("Login.aspx", False)
            '    Exit Sub
            'End If
            'objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            ''Response.Filter = New WhitespaceFilter(Response.Filter)
            If IsPostBack = False Then
                vws_UserMaster.Columns.Add("NameOfUser AS UserName")
                vws_UserMaster.Columns.Add("UT.UserTypeName")

                vws_UserMaster.Columns.Add("UserName as LoginName")
                vws_UserMaster.Columns.Add("BranchName")
                vws_UserMaster.Columns.Add("MobileNo")
                vws_UserMaster.Columns.Add("UM.UserId")

                vws_UserMaster.ColumnWidths.Add(150)
                vws_UserMaster.ColumnWidths.Add(150)
                vws_UserMaster.ColumnWidths.Add(150)
                vws_UserMaster.ColumnWidths.Add(100)
                vws_UserMaster.ColumnWidths.Add(100)
                vws_UserMaster.ColumnWidths.Add(50)
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

End Class
