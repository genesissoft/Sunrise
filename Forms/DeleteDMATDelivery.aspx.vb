Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class Forms_DeleteDMATDelivery
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim arrIssueDetailIds() As String
    Dim sqlComm As New SqlCommand
    Dim sqlConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            'objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            btn_Delete.Attributes.Add("onclick", "return validation();")
            'btn_Delete.Attributes.Add("onclick", "return Delete_entry();")
            If IsPostBack = False Then
                Session("ClientName") = ""

                Session("DematInfoTable") = ""
                Hid_Id.Value = "A"
                SetAttributes()
                SetControls()
                Hid_DematAccTo.Value = Trim(Request.QueryString("Hid_DematAccTo.value") & "")
                If Val(Request.QueryString("Id") & "") <> 0 Then
                    ViewState("Id") = Val(Request.QueryString("Id") & "")
                    srh_TransCode.SelectedId = ViewState("Id")
                    FillFields()
                    btn_Delete.Visible = False
                    ClearFields()
                End If
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "show", "SlipType();", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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

    Private Sub SetAttributes()

        btn_Delete.Attributes.Add("onclick", "return validation();")

    End Sub
   

  

    Private Sub FillFields()
        Try
            Dim dt As DataTable
            Dim i As Integer
            Dim balamt As Integer = 0
            Dim totalamt As Integer
            OpenConn()
            If rdo_DateType.SelectedValue = "D" Then
                dt = objCommon.FillDataTable(sqlConn, "Id_FILL_DematDelivery", srh_TransCode.SelectedId, "DealSlipID")
            Else
                dt = objCommon.FillDataTable(sqlConn, "Id_FILL_FinancialDelivery", srh_FinanCode.SelectedId, "DealSlipID")

            End If
            If dt.Rows.Count > 0 Then
                Hid_TransType.Value = Trim(dt.Rows(0).Item("TransType").ToString)
                txt_ClientName.Text = Trim(dt.Rows(0).Item("CustomerName").ToString)
                Session("ClientName") = Trim(dt.Rows(0).Item("CustomerName").ToString)
                If rdo_DateType.SelectedValue = "D" Then
                    txt_FaceValue.Text = Val(dt.Rows(0).Item("FV") & "")
                Else
                    txt_FaceValue.Text = Val(dt.Rows(0).Item("FaceValue") & "")
                End If
                txt_IssuerOfSecurity.Text = Trim(dt.Rows(0).Item("SecurityIssuer") & "")
                txt_SecurityName.Text = Trim(dt.Rows(0).Item("SecurityName") & "")
                txt_SettlmtDate.Text = Format(dt.Rows(0).Item("SettmentDate"), "dd/MM/yyyy")
                txt_SettlmntAmt.Text = Val(dt.Rows(0).Item("SettlementAmt") & "")
                Hid_CustomerId.Value = Val(dt.Rows(0).Item("CustomerID") & "")
                Hid_DealSlipId.Value = Val(dt.Rows(0).Item("DealSlipID") & "")
                srh_TransCode.SelectedId = Val(dt.Rows(0).Item("DealSlipID") & "")
                srh_TransCode.SearchTextBox.Text = Trim(dt.Rows(0).Item("DealSlipNo") & "")
                Cbo_FaceValue.SelectedValue = Val(dt.Rows(0).Item("FaceValueMultiple") & "")
                txt_TransDate.Text = Format(dt.Rows(0).Item("DealDate"), "dd/MM/yyyy")
                If rdo_DateType.SelectedValue = "D" Then
                    Hid_DematInfoId.Value = Val(dt.Rows(0).Item("Dmatinfoid") & "")
                End If

                Hid_PayMode.Value = Trim(dt.Rows(0).Item("ModeOFPayment") & "")
             
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub SetControls()
        Try
            'srh_TransCode.Columns.Add("DealSlipNo")
            'srh_TransCode.Columns.Add("CustomerName")
            'srh_TransCode.Columns.Add("CONVERT(VARCHAR,SettmentDate,103) As SettlementDate")
            '' srh_TransCode.Columns.Add("FaceValue")
            'srh_TransCode.Columns.Add("DealSlipId")

            'srh_FinanCode.Columns.Add("DealSlipNo")
            'srh_FinanCode.Columns.Add("CustomerName")
            'srh_FinanCode.Columns.Add("CONVERT(VARCHAR,SettmentDate,103) As SettlementDate")
            '' srh_TransCode.Columns.Add("FaceValue")
            'srh_FinanCode.Columns.Add("DealSlipId")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Protected Sub srh_TransCode_ButtonClick() Handles srh_TransCode.ButtonClick
        Try
           
            FillFields()
            srh_TransCode.SearchButton.Visible = True
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub SetSaveUpdate()
        Try
            OpenConn()
            Dim sqlTrans As SqlTransaction
            sqlTrans = sqlConn.BeginTransaction
            If rdo_DateType.SelectedValue = "D" Then
                If DeleteDematDetails(sqlTrans) = False Then Exit Sub
            Else
                If DeleteFinanDetails(sqlTrans) = False Then Exit Sub
            End If
            sqlTrans.Commit()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
   
    Protected Sub btn_Delete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Delete.Click
        Try
            SetSaveUpdate()
            Dim strHtml As String
            Dim msg As String = "Slip Deleted successfully"
            strHtml = "alert('" + msg + "');"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", strHtml, True)
            btn_Delete.Visible = True
            ClearFields()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub

    Private Sub ClearFields()

        txt_ClientName.Text = ""
        txt_FaceValue.Text = ""
        txt_IssuerOfSecurity.Text = ""
        txt_SecurityName.Text = ""
        txt_SettlmntAmt.Text = ""
        txt_SettlmtDate.Text = ""

        txt_TransDate.Text = ""

        If rdo_DateType.SelectedValue = "D" Then
            srh_TransCode.SearchTextBox.Text = ""
            txt_DmatRemark.Text = ""
        Else
            srh_FinanCode.SearchTextBox.Text = ""
            txt_FinanRemark.Text = ""
        End If
    End Sub

    Private Function DeleteDematDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_CancelDMat"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(Hid_DealSlipId.Value))
            objCommon.SetCommandParameters(sqlComm, "@CancelRemark", SqlDbType.VarChar, 500, "I", , , Trim(txt_DmatRemark.Text))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function DeleteFinanDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_CancelFinanEntry"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(Hid_DealSlipId.Value))
            objCommon.SetCommandParameters(sqlComm, "@CancelRemark", SqlDbType.VarChar, 500, "I", , , Trim(txt_FinanRemark.Text))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

   
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub

    Protected Sub srh_FinanCode_ButtonClick() Handles srh_FinanCode.ButtonClick
        Try
            FillFields()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub

    Protected Sub rdo_DateType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdo_DateType.SelectedIndexChanged
        Try
            ClearFields()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

        End Try
    End Sub
End Class
