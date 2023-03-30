Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_DebitNoteNormal
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
            srh_Customer.SelectLinkButton.Enabled = True

            If IsPostBack = False Then
                Session("AdvisoryLetterRefNo") = ""
                Session("AdvisoryRefNo") = ""
                Session("DebitRefNoNormal") = ""
                SetAttributes()
                FillBlankDebitnoteGrids()
                Hid_DebitRefNo.Value = ""
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "show", "DateMonthSelection();", True)
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

    Private Sub FillBlankDebitnoteGrids()
        Try
            Dim dt As New DataTable
            Dim objSrh As New clsSearch
            dt.Columns.Add("DealNumber", GetType(String))
            dt.Columns.Add("DealDate", GetType(String))
            dt.Columns.Add("Customername", GetType(String))
            dt.Columns.Add("SecurityName", GetType(String))
            dg_Debitnote.DataSource = dt
            dg_Debitnote.DataBind()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
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
    End Sub

    Protected Sub btn_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Show.Click
        Dim strCond As String = ""
        Dim strRpt As String
        'strRpt = Hid_ReportType.Value
        If rdo_Selection.SelectedValue = "M" Then
            Getdate()
        End If
        Session("DebitRefNoNormal") = ""
        Session("CustomerIds") = ""
        strCond = BuildDBCustStr(srh_Customer.SelectCheckbox, srh_Customer.SelectListBox)
        Session("CustomerIds") = strCond
        GetReportTable()
    End Sub

    Private Function GetReportTable() As DataTable
        Try
            OpenConn()
            Dim sqlcomm As New SqlCommand
            Dim sqlda As New SqlDataAdapter
            Dim dtfill As New DataTable
            Dim DateFrom As Date
            Dim DateTo As Date
            Dim i As Integer
            Dim intimgids As String
            'Dim strusertype As String
            'If txt_FromDate.Text <> "" Then
            '    DateFrom = txt_FromDate.Text
            'End If
            'If txt_ToDate.Text <> "" Then
            '    DateTo = txt_ToDate.Text
            'End If
            DateFrom = objCommon.DateFormat(txt_FromDate.Text)
            DateTo = objCommon.DateFormat(txt_ToDate.Text)

            With sqlcomm
                .Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = "1000"
                strProcName = "Id_RPT_WDMdebtnoteFormNormal"
                .CommandText = strProcName
                .Parameters.Clear()
                If txt_FromDate.Text <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@Fromdate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_FromDate.Text))
                End If
                If txt_ToDate.Text <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@Todate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_ToDate.Text))
                End If
                If Trim(Session("CustomerIds")) <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@CustomerIds", SqlDbType.VarChar, 4000, "I", , , Trim(Session("CustomerIds")))
                End If
                If rdo_DateType.SelectedValue <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@DateTypeFlag", SqlDbType.Char, 1, "I", , , rdo_DateType.SelectedValue & "")
                End If
                If rdo_Selection.SelectedValue <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@DateMonthFlag", SqlDbType.Char, 1, "I", , , rdo_Selection.SelectedValue & "")
                End If

                If cbo_Months.SelectedItem.Text <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@MonthText", SqlDbType.VarChar, 10, "I", , , cbo_Months.SelectedItem.Text & "")
                End If

                If Trim(Request.QueryString("Yeartext1") & "") <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@Yeartext1", SqlDbType.VarChar, 10, "I", , , cbo_Year.SelectedItem.Text & "")
                End If
                objCommon.SetCommandParameters(sqlcomm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                objCommon.SetCommandParameters(sqlcomm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                .ExecuteNonQuery()
                sqlda.SelectCommand = sqlcomm
                sqlda.Fill(dtfill)
                Session("DebitnoteTableNormal") = dtfill
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
                strCond += "    " & Mid(strIds, 1, Len(strIds) - 1) & " "
                Session("CustomerIds") = Mid(strIds, 1, Len(strIds) - 1)
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
            sqlTrans = sqlConn.BeginTransaction

            If UpdateDebitNoteRefNo(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()
            lbl_Msg.Text = "Debit Note Number Save Successfully"
            btn_Save.Visible = False
            strFile = Session("DebitRefNoNormal")
            intLength = Len(strFile)
            strFile = Left(strFile, intLength - 1)
            Session("DebitRefNoNormal") = strFile
            Hid_DebitRefNo.Value = ""
            Hid_ReportType.Value = "DebitNoteNormal"
            Response.Redirect("ViewReports.aspx?&Rpt=" & Hid_ReportType.Value, False)

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try

    End Sub

    Private Function UpdateDebitNoteRefNo(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            dt = Session("DebitnoteTableNormal")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = sqlConn
            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("DealEntryId").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    sqlComm.CommandText = "ID_UPDATE_DebitNoteDealEntryNormal"
                    'If GetWDMDealNo(sqlTrans) = False Then Exit Function

                    objCommon.SetCommandParameters(sqlComm, "@DebtDateType", SqlDbType.Char, 1, "I", , , Trim(rdo_DateType.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@DebtDateMonthflag", SqlDbType.Char, 1, "I", , , Trim(rdo_Selection.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@DebtMonths", SqlDbType.VarChar, 50, "I", , , Trim(cbo_Months.SelectedItem.Text))
                    objCommon.SetCommandParameters(sqlComm, "@DebtYear", SqlDbType.VarChar, 50, "I", , , Trim(cbo_Year.SelectedItem.Text))
                    objCommon.SetCommandParameters(sqlComm, "@DebtFromDate", SqlDbType.SmallDateTime, 8, "I", , , objCommon.DateFormat(txt_FromDate.Text))
                    objCommon.SetCommandParameters(sqlComm, "@DebtToDate", SqlDbType.SmallDateTime, 8, "I", , , objCommon.DateFormat(txt_ToDate.Text))


                    objCommon.SetCommandParameters(sqlComm, "@TransType", SqlDbType.Char, 1, "I", , , Trim(TryCast(dg_Debitnote.Items(I).FindControl("lbl_TransType"), Label).Text))
                    'objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.BigInt, 16, "I", , , Trim(TryCast(dg_Debitnote.Items(I).FindControl("lbl_BrokerId"), Label).Text))
                    objCommon.SetCommandParameters(sqlComm, "@RefNo", SqlDbType.VarChar, 50, "I", , , Trim(TryCast(dg_Debitnote.Items(I).FindControl("lbl_RefNo"), Label).Text))
                    objCommon.SetCommandParameters(sqlComm, "@DealEntryId", SqlDbType.BigInt, 16, "I", , , Val(TryCast(dg_Debitnote.Items(I).FindControl("lbl_DealEntryId"), Label).Text))
                    objCommon.SetCommandParameters(sqlComm, "@WdmDeal", SqlDbType.Char, 1, "I", , , Trim(TryCast(dg_Debitnote.Items(I).FindControl("lbl_WdmDeal"), Label).Text))
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

    Protected Sub dg_Debitnote_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_Debitnote.ItemDataBound
        Try

            Dim strRefNo As String
            Dim strRefNo1 As String
            'Hid_DebitRefNo.Value = ""
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                WdmdealFlag = TryCast(e.Item.FindControl("lbl_WdmDeal"), Label).Text
                'BrokerId = TryCast(e.Item.FindControl("lbl_BrokerId"), Label).Text
                'TransType = TryCast(e.Item.FindControl("lbl_TransType"), Label).Text
                If Val(intPrevDebitNote) = 0 Then
                    GetDebitRefNo()
                End If

                If strPrevCustName <> TryCast(e.Item.FindControl("lbl_Customername"), Label).Text Then
                    intPrevDebitNote = intPrevDebitNote + 1
                    strRefNo = GetRefNo()
                    strRefNo.Remove(strRefNo.Length - 1, 1)
                    Session("DebitRefNoNormal") = Session("DebitRefNoNormal") + strRefNo + ","
                Else
                    strRefNo = strPrevRefNo
                End If



                TryCast(e.Item.FindControl("lbl_RefNo"), Label).Text = strRefNo
                strPrevRefNo = strRefNo
                strPrevCustName = TryCast(e.Item.FindControl("lbl_Customername"), Label).Text



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

    Private Function GetDebitRefNo()
        Try
            OpenConn()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_Get_DebitRefNoNormal"
            sqlComm.Parameters.Clear()
            'objCommon.SetCommandParameters(sqlComm, "@TransType", SqlDbType.Char, 1, "I", , , TransType)
            'objCommon.SetCommandParameters(sqlComm, "@BrokerId", SqlDbType.Char, 1, "I", , , BrokerId)
            'objCommon.SetCommandParameters(sqlComm, "@WdmDeal", SqlDbType.Char, 1, "I", , , WdmdealFlag)
            objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId")))
            objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId")))
            objCommon.SetCommandParameters(sqlComm, "@MaxNo", SqlDbType.Int, 4, "O")

            sqlComm.ExecuteNonQuery()
            intPrevDebitNote = Val(sqlComm.Parameters("@MaxNo").Value & "")
            Return True
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)

            Return False
        Finally
            CloseConn()
        End Try
    End Function

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
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub rdo_Selection_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdo_Selection.SelectedIndexChanged

        Try
            Dim month As String
            Dim test As String

            month = Trim(Today.ToString("MMMM"))

            Dim lstItem As ListItem = cbo_Months.Items.FindByText(month)
            If lstItem IsNot Nothing Then cbo_Months.SelectedValue = lstItem.Value


            'cbo_Months.SelectedItem.Text = month

        Catch ex As Exception

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
End Class
