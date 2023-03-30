Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_DeleteDeal
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim dblPepCoupRate As Double
   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Session("username") = "") Then
                Response.Redirect("Login.aspx")
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
            If IsPostBack = False Then
                SetControls()
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub SetControls()
        Try
            '    Srh_DealNumber.Columns.Add("DealSlipNo")
            '    Srh_DealNumber.Columns.Add("SM.SecurityName")
            '    Srh_DealNumber.Columns.Add("DealDate")
            '    Srh_DealNumber.Columns.Add("SettmentDate")
            '    Srh_DealNumber.Columns.Add("Rate")
            '    Srh_DealNumber.Columns.Add("DealSlipID")
            btn_DeleteDeal.Attributes.Add("onclick", "return  Validation();")
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub Srh_DealNumber_ButtonClick() Handles Srh_DealNumber.ButtonClick
        FillDealSlipFields()
    End Sub
    Protected Sub btn_DeleteDeal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_DeleteDeal.Click
        SetSaveUpdate("ID_INSERT_DeleteDealSlipEntry", True)
        DeleteDeal()
        'Page.ClientScript.RegisterStartupScript(Me.GetType, "open", "SaveRecord()", True)
    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String, Optional ByVal blnRedirect As Boolean = True)
        Try
            Dim sqlTrans As SqlTransaction
            Dim strUrl As String
            sqlTrans = clsCommonFuns.sqlConn.BeginTransaction
            If SaveDeleteDeal(sqlTrans, strProc) = False Then Exit Sub
            sqlTrans.Commit()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function SaveDeleteDeal(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = clsCommonFuns.sqlConn
            objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "I", , , Val(Srh_DealNumber.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@Remark", SqlDbType.VarChar, 500, "I", , , Trim(txt_Remark.Text))
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Sub FillDealSlipFields()
        Try
            Dim dt As DataTable
            dt = objCommon.FillDataTable(clsCommonFuns.sqlConn, "Id_FILL_DealSlipEntry", Val(Srh_DealNumber.SelectedId), "DealSlipID")
            If dt.Rows.Count > 0 Then
                lit_Issuer.Text = Trim(dt.Rows(0).Item("SecurityIssuer") & "")
                lit_SecurityName.Text = Trim(dt.Rows(0).Item("SecurityName") & "")
                lit_CustName.Text = Trim(dt.Rows(0).Item("CustomerName").ToString)
                lit_TransType.Text = Trim(dt.Rows(0).Item("TransType") & "")
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

            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub DeleteDeal()
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_Deal"
            sqlComm.Connection = clsCommonFuns.sqlConn
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "I", , , Val(Srh_DealNumber.SelectedId))
            sqlComm.ExecuteNonQuery()
            lbl_Deleted.Visible = True
            lbl_Deleted.Text = "This Deal is deleted successfully"
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class

    'Private Sub FillFields()
    '    Dim dt As DataTable
    '    dt = objCommon.FillDataTable(clsCommonFuns.sqlConn, "Id_FILL_DealSlipEntry", Val(Srh_DealNumber.SelectedId), "DealSlipID")
    '    If dt.Rows.Count > 0 Then
    '        TransType = Trim(dt.Rows(0).Item("TransType") & "")
    '        DealSlipType = Trim(dt.Rows(0).Item("DealSlipType") & "")
    '        DealTransType = Trim(dt.Rows(0).Item("DealTransType") & "")
    '        DealSlipNo = Trim(dt.Rows(0).Item("DealSlipNo") & "")
    '        SecurityName = Val(dt.Rows(0).Item("SecurityId") & "")
    '        FaceValue = Val(dt.Rows(0).Item("FaceValue") & "")
    '        FaceValueMultiple = Val(dt.Rows(0).Item("FaceValueMultiple") & "")
    '        NoOfBond = Val(dt.Rows(0).Item("NoOfBond") & "")
    '        DealDate = Format(dt.Rows(0).Item("DealDate"), "dd/MM/yyyy")
    '        SettmentDate = Format(dt.Rows(0).Item("SettmentDate"), "dd/MM/yyyy")
    '        Rate = Trim(dt.Rows(0).Item("Rate") & "")
    '        CompId = Val(dt.Rows(0).Item("CompId") & "")
    '        DealerName = Trim(dt.Rows(0).Item("DealerName") & "")
    '        Dealdone = dt.Rows(0).Item("DealDone")
    '        'txt_NoOfBonds.Text = Val(dt.Rows(0).Item("NoOfBond") & "")
    '        'SecurityId = Val(dt.Rows(0).Item("SecurityId") & "")
    '        'Srh_NameOFClient.SelectedId = Val(dt.Rows(0).Item("CustomerId") & "")
    '        'Srh_NameOFClient.SearchTextBox.Text = dt.Rows(0).Item("CustomerName").ToString
    '        'rdo_PhysicalDMAT.SelectedValue = Trim(dt.Rows(0).Item("ModeofDelivery") & "")
    '        'rdo_AccIntDays.SelectedValue = Trim(dt.Rows(0).Item("AccIntDays") & "")
    '        'cbo_ModeOfPayment.SelectedValue = Trim(dt.Rows(0).Item("ModeOFPayment") & "")
    '        'objCommon.FillControl(cbo_SGLWith, clsCommonFuns.sqlConn, "ID_FILL_SGLBankMaster1", "BankName", "SGLId", Val(cbo_Company.SelectedValue), "CompId")
    '        'objCommon.FillControl(cbo_Bank, clsCommonFuns.sqlConn, "ID_FILL_BankMaster1", "BankName", "BankId", Val(cbo_Company.SelectedValue), "CompId")
    '        'cbo_Bank.SelectedValue = Val(dt.Rows(0).Item("bankId") & "")
    '        'cbo_SGLWith.SelectedValue = Val(dt.Rows(0).Item("SGLId") & "")
    '        'objCommon.FillControl(cbo_Exchange, clsCommonFuns.sqlConn, "ID_FILL_ExchangeMaster1", "ExchangeName", "ExchangeId")
    '        'cbo_Exchange.SelectedValue = Val(dt.Rows(0).Item("ExchangeId") & "")
    '        'lbl_Dealar.Text = Trim(Hid_DealerName.Value)
    '        'txt_BrokeragePaidTo.Text = Trim(dt.Rows(0).Item("BrockPaidTo") & "")
    '        'txt_BrokeragePaid.Text = Trim(dt.Rows(0).Item("Brockpaid") & "")
    '        'txt_BrokeragereceivedFrom.Text = Trim(dt.Rows(0).Item("BrockRecvForm") & "")
    '        'txt_Brokeragereceived.Text = Trim(dt.Rows(0).Item("BrockReceived") & "")
    '        'UserId = Val(dt.Rows(0).Item("UserId") & "")
    '        'rbl_DealType.SelectedValue = Trim(dt.Rows(0).Item("SelectMethod") & "")
    '        'srh_BTBDealSlipNo.SelectedId = Val(dt.Rows(0).Item("BTBDealSlipId") & "")
    '        'srh_BTBDealSlipNo.SearchTextBox.Text = dt.Rows(0).Item("BTBdealslipno").ToString
    '        'rbl_RefDealSlip.SelectedValue = Trim(dt.Rows(0).Item("RefDealSlip") & "")
    '        'srh_RefDealSlipNo.SelectedId = Val(dt.Rows(0).Item("RefDealSlipId") & "")
    '        'srh_RefDealSlipNo.SearchTextBox.Text = dt.Rows(0).Item("RefDealSlipNo").ToString
    '        'chk_Brokerage.Checked = dt.Rows(0).Item("BrockEntry")
    '        'txt_CancelRemark.Text = Trim(dt.Rows(0).Item("FORemark") & "")
    '        'objCommon.FillControl(cbo_Demat, clsCommonFuns.sqlConn, "ID_FILL_DMATMaster1", "DPName", "DMatId", Val(cbo_Company.SelectedValue), "CompId")
    '        'cbo_Demat.SelectedValue = Val(dt.Rows(0).Item("DMatId") & "")
    '        'objCommon.FillControl(cbo_CustDemate, clsCommonFuns.sqlConn, "ID_FILL_CustomerdPDetails", "DpName", "CustDPId", Val(Srh_NameOFClient.SelectedId), "CustomerId")
    '        'objCommon.FillControl(cbo_CustSGL, clsCommonFuns.sqlConn, "ID_FILL_CustomerSGLDetails", "SGLTransWith", "CustSGLId", Val(Srh_NameOFClient.SelectedId), "CustomerId")
    '        'cbo_CustDemate.SelectedValue = Val(dt.Rows(0).Item("CustDPId") & "")
    '        'cbo_CustSGL.SelectedValue = Val(dt.Rows(0).Item("CustSGLId") & "")
    '        'objCommon.FillControl(cbo_ContactPerson, clsCommonFuns.sqlConn, "ID_FILL_CustomerContactDetails", "ContactPerson", "ContactId", Val(Srh_NameOFClient.SelectedId), "CustomerId")
    '        'cbo_ContactPerson.SelectedValue = Val(dt.Rows(0).Item("ContactId") & "")
    '        'LastIPDate = IIf(Trim(dt.Rows(0).Item("LastIpDate") & "") = "", Date.MinValue, dt.Rows(0).Item("LastIpDate")) 'dt.Rows(0).Item("LastIpDate")
    '        'txt_FORemark.Text = Trim(dt.Rows(0).Item("FORemark") & "")
    '        'rbl_DealSlipType.Enabled = False
    '        'rbl_DealType.Enabled = False
    '        'cbo_DealTransType.Enabled = False
    '        'srh_BTBDealSlipNo.SearchButton.Visible = False
    '        'rbl_RefDealSlip.Enabled = False
    '        'srh_RefDealSlipNo.SearchButton.Visible = False
    '        'FillSecurityDetails()
    '    End If
    'End Sub
'Dim LastIPDate As Date
'Dim TransType As Char
'Dim DealTransType As Char
'Dim DealSlipType As Char
'Dim DealSlipNo As String
'Dim SecurityName As String
'Dim Remark As String
'Dim FaceValue As Integer
'Dim FaceValueMultiple As Integer
'Dim NoOfBond As Integer
'Dim DealDate As Date
'Dim SettmentDate As Date
'Dim Rate As Decimal
'Dim SecurityId As Integer
'Dim DealslipId As Integer
'Dim CompId As Integer
'Dim DealerName As String
'Dim Dealdone As Char
'Dim Dateofdeletion As Date
