Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_PrintDeals
    Inherits System.Web.UI.Page
    Dim sqlConn As SqlConnection
    Dim objCommon As New clsCommonFuns

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
            btn_MergePrintDealSlip.Visible = False
            btn_MergeSGLLetter.Visible = False
            btn_MergeGenerateContractNote.Visible = False
            Hid_UserId.Value = Val(Session("UserId") & "")
            Hid_UserTypeId.Value = Val(Session("UserTypeId") & "")
            If IsPostBack = False Then
                SetControls()
                SetAttributes()
                'FillDealSlipFields()
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
        btn_MergeDealConfirmation.Attributes.Add("onclick", "return DealConfReportView('');")

        ' btn_MergeSGLLetter.Attributes.Add("onclick", "return MergeDealReportView();")
        btn_SGLFedFormatM.Attributes.Add("onclick", "return FedralSglLatterView();")
        btn_SGLHDFCFormatM.Attributes.Add("onclick", "return HDFCSglLatterView();")
        btn_NDSOM.Attributes.Add("onclick", "return OpenReport('NSDOM','');")
        btn_ExpNDSOM.Attributes.Add("onclick", "return OpenReport('NSDOM','PW');")
        btn_TCSAnnex.Attributes.Add("onclick", "return OpenReport('TCSAnnex','PP');")

        rdo_DateType.Attributes.Add("onclick", "DateType();")
        'btn_Save.Attributes.Add("onclick", "return submitvalidation();")
        btn_DealConfirmation.Attributes.Add("onclick", "return OpenReport('DealConf','');")
        btn_DealConfirmationWord.Attributes.Add("onclick", "return OpenReport('DealConf','PW');")

        btn_DealConfirmationPDF.Attributes.Add("onclick", "return OpenReport('DealConf','PP');")

        btn_PrintDealSlip.Attributes.Add("onclick", "return OpenReport('DealTicket','');")
        btn_DealSlipPDF.Attributes.Add("onclick", "return OpenReport('DealTicket','PP');")
        btn_SGLLetter.Attributes.Add("onclick", "return OpenReport('SGLLetter');")
        btn_CSGL.Attributes.Add("onclick", "return OpenReport('CSGL','PW');")
        btn_SGLFedFormat.Attributes.Add("onclick", "return OpenReport('SGLFedFormat');")
        btn_SGLHDFCFormat.Attributes.Add("onclick", "return OpenReport('SGLHDFCFormat');")
        btn_SGLFedFormatWord.Attributes.Add("onclick", "return OpenReport('SGLFedFormat','PW');")
        btn_SGLFedFormatPDF.Attributes.Add("onclick", "return OpenReport('SGLFedFormat','PP');")
        btn_SGLHDFCFormatWord.Attributes.Add("onclick", "return OpenReport('SGLHDFCFormat','PW');")
        btn_PhyAuthority.Attributes.Add("onclick", "return OpenReport('PhysicalAuthority');")
        'btn_FinAuth.Attributes.Add("onclick", "return Validate();")
        btn_FinAuth.Attributes.Add("onclick", "return OpenReport('FinancialAuthority');")

        btn_PhyAuthorityM.Attributes.Add("onclick", "return OpenMergeReport('PhysicalAuthorityMerge');")
        'btn_FinAuth.Attributes.Add("onclick", "return Validate();")
        btn_FinAuthM.Attributes.Add("onclick", "return OpenMergeReport('FinancialAuthorityMerge');")
        btn_MergePrintDealSlip.Attributes.Add("onclick", "return DealConfReportView();")

        btn_MergeDealConfirmationW.Attributes.Add("onclick", "return DealConfReportView('PW');")
        btn_GenerateContractNote.Attributes.Add("onclick", "return GenerateContractNote('" & Val(srh_TransCode.SelectedId & "") & "')")
        btn_FinAuth.Enabled = False
        btn_PhyAuthority.Enabled = False
        btn_DealConfirmation.Enabled = False
        btn_PrintDealSlip.Enabled = False
        btn_SGLLetter.Visible = False
        btn_CSGL.Visible = False
        btn_SGLFedFormat.Visible = False
        btn_SGLHDFCFormat.Visible = False
        btn_GenerateContractNote.Enabled = False
        btn_DealConfirmationWord.Enabled = False
        btn_DealConfirmationPDF.Enabled = False
        btn_DealSlipPDF.Enabled = False
        btn_SGLFedFormatWord.Visible = False
        btn_SGLHDFCFormatWord.Visible = False
        btn_NDSOM.Enabled = False
        btn_ExpNDSOM.Enabled = False
        btn_TCSAnnex.Enabled = False
        btn_MergePrintDealSlip.Enabled = False
        btn_MergeDealConfirmation.Enabled = False
        btn_MergeDealConfirmationW.Enabled = False
        btn_MergeSGLLetter.Enabled = False
        btn_MergeSGLLetter.Enabled = False
        btn_MergeGenerateContractNote.Enabled = False
        btn_SGLFedFormatM.Enabled = False
        btn_SGLHDFCFormatM.Enabled = False
        btn_PhyAuthorityM.Enabled = False
        btn_FinAuthM.Enabled = False
        rdo_PrntLetterHead.Visible = False
        'btn_SGLFedFormatWord.Enabled = False
        'btn_SGLHDFCFormatWord.Enabled = False

    End Sub
    Private Sub SetControls()
        Try
            'srh_TransCode.Columns.Add("DealSlipNo")
            'srh_TransCode.Columns.Add("convert(varchar,DealDate,103) as TradeDate")
            'srh_TransCode.Columns.Add("CustomerName")
            'srh_TransCode.Columns.Add("SecurityName")
            'srh_TransCode.Columns.Add("DealDate")
            'srh_TransCode.Columns.Add("DealSlipId")

            'Srh_MergeTrnsCode.Columns.Add("MergedealNo")
            'Srh_MergeTrnsCode.Columns.Add("dbo.FA_GET_DealNo_New1(MergedealNo) as DealNo")
            'Srh_MergeTrnsCode.Columns.Add("MergedealNo")

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillDealSlipFields()
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_PrintDealSlip", srh_TransCode.SelectedId, "DealSlipID")

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
                    Hid_SettDate.Value = dt.Rows(0).Item("SettmentDate")
                End If
                If Trim((dt.Rows(0).Item("Rate")) & "") <> "" Then
                    lit_Rate.Text = Format(dt.Rows(0).Item("Rate"), "")
                End If
                srh_TransCode.SearchTextBox.Text = Trim(dt.Rows(0).Item("DealSlipNo") & "")
                Hid_DealSlipNo.Value = Trim(dt.Rows(0).Item("DealSlipNo") & "")
                srh_TransCode.SelectedId = Val(dt.Rows(0).Item("DealSlipId") & "")
                Hid_DealSlipId.Value = Val(dt.Rows(0).Item("DealSlipId") & "")
                Hid_DealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_DealSlipId.Value))
                Hid_DealTransType.Value = Trim(dt.Rows(0).Item("DealTransType") & "")
                lit_ModeofDelivery.Text = Trim(dt.Rows(0).Item("ModeofDelivery") & "")
                Hid_PhysicalDMAT.Value = Trim(dt.Rows(0).Item("ModeofDelivery") & "")

                If Trim(dt.Rows(0).Item("ModeofDelivery") & "") = "D" Then
                    lit_ModeofDelivery.Text = "DEMAT"

                Else
                    lit_ModeofDelivery.Text = "SGL"
                End If
                If Trim(dt.Rows(0).Item("ModeOFPayment") & "") = "H" Then
                    lit_PaymentMode.Text = "High Value Cheque"
                ElseIf Trim(dt.Rows(0).Item("ModeOFPayment") & "") = "T" Then
                    lit_PaymentMode.Text = "Transfer Cheque"
                ElseIf Trim(dt.Rows(0).Item("ModeOFPayment") & "") = "C" Then
                    lit_PaymentMode.Text = "NORMAL CHEQUE"
                ElseIf Trim(dt.Rows(0).Item("ModeOFPayment") & "") = "R" Then
                    lit_PaymentMode.Text = "RTGS"
                ElseIf Trim(dt.Rows(0).Item("ModeOFPayment") & "") = "E" Then
                    lit_PaymentMode.Text = "NEFT"
                ElseIf Trim(dt.Rows(0).Item("ModeOFPayment") & "") = "S" Then
                    lit_PaymentMode.Text = "SGL"
                ElseIf Trim(dt.Rows(0).Item("ModeOFPayment") & "") = "N" Then
                    lit_PaymentMode.Text = "RTGS-NSCCL-Settlement"
                ElseIf Trim(dt.Rows(0).Item("ModeOFPayment") & "") = "B" Then
                    lit_PaymentMode.Text = "RTGS-BSE-ICCL-Settlement"
                ElseIf Trim(dt.Rows(0).Item("ModeOFPayment") & "") = "L" Then
                    lit_PaymentMode.Text = "RTGS-BSE-Settelement"

                End If
                lit_Contact.Text = Trim(dt.Rows(0).Item("ContactPerson") & "")
                Hid_TransType.Value = Trim(dt.Rows(0).Item("TransType") & "")
                Hid_CouponRate.Value = Val(dt.Rows(0).Item("CouponRate") & "")

                If Val(dt.Rows(0).Item("ExchangeId") & "") = 0 Then
                    btn_GenerateContractNote.Visible = False
                Else
                    btn_GenerateContractNote.Visible = True
                    btn_GenerateContractNote.Enabled = True
                End If
                Hid_RateAmt.Value = Val(dt.Rows(0).Item("StaggMaturity") & "")
                If Val(dt.Rows(0).Item("StaggMaturity") & "") > 1 Then
                    row_PrintRateAmt.Visible = True
                Else
                    row_PrintRateAmt.Visible = False
                End If

            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub


    Protected Sub srh_TransCode_ButtonClick() Handles srh_TransCode.ButtonClick
        Try
            FillDealSlipFields()
            row_BrokRept.Visible = True
            If lit_ModeofDelivery.Text = "SGL" Then
                btn_SGLFedFormat.Visible = True
                btn_SGLHDFCFormat.Visible = True
                btn_SGLFedFormatWord.Visible = True
                btn_SGLFedFormatPDF.Visible = True
                btn_SGLHDFCFormatWord.Visible = False
                btn_NDSOM.Visible = True
                btn_ExpNDSOM.Visible = True
                btn_CSGL.Visible = True
                row_DPDetails.Visible = False
            Else
                btn_SGLLetter.Visible = False
                btn_CSGL.Visible = False
                btn_SGLFedFormat.Visible = False
                btn_SGLHDFCFormat.Visible = False
                btn_SGLFedFormatWord.Visible = False
                btn_SGLFedFormatPDF.Visible = False
                btn_SGLHDFCFormatWord.Visible = False
                btn_NDSOM.Visible = False
                btn_ExpNDSOM.Visible = False
                row_DPDetails.Visible = True

            End If
            rdo_PrntLetterHead.Visible = True
            btn_TCSAnnex.Enabled = True
            btn_DealConfirmation.Enabled = True
            btn_PrintDealSlip.Enabled = True
            btn_FinAuth.Enabled = True
            btn_PhyAuthority.Enabled = True
            btn_DealConfirmationWord.Enabled = True
            btn_DealConfirmationPDF.Enabled = True
            btn_DealSlipPDF.Enabled = True
            btn_NDSOM.Enabled = True
            btn_ExpNDSOM.Enabled = True
            If Hid_DealTransType.Value = "B" Then
                If Val(Hid_RateAmt.Value) > 1 Then
                    row_PrintRateAmt.Visible = True
                Else
                    row_PrintRateAmt.Visible = False
                End If

                btn_NDSOM.Visible = False
                btn_ExpNDSOM.Visible = False
            Else

            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            sqlConn.Dispose()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub

    Protected Sub rdo_DateType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdo_DateType.SelectedIndexChanged
        Try
            ClearField()
            ReadOnlyFields()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub ClearField()
        Try
            lit_Issuer.Text = ""
            lit_SecurityName.Text = ""
            lit_FaceValue.Text = ""
            lit_DealDate.Text = ""
            lit_SettlementDate.Text = ""
            lit_Rate.Text = ""
            srh_TransCode.SearchTextBox.Text = ""
            srh_TransCode.SelectedId = ""
            Hid_DealSlipId.Value = ""
            lit_ModeofDelivery.Text = ""
            lit_ModeofDelivery.Text = ""
            lit_ModeofDelivery.Text = ""
            lit_ModeofDelivery.Text = ""
            lit_ModeofDelivery.Text = ""
            lit_PaymentMode.Text = ""
            lit_Contact.Text = ""
            Hid_TransType.Value = ""
            lit_CustName.Text = ""
            lit_Contact.Text = ""
            srh_TransCode.SearchTextBox.Text = ""
            Srh_MergeTrnsCode.SearchTextBox.Text = ""


        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Srh_MergeTrnsCode_ButtonClick() Handles Srh_MergeTrnsCode.ButtonClick
        btn_MergePrintDealSlip.Enabled = True
        btn_MergeDealConfirmation.Enabled = True

        btn_MergeSGLLetter.Enabled = True
        btn_MergeSGLLetter.Enabled = True
        btn_MergeGenerateContractNote.Enabled = True
        btn_SGLFedFormatM.Enabled = True
        btn_SGLHDFCFormatM.Enabled = True
        btn_PhyAuthorityM.Enabled = True
        btn_FinAuthM.Enabled = True
        btn_MergeDealConfirmationW.Enabled = True
        Hid_DealSlipNo.Value = Srh_MergeTrnsCode.SearchTextBox.Text
        GetDealslipid()
        FillMergeDealSlipFields()



    End Sub
    Private Sub FillMergeDealSlipFields()
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "Id_FILL_PrintDealSlip", ViewState("id"), "DealSlipID")
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
                srh_TransCode.SearchTextBox.Text = Trim(dt.Rows(0).Item("DealSlipNo") & "")
                srh_TransCode.SelectedId = Val(dt.Rows(0).Item("DealSlipId") & "")
                Hid_DealSlipId.Value = Val(dt.Rows(0).Item("DealSlipId") & "")
                Hid_DealSlipId.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Hid_DealSlipId.Value))
                lit_ModeofDelivery.Text = Trim(dt.Rows(0).Item("ModeofDelivery") & "")
                Hid_PhysicalDMAT.Value = Trim(dt.Rows(0).Item("ModeofDelivery") & "")
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
                Hid_Frequency.Value = GetFrequency(Trim(dt.Rows(0).Item("FrequencyOfInterest") & ""))
                If lit_ModeofDelivery.Text = "SGL" Then
                    btn_SGLFedFormatM.Visible = True
                    btn_SGLHDFCFormatM.Visible = True
                    btn_CSGL.Visible = True
                Else
                    btn_SGLFedFormatM.Visible = False
                    btn_SGLHDFCFormatM.Visible = False
                End If
                Hid_RateAmt.Value = Val(dt.Rows(0).Item("StaggMaturity") & "")
                If Val(dt.Rows(0).Item("StaggMaturity") & "") > 1 Then
                    row_PrintRateAmt.Visible = True
                Else
                    row_PrintRateAmt.Visible = False
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Sub GetDealslipid()
        'CHANGE 
        Try
            OpenConn()
            Dim dt As DataTable
            dt = objCommon.FillDataTable(sqlConn, "Id_Get_MergeDealentryfield", , , , Hid_DealSlipNo.Value, "MergeDealNo")
            If dt.Rows.Count > 0 Then
                ViewState("id") = Val(dt.Rows(0).Item("dealslipid") & "")
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try


    End Sub

    Private Sub ReadOnlyFields()
        Try
            If rdo_DateType.SelectedValue = "D" Then
                btn_FinAuth.Enabled = False
                btn_PhyAuthority.Enabled = False
                btn_DealConfirmation.Enabled = False
                btn_PrintDealSlip.Enabled = False
                btn_SGLLetter.Visible = False
                btn_CSGL.Visible = False
                btn_SGLFedFormat.Visible = False
                btn_SGLHDFCFormat.Visible = False
                btn_GenerateContractNote.Enabled = False
                btn_DealConfirmationWord.Enabled = False
                btn_DealConfirmationPDF.Enabled = False
                btn_DealSlipPDF.Enabled = False
                btn_SGLFedFormatWord.Visible = False
                btn_SGLFedFormatPDF.Visible = False
                btn_SGLHDFCFormatWord.Visible = False
                btn_NDSOM.Enabled = False
                btn_ExpNDSOM.Enabled = False
                btn_TCSAnnex.Enabled = False
            Else
                btn_TCSAnnex.Enabled = False
                btn_MergePrintDealSlip.Enabled = False
                btn_MergeDealConfirmation.Enabled = False
                btn_MergeDealConfirmationW.Enabled = False
                btn_MergeSGLLetter.Enabled = False
                btn_MergeSGLLetter.Enabled = False
                btn_MergeGenerateContractNote.Enabled = False
                btn_SGLFedFormatM.Enabled = False
                btn_SGLHDFCFormatM.Enabled = False
                btn_PhyAuthorityM.Enabled = False
                btn_FinAuthM.Enabled = False
                btn_SGLFedFormat.Visible = False
                btn_SGLHDFCFormat.Visible = False
                btn_SGLFedFormatWord.Visible = False
                btn_SGLFedFormatPDF.Visible = False
                btn_SGLHDFCFormatWord.Visible = False
                row_BrokRept.Visible = False
                row_PrintRateAmt.Visible = False
                btn_ExpNDSOM.Visible = False
                row_DPDetails.Visible = False
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
End Class
