Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_BulkDealSlipEntry
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim dblPepCoupRate As Double
    Dim LastIPDate As Date = Date.MinValue
    Dim sqlConn As SqlConnection
    Dim PurSettDate As Date
    Dim chrIntFlag As Char
    Dim decFaceValue As Decimal
    Dim decRate As Decimal
    Dim blnNonGovernment As Boolean
    Dim blnRateActual As Boolean
    Dim MatDate() As Date
    Dim MatAmt() As Double
    Dim CoupDate() As Date
    Dim CoupRate() As Double
    Dim intBKDiff As Integer
    Dim datIssue As Date
    Dim datInterest As Date
    Dim datYTM As Date
    Dim dblIntReceivable As Double
    Dim CostMemoFlag As Char
    Dim Costdealconf As String

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

            If rdo_BrokPerc.SelectedValue = "M" Then
                row_Per.Attributes.Add("style", "display:none")
            Else
                row_Per.Attributes.Add("style", "display:")
            End If

            Costdealconf = ""

            rdo_BrokPerc.Items(0).Attributes.Add("onclick", "Brokerage();")
            rdo_BrokPerc.Items(1).Attributes.Add("onclick", "Brokerage();")

            If IsPostBack = False Then

                SetAttributes()
                SetControls()
                Hid_Radio_opt.Value = "false"
                Session("DetailTable") = ""
                Hid_Id.Value = "A"

                btn_Save.Visible = True
                FillBlankDealGrids()
            Else
                If dg_Selected.Rows.Count > 0 Then DisableControls()
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "select", "CheckDealType();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "show", "ReferenceBy();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "xxx", "CheckPhysicalDMAT(true);", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "xx", "Showaddlnk();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "test1203", "CalcRoundofsettAMT();", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Sub FillBlankDealGrids()
        Try
            Dim dt As New DataTable
            Dim objSrh As New clsSearch

            dt.Columns.Add("SrNo", GetType(String))
            dt.Columns.Add("CustomerId", GetType(String))
            dt.Columns.Add("Customer", GetType(String))
            dt.Columns.Add("FaceValue", GetType(String))
            dt.Columns.Add("BrokerageAmt", GetType(String))
            dt.Columns.Add("FaceVal_WMult", GetType(String))
            dt.Columns.Add("NoOfBond", GetType(String))

            Session("Detailtable") = dt
            dg_Selected.DataSource = dt
            dg_Selected.DataBind()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Sub SetControls()
        Try
            BrokerCombo()

            'srh_IssuerOfSecurity.Columns.Add("SecurityIssuer")
            'srh_IssuerOfSecurity.Columns.Add("SecurityIssuer")

            'Srh_NameofSecurity.Columns.Add("SecurityName")
            'Srh_NameofSecurity.Columns.Add("SecurityTypeName")
            'Srh_NameofSecurity.Columns.Add("SecurityIssuer")
            'Srh_NameofSecurity.Columns.Add("SecurityId")

            'Srh_ReferenceBy.Columns.Add("CustomerName")
            'Srh_ReferenceBy.Columns.Add("CustomerCity")
            'Srh_ReferenceBy.Columns.Add("CustomerPhone")
            'Srh_ReferenceBy.Columns.Add("CustomerId")
            'srh_BrokingBTBDealSlipNo.Columns.Add("DealSlipNo")

            'srh_BrokingBTBDealSlipNo.Columns.Add("CM.CustomerName")
            'srh_BrokingBTBDealSlipNo.Columns.Add("SM.SecurityName")
            'srh_BrokingBTBDealSlipNo.Columns.Add("CONVERT(VARCHAR,DealDate,103) As DealDate")
            'srh_BrokingBTBDealSlipNo.Columns.Add("CONVERT(VARCHAR,SettmentDate,103) As SettlementDate ")
            'srh_BrokingBTBDealSlipNo.Columns.Add("Rate")
            'srh_BrokingBTBDealSlipNo.Columns.Add("RemainingFaceValue")
            'srh_BrokingBTBDealSlipNo.Columns.Add("DealSlipID")
            OpenConn()
            Dim dt As DataTable = objCommon.FillControl(cbo_SecurityType, sqlConn, "ID_FILL_SecurityTypeMaster1", "SecurityTypeName", "SecurityTypeId")
            objCommon.FillControl(cbo_Company, sqlConn, "ID_FILL_CompanyMaster1", "CompName", "CompId")

            Dim lstItem1 As ListItem = cbo_Company.Items.FindByValue(Trim(Session("CompId")))
            If lstItem1 IsNot Nothing Then cbo_Company.SelectedValue = lstItem1.Value
            cbo_Company.Enabled = False

            objCommon.FillControl(cbo_Bank, sqlConn, "ID_FILL_BankMaster1", "BankAccInfo", "BankId", Val(cbo_Company.SelectedValue), "CompId")
            dt = objCommon.FillControl(cbo_Demat, sqlConn, "ID_FILL_DMATMaster1", "DematClientInfo", "DMatId", Val(cbo_Company.SelectedValue), "CompId")

            Dim lstItem6 As ListItem = cbo_Demat.Items.FindByText(Trim("INDUSIND BANK LTD  -  10289266"))
            If lstItem6 IsNot Nothing Then cbo_Demat.SelectedValue = lstItem6.Value

            objCommon.FillControl(cbo_ReportedBy, sqlConn, "ID_FILL_BrokerMasterNew", "BrokerName", "BrokerId")
            objCommon.FillControl(cbo_DealerName, sqlConn, "ID_FILL_Dealer", "NameOfUser", "UserId")
            Dim lstItem As ListItem = cbo_DealerName.Items.FindByText(Trim(Session("NameOfUser")))
            If lstItem IsNot Nothing Then cbo_DealerName.SelectedValue = lstItem.Value

            objCommon.FillControl(cbo_ReferenceByDealer, sqlConn, "ID_FILL_Dealer", "NameOfUser", "UserId")
            Dim lstItem5 As ListItem = cbo_DealerName.Items.FindByText(Trim(Session("NameOfUser")))
            If lstItem5 IsNot Nothing Then cbo_DealerName.SelectedValue = lstItem5.Value
            objCommon.FillControl(cbo_SGLWith, sqlConn, "ID_FILL_SGLBankMaster1", "BankName", "SGLId", Val(cbo_Company.SelectedValue), "CompId")

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub SetAttributes()

        txt_DealDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_DealDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_DealDate.Text = Format(Now, "dd/MM/yyyy")
        txt_BrokPerc.Attributes.Add("onkeypress", "OnlyDecimal();")
        txt_SettmentDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_SettmentDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_SettmentDate.Text = Format(Now, "dd/MM/yyyy")
        btn_Save.Attributes.Add("onclick", "return  ValidationForSave();")
        btn_ShowSecurity.Attributes.Add("onclick", "return ShowSecurityMaster();")
        lbl_Dealar.Text = Trim(Session("NameOfUser"))
        cbo_DealTransType.Attributes.Add("onchange", "CheckDealType();")
        rdo_PhysicalDMAT.Attributes.Add("onclick", "CheckPhysicalDMAT(true);")

        btn_ShowBrokPurdeal.Attributes.Add("onclick", "return ShowPurDeal();")
        rdo_FinancialDealType.Attributes.Add("onclick", "CheckDealType()")
        'btn_CalRate.Attributes.Add("onclick", "return ShowYieldCalculation();")

    End Sub

    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        CalcAmt()
        SetSaveUpdate("ID_INSERT_BulkDealSlipEntry", False, True)
        OrderNoMsg()
    End Sub

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try

            Response.Redirect("PendingDealSlip.aspx", False)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Function SetSaveUpdate(ByVal strProc As String, Optional ByVal blnRedirect As Boolean = True,
                                   Optional ByVal blnGenDealNo As Boolean = False)
        Try

            OpenConn()
            Dim sqlTrans As SqlTransaction
            Dim preint As Double
            dblIntReceivable = 0.0

            If SaveUpdate(sqlTrans, strProc) = False Then Exit Function
            Response.Redirect("PendingDealSlip.aspx")

            Return True
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Function

    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try

            Dim sqlComm As New SqlCommand
            sqlComm.CommandText = strProc
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            sqlComm.Parameters.Clear()
            Dim DtDetail As DataTable
            Dim I As Integer

            OpenConn()
            DtDetail = Session("DetailTable")


            For I = 0 To DtDetail.Rows.Count - 1
                Using tran = sqlConn.BeginTransaction()

                    sqlComm.Transaction = tran
                    sqlComm.Parameters.Clear()

                    objCommon.SetCommandParameters(sqlComm, "@DealSlipID", SqlDbType.Int, 4, "O")
                    objCommon.SetCommandParameters(sqlComm, "@TransType", SqlDbType.Char, 1, "I", , , Trim(rbl_TypeOFTranction.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@PurMethod", SqlDbType.Char, 1, "I", , , DBNull.Value) 'null

                    objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , Trim(cbo_DealTransType.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@DealSlipType", SqlDbType.Char, 1, "I", , , Trim("D"))

                    objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , Val(Srh_NameofSecurity.SelectedId))
                    'poon
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , Val(DtDetail.Rows(I)("CustomerId")))
                    objCommon.SetCommandParameters(sqlComm, "@BrokCustomerId", SqlDbType.Int, 4, "I", , , DBNull.Value)

                    Dim FaceVal() As String = DtDetail.Rows(I)("FaceVal_WMult").ToString().Split("!")

                    objCommon.SetCommandParameters(sqlComm, "@FaceValue", SqlDbType.Decimal, 9, "I", , , Val(FaceVal(0)))
                    objCommon.SetCommandParameters(sqlComm, "@FaceValueMultiple", SqlDbType.BigInt, 8, "I", , , Val(FaceVal(1)))

                    objCommon.SetCommandParameters(sqlComm, "@NoOfBond", SqlDbType.Int, 4, "I", , , Val(DtDetail.Rows(I)("NoOfBond")))

                    objCommon.SetCommandParameters(sqlComm, "@DealDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_DealDate.Text))
                    objCommon.SetCommandParameters(sqlComm, "@SettmentDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_SettmentDate.Text))
                    objCommon.SetCommandParameters(sqlComm, "@Rate", SqlDbType.Decimal, 16, "I", , , Val(txt_Rate.Text))
                    objCommon.SetCommandParameters(sqlComm, "@ModeofDelivery", SqlDbType.Char, 1, "I", , , Trim(rdo_PhysicalDMAT.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@AccIntDays", SqlDbType.Char, 1, "I", , , Trim(rdo_AccIntDays.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@ModeOFPayment", SqlDbType.Char, 1, "I", , , Trim(cbo_ModeOfPayment.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(cbo_Company.SelectedValue))

                    If Val(cbo_Bank.SelectedValue) <> 0 Then
                        objCommon.SetCommandParameters(sqlComm, "@bankId", SqlDbType.Int, 4, "I", , , Val(cbo_Bank.SelectedValue))
                    End If
                    If Val(cbo_SGLWith.SelectedValue) <> 0 Then
                        objCommon.SetCommandParameters(sqlComm, "@SGLId", SqlDbType.Int, 4, "I", , , Val(cbo_SGLWith.SelectedValue))
                    End If

                    If rdo_brok_paid_type.SelectedValue = "P" Then
                        objCommon.SetCommandParameters(sqlComm, "@Brockpaid", SqlDbType.Decimal, 16, "I", , , Val(DtDetail.Rows(I)("BrokerageAmt")))
                    End If
                    If rdo_brok_paid_type.SelectedValue = "R" Then
                        objCommon.SetCommandParameters(sqlComm, "@BrockReceived", SqlDbType.Decimal, 16, "I", , , Val(DtDetail.Rows(I)("BrokerageAmt")))
                    End If

                    If cbo_DealTransType.SelectedValue = "B" Then
                        objCommon.SetCommandParameters(sqlComm, "@BTBDealSlipId", SqlDbType.Int, 4, "I", , , Val(srh_BrokingBTBDealSlipNo.SelectedId))
                    Else
                        '  objCommon.SetCommandParameters(sqlComm, "@BTBDealSlipId", SqlDbType.Int, 4, "I", , , Val(srh_BTBDealSlipNo.SelectedId))
                    End If

                    objCommon.SetCommandParameters(sqlComm, "@CancelDeal", SqlDbType.Bit, 1, "I", , , 0)
                    objCommon.SetCommandParameters(sqlComm, "@BrockEntry", SqlDbType.Bit, 1, "I", , , Val(chk_Brokerage1.Checked))
                    If Val(Hid_QuoteId.Value) <> 0 Then
                        objCommon.SetCommandParameters(sqlComm, "@QuoteId", SqlDbType.Int, 4, "I", , , Val(Hid_QuoteId.Value))
                    End If

                    objCommon.SetCommandParameters(sqlComm, "@DealDone", SqlDbType.Bit, 1, "I", , , 0)
                    objCommon.SetCommandParameters(sqlComm, "@RemainingFaceValue", SqlDbType.BigInt, 8, "I", , , (Val(FaceVal(0)) * Val(FaceVal(1))))

                    'objCommon.SetCommandParameters(sqlComm, "@RemainingFaceValue", SqlDbType.BigInt, 8, "I", , , (Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)))
                    objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                    objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 4, "O")


                    If Val(cbo_CounterCustSGLWith.SelectedValue) <> 0 And cbo_DealTransType.SelectedValue = "B" And rdo_PhysicalDMAT.SelectedValue = "S" Then
                        objCommon.SetCommandParameters(sqlComm, "@CounterpartySGLid", SqlDbType.Int, 4, "I", , , Val(cbo_CounterCustSGLWith.SelectedValue))
                    Else
                        objCommon.SetCommandParameters(sqlComm, "@CounterpartySGLid", SqlDbType.Int, 4, "I", , , DBNull.Value)
                    End If

                    If Val(cbo_CustSGL.SelectedValue) <> 0 Then
                        objCommon.SetCommandParameters(sqlComm, "@CustSGLId", SqlDbType.Int, 4, "I", , , Val(cbo_CustSGL.SelectedValue))
                    End If

                    If Val(cbo_Demat.SelectedValue) <> 0 Then
                        objCommon.SetCommandParameters(sqlComm, "@DMatId", SqlDbType.Int, 4, "I", , , Val(cbo_Demat.SelectedValue))
                    End If
                    objCommon.SetCommandParameters(sqlComm, "@CountContactPersons", SqlDbType.VarChar, 500, "I", , , DBNull.Value)
                    If LastIPDate <> Date.MinValue Then
                        objCommon.SetCommandParameters(sqlComm, "@LastIpDate", SqlDbType.DateTime, 4, "I", , , LastIPDate)
                    End If
                    If Val(cbo_ReportedBy.SelectedValue) <> 0 Then
                        objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.Int, 4, "I", , , Val(cbo_ReportedBy.SelectedValue))
                    Else
                        objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.Int, 4, "I", , , DBNull.Value)
                    End If

                    If Val(cbo_BrokeragePaidTo.SelectedValue) <> 0 Then
                        objCommon.SetCommandParameters(sqlComm, "@BrockPaidTo", SqlDbType.Int, 4, "I", , , Val(cbo_BrokeragePaidTo.SelectedValue))
                    Else
                        objCommon.SetCommandParameters(sqlComm, "@BrockPaidTo", SqlDbType.Int, 4, "I", , , DBNull.Value)
                    End If

                    objCommon.SetCommandParameters(sqlComm, "@WdmDeal", SqlDbType.Char, 1, "I", , , "D")

                    objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(cbo_DealerName.SelectedValue))

                    If Srh_ReferenceBy.SearchTextBox.Text = "" Then
                        objCommon.SetCommandParameters(sqlComm, "@ReferenceById", SqlDbType.Int, 4, "I", , , DBNull.Value)
                    Else
                        objCommon.SetCommandParameters(sqlComm, "@ReferenceById", SqlDbType.Int, 4, "I", , , Val(Srh_ReferenceBy.SelectedId))
                    End If

                    objCommon.SetCommandParameters(sqlComm, "@SettDays", SqlDbType.Char, 4, "I", , , Trim(cbo_SettDay.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@EntryUserId", SqlDbType.BigInt, 8, "I", , , Val(Session("UserId")))
                    objCommon.SetCommandParameters(sqlComm, "@FinancialDealType", SqlDbType.Char, 1, "I", , , Trim(rdo_FinancialDealType.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@RefDealerId", SqlDbType.BigInt, 8, "I", , , Val(cbo_ReferenceByDealer.SelectedValue))

                    objCommon.SetCommandParameters(sqlComm, "@FaceValueorNoOfBond", SqlDbType.Char, 1, "I", , , Trim(rdo_SelectOpt.SelectedValue))

                    objCommon.SetCommandParameters(sqlComm, "@CostMemoFlag", SqlDbType.Char, 1, "I", , , "D")
                    objCommon.SetCommandParameters(sqlComm, "@NextNo", SqlDbType.VarChar, 500, "O")
                    objCommon.SetCommandParameters(sqlComm, "@Convincumfromex", SqlDbType.Char, 1, "I", , , Trim(rdo_calcXInt.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@Dealisexintdeal", SqlDbType.Char, 1, "I", , , Trim(rdo_PreviousdealType.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@BulkDeal", SqlDbType.Int, 4, "I", , , 1)

                    sqlComm.ExecuteNonQuery()
                    tran.Commit()
                End Using

            Next

            ViewState("Id") = sqlComm.Parameters("@DealSlipID").Value
            Hid_bond.Value = ""
            Hid_DealSlipId.Value = ViewState("Id")

            If cbo_DealTransType.SelectedValue = "B" And rbl_TypeOFTranction.SelectedValue = "S" Then
                Hid_SellBrokDealSlipId.Value = ViewState("Id")
            End If
            If cbo_DealTransType.SelectedValue = "B" And rbl_TypeOFTranction.SelectedValue = "P" Then
                Hid_PurBrokDealSlipId.Value = ViewState("Id")
            End If

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally

            CloseConn()

        End Try
    End Function

    Private Sub OrderNoMsg()
        Try
            'If (Trim(txt_DealSlipNo.Text) <> "") Then
            '    Dim strHtml As String
            '    Dim msg As String = "Deal No " + txt_DealSlipNo.Text + " is Generated"
            '    strHtml = "alert('" + msg + "');"
            '    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
            'End If
        Catch ex As Exception

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillInterestFieldFromdatabase()
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_DealSlipEntry", ViewState("Id"), "DealSlipID")
            If dt.Rows.Count > 0 Then
                Hid_Amt.Value = Trim(dt.Rows(0).Item("Amount") & "")
                Hid_AddInterest.Value = Trim(dt.Rows(0).Item("InterestAmt") & "")
                Hid_IntDays.Value = Trim(dt.Rows(0).Item("InterestDays") & "")
                Hid_InterestFromTo.Value = Trim(dt.Rows(0).Item("InterestFromToDates") & "")
                Hid_SettlementAmt.Value = Trim(dt.Rows(0).Item("Settamtbeforeroundoff") & "")
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub FillSecurityDetails()
        Try
            OpenConn()
            Dim dt As DataTable

            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_Securitydetails", Srh_NameofSecurity.SelectedId, "SecurityId")
            If dt.Rows.Count > 0 Then
                cbo_SecurityType.SelectedValue = Val(dt.Rows(0).Item("SecurityTypeId").ToString)
                srh_IssuerOfSecurity.SearchTextBox.Text = dt.Rows(0).Item("SecurityIssuer").ToString
                srh_IssuerOfSecurity.SelectedId = dt.Rows(0).Item("SecurityIssuer").ToString
                srh_IssuerOfSecurity.SelectedFieldText = dt.Rows(0).Item("SecurityIssuer").ToString
                Srh_NameofSecurity.SearchTextBox.Text = dt.Rows(0).Item("SecurityName").ToString
                Srh_NameofSecurity.SelectedFieldText = dt.Rows(0).Item("SecurityName").ToString
                Hid_NSDLFaceValue.Value = Val(dt.Rows(0).Item("NSDLFaceValue").ToString)

                Hid_FirstInterestDate.Value = Trim(dt.Rows(0).Item("FirstInterestDate") & "")
                Hid_CouponRate.Value = Trim(dt.Rows(0).Item("CouponRate") & "")

            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Protected Sub Srh_NameofSecurity_ButtonClick() Handles Srh_NameofSecurity.ButtonClick
        Try
            FillSecurityDetails()
            Hid_SecId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Convert.ToString(Srh_NameofSecurity.SelectedId)))
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "select", "CheckDealType();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "show", "ReferenceBy();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "xxx", "CheckDealType();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "xx", "Showaddlnk();", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "test1203", "CalcRoundofsettAMT();", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Sub FillAccuredInterestOptions()
        Try
            Dim datBookClosure As Date = Date.MinValue
            Dim blnDMAT As Boolean = True
            Dim FinalAmt As Double = 0
            Dim IntDate As Date = Date.MinValue
            Dim AddLess As String = ""
            Dim AddLessNoofDays As Integer = 0
            Dim Ratio As Double = 0
            Dim IntAmount As Double = 0
            Dim intDays As Int16 = 0
            Dim intIncludeInt As Int16 = 0
            Dim tmp_ExInt As Char

            MatDate = FillDateArrays(Hid_MatDate.Value)
            MatAmt = FillAmtArrays(Hid_MatAmt.Value)
            CoupDate = FillDateArrays(Hid_CoupDate.Value)
            CoupRate = FillAmtArrays(Hid_CoupRate.Value)

            With objCommon

                datYTM = .DateFormat(txt_SettmentDate.Text)
                datInterest = Hid_InterestDate.Value
                datBookClosure = Hid_BookClosureDate.Value
                blnNonGovernment = IIf(Hid_GovernmentFlag.Value = "N", True, False)
                blnRateActual = True

                If rdo_SelectOpt.Items(0).Selected Then
                    txt_Amount.Text = decFaceValue / 100000
                    cbo_Amount.SelectedIndex = 1
                Else
                    decFaceValue = (Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue))
                End If

                decRate = .DecimalFormat(Val(txt_Rate.Text))
                datIssue = Hid_Issue.Value
                datBookClosure = IIf(blnDMAT = True, Hid_DMATBkDate.Value, Hid_BookClosureDate.Value)
                intBKDiff = CalculateBookClosureDiff(datBookClosure, "D", datInterest, blnNonGovernment)

                If blnNonGovernment = False Then
                    intDays = 360
                Else
                    If rdo_AccIntDays.SelectedValue = 2 Then
                        intDays = 366
                    Else
                        intDays = 365
                    End If
                End If
            End With
            Hid_Quantity.Value = ((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue))) / decFaceValue
            intIncludeInt = 0
            datYTM = DateAdd(DateInterval.Day, intIncludeInt, datYTM)
            tmp_ExInt = rdo_calcXInt.SelectedValue
            CalculateAccuredInterest(datYTM, decFaceValue, decRate, blnNonGovernment, blnRateActual,
                  MatDate, MatAmt, CoupDate, CoupRate, intBKDiff, datInterest, datIssue, Hid_Frequency.Value,
                  intDays, FinalAmt, IntDate, AddLess, AddLessNoofDays, Ratio, IntAmount, tmp_ExInt)

            'dev 201010
            Hid_Amtshow.Value = ""
            Hid_ShowInterest.Value = ""
            Hid_Amt.Value = ""
            Hid_AddInterest.Value = ""
            Hid_IntDays.Value = ""
            Hid_SettlementAmt.Value = ""

            Hid_Amtshow.Value = RoundToTwo((Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue)) * Val(txt_Rate.Text) / 100)
            Hid_ShowInterest.Value = RoundToTwo(IntAmount * Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue))
            Hid_Amt.Value = RoundToTwo(Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue) * Val(txt_Rate.Text) / 100)
            Hid_AddInterest.Value = RoundToTwo(IntAmount * Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue))
            If AddLess = "L" Then
                Hid_IntDays.Value = -1 * Val(AddLessNoofDays & "")
                Hid_SettlementAmt.Value = Val(Hid_Amtshow.Value) - Val(Hid_ShowInterest.Value)
                Hid_AddInterest.Value = -1 * Val(Hid_AddInterest.Value)
            Else
                Hid_IntDays.Value = Val(AddLessNoofDays & "")
                Hid_SettlementAmt.Value = Val(Hid_Amtshow.Value) + Val(Hid_ShowInterest.Value)
            End If
            If AddLess = "A" Then
                Hid_InterestFromTo.Value = "(" & Format(IntDate, "dd/MM/yyyy") & " - " & Format(datYTM, "dd/MM/yyyy") & ")"
            Else
                Hid_InterestFromTo.Value = "(" & Format(datYTM, "dd/MM/yyyy") & " - " & Format(IntDate, "dd/MM/yyyy") & ")"
            End If
            LastIPDate = IntDate
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Private Function FillDateArrays(ByVal strValue As String) As Date()
        Try
            Dim strDate() As String
            Dim arrDate() As Date
            Dim I As Int32
            strDate = Split(strValue, "!")
            ReDim arrDate(strDate.Length - 2)
            For I = 0 To strDate.Length - 2
                If strDate(I) <> "" Then arrDate(I) = CDate(strDate(I))
            Next
            Return arrDate
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Private Function FillAmtArrays(ByVal strValue As String) As Double()
        Try
            Dim strDate() As String
            Dim arrDouble() As Double
            Dim I As Int32
            strDate = Split(strValue, "!")
            ReDim arrDouble(strDate.Length - 2)
            For I = 0 To strDate.Length - 2
                If strDate(I) <> "" Then arrDouble(I) = CDbl(strDate(I))
            Next
            Return arrDouble
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Function RoundToTwo(ByVal dec As Decimal) As Decimal
        Try
            Dim rounded As Decimal = Decimal.Round(dec, 2)
            rounded = Format(rounded, "###################0.00")
            Return rounded
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function

    Private Sub FillSecurityDetailsDeal()
        Try
            OpenConn()
            Dim dt As DataTable
            Dim strSecurityNature As String
            Dim I As Int32
            Dim datInfo As Date
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityInfo", Srh_NameofSecurity.SelectedId, "SecurityId")
            For I = 0 To dt.Rows().Count - 1
                With dt.Rows(I)
                    chrMaxActFlag = Trim(.Item("MaxActualFlag") & "")
                    strSecurityNature = Trim(.Item("NatureOfInstrument") & "")
                    srh_IssuerOfSecurity.SearchTextBox.Text = Trim(.Item("SecurityIssuer") & "")
                    srh_IssuerOfSecurity.SelectedId = Trim(.Item("SecurityIssuer") & "")
                    Srh_NameofSecurity.SearchTextBox.Text = Trim(.Item("SecurityName") & "")
                    lbl_IPDates.Text = Trim(.Item("IPDates") & "")
                    Hid_BookClosureDate.Value = IIf(Trim(.Item("BookClosureDate") & "") = "", Date.MinValue, .Item("BookClosureDate"))
                    Hid_InterestDate.Value = IIf(Trim(.Item("FirstInterestDate") & "") = "", Date.MinValue, .Item("FirstInterestDate"))
                    Hid_DMATBkDate.Value = IIf(Trim(.Item("DMATBookClosureDate") & "") = "", Date.MinValue, .Item("DMATBookClosureDate"))
                    Hid_Issue.Value = IIf(Trim(.Item("IssueDate") & "") = "", Date.MinValue, .Item("IssueDate"))
                    datInfo = IIf(Trim(.Item("SecurityInfoDate") & "") = "", Date.MinValue, .Item("SecurityInfoDate"))
                    Hid_GovernmentFlag.Value = Trim(.Item("GovernmentFlag") & "")
                    Hid_Frequency.Value = GetFrequency(Trim(.Item("FrequencyOfInterest") & ""))

                    If Val(.Item("NSDLFaceValue") & "") <> 0 Then
                        Hid_FaceValue.Value = Val(.Item("NSDLFaceValue") & "")
                    Else
                        Hid_FaceValue.Value = Val(.Item("FaceValue") & "")
                    End If
                    FillSecurityInfoDetails(datInfo, Val(.Item("SecurityInfoAmt") & ""), Trim(.Item("TypeFlag") & ""))
                End With
            Next
            If strSecurityNature = "P" Then
                Hid_CoupDate.Value += CStr(#12/31/9999#) & "!"
                Hid_CoupRate.Value += dblPepCoupRate & "!"
            End If
            If Hid_GovernmentFlag.Value = "G" Then
                'Hid_Days.Visible = False
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
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

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Function
    Private Sub FillSecurityInfoDetails(ByVal InfoDate As Date, ByVal InfoAmt As Decimal, ByVal TypeFlag As String)
        Try
            Select Case TypeFlag
                Case "M"
                    Hid_MatDate.Value += InfoDate & "!"
                    Hid_MatAmt.Value += InfoAmt & "!"
                Case "I"
                    If InfoDate <> Date.MinValue Then
                        Hid_CoupDate.Value += InfoDate & "!"
                        Hid_CoupRate.Value += InfoAmt & "!"
                    Else
                        dblPepCoupRate = InfoAmt
                    End If
                Case "C"
                    Hid_CallDate.Value += InfoDate & "!"
                    Hid_CallAmt.Value += InfoAmt & "!"
                Case "P"
                    Hid_PutDate.Value += InfoDate & "!"
                    Hid_PutAmt.Value += InfoAmt & "!"
            End Select
        Catch ex As Exception

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub cbo_SecurityType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SecurityType.SelectedIndexChanged
        srh_IssuerOfSecurity.SearchTextBox.Text = ""
        Srh_NameofSecurity.SearchTextBox.Text = ""
        txt_Amount.Text = ""
    End Sub

    Protected Sub srh_IssuerOfSecurity_ButtonClick() Handles srh_IssuerOfSecurity.ButtonClick
        Srh_NameofSecurity.SearchTextBox.Text = ""
        GetSecurityIssuer()
    End Sub

    Private Sub GetSecurityIssuer()
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_GET_SecurityName"
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@SecurityIssuer", SqlDbType.VarChar, 500, "I", , , Trim(srh_IssuerOfSecurity.SearchTextBox.Text))
            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 8, "O")
            objCommon.SetCommandParameters(sqlComm, "@SecurityName", SqlDbType.VarChar, 50, "O")
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            If Trim(sqlComm.Parameters("@SecurityName").Value) <> "" Then
                Srh_NameofSecurity.SelectedId = Val(sqlComm.Parameters("@SecurityId").Value & "")
                Srh_NameofSecurity.SelectedFieldText = Trim(sqlComm.Parameters("@SecurityName").Value & "")
                Srh_NameofSecurity_ButtonClick()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
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

    Private Sub CalcAmt()
        FillSecurityDetailsDeal()
        If Val(Hid_Frequency.Value) <> 0 Then
            FillAccuredInterestOptions()
        End If
    End Sub
    Protected Sub srh_BrokingBTBDealSlipNo_ButtonClick() Handles srh_BrokingBTBDealSlipNo.ButtonClick
        ' fillBrokingfields()
        ' brokingreadonlyfields()

    End Sub

    Private Sub BrokerCombo()
        Try

            OpenConn()
            If cbo_DealTransType.SelectedValue = "B" Then

                objCommon.FillControl(cbo_BrokeragePaidTo, sqlConn, "ID_FILL_BrokerMasterNew", "BrokerName", "BrokerId")
                Dim lstItem3 As ListItem = cbo_BrokeragePaidTo.Items.FindByText("TRUST CAPITAL SERVICES (INDIA) PVT. LTD.")
                Dim brName1 As String = lstItem3.Text
                If brName1.ToUpper.IndexOf("TRUST CAPITAL") <> -1 Then
                    If lstItem3 IsNot Nothing Then cbo_BrokeragePaidTo.SelectedValue = lstItem3.Value
                End If

            Else
                objCommon.FillControl(cbo_BrokeragePaidTo, sqlConn, "ID_FILL_BrokerMasterNew", "BrokerName", "BrokerId")
            End If
            CloseConn()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Protected Sub cbo_DealTransType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DealTransType.SelectedIndexChanged
        Try
            BrokerCombo()
            If cbo_DealTransType.SelectedValue = "F" Then
                If rbl_TypeOFTranction.SelectedValue = "P" Then
                    rdo_FinancialDealType.Items(1).Text = "To Sell"
                Else
                    rdo_FinancialDealType.Items(1).Text = "From Purchase"
                End If
            End If

            If cbo_DealTransType.SelectedValue = "B" Then
                If rbl_TypeOFTranction.SelectedValue = "P" Then
                    rbl_TypeOFTranction.SelectedValue = "P"
                    rbl_TypeOFTranction.Enabled = False
                Else
                    rbl_TypeOFTranction.SelectedValue = "P"
                    rbl_TypeOFTranction.Enabled = False
                    Dim strHtml As String
                    Dim msg As String = "You cannot enter broking sell deal"
                    strHtml = "alert('" + msg + "');"
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
                End If
            Else
                rbl_TypeOFTranction.Enabled = True
            End If
            If cbo_DealTransType.SelectedValue = "F" Then
                rbl_TypeOFTranction.SelectedValue = "S"
                rbl_TypeOFTranction.Enabled = False
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try

    End Sub

    Protected Sub cbo_SettDay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SettDay.SelectedIndexChanged
        Try
            FillSettDate()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try

    End Sub
    Private Sub FillSettDate()
        Try
            Dim intLoop As Int16 = 0
            Dim count As Integer = 0
            Dim incDate As Date

            incDate = objCommon.DateFormat(txt_DealDate.Text)
            While count < cbo_SettDay.SelectedValue
                incDate = DateAdd(DateInterval.Day, 1, incDate)
                If checkdate(incDate) = True Then
                    count = count - 1
                End If
                count = count + 1
            End While
            txt_SettmentDate.Text = Format(incDate, "dd/MM/yyyy")

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Function checkdate(ByVal incDate As Date) As Boolean
        Try

            Dim Sqlcomm As New SqlCommand
            OpenConn()
            With Sqlcomm

                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_FILL_HolidaysNew"
                .Parameters.Clear()
                objCommon.SetCommandParameters(Sqlcomm, "@YearId", SqlDbType.Int, 4, "I", , , Session("YearId"))
                objCommon.SetCommandParameters(Sqlcomm, "@Month", SqlDbType.Int, 4, "I", , , incDate.Month)
                objCommon.SetCommandParameters(Sqlcomm, "@HolidayDate", SqlDbType.DateTime, 4, "I", , , incDate)
                objCommon.SetCommandParameters(Sqlcomm, "@RET_CODE", SqlDbType.Int, 4, "O")
            End With
            If Sqlcomm.ExecuteScalar > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Function

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Protected Sub rdo_calcXInt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdo_calcXInt.SelectedIndexChanged
        Try
            FillSecurityDetailsDeal()
            If Val(Hid_Frequency.Value) <> 0 Then
                FillAccuredInterestOptions()
            Else
                If rdo_SelectOpt.SelectedValue = "B" Then
                    Hid_AddInterest.Value = 0
                ElseIf rdo_SelectOpt.SelectedValue = "F" Then
                    Hid_Amt.Value = Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue) * Val(txt_Rate.Text) / 100
                    Hid_AddInterest.Value = 0
                    Hid_SettlementAmt.Value = Val(txt_Amount.Text) * Val(cbo_Amount.SelectedValue) * Val(txt_Rate.Text) / 100
                End If
            End If

            If (Val(Hid_IntDays.Value) < 0) Then
                If Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Then
                    rdo_PreviousdealType.SelectedValue = "Y"
                    rdo_PreviousdealType.Enabled = False
                ElseIf Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                    rdo_PreviousdealType.SelectedValue = "Y"
                    rdo_PreviousdealType.Enabled = False
                End If
                tr_calcXInt.Visible = True
                tr_PreviousdealType.Visible = True

            Else
                If Trim(Request.QueryString("page") & "") = "PendingDealSlip.aspx" Then
                    rdo_PreviousdealType.Enabled = False
                ElseIf Trim(Request.QueryString("page") & "") = "GeneratedDealSlip.aspx" Then
                    rdo_PreviousdealType.Enabled = False
                End If
                If rdo_PreviousdealType.SelectedValue = "Y" And rdo_calcXInt.SelectedValue = "N" Then
                    tr_PreviousdealType.Visible = True
                    tr_calcXInt.Visible = True
                Else
                    tr_PreviousdealType.Visible = False
                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Function Delete()
        Dim sqlComm As New SqlCommand
        Try
            OpenConn()
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.Text
            sqlComm.CommandText = "delete from TempPurdeal"
            sqlComm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        Finally
            CloseConn()
        End Try
    End Function

    Public Sub add_click()
        Try

            Dim imgBtn As ImageButton
            Dim dgItem As DataGridItem

            ViewState("RowIndex") = dgItem.DataSetIndex

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub btn_addDet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_addDet.Click
        If Not (Hid_RetVal.Value = "" Or Hid_RetVal.Value = "undefined") Then
            AddDetails()
            CalCulateTotalAmt()
        End If

    End Sub

    Private Sub AddDetails()
        Try
            Dim strRetValues() As String
            Dim dt As DataTable
            Dim dr As DataRow
            strRetValues = Split(Hid_RetVal.Value, "!")
            DisableControls()
            Dim strRowIndex As String = Hid_RowIndex.Value
            If strRowIndex <> "" Then
                Dim intRowIndex As Integer = Convert.ToInt32(strRowIndex)
                FillGridView(intRowIndex)
            Else
                If Not (Hid_RetVal.Value = "" Or Hid_RetVal.Value = "false") Then
                    dt = Session("Detailtable")
                    Dim view As New DataView(dt)
                    view.Sort = "CustomerId"

                    If view.Find(Val(strRetValues(0))) <> -1 Then
                        'record exists
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Customer Already Exists');", True)
                    Else
                        'record doesn't exist
                        dr = dt.NewRow
                        dr.Item("SrNo") = dg_Selected.Rows.Count + 1
                        dr.Item("CustomerId") = Val(strRetValues(0))
                        dr.Item("Customer") = Trim(strRetValues(1))
                        dr.Item("FaceValue") = Trim(strRetValues(2) * strRetValues(3))
                        dr.Item("BrokerageAmt") = Trim(strRetValues(4))
                        dr.Item("FaceVal_WMult") = strRetValues(2) + "!" + strRetValues(3)
                        dr.Item("NoOfBond") = strRetValues(5)

                        dt.Rows.Add(dr)

                        Session("Detailtable") = dt

                        dg_Selected.DataSource = dt
                        dg_Selected.DataBind()

                        If Hid_Radio_opt.Value = "true" Then
                            rdo_BrokPerc.SelectedValue = "M"
                            'row_Per.Style("display") = "none"
                        Else
                            rdo_BrokPerc.SelectedValue = "F"
                            row_Per.Attributes.Add("style", "display:")
                        End If
                    End If
                End If
            End If

            Hid_RowIndex.Value = ""
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub DisableControls()

        'cbo_DealTransType.Enabled = False
        'cbo_SecurityType.Enabled = False
        'srh_IssuerOfSecurity.SearchButton.Enabled = False
        'Srh_NameofSecurity.SearchButton.Enabled = False
        'rbl_TypeOFTranction.Enabled = False

    End Sub
    Private Sub EnableControls()

        cbo_DealTransType.Enabled = True
        cbo_SecurityType.Enabled = True
        'srh_IssuerOfSecurity.SearchButton.Enabled = True
        'Srh_NameofSecurity.SearchButton.Enabled = True
        rbl_TypeOFTranction.Enabled = True
    End Sub
    Private Sub CalCulateTotalAmt()
        Try
            Dim dblTotAmt As Double
            Dim dt As DataTable
            Dim dbltot As Double

            dt = Session("Detailtable")

            For I As Integer = 0 To dt.Rows.Count - 1
                dblTotAmt = dblTotAmt + dt.Rows(I).Item("FaceValue")
                dbltot = dblTotAmt
            Next

            fnt_TotalAmt.Text = dbltot
            fnt_TotalCnt.Text = dt.Rows.Count


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub dg_Selected_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dg_Selected.RowCommand

        Try
            Dim dt As DataTable
            Dim strRetValues() As String
            Dim imgBtn As ImageButton
            Dim gvRow As GridViewRow
            Dim i As Integer

            dt = CType(Session("DetailTable"), DataTable)
            imgBtn = TryCast(e.CommandSource, ImageButton)
            gvRow = imgBtn.Parent.Parent


            If e.CommandName = "EditRow" Then
                Hid_Id.Value = "E"
                Hid_dgselect_index.Value = gvRow.DataItemIndex.ToString()


            Else
                dt.Rows.RemoveAt(gvRow.DataItemIndex)

                For i = 0 To dt.Rows.Count - 1
                    dt.Rows(i)("SrNo") = i + 1
                Next

                If dt.Rows.Count <= 0 Then EnableControls()
                dg_Selected.DataSource = dt
                dg_Selected.DataBind()
            End If

            Session("DetailTable") = dt

            'dg_Selected.DataSource = dt
            'dg_Selected.DataBind()
            CalCulateTotalAmt()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub
    Private Sub FillGridView(ByVal selindex)
        Try
            Dim dblTotAmt As Double
            Dim dt As DataTable
            Dim dbltot As Double
            Dim strRetValues() As String

            dt = Session("Detailtable")

            If Not (Hid_RetVal.Value = "") Then

                strRetValues = Split(Hid_RetVal.Value, "!")
                If (strRetValues(6) = "'E'") Then
                    If Not (Hid_RetVal.Value = "" Or Hid_RetVal.Value = "false") Then
                        dt = Session("Detailtable")
                        Dim view As New DataView(dt)

                        view.RowFilter = "SrNo <>" + (selindex + 1).ToString()
                        view.Sort = "CustomerId"

                        If view.Find(Val(strRetValues(0))) <> -1 Then
                            'record exists
                            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Customer Already Exists');", True)
                        Else
                            dt.Rows(strRetValues(7)).Item("CustomerId") = Val(strRetValues(0) & "")
                            dt.Rows(strRetValues(7)).Item("Customer") = Trim(strRetValues(1) & "")
                            dt.Rows(strRetValues(7)).Item("FaceValue") = Trim(strRetValues(2) * strRetValues(3) & "")
                            dt.Rows(strRetValues(7)).Item("BrokerageAmt") = Trim(strRetValues(4) & "")
                            dt.Rows(strRetValues(7)).Item("FaceVal_WMult") = Trim(strRetValues(2) + "!" + strRetValues(3) & "")
                            dt.Rows(strRetValues(7)).Item("NoOfBond") = strRetValues(5)

                            dg_Selected.DataSource = dt
                            dg_Selected.DataBind()


                            For I As Integer = 0 To dt.Rows.Count - 1
                                dblTotAmt = dblTotAmt + dt.Rows(I).Item("FaceValue")
                                dbltot = dblTotAmt
                            Next

                            fnt_TotalAmt.Text = dbltot
                            fnt_TotalCnt.Text = dt.Rows.Count
                        End If
                    End If


                End If


                ' dt.Rows(gvRow.DataItemIndex)("SrNo") = Format(Val(strRetValues(0) & ""), "#,###")


            End If


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

    Protected Sub dg_Selected_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dg_Selected.RowDataBound
        Try

            Dim lnkBtnDelete As ImageButton
            Dim lnkBtnEdit As ImageButton
            Dim dt As DataTable
            Dim dr As DataRow
            Dim strVal As String

            dt = CType(Session("DetailTable"), DataTable)

            If e.Row.RowType = DataControlRowType.DataRow Then
                ' e.Row.ID = "itm" & e.Row.DataItemIndex

                dr = dt.Rows(e.Row.DataItemIndex)

                strVal = e.Row.DataItemIndex
                strVal = strVal + "*" + dr.Item("SrNo")
                strVal = strVal + "*" + dr.Item("CustomerId")
                strVal = strVal + "*" + dr.Item("CustomerId")
                'strText.Replace("$", "S"))
                'strVal = strVal + "," + dr.Item("CustomerId")
                strVal = strVal + "*" + dr.Item("FaceValue")
                strVal = strVal + "*" + dr.Item("BrokerageAmt")
                strVal = strVal + "*" + dr.Item("FaceVal_WMult")
                strVal = strVal + "*" + dr.Item("NoOfBond")

                lnkBtnDelete = CType(e.Row.FindControl("imgBtn_Delete"), ImageButton)
                lnkBtnDelete.Attributes.Add("onclick", "return Delete_entry()")

                lnkBtnEdit = CType(e.Row.FindControl("imgBtn_Edit"), ImageButton)
                lnkBtnEdit.Attributes.Add("onclick", "return UpdateDetails('" & strVal & "','" & e.Row.RowIndex & "')")


                'If Hid_Radio_opt.Value = "true" Then
                '    rdo_BrokPerc.SelectedIndex = 0
                '    row_Per.Style("display") = "none"
                'Else
                '    rdo_BrokPerc.SelectedIndex = 1
                'End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "AlertMessage(Validation,'" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "',175,450);", True)
        End Try
    End Sub

End Class


