Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports RKLib.ExportData
Imports ClosedXML.Excel
Partial Class Forms_CRM_Reports
    Inherits System.Web.UI.Page
    Dim sqlconn As SqlConnection
    Dim objCommon As New clsCommonFuns
   
    Dim date1 As DateTime
    Dim date2 As DateTime
    Dim Fvinlacs As Double = 0
    Dim Settamt As Double = 0
    Dim Brokerage As Double = 0
    Dim chekbox As CheckBox
    Dim rptDoc As New ReportDocument

    Dim strFilePath As String
    Dim dtRpt As DataTable
    Dim ScriptOpenModalDialog As String = "javascript:OpenModalDialog('{0}','{1}','{2}');"
    Dim objMIS As New MISReports

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            If IsPostBack = False Then
                txt_FromDate.Text = Format(DateAndTime.Today, "dd/MM/yyyy")
                txt_FromDate.Attributes.Add("onkeypress", "OnlyDate();")

                txt_FromDate.Attributes.Add("onblur", "CheckDate(this,false);")


                txt_ToDate.Attributes.Add("onkeypress", "OnlyDate();")
                txt_ToDate.Attributes.Add("onblur", "CheckDate(this,false);")
                txt_ToDate.Text = Format(DateAdd(DateInterval.Day, 1, Today), "dd/MM/yyyy")
                Hid_ReportType.Value = Trim(Request.QueryString("Rpt") & "")
                srh_CustomerName.SelectLinkButton.Enabled = True
                srh_Staff.SelectLinkButton.Enabled = True
                If Hid_ReportType.Value = "CRMEntry" Then
                    row_customer.Visible = True
                    row_customer2.Visible = True
                End If
                If Hid_ReportType.Value = "CRM_MeetingDetails" Then
                    row_customer.Visible = True
                    row_Staff.Visible = True
                    row_customer2.Visible = True

                End If

            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub OpenConn()
        If sqlconn Is Nothing Then
            sqlconn = New SqlConnection(ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString())
            sqlconn.Open()
        ElseIf sqlconn.State = ConnectionState.Closed Then
            sqlconn.ConnectionString = ConfigurationManager.ConnectionStrings("InstadealConnectionString").ToString()
            sqlconn.Open()
        End If
    End Sub

    Private Sub CloseConn()
        If sqlconn Is Nothing Then Exit Sub
        If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
    End Sub
    Private Sub BuildReportCond()
        Try
            Dim strCond As String = ""
            Dim strRpt As String
            strRpt = Hid_ReportType.Value
            Session("strWhereClause_crm") = ""

            If Hid_ReportType.Value = "CRMEntry" Then
                strCond += BuildConditionStr(srh_CustomerName.SelectCheckbox, srh_CustomerName.SelectListBox, "CM.CustomerId")
                strCond += BuildConditionStr(srh_CustomerTypeName.SelectCheckbox, srh_CustomerTypeName.SelectListBox, "CTM.CustomerTypeId")
            ElseIf Hid_ReportType.Value = "CRM_MeetingDetails" Then
                strCond += BuildConditionStr(srh_CustomerName.SelectCheckbox, srh_CustomerName.SelectListBox, "CM.CustomerId")
                strCond += BuildConditionStr(srh_CustomerTypeName.SelectCheckbox, srh_CustomerTypeName.SelectListBox, "CTM.CustomerTypeId")
                strCond += BuildConditionStr(srh_Staff.SelectCheckbox, srh_Staff.SelectListBox, "UM.UserId")
            End If
            Session("strWhereClause_crm") = strCond
        Catch ex As Exception

        End Try
    End Sub
    Private Function BuildConditionStr(ByVal chkbox As CheckBox, ByVal lstbox As ListBox, ByVal strFieldName As String, Optional ByVal chrFlag As Char = "") As String
        Try
            Dim strCond As String = ""
            Dim strOpt As String '= " WHERE "

            If Hid_ReportType.Value = "CustomerInfo" Or Hid_ReportType.Value = "CustomerCtc" Then
                strOpt = " WHERE "
            Else
                strOpt = " AND "
            End If
            If chkbox.Checked = False Then
                If strCond <> "" Then strOpt = " AND "
                If Hid_ReportType.Value = "StaffwiseTrans" Or Hid_ReportType.Value = "StaffwiseTransP" Then
                    strCond = strOpt & strFieldName & " IN ("
                Else
                    strCond = strOpt & strFieldName & " IN ("
                End If

                For I As Int16 = 0 To lstbox.Items.Count - 1
                    If lstbox.Items(I).Value <> "" Then
                        strCond += lstbox.Items(I).Value & ","
                    End If
                Next
                strCond = Mid(strCond, 1, Len(strCond) - 1) & ")"
            End If
            Return strCond
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Function

    Protected Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Try
            If Hid_ReportType.Value = "CRMEntry" Then
                BuildReportCond()
                dtRpt = GetReportTable("CRM_RPT_EXCEL")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MISCRM(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('No records found.');", True)
                End If
            End If
            If Hid_ReportType.Value = "CRM_MeetingDetails" Then
                BuildReportCond()
                dtRpt = GetReportTable("CRM_FILL_MEETINGDETAILS")
                If dtRpt.Rows.Count > 0 Then
                    objMIS.ExportToExcel_MeetingDetails(dtRpt, txt_FromDate.Text, txt_ToDate.Text)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType(), "AlertMessageBox", "alert('No records found.');", True)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Function GetReportTable(ByVal strProcName As String) As DataTable
        Try
            Dim datFrom As Date
            Dim datTo As Date
            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand
            sqlComm.CommandTimeout = 0
            OpenConn()

            datFrom = objCommon.DateFormat(txt_FromDate.Text)
            datTo = objCommon.DateFormat(txt_ToDate.Text)

            With sqlComm
                .Connection = sqlconn
                .CommandType = CommandType.StoredProcedure
                .CommandText = strProcName

                If Session("strWhereClause_crm") <> "" Then
                    objCommon.SetCommandParameters(sqlComm, "@Cond", SqlDbType.VarChar, 1000, "I", , , Session("strWhereClause_crm"))
                End If

                objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)
                objCommon.SetCommandParameters(sqlComm, "@ToDate", SqlDbType.SmallDateTime, 8, "I", , , datTo)
                .ExecuteNonQuery()
            End With
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dtfill)

            Return dtfill
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try
    End Function
End Class
