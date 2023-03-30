Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_Canceldeals
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim sqlConn As SqlConnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
            'Srh_DealNumber.Columns.Add("DealSlipNo")
            'Srh_DealNumber.Columns.Add("CustomerName")
            'Srh_DealNumber.Columns.Add("DealSlipId")

            'Srh_MergeTrnsCode.Columns.Add("MergedealNo")
            'Srh_MergeTrnsCode.Columns.Add("dbo.FA_GET_DealNo_New1(MergedealNo) as DealNo")
            'Srh_MergeTrnsCode.Columns.Add("MergedealNo")

            btn_DeleteDeal.Visible = False
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillDealSlipFields()
        Try
            Dim dt As DataTable
            OpenConn()
            If rdo_DateType.SelectedValue = "M" Then
                dt = objCommon.FillDataTable(sqlConn, "Id_FILL_CencelDeal", ViewState("Id"), "DealSlipID")
            ElseIf rdo_DateType.SelectedValue = "P" Then
                dt = objCommon.FillDataTable1(sqlConn, "Id_FILL_CencelDeal", Val(Srh_ConvertPending.SelectedId), "DealSlipID")
            Else
                dt = objCommon.FillDataTable1(sqlConn, "Id_FILL_CencelDeal", Val(Srh_DealNumber.SelectedId), "DealSlipID")
            End If

            If dt.Rows.Count > 0 Then
                lit_Issuer.Text = Trim(dt.Rows(0).Item("SecurityIssuer") & "")
                lit_SecurityName.Text = Trim(dt.Rows(0).Item("SecurityName") & "")
                lit_CustName.Text = Trim(dt.Rows(0).Item("CustomerName").ToString)
                lit_FaceValue.Text = Val(dt.Rows(0).Item("FaceValue") & "") * Val(dt.Rows(0).Item("FaceValueMultiple") & "")
                If Trim((dt.Rows(0).Item("DealDate")) & "") <> "" Then
                    lit_DealDate.Text = Format(dt.Rows(0).Item("DealDate"), "dd/MM/yyyy")
                End If
                If Trim((dt.Rows(0).Item("SettmentDate")) & "") <> "" Then
                    lit_SettlementDate.Text = Format(dt.Rows(0).Item("SettmentDate"), "dd/MM/yyyy")
                End If
                If Trim((dt.Rows(0).Item("Rate")) & "") <> "" Then
                    lit_Rate.Text = Format(dt.Rows(0).Item("Rate"), "")
                End If
                'srh_TransCode.SearchTextBox.Text = Trim(dt.Rows(0).Item("DealSlipNo") & "")
                'srh_TransCode.SelectedId = Val(dt.Rows(0).Item("DealSlipId") & "")
                Hid_DealSlipId.Value = Val(dt.Rows(0).Item("DealSlipId") & "")
                lit_ModeofDelivery.Text = Trim(dt.Rows(0).Item("ModeofDelivery") & "")
                If Trim(dt.Rows(0).Item("ModeofDelivery") & "") = "D" Then
                    lit_ModeofDelivery.Text = "DEMAT"
                Else
                    lit_ModeofDelivery.Text = "SGL"
                End If
                If Trim(dt.Rows(0).Item("ModeOFPayment") & "") = "H" Then
                    lit_PaymentMode.Text = "High Value Cheque"
                ElseIf Trim(dt.Rows(0).Item("ModeOFPayment") & "") = "T" Then
                    lit_PaymentMode.Text = "HDFC Transfer Cheque"
                ElseIf Trim(dt.Rows(0).Item("ModeOFPayment") & "") = "P" Then
                    lit_PaymentMode.Text = "Pay Order Non High Value Cheque"
                ElseIf Trim(dt.Rows(0).Item("ModeOFPayment") & "") = "B" Then
                    lit_PaymentMode.Text = "Bank Draft"
                ElseIf Trim(dt.Rows(0).Item("ModeOFPayment") & "") = "E" Then
                    lit_PaymentMode.Text = "ETF"
                ElseIf Trim(dt.Rows(0).Item("ModeOFPayment") & "") = "R" Then
                    lit_PaymentMode.Text = "RTGS"
                Else
                    lit_PaymentMode.Text = "DVP"
                End If
                lit_Contact.Text = Trim(dt.Rows(0).Item("ContactPerson") & "")
                Hid_TransType.Value = Trim(dt.Rows(0).Item("TransType") & "")

                Hid_DealTransType.Value = Trim(dt.Rows(0).Item("DealTransType") & "")
                Hid_FinancialDealType.Value = Trim(dt.Rows(0).Item("FinancialDealType") & "")


                Hid_Frequency.Value = GetFrequency(Trim(dt.Rows(0).Item("FrequencyOfInterest") & ""))
                'Hid_DealType.Value = Trim(dt.Rows(0).Item("DealType") & "")

            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub


    Protected Sub srh_TransCode_ButtonClick() Handles Srh_DealNumber.ButtonClick
        Try
            btn_DeleteDeal.Visible = True
            FillDealSlipFields()
            lbl_Deleted.Text = ""

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function GetFrequency(ByVal strFrequency As String) As Int16
        Try
            Select Case UCase(strFrequency)
                Case "Y"
                    Return 1
                Case "H"
                    Return 2
                Case "Q"
                    Return 4
                Case "M"
                    Return 12
                Case "N"
                    Return 0
            End Select
        Catch ex As Exception

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_DeleteDeal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_DeleteDeal.Click
        SetSaveUpdate("ID_Update_CancelDealSlipEntry", True)
        btn_DeleteDeal.Visible = False
        txt_Remark.Text = ""
    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String, Optional ByVal blnRedirect As Boolean = True)
        Try
            Dim sqlTrans As SqlTransaction
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction

            If rdo_DateType.SelectedValue = "P" Then
                If CheckSaleDeal(sqlTrans) = False Then Exit Sub
                If ConvertToPending(sqlTrans) = False Then Exit Sub
            Else
                If (Hid_TransType.Value = "P") Then
                    If Hid_DealTransType.Value <> "F" Then
                        If CheckSaleDeal(sqlTrans) = False Then Exit Sub
                        If CancelDeal(sqlTrans) = False Then Exit Sub
                    End If
                End If

                If Hid_TransType.Value = "S" Then
                    If DeleteProfit(sqlTrans, strProc) = False Then Exit Sub
                End If

            End If
            sqlTrans.Commit()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function ConvertToPending(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_ConvertToPending_deal"

            sqlComm.Connection = sqlConn
            sqlComm.Transaction = sqlTrans
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "I", , , Val(Srh_ConvertPending.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@EntryUserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            lbl_Deleted.Visible = True
            lbl_Deleted.Text = ""
            lbl_Deleted.Text = "This Deal converted to pending successfully"
            Return True
        Catch ex As Exception


            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            sqlTrans.Rollback()

        End Try
    End Function
    Private Function DeleteProfitFinancial(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = "ID_DELETE_PurchaseProfitUpdate_cancel12"
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure

            sqlComm.Connection = sqlConn

            'sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@PurcDealSlipId", SqlDbType.Int, 4, "I", , , Val(Srh_DealNumber.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@CancelRemark1", SqlDbType.VarChar, 500, "I", , , Trim(txt_Remark.Text))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            lbl_Deleted.Text = ""
            lbl_Deleted.Text = "This Deal is Cancelled successfully"
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

        End Try
    End Function




    Private Function DeleteProfit(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand

            If Hid_DealTransType.Value = "B" Then
                sqlComm.CommandText = "ID_DELETE_Profit_cancel_broking"
            Else
                sqlComm.CommandText = "ID_DELETE_Profit_cancel"

            End If


            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure

            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@SellDealSlipId", SqlDbType.Int, 4, "I", , , Val(Srh_DealNumber.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@CancelRemark1", SqlDbType.VarChar, 500, "I", , , Trim(txt_Remark.Text))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            lbl_Deleted.Text = ""
            lbl_Deleted.Text = "This Deal is Cancelled successfully"

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

        End Try
    End Function
    Private Function DeleteProfitFianancial(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = "ID_DELETE_Profit_cancel1"
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure

            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@SellDealSlipId", SqlDbType.Int, 4, "I", , , Val(Srh_DealNumber.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@CancelRemark1", SqlDbType.VarChar, 500, "I", , , Trim(txt_Remark.Text))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            lbl_Deleted.Text = ""
            lbl_Deleted.Text = "This Deal is Cancelled successfully"

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

        End Try
    End Function



    Private Function SaveCancelDeal(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure

            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "I", , , Val(Srh_DealNumber.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@CancelRemark1", SqlDbType.VarChar, 500, "I", , , Trim(txt_Remark.Text))

            sqlComm.ExecuteNonQuery()
            lbl_Deleted.Text = ""
            lbl_Deleted.Text = "This Deal is Cancelled successfully"
            Return True
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

        End Try
    End Function
    Private Function CheckSaleDeal(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim Count As Integer
            Dim DealSlipNo As String
            sqlComm.CommandType = CommandType.StoredProcedure

            sqlComm.CommandText = "ID_FILL_CheckSaleDeal"

            sqlComm.Connection = sqlConn
            sqlComm.Transaction = sqlTrans
            sqlComm.Parameters.Clear()

            If rdo_DateType.SelectedValue = "P" Then
                objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "I", , , Val(Srh_ConvertPending.SelectedId))
            Else
                objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "I", , , Val(Srh_DealNumber.SelectedId))
            End If
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@Cnt", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@DealSlipNo", SqlDbType.VarChar, 500, "O")
            sqlComm.ExecuteNonQuery()
            Count = sqlComm.Parameters.Item("@Cnt").Value
            DealSlipNo = sqlComm.Parameters.Item("@DealSlipNo").Value
            If Count > 0 Then
                lbl_Deleted.Text = "You Can't cancel/convert This Because Its Sale No " + Trim(DealSlipNo & "") + " Exist."
                sqlTrans.Rollback()
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            sqlTrans.Rollback()

        End Try
    End Function

    Private Function CheckMergeDeal(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim Count As Integer
            Dim DealSlipNo As String

            sqlComm.CommandType = CommandType.StoredProcedure

            sqlComm.CommandText = "ID_FILL_CheckMergeDeal"

            sqlComm.Connection = sqlConn
            sqlComm.Transaction = sqlTrans
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "I", , , Val(Srh_DealNumber.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@Cnt", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@DealSlipNo", SqlDbType.VarChar, 500, "O")
            sqlComm.ExecuteNonQuery()
            Count = sqlComm.Parameters.Item("@Cnt").Value
            DealSlipNo = sqlComm.Parameters.Item("@DealSlipNo").Value
            If Count > 0 Then
                lbl_Deleted.Text = "You Can't Delete This Because Its Merge deal " + Trim(DealSlipNo & "") + " Exist."
                sqlTrans.Rollback()
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            sqlTrans.Rollback()

        End Try
    End Function

    Private Function CheckPurDeal(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim Count As Integer
            Dim DealSlipNo As String
            sqlComm.CommandType = CommandType.StoredProcedure

            sqlComm.CommandText = "ID_FILL_CheckPurDeal"

            sqlComm.Connection = sqlConn
            sqlComm.Transaction = sqlTrans
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "I", , , Val(Srh_DealNumber.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@Cnt", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@DealSlipNo", SqlDbType.VarChar, 500, "O")
            sqlComm.ExecuteNonQuery()
            Count = sqlComm.Parameters.Item("@Cnt").Value
            DealSlipNo = sqlComm.Parameters.Item("@DealSlipNo").Value
            If Count > 0 Then
                lbl_Deleted.Text = "You Can't Delete This Because Its pur No " + Trim(DealSlipNo & "") + " Exist."
                sqlTrans.Rollback()
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            sqlTrans.Rollback()

        End Try
    End Function

    Private Function CheckPurFinancialDeal(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim Count As Integer
            Dim DealSlipNo As String
            sqlComm.CommandType = CommandType.StoredProcedure

            sqlComm.CommandText = "ID_FILL_CheckPurDealFinancial"

            sqlComm.Connection = sqlConn
            sqlComm.Transaction = sqlTrans
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "I", , , Val(Srh_DealNumber.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@Cnt", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@DealSlipNo", SqlDbType.VarChar, 500, "O")
            sqlComm.ExecuteNonQuery()
            Count = sqlComm.Parameters.Item("@Cnt").Value


            If Count > 0 Then
                DealSlipNo = sqlComm.Parameters.Item("@DealSlipNo").Value
                lbl_Deleted.Text = "You Can't Delete This Because Its pur No " + Trim(DealSlipNo & "") + " Exist."
                sqlTrans.Rollback()
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            sqlTrans.Rollback()

        End Try
    End Function


    Private Function CancelDeal(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_Cancel_deal"

            sqlComm.Connection = sqlConn
            sqlComm.Transaction = sqlTrans
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "I", , , Val(Srh_DealNumber.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@CancelRemark1", SqlDbType.VarChar, 500, "I", , , Trim(txt_Remark.Text))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            lbl_Deleted.Visible = True
            lbl_Deleted.Text = ""
            lbl_Deleted.Text = "This Deal is Cancelled successfully"
            Return True
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            sqlTrans.Rollback()

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

    Protected Sub Srh_MergeTrnsCode_ButtonClick() Handles Srh_MergeTrnsCode.ButtonClick
        Try
            btn_DeleteDeal.Visible = True
            GetdealslipIds()
            'FillDealSlipFields()
            btn_DeleteDeal.Visible = True
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub GetdealslipIds()
        Try
            Dim dtDealId As DataTable
            Dim DealIds As Double
            Dim Flag As Boolean
            Dim sqlComm As New SqlCommand()
            OpenConn()
            sqlComm.Connection = sqlConn
            sqlComm.CommandText = "ID_Get_Dealids"
            sqlComm.CommandType = CommandType.StoredProcedure
            objCommon.SetCommandParameters(sqlComm, "@MergedealNo", SqlDbType.VarChar, 30, "I", , , Srh_MergeTrnsCode.SearchTextBox.Text)
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            Dim sqlDa As New SqlDataAdapter()
            Dim dt As New DataTable()
            Dim dt1 As New DataTable()
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dt)
            dtDealId = dt
            Session("dealentrytable") = dt
            For I As Int16 = 0 To dtDealId.Rows.Count - 1
                ViewState("Id") = Val(dtDealId.Rows(I).Item("Dealslipid"))
                If Flag = False Then
                    FillDealSlipFields()
                    Flag = True
                End If
                'SetSaveUpdate("ID_Update_CancelDealSlipEntry", True)
            Next

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try


    End Sub

    Private Function CancelMergeDeal(ByVal sqlTrans As SqlTransaction) As Boolean
        Try

            Dim sqlComm As New SqlCommand
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_Cancel_Mergedeal"
            sqlComm.Connection = sqlConn
            sqlComm.Transaction = sqlTrans
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@MergedealNo", SqlDbType.VarChar, 50, "I", , , Trim(Srh_MergeTrnsCode.SearchTextBox.Text))
            objCommon.SetCommandParameters(sqlComm, "@Remark", SqlDbType.VarChar, 500, "I", , , Trim(txt_Remark.Text))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 2, "O")
            sqlComm.ExecuteNonQuery()
            lbl_Deleted.Visible = True
            lbl_Deleted.Text = ""
            lbl_Deleted.Text = "Merge Deal is Cancelled successfully"
            Return True
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            sqlTrans.Rollback()

        End Try
    End Function

    Private Sub Srh_ConvertPending_ButtonClick(sender As Object, e As EventArgs) Handles Srh_ConvertPending.ButtonClick
        Try
            btn_DeleteDeal.Visible = True
            FillDealSlipFields()
            lbl_Deleted.Text = ""

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub rdo_DateType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdo_DateType.SelectedIndexChanged
        If rdo_DateType.SelectedValue = "P" Then
            btn_DeleteDeal.Text = "Convert"
        Else
            btn_DeleteDeal.Text = "Delete"
        End If
    End Sub
End Class

