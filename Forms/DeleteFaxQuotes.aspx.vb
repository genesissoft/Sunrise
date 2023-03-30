Imports System.Data
Imports System.Data.SqlClient
Partial Class From_DeleteFaxQuotes
    Inherits System.Web.UI.Page
    Dim dt3 As DataTable
    Dim sqlConn As SqlConnection


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
                lbl_Deleted.Visible = False
                FillBlankQuoteGrid()
                FillQuoteGrid()

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

    Private Sub FillBlankQuoteGrid()
        Try
            Dim dtSGLGrid As New DataTable
            dtSGLGrid.Columns.Add("QuoteName", GetType(String))
            dtSGLGrid.Columns.Add("SavedDate", GetType(String))
            dtSGLGrid.Columns.Add("FaxQuoteId", GetType(String))
            Session("FaxQuote") = dtSGLGrid
            dg_Quote.DataSource = dtSGLGrid
            dg_Quote.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillQuoteGrid()
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable(sqlConn, "ID_SELECT_FaxQuoteNames", Session("UserId"), "UserId")
            Session("FaxQuote") = dt
            dg_Quote.DataSource = dt
            dg_Quote.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub btn_Ok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Ok.Click
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_FaxQuote"
            sqlComm.Connection = sqlConn
            For i As Int16 = 0 To dg_Quote.Items.Count - 1
                Dim chk As CheckBox
                chk = CType(dg_Quote.Items(i).FindControl("chk_Select"), CheckBox)
                If chk.Checked = True Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@FaxQuoteId", SqlDbType.Int, 4, "I", , , Val(CType(dg_Quote.Items(i).FindControl("lbl_FaxQuoteId"), Label).Text))
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            FillQuoteGrid()
            lbl_Deleted.Visible = True
            lbl_Deleted.Text = "Fax Quote Deleted successfully"

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
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