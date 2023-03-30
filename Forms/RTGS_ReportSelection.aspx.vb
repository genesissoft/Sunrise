Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class Forms_RTGS_ReportSelection
    Inherits System.Web.UI.Page
    Dim sqlConn As SqlConnection
    Dim objCommon As New clsCommonFuns
    Dim rptDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Dim rptDoc1 As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Dim strFilePath As String
    Dim strFilePath1 As String
    Dim dtRpt As DataTable

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
                SetControls()
            End If
        Catch ex As Exception
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

    Private Sub SetControls()
        Try
            btn_Print.Attributes.Add("onclick", "return  Validation();")
            'btn_Mail.Attributes.Add("onclick", "return  Wait();")
            txt_SettlementAmt.Attributes.Add("onkeypress", "return OnlyDecimal();")
            'btn_Mail.Enabled = False
            btn_Print.Enabled = False
            'btn_Export.Enabled = False
            txt_SettlementAmt.Enabled = False
            'srh_TransCode.Columns.Add("DealSlipNo")
            'srh_TransCode.Columns.Add("CustomerName")
            'srh_TransCode.Columns.Add("SecurityName")
            ''srh_TransCode.Columns.Add("CASE TransType WHEN 'H' THEN 'PUR' ELSE 'SELL' END AS Type")
            'srh_TransCode.Columns.Add("DealSlipId")
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
                Hid_DealSlipNo.Value = Trim(dt.Rows(0).Item("DealSlipNo") & "")
                srh_TransCode.SelectedId = Val(dt.Rows(0).Item("DealSlipId") & "")
                Hid_DealSlipId.Value = Val(dt.Rows(0).Item("DealSlipId") & "")
                Hid_DealTransType.Value = Trim(dt.Rows(0).Item("DealTransType") & "")
                Hid_PhysicalDMAT.Value = Trim(dt.Rows(0).Item("ModeofDelivery") & "")
                txt_SettlementAmt.Text = Val(dt.Rows(0).Item("SettlementAmt") & "")

                txt_PayMode.Text = Trim(dt.Rows(0).Item("payMode") & "")
                Hid_PayMode.Value = Trim(dt.Rows(0).Item("ModeOFPayment") & "")
                Hid_AmtInWords.Value = Trim(dt.Rows(0).Item("SettAmtInWords") & "")
                Hid_AccNo.Value = Trim(dt.Rows(0).Item("AccountNumber") & "")
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
            'btn_Mail.Enabled = True
            btn_Print.Enabled = True
            'btn_Export.Enabled = True
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Try
        
            If rbl_DealSlipType.SelectedValue = "F" Then
                Response.Redirect("ViewReports.aspx?DealSlipId=" & srh_TransCode.SelectedId & "&Rpt=" & "RTGSRepts" & "&RptTyp=" & rbl_DealSlipType.SelectedValue & "&SettAmt=" & (txt_SettlementAmt.Text) & "&AccountNo=" & cbo_FedAccountNo.SelectedItem.Text & "&KindAttn=" & txt_KindAtten.Text & "&PayMode=" & Hid_PayMode.Value & "&Narration=" & txt_Narration.Text, False)
            ElseIf rbl_DealSlipType.SelectedValue = "I" Then
                Response.Redirect("ViewReports.aspx?DealSlipId=" & srh_TransCode.SelectedId & "&Rpt=" & "RTGSRepts" & "&RptTyp=" & rbl_DealSlipType.SelectedValue & "&SettAmt=" & (txt_SettlementAmt.Text) & "&CtcDetails=" & txt_CtcDetails.Text & "&PayMode=" & Hid_PayMode.Value & "&Narration=" & txt_Narration.Text, False)
            Else
                Response.Redirect("ViewReports.aspx?DealSlipId=" & srh_TransCode.SelectedId & "&Rpt=" & "RTGSRepts" & "&RptTyp=" & rbl_DealSlipType.SelectedValue & "&SettAmt=" & (txt_SettlementAmt.Text) & "&PayMode=" & Hid_PayMode.Value & "&Narration=" & txt_Narration.Text, False)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function BindReportS(Optional ByVal blnSepPages As Boolean = False, _
                                Optional ByVal strRptName As String = "", _
                                Optional ByVal strProcName As String = "") As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Try
            Dim strPath As String
            Dim strPath1 As String
            Dim strReportRange As String = ""
            If strRptName = "RTGS_FED.rpt" Or strRptName = "RTGS_HDFC.rpt" Or strRptName = "RTGS_ICICI.rpt" Then
                strPath = ConfigurationSettings.AppSettings("ReportsPath") & "\" & strRptName
                rptDoc.Load(strPath)
            Else
                strPath1 = ConfigurationSettings.AppSettings("ReportsPath") & "\" & strRptName
                rptDoc1.Load(strPath1)
            End If



            dtRpt = GetReportTable("ID_FILL_RTGS_Rpt")

            If (dtRpt.Rows.Count = 0) Then
                Dim msg As String = "No Entries Done"
                Dim strHtml As String
                strHtml = "alert('" + msg + "');"
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
                Exit Function
            Else
                If strRptName = "RTGS_FED.rpt" Or strRptName = "RTGS_HDFC.rpt" Or strRptName = "RTGS_ICICI.rpt" Then
                    rptDoc.SetDataSource(dtRpt)
                    rptDoc.SummaryInfo.ReportComments = strReportRange
                Else
                    rptDoc1.SetDataSource(dtRpt)
                    rptDoc1.SummaryInfo.ReportComments = strReportRange
                End If

                'strReportRange = txt_FromDate.Text & "  TO  " & txt_ToDate.Text & ""

                ' rptDoc.DataDefinition.FormulaFields("Period").Text = _
                '"""" & "From " & datFrom.ToString("dd-MMMM-yyyy") & " To " & datTo.ToString("dd-MMMM-yyyy") & """"
            End If
            If strRptName = "RTGS_FED.rpt" Or strRptName = "RTGS_HDFC.rpt" Or strRptName = "RTGS_ICICI.rpt" Then
                Return rptDoc
            Else
                Return rptDoc1
            End If

        Catch ex As Exception
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Function GetReportTable(ByVal strProcName As String) As DataTable
        Try
            OpenConn()
            
            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand
            sqlComm.CommandTimeout = 0
           
            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = strProcName
                If Session("strWhereClause") <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@Cond", SqlDbType.VarChar, 1000, "I", , , Session("strWhereClause"))
                End If
                objCommon.SetCommandParameters(sqlComm, "@DealslipId", SqlDbType.Int, 4, "I", , , Val(srh_TransCode.SelectedId))
                objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                .ExecuteNonQuery()
            End With
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dtfill)
     
            Return dtfill
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Function
    Private Sub ExportReport(ByVal crReport As CrystalDecisions.CrystalReports.Engine.ReportDocument, ByVal type As CrystalDecisions.Shared.ExportFormatType, _
                           Optional ByVal blnDiskExport As Boolean = False)
        Response.Clear()
        Response.ClearHeaders()
        Response.Buffer = True
        If blnDiskExport = True Then

            'If cbo_WDMTransRepts.SelectedValue = "E" Then
            strFilePath = Server.MapPath("").Replace("Forms", "Temp") & "\RTGS.pdf"

            If File.Exists(strFilePath) = True Then
                File.Delete(strFilePath)
            End If
          
            crReport.ExportToDisk(type, strFilePath)

        Else
            crReport.ExportToHttpResponse(type, Response, True, "Report")
        End If


        'With HttpContext.Current.Response
        '    .Clear()
        '    .Charset = ""
        '    .ClearHeaders()
        '    .ContentType = "application/vnd.ms-excel"
        '    .AddHeader("content-disposition", "attachment;filename=Report.xls")
        '    .WriteFile(strFilePath)
        '    .Flush()
        '    .End()
        'End With
        clsCommonFuns.sqlConn.Close()
    End Sub
    Private Sub ExportReport1(ByVal crReport As CrystalDecisions.CrystalReports.Engine.ReportDocument, ByVal type As CrystalDecisions.Shared.ExportFormatType, _
                           Optional ByVal blnDiskExport As Boolean = False)
        Response.Clear()
        Response.ClearHeaders()
        Response.Buffer = True
        If blnDiskExport = True Then

            strFilePath1 = Server.MapPath("").Replace("Forms", "Temp") & "\RTGS.xls"

            If File.Exists(strFilePath1) = True Then
                File.Delete(strFilePath1)
            End If
            crReport.ExportToDisk(type, strFilePath1)
        Else
            crReport.ExportToHttpResponse(type, Response, True, "Report")
        End If


        'With HttpContext.Current.Response
        '    .Clear()
        '    .Charset = ""
        '    .ClearHeaders()
        '    .ContentType = "application/vnd.ms-excel"
        '    .AddHeader("content-disposition", "attachment;filename=RTGS.xls")
        '    .WriteFile(strFilePath1)
        '    .Flush()
        '    .End()
        'End With
        clsCommonFuns.sqlConn.Close()
    End Sub
    Private Sub SendMail(ByVal strFilePath As String, ByVal strFilePath1 As String, ByVal toAddress As String)
        Try

            'If bmail = True Then
            'Dim strExcelFilePath As String = ConfigurationManager.AppSettings("ExcelPath").ToString()
            'WorkBookEngine.CreateWorkbook(ds, strExcelFilePath);

            'WorkBookEngine.CreateExcelFile(ds.Tables[0], strExcelFilePath + "Report.xls");
            'Dim ret As [Boolean] = Export.CreateExcelDocumentHarDisc(ds)

            'Dim fromAddress As String = "debt@smcindiaonline.com"
            'Dim bcc As String = "debt@smcindiaonline.com"
            'Dim fromAddress As String = "tssuvarna@gmail.com"
            Dim fromAddress As String = "suvarna.sreekant@genesissoftware.co.in"

            ''Gmail Address from where you send the mail
            'Dim toAddress = "poonam.sinha@genesissoftware.co.in"

            'toAddress = "suvarna.sreekant@genesissoftware.co.in"
            ' Dim toAddress As String = "genesismail123@gmail.com"

            ''Password of your gmail address
            Dim subject As String = "RTGS details for Deal " & srh_TransCode.SearchTextBox.Text

            Dim body As String = "Dear Sir/Madam"
            body += Environment.NewLine
            body += " <br/><br/>"
            'body += " <br/>I am sending you the Offer file attachment for your reference. <br/>"
            body += " <br/>Fund to be transferred from " & Trim(Session("CompName") & "") & ", Bank Account " & Hid_AccNo.Value & " to the below mentioned:<br/>"
            'body += " <br/><br/>"
            body += " <br/>Amount Rs. " & txt_SettlementAmt.Text & " " & Hid_AmtInWords.Value & "<br/>"
            'body += " <br/><br/>"
            'body += " <br/><br/>"

            body += "<br/>Beneficiary Name: NSCCL<br/>"
            body += "Beneficiary Bank name:  <br/>"
            body += "Beneficiary Bank IFSC Code: <br/>"
            body += "Beneficiary Bank Account no.:  <br/>"
            body += " <br/>Regards, <br/>" & Convert.ToString(Session("NameOfUser"))

            'strExcelFilePath = strExcelFilePath & "Report.xls"
            MailHelper.SendMailMessage(fromAddress, toAddress, "", "", subject, body, strFilePath, strFilePath1)

            If File.Exists(strFilePath & "RTGS.pdf") Then
                File.Delete(strFilePath & "RTGS.pdf")
            End If

            If File.Exists(strFilePath1 & "RTGS.xls") Then
                File.Delete(strFilePath1 & "RTGS.xls")
            End If

            'Response.Write("true")
            'Response.End()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Protected Sub rbl_DealSlipType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbl_DealSlipType.SelectedIndexChanged
        Try
            If rbl_DealSlipType.SelectedValue = "F" Then
                OpenConn()
                row_Acc.Visible = True
                objCommon.FillControl(cbo_FedAccountNo, sqlConn, "ID_FILL_FEDERALBankAccNo", "AccountNumber", "", Val(Session("CompId")), "CompId")
            Else
                row_Acc.Visible = False
            End If
            If rbl_DealSlipType.SelectedValue = "I" Then
                row_Ctc.Visible = True
            Else
                row_Ctc.Visible = False
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
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

    'Protected Sub btn_Mail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Mail.Click
    '    Try
    '        Dim decSettlementAmt As Decimal
    '        decSettlementAmt = txt_SettlementAmt.Text
    '        'If rbl_DealSlipType.SelectedValue = "H" Then
    '        '    rptDoc = BindReportS(, "RTGS_HDFC.rpt", "ID_FILL_RTGS_Rpt")
    '        'ElseIf rbl_DealSlipType.SelectedValue = "I" Then
    '        '    rptDoc = BindReportS(, "RTGS_ICICI.rpt", "ID_FILL_RTGS_Rpt")
    '        'Else
    '        '    rptDoc = BindReportS(, "RTGS_FED.rpt", "ID_FILL_RTGS_Rpt")
    '        'End If
    '        rptDoc1 = BindReportS(, "RTGS_BankDetails.rpt", "ID_FILL_RTGS_Rpt")
    '        'If rptDoc Is Nothing Then Exit Sub
    '        'ExportReport(rptDoc, CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, True)
    '        If rptDoc1 Is Nothing Then Exit Sub
    '        ExportReport1(rptDoc1, ExportFormatType.ExcelRecord, True)
    '        SendMail(strFilePath1, "", "suvarna.sreekant@genesissoftware.co.in")
    '        lbl_MailSent.Text = ""
    '        lbl_MailSent.Text = "Mail Sent successfully"
    '        'btn_Mail.Visible = True
    '    Catch ex As Exception
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '    End Try
    'End Sub

    'Protected Sub btn_Export_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Export.Click
    '    Try
    '        rptDoc1 = BindReportS(, "RTGS_BankDetails.rpt", "ID_FILL_RTGS_Rpt")
    '        If rptDoc1 Is Nothing Then Exit Sub
    '        ExportReport1(rptDoc1, ExportFormatType.ExcelRecord, True)
    '        With HttpContext.Current.Response
    '            .Clear()
    '            .Charset = ""
    '            .ClearHeaders()
    '            .ContentType = "application/vnd.ms-excel"
    '            .AddHeader("content-disposition", "attachment;filename=RTGS.xls")
    '            .WriteFile(strFilePath1)
    '            .Flush()
    '            .End()
    '        End With
    '    Catch ex As Exception

    '    End Try
    'End Sub
End Class
