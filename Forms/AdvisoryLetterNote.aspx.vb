Imports System.Data
Imports System.Data.SqlClient
Partial Class Forms_AdvisoryLetterNote
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
            'srsrh_Customer.SelectLinkButton.Enabled = True


            If IsPostBack = False Then
                Session("AdvisoryLetterRefNo") = ""
                SetAttributes()
                FillBlankDebitnoteGrids()
                Hid_DebitRefNo.Value = ""
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "show", "DateMonthSelection();", True)
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
    Private Sub FillBlankDebitnoteGrids()
        Try
            Dim dt As New DataTable
            Dim objSrh As New clsSearch
            dt.Columns.Add("WDMDealNumber", GetType(String))
            dt.Columns.Add("DealDate", GetType(String))
            dt.Columns.Add("Brokername", GetType(String))
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
        Session("ADDBrokerids") = ""
        strCond = BuildDBCustStr(srh_Broker.SelectCheckbox, srh_Broker.SelectListBox)
        Session("ADDBrokerids") = strCond
        GetReportTable()
    End Sub

    Private Function GetReportTable() As DataTable
        Try
            Dim sqlcomm As New SqlCommand
            Dim sqlda As New SqlDataAdapter
            Dim dtfill As New DataTable
            Dim DateFrom As Date
            Dim DateTo As Date
            Dim i As Integer
            Dim intimgids As String
            'Dim strusertype As String
            If txt_FromDate.Text <> "" Then
                DateFrom = txt_FromDate.Text
            End If
            If txt_ToDate.Text <> "" Then
                DateTo = txt_ToDate.Text
            End If


            With sqlcomm
                .Connection = clsCommonFuns.sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = "1000"
                strProcName = "Id_RPT_AdvisoryLatterForm"
                .CommandText = strProcName
                .Parameters.Clear()
                If txt_FromDate.Text <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@Fromdate", SqlDbType.SmallDateTime, 4, "I", , , DateFrom)
                End If
                If txt_ToDate.Text <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@Todate", SqlDbType.SmallDateTime, 4, "I", , , DateTo)
                End If
                If Trim(Session("ADDBrokerids")) <> "" Then
                    objCommon.SetCommandParameters(sqlcomm, "@Brokerid", SqlDbType.VarChar, 4000, "I", , , Trim(Session("ADDBrokerids")))
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
                Session("AdvisoryLatterTable") = dtfill
                dg_Debitnote.DataSource = dtfill
                dg_Debitnote.DataBind()
            End With
            Return dtfill

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            ' If (sqlconn.State = ConnectionState.Open) Then sqlconn.Close()
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
                Session("ADDBrokerids") = Mid(strIds, 1, Len(strIds) - 1)
            End If
            Return strCond
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Function
    Protected Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        Try
            Dim strFile As String
            Dim intLength As Int16
            Dim sqlTrans As SqlTransaction
            sqlTrans = clsCommonFuns.sqlConn.BeginTransaction

            If UpdateAdvisoryNoteRefNo(sqlTrans) = False Then Exit Sub
            sqlTrans.Commit()
            lbl_Msg.Text = "Advisory Letter NUmber Save Successfully"
            btn_Save.Visible = False

            strFile = Session("AdvisoryLetterRefNo")
            intLength = Len(strFile)
            strFile = Left(strFile, intLength - 1)
            Session("AdvisoryLetterRefNo") = strFile

            Hid_DebitRefNo.Value = ""
            Hid_ReportType.Value = "AdvisoryLatterReport"
            Response.Redirect("ViewReports.aspx?&Rpt=" & Hid_ReportType.Value, False)



        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Function UpdateAdvisoryNoteRefNo(ByVal sqlTrans As SqlTransaction) As Boolean
        Try
            Dim sqlComm As New SqlCommand
            Dim dt As DataTable
            dt = Session("AdvisoryLatterTable")
            sqlComm.Transaction = sqlTrans
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.Connection = clsCommonFuns.sqlConn
            For I As Int16 = 0 To dt.Rows.Count - 1
                If dt.Rows(I).Item("DealEntryId").ToString <> "" Then
                    sqlComm.Parameters.Clear()
                    sqlComm.CommandText = "ID_UPDATE_AdvisoryLetterDealEntry"
                    'If GetWDMDealNo(sqlTrans) = False Then Exit Function

                    objCommon.SetCommandParameters(sqlComm, "@AdvLetterDateType", SqlDbType.Char, 1, "I", , , Trim(rdo_DateType.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@AdvLetterDateMonthflag", SqlDbType.Char, 1, "I", , , Trim(rdo_Selection.SelectedValue))
                    objCommon.SetCommandParameters(sqlComm, "@AdvLetterMonths", SqlDbType.VarChar, 50, "I", , , Trim(cbo_Months.SelectedItem.Text))
                    objCommon.SetCommandParameters(sqlComm, "@AdvLetterYear", SqlDbType.VarChar, 50, "I", , , Trim(cbo_Year.SelectedItem.Text))
                    objCommon.SetCommandParameters(sqlComm, "@AdvLetterFromDate", SqlDbType.SmallDateTime, 8, "I", , , Trim(txt_FromDate.Text))
                    objCommon.SetCommandParameters(sqlComm, "@AdvLetterToDate", SqlDbType.SmallDateTime, 8, "I", , , Trim(txt_ToDate.Text))


                    objCommon.SetCommandParameters(sqlComm, "@TransType", SqlDbType.Char, 1, "I", , , Trim(TryCast(dg_Debitnote.Items(I).FindControl("lbl_TransType"), Label).Text))
                    objCommon.SetCommandParameters(sqlComm, "@RefNo", SqlDbType.VarChar, 50, "I", , , Trim(TryCast(dg_Debitnote.Items(I).FindControl("lbl_RefNo"), Label).Text))
                    objCommon.SetCommandParameters(sqlComm, "@DealEntryId", SqlDbType.BigInt, 16, "I", , , Trim(TryCast(dg_Debitnote.Items(I).FindControl("lbl_DealEntryId"), Label).Text))
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

            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                'WdmdealFlag = TryCast(e.Item.FindControl("lbl_WdmDeal"), Label).Text
                'BrokerId = TryCast(e.Item.FindControl("lbl_BrokerId"), Label).Text
                TransType = TryCast(e.Item.FindControl("lbl_TransType"), Label).Text
                If Val(intPrevDebitNote) = 0 Then
                    GetDebitRefNo()
                End If

                If strPrevCustName <> TryCast(e.Item.FindControl("lbl_Brokername"), Label).Text Then
                    intPrevDebitNote = intPrevDebitNote + 1
                    strRefNo = GetRefNo()
                    Session("AdvisoryLetterRefNo") = Session("AdvisoryLetterRefNo") + strRefNo + ","
                Else
                    strRefNo = strPrevRefNo
                End If
                TryCast(e.Item.FindControl("lbl_RefNo"), Label).Text = strRefNo
                strPrevRefNo = strRefNo
                strPrevCustName = TryCast(e.Item.FindControl("lbl_Brokername"), Label).Text
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
            OpenSqlConn()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandText = "ID_Get_AdvisoryLetterRefNo"
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

End Class
