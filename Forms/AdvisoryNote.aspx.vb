Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports RKLib.ExportData
Imports log4net

Partial Class Forms_AdvisoryNote
    Inherits System.Web.UI.Page
    Dim PgName As String = "$AdvisoryNote$"
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
    Dim strRef As String
    Dim objUtil As New Util



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
            'srsrh_Customer.SelectLinkButton.Enabled = True

            If IsPostBack = False Then
                Session("AdvisoryLetterRefNo") = ""
                Session("AdvisoryRefNo") = ""
                Session("DebitRefNo") = ""
                SetAttributes()
                FillBlankDebitnoteGrids()
                Hid_DebitRefNo.Value = ""
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "show", "DateMonthSelection();", True)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex)
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
    Private Sub FillBlankDebitnoteGrids()
        Try
            Dim dt As New DataTable
            Dim objSrh As New clsSearch
            dt.Columns.Add("WDMDealNumber", GetType(String))
            dt.Columns.Add("DealDate", GetType(String))
            dt.Columns.Add("SettlementDate", GetType(String))
            dt.Columns.Add("Brokername", GetType(String))
            dt.Columns.Add("SecurityTypeName", GetType(String))
            dt.Columns.Add("SecurityName", GetType(String))
            dg_Debitnote.DataSource = dt
            dg_Debitnote.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillBlankDebitnoteGrids", "Error in FillBlankDebitnoteGrids", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub SetAttributes()
        rdo_Selection.Attributes.Add("onclick", "DateMonthSelection();")
        txt_FromDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
        txt_FromDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_FromDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_ToDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_ToDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_ToDate.Text = Format(DateAdd(DateInterval.Day, 1, Today), "dd/MM/yyyy")
        btn_Save.Attributes.Add("onclick", "return  Validation();")
    End Sub

    Protected Sub btn_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Show.Click
        Dim strCond As String = ""
        Dim strRpt As String
        'strRpt = hid_ReportType.Value
        If rdo_Selection.SelectedValue = "M" Then
            Getdate()
        End If
        Session("DebitRefNo") = ""
        Session("AdvisoryRefNo") = ""
        Session("ADDBrokerids") = ""
        Hid_Intids.Value = ""
        Hid_DealSlipIds.Value = ""
        Hid_TransType.Value = ""
        strCond = BuildDBCustStr(srh_Broker.SelectCheckbox, srh_Broker.SelectListBox)
        Session("ADDBrokerids") = strCond
        GetReportTable()
        If dg_Debitnote.Items.Count > 0 Then
            row_Export.Visible = True
        End If
    End Sub

    Private Function GetReportTable(Optional ByVal strSort As String = "Brokername asc", Optional ByVal IntPageIndex As Int16 = 0) As DataTable
        Try
            OpenConn()
            Dim datFrom As Date
            Dim datTo As Date
            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand
            Dim dvFill As New DataView
            'Dim intColIndex As Int16

            datFrom = objCommon.DateFormat(txt_FromDate.Text)
            datTo = objCommon.DateFormat(txt_ToDate.Text)
            'openconnection()

            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = "1000"
                strProcName = "Id_RPT_AdvisorynoteForm"
                .CommandText = strProcName
                .Parameters.Clear()
                If txt_FromDate.Text <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)
                End If
                If txt_ToDate.Text <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@ToDate", SqlDbType.SmallDateTime, 8, "I", , , datTo)
                End If
                If Trim(Session("ADDBrokerids")) <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@Brokerid", SqlDbType.VarChar, 4000, "I", , , Trim(Session("ADDBrokerids")))
                End If
                If rdo_DateType.SelectedValue <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@DateTypeFlag", SqlDbType.Char, 1, "I", , , rdo_DateType.SelectedValue & "")
                End If

                If rdo_Selection.SelectedValue <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@DateMonthFlag", SqlDbType.Char, 1, "I", , , rdo_Selection.SelectedValue & "")
                End If

                If rdo_Selection.SelectedValue = "D" Then
                    objCommon.SetCommandParameters(sqlComm, "@MonthText", SqlDbType.VarChar, 10, "I", , , "")
                Else
                    objCommon.SetCommandParameters(sqlComm, "@MonthText", SqlDbType.VarChar, 10, "I", , , cbo_Months.SelectedItem.Text & "")
                End If

                'If cbo_Months.SelectedItem.Text <> "" Then
                '    objCommon.SetCommandParameters(sqlComm, "@MonthText", SqlDbType.VarChar, 10, "I", , , cbo_Months.SelectedItem.Text & "")
                'End If

                If Trim(Request.QueryString("Yeartext1") & "") <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@Yeartext1", SqlDbType.VarChar, 10, "I", , , cbo_Year.SelectedItem.Text & "")
                End If
                objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                .ExecuteNonQuery()
                sqlDa.SelectCommand = sqlComm
                sqlDa.Fill(dtfill)
                Session("AdvisorynoteTable") = dtfill
                dvFill = dtfill.DefaultView
                dvFill.Sort = strSort
                dg_Debitnote.CurrentPageIndex = IntPageIndex
                dg_Debitnote.DataSource = dtfill
                dg_Debitnote.DataBind()
            End With
            Return dtfill

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetReportTable", "Error in GetReportTable", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Function
    Private Function BuildDBCustStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox) As String
        Try
            Dim strCond As String = ""
            Dim strIds As String = ""

            'If chkbox.Checked = False Then
            For I As Int16 = 0 To lstbox.Items.Count - 1
                If lstbox.Items(I).Value <> "" Then
                    strIds += lstbox.Items(I).Value & ","
                End If
            Next
            If strIds.Length > 0 Then
                strCond += "    " & Mid(strIds, 1, Len(strIds) - 1) & " "
                Session("ADDBrokerids") = Mid(strIds, 1, Len(strIds) - 1)
            End If

            'End If
            Return strCond
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "BuildDBCustStr", "Error in BuildDBCustStr", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            OpenConn()
            Dim strFile As String
            Dim intLength As Int16
            'Dim sqlTrans As SqlTransaction
            'sqlTrans = sqlConn.BeginTransaction

            'If UpdateAdvisoryNoteRefNo(sqlTrans) = False Then Exit Sub
            UpdateAdvisoryNoteRefNo()
            'sqlTrans.Commit()
            lbl_Msg.Text = "Advisory Note Number Save Successfully"
            btn_Save.Visible = False

            strFile = Session("AdvisoryRefNo")
            intLength = Len(strFile)
            If Val(strFile) > 0 Then
                strFile = Left(strFile, intLength - 1)
                Session("AdvisoryRefNo") = strFile
            End If
            Hid_DebitRefNo.Value = ""
            hid_ReportType.Value = "AdvisoryReport"
            Response.Redirect("ViewReports.aspx?&Rpt=" & hid_ReportType.Value & "&DateTypeFlag=" & rdo_DateType.SelectedValue & "&Fromdate=" & txt_FromDate.Text & "&Todate=" & txt_ToDate.Text, False)

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_Save_Click", "Error in btn_Save_Click", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Sub UpdateAdvisoryNoteRefNo()
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            dt = Session("AdvisorynoteTable")
            Dim strIds As Array
            Dim strBrokerIds As Array
            Dim strtranstype As Array
            strIds = Split(Hid_DealSlipIds.Value, ",")
            strBrokerIds = Split(Hid_Intids.Value, ",")
            strtranstype = Split(Hid_TransType.Value, ",")

            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            For K As Int16 = 0 To dt.Rows.Count - 1
                For I As Int16 = 0 To strIds.Length - 1

                    If Val(strIds(I)) <> 0 Then
                        If Val(strIds(I)) = TryCast(dg_Debitnote.Items(K).FindControl("lbl_DealEntryId"), Label).Text And Trim(strtranstype(I)) = TryCast(dg_Debitnote.Items(K).FindControl("lbl_TransType"), Label).Text Then
                            GetDebitRefNo(strIds(I))
                            If strPrevCustName <> TryCast(dg_Debitnote.Items(K).FindControl("lbl_Brokername"), Label).Text Then
                                intPrevDebitNote = intPrevDebitNote + 1
                                strRef = GetRefNo()
                                strRef.Remove(strRef.Length - 1, 1)
                                Session("AdvisoryRefNo") = Session("AdvisoryRefNo") + strRef + ","
                            Else
                                strRef = strPrevRefNo
                            End If
                            strPrevRefNo = strRef
                            strPrevCustName = TryCast(dg_Debitnote.Items(K).FindControl("lbl_Brokername"), Label).Text
                            sqlComm.Parameters.Clear()
                            sqlComm.CommandText = "ID_UPDATE_AdvisoryNoteDealEntry"
                            objCommon.SetCommandParameters(sqlComm, "@AdvisoryDateType", SqlDbType.Char, 1, "I", , , Trim(rdo_DateType.SelectedValue))
                            objCommon.SetCommandParameters(sqlComm, "@AdvisoryDateMonthflag", SqlDbType.Char, 1, "I", , , Trim(rdo_Selection.SelectedValue))
                            objCommon.SetCommandParameters(sqlComm, "@AdvisoryMonths", SqlDbType.VarChar, 50, "I", , , Trim(cbo_Months.SelectedItem.Text))
                            objCommon.SetCommandParameters(sqlComm, "@AdvisoryYear", SqlDbType.VarChar, 50, "I", , , Trim(cbo_Year.SelectedItem.Text))
                            objCommon.SetCommandParameters(sqlComm, "@AdvsoryFromDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_FromDate.Text))
                            objCommon.SetCommandParameters(sqlComm, "@AdvisoryToDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ToDate.Text))
                            objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.BigInt, 16, "I", , , Val(strBrokerIds(I)))
                            objCommon.SetCommandParameters(sqlComm, "@TransType", SqlDbType.Char, 1, "I", , , Trim(TryCast(dg_Debitnote.Items(K).FindControl("lbl_TransType"), Label).Text))
                            objCommon.SetCommandParameters(sqlComm, "@RefNo", SqlDbType.VarChar, 50, "I", , , strRef)
                            objCommon.SetCommandParameters(sqlComm, "@DealEntryId", SqlDbType.BigInt, 16, "I", , , Val(strIds(I)))
                            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
                            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                            sqlComm.ExecuteNonQuery()
                        End If
                    End If
                Next

            Next
            'Return True
        Catch ex As Exception
            'sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "UpdateAdvisoryNoteRefNo", "Error in UpdateAdvisoryNoteRefNo", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dg_Debitnote_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_Debitnote.ItemDataBound
        Try

            Dim intDealSlipID As Integer

            Dim chkBoxHead As CheckBox
            Dim chkBox As CheckBox

            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                'WdmdealFlag = TryCast(e.Item.FindControl("lbl_WdmDeal"), Label).Text
                'BrokerId = TryCast(e.Item.FindControl("lbl_BrokerId"), Label).Text
                'TransType = TryCast(e.Item.FindControl("lbl_TransType"), Label).Text
                'If Val(intPrevDebitNote) = 0 Then
                '    GetDebitRefNo()
                'End If

                'If strPrevCustName <> TryCast(e.Item.FindControl("lbl_Brokername"), Label).Text Then
                '    intPrevDebitNote = intPrevDebitNote + 1
                '    strRefNo = GetRefNo()
                '    Session("AdvisoryRefNo") = Session("AdvisoryRefNo") + strRefNo + ","
                'Else
                '    strRefNo = strPrevRefNo
                'End If
                'TryCast(e.Item.FindControl("lbl_RefNo"), Label).Text = strRefNo
                'strPrevRefNo = strRefNo
                'strPrevCustName = TryCast(e.Item.FindControl("lbl_Brokername"), Label).Text
            End If

            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                e.Item.ID = "itm" & e.Item.ItemIndex
                intDealSlipID = Val(CType(e.Item.FindControl("lbl_DealEntryId"), Label).Text)

                chkBox = CType(e.Item.FindControl("chk_ItemChecked"), CheckBox)

                Hid_DealSlipIds.Value += Val(CType(e.Item.FindControl("lbl_DealEntryId"), Label).Text) & "!"
                Hid_Intids.Value += Val(CType(e.Item.FindControl("lbl_BrokerId"), Label).Text) & "!"
                Hid_TransType.Value += Trim(CType(e.Item.FindControl("lbl_TransType"), Label).Text) & "!"
            Else
                If (e.Item.ItemType = ListItemType.Header) Then
                    chkBoxHead = CType(e.Item.FindControl("chk_AllItems"), CheckBox)
                End If

            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg_Debitnote_ItemDataBound", "Error in dg_Debitnote_ItemDataBound", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
            objUtil.WritErrorLog(PgName, "GetRefNo", "Error in GetRefNo", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function

    Private Function GetDebitRefNo(ByVal DealEntryId As Int64)
        Try
            OpenSqlConn()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_Get_AdvisoryRefNo"
            sqlComm.Parameters.Clear()
            'objCommon.SetCommandParameters(sqlComm, "@TransType", SqlDbType.Char, 1, "I", , , TransType)
            'objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.Char, 1, "I", , , BrokerId)
            objCommon.SetCommandParameters(sqlComm, "@DateType", SqlDbType.Char, 1, "I", , , rdo_Selection.SelectedValue)
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
            objCommon.SetCommandParameters(sqlComm, "@DealEntryId", SqlDbType.Int, 4, "I", , , DealEntryId)
            objCommon.SetCommandParameters(sqlComm, "@MaxAdvisoryNo", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            intPrevDebitNote = Val(sqlComm.Parameters("@MaxAdvisoryNo").Value & "")
            Return True
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetDebitRefNo", "Error in GetDebitRefNo", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

            Return False
        Finally
            CloseSqlConn()
        End Try
    End Function
    Private Function GetDebitRefNo1()
        Try
            OpenSqlConn()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_Get_AdvisoryRefNo"
            sqlComm.Parameters.Clear()
            'objCommon.SetCommandParameters(sqlComm, "@TransType", SqlDbType.Char, 1, "I", , , TransType)
            'objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.Char, 1, "I", , , BrokerId)
            'objCommon.SetCommandParameters(sqlComm, "@WdmDeal", SqlDbType.Char, 1, "I", , , WdmdealFlag)
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
            objCommon.SetCommandParameters(sqlComm, "@MaxAdvisoryNo", SqlDbType.Int, 4, "O")

            sqlComm.ExecuteNonQuery()
            intPrevDebitNote = Val(sqlComm.Parameters("@MaxAdvisoryNo").Value & "")
            Return True
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetDebitRefNo1", "Error in GetDebitRefNo1", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

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
        If rdo_Selection.SelectedValue = "M" Then
            Getdate()
        End If

    End Sub

    Protected Sub cbo_Year_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Year.SelectedIndexChanged
        If rdo_Selection.SelectedValue = "M" Then
            Getdate()
        End If
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
            objUtil.WritErrorLog(PgName, "Getdate", "Error in Getdate", "", ex)
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

    Protected Sub btn_Export_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Export.Click
        Try
            ConvertToExcel("Id_RPT_AdvisoryExport")
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_Export_Click", "Error in btn_Export_Click", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

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
            objUtil.WritErrorLog(PgName, "ConvertToExcel", "Error in ConvertToExcel", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

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
                strProcName = "Id_RPT_AdvisoryExport"
                .CommandText = strProcName
                .Parameters.Clear()

                If txt_FromDate.Text <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)
                End If
                If txt_ToDate.Text <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@ToDate", SqlDbType.SmallDateTime, 8, "I", , , datTo)
                End If
                If Trim(Session("ADDBrokerids")) <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@Brokerid", SqlDbType.VarChar, 4000, "I", , , Trim(Session("ADDBrokerids")))
                End If
                If rdo_DateType.SelectedValue <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@DateTypeFlag", SqlDbType.Char, 1, "I", , , rdo_DateType.SelectedValue & "")
                End If

                If rdo_Selection.SelectedValue <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@DateMonthFlag", SqlDbType.Char, 1, "I", , , rdo_Selection.SelectedValue & "")
                End If

                If cbo_Months.SelectedItem.Text <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@MonthText", SqlDbType.VarChar, 10, "I", , , cbo_Months.SelectedItem.Text & "")
                End If

                If Trim(Request.QueryString("Yeartext1") & "") <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@Yeartext1", SqlDbType.VarChar, 10, "I", , , cbo_Year.SelectedItem.Text & "")
                End If
                objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                .ExecuteNonQuery()
                sqlDa.SelectCommand = sqlComm
                sqlDa.Fill(dtfill)
                'Session("DebitnoteTable") = dtfill
                'dg_Debitnote.DataSource = dtfill
                'dg_Debitnote.DataBind()
            End With
            Return dtfill

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetReportTableExport", "Error in GetReportTableExport", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Function

    Protected Sub rdo_Selection_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdo_Selection.SelectedIndexChanged
        Try
            Dim month As String
            Dim year As String
            month = Trim(Today.ToString("MMMM"))
            'year = Session("YearText").ToString.Substring(0, 4)
            year = Today.Year
            Dim lstItem As ListItem = cbo_Months.Items.FindByText(month)
            If lstItem IsNot Nothing Then cbo_Months.SelectedValue = lstItem.Value
            Dim lstItemYr As ListItem = cbo_Year.Items.FindByText(year)
            If lstItemYr IsNot Nothing Then cbo_Year.SelectedValue = lstItemYr.Value
            'cbo_Months.SelectedItem.Text = month

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub dg_Debitnote_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dg_Debitnote.PageIndexChanged
        GetReportTable(, e.NewPageIndex)
    End Sub

    Protected Sub dg_Debitnote_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dg_Debitnote.SortCommand
        Try
            dg_Debitnote.CurrentPageIndex = 0
            Dim strSort As String = ""
            Dim objCookieExisting As HttpCookie = Request.Cookies.Get("SortOrder")
            If objCookieExisting Is Nothing Then
            Else
                strSort = objCookieExisting.Value
            End If
            If strSort = e.SortExpression & " Desc" Then
                strSort = e.SortExpression & " Asc"
            Else : strSort = e.SortExpression & " Desc"
            End If
            ViewState("SortOrder") = strSort
            Dim objCookie As New System.Web.HttpCookie("SortOrder", strSort)
            objCookie.Expires = #12/31/2099#
            Response.Cookies.Add(objCookie)
            GetReportTable(CType(ViewState("SortOrder"), String))
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
