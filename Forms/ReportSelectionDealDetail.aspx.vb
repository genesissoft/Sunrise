Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports RKLib.ExportData
Imports System.IO
Imports log4net
Partial Class Forms_ReportSelectionDealDetail
    Inherits System.Web.UI.Page
    Dim objCommon As New clsCommonFuns
    Dim PgName As String = "$ReportSelectionDealDetail$"
    Dim sqlConn As SqlConnection
    Dim rptDoc As New ReportDocument
    Dim strRptName As String
    Dim objUtil As New Util

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Val(Session("UserId") & "") = 0 Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If
            'If  sqlConn Is Nothing Then objCommon.OpenConn()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            'Response.CacheControl = "no-cache"
            'Response.AddHeader("Pragma", "no-cache")
            'Response.AddHeader("Cache-Control", "no-cache")
            'Response.AddHeader("Cache-Control", "no-store")
            rdo_select.Items(0).Attributes.Add("onclick", "HidRow();")
            rdo_select.Items(1).Attributes.Add("onclick", "HidRow();")
            rdo_select.Items(2).Attributes.Add("onclick", "HidRow();")
            rdo_select.Items(3).Attributes.Add("onclick", "HidRow();")
            Hid_UserId.Value = Val(Session("UserId") & "")
            Hid_UserTypeId.Value = Val(Session("UserTypeId") & "")
            If IsPostBack = False Then
                SetAttributes()
                setcontrols()
                FillGrid()
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "hid", "HidRow();", True)
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

    Private Sub setcontrols()
        'Srh_Customer.Columns.Add("CustomerName")
        'Srh_Customer.Columns.Add("CustomerId")
        'Srh_DealSlipNo.Columns.Add("DealSlipNo")
        'Srh_DealSlipNo.Columns.Add("CONVERT(VARCHAR,DealDate,103) As DealDate")
        'Srh_DealSlipNo.Columns.Add("DealDate")
        'Srh_DealSlipNo.Columns.Add("DealSlipId")
        'Srh_security.Columns.Add("SecurityName")

        'Srh_security.Columns.Add("SecurityId")
        'srh_dealdate.Columns.Add("DealDate")
        'srh_dealdate.Columns.Add("DealSlipId")
    End Sub
    Private Sub SetAttributes()
        Try
            Page.SetFocus(btn_Show)
            txt_Date.Attributes.Add("onkeypress", "OnlyDate();")
            txt_Date.Text = Today.ToString("dd/MM/yyyy")
            txt_Date.Attributes.Add("onblur", "CheckDate(this,false);")
            'btn_Save.Attributes.Add("onclick", "return  Validation();")
            btn_Show.Attributes.Add("onclick", "return  validation();")
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "SetAttributes", "Error in SetAttributes", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Protected Sub btn_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Show.Click
        FillGrid()
        If (dg1.Items.Count = 0) Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('No record Found.');", True)
        End If
    End Sub
    Private Function FillGrid()
        Try
            Dim sqlcomm As New SqlCommand
            Dim sqlda As New SqlDataAdapter
            Dim dtfill As New DataTable
            Dim dvfill As New DataView
            OpenConn()
            With sqlcomm
                sqlcomm.Connection = sqlConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "ID_Fill_DealDetailSelection"
                .Parameters.Clear()
                If (rdo_select.SelectedValue = "DealDate") Then
                    If (txt_Date.Text) <> "" Then
                        objCommon.SetCommandParameters(sqlcomm, "@DealDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_Date.Text))
                    End If
                ElseIf (rdo_select.SelectedValue = "DealSlipNo") Then
                    objCommon.SetCommandParameters(sqlcomm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(Srh_DealSlipNo.SelectedId))
                ElseIf (rdo_select.SelectedValue = "CustomerName") Then
                    objCommon.SetCommandParameters(sqlcomm, "@CustomerId", SqlDbType.Int, 4, "I", , , Val(Srh_Customer.SelectedId))
                ElseIf (rdo_select.SelectedValue = "SecurityName") Then
                    objCommon.SetCommandParameters(sqlcomm, "@SecurityId", SqlDbType.Int, 4, "I", , , Val(Srh_security.SelectedId))
                End If
                'objCommon.SetCommandParameters(sqlcomm, "@FromDate", SqlDbType.SmallDateTime, 4, "I", , , objCommon.DateFormat(txt_Date.Text))
                objCommon.SetCommandParameters(sqlcomm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                objCommon.SetCommandParameters(sqlcomm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                objCommon.SetCommandParameters(sqlcomm, "@UserTypeId", SqlDbType.Int, 4, "I", , , Val(Session("UserTypeId") & ""))
                objCommon.SetCommandParameters(sqlcomm, "@UserId", SqlDbType.Int, 4, "I", , , Val(Session("UserId") & ""))
                .ExecuteNonQuery()
                sqlda.SelectCommand = sqlcomm
                sqlda.Fill(dtfill)
                dvfill = dtfill.DefaultView
                dg1.DataSource = dvfill
                dg1.DataBind()
            End With

        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "FillGrid", "Error in FillGrid", "", ex)
            Response.Write(ex.Message)
        Finally
            CloseConn()
        End Try

    End Function

    Protected Sub dg1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg1.ItemDataBound
        Try
            Dim lnkbutton As LinkButton
            Dim intDealSlipId As String
            Dim strDealSlipNo As String
            Dim strTransType As String
            Dim strDealTransType As String
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                intDealSlipId = Trim(CType(e.Item.FindControl("lbl_DealSlipId"), Label).Text)
                strDealSlipNo = Trim(CType(e.Item.FindControl("lnk_DealSlipNo"), LinkButton).Text)
                strTransType = Trim(CType(e.Item.FindControl("lbl_TransType"), Label).Text)
                strDealTransType = Trim(CType(e.Item.FindControl("lbl_DealTransType"), Label).Text)
                lnkbutton = CType(e.Item.FindControl("lnk_DealSlipNo"), LinkButton)
                intDealSlipId = HttpUtility.UrlEncode(objCommon.EncryptText(Convert.ToString(intDealSlipId)))
                Hid_DealslipId.Value = intDealSlipId
                Hid_DealSlipNo.Value = strDealSlipNo
                Hid_dealTransType.Value = strDealTransType
                Hid_TransType.Value = strTransType

                If rdo_PrintOption.SelectedValue = "P" Then
                    lnkbutton.Attributes.Add("onclick", "return Update('" & e.Item.ItemIndex & "','" & intDealSlipId & "','" & strTransType & "','" & strDealTransType & "','" & strDealSlipNo & "');")
                Else
                    lnkbutton.Attributes.Add("onclick", "return OpenReport('" & e.Item.ItemIndex & "','" & intDealSlipId & "','" & strTransType & "','" & strDealTransType & "','" & strDealSlipNo & "');")
                End If

                If Session("UserTypeId") <> 107 Then
                    If strDealSlipNo.ToUpper.IndexOf("PP") <> -1 Or strDealSlipNo.ToUpper.IndexOf("PS") <> -1 Then
                        e.Item.Visible = False
                    End If
                End If
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "dg1_ItemDataBound", "Error in dg1_ItemDataBound", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub


    Protected Sub rdo_PrintOption_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdo_PrintOption.SelectedIndexChanged
        Try
            FillGrid()
            'ConvertToExcel("ID_Fill_DealDetailRptNew")
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "rdo_PrintOption_SelectedIndexChanged", "Error in rdo_PrintOption_SelectedIndexChanged", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub

    Private Sub ConvertToExcel(ByVal strProcName As String)
        Try
            OpenConn()
            Dim rptDoc As ReportDocument
            Dim dt As DataTable
            dt = GetReportTable(strProcName)
            Session("GetData") = dt
            rptDoc = BindReport()
            If dt.Rows.Count > 0 Then
                ExportReport(rptDoc)
                'Dim objExport As New RKLib.ExportData.Export("Web")
                'objExport.ExportDetails(dt, Export.ExportFormat.Excel, "Report.xls")
            Else
                Dim strHtml As String
                strHtml = "alert('Sorry!!! No Records available to show report');"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", strHtml, True)
            End If
            rptDoc = Nothing
        Catch ex As Exception
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally
            CloseConn()
        End Try
    End Sub

    Private Function BindReport(Optional ByVal blnSepPages As Boolean = False) As ReportDocument
        Dim dt As DataTable
        Dim ds As New DataSet
        Dim tw As New System.IO.StringWriter()
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        Dim dgGrid As New DataGrid()
        Dim strPath As String
        Dim dtRpt As DataTable


        'Response.ClearHeaders()
        strRptName = "DealDetailNew.rpt"


        strPath = ConfigurationManager.AppSettings("ReportsPath").ToString & strRptName
        rptDoc.Load(strPath)
        dtRpt = Session("GetData")
        dgGrid.DataSource = Session("GetData")
        rptDoc.SetDataSource(Session("GetData"))
        rptDoc.RecordSelectionFormula = clsCommonFuns.strRecordSelection
        clsCommonFuns.strRecordSelection = ""
        rptDoc = objCommon.GetReport(rptDoc)
        rptDoc.VerifyDatabase()
        rptDoc.Refresh()
        rptDoc.Load(strPath)

        Return rptDoc
    End Function
    Private Sub ExportReport(ByVal crReport As ReportDocument)
        'declare a memorystream object that will hold out output 

        Dim oStream As IO.MemoryStream
        Response.Clear()
        Response.ClearHeaders()
        Response.Buffer = True
        crReport.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, True, "Report")
        Try
            Response.Flush()
            Response.End()
        Catch ex As Exception
        Finally
        End Try
    End Sub

    Private Function GetReportTable(ByVal strProcName As String) As DataTable
        Try
            Dim datFrom As Date
            Dim datTo As Date
            Dim dtfill As New DataTable
            Dim sqlDa As New SqlDataAdapter
            Dim sqlComm As New SqlCommand

            'Dim intColIndex As Int16

            datFrom = objCommon.DateFormat(txt_Date.Text)

            'openconnection()
            With sqlComm
                .Connection = sqlconn
                .CommandType = CommandType.StoredProcedure
                .CommandText = strProcName


                'If Session("strWhereClause") <> "" Then
                '    objCommon.SetCommandParameters(sqlComm, "@Cond", SqlDbType.VarChar, 1000, "I", , , Session("strWhereClause"))
                'End If

                objCommon.SetCommandParameters(sqlComm, "@DealTransType", SqlDbType.Char, 1, "I", , , Trim(Hid_TransType.Value))
                objCommon.SetCommandParameters(sqlComm, "@TransType", SqlDbType.Char, 1, "I", , , Trim(Hid_dealTransType.Value))
                objCommon.SetCommandParameters(sqlComm, "@DealSlipId", SqlDbType.Int, 4, "I", , , Val(Hid_DealslipId.Value))
                objCommon.SetCommandParameters(sqlComm, "@CompId", SqlDbType.Int, 4, "I", , , Val(Session("CompId") & ""))
                objCommon.SetCommandParameters(sqlComm, "@YearId", SqlDbType.Int, 4, "I", , , Val(Session("YearId") & ""))
                objCommon.SetCommandParameters(sqlComm, "@FromDate", SqlDbType.SmallDateTime, 8, "I", , , datFrom)

                .ExecuteNonQuery()
            End With
            sqlDa.SelectCommand = sqlComm
            sqlDa.Fill(dtfill)
            'code naval
            'intColIndex = dtfill.Columns.IndexOf("Order")
            'If intColIndex <> -1 Then dtfill.Columns.RemoveAt(intColIndex)
            Return dtfill
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        Finally

        End Try
    End Function

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            CloseConn()
            If sqlConn IsNot Nothing Then
                sqlConn.Dispose()
            End If
        Catch ex As Exception
            objUtil.WritErrorLog(PgName, "Page_Unload", "Error in Page_Unload", "", ex)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "msg", "alert('" & ex.Message.Replace("'", " ").Replace(Chr(13), " ").Replace(Chr(10), " ") & "');", True)
        End Try
    End Sub
End Class
