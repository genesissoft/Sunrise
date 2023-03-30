Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports log4net
Partial Class Forms_DMATDelivery
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim arrIssueDetailIds() As String
    Dim sqlComm As New SqlCommand
    Dim sqlConn As SqlConnection
    Dim PgName As String = "$DMATDelivery$"
    Dim objUtil As New Util
    Dim intPrevSlipNo As Int16


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
            Hid_UserId.Value = Val(Session("UserId"))
            Hid_UserTypeId.Value = Val(Session("UsertypeId"))
            If IsPostBack = False Then
                Session("ClientName") = ""
                btn_Update.Visible = False
                btn_Save.Attributes.Add("onclick", "return validation();")
                Session("DematInfoTable") = ""
                Hid_Id.Value = "A"
                SetAttributes()
                FillBlankGrid()
                SetControls()
                Hid_DematAccTo.Value = Trim(Request.QueryString("Hid_DematAccTo.value") & "")
                If Request.QueryString("Id") <> "" Then
                    Dim strId As String = objCommon.DecryptText(HttpUtility.UrlDecode(Request.QueryString("Id")))
                    ViewState("Id") = Val(strId)
                    srh_TransCode.SelectedId = ViewState("Id")
                    FillFields()
                    FillDetailsGrid()
                    btn_Update.Visible = True
                    btn_Save.Visible = False
                    ClearFields()
                End If
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

    Private Sub SetAttributes()
        btn_Save.Attributes.Add("onclick", "return validation();")
        'btn_AddInfo.Attributes.Add("onclick", "return AddDetails();")
        txt_Total.ReadOnly = True
        txt_BalAmt.ReadOnly = True
    End Sub
    Private Sub FillDetailsGrid()
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_DematDelivery", Val(srh_TransCode.SelectedId), "DealSlipID")
            dg_FinancialDeal.DataSource = dt
            dg_FinancialDeal.DataBind()
            Session("DematInfoTable") = dt
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillDetailsGrid", "Error in FillDetailsGrid", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub FillBlankGrid()
        Try
            Dim DtInfoGrid As New DataTable
            DtInfoGrid.Columns.Add("DeliveryDate", GetType(String))
            DtInfoGrid.Columns.Add("DmatSlipNo", GetType(String))
            DtInfoGrid.Columns.Add("DealSlipId", GetType(String))
            DtInfoGrid.Columns.Add("ClientId", GetType(String))
            DtInfoGrid.Columns.Add("CustomerId", GetType(String))
            DtInfoGrid.Columns.Add("CustomerName", GetType(String))
            DtInfoGrid.Columns.Add("NSDLFaceValue", GetType(String))
            DtInfoGrid.Columns.Add("FaceValue", GetType(String))
            DtInfoGrid.Columns.Add("CustDPId", GetType(String))
            DtInfoGrid.Columns.Add("Quantity", GetType(String))
            DtInfoGrid.Columns.Add("CustomerSlipNumber", GetType(String))
            DtInfoGrid.Columns.Add("DpId", GetType(String))
            DtInfoGrid.Columns.Add("DMatId", GetType(String))
            DtInfoGrid.Columns.Add("CustClientId", GetType(String))
            DtInfoGrid.Columns.Add("DpName", GetType(String))
            DtInfoGrid.Columns.Add("FaceMultiple", GetType(String))
            DtInfoGrid.Columns.Add("DematAccTo", GetType(String))
            DtInfoGrid.Columns.Add("SettleNo", GetType(String))
            Session("DematInfoTable") = DtInfoGrid
            dg_FinancialDeal.DataSource = DtInfoGrid
            dg_FinancialDeal.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillBlankGrid", "Error in FillBlankGrid", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub FillFields()
        Try
            Dim dt As DataTable
            Dim dtDmat As DataTable
            Dim i As Integer
            Dim balamt As Integer = 0
            Dim totalamt As Integer
            Dim dtCustName As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_DematDelivery", Val(srh_TransCode.SelectedId), "DealSlipID")
            If dt.Rows.Count > 0 Then
                Hid_TransType.Value = Trim(dt.Rows(0).Item("TransType").ToString)
                txt_ClientName.Text = Trim(dt.Rows(0).Item("CustomerName").ToString)
                Session("ClientName") = Trim(dt.Rows(0).Item("CustomerName").ToString)
                txt_FaceValue.Text = Val(dt.Rows(0).Item("FV") & "")
                'cbo_FaceVal.SelectedValue = Val(dt.Rows(0).Item("FaceValueMultiple") & "")
                txt_IssuerOfSecurity.Text = Trim(dt.Rows(0).Item("SecurityIssuer") & "")
                txt_SecurityName.Text = Trim(dt.Rows(0).Item("SecurityName") & "")
                'txt_SettlmtDate.Text = Trim(dt.Rows(0).Item("SettmentDate") & "")
                txt_SettlmtDate.Text = Format(dt.Rows(0).Item("SettmentDate"), "dd/MM/yyyy")
                txt_SettlmntAmt.Text = Val(dt.Rows(0).Item("SettlementAmt") & "")
                'txt_NSDLFaceValue.Text = Trim(dt.Rows(0).Item("NSDLFaceValue") & "")
                'txt_FaceVal.Text = Val(dt.Rows(0).Item("FaceValue") & "")
                'txt_Qty.Text = Val(dt.Rows(0).Item("NoOfBond") & "")
                Hid_CustomerId.Value = Val(dt.Rows(0).Item("CustomerID") & "")
                Hid_DealSlipId.Value = Val(dt.Rows(0).Item("DealSlipID") & "")
                srh_TransCode.SelectedId = Val(dt.Rows(0).Item("DealSlipID") & "")
                srh_TransCode.SearchTextBox.Text = Trim(dt.Rows(0).Item("DealSlipNo") & "")
                Cbo_FaceValue.SelectedValue = Val(dt.Rows(0).Item("FaceValueMultiple") & "")
                txt_TransDate.Text = Format(dt.Rows(0).Item("DealDate"), "dd/MM/yyyy")
                Hid_DematInfoId.Value = Val(dt.Rows(0).Item("Dmatinfoid") & "")
                txt_BalAmt.Text = Val(dt.Rows(0).Item("BalanceAmount") & "")
                Hid_PayMode.Value = Trim(dt.Rows(0).Item("ModeOFPayment") & "")
                Hid_Remark.Value = Trim(dt.Rows(0).Item("Remark") & "")
                If Val(txt_BalAmt.Text) = 0 Then
                    txt_Total.Text = Val(dt.Rows(0).Item("FV") & "") * Val(dt.Rows(0).Item("FaceValueMultiple") & "")
                Else
                    For i = 0 To dt.Rows.Count - 1
                        totalamt = totalamt + Val(dt.Rows(i).Item("FaceValue") & "")
                    Next
                    'txt_Total.Text = Format(totalamt, 
                End If

            End If



            If Val(txt_BalAmt.Text) = 0 Then
                'btn_AddInfo.Visible = False
                btn_AddInfo.Attributes.Add("style", "display:none")

            Else
                ' btn_AddInfo.Visible = True
                btn_AddInfo.Attributes.Add("style", "display:")
            End If

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillFields", "Error in FillFields", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetControls", "Error in SetControls", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Protected Sub srh_TransCode_ButtonClick() Handles srh_TransCode.ButtonClick
        Try
            Dim dg As DataGridItem
            Dim i As Integer
            Dim intDealslipId As Integer
            Dim strHtml As String


            FillFields()
            For i = 0 To dg_FinancialDeal.Items.Count - 1
                dg = dg_FinancialDeal.Items(i)
                intDealslipId = Val(CType(dg.FindControl("lbl_DealSlipId"), Label).Text)
                If intDealslipId = srh_TransCode.SelectedId And txt_BalAmt.Text = 0 Then
                    Dim msg As String = "This Deal Slip No already added."
                    strHtml = "alert('" + msg + "');"
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
                    Exit For
                Else
                    FillBlankGrid()
                End If
            Next
            CalCulateTotalAmt()
            'btn_AddInfo.Visible = True
            btn_AddInfo.Attributes.Add("style", "display:")
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "srh_TransCode_ButtonClick", "Error in srh_TransCode_ButtonClick", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            OpenConn()
            Dim sqlTrans As SqlTransaction
            sqlTrans = sqlConn.BeginTransaction
            If DeleteDematDetails(sqlTrans) = False Then Exit Sub
            'If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            If SaveDetailGrd(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()
            Response.Redirect("DematDeliveryDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetSaveUpdate", "Error in SetSaveUpdate", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function SaveDetailGrd(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            Dim SlipNo As Int16
            dt = Session("DematInfoTable")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_DMATInformation"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("Deliverydate").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    'If GetDMATSlipNo(sqlTrans) = False Then
                    '    Exit Function
                    'End If
                    'intPrevSlipNo = intPrevSlipNo + 1
                    objCommon.SetCommandParameters(sqlComm, "@CustomerSlipNumber", SqlDbType.VarChar, 10, "I", , , dt.Rows(I).Item("CustomerSlipNumber"))
                    objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("DealSlipId"))
                    objCommon.SetCommandParameters(sqlComm, "@CustDPId", SqlDbType.VarChar, 4, "I", , , dt.Rows(I).Item("CustDPId"))
                    objCommon.SetCommandParameters(sqlComm, "@FaceValue", SqlDbType.Decimal, 9, "I", , , dt.Rows(I).Item("FaceValue"))
                    objCommon.SetCommandParameters(sqlComm, "@FaceMultiple", SqlDbType.BigInt, 8, "I", , , dt.Rows(I).Item("FaceMultiple"))
                    objCommon.SetCommandParameters(sqlComm, "@Quantity", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("Quantity"))
                    objCommon.SetCommandParameters(sqlComm, "@DematAccTo", SqlDbType.Char, 1, "I", , , dt.Rows(I).Item("DematAccTo"))
                    objCommon.SetCommandParameters(sqlComm, "@Deliverydate", SqlDbType.SmallDateTime, 9, "I", , , objCommon.DateFormat(dt.Rows(I).Item("Deliverydate")))
                    objCommon.SetCommandParameters(sqlComm, "@DMATslipNo", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("DMATslipNo"))
                    objCommon.SetCommandParameters(sqlComm, "@DMatId", SqlDbType.Int, 4, "I", , , dt.Rows(I).Item("DMatId"))
                    objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 4, "I", , , Val(Session("UserId")))
                    objCommon.SetCommandParameters(sqlComm, "@BalanceAmount", SqlDbType.BigInt, 4, "I", , , Val(txt_BalAmt.Text))
                    objCommon.SetCommandParameters(sqlComm, "@SettleNo", SqlDbType.BigInt, 4, "I", , , dt.Rows(I).Item("SettleNo"))
                    objCommon.SetCommandParameters(sqlComm, "@Remark", SqlDbType.VarChar, 500, "I", , , Trim(Hid_Remark.Value))
                    objCommon.SetCommandParameters(sqlComm, "@Dealslipno", SqlDbType.VarChar, 500, "I", , , Trim(srh_TransCode.SearchTextBox.Text))
                    objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                    objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
                    objCommon.SetCommandParameters(sqlComm, "@Dmatinfoid", SqlDbType.BigInt, 8, "O")
                    objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 50, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            ViewState("Id") = sqlComm.Parameters("@DealSlipId").Value
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "SaveDetailGrd", "Error in SaveDetailGrd", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub dg_FinancialDeal_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_FinancialDeal.ItemCommand
        Try
            Dim dt As DataTable
            Dim strRetValues() As String
            dt = CType(Session("DematInfoTable"), DataTable)
            If e.CommandName = "Edit" Then
                Hid_Id.Value = "E"
                strRetValues = Split(Hid_RetValues.Value, "!")
                dt.Rows(e.Item.ItemIndex).Item("FaceValue") = Val(strRetValues(0) & "")
                dt.Rows(e.Item.ItemIndex).Item("NSDLFaceValue") = Val(strRetValues(1) & "")
                dt.Rows(e.Item.ItemIndex).Item("DMATslipNo") = Val(strRetValues(2) & "")
                dt.Rows(e.Item.ItemIndex).Item("CustomerName") = Session("ClientName")
                If Hid_TransType.Value = "S" Then
                    dt.Rows(e.Item.ItemIndex).Item("DMatId") = Trim(strRetValues(4) & "")
                    dt.Rows(e.Item.ItemIndex).Item("DPId") = Trim(strRetValues(12) & "")
                    dt.Rows(e.Item.ItemIndex).Item("ClientId") = Trim(strRetValues(13) & "")
                    dt.Rows(e.Item.ItemIndex).Item("DpName") = Trim(strRetValues(16) & "")
                Else
                    dt.Rows(e.Item.ItemIndex).Item("DMatId") = Trim(strRetValues(4) & "")
                    dt.Rows(e.Item.ItemIndex).Item("DPId") = Trim(strRetValues(5) & "")
                    dt.Rows(e.Item.ItemIndex).Item("ClientId") = Trim(strRetValues(6) & "")
                    dt.Rows(e.Item.ItemIndex).Item("DpName") = Trim(strRetValues(15) & "")
                End If


                dt.Rows(e.Item.ItemIndex).Item("Quantity") = Trim(strRetValues(7) & "")
                dt.Rows(e.Item.ItemIndex).Item("DeliveryDate") = Trim(strRetValues(8) & "")
                dt.Rows(e.Item.ItemIndex).Item("DematAccTo") = Trim(strRetValues(9) & "")
                dt.Rows(e.Item.ItemIndex).Item("CustomerSlipNumber") = Trim(strRetValues(10) & "")
                dt.Rows(e.Item.ItemIndex).Item("CustDPId") = Val(strRetValues(11) & "")

                ' dt.Rows(e.Item.ItemIndex).Item("CustClientId") = Val(strRetValues(13) & "")
                'dt.Rows(e.Item.ItemIndex).Item("DPId") = Trim(strRetValues(12) & "")
                dt.Rows(e.Item.ItemIndex).Item("FaceMultiple") = Trim(strRetValues(17) & "")

                Hid_DematAccTo.Value = Trim(strRetValues(9) & "")
                ' dt.Rows(e.Item.ItemIndex).Item("ClientId") = Trim(strRetValues(11) & "")
                dt.Rows(e.Item.ItemIndex).Item("DealSlipId") = Trim(strRetValues(14) & "")
                dt.Rows(e.Item.ItemIndex).Item("SettleNo") = Val(strRetValues(18))
                Hid_Remark.Value = Trim(strRetValues(19) & "")
            Else
                dt.Rows.RemoveAt(e.Item.ItemIndex)
            End If
            Session("DematInfoTable") = dt
            dg_FinancialDeal.DataSource = dt
            dg_FinancialDeal.DataBind()
            CalCulateTotalAmt()

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_FinancialDeal_ItemCommand", "Error in dg_FinancialDeal_ItemCommand", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub



    Protected Sub dg_FinancialDeal_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_FinancialDeal.ItemDataBound
        Try
            Dim intDealslipId As Integer
            Dim lnkBtnDelete As ImageButton
            Dim lnkBtnEdit As ImageButton
            Dim dtGrid As DataTable
            Dim strFaceVal As Decimal
            Dim strNSDLFaceVal As Decimal
            Dim strDmatSlipNo As String
            Dim strChqNo As String
            Dim strDPName As String
            Dim strDPId As String
            Dim strClientId As String
            Dim strQty As String
            Dim strDelDate As String
            Dim strDematAccTo As String
            Dim strCustSlipNo As String
            Dim strCustDPName As String
            Dim strCustDPId As String
            Dim strCustClientId As String
            Dim strFaceMultiple As String
            Dim intSettleNo As Integer
            Dim lnkhtmlbtn As HtmlInputButton

            If e.Item.ItemType = ListItemType.Header Then
                If Hid_TransType.Value = "S" Then
                    e.Item.Cells(5).Text = "Cust.Name"
                    e.Item.Cells(6).Text = "Cust.Id"
                    e.Item.Cells(7).Text = "Cust.ClientId"
                Else
                    e.Item.Cells(5).Text = "DPName"
                    e.Item.Cells(6).Text = "DPId"
                    e.Item.Cells(7).Text = "ClientId"
                End If
            End If

            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                'e.Item.ID = "itm" & e.Item.ItemIndex

                strFaceVal = Val(CType(e.Item.FindControl("lbl_FaceValue"), Label).Text) '* Val(CType(e.Item.FindControl("lbl_FaceMultiple"), Label).Text)

                strNSDLFaceVal = Val(CType(e.Item.FindControl("lbl_NSDLFaceValue"), Label).Text)
                strDmatSlipNo = Trim(CType(e.Item.FindControl("lbl_ChequeNumber"), Label).Text)
                strChqNo = Trim(CType(e.Item.FindControl("lbl_CustName"), Label).Text)

                strDPName = Val(CType(e.Item.FindControl("lbl_DpName"), Label).Text)
                strDPId = Trim(CType(e.Item.FindControl("lbl_DpId"), Label).Text)
                strClientId = Trim(CType(e.Item.FindControl("lbl_BankName"), Label).Text)
                strQty = Trim(CType(e.Item.FindControl("lbl_Qty"), Label).Text)
                strDelDate = CType(e.Item.FindControl("lbl_DelDate"), Label).Text
                strCustSlipNo = CType(e.Item.FindControl("lbl_CustSlipNo"), Label).Text

                strCustDPId = CType(e.Item.FindControl("lbl_CustDPId"), Label).Text
                strCustClientId = CType(e.Item.FindControl("lbl_DMatId"), Label).Text

                strCustDPName = CType(e.Item.FindControl("lbl_CustSlipNo"), Label).Text
                strDematAccTo = Trim(CType(e.Item.FindControl("lbl_DematAccTo"), Label).Text)

                strFaceMultiple = Val(CType(e.Item.FindControl("lbl_FaceMultiple"), Label).Text)
                intSettleNo = Val(CType(e.Item.FindControl("lbl_SettleNo"), Label).Text)
                lnkBtnDelete = CType(e.Item.FindControl("imgBtn_Delete"), ImageButton)
                lnkBtnDelete.Attributes.Add("onclick", "return Delete_entry()")

                lnkBtnEdit = CType(e.Item.FindControl("imgBtn_Edit"), ImageButton)
                lnkBtnEdit.Attributes.Add("onclick", "return UpdateDetails('" & e.Item.ItemIndex & "','" & strFaceVal & "','" & strNSDLFaceVal & "','" & strDmatSlipNo & "','" & strChqNo & "','" & strDPName & "','" & strDPId & "','" & strClientId & "','" & strQty & "','" & strDelDate & "','" & strDematAccTo & "','" & strDematAccTo & "','" & strCustDPId & "','" & strCustDPName & "','" & strCustClientId & "','" & intSettleNo & "')")

                lnkhtmlbtn = CType(e.Item.FindControl("imgBtn_Edit1"), HtmlInputButton)
                lnkhtmlbtn.Attributes.Add("onclick", "return UpdateDetails('" & e.Item.ItemIndex & "','" & strFaceVal & "','" & strNSDLFaceVal & "','" & strDmatSlipNo & "','" & strChqNo & "','" & strDPName & "','" & strDPId & "','" & strClientId & "','" & strQty & "','" & strDelDate & "','" & strDematAccTo & "','" & strDematAccTo & "','" & strCustDPId & "','" & strCustDPName & "','" & strCustClientId & "','" & intSettleNo & "')")

                dtGrid = CType(Session("DematInfoTable"), DataTable)
                Hid_DealSlipId.Value = Val(CType(e.Item.FindControl("lbl_DealSlipId"), Label).Text)
                Hid_facevalue.Value = Val(CType(e.Item.FindControl("lbl_FaceValue"), Label).Text)
                Hid_FaceMultiple.Value = Val(CType(e.Item.FindControl("lbl_FaceMultiple"), Label).Text)
                Hid_DematAccTo.Value = strDematAccTo

                'Dim FVS As Integer
                'FVS = FVS + Val(CType(e.Item.FindControl("lbl_FaceValue"), Label).Text)
                'Hid_FVS.value = Val(FVS)


            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_FinancialDeal_ItemDataBound", "Error in dg_FinancialDeal_ItemDataBound", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_AddInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddInfo.Click
        AddDetails()
        CalCulateTotalAmt()
    End Sub
    Private Sub AddDetails()
        Try
            Dim strRetValues() As String
            Dim dt As DataTable
            Dim dr As DataRow
            Dim strRowIndex As String = Hid_RowIndex.Value
            If strRowIndex <> "" Then
                Dim intRowIndex As Integer = Convert.ToInt32(strRowIndex)
                dt = Session("DematInfoTable")
                Hid_Id.Value = "E"
                strRetValues = Split(Hid_RetValues.Value, "!")
                dt.Rows(intRowIndex).Item("FaceValue") = Val(strRetValues(0) & "")
                dt.Rows(intRowIndex).Item("NSDLFaceValue") = Val(strRetValues(1) & "")
                dt.Rows(intRowIndex).Item("DMATslipNo") = Val(strRetValues(2) & "")
                dt.Rows(intRowIndex).Item("CustomerName") = Session("ClientName")
                If Hid_TransType.Value = "S" Then
                    dt.Rows(intRowIndex).Item("DMatId") = Trim(strRetValues(4) & "")
                    dt.Rows(intRowIndex).Item("DPId") = Trim(strRetValues(12) & "")
                    dt.Rows(intRowIndex).Item("ClientId") = Trim(strRetValues(13) & "")
                    dt.Rows(intRowIndex).Item("DpName") = Trim(strRetValues(16) & "")
                Else
                    dt.Rows(intRowIndex).Item("DMatId") = Trim(strRetValues(4) & "")
                    dt.Rows(intRowIndex).Item("DPId") = Trim(strRetValues(5) & "")
                    dt.Rows(intRowIndex).Item("ClientId") = Trim(strRetValues(6) & "")
                    dt.Rows(intRowIndex).Item("DpName") = Trim(strRetValues(15) & "")
                End If


                dt.Rows(intRowIndex).Item("Quantity") = Trim(strRetValues(7) & "")
                dt.Rows(intRowIndex).Item("DeliveryDate") = Trim(strRetValues(8) & "")
                dt.Rows(intRowIndex).Item("DematAccTo") = Trim(strRetValues(9) & "")
                dt.Rows(intRowIndex).Item("CustomerSlipNumber") = Trim(strRetValues(10) & "")
                dt.Rows(intRowIndex).Item("CustDPId") = Val(strRetValues(11) & "")

                ' dt.Rows(e.Item.ItemIndex).Item("CustClientId") = Val(strRetValues(13) & "")
                'dt.Rows(e.Item.ItemIndex).Item("DPId") = Trim(strRetValues(12) & "")
                dt.Rows(intRowIndex).Item("FaceMultiple") = Trim(strRetValues(17) & "")

                Hid_DematAccTo.Value = Trim(strRetValues(9) & "")
                ' dt.Rows(e.Item.ItemIndex).Item("ClientId") = Trim(strRetValues(11) & "")
                dt.Rows(intRowIndex).Item("DealSlipId") = Trim(strRetValues(14) & "")
                dt.Rows(intRowIndex).Item("SettleNo") = Val(strRetValues(18))
                Hid_Remark.Value = Trim(strRetValues(19) & "")
            Else

                strRetValues = Split(Hid_RetValues.Value, "!")
                dt = Session("DematInfoTable")
                dr = dt.NewRow
                dr.Item("FaceValue") = Val(strRetValues(0))
                dr.Item("NSDLFaceValue") = Val(strRetValues(1))
                dr.Item("DMATslipNo") = Val(strRetValues(2))
                dr.Item("CustomerName") = Trim(strRetValues(3))
                dr.Item("CustomerId") = Val(Hid_CustomerId.Value)

                dr.Item("DMatId") = Trim(strRetValues(4))
                dr.Item("Quantity") = Trim(strRetValues(7))
                dr.Item("DeliveryDate") = Trim(strRetValues(8))
                dr.Item("DematAccTo") = Trim(strRetValues(9))


                dr.Item("CustomerSlipNumber") = Trim(strRetValues(10))
                If Trim(strRetValues(11)) <> "" Then
                    dr.Item("CustDPId") = Trim(strRetValues(11))
                End If

                If Hid_TransType.Value = "P" Then
                    dr.Item("DpName") = Trim(strRetValues(15))
                    dr.Item("DPId") = Trim(strRetValues(5))
                    dr.Item("ClientId") = Trim(strRetValues(6))
                ElseIf Hid_TransType.Value = "S" Then
                    dr.Item("DpName") = Trim(strRetValues(16))
                    dr.Item("DPId") = Trim(strRetValues(12))
                    dr.Item("ClientId") = Trim(strRetValues(13))
                End If
                If Hid_TransType.Value = "P" Then
                    If Hid_PayMode.Value = "B" Or Hid_PayMode.Value = "N" Then
                        dr.Item("DpName") = Trim(strRetValues(15))
                        dr.Item("DMatId") = Trim(strRetValues(4))
                        dr.Item("DPId") = Trim(strRetValues(5))
                        dr.Item("ClientId") = Trim(strRetValues(6))
                    End If
                End If
                dr.Item("FaceMultiple") = Val(strRetValues(17))
                dr.Item("SettleNo") = Val(strRetValues(18))
                Hid_BalanceFV.Value = (txt_FaceValue.Text * Cbo_FaceValue.SelectedValue) - Val(strRetValues(0))
                dr.Item("DealSlipId") = Val(Hid_DealSlipId.Value)
                Hid_Remark.Value = Trim(strRetValues(19))
                dt.Rows.Add(dr)
                'Hid_facevalue.Value = ""
            End If
            Session("DematInfoTable") = dt
            dg_FinancialDeal.DataSource = dt
            dg_FinancialDeal.DataBind()
            Hid_RowIndex.Value = ""
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "AddDetails", "Error in AddDetails", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        SetSaveUpdate("ID_INSERT_DMATInformation")
        btn_Save.Visible = True
    End Sub
    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click

        If Val(ViewState("Id")) <> 0 Then
            Response.Redirect("DematDeliveryDetail.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Else
            Response.Redirect("DematDeliveryDetail.aspx")
        End If

    End Sub

    Private Sub ClearFields()
        srh_TransCode.SearchButton.Attributes.Add("style", "display:none")
        'srh_TransCode.SearchButton.Visible = False

    End Sub
    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        btn_Save_Click(sender, e)
        Response.Redirect("DematDeliveryDetail.aspx", False)
    End Sub

    Private Function DeleteDematDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_DMATinformation"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(Hid_DealSlipId.Value))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "DeleteDematDetails", "Error in DeleteDematDetails", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Sub CalCulateTotalAmt()
        Try
            Dim dblTotAmt As Double
            Dim dblBalanceAmt As Double
            Dim aa As Integer
            Dim dbltot As Double
            For I As Integer = 0 To dg_FinancialDeal.Items.Count - 1
                aa = (Val(CType(dg_FinancialDeal.Items(I).FindControl("lbl_FaceValue"), Label).Text))
                dblTotAmt = dblTotAmt + (Val(CType(dg_FinancialDeal.Items(I).FindControl("lbl_FaceValue"), Label).Text)) '* Val(CType(dg_FinancialDeal.Items(I).FindControl("lbl_FaceMultiple"), Label).Text))
                dbltot = dblTotAmt
            Next

            dblBalanceAmt = (Val(txt_FaceValue.Text) * Val(Cbo_FaceValue.SelectedValue)) - dbltot
            txt_BalAmt.Text = dblBalanceAmt
            txt_Total.Text = dbltot

            If Val(txt_BalAmt.Text) = 0 Then
                'btn_AddInfo.Visible = False
                btn_AddInfo.Attributes.Add("style", "display:none")
            Else
                '  btn_AddInfo.Visible = True
                btn_AddInfo.Attributes.Add("style", "display:")
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "CalCulateTotalAmt", "Error in CalCulateTotalAmt", "", ex)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function GetDMATSlipNo(ByVal sqlTrans As SqlTransaction)
        Try
            'OpenConn()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_Get_DMATSlipNo"
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
            objCommon.SetCommandParameters(sqlComm, "@MaxNo", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            intPrevSlipNo = Val(sqlComm.Parameters("@MaxNo").Value & "")
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
            Return False
        Finally
            'CloseConn()
        End Try
    End Function
    Private Function GetRefNo() As String
        Try
            Dim SB As New StringBuilder
            For I As Int16 = 1 To 4 - intPrevSlipNo.ToString.Length
                SB.Append("0")
            Next
            SB.Append(intPrevSlipNo.ToString)
            Return SB.ToString
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If

        Catch ex As Exception

        End Try

    End Sub
End Class
