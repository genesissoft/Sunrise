Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_FinancialDelivery
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
            Hid_UserId.Value = Val(Session("UserId"))
            Hid_UserTypeId.Value = Val(Session("UsertypeId"))
            If IsPostBack = False Then
                FillBlankGrid()
                SetControls()
                SetAttributes()
                If Request.QueryString("Id") <> "" Then
                    Dim strId As String = objCommon.DecryptText(HttpUtility.UrlDecode(Request.QueryString("Id")))
                    ViewState("Id") = Val(strId)
                    Session("FinalcialTable") = ""
                    srh_TransCode.SearchButton.Attributes.Add("style", "display:none")
                    'ViewState("Id") = Val(Request.QueryString("Id") & "")
                    Hid_DealSlipId.Value = ViewState("Id")
                    srh_TransCode.SelectedId = ViewState("Id")
                    FillDealSlipFields()
                    FillFinancialGrid()
                    CalcTotal()

                    btn_Save.Visible = False
                    btn_Update.Visible = True
                Else
                    btn_Save.Visible = True
                    btn_Update.Visible = False
                End If
            End If
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
    Private Sub FillFinancialGrid()
        Try
            Dim sqlda As New SqlDataAdapter
            Dim sqldt As New DataTable
            Dim sqldv As New DataView
            Dim dt As DataTable
            OpenConn()
            If TypeOf Session("FinalcialTable") Is DataTable Then
                sqldt = Session("FinalcialTable")
            Else
                sqlComm.Connection = sqlConn
                sqlComm.CommandType = CommandType.StoredProcedure
                sqlComm.CommandText = "Id_FILL_FinancialInfo"
                sqlComm.Parameters.Clear()
                objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , ViewState("Id"))
                objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
                sqlComm.ExecuteNonQuery()
                sqlda.SelectCommand = sqlComm
                sqlda.Fill(sqldt)
            End If
            Session("FinalcialTable") = sqldt
            dg_FinancialDeal.DataSource = Session("FinalcialTable")
            dg_FinancialDeal.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub SetAttributes()
        btn_Update.Attributes.Add("onclick", "return  ValidateGrid();")
        btn_Save.Attributes.Add("onclick", "return  ValidateGrid();")
        'btn_AddInfo.Attributes.Add("onclick", "return  AddDetails();")
    End Sub
    Private Sub FillBlankGrid()
        Try

            Dim DtInfoGrid As New DataTable
            DtInfoGrid.Columns.Add("PaymentDate", GetType(String))
            DtInfoGrid.Columns.Add("ChequeNumber", GetType(String))
            DtInfoGrid.Columns.Add("Amount", GetType(String))
            DtInfoGrid.Columns.Add("BankName", GetType(String))
            DtInfoGrid.Columns.Add("FDType", GetType(String))
            DtInfoGrid.Columns.Add("ChequeDate", GetType(String))
            DtInfoGrid.Columns.Add("Remark", GetType(String))
            DtInfoGrid.Columns.Add("FDId", GetType(String))
            DtInfoGrid.Columns.Add("DealSlipId", GetType(String))
            DtInfoGrid.Columns.Add("TransType", GetType(String))
            DtInfoGrid.Columns.Add("BankId", GetType(String))
            Session("FinalcialTable") = DtInfoGrid
            dg_FinancialDeal.DataSource = DtInfoGrid
            dg_FinancialDeal.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub SetControls()
        Try
            'srh_TransCode.Columns.Add("DealSlipNo")
            'srh_TransCode.Columns.Add("CustomerName")
            'srh_TransCode.Columns.Add("DealSlipId")
            txt_IssuerOfSecurity.ReadOnly = True
            txt_SecurityName.ReadOnly = True
            txt_ClientName.ReadOnly = True
            txt_FaceValue.ReadOnly = True
            txt_SettlemntAmt.ReadOnly = True
            txt_DealDate.ReadOnly = True
            txt_SettlemntDate.ReadOnly = True
            txt_Total.ReadOnly = True
            txt_BalAmt.ReadOnly = True
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Protected Sub srh_TransCode_ButtonClick() Handles srh_TransCode.ButtonClick
        Try
            FillDealSlipFields()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillDealSlipFields()
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_FinancialDelivery", srh_TransCode.SelectedId, "DealSlipID")
            If dt.Rows.Count > 0 Then
                txt_IssuerOfSecurity.Text = Trim(dt.Rows(0).Item("SecurityIssuer") & "")
                txt_SecurityName.Text = Trim(dt.Rows(0).Item("SecurityName") & "")
                txt_ClientName.Text = Trim(dt.Rows(0).Item("CustomerName").ToString)
                txt_FaceValue.Text = Val(dt.Rows(0).Item("FaceValue") & "") * Val(dt.Rows(0).Item("FaceValueMultiple") & "")
                txt_SettlemntAmt.Text = Val(dt.Rows(0).Item("SettlementAmt") & "")
                If Trim((dt.Rows(0).Item("DealDate")) & "") <> "" Then
                    txt_DealDate.Text = Format(dt.Rows(0).Item("DealDate"), "dd/MM/yyyy")
                End If
                If Trim((dt.Rows(0).Item("SettmentDate")) & "") <> "" Then
                    txt_SettlemntDate.Text = Format(dt.Rows(0).Item("SettmentDate"), "dd/MM/yyyy")
                End If
                srh_TransCode.SearchTextBox.Text = Trim(dt.Rows(0).Item("DealSlipNo") & "")
                Hid_CustomerId.Value = Val(dt.Rows(0).Item("CustomerID") & "")
                Hid_DealSlipId.Value = Val(dt.Rows(0).Item("DealSlipId") & "")
                Hid_TransType.Value = Trim(dt.Rows(0).Item("TransType") & "")

            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub CalcTotal()
        Try
            Dim dblTotAmt As Double
            Dim dblCurrAmt As Double
            dblTotAmt = 0
            For I As Int16 = 0 To dg_FinancialDeal.Items.Count - 1
                dblCurrAmt = Val(CType(dg_FinancialDeal.Items(I).FindControl("lbl_Amount"), Label).Text)
                dblTotAmt = dblTotAmt + dblCurrAmt
            Next
            dblTotAmt = dblTotAmt
            txt_BalAmt.Text = Val(txt_SettlemntAmt.Text) - dblTotAmt
            txt_Total.Text = Format(dblTotAmt, "############0.00")
            txt_BalAmt.Text = Format(Val(txt_BalAmt.Text), "############0.00")
            If Val(txt_BalAmt.Text) <= 0 Then
                txt_BalAmt.Text = "0.00"
                tr_AddInfo.Attributes.Add("style", "display:none")
            Else
                tr_AddInfo.Attributes.Add("style", "display:")
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        SetSaveUpdate("ID_INSERT_FinancialInformation")
    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            OpenConn()
            Dim sqlTrans As SqlTransaction
            sqlTrans = sqlConn.BeginTransaction
            If DeleteFinancialDetails(sqlTrans) = False Then Exit Sub
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub

            sqlTrans.Commit()
            Response.Redirect("FinancialDeliveryInformation.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function DeleteFinancialDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_DELETE_FinancialDel"
            sqlComm.Connection = sqlConn
            objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(Hid_DealSlipId.Value))
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            Dim dgItem As DataGridItem
            Dim I As Int16
            Dim strPaymentDate As String
            Dim datPaymentDate As DateTime
            Dim strChqNo As String
            Dim decAmount As Decimal
            Dim strBankName As String
            Dim strBank As String
            Dim strFDType As String
            Dim strChqDate As String
            Dim strRemark As String
            Dim strDealSlipIds As String
            Dim strFDId As Integer
            Dim datChqDate As DateTime
            Dim strTransType As String
            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            For I = 0 To dg_FinancialDeal.Items.Count - 1
                dgItem = dg_FinancialDeal.Items(I)
                strPaymentDate = Trim(CType(dgItem.FindControl("lbl_PaymentDate"), Label).Text)
                datPaymentDate = objCommon.DateFormat(CType(dgItem.FindControl("lbl_PaymentDate"), Label).Text)
                strChqNo = Trim(CType(dgItem.FindControl("lbl_ChequeNumber"), Label).Text)
                decAmount = objCommon.DecimalFormat(CType(dgItem.FindControl("lbl_Amount"), Label).Text)
                strBankName = Trim(CType(dgItem.FindControl("txt_BankName"), TextBox).Text)
                ' strBank = Trim(CType(dgItem.FindControl("lbl_BankId"), Label).Text)
                strFDType = Trim(CType(dgItem.FindControl("lbl_FDType"), Label).Text)
                strChqDate = Trim(CType(dgItem.FindControl("lbl_ChequeDate"), Label).Text)
                datChqDate = objCommon.DateFormat(CType(dgItem.FindControl("lbl_ChequeDate"), Label).Text)
                strRemark = Trim(CType(dgItem.FindControl("txt_Remark"), TextBox).Text)
                strDealSlipIds = Trim(CType(dgItem.FindControl("lbl_DealSlipIds"), Label).Text)
                'strTransType = Trim(CType(dgItem.FindControl("lbl_TransType"), Label).Text)

                sqlComm.Parameters.Clear()
                objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(Hid_DealSlipId.Value))
                If strPaymentDate <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@PaymentDate", SqlDbType.SmallDateTime, 4, "I", , , datPaymentDate)
                End If
                If strChqDate <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@ChequeDate", SqlDbType.SmallDateTime, 4, "I", , , datChqDate)
                End If
                objCommon.SetCommandParameters(sqlComm, "@Amount", SqlDbType.Decimal, 9, "I", , , decAmount)
                If strChqNo <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@ChequeNumber", SqlDbType.VarChar, 50, "I", , , strChqNo)
                Else
                    objCommon.SetCommandParameters(sqlComm, "@ChequeNumber", SqlDbType.VarChar, 50, "I", , , DBNull.Value)
                End If
                If strBankName <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@BankName", SqlDbType.VarChar, 100, "I", , , strBankName)
                Else
                    objCommon.SetCommandParameters(sqlComm, "@BankName", SqlDbType.VarChar, 100, "I", , , DBNull.Value)
                End If

                objCommon.SetCommandParameters(sqlComm, "@FdType", SqlDbType.VarChar, 100, "I", , , strFDType)
                If strRemark <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@Remark", SqlDbType.VarChar, 100, "I", , , strRemark)
                Else
                    objCommon.SetCommandParameters(sqlComm, "@Remark", SqlDbType.VarChar, 100, "I", , , DBNull.Value)
                End If
                objCommon.SetCommandParameters(sqlComm, "@FDId", SqlDbType.BigInt, 8, "O")
                objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                sqlComm.ExecuteNonQuery()
                ViewState("Id") = sqlComm.Parameters("@FDId").Value
                strFDId = Val(ViewState("Id"))
                If strFDType = "A" Then
                    If SaveTransDetails(sqlTrans, strDealSlipIds, strFDId) = False Then Exit Function
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function SaveTransDetails(ByVal sqlTrans As SqlTransaction, ByVal strDealSlipIds As String,
                                      ByVal intFDId As Integer) As Boolean
        Try
            Dim strIds() As String
            Dim sqlComm As New SqlCommand
            strIds = Split(strDealSlipIds, ",")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_INSERT_FinancialTransAdjst"
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To strIds.Length - 1
                If strIds(I) <> "" Then
                    sqlComm.Parameters.Clear()
                    objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , strIds(I))
                    objCommon.SetCommandParameters(sqlComm, "@FDId", SqlDbType.BigInt, 8, "I", , , intFDId)
                    objCommon.SetCommandParameters(sqlComm, "@TrandAdjstmentId", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        btn_Save_Click(sender, e)
    End Sub

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("FinancialDeliveryInformation.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("FinancialDeliveryInformation.aspx")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Protected Sub dg_FinancialDeal_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg_FinancialDeal.ItemCommand
        Try
            Dim dt As DataTable
            Dim strRetValues() As String
            dt = CType(Session("FinalcialTable"), DataTable)
            If e.CommandName = "Edit" Then
            Else
                dt.Rows.RemoveAt(e.Item.ItemIndex)
                Session("FinalcialTable") = dt
            End If
            FillFinancialGrid()
            CalcTotal()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Protected Sub dg_FinancialDeal_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_FinancialDeal.ItemDataBound
        Try
            Dim imgEdit As ImageButton
            Dim imgDelete As ImageButton
            Dim strFDType As String
            Dim decAmount As Decimal
            Dim strRemark As String
            Dim strPaymentDate As String
            Dim strBankName As String
            Dim strChequeNo As String
            Dim strChequeDate As String
            Dim strDealSlipIds As String
            Dim strTransType As String
            Dim strBank As Integer
            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                strFDType = Trim(CType(e.Item.FindControl("lbl_FDType"), Label).Text)
                decAmount = Val(CType(e.Item.FindControl("lbl_Amount"), Label).Text)
                strRemark = Trim(CType(e.Item.FindControl("txt_Remark"), TextBox).Text)
                strPaymentDate = Trim(CType(e.Item.FindControl("lbl_PaymentDate"), Label).Text)
                strBankName = Trim(CType(e.Item.FindControl("txt_BankName"), TextBox).Text)
                ' strBank = Val(CType(e.Item.FindControl("lbl_BankId"), Label).Text)


                strChequeNo = Trim(CType(e.Item.FindControl("lbl_ChequeNumber"), Label).Text)
                strChequeDate = Trim(CType(e.Item.FindControl("lbl_ChequeDate"), Label).Text)
                strDealSlipIds = CType(e.Item.FindControl("lbl_DealSlipIds"), Label).Text
                strTransType = CType(e.Item.FindControl("lbl_TransType"), Label).Text
                Hid_TransType.Value = strTransType
                imgEdit = CType(e.Item.FindControl("imgBtn_Edit"), ImageButton)
                imgEdit.Attributes.Add("onclick", "return UpdateDetails('" & e.Item.ItemIndex & "','" & strFDType & "','" & strTransType & "','" & decAmount & "','" & strRemark & "','" & strPaymentDate & "','" & strBankName & "','" & strBank & "','" & strChequeNo & "','" & strChequeDate & "','" & Val(Hid_CustomerId.Value) & "','" & Val(Hid_DealSlipId.Value) & "','" & strDealSlipIds & "')")
                imgDelete = CType(e.Item.FindControl("imgBtn_Delete"), ImageButton)
                imgDelete.Attributes.Add("onclick", "return Delete_entry()")
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_AddInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddInfo.Click
        AddDetails()
    End Sub
    Protected Sub AddDetails()
        Try
            Dim strRetValues() As String
            Dim dt As DataTable
            Dim dr As DataRow
            Dim strRowIndex As String = Hid_RowIndex.Value
            If strRowIndex <> "" Then
                Dim intRowIndex As Integer = Convert.ToInt32(strRowIndex)
                strRetValues = Split(Hid_RetValues.Value, "!")
                dt = Session("FinalcialTable")
                If Trim(strRetValues(0) & "") = "N" Then
                    dt.Rows(intRowIndex).Item("PaymentDate") = Trim(strRetValues(3))
                    dt.Rows(intRowIndex).Item("BankName") = Trim(strRetValues(4))
                    dt.Rows(intRowIndex).Item("ChequeNumber") = Trim(strRetValues(5))
                    dt.Rows(intRowIndex).Item("ChequeDate") = Trim(strRetValues(6))
                End If
                dt.Rows(intRowIndex).Item("FDType") = Trim(strRetValues(0))
                dt.Rows(intRowIndex).Item("Amount") = Val(strRetValues(1))
                dt.Rows(intRowIndex).Item("Remark") = Trim(strRetValues(2))
                dt.Rows(intRowIndex).Item("DealSlipId") = Trim(strRetValues(7))
                dt.Rows(intRowIndex).Item("TransType") = Trim(strRetValues(8))
                Hid_TransType.Value = Trim(strRetValues(8))
                If Trim(strRetValues(8)) = "P" Then
                    dt.Rows(intRowIndex).Item("BankName") = Trim(strRetValues(9))
                End If
            Else
                strRetValues = Split(Hid_RetValues.Value, "!")
                dt = Session("FinalcialTable")
                dr = dt.NewRow
                If Trim(strRetValues(0) & "") = "N" Then
                    dr.Item("PaymentDate") = Trim(strRetValues(3))
                    dr.Item("BankName") = Trim(strRetValues(4))
                    dr.Item("ChequeNumber") = Trim(strRetValues(5))
                    dr.Item("ChequeDate") = Trim(strRetValues(6) & "")
                End If
                dr.Item("FDType") = Trim(strRetValues(0))
                dr.Item("Amount") = Val(strRetValues(1))
                dr.Item("Remark") = Trim(strRetValues(2))
                dr.Item("DealSlipId") = strRetValues(7)
                dr.Item("TransType") = strRetValues(8)
                If strRetValues(8) = "P" Then
                    dr.Item("BankName") = strRetValues(9)
                End If
                ' dr.Item("BankName") = strRetValues(9)
                dt.Rows.Add(dr)
            End If
            Session("FinalcialTable") = dt
            FillFinancialGrid()
            CalcTotal()
            Hid_RowIndex.Value = ""
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
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
End Class