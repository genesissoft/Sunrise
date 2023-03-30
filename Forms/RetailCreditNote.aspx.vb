Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports RKLib.ExportData

Partial Class Forms_RetailDebitNote
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim strProcName As String
    Dim strCond As String = ""
    Dim strPrevCustName As String
    Dim strPrevRefNo As String
    Dim intPrevDebitNote As Int16
    Dim sqlConn As SqlConnection
    Dim WdmdealFlag As Char
    Dim BrokerId As Int16
    Dim TransType As Char
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'If Val(Session("UserId") & "") = 0 Then
            '    Response.Redirect("Login.aspx", False)
            '    Exit Sub
            'End If
            'objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.AddHeader("Cache-Control", "no-cache")
            Response.AddHeader("Cache-Control", "no-store")

            srh_Broker.SelectLinkButton.Enabled = True
            If cbo_DealTransType.SelectedValue = "B" Then
                rdo_DateType.SelectedValue = "D"
                rdo_DateType.Enabled = False
                'tr_Customer.Visible = True
                tr_Broker.Visible = False
            Else
                rdo_DateType.SelectedValue = "S"
                rdo_DateType.Enabled = False
                tr_Broker.Visible = True
                'tr_Customer.Visible = False

            End If
            If IsPostBack = False Then
                Session("DebitLetterRefNo") = ""
                Session("RetailDebitRefNo") = ""
                Session("RetDebitRefNo") = ""
                Session("ADDCustids") = ""
                Session("ADDBrokerids") = ""
                Session("RetCreditRefNo") = ""
                SetAttributes()
                FillServiceTax()
                FillBlankDebitnoteGrids()
                Hid_DebitRefNo.Value = ""
            End If
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "show", "DateMonthSelection();", True)
        Catch ex As Exception
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
    Sub GetCurrentMthYear()




    End Sub
    Private Sub FillBlankDebitnoteGrids()
        Try
            Dim dt As New DataTable
            Dim objSrh As New clsSearch
            dt.Columns.Add("DealSlipNo", GetType(String))
            dt.Columns.Add("DealDate", GetType(String))
            dt.Columns.Add("BrokerName", GetType(String))
            dt.Columns.Add("BrokerageAmt", GetType(String))
            'dt.Columns.Add("SecurityName", GetType(String))
            dg_Debitnote.DataSource = dt
            dg_Debitnote.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub SetAttributes()
        'rdo_Selection.Attributes.Add("onclick", "DateMonthSelection();")
        txt_FromDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
        txt_FromDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_FromDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_ToDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_ToDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_ToDate.Text = Format(DateAdd(DateInterval.Day, 1, Today), "dd/MM/yyyy")
        btn_Save.Attributes.Add("onclick", "return  Validation();")
        cbo_Months.SelectedValue = Month(Now)
        cbo_Year.SelectedValue = Val(Year(Now))
        txt_DebitDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_DebitDate.Attributes.Add("onblur", "CheckDate(this,false);")
        btn_Show.Attributes.Add("onclick", "return showvalidation();")

    End Sub

    Protected Sub btn_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Show.Click
        Dim strCond As String = ""
        Dim strRpt As String
        'strRpt = hid_ReportType.Value
        'If rdo_Selection.SelectedValue = "M" Then
        Getdate()
        'End If
        Session("RetDebitRefNo") = ""
        Session("RetailDebitRefNo") = ""
        Session("BrokeridsADD") = ""
        Session("RetCreditRefNo") = ""
        strCond = BuildDBCustStr(srh_Broker.SelectCheckbox, srh_Broker.SelectListBox)
        Session("ADDBrokerids") = strCond
        GetReportTable()
        If dg_Debitnote.Items.Count > 0 Then
            row_Export.Visible = True
        End If
    End Sub

    Private Function GetReportTable() As DataTable
        Try
            OpenConn()
            Dim datFrom As Date
            Dim datTo As Date
            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand

            'Dim intColIndex As Int16

            datFrom = objCommon.DateFormat(txt_FromDate.Text)
            datTo = objCommon.DateFormat(txt_ToDate.Text)
            'openconnection()

            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = "1000"
                strProcName = "Id_RPT_RetailCreditNote"
                .CommandText = strProcName
                .Parameters.Clear()
                If txt_FromDate.Text <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)
                End If
                If txt_ToDate.Text <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@ToDate", SqlDbType.SmallDateTime, 8, "I", , , datTo)
                End If
                If Trim(Session("ADDBrokerids")) <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.VarChar, 4000, "I", , , Trim(Session("ADDBrokerids")))
                End If
                'If Trim(Session("ADDCustids")) <> "" Then
                '    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.VarChar, 4000, "I", , , Trim(Session("ADDCustids")))
                'End If
                If rdo_DateType.SelectedValue <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@DateTypeFlag", SqlDbType.Char, 1, "I", , , rdo_DateType.SelectedValue & "")
                End If

                'If rdo_Selection.SelectedValue <> "" Then
                '    objCommon.SetCommandParameters(sqlComm, "@DateMonthFlag", SqlDbType.Char, 1, "I", , , rdo_Selection.SelectedValue & "")
                'End If

                objCommon.SetCommandParameters(sqlComm, "@MonthText", SqlDbType.VarChar, 10, "I", , , cbo_Months.SelectedItem.Text & "")

                objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , cbo_DealTransType.SelectedValue)

                'If cbo_Months.SelectedItem.Text <> "" Then
                '    objCommon.SetCommandParameters(sqlComm, "@MonthText", SqlDbType.VarChar, 10, "I", , , cbo_Months.SelectedItem.Text & "")
                'End If

                If cbo_Year.SelectedItem.Text <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@Yeartext1", SqlDbType.VarChar, 10, "I", , , cbo_Year.SelectedItem.Text & "")
                End If
                objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                .ExecuteNonQuery()
                sqlDa.SelectCommand = sqlComm
                sqlDa.Fill(dtfill)
                Session("RetailCreditTable") = dtfill
                dg_Debitnote.DataSource = dtfill
                dg_Debitnote.DataBind()
            End With
            Return dtfill

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try

    End Function
    Private Function BuildDBCustStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox) As String
        Try
            Dim strCond As String = ""
            Dim strIds As String = ""

            If chkbox.Checked = False Then
                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strIds += lstbox.Items(I).Value & ","
                    End If
                Next
                'strCond = 
                strCond += Mid(strIds, 1, Len(strIds) - 1) & " "
                Session("ADDBrokerids") = Mid(strIds, 1, Len(strIds) - 1)
                'If cbo_DealTransType.SelectedValue <> "B" Then
                '    Session("ADDBrokerids") = Mid(strIds, 1, Len(strIds) - 1)
                'Else
                '    Session("ADDCustids") = Mid(strIds, 1, Len(strIds) - 1)
                'End If

            End If
            Return strCond
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Function
    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            Dim strFile As String
            Dim intLength As Int16
            Dim sqlTrans As SqlTransaction
            'sqlTrans = sqlConn.BeginTransaction
            'If SaveDebitDetails(sqlTrans) = False Then Exit Sub
            SaveDebitDetails()
            'If UpdateAdvisoryNoteRefNo(sqlTrans) = False Then Exit Sub
            'sqlTrans.Commit()
            lbl_Msg.Text = "Debit Note details Saved Successfully"
            btn_Save.Visible = False
            strFile = Session("RetCreditRefNo")
            intLength = Len(strFile)
            If Val(strFile) > 0 Then
                strFile = Left(strFile, intLength - 1)
                Session("RetCreditRefNo") = strFile
            End If


            Hid_DebitRefNo.Value = ""
            hid_ReportType.Value = "RetailCreditRpt"
            Hid_ReptForm.Value = "RetailCreditForm"
            Response.Redirect("Reportselection.aspx?&Rpt=" & hid_ReportType.Value & "&RptForm=" & Hid_ReptForm.Value & "&DealTransType=" & cbo_DealTransType.SelectedValue, False)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function UpdateAdvisoryNoteRefNo(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            dt = Session("RetailCreditTable")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("CustomerId").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    sqlComm.CommandText = "ID_UPDATE_RetailNoteDeal"
                    objCommon.SetCommandParameters(sqlComm, "@DebtDateType", SqlDbType.Char, 1, "I", , , Trim(rdo_DateType.SelectedValue))
                    'objCommon.SetCommandParameters(sqlComm, "@DebtDateMonthflag", SqlDbType.Char, 1, "I", , , Trim(rdo_Selection.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@DebtMonths", SqlDbType.VarChar, 50, "I", , , Trim(cbo_Months.SelectedItem.Text))
                    objCommon.SetCommandParameters(sqlComm, "@DebtYear", SqlDbType.VarChar, 50, "I", , , Trim(cbo_Year.SelectedItem.Text))
                    objCommon.SetCommandParameters(sqlComm, "@DebtFromDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_FromDate.Text))
                    objCommon.SetCommandParameters(sqlComm, "@DebtToDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ToDate.Text))
                    objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , Trim(cbo_DealTransType.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@RefNo", SqlDbType.VarChar, 50, "I", , , Trim(TryCast(dg_Debitnote.Items(I).FindControl("lbl_RefNo"), Label).Text))
                    If cbo_DealTransType.SelectedValue = "B" Then
                        objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.BigInt, 16, "I", , , Trim(TryCast(dg_Debitnote.Items(I).FindControl("lbl_CustomerId"), Label).Text))
                    Else
                        objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.Int, 4, "I", , , Trim(TryCast(dg_Debitnote.Items(I).FindControl("lbl_CustomerId"), Label).Text))
                    End If
                    'objCommon.SetCommandParameters(sqlComm, "@DebitCredit", SqlDbType.Char, 1, "I", , , Trim(rdo_DebitCredit.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
                    objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Sub SaveDebitDetails()
        Try

            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            Dim strRefNo As String
            Dim strServTx As String

            dt = Session("RetailCreditTable")
            Dim strIds As Array
            Dim strServtax As Array
            Dim strarr As Array()

            strIds = Split(Hid_DealSlipIds.Value, ",")
            strServtax = Split(Hid_STInEx.Value, ",")
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            Session("RetCreditRefNo") = ""
            For K As Int16 = 0 To dt.Rows.Count - 1
                For I As Int16 = 0 To strIds.Length - 1

                    If Val(strIds(I)) <> 0 Then
                        'strServTx = strServtax(K)
                        If Val(strIds(I)) = TryCast(dg_Debitnote.Items(K).FindControl("lbl_DealSlipId"), Label).Text Then
                            GetCreditRefNo()

                            If strPrevCustName <> TryCast(dg_Debitnote.Items(K).FindControl("lbl_Brokername"), Label).Text Then
                                intPrevDebitNote = intPrevDebitNote + 1
                                strRefNo = GetRefNo()
                                strRefNo.Remove(strRefNo.Length - 1, 1)
                                Session("RetCreditRefNo") = Session("RetCreditRefNo") + strRefNo + ","

                            Else
                                strRefNo = strPrevRefNo
                            End If
                            strPrevRefNo = strRefNo
                            strPrevCustName = TryCast(dg_Debitnote.Items(K).FindControl("lbl_Brokername"), Label).Text
                            sqlComm.Parameters.Clear()
                            sqlComm.CommandText = "ID_INSERT_RetCreditDetails"
                            objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.BigInt, 8, "I", , , Val(TryCast(dg_Debitnote.Items(K).FindControl("lbl_BrokerId"), Label).Text))
                            objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_FromDate.Text))
                            objCommon.SetCommandParameters(sqlComm, "@ToDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ToDate.Text))
                            objCommon.SetCommandParameters(sqlComm, "@BrokerageAmount", SqlDbType.Decimal, 9, "I", , , Val(TryCast(dg_Debitnote.Items(K).FindControl("lbl_BrokAmt"), Label).Text))
                            objCommon.SetCommandParameters(sqlComm, "@DebtMonths", SqlDbType.VarChar, 50, "I", , , Trim(cbo_Months.SelectedItem.Text))
                            objCommon.SetCommandParameters(sqlComm, "@DebtYear", SqlDbType.VarChar, 50, "I", , , Trim(cbo_Year.SelectedItem.Text))
                            objCommon.SetCommandParameters(sqlComm, "@RefNo", SqlDbType.VarChar, 50, "I", , , strRefNo)

                            objCommon.SetCommandParameters(sqlComm, "@ServInEx ", SqlDbType.Char, 1, "I", , , strServtax(K))
                            'objCommon.SetCommandParameters(sqlComm, "@ServInEx", SqlDbType.Char, 1, "I", , , rdo_incServtax.SelectedValue)


                            objCommon.SetCommandParameters(sqlComm, "@CreditDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_DebitDate.Text))
                            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Session("YearId"))
                            objCommon.SetCommandParameters(sqlComm, "@Compid", SqlDbType.Int, 4, "I", , , Session("Compid"))
                            objCommon.SetCommandParameters(sqlComm, "@CancelFlag", SqlDbType.TinyInt, 2, "I", , , 0)
                            objCommon.SetCommandParameters(sqlComm, "@DebtFromDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_FromDate.Text))
                            objCommon.SetCommandParameters(sqlComm, "@DebtToDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ToDate.Text))
                            objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(TryCast(dg_Debitnote.Items(K).FindControl("lbl_DealSlipId"), Label).Text))
                            objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, "O")
                            objCommon.SetCommandParameters(sqlComm, "@CreditDetailId", SqlDbType.BigInt, 8, "O")
                            sqlComm.ExecuteNonQuery()
                        End If
                    End If


                Next
            Next



        Catch ex As Exception
            'sqlTrans.Rollback()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillServiceTax()
        Try
            Dim dt As DataTable
            OpenConn()
            dt = objCommon.FillDataTable(sqlConn, "MB_FILL_IssueServiceTax", 0, "")
            If dt.Rows.Count > 0 Then
                Hid_ServTax.Value = dt.Rows(0).Item("Tax").ToString
                Hid_Cess.Value = dt.Rows(0).Item("Cess").ToString
                Hid_ECess.Value = dt.Rows(0).Item("ECess").ToString
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub


    Protected Sub dg_Debitnote_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_Debitnote.ItemDataBound
        Try
            Dim intDealSlipID As Integer
            Dim intBrokerId As Integer
            Dim boolServ As Boolean
            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                e.Item.ID = "itm" & e.Item.ItemIndex
                intBrokerId = Val(CType(e.Item.FindControl("lbl_BrokerId"), Label).Text)
                intDealSlipID = Val(CType(e.Item.FindControl("lbl_DealSlipId"), Label).Text)
                Hid_DealSlipIds.Value += Val(CType(e.Item.FindControl("lbl_DealSlipId"), Label).Text) & "!"
                'Hid_STInEx.Value += CType(e.Item.FindControl("chk_ItemChecked1"), CheckBox).Checked & "!"
                'If CType(e.Item.FindControl("chk_ItemChecked1"), CheckBox).Checked Then
                '    Hid_STInEx.Value += CType(e.Item.FindControl("chk_ItemChecked1"), CheckBox).Checked & "!"
                'End If
            End If
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Function GetRefNo() As String
        Try
            Dim SB As New StringBuilder
            For I As Int16 = 1 To 4 - intPrevDebitNote.ToString.Length
                SB.Append("0")
            Next
            SB.Append(intPrevDebitNote.ToString)
            Return SB.ToString
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function GetCreditRefNo()
        Try
            OpenSqlConn()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_Get_RetailDebitRefNo"
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
            'objCommon.SetCommandParameters(sqlComm, "@DebitCredit", SqlDbType.Char, 1, "I", , , Trim(rdo_DebitCredit.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@RetDealType", SqlDbType.Char, 1, "I", , , Trim(cbo_DealTransType.SelectedValue))
            objCommon.SetCommandParameters(sqlComm, "@MaxDebitNo", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            intPrevDebitNote = Val(sqlComm.Parameters("@MaxDebitNo").Value & "")

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

            Return False
        Finally
            CloseSqlConn()
        End Try
    End Function
    Private Sub CloseSqlConn()
        If sqlConn Is Nothing Then Exit Sub
        If sqlConn.State = ConnectionState.Open Then sqlConn.Close()
    End Sub
    Private Sub OpenSqlConn()
        sqlConn = New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        sqlConn.Open()
    End Sub
    Protected Sub cbo_Months_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Months.SelectedIndexChanged
        Getdate()
    End Sub

    Protected Sub cbo_Year_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Year.SelectedIndexChanged

        Getdate()

    End Sub
    Private Sub Getdate()
        Try
            Dim dateFrom As DateTime
            Dim dateTo As DateTime
            Dim m As Int16
            Dim Y As Int16
            m = cbo_Months.SelectedValue
            Y = cbo_Year.SelectedValue
            dateFrom = DateSerial(Y, m, 1)
            dateTo = DateSerial(Y, m + 1, 1 - 1)
            txt_FromDate.Text = Format(dateFrom, "dd/MM/yyyy")
            txt_ToDate.Text = Format(dateTo, "dd/MM/yyyy")
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Export_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Export.Click
        Try
            ConvertToExcel("Id_RPT_RetailCreditExport")
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

        End Try
    End Sub

    Private Sub ConvertToExcel(ByVal strProcName As String)
        Try
            Dim dt As DataTable
            Dim ds As New DataSet
            Dim tw As New System.IO.StringWriter()
            Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            Dim dgGrid As New DataGrid()
            Response.ClearHeaders()
            dt = GetReportTableExport()
            'dgGrid.DataSource = dt
            If dt.Rows.Count > 0 Then
                Dim objExport As New RKLib.ExportData.Export("Web")
                objExport.ExportDetails(dt, Export.ExportFormat.Excel, "Report.xls")
            Else
                Dim strHtml As String
                strHtml = "alert('Sorry!!! No Records available to show report');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
            End If

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

        End Try
    End Sub


    Private Function GetReportTableExport() As DataTable
        Try
            OpenConn()
            Dim datFrom As Date
            Dim datTo As Date
            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand

            'Dim intColIndex As Int16

            datFrom = objCommon.DateFormat(txt_FromDate.Text)
            datTo = objCommon.DateFormat(txt_ToDate.Text)
            'openconnection()

            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = "1000"
                strProcName = "Id_RPT_RetailCreditExport"
                .CommandText = strProcName
                .Parameters.Clear()
                If txt_FromDate.Text <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)
                End If
                If txt_ToDate.Text <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@ToDate", SqlDbType.SmallDateTime, 8, "I", , , datTo)
                End If
                If Trim(Session("ADDBrokerids")) <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.VarChar, 4000, "I", , , Trim(Session("ADDBrokerids")))
                End If
                'If Trim(Session("ADDCustids")) <> "" Then
                '    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.VarChar, 4000, "I", , , Trim(Session("ADDCustids")))
                'End If
                If rdo_DateType.SelectedValue <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@DateTypeFlag", SqlDbType.Char, 1, "I", , , rdo_DateType.SelectedValue & "")
                End If

                'If rdo_Selection.SelectedValue <> "" Then
                '    objCommon.SetCommandParameters(sqlComm, "@DateMonthFlag", SqlDbType.Char, 1, "I", , , rdo_Selection.SelectedValue & "")
                'End If

                objCommon.SetCommandParameters(sqlComm, "@MonthText", SqlDbType.VarChar, 10, "I", , , cbo_Months.SelectedItem.Text & "")

                objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , cbo_DealTransType.SelectedValue)

                'If cbo_Months.SelectedItem.Text <> "" Then
                '    objCommon.SetCommandParameters(sqlComm, "@MonthText", SqlDbType.VarChar, 10, "I", , , cbo_Months.SelectedItem.Text & "")
                'End If

                If cbo_Year.SelectedItem.Text <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@Yeartext1", SqlDbType.VarChar, 10, "I", , , cbo_Year.SelectedItem.Text & "")
                End If
                objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                .ExecuteNonQuery()
                sqlDa.SelectCommand = sqlComm
                sqlDa.Fill(dtfill)
                'Session("RetailCreditTable") = dtfill
                'dg_Debitnote.DataSource = dtfill
                'dg_Debitnote.DataBind()
            End With
            Return dtfill

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try

    End Function
    Protected Sub cbo_DealTransType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DealTransType.SelectedIndexChanged
        Try
            If cbo_DealTransType.SelectedValue = "B" Then
                rdo_DateType.SelectedValue = "D"
                rdo_DateType.Enabled = False
                'tr_Customer.Visible = True
                tr_Broker.Visible = False
            Else
                rdo_DateType.SelectedValue = "S"
                rdo_DateType.Enabled = False
                tr_Broker.Visible = True
                'tr_Customer.Visible = False

            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

  
End Class
