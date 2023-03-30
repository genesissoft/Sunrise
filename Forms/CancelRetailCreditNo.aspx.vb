Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_CancelRetailCreditNo
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As SqlConnection


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

                'srh_CeditRefNo.Columns.Add("CreditRefNo")
                'srh_CeditRefNo.Columns.Add("CreditRefNo")
                'Srh_Broker.Columns.Add("BrokerName")
                'Srh_Broker.Columns.Add("BrokerId")

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

    Protected Sub btn_DeleteDeal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_DeleteDeal.Click
        Try
            Dim sqlTrans As SqlTransaction
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction
            If DeleteCreditRefNo(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()
            lbl_Deleted.Text = ""
            lbl_Deleted.Text = "This Credit Ref. No. is Cancelled successfully"
            Srh_Broker.SearchTextBox.Text = ""
            srh_CeditRefNo.SearchTextBox.Text = ""
            txt_Remark.Text = ""
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function DeleteCreditRefNo(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            'OpenConn()
            Dim sqlComm As New SqlCommand
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_Cancel_RetCreditRefNo"
            sqlComm.Connection = sqlConn
            sqlComm.Transaction = sqlTrans
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.Int, 4, "I", , , Val(Srh_Broker.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@CancelRemark", SqlDbType.VarChar, 500, "I", , , Trim(txt_Remark.Text))
            objCommon.SetCommandParameters(sqlComm, "@RefNo", SqlDbType.VarChar, 20, "I", , , Trim(srh_CeditRefNo.SearchTextBox.Text))
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            sqlTrans.Rollback()
        Finally
            'CloseConn()
        End Try
    End Function

    Protected Sub srh_CeditRefNo_ButtonClick() Handles srh_CeditRefNo.ButtonClick
        Try

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
