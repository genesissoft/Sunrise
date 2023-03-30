Imports DocumentFormat.OpenXml
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

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
    Dim objUtil As New Util
    Dim PgName As String = "$RetailDebitNote$"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'DivGrid.Attributes.Add("onscroll", "setonScroll	(this,""" & dg_Debitnote.ClientID & """);")
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
            'srh_Customer.SelectLinkButton.Enabled = True
            'srh_Broker.SelectLinkButton.Enabled = True
            srh_Customer.SelectCheckBox.Checked = False
            rdo_DateType.SelectedValue = "S"
            rdo_DateType.Enabled = False

            Hid_UserTypeId.Value = Val(Session("UserTypeId"))
            Hid_UserId.Value = Val(Session("UserId"))



            If IsPostBack = False Then
                Session("DebitLetterRefNo") = ""
                Session("RetailDebitRefNo") = ""
                Session("RetDebitRefNo") = ""
                Session("ADDCustids") = ""
                Session("ADDBrokerids") = ""
                Hid_Intids.Value = ""
                Hid_DealSlipIds.Value = ""
                Hid_TransType.Value = ""
                SetAttributes()

                FillBlankDebitnoteGrids()
                Hid_DebitRefNo.Value = ""
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "show", "DateMonthSelection();", True)
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
            dt.Columns.Add("DealSlipId", GetType(String))
            dt.Columns.Add("DealSlipno", GetType(String))
            dt.Columns.Add("Brokername", GetType(String))
            dt.Columns.Add("CustomerName", GetType(String))
            dt.Columns.Add("SecurityName", GetType(String))
            dt.Columns.Add("DealDate", GetType(String))
            dt.Columns.Add("BrokerageAmt", GetType(String))
            dt.Columns.Add("BrokerId", GetType(String))
            dt.Columns.Add("TransType", GetType(String))
            dg_Debitnote.DataSource = dt
            dg_Debitnote.DataBind()
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillBlankDebitnoteGrids", "Error in FillBlankDebitnoteGrids", "", ex)

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub SetAttributes()

        txt_FromDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
        txt_FromDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_FromDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_ToDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_ToDate.Attributes.Add("onblur", "CheckDate(this,false);")
        txt_ToDate.Text = Format(DateAdd(DateInterval.Day, 1, Today), "dd/MM/yyyy")
        btn_Save.Attributes.Add("onclick", "return  Validation();")

        txt_DebitDate.Attributes.Add("onkeypress", "OnlyDate();")
        txt_DebitDate.Attributes.Add("onblur", "CheckDate(this,false);")
        btn_Show.Attributes.Add("onclick", "return showvalidation();")

    End Sub

    Protected Sub btn_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Show.Click
        Dim strCond As String = ""
        Dim strRpt As String
        'strRpt = hid_ReportType.Value

        Session("RetDebitRefNo") = ""
        Session("RetailDebitRefNo") = ""
        Session("BrokeridsADD") = ""
        Hid_Intids.Value = ""
        Hid_DealSlipIds.Value = ""
        Hid_TransType.Value = ""

        strCond += BuildConditionStr(srh_Customer.SelectCheckBox, srh_Customer.SelectListBox, "BM.BrokerId")
        Session("ADDBrokerids") = strCond
        GetReportTable()
        If dg_Debitnote.Items.Count > 0 Then
            row_Export.Visible = True
            btn_Save.Visible = True
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "NoRecordsFound();", True)
        End If
    End Sub

    Private Function GetReportTable() As DataTable
        Try
            OpenConn()
            Dim datFrom As Date
            Dim datTo As Date
            Dim DebitDate As Date
            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand

            'Dim intColIndex As Int16

            datFrom = objCommon.DateFormat(txt_FromDate.Text)
            datTo = objCommon.DateFormat(txt_ToDate.Text)
            DebitDate = objCommon.DateFormat(txt_DebitDate.Text)
            'openconnection()

            With sqlComm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = "0"

                strProcName = "Id_FILL_DebitNoteDetails"


                    .CommandText = strProcName
                .Parameters.Clear()
                If txt_FromDate.Text <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)
                End If
                If txt_ToDate.Text <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@ToDate", SqlDbType.SmallDateTime, 8, "I", , , datTo)
                End If

                If Trim(Session("ADDBrokerids")) <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@BrokerID", SqlDbType.VarChar, 4000, "I", , , Trim(Session("ADDBrokerids")))
                End If
                If Trim(Session("ADDCustids")) <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@CustomerId", SqlDbType.VarChar, 4000, "I", , , Trim(Session("ADDCustids")))
                End If
                If rdo_DateType.SelectedValue <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@DateTypeFlag", SqlDbType.Char, 1, "I", , , rdo_DateType.SelectedValue & "")
                End If
                objCommon.SetCommandParameters(sqlComm, "@DateMonthFlag", SqlDbType.Char, 1, "I", , , "D")

                objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                .ExecuteNonQuery()
                sqlDa.SelectCommand = sqlComm
                sqlDa.Fill(dtfill)
                Session("RetaildebitTable") = dtfill
                dg_Debitnote.DataSource = dtfill
                dg_Debitnote.DataBind()
            End With
            Return dtfill

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetReportTable", "Error in GetReportTable", "", ex)

            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try

    End Function
    Private Function BuildConditionStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox, ByVal strFieldName As String, Optional ByVal chrFlag As Char = "") As String
        Try

            ' BuildConditionStr(srh_CustomerName.SelectCheckBox, srh_CustomerName.SelectListBox, "CM.CustomerId")
            Dim strCond As String = ""
            Dim strOpt As String '= " WHERE "

            If hid_ReportType.Value = "CustomerInfo" Or hid_ReportType.Value = "CustomerCtc" Then
                strOpt = " WHERE "
            Else
                strOpt = " AND "
            End If
            If chkbox.Checked = True Then
                srh_Customer.SelectListBox.Items.Clear()
            End If
            If chkbox.Checked = False Then
                If strCond <> "" Then strOpt = " AND "
                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strCond += lstbox.Items(I).Value & ","
                    End If
                Next
                If strCond <> "" Then
                    'strCond = "(" & Mid(strCond, 1, Len(strCond) - 1) & ")"
                    strCond = Mid(strCond, 1, Len(strCond) - 1)
                Else
                    strCond = strCond
                End If

                'If Val(Session("UserTypeId")) <> 1 Then
                '    strCond = (Session("UserId"))
                'Else
                '    If strCond <> "" Then
                '        strCond = Mid(strCond, 1, Len(strCond) - 1)
                '    Else
                '        strCond = strCond
                '    End If

                'End If

            End If
            Return strCond
        Catch ex As Exception
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
            SaveDebitDetails()
            'If UpdateDealDetails(sqlTrans) = False Then Exit Sub

            lbl_Msg.Text = "Debit Note details Saved Successfully"
            btn_Save.Visible = False
            strFile = Session("RetailDebitRefNo")
            intLength = Len(strFile)
            strFile = Left(strFile, intLength - 1)
            Session("RetailDebitRefNo") = strFile
            Hid_DebitRefNo.Value = ""
            hid_ReportType.Value = "RetailDebitRpt"
            Hid_ReptForm.Value = "RetailDebitForm"

            GetReportTable()
            'Response.Redirect("ViewReports.aspx?&Rpt=" & hid_ReportType.Value & "&RptForm=" & Hid_ReptForm.Value & "&DealTransType=" & "T" & "&Fromdate=" & txt_FromDate.Text & "&Todate=" & txt_ToDate.Text, False)
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "btn_Save_Click", "Error in btn_Save_Click", "", ex)

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function UpdateDealDetails(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            Dim CurrentYear As String
            If DateTime.Today.Month <= 3 Then
                CurrentYear = DateTime.Today.Year.ToString()
                CurrentYear = CurrentYear - 1
            Else
                CurrentYear = DateTime.Today.Year.ToString()
            End If
            Dim DebitDate As Date
            DebitDate = objCommon.DateFormat(txt_DebitDate.Text)
            dt = Session("RetaildebitTable")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("DistributorId").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    sqlComm.CommandText = "ID_UPDATE_RetailNoteDeal"
                    objCommon.SetCommandParameters(sqlComm, "@DebtDateType", SqlDbType.Char, 1, "I", , , Trim(rdo_DateType.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@DebtDateMonthflag", SqlDbType.Char, 1, "I", , , "D")
                    objCommon.SetCommandParameters(sqlComm, "@DebtMonths", SqlDbType.VarChar, 10, "I", , , "")
                    objCommon.SetCommandParameters(sqlComm, "@DebtYear", SqlDbType.VarChar, 50, "I", , , CurrentYear)
                    objCommon.SetCommandParameters(sqlComm, "@BrokerageAmount", SqlDbType.Decimal, 9, "I", , , Val(dt.Rows(I).Item("BrokerageAmt")))
                    objCommon.SetCommandParameters(sqlComm, "@DebtFromDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_FromDate.Text))
                    objCommon.SetCommandParameters(sqlComm, "@DebtToDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ToDate.Text))
                    objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , "T")
                    objCommon.SetCommandParameters(sqlComm, "@RefNo", SqlDbType.VarChar, 50, "I", , , Trim(TryCast(dg_Debitnote.Items(I).FindControl("lbl_RefNo"), Label).Text))
                    objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.Int, 4, "I", , , Trim(TryCast(dg_Debitnote.Items(I).FindControl("lbl_DistributorId"), Label).Text))
                End If
                objCommon.SetCommandParameters(sqlComm, "@DebitCredit", SqlDbType.Char, 1, "I", , , "D")
                objCommon.SetCommandParameters(sqlComm, "@DebitDate", SqlDbType.SmallDateTime, 8, "I", , , DebitDate)
                objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
                objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                sqlComm.ExecuteNonQuery()

            Next
            Return True
        Catch ex As Exception
            sqlTrans.Rollback()
            objUtil.WritErrorLog(PgName, "UpdateDealDetails", "Error in UpdateDealDetails", "", ex)

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Function
    Private Sub SaveDebitDetails()
        Try

            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            Dim CurrentYear As String
            Dim strRefNo As String
            If DateTime.Today.Month <= 3 Then
                CurrentYear = DateTime.Today.Year.ToString()
                CurrentYear = CurrentYear - 1
            Else
                CurrentYear = DateTime.Today.Year.ToString()
            End If
            dt = Session("RetaildebitTable")
            Dim strIds As Array
            Dim strCustIds As Array
            Dim strtranstype As Array
            strIds = Split(Hid_DealSlipIds.Value, ",")
            strCustIds = Split(Hid_Intids.Value, ",")
            strtranstype = Split(Hid_TransType.Value, ",")
            Dim SameDeal As Integer
            Dim DebitDate As Date
            DebitDate = objCommon.DateFormat(txt_DebitDate.Text)
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            sqlComm.CommandTimeout = 100000

            Session("RetailDebitRefNo") = ""
            For K As Int16 = 0 To dt.Rows.Count - 1
                For I As Int16 = 0 To strIds.Length - 1
                    If Val(strIds(I)) <> 0 Then
                        If Val(strIds(I)) = TryCast(dg_Debitnote.Items(K).FindControl("lbl_DealSlipId"), Label).Text Then

                            GetDebitRefNo()

                            If strPrevCustName <> TryCast(dg_Debitnote.Items(K).FindControl("lbl_Brokername"), Label).Text Then
                                intPrevDebitNote = intPrevDebitNote + 1
                                strRefNo = GetRefNo()
                                strRefNo.Remove(strRefNo.Length - 1, 1)
                                Session("RetailDebitRefNo") = Session("RetailDebitRefNo") + strRefNo + ","
                                SameDeal = 0
                            Else
                                strRefNo = strPrevRefNo
                                SameDeal = 1
                            End If

                            strPrevRefNo = strRefNo
                            strPrevCustName = TryCast(dg_Debitnote.Items(K).FindControl("lbl_Brokername"), Label).Text
                            sqlComm.Parameters.Clear()
                            sqlComm.CommandText = "ID_UPDATE_DebitNoteDealEntry"

                            objCommon.SetCommandParameters(sqlComm, "@DebtDateType", SqlDbType.Char, 1, "I", , , "D")
                            objCommon.SetCommandParameters(sqlComm, "@DebtDateMonthflag", SqlDbType.Char, 1, "I", , , "D")
                            objCommon.SetCommandParameters(sqlComm, "@DebtMonths", SqlDbType.VarChar, 10, "I", , , "")
                            objCommon.SetCommandParameters(sqlComm, "@DebtYear", SqlDbType.VarChar, 50, "I", , , CurrentYear)
                            objCommon.SetCommandParameters(sqlComm, "@DebtFromDate", SqlDbType.SmallDateTime, 8, "I", , , objCommon.DateFormat(txt_FromDate.Text))
                            objCommon.SetCommandParameters(sqlComm, "@DebtToDate", SqlDbType.SmallDateTime, 8, "I", , , objCommon.DateFormat(txt_ToDate.Text))
                            objCommon.SetCommandParameters(sqlComm, "@TransType", SqlDbType.Char, 1, "I", , , Trim(TryCast(dg_Debitnote.Items(K).FindControl("lbl_TransType"), Label).Text))
                            objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.BigInt, 16, "I", , , Trim(TryCast(dg_Debitnote.Items(K).FindControl("lbl_BrokerId"), Label).Text))
                            objCommon.SetCommandParameters(sqlComm, "@RefNo", SqlDbType.VarChar, 50, "I", , , strRefNo)
                            objCommon.SetCommandParameters(sqlComm, "@DealslipId", SqlDbType.BigInt, 16, "I", , , strIds(I))
                            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
                            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
                            objCommon.SetCommandParameters(sqlComm, "@SameDeal", SqlDbType.Int, 0, "I", , , SameDeal)
                            objCommon.SetCommandParameters(sqlComm, "@DebitDate", SqlDbType.SmallDateTime, 8, "I", , , DebitDate)
                            objCommon.SetCommandParameters(sqlComm, "@MaxNo", SqlDbType.VarChar, 500, "O")
                            sqlComm.ExecuteNonQuery()
                        End If
                    End If
                Next
            Next

        Catch ex As Exception

            objUtil.WritErrorLog(PgName, "SaveDebitDetails", "Error in SaveDebitDetails", "", ex)

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            sqlConn.Close()
        End Try
    End Sub

    Protected Sub dg_Debitnote_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_Debitnote.ItemDataBound
        Try
            Dim intDealSlipID As Integer
            Dim intBrokerId As Integer

            Dim chkBoxHead As CheckBox
            Dim chkBox As CheckBox
            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                e.Item.ID = "itm" & e.Item.ItemIndex
                e.Item.Cells(0).CssClass = "locked"
                intBrokerId = Val(CType(e.Item.FindControl("lbl_BrokerId"), Label).Text)
                intDealSlipID = Val(CType(e.Item.FindControl("lbl_DealSlipId"), Label).Text)
                chkBox = CType(e.Item.FindControl("chk_ItemChecked"), CheckBox)

                Hid_Intids.Value += Val(CType(e.Item.FindControl("lbl_BrokerId"), Label).Text) & "!"
                    Hid_DealSlipIds.Value += Val(CType(e.Item.FindControl("lbl_DealSlipId"), Label).Text) & "!"
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

    Private Function GetDebitRefNo()
        Try
            OpenSqlConn()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_Get_RetailDebitRefNo"
            sqlComm.Parameters.Clear()
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
            objCommon.SetCommandParameters(sqlComm, "@DebitCredit", SqlDbType.Char, 1, "I", , , "D")
            objCommon.SetCommandParameters(sqlComm, "@RetDealType", SqlDbType.Char, 1, "I", , , "T")
            objCommon.SetCommandParameters(sqlComm, "@MaxDebitNo", SqlDbType.Int, 4, "O")
            sqlComm.ExecuteNonQuery()
            intPrevDebitNote = Val(sqlComm.Parameters("@MaxDebitNo").Value & "")

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "GetDebitRefNo", "Error in GetDebitRefNo", "", ex)

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

            Return False
        Finally
            CloseSqlConn()

        End Try
        Return True
    End Function
    Private Sub CloseSqlConn()
        If sqlConn Is Nothing Then Exit Sub
        If sqlConn.State = ConnectionState.Open Then sqlConn.Close()
    End Sub
    Private Sub OpenSqlConn()
        sqlConn = New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
        sqlConn.Open()
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
