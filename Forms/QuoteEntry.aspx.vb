Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_QuoteEntry
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim arrIssueDetailIds() As String
    Dim dblTotalFaceValue As Decimal
    Dim intUserId As Integer
    Dim sqlConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If

            ''*******************Mehul***************25/Nov/2019*****
            yld_Calc.FindControl("btn_CalInterest").Visible = False
            yld_Calc.RdoIPCalc.Visible = False
            yld_Calc.FindControl("rbl_Days").Visible = False
            yld_Calc.FindControl("rbl_DaysOptions").Visible = False
            HDAcc_Id.Value = yld_Calc.SecurityId
            HDAcc_Date.Value = CType(yld_Calc.FindControl("txt_SettDate"), TextBox).Text
            HDAcc_Rate.Value = yld_Calc.TextBoxRate.Text
            HDAcc_FaceValue.Value = CType(yld_Calc.FindControl("txt_FaceValue"), TextBox).Text
            HDAcc_Multiple.Value = CType(yld_Calc.FindControl("cbo_FaceValue"), DropDownList).SelectedValue
            HDAcc_StepUp.Value = CType(yld_Calc.FindControl("Hid_StepUp"), HiddenField).Value
            HDAcc_RateActual.Value = CType(yld_Calc.FindControl("rdo_RateActual"), RadioButtonList).SelectedValue
            ''*******************Mehul***************25/Nov/2019*****


            Session("StepUpDown") = rdo_TaxFree.SelectedValue
            Session("URL") = "QuoteEntry.aspx"
            'objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")
            rdo_CustomerType.Items(0).Attributes.Add("onclick", "ShowCustomer('C')")
            rdo_CustomerType.Items(1).Attributes.Add("onclick", "ShowCustomer('T')")
            Page.Form.DefaultButton = btn_Save.UniqueID
            If IsPostBack = False Then
                btn_ShowSecurity.Visible = False
                SetAttributes()
                SetTimeCombos()
                SetControls()
                If Request.QueryString("Id") <> "" Then
                    Hid_Id.Value = Request.QueryString("Id")
                    Dim strId As String = objCommon.DecryptText(HttpUtility.UrlDecode(Request.QueryString("Id")))
                    ViewState("Id") = Val(strId)
                    FillFields()
                    btn_Save.Visible = False
                    If intUserId = Val(Session("UserId")) Then
                        btn_Update.Visible = True
                        'btn_GenerateDealSlip.Visible = True
                    Else
                        btn_Update.Visible = False
                        'btn_GenerateDealSlip.Visible = False
                    End If
                Else
                    btn_Save.Visible = True
                    btn_Update.Visible = False
                    btn_GenerateDealSlip.Visible = False
                End If
            Else
                yld_Calc.SecurityId = Val(srh_NameofSecurity.SelectedId)
                If Val(srh_NameofSecurity.SelectedId) <> 0 Then
                    FillSecurityDetails()
                End If

            End If
            'rdo_CustomerType.Items(1).Attributes.Add ("style", "display:none")
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "Cust", "ShowCustomer('" & rdo_CustomerType.SelectedValue & "');", True)
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
        If sqlConn.State = ConnectionState.Open Then
            sqlConn.Close()
            sqlConn = Nothing
        End If
    End Sub

    Private Sub SetAttributes()

        txt_Date.Attributes.Add("onkeypress", "OnlyDate();")
        txt_ValidTill.Attributes.Add("onkeypress", "OnlyDate();")
        btn_ShowSecurity.Attributes.Add("onclick", "return ShowSecurityMaster();")
        btn_Refer.Attributes.Add("onclick", "return Refer();")
        txt_Date.Text = Format(Now, "dd/MM/yyyy hh:mm tt")
        txt_ValidTill.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
        btn_Save.Attributes.Add("onclick", "return Validation();")
        btn_Update.Attributes.Add("onclick", "return Validation();")
        btn_AddTempCustomer.Attributes.Add("onclick", "return ShowTempCust();")
        'cbo_SecurityType.Attributes.Add("onchange", "return CheckSecurity();")
        txt_DealerName.Text = Session("NameOfUser")
    End Sub

    Private Sub SetControls()
        Try

            'srh_IssuerOfSecurity.Columns.Add("SecurityIssuer")
            'srh_IssuerOfSecurity.Columns.Add("SecurityIssuer")

            'srh_NameofSecurity.Columns.Add("SecurityName")
            'srh_NameofSecurity.Columns.Add("SecurityTypeName")
            'srh_NameofSecurity.Columns.Add("SecurityIssuer")
            'srh_NameofSecurity.Columns.Add("CouponRate")
            'srh_NameofSecurity.Columns.Add("NSDLAcNumber As ISIN")

            'srh_NameofSecurity.Columns.Add("SecurityId")

            'srh_NameOFClient.Columns.Add("CustomerName")
            'srh_NameOFClient.Columns.Add("CustomerCity")
            'srh_NameOFClient.Columns.Add("CustomerPhone")
            'srh_NameOFClient.Columns.Add("CustomerId")

            OpenConn()
            objCommon.FillControl(cbo_SecurityType, sqlConn, "ID_FILL_SecurityTypeMaster1", "SecurityTypeName", "SecurityTypeId")



        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub


    Private Sub FillFields()
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_QuoteList", ViewState("Id"), "QuoteId")
            If dt.Rows.Count > 0 Then
                cbo_SecurityType.SelectedValue = Val(dt.Rows(0).Item("SecurityTypeId") & "")
                'txt_FaceValue.Text = Trim(dt.Rows(0).Item("FaceValue") & "")
                txt_DealerName.Text = Trim(dt.Rows(0).Item("NameOfUser") & "")
                txt_Remark.Text = Trim(dt.Rows(0).Item("Remark") & "")
                txt_Date.Text = Format(CDate(dt.Rows(0).Item("QuoteDate").ToString()), "dd/MM/yyyy hh:mm tt")
                'txt_ValidTill.Text = Trim(dt.Rows(0).Item("ValidDate") & "")
                txt_ValidTill.Text = Format(CDate(dt.Rows(0).Item("ValidDate").ToString()), "dd/MM/yyyy")
                cbo_hr.SelectedValue = Format(dt.Rows(0).Item("ValidDate"), "hh")
                cbo_minute.SelectedValue = Format(dt.Rows(0).Item("ValidDate"), "mm")
                cbo_ampm.SelectedValue = Right(dt.Rows(0).Item("ValidDate").ToString, 2)
                Rdo_TransactionType.SelectedValue = Trim(dt.Rows(0).Item("Branch_HeadOff_Flag") & "")
                Rdo_Routing.SelectedValue = Trim(dt.Rows(0).Item("RoutingNonRouting") & "")
                Rdo_QuoteFor.SelectedValue = Trim(dt.Rows(0).Item("QuoteType") & "")
                srh_NameofSecurity.SelectedId = Val(dt.Rows(0).Item("SecurityId") & "")
                srh_NameOFClient.SelectedId = Val(dt.Rows(0).Item("CustomerId") & "")
                srh_NameOFClient.SearchTextBox.Text = dt.Rows(0).Item("CustomerName").ToString
                Hid_CustomeId.Value = Val(dt.Rows(0).Item("TempCustomerId") & "")


                rdo_CustomerType.SelectedValue = Trim(dt.Rows(0).Item("CustomerTypeFlag") & "")
                If rdo_CustomerType.SelectedValue = "T" Then
                    txt_TempCustomer.Text = Trim(dt.Rows(0).Item("TempCustomerName") & "")
                    Hid_CustomeId.Value = Val(dt.Rows(0).Item("TempCustomerId") & "")
                    txt_TempContact.Text = Trim(dt.Rows(0).Item("TempCtcPerson") & "")
                ElseIf rdo_CustomerType.SelectedValue = "C" Then
                    srh_NameOFClient.SelectedId = Val(dt.Rows(0).Item("CustomerId") & "")
                    srh_NameOFClient.SearchTextBox.Text = Trim(dt.Rows(0).Item("CustomerName") & "")
                End If

                'cbo_ContactPerson.SelectedValue = Val(dt.Rows(0).Item("ContactId") & "")
                yld_Calc.RdoIPCalc.SelectedValue = (dt.Rows(0).Item("Equal_Actual_Flag") & "")
                yld_Calc.RdoRateActual.SelectedValue = (dt.Rows(0).Item("Rate_Actual_Flag") & "")
                yld_Calc.RdoPhysicalDMAT.SelectedValue = (dt.Rows(0).Item("Physical_DMAT_Flag") & "")
                yld_Calc.RdoSemiAnn.SelectedValue = (dt.Rows(0).Item("Semi_Ann_Flag") & "")
                yld_Calc.TextBoxYTMDate.Text = (dt.Rows(0).Item("YTMDate") & "")
                yld_Calc.TextBoxRate.Text = (dt.Rows(0).Item("Rate") & "")
                yld_Calc.TextBoxFaceValue.Text = (dt.Rows(0).Item("FaceValue") & "")
                yld_Calc.cboFaceValue.SelectedValue = (dt.Rows(0).Item("Multiple") & "")
                yld_Calc.TextBoxYTMSemi.Text = (dt.Rows(0).Item("YTMSemi") & "")
                yld_Calc.TextBoxYTMAnn.Text = (dt.Rows(0).Item("YTMAnn") & "")
                yld_Calc.TextBoxYTCSemi.Text = (dt.Rows(0).Item("YTCSemi") & "")
                yld_Calc.TextBoxYTCAnn.Text = (dt.Rows(0).Item("YTCAnn") & "")
                yld_Calc.TextBoxYTPSemi.Text = (dt.Rows(0).Item("YTPSemi") & "")
                yld_Calc.TextBoxYTPAnn.Text = (dt.Rows(0).Item("YTPAnn") & "")
                intUserId = Val(dt.Rows(0).Item("UserId") & "")
                If Val(dt.Rows(0).Item("CombineIPMat")) = 0 Then
                    yld_Calc.chkCombineIPMat.Checked = False
                Else
                    yld_Calc.chkCombineIPMat.Checked = True
                End If
                If (dt.Rows(0).Item("Ytm_Mat_Call_Put_Flag") & "") = "Y" Then
                    yld_Calc.RdoYield.Checked = True
                ElseIf (dt.Rows(0).Item("Ytm_Mat_Call_Put_Flag") & "") = "M" Then
                    yld_Calc.RdoMatToRate.Checked = True
                ElseIf (dt.Rows(0).Item("Ytm_Mat_Call_Put_Flag") & "") = "C" Then
                    yld_Calc.RdoMatCallToRate.Checked = True
                ElseIf (dt.Rows(0).Item("Ytm_Mat_Call_Put_Flag") & "") = "P" Then
                    yld_Calc.RdoPutToRate.Checked = True
                End If
                yld_Calc.RdoYXM.SelectedValue = (dt.Rows(0).Item("YTM_XIRR_MMY_Flag") & "")
                yld_Calc.SecurityId = srh_NameofSecurity.SelectedId
                FillContactPerson()
                FillSecurityDetails()
                FillMaturityPutCallCouponDetails()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Sub FillSecurityDetails()
        Try
            Dim dt As DataTable
            OpenConn()

            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_SecurityMaster1", srh_NameofSecurity.SelectedId, "SecurityId")
            If dt.Rows.Count > 0 Then
                cbo_SecurityType.SelectedValue = Trim(dt.Rows(0).Item("SecurityTypeId").ToString)
                srh_IssuerOfSecurity.SearchTextBox.Text = dt.Rows(0).Item("SecurityIssuer").ToString
                'srh_IssuerOfSecurity.SelectedId = dt.Rows(0).Item("SecurityIssuer").ToString
                srh_NameofSecurity.SearchTextBox.Text = dt.Rows(0).Item("SecurityName").ToString
                srh_IssuerOfSecurity.SelectedFieldText = dt.Rows(0).Item("SecurityIssuer").ToString
                btn_ShowSecurity.Visible = True
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub



    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        'If objCommon.CheckDuplicate(clsCommonFuns.sqlConn, "SecurityMaster", "SecurityName", Trim(txt_NameOfSecurity.Text)) = False Then
        '    Dim msg As String = "This Security Name Already Exist"
        '    Dim strHtml As String
        '    strHtml = "alert('" + msg + "');"
        '    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
        '    Exit Sub
        'End If
        SetSaveUpdate("ID_INSERT_QuoteList")
    End Sub

    Protected Sub btn_Update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Update.Click
        'If objCommon.CheckDuplicate(clsCommonFuns.sqlConn, "SecurityMaster", "SecurityName", Trim(txt_NameOfSecurity.Text), "SecurityId", Val(ViewState("Id"))) = False Then
        '    Dim msg As String = "This Security Name Already Exist"
        '    Dim strHtml As String
        '    strHtml = "alert('" + msg + "');"
        '    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "msg", strHtml, True)
        '    Exit Sub
        'End If
        SetSaveUpdate("ID_UPDATE_QuoteList")
    End Sub
    Private Sub SetSaveUpdate(ByVal strProc As String)
        Try
            Dim sqlTrans As SqlTransaction
            OpenConn()
            sqlTrans = sqlConn.BeginTransaction
            If SaveUpdate(sqlTrans, strProc) = False Then Exit Sub
            'If DeleteDetails(sqlTrans) = False Then Exit Sub
            'If SaveSecurityInfo(sqlTrans) = False Then Exit Sub
            'If DeleteCustomerApproval(sqlTrans) = False Then Exit Sub
            'If SaveCustomerApproval(sqlTrans) = False Then Exit Sub

            sqlTrans.Commit()
            Response.Redirect("QuoteDetails.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub
    Private Function SaveUpdate(ByVal sqlTrans As SqlTransaction, ByVal strProc As String) As Boolean
        Try
            'CHANGE 
            Dim sqlComm As New SqlCommand
            Dim datValid As Date
            sqlComm.CommandText = strProc
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure

            sqlComm.Connection = sqlConn



            sqlComm.Parameters.Clear()

            datValid = objCommon.DateFormat(txt_ValidTill.Text) & " " & cbo_hr.SelectedValue & ":" & cbo_minute.SelectedValue & " " & cbo_ampm.SelectedValue
            dblTotalFaceValue = Val(yld_Calc.TextBoxFaceValue.Text) * Val(yld_Calc.cboFaceValue.SelectedValue)
            Hid_TotalValue.Value = dblTotalFaceValue / Val(Hid_FaceValue.Value)
            If Val(ViewState("Id") & "") = 0 Then
                objCommon.SetCommandParameters(sqlComm, "@QuoteId", SqlDbType.SmallInt, 2, "O")
            Else
                objCommon.SetCommandParameters(sqlComm, "@QuoteId", SqlDbType.SmallInt, 2, "I", , , ViewState("Id"))
            End If
            objCommon.SetCommandParameters(sqlComm, "@CustomerTypeFlag", SqlDbType.Char, 1, "I", , , (rdo_CustomerType.SelectedValue))
            If rdo_CustomerType.SelectedValue = "C" Then
                If Val(srh_NameOFClient.SelectedId) <> 0 Then
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , Val(srh_NameOFClient.SelectedId))
                    objCommon.SetCommandParameters(sqlComm, "@ContactId", SqlDbType.Int, 4, "I", , , DBNull.Value)
                    objCommon.SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.SmallInt, 4, "I", , , Val(cbo_ContactPerson.SelectedValue))

                Else
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , DBNull.Value)
                End If
            Else

                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , DBNull.Value)
                objCommon.SetCommandParameters(sqlComm, "@ContactId", SqlDbType.Int, 4, "I", , , DBNull.Value)
                objCommon.SetCommandParameters(sqlComm, "@ContactDetailId", SqlDbType.SmallInt, 4, "I", , , DBNull.Value)
                objCommon.SetCommandParameters(sqlComm, "@TempCustomerId", SqlDbType.Int, 4, "I", , , Hid_CustomeId.Value)

            End If
            'objCommon.SetCommandParameters(sqlComm, "@SecurityTypeId", SqlDbType.Int, 4, "I", , , Val(cbo_SecurityType.SelectedValue))
            'objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.Int, 4, "I", , , Val(srh_NameOFClient.SelectedId))
            'objCommon.SetCommandParameters(sqlComm, "@ContactId", SqlDbType.Int, 4, "I", , , Val(cbo_ContactPerson.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.Int, 4, "I", , , Val(srh_NameofSecurity.SelectedId))

            objCommon.SetCommandParameters(sqlComm, "@Branch_HeadOff_Flag", SqlDbType.Char, 1, "I", , , (Rdo_TransactionType.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@RoutingNonRouting", SqlDbType.Char, 1, "I", , , (Rdo_Routing.SelectedValue))

            objCommon.SetCommandParameters(sqlComm, "@QuoteType", SqlDbType.Char, 1, "I", , , (Rdo_QuoteFor.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@FaceValue", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat4(yld_Calc.TextBoxFaceValue.Text))
            objCommon.SetCommandParameters(sqlComm, "@Multiple", SqlDbType.Int, 4, "I", , , Val(yld_Calc.cboFaceValue.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId")))
            objCommon.SetCommandParameters(sqlComm, "@Qty", SqlDbType.Decimal, 9, "I", , , Val(Hid_TotalValue.Value))
            If txt_Date.Text <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@QuoteDate", SqlDbType.SmallDateTime, SqlDbType.SmallDateTime, "I", , , Now)
            End If
            objCommon.SetCommandParameters(sqlComm, "@Remark", SqlDbType.VarChar, 100, "I", , , Trim(txt_Remark.Text))
            If txt_ValidTill.Text <> "" Then
                objCommon.SetCommandParameters(sqlComm, "@ValidDate", SqlDbType.SmallDateTime, 4, "I", , , datValid)
            End If
            If yld_Calc.RdoYield.Checked = True Then
                objCommon.SetCommandParameters(sqlComm, "@Ytm_Mat_Call_Put_Flag", SqlDbType.Char, 1, "I", , , "Y")
            ElseIf yld_Calc.RdoMatToRate.Checked = True Then
                objCommon.SetCommandParameters(sqlComm, "@Ytm_Mat_Call_Put_Flag", SqlDbType.Char, 1, "I", , , "M")
            ElseIf yld_Calc.RdoMatCallToRate.Checked = True Then
                objCommon.SetCommandParameters(sqlComm, "@Ytm_Mat_Call_Put_Flag", SqlDbType.Char, 1, "I", , , "C")
            ElseIf yld_Calc.RdoPutToRate.Checked = True Then
                objCommon.SetCommandParameters(sqlComm, "@Ytm_Mat_Call_Put_Flag", SqlDbType.Char, 1, "I", , , "P")
            End If

            objCommon.SetCommandParameters(sqlComm, "@Equal_Actual_Flag", SqlDbType.Char, 1, "I", , , yld_Calc.RdoIPCalc.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@Rate_Actual_Flag", SqlDbType.Char, 1, "I", , , yld_Calc.RdoRateActual.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@Physical_DMAT_Flag", SqlDbType.Char, 1, "I", , , Trim(yld_Calc.RdoPhysicalDMAT.SelectedValue) & "")
            objCommon.SetCommandParameters(sqlComm, "@YTM_XIRR_MMY_Flag", SqlDbType.Char, 1, "I", , , (yld_Calc.RdoYXM.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@Semi_Ann_Flag", SqlDbType.Char, 1, "I", , , yld_Calc.RdoSemiAnn.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@Rate", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat4(yld_Calc.TextBoxRate.Text))
            objCommon.SetCommandParameters(sqlComm, "@YTMDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(yld_Calc.TextBoxYTMDate.Text))
            If Val(yld_Calc.TextBoxYTMSemi.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@YTMSemi", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat4(yld_Calc.TextBoxYTMSemi.Text))
            End If
            If Val(yld_Calc.TextBoxYTMAnn.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@YTMAnn", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat4(yld_Calc.TextBoxYTMAnn.Text))
            End If
            If Val(yld_Calc.TextBoxYTCSemi.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@YTCSemi", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat4(yld_Calc.TextBoxYTCSemi.Text))
            End If

            If Val(yld_Calc.TextBoxYTCAnn.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@YTCAnn", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat4(yld_Calc.TextBoxYTCAnn.Text))
            End If
            If Val(yld_Calc.TextBoxYTPSemi.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@YTPSemi", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat4(yld_Calc.TextBoxYTPSemi.Text))
            End If
            If Val(yld_Calc.TextBoxYTPAnn.Text) <> 0 Then
                objCommon.SetCommandParameters(sqlComm, "@YTPAnn", SqlDbType.Decimal, 9, "I", , , objCommon.DecimalFormat4(yld_Calc.TextBoxYTPAnn.Text))
            End If
            If yld_Calc.chkCombineIPMat.Checked = True Then
                objCommon.SetCommandParameters(sqlComm, "@CombineIPMat", SqlDbType.Bit, 1, "I", , , 1)
            Else
                objCommon.SetCommandParameters(sqlComm, "@CombineIPMat", SqlDbType.Bit, 1, "I", , , 0)
            End If
            objCommon.SetCommandParameters(sqlComm, "@intflag", SqlDbType.Int, 2, "O")
            objCommon.SetCommandParameters(sqlComm, "@strmessage", SqlDbType.VarChar, 100, "O")
            sqlComm.ExecuteNonQuery()
            ViewState("Id") = sqlComm.Parameters("@QuoteId").Value
            Hid_QuoteId.Value = ViewState("Id")

            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Protected Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Try
            If Val(ViewState("Id")) <> 0 Then
                Response.Redirect("QuoteDetails.aspx?Id=" & HttpUtility.UrlEncode(objCommon.EncryptText(ViewState("Id"))), False)
            Else
                Response.Redirect("QuoteDetails.aspx")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    Private Sub SetTimeCombos()
        Try
            Dim i As Integer
            cbo_hr.Items.Clear()
            cbo_minute.Items.Clear()
            For i = 1 To 12
                If i < 10 Then
                    cbo_hr.Items.Add(New ListItem(Trim("0" & i), Trim("0" & i)))
                Else
                    cbo_hr.Items.Add(New ListItem(Val(i), Val(i)))
                End If
            Next
            'cbo_hr.SelectedValue = 12
            For i = 0 To 59 Step 15
                If i < 10 Then
                    cbo_minute.Items.Add(New ListItem(Trim("0" & i), Trim("0" & i)))
                Else
                    cbo_minute.Items.Add(New ListItem(Val(i), Val(i)))
                End If
            Next
            Dim lstHr = cbo_hr.Items.FindByText("06")
            If Not lstHr Is Nothing Then cbo_hr.SelectedValue = lstHr.Value

            Dim lstampm = cbo_ampm.Items.FindByText("PM")
            If Not lstampm Is Nothing Then cbo_ampm.SelectedValue = lstampm.Value

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub Srh_NameofSecurity_ButtonClick() Handles srh_NameofSecurity.ButtonClick
        Try
            FillSecurityDetails()
            clearlitcontrol()
            FillMaturityPutCallCouponDetails()
            Hid_Id.Value = HttpUtility.UrlEncode(objCommon.EncryptText(Convert.ToString(srh_NameofSecurity.SelectedId)))
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub srh_NameOFClient_ButtonClick() Handles srh_NameOFClient.ButtonClick
        Try
            OpenConn()
            'seema06aug2009
            'objCommon.FillControl(cbo_ContactPerson, clsCommonFuns.sqlConn, "ID_FILL_CustomerContact", "ContactPerson", "ContactId", srh_NameOFClient.SelectedId, "CustomerId")
            objCommon.FillControl(cbo_ContactPerson, sqlConn, "ID_FILL_CustContDetails", "ContactPerson", "ContactDetailId", Val(srh_NameOFClient.SelectedId), "CustomerId")

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Sub clearlitcontrol()
        lit_IpDates.Text = ""
        lit_Maturity.Text = ""
        lit_Coupon.Text = ""
        lit_Call.Text = ""
        lit_Put.Text = ""


    End Sub

    Private Sub FillMaturityPutCallCouponDetails()
        Try
            Dim dt As DataTable
            'Dim Coupon As String
            'Dim Calls As String
            'Dim Put As String
            'Dim Maturity As String
            OpenConn()
            Dim i As Int16
            dt = objCommon.FillDataTable(sqlConn, "ID_FILL_Securitydetails", srh_NameofSecurity.SelectedId, "SecurityId")
            If dt.Rows.Count > 0 Then
                lit_IpDates.Text = (dt.Rows(i).Item("IPDates") & "")
                Hid_NatureOfInstrument.Value = (dt.Rows(i).Item("NatureOfInstrument") & "")
                For i = 0 To dt.Rows.Count - 1
                    Hid_TypeFlag.Value = Trim(dt.Rows(i).Item("TypeFlag") & "")
                    If Hid_TypeFlag.Value = "M" Then
                        lit_Maturity.Text = lit_Maturity.Text & dt.Rows(i).Item(0).ToString() & "-" & (Format(Val(dt.Rows(i).Item(1)), "############")) & ", "
                    ElseIf Hid_TypeFlag.Value = "I" Then
                        lit_Coupon.Text = lit_Coupon.Text & IIf(dt.Rows(i).Item(0).ToString() = "30/12/9999", "", dt.Rows(i).Item(0).ToString()) & "-" & dt.Rows(i).Item(1).ToString() & ", "
                        'If Hid_NatureOfInstrument.Value = "NP" Then
                        '    lit_Coupon.Text = lit_Coupon.Text & dt.Rows(i).Item(0).ToString() & "-" & dt.Rows(i).Item(1).ToString() & ", "
                        'Else
                        '    If dt.Rows(i).Item(0) = Date.MaxValue Then
                        '        lit_Coupon.Text = lit_Coupon.Text & dt.Rows(i).Item(1).ToString() & ", "
                        '    Else
                        '        lit_Coupon.Text = lit_Coupon.Text & dt.Rows(i).Item(0).ToString() & "-" & dt.Rows(i).Item(1).ToString() & ", "
                        '    End If
                        'End If
                    ElseIf Hid_TypeFlag.Value = "C" Then
                        lit_Call.Text = lit_Call.Text & dt.Rows(i).Item(0).ToString() & "-" & (Format(Val(dt.Rows(i).Item(1)), "############")) & ", "

                    ElseIf Hid_TypeFlag.Value = "P" Then
                        lit_Put.Text = lit_Put.Text & dt.Rows(i).Item(0).ToString() & "-" & (Format(Val(dt.Rows(i).Item(1)), "############")) & ", "


                    End If
                Next
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Private Sub FillContactPerson()
        Try
            Dim dt As DataTable
            'seema06aug09
            OpenConn()
            dt = objCommon.FillControl(cbo_ContactPerson, sqlConn, "ID_FILL_CustContDetails", "ContactPerson", "ContactDetailId", Val(srh_NameOFClient.SelectedId), "CustomerId")

            'dt = objCommon.FillControl(cbo_ContactPerson, clsCommonFuns.sqlConn, "ID_FILL_CustomerContact", "ContactPerson", "ContactId", srh_NameOFClient.SelectedId, "CustomerId")
            If dt.Rows.Count > 0 Then
                'seema06aug09
                'cbo_ContactPerson.SelectedValue = Trim(dt.Rows(0).Item("ContactId").ToString)
                cbo_ContactPerson.SelectedValue = Trim(dt.Rows(0).Item("ContactDetailId").ToString)

            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Protected Sub btn_GenerateDealSlip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_GenerateDealSlip.Click
        Try
            Hid_PageName.Value = "QuoteEntry.aspx"
            Response.Redirect("DealSlipEntry.aspx?SecurityId=" & Val(srh_NameofSecurity.SelectedId) & "&PageName=" & Hid_PageName.Value & "&QuoteId=" & Val(ViewState("Id")) & "&Rate=" & yld_Calc.TextBoxRate.Text & "&FaceValue=" & yld_Calc.TextBoxFaceValue.Text & "&Multiple=" & yld_Calc.cboFaceValue.SelectedValue, False)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub cbo_SecurityType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SecurityType.SelectedIndexChanged
        srh_IssuerOfSecurity.SearchTextBox.Text = ""
        srh_IssuerOfSecurity.SelectedId = ""
        srh_NameofSecurity.SearchTextBox.Text = ""
        srh_NameofSecurity.SelectedId = ""
        'srh_NameOFClient.SearchTextBox.Text = ""
        'srh_NameOFClient.SelectedId = "" 
        'lit_IpDates.Text = ""
        'cbo_ContactPerson.SelectedIndex = 0

    End Sub

    Protected Sub srh_IssuerOfSecurity_ButtonClick() Handles srh_IssuerOfSecurity.ButtonClick
        srh_NameofSecurity.SearchTextBox.Text = ""
    End Sub

    Protected Sub btn_AddTempCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_AddTempCustomer.Click
        FillTempCustomer()
    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            Session("URL") = ""
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try

    End Sub

    Private Function FillTempCustomer()
        Try
            Dim sqlComm As New SqlCommand
            Dim sqlDa As New SqlDataAdapter
            Dim dtFill As New DataTable
            OpenConn()

            sqlComm.Connection = sqlConn
            With sqlComm
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_FILL_TempCustomer"
                .Parameters.Clear()
                objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 8, "I", , , Val(Hid_CustomeId.Value))
                objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.BigInt, 8, "I", , , Val(Session("UserId")))
                objCommon.SetCommandParameters(sqlComm, "@Ret_Code", SqlDbType.Int, 4, "O")
                .ExecuteNonQuery()
                sqlDa.SelectCommand = sqlComm
                sqlDa.Fill(dtFill)

                If dtFill.Rows.Count > 0 Then
                    txt_TempCustomer.Text = Trim(dtFill.Rows(0).Item("CustomerName") & "")
                    txt_TempContact.Text = Trim(dtFill.Rows(0).Item("ContactPerson") & "")
                End If
            End With
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Function

    Protected Sub btn_Refresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Refresh.Click
        srh_IssuerOfSecurity.SearchTextBox.Text = ""
        'srh_IssuerOfSecurity.SelectedId = 0
        srh_NameOFClient.SearchTextBox.Text = ""
        srh_NameOFClient.SelectedId = 0
        srh_NameofSecurity.SearchTextBox.Text = ""
        srh_NameofSecurity.SelectedId = 0
        cbo_SecurityType.SelectedIndex = 0
        yld_Calc.TextBoxRate.Text = ""
        yld_Calc.TextBoxYTCAnn.Text = ""
        yld_Calc.TextBoxYTCSemi.Text = ""
        yld_Calc.TextBoxYTMAnn.Text = ""
        yld_Calc.TextBoxYTMDate.Text = ""
        yld_Calc.TextBoxYTMSemi.Text = ""
        yld_Calc.TextBoxYTPAnn.Text = ""
        yld_Calc.TextBoxYTPSemi.Text = ""
        yld_Calc.RdoRateActual.SelectedIndex = 0
        yld_Calc.RdoYXM.SelectedIndex = 0
        yld_Calc.RdoSemiAnn.SelectedIndex = 0
        yld_Calc.RdoYield.Checked = True
        yld_Calc.RdoMatToRate.Checked = False
        yld_Calc.RdoPutToRate.Checked = False
        yld_Calc.RdoMatCallToRate.Checked = False
        yld_Calc.TextBoxFaceValue.Text = ""
        yld_Calc.cboFaceValue.SelectedIndex = 2
        yld_Calc.RdoSemiAnn.Enabled = True
        yld_Calc.TextBoxYTMDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
        yld_Calc.cboSettDay.SelectedValue = "0"
        yld_Calc.TextBoxSettDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
        yld_Calc.RdoMatCallToRate.Enabled = True
        yld_Calc.RdoPutToRate.Enabled = True
        lit_Maturity.Text = ""
        lit_Call.Text = ""
        lit_Coupon.Text = ""
        lit_Put.Text = ""
        lit_IpDates.Text = ""
        tblAccInterest.Visible = False
        btn_ShowSecurity.Visible = False
    End Sub
    ''*******************Mehul***************25/Nov/2019*****
    Protected Sub btnshowAccInerest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAccInerest.Click
        Try
            If Val(HDAcc_Rate.Value) = 0 Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Please Enter Rate...');", True)
                Exit Sub
            End If
            If Val(HDAcc_FaceValue.Value) = 0 Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Please Enter Facevalue...');", True)
                Exit Sub
            End If
            If Val(srh_NameofSecurity.SelectedId) = 0 Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('Please Enter Name Of Security...');", True)
                Exit Sub
            End If
            tblAccInterest.Visible = True
            FillAccruedDetails()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillAccruedDetails()
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            Dim sqlda As New SqlDataAdapter
            Dim dt As New DataTable
            Dim sqldv As New DataView
            Dim RateActual As String = "R"


            sqlComm.Connection = sqlConn
            sqlComm.CommandText = "ID_Fill_QuoteEntry_AccruedDetails"
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Parameters.Clear()

            objCommon.SetCommandParameters(sqlComm, "@SecurityId", SqlDbType.BigInt, 4, "I", , , Val(srh_NameofSecurity.SelectedId))
            objCommon.SetCommandParameters(sqlComm, "@SettlementDate", SqlDbType.Date, 4, "I", , , objCommon.DateFormat(HDAcc_Date.Value))
            objCommon.SetCommandParameters(sqlComm, "@TotalFaceValue", SqlDbType.Decimal, 20, "I", , , Val(HDAcc_FaceValue.Value) * Val(HDAcc_Multiple.Value))
            objCommon.SetCommandParameters(sqlComm, "@StepUp", SqlDbType.Char, 1, "I", , , HDAcc_StepUp.Value)
            sqlComm.ExecuteNonQuery()
            sqlda.SelectCommand = sqlComm
            sqlda.Fill(dt)

            If dt.Rows.Count > 0 Then
                '    Hid_IntDays.Value = Val(dt.Rows(0).Item("AccruedDays") & "")
                If RateActual = "R" Then
                    txt_Amount.Text = RoundToTwo(Val(HDAcc_FaceValue.Value) * Val(HDAcc_Multiple.Value) * RoundToFour(Val(HDAcc_Rate.Value)) / 100)
                    txt_AddInterest.Text = RoundToTwo(Val(dt.Rows(0).Item("AccruedInterest") & ""))
                Else
                    txt_Amount.Text = RoundToTwo((txt_Amount.Text + txt_AddInterest.Text) / Val(Hid_FaceValue.Value) * Val(HDAcc_FaceValue.Value) * Val(HDAcc_Multiple.Value))
                    txt_AddInterest.Text = RoundToTwo(Val(dt.Rows(0).Item("AccruedInterest") & ""))
                End If

                txt_SettAmt.Text = Val(txt_Amount.Text) + Val(txt_AddInterest.Text)
                lbl_AddInterest.Visible = True
                lbl_SettAmt.Visible = True
                If Val(txt_AddInterest.Text) = 0 Then
                    lbl_AddInterest.Text = ""
                    lbl_SettAmt.Text = ""
                End If
                If Val(dt.Rows(0).Item("AccruedDays") & "") > 0 Then
                    row_Interest.TagName = "Add Interest:"
                    lbl_AddInterest.Text = Trim(dt.Rows(0).Item("AccruedDates") & "")
                    lbl_SettAmt.Text = "(" & Val(dt.Rows(0).Item("AccruedDays") & "") & " Days)"
                    txt_AddInterest.Attributes.Add("style", "color:black")
                    lbl_SettAmt.Attributes.Add("style", "color:black")
                Else
                    row_Interest.TagName = "Less Interest:"
                    Hid_IntDays.Value = Val(dt.Rows(0).Item("AccruedDays") & "")
                    lbl_AddInterest.Text = Trim(dt.Rows(0).Item("AccruedDates") & "")
                    lbl_SettAmt.Text = "(" & Hid_IntDays.Value & " Days)"
                    txt_AddInterest.Text = "-" & txt_AddInterest.Text
                    txt_AddInterest.Attributes.Add("style", "color:red")
                    lbl_SettAmt.Attributes.Add("style", "color:red")
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub
    Private Function RoundToTwo(ByVal dec As Decimal) As Decimal
        Try
            Dim rounded As Decimal = Decimal.Round(dec, 2)
            rounded = Format(rounded, "###################0.00")
            Return rounded
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function RoundToFour(ByVal dec As Decimal) As Decimal
        Try
            Dim rounded As Decimal = Decimal.Round(dec, 4)
            rounded = Format(rounded, "###################0.0000")
            Return rounded
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    '' '' '' ''*******************Mehul***************25/Nov/2019*****
    '' '' ''Protected Sub btnCalculateCurrentRate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCalculateCurrentRate.Click
    '' '' ''    Try
    '' '' ''        iframe1.Visible = True
    '' '' ''        iframe1.Attributes.Add("src", "CurrentRate.aspx?Id=" & HDAcc_Id.Value & "&Rate=" & HDAcc_Rate.Value & "&PurDate=" & HDAcc_Date.Value & "&FaceValue=" & HDAcc_FaceValue.Value & "&Multiple=" & HDAcc_Multiple.Value & "&IsClose=True")
    '' '' ''        iframe1.Attributes.Add("width", "700px")
    '' '' ''        iframe1.Attributes.Add("height", "350px")
    '' '' ''    Catch ex As Exception
    '' '' ''        Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
    '' '' ''    End Try
    '' '' ''End Sub

End Class
