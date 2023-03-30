Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Partial Class Forms_RetailCancelDebitNo
    Inherits System.Web.UI.Page
    Dim PgName As String = "$RetailCancelDebitNo$"
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As SqlConnection
    Dim objUtil As New Util

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Session("username") = "") Then
                Response.Redirect("Login.aspx")
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
                'SetAttributes()
                btn_DeleteDeal.Attributes.Add("onclick", "return Validation();")
                'srh_DebitRefNo.Columns.Add("DebitRefNo")
                'srh_DebitRefNo.Columns.Add("DebitRefNo")

                'Srh_NameOFClient.Columns.Add("CustomerName")
                'Srh_NameOFClient.Columns.Add("CustomerId")

            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex)
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

    Protected Sub btn_DeleteDeal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_DeleteDeal.Click
        Try
            Dim sqlTrans As SqlTransaction
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction
            If DeleteDebitRefNo(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()
            lbl_Deleted.Text = ""
            lbl_Deleted.Text = "This Debit Ref. No. is Cancelled successfully"
            srh_DebitRefNo.SelectedId = 0
            srh_DebitRefNo.SearchTextBox.Text = ""
            Srh_NameOFClient.SearchTextBox.Text = ""
            txt_Remark.Text = ""
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_DeleteDeal_Click", "Error in btn_DeleteDeal_Click", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function DeleteDebitRefNo(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            'OpenConn()
            Dim sqlComm As New SqlCommand
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_Cancel_RetDebitRefNo"
            sqlComm.Connection = sqlConn
            sqlComm.Transaction = sqlTrans
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , Val(Srh_NameOFClient.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@CancelRemark", SqlDbType.VarChar, 500, "I", , , Trim(txt_Remark.Text))
            objCommon.SetCommandParameters(sqlComm, "@RefNo", SqlDbType.VarChar, 20, "I", , , Trim(srh_DebitRefNo.SearchTextBox.Text))
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "DeleteDebitRefNo", "Error in DeleteDebitRefNo", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            sqlTrans.Rollback()
        Finally
            'CloseConn()
        End Try
    End Function

    Protected Sub Srh_NameOFClient_ButtonClick() Handles Srh_NameOFClient.ButtonClick
        srh_DebitRefNo.SelectedId = 0
        srh_DebitRefNo.SearchTextBox.Text = ""
    End Sub
End Class
