Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_CancelRetailDebitNote
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As SqlConnection

    Private Sub Forms_CancelRetailDebitNote_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            'objCommon.OpenConn() DFD
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            btn_DeleteDeal.Attributes.Add("onclick", "return  Validation();")
            Hid_UserId.Value = Val(Session("UserId"))
            Hid_UserTypeId.Value = Val(Session("UsertypeId"))
            If IsPostBack = False Then
                SetControls()
                SetAttributes()    'FillDealSlipFields()
            End If
        Catch ex As Exception

        End Try
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "show", "DateType();", True)

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
    Private Sub SetAttributes()
        txt_Remark.Attributes.Add("onblur", "ConvertUCase(this);")
    End Sub
    Private Sub SetControls()
        Try
            'btn_DeleteDeal.Visible = False
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub SetSaveUpdate()
        Try
            Dim sqlTrans As SqlTransaction
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction
            If CancelInvoice(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub btn_DeleteDeal_Click(sender As Object, e As EventArgs) Handles btn_DeleteDeal.Click
        SetSaveUpdate()
        btn_DeleteDeal.Visible = False
        txt_Remark.Text = ""
        Srh_RetailDebitNo.SearchTextBox.Text = ""
    End Sub
    Private Function CancelInvoice(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_Cancel_RetDebitRefNo"

            sqlComm.Connection = sqlConn
            sqlComm.Transaction = sqlTrans
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.Int, 4, "I", , , Val(Hid_BrokerId.Value))
            objCommon.SetCommandParameters(sqlComm, "@RefNo", SqlDbType.VarChar, 50, "I", , , Trim(Srh_RetailDebitNo.SearchTextBox.Text))
            objCommon.SetCommandParameters(sqlComm, "@CancelRemark", SqlDbType.VarChar, 500, "I", , , Trim(txt_Remark.Text))
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Session("CompId"))
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Session("YearId"))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            lbl_Deleted.Visible = True
            lbl_Deleted.Text = ""
            lbl_Deleted.Text = "This Invoice is Cancelled successfully"
            Return True
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            sqlTrans.Rollback()

        End Try
    End Function

    Private Sub Srh_RetailDebitNo_ButtonClick(sender As Object, e As EventArgs) Handles Srh_RetailDebitNo.ButtonClick
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_BrokerMasterNew", Srh_RetailDebitNo.SelectedId, "BrokerId")
            If dt.Rows.Count > 0 Then
                Hid_BrokerId.Value = Val(dt.Rows(0).Item("BrokerId") & "")
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
